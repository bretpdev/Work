USE UDW
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @TODAY DATE = GETDATE();
DECLARE @DAYSDQ1 TINYINT = 80;
DECLARE @DAYSDQ2 TINYINT = 90;
DECLARE @DAYSOCC TINYINT = 0;
--DECLARE @DAYSDQ1 TINYINT = 170;
--DECLARE @DAYSDQ2 TINYINT = 180;
--DECLARE @DAYSOCC TINYINT = 90;

;WITH BORROWERS AS
(
	SELECT DISTINCT 
		LN10.BF_SSN
		,PD10.DF_SPE_ACC_ID AS [Borrower Account Number]
		,LN16_MAX.[Days Delinquent]
		,PD30.DI_VLD_ADR AS [Address Validity]
		,ATTEMPT.LD_ATY_RSP AS [Date Last Attempt]
		,CONTACT.LD_ATY_RSP AS [Date Last Contact]
		--max case puts all values in all max cases on one line
		,MAX(CASE 
			WHEN PD42.DC_PHN = 'H'
			THEN PD42.DI_PHN_VLD
			ELSE NULL
		END) AS [Home Phone Validity]
		,MAX(CASE
			WHEN PD42.DC_PHN = 'A'
			THEN PD42.DI_PHN_VLD
			ELSE NULL
		END) AS [Alt Phone Validity]
		,MAX(CASE
			WHEN PD42.DC_PHN = 'W'
			THEN PD42.DI_PHN_VLD
			ELSE NULL
		END) AS [Work Phone Validity]
		,MAX(CASE
			WHEN PD42.DC_PHN = 'M'
			THEN PD42.DI_PHN_VLD
			ELSE NULL
		END) AS [Mobile Phone Validity]
	FROM
		UDW..LN10_LON LN10
		INNER JOIN UDW..LN16_LON_DLQ_HST LN16
			ON LN10.BF_SSN = LN16.BF_SSN
			AND LN10.LN_SEQ = LN16.LN_SEQ
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		INNER JOIN UDW..PD30_PRS_ADR PD30
			ON PD30.DF_PRS_ID = LN10.BF_SSN
		INNER JOIN UDW..PD42_PRS_PHN PD42
			ON PD42.DF_PRS_ID = LN10.BF_SSN
		INNER JOIN UDW..DW01_DW_CLC_CLU DW01
			ON LN10.BF_SSN = DW01.BF_SSN
		INNER JOIN 
		(--accounts in repayment
			SELECT 
				BF_SSN 
				,LN_SEQ
				,MAX(LD_DLQ_OCC) AS LD_DLQ_OCC 
				,(LN_DLQ_MAX + 1) AS [Days Delinquent]  
				,LN_DLQ_ITL
			FROM
				UDW..LN16_LON_DLQ_HST 
			WHERE
				LC_STA_LON16 = '1'
				AND (LN_DLQ_MAX + 1) BETWEEN @DAYSDQ1 AND @DAYSDQ2
			GROUP BY 
				BF_SSN
				,LN_SEQ
				,LN_DLQ_ITL
				,LN_DLQ_MAX
		) LN16_MAX
			ON LN10.BF_SSN = LN16_MAX.BF_SSN
			AND LN10.LN_SEQ = LN16_MAX.LN_SEQ
		INNER JOIN
		(--get accts with "no contact/invalid phone" arc and "contact" arc not on acct within last 30 days
			SELECT
				NOCONTACT_INVALIDPH.BF_SSN
			FROM
				(--get no contact/invalid phone between delq dates and <2 instances
					SELECT
						AY10.BF_SSN
						--,COUNT(AY10.BF_SSN) AS BORRCOUNT
					FROM 
						UDW..AY10_BR_LON_ATY AY10
						INNER JOIN UDW..AC10_ACT_REQ AC10
							ON AY10.PF_REQ_ACT = AC10.PF_REQ_ACT
						INNER JOIN UDW..LN10_LON LN10
							ON AY10.BF_SSN = LN10.BF_SSN
						INNER JOIN UDW..LN16_LON_DLQ_HST LN16
							ON LN10.BF_SSN = LN16.BF_SSN
							AND LN10.LN_SEQ = LN16.LN_SEQ
					WHERE 
						AY10.PF_RSP_ACT IN ('NOCTC','INVPH') --no contact/invalid phone
						AND AC10.PC_DD_ACT_STA = 'D' --delinquent
						AND AY10.LD_ATY_RSP BETWEEN (LN16.LD_DLQ_OCC + @DAYSOCC) AND (LN16.LD_DLQ_MAX + 1)
						AND LN16.LC_STA_LON16 = '1'
						AND LN10.LC_STA_LON10 = 'R'
						AND LN10.LA_CUR_PRI > 0.00
						AND LN10.LI_CON_PAY_STP_PUR <> 'Y' --exclude CNSLD-STOP-PURSUIT
					GROUP BY
						AY10.BF_SSN
					HAVING 
						COUNT(AY10.BF_SSN) < 2 --less than 2 instances

					UNION --get no contact/invalid phone outside delq dates
					SELECT 
						FINDMAX.BF_SSN
					FROM
					(
						SELECT
							AY10.BF_SSN
							,MAX(AY10.LD_ATY_RSP) OVER (PARTITION BY AY10.BF_SSN) AS MAX_LD_ATY_RSP
							,LN16.LD_DLQ_OCC
							,LN16.LD_DLQ_MAX
						FROM 
							UDW..AY10_BR_LON_ATY AY10
							INNER JOIN UDW..AC10_ACT_REQ AC10
								ON AY10.PF_REQ_ACT = AC10.PF_REQ_ACT
							INNER JOIN UDW..LN10_LON LN10
								ON AY10.BF_SSN = LN10.BF_SSN
							INNER JOIN UDW..LN16_LON_DLQ_HST LN16
								ON LN10.BF_SSN = LN16.BF_SSN
								AND LN10.LN_SEQ = LN16.LN_SEQ
						WHERE 
							AY10.PF_RSP_ACT IN ('NOCTC','INVPH') --no contact/invalid phone
							AND AC10.PC_DD_ACT_STA = 'D' --delinquent
							AND LN16.LC_STA_LON16 = '1'
							AND LN10.LC_STA_LON10 = 'R'
							AND LN10.LA_CUR_PRI > 0.00
							AND LN10.LI_CON_PAY_STP_PUR <> 'Y' --exclude CNSLD-STOP-PURSUIT
					) FINDMAX
					WHERE 
						FINDMAX.MAX_LD_ATY_RSP NOT BETWEEN CAST(DATEADD(DAY,@DAYSOCC,LD_DLQ_OCC) AS DATE) AND CAST(DATEADD(DAY,1,LD_DLQ_MAX) AS DATE)
				) NOCONTACT_INVALIDPH
				LEFT JOIN 
				(--flag for exclusion: "contact" arc left between delq dates
					SELECT
						AY10.BF_SSN
					FROM 
						UDW..AY10_BR_LON_ATY AY10
						INNER JOIN UDW..AC10_ACT_REQ AC10
							ON AY10.PF_REQ_ACT = AC10.PF_REQ_ACT
						INNER JOIN UDW..LN10_LON LN10
							ON AY10.BF_SSN = LN10.BF_SSN
						INNER JOIN UDW..LN16_LON_DLQ_HST LN16
							ON LN10.BF_SSN = LN16.BF_SSN
							AND LN10.LN_SEQ = LN16.LN_SEQ
					WHERE 
						AY10.PF_RSP_ACT IN ('CNTCT') --contact
						AND AC10.PC_DD_ACT_STA = 'D' --delinquent
						AND AY10.LD_ATY_RSP BETWEEN (LN16.LD_DLQ_OCC + @DAYSOCC) AND (LN16.LD_DLQ_MAX + 1)
						AND LN16.LC_STA_LON16 = '1'	
						AND LN10.LC_STA_LON10 = 'R'
						AND LN10.LA_CUR_PRI > 0.00
						AND LN10.LI_CON_PAY_STP_PUR <> 'Y' --exclude CNSLD-STOP-PURSUIT					
				) CONTACT
					ON NOCONTACT_INVALIDPH.BF_SSN = CONTACT.BF_SSN
			WHERE
				CONTACT.BF_SSN IS NULL --excludes
		) NOCONTACT_INVALIDPH
			ON NOCONTACT_INVALIDPH.BF_SSN = LN10.BF_SSN
		LEFT JOIN
		(--get last attempt
			SELECT
				AY10.BF_SSN
				,MAX(LD_ATY_RSP) AS LD_ATY_RSP
			FROM 
				UDW..AY10_BR_LON_ATY AY10
				INNER JOIN UDW..AC10_ACT_REQ AC10
					ON AY10.PF_REQ_ACT = AC10.PF_REQ_ACT	
			WHERE 
				AY10.PF_RSP_ACT IN ('CNTCT','NOCTC','INVPH') --contact/no contact/invalid phone
				AND AC10.PC_DD_ACT_STA = 'D' --delinquent
			GROUP BY
				AY10.BF_SSN
		) ATTEMPT
			ON ATTEMPT.BF_SSN = LN10.BF_SSN
		LEFT JOIN
		(--get last contact
			SELECT
				AY10.BF_SSN
				,MAX(LD_ATY_RSP) AS LD_ATY_RSP
			FROM 
				UDW..AY10_BR_LON_ATY AY10
				INNER JOIN UDW..AC10_ACT_REQ AC10
					ON AY10.PF_REQ_ACT = AC10.PF_REQ_ACT
			WHERE 
				AY10.PF_RSP_ACT IN ('CNTCT') --contact
				AND AC10.PC_DD_ACT_STA = 'D' --delinquent
			GROUP BY
				AY10.BF_SSN
		) CONTACT
			ON CONTACT.BF_SSN = LN10.BF_SSN
		LEFT JOIN
		(--flag for exclusion: paid ahead
			SELECT 
				BL10.BF_SSN
			FROM
				(--get most recent bill
					SELECT
						BF_SSN
						,MAX(LD_BIL_CRT) AS LD_BIL_CRT
					FROM
						UDW..BL10_BR_BIL
					WHERE
						LC_STA_BIL10 = 'A'
						AND LC_IND_BIL_SNT = '5'
					GROUP BY
						BF_SSN
				) BL10
				INNER JOIN UDW..LN80_LON_BIL_CRF LN80
					ON BL10.BF_SSN = LN80.BF_SSN
					AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
			WHERE
				LN80.LD_BIL_DU_LON > @TODAY
		) PAIDAHEAD
			ON PAIDAHEAD.BF_SSN = LN10.BF_SSN
		LEFT JOIN
		(--flag for exclusion: all valid ph#'s have validity indicator < 20 days
			SELECT
				DF_PRS_ID
			FROM
				UDW..PD42_PRS_PHN
			WHERE
				DI_PHN_VLD = 'Y' --valid #'s only
				AND DATEDIFF(DAY, DD_PHN_VER, @TODAY) < 20
		) PD42_20DAYS
			ON PD42_20DAYS.DF_PRS_ID = LN10.BF_SSN
		LEFT JOIN
		(--flag for exclusion: deferment
			SELECT 
				LN50.BF_SSN
			FROM
				UDW..DF10_BR_DFR_REQ DF10
				INNER JOIN UDW..LN50_BR_DFR_APV LN50
					ON DF10.BF_SSN = LN50.BF_SSN
					AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
			WHERE
				LN50.LC_STA_LON50 = 'A'
				AND LN50.LD_DFR_END > @TODAY
				AND DF10.LC_DFR_STA = 'A'
				AND DF10.LC_STA_DFR10 = 'A'
		) LN50X
			ON LN50X.BF_SSN = LN10.BF_SSN
		LEFT JOIN
		(--flag for exclusion: forbearance
			SELECT 
				LN60.BF_SSN
			FROM
				UDW..FB10_BR_FOR_REQ FB10
				INNER JOIN UDW..LN60_BR_FOR_APV LN60
					ON FB10.BF_SSN = LN60.BF_SSN
					AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM 
			WHERE
				LN60.LC_STA_LON60 = 'A'
				AND LN60.LD_FOR_END > @TODAY
				AND FB10.LC_FOR_STA = 'A'
				AND FB10.LC_STA_FOR10 = 'A'
		) LN60X
			ON LN60X.BF_SSN = LN10.BF_SSN
		LEFT JOIN 
		(--flag for exclusion: bankruptcy within past 10 days
			SELECT 
				PD24.DF_PRS_ID
			FROM
				UDW..PD24_PRS_BKR PD24
			WHERE
				PD24.DC_BKR_DCH_NDC = 'D'
				AND PD24.DC_BKR_STA = '05'
				AND PD24.DD_BKR_STA >= DATEADD(DAY, -10, @TODAY)
		) PD24X
			ON PD24X.DF_PRS_ID = LN10.BF_SSN
	WHERE
		PAIDAHEAD.BF_SSN IS NULL --excludes
		AND PD42_20DAYS.DF_PRS_ID IS NULL --excludes
		AND LN50X.BF_SSN IS NULL --excludes
		AND LN60X.BF_SSN IS NULL --excludes
		AND PD24X.DF_PRS_ID IS NULL --excludes
		AND LN10.LC_STA_LON10 = 'R'
		AND LN10.LA_CUR_PRI > 0.00
		AND LN10.LI_CON_PAY_STP_PUR <> 'Y' --exclude CNSLD-STOP-PURSUIT
		AND LN16.LC_STA_LON16 = '1'
		AND DW01.WC_DW_LON_STA NOT IN ('01','02','04','05','06','12','17','19','21','22','23','88','98')
		AND PD42.DC_PHN IN ('H','A','W','M') --valid #'s only
		AND PD42.DI_PHN_VLD = 'Y' --valid #'s only
		AND PD42.DN_DOM_PHN_ARA + PD42.DN_DOM_PHN_XCH + PD42.DN_DOM_PHN_LCL <> '' --exclude blank ph#'s
		AND PD30.DC_ADR = 'L'
	GROUP BY
		LN10.BF_SSN
		,PD10.DF_SPE_ACC_ID 
		,LN16_MAX.[Days Delinquent]
		,PD30.DI_VLD_ADR
		,ATTEMPT.LD_ATY_RSP 
		,CONTACT.LD_ATY_RSP 
)
,COBORROWERS AS
(
	SELECT DISTINCT
		LN20.LF_EDS AS BF_SSN
		,PD10.DF_SPE_ACC_ID AS [Borrower Account Number]
		,BORROWERS.[Days Delinquent]
		,PD30.DI_VLD_ADR AS [Address Validity]
		,BORROWERS.[Date Last Attempt]
		,BORROWERS.[Date Last Contact]
		--max case puts all values in all max cases on one line
		,MAX(CASE 
			WHEN PD42.DC_PHN = 'H'
			THEN PD42.DI_PHN_VLD
			ELSE NULL
		END) AS [Home Phone Validity]
		,MAX(CASE
			WHEN PD42.DC_PHN = 'A'
			THEN PD42.DI_PHN_VLD
			ELSE NULL
		END) AS [Alt Phone Validity]
		,MAX(CASE
			WHEN PD42.DC_PHN = 'W'
			THEN PD42.DI_PHN_VLD
			ELSE NULL
		END) AS [Work Phone Validity]
		,MAX(CASE
			WHEN PD42.DC_PHN = 'M'
			THEN PD42.DI_PHN_VLD
			ELSE NULL
		END) AS [Mobile Phone Validity]
	FROM 
		BORROWERS --limits pop to borrowers from previous CTE
		INNER JOIN UDW..LN20_EDS LN20
			ON BORROWERS.BF_SSN = LN20.BF_SSN --matches borrowers to endorsers
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = LN20.LF_EDS --endorser info
		INNER JOIN UDW..PD30_PRS_ADR PD30
			ON PD30.DF_PRS_ID = LN20.LF_EDS --endorser info
		INNER JOIN UDW..PD42_PRS_PHN PD42
			ON PD42.DF_PRS_ID = LN20.LF_EDS --endorser info
		LEFT JOIN
		(--flag for exclusion: all valid ph#'s have validity indicator < 20 days
			SELECT
				DF_PRS_ID
			FROM
				UDW..PD42_PRS_PHN
			WHERE
				DI_PHN_VLD = 'Y' --valid #'s only
				AND DATEDIFF(DAY, DD_PHN_VER, @TODAY) < 20
		) PD42_20DAYS
			ON PD42_20DAYS.DF_PRS_ID = LN20.LF_EDS --endorser info
	WHERE 
		PD42_20DAYS.DF_PRS_ID IS NULL --excludes
		AND LN20.LC_STA_LON20 = 'A'
		AND LN20.LC_EDS_TYP = 'M'
		AND PD30.DC_ADR = 'L'
		AND PD42.DC_PHN IN ('H','A','W','M') --valid #'s only
		AND PD42.DI_PHN_VLD = 'Y' --valid #'s only
		AND PD42.DN_DOM_PHN_ARA + PD42.DN_DOM_PHN_XCH + PD42.DN_DOM_PHN_LCL <> '' --exclude blank ph#'s
	GROUP BY
		LN20.LF_EDS
		,PD10.DF_SPE_ACC_ID 
		,PD30.DI_VLD_ADR
		,BORROWERS.[Days Delinquent]
		,BORROWERS.[Date Last Attempt]
		,BORROWERS.[Date Last Contact]
)
SELECT 
	* 
FROM 
	BORROWERS

UNION

SELECT 
	* 
FROM 
	COBORROWERS
;