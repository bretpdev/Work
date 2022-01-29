USE UDW
GO

TRUNCATE TABLE UDW.[calc].[IvrDataStaging];

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
SET ANSI_WARNINGS OFF;

DECLARE @TODAY DATE = GETDATE();

INSERT INTO calc.IvrDataStaging(SSN,AccountNumber,DateOfBirth,IVRDateOfBirth,ZipIvr,[Name],MultiDueDate,Bankruptcy,DaysPastDue,IsCompleteLoan,HasSpousalLoans,Military,Principal,Interest,Fees,TotalBalance,PastDue,CurrentAmoutDue,TotalDue,DueDate,MonthlyPaymentAmount,LastPaymentDate,IVRLastPaymentDate,LastPaymentAmount,TaxYear1098,TaxAmount1098,TaxYear1099,TaxAmount1099,PrimaryPhoneNumber,PrimaryPhoneValidity,PrimaryPhoneConsent,SecondPhoneNumber,SecondPhoneValidity,SecondPhoneConsent,ThirdPhoneNumber,ThirdPhoneValidity,ThirdPhoneConsent,FourthPhoneNumber,FourthPhoneValidity,FourthPhoneConsent,LegalAddress1,LegalAddress2,LegalAddressCity,LegalAddressState,LegalAddressZipCode,LegalAddressCountry,LegalAddressValidity,MultiDueDateCount,AddressLastVerified,PhoneLastVerified)
SELECT DISTINCT
	PD10.DF_PRS_ID AS SSN,
	PD10.DF_SPE_ACC_ID AS AccountNumber,
	CAST(PD10.DD_BRT AS DATE) AS DateOfBirth,
	CONVERT(VARCHAR(10), CAST(PD10.DD_BRT AS DATE), 101) AS IVRDateOfBirth,
	SUBSTRING(DF_ZIP_CDE,1,5) AS ZipIvr,
	LTRIM(RTRIM(PD10.DM_PRS_1))+ ' ' + LTRIM(RTRIM(PD10.DM_PRS_LST)) AS [Name],
	CASE 
		WHEN RS.DUE_DATE_COUNT > 1 THEN 1
		ELSE 0
	END  AS MultiDueDate,
	CASE 
		WHEN DW01.BK_STATUS > 0 THEN 1 
		ELSE 0 
	END AS Bankruptcy,
	ISNULL(LN16.LN_DELQ_MAX, 0) AS DaysPastDue,
	CASE
		WHEN COMPLT.LOAN_COUNT = LN10.LOAN_COUNT THEN 1
		ELSE 0
	END AS IsCompleteLoan,
	LN10.HAS_SPOUSAL AS HasSpousalLoans,
	CASE 
		WHEN SCRA.BorrSSN IS NOT NULL THEN 1
		ELSE 0
	END AS Military,
	LN10.LA_CUR_PRI AS Principal,
	ISNULL(DW01.WA_TOT_BRI_OTS,0) AS Interest,
	LN10.LA_LTE_FEE_OTS AS Fees,
	LN10.LA_CUR_PRI + ISNULL(DW01.WA_TOT_BRI_OTS,0) + LN10.LA_LTE_FEE_OTS AS TotalBalance,
	BILL.PAST_DUE AS PastDue,
	BILL.CUR_DUE AS CurrentAmoutDue,
	(BILL.PAST_DUE  + BILL.CUR_DUE  +  LN10.LA_LTE_FEE_OTS ) AS TotalDue,
	RS.DUE_DAY AS DueDate,
	RS.LA_RPS_ISL AS MonthlyPaymentAmount,
	CAST(LN90.LST_PMT_RCVD AS DATE) AS LastPaymentDate,
	CONVERT(VARCHAR(10), CAST(LN90.LST_PMT_RCVD AS DATE), 101) AS IVRLastPaymentDate,
	ABS(LN90.LST_AMT_IVR) AS LastPaymentAmount,
	MR64.Year1098 AS TaxYear1098,
	MR64.Amount1098 AS TaxAmount1098,
	MR65.Year1099 AS TaxYear1099,
	MR65.Amount1099 AS TaxAmount1099,
	PD42.HOME AS PrimaryPhoneNumber,
	PD42.HOME_VLD AS PrimaryPhoneValidity,
	CASE 
		WHEN PD42.HOME_CONSENT IN ('L', 'P', 'X') THEN 'Y'
		ELSE 'N'
	END AS PrimaryPhoneConsent,
	PD42.ALT AS SecondPhoneNumber,
	PD42.ALT_VLD AS SecondPhoneValidity,
	CASE 
		WHEN PD42.ALT_CONSENT IN ('L', 'P', 'X') THEN 'Y'
		ELSE 'N'
	END AS SecondPhoneConsent,
	PD42.WORK AS ThirdPhoneNumber,
	PD42.WORK_VLD AS ThirdPhoneValidity,
	CASE 
		WHEN PD42.WORK_CONSENT IN ('L', 'P', 'X') THEN 'Y'
		ELSE 'N'
	END AS ThirdPhoneConsent,
	PD42.MOBILE AS FourthPhoneNumber,
	PD42.MOBILE_VLD AS FourthPhoneValidity,
	CASE 
		WHEN PD42.MOBILE_CONSENT IN ('L', 'P', 'X') THEN 'Y'
		ELSE 'N'
	END AS FourthPhoneConsent,
	REPLACE(REPLACE(PD30.DX_STR_ADR_1,'PO BOX', 'P.O. Box'), 'BOX', 'Box') AS LegalAddress1,
	REPLACE(REPLACE(PD30.DX_STR_ADR_2, 'PO BOX', 'P.O. Box'), 'BOX', 'Box') AS LegalAddress2,
	PD30.DM_CT AS LegalAddressCity,
	CASE 
		WHEN ISNULL(LK10.PX_DSC_LNG, '') != '' THEN LK10.PX_DSC_LNG
		WHEN PD30.DC_DOM_ST != '' THEN PD30.DC_DOM_ST
		ELSE PD30.DM_FGN_ST 
	END AS LegalAddressState,
	PD30.DF_ZIP_CDE AS LegalAddressZipCode,
	PD30.DM_FGN_CNY AS LegalAddressCountry,
	PD30.DI_VLD_ADR AS LegalAddressValidity,
	CASE 
		WHEN RS.DUE_DATE_COUNT > 1 THEN RS.DUE_DATE_COUNT
		ELSE 1
	END AS MultiDueDateCount,
	CONVERT(VARCHAR(10), CAST(PD30.DD_VER_ADR AS DATE), 101) AS [AddressLastVerified],
	CONVERT(VARCHAR(10), CAST(PD42.PHONE_VERIFIED AS DATE), 101) AS [PhoneLastVerified]
