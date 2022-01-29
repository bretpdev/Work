
CREATE PROCEDURE [print].[AddPIFLetterData]
	@ScriptDataId INT ,
	@AccountNumber VARCHAR(10),
	@CostCenter VARCHAR(10),
	@LetterData VARCHAR(MAX)
AS
	INSERT INTO ULS.[print].PrintProcessing(AccountNumber, EmailAddress, ScriptDataId, LetterData, CostCenter, OnEcorr, ArcNeeded, ImagingNeeded, AddedBy)
	SELECT TOP 1
		@AccountNumber,
		ISNULL(PH05.DX_CNC_EML_ADR, 'ECORR@UHEAA.ORG'),
		@ScriptDataId,
		@LetterData,
		@CostCenter,
		CASE
			WHEN PH05.DF_SPE_ID IS NOT NULL AND PH05.DI_VLD_CNC_EML_ADR = 'Y' AND PH05.DI_CNC_ELT_OPI = 'Y' THEN 1
			ELSE 0
		END,
		1,
		1,
		'PIFLTR'
	FROM
		UDW..PD10_PRS_NME PD10
		LEFT JOIN UDW..PH05_CNC_EML PH05
			ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
RETURN 0
