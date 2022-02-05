/************ main query converted from SAS ***********************/
USE CDW
GO

TRUNCATE TABLE CLS.scra.ActiveDutyValidation;

;WITH BOR AS
(--borrower data
	SELECT
		PD10.DF_SPE_ACC_ID
		,LN10.BF_SSN
		,PD10.DM_PRS_1
		,PD10.DM_PRS_MID
		,PD10.DM_PRS_LST
		,CAST(PD10.DD_BRT AS DATE) [DD_BRT]
		,CAST(GETDATE() AS DATE) [ACTIVE_DUTY_STATUS_DATE]
		--,'B' AS FLAG
	FROM
		PD10_PRS_NME PD10
		INNER JOIN LN10_LON LN10
			ON PD10.DF_PRS_ID = LN10.BF_SSN
		INNER JOIN DW01_DW_CLC_CLU DW01
			ON PD10.DF_PRS_ID = DW01.BF_SSN
			AND LN10.LN_SEQ = DW01.LN_SEQ
	WHERE
		(--delinquency transfer status:
			LN10.LC_STA_LON10 IN ('R','L') --R=released; L=litigation
			OR 
			(
				LN10.LC_STA_LON10 = 'D' 
				AND LN10.LC_SST_LON10 IN ('4','7','8')
			)
		)
		AND DW01.WC_DW_LON_STA NOT IN ('17','22') --17=verified death; 22=paid in full
		AND PD10.DD_BRT IS NOT NULL
		AND LN10.BF_SSN NOT LIKE 'P%'
		AND NOT 
		(
			LC_STA_LON10 = 'D' --deconverted
			AND LC_SST_LON10 <> '4'
		)
),
EDR AS
(--endorser data
	SELECT
		PD10.DF_SPE_ACC_ID
		,LN20.LF_EDS [BF_SSN]
		,PD10.DM_PRS_1
		,PD10.DM_PRS_MID
		,PD10.DM_PRS_LST
		,CAST(PD10.DD_BRT AS DATE) [DD_BRT]
		,CAST(GETDATE() AS DATE) [ACTIVE_DUTY_STATUS_DATE]
		--,'E' AS FLAG
	FROM 
		BOR
		INNER JOIN LN20_EDS LN20
			ON BOR.BF_SSN = LN20.BF_SSN
		INNER JOIN PD10_PRS_NME PD10
			ON LN20.LF_EDS = PD10.DF_PRS_ID
	WHERE 
		LN20.LC_STA_LON20 = 'A'
		AND PD10.DD_BRT IS NOT NULL
		AND LN20.LF_EDS NOT LIKE 'P%'
),
BOR_EDR AS
(--earmarks output group based on number
	SELECT
		*,
		CASE
			WHEN [TALLY] BETWEEN 1 AND 249999		THEN 'UTNWS81R1'
			WHEN [TALLY] BETWEEN 250000 AND 499998	THEN 'UTNWS81R2'
			WHEN [TALLY] BETWEEN 499999 AND 749997	THEN 'UTNWS81R3'
			WHEN [TALLY] BETWEEN 749998 AND 999996	THEN 'UTNWS81R4'
			WHEN [TALLY] BETWEEN 999997 AND 1249995	THEN 'UTNWS81R5'
			ELSE 'UTNWS81R6'
		END AS [FILENAME]
	FROM
	(--puts a distinct number on each record
		SELECT
			ROW_NUMBER() OVER(ORDER BY BF_SSN) [TALLY]
			,*
		FROM
		(--puts borrower and endorser data together
			SELECT DISTINCT * FROM EDR

			UNION

			SELECT DISTINCT	* FROM BOR
		) A
	)B
)
INSERT INTO
	CLS.[scra].[ActiveDutyValidation]
	(
		[TALLY]
		,[DF_SPE_ACC_ID]
		,[BF_SSN]
		,[DM_PRS_1]
		,[DM_PRS_MID]
		,[DM_PRS_LST]
		,[DD_BRT]
		,[ACTIVE_DUTY_STATUS_DATE]
		,[FILENAME]
		,[SSIS_DOB]
		,[SSIS_ADSD]
	)
SELECT
	[TALLY]
	,[DF_SPE_ACC_ID]
	,[BF_SSN]
	,[DM_PRS_1]
	,[DM_PRS_MID] + '       ' [DM_PRS_MID]
	,[DM_PRS_LST]
	,[DD_BRT]
	,[ACTIVE_DUTY_STATUS_DATE]
	,[FILENAME]
	,FORMAT(DD_BRT,'yyyyMMdd','en-US') [SSIS_DOB]
	,FORMAT([ACTIVE_DUTY_STATUS_DATE],'yyyyMMdd','en-US') [SSIS_ADSD]
FROM
	BOR_EDR
;

/************ SSIS queries ***********************/

--execute SQL task query:
SELECT DISTINCT [FILENAME] FROM [CLS].[scra].[ActiveDutyValidation]

--data flow OLEDB source query:
SELECT
	*
FROM
(
	SELECT
		BF_SSN
		,SSIS_DOB
		,DM_PRS_LST
		,DM_PRS_1
		,DF_SPE_ACC_ID
		,SSIS_PLACEHOLDER
		,SSIS_ADSD
		,DM_PRS_MID
	FROM
		[CLS].[scra].[ActiveDutyValidation]
	WHERE 
		[FILENAME]=?

	UNION

	SELECT
		'EOF' [BF_SSN]
		,'' [SSIS_DOB]
		,'' [DM_PRS_LST]
		,'' [DM_PRS_1]
		,'' [DF_SPE_ACC_ID]
		,'' [SSIS_PLACEHOLDER]
		,'' [SSIS_ADSD]
		,'' [DM_PRS_MID]
)U
ORDER BY
	BF_SSN
