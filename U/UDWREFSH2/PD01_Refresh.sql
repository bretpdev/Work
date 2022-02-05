USE UDW
GO

--SELECT TOP 1
--	*
--INTO
--	PD01_PDM_INF
--FROM
--	OPENQUERY
--	(
--		DUSTER,
--		'
--			SELECT
--				*
--			FROM
--				OLWHRM1.PD01_PDM_INF PD01
--		'
--	) 

DECLARE 
	@SSNs VARCHAR(MAX) = '''''0''''', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(MAX(PD01.DF_LST_DTS_PD01), '1-1-1900 00:00:00'), 21) FROM PD01_PDM_INF PD01)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
	'

	MERGE 
		dbo.PD01_PDM_INF LOCAL
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
							REMOTE.*
						FROM
							OLWHRM1.PD01_PDM_INF REMOTE
						WHERE
							REMOTE.DF_LST_DTS_PD01 > ''''' + @LastRefresh + '''''
							OR
							(
								REMOTE.DF_PRS_ID IN
								(
									' + @SSNs + '
								)
							)
					''
				) 
			) R ON R.DF_PRS_ID = LOCAL.DF_PRS_ID
			WHEN MATCHED THEN 
				UPDATE SET 
				LOCAL.DD_BRT = R.DD_BRT,				LOCAL.DM_PRS_MID = R.DM_PRS_MID,				LOCAL.DM_PRS_1 = R.DM_PRS_1,				LOCAL.DM_PRS_LST = R.DM_PRS_LST,				LOCAL.DF_DRV_LIC = R.DF_DRV_LIC,				LOCAL.DF_ALN_RGS = R.DF_ALN_RGS,				LOCAL.DF_LST_DTS_PD01 = R.DF_LST_DTS_PD01,				LOCAL.DC_PRS_HLD_REA_1 = R.DC_PRS_HLD_REA_1,				LOCAL.DC_PRS_HLD_REA_2 = R.DC_PRS_HLD_REA_2,				LOCAL.DC_PRS_HLD_REA_3 = R.DC_PRS_HLD_REA_3,				LOCAL.DC_PRS_HLD_REA_4 = R.DC_PRS_HLD_REA_4,				LOCAL.DC_PRS_HLD_REA_5 = R.DC_PRS_HLD_REA_5,				LOCAL.DX_SRC = R.DX_SRC,				LOCAL.DC_ADR = R.DC_ADR,				LOCAL.DI_VLD_ADR = R.DI_VLD_ADR,				LOCAL.DF_ZIP = R.DF_ZIP,				LOCAL.DM_CT = R.DM_CT,				LOCAL.DX_STR_ADR_1 = R.DX_STR_ADR_1,				LOCAL.DX_STR_ADR_2 = R.DX_STR_ADR_2,				LOCAL.DC_DOM_ST = R.DC_DOM_ST,				LOCAL.DM_FGN_CNY = R.DM_FGN_CNY,				LOCAL.DI_PHN_VLD = R.DI_PHN_VLD,				LOCAL.DN_PHN = R.DN_PHN,				LOCAL.DN_ALT_PHN = R.DN_ALT_PHN,				LOCAL.DN_PHN_XTN = R.DN_PHN_XTN,				LOCAL.DN_ALT_PHN_XTN = R.DN_ALT_PHN_XTN,				LOCAL.DF_LST_DTS_PD03 = R.DF_LST_DTS_PD03,				LOCAL.DC_SKP_TRC_STA = R.DC_SKP_TRC_STA,				LOCAL.DD_SKP_TRC_EFF = R.DD_SKP_TRC_EFF,				LOCAL.DX_UMP_ADR = R.DX_UMP_ADR,				LOCAL.BD_UMP_ADR_UPD = R.BD_UMP_ADR_UPD,				LOCAL.BF_EMP_ID_1 = R.BF_EMP_ID_1,				LOCAL.BN_WGE_YR_1 = R.BN_WGE_YR_1,				LOCAL.BN_WGE_QTR_1 = R.BN_WGE_QTR_1,				LOCAL.BA_WGE_RPT_1 = R.BA_WGE_RPT_1,				LOCAL.BF_EMP_ID_2 = R.BF_EMP_ID_2,				LOCAL.BN_WGE_YR_2 = R.BN_WGE_YR_2,				LOCAL.BN_WGE_QTR_2 = R.BN_WGE_QTR_2,				LOCAL.BA_WGE_RPT_2 = R.BA_WGE_RPT_2,				LOCAL.BF_EMP_ID_3 = R.BF_EMP_ID_3,				LOCAL.BN_WGE_YR_3 = R.BN_WGE_YR_3,				LOCAL.BN_WGE_QTR_3 = R.BN_WGE_QTR_3,				LOCAL.BA_WGE_RPT_3 = R.BA_WGE_RPT_3,				LOCAL.BF_EMP_ID_4 = R.BF_EMP_ID_4,				LOCAL.BN_WGE_YR_4 = R.BN_WGE_YR_4,				LOCAL.BN_WGE_QTR_4 = R.BN_WGE_QTR_4,				LOCAL.BA_WGE_RPT_4 = R.BA_WGE_RPT_4,				LOCAL.BF_LST_DTS_BR02 = R.BF_LST_DTS_BR02,				LOCAL.DI_ALT_PHN_VLD = R.DI_ALT_PHN_VLD,				LOCAL.DC_ST_DRV_LIC = R.DC_ST_DRV_LIC,				LOCAL.DD_ENT_ITV_NTF = R.DD_ENT_ITV_NTF,				LOCAL.DX_EML_ADR = R.DX_EML_ADR,				LOCAL.DD_LST_UPD_ADR = R.DD_LST_UPD_ADR,				LOCAL.DD_DTH = R.DD_DTH,				LOCAL.BI_EMP_INF_OVR = R.BI_EMP_INF_OVR,				LOCAL.DF_USR_UPD_ADR = R.DF_USR_UPD_ADR,				LOCAL.DF_USR_UPD_PHN = R.DF_USR_UPD_PHN,				LOCAL.DD_LST_UPD_PHN = R.DD_LST_UPD_PHN,				LOCAL.DD_ADR_EFF = R.DD_ADR_EFF,				LOCAL.DD_PHN_EFF = R.DD_PHN_EFF,				LOCAL.DF_SPE_ACC_ID = R.DF_SPE_ACC_ID,				LOCAL.DD_PMN_DSA = R.DD_PMN_DSA
			WHEN NOT MATCHED THEN
				INSERT 
				(
					DF_PRS_ID,					DD_BRT,					DM_PRS_MID,					DM_PRS_1,					DM_PRS_LST,					DF_DRV_LIC,					DF_ALN_RGS,					DF_LST_DTS_PD01,					DC_PRS_HLD_REA_1,					DC_PRS_HLD_REA_2,					DC_PRS_HLD_REA_3,					DC_PRS_HLD_REA_4,					DC_PRS_HLD_REA_5,					DX_SRC,					DC_ADR,					DI_VLD_ADR,					DF_ZIP,					DM_CT,					DX_STR_ADR_1,					DX_STR_ADR_2,					DC_DOM_ST,					DM_FGN_CNY,					DI_PHN_VLD,					DN_PHN,					DN_ALT_PHN,					DN_PHN_XTN,					DN_ALT_PHN_XTN,					DF_LST_DTS_PD03,					DC_SKP_TRC_STA,					DD_SKP_TRC_EFF,					DX_UMP_ADR,					BD_UMP_ADR_UPD,					BF_EMP_ID_1,					BN_WGE_YR_1,					BN_WGE_QTR_1,					BA_WGE_RPT_1,					BF_EMP_ID_2,					BN_WGE_YR_2,					BN_WGE_QTR_2,					BA_WGE_RPT_2,					BF_EMP_ID_3,					BN_WGE_YR_3,					BN_WGE_QTR_3,					BA_WGE_RPT_3,					BF_EMP_ID_4,					BN_WGE_YR_4,					BN_WGE_QTR_4,					BA_WGE_RPT_4,					BF_LST_DTS_BR02,					DI_ALT_PHN_VLD,					DC_ST_DRV_LIC,					DD_ENT_ITV_NTF,					DX_EML_ADR,					DD_LST_UPD_ADR,					DD_DTH,					BI_EMP_INF_OVR,					DF_USR_UPD_ADR,					DF_USR_UPD_PHN,					DD_LST_UPD_PHN,					DD_ADR_EFF,					DD_PHN_EFF,					DF_SPE_ACC_ID,					DD_PMN_DSA
				)
				VALUES 
				(
				R.DF_PRS_ID,				R.DD_BRT,				R.DM_PRS_MID,				R.DM_PRS_1,				R.DM_PRS_LST,				R.DF_DRV_LIC,				R.DF_ALN_RGS,				R.DF_LST_DTS_PD01,				R.DC_PRS_HLD_REA_1,				R.DC_PRS_HLD_REA_2,				R.DC_PRS_HLD_REA_3,				R.DC_PRS_HLD_REA_4,				R.DC_PRS_HLD_REA_5,				R.DX_SRC,				R.DC_ADR,				R.DI_VLD_ADR,				R.DF_ZIP,				R.DM_CT,				R.DX_STR_ADR_1,				R.DX_STR_ADR_2,				R.DC_DOM_ST,				R.DM_FGN_CNY,				R.DI_PHN_VLD,				R.DN_PHN,				R.DN_ALT_PHN,				R.DN_PHN_XTN,				R.DN_ALT_PHN_XTN,				R.DF_LST_DTS_PD03,				R.DC_SKP_TRC_STA,				R.DD_SKP_TRC_EFF,				R.DX_UMP_ADR,				R.BD_UMP_ADR_UPD,				R.BF_EMP_ID_1,				R.BN_WGE_YR_1,				R.BN_WGE_QTR_1,				R.BA_WGE_RPT_1,				R.BF_EMP_ID_2,				R.BN_WGE_YR_2,				R.BN_WGE_QTR_2,				R.BA_WGE_RPT_2,				R.BF_EMP_ID_3,				R.BN_WGE_YR_3,				R.BN_WGE_QTR_3,				R.BA_WGE_RPT_3,				R.BF_EMP_ID_4,				R.BN_WGE_YR_4,				R.BN_WGE_QTR_4,				R.BA_WGE_RPT_4,				R.BF_LST_DTS_BR02,				R.DI_ALT_PHN_VLD,				R.DC_ST_DRV_LIC,				R.DD_ENT_ITV_NTF,				R.DX_EML_ADR,				R.DD_LST_UPD_ADR,				R.DD_DTH,				R.BI_EMP_INF_OVR,				R.DF_USR_UPD_ADR,				R.DF_USR_UPD_PHN,				R.DD_LST_UPD_PHN,				R.DD_ADR_EFF,				R.DD_PHN_EFF,				R.DF_SPE_ACC_ID,				R.DD_PMN_DSA
				)
		-- !!! uncomment lines below ONLY when doing a full table refresh 
		--WHEN NOT MATCHED BY SOURCE THEN
		--    DELETE
		;
	'
