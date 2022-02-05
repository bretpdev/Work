DECLARE @30DaysPrior DATE = DATEADD(DAY,-31,GETDATE());
DECLARE @ScriptDataId INT = (SELECT ScriptDataId from ULS.[print].ScriptData WHERE ScriptID = 'SKPBRWATRN')
DECLARE @TODAY DATE = GETDATE();
INSERT INTO ULS.[print].PrintProcessing(AccountNumber, EmailAddress, ScriptDataId, SourceFile, LetterData, CostCenter, InValidAddress, DoNotProcessEcorr, OnEcorr, ArcNeeded, ImagingNeeded, AddedAt, AddedBy)
SELECT DISTINCT
	BorrDemos.DF_SPE_ACC_ID AS AccountNumber,
	'Ecorr@Uheaa.org' AS EmailAddress, --Since letter is going to Reference we could get their email here, but they wont have an ecorr account
	@ScriptDataId AS ScriptDataId,
	NULL AS SourceFile,
		CAST(RF10.BF_SSN AS VARCHAR(9)) + ',' +
		CAST(RF10.BF_RFR AS VARCHAR(9)) + ',' +
		'BWNHDGF' + ',' +
		CentralData.dbo.CreateACSKeyline(RF10.BF_SSN,'B','L') + ',' +
		'"' + IIF(RefDemos.DM_PRS_MID = '',RTRIM(RefDemos.DM_PRS_1) + ' ' + RTRIM(RefDemos.DM_PRS_LST), RTRIM(RefDemos.DM_PRS_1) + ' ' + RTRIM(RefDemos.DM_PRS_MID) + ' ' + RTRIM(RefDemos.DM_PRS_LST)) + '"' + ',' +
		'"' + ISNULL(RTRIM(RefDemos.DX_STR_ADR_1),'') + '"' + ',' +
		'"' + ISNULL(RTRIM(RefDemos.DX_STR_ADR_2),'') + '"' + ',' +
		'"' + ISNULL(RTRIM(RefDemos.DM_CT),'') + '"' + ',' +
		'"' + IIF(RefDemos.DC_DOM_ST = 'FC','',ISNULL(RTRIM(RefDemos.DC_DOM_ST),'')) + '"' + ',' +
		'"' + ISNULL(RTRIM(RefDemos.DF_ZIP_CDE),'') + '"' + ',' +
		'"' + ISNULL(RTRIM(RefDemos.DM_FGN_CNY),'') + '"' + ',' +
		ISNULL(Lender.LF_STU_SSN,'') + ',' +
		'"' + IIF(BorrDemos.DM_PRS_MID = '',RTRIM(BorrDemos.DM_PRS_1) + ' ' + RTRIM(BorrDemos.DM_PRS_LST), RTRIM(BorrDemos.DM_PRS_1) + ' ' + RTRIM(BorrDemos.DM_PRS_MID) + ' ' + RTRIM(BorrDemos.DM_PRS_LST)) + '"' + ',' +
		BorrDemos.DF_SPE_ACC_ID + ',' +
		PD24.BKY_CASE + ',' +
		'"' + ISNULL(RTRIM(BorrDemos.B_DX_STR_ADR_1),'') + '"' + ',' +
		'"' + ISNULL(RTRIM(BorrDemos.B_DX_STR_ADR_2),'') + '"' + ',' +
		'"' + ISNULL(RTRIM(BorrDemos.B_DM_CT),'') + '"' + ',' +
		'"' + ISNULL(RTRIM(BorrDemos.B_DC_DOM_ST), '') + '"' + ',' +
		'"' + ISNULL(RTRIM(BorrDemos.B_DF_ZIP_CDE), '') + '"' + ',' +
		'"' + ISNULL(RTRIM(BorrDemos.B_DM_FGN_CNY), '') + '"' + ',' +
		'"' + ISNULL(RTRIM(BorrDemos.DN_PHN), '') + '"' + ',' +
		'"' + ISNULL(RTRIM(BorrDemos.B_DC_DOM_ST), '') + '"' + ',' +
		IIF(Lender.LF_LON_CUR_OWN IN('828476','834396','834437','82847601','834493','834529','826717','830248','971357','900749'), 'MA2324', 'MA2327') 
	AS LetterData,
	IIF(Lender.LF_LON_CUR_OWN IN('828476','834396','834437','82847601','834493','834529','826717','830248','971357','900749'), 'MA2324', 'MA2327') AS CostCenter,
	0 AS InvalidAddress,
	1 AS DoNotProcessEcorr, --Letter goes to reference.  Probably wont need ecorr
	0 AS OnEcorr,
	1 AS ArcNeeded,
	0 AS ImagingNeeded,
	GETDATE() AS AddedAt,
	SUSER_SNAME() AS AddedBy
