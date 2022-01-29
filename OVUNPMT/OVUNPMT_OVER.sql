--OverPayments
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @ScriptId VARCHAR(20) = 'OVUNPMT';
DECLARE @CurrentDateTime DATETIME = GETDATE();
DECLARE @LookBackDate DATE = DATEADD(DAY,-7,@CurrentDateTime); --TODO (FOR TESTING):  set to -7 from -50
DECLARE @HTMLFile VARCHAR(20) = 'OVRPYMTEUH.html'; --Letter Id of email message to get EmailCampaignId from Email Campaigns Table
DECLARE @EmailCampaignId INT = (SELECT EML.EmailCampaignId FROM ULS.emailbatch.HTMLFiles AS HTML INNER JOIN ULS.emailbatch.EmailCampaigns AS EML ON HTML.HTMLFileId = EML.HTMLFileId WHERE HTMLFile = @HTMLFile);
DECLARE @ARC VARCHAR(5) = 'PMTAJ'; -- ARC to add queue task for agents to reapply overpayment


DROP TABLE IF EXISTS #OVR_PMT_UH;

--select population into temp table for inserts into email and arc add tables
SELECT DISTINCT
	PD10.DF_SPE_ACC_ID AS AccountNumber,
	--PD10.DF_SPE_ACC_ID + ',' + CentralData.dbo.PascalCase(PD10.DM_PRS_1) + ',' + ISNULL(PH05.DX_CNC_EML_ADR, PD32.DX_ADR_EML) AS emailData, TODO: replaced with data from UDW.calc.EmailAddress, remove
	PD10.DF_SPE_ACC_ID + ',' + CentralData.dbo.PascalCase(PD10.DM_PRS_1) + ',' + EMAIL.EmailAddress AS emailData,
	'See instruction to reapply payment in ARC OVPRY.  Payment Post Date = ' + COALESCE(RTRIM(CONVERT(VARCHAR,MAX_PMT.MAX_LD_FAT_PST,101)),'') + '; Amount = $' + CONVERT(VARCHAR(12),CAST(COALESCE(PMT.AMOUNT,0) AS money),1) + '; Type = ' + MAX_PMT.PC_FAT_TYP + MAX_PMT.PC_FAT_SUB_TYP + '.'  AS Comment,
	IIF(OVPRY.BF_SSN IS NOT NULL,1,0) AS HasOVPRY
	--TODO:  the following attributes are only used for testing so the lines can be commented out for production
	--,CAST(PAQ.PAYMENT_AMOUNT AS NUMERIC(8,2)) AS PAQ_PMT,
	--PMT.AMOUNT,
	--DUE.AMT_DUE,
	--MAX_PMT.MAX_LD_FAT_PST,
	--MAX_PMT.PC_FAT_TYP + MAX_PMT.PC_FAT_SUB_TYP AS PMT_TYP
	INTO #OVR_PMT_UH
FROM
	UDW..PD10_PRS_NME PD10
-- borrower has a valid legal address
	INNER JOIN UDW..PD30_PRS_ADR PD30 --TODO (FOR TESTING): change back to INNER
		ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
		AND PD30.DC_ADR = 'L'
		AND PD30.DI_VLD_ADR = 'Y'
--borrower is from a targeted state
	INNER JOIN ULS.ovunpmt.States States
		ON States.Abbreviation = PD30.DC_DOM_ST
		AND States.Active = 1
--borrower has a valid e-mail address
	INNER JOIN UDW.calc.EmailAddress EMAIL
		ON EMAIL.DF_PRS_ID = PD10.DF_PRS_ID
