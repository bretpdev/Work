USE UDW
GO

--SELECT TOP 1
--	*
--INTO 
--	dbo.CL10_CLM_PCL
--FROM
--	OPENQUERY
--	(
--		DUSTER,
--		'
--			SELECT
--				*
--			FROM
--				OLWHRM1.CL10_CLM_PCL
--		'
--	)
	 
DECLARE 
	@SSNs VARCHAR(MAX) = '0', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(MAX(CL10.LF_LST_DTS_CL10), '1-1-1900 00:00:00'), 21) FROM CL10_CLM_PCL CL10)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.CL10_CLM_PCL CL10
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
							CL10.*
						FROM
							OLWHRM1.CL10_CLM_PCL CL10
						-- comment WHERE clause for full table refresh
						WHERE
							CL10.LF_LST_DTS_CL10 > ''''' + @LastRefresh + '''''
							OR
							(
								CL10.BF_SSN IN
								(
									' + @SSNs + '
								)
							)
					''
				) 
		) D ON D.BF_SSN = CL10.BF_SSN AND D.LN_SEQ_CLM_PCL = CL10.LN_SEQ_CLM_PCL
	WHEN MATCHED THEN 
		UPDATE SET
			CL10.LC_REA_CLM_PCL = D.LC_REA_CLM_PCL,
			CL10.LC_TYP_REC_CLM_PCL = D.LC_TYP_REC_CLM_PCL,
			CL10.LF_USR_ASN_CLM_PCL = D.LF_USR_ASN_CLM_PCL,
			CL10.LI_CLM_GTR_RCV = D.LI_CLM_GTR_RCV,
			CL10.LD_CLM_RQR = D.LD_CLM_RQR,
			CL10.LF_CLM_BCH = D.LF_CLM_BCH,
			CL10.LF_LST_DTS_CL10 = D.LF_LST_DTS_CL10,
			CL10.LI_CLM_QA = D.LI_CLM_QA,
			CL10.LD_GTR_CLM_RCI = D.LD_GTR_CLM_RCI,
			CL10.LC_GTR_CLM_ACK = D.LC_GTR_CLM_ACK,
			CL10.LC_CAN_STA_CCI = D.LC_CAN_STA_CCI,
			CL10.LI_CLM_CLL_RCV = D.LI_CLM_CLL_RCV,
			CL10.LC_XCP_PRF = D.LC_XCP_PRF
	WHEN NOT MATCHED THEN
		INSERT 
		(
			BF_SSN,
			LN_SEQ_CLM_PCL,
			LC_REA_CLM_PCL,
			LC_TYP_REC_CLM_PCL,
			LF_USR_ASN_CLM_PCL,
			LI_CLM_GTR_RCV,
			LD_CLM_RQR,
			LF_CLM_BCH,
			LF_LST_DTS_CL10,
			LI_CLM_QA,
			LD_GTR_CLM_RCI,
			LC_GTR_CLM_ACK,
			LC_CAN_STA_CCI,
			LI_CLM_CLL_RCV,
			LC_XCP_PRF
		)
		VALUES 
		(
			D.BF_SSN,
			D.LN_SEQ_CLM_PCL,
			D.LC_REA_CLM_PCL,
			D.LC_TYP_REC_CLM_PCL,
			D.LF_USR_ASN_CLM_PCL,
			D.LI_CLM_GTR_RCV,
			D.LD_CLM_RQR,
			D.LF_CLM_BCH,
			D.LF_LST_DTS_CL10,
			D.LI_CLM_QA,
			D.LD_GTR_CLM_RCI,
			D.LC_GTR_CLM_ACK,
			D.LC_CAN_STA_CCI,
			D.LI_CLM_CLL_RCV,
			D.LC_XCP_PRF
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
				OLWHRM1.CL10_CLM_PCL CL10
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			UDW..CL10_CLM_PCL Cl10
	) L ON 1 = 1
	
IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('CL10_CLM_PCL - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
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
			R.BF_SSN,
			L.BF_SSN
		FROM
			OPENQUERY
			(
				DUSTER,	
				'
					SELECT
						CL10.BF_SSN,
						COUNT(*) AS "RemoteCount"
					FROM
						OLWHRM1.CL10_CLM_PCL CL10
					GROUP BY
						CL10.BF_SSN
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					CL10.BF_SSN,
					COUNT(*) [LocalCount]
				FROM
					UDW..CL10_CLM_PCL CL10
				GROUP BY
					CL10.BF_SSN
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

		PRINT 'The local record count for these SSNs does not match the remote warehouse count: ' + @SSNs + '  Deleting all local CL10 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			CL10
		FROM
			UDW..CL10_CLM_PCL CL10
			INNER JOIN @SSN_LIST SL ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = CL10.BF_SSN

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		GOTO RefreshStart;

	END
