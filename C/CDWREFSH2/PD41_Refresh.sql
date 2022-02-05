USE CDW
GO



DECLARE 
	@SSNs VARCHAR(MAX) = '''''o''''',
	@LoopCount TINYINT = 0

RefreshStart:



DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(DATEADD(HOUR,-3,MAX(PD41.DF_LST_DTS_PD41)), '1-1-1900 00:00:00'), 21) FROM PD41_PHN_HST PD41)
DECLARE @CreateDate VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(DATEADD(HOUR,-3,CAST(MAX(PD41.DD_CRT_41) AS DATETIME)), '1-1-1900 00:00:00'), 21) FROM PD41_PHN_HST PD41)
PRINT 'Last Refreshed timestamp at: ' + @LastRefresh
PRINT 'Record Create date: ' + @CreateDate


DECLARE @SQLStatement VARCHAR(MAX) =  
'
	MERGE 
			dbo.PD41_PHN_HST LOCAL
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
								REMOTE.*
							FROM
								PKUB.PD41_PHN_HST REMOTE
						WHERE
							REMOTE.DF_LST_DTS_PD41 > ''''' + @LastRefresh + '''''
							OR REMOTE.DD_CRT_41 >= ''''' + @CreateDate + '''''
							OR REMOTE.DF_PRS_ID IN
								(
									' + @SSNs + '
								)
							

					''
				) 
		) R 
			ON R.DC_PHN_HST = LOCAL.DC_PHN_HST 
			AND R.DF_PRS_ID = LOCAL.DF_PRS_ID 
			AND R.DN_PHN_SEQ = LOCAL.DN_PHN_SEQ
	WHEN MATCHED THEN 
		UPDATE SET 
			LOCAL.DT_PHN_BCL_HST = R.DT_PHN_BCL_HST,
			LOCAL.DI_PHN_VLD_HST = R.DI_PHN_VLD_HST,
			LOCAL.DD_PHN_VER_HST = R.DD_PHN_VER_HST,
			LOCAL.DN_PHN_XTN_HST = R.DN_PHN_XTN_HST,
			LOCAL.DN_DOM_PHN_LCL_HST = R.DN_DOM_PHN_LCL_HST,
			LOCAL.DN_DOM_PHN_XCH_HST = R.DN_DOM_PHN_XCH_HST,
			LOCAL.DN_DOM_PHN_ARA_HST = R.DN_DOM_PHN_ARA_HST,
			LOCAL.DI_PHN_WTS_HST = R.DI_PHN_WTS_HST,
			LOCAL.DX_PHN_TZ_HST = R.DX_PHN_TZ_HST,
			LOCAL.DF_LST_USR_PD41 = R.DF_LST_USR_PD41,
			LOCAL.DN_FGN_PHN_INL_HST = R.DN_FGN_PHN_INL_HST,
			LOCAL.DN_FGN_PHN_CNY_HST = R.DN_FGN_PHN_CNY_HST,
			LOCAL.DN_FGN_PHN_CT_HST = R.DN_FGN_PHN_CT_HST,
			LOCAL.DN_FGN_PHN_LCL_HST = R.DN_FGN_PHN_LCL_HST,
			LOCAL.DF_LST_DTS_PD41 = R.DF_LST_DTS_PD41,
			LOCAL.DD_CRT_41 = R.DD_CRT_41,
			LOCAL.DC_NO_HME_PHN_HST = R.DC_NO_HME_PHN_HST,
			LOCAL.DD_NO_HME_PHN_HST = R.DD_NO_HME_PHN_HST,
			LOCAL.DC_PHN_SRC_HST = R.DC_PHN_SRC_HST,
			LOCAL.DI_HST_OLY_PD41 = R.DI_HST_OLY_PD41,
			LOCAL.DC_ALW_ADL_PHN_HST = R.DC_ALW_ADL_PHN_HST
	WHEN NOT MATCHED THEN
		INSERT 
		(
			DF_PRS_ID,
			DC_PHN_HST,
			DN_PHN_SEQ,
			DT_PHN_BCL_HST,
			DI_PHN_VLD_HST,
			DD_PHN_VER_HST,
			DN_PHN_XTN_HST,
			DN_DOM_PHN_LCL_HST,
			DN_DOM_PHN_XCH_HST,
			DN_DOM_PHN_ARA_HST,
			DI_PHN_WTS_HST,
			DX_PHN_TZ_HST,
			DF_LST_USR_PD41,
			DN_FGN_PHN_INL_HST,
			DN_FGN_PHN_CNY_HST,
			DN_FGN_PHN_CT_HST,
			DN_FGN_PHN_LCL_HST,
			DF_LST_DTS_PD41,
			DD_CRT_41,
			DC_NO_HME_PHN_HST,
			DD_NO_HME_PHN_HST,
			DC_PHN_SRC_HST,
			DI_HST_OLY_PD41,
			DC_ALW_ADL_PHN_HST
		)
		VALUES 
		(
			R.DF_PRS_ID,
			R.DC_PHN_HST,
			R.DN_PHN_SEQ,
			R.DT_PHN_BCL_HST,
			R.DI_PHN_VLD_HST,
			R.DD_PHN_VER_HST,
			R.DN_PHN_XTN_HST,
			R.DN_DOM_PHN_LCL_HST,
			R.DN_DOM_PHN_XCH_HST,
			R.DN_DOM_PHN_ARA_HST,
			R.DI_PHN_WTS_HST,
			R.DX_PHN_TZ_HST,
			R.DF_LST_USR_PD41,
			R.DN_FGN_PHN_INL_HST,
			R.DN_FGN_PHN_CNY_HST,
			R.DN_FGN_PHN_CT_HST,
			R.DN_FGN_PHN_LCL_HST,
			R.DF_LST_DTS_PD41,
			R.DD_CRT_41,
			R.DC_NO_HME_PHN_HST,
			R.DD_NO_HME_PHN_HST,
			R.DC_PHN_SRC_HST,
			R.DI_HST_OLY_PD41,
			R.DC_ALW_ADL_PHN_HST
		)
	;
