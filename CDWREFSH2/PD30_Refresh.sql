USE CDW
GO

--SELECT
--	*
--INTO 
--	dbo.PD30_PRS_ADR
--FROM
--	OPENQUERY
--	(
--		LEGEND,
--		'
--			SELECT
--				*
--			FROM
--				PKUB.PD30_PRS_ADR
--		'
--	) 

DECLARE 
	@SSNs VARCHAR(MAX) = '''''0''''', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(MAX(PD30.DF_LST_DTS_PD30), '1-1-1900 00:00:00'), 21) FROM PD30_PRS_ADR PD30)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.PD30_PRS_ADR PD30
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
							*
						FROM
							PKUB.PD30_PRS_ADR PD30
						-- comment WHERE clause for full table refresh
						WHERE
							PD30.DF_LST_DTS_PD30 > ''''' + @LastRefresh + '''''
					''
				) 
		) L ON L.DF_PRS_ID = PD30.DF_PRS_ID AND L.DC_ADR = PD30.DC_ADR 
	WHEN MATCHED THEN 
		UPDATE SET 
			PD30.DD_STA_PDEM30 = L.DD_STA_PDEM30,
			PD30.DD_VER_ADR = L.DD_VER_ADR,
			PD30.DI_VLD_ADR = L.DI_VLD_ADR,
			PD30.DF_ZIP_CDE = L.DF_ZIP_CDE,
			PD30.DM_CT = L.DM_CT,
			PD30.DX_STR_ADR_3 = L.DX_STR_ADR_3,
			PD30.DX_STR_ADR_2 = L.DX_STR_ADR_2,
			PD30.DX_STR_ADR_1 = L.DX_STR_ADR_1,
			PD30.DC_DOM_ST = L.DC_DOM_ST,
			PD30.DF_LST_USR_PD30 = L.DF_LST_USR_PD30,
			PD30.DF_3PT_ADR = L.DF_3PT_ADR,
			PD30.DC_3PT_ADR = L.DC_3PT_ADR,
			PD30.DC_SRC_ADR = L.DC_SRC_ADR,
			PD30.DM_FGN_CNY = L.DM_FGN_CNY,
			PD30.DM_FGN_ST = L.DM_FGN_ST,
			PD30.DF_LST_DTS_PD30 = L.DF_LST_DTS_PD30,
			PD30.DX_DLV_PTR_BCD = L.DX_DLV_PTR_BCD,
			PD30.DC_FGN_CNY = L.DC_FGN_CNY,
			PD30.DD_DSB_ADR_BEG = L.DD_DSB_ADR_BEG,
			PD30.DD_DSB_ADR_END = L.DD_DSB_ADR_END
	WHEN NOT MATCHED THEN
		INSERT 
		(
			DF_PRS_ID,
			DC_ADR,
			DD_STA_PDEM30,
			DD_VER_ADR,
			DI_VLD_ADR,
			DF_ZIP_CDE,
			DM_CT,
			DX_STR_ADR_3,
			DX_STR_ADR_2,
			DX_STR_ADR_1,
			DC_DOM_ST,
			DF_LST_USR_PD30,
			DF_3PT_ADR,
			DC_3PT_ADR,
			DC_SRC_ADR,
			DM_FGN_CNY,
			DM_FGN_ST,
			DF_LST_DTS_PD30,
			DX_DLV_PTR_BCD,
			DC_FGN_CNY,
			DD_DSB_ADR_BEG,
			DD_DSB_ADR_END
		)
		VALUES 
		(
			L.DF_PRS_ID,
			L.DC_ADR,
			L.DD_STA_PDEM30,
			L.DD_VER_ADR,
			L.DI_VLD_ADR,
			L.DF_ZIP_CDE,
			L.DM_CT,
			L.DX_STR_ADR_3,
			L.DX_STR_ADR_2,
			L.DX_STR_ADR_1,
			L.DC_DOM_ST,
			L.DF_LST_USR_PD30,
			L.DF_3PT_ADR,
			L.DC_3PT_ADR,
			L.DC_SRC_ADR,
			L.DM_FGN_CNY,
			L.DM_FGN_ST,
			L.DF_LST_DTS_PD30,
			L.DX_DLV_PTR_BCD,
			L.DC_FGN_CNY,
			L.DD_DSB_ADR_BEG,
			L.DD_DSB_ADR_END
		)
	-- !!! uncomment lines below ONLY when doing a full table refresh 
	--WHEN NOT MATCHED BY SOURCE THEN
	    --DELETE
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
				PKUB.PD30_PRS_ADR
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			PD30_PRS_ADR
	) L ON 1 = 1
	
IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('PD30_PRS_ADR - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
	END
ELSE IF @CountDifference != 0 AND @LoopCount = 0
	BEGIN

		SET @LoopCount = @LoopCount + 1
		
		DECLARE @SSN_LIST TABLE
		(
			R_BF_SSN CHAR(9),
			L_BF_SSN CHAR(9)
		)

		PRINT 'Insert SSN with inconsistent counts'
		INSERT INTO
			@SSN_LIST
		SELECT TOP 20
			R.DF_PRS_ID,
			L.DF_PRS_ID
		FROM
			OPENQUERY
			(
				LEGEND,	
				'
					SELECT
						PD30.DF_PRS_ID,
						COUNT(*) AS "RemoteCount"
					FROM
						PKUB.PD30_PRS_ADR PD30
					GROUP BY
						PD30.DF_PRS_ID
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					PD30.DF_PRS_ID,
					COUNT(*) [LocalCount]
				FROM
					CDW..PD30_PRS_ADR PD30
				GROUP BY
					PD30.DF_PRS_ID
			) L ON L.DF_PRS_ID = R.DF_PRS_ID
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

		PRINT 'The local record count for these SSNs does not match the remote warehouse count: ' + @SSNs + '  Deleting all local PD30 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			PD30
		FROM
			CDW..PD30_PRS_ADR PD30
			INNER JOIN @SSN_LIST SL ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = PD30.DF_PRS_ID

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		GOTO RefreshStart;

	END