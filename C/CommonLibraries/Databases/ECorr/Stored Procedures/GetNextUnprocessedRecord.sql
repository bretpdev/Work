CREATE PROCEDURE [dbo].[GetNextUnprocessedRecord]

AS begin
	BEGIN TRANSACTION
		SET NOCOUNT ON;

	DECLARE @ERROR INT

	DECLARE @MyTableVar 
		TABLE
		(
			DocumentDetailsId int NOT NULL
		);


	SELECT @ERROR = @@ERROR

	UPDATE TOP (1)
		DocumentDetails
	SET 
		Printed = GETDATE()
	OUTPUT inserted.DocumentDetailsId
	INTO @MyTableVar
	WHERE 
		Printed IS NULL
		AND Active = 1

	SELECT @ERROR = @@ERROR + @ERROR

	SELECT
		DD.[DocumentDetailsId],
		[PATH],
		[Ssn],
		[DocDate] AS DOC_DATE,
		[DocId] AS DOC_ID,
		[Letter] AS LETTER_ID,
		[ADDR_ACCT_NUM],
		RequestUser AS REQUEST_USER,
		[Viewable] as VIEWABLE,
		[CorrMethod] AS CORR_METHOD,
		[ReportDescription] AS REPORT_DESC,
		[LoadTime] AS LOAD_TIME,
		[ReportName] AS REPORT_NAME,
		AddresseeEmail as ADDRESSEE_EMAIL,
		[Viewed],
		[CreateDate] AS CREATE_DATE,
		[DueDate] AS DUE_DATE,
		[MainframeRegion] AS MAINFRAME_REGION,
		DD.[DocumentDetailsId] as DCN,
		[TotalDue] AS TOTAL_DUE,
		[BillSeq] AS BILL_SEQ,
		[SubjectLine] AS SUBJECT_LINE,
		[DocSource] AS DOC_SOURCE,
		[DocComment] AS DOC_COMMENT,
		[WorkFlow] AS WORKFLOW,
		[DocDelete] AS DOC_DELETE

	FROM 
		[dbo].[DocumentDetails] DD
	INNER JOIN [dbo].[Letters] LTRS
		ON LTRS.LetterId = DD.LetterId
	INNER JOIN @MyTableVar T
		ON T.DocumentDetailsId = DD.DocumentDetailsId


	SELECT @ERROR = @ERROR + @@ERROR

	IF @ERROR = 0
		BEGIN
			COMMIT TRANSACTION
		END
	ELSE
		BEGIN
			RAISERROR ('Failed to retrieve dbo.DocumentDetails record for processing.', -- error message
				   16, -- Severity.
				   1 -- State.
				   );
			ROLLBACK TRANSACTION
		END
	END

go	
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNextUnprocessedRecord] TO [db_executor]
    AS [dbo];
