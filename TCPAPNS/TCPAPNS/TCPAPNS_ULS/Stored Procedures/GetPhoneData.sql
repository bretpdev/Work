CREATE PROCEDURE [tcpapns].[GetPhoneData]
	@AccountIdentifier VARCHAR(10),
	@Phone VARCHAR(12),
	@MobileIndicator BIT,
	@HasConsentArc BIT
AS	
	SELECT 
		PD42.DC_PHN
	FROM
		UDW..PD10_PRS_NME PD10
		INNER JOIN UDW..PD42_PRS_PHN PD42
			ON PD10.DF_PRS_ID = PD42.DF_PRS_ID
	WHERE
		@AccountIdentifier IN (PD10.DF_PRS_ID, PD10.DF_SPE_ACC_ID)
		AND DN_DOM_PHN_ARA + DN_DOM_PHN_XCH + DN_DOM_PHN_LCL = @Phone
		AND
		(
			(@MobileIndicator = 1 AND DC_ALW_ADL_PHN  = 'N' AND @HasConsentArc = 1) OR
			(@MobileIndicator = 1 AND DC_ALW_ADL_PHN IN ('L', 'U', 'X', 'Q')) OR --Q is needed since it is a uheaa compass only flag
			(@MobileIndicator = 0 AND DC_ALW_ADL_PHN IN ('P', 'N', 'U', 'X','Q'))
		)

