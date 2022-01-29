CREATE PROCEDURE [qstatsextr].[GetBusinessUnitsForReports]
AS

	SELECT DISTINCT 
		A.BusinessUnit
	FROM 
		QSTA_LST_QueueDetail A
		INNER JOIN GENR_REF_BU_Agent_Xref B 
			ON A.BusinessUnit = B.BusinessUnit 
	WHERE 
		B.[Role] = 'Manager'

RETURN 0