--MAX_PMT - MAX payment posted date and payment type (for ARC comment) within lookback period for active, non-reversed 1010 and 1035 payments
	INNER JOIN
	(
		SELECT
			LN10.BF_SSN,
			LN90.PC_FAT_TYP,
			LN90.PC_FAT_SUB_TYP,
			MAX(LD_FAT_PST) AS MAX_LD_FAT_PST
		FROM 
			UDW..LN10_LON LN10
			INNER JOIN UDW..LN90_FIN_ATY LN90
				ON LN10.BF_SSN = LN90.BF_SSN
				AND LN10.LN_SEQ = LN90.LN_SEQ
		WHERE
			LN10.LA_CUR_PRI > 0.00 --open loans
			AND LN10.LC_STA_LON10 = 'R' --released loans
			AND LN90.LC_STA_LON90 = 'A' --active transactions
			AND (LC_FAT_REV_REA IS NULL	OR LC_FAT_REV_REA = '') --non-reversed transactions
			AND PC_FAT_SUB_TYP IN ('10','35')
			AND LN90.LD_FAT_PST >= @LookBackDate --payment effective within target date range
		GROUP BY
			LN10.BF_SSN,
			LN90.PC_FAT_TYP,
			LN90.PC_FAT_SUB_TYP
	) MAX_PMT
		ON PD10.DF_PRS_ID = MAX_PMT.BF_SSN
--PMT calculate borrower payments the same way UTLWF07 calculates them so they can be compared to the amount in the PA queue task which comes from UTNWF07
	INNER JOIN
	(
		SELECT DISTINCT
			LN90.BF_SSN,
			ABS(SUM(ISNULL(LN90.LA_FAT_NSI,0) + ISNULL(LN90.LA_FAT_CUR_PRI,0) + ISNULL(LN90.LA_FAT_LTE_FEE,0))) AS AMOUNT
		FROM
		--MAX_PST - MAX payment posted date within lookback period for active, non-reversed 1010 and 1035 payments
			(
				SELECT
					LN10.BF_SSN,
					LN10.LN_SEQ,
					MAX(LD_FAT_PST) AS MAX_LD_FAT_PST
				FROM 
					UDW..LN10_LON LN10
					INNER JOIN UDW..LN90_FIN_ATY LN90
						ON LN10.BF_SSN = LN90.BF_SSN
						AND LN10.LN_SEQ = LN90.LN_SEQ
				WHERE
					LN10.LA_CUR_PRI > 0.00 --open loans
					AND LN10.LC_STA_LON10 = 'R' --released loans
					AND LN90.LC_STA_LON90 = 'A' --active transactions
					AND (LC_FAT_REV_REA IS NULL	OR LC_FAT_REV_REA = '') --non-reversed transactions
					AND PC_FAT_TYP = '10' --1010 AND 1035 transactions
					AND PC_FAT_SUB_TYP IN ('10','35')
					AND LN90.LD_FAT_PST >= @LookBackDate --payment effective within target date range
				GROUP BY
					LN10.BF_SSN,
					LN10.LN_SEQ
			) MAX_PST
		--only include open, released, in repayment loans
			INNER JOIN UDW..LN10_LON LN10
				ON MAX_PST.BF_SSN = LN10.BF_SSN
				AND MAX_PST.LN_SEQ = LN10.LN_SEQ
			INNER JOIN UDW..DW01_DW_CLC_CLU DW01
				ON LN10.BF_SSN = DW01.BF_SSN
				AND LN10.LN_SEQ = DW01.LN_SEQ
				AND DW01.WC_DW_LON_STA = '03' -- in repayment
				AND ISNULL(DW01.WX_OVR_DW_LON_STA, '') = ''
		--payment information
			INNER JOIN UDW..LN90_FIN_ATY LN90
				ON LN10.BF_SSN = LN90.BF_SSN
				AND LN10.LN_SEQ = LN90.LN_SEQ
		--find the "current" bill as defined by the most recent bill sent
			INNER JOIN 
				(
					SELECT
						LN80.BF_SSN,
						LN80.LN_SEQ,
						MAX(LN80.LD_BIL_CRT) AS LD_BIL_CRT,
						MAX(LN80.LN_SEQ_BIL_WI_DTE) AS LN_SEQ_BIL_WI_DTE,
						MAX(LN80.LN_BIL_OCC_SEQ) AS LN_BIL_OCC_SEQ
					FROM
						UDW..BL10_BR_BIL BL10
						INNER JOIN UDW..LN80_LON_BIL_CRF LN80
							ON BL10.BF_SSN = LN80.BF_SSN
							AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
							AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
					WHERE
						LC_STA_LON80 = 'A'
						AND LC_BIL_TYP_LON = 'P'
						AND BL10.LC_IND_BIL_SNT  IN ('1','G','R','2','7','4','F','I','H') -- bill sent
					GROUP BY
						LN80.BF_SSN,
						LN80.LN_SEQ
				) CUR_BILL
				ON LN10.BF_SSN = CUR_BILL.BF_SSN
				AND LN10.LN_SEQ = CUR_BILL.LN_SEQ
		WHERE
			LN10.LA_CUR_PRI > 0.00 --open loans
			AND LN10.LC_STA_LON10 = 'R' --released loans
			AND LN90.LC_STA_LON90 = 'A' --active transactions
			AND (LC_FAT_REV_REA IS NULL	OR LC_FAT_REV_REA = '') --non-reversed transactions
			AND PC_FAT_TYP = '10' --1010 AND 1035 transactions
			AND PC_FAT_SUB_TYP IN ('10','35')
			AND LN90.LD_FAT_PST BETWEEN CAST(CUR_BILL.LD_BIL_CRT AS DATE) AND MAX_PST.MAX_LD_FAT_PST
		GROUP BY
			LN90.BF_SSN
	) PMT
		ON PD10.DF_PRS_ID = PMT.BF_SSN
	INNER JOIN
