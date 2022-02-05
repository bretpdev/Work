CREATE PROCEDURE [payhistlpp].[InsertSingleRecord]
	@UserAccessId INT,
	@RunId INT,
	@Ssn VARCHAR(10),
	@Lender VARCHAR(6)
AS

	DECLARE @Account VARCHAR(9)
	IF (LEN(@Ssn) = 10)
		BEGIN
			SET @Account = (SELECT BF_SSN FROM ULS..PD10_PRS_NME WHERE DF_SPE_ACC_ID = @Ssn)
		END
	ELSE
		SET @Account = @Ssn

	INSERT INTO ULS.payhistlpp.Accounts(Ssn, UserAccessId, RunId, Lender)
	VALUES(@Account, @UserAccessId, @RunId, @Lender)