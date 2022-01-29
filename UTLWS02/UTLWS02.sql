USE [CentralData]
GO
/****** Object:  StoredProcedure [dbo].[SsrsUTLWS02]    Script Date: 6/17/2021 7:44:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE [dbo].[SsrsUTLWS02_AutoDelq]
GO
DROP PROCEDURE [dbo].[SsrsUTLWS02_Manual]
GO

CREATE PROCEDURE [dbo].[SsrsUTLWS02_AutoDelq_Low]
	
AS
--R4
--Delq
SELECT
	RecipientAccountNumber,
	Delinquency,
	RecipientDescription,
	BorrowerAccountNumber,
	TCPAConsentForHighestRankedPhone,
	HighestRankedPhone,
	TCPAConsentForSecondHighestRankedPhone,
	SecondHighestPhone,
	TCPAConsentForThirdHighestRankedPhone,
	ThirdHighestPhone,
	[State],
	ZIP,
	AmountDue,
	TotalBalance,
	BorrowerAccountNumber2,
	Portfolio,
	TotalForbearanceTime,
	TotalEconomicHardshipDefer,
	TotalUnemploymentDefer,
	RecipientName,
	BorrowerName,
	DateOfLastCall
FROM
(
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID AS RecipientAccountNumber,
		MAX(LN16.LN_DLQ_MAX) OVER(PARTITION BY LN10.BF_SSN) + 1 AS Delinquency,
		'BORROWER' AS RecipientDescription,
		PD10.DF_SPE_ACC_ID AS BorrowerAccountNumber,
		PD42.PHN_H_CON AS TCPAConsentForHighestRankedPhone,
		PD42.PHN_H AS HighestRankedPhone,
		PD42.PHN_A_CON AS TCPAConsentForSecondHighestRankedPhone,
		PD42.PHN_A AS SecondHighestPhone,
		PD42.PHN_W_CON AS TCPAConsentForThirdHighestRankedPhone,
		PD42.PHN_W AS ThirdHighestPhone,
		PD30.DC_DOM_ST AS [State],
		LEFT(ISNULL(PD30.DF_ZIP_CDE, '00000'),5) AS ZIP,
		ISNULL(AmountDue.AMTDU,0.00) AS AmountDue, --Dialer requires no decimals
		SUM(ISNULL(LN10.LA_CUR_PRI,0.00) + ISNULL(DW01.WA_TOT_BRI_OTS,0.00) + ISNULL(AmountDue.LA_LTE_FEE_OTS_PRT,0.00)) OVER(PARTITION BY PD10.DF_SPE_ACC_ID) AS TotalBalance, --Dialer requires no decimals
		PD10.DF_SPE_ACC_ID AS BorrowerAccountNumber2,
		CASE WHEN LN10.LF_LON_CUR_OWN = '900749' THEN 'CLP'
			 WHEN LN10.LF_LON_CUR_OWN = '971357' THEN 'TILP'
			 WHEN LN10.LF_LON_CUR_OWN LIKE '829769%' THEN 'BANA'
			 WHEN LN10.LF_LON_CUR_OWN IN('826717','830248') THEN 'ALIGN'
			 WHEN LN10.LF_LON_CUR_OWN = '828476' THEN 'UTAH'
			 ELSE LN10.LF_LON_CUR_OWN 
		END AS Portfolio,
		ISNULL(MAX(HardForb.TotalFrbMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalForbearanceTime, 
		ISNULL(MAX(EconDefer.TotalDfrMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalEconomicHardshipDefer, 
		ISNULL(MAX(UnempDefer.TotalDfrMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalUnemploymentDefer, 
		RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_MID) + ' ' + RTRIM(PD10.DM_PRS_LST) AS [RecipientName],
		RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_MID) + ' ' + RTRIM(PD10.DM_PRS_LST) AS [BorrowerName],
		ISNULL(LastCallDate.DateOfLastCall, '1900-01-01') AS DateOfLastCall
	FROM 
		UDW..LN10_LON LN10
		INNER JOIN UDW..LN16_LON_DLQ_HST LN16
			ON LN16.BF_SSN = LN10.BF_SSN
			AND LN16.LN_SEQ = LN10.LN_SEQ
			AND LN16.LC_STA_LON16 = '1'
			AND DATEDIFF(DAY, LN16.LD_DLQ_MAX, GETDATE()) <= 5
			AND DATEDIFF(DAY, LN16.LD_DLQ_OCC, GETDATE()) > 10
			AND (LN16.LN_DLQ_MAX + 1) % 2 = 1 --Gets odd delq 
			AND (LN16.LN_DLQ_MAX + 1) BETWEEN 15 AND 59 --Low Delq
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = LN10.BF_SSN
		INNER JOIN 
		(
			SELECT DISTINCT
				DF_PRS_ID,
				MAX(IIF(OverallRank = 1, PHN, '')) AS PHN_H,
				MAX(IIF(OverallRank = 1, DC_ALW_ADL_PHN, '')) AS PHN_H_CON,
				MAX(IIF(OverallRank = 2, PHN, '')) AS PHN_A,
				MAX(IIF(OverallRank = 2, DC_ALW_ADL_PHN, '')) AS PHN_A_CON,
				MAX(IIF(OverallRank = 3, PHN, '')) AS PHN_W,
				MAX(IIF(OverallRank = 3, DC_ALW_ADL_PHN, '')) AS PHN_W_CON
			FROM
			(
				SELECT DISTINCT
					PD42.DF_PRS_ID,
					ISNULL(PD42.DN_DOM_PHN_ARA,'') + ISNULL(PD42.DN_DOM_PHN_XCH,'') + ISNULL(PD42.DN_DOM_PHN_LCL,'') AS PHN,
					PD42.DC_PHN,
					CASE WHEN PD42.DC_ALW_ADL_PHN IN ('L','X','P') 
						THEN 'Y'
						ELSE 'N'
					END AS DC_ALW_ADL_PHN,
					PD42.DC_ALW_ADL_PHN AS Consent,
					/*Rank phones by category and type*/
					DENSE_RANK() OVER (PARTITION BY PD42.DF_PRS_ID ORDER BY Ranks.ConsentRank, Ranks.TypeRank) AS OverallRank
				FROM
					UDW..PD42_PRS_PHN PD42
					INNER JOIN
					(
						SELECT
							DF_PRS_ID,
							DC_PHN,
							DC_ALW_ADL_PHN,
							CASE WHEN DC_PHN = 'H' THEN 1
									WHEN DC_PHN = 'A' THEN 2
									WHEN DC_PHN = 'W' THEN 3
									WHEN DC_PHN = 'M' THEN 4
							END AS TypeRank, --H A W M TYPE
							CASE WHEN DC_ALW_ADL_PHN = 'N' THEN 1
									WHEN DC_ALW_ADL_PHN = 'P' THEN 2 
									WHEN DC_ALW_ADL_PHN = 'X' THEN 3
									WHEN DC_ALW_ADL_PHN = 'L' THEN 4
									WHEN DC_ALW_ADL_PHN = 'Q' THEN 5
									WHEN DC_ALW_ADL_PHN = 'U' THEN 6 
							END AS ConsentRank--N P X L Q U CONSENT
						FROM
							UDW..PD42_PRS_PHN PD42
						WHERE
							PD42.DI_PHN_VLD = 'Y'
							AND PD42.DC_NO_HME_PHN != 'J'
							AND PD42.DC_ALW_ADL_PHN IN ('L','X','P') --Consent
					) Ranks
						ON Ranks.DF_PRS_ID = PD42.DF_PRS_ID
						AND Ranks.DC_PHN = PD42.DC_PHN
						AND Ranks.DC_ALW_ADL_PHN = PD42.DC_ALW_ADL_PHN
				WHERE
					PD42.DI_PHN_VLD = 'Y'
					AND PD42.DC_NO_HME_PHN != 'J'
					AND PD42.DC_ALW_ADL_PHN IN ('L','X','P') --Consent
			) Ranked
			WHERE
				Ranked.OverallRank IN (1,2,3)
			GROUP BY
				DF_PRS_ID
		) PD42
			ON PD42.DF_PRS_ID = LN10.BF_SSN
			AND 
			(
				PD42.PHN_H != ''
				OR PD42.PHN_A != ''
				OR PD42.PHN_W != ''
			)
		INNER JOIN UDW..DW01_DW_CLC_CLU DW01
			ON DW01.BF_SSN = LN10.BF_SSN
			AND DW01.LN_SEQ = LN10.LN_SEQ
			AND DW01.WC_DW_LON_STA != '23'
			AND DW01.WC_DW_LON_STA IN ('03','04','05','08','14')
			AND RTRIM(DW01.WX_OVR_DW_LON_STA) NOT IN('PENDING ID THEFT','REALLOCATED','CNSLD-STOP PURSUIT')
		LEFT JOIN UDW..PD30_PRS_ADR PD30
			ON PD30.DF_PRS_ID = LN10.BF_SSN
			AND PD30.DC_ADR = 'L'
		LEFT JOIN UDW..WQ20_TSK_QUE WQ20
			ON WQ20.BF_SSN = LN10.BF_SSN
			AND WQ20.WF_QUE IN ('2A', '2P', 'GP', '9R', 'SI', 'S4', 'PR', 'VR', 'VB', 'MF', '1P', 'SF', 'DU', 'AT', '87')
			AND WQ20.WC_STA_WQUE20 IN ('A', 'H', 'P', 'U', 'W')
		LEFT JOIN ODW..CT30_CALL_QUE CT30
			ON CT30.DF_PRS_ID_BR = LN10.BF_SSN
			AND CT30.IF_WRK_GRP IN ('ASBKP', 'ASCON', 'ASMOC')
			AND CT30.IF_WRK_GRP IN ('A', 'W')	
		LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY DOC ID*/
		(
			SELECT
				BF_SSN,
				MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
			FROM
				UDW..AY10_BR_LON_ATY
			WHERE
				PF_REQ_ACT = 'DIDDA'
				AND DATEDIFF(DAY, LD_ATY_REQ_RCV, GETDATE()) <= 14
				AND LC_STA_ACTY10 = 'A'
			GROUP BY
				BF_SSN
		)DID 
			ON DID.BF_SSN = LN10.BF_SSN
		LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY APPROVAL*/
		(
			SELECT
				BF_SSN,
				MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
			FROM
				UDW..AY10_BR_LON_ATY
			WHERE
				PF_REQ_ACT = 'PAUTO'
				AND LC_STA_ACTY10 = 'A'
			GROUP BY
				BF_SSN
		) AUTO 
			ON AUTO.BF_SSN = LN10.BF_SSN
		LEFT JOIN /*Gets amount due from billing tables*/
		(
			SELECT 
				LN80.BF_SSN,
				SUM(ISNULL(LN80.LA_BIL_PAS_DU,0.00) + ISNULL(LN80.LA_BIL_DU_PRT,0.00) + ISNULL(LN80.LA_LTE_FEE_OTS_PRT,0.00) - ISNULL(LN80.LA_TOT_BIL_STS,0.00)) AS AMTDU,
				SUM(ISNULL(LN80.LA_LTE_FEE_OTS_PRT,0.00)) AS LA_LTE_FEE_OTS_PRT
			FROM 
				UDW..LN80_LON_BIL_CRF LN80
				INNER JOIN 
				(
					SELECT 
						BF_SSN,
						MAX(LD_BIL_CRT) AS LD_BIL_CRT
					FROM 
						UDW..BL10_BR_BIL
					WHERE 
						LC_STA_BIL10 = 'A'
						AND LC_BIL_TYP = 'P'
					GROUP BY 
						BF_SSN
				) BL10
					ON BL10.BF_SSN = LN80.BF_SSN
					AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
			WHERE 
				LN80.LC_STA_LON80 = 'A'
			GROUP BY 
				LN80.BF_SSN
		) AmountDue 
			ON AmountDue.BF_SSN = LN10.BF_SSN
		LEFT JOIN 
		(/*exclude future arc date*/
			SELECT DISTINCT
				BF_SSN,
				LD_ATY_REQ_RCV,
				PARSED_DAYS,
				PARSED_DAYS AS COMMENT_DAYS, /*takes lesser of 14 or parsed days*/
				DATEADD(DAY,PARSED_DAYS, LD_ATY_REQ_RCV) AS CALC_DATE,	
				CASE WHEN CAST(DATEADD(DAY,PARSED_DAYS, LD_ATY_REQ_RCV) AS DATE) > CAST(GETDATE() AS DATE)
					THEN 1
					ELSE 0
				END AS FUTURE_DATE
			FROM
			(/*parse out numeric days from any extraneous text*/
				SELECT 
					AY10.BF_SSN,
					AY10.LD_ATY_REQ_RCV,
					AY20.LX_ATY,
					IIF(COALESCE(TRY_CAST(LTRIM(RTRIM(AY20.LX_ATY)) AS INT),14) >= 14, 14, COALESCE(TRY_CAST(LTRIM(RTRIM(AY20.LX_ATY)) AS INT),14)) AS PARSED_DAYS	
				FROM
					UDW..AY10_BR_LON_ATY AY10
					LEFT JOIN UDW..AY20_ATY_TXT AY20
						ON AY20.BF_SSN = AY10.BF_SSN
						AND AY20.LN_ATY_SEQ = AY10.LN_ATY_SEQ
				WHERE
					AY10.LC_STA_ACTY10 = 'A'
					AND AY10.PF_REQ_ACT = 'BRPTP' /*borrower promise to repay*/
			) P
		) ARC_BRPTP 
			ON ARC_BRPTP.BF_SSN = LN10.BF_SSN
			AND ARC_BRPTP.FUTURE_DATE = 1 /*exclude future arc date*/
		LEFT JOIN
		( -- Deferment months already used
			SELECT DISTINCT
				LN50.BF_SSN,
				LN50.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN50.LD_DFR_BEG, DATEADD(DAY,1,LN50.LD_DFR_END))/365.00)*12) OVER (PARTITION BY LN50.BF_SSN, LN50.LN_SEQ), 1) AS [TotalDfrMonthsUsed]
			FROM
				UDW..DF10_BR_DFR_REQ DF10
				INNER JOIN UDW..LN50_BR_DFR_APV LN50 
					ON LN50.BF_SSN = DF10.BF_SSN 
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
			WHERE
				DF10.LC_DFR_STA = 'A'
				AND DF10.LC_STA_DFR10 = 'A'
				AND DF10.LC_DFR_TYP = 'D29'
				AND LN50.LC_STA_LON50 = 'A'
				AND LN50.LC_DFR_RSP != '003' -- 003 = DEFERMENT REQUEST DENIED
				AND LN50.LD_DFR_BEG <= GETDATE()
		) EconDefer
			ON EconDefer.BF_SSN = LN10.BF_SSN 
			AND EconDefer.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN
		( -- Deferment months already used
			SELECT DISTINCT
				LN50.BF_SSN,
				LN50.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN50.LD_DFR_BEG, DATEADD(DAY,1,LN50.LD_DFR_END))/365.00)*12) OVER (PARTITION BY LN50.BF_SSN, LN50.LN_SEQ), 1) AS [TotalDfrMonthsUsed]
			FROM
				UDW..DF10_BR_DFR_REQ DF10
				INNER JOIN UDW..LN50_BR_DFR_APV LN50 
					ON LN50.BF_SSN = DF10.BF_SSN 
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
			WHERE
				DF10.LC_DFR_STA = 'A'
				AND DF10.LC_STA_DFR10 = 'A'
				AND DF10.LC_DFR_TYP = 'D13'
				AND LN50.LC_STA_LON50 = 'A'
				AND LN50.LC_DFR_RSP != '003' -- 003 = DEFERMENT REQUEST DENIED
				AND LN50.LD_DFR_BEG <= GETDATE()
		) UnempDefer
			ON UnempDefer.BF_SSN = LN10.BF_SSN 
			AND UnempDefer.LN_SEQ = LN10.LN_SEQ 
		LEFT JOIN
		( -- Forb months already used
			SELECT DISTINCT
				LN60.BF_SSN,
				LN60.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN60.LD_FOR_BEG, DATEADD(DAY,1,LN60.LD_FOR_END))/365.00)*12) OVER (PARTITION BY LN60.BF_SSN, LN60.LN_SEQ), 1) AS [TotalFrbMonthsUsed]
			FROM
				UDW..FB10_BR_FOR_REQ FB10
				INNER JOIN UDW..LN60_BR_FOR_APV LN60 
					ON LN60.BF_SSN = FB10.BF_SSN
					AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
			WHERE
				FB10.LC_FOR_STA = 'A'
				AND FB10.LC_STA_FOR10 = 'A'
				AND FB10.LC_FOR_TYP = '05'
				AND LN60.LC_STA_LON60 = 'A'
				AND LN60.LC_FOR_RSP != '003' --003 = FORBEARANCE REQUEST DENIED
				AND LN60.LD_FOR_BEG <= GETDATE()
		) HardForb
			ON HardForb.BF_SSN = LN10.BF_SSN
			AND HardForb.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN
		(
			SELECT 
				NCH.AccountIdentifier, 
				NCH.ActivityDate 
			FROM 
				NobleCalls..NobleCallHistory NCH
				INNER JOIN NobleCalls..DispositionCodeMapping DCM
					ON DCM.DispositionCode = NCH.DispositionCode
				INNER JOIN NobleCalls..ResponseCodes RC
					ON RC.ResponseCodeId = DCM.ResponseCodeId
					AND RC.ResponseCode = 'CNTCT'
			WHERE
				CAST(NCH.ActivityDate AS DATE) BETWEEN CAST(DATEADD(DAY,-7,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE) --last 7 days
		) ContactLastSeven
			ON ContactLastSeven.AccountIdentifier = PD10.DF_SPE_ACC_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				NCH.AccountIdentifier, 
				COUNT(*) AS NumberAttempts
			FROM 
				NobleCalls..NobleCallHistory NCH
				INNER JOIN NobleCalls..DispositionCodeMapping DCM
					ON DCM.DispositionCode = NCH.DispositionCode
				INNER JOIN NobleCalls..ResponseCodes RC
					ON RC.ResponseCodeId = DCM.ResponseCodeId
					AND RC.ResponseCode IN('INVPH', 'NOCTC')
			WHERE
				CAST(NCH.ActivityDate AS DATE) BETWEEN CAST(DATEADD(DAY,-7,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE) --last 7 days
			GROUP BY
				NCH.AccountIdentifier
		) OneAttemptLastSeven
			ON OneAttemptLastSeven.AccountIdentifier = PD10.DF_SPE_ACC_ID
			AND OneAttemptLastSeven.NumberAttempts >= 1
		LEFT JOIN 
		(
			SELECT
				NCH.AccountIdentifier,
				MAX(NCH.ActivityDate) AS DateOfLastCall
			FROM
				NobleCalls..NobleCallHistory NCH
			GROUP BY
				NCH.AccountIdentifier
		) LastCallDate
			ON LastCallDate.AccountIdentifier = PD10.DF_SPE_ACC_ID
	WHERE
		LN10.LC_STA_LON10 = 'R'
		AND ISNULL(LN10.LA_CUR_PRI,0.00) + ISNULL(DW01.WA_TOT_BRI_OTS,0.00) > 0.00
		AND LN10.LI_CON_PAY_STP_PUR != 'Y'
		AND ISNULL(DID.LD_ATY_REQ_RCV,'2099-01-01') > ISNULL(AUTO.LD_ATY_REQ_RCV,'1900-01-01')
		AND ARC_BRPTP.BF_SSN IS NULL /*exclude future arc date*/
		AND WQ20.BF_SSN IS NULL /*exclude pending queue*/
		AND CT30.DF_PRS_ID_BR IS NULL /*exclude pending one link task*/
		AND ContactLastSeven.AccountIdentifier IS NULL /*exclude successful contacts in last week*/
		AND OneAttemptLastSeven.AccountIdentifier IS NULL /*exclude 3 failed contacts in last week*/
	 
	UNION ALL

	/*	begin DELQ_EDR*/
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID AS RecipientAccountNumber,
		MAX(LN16.LN_DLQ_MAX) OVER(PARTITION BY LN10.BF_SSN) + 1 AS Delinquency,
		CASE WHEN LC_EDS_TYP = 'M' THEN 'COBORROWER'
			 WHEN LC_EDS_TYP = 'S' THEN 'ENDORSER'
			 ELSE ''
		END AS RecipientDescription,
		PD10Borr.DF_SPE_ACC_ID AS BorrowerAccountNumber,
		PD42.PHN_H_CON AS TCPAConsentForHighestRankedPhone,
		PD42.PHN_H AS HighestRankedPhone,
		PD42.PHN_A_CON AS TCPAConsentForSecondHighestRankedPhone,
		PD42.PHN_A AS SecondHighestPhone,
		PD42.PHN_W_CON AS TCPAConsentForThirdHighestRankedPhone,
		PD42.PHN_W AS ThirdHighestPhone,
		PD30.DC_DOM_ST AS [State],
		LEFT(ISNULL(PD30.DF_ZIP_CDE, '00000'),5) AS ZIP,
		ISNULL(AmountDue.AMTDU,0.00) AS AmountDue, --Dialer requires no decimals
		SUM(ISNULL(LN10.LA_CUR_PRI,0.00) + ISNULL(DW01.WA_TOT_BRI_OTS,0.00) + ISNULL(AmountDue.LA_LTE_FEE_OTS_PRT,0.00)) OVER(PARTITION BY PD10.DF_SPE_ACC_ID) AS TotalBalance, --Dialer requires no decimals
		PD10Borr.DF_SPE_ACC_ID AS BorrowerAccountNumber2,
		CASE WHEN LN10.LF_LON_CUR_OWN = '900749' THEN 'CLP'
			 WHEN LN10.LF_LON_CUR_OWN = '971357' THEN 'TILP'
			 WHEN LN10.LF_LON_CUR_OWN LIKE '829769%' THEN 'BANA'
			 WHEN LN10.LF_LON_CUR_OWN IN('826717','830248') THEN 'ALIGN'
			 WHEN LN10.LF_LON_CUR_OWN = '828476' THEN 'UTAH'
			 ELSE LN10.LF_LON_CUR_OWN 
		END AS Portfolio,
		ISNULL(MAX(HardForb.TotalFrbMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalForbearanceTime, 
		ISNULL(MAX(EconDefer.TotalDfrMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalEconomicHardshipDefer, 
		ISNULL(MAX(UnempDefer.TotalDfrMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalUnemploymentDefer, 
		RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_MID) + ' ' + RTRIM(PD10.DM_PRS_LST) AS [RecipientName],
		RTRIM(PD10Borr.DM_PRS_1) + ' ' + RTRIM(PD10Borr.DM_PRS_MID) + ' ' + RTRIM(PD10Borr.DM_PRS_LST) AS [BorrowerName],
		ISNULL(LastCallDate.DateOfLastCall, '1900-01-01') AS DateOfLastCall
	FROM 
		UDW..LN10_LON LN10
		INNER JOIN UDW..LN20_EDS LN20
			ON LN20.BF_SSN = LN10.BF_SSN
			AND LN20.LN_SEQ = LN10.LN_SEQ
			AND LN20.LC_STA_LON20 = 'A'								
		INNER JOIN UDW..LN16_LON_DLQ_HST LN16
			ON LN16.BF_SSN = LN10.BF_SSN
			AND LN16.LN_SEQ = LN10.LN_SEQ
			AND LN16.LC_STA_LON16 = '1'
			AND DATEDIFF(DAY, LN16.LD_DLQ_MAX, GETDATE()) <= 5
			AND (LN16.LN_DLQ_MAX + 1) % 2 = 1 --Gets odd delq
			AND (LN16.LN_DLQ_MAX + 1) BETWEEN 15 AND 59 --Low Delq
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = LN20.LF_EDS
		INNER JOIN UDW..PD10_PRS_NME PD10Borr
			ON PD10Borr.DF_PRS_ID = LN10.BF_SSN
		INNER JOIN
		(
			SELECT DISTINCT
				DF_PRS_ID,
				MAX(IIF(OverallRank = 1, PHN, '')) AS PHN_H,
				MAX(IIF(OverallRank = 1, DC_ALW_ADL_PHN, '')) AS PHN_H_CON,
				MAX(IIF(OverallRank = 2, PHN, '')) AS PHN_A,
				MAX(IIF(OverallRank = 2, DC_ALW_ADL_PHN, '')) AS PHN_A_CON,
				MAX(IIF(OverallRank = 3, PHN, '')) AS PHN_W,
				MAX(IIF(OverallRank = 3, DC_ALW_ADL_PHN, '')) AS PHN_W_CON
			FROM
			(
				SELECT DISTINCT
					PD42.DF_PRS_ID,
					ISNULL(PD42.DN_DOM_PHN_ARA,'') + ISNULL(PD42.DN_DOM_PHN_XCH,'') + ISNULL(PD42.DN_DOM_PHN_LCL,'') AS PHN,
					PD42.DC_PHN,
					CASE WHEN PD42.DC_ALW_ADL_PHN IN ('L','X','P') 
						THEN 'Y'
						ELSE 'N'
					END AS DC_ALW_ADL_PHN,
					PD42.DC_ALW_ADL_PHN AS Consent,
					/*Rank phones by category and type*/
					DENSE_RANK() OVER (PARTITION BY PD42.DF_PRS_ID ORDER BY Ranks.ConsentRank, Ranks.TypeRank) AS OverallRank
				FROM
					UDW..PD42_PRS_PHN PD42
					INNER JOIN
					(
						SELECT
							DF_PRS_ID,
							DC_PHN,
							DC_ALW_ADL_PHN,
							CASE WHEN DC_PHN = 'H' THEN 1
									WHEN DC_PHN = 'A' THEN 2
									WHEN DC_PHN = 'W' THEN 3
									WHEN DC_PHN = 'M' THEN 4
							END AS TypeRank, --H A W M TYPE
							CASE WHEN DC_ALW_ADL_PHN = 'N' THEN 1
									WHEN DC_ALW_ADL_PHN = 'P' THEN 2 
									WHEN DC_ALW_ADL_PHN = 'X' THEN 3
									WHEN DC_ALW_ADL_PHN = 'L' THEN 4
									WHEN DC_ALW_ADL_PHN = 'Q' THEN 5
									WHEN DC_ALW_ADL_PHN = 'U' THEN 6 
							END AS ConsentRank--N P X L Q U CONSENT
						FROM
							UDW..PD42_PRS_PHN PD42
						WHERE
							PD42.DI_PHN_VLD = 'Y'
							AND PD42.DC_NO_HME_PHN != 'J'
							AND PD42.DC_ALW_ADL_PHN IN ('L','X','P') --Consent
					) Ranks
						ON Ranks.DF_PRS_ID = PD42.DF_PRS_ID
						AND Ranks.DC_PHN = PD42.DC_PHN
						AND Ranks.DC_ALW_ADL_PHN = PD42.DC_ALW_ADL_PHN
				WHERE
					PD42.DI_PHN_VLD = 'Y'
					AND PD42.DC_NO_HME_PHN != 'J'
					AND PD42.DC_ALW_ADL_PHN IN ('L','X','P') --Consent
			) Ranked
			WHERE
				Ranked.OverallRank IN (1,2,3)
			GROUP BY
				DF_PRS_ID
		) PD42 
			ON PD42.DF_PRS_ID = LN20.LF_EDS
			AND 
			(
				PD42.PHN_H != ''
				OR PD42.PHN_A != ''
				OR PD42.PHN_W != ''
			)
		INNER JOIN UDW..DW01_DW_CLC_CLU DW01
			ON DW01.BF_SSN = LN10.BF_SSN
			AND DW01.LN_SEQ = LN10.LN_SEQ
			AND DW01.WC_DW_LON_STA != '23'
			AND DW01.WC_DW_LON_STA IN ('03','04','05','08','14')
			AND RTRIM(DW01.WX_OVR_DW_LON_STA) NOT IN('PENDING ID THEFT','REALLOCATED','CNSLD-STOP PURSUIT')
		LEFT JOIN UDW..PD30_PRS_ADR PD30
			ON PD30.DF_PRS_ID = LN20.LF_EDS
			AND PD30.DC_ADR = 'L'
		LEFT JOIN UDW..WQ20_TSK_QUE WQ20
			ON WQ20.BF_SSN = LN10.BF_SSN
			AND WQ20.WF_QUE IN ('2A', '2P', 'GP', '9R', 'SI', 'S4', 'PR', 'VR', 'VB', 'MF', '1P', 'SF', 'DU', 'AT', '87')
			AND WQ20.WC_STA_WQUE20 IN ('A', 'H', 'P', 'U', 'W')
		LEFT JOIN ODW..CT30_CALL_QUE CT30
			ON CT30.DF_PRS_ID_BR = LN10.BF_SSN
			AND CT30.IF_WRK_GRP IN ('ASBKP', 'ASCON', 'ASMOC')
			AND CT30.IF_WRK_GRP IN ('A', 'W')	
		LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY DOC ID*/
		(
			SELECT
				BF_SSN,
				MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
			FROM
				UDW..AY10_BR_LON_ATY
			WHERE
				PF_REQ_ACT = 'DIDDA'
				AND DATEDIFF(DAY, LD_ATY_REQ_RCV, GETDATE()) <= 14
				AND LC_STA_ACTY10 = 'A'
			GROUP BY
				BF_SSN
		) DID 
			ON DID.BF_SSN = LN10.BF_SSN
		LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY APPROVAL*/
		(
			SELECT
				BF_SSN,
				MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
			FROM
				UDW..AY10_BR_LON_ATY
			WHERE
				PF_REQ_ACT = 'PAUTO'
				AND LC_STA_ACTY10 = 'A'
			GROUP BY
				BF_SSN
		) AUTO 
			ON AUTO.BF_SSN = LN10.BF_SSN
		LEFT JOIN /*Gets amount due from billing tables*/
		(
			SELECT 
				LN80.BF_SSN,
				SUM(ISNULL(LN80.LA_BIL_PAS_DU,0.00) + ISNULL(LN80.LA_BIL_DU_PRT,0.00) + ISNULL(LN80.LA_LTE_FEE_OTS_PRT,0.00) - ISNULL(LN80.LA_TOT_BIL_STS,0.00)) AS AMTDU,
				SUM(ISNULL(LN80.LA_LTE_FEE_OTS_PRT,0.00)) AS LA_LTE_FEE_OTS_PRT
			FROM 
				UDW..LN80_LON_BIL_CRF LN80
				INNER JOIN 
				(
					SELECT 
						BF_SSN,
						MAX(LD_BIL_CRT) AS LD_BIL_CRT
					FROM 
						UDW..BL10_BR_BIL
					WHERE 
						LC_STA_BIL10 = 'A'
						AND LC_BIL_TYP = 'P'
					GROUP BY 
						BF_SSN
				) BL10
					ON BL10.BF_SSN = LN80.BF_SSN
					AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
			WHERE 
				LN80.LC_STA_LON80 = 'A'
			GROUP BY 
				LN80.BF_SSN
		) AmountDue 
			ON AmountDue.BF_SSN = LN10.BF_SSN
		LEFT JOIN 
		(/*exclude future arc date*/
			SELECT DISTINCT
				BF_SSN,
				LD_ATY_REQ_RCV,
				PARSED_DAYS,
				PARSED_DAYS AS COMMENT_DAYS, /*takes lesser of 14 or parsed days*/
				DATEADD(DAY,PARSED_DAYS, LD_ATY_REQ_RCV) AS CALC_DATE,	
				CASE WHEN CAST(DATEADD(DAY,PARSED_DAYS, LD_ATY_REQ_RCV) AS DATE) > CAST(GETDATE() AS DATE)
					THEN 1
					ELSE 0
				END AS FUTURE_DATE
			FROM
			(/*parse out numeric days from any extraneous text*/
				SELECT 
					AY10.BF_SSN,
					AY10.LD_ATY_REQ_RCV,
					AY20.LX_ATY,
					IIF(COALESCE(TRY_CAST(LTRIM(RTRIM(AY20.LX_ATY)) AS INT),14) >= 14, 14, COALESCE(TRY_CAST(LTRIM(RTRIM(AY20.LX_ATY)) AS INT),14)) AS PARSED_DAYS	
				FROM
					UDW..AY10_BR_LON_ATY AY10
					LEFT JOIN UDW..AY20_ATY_TXT AY20
						ON AY20.BF_SSN = AY10.BF_SSN
						AND AY20.LN_ATY_SEQ = AY10.LN_ATY_SEQ
				WHERE
					AY10.LC_STA_ACTY10 = 'A'
					AND AY10.PF_REQ_ACT = 'BRPTP' /*borrower promise to repay*/
			) P
		) ARC_BRPTP 
			ON LN10.BF_SSN = ARC_BRPTP.BF_SSN
			AND ARC_BRPTP.FUTURE_DATE = 1 /*exclude future arc date*/
		LEFT JOIN
		( -- Deferment months already used
			SELECT DISTINCT
				LN50.BF_SSN,
				LN50.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN50.LD_DFR_BEG, DATEADD(DAY,1,LN50.LD_DFR_END))/365.00)*12) OVER (PARTITION BY LN50.BF_SSN, LN50.LN_SEQ), 1) AS [TotalDfrMonthsUsed]
			FROM
				UDW..DF10_BR_DFR_REQ DF10
				INNER JOIN UDW..LN50_BR_DFR_APV LN50 
					ON LN50.BF_SSN = DF10.BF_SSN 
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
			WHERE
				DF10.LC_DFR_STA = 'A'
				AND DF10.LC_STA_DFR10 = 'A'
				AND DF10.LC_DFR_TYP = 'D29'
				AND LN50.LC_STA_LON50 = 'A'
				AND LN50.LC_DFR_RSP != '003' -- 003 = DEFERMENT REQUEST DENIED
				AND LN50.LD_DFR_BEG <= GETDATE()
		) EconDefer
			ON EconDefer.BF_SSN = LN10.BF_SSN 
			AND EconDefer.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN
		( -- Deferment months already used
			SELECT DISTINCT
				LN50.BF_SSN,
				LN50.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN50.LD_DFR_BEG, DATEADD(DAY,1,LN50.LD_DFR_END))/365.00)*12) OVER (PARTITION BY LN50.BF_SSN, LN50.LN_SEQ), 1) AS [TotalDfrMonthsUsed]
			FROM
				UDW..DF10_BR_DFR_REQ DF10
				INNER JOIN UDW..LN50_BR_DFR_APV LN50 
					ON LN50.BF_SSN = DF10.BF_SSN 
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
			WHERE
				DF10.LC_DFR_STA = 'A'
				AND DF10.LC_STA_DFR10 = 'A'
				AND DF10.LC_DFR_TYP = 'D13'
				AND LN50.LC_STA_LON50 = 'A'
				AND LN50.LC_DFR_RSP != '003' -- 003 = DEFERMENT REQUEST DENIED
				AND LN50.LD_DFR_BEG <= GETDATE()
		) UnempDefer
			ON UnempDefer.BF_SSN = LN10.BF_SSN 
			AND UnempDefer.LN_SEQ = LN10.LN_SEQ 
		LEFT JOIN
		( -- Forb months already used
			SELECT DISTINCT
				LN60.BF_SSN,
				LN60.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN60.LD_FOR_BEG, DATEADD(DAY,1,LN60.LD_FOR_END))/365.00)*12) OVER (PARTITION BY LN60.BF_SSN, LN60.LN_SEQ), 1) AS [TotalFrbMonthsUsed]
			FROM
				UDW..FB10_BR_FOR_REQ FB10
				INNER JOIN UDW..LN60_BR_FOR_APV LN60 
					ON LN60.BF_SSN = FB10.BF_SSN
					AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
			WHERE
				FB10.LC_FOR_STA = 'A'
				AND FB10.LC_STA_FOR10 = 'A'
				AND FB10.LC_FOR_TYP = '05'
				AND LN60.LC_STA_LON60 = 'A'
				AND LN60.LC_FOR_RSP != '003' --003 = FORBEARANCE REQUEST DENIED
				AND LN60.LD_FOR_BEG <= GETDATE()
		) HardForb
			ON HardForb.BF_SSN = LN10.BF_SSN
			AND HardForb.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN
		(
			SELECT 
				NCH.AccountIdentifier, 
				NCH.ActivityDate 
			FROM 
				NobleCalls..NobleCallHistory NCH
				INNER JOIN NobleCalls..DispositionCodeMapping DCM
					ON DCM.DispositionCode = NCH.DispositionCode
				INNER JOIN NobleCalls..ResponseCodes RC
					ON RC.ResponseCodeId = DCM.ResponseCodeId
					AND RC.ResponseCode = 'CNTCT'
			WHERE
				CAST(NCH.ActivityDate AS DATE) BETWEEN CAST(DATEADD(DAY,-7,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE) --last 7 days
		) ContactLastSeven
			ON ContactLastSeven.AccountIdentifier = PD10.DF_SPE_ACC_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				NCH.AccountIdentifier, 
				COUNT(*) AS NumberAttempts
			FROM 
				NobleCalls..NobleCallHistory NCH
				INNER JOIN NobleCalls..DispositionCodeMapping DCM
					ON DCM.DispositionCode = NCH.DispositionCode
				INNER JOIN NobleCalls..ResponseCodes RC
					ON RC.ResponseCodeId = DCM.ResponseCodeId
					AND RC.ResponseCode IN('INVPH', 'NOCTC')
			WHERE
				CAST(NCH.ActivityDate AS DATE) BETWEEN CAST(DATEADD(DAY,-7,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE) --last 7 days
			GROUP BY
				NCH.AccountIdentifier
		) OneAttemptLastSeven
			ON OneAttemptLastSeven.AccountIdentifier = PD10.DF_SPE_ACC_ID
			AND OneAttemptLastSeven.NumberAttempts >= 1
		LEFT JOIN 
		(
			SELECT
				NCH.AccountIdentifier,
				MAX(NCH.ActivityDate) AS DateOfLastCall
			FROM
				NobleCalls..NobleCallHistory NCH
			GROUP BY
				NCH.AccountIdentifier
		) LastCallDate
			ON LastCallDate.AccountIdentifier = PD10.DF_SPE_ACC_ID
	WHERE
		LN10.LC_STA_LON10 = 'R'
		AND ISNULL(LN10.LA_CUR_PRI,0.00) + ISNULL(DW01.WA_TOT_BRI_OTS,0.00) > 0.00
		AND LN10.LI_CON_PAY_STP_PUR != 'Y'
		AND ISNULL(DID.LD_ATY_REQ_RCV,'2099-01-01') > ISNULL(AUTO.LD_ATY_REQ_RCV,'1900-01-01')
		AND ARC_BRPTP.BF_SSN IS NULL /*exclude future arc date*/
		AND WQ20.BF_SSN IS NULL /*exclude pending queue*/
		AND CT30.DF_PRS_ID_BR IS NULL /*exclude pending one link task*/
		AND ContactLastSeven.AccountIdentifier IS NULL /*exclude successful contacts in last week*/
		AND OneAttemptLastSeven.AccountIdentifier IS NULL /*exclude 3 failed contacts in last week*/
) ADL --AutoDelq_Low
ORDER BY
	ADL.DateOfLastCall ASC


