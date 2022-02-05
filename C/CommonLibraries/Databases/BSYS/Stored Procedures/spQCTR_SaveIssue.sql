CREATE PROCEDURE [dbo].[spQCTR_SaveIssue]

@IssueNumber			bigint,
@Category				varchar(200),
@Subject				varchar(200),
@Urgency				varchar(200),
@Requester				nvarchar(50),
@Priority				smallint,
@Requested				datetime,
@RequiredDt				datetime,
@DateOfActivity			datetime,
@Issue					varchar(8000),
@Status					varchar(50),
@StatusDt				datetime,
@Court					nvarchar(50),
@CourtDt				datetime,
@History				text,
@Resolution				varchar(8000),
@BUs					varchar(8000),
@ResponsibleUsersID		varchar(8000),
@EmailRecips			varchar(8000),
@SelectedOption			nvarchar(50)




AS

DECLARE @SingleEmail 	varchar(50)
DECLARE @SingleBU	 	varchar(50)
DECLARE @SingleResponsible 	varchar(50)
DECLARE @LastPosition		int
DECLARE @QCManager 		varchar(50)
DECLARE @Director 		varchar(50)
DECLARE @PrevStatus 		varchar(50)
DECLARE @BUManager		varchar(50)
DECLARE @Perp			varchar(50)

/*  don't add '' to required date field, it doesn't display well on the interface */
IF @RequiredDt	= ''
BEGIN
	SET @RequiredDt = NULL
END

/*change court*/
--was below the SelectedOption if statement but I believe it should be here so the system can put it
--where it should go even if the user changed the court.
IF @Court <> (SELECT UserID FROM QCTR_DAT_Court WHERE IssueID = @IssueNumber and Ended is null)
BEGIN
	--update court if different
	update QCTR_DAT_Court set Ended = GETDATE() where IssueID = @IssueNumber and Ended is null
	insert into QCTR_DAT_Court (IssueID,UserID,Updated,Ended) values(@IssueNumber,@Court,GETDATE(),NULL)
END

IF @SelectedOption = 'Submit' 
BEGIN
	/*change court to requester*/
	update QCTR_DAT_Court set Ended = GETDATE() where IssueID = @IssueNumber and Ended is null
	IF @Priority >= 7
	BEGIN
		--get manager of BU
		IF CHARINDEX(';',@BUs) = 0
		BEGIN
			--if one BU then get it
			SET @SingleBU = @BUs
		END
		ELSE
		BEGIN
			--if multiple BUs then get the first one
			SET @SingleBU = LEFT(@BUs, CHARINDEX(';',@BUs) - 1)
		END
		SET @BUManager = (SELECT WindowsUserID FROM dbo.GENR_REF_BU_Agent_Xref WHERE BusinessUnit = @SingleBU AND Role = 'Manager')
		--goes to manager if priority >= 7
		insert into QCTR_DAT_Court (IssueID,UserID,Updated,Ended) values(@IssueNumber,@BUManager,GETDATE(),NULL)
	END
	ELSE
	BEGIN
		--get perp.
		IF CHARINDEX(';',@ResponsibleUsersID) = 0
		BEGIN
			--if one perp then get it
			SET @SingleResponsible = @ResponsibleUsersID
		END
		ELSE
		BEGIN
			--if multiple perps then get the first one
			SET @SingleResponsible = LEFT(@ResponsibleUsersID, CHARINDEX(';',@ResponsibleUsersID) - 1)
		END
		SET @Perp = (select WindowsUserName 
					from dbo.SYSA_LST_UserIDInfo
					where UserID = @SingleResponsible)
		--goes to Perp if priority < 7
		insert into QCTR_DAT_Court (IssueID,UserID,Updated,Ended) values(@IssueNumber,@Perp,GETDATE(),NULL)
	END
	/*Change status to Discusion*/
	update QCTR_DAT_Status set Ended = GETDATE() where IssueID = @IssueNumber and Ended is null
	insert into QCTR_DAT_Status (IssueID,Status,Updated,Ended) values(@IssueNumber,'Discussion',GETDATE(),NULL)
