

CREATE PROCEDURE [dbo].[spNDHP_GetTicket] 

@TicketNum			bigint,
@FileStr			varchar(8000),
@LockOrEdit			varchar(10)

AS

DECLARE @Sys varchar(8000)
DECLARE @TempSys varchar(100)
/*create cursor to iterate through query results*/
DECLARE Systems CURSOR FOR 
	SELECT [System] 
	FROM dbo.NDHP_REF_Systems
	WHERE Ticket = @TicketNum

/*****************************Systems**********************************/
SET @Sys = ''

/* open cursor */
OPEN Systems

/* iterate through query results */
FETCH NEXT FROM Systems
INTO @TempSys
WHILE @@FETCH_STATUS = 0 
BEGIN
	/* Create Recipient string */
	IF @Sys = ''
	BEGIN
		SET @Sys = @TempSys
	END
	ELSE
	BEGIN
		SET @Sys = @Sys + ';' + @TempSys
	END
	/* get next record */
	FETCH NEXT FROM Systems
	INTO @TempSys
END

/* close cursor */
CLOSE Systems
/* release memory */
DEALLOCATE Systems

/****************************Email Recipients*******************************/
DECLARE @Recip varchar(8000)
DECLARE @TempRecip varchar(100)

DECLARE @TempReq varchar(50)
DECLARE @TempCourt varchar(50)
DECLARE @TempPreCourt varchar(50)
DECLARE @TempAssignTo varchar(50)

Select @TempReq = UserID
From dbo.NDHP_DAT_UpdateTicketUserIDs
Where Ticket = @TicketNum AND Role = 'Requester'

Select @TempCourt = UserID
From dbo.NDHP_DAT_UpdateTicketUserIDs
Where Ticket = @TicketNum AND Role = 'Court'

Select @TempPreCourt = UserID
From dbo.NDHP_DAT_UpdateTicketUserIDs
Where Ticket = @TicketNum AND Role = 'PreviousCourt'

Select @TempAssignTo = UserID
From dbo.NDHP_DAT_UpdateTicketUserIDs
Where Ticket = @TicketNum AND Role = 'AssignedTo'

/*create cursor to iterate through query results*/
DECLARE EmailRecip CURSOR FOR 
	SELECT WindowsUserName
	FROM dbo.NDHP_REF_EMailRecipients
	WHERE Ticket = @TicketNum

SET @Recip = ''

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

SELECT /**/ A.Ticket, 
	A.TicketCode, 
	COALESCE(A.Subject,'') as Subject, 
	COALESCE(@TempReq,'')  as Requester,
	CONVERT(varchar, A.Requested, 101) as Requested,
	COALESCE(A.Unit,'') as Unit,
	COALESCE(A.Area,'') as Area,
	CASE 
		WHEN A.Required IS NULL THEN ''
		ELSE CONVERT(varchar, A.Required, 101)
	END as Required,
	COALESCE(A.Issue,'') as Issue,
	COALESCE(A.ResolutionCause,'') as ResolutionCase,
	COALESCE(A.ResolutionFix,'') as ResolutionFix,
	COALESCE(A.ResolutionPrevention,'') as ResolutionPrevention,
	COALESCE(A.Status,'') as Status,
	CONVERT(varchar, A.StatusDate, 101) as StatusDate,
	COALESCE(@TempCourt,'') as Court,
	CONVERT(varchar, A.CourtDate, 101) as CourtDate,
	COALESCE(A.IssueUpdate,'') as IssueUpdate,
	COALESCE(A.History,'') as History,
	--COALESCE(A.PreviousCourt,'') as PreviousCourt,
	COALESCE(@TempPreCourt,'')  as PreviousCourt,
	COALESCE(A.PreviousStatus,'') as PreviousStatus,
	COALESCE(A.UrgencyOption,'') as UrgencyOption,
	COALESCE(A.CatOption,'') as CatOption,
	COALESCE(A.Priority,'') as Priority,
	CASE 
		WHEN A.LastUpdated IS NULL THEN ''
		ELSE CONVERT(varchar, A.LastUpdated, 101)
	END as LastUpdated,
	COALESCE(A.CCCIssue,'') as CCCIssue,
	COALESCE(A.RequestProjectNum,'') as RequestProjectNum,
	@Sys as Systems,
	COALESCE(@TempAssignTo,'') as AssignedTo,
	B.TicketType as TypeText,
	@Recip as Recipients,
	@FileStr as AttachedFiles,
	@LockOrEdit as LockOrEdit,
	 CONVERT(varchar, GetDate(), 101) as CurrentDate,
	COALESCE(A.Comments,'') as Comments
FROM dbo.NDHP_DAT_Tickets A 
JOIN dbo.NDHP_LST_TicketTypes B ON A.TicketCode = B.TicketCode
WHERE Ticket = @TicketNum
FOR XML AUTO, ELEMENTS