GO
CREATE PROCEDURE [dbo].[SsrsUTLWS02_AutoDelq_High]
	
AS
--R4
--Delq
SELECT
	RecipientAccountNumber,
	Delinquency,
	RecipientDescription,
	BorrowerAccountNumber,
	TCPAConsentForHighestRankedPhone,
	HighestRankedPhone,
	TCPAConsentForSecondHighestRankedPhone,
	SecondHighestPhone,
	TCPAConsentForThirdHighestRankedPhone,
	ThirdHighestPhone,
	[State],
	ZIP,
	AmountDue,
	TotalBalance,
	BorrowerAccountNumber2,
	Portfolio,
	TotalForbearanceTime,
	TotalEconomicHardshipDefer,
	TotalUnemploymentDefer,
	RecipientName,
	BorrowerName,
	DateOfLastCall
FROM
(
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID AS RecipientAccountNumber,
		MAX(LN16.LN_DLQ_MAX) OVER(PARTITION BY LN10.BF_SSN) + 1 AS Delinquency,
		'BORROWER' AS RecipientDescription,
		PD10.DF_SPE_ACC_ID AS BorrowerAccountNumber,
		PD42.PHN_H_CON AS TCPAConsentForHighestRankedPhone,
		PD42.PHN_H AS HighestRankedPhone,
		PD42.PHN_A_CON AS TCPAConsentForSecondHighestRankedPhone,
		PD42.PHN_A AS SecondHighestPhone,
		PD42.PHN_W_CON AS TCPAConsentForThirdHighestRankedPhone,
		PD42.PHN_W AS ThirdHighestPhone,
		PD30.DC_DOM_ST AS [State],
		LEFT(ISNULL(PD30.DF_ZIP_CDE, '00000'),5) AS ZIP,
		ISNULL(AmountDue.AMTDU,0.00) AS AmountDue, --Dialer requires no decimals
		SUM(ISNULL(LN10.LA_CUR_PRI,0.00) + ISNULL(DW01.WA_TOT_BRI_OTS,0.00) + ISNULL(AmountDue.LA_LTE_FEE_OTS_PRT,0.00)) OVER(PARTITION BY PD10.DF_SPE_ACC_ID) AS TotalBalance, --Dialer requires no decimals
		PD10.DF_SPE_ACC_ID AS BorrowerAccountNumber2,
		CASE WHEN LN10.LF_LON_CUR_OWN = '900749' THEN 'CLP'
			 WHEN LN10.LF_LON_CUR_OWN = '971357' THEN 'TILP'
			 WHEN LN10.LF_LON_CUR_OWN LIKE '829769%' THEN 'BANA'
			 WHEN LN10.LF_LON_CUR_OWN IN('826717','830248') THEN 'ALIGN'
			 WHEN LN10.LF_LON_CUR_OWN = '828476' THEN 'UTAH'
			 ELSE LN10.LF_LON_CUR_OWN 
		END AS Portfolio,
		ISNULL(MAX(HardForb.TotalFrbMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalForbearanceTime, 
		ISNULL(MAX(EconDefer.TotalDfrMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalEconomicHardshipDefer, 
		ISNULL(MAX(UnempDefer.TotalDfrMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalUnemploymentDefer, 
		RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_MID) + ' ' + RTRIM(PD10.DM_PRS_LST) AS [RecipientName],
		RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_MID) + ' ' + RTRIM(PD10.DM_PRS_LST) AS [BorrowerName],
		ISNULL(LastCallDate.DateOfLastCall, '1900-01-01') AS DateOfLastCall
	FROM 
		UDW..LN10_LON LN10
		INNER JOIN UDW..LN16_LON_DLQ_HST LN16
			ON LN16.BF_SSN = LN10.BF_SSN
			AND LN16.LN_SEQ = LN10.LN_SEQ
			AND LN16.LC_STA_LON16 = '1'
			AND DATEDIFF(DAY, LN16.LD_DLQ_MAX, GETDATE()) <= 5
			AND DATEDIFF(DAY, LN16.LD_DLQ_OCC, GETDATE()) > 10
			AND (LN16.LN_DLQ_MAX + 1) % 2 = 1 --Gets odd delq 
			AND (LN16.LN_DLQ_MAX + 1) >= 60 --High Delq
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = LN10.BF_SSN
		INNER JOIN 
		(
			SELECT DISTINCT
				DF_PRS_ID,
				MAX(IIF(OverallRank = 1, PHN, '')) AS PHN_H,
				MAX(IIF(OverallRank = 1, DC_ALW_ADL_PHN, '')) AS PHN_H_CON,
				MAX(IIF(OverallRank = 2, PHN, '')) AS PHN_A,
				MAX(IIF(OverallRank = 2, DC_ALW_ADL_PHN, '')) AS PHN_A_CON,
				MAX(IIF(OverallRank = 3, PHN, '')) AS PHN_W,
				MAX(IIF(OverallRank = 3, DC_ALW_ADL_PHN, '')) AS PHN_W_CON
			FROM
			(
				SELECT DISTINCT
					PD42.DF_PRS_ID,
					ISNULL(PD42.DN_DOM_PHN_ARA,'') + ISNULL(PD42.DN_DOM_PHN_XCH,'') + ISNULL(PD42.DN_DOM_PHN_LCL,'') AS PHN,
					PD42.DC_PHN,
					CASE WHEN PD42.DC_ALW_ADL_PHN IN ('L','X','P') 
						THEN 'Y'
						ELSE 'N'
					END AS DC_ALW_ADL_PHN,
					PD42.DC_ALW_ADL_PHN AS Consent,
					/*Rank phones by category and type*/
					DENSE_RANK() OVER (PARTITION BY PD42.DF_PRS_ID ORDER BY Ranks.ConsentRank, Ranks.TypeRank) AS OverallRank
				FROM
					UDW..PD42_PRS_PHN PD42
					INNER JOIN
					(
						SELECT
							DF_PRS_ID,
							DC_PHN,
							DC_ALW_ADL_PHN,
							CASE WHEN DC_PHN = 'H' THEN 1
									WHEN DC_PHN = 'A' THEN 2
									WHEN DC_PHN = 'W' THEN 3
									WHEN DC_PHN = 'M' THEN 4
							END AS TypeRank, --H A W M TYPE
							CASE WHEN DC_ALW_ADL_PHN = 'N' THEN 1
									WHEN DC_ALW_ADL_PHN = 'P' THEN 2 
									WHEN DC_ALW_ADL_PHN = 'X' THEN 3
									WHEN DC_ALW_ADL_PHN = 'L' THEN 4
									WHEN DC_ALW_ADL_PHN = 'Q' THEN 5
									WHEN DC_ALW_ADL_PHN = 'U' THEN 6 
							END AS ConsentRank--N P X L Q U CONSENT
						FROM
							UDW..PD42_PRS_PHN PD42
						WHERE
							PD42.DI_PHN_VLD = 'Y'
							AND PD42.DC_NO_HME_PHN != 'J'
							AND PD42.DC_ALW_ADL_PHN IN ('L','X','P') --Consent
					) Ranks
						ON Ranks.DF_PRS_ID = PD42.DF_PRS_ID
						AND Ranks.DC_PHN = PD42.DC_PHN
						AND Ranks.DC_ALW_ADL_PHN = PD42.DC_ALW_ADL_PHN
				WHERE
					PD42.DI_PHN_VLD = 'Y'
					AND PD42.DC_NO_HME_PHN != 'J'
					AND PD42.DC_ALW_ADL_PHN IN ('L','X','P') --Consent
			) Ranked
			WHERE
				Ranked.OverallRank IN (1,2,3)
			GROUP BY
				DF_PRS_ID
		) PD42
			ON PD42.DF_PRS_ID = LN10.BF_SSN
			AND 
			(
				PD42.PHN_H != ''
				OR PD42.PHN_A != ''
				OR PD42.PHN_W != ''
			)
		INNER JOIN UDW..DW01_DW_CLC_CLU DW01
			ON DW01.BF_SSN = LN10.BF_SSN
			AND DW01.LN_SEQ = LN10.LN_SEQ
			AND DW01.WC_DW_LON_STA != '23'
			AND DW01.WC_DW_LON_STA IN ('03','04','05','08','14')
			AND RTRIM(DW01.WX_OVR_DW_LON_STA) NOT IN('PENDING ID THEFT','REALLOCATED','CNSLD-STOP PURSUIT')
		LEFT JOIN UDW..PD30_PRS_ADR PD30
			ON PD30.DF_PRS_ID = LN10.BF_SSN
			AND PD30.DC_ADR = 'L'
		LEFT JOIN UDW..WQ20_TSK_QUE WQ20
			ON WQ20.BF_SSN = LN10.BF_SSN
			AND WQ20.WF_QUE IN ('2A', '2P', 'GP', '9R', 'SI', 'S4', 'PR', 'VR', 'VB', 'MF', '1P', 'SF', 'DU', 'AT', '87')
			AND WQ20.WC_STA_WQUE20 IN ('A', 'H', 'P', 'U', 'W')
		LEFT JOIN ODW..CT30_CALL_QUE CT30
			ON CT30.DF_PRS_ID_BR = LN10.BF_SSN
			AND CT30.IF_WRK_GRP IN ('ASBKP', 'ASCON', 'ASMOC')
			AND CT30.IF_WRK_GRP IN ('A', 'W')	
		LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY DOC ID*/
		(
			SELECT
				BF_SSN,
				MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
			FROM
				UDW..AY10_BR_LON_ATY
			WHERE
				PF_REQ_ACT = 'DIDDA'
				AND DATEDIFF(DAY, LD_ATY_REQ_RCV, GETDATE()) <= 14
				AND LC_STA_ACTY10 = 'A'
			GROUP BY
				BF_SSN
		)DID 
			ON DID.BF_SSN = LN10.BF_SSN
		LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY APPROVAL*/
		(
			SELECT
				BF_SSN,
				MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
			FROM
				UDW..AY10_BR_LON_ATY
			WHERE
				PF_REQ_ACT = 'PAUTO'
				AND LC_STA_ACTY10 = 'A'
			GROUP BY
				BF_SSN
		) AUTO 
			ON AUTO.BF_SSN = LN10.BF_SSN
		LEFT JOIN /*Gets amount due from billing tables*/
		(
			SELECT 
				LN80.BF_SSN,
				SUM(ISNULL(LN80.LA_BIL_PAS_DU,0.00) + ISNULL(LN80.LA_BIL_DU_PRT,0.00) + ISNULL(LN80.LA_LTE_FEE_OTS_PRT,0.00) - ISNULL(LN80.LA_TOT_BIL_STS,0.00)) AS AMTDU,
				SUM(ISNULL(LN80.LA_LTE_FEE_OTS_PRT,0.00)) AS LA_LTE_FEE_OTS_PRT
			FROM 
				UDW..LN80_LON_BIL_CRF LN80
				INNER JOIN 
				(
					SELECT 
						BF_SSN,
						MAX(LD_BIL_CRT) AS LD_BIL_CRT
					FROM 
						UDW..BL10_BR_BIL
					WHERE 
						LC_STA_BIL10 = 'A'
						AND LC_BIL_TYP = 'P'
					GROUP BY 
						BF_SSN
				) BL10
					ON BL10.BF_SSN = LN80.BF_SSN
					AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
			WHERE 
				LN80.LC_STA_LON80 = 'A'
			GROUP BY 
				LN80.BF_SSN
		) AmountDue 
			ON AmountDue.BF_SSN = LN10.BF_SSN
		LEFT JOIN 
		(/*exclude future arc date*/
			SELECT DISTINCT
				BF_SSN,
				LD_ATY_REQ_RCV,
				PARSED_DAYS,
				PARSED_DAYS AS COMMENT_DAYS, /*takes lesser of 14 or parsed days*/
				DATEADD(DAY,PARSED_DAYS, LD_ATY_REQ_RCV) AS CALC_DATE,	
				CASE WHEN CAST(DATEADD(DAY,PARSED_DAYS, LD_ATY_REQ_RCV) AS DATE) > CAST(GETDATE() AS DATE)
					THEN 1
					ELSE 0
				END AS FUTURE_DATE
			FROM
			(/*parse out numeric days from any extraneous text*/
				SELECT 
					AY10.BF_SSN,
					AY10.LD_ATY_REQ_RCV,
					AY20.LX_ATY,
					IIF(COALESCE(TRY_CAST(LTRIM(RTRIM(AY20.LX_ATY)) AS INT),14) >= 14, 14, COALESCE(TRY_CAST(LTRIM(RTRIM(AY20.LX_ATY)) AS INT),14)) AS PARSED_DAYS	
				FROM
					UDW..AY10_BR_LON_ATY AY10
					LEFT JOIN UDW..AY20_ATY_TXT AY20
						ON AY20.BF_SSN = AY10.BF_SSN
						AND AY20.LN_ATY_SEQ = AY10.LN_ATY_SEQ
				WHERE
					AY10.LC_STA_ACTY10 = 'A'
					AND AY10.PF_REQ_ACT = 'BRPTP' /*borrower promise to repay*/
			) P
		) ARC_BRPTP 
			ON ARC_BRPTP.BF_SSN = LN10.BF_SSN
			AND ARC_BRPTP.FUTURE_DATE = 1 /*exclude future arc date*/
		LEFT JOIN
		( -- Deferment months already used
			SELECT DISTINCT
				LN50.BF_SSN,
				LN50.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN50.LD_DFR_BEG, DATEADD(DAY,1,LN50.LD_DFR_END))/365.00)*12) OVER (PARTITION BY LN50.BF_SSN, LN50.LN_SEQ), 1) AS [TotalDfrMonthsUsed]
			FROM
				UDW..DF10_BR_DFR_REQ DF10
				INNER JOIN UDW..LN50_BR_DFR_APV LN50 
					ON LN50.BF_SSN = DF10.BF_SSN 
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
			WHERE
				DF10.LC_DFR_STA = 'A'
				AND DF10.LC_STA_DFR10 = 'A'
				AND DF10.LC_DFR_TYP = 'D29'
				AND LN50.LC_STA_LON50 = 'A'
				AND LN50.LC_DFR_RSP != '003' -- 003 = DEFERMENT REQUEST DENIED
				AND LN50.LD_DFR_BEG <= GETDATE()
		) EconDefer
			ON EconDefer.BF_SSN = LN10.BF_SSN 
			AND EconDefer.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN
		( -- Deferment months already used
			SELECT DISTINCT
				LN50.BF_SSN,
				LN50.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN50.LD_DFR_BEG, DATEADD(DAY,1,LN50.LD_DFR_END))/365.00)*12) OVER (PARTITION BY LN50.BF_SSN, LN50.LN_SEQ), 1) AS [TotalDfrMonthsUsed]
			FROM
				UDW..DF10_BR_DFR_REQ DF10
				INNER JOIN UDW..LN50_BR_DFR_APV LN50 
					ON LN50.BF_SSN = DF10.BF_SSN 
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
			WHERE
				DF10.LC_DFR_STA = 'A'
				AND DF10.LC_STA_DFR10 = 'A'
				AND DF10.LC_DFR_TYP = 'D13'
				AND LN50.LC_STA_LON50 = 'A'
				AND LN50.LC_DFR_RSP != '003' -- 003 = DEFERMENT REQUEST DENIED
				AND LN50.LD_DFR_BEG <= GETDATE()
		) UnempDefer
			ON UnempDefer.BF_SSN = LN10.BF_SSN 
			AND UnempDefer.LN_SEQ = LN10.LN_SEQ 
		LEFT JOIN
		( -- Forb months already used
			SELECT DISTINCT
				LN60.BF_SSN,
				LN60.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN60.LD_FOR_BEG, DATEADD(DAY,1,LN60.LD_FOR_END))/365.00)*12) OVER (PARTITION BY LN60.BF_SSN, LN60.LN_SEQ), 1) AS [TotalFrbMonthsUsed]
			FROM
				UDW..FB10_BR_FOR_REQ FB10
				INNER JOIN UDW..LN60_BR_FOR_APV LN60 
					ON LN60.BF_SSN = FB10.BF_SSN
					AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
			WHERE
				FB10.LC_FOR_STA = 'A'
				AND FB10.LC_STA_FOR10 = 'A'
				AND FB10.LC_FOR_TYP = '05'
				AND LN60.LC_STA_LON60 = 'A'
				AND LN60.LC_FOR_RSP != '003' --003 = FORBEARANCE REQUEST DENIED
				AND LN60.LD_FOR_BEG <= GETDATE()
		) HardForb
			ON HardForb.BF_SSN = LN10.BF_SSN
			AND HardForb.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN
		(
			SELECT 
				NCH.AccountIdentifier, 
				NCH.ActivityDate 
			FROM 
				NobleCalls..NobleCallHistory NCH
				INNER JOIN NobleCalls..DispositionCodeMapping DCM
					ON DCM.DispositionCode = NCH.DispositionCode
				INNER JOIN NobleCalls..ResponseCodes RC
					ON RC.ResponseCodeId = DCM.ResponseCodeId
					AND RC.ResponseCode = 'CNTCT'
			WHERE
				CAST(NCH.ActivityDate AS DATE) BETWEEN CAST(DATEADD(DAY,-7,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE) --last 7 days
		) ContactLastSeven
			ON ContactLastSeven.AccountIdentifier = PD10.DF_SPE_ACC_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				NCH.AccountIdentifier, 
				COUNT(*) AS NumberAttempts
			FROM 
				NobleCalls..NobleCallHistory NCH
				INNER JOIN NobleCalls..DispositionCodeMapping DCM
					ON DCM.DispositionCode = NCH.DispositionCode
				INNER JOIN NobleCalls..ResponseCodes RC
					ON RC.ResponseCodeId = DCM.ResponseCodeId
					AND RC.ResponseCode IN('INVPH', 'NOCTC')
			WHERE
				CAST(NCH.ActivityDate AS DATE) BETWEEN CAST(DATEADD(DAY,-7,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE) --last 7 days
			GROUP BY
				NCH.AccountIdentifier
		) OneAttemptLastSeven
			ON OneAttemptLastSeven.AccountIdentifier = PD10.DF_SPE_ACC_ID
			AND OneAttemptLastSeven.NumberAttempts >= 1
		LEFT JOIN 
		(
			SELECT
				NCH.AccountIdentifier,
				MAX(NCH.ActivityDate) AS DateOfLastCall
			FROM
				NobleCalls..NobleCallHistory NCH
			GROUP BY
				NCH.AccountIdentifier
		) LastCallDate
			ON LastCallDate.AccountIdentifier = PD10.DF_SPE_ACC_ID
	WHERE
		LN10.LC_STA_LON10 = 'R'
		AND ISNULL(LN10.LA_CUR_PRI,0.00) + ISNULL(DW01.WA_TOT_BRI_OTS,0.00) > 0.00
		AND LN10.LI_CON_PAY_STP_PUR != 'Y'
		AND ISNULL(DID.LD_ATY_REQ_RCV,'2099-01-01') > ISNULL(AUTO.LD_ATY_REQ_RCV,'1900-01-01')
		AND ARC_BRPTP.BF_SSN IS NULL /*exclude future arc date*/
		AND WQ20.BF_SSN IS NULL /*exclude pending queue*/
		AND CT30.DF_PRS_ID_BR IS NULL /*exclude pending one link task*/
		AND ContactLastSeven.AccountIdentifier IS NULL /*exclude successful contacts in last week*/
		AND OneAttemptLastSeven.AccountIdentifier IS NULL /*exclude 3 failed contacts in last week*/

	UNION ALL

	/*	begin DELQ_EDR*/
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID AS RecipientAccountNumber,
		MAX(LN16.LN_DLQ_MAX) OVER(PARTITION BY LN10.BF_SSN) + 1 AS Delinquency,
		CASE WHEN LC_EDS_TYP = 'M' THEN 'COBORROWER'
			 WHEN LC_EDS_TYP = 'S' THEN 'ENDORSER'
			 ELSE ''
		END AS RecipientDescription,
		PD10Borr.DF_SPE_ACC_ID AS BorrowerAccountNumber,
		PD42.PHN_H_CON AS TCPAConsentForHighestRankedPhone,
		PD42.PHN_H AS HighestRankedPhone,
		PD42.PHN_A_CON AS TCPAConsentForSecondHighestRankedPhone,
		PD42.PHN_A AS SecondHighestPhone,
		PD42.PHN_W_CON AS TCPAConsentForThirdHighestRankedPhone,
		PD42.PHN_W AS ThirdHighestPhone,
		PD30.DC_DOM_ST AS [State],
		LEFT(ISNULL(PD30.DF_ZIP_CDE, '00000'),5) AS ZIP,
		ISNULL(AmountDue.AMTDU,0.00) AS AmountDue, --Dialer requires no decimals
		SUM(ISNULL(LN10.LA_CUR_PRI,0.00) + ISNULL(DW01.WA_TOT_BRI_OTS,0.00) + ISNULL(AmountDue.LA_LTE_FEE_OTS_PRT,0.00)) OVER(PARTITION BY PD10.DF_SPE_ACC_ID) AS TotalBalance, --Dialer requires no decimals
		PD10Borr.DF_SPE_ACC_ID AS BorrowerAccountNumber2,
		CASE WHEN LN10.LF_LON_CUR_OWN = '900749' THEN 'CLP'
			 WHEN LN10.LF_LON_CUR_OWN = '971357' THEN 'TILP'
			 WHEN LN10.LF_LON_CUR_OWN LIKE '829769%' THEN 'BANA'
			 WHEN LN10.LF_LON_CUR_OWN IN('826717','830248') THEN 'ALIGN'
			 WHEN LN10.LF_LON_CUR_OWN = '828476' THEN 'UTAH'
			 ELSE LN10.LF_LON_CUR_OWN 
		END AS Portfolio,
		ISNULL(MAX(HardForb.TotalFrbMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalForbearanceTime, 
		ISNULL(MAX(EconDefer.TotalDfrMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalEconomicHardshipDefer, 
		ISNULL(MAX(UnempDefer.TotalDfrMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalUnemploymentDefer, 
		RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_MID) + ' ' + RTRIM(PD10.DM_PRS_LST) AS [RecipientName],
		RTRIM(PD10Borr.DM_PRS_1) + ' ' + RTRIM(PD10Borr.DM_PRS_MID) + ' ' + RTRIM(PD10Borr.DM_PRS_LST) AS [BorrowerName],
		ISNULL(LastCallDate.DateOfLastCall, '1900-01-01') AS DateOfLastCall
	FROM 
		UDW..LN10_LON LN10
		INNER JOIN UDW..LN20_EDS LN20
			ON LN20.BF_SSN = LN10.BF_SSN
			AND LN20.LN_SEQ = LN10.LN_SEQ
			AND LN20.LC_STA_LON20 = 'A'								
		INNER JOIN UDW..LN16_LON_DLQ_HST LN16
			ON LN16.BF_SSN = LN10.BF_SSN
			AND LN16.LN_SEQ = LN10.LN_SEQ
			AND LN16.LC_STA_LON16 = '1'
			AND DATEDIFF(DAY, LN16.LD_DLQ_MAX, GETDATE()) <= 5
			AND (LN16.LN_DLQ_MAX + 1) % 2 = 1 --Gets odd delq 
			AND (LN16.LN_DLQ_MAX + 1) >= 60 --High Delq
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = LN20.LF_EDS
		INNER JOIN UDW..PD10_PRS_NME PD10Borr
			ON PD10Borr.DF_PRS_ID = LN10.BF_SSN
		INNER JOIN
		(
			SELECT DISTINCT
				DF_PRS_ID,
				MAX(IIF(OverallRank = 1, PHN, '')) AS PHN_H,
				MAX(IIF(OverallRank = 1, DC_ALW_ADL_PHN, '')) AS PHN_H_CON,
				MAX(IIF(OverallRank = 2, PHN, '')) AS PHN_A,
				MAX(IIF(OverallRank = 2, DC_ALW_ADL_PHN, '')) AS PHN_A_CON,
				MAX(IIF(OverallRank = 3, PHN, '')) AS PHN_W,
				MAX(IIF(OverallRank = 3, DC_ALW_ADL_PHN, '')) AS PHN_W_CON
			FROM
			(
				SELECT DISTINCT
					PD42.DF_PRS_ID,
					ISNULL(PD42.DN_DOM_PHN_ARA,'') + ISNULL(PD42.DN_DOM_PHN_XCH,'') + ISNULL(PD42.DN_DOM_PHN_LCL,'') AS PHN,
					PD42.DC_PHN,
					CASE WHEN PD42.DC_ALW_ADL_PHN IN ('L','X','P') 
						THEN 'Y'
						ELSE 'N'
					END AS DC_ALW_ADL_PHN,
					PD42.DC_ALW_ADL_PHN AS Consent,
					/*Rank phones by category and type*/
					DENSE_RANK() OVER (PARTITION BY PD42.DF_PRS_ID ORDER BY Ranks.ConsentRank, Ranks.TypeRank) AS OverallRank
				FROM
					UDW..PD42_PRS_PHN PD42
					INNER JOIN
					(
						SELECT
							DF_PRS_ID,
							DC_PHN,
							DC_ALW_ADL_PHN,
							CASE WHEN DC_PHN = 'H' THEN 1
									WHEN DC_PHN = 'A' THEN 2
									WHEN DC_PHN = 'W' THEN 3
									WHEN DC_PHN = 'M' THEN 4
							END AS TypeRank, --H A W M TYPE
							CASE WHEN DC_ALW_ADL_PHN = 'N' THEN 1
									WHEN DC_ALW_ADL_PHN = 'P' THEN 2 
									WHEN DC_ALW_ADL_PHN = 'X' THEN 3
									WHEN DC_ALW_ADL_PHN = 'L' THEN 4
									WHEN DC_ALW_ADL_PHN = 'Q' THEN 5
									WHEN DC_ALW_ADL_PHN = 'U' THEN 6 
							END AS ConsentRank--N P X L Q U CONSENT
						FROM
							UDW..PD42_PRS_PHN PD42
						WHERE
							PD42.DI_PHN_VLD = 'Y'
							AND PD42.DC_NO_HME_PHN != 'J'
							AND PD42.DC_ALW_ADL_PHN IN ('L','X','P') --Consent
					) Ranks
						ON Ranks.DF_PRS_ID = PD42.DF_PRS_ID
						AND Ranks.DC_PHN = PD42.DC_PHN
						AND Ranks.DC_ALW_ADL_PHN = PD42.DC_ALW_ADL_PHN
				WHERE
					PD42.DI_PHN_VLD = 'Y'
					AND PD42.DC_NO_HME_PHN != 'J'
					AND PD42.DC_ALW_ADL_PHN IN ('L','X','P') --Consent
			) Ranked
			WHERE
				Ranked.OverallRank IN (1,2,3)
			GROUP BY
				DF_PRS_ID
		) PD42 
			ON PD42.DF_PRS_ID = LN20.LF_EDS
			AND 
			(
				PD42.PHN_H != ''
				OR PD42.PHN_A != ''
				OR PD42.PHN_W != ''
			)
		INNER JOIN UDW..DW01_DW_CLC_CLU DW01
			ON DW01.BF_SSN = LN10.BF_SSN
			AND DW01.LN_SEQ = LN10.LN_SEQ
			AND DW01.WC_DW_LON_STA != '23'
			AND DW01.WC_DW_LON_STA IN ('03','04','05','08','14')
			AND RTRIM(DW01.WX_OVR_DW_LON_STA) NOT IN('PENDING ID THEFT','REALLOCATED','CNSLD-STOP PURSUIT')
		LEFT JOIN UDW..PD30_PRS_ADR PD30
			ON PD30.DF_PRS_ID = LN20.LF_EDS
			AND PD30.DC_ADR = 'L'
		LEFT JOIN UDW..WQ20_TSK_QUE WQ20
			ON WQ20.BF_SSN = LN10.BF_SSN
			AND WQ20.WF_QUE IN ('2A', '2P', 'GP', '9R', 'SI', 'S4', 'PR', 'VR', 'VB', 'MF', '1P', 'SF', 'DU', 'AT', '87')
			AND WQ20.WC_STA_WQUE20 IN ('A', 'H', 'P', 'U', 'W')
		LEFT JOIN ODW..CT30_CALL_QUE CT30
			ON CT30.DF_PRS_ID_BR = LN10.BF_SSN
			AND CT30.IF_WRK_GRP IN ('ASBKP', 'ASCON', 'ASMOC')
			AND CT30.IF_WRK_GRP IN ('A', 'W')	
		LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY DOC ID*/
		(
			SELECT
				BF_SSN,
				MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
			FROM
				UDW..AY10_BR_LON_ATY
			WHERE
				PF_REQ_ACT = 'DIDDA'
				AND DATEDIFF(DAY, LD_ATY_REQ_RCV, GETDATE()) <= 14
				AND LC_STA_ACTY10 = 'A'
			GROUP BY
				BF_SSN
		) DID 
			ON DID.BF_SSN = LN10.BF_SSN
		LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY APPROVAL*/
		(
			SELECT
				BF_SSN,
				MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
			FROM
				UDW..AY10_BR_LON_ATY
			WHERE
				PF_REQ_ACT = 'PAUTO'
				AND LC_STA_ACTY10 = 'A'
			GROUP BY
				BF_SSN
		) AUTO 
			ON AUTO.BF_SSN = LN10.BF_SSN
		LEFT JOIN /*Gets amount due from billing tables*/
		(
			SELECT 
				LN80.BF_SSN,
				SUM(ISNULL(LN80.LA_BIL_PAS_DU,0.00) + ISNULL(LN80.LA_BIL_DU_PRT,0.00) + ISNULL(LN80.LA_LTE_FEE_OTS_PRT,0.00) - ISNULL(LN80.LA_TOT_BIL_STS,0.00)) AS AMTDU,
				SUM(ISNULL(LN80.LA_LTE_FEE_OTS_PRT,0.00)) AS LA_LTE_FEE_OTS_PRT
			FROM 
				UDW..LN80_LON_BIL_CRF LN80
				INNER JOIN 
				(
					SELECT 
						BF_SSN,
						MAX(LD_BIL_CRT) AS LD_BIL_CRT
					FROM 
						UDW..BL10_BR_BIL
					WHERE 
						LC_STA_BIL10 = 'A'
						AND LC_BIL_TYP = 'P'
					GROUP BY 
						BF_SSN
				) BL10
					ON BL10.BF_SSN = LN80.BF_SSN
					AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
			WHERE 
				LN80.LC_STA_LON80 = 'A'
			GROUP BY 
				LN80.BF_SSN
		) AmountDue 
			ON AmountDue.BF_SSN = LN10.BF_SSN
		LEFT JOIN 
		(/*exclude future arc date*/
			SELECT DISTINCT
				BF_SSN,
				LD_ATY_REQ_RCV,
				PARSED_DAYS,
				PARSED_DAYS AS COMMENT_DAYS, /*takes lesser of 14 or parsed days*/
				DATEADD(DAY,PARSED_DAYS, LD_ATY_REQ_RCV) AS CALC_DATE,	
				CASE WHEN CAST(DATEADD(DAY,PARSED_DAYS, LD_ATY_REQ_RCV) AS DATE) > CAST(GETDATE() AS DATE)
					THEN 1
					ELSE 0
				END AS FUTURE_DATE
			FROM
			(/*parse out numeric days from any extraneous text*/
				SELECT 
					AY10.BF_SSN,
					AY10.LD_ATY_REQ_RCV,
					AY20.LX_ATY,
					IIF(COALESCE(TRY_CAST(LTRIM(RTRIM(AY20.LX_ATY)) AS INT),14) >= 14, 14, COALESCE(TRY_CAST(LTRIM(RTRIM(AY20.LX_ATY)) AS INT),14)) AS PARSED_DAYS	
				FROM
					UDW..AY10_BR_LON_ATY AY10
					LEFT JOIN UDW..AY20_ATY_TXT AY20
						ON AY20.BF_SSN = AY10.BF_SSN
						AND AY20.LN_ATY_SEQ = AY10.LN_ATY_SEQ
				WHERE
					AY10.LC_STA_ACTY10 = 'A'
					AND AY10.PF_REQ_ACT = 'BRPTP' /*borrower promise to repay*/
			) P
		) ARC_BRPTP 
			ON LN10.BF_SSN = ARC_BRPTP.BF_SSN
			AND ARC_BRPTP.FUTURE_DATE = 1 /*exclude future arc date*/
		LEFT JOIN
		( -- Deferment months already used
			SELECT DISTINCT
				LN50.BF_SSN,
				LN50.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN50.LD_DFR_BEG, DATEADD(DAY,1,LN50.LD_DFR_END))/365.00)*12) OVER (PARTITION BY LN50.BF_SSN, LN50.LN_SEQ), 1) AS [TotalDfrMonthsUsed]
			FROM
				UDW..DF10_BR_DFR_REQ DF10
				INNER JOIN UDW..LN50_BR_DFR_APV LN50 
					ON LN50.BF_SSN = DF10.BF_SSN 
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
			WHERE
				DF10.LC_DFR_STA = 'A'
				AND DF10.LC_STA_DFR10 = 'A'
				AND DF10.LC_DFR_TYP = 'D29'
				AND LN50.LC_STA_LON50 = 'A'
				AND LN50.LC_DFR_RSP != '003' -- 003 = DEFERMENT REQUEST DENIED
				AND LN50.LD_DFR_BEG <= GETDATE()
		) EconDefer
			ON EconDefer.BF_SSN = LN10.BF_SSN 
			AND EconDefer.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN
		( -- Deferment months already used
			SELECT DISTINCT
				LN50.BF_SSN,
				LN50.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN50.LD_DFR_BEG, DATEADD(DAY,1,LN50.LD_DFR_END))/365.00)*12) OVER (PARTITION BY LN50.BF_SSN, LN50.LN_SEQ), 1) AS [TotalDfrMonthsUsed]
			FROM
				UDW..DF10_BR_DFR_REQ DF10
				INNER JOIN UDW..LN50_BR_DFR_APV LN50 
					ON LN50.BF_SSN = DF10.BF_SSN 
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
			WHERE
				DF10.LC_DFR_STA = 'A'
				AND DF10.LC_STA_DFR10 = 'A'
				AND DF10.LC_DFR_TYP = 'D13'
				AND LN50.LC_STA_LON50 = 'A'
				AND LN50.LC_DFR_RSP != '003' -- 003 = DEFERMENT REQUEST DENIED
				AND LN50.LD_DFR_BEG <= GETDATE()
		) UnempDefer
			ON UnempDefer.BF_SSN = LN10.BF_SSN 
			AND UnempDefer.LN_SEQ = LN10.LN_SEQ 
		LEFT JOIN
		( -- Forb months already used
			SELECT DISTINCT
				LN60.BF_SSN,
				LN60.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN60.LD_FOR_BEG, DATEADD(DAY,1,LN60.LD_FOR_END))/365.00)*12) OVER (PARTITION BY LN60.BF_SSN, LN60.LN_SEQ), 1) AS [TotalFrbMonthsUsed]
			FROM
				UDW..FB10_BR_FOR_REQ FB10
				INNER JOIN UDW..LN60_BR_FOR_APV LN60 
					ON LN60.BF_SSN = FB10.BF_SSN
					AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
			WHERE
				FB10.LC_FOR_STA = 'A'
				AND FB10.LC_STA_FOR10 = 'A'
				AND FB10.LC_FOR_TYP = '05'
				AND LN60.LC_STA_LON60 = 'A'
				AND LN60.LC_FOR_RSP != '003' --003 = FORBEARANCE REQUEST DENIED
				AND LN60.LD_FOR_BEG <= GETDATE()
		) HardForb
			ON HardForb.BF_SSN = LN10.BF_SSN
			AND HardForb.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN
		(
			SELECT 
				NCH.AccountIdentifier, 
				NCH.ActivityDate 
			FROM 
				NobleCalls..NobleCallHistory NCH
				INNER JOIN NobleCalls..DispositionCodeMapping DCM
					ON DCM.DispositionCode = NCH.DispositionCode
				INNER JOIN NobleCalls..ResponseCodes RC
					ON RC.ResponseCodeId = DCM.ResponseCodeId
					AND RC.ResponseCode = 'CNTCT'
			WHERE
				CAST(NCH.ActivityDate AS DATE) BETWEEN CAST(DATEADD(DAY,-7,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE) --last 7 days
		) ContactLastSeven
			ON ContactLastSeven.AccountIdentifier = PD10.DF_SPE_ACC_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				NCH.AccountIdentifier, 
				COUNT(*) AS NumberAttempts
			FROM 
				NobleCalls..NobleCallHistory NCH
				INNER JOIN NobleCalls..DispositionCodeMapping DCM
					ON DCM.DispositionCode = NCH.DispositionCode
				INNER JOIN NobleCalls..ResponseCodes RC
					ON RC.ResponseCodeId = DCM.ResponseCodeId
					AND RC.ResponseCode IN('INVPH', 'NOCTC')
			WHERE
				CAST(NCH.ActivityDate AS DATE) BETWEEN CAST(DATEADD(DAY,-7,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE) --last 7 days
			GROUP BY
				NCH.AccountIdentifier
		) OneAttemptLastSeven
			ON OneAttemptLastSeven.AccountIdentifier = PD10.DF_SPE_ACC_ID
			AND OneAttemptLastSeven.NumberAttempts >= 1
		LEFT JOIN 
		(
			SELECT
				NCH.AccountIdentifier,
				MAX(NCH.ActivityDate) AS DateOfLastCall
			FROM
				NobleCalls..NobleCallHistory NCH
			GROUP BY
				NCH.AccountIdentifier
		) LastCallDate
			ON LastCallDate.AccountIdentifier = PD10.DF_SPE_ACC_ID
	WHERE
		LN10.LC_STA_LON10 = 'R'
		AND ISNULL(LN10.LA_CUR_PRI,0.00) + ISNULL(DW01.WA_TOT_BRI_OTS,0.00) > 0.00
		AND LN10.LI_CON_PAY_STP_PUR != 'Y'
		AND ISNULL(DID.LD_ATY_REQ_RCV,'2099-01-01') > ISNULL(AUTO.LD_ATY_REQ_RCV,'1900-01-01')
		AND ARC_BRPTP.BF_SSN IS NULL /*exclude future arc date*/
		AND WQ20.BF_SSN IS NULL /*exclude pending queue*/
		AND CT30.DF_PRS_ID_BR IS NULL /*exclude pending one link task*/
		AND ContactLastSeven.AccountIdentifier IS NULL /*exclude successful contacts in last week*/
		AND OneAttemptLastSeven.AccountIdentifier IS NULL /*exclude 3 failed contacts in last week*/
) ADH --AutoDelq-High
ORDER BY
	ADH.DateOfLastCall ASC



