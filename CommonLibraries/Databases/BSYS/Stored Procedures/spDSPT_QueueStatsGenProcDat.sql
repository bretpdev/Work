

CREATE PROCEDURE [dbo].[spDSPT_QueueStatsGenProcDat]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT C.QueueName,  
		C.BusinessUnit, 
		C.QueueDesc, 
		C.QueueStatusCode, 
		C.QueueStatusDesc, 
		C.COMPASSShrtDesc, 
		C.SystemIndicator, 
		C.NumOfDaysLateTask, 
		C.SystemQInd 
	FROM QSTA_LST_QueueDetail C 
	Order By C.BusinessUnit, C.QueueName
END