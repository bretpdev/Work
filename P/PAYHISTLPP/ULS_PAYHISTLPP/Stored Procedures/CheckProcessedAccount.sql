CREATE PROCEDURE [payhistlpp].[CheckProcessedAccount]
	@Account VARCHAR(10)
AS
	SELECT DISTINCT
		CASE
			WHEN A.ProcessedAt IS NOT NULL AND A.DeletedAt IS NULL THEN CAST(1 AS BIT)
			ELSE CAST(0 AS BIT)
		END AS IsProcessed
	FROM
		ULS.payhistlpp.Accounts A
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = A.Ssn
	WHERE
		@Account IN (PD10.DF_PRS_ID, PD10.DF_SPE_ACC_ID)