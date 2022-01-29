-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertEcorrRecord] 


@SSN CHAR(9),
@DOC_DATE DATE,
@ADDR_ACCT_NUM VARCHAR(10),
@LETTER VARCHAR(10),
@REQUEST_USER VARCHAR(8),
@LOAD_TIME DATETIME,
@ADDRESSEE_EMAIL VARCHAR(254),
@CREATE_DATE DATE,
@DUE_DATE datetime = NULL,
@TOTAL_DUE VARCHAR(10) = NULL,
@BILL_SEQ VARCHAR(4) = NULL,
@CORR_METHOD VARCHAR(15),
@PATH VARCHAR(500),
@Format int = 1




AS
BEGIN

	DECLARE @LETTER_ID INT = (SELECT [LetterId] FROM [dbo].[Letters] WHERE [Letter] = @LETTER)

	SET NOCOUNT ON;

	INSERT INTO [dbo].[DocumentDetails]([Ssn],[ADDR_ACCT_NUM],[Path],[DocDate],[LetterId],[RequestUser],[CorrMethod],[LoadTime],[AddresseeEmail],[CreateDate],[DueDate],[TotalDue],[BillSeq], CorrespondenceFormatId)
	VALUES(@SSN, @ADDR_ACCT_NUM,@PATH,@DOC_DATE,  @LETTER_ID, @REQUEST_USER, @CORR_METHOD, @LOAD_TIME, @ADDRESSEE_EMAIL,@CREATE_DATE, @DUE_DATE, @TOTAL_DUE, @BILL_SEQ, @Format)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertEcorrRecord] TO [db_executor]
    AS [dbo];

