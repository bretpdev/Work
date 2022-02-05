USE UDW
GO

--SELECT TOP 1
--	*
--INTO 
--	dbo.PD42_PRS_PHN
--FROM
--	OPENQUERY
--	(
--		DUSTER,
--		'
--			SELECT
--				*
--			FROM
--				OLWHRM1.PD42_PRS_PHN PD42
--		'
--	) 

DECLARE 
	@SSNs VARCHAR(MAX) = '''''0''''', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(MAX(PD42.DF_LST_DTS_PD42), '1-1-1900 00:00:00'), 21) FROM PD42_PRS_PHN PD42)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.PD42_PRS_PHN PD42
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
							PD42.*
						FROM
							OLWHRM1.PD42_PRS_PHN PD42
						-- comment WHERE clause for full table refresh
						WHERE
							PD42.DF_LST_DTS_PD42 > ''''' + @LastRefresh + '''''
							OR
							(
								PD42.DF_PRS_ID IN
								(
									' + @SSNs + '
								)
							)
					''
				) 
		) D ON D.DF_PRS_ID = PD42.DF_PRS_ID AND D.DC_PHN = PD42.DC_PHN
	WHEN MATCHED THEN 
		UPDATE SET
			PD42.DT_PHN_BST_CL = D.DT_PHN_BST_CL,
			PD42.DD_PHN_VER = D.DD_PHN_VER,
			PD42.DI_PHN_VLD = D.DI_PHN_VLD,
			PD42.DN_PHN_XTN = D.DN_PHN_XTN,
			PD42.DN_DOM_PHN_LCL = D.DN_DOM_PHN_LCL,
			PD42.DN_DOM_PHN_XCH = D.DN_DOM_PHN_XCH,
			PD42.DN_DOM_PHN_ARA = D.DN_DOM_PHN_ARA,
			PD42.DI_PHN_WTS = D.DI_PHN_WTS,
			PD42.DX_PHN_TME_ZNE = D.DX_PHN_TME_ZNE,
			PD42.DF_LST_USR_PD42 = D.DF_LST_USR_PD42,
			PD42.DN_FGN_PHN_INL = D.DN_FGN_PHN_INL,
			PD42.DN_FGN_PHN_CNY = D.DN_FGN_PHN_CNY,
			PD42.DN_FGN_PHN_CT = D.DN_FGN_PHN_CT,
			PD42.DN_FGN_PHN_LCL = D.DN_FGN_PHN_LCL,
			PD42.DF_LST_DTS_PD42 = D.DF_LST_DTS_PD42,
			PD42.DD_CRT_PD42 = D.DD_CRT_PD42,
			PD42.DC_NO_HME_PHN = D.DC_NO_HME_PHN,
			PD42.DD_NO_HME_PHN = D.DD_NO_HME_PHN,
			PD42.DC_PHN_SRC = D.DC_PHN_SRC,
			PD42.DD_ORG_DB_CRT_PD40 = D.DD_ORG_DB_CRT_PD40,
			PD42.DI_HST_OLY_PD40 = D.DI_HST_OLY_PD40,
			PD42.DC_ALW_ADL_PHN = D.DC_ALW_ADL_PHN
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
			DF_LST_USR_PD42,
			DN_FGN_PHN_INL,
			DN_FGN_PHN_CNY,
			DN_FGN_PHN_CT,
			DN_FGN_PHN_LCL,
			DF_LST_DTS_PD42,
			DD_CRT_PD42,
			DC_NO_HME_PHN,
			DD_NO_HME_PHN,
			DC_PHN_SRC,
			DD_ORG_DB_CRT_PD40,
			DI_HST_OLY_PD40,
			DC_ALW_ADL_PHN
		)
		VALUES 
		(
			D.DF_PRS_ID,
			D.DC_PHN,
			D.DT_PHN_BST_CL,
			D.DD_PHN_VER,
			D.DI_PHN_VLD,
			D.DN_PHN_XTN,
			D.DN_DOM_PHN_LCL,
			D.DN_DOM_PHN_XCH,
			D.DN_DOM_PHN_ARA,
			D.DI_PHN_WTS,
			D.DX_PHN_TME_ZNE,
			D.DF_LST_USR_PD42,
			D.DN_FGN_PHN_INL,
			D.DN_FGN_PHN_CNY,
			D.DN_FGN_PHN_CT,
			D.DN_FGN_PHN_LCL,
			D.DF_LST_DTS_PD42,
			D.DD_CRT_PD42,
			D.DC_NO_HME_PHN,
			D.DD_NO_HME_PHN,
			D.DC_PHN_SRC,
			D.DD_ORG_DB_CRT_PD40,
			D.DI_HST_OLY_PD40,
			D.DC_ALW_ADL_PHN
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
				OLWHRM1.PD42_PRS_PHN PD42
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			UDW..PD42_PRS_PHN PD42
	) L ON 1 = 1
	
IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('PD42_PRS_PHN - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
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
				DUSTER,	
				'
					SELECT
						PD42.DF_PRS_ID,
						COUNT(*) AS "RemoteCount"
					FROM
						OLWHRM1.PD42_PRS_PHN PD42
					GROUP BY
						PD42.DF_PRS_ID
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					PD42.DF_PRS_ID,
					COUNT(*) [LocalCount]
				FROM
					UDW..PD42_PRS_PHN PD42
				GROUP BY
					PD42.DF_PRS_ID
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

		PRINT 'The local record count for these SSNs does not match the remote warehouse count: ' + @SSNs + '  Deleting all local PD42 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			PD42
		FROM
			UDW..PD42_PRS_PHN PD42
			INNER JOIN @SSN_LIST SL ON COALESCE(SL.L_DF_PRS_ID, SL.R_DF_PRS_ID) = PD42.DF_PRS_ID

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		GOTO RefreshStart;

	END
