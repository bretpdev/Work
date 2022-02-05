CREATE PROCEDURE [dbo].[GetSpouseFilingStatuses]
	
AS
	SELECT 
		SpouseFilingStatusId,
		SpouseFilingStatus
	FROM
		Spouse_Filing_Statuses
RETURN 0