USE UDW
GO

--SELECT TOP 1
--	*
--INTO 
--	dbo.DF10_BR_DFR_REQ
--FROM
--	OPENQUERY
--	(
--		DUSTER,
--		'
--			SELECT
--				*
--			FROM
--				OLWHRM1.DF10_BR_DFR_REQ DF10
--		'
--	) 

DECLARE 
	@SSNs VARCHAR(MAX) = '0', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(MAX(DF10.LF_LST_DTS_DF10), '1-1-1900 00:00:00'), 21) FROM DF10_BR_DFR_REQ DF10)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.DF10_BR_DFR_REQ DF10
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
							DF10.*
						FROM
							OLWHRM1.DF10_BR_DFR_REQ DF10
						-- comment WHERE clause for full table refresh
						WHERE
							DF10.LF_LST_DTS_DF10 > ''''' + @LastRefresh + '''''
							OR
							(
								DF10.BF_SSN IN
								(
									' + @SSNs + '
								)
							)
					''
				) 
		) D ON 
			DF10.BF_SSN = D.BF_SSN 
			AND DF10.LF_DFR_CTL_NUM = D.LF_DFR_CTL_NUM
	WHEN MATCHED THEN 
		UPDATE SET
			DF10.LC_DFR_TYP = D.LC_DFR_TYP,
			DF10.LI_REQ_FOR_DLQ = D.LI_REQ_FOR_DLQ,
			DF10.LI_DFR_BR_SIG = D.LI_DFR_BR_SIG,
			DF10.LI_DFR_CER_SIG = D.LI_DFR_CER_SIG,
			DF10.LI_DFR_LCO_SIG = D.LI_DFR_LCO_SIG,
			DF10.LD_DFR_REQ_BEG = D.LD_DFR_REQ_BEG,
			DF10.LD_DFR_REQ_END = D.LD_DFR_REQ_END,
			DF10.LC_DFR_REQ_COR = D.LC_DFR_REQ_COR,
			DF10.LF_USR_CRT_REQ_DFR = D.LF_USR_CRT_REQ_DFR,
			DF10.LD_CRT_REQ_DFR = D.LD_CRT_REQ_DFR,
			DF10.LI_CAP_DFR_INT_REQ = D.LI_CAP_DFR_INT_REQ,
			DF10.LC_DFR_STA = D.LC_DFR_STA,
			DF10.LI_DFR_NSI_PAY = D.LI_DFR_NSI_PAY,
			DF10.LD_STA_DFR10 = D.LD_STA_DFR10,
			DF10.LC_STA_DFR10 = D.LC_STA_DFR10,
			DF10.LF_DOE_SCL_DFR = D.LF_DOE_SCL_DFR,
			DF10.LI_ACD_CCL = D.LI_ACD_CCL,
			DF10.LI_FUL_ACT_DTY = D.LI_FUL_ACT_DTY,
			DF10.LI_RES_NG = D.LI_RES_NG,
			DF10.LC_ENR_STA = D.LC_ENR_STA,
			DF10.LI_NTF_STM = D.LI_NTF_STM,
			DF10.LI_LON_RCV_ENR_PRD = D.LI_LON_RCV_ENR_PRD,
			DF10.LI_EMP_AGY_50_MLE = D.LI_EMP_AGY_50_MLE,
			DF10.LI_RQR_ATT_LIS = D.LI_RQR_ATT_LIS,
			DF10.LC_ARA_TSH = D.LC_ARA_TSH,
			DF10.LD_NXT_TRM_BEG = D.LD_NXT_TRM_BEG,
			DF10.LD_RGR_EMP_AGY = D.LD_RGR_EMP_AGY,
			DF10.LD_UMP_BEG = D.LD_UMP_BEG,
			DF10.LI_ALI_FOR_REQ = D.LI_ALI_FOR_REQ,
			DF10.LF_LST_DTS_DF10 = D.LF_LST_DTS_DF10,
			DF10.LF_DFR_PRV = D.LF_DFR_PRV,
			DF10.LF_STU_SSN = D.LF_STU_SSN,
			DF10.LD_NTF_DFR_END = D.LD_NTF_DFR_END,
			DF10.LI_BR_EMP_FT = D.LI_BR_EMP_FT,
			DF10.LI_BR_SBM_POO_INC = D.LI_BR_SBM_POO_INC,
			DF10.LI_BR_SBM_FED_IXR = D.LI_BR_SBM_FED_IXR,
			DF10.LI_BR_SBM_CER_HWG = D.LI_BR_SBM_CER_HWG,
			DF10.LI_BR_MIN_INC_RQR = D.LI_BR_MIN_INC_RQR,
			DF10.LI_BR_SBM_POO_SLP = D.LI_BR_SBM_POO_SLP,
			DF10.LI_BR_SBM_WRT_STM = D.LI_BR_SBM_WRT_STM,
			DF10.LI_BR_SBM_CER_APC = D.LI_BR_SBM_CER_APC,
			DF10.LI_BR_ENR_LST_6MO = D.LI_BR_ENR_LST_6MO,
			DF10.LI_BR_SBM_CER_CHB = D.LI_BR_SBM_CER_CHB,
			DF10.LI_BR_SBM_MIL_ID = D.LI_BR_SBM_MIL_ID,
			DF10.LI_BR_SBM_MIL_ORD = D.LI_BR_SBM_MIL_ORD,
			DF10.LI_MIN_TME_DSA = D.LI_MIN_TME_DSA,
			DF10.LI_RGR_EMP_AGY = D.LI_RGR_EMP_AGY,
			DF10.LD_DFR_CER = D.LD_DFR_CER,
			DF10.LD_DFR_INF_CER = D.LD_DFR_INF_CER,
			DF10.LI_BR_POO_UMP_BNF = D.LI_BR_POO_UMP_BNF,
			DF10.LI_CMK_ELG_DFR = D.LI_CMK_ELG_DFR,
			DF10.LC_DFR_SUB_TYP = D.LC_DFR_SUB_TYP,
			DF10.LI_DFR_DOC_PVD = D.LI_DFR_DOC_PVD,
			DF10.LI_DFR_RQR_CMP = D.LI_DFR_RQR_CMP,
			DF10.LD_BR_REQ_DFR_BEG = D.LD_BR_REQ_DFR_BEG,
			DF10.LD_DFR_SPT_DOC_BEG = D.LD_DFR_SPT_DOC_BEG,
			DF10.LD_DFR_SPT_DOC_END = D.LD_DFR_SPT_DOC_END,
			DF10.LI_DFR_SPT_DOC_ACP = D.LI_DFR_SPT_DOC_ACP,
			DF10.LC_DFR_DNL_USR_ENT = D.LC_DFR_DNL_USR_ENT,
			DF10.LI_DFR_DOC_SPT_REQ = D.LI_DFR_DOC_SPT_REQ,
			DF10.LI_REQ_PST_DFR_DFR = D.LI_REQ_PST_DFR_DFR,
			DF10.LI_REQ_IN_SCL_DFR = D.LI_REQ_IN_SCL_DFR,
			DF10.LD_STP_ENR_MIN_HT = D.LD_STP_ENR_MIN_HT,
			DF10.LA_BR_PAY_CHK_JOB = D.LA_BR_PAY_CHK_JOB,
			DF10.LC_BR_PAY_CHK_FRQ = D.LC_BR_PAY_CHK_FRQ,
			DF10.LC_BR_EMP_STA = D.LC_BR_EMP_STA,
			DF10.LN_BR_FAM_SIZ = D.LN_BR_FAM_SIZ,
			DF10.LC_FED_POV_GID_ST = D.LC_FED_POV_GID_ST,
			DF10.LA_MTH_FED_MIN_WGE = D.LA_MTH_FED_MIN_WGE,
			DF10.LA_BR_CLC_POV = D.LA_BR_CLC_POV,
			DF10.LC_SEL_EHD_DFR_TYP = D.LC_SEL_EHD_DFR_TYP
	WHEN NOT MATCHED THEN
		INSERT 
		(
			BF_SSN,
			LF_DFR_CTL_NUM,
			LC_DFR_TYP,
			LI_REQ_FOR_DLQ,
			LI_DFR_BR_SIG,
			LI_DFR_CER_SIG,
			LI_DFR_LCO_SIG,
			LD_DFR_REQ_BEG,
			LD_DFR_REQ_END,
			LC_DFR_REQ_COR,
			LF_USR_CRT_REQ_DFR,
			LD_CRT_REQ_DFR,
			LI_CAP_DFR_INT_REQ,
			LC_DFR_STA,
			LI_DFR_NSI_PAY,
			LD_STA_DFR10,
			LC_STA_DFR10,
			LF_DOE_SCL_DFR,
			LI_ACD_CCL,
			LI_FUL_ACT_DTY,
			LI_RES_NG,
			LC_ENR_STA,
			LI_NTF_STM,
			LI_LON_RCV_ENR_PRD,
			LI_EMP_AGY_50_MLE,
			LI_RQR_ATT_LIS,
			LC_ARA_TSH,
			LD_NXT_TRM_BEG,
			LD_RGR_EMP_AGY,
			LD_UMP_BEG,
			LI_ALI_FOR_REQ,
			LF_LST_DTS_DF10,
			LF_DFR_PRV,
			LF_STU_SSN,
			LD_NTF_DFR_END,
			LI_BR_EMP_FT,
			LI_BR_SBM_POO_INC,
			LI_BR_SBM_FED_IXR,
			LI_BR_SBM_CER_HWG,
			LI_BR_MIN_INC_RQR,
			LI_BR_SBM_POO_SLP,
			LI_BR_SBM_WRT_STM,
			LI_BR_SBM_CER_APC,
			LI_BR_ENR_LST_6MO,
			LI_BR_SBM_CER_CHB,
			LI_BR_SBM_MIL_ID,
			LI_BR_SBM_MIL_ORD,
			LI_MIN_TME_DSA,
			LI_RGR_EMP_AGY,
			LD_DFR_CER,
			LD_DFR_INF_CER,
			LI_BR_POO_UMP_BNF,
			LI_CMK_ELG_DFR,
			LC_DFR_SUB_TYP,
			LI_DFR_DOC_PVD,
			LI_DFR_RQR_CMP,
			LD_BR_REQ_DFR_BEG,
			LD_DFR_SPT_DOC_BEG,
			LD_DFR_SPT_DOC_END,
			LI_DFR_SPT_DOC_ACP,
			LC_DFR_DNL_USR_ENT,
			LI_DFR_DOC_SPT_REQ,
			LI_REQ_PST_DFR_DFR,
			LI_REQ_IN_SCL_DFR,
			LD_STP_ENR_MIN_HT,
			LA_BR_PAY_CHK_JOB,
			LC_BR_PAY_CHK_FRQ,
			LC_BR_EMP_STA,
			LN_BR_FAM_SIZ,
			LC_FED_POV_GID_ST,
			LA_MTH_FED_MIN_WGE,
			LA_BR_CLC_POV,
			LC_SEL_EHD_DFR_TYP
		)
		VALUES 
		(
			D.BF_SSN,
			D.LF_DFR_CTL_NUM,
			D.LC_DFR_TYP,
			D.LI_REQ_FOR_DLQ,
			D.LI_DFR_BR_SIG,
			D.LI_DFR_CER_SIG,
			D.LI_DFR_LCO_SIG,
			D.LD_DFR_REQ_BEG,
			D.LD_DFR_REQ_END,
			D.LC_DFR_REQ_COR,
			D.LF_USR_CRT_REQ_DFR,
			D.LD_CRT_REQ_DFR,
			D.LI_CAP_DFR_INT_REQ,
			D.LC_DFR_STA,
			D.LI_DFR_NSI_PAY,
			D.LD_STA_DFR10,
			D.LC_STA_DFR10,
			D.LF_DOE_SCL_DFR,
			D.LI_ACD_CCL,
			D.LI_FUL_ACT_DTY,
			D.LI_RES_NG,
			D.LC_ENR_STA,
			D.LI_NTF_STM,
			D.LI_LON_RCV_ENR_PRD,
			D.LI_EMP_AGY_50_MLE,
			D.LI_RQR_ATT_LIS,
			D.LC_ARA_TSH,
			D.LD_NXT_TRM_BEG,
			D.LD_RGR_EMP_AGY,
			D.LD_UMP_BEG,
			D.LI_ALI_FOR_REQ,
			D.LF_LST_DTS_DF10,
			D.LF_DFR_PRV,
			D.LF_STU_SSN,
			D.LD_NTF_DFR_END,
			D.LI_BR_EMP_FT,
			D.LI_BR_SBM_POO_INC,
			D.LI_BR_SBM_FED_IXR,
			D.LI_BR_SBM_CER_HWG,
			D.LI_BR_MIN_INC_RQR,
			D.LI_BR_SBM_POO_SLP,
			D.LI_BR_SBM_WRT_STM,
			D.LI_BR_SBM_CER_APC,
			D.LI_BR_ENR_LST_6MO,
			D.LI_BR_SBM_CER_CHB,
			D.LI_BR_SBM_MIL_ID,
			D.LI_BR_SBM_MIL_ORD,
			D.LI_MIN_TME_DSA,
			D.LI_RGR_EMP_AGY,
			D.LD_DFR_CER,
			D.LD_DFR_INF_CER,
			D.LI_BR_POO_UMP_BNF,
			D.LI_CMK_ELG_DFR,
			D.LC_DFR_SUB_TYP,
			D.LI_DFR_DOC_PVD,
			D.LI_DFR_RQR_CMP,
			D.LD_BR_REQ_DFR_BEG,
			D.LD_DFR_SPT_DOC_BEG,
			D.LD_DFR_SPT_DOC_END,
			D.LI_DFR_SPT_DOC_ACP,
			D.LC_DFR_DNL_USR_ENT,
			D.LI_DFR_DOC_SPT_REQ,
			D.LI_REQ_PST_DFR_DFR,
			D.LI_REQ_IN_SCL_DFR,
			D.LD_STP_ENR_MIN_HT,
			D.LA_BR_PAY_CHK_JOB,
			D.LC_BR_PAY_CHK_FRQ,
			D.LC_BR_EMP_STA,
			D.LN_BR_FAM_SIZ,
			D.LC_FED_POV_GID_ST,
			D.LA_MTH_FED_MIN_WGE,
			D.LA_BR_CLC_POV,
			D.LC_SEL_EHD_DFR_TYP
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
		DUSTER,
		'
			SELECT
				COUNT(*) AS "RemoteCount"
			FROM
				OLWHRM1.DF10_BR_DFR_REQ DF10
		'	
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			UDW..DF10_BR_DFR_REQ DF10
	) L ON 1 = 1

IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('DF10_BR_DFR_REQ - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
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
						DF10.BF_SSN,
						COUNT(*) AS "RemoteCount"
					FROM
						OLWHRM1.DF10_BR_DFR_REQ DF10
					GROUP BY
						DF10.BF_SSN
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					DF10.BF_SSN,
					COUNT(*) [LocalCount]
				FROM
					UDW..DF10_BR_DFR_REQ DF10
				GROUP BY
					DF10.BF_SSN
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

		PRINT 'The local record count for these SSNs does not match the remote warehouse count: ' + @SSNs + '  Deleting all local DF10 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			DF10
		FROM
			UDW..DF10_BR_DFR_REQ DF10
			INNER JOIN @SSN_LIST SL ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = DF10.BF_SSN

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))
		-- Local and remote record counts did not match.  Run refresh again.
		GOTO RefreshStart;
	END

