CREATE PROCEDURE [clschllnfd].[Test_GetSchoolDataForBorrower]
	@BF_SSN CHAR(9),
	@LN_SEQ INT
AS
	SELECT
	SUBSTRING(LN10.LF_DOE_SCL_ORG, 1, 6) [SchoolCode],
	LN10.LF_DOE_SCL_ORG [SchoolBranchCode],
	SC10.IM_SCL_FUL [SchoolName],
	CAST(DATEADD(MONTH, -1, GETDATE()) AS DATE) [CloseDate],
	'502' [CurrentGaCode],
	PD10.DF_PRS_ID [StudentSsn],
	PD10.DM_PRS_LST [LastName],
	PD10.DM_PRS_1 [FirstName],
	CAST(PD10.DD_BRT AS DATE) [StudentDob],
	'' [PlusSsn],
	'' [PlusLastName],
	'' [PlusFirstName],
	'' [PlusDob],
	ISNULL(LP.NsldsCode, '  ') [LoanType],
	LN10.LD_LON_GTR [LoanDate],
	LN10.LA_LON_AMT_GTR [LoanAmount],
	LN15.LA_DSB [TotalDisbursed],
	CAN.CancelledAmount [TotalCancelled],
	LN10.LC_STA_LON10 [CurrentLoanStatus],
	LN10.LD_STA_LON10 [CurrentLoanStatusDate],
	LN10.LA_CUR_PRI [OutstandingPrincipalBalance],
	CAST(LF_LST_DTS_LN10 AS DATE) [OutstandingPrincipalBalanceDate],
	LN10.LA_NSI_OTS [OutstandingInterestBalance],
	CAST(LF_LST_DTS_LN10 AS DATE) [OutstandingInterestBalanceDate],
	FS10.LF_FED_AWD [AwardId],
	'12345678901234567' [LoanIdentifier],
	PD10.DF_PRS_ID [StudentNumber],
	'1234567890123' [StudentIdentifier],
	'502' [ContactCode],
	ISNULL(PD32.DX_ADR_EML, '') [Email],
	PD30.DC_ADR [AddressType],
	PD30.DX_STR_ADR_1 [StreetAddress1],
	PD30.DX_STR_ADR_2 [StreetAddress2],
	PD30.DM_CT [City],
	COALESCE(PD30.DC_DOM_ST, PD30.DM_FGN_ST) [State],
	PD30.DM_FGN_CNY [Country],
	PD30.DF_ZIP_CDE [ZIP]
FROM
	PD10_PRS_NME PD10
	INNER JOIN LN10_LON LN10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN SC10_SCH_DMO SC10
		ON SC10.IF_DOE_SCL = LN10.LF_DOE_SCL_ORG
	INNER JOIN FS10_DL_LON FS10
		ON FS10.BF_SSN = PD10.DF_PRS_ID
		AND FS10.LN_SEQ = LN10.LN_SEQ
	INNER JOIN PD30_PRS_ADR PD30
		ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
	INNER JOIN 
		(
			SELECT
				SUM(LA_DSB) [LA_DSB],
				BF_SSN,
				LN_SEQ
			FROM
				LN15_DSB
			WHERE
				LA_DSB_CAN IS NULL
				AND LC_DSB_TYP = '2'
				AND LC_STA_LON15 = '1'
			GROUP BY
				BF_SSN, 
				LN_SEQ
		) LN15
			ON LN15.BF_SSN = LN10.BF_SSN
			AND LN15.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN PD32_PRS_ADR_EML PD32
		ON PD32.DF_PRS_ID = PD10.DF_PRS_ID
		AND PD32.DC_STA_PD32 = 'A'
		AND DI_VLD_ADR_EML = 'Y'
	LEFT JOIN 
		(
			SELECT
				SUM(LA_DSB_CAN) [CancelledAmount],
				BF_SSN,
				LN_SEQ,
				LA_DSB_CAN
			FROM
				LN15_DSB
			WHERE
				LA_DSB_CAN IS NOT NULL
				AND LC_DSB_TYP = '2'
				AND LC_STA_LON15 = '1'
			GROUP BY
				BF_SSN, 
				LN_SEQ,
				LA_DSB_CAN
		) CAN
			ON CAN.BF_SSN = LN15.BF_SSN
			AND CAN.LN_SEQ = LN15.LN_SEQ
	LEFT JOIN Income_Driven_Repayment..LoanPrograms LP
		ON LP.LoanProgram = LN10.IC_LON_PGM
WHERE
	PD10.DF_PRS_ID = @BF_SSN
	AND LN10.LN_SEQ = @LN_SEQ
RETURN 0