GO
CREATE PROCEDURE [dbo].[SsrsUTLWS02_Blast]
	
AS
--R4
--Blast
SELECT
	RecipientAccountNumber,
	Delinquency,
	RecipientDescription,
	BorrowerAccountNumber,
	TCPAConsentForHighestRankedPhone,
	HighestRankedPhone,
	TCPAConsentForSecondHighestRankedPhone,
	SecondHighestPhone,
	TCPAConsentForThirdHighestRankedPhone,
	ThirdHighestPhone,
	[State],
	ZIP,
	AmountDue,
	TotalBalance,
	BorrowerAccountNumber2,
	Portfolio,
	TotalForbearanceTime,
	TotalEconomicHardshipDefer,
	TotalUnemploymentDefer,
	RecipientName,
	BorrowerName,
	DateOfLastCall
FROM
(
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID AS RecipientAccountNumber,
		MAX(LN16.LN_DLQ_MAX) OVER(PARTITION BY LN10.BF_SSN) + 1 AS Delinquency,
		'BORROWER' AS RecipientDescription,
		PD10.DF_SPE_ACC_ID AS BorrowerAccountNumber,
		PD42.PHN_H_CON AS TCPAConsentForHighestRankedPhone,
		PD42.PHN_H AS HighestRankedPhone,
		PD42.PHN_A_CON AS TCPAConsentForSecondHighestRankedPhone,
		PD42.PHN_A AS SecondHighestPhone,
		PD42.PHN_W_CON AS TCPAConsentForThirdHighestRankedPhone,
		PD42.PHN_W AS ThirdHighestPhone,
		PD30.DC_DOM_ST AS [State],
		LEFT(ISNULL(PD30.DF_ZIP_CDE, '00000'),5) AS ZIP,
		ISNULL(AmountDue.AMTDU,0.00) AS AmountDue, --Dialer requires no decimals
		SUM(ISNULL(LN10.LA_CUR_PRI,0.00) + ISNULL(DW01.WA_TOT_BRI_OTS,0.00) + ISNULL(AmountDue.LA_LTE_FEE_OTS_PRT,0.00)) OVER(PARTITION BY PD10.DF_SPE_ACC_ID) AS TotalBalance, --Dialer requires no decimals
		PD10.DF_SPE_ACC_ID AS BorrowerAccountNumber2,
		CASE WHEN LN10.LF_LON_CUR_OWN = '900749' THEN 'CLP'
			 WHEN LN10.LF_LON_CUR_OWN = '971357' THEN 'TILP'
			 WHEN LN10.LF_LON_CUR_OWN LIKE '829769%' THEN 'BANA'
			 WHEN LN10.LF_LON_CUR_OWN IN('826717','830248') THEN 'ALIGN'
			 WHEN LN10.LF_LON_CUR_OWN = '828476' THEN 'UTAH'
			 ELSE LN10.LF_LON_CUR_OWN 
		END AS Portfolio,
		ISNULL(MAX(HardForb.TotalFrbMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalForbearanceTime, 
		ISNULL(MAX(EconDefer.TotalDfrMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalEconomicHardshipDefer, 
		ISNULL(MAX(UnempDefer.TotalDfrMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalUnemploymentDefer, 
		RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_MID) + ' ' + RTRIM(PD10.DM_PRS_LST) AS [RecipientName],
		RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_MID) + ' ' + RTRIM(PD10.DM_PRS_LST) AS [BorrowerName],
		ISNULL(LastCallDate.DateOfLastCall, '1900-01-01') AS DateOfLastCall
	FROM 
		UDW..LN10_LON LN10
		INNER JOIN UDW..LN16_LON_DLQ_HST LN16
			ON LN16.BF_SSN = LN10.BF_SSN
			AND LN16.LN_SEQ = LN10.LN_SEQ
			AND LN16.LC_STA_LON16 = '1'
			AND DATEDIFF(DAY, LN16.LD_DLQ_MAX, GETDATE()) <= 5
			AND DATEDIFF(DAY, LN16.LD_DLQ_OCC, GETDATE()) > 10
			AND (LN16.LN_DLQ_MAX + 1) % 2 = 0 --Gets even delq 
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = LN10.BF_SSN
		INNER JOIN 
		(
			SELECT DISTINCT
				DF_PRS_ID,
				MAX(IIF(OverallRank = 1, PHN, '')) AS PHN_H,
				MAX(IIF(OverallRank = 1, DC_ALW_ADL_PHN, '')) AS PHN_H_CON,
				MAX(IIF(OverallRank = 2, PHN, '')) AS PHN_A,
				MAX(IIF(OverallRank = 2, DC_ALW_ADL_PHN, '')) AS PHN_A_CON,
				MAX(IIF(OverallRank = 3, PHN, '')) AS PHN_W,
				MAX(IIF(OverallRank = 3, DC_ALW_ADL_PHN, '')) AS PHN_W_CON
			FROM
			(
				SELECT DISTINCT
					PD42.DF_PRS_ID,
					ISNULL(PD42.DN_DOM_PHN_ARA,'') + ISNULL(PD42.DN_DOM_PHN_XCH,'') + ISNULL(PD42.DN_DOM_PHN_LCL,'') AS PHN,
					PD42.DC_PHN,
					CASE WHEN PD42.DC_ALW_ADL_PHN IN ('L','X','P') 
						THEN 'Y'
						ELSE 'N'
					END AS DC_ALW_ADL_PHN,
					PD42.DC_ALW_ADL_PHN AS Consent,
					/*Rank phones by category and type*/
					DENSE_RANK() OVER (PARTITION BY PD42.DF_PRS_ID ORDER BY Ranks.ConsentRank, Ranks.TypeRank) AS OverallRank
				FROM
					UDW..PD42_PRS_PHN PD42
					INNER JOIN
					(
						SELECT
							DF_PRS_ID,
							DC_PHN,
							DC_ALW_ADL_PHN,
							CASE WHEN DC_PHN = 'H' THEN 1
									WHEN DC_PHN = 'A' THEN 2
									WHEN DC_PHN = 'W' THEN 3
									WHEN DC_PHN = 'M' THEN 4
							END AS TypeRank, --H A W M TYPE
							CASE WHEN DC_ALW_ADL_PHN = 'N' THEN 1
									WHEN DC_ALW_ADL_PHN = 'P' THEN 2 
									WHEN DC_ALW_ADL_PHN = 'X' THEN 3
									WHEN DC_ALW_ADL_PHN = 'L' THEN 4
									WHEN DC_ALW_ADL_PHN = 'Q' THEN 5
									WHEN DC_ALW_ADL_PHN = 'U' THEN 6 
							END AS ConsentRank--N P X L Q U CONSENT
						FROM
							UDW..PD42_PRS_PHN PD42
						WHERE
							PD42.DI_PHN_VLD = 'Y'
							AND PD42.DC_NO_HME_PHN != 'J'
							AND PD42.DC_ALW_ADL_PHN IN ('L','X','P') --Consent
					) Ranks
						ON Ranks.DF_PRS_ID = PD42.DF_PRS_ID
						AND Ranks.DC_PHN = PD42.DC_PHN
						AND Ranks.DC_ALW_ADL_PHN = PD42.DC_ALW_ADL_PHN
				WHERE
					PD42.DI_PHN_VLD = 'Y'
					AND PD42.DC_NO_HME_PHN != 'J'
					AND PD42.DC_ALW_ADL_PHN IN ('L','X','P') --Consent
			) Ranked
			WHERE
				Ranked.OverallRank IN (1,2,3)
			GROUP BY
				DF_PRS_ID
		) PD42
			ON PD42.DF_PRS_ID = LN10.BF_SSN
			AND 
			(
				PD42.PHN_H != ''
				OR PD42.PHN_A != ''
				OR PD42.PHN_W != ''
			)
		INNER JOIN UDW..DW01_DW_CLC_CLU DW01
			ON DW01.BF_SSN = LN10.BF_SSN
			AND DW01.LN_SEQ = LN10.LN_SEQ
			AND DW01.WC_DW_LON_STA != '23'
			AND DW01.WC_DW_LON_STA IN ('03','04','05','08','14')
			AND RTRIM(DW01.WX_OVR_DW_LON_STA) NOT IN('PENDING ID THEFT','REALLOCATED','CNSLD-STOP PURSUIT')
		LEFT JOIN UDW..PD30_PRS_ADR PD30
			ON PD30.DF_PRS_ID = LN10.BF_SSN
			AND PD30.DC_ADR = 'L'
		LEFT JOIN UDW..WQ20_TSK_QUE WQ20
			ON WQ20.BF_SSN = LN10.BF_SSN
			AND WQ20.WF_QUE IN ('2A', '2P', 'GP', '9R', 'SI', 'S4', 'PR', 'VR', 'VB', 'MF', '1P', 'SF', 'DU', 'AT', '87')
			AND WQ20.WC_STA_WQUE20 IN ('A', 'H', 'P', 'U', 'W')
		LEFT JOIN ODW..CT30_CALL_QUE CT30
			ON CT30.DF_PRS_ID_BR = LN10.BF_SSN
			AND CT30.IF_WRK_GRP IN ('ASBKP', 'ASCON', 'ASMOC')
			AND CT30.IF_WRK_GRP IN ('A', 'W')	
		LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY DOC ID*/
		(
			SELECT
				BF_SSN,
				MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
			FROM
				UDW..AY10_BR_LON_ATY
			WHERE
				PF_REQ_ACT = 'DIDDA'
				AND DATEDIFF(DAY, LD_ATY_REQ_RCV, GETDATE()) <= 14
				AND LC_STA_ACTY10 = 'A'
			GROUP BY
				BF_SSN
		)DID 
			ON DID.BF_SSN = LN10.BF_SSN
		LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY APPROVAL*/
		(
			SELECT
				BF_SSN,
				MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
			FROM
				UDW..AY10_BR_LON_ATY
			WHERE
				PF_REQ_ACT = 'PAUTO'
				AND LC_STA_ACTY10 = 'A'
			GROUP BY
				BF_SSN
		) AUTO 
			ON AUTO.BF_SSN = LN10.BF_SSN
		LEFT JOIN /*Gets amount due from billing tables*/
		(
			SELECT 
				LN80.BF_SSN,
				SUM(ISNULL(LN80.LA_BIL_PAS_DU,0.00) + ISNULL(LN80.LA_BIL_DU_PRT,0.00) + ISNULL(LN80.LA_LTE_FEE_OTS_PRT,0.00) - ISNULL(LN80.LA_TOT_BIL_STS,0.00)) AS AMTDU,
				SUM(ISNULL(LN80.LA_LTE_FEE_OTS_PRT,0.00)) AS LA_LTE_FEE_OTS_PRT
			FROM 
				UDW..LN80_LON_BIL_CRF LN80
				INNER JOIN 
				(
					SELECT 
						BF_SSN,
						MAX(LD_BIL_CRT) AS LD_BIL_CRT
					FROM 
						UDW..BL10_BR_BIL
					WHERE 
						LC_STA_BIL10 = 'A'
						AND LC_BIL_TYP = 'P'
					GROUP BY 
						BF_SSN
				) BL10
					ON BL10.BF_SSN = LN80.BF_SSN
					AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
			WHERE 
				LN80.LC_STA_LON80 = 'A'
			GROUP BY 
				LN80.BF_SSN
		) AmountDue 
			ON AmountDue.BF_SSN = LN10.BF_SSN
		LEFT JOIN 
		(/*exclude future arc date*/
			SELECT DISTINCT
				BF_SSN,
				LD_ATY_REQ_RCV,
				PARSED_DAYS,
				PARSED_DAYS AS COMMENT_DAYS, /*takes lesser of 14 or parsed days*/
				DATEADD(DAY,PARSED_DAYS, LD_ATY_REQ_RCV) AS CALC_DATE,	
				CASE WHEN CAST(DATEADD(DAY,PARSED_DAYS, LD_ATY_REQ_RCV) AS DATE) > CAST(GETDATE() AS DATE)
					THEN 1
					ELSE 0
				END AS FUTURE_DATE
			FROM
			(/*parse out numeric days from any extraneous text*/
				SELECT 
					AY10.BF_SSN,
					AY10.LD_ATY_REQ_RCV,
					AY20.LX_ATY,
					IIF(COALESCE(TRY_CAST(LTRIM(RTRIM(AY20.LX_ATY)) AS INT),14) >= 14, 14, COALESCE(TRY_CAST(LTRIM(RTRIM(AY20.LX_ATY)) AS INT),14)) AS PARSED_DAYS	
				FROM
					UDW..AY10_BR_LON_ATY AY10
					LEFT JOIN UDW..AY20_ATY_TXT AY20
						ON AY20.BF_SSN = AY10.BF_SSN
						AND AY20.LN_ATY_SEQ = AY10.LN_ATY_SEQ
				WHERE
					AY10.LC_STA_ACTY10 = 'A'
					AND AY10.PF_REQ_ACT = 'BRPTP' /*borrower promise to repay*/
			) P
		) ARC_BRPTP 
			ON ARC_BRPTP.BF_SSN = LN10.BF_SSN
			AND ARC_BRPTP.FUTURE_DATE = 1 /*exclude future arc date*/
		LEFT JOIN
		( -- Deferment months already used
			SELECT DISTINCT
				LN50.BF_SSN,
				LN50.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN50.LD_DFR_BEG, DATEADD(DAY,1,LN50.LD_DFR_END))/365.00)*12) OVER (PARTITION BY LN50.BF_SSN, LN50.LN_SEQ), 1) AS [TotalDfrMonthsUsed]
			FROM
				UDW..DF10_BR_DFR_REQ DF10
				INNER JOIN UDW..LN50_BR_DFR_APV LN50 
					ON LN50.BF_SSN = DF10.BF_SSN 
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
			WHERE
				DF10.LC_DFR_STA = 'A'
				AND DF10.LC_STA_DFR10 = 'A'
				AND DF10.LC_DFR_TYP = 'D29'
				AND LN50.LC_STA_LON50 = 'A'
				AND LN50.LC_DFR_RSP != '003' -- 003 = DEFERMENT REQUEST DENIED
				AND LN50.LD_DFR_BEG <= GETDATE()
		) EconDefer
			ON EconDefer.BF_SSN = LN10.BF_SSN 
			AND EconDefer.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN
		( -- Deferment months already used
			SELECT DISTINCT
				LN50.BF_SSN,
				LN50.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN50.LD_DFR_BEG, DATEADD(DAY,1,LN50.LD_DFR_END))/365.00)*12) OVER (PARTITION BY LN50.BF_SSN, LN50.LN_SEQ), 1) AS [TotalDfrMonthsUsed]
			FROM
				UDW..DF10_BR_DFR_REQ DF10
				INNER JOIN UDW..LN50_BR_DFR_APV LN50 
					ON LN50.BF_SSN = DF10.BF_SSN 
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
			WHERE
				DF10.LC_DFR_STA = 'A'
				AND DF10.LC_STA_DFR10 = 'A'
				AND DF10.LC_DFR_TYP = 'D13'
				AND LN50.LC_STA_LON50 = 'A'
				AND LN50.LC_DFR_RSP != '003' -- 003 = DEFERMENT REQUEST DENIED
				AND LN50.LD_DFR_BEG <= GETDATE()
		) UnempDefer
			ON UnempDefer.BF_SSN = LN10.BF_SSN 
			AND UnempDefer.LN_SEQ = LN10.LN_SEQ 
		LEFT JOIN
		( -- Forb months already used
			SELECT DISTINCT
				LN60.BF_SSN,
				LN60.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN60.LD_FOR_BEG, DATEADD(DAY,1,LN60.LD_FOR_END))/365.00)*12) OVER (PARTITION BY LN60.BF_SSN, LN60.LN_SEQ), 1) AS [TotalFrbMonthsUsed]
			FROM
				UDW..FB10_BR_FOR_REQ FB10
				INNER JOIN UDW..LN60_BR_FOR_APV LN60 
					ON LN60.BF_SSN = FB10.BF_SSN
					AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
			WHERE
				FB10.LC_FOR_STA = 'A'
				AND FB10.LC_STA_FOR10 = 'A'
				AND FB10.LC_FOR_TYP = '05'
				AND LN60.LC_STA_LON60 = 'A'
				AND LN60.LC_FOR_RSP != '003' --003 = FORBEARANCE REQUEST DENIED
				AND LN60.LD_FOR_BEG <= GETDATE()
		) HardForb
			ON HardForb.BF_SSN = LN10.BF_SSN
			AND HardForb.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN
		(
			SELECT 
				NCH.AccountIdentifier, 
				NCH.ActivityDate 
			FROM 
				NobleCalls..NobleCallHistory NCH
				INNER JOIN NobleCalls..DispositionCodeMapping DCM
					ON DCM.DispositionCode = NCH.DispositionCode
				INNER JOIN NobleCalls..ResponseCodes RC
					ON RC.ResponseCodeId = DCM.ResponseCodeId
					AND RC.ResponseCode = 'CNTCT'
			WHERE
				CAST(NCH.ActivityDate AS DATE) BETWEEN CAST(DATEADD(DAY,-7,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE) --last 7 days
		) ContactLastSeven
			ON ContactLastSeven.AccountIdentifier = PD10.DF_SPE_ACC_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				NCH.AccountIdentifier, 
				COUNT(*) AS NumberAttempts
			FROM 
				NobleCalls..NobleCallHistory NCH
				INNER JOIN NobleCalls..DispositionCodeMapping DCM
					ON DCM.DispositionCode = NCH.DispositionCode
				INNER JOIN NobleCalls..ResponseCodes RC
					ON RC.ResponseCodeId = DCM.ResponseCodeId
					AND RC.ResponseCode IN('INVPH', 'NOCTC')
			WHERE
				CAST(NCH.ActivityDate AS DATE) BETWEEN CAST(DATEADD(DAY,-7,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE) --last 7 days
			GROUP BY
				NCH.AccountIdentifier
		) OneAttemptLastSeven
			ON OneAttemptLastSeven.AccountIdentifier = PD10.DF_SPE_ACC_ID
			AND OneAttemptLastSeven.NumberAttempts >= 1
		LEFT JOIN 
		(
			SELECT
				NCH.AccountIdentifier,
				MAX(NCH.ActivityDate) AS DateOfLastCall
			FROM
				NobleCalls..NobleCallHistory NCH
			GROUP BY
				NCH.AccountIdentifier
		) LastCallDate
			ON LastCallDate.AccountIdentifier = PD10.DF_SPE_ACC_ID
	WHERE
		LN10.LC_STA_LON10 = 'R'
		AND ISNULL(LN10.LA_CUR_PRI,0.00) + ISNULL(DW01.WA_TOT_BRI_OTS,0.00) > 0.00
		AND LN10.LI_CON_PAY_STP_PUR != 'Y'
		AND ISNULL(DID.LD_ATY_REQ_RCV,'2099-01-01') > ISNULL(AUTO.LD_ATY_REQ_RCV,'1900-01-01')
		AND ARC_BRPTP.BF_SSN IS NULL /*exclude future arc date*/
		AND WQ20.BF_SSN IS NULL /*exclude pending queue*/
		AND CT30.DF_PRS_ID_BR IS NULL /*exclude pending one link task*/
		AND ContactLastSeven.AccountIdentifier IS NULL /*exclude successful contacts in last week*/
		AND OneAttemptLastSeven.AccountIdentifier IS NULL /*exclude 3 failed contacts in last week*/

	UNION ALL

	/*	begin DELQ_EDR*/
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID AS RecipientAccountNumber,
		MAX(LN16.LN_DLQ_MAX) OVER(PARTITION BY LN10.BF_SSN) + 1 AS Delinquency,
		CASE WHEN LC_EDS_TYP = 'M' THEN 'COBORROWER'
			 WHEN LC_EDS_TYP = 'S' THEN 'ENDORSER'
			 ELSE ''
		END AS RecipientDescription,
		PD10Borr.DF_SPE_ACC_ID AS BorrowerAccountNumber,
		PD42.PHN_H_CON AS TCPAConsentForHighestRankedPhone,
		PD42.PHN_H AS HighestRankedPhone,
		PD42.PHN_A_CON AS TCPAConsentForSecondHighestRankedPhone,
		PD42.PHN_A AS SecondHighestPhone,
		PD42.PHN_W_CON AS TCPAConsentForThirdHighestRankedPhone,
		PD42.PHN_W AS ThirdHighestPhone,
		PD30.DC_DOM_ST AS [State],
		LEFT(ISNULL(PD30.DF_ZIP_CDE, '00000'),5) AS ZIP,
		ISNULL(AmountDue.AMTDU,0.00) AS AmountDue, --Dialer requires no decimals
		SUM(ISNULL(LN10.LA_CUR_PRI,0.00) + ISNULL(DW01.WA_TOT_BRI_OTS,0.00) + ISNULL(AmountDue.LA_LTE_FEE_OTS_PRT,0.00)) OVER(PARTITION BY PD10.DF_SPE_ACC_ID) AS TotalBalance, --Dialer requires no decimals
		PD10Borr.DF_SPE_ACC_ID AS BorrowerAccountNumber2,
		CASE WHEN LN10.LF_LON_CUR_OWN = '900749' THEN 'CLP'
			 WHEN LN10.LF_LON_CUR_OWN = '971357' THEN 'TILP'
			 WHEN LN10.LF_LON_CUR_OWN LIKE '829769%' THEN 'BANA'
			 WHEN LN10.LF_LON_CUR_OWN IN('826717','830248') THEN 'ALIGN'
			 WHEN LN10.LF_LON_CUR_OWN = '828476' THEN 'UTAH'
			 ELSE LN10.LF_LON_CUR_OWN 
		END AS Portfolio,
		ISNULL(MAX(HardForb.TotalFrbMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalForbearanceTime, 
		ISNULL(MAX(EconDefer.TotalDfrMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalEconomicHardshipDefer, 
		ISNULL(MAX(UnempDefer.TotalDfrMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalUnemploymentDefer, 
		RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_MID) + ' ' + RTRIM(PD10.DM_PRS_LST) AS [RecipientName],
		RTRIM(PD10Borr.DM_PRS_1) + ' ' + RTRIM(PD10Borr.DM_PRS_MID) + ' ' + RTRIM(PD10Borr.DM_PRS_LST) AS [BorrowerName],
		ISNULL(LastCallDate.DateOfLastCall, '1900-01-01') AS DateOfLastCall
	FROM 
		UDW..LN10_LON LN10
		INNER JOIN UDW..LN20_EDS LN20
			ON LN20.BF_SSN = LN10.BF_SSN
			AND LN20.LN_SEQ = LN10.LN_SEQ
			AND LN20.LC_STA_LON20 = 'A'								
		INNER JOIN UDW..LN16_LON_DLQ_HST LN16
			ON LN16.BF_SSN = LN10.BF_SSN
			AND LN16.LN_SEQ = LN10.LN_SEQ
			AND LN16.LC_STA_LON16 = '1'
			AND DATEDIFF(DAY, LN16.LD_DLQ_MAX, GETDATE()) <= 5
			AND (LN16.LN_DLQ_MAX + 1) % 2 = 0 --Gets even delq 
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = LN20.LF_EDS
		INNER JOIN UDW..PD10_PRS_NME PD10Borr
			ON PD10Borr.DF_PRS_ID = LN10.BF_SSN
		INNER JOIN
		(
			SELECT DISTINCT
				DF_PRS_ID,
				MAX(IIF(OverallRank = 1, PHN, '')) AS PHN_H,
				MAX(IIF(OverallRank = 1, DC_ALW_ADL_PHN, '')) AS PHN_H_CON,
				MAX(IIF(OverallRank = 2, PHN, '')) AS PHN_A,
				MAX(IIF(OverallRank = 2, DC_ALW_ADL_PHN, '')) AS PHN_A_CON,
				MAX(IIF(OverallRank = 3, PHN, '')) AS PHN_W,
				MAX(IIF(OverallRank = 3, DC_ALW_ADL_PHN, '')) AS PHN_W_CON
			FROM
			(
				SELECT DISTINCT
					PD42.DF_PRS_ID,
					ISNULL(PD42.DN_DOM_PHN_ARA,'') + ISNULL(PD42.DN_DOM_PHN_XCH,'') + ISNULL(PD42.DN_DOM_PHN_LCL,'') AS PHN,
					PD42.DC_PHN,
					CASE WHEN PD42.DC_ALW_ADL_PHN IN ('L','X','P') 
						THEN 'Y'
						ELSE 'N'
					END AS DC_ALW_ADL_PHN,
					PD42.DC_ALW_ADL_PHN AS Consent,
					/*Rank phones by category and type*/
					DENSE_RANK() OVER (PARTITION BY PD42.DF_PRS_ID ORDER BY Ranks.ConsentRank, Ranks.TypeRank) AS OverallRank
				FROM
					UDW..PD42_PRS_PHN PD42
					INNER JOIN
					(
						SELECT
							DF_PRS_ID,
							DC_PHN,
							DC_ALW_ADL_PHN,
							CASE WHEN DC_PHN = 'H' THEN 1
									WHEN DC_PHN = 'A' THEN 2
									WHEN DC_PHN = 'W' THEN 3
									WHEN DC_PHN = 'M' THEN 4
							END AS TypeRank, --H A W M TYPE
							CASE WHEN DC_ALW_ADL_PHN = 'N' THEN 1
									WHEN DC_ALW_ADL_PHN = 'P' THEN 2 
									WHEN DC_ALW_ADL_PHN = 'X' THEN 3
									WHEN DC_ALW_ADL_PHN = 'L' THEN 4
									WHEN DC_ALW_ADL_PHN = 'Q' THEN 5
									WHEN DC_ALW_ADL_PHN = 'U' THEN 6 
							END AS ConsentRank--N P X L Q U CONSENT
						FROM
							UDW..PD42_PRS_PHN PD42
						WHERE
							PD42.DI_PHN_VLD = 'Y'
							AND PD42.DC_NO_HME_PHN != 'J'
							AND PD42.DC_ALW_ADL_PHN IN ('L','X','P') --Consent
					) Ranks
						ON Ranks.DF_PRS_ID = PD42.DF_PRS_ID
						AND Ranks.DC_PHN = PD42.DC_PHN
						AND Ranks.DC_ALW_ADL_PHN = PD42.DC_ALW_ADL_PHN
				WHERE
					PD42.DI_PHN_VLD = 'Y'
					AND PD42.DC_NO_HME_PHN != 'J'
					AND PD42.DC_ALW_ADL_PHN IN ('L','X','P') --Consent
			) Ranked
			WHERE
				Ranked.OverallRank IN (1,2,3)
			GROUP BY
				DF_PRS_ID
		) PD42 
			ON PD42.DF_PRS_ID = LN20.LF_EDS
			AND 
			(
				PD42.PHN_H != ''
				OR PD42.PHN_A != ''
				OR PD42.PHN_W != ''
			)
		INNER JOIN UDW..DW01_DW_CLC_CLU DW01
			ON DW01.BF_SSN = LN10.BF_SSN
			AND DW01.LN_SEQ = LN10.LN_SEQ
			AND DW01.WC_DW_LON_STA != '23'
			AND DW01.WC_DW_LON_STA IN ('03','04','05','08','14')
			AND RTRIM(DW01.WX_OVR_DW_LON_STA) NOT IN('PENDING ID THEFT','REALLOCATED','CNSLD-STOP PURSUIT')
		LEFT JOIN UDW..PD30_PRS_ADR PD30
			ON PD30.DF_PRS_ID = LN20.LF_EDS
			AND PD30.DC_ADR = 'L'
		LEFT JOIN UDW..WQ20_TSK_QUE WQ20
			ON WQ20.BF_SSN = LN10.BF_SSN
			AND WQ20.WF_QUE IN ('2A', '2P', 'GP', '9R', 'SI', 'S4', 'PR', 'VR', 'VB', 'MF', '1P', 'SF', 'DU', 'AT', '87')
			AND WQ20.WC_STA_WQUE20 IN ('A', 'H', 'P', 'U', 'W')
		LEFT JOIN ODW..CT30_CALL_QUE CT30
			ON CT30.DF_PRS_ID_BR = LN10.BF_SSN
			AND CT30.IF_WRK_GRP IN ('ASBKP', 'ASCON', 'ASMOC')
			AND CT30.IF_WRK_GRP IN ('A', 'W')	
		LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY DOC ID*/
		(
			SELECT
				BF_SSN,
				MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
			FROM
				UDW..AY10_BR_LON_ATY
			WHERE
				PF_REQ_ACT = 'DIDDA'
				AND DATEDIFF(DAY, LD_ATY_REQ_RCV, GETDATE()) <= 14
				AND LC_STA_ACTY10 = 'A'
			GROUP BY
				BF_SSN
		) DID 
			ON DID.BF_SSN = LN10.BF_SSN
		LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY APPROVAL*/
		(
			SELECT
				BF_SSN,
				MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
			FROM
				UDW..AY10_BR_LON_ATY
			WHERE
				PF_REQ_ACT = 'PAUTO'
				AND LC_STA_ACTY10 = 'A'
			GROUP BY
				BF_SSN
		) AUTO 
			ON AUTO.BF_SSN = LN10.BF_SSN
		LEFT JOIN /*Gets amount due from billing tables*/
		(
			SELECT 
				LN80.BF_SSN,
				SUM(ISNULL(LN80.LA_BIL_PAS_DU,0.00) + ISNULL(LN80.LA_BIL_DU_PRT,0.00) + ISNULL(LN80.LA_LTE_FEE_OTS_PRT,0.00) - ISNULL(LN80.LA_TOT_BIL_STS,0.00)) AS AMTDU,
				SUM(ISNULL(LN80.LA_LTE_FEE_OTS_PRT,0.00)) AS LA_LTE_FEE_OTS_PRT
			FROM 
				UDW..LN80_LON_BIL_CRF LN80
				INNER JOIN 
				(
					SELECT 
						BF_SSN,
						MAX(LD_BIL_CRT) AS LD_BIL_CRT
					FROM 
						UDW..BL10_BR_BIL
					WHERE 
						LC_STA_BIL10 = 'A'
						AND LC_BIL_TYP = 'P'
					GROUP BY 
						BF_SSN
				) BL10
					ON BL10.BF_SSN = LN80.BF_SSN
					AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
			WHERE 
				LN80.LC_STA_LON80 = 'A'
			GROUP BY 
				LN80.BF_SSN
		) AmountDue 
			ON AmountDue.BF_SSN = LN10.BF_SSN
		LEFT JOIN 
		(/*exclude future arc date*/
			SELECT DISTINCT
				BF_SSN,
				LD_ATY_REQ_RCV,
				PARSED_DAYS,
				PARSED_DAYS AS COMMENT_DAYS, /*takes lesser of 14 or parsed days*/
				DATEADD(DAY,PARSED_DAYS, LD_ATY_REQ_RCV) AS CALC_DATE,	
				CASE WHEN CAST(DATEADD(DAY,PARSED_DAYS, LD_ATY_REQ_RCV) AS DATE) > CAST(GETDATE() AS DATE)
					THEN 1
					ELSE 0
				END AS FUTURE_DATE
			FROM
			(/*parse out numeric days from any extraneous text*/
				SELECT 
					AY10.BF_SSN,
					AY10.LD_ATY_REQ_RCV,
					AY20.LX_ATY,
					IIF(COALESCE(TRY_CAST(LTRIM(RTRIM(AY20.LX_ATY)) AS INT),14) >= 14, 14, COALESCE(TRY_CAST(LTRIM(RTRIM(AY20.LX_ATY)) AS INT),14)) AS PARSED_DAYS	
				FROM
					UDW..AY10_BR_LON_ATY AY10
					LEFT JOIN UDW..AY20_ATY_TXT AY20
						ON AY20.BF_SSN = AY10.BF_SSN
						AND AY20.LN_ATY_SEQ = AY10.LN_ATY_SEQ
				WHERE
					AY10.LC_STA_ACTY10 = 'A'
					AND AY10.PF_REQ_ACT = 'BRPTP' /*borrower promise to repay*/
			) P
		) ARC_BRPTP 
			ON LN10.BF_SSN = ARC_BRPTP.BF_SSN
			AND ARC_BRPTP.FUTURE_DATE = 1 /*exclude future arc date*/
		LEFT JOIN
		( -- Deferment months already used
			SELECT DISTINCT
				LN50.BF_SSN,
				LN50.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN50.LD_DFR_BEG, DATEADD(DAY,1,LN50.LD_DFR_END))/365.00)*12) OVER (PARTITION BY LN50.BF_SSN, LN50.LN_SEQ), 1) AS [TotalDfrMonthsUsed]
			FROM
				UDW..DF10_BR_DFR_REQ DF10
				INNER JOIN UDW..LN50_BR_DFR_APV LN50 
					ON LN50.BF_SSN = DF10.BF_SSN 
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
			WHERE
				DF10.LC_DFR_STA = 'A'
				AND DF10.LC_STA_DFR10 = 'A'
				AND DF10.LC_DFR_TYP = 'D29'
				AND LN50.LC_STA_LON50 = 'A'
				AND LN50.LC_DFR_RSP != '003' -- 003 = DEFERMENT REQUEST DENIED
				AND LN50.LD_DFR_BEG <= GETDATE()
		) EconDefer
			ON EconDefer.BF_SSN = LN10.BF_SSN 
			AND EconDefer.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN
		( -- Deferment months already used
			SELECT DISTINCT
				LN50.BF_SSN,
				LN50.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN50.LD_DFR_BEG, DATEADD(DAY,1,LN50.LD_DFR_END))/365.00)*12) OVER (PARTITION BY LN50.BF_SSN, LN50.LN_SEQ), 1) AS [TotalDfrMonthsUsed]
			FROM
				UDW..DF10_BR_DFR_REQ DF10
				INNER JOIN UDW..LN50_BR_DFR_APV LN50 
					ON LN50.BF_SSN = DF10.BF_SSN 
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
			WHERE
				DF10.LC_DFR_STA = 'A'
				AND DF10.LC_STA_DFR10 = 'A'
				AND DF10.LC_DFR_TYP = 'D13'
				AND LN50.LC_STA_LON50 = 'A'
				AND LN50.LC_DFR_RSP != '003' -- 003 = DEFERMENT REQUEST DENIED
				AND LN50.LD_DFR_BEG <= GETDATE()
		) UnempDefer
			ON UnempDefer.BF_SSN = LN10.BF_SSN 
			AND UnempDefer.LN_SEQ = LN10.LN_SEQ 
		LEFT JOIN
		( -- Forb months already used
			SELECT DISTINCT
				LN60.BF_SSN,
				LN60.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN60.LD_FOR_BEG, DATEADD(DAY,1,LN60.LD_FOR_END))/365.00)*12) OVER (PARTITION BY LN60.BF_SSN, LN60.LN_SEQ), 1) AS [TotalFrbMonthsUsed]
			FROM
				UDW..FB10_BR_FOR_REQ FB10
				INNER JOIN UDW..LN60_BR_FOR_APV LN60 
					ON LN60.BF_SSN = FB10.BF_SSN
					AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
			WHERE
				FB10.LC_FOR_STA = 'A'
				AND FB10.LC_STA_FOR10 = 'A'
				AND FB10.LC_FOR_TYP = '05'
				AND LN60.LC_STA_LON60 = 'A'
				AND LN60.LC_FOR_RSP != '003' --003 = FORBEARANCE REQUEST DENIED
				AND LN60.LD_FOR_BEG <= GETDATE()
		) HardForb
			ON HardForb.BF_SSN = LN10.BF_SSN
			AND HardForb.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN
		(
			SELECT 
				NCH.AccountIdentifier, 
				NCH.ActivityDate 
			FROM 
				NobleCalls..NobleCallHistory NCH
				INNER JOIN NobleCalls..DispositionCodeMapping DCM
					ON DCM.DispositionCode = NCH.DispositionCode
				INNER JOIN NobleCalls..ResponseCodes RC
					ON RC.ResponseCodeId = DCM.ResponseCodeId
					AND RC.ResponseCode = 'CNTCT'
			WHERE
				CAST(NCH.ActivityDate AS DATE) BETWEEN CAST(DATEADD(DAY,-7,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE) --last 7 days
		) ContactLastSeven
			ON ContactLastSeven.AccountIdentifier = PD10.DF_SPE_ACC_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				NCH.AccountIdentifier, 
				COUNT(*) AS NumberAttempts
			FROM 
				NobleCalls..NobleCallHistory NCH
				INNER JOIN NobleCalls..DispositionCodeMapping DCM
					ON DCM.DispositionCode = NCH.DispositionCode
				INNER JOIN NobleCalls..ResponseCodes RC
					ON RC.ResponseCodeId = DCM.ResponseCodeId
					AND RC.ResponseCode IN('INVPH', 'NOCTC')
			WHERE
				CAST(NCH.ActivityDate AS DATE) BETWEEN CAST(DATEADD(DAY,-7,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE) --last 7 days
			GROUP BY
				NCH.AccountIdentifier
		) OneAttemptLastSeven
			ON OneAttemptLastSeven.AccountIdentifier = PD10.DF_SPE_ACC_ID
			AND OneAttemptLastSeven.NumberAttempts >= 1
		LEFT JOIN 
		(
			SELECT
				NCH.AccountIdentifier,
				MAX(NCH.ActivityDate) AS DateOfLastCall
			FROM
				NobleCalls..NobleCallHistory NCH
			GROUP BY
				NCH.AccountIdentifier
		) LastCallDate
			ON LastCallDate.AccountIdentifier = PD10.DF_SPE_ACC_ID
	WHERE
		LN10.LC_STA_LON10 = 'R'
		AND ISNULL(LN10.LA_CUR_PRI,0.00) + ISNULL(DW01.WA_TOT_BRI_OTS,0.00) > 0.00
		AND LN10.LI_CON_PAY_STP_PUR != 'Y'
		AND ISNULL(DID.LD_ATY_REQ_RCV,'2099-01-01') > ISNULL(AUTO.LD_ATY_REQ_RCV,'1900-01-01')
		AND ARC_BRPTP.BF_SSN IS NULL /*exclude future arc date*/
		AND WQ20.BF_SSN IS NULL /*exclude pending queue*/
		AND CT30.DF_PRS_ID_BR IS NULL /*exclude pending one link task*/
		AND ContactLastSeven.AccountIdentifier IS NULL /*exclude successful contacts in last week*/
		AND OneAttemptLastSeven.AccountIdentifier IS NULL /*exclude 3 failed contacts in last week*/
) BLST --Blast report
ORDER BY
	BLST.DateOfLastCall ASC


