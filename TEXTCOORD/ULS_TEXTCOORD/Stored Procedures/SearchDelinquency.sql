CREATE PROCEDURE [textcoord].[SearchDelinquency]
	@NumberToSend INT,
	@DelqLower INT,
	@DelqUpper INT,
	@AgeLower INT,
	@AgeUpper INT,
	@ContentType VARCHAR(20),
	@Segment textcoord.Ranks READONLY,
	@PerformanceCategory textcoord.Ranks READONLY
AS
BEGIN
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DROP TABLE IF EXISTS #CALLS
SELECT DISTINCT 
	PD10.DF_SPE_ACC_ID 
INTO 
	#CALLS
FROM 
	textcoord.UheaaApp1 OQ
	INNER JOIN UDW..PD10_PRS_NME PD10 
		ON PD10.DF_SPE_ACC_ID = OQ.lm_filler2

SELECT
	*
FROM
	(
		--Call on Search button 
		SELECT DISTINCT TOP (@NumberToSend)
			PD10.DF_SPE_ACC_ID AS AccountNumber,
			LTRIM(RTRIM(SUBSTRING(PD10.DM_PRS_1,1,1) + LOWER(SUBSTRING(PD10.DM_PRS_1,2,13)))) AS FirstName,
			LTRIM(RTRIM(SUBSTRING(PD10.DM_PRS_LST,1,1) + LOWER(SUBSTRING(PD10.DM_PRS_LST,2,23)))) AS LastName,
			BL.SEGMENT,
			PhoneRank.ARA + PhoneRank.PHN AS PhoneNumber,
			BL.PerformanceCategory,
			AddressInfo.DC_DOM_ST AS [State],
			(Delinquency.LN_DLQ_MAX + 1) AS MaximumDelinquency,
			DATEDIFF(YEAR,PD10.DD_BRT,GETDATE()) AS Age,
			PhoneRank.DC_PHN AS PhoneType,
			CAST(Arc.MaxArcDate AS DATE) AS LastBRTXT,
			SUBSTRING(PD10.DF_PRS_ID,5,4) AS last4
		FROM
			UDW..PD10_PRS_NME PD10
			INNER JOIN
			(
				SELECT DISTINCT
					BF_SSN,
					PerformanceCategory,
					SEGMENT
				FROM
					UDW.uhedaymet.Daily_BorrowerLevel BL
				WHERE
					(
						BL.SEGMENT IN(SELECT DISTINCT Ranking FROM @Segment)
						OR NOT EXISTS(SELECT DISTINCT Ranking FROM @Segment)
					)
					AND BL.PerformanceCategory NOT IN ('PIF','TRN','PIFPRV','TRNPRV','PRV') 
					AND 
					(
						BL.PerformanceCategory IN(SELECT DISTINCT Ranking FROM @PerformanceCategory)
						OR NOT EXISTS(SELECT DISTINCT Ranking FROM @PerformanceCategory)
					)
			)BL
				ON PD10.DF_PRS_ID = BL.BF_SSN
			INNER JOIN
			(
				SELECT DISTINCT
					DF_PRS_ID,
					ARA,
					PHN,
					DC_PHN,
					DENSE_RANK() OVER(PARTITION BY ARA, PHN, DC_PHN ORDER BY MaxArcDate, DF_PRS_ID) AS PHN_RNK
				FROM
				(
					SELECT DISTINCT
						PD42.DF_PRS_ID,
						COALESCE(PD42.DN_DOM_PHN_ARA,'') AS ARA,
						COALESCE(PD42.DN_DOM_PHN_XCH,'') + COALESCE(PD42.DN_DOM_PHN_LCL,'') AS PHN,
						PD42.DC_PHN,
						DENSE_RANK() OVER (PARTITION BY PD42.DF_PRS_ID ORDER BY Ranks.TypeRank) AS OverallRank /*Rank phones by type*/
					FROM
						UDW..PD42_PRS_PHN PD42
						INNER JOIN
						(
							SELECT
								*,
								CASE WHEN DC_PHN = 'H' THEN 1
									 WHEN DC_PHN = 'A' THEN 2
									 WHEN DC_PHN = 'W' THEN 3
								END AS TypeRank
							FROM
								UDW..PD42_PRS_PHN PD42
								LEFT JOIN txt..dnt DontText
									ON DontText.friendly_from = COALESCE(PD42.DN_DOM_PHN_ARA,'') + COALESCE(PD42.DN_DOM_PHN_XCH,'') + COALESCE(PD42.DN_DOM_PHN_LCL,'')
							WHERE
								PD42.DI_PHN_VLD = 'Y'
								AND (PD42.DC_NO_HME_PHN IS NULL OR PD42.DC_NO_HME_PHN = '')
								AND PD42.DC_ALW_ADL_PHN = 'P'
								AND PD42.DC_PHN IN ('H','A','W')
								AND COALESCE(PD42.DN_DOM_PHN_ARA,'') + COALESCE(PD42.DN_DOM_PHN_XCH,'') + COALESCE(PD42.DN_DOM_PHN_LCL,'') != ''
								AND DontText.friendly_from IS NULL
						) Ranks
							ON Ranks.DF_PRS_ID = PD42.DF_PRS_ID
							AND Ranks.DC_PHN = PD42.DC_PHN
						LEFT JOIN txt..dnt DontText
							ON DontText.friendly_from = COALESCE(PD42.DN_DOM_PHN_ARA,'') + COALESCE(PD42.DN_DOM_PHN_XCH,'') + COALESCE(PD42.DN_DOM_PHN_LCL,'')
					WHERE
						PD42.DI_PHN_VLD = 'Y'
						AND (PD42.DC_NO_HME_PHN IS NULL OR PD42.DC_NO_HME_PHN = '')
						AND PD42.DC_ALW_ADL_PHN = 'P'
						AND PD42.DC_PHN IN ('H','A','W')
						AND COALESCE(PD42.DN_DOM_PHN_ARA,'') + COALESCE(PD42.DN_DOM_PHN_XCH,'') + COALESCE(PD42.DN_DOM_PHN_LCL,'') != ''
						AND DontText.friendly_from IS NULL
				) Ranked
				INNER JOIN
				(
					SELECT DISTINCT
						LN10.BF_SSN
					FROM
						UDW..LN10_LON LN10
						INNER JOIN UDW..GR10_RPT_LON_APL GR10
							ON GR10.BF_SSN = LN10.BF_SSN
							AND GR10.LN_SEQ = LN10.LN_SEQ
						LEFT JOIN
						(
							SELECT
								BF_SSN,
								PF_REQ_ACT
							FROM
								UDW..AY10_BR_LON_ATY AY10
							WHERE
								PF_REQ_ACT IN ('TEXTY','TEXTN')
								AND LC_STA_ACTY10 = 'A'
								AND LD_ATY_REQ_RCV = (SELECT MAX(LD_ATY_REQ_RCV) FROM UDW..AY10_BR_LON_ATY WHERE BF_SSN = AY10.BF_SSN AND PF_REQ_ACT IN ('TEXTY','TEXTN'))
							GROUP BY
								BF_SSN,
								LD_ATY_REQ_RCV,
								PF_REQ_ACT
						) AY10_T
							ON AY10_T.BF_SSN = LN10.BF_SSN
					WHERE
						LN10.LA_CUR_PRI > 0.00
						AND LN10.LC_STA_LON10 = 'R'
						AND
						(
							(LN10.IC_LON_PGM IN('STFFRD','UNSTFD','PLUS','PLUSGB') AND GR10.WD_BR_SIG_MPN >= '2009-10-01' AND AY10_T.BF_SSN IS NULL)
							OR
							(LN10.IC_LON_PGM IN('STFFRD','UNSTFD','PLUS','PLUSGB') AND GR10.WD_BR_SIG_MPN >= '2009-10-01' AND AY10_T.PF_REQ_ACT = 'TEXTY')
							OR
							(AY10_T.PF_REQ_ACT = 'TEXTY')
						)
						AND (AY10_T.BF_SSN IS NULL OR AY10_T.PF_REQ_ACT != 'TEXTN')
				) LN10 --Only allow MPN people
					ON LN10.BF_SSN = Ranked.DF_PRS_ID
				LEFT JOIN 
				(
					SELECT DISTINCT
						LN10.BF_SSN,
						MAX(AY10.LD_ATY_REQ_RCV) AS MaxArcDate
					FROM
						UDW..LN10_LON LN10
						LEFT JOIN UDW..AY10_BR_LON_ATY AY10
							ON AY10.BF_SSN = LN10.BF_SSN
							AND AY10.PF_REQ_ACT = 'BRTXT'
							AND AY10.LC_STA_ACTY10 = 'A'
					WHERE
						LN10.LC_STA_LON10 = 'R'
					GROUP BY
						LN10.BF_SSN
				) AY10
					ON AY10.BF_SSN = RANKED.DF_PRS_ID
				WHERE
					Ranked.OverallRank = 1
			) PhoneRank
				ON PhoneRank.DF_PRS_ID = PD10.DF_PRS_ID
				AND PhoneRank.PHN_RNK = 1
			INNER JOIN
			(
				SELECT
					BF_SSN,
					MAX(LN_DLQ_MAX) AS LN_DLQ_MAX
				FROM
					UDW..LN16_LON_DLQ_HST
				WHERE
					LC_STA_LON16 = 1
					AND CAST(LD_DLQ_MAX AS DATE) BETWEEN CAST(DATEADD(DAY, -4, GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE)
					AND LN_DLQ_MAX BETWEEN @DelqLower AND @DelqUpper
				GROUP BY
					BF_SSN
			) Delinquency
				ON Delinquency.BF_SSN = PD10.DF_PRS_ID
			LEFT JOIN 
			(
				SELECT DISTINCT
					BF_SSN
				FROM
					UDW..DW01_DW_CLC_CLU DW01
				WHERE
					(
						DW01.WC_DW_LON_STA IN ('16','17','18','19','20','21')
						OR DW01.WX_OVR_DW_LON_STA LIKE 'PENDING ID THEFT%' 
						OR DW01.WX_OVR_DW_LON_STA LIKE 'IDENTITY THEFT%' 
						OR DW01.WX_OVR_DW_LON_STA LIKE 'LITIGATION%' 
					)
			) BadStatus
				ON BadStatus.BF_SSN = PD10.DF_PRS_ID
			LEFT JOIN
			(
				SELECT
					ADR.DF_PRS_ID,
					ADR.DC_DOM_ST,
					ROW_NUMBER() OVER (PARTITION BY ADR.DF_PRS_ID ORDER BY ADR.PriorityNumber) [AddressPriority] -- number in order of Address
				FROM
				(
					SELECT
						PD30.DF_PRS_ID,
						PD30.DX_STR_ADR_1,
						PD30.DX_STR_ADR_2,
						PD30.DM_CT,
						PD30.DC_DOM_ST,
						PD30.DF_ZIP_CDE,
						PD30.DM_FGN_CNY,
						PD30.DM_FGN_ST,
						CASE	  
 							WHEN DC_ADR = 'L' THEN 1 -- legal 
 							WHEN DC_ADR = 'B' THEN 2 -- billing
 							WHEN DC_ADR = 'D' THEN 3 -- disbursement
 						END AS PriorityNumber
					FROM
						UDW..PD30_PRS_ADR PD30
						LEFT JOIN CentralData..StrictStates SS
							ON SS.StateCode = PD30.DC_DOM_ST
					WHERE
						SS.StateCode IS NULL -- Not in strict state address list
				) ADR
			) AddressInfo
				ON AddressInfo.DF_PRS_ID = PD10.DF_PRS_ID
				AND AddressInfo.AddressPriority = 1
			LEFT JOIN
			(
				SELECT DISTINCT
					LN10.BF_SSN,
					MAX(AY10.LD_ATY_REQ_RCV) AS MaxArcDate
				FROM
					UDW..LN10_LON LN10
					LEFT JOIN UDW..AY10_BR_LON_ATY AY10
						ON AY10.BF_SSN = LN10.BF_SSN
						AND AY10.PF_REQ_ACT = 'BRTXT'
						AND AY10.LC_STA_ACTY10 = 'A'
				WHERE
					LN10.LC_STA_LON10 = 'R'
				GROUP BY
					LN10.BF_SSN
			) Arc
				ON Arc.BF_SSN = PD10.DF_PRS_ID
			LEFT JOIN
			(
				SELECT DISTINCT
					AccountNumber
				FROM
					ULS.textcoord.TrackingData
				WHERE
					CAST(CreatedAt AS DATE) = CAST(GETDATE() AS DATE)
					AND DeletedAt IS NULL
			) ExistingTracking
				ON ExistingTracking.AccountNumber = PD10.DF_SPE_ACC_ID
			LEFT JOIN #CALLS CallsToday
				ON CallsToday.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
		WHERE
			CallsToday.DF_SPE_ACC_ID IS NULL --Didnt have a call today
			AND CAST(ISNULL(Arc.MaxArcDate,'1900-01-01') AS DATE) <= CAST(DATEADD(DAY,-7,GETDATE()) AS DATE) --Can have arc as long as it isnt in last 7 days
			AND ExistingTracking.AccountNumber IS NULL --No insert into tracking table today
			AND AddressInfo.DF_PRS_ID IS NOT NULL --Must have a valid address
			AND BadStatus.BF_SSN IS NULL --No borrowers having a bad status
			AND DATEDIFF(YEAR, PD10.DD_BRT, GETDATE()) BETWEEN @AgeLower AND @AgeUpper
	) FINAL_DATA
ORDER BY
	CAST(FINAL_DATA.LastBRTXT AS DATE),
	last4
END