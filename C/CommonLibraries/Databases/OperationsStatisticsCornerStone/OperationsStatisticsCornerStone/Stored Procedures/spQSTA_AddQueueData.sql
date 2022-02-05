
CREATE PROCEDURE [dbo].[spQSTA_AddQueueData]
	@QueueDataTable QueueData readonly

AS
BEGIN
SET XACT_ABORT ON
	BEGIN TRANSACTION
	INSERT INTO QSTA_DAT_QueueData (RunDateTime, [Queue], Total, Complete, Critical, Canceled, Outstanding, Problem, Dept, Late) 
		SELECT
			[RunDateTime],
			[Queue],
			[Total],
			[Complete],
			[Critical],
			[Canceled],
			[OutStanding],
			[Problem],
			[Dept],
			[Late] 
		FROM
			@QueueDataTable
	COMMIT

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spQSTA_AddQueueData] TO [db_executor]
    AS [dbo];