--DUE calculate total amount due for all loans for current bill
	(
		SELECT DISTINCT
			LN80.BF_SSN,
			SUM(ISNULL(LN80.LA_BIL_CUR_DU,0) + ISNULL(LN80.LA_BIL_PAS_DU,0)) AS AMT_DUE
		FROM
		--MAX_PST - MAX payment posted date within lookback period for active, non-reversed 1010 and 1035 payments
			(
				SELECT
					LN10.BF_SSN,
					LN10.LN_SEQ,
					MAX(LD_FAT_PST) AS MAX_LD_FAT_PST
				FROM 
					UDW..LN10_LON LN10
					INNER JOIN UDW..LN90_FIN_ATY LN90
						ON LN10.BF_SSN = LN90.BF_SSN
						AND LN10.LN_SEQ = LN90.LN_SEQ
				WHERE
					LN10.LA_CUR_PRI > 0.00 --open loans
					AND LN10.LC_STA_LON10 = 'R' --released loans
					AND LN90.LC_STA_LON90 = 'A' --active transactions
					AND (LC_FAT_REV_REA IS NULL	OR LC_FAT_REV_REA = '') --non-reversed transactions
					AND PC_FAT_TYP = '10' --1010 AND 1035 transactions
					AND PC_FAT_SUB_TYP IN ('10','35')
					AND LN90.LD_FAT_PST >= @LookBackDate --payment effective within target date range
				GROUP BY
					LN10.BF_SSN,
					LN10.LN_SEQ
			) MAX_PST
		--only include open, released, in repayment loans
			INNER JOIN UDW..LN10_LON LN10
				ON MAX_PST.BF_SSN = LN10.BF_SSN
				AND MAX_PST.LN_SEQ = LN10.LN_SEQ
			INNER JOIN UDW..DW01_DW_CLC_CLU DW01
				ON LN10.BF_SSN = DW01.BF_SSN
				AND LN10.LN_SEQ = DW01.LN_SEQ
				AND DW01.WC_DW_LON_STA = '03' -- in repayment
				AND ISNULL(DW01.WX_OVR_DW_LON_STA, '') = ''
		--find the "current" bill as defined by the most recent bill sent
			INNER JOIN 
				(
					SELECT
						LN80.BF_SSN,
						LN80.LN_SEQ,
						MAX(LN80.LD_BIL_CRT) AS LD_BIL_CRT,
						MAX(LN80.LN_SEQ_BIL_WI_DTE) AS LN_SEQ_BIL_WI_DTE,
						MAX(LN80.LN_BIL_OCC_SEQ) AS LN_BIL_OCC_SEQ
					FROM
						UDW..BL10_BR_BIL BL10
						INNER JOIN UDW..LN80_LON_BIL_CRF LN80
							ON BL10.BF_SSN = LN80.BF_SSN
							AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
							AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
					WHERE
						LC_STA_LON80 = 'A'
						AND LC_BIL_TYP_LON = 'P'
						AND BL10.LC_IND_BIL_SNT  IN ( '1','2','4','7','G','A','B','C','D','E','F','H','I','J','K','L','M','P','Q','R','8','T') -- bill sent
					GROUP BY
						LN80.BF_SSN,
						LN80.LN_SEQ
				) CUR_BILL
				ON LN10.BF_SSN = CUR_BILL.BF_SSN
				AND LN10.LN_SEQ = CUR_BILL.LN_SEQ
		--join to LN80 for the "current" bill (see join above) to get the amount due for the bill
			INNER JOIN UDW..LN80_LON_BIL_CRF LN80
				ON CUR_BILL.BF_SSN = LN80.BF_SSN
				AND CUR_BILL.LN_SEQ = LN80.LN_SEQ
				AND CUR_BILL.LD_BIL_CRT = LN80.LD_BIL_CRT
				AND CUR_BILL.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
				AND CUR_BILL.LN_BIL_OCC_SEQ = LN80.LN_BIL_OCC_SEQ
				AND LN80.LC_STA_LON80 = 'A'
				AND LN80.LC_BIL_TYP_LON = 'P'
		WHERE
			LN10.LA_CUR_PRI > 0.00 --open loans
			AND LN10.LC_STA_LON10 = 'R' --released loans
		GROUP BY
			LN80.BF_SSN
	) DUE
		ON PMT.BF_SSN = DUE.BF_SSN
