CREATE PROCEDURE [batchesp].[HasOpenReviewTask]
	@BorrowerSsn CHAR(9)
AS
	
	SELECT
		CAST(CASE WHEN COUNT(WQ20.BF_SSN) + COUNT(AAP.AccountNumber) > 0 THEN 1 ELSE 0 END AS BIT) AS HasTask
	FROM
		UDW..PD10_PRS_NME PD10
		LEFT JOIN UDW..WQ20_TSK_QUE WQ20
			ON WQ20.BF_SSN = PD10.DF_PRS_ID
			AND WQ20.WF_QUE = 'DR'
			AND WQ20.WC_STA_WQUE20 IN ('U', 'H', 'W', 'A')
			AND WQ20.BF_SSN = @BorrowerSsn
		LEFT JOIN ULS..ArcAddProcessing AAP
			ON AAP.AccountNumber = PD10.DF_SPE_ACC_ID
			AND AAP.ARC = 'ERBEQ'
			AND 
			(
				AAP.ProcessedAt IS NULL --ARC will be added
				OR
				(
					AAP.ProcessedAt IS NOT NULL
					AND AAP.CreatedAt >= DATEADD(HOUR, -4, GETDATE()) --Recently added ARC. Not enough time has elapsed to ensure WQ20 refresh
				)
			)

RETURN 0
