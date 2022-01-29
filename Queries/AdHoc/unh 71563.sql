--We would like to a quick query performed to identify all accounts that entered into a Rehabilitation Agreement 
--on or after 6/1/2019 and have an open balance in OneLINK.These would be accounts that have a DRWTN action code on 
--or after 6/1/2019 and the total balance is greater than $0.00. 

--For the output we would like the following data fields:

--SSN
--Account number
--Name (can be separated into first and last name if needed)
--Oustanding principal balance 
--Number of rehab payments (BN_RHB_PAY_CTR)
USE ODW
GO

SELECT
	PD01.DF_PRS_ID AS SSN,
	PD01.DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
	RTRIM(PD01.DM_PRS_1) + ' ' + RTRIM(PD01.DM_PRS_LST) AS [NAME],
	SUM(DC02.LA_CLM_BAL) AS CurrentBalance,
	BR01.BN_RHB_PAY_CTR
FROM
	ODW..PD01_PDM_INF PD01
	INNER JOIN 
	(
		SELECT DISTINCT
			DC01.BF_SSN,
			DC01.AF_APL_ID,
			DC01.AF_APL_ID_SFX,
			MAX(DC01.LF_CRT_DTS_DC10) AS MaxDate
		FROM
			ODW..DC01_LON_CLM_INF DC01
	
		WHERE
			DC01.LC_PCL_REA = 'DF'
			AND DC01.LC_STA_DC10 = '03'
			AND DC01.LD_CLM_ASN_DOE IS NULL
			AND LTRIM(ISNULL(DC01.LC_AUX_STA,'')) = ''
			AND ISNULL(DC01.LC_REA_CLM_ASN_DOE,'') = ''
		
		GROUP BY
			DC01.BF_SSN,
			DC01.AF_APL_ID,
			DC01.AF_APL_ID_SFX
	) DC01
		ON DC01.BF_SSN = PD01.DF_PRS_ID
	INNER JOIN ODW..DC02_BAL_INT DC02
		ON DC02.AF_APL_ID = DC01.AF_APL_ID
		AND DC02.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
		AND DC02.LF_CRT_DTS_DC10 = DC01.MaxDate
		AND DC02.LA_CLM_BAL > 0.00
	INNER JOIN ODW..BR01_BR_CRF BR01
		ON BR01.BF_SSN = PD01.DF_PRS_ID
	INNER JOIN ODW..AY01_BR_ATY AY01
		ON AY01.DF_PRS_ID = PD01.DF_PRS_ID
		AND AY01.PF_ACT = 'DRWTN'
		AND AY01.BF_CRT_DTS_AY01 >= '06/01/2019'
GROUP BY
	PD01.DF_PRS_ID ,
	PD01.DF_SPE_ACC_ID ,
	RTRIM(PD01.DM_PRS_1) + ' ' + RTRIM(PD01.DM_PRS_LST),
	BR01.BN_RHB_PAY_CTR
ORDER BY 
	SSN