--LN10_CNT count of open, released, non consolidation loans in repayment status to which the overpayment may be applied (there is no need to ask how to apply the overpayment if there is only one loan)
	LEFT JOIN
	(
		SELECT DISTINCT
			LN10.BF_SSN,
			COUNT(DISTINCT LN10.LN_SEQ) AS CNT
		FROM 
			UDW..LN10_LON LN10
			INNER JOIN UDW..DW01_DW_CLC_CLU DW01
				ON LN10.BF_SSN = DW01.BF_SSN
				AND LN10.LN_SEQ = DW01.LN_SEQ
		WHERE 
			LA_CUR_PRI > 0.00
			AND LC_STA_LON10 = 'R'
			AND DW01.WC_DW_LON_STA = '03' -- in repayment
			AND ISNULL(DW01.WX_OVR_DW_LON_STA, '') = ''
			AND LN10.IC_LON_PGM  NOT IN ('SUBCNS','UNCNS','SUBSPC','UNSPC') -- non consolidation loan
		GROUP BY
			LN10.BF_SSN
	) LN10_CNT
		ON PD10.DF_PRS_ID = LN10_CNT.BF_SSN
--CONSOL_CNT count of open, released, non-spousal consolidation loans in repayment status to which the overpayment may be applied (there is no need to ask how to apply the overpayment if there is only one loan) consolidation loans in Compass appear as two loans, a sub and unsub loan, hence the aggregation on disbursement date
	LEFT JOIN
	(
		SELECT  DISTINCT
			LN10.BF_SSN,
			COUNT(DISTINCT LN10.LD_LON_1_DSB) AS CNT
		FROM 
			UDW..LN10_LON LN10
			INNER JOIN UDW..DW01_DW_CLC_CLU DW01
				ON LN10.BF_SSN = DW01.BF_SSN
				AND LN10.LN_SEQ = DW01.LN_SEQ
		WHERE 
			LA_CUR_PRI > 0.00
			AND LC_STA_LON10 = 'R'
			AND DW01.WC_DW_LON_STA = '03' -- in repayment
			AND ISNULL(DW01.WX_OVR_DW_LON_STA, '') = ''
			AND LN10.IC_LON_PGM IN ('SUBCNS','UNCNS') --non-spousal consolidation loan
		GROUP BY
			LN10.BF_SSN
	) CONSOL_CNT
		ON PD10.DF_PRS_ID = CONSOL_CNT.BF_SSN