FROM
	UDW..PD10_PRS_NME PD10
	INNER JOIN  /*Gets borrowers LN10 data*/
	(
		SELECT	
			LN10.BF_SSN,
			SUM(LN10.LA_CUR_PRI) AS LA_CUR_PRI,
			SUM(ISNULL(LA_LTE_FEE_OTS,0)) AS  LA_LTE_FEE_OTS,
			MAX(CASE 
				WHEN LA_CUR_PRI > 0 AND  IC_LON_PGM IN ('SPCNSL', 'SUBSPC', 'UNSPC', 'DLSPCN', 'DLSSPL', 'DLUSPL') THEN 1
				ELSE 0
			END) AS HAS_SPOUSAL,
			COUNT(*) AS LOAN_COUNT
		FROM
			UDW..LN10_LON LN10
		WHERE
			LN10.LC_STA_LON10 IN ('R','D','L')
		GROUP BY
			LN10.BF_SSN
	)LN10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
	LEFT JOIN /*Will grab outstanding interest and if the borrower is in a BK status*/
	(
		SELECT	
			DW01.BF_SSN,
			SUM(ISNULL(DW01.WA_TOT_BRI_OTS,0)) AS WA_TOT_BRI_OTS,
			SUM(CASE WHEN DW01.WC_DW_LON_STA = '21' THEN 1 ELSE 0 END) AS BK_STATUS
		FROM
			UDW..DW01_DW_CLC_CLU DW01
			INNER JOIN UDW..LN10_LON LN10
				ON LN10.BF_SSN = DW01.BF_SSN
				AND LN10.LN_SEQ = DW01.LN_SEQ
		WHERE
			LN10.LC_STA_LON10 IN ('R','L')
		GROUP BY
			DW01.BF_SSN
	)DW01
		ON DW01.BF_SSN = PD10.DF_PRS_ID
	LEFT JOIN UDW..PD30_PRS_ADR PD30
		ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
		AND PD30.DC_ADR = 'L'	
	LEFT JOIN /*Gets all of the borrowers phone information we do not care about priority we just need all of the data*/
	(
		SELECT 
			DF_PRS_ID,
			MAX(CASE WHEN PD42.DC_PHN = 'H' THEN ISNULL(PD42.DN_DOM_PHN_ARA,'') +  ISNULL(PD42.DN_DOM_PHN_XCH,'')   +  ISNULL(DN_DOM_PHN_LCL,'')   + ISNULL(DN_FGN_PHN_INL,'')   + ISNULL(DN_FGN_PHN_CNY,'')   + ISNULL(DN_FGN_PHN_CT,'')   + ISNULL(DN_FGN_PHN_LCL,'')  END) AS HOME,
			MAX(CASE WHEN PD42.DC_PHN = 'H' THEN ISNULL(PD42.DI_PHN_VLD, '') END) AS HOME_VLD,
			MAX(CASE WHEN PD42.DC_PHN = 'H' THEN ISNULL(PD42.DC_ALW_ADL_PHN, '')   END) AS HOME_CONSENT,
			MAX(CASE WHEN PD42.DC_PHN = 'A' THEN ISNULL(PD42.DN_DOM_PHN_ARA,'') +  ISNULL(PD42.DN_DOM_PHN_XCH,'')   +  ISNULL(DN_DOM_PHN_LCL,'')   + ISNULL(DN_FGN_PHN_INL,'')   + ISNULL(DN_FGN_PHN_CNY,'')   + ISNULL(DN_FGN_PHN_CT,'')   + ISNULL(DN_FGN_PHN_LCL,'')  END) AS ALT,
			MAX(CASE WHEN PD42.DC_PHN = 'A' THEN ISNULL(PD42.DI_PHN_VLD, '')  END) AS ALT_VLD,
			MAX(CASE WHEN PD42.DC_PHN = 'A' THEN ISNULL(PD42.DC_ALW_ADL_PHN, '')   END) AS ALT_CONSENT,
			MAX(CASE WHEN PD42.DC_PHN = 'W' THEN ISNULL(PD42.DN_DOM_PHN_ARA,'') +  ISNULL(PD42.DN_DOM_PHN_XCH,'')   +  ISNULL(DN_DOM_PHN_LCL,'')   + ISNULL(DN_FGN_PHN_INL,'')   + ISNULL(DN_FGN_PHN_CNY,'')   + ISNULL(DN_FGN_PHN_CT,'')   + ISNULL(DN_FGN_PHN_LCL,'')  END) AS WORK,
			MAX(CASE WHEN PD42.DC_PHN = 'W' THEN ISNULL(PD42.DI_PHN_VLD, '')  END) AS WORK_VLD,
			MAX(CASE WHEN PD42.DC_PHN = 'W' THEN ISNULL(PD42.DC_ALW_ADL_PHN, '')   END) AS WORK_CONSENT,
			MAX(CASE WHEN PD42.DC_PHN = 'M' THEN ISNULL(PD42.DN_DOM_PHN_ARA,'') +  ISNULL(PD42.DN_DOM_PHN_XCH,'')   +  ISNULL(DN_DOM_PHN_LCL,'')   + ISNULL(DN_FGN_PHN_INL,'')   + ISNULL(DN_FGN_PHN_CNY,'')   + ISNULL(DN_FGN_PHN_CT,'')   + ISNULL(DN_FGN_PHN_LCL,'')  END) AS MOBILE,
			MAX(CASE WHEN PD42.DC_PHN = 'M' THEN ISNULL(PD42.DI_PHN_VLD,'') END) AS MOBILE_VLD,
			MAX(CASE WHEN PD42.DC_PHN = 'M' THEN ISNULL(PD42.DC_ALW_ADL_PHN,'')  END) AS MOBILE_CONSENT,
			MAX(CASE WHEN PD42.DC_PHN = 'H' THEN ISNULL(PD42.DD_PHN_VER,'1900-01-01') END) AS PHONE_VERIFIED
		FROM
			UDW..PD42_PRS_PHN PD42
		GROUP BY
			DF_PRS_ID
	) PD42
		ON PD42.DF_PRS_ID = PD10.DF_PRS_ID
	LEFT JOIN
	(
		SELECT	
			BF_SSN,
			(MAX(LN16.LN_DLQ_MAX) + 1) AS LN_DELQ_MAX
		FROM
			UDW..LN16_LON_DLQ_HST LN16
		WHERE
			LN16.LC_STA_LON16 = '1'
			AND DATEDIFF(DAY, CAST(LN16.LD_DLQ_MAX AS DATE), @TODAY) < 5
		GROUP BY
			LN16.BF_SSN
	) LN16
		ON LN16.BF_SSN = PD10.DF_PRS_ID
	LEFT JOIN /*GETS THE BORROWERS LAST PAYMENT DATE AND AMOUNT*/
	(
		SELECT DISTINCT
			LN90.BF_SSN,
			LN90_M.LD_FAT_EFF AS LST_PMT_RCVD,
			CAST(SUM(ISNULL(LA_FAT_CUR_PRI,0) + ISNULL(LA_FAT_NSI, 0) + ISNULL(LA_FAT_PCL_FEE,0) + ISNULL(LA_FAT_LTE_FEE,0)) OVER(PARTITION BY LN90.BF_SSN) AS VARCHAR(12)) AS LST_AMT_IVR
		FROM
			UDW..LN90_FIN_ATY LN90
			INNER JOIN 
			(
				SELECT
					LN90.BF_SSN,
					MAX(LN90.LD_FAT_EFF) AS LD_FAT_EFF
				FROM
					UDW..LN90_FIN_ATY LN90
				WHERE
					LN90.PC_FAT_TYP = '10'
					AND LN90.PC_FAT_SUB_TYP = '10'
					AND LN90.LC_STA_LON90 = 'A'
					AND LTRIM(RTRIM(ISNULL(LN90.LC_FAT_REV_REA,''))) = '' 
				GROUP BY
					LN90.BF_SSN
			) LN90_M
				ON LN90_M.BF_SSN = LN90.BF_SSN
				AND LN90_M.LD_FAT_EFF = LN90.LD_FAT_EFF
		WHERE
			LN90.PC_FAT_TYP = '10'
			AND LN90.PC_FAT_SUB_TYP = '10'
			AND LN90.LC_STA_LON90 = 'A'
			AND LTRIM(RTRIM(ISNULL(LN90.LC_FAT_REV_REA,''))) = '' 

	)LN90
		ON LN90.BF_SSN = PD10.DF_PRS_ID
	LEFT JOIN /*GET THE BORROWERS BILLING INFORMATION*/
	(
		SELECT	
			POP.BF_SSN,
			CASE 
				WHEN POP.CUR_DUE < 0 THEN 0
				ELSE POP.CUR_DUE
			END AS CUR_DUE,
			CASE 
				WHEN POP.PAST_DUE < 0 THEN 0
				ELSE POP.PAST_DUE
			END AS PAST_DUE
		FROM
		(
			SELECT DISTINCT
				LN10.BF_SSN,
				SUM(ISNULL(CUR.CUR_DUE,0)) AS CUR_DUE,
				SUM(ISNULL(PST.PAST_DUE,0)) AS PAST_DUE
			FROM 
				UDW..LN10_LON LN10
				INNER JOIN UDW..PD10_PRS_NME PD10
					ON PD10.DF_PRS_ID = LN10.BF_SSN
				LEFT JOIN 
				(
					SELECT
						LN80.BF_SSN,
						LN80.LN_SEQ,
						CASE WHEN SUM(COALESCE(LN80.LA_BIL_CUR_DU,0) - COALESCE(LN80.LA_TOT_BIL_STS,0)) < 0 THEN 0 
						ELSE SUM(COALESCE(LN80.LA_BIL_CUR_DU,0) - COALESCE(LN80.LA_TOT_BIL_STS,0)) END  AS CUR_DUE
					FROM 
						UDW..LN80_LON_BIL_CRF LN80
						INNER JOIN 
						(
							SELECT
								LN80.BF_SSN,
								LN80.LN_SEQ,
								MIN(LN80.LD_BIL_DU_LON) AS LD_BIL_DU_LON
							FROM 
								UDW..LN80_LON_BIL_CRF LN80
								INNER JOIN UDW..PD10_PRS_NME PD10
									ON PD10.DF_PRS_ID = LN80.BF_SSN
								INNER JOIN UDW..LN10_LON LN10
									ON LN10.BF_SSN = LN80.BF_SSN
									AND LN10.LN_SEQ = LN80.LN_SEQ
							WHERE 
								LN80.LC_STA_LON80 = 'A'
								AND LN80.LC_LON_STA_BIL = '1'
								AND CAST(LN80.LD_BIL_DU_LON AS DATE) >= @TODAY
								AND LN10.LC_STA_LON10 IN ('R','L')
								AND LN10.LA_CUR_PRI > 0
							GROUP BY 
								LN80.BF_SSN,
								LN80.LN_SEQ
						) MIN_DTE
							ON LN80.BF_SSN = MIN_DTE.BF_SSN
								AND LN80.LN_SEQ = MIN_DTE.LN_SEQ
								AND LN80.LD_BIL_DU_LON = MIN_DTE.LD_BIL_DU_LON
					WHERE 
						LN80.LC_STA_LON80 = 'A'
						AND LN80.LC_LON_STA_BIL = '1'
					GROUP BY 
						LN80.BF_SSN,
						LN80.LN_SEQ
				) CUR
				ON LN10.BF_SSN = CUR.BF_SSN
				AND LN10.LN_SEQ = CUR.LN_SEQ
			LEFT JOIN /*GETS THE CURRENT BILL INFORMATION*/
			(
				SELECT
					LN80.BF_SSN,
					LN80.LN_SEQ,
					SUM(COALESCE(LN80.LA_BIL_CUR_DU,0) - COALESCE(LN80.LA_TOT_BIL_STS,0)) AS PAST_DUE,
					SUM(isnull(LN80.LA_BIL_CUR_DU,0)) AS LA_BIL_CUR_DU,
					SUM(ISNULL(LN80.LA_TOT_BIL_STS, 0)) AS LA_TOT_BIL_STS
				FROM 
					UDW..LN80_LON_BIL_CRF LN80
					INNER JOIN UDW..LN10_LON LN10
						ON LN10.BF_SSN = LN80.BF_SSN
						AND LN10.LN_SEQ = LN80.LN_SEQ
				WHERE 
					LN80.LC_STA_LON80 = 'A'
					AND LN80.LC_LON_STA_BIL = '1'
					AND CAST(LN80.LD_BIL_DU_LON AS DATE) < @TODAY
					AND LN10.LC_STA_LON10 IN ('R','L')
					AND LN10.LA_CUR_PRI > 0
				GROUP BY 
					LN80.BF_SSN,
					LN80.LN_SEQ
			) PST
				ON LN10.BF_SSN = PST.BF_SSN
				AND LN10.LN_SEQ = PST.LN_SEQ
			WHERE
				LN10.LC_STA_LON10 IN ('R','L')
				AND LN10.LA_CUR_PRI > 0
			GROUP BY 
				LN10.BF_SSN
		)pop
	) BILL
		ON BILL.BF_SSN = LN10.BF_SSN
	LEFT JOIN /*GETS THE BORROWERS REPAMYENT SCHEDULE DATA*/
	(

		SELECT DISTINCT
			RS.BF_SSN,
			DC.DUE_DATE_COUNT,
			SUM(RS.LA_RPS_ISL) OVER(PARTITION BY RS.BF_SSN) AS LA_RPS_ISL,
			STUFF(
			(
				SELECT DISTINCT
					', ' + CASE WHEN LTRIM(RTRIM(DW01.WX_OVR_DW_LON_STA)) = 'LITIGATION' THEN '' ELSE CAST(DAY(R.LD_RPS_1_PAY_DU) AS VARCHAR(20)) END AS [text()]
				FROM
					calc.RepaymentSchedules R
					INNER JOIN DW01_DW_CLC_CLU DW01
						ON R.BF_SSN = DW01.BF_SSN
						AND R.LN_SEQ = DW01.LN_SEQ
				WHERE
					RS.BF_SSN = R.BF_SSN
					AND R.CurrentGradation = 1
			FOR XML PATH('')
			)
			,1,1,'')  AS DUE_DAY
		FROM	
			calc.RepaymentSchedules RS
			INNER JOIN 
			(
				SELECT
					BF_SSN,
					COUNT(DISTINCT DAY(LD_RPS_1_PAY_DU)) AS DUE_DATE_COUNT
				FROM
					UDW.calc.RepaymentSchedules
				WHERE
					CurrentGradation = 1
				GROUP BY
					BF_SSN
			) DC
				ON DC.BF_SSN = RS.BF_SSN
		WHERE
			RS.CurrentGradation = 1
	) RS
		ON RS.BF_SSN = LN10.BF_SSN
	LEFT JOIN
	(
		SELECT
			BF_SSN,
			COUNT(*) AS LOAN_COUNT
		FROM
			UDW..LN10_LON
		WHERE
			IC_LON_PGM = 'COMPLT'
			AND LC_STA_LON10 IN ('R','D')
		GROUP BY
			BF_SSN
	) COMPLT
		ON COMPLT.BF_SSN = LN10.BF_SSN
	LEFT JOIN
	(
	SELECT  
		DC.BorrSSN
	FROM 
		ULS.scra.DataComparison DC
		INNER JOIN ULS.scra.ScriptProcessing SP
			ON DC.DataComparisonId = SP.DataComparisonId
	WHERE 
		ActiveRow = 1 
		AND SP.LN10CurPri > 0
		AND (BorrActive = 1 or EndrActive = 1)
		AND @TODAY BETWEEN SP.TXCXBegin AND TXCXEnd
	)SCRA
		ON SCRA.BorrSSN = LN10.BF_SSN
	LEFT JOIN
	(
		SELECT
			MR64.BF_SSN,
			MR64.LF_TAX_YR AS Year1098,
			SUM(ISNULL(LA_BR_TOT_PD_YR,0)) AS Amount1098
		FROM 
			UDW..MR64_BR_TAX MR64
			INNER JOIN
			(
				SELECT
					BF_SSN,
					MAX(CAST(LF_TAX_YR AS int)) AS LF_TAX_YR
				FROM
					UDW..MR64_BR_TAX
				WHERE
					MR64_BR_TAX.LC_STA_MR64 = 'A'
				GROUP BY
					BF_SSN
			) MR64_MAX
				ON MR64_MAX.BF_SSN = MR64.BF_SSN
				AND MR64_MAX.LF_TAX_YR = MR64.LF_TAX_YR
		GROUP BY
			MR64.BF_SSN,
			MR64.LF_TAX_YR		
	) MR64
		ON MR64.BF_SSN = LN10.BF_SSN
	LEFT JOIN
	(
		SELECT
			MR65.BF_SSN,
			MR65.LF_TAX_YR AS Year1099,
			SUM(ISNULL(WA_PRI_INC_CRD,0)  + ISNULL(WA_INT_INC_CRD,0)) AS Amount1099
		FROM
			UDW..MR65_MSC_TAX_RPT MR65
			INNER JOIN
			(
				SELECT
					BF_SSN,
					MAX(CAST(LF_TAX_YR AS int)) AS LF_TAX_YR
				FROM
					UDW..MR65_MSC_TAX_RPT
				WHERE
					WC_STA_MR65 = 'A'
				GROUP BY
					BF_SSN
			) MR65_MAX
				ON MR65_MAX.BF_SSN = MR65.BF_SSN
				AND MR65_MAX.LF_TAX_YR = MR65.LF_TAX_YR
		GROUP BY 
			MR65.BF_SSN,
			MR65.LF_TAX_YR 
	) MR65
		ON MR65.BF_SSN = LN10.BF_SSN
	LEFT JOIN LK10_LS_CDE_LKP LK10
		ON LK10.PM_ATR = 'DC-DOM-ST'
		AND LK10.PX_ATR_VAL = PD30.DC_DOM_ST;


	-- wrap in trans
	BEGIN TRANSACTION;
	TRUNCATE TABLE UDW.calc.IvrData;

	INSERT INTO UDW.calc.IvrData(SSN,AccountNumber,DateOfBirth,IVRDateOfBirth,ZipIvr,[Name],MultiDueDate,Bankruptcy,DaysPastDue,IsCompleteLoan,HasSpousalLoans,Military,Principal,Interest,Fees,TotalBalance,PastDue,CurrentAmoutDue,TotalDue,DueDate,MonthlyPaymentAmount,LastPaymentDate,IVRLastPaymentDate,LastPaymentAmount,TaxYear1098,TaxAmount1098,TaxYear1099,TaxAmount1099,PrimaryPhoneNumber,PrimaryPhoneValidity,PrimaryPhoneConsent,SecondPhoneNumber,SecondPhoneValidity,SecondPhoneConsent,ThirdPhoneNumber,ThirdPhoneValidity,ThirdPhoneConsent,FourthPhoneNumber,FourthPhoneValidity,FourthPhoneConsent,LegalAddress1,LegalAddress2,LegalAddressCity,LegalAddressState,LegalAddressZipCode,LegalAddressCountry,LegalAddressValidity,MultiDueDateCount,AddressLastVerified,PhoneLastVerified)
	SELECT 
		SSN,
		AccountNumber,
		DateOfBirth,
		IVRDateOfBirth,
		ZipIvr,
		[Name],
		MultiDueDate,
		Bankruptcy,
		DaysPastDue,
		IsCompleteLoan,
		HasSpousalLoans,
		Military,
		Principal,
		Interest,
		Fees,
		TotalBalance,
		PastDue,
		CurrentAmoutDue,
		TotalDue,
		DueDate,
		MonthlyPaymentAmount,
		LastPaymentDate,
		IVRLastPaymentDate,
		LastPaymentAmount,
		TaxYear1098,
		TaxAmount1098,
		TaxYear1099,
		TaxAmount1099,
		PrimaryPhoneNumber,
		PrimaryPhoneValidity,
		PrimaryPhoneConsent,
		SecondPhoneNumber,
		SecondPhoneValidity,
		SecondPhoneConsent,
		ThirdPhoneNumber,
		ThirdPhoneValidity,
		ThirdPhoneConsent,
		FourthPhoneNumber,
		FourthPhoneValidity,
		FourthPhoneConsent,
		LegalAddress1,
		LegalAddress2,
		LegalAddressCity,
		LegalAddressState,
		LegalAddressZipCode,
		LegalAddressCountry,
		LegalAddressValidity,
		MultiDueDateCount,
		AddressLastVerified,
		PhoneLastVerified
	FROM 
		UDW.calc.IvrDataStaging;

	COMMIT;