CREATE PROCEDURE _test
(
	
	@Ssn CHAR(9),
	@AccountNumber CHAR(10) OUTPUT,
	@ScopeIdentity INT OUTPUT
)

AS

	SELECT
		@AccountNumber = PD10.DF_SPE_ACC_ID
	FROM
		CDW..PD10_PRS_NME PD10
	WHERE
		PD10.DF_PRS_ID = @Ssn

	SELECT @ScopeIdentity = SCOPE_IDENTITY()