--SPOUSAL_CNT count of one open, released, spousal consolidation loans in repayment status to which the overpayment may be applied (there is no need to ask how to apply the overpayment if there is only one loan) consolidation loans in Compass appear as two loans, a sub and unsub loan, hence the aggregation on disbursement date
	LEFT JOIN
	(
		SELECT  DISTINCT
			LN10.BF_SSN,
			COUNT(DISTINCT LN10.LD_LON_1_DSB) AS CNT
		FROM 
			UDW..LN10_LON LN10
			INNER JOIN UDW..DW01_DW_CLC_CLU DW01
				ON LN10.BF_SSN = DW01.BF_SSN
				AND LN10.LN_SEQ = DW01.LN_SEQ
		WHERE 
			LA_CUR_PRI > 0.00
			AND LC_STA_LON10 = 'R'
			AND DW01.WC_DW_LON_STA = '03' -- in repayment
			AND ISNULL(DW01.WX_OVR_DW_LON_STA, '') = ''
			AND LN10.IC_LON_PGM IN ('SUBSPC','UNSPC') --spousal consolidation loan
		GROUP BY
			LN10.BF_SSN
	) SPOUSAL_CNT
		ON PD10.DF_PRS_ID = SPOUSAL_CNT.BF_SSN
--PAQ extract payment amount from open PA queue tasks left by UTNWF07
	LEFT JOIN
	(
		SELECT DISTINCT
			WQ20.BF_SSN,
			WQ20.WF_QUE,
			WQ20.WF_SUB_QUE,
			WQ20.WN_CTL_TSK,
			WQ20.PF_REQ_ACT,
			WQ20.WC_STA_WQUE20,
			WQ20.WX_MSG_2_TSK,
			CASE
				WHEN CHARINDEX('=', WQ20.WX_MSG_2_TSK, 0) = 0 OR CHARINDEX('; TYPE', WQ20.WX_MSG_2_TSK, 0) = 0
				THEN '0'
				ELSE REPLACE(REPLACE(ISNULL(SUBSTRING(WQ20.WX_MSG_2_TSK, CHARINDEX('=', WQ20.WX_MSG_2_TSK, 0) + 2, CHARINDEX('; TYPE', WQ20.WX_MSG_2_TSK, 0) - (CHARINDEX('=', WQ20.WX_MSG_2_TSK, 0) + 2) ), '0') , '$', '') , ',', '')
			END AS PAYMENT_AMOUNT --the amount is between the first '=' and '; TYPE'
		FROM 
			UDW..WQ20_TSK_QUE WQ20
		WHERE 
			WQ20.WF_QUE = 'PA'
			AND WQ20.WC_STA_WQUE20 NOT IN ('C','X')
	) PAQ
		ON PD10.DF_PRS_ID = PAQ.BF_SSN
		AND PMT.AMOUNT = CAST(PAQ.PAYMENT_AMOUNT AS NUMERIC(8,2))  --payment amount as calculated by UTNWF07 = amount in comment of open PA queue task
