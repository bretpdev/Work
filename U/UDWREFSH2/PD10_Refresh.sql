USE UDW
GO

--SELECT TOP 1
--	*
--INTO
--	PD10_PRS_NME
--FROM
--	OPENQUERY
--	(
--		DUSTER,
--		'
--			SELECT
--				*
--			FROM
--				OLWHRM1.PD10_PRS_NME PD10
--		'
--	) 

DECLARE 
	@SSNs VARCHAR(MAX) = '''''0''''', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(MAX(PD10.DF_LST_DTS_PD10), '1-1-1900 00:00:00'), 21) FROM PD10_PRS_NME PD10)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
	'
		MERGE 
			dbo.PD10_PRS_NME PD10
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
								*
							FROM
								OLWHRM1.PD10_PRS_NME PD10
							-- comment WHERE clause for full table refresh
							WHERE
								PD10.DF_LST_DTS_PD10 > ''''' + @LastRefresh + '''''
								OR
								(
									PD10.DF_PRS_ID IN
									(
										' + @SSNs + '
									)
								)
						''
					) 
			) L ON L.DF_PRS_ID = PD10.DF_PRS_ID
		WHEN MATCHED THEN 
			UPDATE SET 
				PD10.DD_STA_PRS = L.DD_STA_PRS,
				PD10.DC_LAG_FGN = L.DC_LAG_FGN,
				PD10.DC_SEX = L.DC_SEX,
				PD10.DD_BRT = L.DD_BRT,
				PD10.DM_PRS_MID = L.DM_PRS_MID,
				PD10.DM_PRS_1 = L.DM_PRS_1,
				PD10.DM_PRS_LST_SFX = L.DM_PRS_LST_SFX,
				PD10.DM_PRS_LST = L.DM_PRS_LST,
				PD10.DD_DRV_LIC_REN = L.DD_DRV_LIC_REN,
				PD10.DC_ST_DRV_LIC = L.DC_ST_DRV_LIC,
				PD10.DF_DRV_LIC = L.DF_DRV_LIC,
				PD10.DD_NME_VER_LST = L.DD_NME_VER_LST,
				PD10.DI_ORG_HLD = L.DI_ORG_HLD,
				PD10.DF_LST_USR_PD10 = L.DF_LST_USR_PD10,
				PD10.DF_ALN_RGS = L.DF_ALN_RGS,
				PD10.DI_US_CTZ = L.DI_US_CTZ,
				PD10.DF_LST_DTS_PD10 = L.DF_LST_DTS_PD10,
				PD10.DF_SPE_ACC_ID = L.DF_SPE_ACC_ID,
				PD10.DF_PRS_LST_4_SSN = L.DF_PRS_LST_4_SSN,
				PD10.DI_ATU_FMT = L.DI_ATU_FMT,
				PD10.DC_ATU_FMT_TYP= L.DC_ATU_FMT_TYP
		WHEN NOT MATCHED THEN
			INSERT 
			(
				DF_PRS_ID, 
				DC_LAG_FGN,
				DC_SEX,
				DD_BRT,
				DM_PRS_MID,
				DM_PRS_1,
				DM_PRS_LST_SFX,
				DM_PRS_LST,
				DD_DRV_LIC_REN,
				DC_ST_DRV_LIC,
				DF_DRV_LIC,
				DD_NME_VER_LST,
				DI_ORG_HLD,
				DF_LST_USR_PD10,
				DF_ALN_RGS,
				DI_US_CTZ,
				DF_LST_DTS_PD10,
				DF_SPE_ACC_ID,
				DF_PRS_LST_4_SSN,
				DI_ATU_FMT,
				DC_ATU_FMT_TYP
			)
			VALUES 
			(
				L.DF_PRS_ID,
				L.DC_LAG_FGN,
				L.DC_SEX,
				L.DD_BRT,
				L.DM_PRS_MID,
				L.DM_PRS_1,
				L.DM_PRS_LST_SFX,
				L.DM_PRS_LST,
				L.DD_DRV_LIC_REN,
				L.DC_ST_DRV_LIC,
				L.DF_DRV_LIC,
				L.DD_NME_VER_LST,
				L.DI_ORG_HLD,
				L.DF_LST_USR_PD10,
				L.DF_ALN_RGS,
				L.DI_US_CTZ,
				L.DF_LST_DTS_PD10,
				L.DF_SPE_ACC_ID,
				L.DF_PRS_LST_4_SSN,
				L.DI_ATU_FMT,
				L.DC_ATU_FMT_TYP
			)
		-- !!! uncomment lines below ONLY when doing a full table refresh 
		--WHEN NOT MATCHED BY SOURCE THEN
		--	DELETE
		;
	'
PRINT @SQLStatement
EXEC (@SQLStatement)


-- ###### VALIDATION
-- the DF_LST_DTS_PD10 date is unreliable when a person's SSN has been changed.
-- sum all SSNs in order to dentify missing/change/added SSNs
DECLARE 
	@SumDifference INT

SELECT
	@SumDifference = L.LocalSum - R.RemoteSum
FROM
	OPENQUERY
	(
		DUSTER,
		'
			SELECT
				SUM(CAST(DF_PRS_ID AS BIGINT)) AS "RemoteSum"
			FROM
				OLWHRM1.PD10_PRS_NME PD10
			WHERE
				PD10.DF_PRS_ID NOT LIKE ''P%''
		'
	) R
	FULL OUTER JOIN
	(
		SELECT
			SUM(CAST(DF_PRS_ID AS BIGINT)) [LocalSum]
		FROM
			UDW..PD10_PRS_NME PD10
		WHERE
			PD10.DF_PRS_ID NOT LIKE 'P%'
	) L ON 1 = 1


IF @SumDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('PD10_PRS_NME - The remote and local SSN sums do not match.  A full refresh of the table may be required.', 16, 11, @SumDifference)
	END
ELSE IF @SumDifference != 0 AND @LoopCount = 0
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
			R.BF_SSN,
			L.BF_SSN
		FROM
			OPENQUERY
			(
				DUSTER,	
				'
					SELECT
						PD10.DF_PRS_ID AS "BF_SSN",
						COUNT(*) AS "RemoteCount"
					FROM
						OLWHRM1.PD10_PRS_NME PD10
					GROUP BY
						PD10.DF_PRS_ID
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					PD10.DF_PRS_ID [BF_SSN],
					COUNT(*) [LocalCount]
				FROM
					UDW..PD10_PRS_NME PD10
				GROUP BY
					PD10.DF_PRS_ID
			) L ON L.BF_SSN = R.BF_SSN
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

		PRINT 'The local record count for these SSNs does not match the remote warehouse count.  Deleting all local PD10 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			PD10
		FROM
			UDW..PD10_PRS_NME PD10
			INNER JOIN @SSN_LIST SL ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = PD10.DF_PRS_ID

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		GOTO RefreshStart;

	END
