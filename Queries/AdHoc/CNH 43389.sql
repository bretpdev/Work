--RUN ON UHEAASQLDB
--NOTE: you need to copy CLS.quecomplet.CNH_XXXXX from OPSDEV into UHEAASQLDB first, then run this query

--CREATE TABLE quecomplet.CNH_XXXXX (BF_SSN VARCHAR(X),LN_SEQ VARCHAR(X),LOAN_STATUS VARCHAR(XX));

USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X,
			@ROWCOUNT INT = X,
			@ExpectedRows INT = XXXXX; --distinct SSN's

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
		WQXX.WF_QUE AS [Queue],
		WQXX.WF_SUB_QUE AS SubQueue,
		WQXX.WN_CTL_TSK AS TaskControlNumber,
		PDXX.DF_SPE_ACC_ID AS AccountIdentifier,
		X AS TaskStatusId, --X
		X AS ActionResponseId, --CANCL
		GETDATE() AS AddedAt,
		'CNH XXXXX' AS AddedBy
	FROM
		CLS.quecomplet.CNH_XXXXX NH
		INNER JOIN CDW..LNXX_LON LNXX
			ON NH.BF_SSN = LNXX.BF_SSN
			AND NH.LN_SEQ = LNXX.LN_SEQ
		INNER JOIN CDW..AYXX_BR_LON_ATY AYXX
			ON LNXX.BF_SSN = AYXX.BF_SSN
		INNER JOIN CDW..WQXX_TSK_QUE WQXX
			ON AYXX.BF_SSN = WQXX.BF_SSN
			AND AYXX.LN_ATY_SEQ = WQXX.LN_ATY_SEQ
		INNER JOIN CDW..PDXX_PRS_NME PDXX
			ON NH.BF_SSN = PDXX.DF_PRS_ID
	WHERE
		WQXX.WF_QUE = 'XX'
		AND WQXX.WF_SUB_QUE = 'XX'
		AND WQXX.PF_REQ_ACT = 'DIFST'
		AND AYXX.PF_REQ_ACT = 'DIFST'
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