--TODO:	replaced by join to calc.emailaddress, remove
---- PD32 email address for highest priority email address type if the borrower has a PD32 email address
--	LEFT JOIN
--	( 
--		SELECT
--			Email.DF_PRS_ID,
--			Email.DX_ADR_EML,
--			ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) EmailPriority -- number in order of Email.PriorityNumber
--		FROM
--		(
--			SELECT
--				PD10.DF_PRS_ID,
--				PD32.DX_ADR_EML,
--				CASE
--					WHEN PD32.DC_ADR_EML = 'H' THEN 2 -- home
--					WHEN PD32.DC_ADR_EML = 'A' THEN 3 -- alternate
--					WHEN PD32.DC_ADR_EML = 'W' THEN 4 -- work
--				END PriorityNumber
--			FROM
--				UDW..PD10_PRS_NME PD10
--				LEFT JOIN UDW..PD32_PRS_ADR_EML PD32
--					ON PD10.DF_PRS_ID = PD32.DF_PRS_ID
--					AND PD32.DI_VLD_ADR_EML = 'Y' -- valid email address
--					AND PD32.DC_STA_PD32 = 'A' -- active email address record
--		) Email
--	) PD32
--		ON PD32.DF_PRS_ID = PD10.DF_PRS_ID
--		AND PD32.EmailPriority = 1
----PH05 email address if it exists
--	LEFT JOIN UDW..PH05_CNC_EML PH05
--		ON PD10.DF_SPE_ACC_ID = PH05.DF_SPE_ID
--		AND PH05.DI_VLD_CNC_EML_ADR = 'Y'
--If a borrower has a PMTAD arc left of the same day of the payment postdate then we can exclude the borrower.   This would mean that a PA adjustment is causing an overpayment.
	LEFT JOIN UDW..AY10_BR_LON_ATY PMTAD
		ON PD10.DF_PRS_ID = PMTAD.BF_SSN
		AND PMTAD.PF_REQ_ACT = 'PMTAD'
		AND PMTAD.LC_STA_ACTY10 = 'A'
		AND MAX_PMT.MAX_LD_FAT_PST = CAST(PMTAD.LD_ATY_REQ_RCV AS DATE)
--borrowers with an OVPRY ARC have already  provided instructions on how to apply the overpayments going forward and additional emails are not needed but a queue task to reapply the over payment is needed
	LEFT JOIN 
	(
		SELECT
			BF_SSN,
			MAX(CAST(LD_ATY_REQ_RCV AS DATE)) AS LD_ATY_REQ_RCV
		FROM
			UDW..AY10_BR_LON_ATY
		WHERE
			PF_REQ_ACT = 'OVPRY'
			AND LC_STA_ACTY10 = 'A'
		GROUP BY
			BF_SSN
	) OVPRY
		ON PD10.DF_PRS_ID = OVPRY.BF_SSN
--check for the existance of an active ARC to exclude borrowers to whom the email has already been sent
	LEFT JOIN
	(
		SELECT
			BF_SSN,
			CAST(MAX(LD_ATY_REQ_RCV) AS DATE) AS LD_ATY_REQ_RCV
		FROM
			UDW..AY10_BR_LON_ATY
		WHERE
			PF_REQ_ACT = 'OVPST'
			AND LC_STA_ACTY10 = 'A'
		GROUP BY
			BF_SSN
	) OVPST
		ON PD10.DF_PRS_ID = OVPST.BF_SSN
		AND OVPST.LD_ATY_REQ_RCV >= MAX_PMT.MAX_LD_FAT_PST
--check for the existance of an active SCSPP ARC to exclude borrowers who want the system to decide how to post overpayments
	LEFT JOIN
	(
		SELECT
			BF_SSN,
			CAST(MAX(LD_ATY_REQ_RCV) AS DATE) AS LD_ATY_REQ_RCV
		FROM
			UDW..AY10_BR_LON_ATY
		WHERE
			PF_REQ_ACT = 'SCSPP'
			AND LC_STA_ACTY10 = 'A'
		GROUP BY
			BF_SSN
	) SCSPP
		ON PD10.DF_PRS_ID = SCSPP.BF_SSN
		AND SCSPP.LD_ATY_REQ_RCV > ISNULL(OVPRY.LD_ATY_REQ_RCV,'1900-01-01')
 --TODO (FOR TESTING):  uncomment
