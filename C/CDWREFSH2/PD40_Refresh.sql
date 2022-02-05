USE CDW
GO

--SELECT TOP 1
--	*
--INTO 
--	dbo.PD40_PRS_PHN
--FROM
--	OPENQUERY
--	(
--		LEGEND,
--		'
--			SELECT
--				*
--			FROM
--				PKUB.PD40_PRS_PHN PD40
--		'
--	) 

DECLARE 
	@SSNs VARCHAR(MAX) = '''''0''''', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(MAX(PD40.DF_LST_DST_PD40), '1-1-1900 00:00:00'), 21) FROM PD40_PRS_PHN PD40)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.PD40_PRS_PHN PD40
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
							PD40.*
						FROM
							PKUB.PD40_PRS_PHN PD40
						-- comment WHERE clause for full table refresh
						WHERE
							PD40.DF_LST_DST_PD40 > ''''' + @LastRefresh + '''''
							OR
							(
								PD40.DF_PRS_ID IN
								(
									' + @SSNs + '
								)
							)
					''
				) 
		) L ON L.DF_PRS_ID = PD40.DF_PRS_ID AND L.DC_PHN = PD40.DC_PHN
	WHEN MATCHED THEN 
		UPDATE SET
			PD40.DT_PHN_BST_CL = L.DT_PHN_BST_CL,
			PD40.DD_PHN_VER = L.DD_PHN_VER,
			PD40.DI_PHN_VLD = L.DI_PHN_VLD,
			PD40.DN_PHN_XTN = L.DN_PHN_XTN,
			PD40.DN_DOM_PHN_LCL = L.DN_DOM_PHN_LCL,
			PD40.DN_DOM_PHN_XCH = L.DN_DOM_PHN_XCH,
			PD40.DN_DOM_PHN_ARA = L.DN_DOM_PHN_ARA,
			PD40.DI_PHN_WTS = L.DI_PHN_WTS,
			PD40.DX_PHN_TME_ZNE = L.DX_PHN_TME_ZNE,
			PD40.DF_LST_USR_PD40 = L.DF_LST_USR_PD40,
			PD40.DN_FGN_PHN_INL = L.DN_FGN_PHN_INL,
			PD40.DN_FGN_PHN_CNY = L.DN_FGN_PHN_CNY,
			PD40.DN_FGN_PHN_CT = L.DN_FGN_PHN_CT,
			PD40.DN_FGN_PHN_LCL = L.DN_FGN_PHN_LCL,
			PD40.DF_LST_DTS_PD40 = L.DF_LST_DTS_PD40,
			PD40.DD_CRT_PD40 = L.DD_CRT_PD40,
			PD40.DC_NO_HME_PHN = L.DC_NO_HME_PHN,
			PD40.DD_NO_HME_PHN = L.DD_NO_HME_PHN,
			PD40.DC_PHN_SRC = L.DC_PHN_SRC,
			PD40.DD_ORG_DB_CRT_PD40 = L.DD_ORG_DB_CRT_PD40,
			PD40.DI_HST_OLY_PD40 = L.DI_HST_OLY_PD40,
			PD40.DC_ALW_ADL_PHN = L.DC_ALW_ADL_PHN
	WHEN NOT MATCHED THEN
		INSERT 
		(
			DF_PRS_ID,
			DC_PHN,
			DT_PHN_BST_CL,
			DD_PHN_VER,
			DI_PHN_VLD,
			DN_PHN_XTN,
			DN_DOM_PHN_LCL,
			DN_DOM_PHN_XCH,
			DN_DOM_PHN_ARA,
			DI_PHN_WTS,
			DX_PHN_TME_ZNE,
			DF_LST_USR_PD40,
			DN_FGN_PHN_INL,
			DN_FGN_PHN_CNY,
			DN_FGN_PHN_CT,
			DN_FGN_PHN_LCL,
			DF_LST_DTS_PD40,
			DD_CRT_PD40,
			DC_NO_HME_PHN,
			DD_NO_HME_PHN,
			DC_PHN_SRC,
			DD_ORG_DB_CRT_PD40,
			DI_HST_OLY_PD40,
			DC_ALW_ADL_PHN
		)
		VALUES 
		(
			L.DF_PRS_ID,
			L.DC_PHN,
			L.DT_PHN_BST_CL,
			L.DD_PHN_VER,
			L.DI_PHN_VLD,
			L.DN_PHN_XTN,
			L.DN_DOM_PHN_LCL,
			L.DN_DOM_PHN_XCH,
			L.DN_DOM_PHN_ARA,
			L.DI_PHN_WTS,
			L.DX_PHN_TME_ZNE,
			L.DF_LST_USR_PD40,
			L.DN_FGN_PHN_INL,
			L.DN_FGN_PHN_CNY,
			L.DN_FGN_PHN_CT,
			L.DN_FGN_PHN_LCL,
			L.DF_LST_DTS_PD40,
			L.DD_CRT_PD40,
			L.DC_NO_HME_PHN,
			L.DD_NO_HME_PHN,
			L.DC_PHN_SRC,
			L.DD_ORG_DB_CRT_PD40,
			L.DI_HST_OLY_PD40,
			L.DC_ALW_ADL_PHN
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
				PKUB.PD40_PRS_PHN PD40
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			CDW..PD40_PRS_PHN PD40
	) L ON 1 = 1
	
IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('PD40_PRS_PHN - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is requireL.', 16, 11, @CountDifference)
	END
ELSE IF @CountDifference != 0 AND @LoopCount = 0
	BEGIN

		SET @LoopCount = @LoopCount + 1
		
		DECLARE @SSN_LIST TABLE
		(
			R_DF_PRS_ID CHAR(9),
			L_DF_PRS_ID CHAR(9)
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
						PD40.DF_PRS_ID,
						COUNT(*) AS "RemoteCount"
					FROM
						PKUB.PD40_PRS_PHN PD40
					GROUP BY
						PD40.DF_PRS_ID
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					PD40.DF_PRS_ID,
					COUNT(*) [LocalCount]
				FROM
					CDW..PD40_PRS_PHN PD40
				GROUP BY
					PD40.DF_PRS_ID
			) L ON L.DF_PRS_ID = R.DF_PRS_ID
		WHERE
			ISNULL(L.LocalCount, 0) != ISNULL(R.RemoteCount, 0)

		SELECT
			@SSNs = 
			(
				SELECT
					'''''' + COALESCE(SL.L_DF_PRS_ID, SL.R_DF_PRS_ID) + ''''',' AS [text()]
				FROM
					@SSN_LIST SL
				ORDER BY
					COALESCE(SL.L_DF_PRS_ID, SL.R_DF_PRS_ID)
				FOR XML PATH ('')
			)

		SELECT	@SSNs = LEFT(@SSNs, LEN(@SSNs) -1)

		PRINT 'The local record count for these SSNs does not match the remote warehouse count: ' + @SSNs + '  Deleting all local PD40 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			PD40
		FROM
			CDW..PD40_PRS_PHN PD40
			INNER JOIN @SSN_LIST SL ON COALESCE(SL.L_DF_PRS_ID, SL.R_DF_PRS_ID) = PD40.DF_PRS_ID

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		GOTO RefreshStart;

	END
