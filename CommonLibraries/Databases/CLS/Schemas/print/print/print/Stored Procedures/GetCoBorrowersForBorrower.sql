﻿CREATE PROCEDURE [schrpt].[GetBorrowersForSchool]
(
	@School VARCHAR(8)
)
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT
	LN10.BF_SSN AS [Borrower SSN],
	CONVERT(VARCHAR,PD10.DD_BRT,110) AS [Borrower Birth Date],
	PD10.DM_PRS_1 AS [Borrower First Name],
	PD10.DM_PRS_LST AS [Borrower Last Name],
	PD10.DM_PRS_MID AS [Borrower Middle Name],
	'"' + PD30.DX_STR_ADR_1 + '"' AS [Borrower Address Line 1],
	'"' + PD30.DX_STR_ADR_2 + '"' AS [Borrower Address Line 2],
	'"' + PD30.DX_STR_ADR_3 + '"' AS [Borrower Address Line 3],
	'"' + PD30.DM_CT + '"' AS [Borrower City],
	'"' + PD30.DC_DOM_ST + '"' AS [Borrower State],
	PD30.DF_ZIP_CDE AS [Borrower ZIP],
	PD30.DM_FGN_CNY AS [Borrower Country],
	PD30.DI_VLD_ADR AS [Borrower Address Validity Indicator],
	CONVERT(VARCHAR,PD30.DD_VER_ADR,110) AS [Borrower Address Validity Date],
	PD32.DX_ADR_EML AS [Borrower Email Address],
	PD32.DI_VLD_ADR_EML AS [Email Validity Indicator],
	CONVERT(VARCHAR,PD32.DD_VER_ADR_EML,110) AS [Email Validity Date],
	PD40.[Borrower Phone 1],
	PD40.[Borrower Phone Number Extension 1],
	PD40.[Borrower International Phone Prefix Code 1],
	PD40.[Borrower Phone Type 1],
	PD40.[Borrower Phone 1 Validity],
	PD40.[Borrower Phone 2],
	PD40.[Borrower Phone Number Extension 2],
	PD40.[Borrower International Phone Prefix Code 2],
	PD40.[Borrower Phone Type 2],
	PD40.[Borrower Phone 2 Validity],
	PD40.[Borrower Phone 3],
	PD40.[Borrower Phone Number Extension 3],
	PD40.[Borrower International Phone Prefix Code 3],
	PD40.[Borrower Phone Type 3],
	PD40.[Borrower Phone 3 Validity],
	PD40.[Borrower Phone 4],
	PD40.[Borrower Phone Number Extension 4],
	PD40.[Borrower International Phone Prefix Code 4],
	PD40.[Borrower Phone Type 4],
	PD40.[Borrower Phone 4 Validity],
	CONVERT(VARCHAR,WB24.DF_CRT_DTS_WB24,110) AS [Website Access Date],
	PD11.DM_PRS_1_HST AS [Borrower Previous First Name],
	PD11.DM_PRS_LST_HST AS [Borrower Previous Last Name],
	NULL AS [New Borrower Indicator], 
	FS10.LF_FED_AWD + RIGHT('00' + CAST(FS10.LN_FED_AWD_SEQ AS VARCHAR),2) AS [Award ID],
	LN10.LF_LON_ALT + RIGHT('00' + CAST(LN10.LN_LON_ALT_SEQ AS VARCHAR),2) AS [CommonLine Unique ID and Sequence Number], 
	PD10.DF_SPE_ACC_ID AS [Service Provider Borrower Account Number],
	NULL AS [Guarantor Loan ID Number],
	CASE WHEN DW01.WC_DW_LON_STA = '02' THEN CONVERT(VARCHAR,LN10.LD_LON_1_DSB,110)
		 WHEN DW01.WC_DW_LON_STA = '04' AND InSchoolDefer.BF_SSN IS NOT NULL THEN CONVERT(VARCHAR,InSchoolDefer.LD_DFR_BEG,110)
		 ELSE CONVERT(VARCHAR,SD10.LD_SCL_SPR,110)
	END AS [Enrollment Status Effective Date],
	CASE WHEN DW01.WC_DW_LON_STA = '02' THEN 'F'
		 WHEN DW01.WC_DW_LON_STA = '04' AND InSchoolDefer.BF_SSN IS NOT NULL AND InSchoolDefer.LC_DFR_TYP IN ('15','19','23') THEN 'F'
		 WHEN DW01.WC_DW_LON_STA = '04' AND InSchoolDefer.BF_SSN IS NOT NULL AND InSchoolDefer.LC_DFR_TYP IN ('18','22') THEN 'H'
		 ELSE SD10.CalculatedType
	END AS [Enrollment Status],
	CASE WHEN LN10.LC_ACA_GDE_LEV IN ('01','1') THEN '1'
	     WHEN LN10.LC_ACA_GDE_LEV IN ('02','2') THEN '2'
		 WHEN LN10.LC_ACA_GDE_LEV IN ('03','3') THEN '3'
		 WHEN LN10.LC_ACA_GDE_LEV IN ('04','4') THEN '4'
		 WHEN LN10.LC_ACA_GDE_LEV IN ('05','5') THEN '5'
		 WHEN LN10.LC_ACA_GDE_LEV IN ('A','06','6') THEN 'A'
		 WHEN LN10.LC_ACA_GDE_LEV IN ('B','07','7') THEN 'B'
		 WHEN LN10.LC_ACA_GDE_LEV IN ('C','08','8') THEN 'C'
		 WHEN LN10.LC_ACA_GDE_LEV IN ('D','09','9','10','11','12','13','14','15') THEN 'D'
		 WHEN LN10.LC_ACA_GDE_LEV IN ('N') THEN 'R'
		 ELSE '' 
	END AS [Grade Level],
	CASE WHEN DW01.WC_DW_LON_STA = '04' AND InSchoolDefer.BF_SSN IS NOT NULL THEN CONVERT(VARCHAR,InSchoolDefer.LD_DFR_END,110)
	     WHEN InSchoolDefer.BF_SSN IS NOT NULL THEN CONVERT(VARCHAR,SD10.LD_SCL_SPR,110) 
		 ELSE CONVERT(VARCHAR,LN10.LD_LON_1_DSB,110)
	END AS [Separation Date],
	CASE WHEN DW01.WC_DW_LON_STA = '04' AND InSchoolDefer.BF_SSN IS NOT NULL THEN CONVERT(VARCHAR,DATEADD(DAY, 1, InSchoolDefer.LD_DFR_END),110)
		 WHEN InSchoolDefer.BF_SSN IS NOT NULL THEN CONVERT(VARCHAR,COALESCE(DATEADD(DAY, 1, LN10.LD_END_GRC_PRD),LN10.LD_LON_1_DSB),110)
		 ELSE CONVERT(VARCHAR,RS.LD_RPS_1_PAY_DU,110)
	END AS [Date Entered Repayment],
	LEFT(LN10.LF_FED_CLC_RSK,4) AS [Cohort Year],
	LN10.LF_DOE_SCL_ORG AS [School Code + School Branch],
	SC10.IM_SCL_FUL AS [School Name],
	LN10.IF_DOE_LDR AS [Current Lender Code],
	NULL AS [Original Lender Code],
	'000502' AS [Current Loan Servicer Code],
	NULL AS [Original GA/ED Servicer Code],
	'000502' AS [Guarantor ID],
	CASE WHEN LN10.IC_LON_PGM IN ('DLSTFD','DLSCT') THEN 'D1'
	     WHEN LN10.IC_LON_PGM IN ('DLUNST','DLSCUN') THEN 'D2'
		 WHEN LN10.IC_LON_PGM IN ('DLSCPG') THEN 'D3'
		 WHEN LN10.IC_LON_PGM IN ('DLPLUS','DLSCPL') THEN 'D4'
		 WHEN LN10.IC_LON_PGM IN ('DLUCNS','DLSCUC','DLSCCN','CNSLDN') THEN 'D5'
		 WHEN LN10.IC_LON_PGM IN ('DLSCNS','DLSCSC') THEN 'D6'
		 WHEN LN10.IC_LON_PGM IN ('DLPCNS') THEN 'D7'
		 WHEN LN10.IC_LON_PGM IN ('TEACH') THEN 'D8'
		 WHEN LN10.IC_LON_PGM IN ('FISL') THEN 'FI'
		 WHEN LN10.IC_LON_PGM IN ('SLS') THEN 'SL'
		 ELSE ''
	END AS [Loan Type],
	NULL AS [Consolidation Indicator],
	NULL AS [Consolidation Loan Identifier],
	NULL AS [Indicator of Separate Loan],
	CONVERT(VARCHAR,LN10.LD_LON_GTR,110) AS [Loan Date],
	CONVERT(VARCHAR,LN10.LD_TRM_BEG,110) AS [Loan Period Begin Date],
	CONVERT(VARCHAR,LN10.LD_TRM_END,110) AS [Loan Period End Date],
	CASE WHEN DW01.WC_DW_LON_STA = '01' THEN 'IG'
		 WHEN DW01.WC_DW_LON_STA = '02' THEN 'ID'
		 WHEN DW01.WC_DW_LON_STA = '03' THEN 'RP'
		 WHEN DW01.WC_DW_LON_STA = '04' THEN 'DA'
		 WHEN DW01.WC_DW_LON_STA = '05' THEN 'FB'
		 WHEN DW01.WC_DW_LON_STA IN('07','08','13','14') THEN 'DF'
		 WHEN DW01.WC_DW_LON_STA IN('16','17') THEN 'DE'
		 WHEN DW01.WC_DW_LON_STA IN('18','19') THEN 'DI'
		 WHEN DW01.WC_DW_LON_STA IN('20','21') THEN 'BK'
		 ELSE 'RP'
	END AS [Loan Status],
	CASE WHEN DW01.WC_DW_LON_STA = '01' THEN CONVERT(VARCHAR,SD10.LD_SCL_SPR,110)
		 WHEN DW01.WC_DW_LON_STA = '02' THEN CONVERT(VARCHAR,LN10.LD_LON_1_DSB,110)
		 WHEN DW01.WC_DW_LON_STA = '03' THEN CONVERT(VARCHAR,DW01.WD_LON_RPD_SR,110)
		 WHEN DW01.WC_DW_LON_STA = '04' AND MaxLN50.BF_SSN IS NOT NULL THEN CONVERT(VARCHAR,MaxLN50.MaxBegin,110)
		 WHEN DW01.WC_DW_LON_STA = '05' AND ActiveForb.BF_SSN IS NOT NULL THEN CONVERT(VARCHAR,ActiveForb.LD_FOR_BEG,110)
		 WHEN DW01.WC_DW_LON_STA = '16' THEN CONVERT(VARCHAR,PD21.DD_DTH_STA,110)
		 WHEN DW01.WC_DW_LON_STA = '17' THEN CONVERT(VARCHAR,PD20.DD_DTH_NTF,110)
		 WHEN DW01.WC_DW_LON_STA = '18' AND PD23.DC_DSA_STA = '07' THEN CONVERT(VARCHAR,PD22.DD_PRS_DSA_SPS_SR,110)
		 WHEN DW01.WC_DW_LON_STA = '19' AND PD23.DC_DSA_STA = '09' THEN CONVERT(VARCHAR,PD22.DD_DSA_RPT,110)
		 WHEN DW01.WC_DW_LON_STA = '20' AND PD24.DC_BKR_STA = 4 THEN CONVERT(VARCHAR,PD24.DD_BKR_FIL,110)
		 WHEN DW01.WC_DW_LON_STA = '21' AND PD24.DC_BKR_STA = 6 THEN CONVERT(VARCHAR,PD24.DD_BKR_VER,110)
		 ELSE CONVERT(VARCHAR,LN10.LD_LON_1_DSB,110) -- confirmed 6 - 15 do not apply in this as well as 22 +.  Catch them in the else using LN10.LD_LON_1_DSB
	END AS [Loan Status Effective Date], 
	NULL AS [Loan Status Expiration Date],
	NULL AS [Previous Loan Status],
	CASE WHEN DW01.WC_DW_LON_STA = '17' THEN 'VDC'
		 WHEN DW01.WC_DW_LON_STA = '19' THEN 'VTP'
		 WHEN DW01.WC_DW_LON_STA = '21' THEN 'VB'
		 ELSE ''
	END AS [Hold Reason],
	LN10.LA_LON_AMT_GTR AS [Loan Amount], --Previously CLS.[schrpt].FormatMoney() 
	CONVERT(VARCHAR,LN10.LD_LON_1_DSB,110) AS [First Disbursement Date],
	LN15.LA_DSB AS [Total Original Disbursement Amount], --Previously CLS.[schrpt].FormatMoney() 
	LN15.LA_DSB_CAN_RFD AS [Total Loan Reduction Amount],--Previously CLS.[schrpt].FormatMoney() 
	LN72.LR_ITR AS [Interest Rate], 
	CASE WHEN LN72.LC_ITR_TYP = 'F1' THEN 'F'
	     WHEN LN72.LC_ITR_TYP IN('RN','RY','SV') THEN 'V'
		 ELSE '' 
	END AS [Interest Rate Code], 
	CASE WHEN DW01.WC_DW_LON_STA != '03' THEN 'NR'
		 WHEN RS.LC_TYP_SCH_DIS = 'FS' THEN 'J2'
		 WHEN RS.LC_TYP_SCH_DIS IN('EL','PL') THEN 'FE'
		 WHEN RS.LC_TYP_SCH_DIS IN('G','PG') THEN 'GR'
		 WHEN RS.LC_TYP_SCH_DIS = 'IP' THEN 'IL'
		 WHEN RS.LC_TYP_SCH_DIS = 'CA' THEN 'PA'
		 WHEN RS.LC_TYP_SCH_DIS = 'CP' THEN 'P1'
		 WHEN RS.LC_TYP_SCH_DIS = 'FG' THEN 'J3'
		 WHEN RS.LC_TYP_SCH_DIS IN('S2','S5') THEN 'CG'
		 ELSE RS.LC_TYP_SCH_DIS
	END AS [Repayment Plan Type],
	RS.LN_RPS_TRM AS [Current Repayment Plan Term],
	CONVERT(VARCHAR,RS.LD_RPS_1_PAY_DU,110) AS [Current Repayment Plan Begin Date],
	NULL AS [Repayment Term Remaining],
	RS.LA_RPS_ISL AS [Monthly Payment Amount],
	CONVERT(VARCHAR,LN90Dates.Min_LD_FAT_EFF,110) AS [First Payment Date], 
	LN80.LA_BIL_CUR_DU AS [Next Payment Amount],
	CONVERT(VARCHAR,LN80.LD_BIL_DU_LON,110) AS [Next Payment Due Date],
	LN90Amounts.PaymentAmount AS [Last Borrower Payment Amount],
	CONVERT(VARCHAR,LN90Dates.Max_LD_FAT_EFF,110) AS [Last Borrower Payment Date],
	CASE WHEN LN83.BF_SSN IS NOT NULL THEN 'Y' ELSE 'N' END AS [Auto Debit Status],
	NULL AS [Loan Balance as of Repayment Start Date],
	LN10.LA_CUR_PRI AS [Outstanding Principal Balance], --Previously CLS.[schrpt].FormatMoney() 
	DW01.WA_TOT_BRI_OTS AS [Outstanding Interest Balance], --Previously CLS.[schrpt].FormatMoney() 
	NULL AS [Capitalized Interest Amount],
	NULL AS [Fees Amount],
	CONVERT(VARCHAR,GETDATE(),110) AS [Balance Date],
	CONVERT(VARCHAR,LN16.Min_LD_DLQ_OCC,110) AS [First Delinquency Date],
	CONVERT(VARCHAR,LN16.Current_LD_DLQ_OCC,110) AS [Current Delinquency Date],
	LN16.Current_LN_DLQ_MAX + 1 AS [Days Delinquent/Past Due],
	LN80.LA_BIL_PAS_DU AS [Amount Delinquent], --Previously CLS.[schrpt].FormatMoney() 
	CONVERT(VARCHAR,LN16.ResolutionDate,110) AS [Delinquency Resolution Date],
	CASE WHEN MaxLN60.MaxBegin >= MaxLN50.MaxBegin THEN MaxLN60.MaxBegin
		 WHEN MaxLN50.MaxBegin > MaxLN60.MaxBegin THEN MaxLN50.MaxBegin
		 ELSE NULL 
	END AS [Most Recent Deferment/Forbearance Start Date], 
	CASE WHEN MaxLN60.MaxEnd >= MaxLN50.MaxEnd THEN MaxLN60.MaxEnd
		 WHEN MaxLN50.MaxEnd > MaxLN60.MaxEnd THEN MaxLN50.MaxEnd
		 ELSE NULL 
	END AS [Most Recent Deferment/Forbearance End Date], 
	CASE WHEN MaxLN60.MaxBegin >= MaxLN50.MaxBegin THEN 
			CASE WHEN FB10.LC_FOR_TYP IN('02','03','06','09','10','11','14','15','16','17','25','26','28','32','34','35','36','37','38','40','41','43') THEN 'AD'
			     WHEN FB10.LC_FOR_TYP IN('04','12','13','21','27','33','39','42') THEN 'MA'
				 WHEN FB10.LC_FOR_TYP IN('01','07','18','20','22','44') THEN 'MN'
				 WHEN FB10.LC_FOR_TYP IN('05','24','29','31') THEN 'DC'
				 ELSE ''
			END
		 WHEN MaxLN50.MaxBegin > MaxLN60.MaxBegin THEN 
			CASE WHEN DF10.LC_DFR_TYP = '01' THEN 'NO'
			     WHEN DF10.LC_DFR_TYP = '02' THEN 'TD'
				 WHEN DF10.LC_DFR_TYP IN('03','20') THEN 'RT'
				 WHEN DF10.LC_DFR_TYP = '04' THEN 'AP'
				 WHEN DF10.LC_DFR_TYP = '05' THEN 'AC'
				 WHEN DF10.LC_DFR_TYP IN('06','21') THEN 'GF'
				 WHEN DF10.LC_DFR_TYP IN('07','27','34') THEN 'PC'
				 WHEN DF10.LC_DFR_TYP IN('08','09','28') THEN 'IR'
				 WHEN DF10.LC_DFR_TYP = '10' THEN 'TE'
				 WHEN DF10.LC_DFR_TYP = '11' THEN 'WM'
				 WHEN DF10.LC_DFR_TYP = '12' THEN 'PL'
				 WHEN DF10.LC_DFR_TYP = '13' THEN 'UN'
				 WHEN DF10.LC_DFR_TYP = '14' THEN 'TS'
				 WHEN DF10.LC_DFR_TYP IN('15','16') THEN 'FT'
				 WHEN DF10.LC_DFR_TYP IN('17','38','40') THEN 'MO'
				 WHEN DF10.LC_DFR_TYP IN('18','22') THEN 'HT'
				 WHEN DF10.LC_DFR_TYP = '19' THEN 'PP'
				 WHEN DF10.LC_DFR_TYP IN('29','41','42','43') THEN 'EH'
				 WHEN DF10.LC_DFR_TYP = '34' THEN 'PC'
				 WHEN DF10.LC_DFR_TYP = '45' THEN 'PD'
				 WHEN DF10.LC_DFR_TYP = '46' THEN 'PE'
				 ELSE ''
			END
		 ELSE NULL
	END AS [Most Recent Deferment/Forbearance Type], 
	NULL AS [Claim Filed Date], 
	NULL AS [Claim Paid Date], 
	NULL AS [Claim Type], 
	NULL AS [Claim Discharge Amount],
	CONVERT(VARCHAR,DLFDL.LD_ATY_REQ_RCV,110) AS [Date of Technical Default of Loan Status],
	CONVERT(VARCHAR,CDR.LD_FAT_APL,110) AS [CDR Date of Default], 
	CASE WHEN LN09.LD_LON_RHB_PCV IS NOT NULL THEN 'Y' ELSE 'N' END AS [Rehabiliation/Repurchased Indicator],
	CONVERT(VARCHAR,LN09.LD_LON_RHB_PCV,110) AS [Rehabilitation/Repurchased Date],
	MaxLN50.[Cumulative Economic Hardship Time Used],
	MaxLN50.[Cumulative Graduate Fellowship Time Used],
	MaxLN50.[Cumulative Internship Residency Time Used],
	MaxLN50.[Cumulative NOAA Deferment Time Used],
	MaxLN50.[Cumulative Parent PLUS Six Month Post Enrollment Time Used],
	MaxLN50.[Cumulative Parental Leave Deferment Time Used],
	MaxLN50.[Cumulative Peace Corps Deferment Time Used],
	MaxLN50.[Cumulative Public Health Service Deferment Time Used],
	MaxLN50.[Cumulative Rehabilitation Training Deferment Time Used],
	MaxLN50.[Cumulative Tax Exempt Organization Deferment Time Used],
	MaxLN50.[Cumulative Teacher Shortage Area Deferment Time Used],
	MaxLN50.[Cumulative Temporary Disability Deferment Time Used],
	MaxLN50.[Cumulative Unemployment Deferment Time Used],
	MaxLN50.[Cumulative Working Mother Deferment Time Used],
	RF10.[Reference 1 First Name] AS [Reference 1 First Name],
	RF10.[Reference 1 Last Name] AS [Reference 1 Last Name],
	RF10.[Reference 1 Relation] AS [Reference 1 Relationship Code],
	RF10.[Reference 1 Phone] AS [Reference 1 Phone],
	RF10.[Reference 1 Phone Number Extension] AS [Reference 1 Phone Number Extension],
	RF10.[Reference 1 International Prefix Code] AS [Reference 1 International Phone Prefix Code],
	RF10.[Reference 2 First Name] AS [Reference 2 First Name],
	RF10.[Reference 2 Last Name] AS [Reference 2 Last Name],
	RF10.[Reference 2 Relation] AS [Reference 2 Relationship Code],
	RF10.[Reference 2 Phone] AS [Reference 2 Phone],
	RF10.[Reference 2 Phone Number Extension] AS [Reference 2 Phone Number Extension],
	RF10.[Reference 2 International Prefix Code] AS [Reference 2 International Phone Prefix Code],
	LN10.LF_STU_SSN AS [Student SSN],
	StudentDemos.[Student First Name] AS [Student First Name],
	StudentDemos.[Student Last Name] AS [Student Last Name],
	StudentDemos.[Student Middle Name] AS [Student Middle Name],
	StudentDemos.[Student DOB] AS [Student DOB],
	StudentDemos.[Student Address Line 1] AS [Student Address Line 1],
	StudentDemos.[Student Address Line 2] AS [Student Address Line 2],
	StudentDemos.[Student City] AS [Student City],
	StudentDemos.[Student State] AS [Student State],
	StudentDemos.[Student ZIP] AS [Student ZIP],
	'CornerStone Education Loan Services' AS [Servicer Name],
	'8006631662' AS [Servicer Phone Number],
	'www.mycornerstoneloan.org' AS [Servicer Website],
	PD40.[Borrower Phone 1 Validity Date],
	PD40.[Borrower Phone 2 Validity Date],
	PD40.[Borrower Phone 3 Validity Date],
	PD40.[Borrower Phone 4 Validity Date],
	MaxLN60.[Forbearance Time Available],
	'000502' AS [Source ID],
	CONVERT(VARCHAR,GETDATE(),110) AS [File Create Date]
