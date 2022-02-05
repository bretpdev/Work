DECLARE @PrevWorkDay DATE = 
(--if run on Monday then do Saturday, else do yesterday
	SELECT
		CASE
			WHEN DATENAME(WEEKDAY,GETDATE()) = 'Monday'
			THEN DATEADD(DAY,-3,GETDATE())
			ELSE DATEADD(DAY,-1,GETDATE())
		END
);
DECLARE @Yesterday DATE = DATEADD(DAY,-1,GETDATE());

SELECT
	P.AccountNumber AS [Account Number],
	'Borrower Payment' AS [Payment Description],
	DC11.LA_TRX AS [Payment Amount],
	CONVERT(VARCHAR(8),DC11.LD_TRX_EFF,1) AS [Transaction Date],
	P.AccountNumber AS [Account Number2],
	'Payment' AS [Category Code],
	'' AS [Input Source Code],
	'' AS [NSF Check Amount],
	'' AS [Number of Debits],
	'' AS [Original Tran Info],
	'' AS [Reference Number]
FROM
	OLS.trueaccord.Placements P
	INNER JOIN ODW..PD01_PDM_INF PD01
		ON PD01.DF_SPE_ACC_ID = P.AccountNumber
	INNER JOIN ODW..DC01_LON_CLM_INF DC01
		ON DC01.BF_SSN = PD01.DF_PRS_ID
		AND DC01.AF_APL_ID = P.AF_APL_ID
		AND DC01.AF_APL_ID_SFX = P.AF_APL_ID_SFX
	INNER JOIN ODW..DC11_LON_FAT DC11
		ON DC11.AF_APL_ID = DC01.AF_APL_ID
		AND DC11.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
		AND DC11.LF_CRT_DTS_DC10 = DC01.LF_CRT_DTS_DC10
		AND DC11.LC_REV_IND_TYP = ''
		AND DC11.LC_TRX_TYP = 'BR'
WHERE
	P.DeletedAt IS NULL
	AND P.RetractedAt IS NULL
	AND CAST(DC11.LF_LST_DTS_DC11 AS DATE) BETWEEN @PrevWorkDay AND @Yesterday


