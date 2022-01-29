CREATE PROCEDURE [qstatsextr].[AddQueueData]
	@RunTimeDate DATETIME,
	@Queue VARCHAR(8),
	@Total BIGINT,
	@Complete BIGINT,
	@Critical BIGINT,
	@Cancelled BIGINT,
	@Outstanding BIGINT,
	@Problem BIGINT,
	@Late BIGINT,
	@Dept VARCHAR(3)
AS
	
	INSERT INTO 
		QSTA_DAT_QueueData (RunTimeDate, [Queue], Total, Complete, Critical, Cancelled, Outstanding, Problem, Late, Dept) 
	VALUES 
		(@RunTimeDate, @Queue, @Total, @Complete, @Critical, @Cancelled, @Outstanding, @Problem, @Late, @Dept)


RETURN 0
