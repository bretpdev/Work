USE CDW
GO

DECLARE 
	@SSNs VARCHAR(MAX) = '0', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(DATEADD(HOUR,-3,MAX(PH05.DF_LST_DTS_PH05)), '1-1-1900 00:00:00'), 21) FROM PH05_CNC_EML PH05)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.PH05_CNC_EML PH05
	USING
		(
			SELECT
				*
			FROM
				OPENQUERY
				(
					LEGEND,
					''
						SELECT
							DF_CNC_SYS_ID,
							RIGHT(CONCAT(''''0000000000'''', CAST(DF_SPE_ID AS VARCHAR(10))), 10) AS "DF_SPE_ID",  -- add in DF_SPE_ID leading zeros; PH05 data type is decimal
							DX_CNC_EML_ADR,
							DC_EML_APL_SYS_SRC,
							DC_CNC_EML_DAT_SRC,
							DF_DTS_EML_ADR_EFF,
							DF_LST_USR_EML_ADR,
							DI_VLD_CNC_EML_ADR,
							DC_VLD_EML_APL_SRC,
							DC_VLD_CNC_EML_SRC,
							DF_DTS_VLD_EML_EFF,
							DF_LST_USR_VLD_EML,
							DI_CNC_ELT_OPI,
							DC_ELT_OPI_APL_SRC,
							DC_ELT_OPI_SRC,
							DF_DTS_ELT_OPI_EFF,
							DF_LST_USR_ELT_OPI,
							DI_CNC_EBL_OPI,
							DC_EBL_OPI_APL_SRC,
							DC_EBL_OPI_SRC,
							DF_DTS_EBL_OPI_EFF,
							DF_LST_USR_EBL_OPI,
							DF_LST_DTS_PH05,
							DF_LST_USR_PH05,
							DD_CNC_EML_ADR_VER,
							DF_LST_DTS_EML_VER,
							DC_LST_EML_VER_SYS,
							DC_LST_EML_VER_SRC,
							DX_CNC_EML_ADR_UC,
							DF_LST_USR_EML_VER,
							DI_CNC_TAX_OPI,
							DC_TAX_OPI_APL_SRC,
							DC_TAX_OPI_SRC,
							DF_DTS_TAX_OPI_EFF,
							DF_LST_USR_TAX_OPI,
							DF_ACP_TRM_VRS_ID,
							DF_DTS_ACP_TRM_EFF,
							DF_LST_USR_ACP_TRM
						FROM
							AES.PH05_CNC_EML PH05
						-- comment WHERE clause for full table refresh
						WHERE
							PH05.DF_LST_DTS_PH05 > ''''' + @LastRefresh + '''''
							OR
							(
								PH05.DF_SPE_ID IN
								(
									' + @SSNs + '
								)
							)
					''
				) 
		) L 
			ON L.DF_CNC_SYS_ID = PH05.DF_CNC_SYS_ID 
			AND L.DF_SPE_ID = PH05.DF_SPE_ID -- add in DF_SPE_ID leading zeros; PH05 data type is decimal
	WHEN MATCHED THEN 
		UPDATE SET 
			PH05.DX_CNC_EML_ADR = L.DX_CNC_EML_ADR,
			PH05.DC_EML_APL_SYS_SRC = L.DC_EML_APL_SYS_SRC,
			PH05.DC_CNC_EML_DAT_SRC = L.DC_CNC_EML_DAT_SRC,
			PH05.DF_DTS_EML_ADR_EFF = L.DF_DTS_EML_ADR_EFF,
			PH05.DF_LST_USR_EML_ADR = L.DF_LST_USR_EML_ADR,
			PH05.DI_VLD_CNC_EML_ADR = L.DI_VLD_CNC_EML_ADR,
			PH05.DC_VLD_EML_APL_SRC = L.DC_VLD_EML_APL_SRC,
			PH05.DC_VLD_CNC_EML_SRC = L.DC_VLD_CNC_EML_SRC,
			PH05.DF_DTS_VLD_EML_EFF = L.DF_DTS_VLD_EML_EFF,
			PH05.DF_LST_USR_VLD_EML = L.DF_LST_USR_VLD_EML,
			PH05.DI_CNC_ELT_OPI = L.DI_CNC_ELT_OPI,
			PH05.DC_ELT_OPI_APL_SRC = L.DC_ELT_OPI_APL_SRC,
			PH05.DC_ELT_OPI_SRC = L.DC_ELT_OPI_SRC,
			PH05.DF_DTS_ELT_OPI_EFF = L.DF_DTS_ELT_OPI_EFF,
			PH05.DF_LST_USR_ELT_OPI = L.DF_LST_USR_ELT_OPI,
			PH05.DI_CNC_EBL_OPI = L.DI_CNC_EBL_OPI,
			PH05.DC_EBL_OPI_APL_SRC = L.DC_EBL_OPI_APL_SRC,
			PH05.DC_EBL_OPI_SRC = L.DC_EBL_OPI_SRC,
			PH05.DF_DTS_EBL_OPI_EFF = L.DF_DTS_EBL_OPI_EFF,
			PH05.DF_LST_USR_EBL_OPI = L.DF_LST_USR_EBL_OPI,
			PH05.DF_LST_DTS_PH05 = L.DF_LST_DTS_PH05,
			PH05.DF_LST_USR_PH05 = L.DF_LST_USR_PH05,
			PH05.DD_CNC_EML_ADR_VER = L.DD_CNC_EML_ADR_VER,
			PH05.DF_LST_DTS_EML_VER = L.DF_LST_DTS_EML_VER,
			PH05.DC_LST_EML_VER_SYS = L.DC_LST_EML_VER_SYS,
			PH05.DC_LST_EML_VER_SRC = L.DC_LST_EML_VER_SRC,
			PH05.DX_CNC_EML_ADR_UC = L.DX_CNC_EML_ADR_UC,
			PH05.DF_LST_USR_EML_VER = L.DF_LST_USR_EML_VER,
			PH05.DI_CNC_TAX_OPI = L.DI_CNC_TAX_OPI,
			PH05.DC_TAX_OPI_APL_SRC = L.DC_TAX_OPI_APL_SRC,
			PH05.DC_TAX_OPI_SRC = L.DC_TAX_OPI_SRC,
			PH05.DF_DTS_TAX_OPI_EFF = L.DF_DTS_TAX_OPI_EFF,
			PH05.DF_LST_USR_TAX_OPI = L.DF_LST_USR_TAX_OPI,
			PH05.DF_ACP_TRM_VRS_ID = L.DF_ACP_TRM_VRS_ID,
			PH05.DF_DTS_ACP_TRM_EFF = L.DF_DTS_ACP_TRM_EFF,
			PH05.DF_LST_USR_ACP_TRM = L.DF_LST_USR_ACP_TRM
	WHEN NOT MATCHED THEN
		INSERT 
		(
			DF_CNC_SYS_ID,
			DF_SPE_ID,
			DX_CNC_EML_ADR,
			DC_EML_APL_SYS_SRC,
			DC_CNC_EML_DAT_SRC,
			DF_DTS_EML_ADR_EFF,
			DF_LST_USR_EML_ADR,
			DI_VLD_CNC_EML_ADR,
			DC_VLD_EML_APL_SRC,
			DC_VLD_CNC_EML_SRC,
			DF_DTS_VLD_EML_EFF,
			DF_LST_USR_VLD_EML,
			DI_CNC_ELT_OPI,
			DC_ELT_OPI_APL_SRC,
			DC_ELT_OPI_SRC,
			DF_DTS_ELT_OPI_EFF,
			DF_LST_USR_ELT_OPI,
			DI_CNC_EBL_OPI,
			DC_EBL_OPI_APL_SRC,
			DC_EBL_OPI_SRC,
			DF_DTS_EBL_OPI_EFF,
			DF_LST_USR_EBL_OPI,
			DF_LST_DTS_PH05,
			DF_LST_USR_PH05,
			DD_CNC_EML_ADR_VER,
			DF_LST_DTS_EML_VER,
			DC_LST_EML_VER_SYS,
			DC_LST_EML_VER_SRC,
			DX_CNC_EML_ADR_UC,
			DF_LST_USR_EML_VER,
			DI_CNC_TAX_OPI,
			DC_TAX_OPI_APL_SRC,
			DC_TAX_OPI_SRC,
			DF_DTS_TAX_OPI_EFF,
			DF_LST_USR_TAX_OPI,
			DF_ACP_TRM_VRS_ID,
			DF_DTS_ACP_TRM_EFF,
			DF_LST_USR_ACP_TRM
		)
		VALUES 
		(
			L.DF_CNC_SYS_ID,
			L.DF_SPE_ID,
			L.DX_CNC_EML_ADR,
			L.DC_EML_APL_SYS_SRC,
			L.DC_CNC_EML_DAT_SRC,
			L.DF_DTS_EML_ADR_EFF,
			L.DF_LST_USR_EML_ADR,
			L.DI_VLD_CNC_EML_ADR,
			L.DC_VLD_EML_APL_SRC,
			L.DC_VLD_CNC_EML_SRC,
			L.DF_DTS_VLD_EML_EFF,
			L.DF_LST_USR_VLD_EML,
			L.DI_CNC_ELT_OPI,
			L.DC_ELT_OPI_APL_SRC,
			L.DC_ELT_OPI_SRC,
			L.DF_DTS_ELT_OPI_EFF,
			L.DF_LST_USR_ELT_OPI,
			L.DI_CNC_EBL_OPI,
			L.DC_EBL_OPI_APL_SRC,
			L.DC_EBL_OPI_SRC,
			L.DF_DTS_EBL_OPI_EFF,
			L.DF_LST_USR_EBL_OPI,
			L.DF_LST_DTS_PH05,
			L.DF_LST_USR_PH05,
			L.DD_CNC_EML_ADR_VER,
			L.DF_LST_DTS_EML_VER,
			L.DC_LST_EML_VER_SYS,
			L.DC_LST_EML_VER_SRC,
			L.DX_CNC_EML_ADR_UC,
			L.DF_LST_USR_EML_VER,
			L.DI_CNC_TAX_OPI,
			L.DC_TAX_OPI_APL_SRC,
			L.DC_TAX_OPI_SRC,
			L.DF_DTS_TAX_OPI_EFF,
			L.DF_LST_USR_TAX_OPI,
			L.DF_ACP_TRM_VRS_ID,
			L.DF_DTS_ACP_TRM_EFF,
			L.DF_LST_USR_ACP_TRM
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
		LEGEND,
		'
			SELECT
				COUNT(*) AS "RemoteCount"
			FROM
				AES.PH05_CNC_EML
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			PH05_CNC_EML
	) L 
		ON 1 = 1
	

IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('AES.PH05_CNC_EML - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is requireL.', 16, 11, @CountDifference)
	END
ELSE IF @CountDifference != 0 AND @LoopCount = 0
	BEGIN

		SET @LoopCount = @LoopCount + 1
		
		DECLARE @SSN_LIST TABLE
		(
			R_BF_SSN CHAR(10),
			L_BF_SSN CHAR(10)
		)

		PRINT 'Insert SSN (DF_SPE_ID) with inconsistent counts'
		INSERT INTO
			@SSN_LIST
		SELECT TOP 20
			R.DF_SPE_ID,
			L.DF_SPE_ID
		FROM
			OPENQUERY
			(
				LEGEND,	
				'
					SELECT
						RIGHT(CONCAT(''0000000000'', CAST(PH05.DF_SPE_ID AS VARCHAR(10))), 10) AS "DF_SPE_ID",
						COUNT(*) AS "RemoteCount"
					FROM
						AES.PH05_CNC_EML PH05
					GROUP BY
						PH05.DF_SPE_ID
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					PH05.DF_SPE_ID,
					COUNT(*) [LocalCount]
				FROM
					CDW..PH05_CNC_EML PH05
				GROUP BY
					PH05.DF_SPE_ID
			) L 
				ON L.DF_SPE_ID = R.DF_SPE_ID
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

		PRINT 'The local record count for these SSNs (DF_SPE_ID) does not match the remote warehouse count: ' + @SSNs + '  Deleting all local PH05 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			PH05
		FROM
			CDW..PH05_CNC_EML PH05
			INNER JOIN @SSN_LIST SL 
				ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = PH05.DF_SPE_ID

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		GOTO RefreshStart;

	END