GO
CREATE PROCEDURE [dbo].[SsrsUTLWS02_Manual_Low]
	
AS
--R4
--Manual
SELECT
	RecipientAccountNumber,
	Delinquency,
	RecipientDescription,
	BorrowerAccountNumber,
	TCPAConsentForHighestRankedPhone,
	HighestRankedPhone,
	TCPAConsentForSecondHighestRankedPhone,
	SecondHighestPhone,
	TCPAConsentForThirdHighestRankedPhone,
	ThirdHighestPhone,
	[State],
	ZIP,
	AmountDue,
	TotalBalance,
	BorrowerAccountNumber2,
	Portfolio,
	TotalForbearanceTime,
	TotalEconomicHardshipDefer,
	TotalUnemploymentDefer,
	RecipientName,
	BorrowerName,
	DateOfLastCall
FROM
(
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID AS RecipientAccountNumber,
		MAX(LN16.LN_DLQ_MAX) OVER(PARTITION BY LN10.BF_SSN) + 1 AS Delinquency,
		'BORROWER' AS RecipientDescription,
		PD10.DF_SPE_ACC_ID AS BorrowerAccountNumber,
		PD42.PHN_H_CON AS TCPAConsentForHighestRankedPhone,
		PD42.PHN_H AS HighestRankedPhone,
		PD42.PHN_A_CON AS TCPAConsentForSecondHighestRankedPhone,
		PD42.PHN_A AS SecondHighestPhone,
		PD42.PHN_W_CON AS TCPAConsentForThirdHighestRankedPhone,
		PD42.PHN_W AS ThirdHighestPhone,
		PD30.DC_DOM_ST AS [State],
		LEFT(ISNULL(PD30.DF_ZIP_CDE, '00000'),5) AS ZIP,
		ISNULL(AmountDue.AMTDU,0.00) AS AmountDue, --Dialer requires no decimals
		SUM(ISNULL(LN10.LA_CUR_PRI,0.00) + ISNULL(DW01.WA_TOT_BRI_OTS,0.00) + ISNULL(AmountDue.LA_LTE_FEE_OTS_PRT,0.00)) OVER(PARTITION BY PD10.DF_SPE_ACC_ID) AS TotalBalance, --Dialer requires no decimals
		PD10.DF_SPE_ACC_ID AS BorrowerAccountNumber2,
		CASE WHEN LN10.LF_LON_CUR_OWN = '900749' THEN 'CLP'
			 WHEN LN10.LF_LON_CUR_OWN = '971357' THEN 'TILP'
			 WHEN LN10.LF_LON_CUR_OWN LIKE '829769%' THEN 'BANA'
			 WHEN LN10.LF_LON_CUR_OWN IN('826717','830248') THEN 'ALIGN'
			 WHEN LN10.LF_LON_CUR_OWN = '828476' THEN 'UTAH'
			 ELSE LN10.LF_LON_CUR_OWN 
		END AS Portfolio,
		ISNULL(MAX(HardForb.TotalFrbMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalForbearanceTime, 
		ISNULL(MAX(EconDefer.TotalDfrMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalEconomicHardshipDefer, 
		ISNULL(MAX(UnempDefer.TotalDfrMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalUnemploymentDefer, 
		RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_MID) + ' ' + RTRIM(PD10.DM_PRS_LST) AS [RecipientName],
		RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_MID) + ' ' + RTRIM(PD10.DM_PRS_LST) AS [BorrowerName],
		ISNULL(LastCallDate.DateOfLastCall, '1900-01-01') AS DateOfLastCall
	FROM 
		UDW..LN10_LON LN10
		INNER JOIN UDW..LN16_LON_DLQ_HST LN16
			ON LN16.BF_SSN = LN10.BF_SSN
			AND LN16.LN_SEQ = LN10.LN_SEQ
			AND LN16.LC_STA_LON16 = '1'
			AND DATEDIFF(DAY, LN16.LD_DLQ_MAX, GETDATE()) <= 5
			AND DATEDIFF(DAY, LN16.LD_DLQ_OCC, GETDATE()) > 10
			AND (LN16.LN_DLQ_MAX + 1) BETWEEN 15 AND 59 --Low Delq
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = LN10.BF_SSN
		INNER JOIN 
		(
			SELECT DISTINCT
				DF_PRS_ID,
				MAX(IIF(OverallRank = 1, PHN, '')) AS PHN_H,
				MAX(IIF(OverallRank = 1, DC_ALW_ADL_PHN, '')) AS PHN_H_CON,
				MAX(IIF(OverallRank = 2, PHN, '')) AS PHN_A,
				MAX(IIF(OverallRank = 2, DC_ALW_ADL_PHN, '')) AS PHN_A_CON,
				MAX(IIF(OverallRank = 3, PHN, '')) AS PHN_W,
				MAX(IIF(OverallRank = 3, DC_ALW_ADL_PHN, '')) AS PHN_W_CON
			FROM
			(
				SELECT DISTINCT
					PD42.DF_PRS_ID,
					ISNULL(PD42.DN_DOM_PHN_ARA,'') + ISNULL(PD42.DN_DOM_PHN_XCH,'') + ISNULL(PD42.DN_DOM_PHN_LCL,'') AS PHN,
					PD42.DC_PHN,
					CASE WHEN PD42.DC_ALW_ADL_PHN IN ('L','X','P') 
						THEN 'Y'
						ELSE 'N'
					END AS DC_ALW_ADL_PHN,
					PD42.DC_ALW_ADL_PHN AS Consent,
					/*Rank phones by category and type*/
					DENSE_RANK() OVER (PARTITION BY PD42.DF_PRS_ID ORDER BY Ranks.ConsentRank, Ranks.TypeRank) AS OverallRank
				FROM
					UDW..PD42_PRS_PHN PD42
					INNER JOIN
					(
						SELECT
							DF_PRS_ID,
							DC_PHN,
							DC_ALW_ADL_PHN,
							CASE WHEN DC_PHN = 'H' THEN 1
									WHEN DC_PHN = 'A' THEN 2
									WHEN DC_PHN = 'W' THEN 3
									WHEN DC_PHN = 'M' THEN 4
							END AS TypeRank, --H A W M TYPE
							CASE WHEN DC_ALW_ADL_PHN = 'N' THEN 1
									WHEN DC_ALW_ADL_PHN = 'P' THEN 2 
									WHEN DC_ALW_ADL_PHN = 'X' THEN 3
									WHEN DC_ALW_ADL_PHN = 'L' THEN 4
									WHEN DC_ALW_ADL_PHN = 'Q' THEN 5
									WHEN DC_ALW_ADL_PHN = 'U' THEN 6 
							END AS ConsentRank--N P X L Q U CONSENT
						FROM
							UDW..PD42_PRS_PHN PD42
						WHERE
							PD42.DI_PHN_VLD = 'Y'
							AND PD42.DC_NO_HME_PHN != 'J'
							AND PD42.DC_ALW_ADL_PHN NOT IN ('L','X','P') -- NO Consent
					) Ranks
						ON Ranks.DF_PRS_ID = PD42.DF_PRS_ID
						AND Ranks.DC_PHN = PD42.DC_PHN
						AND Ranks.DC_ALW_ADL_PHN = PD42.DC_ALW_ADL_PHN
				WHERE
					PD42.DI_PHN_VLD = 'Y'
					AND PD42.DC_NO_HME_PHN != 'J'
					AND PD42.DC_ALW_ADL_PHN NOT IN ('L','X','P') -- NO Consent
			) Ranked
			WHERE
				Ranked.OverallRank IN (1,2,3)
			GROUP BY
				DF_PRS_ID
		) PD42
			ON PD42.DF_PRS_ID = LN10.BF_SSN
			AND 
			(
				PD42.PHN_H != ''
				OR PD42.PHN_A != ''
				OR PD42.PHN_W != ''
			)
		INNER JOIN UDW..DW01_DW_CLC_CLU DW01
			ON DW01.BF_SSN = LN10.BF_SSN
			AND DW01.LN_SEQ = LN10.LN_SEQ
			AND DW01.WC_DW_LON_STA != '23'
			AND DW01.WC_DW_LON_STA IN ('03','04','05','08','14')
			AND RTRIM(DW01.WX_OVR_DW_LON_STA) NOT IN('PENDING ID THEFT','REALLOCATED','CNSLD-STOP PURSUIT')
		LEFT JOIN UDW..PD30_PRS_ADR PD30
			ON PD30.DF_PRS_ID = LN10.BF_SSN
			AND PD30.DC_ADR = 'L'
		LEFT JOIN UDW..WQ20_TSK_QUE WQ20
			ON WQ20.BF_SSN = LN10.BF_SSN
			AND WQ20.WF_QUE IN ('2A', '2P', 'GP', '9R', 'SI', 'S4', 'PR', 'VR', 'VB', 'MF', '1P', 'SF', 'DU', 'AT', '87')
			AND WQ20.WC_STA_WQUE20 IN ('A', 'H', 'P', 'U', 'W')
		LEFT JOIN ODW..CT30_CALL_QUE CT30
			ON CT30.DF_PRS_ID_BR = LN10.BF_SSN
			AND CT30.IF_WRK_GRP IN ('ASBKP', 'ASCON', 'ASMOC')
			AND CT30.IF_WRK_GRP IN ('A', 'W')	
		LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY DOC ID*/
		(
			SELECT
				BF_SSN,
				MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
			FROM
				UDW..AY10_BR_LON_ATY
			WHERE
				PF_REQ_ACT = 'DIDDA'
				AND DATEDIFF(DAY, LD_ATY_REQ_RCV, GETDATE()) <= 14
				AND LC_STA_ACTY10 = 'A'
			GROUP BY
				BF_SSN
		)DID 
			ON DID.BF_SSN = LN10.BF_SSN
		LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY APPROVAL*/
		(
			SELECT
				BF_SSN,
				MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
			FROM
				UDW..AY10_BR_LON_ATY
			WHERE
				PF_REQ_ACT = 'PAUTO'
				AND LC_STA_ACTY10 = 'A'
			GROUP BY
				BF_SSN
		) AUTO 
			ON AUTO.BF_SSN = LN10.BF_SSN
		LEFT JOIN /*Gets amount due from billing tables*/
		(
			SELECT 
				LN80.BF_SSN,
				SUM(ISNULL(LN80.LA_BIL_PAS_DU,0.00) + ISNULL(LN80.LA_BIL_DU_PRT,0.00) + ISNULL(LN80.LA_LTE_FEE_OTS_PRT,0.00) - ISNULL(LN80.LA_TOT_BIL_STS,0.00)) AS AMTDU,
				SUM(ISNULL(LN80.LA_LTE_FEE_OTS_PRT,0.00)) AS LA_LTE_FEE_OTS_PRT
			FROM 
				UDW..LN80_LON_BIL_CRF LN80
				INNER JOIN 
				(
					SELECT 
						BF_SSN,
						MAX(LD_BIL_CRT) AS LD_BIL_CRT
					FROM 
						UDW..BL10_BR_BIL
					WHERE 
						LC_STA_BIL10 = 'A'
						AND LC_BIL_TYP = 'P'
					GROUP BY 
						BF_SSN
				) BL10
					ON BL10.BF_SSN = LN80.BF_SSN
					AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
			WHERE 
				LN80.LC_STA_LON80 = 'A'
			GROUP BY 
				LN80.BF_SSN
		) AmountDue 
			ON AmountDue.BF_SSN = LN10.BF_SSN
		LEFT JOIN 
		(/*exclude future arc date*/
			SELECT DISTINCT
				BF_SSN,
				LD_ATY_REQ_RCV,
				PARSED_DAYS,
				PARSED_DAYS AS COMMENT_DAYS, /*takes lesser of 14 or parsed days*/
				DATEADD(DAY,PARSED_DAYS, LD_ATY_REQ_RCV) AS CALC_DATE,	
				CASE WHEN CAST(DATEADD(DAY,PARSED_DAYS, LD_ATY_REQ_RCV) AS DATE) > CAST(GETDATE() AS DATE)
					THEN 1
					ELSE 0
				END AS FUTURE_DATE
			FROM
			(/*parse out numeric days from any extraneous text*/
				SELECT 
					AY10.BF_SSN,
					AY10.LD_ATY_REQ_RCV,
					AY20.LX_ATY,
					IIF(COALESCE(TRY_CAST(LTRIM(RTRIM(AY20.LX_ATY)) AS INT),14) >= 14, 14, COALESCE(TRY_CAST(LTRIM(RTRIM(AY20.LX_ATY)) AS INT),14)) AS PARSED_DAYS	
				FROM
					UDW..AY10_BR_LON_ATY AY10
					LEFT JOIN UDW..AY20_ATY_TXT AY20
						ON AY20.BF_SSN = AY10.BF_SSN
						AND AY20.LN_ATY_SEQ = AY10.LN_ATY_SEQ
				WHERE
					AY10.LC_STA_ACTY10 = 'A'
					AND AY10.PF_REQ_ACT = 'BRPTP' /*borrower promise to repay*/
			) P
		) ARC_BRPTP 
			ON ARC_BRPTP.BF_SSN = LN10.BF_SSN
			AND ARC_BRPTP.FUTURE_DATE = 1 /*exclude future arc date*/
		LEFT JOIN
		( -- Deferment months already used
			SELECT DISTINCT
				LN50.BF_SSN,
				LN50.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN50.LD_DFR_BEG, DATEADD(DAY,1,LN50.LD_DFR_END))/365.00)*12) OVER (PARTITION BY LN50.BF_SSN, LN50.LN_SEQ), 1) AS [TotalDfrMonthsUsed]
			FROM
				UDW..DF10_BR_DFR_REQ DF10
				INNER JOIN UDW..LN50_BR_DFR_APV LN50 
					ON LN50.BF_SSN = DF10.BF_SSN 
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
			WHERE
				DF10.LC_DFR_STA = 'A'
				AND DF10.LC_STA_DFR10 = 'A'
				AND DF10.LC_DFR_TYP = 'D29'
				AND LN50.LC_STA_LON50 = 'A'
				AND LN50.LC_DFR_RSP != '003' -- 003 = DEFERMENT REQUEST DENIED
				AND LN50.LD_DFR_BEG <= GETDATE()
		) EconDefer
			ON EconDefer.BF_SSN = LN10.BF_SSN 
			AND EconDefer.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN
		( -- Deferment months already used
			SELECT DISTINCT
				LN50.BF_SSN,
				LN50.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN50.LD_DFR_BEG, DATEADD(DAY,1,LN50.LD_DFR_END))/365.00)*12) OVER (PARTITION BY LN50.BF_SSN, LN50.LN_SEQ), 1) AS [TotalDfrMonthsUsed]
			FROM
				UDW..DF10_BR_DFR_REQ DF10
				INNER JOIN UDW..LN50_BR_DFR_APV LN50 
					ON LN50.BF_SSN = DF10.BF_SSN 
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
			WHERE
				DF10.LC_DFR_STA = 'A'
				AND DF10.LC_STA_DFR10 = 'A'
				AND DF10.LC_DFR_TYP = 'D13'
				AND LN50.LC_STA_LON50 = 'A'
				AND LN50.LC_DFR_RSP != '003' -- 003 = DEFERMENT REQUEST DENIED
				AND LN50.LD_DFR_BEG <= GETDATE()
		) UnempDefer
			ON UnempDefer.BF_SSN = LN10.BF_SSN 
			AND UnempDefer.LN_SEQ = LN10.LN_SEQ 
		LEFT JOIN
		( -- Forb months already used
			SELECT DISTINCT
				LN60.BF_SSN,
				LN60.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN60.LD_FOR_BEG, DATEADD(DAY,1,LN60.LD_FOR_END))/365.00)*12) OVER (PARTITION BY LN60.BF_SSN, LN60.LN_SEQ), 1) AS [TotalFrbMonthsUsed]
			FROM
				UDW..FB10_BR_FOR_REQ FB10
				INNER JOIN UDW..LN60_BR_FOR_APV LN60 
					ON LN60.BF_SSN = FB10.BF_SSN
					AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
			WHERE
				FB10.LC_FOR_STA = 'A'
				AND FB10.LC_STA_FOR10 = 'A'
				AND FB10.LC_FOR_TYP = '05'
				AND LN60.LC_STA_LON60 = 'A'
				AND LN60.LC_FOR_RSP != '003' --003 = FORBEARANCE REQUEST DENIED
				AND LN60.LD_FOR_BEG <= GETDATE()
		) HardForb
			ON HardForb.BF_SSN = LN10.BF_SSN
			AND HardForb.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN
		(
			SELECT 
				NCH.AccountIdentifier, 
				NCH.ActivityDate 
			FROM 
				NobleCalls..NobleCallHistory NCH
				INNER JOIN NobleCalls..DispositionCodeMapping DCM
					ON DCM.DispositionCode = NCH.DispositionCode
				INNER JOIN NobleCalls..ResponseCodes RC
					ON RC.ResponseCodeId = DCM.ResponseCodeId
					AND RC.ResponseCode = 'CNTCT'
			WHERE
				CAST(NCH.ActivityDate AS DATE) BETWEEN CAST(DATEADD(DAY,-7,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE) --last 7 days
		) ContactLastSeven
			ON ContactLastSeven.AccountIdentifier = PD10.DF_SPE_ACC_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				NCH.AccountIdentifier, 
				COUNT(*) AS NumberAttempts
			FROM 
				NobleCalls..NobleCallHistory NCH
				INNER JOIN NobleCalls..DispositionCodeMapping DCM
					ON DCM.DispositionCode = NCH.DispositionCode
				INNER JOIN NobleCalls..ResponseCodes RC
					ON RC.ResponseCodeId = DCM.ResponseCodeId
					AND RC.ResponseCode IN('INVPH', 'NOCTC')
			WHERE
				CAST(NCH.ActivityDate AS DATE) BETWEEN CAST(DATEADD(DAY,-7,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE) --last 7 days
			GROUP BY
				NCH.AccountIdentifier
		) OneAttemptLastSeven
			ON OneAttemptLastSeven.AccountIdentifier = PD10.DF_SPE_ACC_ID
			AND OneAttemptLastSeven.NumberAttempts >= 1
		LEFT JOIN 
		(
			SELECT
				NCH.AccountIdentifier,
				MAX(NCH.ActivityDate) AS DateOfLastCall
			FROM
				NobleCalls..NobleCallHistory NCH
			GROUP BY
				NCH.AccountIdentifier
		) LastCallDate
			ON LastCallDate.AccountIdentifier = PD10.DF_SPE_ACC_ID
	WHERE
		LN10.LC_STA_LON10 = 'R'
		AND ISNULL(LN10.LA_CUR_PRI,0.00) + ISNULL(DW01.WA_TOT_BRI_OTS,0.00) > 0.00
		AND LN10.LI_CON_PAY_STP_PUR != 'Y'
		AND ISNULL(DID.LD_ATY_REQ_RCV,'2099-01-01') > ISNULL(AUTO.LD_ATY_REQ_RCV,'1900-01-01')
		AND ARC_BRPTP.BF_SSN IS NULL /*exclude future arc date*/
		AND WQ20.BF_SSN IS NULL /*exclude pending queue*/
		AND CT30.DF_PRS_ID_BR IS NULL /*exclude pending one link task*/
		AND ContactLastSeven.AccountIdentifier IS NULL /*exclude successful contacts in last week*/
		AND OneAttemptLastSeven.AccountIdentifier IS NULL /*exclude 3 failed contacts in last week*/

	UNION ALL

	/*	begin DELQ_EDR*/
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID AS RecipientAccountNumber,
		MAX(LN16.LN_DLQ_MAX) OVER(PARTITION BY LN10.BF_SSN) + 1 AS Delinquency,
		CASE WHEN LC_EDS_TYP = 'M' THEN 'COBORROWER'
			 WHEN LC_EDS_TYP = 'S' THEN 'ENDORSER'
			 ELSE ''
		END AS RecipientDescription,
		PD10Borr.DF_SPE_ACC_ID AS BorrowerAccountNumber,
		PD42.PHN_H_CON AS TCPAConsentForHighestRankedPhone,
		PD42.PHN_H AS HighestRankedPhone,
		PD42.PHN_A_CON AS TCPAConsentForSecondHighestRankedPhone,
		PD42.PHN_A AS SecondHighestPhone,
		PD42.PHN_W_CON AS TCPAConsentForThirdHighestRankedPhone,
		PD42.PHN_W AS ThirdHighestPhone,
		PD30.DC_DOM_ST AS [State],
		LEFT(ISNULL(PD30.DF_ZIP_CDE, '00000'),5) AS ZIP,
		ISNULL(AmountDue.AMTDU,0.00) AS AmountDue, --Dialer requires no decimals
		SUM(ISNULL(LN10.LA_CUR_PRI,0.00) + ISNULL(DW01.WA_TOT_BRI_OTS,0.00) + ISNULL(AmountDue.LA_LTE_FEE_OTS_PRT,0.00)) OVER(PARTITION BY PD10.DF_SPE_ACC_ID) AS TotalBalance, --Dialer requires no decimals
		PD10Borr.DF_SPE_ACC_ID AS BorrowerAccountNumber2,
		CASE WHEN LN10.LF_LON_CUR_OWN = '900749' THEN 'CLP'
			 WHEN LN10.LF_LON_CUR_OWN = '971357' THEN 'TILP'
			 WHEN LN10.LF_LON_CUR_OWN LIKE '829769%' THEN 'BANA'
			 WHEN LN10.LF_LON_CUR_OWN IN('826717','830248') THEN 'ALIGN'
			 WHEN LN10.LF_LON_CUR_OWN = '828476' THEN 'UTAH'
			 ELSE LN10.LF_LON_CUR_OWN 
		END AS Portfolio,
		ISNULL(MAX(HardForb.TotalFrbMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalForbearanceTime, 
		ISNULL(MAX(EconDefer.TotalDfrMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalEconomicHardshipDefer, 
		ISNULL(MAX(UnempDefer.TotalDfrMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalUnemploymentDefer, 
		RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_MID) + ' ' + RTRIM(PD10.DM_PRS_LST) AS [RecipientName],
		RTRIM(PD10Borr.DM_PRS_1) + ' ' + RTRIM(PD10Borr.DM_PRS_MID) + ' ' + RTRIM(PD10Borr.DM_PRS_LST) AS [BorrowerName],
		ISNULL(LastCallDate.DateOfLastCall, '1900-01-01') AS DateOfLastCall
	FROM 
		UDW..LN10_LON LN10
		INNER JOIN UDW..LN20_EDS LN20
			ON LN20.BF_SSN = LN10.BF_SSN
			AND LN20.LN_SEQ = LN10.LN_SEQ
			AND LN20.LC_STA_LON20 = 'A'								
		INNER JOIN UDW..LN16_LON_DLQ_HST LN16
			ON LN16.BF_SSN = LN10.BF_SSN
			AND LN16.LN_SEQ = LN10.LN_SEQ
			AND LN16.LC_STA_LON16 = '1'
			AND DATEDIFF(DAY, LN16.LD_DLQ_MAX, GETDATE()) <= 5
			AND (LN16.LN_DLQ_MAX + 1) BETWEEN 15 AND 59 --Low Delq
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = LN20.LF_EDS
		INNER JOIN UDW..PD10_PRS_NME PD10Borr
			ON PD10Borr.DF_PRS_ID = LN10.BF_SSN
		INNER JOIN
		(
			SELECT DISTINCT
				DF_PRS_ID,
				MAX(IIF(OverallRank = 1, PHN, '')) AS PHN_H,
				MAX(IIF(OverallRank = 1, DC_ALW_ADL_PHN, '')) AS PHN_H_CON,
				MAX(IIF(OverallRank = 2, PHN, '')) AS PHN_A,
				MAX(IIF(OverallRank = 2, DC_ALW_ADL_PHN, '')) AS PHN_A_CON,
				MAX(IIF(OverallRank = 3, PHN, '')) AS PHN_W,
				MAX(IIF(OverallRank = 3, DC_ALW_ADL_PHN, '')) AS PHN_W_CON
			FROM
			(
				SELECT DISTINCT
					PD42.DF_PRS_ID,
					ISNULL(PD42.DN_DOM_PHN_ARA,'') + ISNULL(PD42.DN_DOM_PHN_XCH,'') + ISNULL(PD42.DN_DOM_PHN_LCL,'') AS PHN,
					PD42.DC_PHN,
					CASE WHEN PD42.DC_ALW_ADL_PHN IN ('L','X','P') 
						THEN 'Y'
						ELSE 'N'
					END AS DC_ALW_ADL_PHN,
					PD42.DC_ALW_ADL_PHN AS Consent,
					/*Rank phones by category and type*/
					DENSE_RANK() OVER (PARTITION BY PD42.DF_PRS_ID ORDER BY Ranks.ConsentRank, Ranks.TypeRank) AS OverallRank
				FROM
					UDW..PD42_PRS_PHN PD42
					INNER JOIN
					(
						SELECT
							DF_PRS_ID,
							DC_PHN,
							DC_ALW_ADL_PHN,
							CASE WHEN DC_PHN = 'H' THEN 1
									WHEN DC_PHN = 'A' THEN 2
									WHEN DC_PHN = 'W' THEN 3
									WHEN DC_PHN = 'M' THEN 4
							END AS TypeRank, --H A W M TYPE
							CASE WHEN DC_ALW_ADL_PHN = 'N' THEN 1
									WHEN DC_ALW_ADL_PHN = 'P' THEN 2 
									WHEN DC_ALW_ADL_PHN = 'X' THEN 3
									WHEN DC_ALW_ADL_PHN = 'L' THEN 4
									WHEN DC_ALW_ADL_PHN = 'Q' THEN 5
									WHEN DC_ALW_ADL_PHN = 'U' THEN 6 
							END AS ConsentRank--N P X L Q U CONSENT
						FROM
							UDW..PD42_PRS_PHN PD42
						WHERE
							PD42.DI_PHN_VLD = 'Y'
							AND PD42.DC_NO_HME_PHN != 'J'
							AND PD42.DC_ALW_ADL_PHN NOT IN ('L','X','P') --NO Consent
					) Ranks
						ON Ranks.DF_PRS_ID = PD42.DF_PRS_ID
						AND Ranks.DC_PHN = PD42.DC_PHN
						AND Ranks.DC_ALW_ADL_PHN = PD42.DC_ALW_ADL_PHN
				WHERE
					PD42.DI_PHN_VLD = 'Y'
					AND PD42.DC_NO_HME_PHN != 'J'
					AND PD42.DC_ALW_ADL_PHN NOT IN ('L','X','P') -- NO Consent
			) Ranked
			WHERE
				Ranked.OverallRank IN (1,2,3)
			GROUP BY
				DF_PRS_ID
		) PD42 
			ON PD42.DF_PRS_ID = LN20.LF_EDS
			AND 
			(
				PD42.PHN_H != ''
				OR PD42.PHN_A != ''
				OR PD42.PHN_W != ''
			)
		INNER JOIN UDW..DW01_DW_CLC_CLU DW01
			ON DW01.BF_SSN = LN10.BF_SSN
			AND DW01.LN_SEQ = LN10.LN_SEQ
			AND DW01.WC_DW_LON_STA != '23'
			AND DW01.WC_DW_LON_STA IN ('03','04','05','08','14')
			AND RTRIM(DW01.WX_OVR_DW_LON_STA) NOT IN('PENDING ID THEFT','REALLOCATED','CNSLD-STOP PURSUIT')
		LEFT JOIN UDW..PD30_PRS_ADR PD30
			ON PD30.DF_PRS_ID = LN20.LF_EDS
			AND PD30.DC_ADR = 'L'
		LEFT JOIN UDW..WQ20_TSK_QUE WQ20
			ON WQ20.BF_SSN = LN10.BF_SSN
			AND WQ20.WF_QUE IN ('2A', '2P', 'GP', '9R', 'SI', 'S4', 'PR', 'VR', 'VB', 'MF', '1P', 'SF', 'DU', 'AT', '87')
			AND WQ20.WC_STA_WQUE20 IN ('A', 'H', 'P', 'U', 'W')
		LEFT JOIN ODW..CT30_CALL_QUE CT30
			ON CT30.DF_PRS_ID_BR = LN10.BF_SSN
			AND CT30.IF_WRK_GRP IN ('ASBKP', 'ASCON', 'ASMOC')
			AND CT30.IF_WRK_GRP IN ('A', 'W')	
		LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY DOC ID*/
		(
			SELECT
				BF_SSN,
				MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
			FROM
				UDW..AY10_BR_LON_ATY
			WHERE
				PF_REQ_ACT = 'DIDDA'
				AND DATEDIFF(DAY, LD_ATY_REQ_RCV, GETDATE()) <= 14
				AND LC_STA_ACTY10 = 'A'
			GROUP BY
				BF_SSN
		) DID 
			ON DID.BF_SSN = LN10.BF_SSN
		LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY APPROVAL*/
		(
			SELECT
				BF_SSN,
				MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
			FROM
				UDW..AY10_BR_LON_ATY
			WHERE
				PF_REQ_ACT = 'PAUTO'
				AND LC_STA_ACTY10 = 'A'
			GROUP BY
				BF_SSN
		) AUTO 
			ON AUTO.BF_SSN = LN10.BF_SSN
		LEFT JOIN /*Gets amount due from billing tables*/
		(
			SELECT 
				LN80.BF_SSN,
				SUM(ISNULL(LN80.LA_BIL_PAS_DU,0.00) + ISNULL(LN80.LA_BIL_DU_PRT,0.00) + ISNULL(LN80.LA_LTE_FEE_OTS_PRT,0.00) - ISNULL(LN80.LA_TOT_BIL_STS,0.00)) AS AMTDU,
				SUM(ISNULL(LN80.LA_LTE_FEE_OTS_PRT,0.00)) AS LA_LTE_FEE_OTS_PRT
			FROM 
				UDW..LN80_LON_BIL_CRF LN80
				INNER JOIN 
				(
					SELECT 
						BF_SSN,
						MAX(LD_BIL_CRT) AS LD_BIL_CRT
					FROM 
						UDW..BL10_BR_BIL
					WHERE 
						LC_STA_BIL10 = 'A'
						AND LC_BIL_TYP = 'P'
					GROUP BY 
						BF_SSN
				) BL10
					ON BL10.BF_SSN = LN80.BF_SSN
					AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
			WHERE 
				LN80.LC_STA_LON80 = 'A'
			GROUP BY 
				LN80.BF_SSN
		) AmountDue 
			ON AmountDue.BF_SSN = LN10.BF_SSN
		LEFT JOIN 
		(/*exclude future arc date*/
			SELECT DISTINCT
				BF_SSN,
				LD_ATY_REQ_RCV,
				PARSED_DAYS,
				PARSED_DAYS AS COMMENT_DAYS, /*takes lesser of 14 or parsed days*/
				DATEADD(DAY,PARSED_DAYS, LD_ATY_REQ_RCV) AS CALC_DATE,	
				CASE WHEN CAST(DATEADD(DAY,PARSED_DAYS, LD_ATY_REQ_RCV) AS DATE) > CAST(GETDATE() AS DATE)
					THEN 1
					ELSE 0
				END AS FUTURE_DATE
			FROM
			(/*parse out numeric days from any extraneous text*/
				SELECT 
					AY10.BF_SSN,
					AY10.LD_ATY_REQ_RCV,
					AY20.LX_ATY,
					IIF(COALESCE(TRY_CAST(LTRIM(RTRIM(AY20.LX_ATY)) AS INT),14) >= 14, 14, COALESCE(TRY_CAST(LTRIM(RTRIM(AY20.LX_ATY)) AS INT),14)) AS PARSED_DAYS	
				FROM
					UDW..AY10_BR_LON_ATY AY10
					LEFT JOIN UDW..AY20_ATY_TXT AY20
						ON AY20.BF_SSN = AY10.BF_SSN
						AND AY20.LN_ATY_SEQ = AY10.LN_ATY_SEQ
				WHERE
					AY10.LC_STA_ACTY10 = 'A'
					AND AY10.PF_REQ_ACT = 'BRPTP' /*borrower promise to repay*/
			) P
		) ARC_BRPTP 
			ON LN10.BF_SSN = ARC_BRPTP.BF_SSN
			AND ARC_BRPTP.FUTURE_DATE = 1 /*exclude future arc date*/
		LEFT JOIN
		( -- Deferment months already used
			SELECT DISTINCT
				LN50.BF_SSN,
				LN50.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN50.LD_DFR_BEG, DATEADD(DAY,1,LN50.LD_DFR_END))/365.00)*12) OVER (PARTITION BY LN50.BF_SSN, LN50.LN_SEQ), 1) AS [TotalDfrMonthsUsed]
			FROM
				UDW..DF10_BR_DFR_REQ DF10
				INNER JOIN UDW..LN50_BR_DFR_APV LN50 
					ON LN50.BF_SSN = DF10.BF_SSN 
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
			WHERE
				DF10.LC_DFR_STA = 'A'
				AND DF10.LC_STA_DFR10 = 'A'
				AND DF10.LC_DFR_TYP = 'D29'
				AND LN50.LC_STA_LON50 = 'A'
				AND LN50.LC_DFR_RSP != '003' -- 003 = DEFERMENT REQUEST DENIED
				AND LN50.LD_DFR_BEG <= GETDATE()
		) EconDefer
			ON EconDefer.BF_SSN = LN10.BF_SSN 
			AND EconDefer.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN
		( -- Deferment months already used
			SELECT DISTINCT
				LN50.BF_SSN,
				LN50.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN50.LD_DFR_BEG, DATEADD(DAY,1,LN50.LD_DFR_END))/365.00)*12) OVER (PARTITION BY LN50.BF_SSN, LN50.LN_SEQ), 1) AS [TotalDfrMonthsUsed]
			FROM
				UDW..DF10_BR_DFR_REQ DF10
				INNER JOIN UDW..LN50_BR_DFR_APV LN50 
					ON LN50.BF_SSN = DF10.BF_SSN 
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
			WHERE
				DF10.LC_DFR_STA = 'A'
				AND DF10.LC_STA_DFR10 = 'A'
				AND DF10.LC_DFR_TYP = 'D13'
				AND LN50.LC_STA_LON50 = 'A'
				AND LN50.LC_DFR_RSP != '003' -- 003 = DEFERMENT REQUEST DENIED
				AND LN50.LD_DFR_BEG <= GETDATE()
		) UnempDefer
			ON UnempDefer.BF_SSN = LN10.BF_SSN 
			AND UnempDefer.LN_SEQ = LN10.LN_SEQ 
		LEFT JOIN
		( -- Forb months already used
			SELECT DISTINCT
				LN60.BF_SSN,
				LN60.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN60.LD_FOR_BEG, DATEADD(DAY,1,LN60.LD_FOR_END))/365.00)*12) OVER (PARTITION BY LN60.BF_SSN, LN60.LN_SEQ), 1) AS [TotalFrbMonthsUsed]
			FROM
				UDW..FB10_BR_FOR_REQ FB10
				INNER JOIN UDW..LN60_BR_FOR_APV LN60 
					ON LN60.BF_SSN = FB10.BF_SSN
					AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
			WHERE
				FB10.LC_FOR_STA = 'A'
				AND FB10.LC_STA_FOR10 = 'A'
				AND FB10.LC_FOR_TYP = '05'
				AND LN60.LC_STA_LON60 = 'A'
				AND LN60.LC_FOR_RSP != '003' --003 = FORBEARANCE REQUEST DENIED
				AND LN60.LD_FOR_BEG <= GETDATE()
		) HardForb
			ON HardForb.BF_SSN = LN10.BF_SSN
			AND HardForb.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN
		(
			SELECT 
				NCH.AccountIdentifier, 
				NCH.ActivityDate 
			FROM 
				NobleCalls..NobleCallHistory NCH
				INNER JOIN NobleCalls..DispositionCodeMapping DCM
					ON DCM.DispositionCode = NCH.DispositionCode
				INNER JOIN NobleCalls..ResponseCodes RC
					ON RC.ResponseCodeId = DCM.ResponseCodeId
					AND RC.ResponseCode = 'CNTCT'
			WHERE
				CAST(NCH.ActivityDate AS DATE) BETWEEN CAST(DATEADD(DAY,-7,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE) --last 7 days
		) ContactLastSeven
			ON ContactLastSeven.AccountIdentifier = PD10.DF_SPE_ACC_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				NCH.AccountIdentifier, 
				COUNT(*) AS NumberAttempts
			FROM 
				NobleCalls..NobleCallHistory NCH
				INNER JOIN NobleCalls..DispositionCodeMapping DCM
					ON DCM.DispositionCode = NCH.DispositionCode
				INNER JOIN NobleCalls..ResponseCodes RC
					ON RC.ResponseCodeId = DCM.ResponseCodeId
					AND RC.ResponseCode IN('INVPH', 'NOCTC')
			WHERE
				CAST(NCH.ActivityDate AS DATE) BETWEEN CAST(DATEADD(DAY,-7,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE) --last 7 days
			GROUP BY
				NCH.AccountIdentifier
		) OneAttemptLastSeven
			ON OneAttemptLastSeven.AccountIdentifier = PD10.DF_SPE_ACC_ID
			AND OneAttemptLastSeven.NumberAttempts >= 1
		LEFT JOIN 
		(
			SELECT
				NCH.AccountIdentifier,
				MAX(NCH.ActivityDate) AS DateOfLastCall
			FROM
				NobleCalls..NobleCallHistory NCH
			GROUP BY
				NCH.AccountIdentifier
		) LastCallDate
			ON LastCallDate.AccountIdentifier = PD10.DF_SPE_ACC_ID
	WHERE
		LN10.LC_STA_LON10 = 'R'
		AND ISNULL(LN10.LA_CUR_PRI,0.00) + ISNULL(DW01.WA_TOT_BRI_OTS,0.00) > 0.00
		AND LN10.LI_CON_PAY_STP_PUR != 'Y'
		AND ISNULL(DID.LD_ATY_REQ_RCV,'2099-01-01') > ISNULL(AUTO.LD_ATY_REQ_RCV,'1900-01-01')
		AND ARC_BRPTP.BF_SSN IS NULL /*exclude future arc date*/
		AND WQ20.BF_SSN IS NULL /*exclude pending queue*/
		AND CT30.DF_PRS_ID_BR IS NULL /*exclude pending one link task*/
		AND ContactLastSeven.AccountIdentifier IS NULL /*exclude successful contacts in last week*/
		AND OneAttemptLastSeven.AccountIdentifier IS NULL /*exclude 3 failed contacts in last week*/
) ML --Manual_Low
ORDER BY
	ML.DateOfLastCall ASC

