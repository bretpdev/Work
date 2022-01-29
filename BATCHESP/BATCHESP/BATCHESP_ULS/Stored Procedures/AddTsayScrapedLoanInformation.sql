CREATE PROCEDURE [batchesp].[AddTsayScrapedLoanInformation]
	@BorrowerSsn CHAR(9),
	@LoanSequence INT,
	@LoanProgramType VARCHAR(6),
	@Type VARCHAR(9),
	@DeferSchool VARCHAR(8),
	@BeginDate DATE = NULL,
	@EndDate DATE = NULL,
	@CertificationDate DATE = NULL,
	@AppliedDate DATE = NULL,
	@DisbursementDate DATE = NULL,
	@ApprovalStatus VARCHAR(15),
	@SourceModule VARCHAR(100)

AS
	INSERT INTO batchesp.TsayScrapedLoanInformation(BorrowerSsn, LoanSequence, LoanProgramType, [Type], DeferSchool, BeginDate, EndDate, CertificationDate, AppliedDate, DisbursementDate, ApprovalStatus, SourceModule)
	VALUES (@BorrowerSsn, @LoanSequence, @LoanProgramType, @Type, @DeferSchool, @BeginDate, @EndDate, @CertificationDate, @AppliedDate, @DisbursementDate, @ApprovalStatus, @SourceModule)

RETURN 0