'
select @SQLStatement
PRINT @SQLStatement
EXEC (@SQLStatement)

				
-- ###### VALIDATION

DECLARE 
	@SumDifference BIGINT

SELECT
	@SumDifference = L.LocalSum - R.RemoteSum
FROM
	OPENQUERY
	(
		LEGEND,
		'
			SELECT
				SUM(CAST(DF_PRS_ID AS BIGINT)) AS "RemoteSum"
			FROM
				PKUB.PD41_PHN_HST PD41
			WHERE
				PD41.DF_PRS_ID NOT LIKE ''P%''
		'
	) R
	FULL OUTER JOIN
	(
		SELECT
			SUM(CAST(DF_PRS_ID AS BIGINT)) [LocalSum]
		FROM
			CDW..PD41_PHN_HST PD41
		WHERE
			PD41.DF_PRS_ID NOT LIKE 'P%'
	) L 
		ON 1 = 1


IF @SumDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('PD41_PHN_HST - The remote and local SSN sums do not match.  A full refresh of the table may be required.', 16, 11, @SumDifference)
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
				LEGEND,	
				'
					SELECT
						PD41.DF_PRS_ID AS "BF_SSN",
						COUNT(*) AS "RemoteCount"
					FROM
						PKUB.PD41_PHN_HST PD41
					WHERE PD41.DF_PRS_ID NOT LIKE ''P%''
					GROUP BY
						PD41.DF_PRS_ID
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					PD41.DF_PRS_ID [BF_SSN],
					COUNT(*) [LocalCount]
				FROM
					CDW..PD41_PHN_HST PD41
				WHERE PD41.DF_PRS_ID NOT LIKE 'P%'
				GROUP BY
					PD41.DF_PRS_ID
			) L 
				ON L.BF_SSN = R.BF_SSN
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

		PRINT 'The local record count for these SSNs does not match the remote warehouse count.  Deleting all local PD41 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			PD41
		FROM
			CDW..PD41_PHN_HST PD41
			INNER JOIN @SSN_LIST SL 
				ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = PD41.DF_PRS_ID

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		GOTO RefreshStart;

	END