PRINT @SQLStatement
EXEC (@SQLStatement)


-- ###### VALIDATION
-- the DF_LST_DTS_PD01 date is unreliable when a person's SSN has been changed.
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
				OLWHRM1.PD01_PDM_INF PD01
			WHERE
				PD01.DF_PRS_ID NOT LIKE ''P%''
		'
	) R
	FULL OUTER JOIN
	(
		SELECT
			SUM(CAST(DF_PRS_ID AS BIGINT)) [LocalSum]
		FROM
			UDW..PD01_PDM_INF PD01
		WHERE
			PD01.DF_PRS_ID NOT LIKE 'P%'
	) L ON 1 = 1


IF @SumDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('PD01_PDM_INF - The remote and local SSN sums do not match.  A full refresh of the table may be required.', 16, 11, @SumDifference)
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
						PD01.DF_PRS_ID AS "BF_SSN",
						COUNT(*) AS "RemoteCount"
					FROM
						OLWHRM1.PD01_PDM_INF PD01
					GROUP BY
						PD01.DF_PRS_ID
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					PD01.DF_PRS_ID [BF_SSN],
					COUNT(*) [LocalCount]
				FROM
					UDW..PD01_PDM_INF PD01
				GROUP BY
					PD01.DF_PRS_ID
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

		PRINT 'The local record count for these SSNs does not match the remote warehouse count.  Deleting all local PD01 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			PD01
		FROM
			UDW..PD01_PDM_INF PD01
			INNER JOIN @SSN_LIST SL ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = PD01.DF_PRS_ID

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		GOTO RefreshStart;

	END
