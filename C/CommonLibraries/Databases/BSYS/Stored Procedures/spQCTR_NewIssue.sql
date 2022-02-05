CREATE PROCEDURE dbo.spQCTR_NewIssue

@Requester			NVARCHAR (50)

AS

DECLARE @IssueNumber	BIGINT

--add issue record
INSERT INTO dbo.QCTR_DAT_Issue (Subject,Requester, Requested, Required, DateofActivity) VALUES ('',@Requester, CONVERT(VARCHAR(10),GETDATE(),101) ,CONVERT(VARCHAR(10),GETDATE()+3,101) ,CONVERT(VARCHAR(10),GETDATE(),101) )

SET @IssueNumber = @@IDENTITY

--add status record
INSERT INTO QCTR_DAT_Status (IssueID,Status,Updated) VALUES (@IssueNumber,'Draft',CONVERT(VARCHAR(10),GETDATE(),101))

--add court record
INSERT INTO QCTR_DAT_Court (IssueID,UserID,Updated) VALUES (@IssueNumber,@Requester,CONVERT(VARCHAR(10),GETDATE(),101))

RETURN @IssueNumber