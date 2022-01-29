--This can not use the sproc because it loos at 2 fields
USE UDW
GO

DECLARE 
	@SSNs VARCHAR(MAX) = '''''0''''', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:




DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(DATEADD(HOUR,-3,MAX(PD32.DF_LST_DTS_PD32)), '1-1-1900 00:00:00'), 21) FROM PD32_PRS_ADR_EML PD32)
DECLARE @LastRefreshStatusDate VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(DATEADD(HOUR,-3,CAST(MAX(PD32.DD_STA_PD32) AS DATETIME)), '1-1-1900 00:00:00'), 21) FROM PD32_PRS_ADR_EML PD32)
PRINT 'Last Refreshed timestamp at: ' + @LastRefresh
PRINT 'Last Refreshed status date at: ' + @LastRefreshStatusDate


DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.PD32_PRS_ADR_EML PD32
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
							OLWHRM1.PD32_PRS_ADR_EML PD32
						-- comment WHERE clause for full table refresh
						WHERE
							PD32.DF_CRT_DTS_PD32 > ''''' + @LastRefresh + '''''
							OR PD32.DF_LST_DTS_PD32 > ''''' + @LastRefresh + '''''
							OR PD32.DD_STA_PD32 >= ''''' + @LastRefreshStatusDate + '''''
							OR
							(
								PD32.DF_PRS_ID IN
								(
									' + @SSNs + '
								)
							)
					''
				) 
		) d 
			ON D.DF_PRS_ID = PD32.DF_PRS_ID 
			AND D.DC_ADR_EML = PD32.DC_ADR_EML 
			AND D.DF_CRT_DTS_PD32 = PD32.DF_CRT_DTS_PD32
	WHEN MATCHED THEN 
		UPDATE SET 
			PD32.DC_STA_PD32 = D.DC_STA_PD32,
			PD32.DD_STA_PD32 = D.DD_STA_PD32,
			PD32.DD_VER_ADR_EML = D.DD_VER_ADR_EML,
			PD32.DD_CRT_PD32 = D.DD_CRT_PD32,
			PD32.DC_SRC_ADR = D.DC_SRC_ADR,
			PD32.DI_VLD_ADR_EML = D.DI_VLD_ADR_EML,
			PD32.DF_LST_USR_PD32 = D.DF_LST_USR_PD32,
			PD32.DF_LST_DTS_PD32 = D.DF_LST_DTS_PD32,
			PD32.DX_ADR_EML = D.DX_ADR_EML,
			PD32.DI_SND_LTR_EML = D.DI_SND_LTR_EML,
			PD32.DC_REA_EML_NOT_SND = D.DC_REA_EML_NOT_SND,
			PD32.DC_DL_COR_TYP = D.DC_DL_COR_TYP
	WHEN NOT MATCHED THEN
		INSERT 
		(
			DF_PRS_ID,
			DC_ADR_EML,
			DF_CRT_DTS_PD32,
			DC_STA_PD32,
			DD_STA_PD32,
			DD_VER_ADR_EML,
			DD_CRT_PD32,
			DC_SRC_ADR,
			DI_VLD_ADR_EML,
			DF_LST_USR_PD32,
			DF_LST_DTS_PD32,
			DX_ADR_EML,
			DI_SND_LTR_EML,
			DC_REA_EML_NOT_SND,
			DC_DL_COR_TYP

		)
		VALUES 
		(
			D.DF_PRS_ID,
			D.DC_ADR_EML,
			D.DF_CRT_DTS_PD32,
			D.DC_STA_PD32,
			D.DD_STA_PD32,
			D.DD_VER_ADR_EML,
			D.DD_CRT_PD32,
			D.DC_SRC_ADR,
			D.DI_VLD_ADR_EML,
			D.DF_LST_USR_PD32,
			D.DF_LST_DTS_PD32,
			D.DX_ADR_EML,
			D.DI_SND_LTR_EML,
			D.DC_REA_EML_NOT_SND,
			D.DC_DL_COR_TYP
		)
	-- !!!  uncomment lines below ONLY when doing a full table refresh 
	--WHEN NOT MATCHED BY SOURCE THEN
	    --DELETE
	;
'
PRINT @SQLStatement
EXEC (@SQLStatement)

				
-- ###### VALIDATION
-- the PD32_PRS_ADR_EML date is unreliable when a person's SSN has been changed.
-- sum all SSNs in order to dentify missing/change/added SSNs
DECLARE 
	@SumDifference BIGINT

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
				OLWHRM1.PD32_PRS_ADR_EML PD32
			WHERE
				PD32.DF_PRS_ID NOT LIKE ''P%''
		'
	) R
	FULL OUTER JOIN
	(
		SELECT
			SUM(CAST(DF_PRS_ID AS BIGINT)) [LocalSum]
		FROM
			UDW..PD32_PRS_ADR_EML PD32
		WHERE
			PD32.DF_PRS_ID NOT LIKE 'P%'
	) L 
		ON 1 = 1


IF @SumDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('PD32_PRS_ADR_EML - The remote and local SSN sums do not match.  A full refresh of the table may be required.', 16, 11, @SumDifference)
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
						PD32.DF_PRS_ID AS "BF_SSN",
						COUNT(*) AS "RemoteCount"
					FROM
						OLWHRM1.PD32_PRS_ADR_EML PD32
					GROUP BY
						PD32.DF_PRS_ID
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					PD32.DF_PRS_ID [BF_SSN],
					COUNT(*) [LocalCount]
				FROM
					UDW..PD32_PRS_ADR_EML PD32
				GROUP BY
					PD32.DF_PRS_ID
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

		PRINT 'The local record count for these SSNs does not match the remote warehouse count.  Deleting all local PD32 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			PD32
		FROM
			UDW..PD32_PRS_ADR_EML PD32
			INNER JOIN @SSN_LIST SL 
				ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = PD32.DF_PRS_ID

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		GOTO RefreshStart;

	END