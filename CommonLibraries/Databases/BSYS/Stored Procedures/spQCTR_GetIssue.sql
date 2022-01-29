CREATE PROCEDURE [dbo].[spQCTR_GetIssue] 

@IssueNum			bigint,
@LockOrEdit			varchar(10)

AS

/****************************Email Recipients*******************************/
DECLARE @Recip varchar(8000)
DECLARE @TempRecip varchar(100)
DECLARE @BU 		nvarchar(50)
DECLARE @TRecip 		varchar(8000)
DECLARE @TRecip2 		varchar(8000)
DECLARE @TTempRecip 	varchar(8000)
DECLARE @RolesStr		varchar(100)
DECLARE @IssueIDCode	char(3)
DECLARE @BUManagers 	varchar(8000)
DECLARE @IssuePriority	int
DECLARE @AllRecipients	varchar(8000)
DECLARE @PriorityCategory varchar(200)
DECLARE @PriorityUrgency varchar(200)

CREATE TABLE #TEMP (Recips VARCHAR(8000))
CREATE TABLE #TempPriority (Priority INT)

SET @BU = ''
SET @TRecip = ''
SET @TRecip2 = ''
SET @BUManagers = ''
		
--get priority of issue

--a recalulation of the priority is needed because for some reason the original coder set the priority to 0
--when the issue is closed and in an effort to dodge side effects to undoing that we are just going to calculate things again

--get urgency and category for issue
SELECT PriorityCategory, PriorityUrgency
INTO #TempPriorityParts
FROM dbo.QCTR_DAT_Issue
WHERE ID = @IssueNum