END
ELSE IF @SelectedOption = 'Resolution' 
BEGIN
	/*change court to QC Manager*/
	SET @QCManager = (select top 1 WindowsUserID from GENR_REF_BU_Agent_Xref where BusinessUnit = 'Quality Control' and Role = 'Manager')
	update QCTR_DAT_Court set Ended = GETDATE() where IssueID = @IssueNumber and Ended is null
	insert into QCTR_DAT_Court (IssueID,UserID,Updated,Ended) values(@IssueNumber,@QCManager,GETDATE(),NULL)
	/*Change status to QC Approval*/
	update QCTR_DAT_Status set Ended = GETDATE() where IssueID = @IssueNumber and Ended is null
	insert into QCTR_DAT_Status (IssueID,Status,Updated,Ended) values(@IssueNumber,'QC Approval',GETDATE(),NULL)
END
ELSE IF @SelectedOption = 'QC Approval' 
BEGIN
	/*change court to no one*/
	update QCTR_DAT_Court set Ended = GETDATE() where IssueID = @IssueNumber and Ended is null
	/*Change status to Complete*/
	update QCTR_DAT_Status set Ended = GETDATE() where IssueID = @IssueNumber and Ended is null
	insert into QCTR_DAT_Status (IssueID,Status,Updated,Ended) values(@IssueNumber,'Complete',GETDATE(),GETDATE())
END
ELSE IF @SelectedOption = 'Return' 
BEGIN
	/*change court to requester*/
	update QCTR_DAT_Court set Ended = GETDATE() where IssueID = @IssueNumber and Ended is null
	insert into QCTR_DAT_Court (IssueID,UserID,Updated,Ended) values(@IssueNumber,@Requester,GETDATE(),NULL)
	/*Change status to Discusion*/
	update QCTR_DAT_Status set Ended = GETDATE() where IssueID = @IssueNumber and Ended is null
	insert into QCTR_DAT_Status (IssueID,Status,Updated,Ended) values(@IssueNumber,'Discussion',GETDATE(),NULL)
END
ELSE IF @SelectedOption = 'Hold' 
BEGIN
	/*Change status to Hold*/
	update QCTR_DAT_Status set Ended = GETDATE() where IssueID = @IssueNumber and Ended is null
	insert into QCTR_DAT_Status (IssueID,Status,Updated,Ended) values(@IssueNumber,'Hold',GETDATE(),NULL)
END
ELSE IF @SelectedOption = 'Hold_Release' 
BEGIN
	
	SET @PrevStatus = (select A.Status from QCTR_DAT_Status A inner join (select IssueID, max(Updated) as Updated from QCTR_DAT_Status where Ended is not Null and IssueID = @IssueNumber Group by IssueID) B on A.IssueID = B.IssueID and A.Updated = B.Updated)
	update QCTR_DAT_Status set Ended = GETDATE() where IssueID = @IssueNumber and Ended is null
	insert into QCTR_DAT_Status (IssueID,Status,Updated,Ended) values(@IssueNumber,@PrevStatus,GETDATE(),NULL)
END
ELSE IF @SelectedOption = 'Withdraw' 
BEGIN
	/*change court to no one*/
	update QCTR_DAT_Court set Ended = GETDATE() where IssueID = @IssueNumber and Ended is null
	/*Change status to Withdrawn*/
	update QCTR_DAT_Status set Ended = GETDATE() where IssueID = @IssueNumber and Ended is null
	insert into QCTR_DAT_Status (IssueID,Status,Updated,Ended) values(@IssueNumber,'Withdrawn',GETDATE(),GETDATE())
END
ELSE IF @SelectedOption = 'Deactivate' 
BEGIN
	/*change court to no one*/
	update QCTR_DAT_Court set Ended = GETDATE() where IssueID = @IssueNumber and Ended is null
	/*Change status to Deactivated*/
	update QCTR_DAT_Status set Ended = GETDATE() where IssueID = @IssueNumber and Ended is null
	insert into QCTR_DAT_Status (IssueID,Status,Updated,Ended) values(@IssueNumber,'Deactivated',GETDATE(),GETDATE())