WHERE
	PMT.AMOUNT > DUE.AMT_DUE --payment amount is more than the amount due --TODO (FOR TESTING): change equality operator back to >
	AND PAQ.BF_SSN IS NULL --exclude borrowers which already have an open PA queue task for the payment amount
	AND OVPST.BF_SSN IS NULL --exclude borrowers which already have an OVPST active email sent ARC after the payment
	AND PMTAD.BF_SSN IS NULL --If a borrower has a PMTAD arc left of the same day of the payment postdate then we can exclude the borrower.   This would mean that a PA adjustment is causing an overpayment.
	AND SCSPP.BF_SSN IS NULL --exclude borrowers who want the system to decide how to post overpayments
	--AND (PH05.DX_CNC_EML_ADR IS NOT NULL OR PD32.DX_ADR_EML IS NOT NULL) --the borrower has a valid email address TODO: replaced by join to UDW.calc.EmailAddress, remove
	AND ISNULL(LN10_CNT.CNT,0) + ISNULL(CONSOL_CNT.CNT,0) + ISNULL(SPOUSAL_CNT.CNT,0) > 1 -- borrower has more than one loan


--********************************************************************************************
--*                                                                                          *
--*                            END OF SELECT - BEGIN OF INSERTS                              *
--*                                                                                          *
--********************************************************************************************



--EMAILBATCH.EMAILPROCESSING
--insert records to send e-mail messages to borrowers who have NOT provided instructions on how to apply overpayments going forward
BEGIN TRY
	BEGIN TRANSACTION
	--TODO:  uncomment insert for production
	INSERT INTO ULS.emailbatch.EmailProcessing
		(
			EmailCampaignId,
			AccountNumber,
			ActualFile,
			EmailData,
			EmailSentAt,
			ArcNeeded,
			ArcAddProcessingId,
			ProcessingAttempts,
			AddedBy,
			AddedAt,
			DeletedBy,
			DeletedAt
		)
	SELECT DISTINCT
		@EmailCampaignId AS EmailCampaignId,
		NEW_DATA.AccountNumber,
		NULL AS Actualfile,
		NEW_DATA.emailData,
		NULL AS	EmailSentAt,
		1 AS ArcNeeded,
		NULL AS ArcProcessingId,
		0 AS ProcessingAttempts,
		@ScriptId AS AddedBy,
		@CurrentDateTime AS AddedAt,
		NULL AS DeletedBy,
		NULL AS DeletedAt
	FROM
		#OVR_PMT_UH NEW_DATA
	 --flag to exclude borrowers who already have a message queued to be sent
		LEFT JOIN ULS.emailbatch.EmailProcessing EXISTING_DATA
			ON EXISTING_DATA.EmailCampaignId = @EmailCampaignId
			AND 
				(
					(
						EXISTING_DATA.AccountNumber = NEW_DATA.AccountNumber --AccountNumber
						AND CONVERT(DATE, EXISTING_DATA.AddedAt) = CONVERT(DATE, @CurrentDateTime) --to remove anyone added today
					)
					OR EXISTING_DATA.EmailSentAt IS NULL --a message is already queued to be sent
				)
			AND EXISTING_DATA.DeletedBy IS NULL
	WHERE
		NEW_DATA.HasOVPRY = 0 --borrower has NOT already provided instructions on how to apply overpayments
		AND EXISTING_DATA.AccountNumber IS NULL --doesn't already have a message queued to be sent --TODO (FOR TESTING): uncomment

	COMMIT TRANSACTION;

