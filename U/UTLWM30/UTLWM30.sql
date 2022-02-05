DECLARE @ScriptId VARCHAR(10) = 'UTLWM30'
DECLARE @AddedAt DATETIME = GETDATE()
DECLARE @Today DATE = GETDATE()

INSERT INTO OLS.olqtskbldr.Queues
(
	TargetId,
	QueueName,
	InstitutionId,
	InstitutionType,
	DateDue,
	TimeDue,
	Comment,
	SourceFilename,
	AddedAt,
	AddedBy
)
SELECT DISTINCT
	LOANS.BF_SSN AS TargetId,
	LOANS.QueueName AS QueueName,
	'' AS InstitutionId,
	'' AS InstitutionType,
	NULL AS DateDue,
	NULL AS TimeDue,
	'' AS Comment,
	'' AS SourceFilename,
	@AddedAt AS AddedAt,
	@ScriptId AS AddedBy
FROM
	(
		--LOANS
		SELECT DISTINCT 
			GA01.DF_PRS_ID_BR AS BF_SSN,
			CASE 
				WHEN BR01.BN_RHB_PAY_CTR = 5 THEN 'REHABCAN' --R3
				WHEN BR01.BN_RHB_PAY_CTR = 6 THEN 'RHBCAN6' --R5
				WHEN BR01.BN_RHB_PAY_CTR = 7 THEN 'RHBCAN7' --R6
				WHEN LA11.BC_LEG_ACT_ATY_TYP = 'JD' THEN 'RHBCANJD' --R12
				WHEN BR01.BN_RHB_PAY_CTR = 8 THEN 'RHBCAN8' --R7
				ELSE NULL --Don't include
			END AS QueueName,
			GA10.AF_APL_ID + GA10.AF_APL_ID_SFX AS CLUID,
			DC01.LD_LDR_POF,
			DC01.LA_CLM_PRI,
			DC01.LA_CLM_INT,
			DC01.LA_PRI_COL,
			DC01.LA_INT_ACR,
			DC01.LA_INT_COL,
			DC01.LA_LEG_CST_ACR, 
			DC01.LA_LEG_CST_COL, 
			DC01.LA_OTH_CHR_ACR, 
			DC01.LA_OTH_CHR_COL, 
			DC01.LA_COL_CST_ACR, 
			DC01.LA_COL_CST_COL,
			DC01.LI_DIR_PAY,
			DC01.LD_LST_BR_PAY, 
			DC01.LD_NXT_PAY_DUE,
			COALESCE(DC02.LA_CLM_INT_ACR,0) AS LA_CLM_INT_ACR,
			COALESCE(DC02.LA_CLM_PRJ_COL_CST,0) AS LA_CLM_PRJ_COL_CST,
			DC01.LC_GRN,
			PD01.DF_SPE_ACC_ID,
			PD01.DM_PRS_1,
			PD01.DM_PRS_LST,
			PD01.DI_PHN_VLD,
			AY01.BD_ATY_PRF,
			AY01.PF_ACT,
			(DC01.LA_CLM_PRI + DC01.LA_CLM_INT -DC01.LA_PRI_COL +DC01.LA_INT_ACR + DC02.LA_CLM_INT_ACR - DC01.LA_INT_COL)	+ (DC01.LA_LEG_CST_ACR - DC01.LA_LEG_CST_COL + DC01.LA_OTH_CHR_ACR	
				-DC01.LA_OTH_CHR_COL + DC01.LA_COL_CST_ACR - DC01.LA_COL_CST_COL) + (DC02.LA_CLM_PRJ_COL_CST)
			AS TOT_PAYOFF,
			DC02.LR_CUR_INT / 1200 AS RATE,
			111 AS MONTHS,
			BR01.BN_RHB_PAY_CTR,
			DC01.LD_PCL_SUP_LST_ATT, 
			DC01.LD_PCL_SUP_LST_CNC,
			DC01.LC_RHB_CON,
			LA11.BC_LEG_ACT_ATY_TYP 
		FROM 
			ODW..GA01_APP GA01
			INNER JOIN ODW..GA10_LON_APP GA10
				ON GA01.AF_APL_ID = GA10.AF_APL_ID
			INNER JOIN ODW..DC01_LON_CLM_INF DC01
				ON GA10.AF_APL_ID = DC01.AF_APL_ID
				AND GA10.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
			INNER JOIN ODW..DC02_BAL_INT DC02
				ON DC01.AF_APL_ID = DC02.AF_APL_ID
				AND DC01.AF_APL_ID_SFX = DC02.AF_APL_ID_SFX
			INNER JOIN ODW..PD01_PDM_INF PD01
				ON DC01.BF_SSN = PD01.DF_PRS_ID
			INNER JOIN ODW..BR01_BR_CRF BR01
				ON DC01.BF_SSN = BR01.BF_SSN
			LEFT JOIN ODW..LA11_LEG_ACT_ATY LA11
				ON GA01.DF_PRS_ID_BR = LA11.DF_PRS_ID_BR
				AND BC_LEG_ACT_ATY_TYP = 'JD' 
			LEFT JOIN 
			(
				SELECT 
					DF_PRS_ID,
					MAX(BD_ATY_PRF) AS BD_ATY_PRF,
					PF_ACT
				FROM 
					ODW..AY01_BR_ATY
				WHERE 
					PF_ACT IN ('DLHB1','DLHB2','DLHB3','DLHB4','DLBH1','DRTRE','DRBWR')
				GROUP BY 
					DF_PRS_ID,
					PF_ACT
			) AY01 
				ON PD01.DF_PRS_ID = AY01.DF_PRS_ID
		WHERE 
			DC01.LC_STA_DC10 = '03'
			AND DC01.LC_AUX_STA = ''
			AND DC01.LD_CLM_ASN_DOE IS NULL
			AND DC01.LC_PCL_REA IN ('DF','DQ','DB')
			AND DC01.LC_RHB_CON != '96'
			AND BR01.BC_RHB_NOT_ELG IN ('LB','')
			AND (DC01.LA_CLM_PRI + DC01.LA_CLM_INT -DC01.LA_PRI_COL +DC01.LA_INT_ACR + DC02.LA_CLM_INT_ACR - DC01.LA_INT_COL)	
				+ (DC01.LA_LEG_CST_ACR - DC01.LA_LEG_CST_COL + DC01.LA_OTH_CHR_ACR	- DC01.LA_OTH_CHR_COL + DC01.LA_COL_CST_ACR - DC01.LA_COL_CST_COL)
				+ (DC02.LA_CLM_PRJ_COL_CST) >= 500 --TOT_PAYOFF
	) LOANS
	--check for existing record to add queue task for the current date
	LEFT JOIN  OLS.olqtskbldr.Queues ExistingData
		ON ExistingData.TargetId = LOANS.BF_SSN
		AND ExistingData.QueueName = LOANS.QueueName
		AND 
		(
			CAST(ExistingData.AddedAt AS DATE) = @Today
			OR ExistingData.ProcessedAt IS NULL
		)
		AND ExistingData.DeletedAt IS NULL
		AND ExistingData.DeletedBy IS NULL
WHERE
	LOANS.BN_RHB_PAY_CTR >= 3
	AND LOANS.QueueName IS NOT NULL
	AND ExistingData.TargetId IS NULL
	AND
	(
		ISNULL(LOANS.BC_LEG_ACT_ATY_TYP,'') = 'JD'
		OR
		(
			LOANS.DI_PHN_VLD = 'Y' 
			AND LOANS.LD_LST_BR_PAY < DATEADD(DAY, -20, @Today)
			AND LOANS.LD_NXT_PAY_DUE BETWEEN DATEADD(DAY, -5, @Today) AND @Today
		)
	)	
;