-- =============================================
-- Author:		Jarom Ryan
-- Create date: 10/28/2013
-- Description:	Will Gather all of the unprocessed records from the e corr db
-- =============================================
CREATE PROCEDURE [dbo].[GetUnprocessedRecords] 

AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		[DocumentDetailsId],
		[PATH],
		[SSN],
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
		[Printed] IS NULL
END
