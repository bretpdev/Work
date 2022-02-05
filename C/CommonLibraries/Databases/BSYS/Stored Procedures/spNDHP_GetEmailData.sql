

CREATE PROCEDURE [dbo].[spNDHP_GetEmailData] 

@Ticket	bigint

AS

DECLARE @BU 		nvarchar(50)
DECLARE @Recip 		varchar(8000)
DECLARE @TempRecip 	varchar(8000)
DECLARE @RolesStr		varchar(100)
DECLARE @TicketCode		char(3)

CREATE TABLE #TEMP (RecipStr VARCHAR(8000))

SET @BU = (SELECT Unit FROM NDHP_DAT_Tickets WHERE TICKET= @Ticket)
SET @Recip = ''
SET @TicketCode = (SELECT TicketCode FROM NDHP_DAT_Tickets WHERE TICKET= @Ticket) 

/* get director level recipients */
EXEC spNDHP_CheckForDirectorLvlEmailCopy @BU, @TempRecip OUTPUT
SET @Recip = @Recip + @TempRecip

IF @TicketCode <> 'FAC'
BEGIN --only if not a facilities ticket
	/* get all people that should be copied on all tickets */
	INSERT INTO #TEMP EXEC spGENRRecipientString 'HPCopyAll'
	IF (SELECT RecipStr FROM #TEMP) <> ''
	BEGIN
		IF @Recip = ''
		BEGIN
			SET @Recip = (SELECT RecipStr FROM #TEMP)
		END
		ELSE
		BEGIN
			SET @Recip = @Recip + ';' + (SELECT RecipStr FROM #TEMP)
		END 
	END
END

/*create cursor to iterate through query results*/
DECLARE EmailRecip CURSOR FOR 
	/* normal email list */
	((SELECT WindowsUserName + '@utahsbr.edu' AS EmailAddr 
	FROM dbo.NDHP_REF_EMailRecipients
	WHERE Ticket = @Ticket)
	
	UNION
	
	/* Requester */
	(SELECT UserID + '@utahsbr.edu' as EmailAddr
	FROM dbo.NDHP_DAT_UpdateTicketUserIDs 
	WHERE Ticket = @Ticket AND Role = 'Requester' AND (UserID <> '' AND UserID IS NOT NULL))
	
	UNION 
	 
	/* court */
	(SELECT UserID + '@utahsbr.edu' as EmailAddr
	FROM dbo.NDHP_DAT_UpdateTicketUserIDs 
	WHERE Ticket = @Ticket AND Role = 'Court' AND (UserID <> '' AND UserID IS NOT NULL))
	
	UNION

	/* AssignedTo */
	(SELECT UserID + '@utahsbr.edu' as EmailAddr
	FROM dbo.NDHP_DAT_UpdateTicketUserIDs 
	WHERE Ticket = @Ticket AND Role = 'AssignedTo' AND (UserID <> '' AND UserID IS NOT NULL))
	
	 UNION
	
	/*OS Analyst and Manager not for POL*/
	(SELECT  B.WindowsUserID + '@utahsbr.edu' as EmailAddr
	FROM dbo.NDHP_DAT_Tickets A
	JOIN dbo.GENR_REF_BU_Agent_Xref B ON A.Unit = B.BusinessUnit
	WHERE (@TicketCode <> 'POL' AND @TicketCode <> 'FAC'  AND (B.Role = 'Manager' OR B.Role = 'OS Assigned') AND A.Ticket = @Ticket) 
		))

	 UNION
	
	/*add for POL*/
	(SELECT  WindowsUserID + '@utahsbr.edu' as EmailAddr
	FROM dbo.GENR_REF_BU_Agent_Xref
	WHERE (@TicketCode = 'POL'  AND ((Role = 'Manager' OR Role = 'Member Of') AND (BusinessUnit = 'Compliance')))

	UNION

	/*Add for All FAR tickets*/
	(SELECT  WinUName + '@utahsbr.edu' as EmailAddr
	FROM dbo.GENR_REF_MiscEmailNotif
	WHERE (@TicketCode = 'FAR'  AND (TypeKey = 'HPFinAdj'))))

/* open cursor */
OPEN EmailRecip

/* iterate through query results */
FETCH NEXT FROM EmailRecip
INTO @TempRecip
WHILE @@FETCH_STATUS = 0 
BEGIN
	/* Create Recipient string */
	IF @Recip = ''
	BEGIN
		SET @Recip = @TempRecip
	END
	ELSE
	BEGIN
		SET @Recip = @Recip + ';' + @TempRecip
	END
	/* get next record */
	FETCH NEXT FROM EmailRecip
	INTO @TempRecip
END

/* close cursor */
CLOSE EmailRecip
/* release memory */
DEALLOCATE EmailRecip

SELECT @Recip as Recipients, Ticket, Subject, Status, B.TicketType AS TicketType, History
FROM dbo.NDHP_DAT_Tickets A 
JOIN NDHP_LST_TicketTypes B ON A.TicketCode = B.TicketCode
WHERE Ticket = @Ticket