FROM 
	UDW..RF10_RFR RF10
	INNER JOIN 
	( --Get borrowers with bad addresses and bad phones
		SELECT DISTINCT 
			DF_PRS_ID AS BF_SSN
		FROM 
			UDW..PD30_PRS_ADR 
		WHERE 
			DC_ADR = 'L'
			AND DI_VLD_ADR = 'N'

		UNION ALL

		SELECT DISTINCT 
			DF_PRS_ID AS BF_SSN
		FROM 
			UDW..PD42_PRS_PHN 
		WHERE 
			DI_PHN_VLD = 'N'
			AND DC_PHN = 'H'
	) BadDemos
		ON RF10.BF_SSN = BadDemos.BF_SSN
	INNER JOIN 
	( --Lender logic for cost center code
		SELECT DISTINCT 
			LN10.BF_SSN,
			LN10.LF_STU_SSN,
			LN10.LF_LON_CUR_OWN AS LF_LON_CUR_OWN
		FROM 
			UDW..LN10_LON LN10
		WHERE 
			LN10.LA_CUR_PRI > 0
			AND LN10.LC_STA_LON10 = 'R'
			AND LN10.LF_LON_CUR_OWN != '826717'
	) Lender
		ON RF10.BF_SSN = Lender.BF_SSN
	INNER JOIN 
	( --Bankruptcy info
		SELECT DISTINCT 
			PD24.DF_PRS_ID AS BF_SSN,
			PD24.DD_BKR_STA,
			PD24.DF_COU_DKT AS BKY_CASE,
			PD24.DF_ATT 
		FROM 
			UDW..PD24_PRS_BKR PD24
			INNER JOIN
			(
				SELECT 
					PD24.DF_PRS_ID,
					MAX(PD24.DD_BKR_STA) AS DD_BKR_STA
				FROM 
					UDW..PD24_PRS_BKR PD24
				WHERE
					PD24.DF_COU_DKT != ''
				GROUP BY
					PD24.DF_PRS_ID
			) MaxBky
				ON MaxBky.DF_PRS_ID = PD24.DF_PRS_ID
				AND MaxBky.DD_BKR_STA = PD24.DD_BKR_STA
		WHERE 
			PD24.DF_COU_DKT != ''
	) PD24
		ON RF10.BF_SSN = PD24.BF_SSN
		AND RF10.BF_RFR = PD24.DF_ATT
	LEFT JOIN 
	(
		SELECT DISTINCT 
			PD10.DF_PRS_ID AS BF_SSN,
			PD10.DF_SPE_ACC_ID,
			PD10.DM_PRS_MID,
			PD10.DM_PRS_1,
			PD10.DM_PRS_LST,
			PD30.DX_STR_ADR_1 AS B_DX_STR_ADR_1,
			PD30.DX_STR_ADR_2 AS B_DX_STR_ADR_2,
			PD30.DM_CT AS B_DM_CT,
			PD30.DC_DOM_ST AS B_DC_DOM_ST,
			PD30.DF_ZIP_CDE AS B_DF_ZIP_CDE,
			PD30.DM_FGN_CNY AS B_DM_FGN_CNY,
			PD30.DC_ADR,
			PD42.DN_DOM_PHN_ARA + PD42.DN_DOM_PHN_XCH + PD42.DN_DOM_PHN_LCL AS DN_PHN
		FROM 
			UDW..PD10_PRS_NME PD10
			INNER JOIN UDW..PD30_PRS_ADR PD30
				ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
				AND PD30.DC_ADR = 'L'
				--Borrower address will likely be invalid as we are trying to get a good address from attorney
			INNER JOIN UDW..PD42_PRS_PHN PD42
				ON PD42.DF_PRS_ID = PD10.DF_PRS_ID
				AND PD42.DC_PHN = 'H'
				--Borrower phone will likely be invalid as we are trying to get a good phone from attorney
	) BorrDemos
		ON RF10.BF_SSN = BorrDemos.BF_SSN
	LEFT JOIN
	(
		SELECT DISTINCT 
			PD10.DF_PRS_ID AS BF_SSN,
			PD10.DM_PRS_MID,
			PD10.DM_PRS_1,
			PD10.DM_PRS_LST,
			PD30.DX_STR_ADR_1,
			PD30.DX_STR_ADR_2,
			PD30.DM_CT,
			PD30.DC_DOM_ST,
			PD30.DF_ZIP_CDE,
			PD30.DM_FGN_CNY
		FROM
			UDW..PD10_PRS_NME PD10
			INNER JOIN UDW..PD30_PRS_ADR PD30
				ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
	) RefDemos
		ON RF10.BF_RFR = RefDemos.BF_SSN
	LEFT JOIN UDW..WQ20_TSK_QUE WQ20
		ON WQ20.BF_SSN = RF10.BF_SSN
		AND WQ20.WF_QUE = 'KA'
		AND WQ20.WF_SUB_QUE = '01'
		AND WQ20.WC_STA_WQUE20 NOT IN('X','C')	
	LEFT JOIN UDW..AY10_BR_LON_ATY AY10
		ON AY10.BF_SSN = BadDemos.BF_SSN
		AND AY10.PF_REQ_ACT = 'KATNY'
		AND AY10.LC_STA_ACTY10 = 'A'
		AND AY10.LD_ATY_REQ_RCV > @30DaysPrior
	LEFT JOIN ULS.[print].PrintProcessing EXISTING_DATA
		ON EXISTING_DATA.AccountNumber = BorrDemos.DF_SPE_ACC_ID
		AND EXISTING_DATA.EmailAddress = 'Ecorr@Uheaa.org'
		AND EXISTING_DATA.ScriptDataId = @ScriptDataId
		AND CONVERT(DATE,EXISTING_DATA.AddedAt) = @TODAY
		AND 
		(
			EXISTING_DATA.EcorrDocumentCreatedAt IS NULL
			OR CAST(EXISTING_DATA.EcorrDocumentCreatedAt AS DATE) = @TODAY
		)
		AND
		(
			EXISTING_DATA.PrintedAt IS NULL
			OR CAST(EXISTING_DATA.PrintedAt AS DATE) = @TODAY
		)
		AND EXISTING_DATA.DeletedAt IS NULL
	WHERE
		EXISTING_DATA.AccountNumber IS NULL --Not already in the database
		AND RF10.BC_STA_REFR10 = 'A'
		AND RF10.BC_RFR_REL_BR = '15'
		AND WQ20.BF_SSN IS NULL --No outstanding task
		AND AY10.BF_SSN IS NULL --No arc in last 30
ORDER BY 
	DF_SPE_ACC_ID,
	CostCenter