--This can not use the sproc because it loos at 2 fields
USE CDW
GO
DECLARE 
	@SSNs VARCHAR(MAX) = '''''0''''', -- initialize to a non-SSN
	@LoopCount TINYINT = 0

RefreshStart:

DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(DATEADD(HOUR,-3,MAX(LN15.LF_LST_DTS_LN15)), '1-1-1900 00:00:00'), 21) FROM LN15_DSB LN15)
DECLARE @LastRefreshStatusDate VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(DATEADD(HOUR,-3,CAST(MAX(LN15.LD_STA_LON15) AS DATETIME)), '1-1-1900 00:00:00'), 21) FROM LN15_DSB LN15)
PRINT 'Last Refreshed timestamp at: ' + @LastRefresh
PRINT 'Last Refreshed status date at: ' + @LastRefreshStatusDate


DECLARE @SQLStatement VARCHAR(MAX) = 
'
	MERGE 
		dbo.LN15_DSB LN15
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
							*
						FROM
							PKUB.LN15_DSB LN15
						-- comment WHERE clause for full table refresh
						WHERE
							LN15.LF_LST_DTS_LN15 > ''''' + @LastRefresh + '''''
							OR LN15.LD_STA_LON15 >= ''''' + @LastRefreshStatusDate + '''''
							OR
							(
								LN15.BF_SSN IN
								(
									' + @SSNs + '
								)
							)
							FOR READ ONLY WITH UR
					''
				) 
		) L 
			ON L.BF_SSN = LN15.BF_SSN 
			AND L.LN_BR_DSB_SEQ = LN15.LN_BR_DSB_SEQ
	WHEN MATCHED THEN 
		UPDATE SET 
			LN15.LA_DSB = L.LA_DSB,
			LN15.LA_DSB_CAN = L.LA_DSB_CAN,
			LN15.LC_DSB_CAN_TYP = L.LC_DSB_CAN_TYP,
			LN15.LC_RCP_CHK_DSB = L.LC_RCP_CHK_DSB,
			LN15.LD_CAN_RPT_GTR = L.LD_CAN_RPT_GTR,
			LN15.LD_DSB = L.LD_DSB,
			LN15.LD_DSB_RPT_GTR = L.LD_DSB_RPT_GTR,
			LN15.LF_DSB_CHK = L.LF_DSB_CHK,
			LN15.LC_DSB_TYP = L.LC_DSB_TYP,
			LN15.LD_DSB_CAN = L.LD_DSB_CAN,
			LN15.LI_LTE_DSB_APV = L.LI_LTE_DSB_APV,
			LN15.LF_RCP_DSB_CHK = L.LF_RCP_DSB_CHK,
			LN15.LC_LDR_DSB_MDM = L.LC_LDR_DSB_MDM,
			LN15.LI_RDS = L.LI_RDS,
			LN15.LD_DSB_ROS_PRT = L.LD_DSB_ROS_PRT,
			LN15.LR_DSB_ITR = L.LR_DSB_ITR,
			LN15.LD_STA_LON15 = L.LD_STA_LON15,
			LN15.LC_STA_LON15 = L.LC_STA_LON15,
			LN15.LD_EFT_FND_RLS = L.LD_EFT_FND_RLS,
			LN15.LD_EFT_RLS = L.LD_EFT_RLS,
			LN15.LF_LST_DTS_LN15 = L.LF_LST_DTS_LN15,
			LN15.LR_IRM_APR = L.LR_IRM_APR,
			LN15.LI_IDF_SIG = L.LI_IDF_SIG,
			LN15.AF_DSB_RPT = L.AF_DSB_RPT,
			LN15.LN_SEQ = L.LN_SEQ,
			LN15.AN_SEQ = L.AN_SEQ,
			LN15.IC_LON_PGM = L.IC_LON_PGM,
			LN15.LR_RPD_APR = L.LR_RPD_APR,
			LN15.LA_PCV_LDR_CHK = L.LA_PCV_LDR_CHK,
			LN15.LC_DSB_RLS_STA = L.LC_DSB_RLS_STA,
			LN15.LC_LON_EXT_ORG_SRC = L.LC_LON_EXT_ORG_SRC,
			LN15.LN_LON_DSB_SEQ = L.LN_LON_DSB_SEQ,
			LN15.LA_DSB_ORG_CHK_EFT = L.LA_DSB_ORG_CHK_EFT,
			LN15.LA_DSB_CAN_PCV = L.LA_DSB_CAN_PCV,
			LN15.LD_PRE_DSB_ROS_SNT = L.LD_PRE_DSB_ROS_SNT,
			LN15.LD_SCL_DSB_ATH_RCV = L.LD_SCL_DSB_ATH_RCV,
			LN15.LC_DSB_CAN_PCV = L.LC_DSB_CAN_PCV,
			LN15.LC_CMN_LN_HLD_RLS = L.LC_CMN_LN_HLD_RLS,
			LN15.LC_PAY_DSB_CAN_LTR = L.LC_PAY_DSB_CAN_LTR,
			LN15.LD_PAY_DSB_CAN_LTR = L.LD_PAY_DSB_CAN_LTR,
			LN15.LD_PRE_DSB_PRE_ROS = L.LD_PRE_DSB_PRE_ROS,
			LN15.LA_PRE_DSB_CAN = L.LA_PRE_DSB_CAN,
			LN15.LD_PRE_DSB_CAN = L.LD_PRE_DSB_CAN,
			LN15.LN_CMN_LN_DSB_SEQ = L.LN_CMN_LN_DSB_SEQ,
			LN15.AF_APL_ID = L.AF_APL_ID,
			LN15.LD_NTF_SCL_CDA = L.LD_NTF_SCL_CDA,
			LN15.LD_NTF_OWN = L.LD_NTF_OWN,
			LN15.LN_DSB_SUM_SEQ = L.LN_DSB_SUM_SEQ,
			LN15.LC_REU_REA = L.LC_REU_REA,
			LN15.IF_CDA = L.IF_CDA,
			LN15.LC_DSB_CAN_REA = L.LC_DSB_CAN_REA,
			LN15.LC_DSB_EVT_LN15 = L.LC_DSB_EVT_LN15,
			LN15.LF_LST_USR_LN15 = L.LF_LST_USR_LN15,
			LN15.LF_DSB_EVT_LN15 = L.LF_DSB_EVT_LN15,
			LN15.LD_DSB_RLS_STA = L.LD_DSB_RLS_STA,
			LN15.LA_DSB_RAL = L.LA_DSB_RAL,
			LN15.LD_DSB_RAL = L.LD_DSB_RAL,
			LN15.LC_DSB_TYP_RAL = L.LC_DSB_TYP_RAL,
			LN15.LC_DSB_SRC_RAL = L.LC_DSB_SRC_RAL,
			LN15.LC_LO_AUT_RAL = L.LC_LO_AUT_RAL,
			LN15.LC_DSB_IN_ERR = L.LC_DSB_IN_ERR,
			LN15.LI_DSB_DIR_BR = L.LI_DSB_DIR_BR,
			LN15.LC_DSB_DIR_BR_VER = L.LC_DSB_DIR_BR_VER,
			LN15.AN_LC_APL_SEQ = L.AN_LC_APL_SEQ,
			LN15.LA_DSB_CAN_PCV_RFD = L.LA_DSB_CAN_PCV_RFD,
			LN15.LA_DSB_CAN_RFD = L.LA_DSB_CAN_RFD,
			LN15.LD_DSB_CAN_RFD = L.LD_DSB_CAN_RFD,
			LN15.LD_DSB_RFD_RPT_GTR = L.LD_DSB_RFD_RPT_GTR,
			LN15.LF_ECA_LON_SCH_BCH = L.LF_ECA_LON_SCH_BCH,
			LN15.LF_ECA_CTD = L.LF_ECA_CTD,
			LN15.LD_ECA_LON_SCH_CRT = L.LD_ECA_LON_SCH_CRT,
			LN15.LD_ECA_DSB_FUD = L.LD_ECA_DSB_FUD,
			LN15.LC_DSB_ACH_OWN = L.LC_DSB_ACH_OWN,
			LN15.LC_DSB_ACH_SCL = L.LC_DSB_ACH_SCL,
			LN15.LA_DL_DSB_REB = L.LA_DL_DSB_REB,
			LN15.LA_DSB_REB_CAN = L.LA_DSB_REB_CAN,
			LN15.LA_PCV_DSB_REB_CAN = L.LA_PCV_DSB_REB_CAN,
			LN15.LC_CNL_DSB_CAN_REA = L.LC_CNL_DSB_CAN_REA,
			LN15.LA_PRE_DSB_CAN_RPT = L.LA_PRE_DSB_CAN_RPT,
			LN15.LD_PRE_DSB_CAN_RPT = L.LD_PRE_DSB_CAN_RPT,
			LN15.LD_DSB_ACP_FR_COD = L.LD_DSB_ACP_FR_COD,
			LN15.LN_DL_DSB_PAY_SEQ = L.LN_DL_DSB_PAY_SEQ,
			LN15.LF_DL_SCL_ENR_DSB = L.LF_DL_SCL_ENR_DSB,
			LN15.LD_COD_DSB_PST = L.LD_COD_DSB_PST,
			LN15.LC_CIP = L.LC_CIP,
			LN15.LC_DSB_ACP_MTD = L.LC_DSB_ACP_MTD
	WHEN NOT MATCHED THEN
		INSERT 
		(
			BF_SSN,
			LN_BR_DSB_SEQ,
			LA_DSB,
			LA_DSB_CAN,
			LC_DSB_CAN_TYP,
			LC_RCP_CHK_DSB,
			LD_CAN_RPT_GTR,
			LD_DSB,
			LD_DSB_RPT_GTR,
			LF_DSB_CHK,
			LC_DSB_TYP,
			LD_DSB_CAN,
			LI_LTE_DSB_APV,
			LF_RCP_DSB_CHK,
			LC_LDR_DSB_MDM,
			LI_RDS,
			LD_DSB_ROS_PRT,
			LR_DSB_ITR,
			LD_STA_LON15,
			LC_STA_LON15,
			LD_EFT_FND_RLS,
			LD_EFT_RLS,
			LF_LST_DTS_LN15,
			LR_IRM_APR,
			LI_IDF_SIG,
			AF_DSB_RPT,
			LN_SEQ,
			AN_SEQ,
			IC_LON_PGM,
			LR_RPD_APR,
			LA_PCV_LDR_CHK,
			LC_DSB_RLS_STA,
			LC_LON_EXT_ORG_SRC,
			LN_LON_DSB_SEQ,
			LA_DSB_ORG_CHK_EFT,
			LA_DSB_CAN_PCV,
			LD_PRE_DSB_ROS_SNT,
			LD_SCL_DSB_ATH_RCV,
			LC_DSB_CAN_PCV,
			LC_CMN_LN_HLD_RLS,
			LC_PAY_DSB_CAN_LTR,
			LD_PAY_DSB_CAN_LTR,
			LD_PRE_DSB_PRE_ROS,
			LA_PRE_DSB_CAN,
			LD_PRE_DSB_CAN,
			LN_CMN_LN_DSB_SEQ,
			AF_APL_ID,
			LD_NTF_SCL_CDA,
			LD_NTF_OWN,
			LN_DSB_SUM_SEQ,
			LC_REU_REA,
			IF_CDA,
			LC_DSB_CAN_REA,
			LC_DSB_EVT_LN15,
			LF_LST_USR_LN15,
			LF_DSB_EVT_LN15,
			LD_DSB_RLS_STA,
			LA_DSB_RAL,
			LD_DSB_RAL,
			LC_DSB_TYP_RAL,
			LC_DSB_SRC_RAL,
			LC_LO_AUT_RAL,
			LC_DSB_IN_ERR,
			LI_DSB_DIR_BR,
			LC_DSB_DIR_BR_VER,
			AN_LC_APL_SEQ,
			LA_DSB_CAN_PCV_RFD,
			LA_DSB_CAN_RFD,
			LD_DSB_CAN_RFD,
			LD_DSB_RFD_RPT_GTR,
			LF_ECA_LON_SCH_BCH,
			LF_ECA_CTD,
			LD_ECA_LON_SCH_CRT,
			LD_ECA_DSB_FUD,
			LC_DSB_ACH_OWN,
			LC_DSB_ACH_SCL,
			LA_DL_DSB_REB,
			LA_DSB_REB_CAN,
			LA_PCV_DSB_REB_CAN,
			LC_CNL_DSB_CAN_REA,
			LA_PRE_DSB_CAN_RPT,
			LD_PRE_DSB_CAN_RPT,
			LD_DSB_ACP_FR_COD,
			LN_DL_DSB_PAY_SEQ,
			LF_DL_SCL_ENR_DSB,
			LD_COD_DSB_PST,
			LC_CIP,
			LC_DSB_ACP_MTD
		)
		VALUES 
		(
			L.BF_SSN,
			L.LN_BR_DSB_SEQ,
			L.LA_DSB,
			L.LA_DSB_CAN,
			L.LC_DSB_CAN_TYP,
			L.LC_RCP_CHK_DSB,
			L.LD_CAN_RPT_GTR,
			L.LD_DSB,
			L.LD_DSB_RPT_GTR,
			L.LF_DSB_CHK,
			L.LC_DSB_TYP,
			L.LD_DSB_CAN,
			L.LI_LTE_DSB_APV,
			L.LF_RCP_DSB_CHK,
			L.LC_LDR_DSB_MDM,
			L.LI_RDS,
			L.LD_DSB_ROS_PRT,
			L.LR_DSB_ITR,
			L.LD_STA_LON15,
			L.LC_STA_LON15,
			L.LD_EFT_FND_RLS,
			L.LD_EFT_RLS,
			L.LF_LST_DTS_LN15,
			L.LR_IRM_APR,
			L.LI_IDF_SIG,
			L.AF_DSB_RPT,
			L.LN_SEQ,
			L.AN_SEQ,
			L.IC_LON_PGM,
			L.LR_RPD_APR,
			L.LA_PCV_LDR_CHK,
			L.LC_DSB_RLS_STA,
			L.LC_LON_EXT_ORG_SRC,
			L.LN_LON_DSB_SEQ,
			L.LA_DSB_ORG_CHK_EFT,
			L.LA_DSB_CAN_PCV,
			L.LD_PRE_DSB_ROS_SNT,
			L.LD_SCL_DSB_ATH_RCV,
			L.LC_DSB_CAN_PCV,
			L.LC_CMN_LN_HLD_RLS,
			L.LC_PAY_DSB_CAN_LTR,
			L.LD_PAY_DSB_CAN_LTR,
			L.LD_PRE_DSB_PRE_ROS,
			L.LA_PRE_DSB_CAN,
			L.LD_PRE_DSB_CAN,
			L.LN_CMN_LN_DSB_SEQ,
			L.AF_APL_ID,
			L.LD_NTF_SCL_CDA,
			L.LD_NTF_OWN,
			L.LN_DSB_SUM_SEQ,
			L.LC_REU_REA,
			L.IF_CDA,
			L.LC_DSB_CAN_REA,
			L.LC_DSB_EVT_LN15,
			L.LF_LST_USR_LN15,
			L.LF_DSB_EVT_LN15,
			L.LD_DSB_RLS_STA,
			L.LA_DSB_RAL,
			L.LD_DSB_RAL,
			L.LC_DSB_TYP_RAL,
			L.LC_DSB_SRC_RAL,
			L.LC_LO_AUT_RAL,
			L.LC_DSB_IN_ERR,
			L.LI_DSB_DIR_BR,
			L.LC_DSB_DIR_BR_VER,
			L.AN_LC_APL_SEQ,
			L.LA_DSB_CAN_PCV_RFD,
			L.LA_DSB_CAN_RFD,
			L.LD_DSB_CAN_RFD,
			L.LD_DSB_RFD_RPT_GTR,
			L.LF_ECA_LON_SCH_BCH,
			L.LF_ECA_CTD,
			L.LD_ECA_LON_SCH_CRT,
			L.LD_ECA_DSB_FUD,
			L.LC_DSB_ACH_OWN,
			L.LC_DSB_ACH_SCL,
			L.LA_DL_DSB_REB,
			L.LA_DSB_REB_CAN,
			L.LA_PCV_DSB_REB_CAN,
			L.LC_CNL_DSB_CAN_REA,
			L.LA_PRE_DSB_CAN_RPT,
			L.LD_PRE_DSB_CAN_RPT,
			L.LD_DSB_ACP_FR_COD,
			L.LN_DL_DSB_PAY_SEQ,
			L.LF_DL_SCL_ENR_DSB,
			L.LD_COD_DSB_PST,
			L.LC_CIP,
			L.LC_DSB_ACP_MTD
		)
	-- !!!  uncomment lines below ONLY when doing a full table refresh 
	--WHEN NOT MATCHED BY SOURCE THEN
	    --DELETE
	;