FROM
	CDW..PD10_PRS_NME PD10
	INNER JOIN CDW..LN10_LON LN10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
	LEFT JOIN CDW..DW01_DW_CLC_CLU DW01
		ON DW01.BF_SSN = LN10.BF_SSN
		AND DW01.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN CDW..FS10_DL_LON FS10
		ON FS10.BF_SSN = LN10.BF_SSN
		AND FS10.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN CDW..SC10_SCH_DMO SC10
		ON SC10.IF_DOE_SCL = LN10.LF_DOE_SCL_ORG
	LEFT JOIN 
	(
		SELECT
			LN15.BF_SSN,
			LN15.LN_SEQ,
			SUM(COALESCE(LN15.LA_DSB,0.00)) AS LA_DSB, --JW 10/2
			SUM(COALESCE(LN15.LA_DSB_CAN_RFD,0.00)) AS LA_DSB_CAN_RFD --JW 10/2
		FROM
			CDW..LN15_DSB LN15
			INNER JOIN CDW..LN10_LON LN10	
				ON LN10.BF_SSN = LN15.BF_SSN
				AND LN10.LN_SEQ = LN15.LN_SEQ
		WHERE
			LN10.LF_DOE_SCL_ORG = @School
			AND LN15.LC_STA_LON15 = 'A' --JW 10/2
		GROUP BY
			LN15.BF_SSN,
			LN15.LN_SEQ
	) LN15
		ON LN15.BF_SSN = LN10.BF_SSN
		AND LN15.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN CDW..PD20_PRS_DTH PD20 
		ON PD20.DF_PRS_ID = PD10.DF_PRS_ID
	LEFT JOIN CDW..PD21_GTR_DTH PD21 
		ON PD21.DF_PRS_ID = PD10.DF_PRS_ID
		--JW 10/2 Death status?
	LEFT JOIN CDW..PD22_PRS_DSA PD22 
		ON PD22.DF_PRS_ID = PD10.DF_PRS_ID
	LEFT JOIN CDW..PD23_GTR_DSA PD23 
		ON PD23.DF_PRS_ID = PD10.DF_PRS_ID
		--JW 10/2 needs review for status?
	LEFT JOIN CDW..PD24_PRS_BKR PD24
		ON PD24.DF_PRS_ID = PD10.DF_PRS_ID
	LEFT JOIN CDW..WB24_CSM_USR_ACC WB24 
		ON WB24.DF_USR_SSN = PD10.DF_PRS_ID
	LEFT JOIN 
	(
		SELECT
			PD30.DF_PRS_ID,
			PD30.DX_STR_ADR_1,
			PD30.DX_STR_ADR_2,
			PD30.DX_STR_ADR_3,
			PD30.DM_CT,
			PD30.DC_DOM_ST,
			PD30.DF_ZIP_CDE,
			PD30.DM_FGN_CNY,
			PD30.DI_VLD_ADR,
			PD30.DD_VER_ADR
		FROM
			CDW..PD30_PRS_ADR PD30
			INNER JOIN
			(
				SELECT DISTINCT
					PD30m.DF_PRS_ID,
					PD30m.DC_ADR,
					MAX(PD30m.DF_LST_DTS_PD30) AS MostRecent
				FROM
					CDW..PD30_PRS_ADR PD30m
					INNER JOIN CDW..LN10_LON LN10
						ON LN10.BF_SSN = PD30m.DF_PRS_ID
				WHERE
					PD30m.DC_ADR = 'L'
					AND LN10.LF_DOE_SCL_ORG = @School
				GROUP BY
					PD30m.DF_PRS_ID,
					PD30m.DC_ADR
			) MaxLegal
				ON MaxLegal.DF_PRS_ID = PD30.DF_PRS_ID
				AND MaxLegal.DC_ADR = PD30.DC_ADR
				AND MaxLegal.MostRecent = PD30.DF_LST_DTS_PD30
	) PD30 
		ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
	LEFT JOIN
	(
		SELECT
			PD32.DF_PRS_ID,
			PD32.DX_ADR_EML,
			PD32.DI_VLD_ADR_EML,
			PD32.DD_VER_ADR_EML
		FROM
			CDW..PD32_PRS_ADR_EML PD32
			INNER JOIN
			(
				SELECT DISTINCT
					PD32m.DF_PRS_ID,
					PD32m.DC_ADR_EML,
					MAX(PD32m.DF_LST_DTS_PD32) AS MostRecent
				FROM
					CDW..PD32_PRS_ADR_EML PD32m
					INNER JOIN CDW..LN10_LON LN10
						ON LN10.BF_SSN = PD32m.DF_PRS_ID
				WHERE
					PD32m.DC_ADR_EML = 'H'
					AND LN10.LF_DOE_SCL_ORG = @School
				GROUP BY
					PD32m.DF_PRS_ID,
					PD32m.DC_ADR_EML
			) MaxHomeEmail
				ON MaxHomeEmail.DF_PRS_ID = PD32.DF_PRS_ID
				AND MaxHomeEmail.DC_ADR_EML = PD32.DC_ADR_EML
				AND MaxHomeEmail.MostRecent = PD32.DF_LST_DTS_PD32
	) PD32 
		ON PD32.DF_PRS_ID = PD10.DF_PRS_ID
	LEFT JOIN
	( 
		SELECT
			PD40.DF_PRS_ID,
			MAX(CASE WHEN PD40.DC_PHN = 'H' THEN COALESCE(PD40.DN_DOM_PHN_ARA + PD40.DN_DOM_PHN_XCH + PD40.DN_DOM_PHN_LCL, PD40.DN_FGN_PHN_LCL) ELSE NULL END) AS [Borrower Phone 1],
			MAX(CASE WHEN PD40.DC_PHN = 'H' THEN PD40.DN_PHN_XTN ELSE NULL END) AS [Borrower Phone Number Extension 1],
			MAX(CASE WHEN PD40.DC_PHN = 'H' THEN PD40.DN_FGN_PHN_INL + PD40.DN_FGN_PHN_CNY + PD40.DN_FGN_PHN_CT ELSE NULL END) AS [Borrower International Phone Prefix Code 1],
			MAX(CASE WHEN PD40.DC_PHN = 'H' THEN PD40.DC_PHN ELSE NULL END) AS [Borrower Phone Type 1],
			MAX(CASE WHEN PD40.DC_PHN = 'H' THEN PD40.DI_PHN_VLD ELSE NULL END) AS [Borrower Phone 1 Validity],
			MAX(CASE WHEN PD40.DC_PHN = 'H' THEN CONVERT(VARCHAR,PD40.DD_PHN_VER,110) ELSE NULL END) AS [Borrower Phone 1 Validity Date],
			MAX(CASE WHEN PD40.DC_PHN = 'W' THEN COALESCE(PD40.DN_DOM_PHN_ARA + PD40.DN_DOM_PHN_XCH + PD40.DN_DOM_PHN_LCL, PD40.DN_FGN_PHN_LCL) ELSE NULL END) AS [Borrower Phone 2],
			MAX(CASE WHEN PD40.DC_PHN = 'W' THEN PD40.DN_PHN_XTN ELSE NULL END) AS [Borrower Phone Number Extension 2],
			MAX(CASE WHEN PD40.DC_PHN = 'W' THEN PD40.DN_FGN_PHN_INL + PD40.DN_FGN_PHN_CNY + PD40.DN_FGN_PHN_CT ELSE NULL END) AS [Borrower International Phone Prefix Code 2],
			MAX(CASE WHEN PD40.DC_PHN = 'W' THEN PD40.DC_PHN ELSE NULL END) AS [Borrower Phone Type 2],
			MAX(CASE WHEN PD40.DC_PHN = 'W' THEN PD40.DI_PHN_VLD ELSE NULL END) AS [Borrower Phone 2 Validity],
			MAX(CASE WHEN PD40.DC_PHN = 'W' THEN CONVERT(VARCHAR,PD40.DD_PHN_VER,110) ELSE NULL END) AS [Borrower Phone 2 Validity Date],
			MAX(CASE WHEN PD40.DC_PHN = 'A' THEN COALESCE(PD40.DN_DOM_PHN_ARA + PD40.DN_DOM_PHN_XCH + PD40.DN_DOM_PHN_LCL, PD40.DN_FGN_PHN_LCL) ELSE NULL END) AS [Borrower Phone 3],
			MAX(CASE WHEN PD40.DC_PHN = 'A' THEN PD40.DN_PHN_XTN ELSE NULL END) AS [Borrower Phone Number Extension 3],
			MAX(CASE WHEN PD40.DC_PHN = 'A' THEN PD40.DN_FGN_PHN_INL + PD40.DN_FGN_PHN_CNY + PD40.DN_FGN_PHN_CT ELSE NULL END) AS [Borrower International Phone Prefix Code 3],
			MAX(CASE WHEN PD40.DC_PHN = 'A' THEN 'O' ELSE NULL END) AS [Borrower Phone Type 3],
			MAX(CASE WHEN PD40.DC_PHN = 'A' THEN PD40.DI_PHN_VLD ELSE NULL END) AS [Borrower Phone 3 Validity],
			MAX(CASE WHEN PD40.DC_PHN = 'A' THEN CONVERT(VARCHAR,PD40.DD_PHN_VER,110) ELSE NULL END) AS [Borrower Phone 3 Validity Date],
			MAX(CASE WHEN PD40.DC_PHN = 'M' THEN COALESCE(PD40.DN_DOM_PHN_ARA + PD40.DN_DOM_PHN_XCH + PD40.DN_DOM_PHN_LCL, PD40.DN_FGN_PHN_LCL) ELSE NULL END) AS [Borrower Phone 4],
			MAX(CASE WHEN PD40.DC_PHN = 'M' THEN PD40.DN_PHN_XTN ELSE NULL END) AS [Borrower Phone Number Extension 4],
			MAX(CASE WHEN PD40.DC_PHN = 'M' THEN PD40.DN_FGN_PHN_INL + PD40.DN_FGN_PHN_CNY + PD40.DN_FGN_PHN_CT ELSE NULL END) AS [Borrower International Phone Prefix Code 4],
			MAX(CASE WHEN PD40.DC_PHN = 'M' THEN 'C' ELSE NULL END) AS [Borrower Phone Type 4],
			MAX(CASE WHEN PD40.DC_PHN = 'M' THEN PD40.DI_PHN_VLD ELSE NULL END) AS [Borrower Phone 4 Validity],
			MAX(CASE WHEN PD40.DC_PHN = 'M' THEN CONVERT(VARCHAR,PD40.DD_PHN_VER,110) ELSE NULL END) AS [Borrower Phone 4 Validity Date]
		FROM
			CDW..PD40_PRS_PHN PD40
			INNER JOIN
			( 
				SELECT DISTINCT
					PD40m.DF_PRS_ID,
					PD40m.DC_PHN,
					MAX(PD40m.DF_LST_DTS_PD40) OVER(PARTITION BY PD40m.DF_PRS_ID, PD40m.DC_PHN) AS MaxDateByType
				FROM
					CDW..PD40_PRS_PHN PD40m
					INNER JOIN CDW..LN10_LON LN10
						ON LN10.BF_SSN = PD40m.DF_PRS_ID
				WHERE
					LN10.LF_DOE_SCL_ORG = @School
			) MaxPhoneByType
				ON MaxPhoneByType.DF_PRS_ID = PD40.DF_PRS_ID
				AND MaxPhoneByType.DC_PHN = PD40.DC_PHN
				AND MaxPhoneByType.MaxDateByType = PD40.DF_LST_DTS_PD40
		GROUP BY
			PD40.DF_PRS_ID
	) PD40
		ON PD40.DF_PRS_ID = PD10.DF_PRS_ID
	LEFT JOIN 
	(
		SELECT
			DF_PRS_ID,
			DM_PRS_1_HST,
			DM_PRS_LST_HST,
			MAX(DF_LST_DTS_PD11) AS DF_LST_DTS_PD11
		FROM
			CDW..PD11_PRS_NME_HST
		GROUP BY
			DF_PRS_ID,
			DM_PRS_1_HST,
			DM_PRS_LST_HST
	) PD11
		ON PD11.DF_PRS_ID = PD10.DF_PRS_ID
	LEFT JOIN 
	(
		SELECT
			LF_STU_SSN,
			CASE WHEN LC_REA_SCL_SPR = '01' THEN 'G'
				 WHEN LC_REA_SCL_SPR = '02' THEN 'W'
				 WHEN LC_REA_SCL_SPR = '05' THEN 'W'
				 WHEN LC_REA_SCL_SPR = '06' THEN 'W'
				 WHEN LC_REA_SCL_SPR = '07' THEN 'W'
				 WHEN LC_REA_SCL_SPR = '08' THEN 'W'
				 WHEN LC_REA_SCL_SPR = '10' THEN 'W'
				 WHEN LC_REA_SCL_SPR = '18' THEN 'W'
				 WHEN LC_REA_SCL_SPR = '19' THEN 'W'
				 ELSE ''
			END AS CalculatedType,
			LD_SCL_SPR
		FROM
			CDW..SD10_STU_SPR
		WHERE
			LC_STA_STU10 = 'A'
	) SD10
		ON LN10.LF_STU_SSN = SD10.LF_STU_SSN
	LEFT JOIN
	(	
		SELECT
			LN72.BF_SSN, 
			LN72.LN_SEQ,
			LN72.LR_ITR,
			LN72.LC_ITR_TYP,
			ROW_NUMBER() OVER (PARTITION BY LN72.BF_SSN, LN72.LN_SEQ ORDER BY LD_STA_LON72 DESC) AS SEQ
		FROM
			CDW..LN72_INT_RTE_HST LN72
			INNER JOIN CDW..LN10_LON LN10
				ON LN10.BF_SSN = LN72.BF_SSN
				AND LN10.LN_SEQ = LN72.LN_SEQ
		WHERE
			LN72.LC_STA_LON72 = 'A'
			AND	CAST(GETDATE() AS DATE) BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
			AND LN10.LF_DOE_SCL_ORG = @School
	) LN72 
		ON LN10.BF_SSN = LN72.BF_SSN
		AND LN10.LN_SEQ = LN72.LN_SEQ
		AND LN72.SEQ = 1
	LEFT JOIN CDW.calc.RepaymentSchedules RS
		ON RS.BF_SSN = LN10.BF_SSN
		AND RS.LN_SEQ = LN10.LN_SEQ
		AND RS.CurrentGradation = 1
	LEFT JOIN
	(
		SELECT
			BF_SSN,
			MAX(CASE WHEN RankedReference = 1 THEN DM_PRS_1 ELSE NULL END) AS [Reference 1 First Name],
			MAX(CASE WHEN RankedReference = 1 THEN DM_PRS_LST ELSE NULL END) AS [Reference 1 Last Name],
			MAX(CASE WHEN RankedReference = 1 THEN BC_RFR_REL_BR ELSE NULL END) AS [Reference 1 Relation],
			MAX(CASE WHEN RankedReference = 1 THEN [Reference 1 Phone] ELSE NULL END) AS [Reference 1 Phone],
			MAX(CASE WHEN RankedReference = 1 THEN [Reference 1 Phone Number Extension] ELSE NULL END) AS [Reference 1 Phone Number Extension],
			MAX(CASE WHEN RankedReference = 1 THEN [Reference 1 International Prefix Code] ELSE NULL END) AS [Reference 1 International Prefix Code],
			MAX(CASE WHEN RankedReference = 2 THEN DM_PRS_1 ELSE NULL END) AS [Reference 2 First Name],
			MAX(CASE WHEN RankedReference = 2 THEN DM_PRS_LST ELSE NULL END) AS [Reference 2 Last Name],
			MAX(CASE WHEN RankedReference = 2 THEN BC_RFR_REL_BR ELSE NULL END) AS [Reference 2 Relation],
			MAX(CASE WHEN RankedReference = 2 THEN [Reference 1 Phone] ELSE NULL END) AS [Reference 2 Phone],
			MAX(CASE WHEN RankedReference = 2 THEN [Reference 1 Phone Number Extension] ELSE NULL END) AS [Reference 2 Phone Number Extension],
			MAX(CASE WHEN RankedReference = 2 THEN [Reference 1 International Prefix Code] ELSE NULL END) AS [Reference 2 International Prefix Code]
		FROM
		(	
			SELECT
				RF10.BF_SSN,
				PD10.DM_PRS_1,
				PD10.DM_PRS_LST,
				RF10.BC_RFR_REL_BR,
				COALESCE(PD40.DN_DOM_PHN_ARA + PD40.DN_DOM_PHN_XCH + PD40.DN_DOM_PHN_LCL, PD40.DN_FGN_PHN_LCL) AS [Reference 1 Phone],
				PD40.DN_PHN_XTN AS [Reference 1 Phone Number Extension],
				PD40.DN_FGN_PHN_INL + PD40.DN_FGN_PHN_CNY + PD40.DN_FGN_PHN_CT AS [Reference 1 International Prefix Code],
				RANK() OVER(PARTITION BY RF10.BF_SSN ORDER BY RF10.BF_LST_DTS_RF10) AS RankedReference
			FROM
				CDW..RF10_RFR RF10
				INNER JOIN CDW..PD10_PRS_NME PD10
					ON PD10.DF_PRS_ID = RF10.BF_RFR
				INNER JOIN 
				(
					SELECT DISTINCT
						BF_SSN
					FROM
						CDW..LN10_LON
					WHERE 
						LF_DOE_SCL_ORG = @School
				) LN10
					ON LN10.BF_SSN = RF10.BF_SSN
				LEFT JOIN CDW..PD40_PRS_PHN PD40 
					ON PD40.DF_PRS_ID = PD10.DF_PRS_ID
					AND PD40.DC_PHN = 'H'
					AND PD40.DI_PHN_VLD = 'Y'
					AND RF10.BC_RFR_TYP = 'P'
			WHERE
				RF10.BC_STA_REFR10 = 'A'
		) Ranked
		WHERE
			Ranked.RankedReference IN (1,2)
		GROUP BY
			Ranked.BF_SSN
	) RF10
		ON RF10.BF_SSN = LN10.BF_SSN
	LEFT JOIN
	(
		SELECT DISTINCT
			LN10.LF_STU_SSN,
			PD10.DM_PRS_1 AS [Student First Name],
			PD10.DM_PRS_LST AS [Student Last Name],
			PD10.DM_PRS_MID AS [Student Middle Name],
			CONVERT(VARCHAR,PD10.DD_BRT,110) AS [Student DOB],
			PD30.DX_STR_ADR_1 AS [Student Address Line 1],
			PD30.DX_STR_ADR_2 AS [Student Address Line 2],
			PD30.DM_CT AS [Student City],
			PD30.DC_DOM_ST AS [Student State],
			PD30.DF_ZIP_CDE AS [Student ZIP]
		FROM
			CDW..LN10_LON LN10
			INNER JOIN CDW..PD10_PRS_NME PD10
				ON LN10.LF_STU_SSN = PD10.DF_PRS_ID
			INNER JOIN CDW..PD13_PRS_TYP_PVT PD13
				ON PD13.DF_PRS_ID = PD10.DF_PRS_ID
				AND PD13.DC_PRS_TYP != 'B'
			LEFT JOIN CDW..PD30_PRS_ADR PD30 
				ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
				AND PD30.DC_ADR = 'L'
				AND PD30.DI_VLD_ADR = 'Y'
	)StudentDemos
		ON StudentDemos.LF_STU_SSN = LN10.LF_STU_SSN
	LEFT JOIN
	(
		SELECT
			LN50.BF_SSN,
			LN50.LN_SEQ,
			DF10.LC_DFR_TYP,
			LN50.LD_DFR_BEG,
			LN50.LD_DFR_END
		FROM
			CDW..LN50_BR_DFR_APV LN50
			INNER JOIN CDW..DF10_BR_DFR_REQ DF10
				ON LN50.BF_SSN = DF10.BF_SSN
				AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
			INNER JOIN CDW..LN10_LON LN10
				ON LN10.BF_SSN = LN50.BF_SSN
				AND LN10.LN_SEQ = LN50.LN_SEQ
		WHERE
			DF10.LC_DFR_TYP IN ('15','18','19','22','23')
			AND DF10.LC_STA_DFR10 = 'A'
			AND LN50.LC_STA_LON50 = 'A'
			AND LN50.LC_DFR_RSP != '003'	
			AND DF10.LC_DFR_STA = 'A'
			AND CAST(GETDATE() AS DATE) BETWEEN LN50.LD_DFR_BEG AND LN50.LD_DFR_END
			AND LN10.LF_DOE_SCL_ORG = @School
	) InSchoolDefer
		ON InSchoolDefer.BF_SSN = LN10.BF_SSN
		AND InSchoolDefer.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	(
		SELECT
			d.BF_SSN,
			d.LN_SEQ,
			MAX(CASE WHEN d.LC_DFR_TYP = '29' THEN d.TimeUsed ELSE NULL END) OVER(PARTITION BY d.BF_SSN, d.LN_SEQ) AS [Cumulative Economic Hardship Time Used],
			MAX(CASE WHEN d.LC_DFR_TYP = '06' THEN d.TimeUsed ELSE NULL END) OVER(PARTITION BY d.BF_SSN, d.LN_SEQ) AS [Cumulative Graduate Fellowship Time Used],
			MAX(CASE WHEN d.LC_DFR_TYP = '09' THEN d.TimeUsed ELSE NULL END) OVER(PARTITION BY d.BF_SSN, d.LN_SEQ) AS [Cumulative Internship Residency Time Used],
			MAX(CASE WHEN d.LC_DFR_TYP = '01' THEN d.TimeUsed ELSE NULL END) OVER(PARTITION BY d.BF_SSN, d.LN_SEQ) AS [Cumulative NOAA Deferment Time Used],
			MAX(CASE WHEN d.LC_DFR_TYP = '46' THEN d.TimeUsed ELSE NULL END) OVER(PARTITION BY d.BF_SSN, d.LN_SEQ) AS [Cumulative Parent PLUS Six Month Post Enrollment Time Used],
			MAX(CASE WHEN d.LC_DFR_TYP = '12' THEN d.TimeUsed ELSE NULL END) OVER(PARTITION BY d.BF_SSN, d.LN_SEQ) AS [Cumulative Parental Leave Deferment Time Used],
			MAX(CASE WHEN d.LC_DFR_TYP = '07' THEN d.TimeUsed ELSE NULL END) OVER(PARTITION BY d.BF_SSN, d.LN_SEQ) AS [Cumulative Peace Corps Deferment Time Used],
			MAX(CASE WHEN d.LC_DFR_TYP = '04' THEN d.TimeUsed ELSE NULL END) OVER(PARTITION BY d.BF_SSN, d.LN_SEQ) AS [Cumulative Public Health Service Deferment Time Used],
			MAX(CASE WHEN d.LC_DFR_TYP = '20' THEN d.TimeUsed ELSE NULL END) OVER(PARTITION BY d.BF_SSN, d.LN_SEQ) AS [Cumulative Rehabilitation Training Deferment Time Used],
			MAX(CASE WHEN d.LC_DFR_TYP = '10' THEN d.TimeUsed ELSE NULL END) OVER(PARTITION BY d.BF_SSN, d.LN_SEQ) AS [Cumulative Tax Exempt Organization Deferment Time Used],
			MAX(CASE WHEN d.LC_DFR_TYP = '14' THEN d.TimeUsed ELSE NULL END) OVER(PARTITION BY d.BF_SSN, d.LN_SEQ) AS [Cumulative Teacher Shortage Area Deferment Time Used],
			MAX(CASE WHEN d.LC_DFR_TYP = '02' THEN d.TimeUsed ELSE NULL END) OVER(PARTITION BY d.BF_SSN, d.LN_SEQ) AS [Cumulative Temporary Disability Deferment Time Used],
			MAX(CASE WHEN d.LC_DFR_TYP = '13' THEN d.TimeUsed ELSE NULL END) OVER(PARTITION BY d.BF_SSN, d.LN_SEQ) AS [Cumulative Unemployment Deferment Time Used],
			MAX(CASE WHEN d.LC_DFR_TYP = '11' THEN d.TimeUsed ELSE NULL END) OVER(PARTITION BY d.BF_SSN, d.LN_SEQ) AS [Cumulative Working Mother Deferment Time Used],
			MAX(d.LD_DFR_BEG) OVER(PARTITION BY d.BF_SSN, d.LN_SEQ) AS MaxBegin,
			MAX(d.LD_DFR_END) OVER(PARTITION BY d.BF_SSN, d.LN_SEQ) AS MaxEnd
		FROM
		(
			SELECT DISTINCT
				LN50.BF_SSN,
				LN50.LN_SEQ,
				DF10.LC_DFR_TYP,
				MAX(LN50.LD_DFR_BEG) OVER(PARTITION BY LN50.BF_SSN, LN50.LN_SEQ, DF10.LC_DFR_TYP) AS LD_DFR_BEG,
				MAX(LN50.LD_DFR_END) OVER(PARTITION BY LN50.BF_SSN, LN50.LN_SEQ, DF10.LC_DFR_TYP) AS LD_DFR_END,
				SUM(CASE WHEN LN50.LD_DFR_END < CAST(GETDATE() AS DATE) THEN DATEDIFF(DAY,LN50.LD_DFR_BEG, LN50.LD_DFR_END)
						ELSE DATEDIFF(DAY,LN50.LD_DFR_BEG, CAST(GETDATE() AS DATE))
				END) OVER(PARTITION BY LN50.BF_SSN, LN50.LN_SEQ, DF10.LC_DFR_TYP) / 30.4 AS TimeUsed
			FROM
				CDW..LN50_BR_DFR_APV LN50
				INNER JOIN CDW..DF10_BR_DFR_REQ DF10
					ON LN50.BF_SSN = DF10.BF_SSN
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
				INNER JOIN CDW..LN10_LON LN10
					ON LN10.BF_SSN = LN50.BF_SSN
					AND LN10.LN_SEQ = LN50.LN_SEQ
			WHERE
				DF10.LC_STA_DFR10 = 'A'
				AND LN50.LC_STA_LON50 = 'A'
				AND LN50.LC_DFR_RSP != '003'	
				AND DF10.LC_DFR_STA = 'A'
				AND CAST(GETDATE() AS DATE) >= LN50.LD_DFR_BEG
				AND LN10.LF_DOE_SCL_ORG = @School
		) d
	) MaxLN50
		ON MaxLN50.BF_SSN = LN10.BF_SSN
		AND MaxLN50.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	(
		SELECT
			LN50.BF_SSN,
			LN50.LN_SEQ,
			LN50.LD_DFR_BEG,
			LN50.LD_DFR_END,
			DF10.LC_DFR_TYP
		FROM
			CDW..LN50_BR_DFR_APV LN50
			INNER JOIN CDW..DF10_BR_DFR_REQ DF10
				ON DF10.BF_SSN = LN50.BF_SSN
				AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
	) DF10
		ON DF10.BF_SSN = MaxLN50.BF_SSN
		AND DF10.LN_SEQ = MaxLN50.LN_SEQ
		AND DF10.LD_DFR_BEG = MaxLN50.MaxBegin
		AND DF10.LD_DFR_END = MaxLN50.MaxEnd
	LEFT JOIN
	(
		SELECT
			LN60.BF_SSN,
			LN60.LN_SEQ,
			LN60.LD_FOR_BEG,
			LN60.LD_FOR_END
		FROM
			CDW..LN60_BR_FOR_APV LN60
			INNER JOIN CDW..FB10_BR_FOR_REQ FB10
				ON LN60.BF_SSN = FB10.BF_SSN
				AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
			INNER JOIN CDW..LN10_LON LN10
				ON LN10.BF_SSN = LN60.BF_SSN
				AND LN10.LN_SEQ = LN60.LN_SEQ
		WHERE
			FB10.LC_STA_FOR10 = 'A'
			AND LN60.LC_STA_LON60 = 'A'
			AND LN60.LC_FOR_RSP != '003'	
			AND FB10.LC_FOR_STA = 'A'
			AND CAST(GETDATE() AS DATE) BETWEEN LN60.LD_FOR_BEG AND LN60.LD_FOR_END
			AND LN10.LF_DOE_SCL_ORG = @School
	) ActiveForb
		ON ActiveForb.BF_SSN = LN10.BF_SSN
		AND ActiveForb.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	(
		SELECT DISTINCT
			f.BF_SSN,
			f.LN_SEQ,
			MAX(CASE WHEN f.LC_FOR_TYP = '05' THEN 36 - f.TimeUsed ELSE NULL END) OVER(PARTITION BY f.BF_SSN, f.LN_SEQ) AS [Forbearance Time Available],
			MAX(f.LD_FOR_BEG) OVER(PARTITION BY f.BF_SSN, f.LN_SEQ) AS MaxBegin,
			MAX(f.LD_FOR_END) OVER(PARTITION BY f.BF_SSN, f.LN_SEQ) AS MaxEnd
		FROM
		(
			SELECT DISTINCT
				LN60.BF_SSN,
				LN60.LN_SEQ,
				FB10.LC_FOR_TYP,
				MAX(LN60.LD_FOR_BEG) OVER(PARTITION BY LN60.BF_SSN, LN60.LN_SEQ, FB10.LC_FOR_TYP) AS LD_FOR_BEG,
				MAX(LN60.LD_FOR_END) OVER(PARTITION BY LN60.BF_SSN, LN60.LN_SEQ, FB10.LC_FOR_TYP) AS LD_FOR_END,
				SUM(CASE WHEN LN60.LD_FOR_END < CAST(GETDATE() AS DATE) THEN DATEDIFF(DAY,LN60.LD_FOR_BEG, LN60.LD_FOR_END)
						ELSE DATEDIFF(DAY,LN60.LD_FOR_BEG, CAST(GETDATE() AS DATE))
				END) OVER(PARTITION BY LN60.BF_SSN, LN60.LN_SEQ, FB10.LC_FOR_TYP) / 30.4 AS TimeUsed
			FROM
				CDW..LN60_BR_FOR_APV LN60
				INNER JOIN CDW..FB10_BR_FOR_REQ FB10
					ON LN60.BF_SSN = FB10.BF_SSN
					AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
				INNER JOIN CDW..LN10_LON LN10
					ON LN10.BF_SSN = LN60.BF_SSN
					AND LN10.LN_SEQ = LN60.LN_SEQ
			WHERE
				FB10.LC_STA_FOR10 = 'A'
				AND LN60.LC_STA_LON60 = 'A'
				AND LN60.LC_FOR_RSP != '003'	
				AND FB10.LC_FOR_STA = 'A'
				AND CAST(GETDATE() AS DATE) >= LN60.LD_FOR_BEG
				AND LN10.LF_DOE_SCL_ORG = @School
		) f
	) MaxLN60
		ON MaxLN60.BF_SSN = LN10.BF_SSN
		AND MaxLN60.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	(
		SELECT
			LN60.BF_SSN,
			LN60.LN_SEQ,
			LN60.LD_FOR_BEG,
			LN60.LD_FOR_END,
			FB10.LC_FOR_TYP
		FROM
			CDW..LN60_BR_FOR_APV LN60
			INNER JOIN CDW..FB10_BR_FOR_REQ FB10
				ON FB10.BF_SSN = LN60.BF_SSN
				AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
	) FB10
		ON FB10.BF_SSN = MaxLN60.BF_SSN
		AND FB10.LN_SEQ = MaxLN60.LN_SEQ
		AND FB10.LD_FOR_BEG = MaxLN60.MaxBegin
		AND FB10.LD_FOR_END = MaxLN60.MaxEnd
	LEFT JOIN CDW..LN09_RPD_PIO_CVN LN09
		ON LN09.BF_SSN = LN10.BF_SSN
		AND LN09.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN CDW..LN83_EFT_TO_LON LN83
		ON LN83.BF_SSN = LN10.BF_SSN
		AND LN83.LN_SEQ = LN10.LN_SEQ
		AND LN83.LC_STA_LN83 = 'A'
	LEFT JOIN 
	(
		SELECT DISTINCT
			LN16.BF_SSN,
			LN16.LN_SEQ,
			MIN(CASE WHEN LN16.LC_STA_LON16 IN('1','3') THEN LN16.LD_DLQ_OCC ELSE NULL END) OVER(PARTITION BY LN16.BF_SSN, LN16.LN_SEQ) AS Min_LD_DLQ_OCC,
			CASE WHEN CAST(DATEADD(DAY,-4,GETDATE()) AS DATE) <= CAST(LN16.LD_DLQ_MAX AS DATE) AND LN16.LC_STA_LON16 = '1' THEN LN16.LD_DLQ_OCC ELSE NULL END AS Current_LD_DLQ_OCC,
			CASE WHEN CAST(DATEADD(DAY,-4,GETDATE()) AS DATE) <= CAST(LN16.LD_DLQ_MAX AS DATE) AND LN16.LC_STA_LON16 = '1' THEN LN16.LN_DLQ_MAX ELSE NULL END AS Current_LN_DLQ_MAX,
			MAX(CASE WHEN LN16.LC_STA_LON16 = '3' THEN LN16.LD_STA_LON16 ELSE NULL END) OVER(PARTITION BY LN16.BF_SSN, LN16.LN_SEQ, LN16.LC_STA_LON16) AS ResolutionDate
		FROM
			CDW..LN16_LON_DLQ_HST LN16
			INNER JOIN CDW..LN10_LON LN10
				ON LN10.BF_SSN = LN16.BF_SSN
				AND LN10.LN_SEQ = LN16.LN_SEQ
		WHERE
			LN10.LF_DOE_SCL_ORG = @School
	) LN16
		ON LN16.BF_SSN = LN10.BF_SSN
		AND LN16.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	(
		SELECT
			LN90.BF_SSN,
			LN90.LN_SEQ,
			MAX(LN90.LD_FAT_APL) AS LD_FAT_APL
		FROM
			CDW..LN90_FIN_ATY LN90
			INNER JOIN CDW..LN10_LON LN10
				ON LN10.BF_SSN = LN90.BF_SSN
				AND LN10.LN_SEQ = LN90.LN_SEQ
		WHERE
			LN90.PC_FAT_TYP = '04'
			AND LN90.PC_FAT_SUB_TYP = '98'
			AND COALESCE(LN90.LC_FAT_REV_REA,'') = '' --JW 10/2
			AND LN90.LC_STA_LON90 = 'A'
			AND LN10.LF_DOE_SCL_ORG = @School
		GROUP BY
			LN90.BF_SSN,
			LN90.LN_SEQ
	) CDR
		ON CDR.BF_SSN = LN10.BF_SSN
		AND CDR.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	(
		SELECT DISTINCT
			LN90.BF_SSN,
			LN90.LN_SEQ,
			MIN(LN90.LD_FAT_EFF) AS Min_LD_FAT_EFF,
			MAX(LN90.LD_FAT_EFF) AS Max_LD_FAT_EFF
		FROM
			CDW..LN90_FIN_ATY LN90
			INNER JOIN CDW..LN10_LON LN10
				ON LN10.BF_SSN = LN90.BF_SSN
				AND LN10.LN_SEQ = LN90.LN_SEQ
		WHERE
			LN90.PC_FAT_TYP = '10'
			AND LN90.PC_FAT_SUB_TYP = '10'
			AND COALESCE(LN90.LC_FAT_REV_REA,'') = '' --JW 10/2
			AND LN90.LC_STA_LON90 = 'A'
			AND LN10.LF_DOE_SCL_ORG = @School
		GROUP BY
			LN90.BF_SSN,
			LN90.LN_SEQ
	) LN90Dates
		ON LN90Dates.BF_SSN = LN10.BF_SSN
		AND LN90Dates.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	(
		SELECT DISTINCT
			LN90.BF_SSN,
			LN90.LN_SEQ,
			LN90.LD_FAT_EFF,
			COALESCE(LN90.LA_FAT_NSI,0) + COALESCE(LN90.LA_FAT_CUR_PRI,0) AS PaymentAmount
		FROM
			CDW..LN90_FIN_ATY LN90
			INNER JOIN CDW..LN10_LON LN10
				ON LN10.BF_SSN = LN90.BF_SSN
				AND LN10.LN_SEQ = LN90.LN_SEQ
		WHERE
			LN90.PC_FAT_TYP = '10'
			AND LN90.PC_FAT_SUB_TYP = '10'
			AND COALESCE(LN90.LC_FAT_REV_REA,'') = '' --JW 10/2
			AND LN90.LC_STA_LON90 = 'A'
			AND LN10.LF_DOE_SCL_ORG = @School
	) LN90Amounts
		ON LN90Amounts.BF_SSN = LN10.BF_SSN
		AND LN90Amounts.LN_SEQ = LN10.LN_SEQ
		AND LN90Amounts.LD_FAT_EFF = LN90Dates.Max_LD_FAT_EFF
	LEFT JOIN
	(
		SELECT DISTINCT
			AY10.BF_SSN,
			LN85.LN_SEQ,
			AY10.LD_ATY_REQ_RCV
		FROM
			CDW..AY10_BR_LON_ATY AY10
			INNER JOIN CDW..LN85_LON_ATY LN85
				ON LN85.BF_SSN = AY10.BF_SSN
				AND LN85.LN_ATY_SEQ = AY10.LN_ATY_SEQ
			INNER JOIN CDW..LN10_LON LN10
				ON LN10.BF_SSN = LN85.BF_SSN
				AND LN10.LN_SEQ = LN85.LN_SEQ
		WHERE
			AY10.PF_REQ_ACT = 'DLFDL'
			AND AY10.LC_STA_ACTY10 = 'A'
			AND CAST(AY10.LD_ATY_REQ_RCV AS DATE) BETWEEN CAST(DATEADD(DAY,-90,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE)
			AND LN10.LF_DOE_SCL_ORG = @School
	) DLFDL
		ON DLFDL.BF_SSN = LN10.BF_SSN
		AND DLFDL.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	(
		SELECT DISTINCT
			LN80.BF_SSN,
			LN80.LN_SEQ,
			LN80.LA_BIL_CUR_DU,
			LN80.LD_BIL_DU_LON,
			LN80.LA_BIL_PAS_DU --INCOMPLETE needs work maybe
		FROM
			CDW..LN80_LON_BIL_CRF LN80
			INNER JOIN CDW..BL10_BR_BIL BL10
				ON BL10.BF_SSN = LN80.BF_SSN
				AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
			INNER JOIN CDW..LN10_LON LN10
				ON LN10.BF_SSN = LN80.BF_SSN
				AND LN10.LN_SEQ = LN80.LN_SEQ
		WHERE
			LN80.LC_STA_LON80 = 'A'
			AND LN80.LC_BIL_TYP_LON = 'P'
			AND BL10.LC_IND_BIL_SNT NOT IN('N','Q','5','7')
			AND CAST(LN80.LD_BIL_DU_LON AS DATE) > CAST(GETDATE() AS DATE)
			AND CAST(LN80.LD_BIL_CRT AS DATE) > CAST(DATEADD(DAY,-30,LN80.LD_BIL_DU_LON) AS DATE)
			AND LN10.LF_DOE_SCL_ORG = @School
	) LN80
		ON LN80.BF_SSN = LN10.BF_SSN
		AND LN80.LN_SEQ = LN10.LN_SEQ
WHERE
	LN10.LF_DOE_SCL_ORG = @School