CREATE PROCEDURE dbo.spQSTA_ReportIterationList

AS

SELECT DISTINCT B.WindowsUserID AS WindowsUserName, 
	A.BusinessUnit as 'Business Unit' 
FROM QSTA_LST_QueueDetail A 
JOIN GENR_REF_BU_Agent_Xref B 
	ON A.BusinessUnit = B.BusinessUnit 
WHERE B.Role = 'Manager'