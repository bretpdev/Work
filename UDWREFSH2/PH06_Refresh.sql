USE UDW
GO

DECLARE 
	@SSNs VARCHAR(MAX) = '0', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(DATEADD(HOUR,-3,MAX(PH06.HF_LST_DTS_PH05)), '1-1-1900 00:00:00'), 21) FROM PH06_CNC_EML_HST PH06)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.PH06_CNC_EML_HST PH06
	USING
		(
			SELECT
				*
			FROM
				OPENQUERY
				(
					DUSTER,
					''
						SELECT
							HF_CNC_SYS_ID
							,RIGHT(CONCAT(''''0000000000'''', CAST(HF_SPE_ID AS VARCHAR(10))), 10) AS "HF_SPE_ID"
							,DF_CRT_DTS_PH06
							,HX_CNC_EML_ADR
							,HC_EML_APL_SYS_SRC
							,HC_CNC_EML_DAT_SRC
							,HF_DTS_EML_ADR_EFF
							,HF_LST_USR_EML_ADR
							,HI_VLD_CNC_EML_ADR
							,HC_VLD_EML_APL_SRC
							,HC_VLD_CNC_EML_SRC
							,HF_DTS_VLD_EML_EFF
							,HF_LST_USR_VLD_EML
							,HI_CNC_ELT_OPI
							,HC_ELT_OPI_APL_SRC
							,HC_ELT_OPI_SRC
							,HF_DTS_ELT_OPI_EFF
							,HF_LST_USR_ELT_OPI
							,HI_CNC_EBL_OPI
							,HC_EBL_OPI_APL_SRC
							,HC_EBL_OPI_SRC
							,HF_DTS_EBL_OPI_EFF
							,HF_LST_USR_EBL_OPI
							,HF_LST_DTS_PH05
							,HF_LST_USR_PH05
							,HD_CNC_EML_ADR_VER
							,HF_LST_DTS_EML_VER
							,HC_LST_EML_VER_SYS
							,HC_LST_EML_VER_SRC
							,HF_LST_USR_EML_VER
							,HI_CNC_TAX_OPI
							,HC_TAX_OPI_APL_SRC
							,HC_TAX_OPI_SRC
							,HF_DTS_TAX_OPI_EFF
							,HF_LST_USR_TAX_OPI
							,HF_ACP_TRM_VRS_ID
							,HF_DTS_ACP_TRM_EFF
							,HF_LST_USR_ACP_TRM
						FROM
							OLWHRM1.PH06_CNC_EML_HST PH06

					''
				) 
		) L 
			ON L.HF_CNC_SYS_ID = PH06.HF_CNC_SYS_ID 
			AND L.HF_SPE_ID = PH06.HF_SPE_ID 
			AND L.DF_CRT_DTS_PH06 = PH06.DF_CRT_DTS_PH06 -- add in DF_SPE_ID leading zeros; PH05 data type is decimal
	WHEN MATCHED THEN 
		UPDATE SET 
			PH06.HX_CNC_EML_ADR	  = L.HX_CNC_EML_ADR,
			PH06.HC_EML_APL_SYS_SRC = L.HC_EML_APL_SYS_SRC,
			PH06.HC_CNC_EML_DAT_SRC = L.HC_CNC_EML_DAT_SRC,
			PH06.HF_DTS_EML_ADR_EFF = L.HF_DTS_EML_ADR_EFF,
			PH06.HF_LST_USR_EML_ADR = L.HF_LST_USR_EML_ADR,
			PH06.HI_VLD_CNC_EML_ADR = L.HI_VLD_CNC_EML_ADR,
			PH06.HC_VLD_EML_APL_SRC = L.HC_VLD_EML_APL_SRC,
			PH06.HC_VLD_CNC_EML_SRC = L.HC_VLD_CNC_EML_SRC,
			PH06.HF_DTS_VLD_EML_EFF = L.HF_DTS_VLD_EML_EFF,
			PH06.HF_LST_USR_VLD_EML = L.HF_LST_USR_VLD_EML,
			PH06.HI_CNC_ELT_OPI	  = L.HI_CNC_ELT_OPI,
			PH06.HC_ELT_OPI_APL_SRC = L.HC_ELT_OPI_APL_SRC,
			PH06.HC_ELT_OPI_SRC	  = L.HC_ELT_OPI_SRC,
			PH06.HF_DTS_ELT_OPI_EFF = L.HF_DTS_ELT_OPI_EFF,
			PH06.HF_LST_USR_ELT_OPI = L.HF_LST_USR_ELT_OPI,
			PH06.HI_CNC_EBL_OPI	  = L.HI_CNC_EBL_OPI,
			PH06.HC_EBL_OPI_APL_SRC = L.HC_EBL_OPI_APL_SRC,
			PH06.HC_EBL_OPI_SRC	  = L.HC_EBL_OPI_SRC,
			PH06.HF_DTS_EBL_OPI_EFF = L.HF_DTS_EBL_OPI_EFF,
			PH06.HF_LST_USR_EBL_OPI = L.HF_LST_USR_EBL_OPI,
			PH06.HF_LST_DTS_PH05	  = L.HF_LST_DTS_PH05,
			PH06.HF_LST_USR_PH05	  = L.HF_LST_USR_PH05,
			PH06.HD_CNC_EML_ADR_VER = L.HD_CNC_EML_ADR_VER,
			PH06.HF_LST_DTS_EML_VER = L.HF_LST_DTS_EML_VER,
			PH06.HC_LST_EML_VER_SYS = L.HC_LST_EML_VER_SYS,
			PH06.HC_LST_EML_VER_SRC = L.HC_LST_EML_VER_SRC,
			PH06.HF_LST_USR_EML_VER = L.HF_LST_USR_EML_VER,
			PH06.HI_CNC_TAX_OPI	  = L.HI_CNC_TAX_OPI,
			PH06.HC_TAX_OPI_APL_SRC = L.HC_TAX_OPI_APL_SRC,
			PH06.HC_TAX_OPI_SRC	  = L.HC_TAX_OPI_SRC,
			PH06.HF_DTS_TAX_OPI_EFF = L.HF_DTS_TAX_OPI_EFF,
			PH06.HF_LST_USR_TAX_OPI = L.HF_LST_USR_TAX_OPI,
			PH06.HF_ACP_TRM_VRS_ID  = L.HF_ACP_TRM_VRS_ID ,
			PH06.HF_DTS_ACP_TRM_EFF = L.HF_DTS_ACP_TRM_EFF,
			PH06.HF_LST_USR_ACP_TRM = L.HF_LST_USR_ACP_TRM
	WHEN NOT MATCHED THEN
		INSERT 
		(
			HF_CNC_SYS_ID	 	,
			HF_SPE_ID		 	,
			DF_CRT_DTS_PH06	, 
			HX_CNC_EML_ADR	, 
			HC_EML_APL_SYS_SRC,
			HC_CNC_EML_DAT_SRC,
			HF_DTS_EML_ADR_EFF,
			HF_LST_USR_EML_ADR,
			HI_VLD_CNC_EML_ADR,
			HC_VLD_EML_APL_SRC,
			HC_VLD_CNC_EML_SRC,
			HF_DTS_VLD_EML_EFF,
			HF_LST_USR_VLD_EML,
			HI_CNC_ELT_OPI	, 
			HC_ELT_OPI_APL_SRC,
			HC_ELT_OPI_SRC	, 
			HF_DTS_ELT_OPI_EFF,
			HF_LST_USR_ELT_OPI,
			HI_CNC_EBL_OPI	, 
			HC_EBL_OPI_APL_SRC,
			HC_EBL_OPI_SRC	, 
			HF_DTS_EBL_OPI_EFF,
			HF_LST_USR_EBL_OPI,
			HF_LST_DTS_PH05	, 
			HF_LST_USR_PH05	, 
			HD_CNC_EML_ADR_VER,
			HF_LST_DTS_EML_VER,
			HC_LST_EML_VER_SYS,
			HC_LST_EML_VER_SRC,
			HF_LST_USR_EML_VER,
			HI_CNC_TAX_OPI	, 
			HC_TAX_OPI_APL_SRC,
			HC_TAX_OPI_SRC	, 
			HF_DTS_TAX_OPI_EFF,
			HF_LST_USR_TAX_OPI,
			HF_ACP_TRM_VRS_ID ,
			HF_DTS_ACP_TRM_EFF,
			HF_LST_USR_ACP_TRM
		)
		VALUES 
		(
			L.HF_CNC_SYS_ID	  ,
			L.HF_SPE_ID		  ,
			L.DF_CRT_DTS_PH06	  ,
			L.HX_CNC_EML_ADR	  ,
			L.HC_EML_APL_SYS_SRC,
			L.HC_CNC_EML_DAT_SRC,
			L.HF_DTS_EML_ADR_EFF,
			L.HF_LST_USR_EML_ADR,
			L.HI_VLD_CNC_EML_ADR,
			L.HC_VLD_EML_APL_SRC,
			L.HC_VLD_CNC_EML_SRC,
			L.HF_DTS_VLD_EML_EFF,
			L.HF_LST_USR_VLD_EML,
			L.HI_CNC_ELT_OPI	  ,
			L.HC_ELT_OPI_APL_SRC,
			L.HC_ELT_OPI_SRC	  ,
			L.HF_DTS_ELT_OPI_EFF,
			L.HF_LST_USR_ELT_OPI,
			L.HI_CNC_EBL_OPI	  ,
			L.HC_EBL_OPI_APL_SRC,
			L.HC_EBL_OPI_SRC	  ,
			L.HF_DTS_EBL_OPI_EFF,
			L.HF_LST_USR_EBL_OPI,
			L.HF_LST_DTS_PH05	  ,
			L.HF_LST_USR_PH05	  ,
			L.HD_CNC_EML_ADR_VER,
			L.HF_LST_DTS_EML_VER,
			L.HC_LST_EML_VER_SYS,
			L.HC_LST_EML_VER_SRC,
			L.HF_LST_USR_EML_VER,
			L.HI_CNC_TAX_OPI	  ,
			L.HC_TAX_OPI_APL_SRC,
			L.HC_TAX_OPI_SRC	  ,
			L.HF_DTS_TAX_OPI_EFF,
			L.HF_LST_USR_TAX_OPI,
			L.HF_ACP_TRM_VRS_ID ,
			L.HF_DTS_ACP_TRM_EFF,
			L.HF_LST_USR_ACP_TRM
		)
	-- !!! uncomment lines below ONLY when doing a full table refresh 
	--WHEN NOT MATCHED BY SOURCE THEN
	--    DELETE
	;
'

PRINT @SQLStatement
EXEC (@SQLStatement)

-- ###### VALIDATION
DECLARE 
	@CountDifference INT

SELECT
	@CountDifference = L.LocalCount - R.RemoteCount
FROM
	OPENQUERY
	(
		DUSTER,
		'
			SELECT
				COUNT(*) AS "RemoteCount"
			FROM
				OLWHRM1.PH06_CNC_EML_HST
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) LocalCount
		FROM
			PH06_CNC_EML_HST
	) L 
		ON 1 = 1
	

IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('OLWHRM1.PH06_CNC_EML_HST - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is requireL.', 16, 11, @CountDifference)
	END
ELSE IF @CountDifference != 0 AND @LoopCount = 0
	BEGIN

		SET @LoopCount = @LoopCount + 1
		
		DECLARE @SSN_LIST TABLE
		(
			R_BF_SSN CHAR(10),
			L_BF_SSN CHAR(10)
		)

		PRINT 'Insert SSN (HF_SPE_ID) with inconsistent counts'
		INSERT INTO
			@SSN_LIST
		SELECT TOP 20
			R.HF_SPE_ID,
			L.HF_SPE_ID
		FROM
			OPENQUERY
			(
				DUSTER,	
				'
					SELECT
						RIGHT(CONCAT(''0000000000'', CAST(PH06.HF_SPE_ID AS VARCHAR(10))), 10) AS "HF_SPE_ID",
						COUNT(*) AS "RemoteCount"
					FROM
						OLWHRM1.PH06_CNC_EML_HST PH06
					GROUP BY
						PH06.HF_SPE_ID
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					PH06.HF_SPE_ID,
					COUNT(*) [LocalCount]
				FROM
					UDW..PH06_CNC_EML_HST PH06
				GROUP BY
					PH06.HF_SPE_ID
			) L 
				ON L.HF_SPE_ID = R.HF_SPE_ID
		WHERE
			ISNULL(L.LocalCount, 0) != ISNULL(R.RemoteCount, 0)

		SELECT
			@SSNs = 
			(
				SELECT
					'''''' + COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) + ''''',' AS [text()]
				FROM
					@SSN_LIST SL
				ORDER BY
					COALESCE(SL.L_BF_SSN, SL.R_BF_SSN)
				FOR XML PATH ('')
			)

		SELECT	@SSNs = LEFT(@SSNs, LEN(@SSNs) -1)

		PRINT 'The local record count for these SSNs (HF_SPE_ID) does not match the remote warehouse count: ' + @SSNs + '  Deleting all local PH05 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			PH06
		FROM
			UDW..PH06_CNC_EML_HST PH06
			INNER JOIN @SSN_LIST SL 
				ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = PH06.HF_SPE_ID

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		GOTO RefreshStart;

	END