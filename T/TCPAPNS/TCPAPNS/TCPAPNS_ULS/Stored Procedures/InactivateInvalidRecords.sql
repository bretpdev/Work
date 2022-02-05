CREATE PROCEDURE [tcpapns].[InactivateInvalidRecords]
	
AS
DECLARE 
	@AccountNumberIndex INT = 0,
	@PhoneIndex INT = 1,
	@DateIndex INT = 2,
	@WirIndex INT = 3

BEGIN
	UPDATE 
		FP
	SET 
		DeletedAt = GETDATE(),
		DeletedBy = SUSER_NAME()
	FROM
		(
			SELECT
				FP.AccountNumber,
				FP.AddedAt,
				FP.AddedBy,
				FP.ArcAddProcessingId,
				FP.DeletedAt,
				FP.DeletedBy,
				FP.FileProcessingId,
				FP.GroupKey,
				FP.HasConsentArc,
				FP.LineData,
				FP.MobileIndicator,
				FP.Phone,
				FP.ProcessedOn,
				FP.RecordDate,
				FP.SourceFile
			FROM

			[tcpapns].FileProcessing FP
		) FP
		LEFT JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_SPE_ACC_ID = FP.AccountNumber
		LEFT JOIN 
		(
			SELECT 
				DF_PRS_ID,
				DC_PHN,
				DN_DOM_PHN_ARA,
				DN_DOM_PHN_XCH,
				DN_DOM_PHN_LCL,
				DC_ALW_ADL_PHN
			FROM
				UDW..PD42_PRS_PHN
			WHERE
				DI_PHN_VLD = 'Y'
		) PD42
			ON PD10.DF_PRS_ID = PD42.DF_PRS_ID
			AND FP.Phone = PD42.DN_DOM_PHN_ARA + PD42.DN_DOM_PHN_XCH + PD42.DN_DOM_PHN_LCL
			AND
			(
				(
					FP.MobileIndicator = 1 
					AND PD42.DC_ALW_ADL_PHN = 'N' 
					AND FP.HasConsentArc = 1
				)
				OR
				(
					FP.MobileIndicator = 1 
					AND PD42.DC_ALW_ADL_PHN IN ('L', 'U', 'Q', 'X')
				)
				OR
				(
					FP.MobileIndicator = 0 
					AND PD42.DC_ALW_ADL_PHN IN ('P', 'N', 'U', 'Q', 'X')
				)
			)		
	WHERE
		FP.ProcessedOn IS NULL
		AND FP.DeletedAt IS NULL
		AND FP.DeletedBy IS NULL
		AND UPPER(FP.GroupKey) NOT LIKE('P%')
		AND PD42.DF_PRS_ID IS NULL

		
END