GO
CREATE PROCEDURE [dbo].[SsrsUTLWS02_Manual_High]
	
AS
--R4
--Manual
SELECT
	RecipientAccountNumber,
	Delinquency,
	RecipientDescription,
	BorrowerAccountNumber,
	TCPAConsentForHighestRankedPhone,
	HighestRankedPhone,
	TCPAConsentForSecondHighestRankedPhone,
	SecondHighestPhone,
	TCPAConsentForThirdHighestRankedPhone,
	ThirdHighestPhone,
	[State],
	ZIP,
	AmountDue,
	TotalBalance,
	BorrowerAccountNumber2,
	Portfolio,
	TotalForbearanceTime,
	TotalEconomicHardshipDefer,
	TotalUnemploymentDefer,
	RecipientName,
	BorrowerName,
	DateOfLastCall
FROM
(
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID AS RecipientAccountNumber,
		MAX(LN16.LN_DLQ_MAX) OVER(PARTITION BY LN10.BF_SSN) + 1 AS Delinquency,
		'BORROWER' AS RecipientDescription,
		PD10.DF_SPE_ACC_ID AS BorrowerAccountNumber,
		PD42.PHN_H_CON AS TCPAConsentForHighestRankedPhone,
		PD42.PHN_H AS HighestRankedPhone,
		PD42.PHN_A_CON AS TCPAConsentForSecondHighestRankedPhone,
		PD42.PHN_A AS SecondHighestPhone,
		PD42.PHN_W_CON AS TCPAConsentForThirdHighestRankedPhone,
		PD42.PHN_W AS ThirdHighestPhone,
		PD30.DC_DOM_ST AS [State],
		LEFT(ISNULL(PD30.DF_ZIP_CDE, '00000'),5) AS ZIP,
		ISNULL(AmountDue.AMTDU,0.00) AS AmountDue, --Dialer requires no decimals
		SUM(ISNULL(LN10.LA_CUR_PRI,0.00) + ISNULL(DW01.WA_TOT_BRI_OTS,0.00) + ISNULL(AmountDue.LA_LTE_FEE_OTS_PRT,0.00)) OVER(PARTITION BY PD10.DF_SPE_ACC_ID) AS TotalBalance, --Dialer requires no decimals
		PD10.DF_SPE_ACC_ID AS BorrowerAccountNumber2,
		CASE WHEN LN10.LF_LON_CUR_OWN = '900749' THEN 'CLP'
			 WHEN LN10.LF_LON_CUR_OWN = '971357' THEN 'TILP'
			 WHEN LN10.LF_LON_CUR_OWN LIKE '829769%' THEN 'BANA'
			 WHEN LN10.LF_LON_CUR_OWN IN('826717','830248') THEN 'ALIGN'
			 WHEN LN10.LF_LON_CUR_OWN = '828476' THEN 'UTAH'
			 ELSE LN10.LF_LON_CUR_OWN 
		END AS Portfolio,
		ISNULL(MAX(HardForb.TotalFrbMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalForbearanceTime, 
		ISNULL(MAX(EconDefer.TotalDfrMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalEconomicHardshipDefer, 
		ISNULL(MAX(UnempDefer.TotalDfrMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalUnemploymentDefer, 
		RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_MID) + ' ' + RTRIM(PD10.DM_PRS_LST) AS [RecipientName],
		RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_MID) + ' ' + RTRIM(PD10.DM_PRS_LST) AS [BorrowerName],
		ISNULL(LastCallDate.DateOfLastCall, '1900-01-01') AS DateOfLastCall
	FROM 
		UDW..LN10_LON LN10
		INNER JOIN UDW..LN16_LON_DLQ_HST LN16
			ON LN16.BF_SSN = LN10.BF_SSN
			AND LN16.LN_SEQ = LN10.LN_SEQ
			AND LN16.LC_STA_LON16 = '1'
			AND DATEDIFF(DAY, LN16.LD_DLQ_MAX, GETDATE()) <= 5
			AND DATEDIFF(DAY, LN16.LD_DLQ_OCC, GETDATE()) > 10
			AND (LN16.LN_DLQ_MAX + 1) >= 60 --High Delq
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = LN10.BF_SSN
		INNER JOIN 
		(
			SELECT DISTINCT
				DF_PRS_ID,
				MAX(IIF(OverallRank = 1, PHN, '')) AS PHN_H,
				MAX(IIF(OverallRank = 1, DC_ALW_ADL_PHN, '')) AS PHN_H_CON,
				MAX(IIF(OverallRank = 2, PHN, '')) AS PHN_A,
				MAX(IIF(OverallRank = 2, DC_ALW_ADL_PHN, '')) AS PHN_A_CON,
				MAX(IIF(OverallRank = 3, PHN, '')) AS PHN_W,
				MAX(IIF(OverallRank = 3, DC_ALW_ADL_PHN, '')) AS PHN_W_CON
			FROM
			(
				SELECT DISTINCT
					PD42.DF_PRS_ID,
					ISNULL(PD42.DN_DOM_PHN_ARA,'') + ISNULL(PD42.DN_DOM_PHN_XCH,'') + ISNULL(PD42.DN_DOM_PHN_LCL,'') AS PHN,
					PD42.DC_PHN,
					CASE WHEN PD42.DC_ALW_ADL_PHN IN ('L','X','P') 
						THEN 'Y'
						ELSE 'N'
					END AS DC_ALW_ADL_PHN,
					PD42.DC_ALW_ADL_PHN AS Consent,
					/*Rank phones by category and type*/
					DENSE_RANK() OVER (PARTITION BY PD42.DF_PRS_ID ORDER BY Ranks.ConsentRank, Ranks.TypeRank) AS OverallRank
				FROM
					UDW..PD42_PRS_PHN PD42
					INNER JOIN
					(
						SELECT
							DF_PRS_ID,
							DC_PHN,
							DC_ALW_ADL_PHN,
							CASE WHEN DC_PHN = 'H' THEN 1
									WHEN DC_PHN = 'A' THEN 2
									WHEN DC_PHN = 'W' THEN 3
									WHEN DC_PHN = 'M' THEN 4
							END AS TypeRank, --H A W M TYPE
							CASE WHEN DC_ALW_ADL_PHN = 'N' THEN 1
									WHEN DC_ALW_ADL_PHN = 'P' THEN 2 
									WHEN DC_ALW_ADL_PHN = 'X' THEN 3
									WHEN DC_ALW_ADL_PHN = 'L' THEN 4
									WHEN DC_ALW_ADL_PHN = 'Q' THEN 5
									WHEN DC_ALW_ADL_PHN = 'U' THEN 6 
							END AS ConsentRank--N P X L Q U CONSENT
						FROM
							UDW..PD42_PRS_PHN PD42
						WHERE
							PD42.DI_PHN_VLD = 'Y'
							AND PD42.DC_NO_HME_PHN != 'J'
							AND PD42.DC_ALW_ADL_PHN NOT IN ('L','X','P') -- NO Consent
					) Ranks
						ON Ranks.DF_PRS_ID = PD42.DF_PRS_ID
						AND Ranks.DC_PHN = PD42.DC_PHN
						AND Ranks.DC_ALW_ADL_PHN = PD42.DC_ALW_ADL_PHN
				WHERE
					PD42.DI_PHN_VLD = 'Y'
					AND PD42.DC_NO_HME_PHN != 'J'
					AND PD42.DC_ALW_ADL_PHN NOT IN ('L','X','P') -- NO Consent
			) Ranked
			WHERE
				Ranked.OverallRank IN (1,2,3)
			GROUP BY
				DF_PRS_ID
		) PD42
			ON PD42.DF_PRS_ID = LN10.BF_SSN
			AND 
			(
				PD42.PHN_H != ''
				OR PD42.PHN_A != ''
				OR PD42.PHN_W != ''
			)
		INNER JOIN UDW..DW01_DW_CLC_CLU DW01
			ON DW01.BF_SSN = LN10.BF_SSN
			AND DW01.LN_SEQ = LN10.LN_SEQ
			AND DW01.WC_DW_LON_STA != '23'
			AND DW01.WC_DW_LON_STA IN ('03','04','05','08','14')
			AND RTRIM(DW01.WX_OVR_DW_LON_STA) NOT IN('PENDING ID THEFT','REALLOCATED','CNSLD-STOP PURSUIT')
		LEFT JOIN UDW..PD30_PRS_ADR PD30
			ON PD30.DF_PRS_ID = LN10.BF_SSN
			AND PD30.DC_ADR = 'L'
		LEFT JOIN UDW..WQ20_TSK_QUE WQ20
			ON WQ20.BF_SSN = LN10.BF_SSN
			AND WQ20.WF_QUE IN ('2A', '2P', 'GP', '9R', 'SI', 'S4', 'PR', 'VR', 'VB', 'MF', '1P', 'SF', 'DU', 'AT', '87')
			AND WQ20.WC_STA_WQUE20 IN ('A', 'H', 'P', 'U', 'W')
		LEFT JOIN ODW..CT30_CALL_QUE CT30
			ON CT30.DF_PRS_ID_BR = LN10.BF_SSN
			AND CT30.IF_WRK_GRP IN ('ASBKP', 'ASCON', 'ASMOC')
			AND CT30.IF_WRK_GRP IN ('A', 'W')	
		LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY DOC ID*/
		(
			SELECT
				BF_SSN,
				MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
			FROM
				UDW..AY10_BR_LON_ATY
			WHERE
				PF_REQ_ACT = 'DIDDA'
				AND DATEDIFF(DAY, LD_ATY_REQ_RCV, GETDATE()) <= 14
				AND LC_STA_ACTY10 = 'A'
			GROUP BY
				BF_SSN
		)DID 
			ON DID.BF_SSN = LN10.BF_SSN
		LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY APPROVAL*/
		(
			SELECT
				BF_SSN,
				MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
			FROM
				UDW..AY10_BR_LON_ATY
			WHERE
				PF_REQ_ACT = 'PAUTO'
				AND LC_STA_ACTY10 = 'A'
			GROUP BY
				BF_SSN
		) AUTO 
			ON AUTO.BF_SSN = LN10.BF_SSN
		LEFT JOIN /*Gets amount due from billing tables*/
		(
			SELECT 
				LN80.BF_SSN,
				SUM(ISNULL(LN80.LA_BIL_PAS_DU,0.00) + ISNULL(LN80.LA_BIL_DU_PRT,0.00) + ISNULL(LN80.LA_LTE_FEE_OTS_PRT,0.00) - ISNULL(LN80.LA_TOT_BIL_STS,0.00)) AS AMTDU,
				SUM(ISNULL(LN80.LA_LTE_FEE_OTS_PRT,0.00)) AS LA_LTE_FEE_OTS_PRT
			FROM 
				UDW..LN80_LON_BIL_CRF LN80
				INNER JOIN 
				(
					SELECT 
						BF_SSN,
						MAX(LD_BIL_CRT) AS LD_BIL_CRT
					FROM 
						UDW..BL10_BR_BIL
					WHERE 
						LC_STA_BIL10 = 'A'
						AND LC_BIL_TYP = 'P'
					GROUP BY 
						BF_SSN
				) BL10
					ON BL10.BF_SSN = LN80.BF_SSN
					AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
			WHERE 
				LN80.LC_STA_LON80 = 'A'
			GROUP BY 
				LN80.BF_SSN
		) AmountDue 
			ON AmountDue.BF_SSN = LN10.BF_SSN
		LEFT JOIN 
		(/*exclude future arc date*/
			SELECT DISTINCT
				BF_SSN,
				LD_ATY_REQ_RCV,
				PARSED_DAYS,
				PARSED_DAYS AS COMMENT_DAYS, /*takes lesser of 14 or parsed days*/
				DATEADD(DAY,PARSED_DAYS, LD_ATY_REQ_RCV) AS CALC_DATE,	
				CASE WHEN CAST(DATEADD(DAY,PARSED_DAYS, LD_ATY_REQ_RCV) AS DATE) > CAST(GETDATE() AS DATE)
					THEN 1
					ELSE 0
				END AS FUTURE_DATE
			FROM
			(/*parse out numeric days from any extraneous text*/
				SELECT 
					AY10.BF_SSN,
					AY10.LD_ATY_REQ_RCV,
					AY20.LX_ATY,
					IIF(COALESCE(TRY_CAST(LTRIM(RTRIM(AY20.LX_ATY)) AS INT),14) >= 14, 14, COALESCE(TRY_CAST(LTRIM(RTRIM(AY20.LX_ATY)) AS INT),14)) AS PARSED_DAYS	
				FROM
					UDW..AY10_BR_LON_ATY AY10
					LEFT JOIN UDW..AY20_ATY_TXT AY20
						ON AY20.BF_SSN = AY10.BF_SSN
						AND AY20.LN_ATY_SEQ = AY10.LN_ATY_SEQ
				WHERE
					AY10.LC_STA_ACTY10 = 'A'
					AND AY10.PF_REQ_ACT = 'BRPTP' /*borrower promise to repay*/
			) P
		) ARC_BRPTP 
			ON ARC_BRPTP.BF_SSN = LN10.BF_SSN
			AND ARC_BRPTP.FUTURE_DATE = 1 /*exclude future arc date*/
		LEFT JOIN
		( -- Deferment months already used
			SELECT DISTINCT
				LN50.BF_SSN,
				LN50.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN50.LD_DFR_BEG, DATEADD(DAY,1,LN50.LD_DFR_END))/365.00)*12) OVER (PARTITION BY LN50.BF_SSN, LN50.LN_SEQ), 1) AS [TotalDfrMonthsUsed]
			FROM
				UDW..DF10_BR_DFR_REQ DF10
				INNER JOIN UDW..LN50_BR_DFR_APV LN50 
					ON LN50.BF_SSN = DF10.BF_SSN 
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
			WHERE
				DF10.LC_DFR_STA = 'A'
				AND DF10.LC_STA_DFR10 = 'A'
				AND DF10.LC_DFR_TYP = 'D29'
				AND LN50.LC_STA_LON50 = 'A'
				AND LN50.LC_DFR_RSP != '003' -- 003 = DEFERMENT REQUEST DENIED
				AND LN50.LD_DFR_BEG <= GETDATE()
		) EconDefer
			ON EconDefer.BF_SSN = LN10.BF_SSN 
			AND EconDefer.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN
		( -- Deferment months already used
			SELECT DISTINCT
				LN50.BF_SSN,
				LN50.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN50.LD_DFR_BEG, DATEADD(DAY,1,LN50.LD_DFR_END))/365.00)*12) OVER (PARTITION BY LN50.BF_SSN, LN50.LN_SEQ), 1) AS [TotalDfrMonthsUsed]
			FROM
				UDW..DF10_BR_DFR_REQ DF10
				INNER JOIN UDW..LN50_BR_DFR_APV LN50 
					ON LN50.BF_SSN = DF10.BF_SSN 
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
			WHERE
				DF10.LC_DFR_STA = 'A'
				AND DF10.LC_STA_DFR10 = 'A'
				AND DF10.LC_DFR_TYP = 'D13'
				AND LN50.LC_STA_LON50 = 'A'
				AND LN50.LC_DFR_RSP != '003' -- 003 = DEFERMENT REQUEST DENIED
				AND LN50.LD_DFR_BEG <= GETDATE()
		) UnempDefer
			ON UnempDefer.BF_SSN = LN10.BF_SSN 
			AND UnempDefer.LN_SEQ = LN10.LN_SEQ 
		LEFT JOIN
		( -- Forb months already used
			SELECT DISTINCT
				LN60.BF_SSN,
				LN60.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN60.LD_FOR_BEG, DATEADD(DAY,1,LN60.LD_FOR_END))/365.00)*12) OVER (PARTITION BY LN60.BF_SSN, LN60.LN_SEQ), 1) AS [TotalFrbMonthsUsed]
			FROM
				UDW..FB10_BR_FOR_REQ FB10
				INNER JOIN UDW..LN60_BR_FOR_APV LN60 
					ON LN60.BF_SSN = FB10.BF_SSN
					AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
			WHERE
				FB10.LC_FOR_STA = 'A'
				AND FB10.LC_STA_FOR10 = 'A'
				AND FB10.LC_FOR_TYP = '05'
				AND LN60.LC_STA_LON60 = 'A'
				AND LN60.LC_FOR_RSP != '003' --003 = FORBEARANCE REQUEST DENIED
				AND LN60.LD_FOR_BEG <= GETDATE()
		) HardForb
			ON HardForb.BF_SSN = LN10.BF_SSN
			AND HardForb.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN
		(
			SELECT 
				NCH.AccountIdentifier, 
				NCH.ActivityDate 
			FROM 
				NobleCalls..NobleCallHistory NCH
				INNER JOIN NobleCalls..DispositionCodeMapping DCM
					ON DCM.DispositionCode = NCH.DispositionCode
				INNER JOIN NobleCalls..ResponseCodes RC
					ON RC.ResponseCodeId = DCM.ResponseCodeId
					AND RC.ResponseCode = 'CNTCT'
			WHERE
				CAST(NCH.ActivityDate AS DATE) BETWEEN CAST(DATEADD(DAY,-7,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE) --last 7 days
		) ContactLastSeven
			ON ContactLastSeven.AccountIdentifier = PD10.DF_SPE_ACC_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				NCH.AccountIdentifier, 
				COUNT(*) AS NumberAttempts
			FROM 
				NobleCalls..NobleCallHistory NCH
				INNER JOIN NobleCalls..DispositionCodeMapping DCM
					ON DCM.DispositionCode = NCH.DispositionCode
				INNER JOIN NobleCalls..ResponseCodes RC
					ON RC.ResponseCodeId = DCM.ResponseCodeId
					AND RC.ResponseCode IN('INVPH', 'NOCTC')
			WHERE
				CAST(NCH.ActivityDate AS DATE) BETWEEN CAST(DATEADD(DAY,-7,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE) --last 7 days
			GROUP BY
				NCH.AccountIdentifier
		) OneAttemptLastSeven
			ON OneAttemptLastSeven.AccountIdentifier = PD10.DF_SPE_ACC_ID
			AND OneAttemptLastSeven.NumberAttempts >= 1
		LEFT JOIN 
		(
			SELECT
				NCH.AccountIdentifier,
				MAX(NCH.ActivityDate) AS DateOfLastCall
			FROM
				NobleCalls..NobleCallHistory NCH
			GROUP BY
				NCH.AccountIdentifier
		) LastCallDate
			ON LastCallDate.AccountIdentifier = PD10.DF_SPE_ACC_ID
	WHERE
		LN10.LC_STA_LON10 = 'R'
		AND ISNULL(LN10.LA_CUR_PRI,0.00) + ISNULL(DW01.WA_TOT_BRI_OTS,0.00) > 0.00
		AND LN10.LI_CON_PAY_STP_PUR != 'Y'
		AND ISNULL(DID.LD_ATY_REQ_RCV,'2099-01-01') > ISNULL(AUTO.LD_ATY_REQ_RCV,'1900-01-01')
		AND ARC_BRPTP.BF_SSN IS NULL /*exclude future arc date*/
		AND WQ20.BF_SSN IS NULL /*exclude pending queue*/
		AND CT30.DF_PRS_ID_BR IS NULL /*exclude pending one link task*/
		AND ContactLastSeven.AccountIdentifier IS NULL /*exclude successful contacts in last week*/
		AND OneAttemptLastSeven.AccountIdentifier IS NULL /*exclude 3 failed contacts in last week*/
 
	UNION ALL

	/*	begin DELQ_EDR*/
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID AS RecipientAccountNumber,
		MAX(LN16.LN_DLQ_MAX) OVER(PARTITION BY LN10.BF_SSN) + 1 AS Delinquency,
		CASE WHEN LC_EDS_TYP = 'M' THEN 'COBORROWER'
			 WHEN LC_EDS_TYP = 'S' THEN 'ENDORSER'
			 ELSE ''
		END AS RecipientDescription,
		PD10Borr.DF_SPE_ACC_ID AS BorrowerAccountNumber,
		PD42.PHN_H_CON AS TCPAConsentForHighestRankedPhone,
		PD42.PHN_H AS HighestRankedPhone,
		PD42.PHN_A_CON AS TCPAConsentForSecondHighestRankedPhone,
		PD42.PHN_A AS SecondHighestPhone,
		PD42.PHN_W_CON AS TCPAConsentForThirdHighestRankedPhone,
		PD42.PHN_W AS ThirdHighestPhone,
		PD30.DC_DOM_ST AS [State],
		LEFT(ISNULL(PD30.DF_ZIP_CDE, '00000'),5) AS ZIP,
		ISNULL(AmountDue.AMTDU,0.00) AS AmountDue, --Dialer requires no decimals
		SUM(ISNULL(LN10.LA_CUR_PRI,0.00) + ISNULL(DW01.WA_TOT_BRI_OTS,0.00) + ISNULL(AmountDue.LA_LTE_FEE_OTS_PRT,0.00)) OVER(PARTITION BY PD10.DF_SPE_ACC_ID) AS TotalBalance, --Dialer requires no decimals
		PD10Borr.DF_SPE_ACC_ID AS BorrowerAccountNumber2,
		CASE WHEN LN10.LF_LON_CUR_OWN = '900749' THEN 'CLP'
			 WHEN LN10.LF_LON_CUR_OWN = '971357' THEN 'TILP'
			 WHEN LN10.LF_LON_CUR_OWN LIKE '829769%' THEN 'BANA'
			 WHEN LN10.LF_LON_CUR_OWN IN('826717','830248') THEN 'ALIGN'
			 WHEN LN10.LF_LON_CUR_OWN = '828476' THEN 'UTAH'
			 ELSE LN10.LF_LON_CUR_OWN 
		END AS Portfolio,
		ISNULL(MAX(HardForb.TotalFrbMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalForbearanceTime, 
		ISNULL(MAX(EconDefer.TotalDfrMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalEconomicHardshipDefer, 
		ISNULL(MAX(UnempDefer.TotalDfrMonthsUsed) OVER(PARTITION BY LN10.BF_SSN),0) AS TotalUnemploymentDefer, 
		RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_MID) + ' ' + RTRIM(PD10.DM_PRS_LST) AS [RecipientName],
		RTRIM(PD10Borr.DM_PRS_1) + ' ' + RTRIM(PD10Borr.DM_PRS_MID) + ' ' + RTRIM(PD10Borr.DM_PRS_LST) AS [BorrowerName],
		ISNULL(LastCallDate.DateOfLastCall, '1900-01-01') AS DateOfLastCall
	FROM 
		UDW..LN10_LON LN10
		INNER JOIN UDW..LN20_EDS LN20
			ON LN20.BF_SSN = LN10.BF_SSN
			AND LN20.LN_SEQ = LN10.LN_SEQ
			AND LN20.LC_STA_LON20 = 'A'								
		INNER JOIN UDW..LN16_LON_DLQ_HST LN16
			ON LN16.BF_SSN = LN10.BF_SSN
			AND LN16.LN_SEQ = LN10.LN_SEQ
			AND LN16.LC_STA_LON16 = '1'
			AND DATEDIFF(DAY, LN16.LD_DLQ_MAX, GETDATE()) <= 5
			AND (LN16.LN_DLQ_MAX + 1) >= 60 --High Delq
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = LN20.LF_EDS
		INNER JOIN UDW..PD10_PRS_NME PD10Borr
			ON PD10Borr.DF_PRS_ID = LN10.BF_SSN
		INNER JOIN
		(
			SELECT DISTINCT
				DF_PRS_ID,
				MAX(IIF(OverallRank = 1, PHN, '')) AS PHN_H,
				MAX(IIF(OverallRank = 1, DC_ALW_ADL_PHN, '')) AS PHN_H_CON,
				MAX(IIF(OverallRank = 2, PHN, '')) AS PHN_A,
				MAX(IIF(OverallRank = 2, DC_ALW_ADL_PHN, '')) AS PHN_A_CON,
				MAX(IIF(OverallRank = 3, PHN, '')) AS PHN_W,
				MAX(IIF(OverallRank = 3, DC_ALW_ADL_PHN, '')) AS PHN_W_CON
			FROM
			(
				SELECT DISTINCT
					PD42.DF_PRS_ID,
					ISNULL(PD42.DN_DOM_PHN_ARA,'') + ISNULL(PD42.DN_DOM_PHN_XCH,'') + ISNULL(PD42.DN_DOM_PHN_LCL,'') AS PHN,
					PD42.DC_PHN,
					CASE WHEN PD42.DC_ALW_ADL_PHN IN ('L','X','P') 
						THEN 'Y'
						ELSE 'N'
					END AS DC_ALW_ADL_PHN,
					PD42.DC_ALW_ADL_PHN AS Consent,
					/*Rank phones by category and type*/
					DENSE_RANK() OVER (PARTITION BY PD42.DF_PRS_ID ORDER BY Ranks.ConsentRank, Ranks.TypeRank) AS OverallRank
				FROM
					UDW..PD42_PRS_PHN PD42
					INNER JOIN
					(
						SELECT
							DF_PRS_ID,
							DC_PHN,
							DC_ALW_ADL_PHN,
							CASE WHEN DC_PHN = 'H' THEN 1
									WHEN DC_PHN = 'A' THEN 2
									WHEN DC_PHN = 'W' THEN 3
									WHEN DC_PHN = 'M' THEN 4
							END AS TypeRank, --H A W M TYPE
							CASE WHEN DC_ALW_ADL_PHN = 'N' THEN 1
									WHEN DC_ALW_ADL_PHN = 'P' THEN 2 
									WHEN DC_ALW_ADL_PHN = 'X' THEN 3
									WHEN DC_ALW_ADL_PHN = 'L' THEN 4
									WHEN DC_ALW_ADL_PHN = 'Q' THEN 5
									WHEN DC_ALW_ADL_PHN = 'U' THEN 6 
							END AS ConsentRank--N P X L Q U CONSENT
						FROM
							UDW..PD42_PRS_PHN PD42
						WHERE
							PD42.DI_PHN_VLD = 'Y'
							AND PD42.DC_NO_HME_PHN != 'J'
							AND PD42.DC_ALW_ADL_PHN NOT IN ('L','X','P') --NO Consent
					) Ranks
						ON Ranks.DF_PRS_ID = PD42.DF_PRS_ID
						AND Ranks.DC_PHN = PD42.DC_PHN
						AND Ranks.DC_ALW_ADL_PHN = PD42.DC_ALW_ADL_PHN
				WHERE
					PD42.DI_PHN_VLD = 'Y'
					AND PD42.DC_NO_HME_PHN != 'J'
					AND PD42.DC_ALW_ADL_PHN NOT IN ('L','X','P') -- NO Consent
			) Ranked
			WHERE
				Ranked.OverallRank IN (1,2,3)
			GROUP BY
				DF_PRS_ID
		) PD42 
			ON PD42.DF_PRS_ID = LN20.LF_EDS
			AND 
			(
				PD42.PHN_H != ''
				OR PD42.PHN_A != ''
				OR PD42.PHN_W != ''
			)
		INNER JOIN UDW..DW01_DW_CLC_CLU DW01
			ON DW01.BF_SSN = LN10.BF_SSN
			AND DW01.LN_SEQ = LN10.LN_SEQ
			AND DW01.WC_DW_LON_STA != '23'
			AND DW01.WC_DW_LON_STA IN ('03','04','05','08','14')
			AND RTRIM(DW01.WX_OVR_DW_LON_STA) NOT IN('PENDING ID THEFT','REALLOCATED','CNSLD-STOP PURSUIT')
		LEFT JOIN UDW..PD30_PRS_ADR PD30
			ON PD30.DF_PRS_ID = LN20.LF_EDS
			AND PD30.DC_ADR = 'L'
		LEFT JOIN UDW..WQ20_TSK_QUE WQ20
			ON WQ20.BF_SSN = LN10.BF_SSN
			AND WQ20.WF_QUE IN ('2A', '2P', 'GP', '9R', 'SI', 'S4', 'PR', 'VR', 'VB', 'MF', '1P', 'SF', 'DU', 'AT', '87')
			AND WQ20.WC_STA_WQUE20 IN ('A', 'H', 'P', 'U', 'W')
		LEFT JOIN ODW..CT30_CALL_QUE CT30
			ON CT30.DF_PRS_ID_BR = LN10.BF_SSN
			AND CT30.IF_WRK_GRP IN ('ASBKP', 'ASCON', 'ASMOC')
			AND CT30.IF_WRK_GRP IN ('A', 'W')	
		LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY DOC ID*/
		(
			SELECT
				BF_SSN,
				MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
			FROM
				UDW..AY10_BR_LON_ATY
			WHERE
				PF_REQ_ACT = 'DIDDA'
				AND DATEDIFF(DAY, LD_ATY_REQ_RCV, GETDATE()) <= 14
				AND LC_STA_ACTY10 = 'A'
			GROUP BY
				BF_SSN
		) DID 
			ON DID.BF_SSN = LN10.BF_SSN
		LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY APPROVAL*/
		(
			SELECT
				BF_SSN,
				MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
			FROM
				UDW..AY10_BR_LON_ATY
			WHERE
				PF_REQ_ACT = 'PAUTO'
				AND LC_STA_ACTY10 = 'A'
			GROUP BY
				BF_SSN
		) AUTO 
			ON AUTO.BF_SSN = LN10.BF_SSN
		LEFT JOIN /*Gets amount due from billing tables*/
		(
			SELECT 
				LN80.BF_SSN,
				SUM(ISNULL(LN80.LA_BIL_PAS_DU,0.00) + ISNULL(LN80.LA_BIL_DU_PRT,0.00) + ISNULL(LN80.LA_LTE_FEE_OTS_PRT,0.00) - ISNULL(LN80.LA_TOT_BIL_STS,0.00)) AS AMTDU,
				SUM(ISNULL(LN80.LA_LTE_FEE_OTS_PRT,0.00)) AS LA_LTE_FEE_OTS_PRT
			FROM 
				UDW..LN80_LON_BIL_CRF LN80
				INNER JOIN 
				(
					SELECT 
						BF_SSN,
						MAX(LD_BIL_CRT) AS LD_BIL_CRT
					FROM 
						UDW..BL10_BR_BIL
					WHERE 
						LC_STA_BIL10 = 'A'
						AND LC_BIL_TYP = 'P'
					GROUP BY 
						BF_SSN
				) BL10
					ON BL10.BF_SSN = LN80.BF_SSN
					AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
			WHERE 
				LN80.LC_STA_LON80 = 'A'
			GROUP BY 
				LN80.BF_SSN
		) AmountDue 
			ON AmountDue.BF_SSN = LN10.BF_SSN
		LEFT JOIN 
		(/*exclude future arc date*/
			SELECT DISTINCT
				BF_SSN,
				LD_ATY_REQ_RCV,
				PARSED_DAYS,
				PARSED_DAYS AS COMMENT_DAYS, /*takes lesser of 14 or parsed days*/
				DATEADD(DAY,PARSED_DAYS, LD_ATY_REQ_RCV) AS CALC_DATE,	
				CASE WHEN CAST(DATEADD(DAY,PARSED_DAYS, LD_ATY_REQ_RCV) AS DATE) > CAST(GETDATE() AS DATE)
					THEN 1
					ELSE 0
				END AS FUTURE_DATE
			FROM
			(/*parse out numeric days from any extraneous text*/
				SELECT 
					AY10.BF_SSN,
					AY10.LD_ATY_REQ_RCV,
					AY20.LX_ATY,
					IIF(COALESCE(TRY_CAST(LTRIM(RTRIM(AY20.LX_ATY)) AS INT),14) >= 14, 14, COALESCE(TRY_CAST(LTRIM(RTRIM(AY20.LX_ATY)) AS INT),14)) AS PARSED_DAYS	
				FROM
					UDW..AY10_BR_LON_ATY AY10
					LEFT JOIN UDW..AY20_ATY_TXT AY20
						ON AY20.BF_SSN = AY10.BF_SSN
						AND AY20.LN_ATY_SEQ = AY10.LN_ATY_SEQ
				WHERE
					AY10.LC_STA_ACTY10 = 'A'
					AND AY10.PF_REQ_ACT = 'BRPTP' /*borrower promise to repay*/
			) P
		) ARC_BRPTP 
			ON LN10.BF_SSN = ARC_BRPTP.BF_SSN
			AND ARC_BRPTP.FUTURE_DATE = 1 /*exclude future arc date*/
		LEFT JOIN
		( -- Deferment months already used
			SELECT DISTINCT
				LN50.BF_SSN,
				LN50.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN50.LD_DFR_BEG, DATEADD(DAY,1,LN50.LD_DFR_END))/365.00)*12) OVER (PARTITION BY LN50.BF_SSN, LN50.LN_SEQ), 1) AS [TotalDfrMonthsUsed]
			FROM
				UDW..DF10_BR_DFR_REQ DF10
				INNER JOIN UDW..LN50_BR_DFR_APV LN50 
					ON LN50.BF_SSN = DF10.BF_SSN 
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
			WHERE
				DF10.LC_DFR_STA = 'A'
				AND DF10.LC_STA_DFR10 = 'A'
				AND DF10.LC_DFR_TYP = 'D29'
				AND LN50.LC_STA_LON50 = 'A'
				AND LN50.LC_DFR_RSP != '003' -- 003 = DEFERMENT REQUEST DENIED
				AND LN50.LD_DFR_BEG <= GETDATE()
		) EconDefer
			ON EconDefer.BF_SSN = LN10.BF_SSN 
			AND EconDefer.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN
		( -- Deferment months already used
			SELECT DISTINCT
				LN50.BF_SSN,
				LN50.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN50.LD_DFR_BEG, DATEADD(DAY,1,LN50.LD_DFR_END))/365.00)*12) OVER (PARTITION BY LN50.BF_SSN, LN50.LN_SEQ), 1) AS [TotalDfrMonthsUsed]
			FROM
				UDW..DF10_BR_DFR_REQ DF10
				INNER JOIN UDW..LN50_BR_DFR_APV LN50 
					ON LN50.BF_SSN = DF10.BF_SSN 
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
			WHERE
				DF10.LC_DFR_STA = 'A'
				AND DF10.LC_STA_DFR10 = 'A'
				AND DF10.LC_DFR_TYP = 'D13'
				AND LN50.LC_STA_LON50 = 'A'
				AND LN50.LC_DFR_RSP != '003' -- 003 = DEFERMENT REQUEST DENIED
				AND LN50.LD_DFR_BEG <= GETDATE()
		) UnempDefer
			ON UnempDefer.BF_SSN = LN10.BF_SSN 
			AND UnempDefer.LN_SEQ = LN10.LN_SEQ 
		LEFT JOIN
		( -- Forb months already used
			SELECT DISTINCT
				LN60.BF_SSN,
				LN60.LN_SEQ,
				ROUND(SUM((DATEDIFF(DAY,LN60.LD_FOR_BEG, DATEADD(DAY,1,LN60.LD_FOR_END))/365.00)*12) OVER (PARTITION BY LN60.BF_SSN, LN60.LN_SEQ), 1) AS [TotalFrbMonthsUsed]
			FROM
				UDW..FB10_BR_FOR_REQ FB10
				INNER JOIN UDW..LN60_BR_FOR_APV LN60 
					ON LN60.BF_SSN = FB10.BF_SSN
					AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
			WHERE
				FB10.LC_FOR_STA = 'A'
				AND FB10.LC_STA_FOR10 = 'A'
				AND FB10.LC_FOR_TYP = '05'
				AND LN60.LC_STA_LON60 = 'A'
				AND LN60.LC_FOR_RSP != '003' --003 = FORBEARANCE REQUEST DENIED
				AND LN60.LD_FOR_BEG <= GETDATE()
		) HardForb
			ON HardForb.BF_SSN = LN10.BF_SSN
			AND HardForb.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN
		(
			SELECT 
				NCH.AccountIdentifier, 
				NCH.ActivityDate 
			FROM 
				NobleCalls..NobleCallHistory NCH
				INNER JOIN NobleCalls..DispositionCodeMapping DCM
					ON DCM.DispositionCode = NCH.DispositionCode
				INNER JOIN NobleCalls..ResponseCodes RC
					ON RC.ResponseCodeId = DCM.ResponseCodeId
					AND RC.ResponseCode = 'CNTCT'
			WHERE
				CAST(NCH.ActivityDate AS DATE) BETWEEN CAST(DATEADD(DAY,-7,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE) --last 7 days
		) ContactLastSeven
			ON ContactLastSeven.AccountIdentifier = PD10.DF_SPE_ACC_ID
		LEFT JOIN
		(
			SELECT DISTINCT
				NCH.AccountIdentifier, 
				COUNT(*) AS NumberAttempts
			FROM 
				NobleCalls..NobleCallHistory NCH
				INNER JOIN NobleCalls..DispositionCodeMapping DCM
					ON DCM.DispositionCode = NCH.DispositionCode
				INNER JOIN NobleCalls..ResponseCodes RC
					ON RC.ResponseCodeId = DCM.ResponseCodeId
					AND RC.ResponseCode IN('INVPH', 'NOCTC')
			WHERE
				CAST(NCH.ActivityDate AS DATE) BETWEEN CAST(DATEADD(DAY,-7,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE) --last 7 days
			GROUP BY
				NCH.AccountIdentifier
		) OneAttemptLastSeven
			ON OneAttemptLastSeven.AccountIdentifier = PD10.DF_SPE_ACC_ID
			AND OneAttemptLastSeven.NumberAttempts >= 1
		LEFT JOIN 
		(
			SELECT
				NCH.AccountIdentifier,
				MAX(NCH.ActivityDate) AS DateOfLastCall
			FROM
				NobleCalls..NobleCallHistory NCH
			GROUP BY
				NCH.AccountIdentifier
		) LastCallDate
			ON LastCallDate.AccountIdentifier = PD10.DF_SPE_ACC_ID
	WHERE
		LN10.LC_STA_LON10 = 'R'
		AND ISNULL(LN10.LA_CUR_PRI,0.00) + ISNULL(DW01.WA_TOT_BRI_OTS,0.00) > 0.00
		AND LN10.LI_CON_PAY_STP_PUR != 'Y'
		AND ISNULL(DID.LD_ATY_REQ_RCV,'2099-01-01') > ISNULL(AUTO.LD_ATY_REQ_RCV,'1900-01-01')
		AND ARC_BRPTP.BF_SSN IS NULL /*exclude future arc date*/
		AND WQ20.BF_SSN IS NULL /*exclude pending queue*/
		AND CT30.DF_PRS_ID_BR IS NULL /*exclude pending one link task*/
		AND ContactLastSeven.AccountIdentifier IS NULL /*exclude successful contacts in last week*/
		AND OneAttemptLastSeven.AccountIdentifier IS NULL /*exclude 3 failed contacts in last week*/
) MH --Manual_High
ORDER BY
	MH.DateOfLastCall ASC
