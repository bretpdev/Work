USE CDW
GO

--SELECT TOP 1
--	*
--INTO 
--	dbo.FB10_BR_FOR_REQ
--FROM
--	OPENQUERY
--	(
--		LEGEND,
--		'
--			SELECT
--				*
--			FROM
--				PKUB.FB10_BR_FOR_REQ FB10
--		'
--	) 

DECLARE 
	@SSNs VARCHAR(MAX) = '0', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(MAX(FB10.LF_LST_DTS_FB10), '1-1-1900 00:00:00'), 21) FROM FB10_BR_FOR_REQ FB10)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.FB10_BR_FOR_REQ FB10
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
							FB10.*
						FROM
							PKUB.FB10_BR_FOR_REQ FB10
						-- comment WHERE clause for full table refresh
						WHERE
							FB10.LF_LST_DTS_FB10 > ''''' + @LastRefresh + '''''
							OR
							(
								FB10.BF_SSN IN
								(
									' + @SSNs + '
								)
							)
							
					''
				) 
		) D ON 
			FB10.BF_SSN = D.BF_SSN 
			AND FB10.LF_FOR_CTL_NUM = D.LF_FOR_CTL_NUM
	WHEN MATCHED THEN 
		UPDATE SET
			FB10.LC_FOR_TYP = D.LC_FOR_TYP,
			FB10.LC_FOR_SUB_TYP = D.LC_FOR_SUB_TYP,
			FB10.LI_FOR_BR_SIG = D.LI_FOR_BR_SIG,
			FB10.LD_FOR_REQ_BEG = D.LD_FOR_REQ_BEG,
			FB10.LC_FOR_REQ_COR = D.LC_FOR_REQ_COR,
			FB10.LD_FOR_REQ_END = D.LD_FOR_REQ_END,
			FB10.LF_USR_CRT_REQ_FOR = D.LF_USR_CRT_REQ_FOR,
			FB10.LD_CRT_REQ_FOR = D.LD_CRT_REQ_FOR,
			FB10.LI_CAP_FOR_INT_REQ = D.LI_CAP_FOR_INT_REQ,
			FB10.LC_FOR_STA = D.LC_FOR_STA,
			FB10.LD_STA_FOR10 = D.LD_STA_FOR10,
			FB10.LC_STA_FOR10 = D.LC_STA_FOR10,
			FB10.LI_PNR_SIG = D.LI_PNR_SIG,
			FB10.LI_XED_ATH = D.LI_XED_ATH,
			FB10.LI_FOR_COS_SIG = D.LI_FOR_COS_SIG,
			FB10.LI_FOR_RES_NG = D.LI_FOR_RES_NG,
			FB10.LF_LST_DTS_FB10 = D.LF_LST_DTS_FB10,
			FB10.LN_RPS_SEQ = D.LN_RPS_SEQ,
			FB10.LF_FOR_PRV = D.LF_FOR_PRV,
			FB10.LI_FOR_CER_OCL_SIG = D.LI_FOR_CER_OCL_SIG,
			FB10.LI_FOR_DOD_FRM_SPY = D.LI_FOR_DOD_FRM_SPY,
			FB10.LI_FOR_BR_ENR_VER = D.LI_FOR_BR_ENR_VER,
			FB10.LI_FOR_BR_EMP_FT = D.LI_FOR_BR_EMP_FT,
			FB10.LI_FOR_BR_POO_INC = D.LI_FOR_BR_POO_INC,
			FB10.LI_FOR_BR_FED_IXR = D.LI_FOR_BR_FED_IXR,
			FB10.LI_FOR_BR_MIN_INC = D.LI_FOR_BR_MIN_INC,
			FB10.LI_FOR_BR_POO_PAY = D.LI_FOR_BR_POO_PAY,
			FB10.LD_FOR_INF_CER = D.LD_FOR_INF_CER,
			FB10.LI_CMK_ELG_FOR = D.LI_CMK_ELG_FOR,
			FB10.LF_DOE_SCL_FOR = D.LF_DOE_SCL_FOR,
			FB10.LA_REQ_RDC_PAY = D.LA_REQ_RDC_PAY,
			FB10.LI_FOR_MED_IRN = D.LI_FOR_MED_IRN,
			FB10.LI_FOR_MED_LIC_CER = D.LI_FOR_MED_LIC_CER,
			FB10.LC_FOR_XCP_DCR_TYP = D.LC_FOR_XCP_DCR_TYP,
			FB10.LC_FOR_NEW_SUB_TYP = D.LC_FOR_NEW_SUB_TYP,
			FB10.LI_FOR_COV_DLQ = D.LI_FOR_COV_DLQ,
			FB10.LA_BR_PAY_CHK_JOB = D.LA_BR_PAY_CHK_JOB,
			FB10.LC_BR_PAY_CHK_FRQ = D.LC_BR_PAY_CHK_FRQ,
			FB10.LD_FOR_BR_REQ_BEG = D.LD_FOR_BR_REQ_BEG,
			FB10.LD_FOR_BR_REQ_END = D.LD_FOR_BR_REQ_END,
			FB10.LC_FOR_DNL_USR_ENT = D.LC_FOR_DNL_USR_ENT,
			FB10.LI_FOR_SPT_DOC_ACP = D.LI_FOR_SPT_DOC_ACP,
			FB10.LA_BR_MTH_IRL_ISL = D.LA_BR_MTH_IRL_ISL,
			FB10.LA_BR_MTH_EXT_ISL = D.LA_BR_MTH_EXT_ISL,
			FB10.LI_BRQ_TMP_DNL_FOR = D.LI_BRQ_TMP_DNL_FOR,
			FB10.LI_BRQ_TMP_FOR_DLQ = D.LI_BRQ_TMP_FOR_DLQ
	WHEN NOT MATCHED THEN
		INSERT 
		(
			BF_SSN,
			LF_FOR_CTL_NUM,
			LC_FOR_TYP,
			LC_FOR_SUB_TYP,
			LI_FOR_BR_SIG,
			LD_FOR_REQ_BEG,
			LC_FOR_REQ_COR,
			LD_FOR_REQ_END,
			LF_USR_CRT_REQ_FOR,
			LD_CRT_REQ_FOR,
			LI_CAP_FOR_INT_REQ,
			LC_FOR_STA,
			LD_STA_FOR10,
			LC_STA_FOR10,
			LI_PNR_SIG,
			LI_XED_ATH,
			LI_FOR_COS_SIG,
			LI_FOR_RES_NG,
			LF_LST_DTS_FB10,
			LN_RPS_SEQ,
			LF_FOR_PRV,
			LI_FOR_CER_OCL_SIG,
			LI_FOR_DOD_FRM_SPY,
			LI_FOR_BR_ENR_VER,
			LI_FOR_BR_EMP_FT,
			LI_FOR_BR_POO_INC,
			LI_FOR_BR_FED_IXR,
			LI_FOR_BR_MIN_INC,
			LI_FOR_BR_POO_PAY,
			LD_FOR_INF_CER,
			LI_CMK_ELG_FOR,
			LF_DOE_SCL_FOR,
			LA_REQ_RDC_PAY,
			LI_FOR_MED_IRN,
			LI_FOR_MED_LIC_CER,
			LC_FOR_XCP_DCR_TYP,
			LC_FOR_NEW_SUB_TYP,
			LI_FOR_COV_DLQ,
			LA_BR_PAY_CHK_JOB,
			LC_BR_PAY_CHK_FRQ,
			LD_FOR_BR_REQ_BEG,
			LD_FOR_BR_REQ_END,
			LC_FOR_DNL_USR_ENT,
			LI_FOR_SPT_DOC_ACP,
			LA_BR_MTH_IRL_ISL,
			LA_BR_MTH_EXT_ISL,
			LI_BRQ_TMP_DNL_FOR,
			LI_BRQ_TMP_FOR_DLQ
		)
		VALUES 
		(
			D.BF_SSN,
			D.LF_FOR_CTL_NUM,
			D.LC_FOR_TYP,
			D.LC_FOR_SUB_TYP,
			D.LI_FOR_BR_SIG,
			D.LD_FOR_REQ_BEG,
			D.LC_FOR_REQ_COR,
			D.LD_FOR_REQ_END,
			D.LF_USR_CRT_REQ_FOR,
			D.LD_CRT_REQ_FOR,
			D.LI_CAP_FOR_INT_REQ,
			D.LC_FOR_STA,
			D.LD_STA_FOR10,
			D.LC_STA_FOR10,
			D.LI_PNR_SIG,
			D.LI_XED_ATH,
			D.LI_FOR_COS_SIG,
			D.LI_FOR_RES_NG,
			D.LF_LST_DTS_FB10,
			D.LN_RPS_SEQ,
			D.LF_FOR_PRV,
			D.LI_FOR_CER_OCL_SIG,
			D.LI_FOR_DOD_FRM_SPY,
			D.LI_FOR_BR_ENR_VER,
			D.LI_FOR_BR_EMP_FT,
			D.LI_FOR_BR_POO_INC,
			D.LI_FOR_BR_FED_IXR,
			D.LI_FOR_BR_MIN_INC,
			D.LI_FOR_BR_POO_PAY,
			D.LD_FOR_INF_CER,
			D.LI_CMK_ELG_FOR,
			D.LF_DOE_SCL_FOR,
			D.LA_REQ_RDC_PAY,
			D.LI_FOR_MED_IRN,
			D.LI_FOR_MED_LIC_CER,
			D.LC_FOR_XCP_DCR_TYP,
			D.LC_FOR_NEW_SUB_TYP,
			D.LI_FOR_COV_DLQ,
			D.LA_BR_PAY_CHK_JOB,
			D.LC_BR_PAY_CHK_FRQ,
			D.LD_FOR_BR_REQ_BEG,
			D.LD_FOR_BR_REQ_END,
			D.LC_FOR_DNL_USR_ENT,
			D.LI_FOR_SPT_DOC_ACP,
			D.LA_BR_MTH_IRL_ISL,
			D.LA_BR_MTH_EXT_ISL,
			D.LI_BRQ_TMP_DNL_FOR,
			D.LI_BRQ_TMP_FOR_DLQ
		)
	-- !!! uncomment lines below ONLY when doing a full table refresh 
	--	WHEN NOT MATCHED BY SOURCE THEN
	--		DELETE
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
				PKUB.FB10_BR_FOR_REQ FB10
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			CDW..FB10_BR_FOR_REQ FB10
	) L ON 1 = 1

IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('FB10_BR_FOR_REQ - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
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
						FB10.BF_SSN,
						COUNT(*) AS "RemoteCount"
					FROM
						PKUB.FB10_BR_FOR_REQ FB10
					GROUP BY
						FB10.BF_SSN
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					FB10.BF_SSN,
					COUNT(*) [LocalCount]
				FROM
					CDW..FB10_BR_FOR_REQ FB10
				GROUP BY
					FB10.BF_SSN
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

		PRINT 'The local record count for these SSNs does not match the remote warehouse count: ' + @SSNs + '  Deleting all local FB10 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			FB10
		FROM
			CDW..FB10_BR_FOR_REQ FB10
			INNER JOIN @SSN_LIST SL ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = FB10.BF_SSN

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))
		-- Local and remote record counts did not match.  Run refresh again.
		GOTO RefreshStart;
	END

