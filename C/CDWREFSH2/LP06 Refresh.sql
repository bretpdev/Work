USE CDW
GO


DECLARE @LastRefresh VARCHAR(30) = (SELECT CONVERT(VARCHAR(30), ISNULL(dateadd(day, 1, MAX(PD_STA_LPD06)), '1-1-1900 00:00:00'), 21) FROM CDW..LP06_ITR_AND_TYP)
PRINT 'Last Refreshed at: ' + @LastRefresh

DECLARE @SQLStatement varchar(max) = 
'
MERGE 
		CDW.dbo.LP06_ITR_AND_TYP LOCAL
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
								PKUB.LP06_ITR_AND_TYP REMOTE
							WHERE
								REMOTE.PD_STA_LPD06 > ''''' + @LastRefresh + '''''
								FOR READ ONLY WITH UR
						''
					) 
			) R ON 
				R.IC_LON_PGM = LOCAL.IC_LON_PGM 
				AND R.IF_BND_ISS = LOCAL.IF_BND_ISS 
				AND R.IF_GTR = LOCAL.IF_GTR 
				AND R.IF_OWN = LOCAL.IF_OWN 
				AND R.PC_ITR_TYP = LOCAL.PC_ITR_TYP 
				AND R.PC_STA_LPD06 = LOCAL.PC_STA_LPD06 
				AND R.PD_EFF_SR_LPD06 = LOCAL.PD_EFF_SR_LPD06 
				AND R.PF_RGL_CAT = LOCAL.PF_RGL_CAT
			WHEN MATCHED THEN 
				UPDATE SET 
	LOCAL.PR_ITR_MAX = R.PR_ITR_MAX,
LOCAL.PR_ITR_MIN = R.PR_ITR_MIN,
LOCAL.PD_STA_LPD06 = R.PD_STA_LPD06,
LOCAL.PI_SPC_ITR = R.PI_SPC_ITR,
LOCAL.PD_EFF_END_LPD06 = R.PD_EFF_END_LPD06,
LOCAL.PF_ITR_CHG_NTF_LTR = R.PF_ITR_CHG_NTF_LTR,
LOCAL.PF_LST_USR_LP06 = R.PF_LST_USR_LP06,
LOCAL.PI_PRC_RTE_CHG = R.PI_PRC_RTE_CHG,
LOCAL.PF_LST_DTS_LP06 = R.PF_LST_DTS_LP06,
LOCAL.PF_BCH_LPD_CHG = R.PF_BCH_LPD_CHG,
LOCAL.PI_PRT_RTE_LTR = R.PI_PRT_RTE_LTR,
LOCAL.PR_TB_ITR_MGN = R.PR_TB_ITR_MGN,
LOCAL.PR_SCL_SUB = R.PR_SCL_SUB,
LOCAL.PC_ITR_CLC_LP06 = R.PC_ITR_CLC_LP06,
LOCAL.PR_ITR_CLC_LP06 = R.PR_ITR_CLC_LP06,
LOCAL.PC_ITR_TYP_LP06 = R.PC_ITR_TYP_LP06,
LOCAL.PC_ITR_MAL_IV = R.PC_ITR_MAL_IV,
LOCAL.PF_LTR_MIL_ITR_APV = R.PF_LTR_MIL_ITR_APV,
LOCAL.PF_LTR_MIL_ITR_EXP = R.PF_LTR_MIL_ITR_EXP,
LOCAL.PN_DAY_PIO_MIL_EXP = R.PN_DAY_PIO_MIL_EXP
			WHEN NOT MATCHED THEN
				INSERT 
				(
	PC_STA_LPD06,
IC_LON_PGM,
PF_RGL_CAT,
IF_GTR,
IF_OWN,
IF_BND_ISS,
PC_ITR_TYP,
PD_EFF_SR_LPD06,
PR_ITR_MAX,
PR_ITR_MIN,
PD_STA_LPD06,
PI_SPC_ITR,
PD_EFF_END_LPD06,
PF_ITR_CHG_NTF_LTR,
PF_LST_USR_LP06,
PI_PRC_RTE_CHG,
PF_LST_DTS_LP06,
PF_BCH_LPD_CHG,
PI_PRT_RTE_LTR,
PR_TB_ITR_MGN,
PR_SCL_SUB,
PC_ITR_CLC_LP06,
PR_ITR_CLC_LP06,
PC_ITR_TYP_LP06,
PC_ITR_MAL_IV,
PF_LTR_MIL_ITR_APV,
PF_LTR_MIL_ITR_EXP,
PN_DAY_PIO_MIL_EXP
				)
				VALUES 
				(
	R.PC_STA_LPD06,
R.IC_LON_PGM,
R.PF_RGL_CAT,
R.IF_GTR,
R.IF_OWN,
R.IF_BND_ISS,
R.PC_ITR_TYP,
R.PD_EFF_SR_LPD06,
R.PR_ITR_MAX,
R.PR_ITR_MIN,
R.PD_STA_LPD06,
R.PI_SPC_ITR,
R.PD_EFF_END_LPD06,
R.PF_ITR_CHG_NTF_LTR,
R.PF_LST_USR_LP06,
R.PI_PRC_RTE_CHG,
R.PF_LST_DTS_LP06,
R.PF_BCH_LPD_CHG,
R.PI_PRT_RTE_LTR,
R.PR_TB_ITR_MGN,
R.PR_SCL_SUB,
R.PC_ITR_CLC_LP06,
R.PR_ITR_CLC_LP06,
R.PC_ITR_TYP_LP06,
R.PC_ITR_MAL_IV,
R.PF_LTR_MIL_ITR_APV,
R.PF_LTR_MIL_ITR_EXP,
R.PN_DAY_PIO_MIL_EXP
				)
		;
';
EXEC (@SQLStatement)

DECLARE @CountDifference INT 

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
				PKUB.LP06_ITR_AND_TYP
		'
	) R
	FULL OUTER JOIN
	(
		SELECT
			COUNT(*) [LocalCount]
		FROM
			CDW.dbo.LP06_ITR_AND_TYP
	) L ON 1 = 1

IF(@CountDifference != 0)
BEGIN
	RAISERROR('LP06_ITR_AND_TYP - The remote and local record counts do not match.  The local count is off by %i records.  A full refresh of the table is required.', 16, 11, @CountDifference)
END