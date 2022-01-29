CREATE PROCEDURE [payhistlpp].[GetSsn]
	@Account VARCHAR(10)
AS
	SELECT
		DF_PRS_ID
	FROM
		UDW..PD10_PRS_NME
	WHERE
		DF_SPE_ACC_ID = @Account