'
--select @SQLStatement
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
				PKUB.LN15_DSB LN15
		'	
	) R
	LEFT OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			LN15_DSB LN15
	) L 
		ON 1 = 1
	
IF @CountDifference != 0 AND @LoopCount > 0
	BEGIN
		RAISERROR('LN15_DSB - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
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
						LN15.BF_SSN,
						COUNT(*) AS "RemoteCount"
					FROM
						PKUB.LN15_DSB LN15
					GROUP BY
						LN15.BF_SSN
				'	
			) R
			FULL OUTER JOIN
			(
				SELECT
					LN15.BF_SSN,
					COUNT(*) [LocalCount]
				FROM
					CDW..LN15_DSB LN15
				GROUP BY
					LN15.BF_SSN
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

		PRINT 'The local record count for these SSNs does not match the remote warehouse count: ' + @SSNs + '  Deleting all local LN15 records for these borrowers fullying refreshing from the remote warehouse.'

		DELETE FROM
			LN15
		FROM
			CDW..LN15_DSB LN15
			INNER JOIN @SSN_LIST SL 
				ON COALESCE(SL.L_BF_SSN, SL.R_BF_SSN) = LN15.BF_SSN

		PRINT 'Loop Count:  ' + CAST(@LoopCount AS VARCHAR(2))

		GOTO RefreshStart;

	END
