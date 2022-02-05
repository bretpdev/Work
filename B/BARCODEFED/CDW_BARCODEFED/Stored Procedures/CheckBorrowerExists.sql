
CREATE PROCEDURE barcodefed.CheckBorrowerExists
	@AccountNumber varchar(10)
AS
BEGIN

	SELECT
		DF_SPE_ACC_ID
	FROM
		PD10_PRS_NME
	WHERE
		@AccountNumber IN (DF_SPE_ACC_ID, DF_PRS_ID)

END