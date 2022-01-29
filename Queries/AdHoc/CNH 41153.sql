SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

--X. currently on an IDR plan with it expiring between X/XX and X/XX
--X. remove any borrower who has the job UTNWOXX run on the account (already in CLS.emailbtcf.CampaignData)
--and any with an IDRPN arc
--X.with an MPN signature date after Jan X XXXX
--X. We also are only looking for borrowers with a valid phone number marked �M� for mobile, and has consent. 

--This list will need to contain the following columns:
--Account Number
--First Name
--Last Name
--Mobile Phone X
--Mobile Phone X (PDXX only allows one phone number per type so not sure what this would be but wasn't an issue as there were no results)
--these were requested but not provided as there were no results
--Performance Category
--Segment

SELECT DISTINCT
	--from previous request
	--COUNT(LNXX.LN_SEQ) OVER(PARTITION BY LNXX.IC_LON_PGM) AS CountLoans,
	--LNXX.IC_LON_PGM
	PDXX.DF_SPE_ACC_ID,
	PDXX.DM_PRS_X,
	PDXX.DM_PRS_LST,
	CONCAT(PDXX.DN_DOM_PHN_ARA,'-',PDXX.DN_DOM_PHN_LCL,'-',PDXX.DN_DOM_PHN_XCH) AS Phone
	--for testing
	--,CAST(DATEADD(MONTH,RS.LN_RPS_TRM, RS.TermStartDate) AS DATE) as idr_exp_date
	--,PDXX.DC_PHN
	--,PDXX.DI_PHN_VLD
	--,PDXX.DC_ALW_ADL_PHN
FROM
	CDW..LNXX_LON LNXX
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	INNER JOIN CDW..PDXX_PRS_PHN PDXX
		ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
		AND PDXX.DC_PHN = 'M'
		AND PDXX.DI_PHN_VLD = 'Y'
		AND PDXX.DC_ALW_ADL_PHN IN ('L','P','X')
	INNER JOIN CDW..GRXX_RPT_LON_APL GRXX
		ON GRXX.BF_SSN = LNXX.BF_SSN
		AND GRXX.LN_SEQ = LNXX.LN_SEQ
	INNER JOIN CDW.calc.RepaymentSchedules RS
		ON RS.BF_SSN = LNXX.BF_SSN
		AND RS.LN_SEQ = LNXX.LN_SEQ
	LEFT JOIN CDW..AYXX_BR_LON_ATY AYXX
		ON AYXX.BF_SSN = LNXX.BF_SSN
		AND AYXX.PF_REQ_ACT = 'IDRPN'
		AND AYXX.LC_STA_ACTYXX = 'A'
	LEFT JOIN CLS.emailbtcf.CampaignData CD
		ON CD.AccountNumber = PDXX.DF_SPE_ACC_ID
		AND CD.AddedBy = 'UTNWOXX'
		AND CD.InactivatedAt IS NULL
		AND 
		(	
			CD.EmailSentAt IS NOT NULL
			OR CD.ArcProcessedAt IS NOT NULL
		)
WHERE
	LNXX.IC_LON_PGM IN('DLSTFD','DLUNST') --Stafford loans
	AND LNXX.LA_CUR_PRI > X.XX
	AND LNXX.LC_STA_LONXX = 'R'
	AND GRXX.WD_BR_SIG_MPN >= 'XXXX-XX-XX' --MPN date
	AND RS.CurrentGradation = X
	AND RS.LC_TYP_SCH_DIS IN ('CX','CX','CX','CQ','CA','CP','IX','IA','IB','IL','IX','IP') --IDR plans
	AND CAST(DATEADD(MONTH,RS.LN_RPS_TRM, RS.TermStartDate) AS DATE) BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX'
	AND AYXX.BF_SSN IS NULL --No IDRPN arc
	AND CD.AccountNumber IS NULL --No UTNWOXX
--for testing
--ORDER BY 
	--CAST(DATEADD(MONTH,RS.LN_RPS_TRM, RS.TermStartDate) AS DATE)