END TRY
	--write message to process logger if an error occurs
	BEGIN CATCH
		DECLARE @EM VARCHAR(4000) = @ScriptId + ' encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

		ROLLBACK TRANSACTION;

		DECLARE @ProcessLogId INT;
		DECLARE @ProcessNotificationId INT;
		DECLARE @NotificationTypeId INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
		DECLARE @NotificationSeverityTypeId INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
		INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'uheaa',SUSER_SNAME());
		SET @ProcessLogId = SCOPE_IDENTITY()

		INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
		SET @ProcessNotificationId = SCOPE_IDENTITY()

		INSERT INTO ULS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;


--ARC ADD PROCESSING
--insert records to create queue tasks to reapply overpayments for borrowers who NAVE provided instructions on how to apply overpayments going forward
BEGIN TRY
	BEGIN TRANSACTION

	INSERT INTO 
		ULS..ArcAddProcessing
			(
				ArcTypeId,
				ArcResponseCodeId,
				AccountNumber,
				RecipientId,
				ARC,
				ActivityType,
				ActivityContact,
				ScriptId,
				ProcessOn,
				Comment,
				IsReference,
				IsEndorser,
				ProcessFrom,
				ProcessTo,
				NeededBy,
				RegardsTo,
				RegardsCode,
				LN_ATY_SEQ,
				ProcessingAttempts,
				CreatedAt,
				CreatedBy,
				ProcessedAt
			)
	SELECT
		1 ArcTypeId,
		NULL ArcResponseCodeId,
		NewData.AccountNumber,
		'' RecipientId,
		@ARC ARC,
		NULL ActivityType,
		NULL ActivityContact,
		@ScriptId ScriptId,
		@CurrentDateTime ProcessOn,
		NewData.Comment,
		0 IsReference,
		0 IsEndorser,
		NULL ProcessFrom,
		NULL ProcessTo,
		NULL NeededBy,
		NULL RegardsTo,
		NULL RegardsCode,
		NULL LN_ATY_SEQ,
		0 AS ProcessingAttempts, --this gets updated by the arc add script so it is initialized as 0 attempts made
		@CurrentDateTime CreatedAt,
		@ScriptId CreatedBy,
		NULL ProcessedAt
	FROM
		#OVR_PMT_UH NewData
		LEFT JOIN ULS..ArcAddProcessing ExistingData
				ON NewData.AccountNumber = ExistingData.AccountNumber
				AND ExistingData.ARC = @ARC
				AND 
					(
						(
							CAST(ExistingData.CreatedAt AS DATE) =  CAST(@CurrentDateTime AS DATE) --to remove anyone added today to prevent duplicates in recovery
							AND ExistingData.Comment = NewData.Comment
						)
						OR ExistingData.ProcessedAt IS NULL
					)
	WHERE
		NewData.HasOVPRY = 1 --borrower HAS already provided instructions on how to apply overpayments --TODO (FOR TESTING): change back to = 1
		AND ExistingData.AccountNumber IS NULL --exclude borrowers who already have an arcadd record --TODO (FOR TESTING): uncomment


		COMMIT TRANSACTION;

END TRY
	--write message to process logger if an error occurs
	BEGIN CATCH
		DECLARE @EM_ VARCHAR(4000) = @ScriptId + ' encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

		ROLLBACK TRANSACTION;

		DECLARE @ProcessLogId_ INT;
		DECLARE @ProcessNotificationId_ INT;
		DECLARE @NotificationTypeId_ INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
		DECLARE @NotificationSeverityTypeId_ INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
		INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(GETDATE(),GETDATE(),@ScriptId,'uheaa',SUSER_SNAME());
		SET @ProcessLogId_ = SCOPE_IDENTITY()

		INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId_,@NotificationSeverityTypeId_,@ProcessLogId_, NULL, NULL)
		SET @ProcessNotificationId_ = SCOPE_IDENTITY()

		INSERT INTO ULS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId_,@EM_);

	THROW;
END CATCH;

