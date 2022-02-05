USE CDW
GO

--SELECT TOP 1
--	*
--INTO 
--	dbo.AY10_BR_LON_ATY
--FROM
--	OPENQUERY
--	(H
--		LEGEND,
--		'
--			SELECT
--				*
--			FROM
--				PKUB.AY10_BR_LON_ATY
--		'
--	) 

DECLARE 
	@SSNs VARCHAR(MAX) = '0', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(MAX(AY10.LF_LST_DTS_AY10), '1-1-1900 00:00:00'), 21) FROM AY10_BR_LON_ATY AY10)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.AY10_BR_LON_ATY AY10
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
							AY10.*
						FROM
							PKUB.AY10_BR_LON_ATY AY10
						-- comment WHERE clause for full table refresh
						WHERE
							AY10.LF_LST_DTS_AY10 > ''''' + @LastRefresh + '''''
							OR
							(
								AY10.BF_SSN IN
								(
									' + @SSNs + '
								)
							)
					''
				) 
		) L ON L.BF_SSN = AY10.BF_SSN AND L.LN_ATY_SEQ = AY10.LN_ATY_SEQ
	WHEN MATCHED THEN 
		UPDATE SET 
			AY10.LD_ATY_RSP = L.LD_ATY_RSP,
			AY10.LT_ATY_RSP = L.LT_ATY_RSP,
			AY10.LF_ATY_RCP = L.LF_ATY_RCP,
			AY10.LD_ATY_REQ_RCV = L.LD_ATY_REQ_RCV,
			AY10.LD_REQ_RSP_ATY_PRF = L.LD_REQ_RSP_ATY_PRF,
			AY10.LF_USR_REQ_ATY = L.LF_USR_REQ_ATY,
			AY10.LC_PRD_CAL = L.LC_PRD_CAL,
			AY10.LF_PRF_BY = L.LF_PRF_BY,
			AY10.LC_STA_ACTY10 = L.LC_STA_ACTY10,
			AY10.LD_STA_ACTY10 = L.LD_STA_ACTY10,
			AY10.LI_ATY_MKP_GRC = L.LI_ATY_MKP_GRC,
			AY10.LC_ATY_RCP = L.LC_ATY_RCP,
			AY10.LF_LST_DTS_AY10 = L.LF_LST_DTS_AY10,
			AY10.LC_ATY_RGD_TO = L.LC_ATY_RGD_TO,
			AY10.LF_ATY_RGD_TO = L.LF_ATY_RGD_TO,
			AY10.PF_RSP_ACT = L.PF_RSP_ACT,
			AY10.PF_REQ_ACT = L.PF_REQ_ACT,
			AY10.LF_COR_DOC = L.LF_COR_DOC,
			AY10.LD_COR_RCV_SNT = L.LD_COR_RCV_SNT,
			AY10.LC_COR_IN_OUT = L.LC_COR_IN_OUT,
			AY10.AN_SEQ = L.AN_SEQ,
			AY10.IC_LON_PGM = L.IC_LON_PGM,
			AY10.AN_LC_APL_SEQ = L.AN_LC_APL_SEQ
	WHEN NOT MATCHED THEN
		INSERT 
		(
			BF_SSN,
			LN_ATY_SEQ,
			LD_ATY_RSP,
			LT_ATY_RSP,
			LF_ATY_RCP,
			LD_ATY_REQ_RCV,
			LD_REQ_RSP_ATY_PRF,
			LF_USR_REQ_ATY,
			LC_PRD_CAL,
			LF_PRF_BY,
			LC_STA_ACTY10,
			LD_STA_ACTY10,
			LI_ATY_MKP_GRC,
			LC_ATY_RCP,
			LF_LST_DTS_AY10,
			LC_ATY_RGD_TO,
			LF_ATY_RGD_TO,
			PF_RSP_ACT,
			PF_REQ_ACT,
			LF_COR_DOC,
			LD_COR_RCV_SNT,
			LC_COR_IN_OUT,
			AN_SEQ,
			IC_LON_PGM,
			AN_LC_APL_SEQ
		)
		VALUES 
		(
			L.BF_SSN,
			L.LN_ATY_SEQ,
			L.LD_ATY_RSP,
			L.LT_ATY_RSP,
			L.LF_ATY_RCP,
			L.LD_ATY_REQ_RCV,
			L.LD_REQ_RSP_ATY_PRF,
			L.LF_USR_REQ_ATY,
			L.LC_PRD_CAL,
			L.LF_PRF_BY,
			L.LC_STA_ACTY10,
			L.LD_STA_ACTY10,
			L.LI_ATY_MKP_GRC,
			L.LC_ATY_RCP,
			L.LF_LST_DTS_AY10,
			L.LC_ATY_RGD_TO,
			L.LF_ATY_RGD_TO,
			L.PF_RSP_ACT,
			L.PF_REQ_ACT,
			L.LF_COR_DOC,
			L.LD_COR_RCV_SNT,
			L.LC_COR_IN_OUT,
			L.AN_SEQ,
			L.IC_LON_PGM,
			L.AN_LC_APL_SEQ
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
				PKUB.AY10_BR_LON_ATY
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			AY10_BR_LON_ATY
	) L ON 1 = 1


IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('AY10_BR_LON_ATY - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
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
				LEGEND,	
				'
					SELECT
						AY10.BF_SSN,
						COUNT(*) AS "RemoteCount"
					FROM
						PKUB.AY10_BR_LON_ATY AY10
					GROUP BY
						AY10.BF_SSN
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					AY10.BF_SSN,
					COUNT(*) [LocalCount]
				FROM
					CDW..AY10_BR_LON_ATY AY10
				GROUP BY
					AY10.BF_SSN
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

		PRINT 'The local record count for these SSNs does not match the remote warehouse count: ' + @SSNs + '  Deleting all local AY10 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			AY10
		FROM
			CDW..AY10_BR_LON_ATY AY10
			INNER JOIN @SSN_LIST SL ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = AY10.BF_SSN

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		--Rerun the refresh proccess for the SSNs in @SSNs
		GOTO RefreshStart;

	END
