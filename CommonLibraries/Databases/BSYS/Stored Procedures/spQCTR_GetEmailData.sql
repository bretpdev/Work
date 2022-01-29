CREATE PROCEDURE [dbo].[spQCTR_GetEmailData] 
/*If this query is modified you will also need to modify the spQCTR_GetIssue to match*/
@IssueID	bigint

AS

DECLARE @BU 		nvarchar(50)
DECLARE @Recip 		varchar(8000)
DECLARE @Recip2 		varchar(8000)
DECLARE @TempRecip 	varchar(8000)
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
SET @Recip = ''
SET @Recip2 = ''
SET @BUManagers = ''

--get priority of issue

--a recalulation of the priority is needed because for some reason the original coder set the priority to 0
--when the issue is closed and in an effort to dodge side effects to undoing that we are just going to calculate things again

--get urgency and category for issue
SELECT PriorityCategory, PriorityUrgency
INTO #TempPriorityParts
FROM dbo.QCTR_DAT_Issue
WHERE ID = @IssueID

SET @PriorityCategory = (SELECT PriorityCategory FROM #TempPriorityParts)
SET @PriorityUrgency = (SELECT PriorityUrgency FROM #TempPriorityParts)

--get calculated priority
INSERT INTO #TempPriority (Priority) EXEC spSYSAPriority @PriorityCategory, @PriorityUrgency

SET @IssuePriority = (SELECT Priority FROM #TempPriority)

print @IssuePriority

IF (@IssuePriority >= 7)
BEGIN 
	--Higher priorities get sent to management
	DECLARE BUList CURSOR FOR 
		SELECT BU FROM QCTR_DAT_BU WHERE IssueID = @IssueID
	OPEN BUList
	FETCH NEXT FROM BUList
	INTO @BU
	WHILE @@FETCH_STATUS = 0 
	BEGIN
		/* Create Recipient string */
		delete from #TEMP
		Insert into #TEMP Execute spGENRHierarchyEmailCopying @BU, 'Branch'
		IF @Recip2 = ''
		BEGIN
			
			SET @Recip2 =  (select Recips from #TEMP)
		END
		ELSE
		BEGIN
			SET @Recip2 = @Recip2 + ',' + (select Recips from #TEMP)
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
		select Requester from QCTR_DAT_Issue where ID = @IssueID
		Union
		/*Business Unit Manager and seconds*/
		select B.WindowsUserID from QCTR_DAT_BU A inner join GENR_REF_BU_Agent_Xref B on A.BU = B.BusinessUnit where A.IssueID = @IssueID and B.Role in ('Manager','Supervisor')
		Union 
		/*Business Unit QC Manager*/
		select WindowsUserID 
		from SYSA_DAT_AccessAndNotificationUserAccess A 
		INNER JOIN QCTR_DAT_BU B 
			ON A.BusinessUnit = B.BU 
		where A.AccessNotificationKey = 'Business Unit QC Manager' and B.IssueID = @IssueID
		Union 
		/*Extra Email Recipients*/
		select Email from QCTR_DAT_Email where IssueID = @IssueID
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
			SET @Recip = @TempRecip + '@utahsbr.edu'
		END
		ELSE
		BEGIN
			SET @Recip = @Recip + ',' + @TempRecip + '@utahsbr.edu'
		END
		/* get next record */
		FETCH NEXT FROM EmailRecip
		INTO @TempRecip
	END

	/* close cursor */
	CLOSE EmailRecip
	/* release memory */
	DEALLOCATE EmailRecip
END
ELSE
BEGIN 
	--Lower priorities can be sent to responsible staff 
	/*create cursor to iterate through query results*/
	DECLARE EmailRecip2 CURSOR FOR 
		/*Requester*/
		select Requester from QCTR_DAT_Issue where ID = @IssueID
		Union 
		/*The Perp.*/
		select U.WindowsUserName 
		from dbo.QCTR_DAT_Responsible R
		INNER JOIN dbo.SYSA_LST_UserIDInfo U
			ON R.UserID = U.UserID
		where R.IssueID = @IssueID
		Union 
		/*Business Unit QC Manager*/
		select WindowsUserID 
		from SYSA_DAT_AccessAndNotificationUserAccess A 
		INNER JOIN QCTR_DAT_BU B 
			ON A.BusinessUnit = B.BU 
		where A.AccessNotificationKey = 'Business Unit QC Manager' and B.IssueID = @IssueID
		Union 
		/*Extra Email Recipients*/
		select Email from QCTR_DAT_Email where IssueID = @IssueID
	/* open cursor */
	OPEN EmailRecip2

	/* iterate through query results */
	FETCH NEXT FROM EmailRecip2
	INTO @TempRecip
	WHILE @@FETCH_STATUS = 0 
	BEGIN
		/* Create Recipient string */
		IF @Recip = ''
		BEGIN
			SET @Recip = @TempRecip + '@utahsbr.edu'
		END
		ELSE
		BEGIN
			SET @Recip = @Recip + ',' + @TempRecip + '@utahsbr.edu'
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

IF @Recip2 = ''
BEGIN
	SET @AllRecipients = @Recip
END
ELSE
BEGIN
	SET @AllRecipients = @Recip + ',' + @Recip2
END

SELECT A1.Priority,Subject, B.Status,History, @AllRecipients as Recipients,A.ID
FROM dbo.QCTR_DAT_Issue A 
inner join QCTR_DAT_Status B
on A.ID = B.IssueID
inner join (select IssueID, max(Updated) as Updated
	from QCTR_DAT_Status
	group by IssueID) C
on B.IssueID = C.IssueID
and B.Updated = C.Updated
inner join dbo.GENR_REF_PriorityCatgryOps B1
	on B1.CatOption = A.PriorityCategory
inner join dbo.GENR_REF_PriorityUrgencyOps C1
	on C1.UrgOption = A.PriorityUrgency
inner join dbo.GENR_LST_Priorities A1
	on A1.Category = B1.Category
	and A1.Urgency = C1.Urgency

WHERE A.ID = @IssueID