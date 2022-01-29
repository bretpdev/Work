--RUN ON UHEAASQLDB
--NOTE TO DBA: you need to copy CLS.quecomplet.CNH_XXXXX from OPSDEV into UHEAASQLDB first, then run this query

--CREATE TABLE CLS.quecomplet.CNH_XXXXX 
--(
--	BF_SSN VARCHAR(X),
--	WF_QUE VARCHAR(X),
--	WF_SUB_QUE VARCHAR(X),
--	WN_CTL_TSK VARCHAR(XX),
--	PF_REQ_ACT VARCHAR(X)
--);

USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X,
			@ROWCOUNT INT = X,
			@ExpectedRows INT = XXXX;

	INSERT INTO CLS.quecomplet.Queues
	(
		[Queue], 
		SubQueue, 
		TaskControlNumber, 
		AccountIdentifier, 
		TaskStatusId, 
		ActionResponseId, 
		AddedAt, 
		AddedBy
	)
	SELECT DISTINCT
		NH.WF_QUE AS [Queue],
		NH.WF_SUB_QUE AS SubQueue,
		NH.WN_CTL_TSK AS TaskControlNumber,
		NH.BF_SSN AS AccountIdentifier,
		X AS TaskStatusId, --X
		X AS ActionResponseId, --CANCL
		GETDATE() AS AddedAt,
		'CNH XXXXX' AS AddedBy
	FROM
		CLS.quecomplet.CNH_XXXXX NH	
	;

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRows AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END