END


UPDATE dbo.QCTR_DAT_Issue 
SET 	Subject = @Subject,
	Requester = @Requester,
	Requested = @Requested,
	DateOfActivity = @DateOfActivity,
	Required = @RequiredDt,
	Priority = @Priority,
	Issue = @Issue,
	History = @History,
	Resolution = @Resolution,
	Type = NULL,
	PriorityCategory = @Category,
	PriorityUrgency = @Urgency
WHERE 	ID = @IssueNumber



/* add individual entries into appropriate tables */

SET @SingleEmail = ''
SET @SingleBU = ''
SET @SingleResponsible = ''

/* delete current entries for the ticket out of the tables */
DELETE FROM dbo.QCTR_DAT_Email WHERE IssueID = @IssueNumber
DELETE FROM dbo.QCTR_DAT_BU WHERE IssueID = @IssueNumber
DELETE FROM dbo.QCTR_DAT_Responsible WHERE IssueID = @IssueNumber


/* populate email recipients */
WHILE @EmailRecips <> ''
BEGIN
	SET @LastPosition = CHARINDEX(';',@EmailRecips)
	IF @LastPosition = 0
	BEGIN
		/* either last entry or the string only had one entry */
		SET @SingleEmail = @EmailRecips
		SET @EmailRecips = ''
	END
	ELSE
	BEGIN
		/* pull out first entry */
		SET @SingleEmail = LEFT(@EmailRecips, CHARINDEX(';',@EmailRecips) - 1)
		/* check if there is another delimiter to handle */ 
		IF CHARINDEX(';',@EmailRecips, @LastPosition) > 0
		BEGIN
			/* trim off the part of the string that has already been processed */
			SET @EmailRecips = RIGHT(@EmailRecips, (LEN(@EmailRecips) - (@LastPosition)))
		END
	END
	INSERT INTO dbo.QCTR_DAT_Email VALUES (@IssueNumber, @SingleEmail)
END

/* populate BU */
WHILE @BUs <> ''
BEGIN
	SET @LastPosition = CHARINDEX(';',@BUs)
	IF @LastPosition = 0
	BEGIN
		/* either last entry or the string only had one entry */
		SET @SingleBU = @BUs
		SET @BUs = ''
	END
	ELSE
	BEGIN
		/* pull out first entry */
		SET @SingleBU = LEFT(@BUs, CHARINDEX(';',@BUs) - 1)
		/* check if there is another delimiter to handle */ 
		IF CHARINDEX(';',@BUs, @LastPosition) > 0
		BEGIN
			/* trim off the part of the string that has already been processed */
			SET @BUs = RIGHT(@BUs, (LEN(@BUs) - (@LastPosition)))
		END
	END
	INSERT INTO dbo.QCTR_DAT_BU VALUES (@SingleBU,@IssueNumber)
END

/* populate Responsible Users */
WHILE @ResponsibleUsersID <> ''
BEGIN
	SET @LastPosition = CHARINDEX(';',@ResponsibleUsersID)
	IF @LastPosition = 0
	BEGIN
		/* either last entry or the string only had one entry */
		SET @SingleResponsible = @ResponsibleUsersID
		SET @ResponsibleUsersID = ''
	END
	ELSE
	BEGIN
		/* pull out first entry */
		SET @SingleResponsible = LEFT(@ResponsibleUsersID, CHARINDEX(';',@ResponsibleUsersID) - 1)
		/* check if there is another delimiter to handle */ 
		IF CHARINDEX(';',@ResponsibleUsersID, @LastPosition) > 0
		BEGIN
			/* trim off the part of the string that has already been processed */
			SET @ResponsibleUsersID = RIGHT(@ResponsibleUsersID, (LEN(@ResponsibleUsersID) - (@LastPosition)))
		END
	END
	INSERT INTO dbo.QCTR_DAT_Responsible VALUES (@IssueNumber, @SingleResponsible)
END