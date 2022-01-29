--The purpose of this query is to find all active FFEL PLUS and SLS Loans (PLUS, PLUSGB, SLS) with a current balance. Can we include the following information:

--Customer Name
--Acct #
--Loan Type (PLUS, PLUSGB, SLS)
--Balance
--Ph #
--Address
--Email Address
--Repayment Plan
--Expected Payoff Date
SELECT DISTINCT
	RTRIM(PD10.DM_PRS_1) AS FirstName,
	RTRIM(PD10.DM_PRS_LST) AS LastName,
	PD10.DF_SPE_ACC_ID AS AccountNumber,
	LN10.LN_SEQ,
	LN10.IC_LON_PGM AS LoanType,
	ISNULL(LN10.LA_CUR_PRI,0.00) + ISNULL(DW01.WA_TOT_BRI_OTS,0.00) AS Balance,
	PD40.DC_PHN AS PhoneType,
	ISNULL(PD40.DN_DOM_PHN_ARA,'') + ISNULL(PD40.DN_DOM_PHN_XCH,'') + ISNULL(PD40.DN_DOM_PHN_LCL,'') AS PhoneNumber,
	ISNULL(PD40.DN_FGN_PHN_INL,'') + ISNULL(PD40.DN_FGN_PHN_CNY,'') + ISNULL(PD40.DN_FGN_PHN_CT,'') + ISNULL(PD40.DN_FGN_PHN_LCL,'') AS ForeignPhone,
	PD30.DC_ADR AS AddrType,
	PD30.DX_STR_ADR_1,
	PD30.DX_STR_ADR_2,
	PD30.DM_CT,
	PD30.DC_DOM_ST,
	PD30.DF_ZIP_CDE,
	PD32.EmailAddr,
	RS.LC_TYP_SCH_DIS AS RepaymentPlan,
	Payoff.ExpectedPayoffDate AS PayoffDate
FROM
	UDW..LN10_LON LN10
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = LN10.BF_SSN
	INNER JOIN UDW..DW01_DW_CLC_CLU DW01
		ON DW01.BF_SSN = LN10.BF_SSN
		AND DW01.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN UDW.calc.RepaymentSchedules RS
		ON RS.BF_SSN = LN10.BF_SSN
		AND RS.LN_SEQ = LN10.LN_SEQ
		AND RS.CurrentGradation = 1
	LEFT JOIN 
	(
		SELECT DISTINCT
			RS.BF_SSN,
			RS.LN_SEQ,
			DATEADD(MONTH, RS.LN_RPS_TRM, RS.TermStartDate) AS ExpectedPayoffDate
		FROM
			UDW.calc.RepaymentSchedules RS
			INNER JOIN 
			(
				SELECT
					BF_SSN,
					LN_SEQ,
					MAX(TermStartDate) AS TermStartDate
				FROM
					UDW.calc.RepaymentSchedules
				GROUP BY
					BF_SSN,
					LN_SEQ
			) AS MaxTerms
				ON MaxTerms.BF_SSN = RS.BF_SSN
				AND MaxTerms.LN_SEQ = RS.LN_SEQ
				AND MaxTerms.TermStartDate = RS.TermStartDate
	) Payoff
		ON Payoff.BF_SSN = LN10.BF_SSN
		AND Payoff.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN UDW..PD42_PRS_PHN PD40
		ON PD40.DF_PRS_ID = PD10.DF_PRS_ID
		AND PD40.DI_PHN_VLD = 'Y'
	LEFT JOIN UDW..PD30_PRS_ADR PD30
		ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
		AND PD30.DI_VLD_ADR = 'Y'
	LEFT JOIN 
	(
		SELECT
			EMAIL.*,
			ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) AS EmailPriority -- number in order of Email.PriorityNumber
		FROM
		(
			SELECT
				PD32.DF_PRS_ID,
				ISNULL(PH05.DX_CNC_EML_ADR, PD32.DX_ADR_EML) AS EmailAddr,
				CASE 
					WHEN PH05.DX_CNC_EML_ADR IS NOT NULL THEN 1
					WHEN DC_ADR_EML = 'H' THEN 2 -- home
					WHEN DC_ADR_EML = 'A' THEN 3 -- alternate
					WHEN DC_ADR_EML = 'W' THEN 4 -- work
				END AS PriorityNumber
			FROM
				UDW..PD10_PRS_NME PD10
				LEFT JOIN UDW..PD32_PRS_ADR_EML PD32
					ON PD10.DF_PRS_ID = PD32.DF_PRS_ID
					AND PD32.DI_VLD_ADR_EML = 'Y' -- valid email address
					AND PD32.DC_STA_PD32 = 'A' -- active email address record
				LEFT JOIN UDW..PH05_CNC_EML PH05
					ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
					AND PH05.DI_CNC_ELT_OPI = 'Y'
					AND PH05.DI_VLD_CNC_EML_ADR = 'Y'				
		) Email
	) PD32 
		ON PD32.DF_PRS_ID = PD10.DF_PRS_ID
		AND PD32.EmailPriority = 1 --highest priority email only
WHERE
	LN10.LA_CUR_PRI > 0.00
	AND LN10.LC_STA_LON10 = 'R'
	AND LN10.IC_LON_PGM IN ('PLUS','PLUSGB','SLS')