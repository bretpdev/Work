--RUN ON UHEAASQLDB
BEGIN TRANSACTION

	DECLARE @ERROR INT = 0,
			@ROWCOUNT INT = 0,
			@ExpectedRowCount INT = 6412;

	INSERT INTO ULS.quecomplet.Queues([Queue], SubQueue, TaskControlNumber, AccountIdentifier, TaskStatusId, ActionResponseId, PickedUpForProcessing, ProcessedAt, HadError, AddedAt, AddedBy, DeletedAt, DeletedBy)
	SELECT
		WF_QUE AS [Queue],
		WF_SUB_QUE AS SubQueue,
		WN_CTL_TSK AS TaskControlNumber,
		BF_SSN AS AccountIdentifier,
		2 AS TaskStatusId, --Complete
		3 AS ActionResponseId, --COMPL
		NULL AS PickedUpForProcessing,
		NULL AS ProcessedAt,
		0 AS HadError,
		GETDATE() AS AddedAt,
		'UNH 64018' AS AddedBy,
		NULL AS DeletedAt,
		NULL AS DeletedBy
	FROM
		UDW..WQ20_TSK_QUE
	WHERE
		WF_QUE = '4X'
		AND WF_SUB_QUE = '01'
		AND WD_ACT_REQ >= CONVERT(DATE,'20191007')
		AND (
				   WX_MSG_1_TSK LIKE '% 0801 %'
				OR WX_MSG_1_TSK LIKE '% 0988 %'
				OR WX_MSG_1_TSK LIKE '% 3601 %'
				OR WX_MSG_1_TSK LIKE '% 2401 %'
				OR WX_MSG_1_TSK LIKE '% 3001 %'
				OR WX_MSG_1_TSK LIKE '% 3101 %'
				OR WX_MSG_1_TSK LIKE '% 3201 %'
				OR WX_MSG_1_TSK LIKE '% 3301 %'
				OR WX_MSG_1_TSK LIKE '% 3302 %'

				OR WX_MSG_2_TSK LIKE '% 0801 %'
				OR WX_MSG_2_TSK LIKE '% 0988 %'
				OR WX_MSG_2_TSK LIKE '% 3601 %'
				OR WX_MSG_2_TSK LIKE '% 2401 %'
				OR WX_MSG_2_TSK LIKE '% 3001 %'
				OR WX_MSG_2_TSK LIKE '% 3101 %'
				OR WX_MSG_2_TSK LIKE '% 3201 %'
				OR WX_MSG_2_TSK LIKE '% 3301 %'
				OR WX_MSG_2_TSK LIKE '% 3302 %'
			);

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END