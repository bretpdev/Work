
-- add a QC record for records from SAS files
CREATE PROCEDURE [dbo].[spQCTR_QCDBUser_CreateIssue]

@Subject			varchar(100),	-- Subject from table
@Requester			varchar(50),	-- User Name running the script
@BusinessUnit		varchar(50),		-- BusinessUnit from table
@ResponsibleUserID	varchar(50),	-- USER from file
@Required			datetime,		-- Calculated as (Today + RequiredDays from table)
@DateOfActivity		datetime,		-- ACT_DT from file
@Issue				varchar(8000),	-- DESCRIPTION from file
@Category			varchar(200),	-- PriorityCategory from table
@Urgency			varchar(200)	-- PriorityUrgency from table


AS

DECLARE @IssueNumber	BIGINT
DECLARE @Priority		smallint


DECLARE @Court			varchar(50)

DECLARE @History		varchar(8000)
DECLARE @RequesterUserName	varchar(50)
DECLARE @WindowsUserName varchar(50)

CREATE TABLE #TEMP (Priority smallint)

--set requester user ID and get priority
Insert into #TEMP Execute dbo.spSYSAPriority @Category, @Urgency
SET @Priority = (SELECT Priority FROM #TEMP)

--add issue record and get new issue number
INSERT INTO dbo.QCTR_DAT_Issue (Subject, Requester, Requested, Required, DateofActivity) VALUES (@Subject,@Requester,CONVERT(VARCHAR(10),GETDATE(),101),@Required,@DateOfActivity)
SET @IssueNumber = @@IDENTITY

--add status record
INSERT INTO QCTR_DAT_Status (IssueID,Status,Updated) VALUES (@IssueNumber,'Draft',CONVERT(VARCHAR(10),GETDATE(),101))

--add court record
INSERT INTO QCTR_DAT_Court (IssueID,UserID,Updated) VALUES (@IssueNumber,@Requester,CONVERT(VARCHAR(10),GETDATE(),101))

--get windows user name of the responsible user (ID)
SET @WindowsUserName = (SELECT TOP 1 WindowsUserName FROM SYSA_LST_UserIDInfo WHERE UserID = @ResponsibleUserID)

--if the responsible user ID is not a system user ID, get the user's unit
IF Left(@ResponsibleUserID,2) = 'UT'
BEGIN
SET @BusinessUnit = (SELECT TOP 1 BusinessUnit FROM dbo.GENR_REF_BU_Agent_Xref WHERE WindowsUserID = @WindowsUserName AND Role = 'Member Of')
END

--set the court
IF @Priority < 7 AND Left(@ResponsibleUserID,2) = 'UT'
BEGIN
SET @Court = @WindowsUserName
END
ELSE
BEGIN
SET @Court = (SELECT WindowsUserID FROM dbo.GENR_REF_BU_Agent_Xref WHERE BusinessUnit = @BusinessUnit AND Role = 'Manager')
END

--change court to responsible user
update QCTR_DAT_Court set Ended = GETDATE() where IssueID = @IssueNumber and Ended is null
insert into QCTR_DAT_Court (IssueID,UserID,Updated,Ended) values(@IssueNumber,@Court,GETDATE(),NULL)

--Change status to Discusion
update QCTR_DAT_Status set Ended = GETDATE() where IssueID = @IssueNumber and Ended is null
insert into QCTR_DAT_Status (IssueID,Status,Updated,Ended) values(@IssueNumber,'Discussion',GETDATE(),NULL)

--get requester's full name
SET @RequesterUserName = (SELECT FirstName + ' ' + LastName AS UserName FROM dbo.SYSA_LST_Users WHERE WindowsUserName = @Requester)

--compile history entry
SET @History = @RequesterUserName + ' ' + CONVERT(VARCHAR(50),GETDATE(),101) + ' ' + CONVERT(VARCHAR(50),GETDATE(),108) + ' - Discussion'
SET @History = @History + CHAR(13) + CHAR(13) + 'Created by the QC Database User Script'
SET @History = @History + CHAR(13) + CHAR(13) + 'Issue: ' + @Issue

--update the issue
UPDATE dbo.QCTR_DAT_Issue 
SET 	
		Priority = @Priority,
		Issue = @Issue,
		History = @History,
		Type = NULL,
		PriorityCategory = @Category,
		PriorityUrgency = @Urgency
WHERE 	ID = @IssueNumber

--add business unit and responsible user records
INSERT INTO dbo.QCTR_DAT_BU VALUES (@BusinessUnit,@IssueNumber)
INSERT INTO dbo.QCTR_DAT_Responsible VALUES (@IssueNumber, @ResponsibleUserID)

Select CAST(@IssueNumber AS BigInt) AS IssueID, CAST(@Priority AS int) As Priority