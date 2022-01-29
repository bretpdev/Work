CREATE PROCEDURE [dbo].[GetNextUnprocessedRecordBills]
	AS begin
	BEGIN TRANSACTION
		SET NOCOUNT ON;

	DECLARE @DocumentDetailsId bigint
	DECLARE @ERROR INT

	SELECT TOP 1
		@DocumentDetailsId = DocumentDetailsId
	FROM
		DocumentDetails
	WHERE 
		[Printed] IS NULL

	SELECT @ERROR = @@ERROR

	SELECT
		[DocumentDetailsId],
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
		[DocumentDetailsId] as DCN,
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
	WHERE 
		DocumentDetailsId = @DocumentDetailsId

	SELECT @ERROR = @ERROR + @@ERROR

	UPDATE
		DocumentDetails
	SET 
		Printed = GETDATE()
	WHERE
		DocumentDetailsId = @DocumentDetailsId

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