SET @PriorityCategory = (SELECT PriorityCategory FROM #TempPriorityParts)
SET @PriorityUrgency = (SELECT PriorityUrgency FROM #TempPriorityParts)

--get calculated priority
INSERT INTO #TempPriority (Priority) EXEC spSYSAPriority @PriorityCategory, @PriorityUrgency

SET @IssuePriority = (SELECT Priority FROM #TempPriority)

IF (@IssuePriority >= 7)
BEGIN 
	/*get all email recipients*/
	--Higher priorities get sent to management
	DECLARE BUList CURSOR FOR 
		SELECT BU FROM QCTR_DAT_BU WHERE IssueID =@IssueNum
	OPEN BUList
	FETCH NEXT FROM BUList
	INTO @BU
	WHILE @@FETCH_STATUS = 0 
	BEGIN
		/* Create Recipient string */
		delete from #TEMP
		Insert into #TEMP Execute spGENRHierarchyEmailCopying @BU, 'Branch'
		IF @TRecip2 = ''
		BEGIN
			
			SET @TRecip2 =  (select Recips from #TEMP)
		END
		ELSE
		BEGIN
			SET @TRecip2 = @TRecip2 + ',' + (select Recips from #TEMP)
		END
		/* get next record */
		FETCH NEXT FROM BUList
		INTO @BU
	END
	/* close cursor */
	CLOSE BUList
	/* release memory */
	DEALLOCATE BUList



	/*create cursor to iterate through query results*/
	DECLARE EmailRecip CURSOR FOR 
		/*Requester*/
		select Requester from QCTR_DAT_Issue where ID = @IssueNum
		Union
		/*Business Unit Manager*/
		select B.WindowsUserID from QCTR_DAT_BU A inner join GENR_REF_BU_Agent_Xref B on A.BU = B.BusinessUnit where A.IssueID = @IssueNum and B.Role = 'Manager'
		Union 
		/*Business Unit QC Manager*/
		select WindowsUserID 
		from SYSA_DAT_AccessAndNotificationUserAccess A 
		INNER JOIN QCTR_DAT_BU B 
			ON A.BusinessUnit = B.BU 
		where A.AccessNotificationKey = 'Business Unit QC Manager' and B.IssueID = @IssueNum
		Union 
		/*Extra Email Recipients*/
		select Email from QCTR_DAT_Email where IssueID = @IssueNum
	/* open cursor */
	OPEN EmailRecip

	/* iterate through query results */
	FETCH NEXT FROM EmailRecip
	INTO @TTempRecip
	WHILE @@FETCH_STATUS = 0 
	BEGIN
		/* Create Recipient string */
		IF @TRecip = ''
		BEGIN
			SET @TRecip = @TTempRecip + '@utahsbr.edu'
		END
		ELSE
		BEGIN
			SET @TRecip = @TRecip + ',' + @TTempRecip + '@utahsbr.edu'
		END
		/* get next record */
		FETCH NEXT FROM EmailRecip
		INTO @TTempRecip
	END

	/* close cursor */
	CLOSE EmailRecip
	/* release memory */
	DEALLOCATE EmailRecip
END
ELSE
BEGIN
	--Lower priorities can be sent to responsible staff
	DECLARE EmailRecip2 CURSOR FOR 
		/*Requester*/
		select Requester from QCTR_DAT_Issue where ID = @IssueNum
		Union 
		/*The Perp.*/
		select U.WindowsUserName 
		from dbo.QCTR_DAT_Responsible R
		INNER JOIN dbo.SYSA_LST_UserIDInfo U
			ON R.UserID = U.UserID
		where R.IssueID = @IssueNum
		Union 
		/*Business Unit QC Manager*/
		select WindowsUserID 
		from SYSA_DAT_AccessAndNotificationUserAccess A 
		INNER JOIN QCTR_DAT_BU B 
			ON A.BusinessUnit = B.BU 
		where A.AccessNotificationKey = 'Business Unit QC Manager' and B.IssueID = @IssueNum
		Union 
		/*Extra Email Recipients*/
		select Email from QCTR_DAT_Email where IssueID = @IssueNum
	/* open cursor */
	OPEN EmailRecip2

	/* iterate through query results */
	FETCH NEXT FROM EmailRecip2
	INTO @TempRecip
	WHILE @@FETCH_STATUS = 0 
	BEGIN
		/* Create Recipient string */
		IF @TRecip = ''
		BEGIN
			SET @TRecip = @TempRecip + '@utahsbr.edu'
		END
		ELSE
		BEGIN
			SET @TRecip = @TRecip + ',' + @TempRecip + '@utahsbr.edu'
		END
		/* get next record */
		FETCH NEXT FROM EmailRecip2
		INTO @TempRecip
	END

	/* close cursor */
	CLOSE EmailRecip2
	/* release memory */
	DEALLOCATE EmailRecip2
END


IF @TRecip2 = ''
BEGIN
	SET @AllRecipients = @TRecip
END
ELSE
BEGIN
	SET @AllRecipients = @TRecip + ',' + @TRecip2
END
/****************************/

/*create cursor to iterate through query results*/
DECLARE EmailRecip CURSOR FOR 
	SELECT Email
	FROM dbo.QCTR_DAT_EMail
	WHERE IssueID = @IssueNum
	order by Email
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
/****************************Email Recipients Names*******************************/
DECLARE @RecipName varchar(8000)
DECLARE @TempRecipName varchar(100)
/*create cursor to iterate through query results*/
DECLARE EmailRecipName CURSOR FOR 
	SELECT C.FirstName + ' ' + C.LastName as UserName
	FROM dbo.QCTR_DAT_EMail A
	Inner Join dbo.SYSA_LST_Users C
		on A.Email = C.WindowsUserName
	WHERE A.IssueID = @IssueNum
	order by A.Email
SET @RecipName = ''
/* open cursor */
OPEN EmailRecipName
/* iterate through query results */
FETCH NEXT FROM EmailRecipName
INTO @TempRecipName
WHILE @@FETCH_STATUS = 0 
BEGIN
	/* Create Recipient string */
	IF @RecipName = ''
	BEGIN
		SET @RecipName = @TempRecipName
	END
	ELSE
	BEGIN
		SET @RecipName = @RecipName + ';' + @TempRecipName
	END
	/* get next record */
	FETCH NEXT FROM EmailRecipName
	INTO @TempRecipName
END
/* close cursor */
CLOSE EmailRecipName
/* release memory */
DEALLOCATE EmailRecipName

/****************************Business Units*******************************/
DECLARE @BUnit varchar(8000)
DECLARE @TempBUnit varchar(100)
/*create cursor to iterate through query results*/
DECLARE DSBUnit CURSOR FOR 
	SELECT BU
	FROM dbo.QCTR_DAT_BU
	WHERE IssueID = @IssueNum
SET @BUnit = ''
/* open cursor */
OPEN DSBUnit
/* iterate through query results */
FETCH NEXT FROM DSBUnit
INTO @TempBUnit
WHILE @@FETCH_STATUS = 0 
BEGIN
	/* Create Recipient string */
	IF @BUnit = ''
	BEGIN
		SET @BUnit = @TempBUnit
	END
	ELSE
	BEGIN
		SET @BUnit = @BUnit + ';' + @TempBUnit
	END
	/* get next record */
	FETCH NEXT FROM DSBUnit
	INTO @TempBUnit
END
/* close cursor */
CLOSE DSBUnit
/* release memory */
DEALLOCATE DSBUnit

/****************************Responsible Users*******************************/
DECLARE @User varchar(8000)
DECLARE @TempUser varchar(100)
/*create cursor to iterate through query results*/
DECLARE ResUser CURSOR FOR 
	SELECT UserID
	FROM dbo.QCTR_DAT_Responsible
	WHERE IssueID = @IssueNum
	order by UserID
SET @User = ''
/* open cursor */
OPEN ResUser
/* iterate through query results */
FETCH NEXT FROM ResUser
INTO @TempUser
WHILE @@FETCH_STATUS = 0 
BEGIN
	/* Create Recipient string */
	IF @User = ''
	BEGIN
		SET @User = @TempUser
	END
	ELSE
	BEGIN
		SET @User = @User + ';' + @TempUser
	END
	/* get next record */
	FETCH NEXT FROM ResUser
	INTO @TempUser
END
/* close cursor */
CLOSE ResUser
/* release memory */
DEALLOCATE ResUser
/****************************Responsible UsersNames*******************************/
DECLARE @UserName varchar(8000)
DECLARE @TempUserName varchar(100)
/*create cursor to iterate through query results*/
DECLARE ResUserName CURSOR FOR 
	SELECT A.UserID + ' ' + C.LastName + ', ' + C.FirstName as UserName
	FROM dbo.QCTR_DAT_Responsible A
	Inner Join dbo.SYSA_LST_UserIDInfo B
		on A.UserID = B.UserID
	Inner Join dbo.SYSA_LST_Users C
		on B.WindowsUserName = C.WindowsUserName
	WHERE A.IssueID = @IssueNum
	order by A.UserID
SET @UserName = ''
/* open cursor */
OPEN ResUserName
/* iterate through query results */
FETCH NEXT FROM ResUserName
INTO @TempUserName
WHILE @@FETCH_STATUS = 0 
BEGIN
	/* Create Recipient string */
	IF @UserName = ''
	BEGIN
		SET @UserName = @TempUserName
	END
	ELSE
	BEGIN
		SET @UserName = @UserName + ';' + @TempUserName
	END
	/* get next record */
	FETCH NEXT FROM ResUserName
	INTO @TempUserName
END
/* close cursor */
CLOSE ResUserName
/* release memory */
DEALLOCATE ResUserName

/*insert into #TEMPLAR(Priority,Subject,History,Recipients)  values (1,'dsfds','sdfsdf','sdfsdf')*/



select A.ID
,A.Subject as FullSubject
,CASE WHEN LEN(A.Subject) > 20 THEN LEFT(A.Subject, 20) + '...' ELSE A.Subject END as Subject
,A.Requester
,CONVERT(varchar,A.Requested,101) as Requested
,CONVERT(varchar,A.DateofActivity,101) as DateofActivity
,CONVERT(varchar,A.Required,101) as Required
,A.Issue
,A.History
,A.Resolution
,Convert(Integer,A.Priority) as Priority
,A.PriorityCategory
,A.PriorityUrgency
,B.UserID as Court
,F.Status
,CONVERT(varchar,B.Updated,101) as CourtDate
,CONVERT(varchar,F.Updated,101) as StatusDate
,CONVERT(varchar, GetDate(), 101) as CurrentDate
,@Recip as Recipients
,@RecipName as RecipientsName
,@BUnit as BusinessUnit
,@User as Responsible
,@UserName as ResponsibleName
,@LockOrEdit as LockOrEdit
,@AllRecipients as AllRecipients
from QCTR_DAT_Issue A
inner join (select z.IssueID, z.UserID, z.Updated 
		from QCTR_DAT_Court z
		inner join (select y.IssueID, MAX(y.Updated) as Updated from QCTR_DAT_Court y group by y.IssueID) x
			on x.IssueID = z.IssueID
			and x.Updated = z.Updated
		) B
	on B.IssueID = A.ID
	
inner join (select z.IssueID, z.Status, z.Updated 
		from QCTR_DAT_Status z
		inner join (select y.IssueID, MAX(y.Updated) as Updated from QCTR_DAT_Status y group by y.IssueID) x
			on x.IssueID = z.IssueID
			and x.Updated = z.Updated
		)F
	on F.IssueID = A.ID
WHERE A.ID = @IssueNum
FOR XML AUTO, ELEMENTS