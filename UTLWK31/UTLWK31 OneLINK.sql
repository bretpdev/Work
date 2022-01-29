USE ODW
GO

DECLARE @QUEUE VARCHAR(8) = 'KFRGNADD'
DECLARE @PFACT VARCHAR(5) = 'MFRGN'
DECLARE @SOURCEFILE VARCHAR(50) = 'ULWK31.LWK31R2'
DECLARE @TODAY DATETIME = GETDATE()

INSERT INTO OLS.olqtskbldr.Queues(TargetId, QueueName, InstitutionId, InstitutionType, DateDue, TimeDue, Comment, SourceFilename, AddedAt, AddedBy)
SELECT DISTINCT
	BR03.DF_PRS_ID_BR --TargetId
	,@QUEUE --QueueName
	,NULL --InstitutionId
	,NULL --InstitutionType
	,NULL --DateDue
	,NULL --TimeDue
	,CONCAT(CAST(BR03.DF_PRS_ID_RFR AS VARCHAR(10)), ' - Review Foreign Address Demographics for Hygiene') --Comment
	,@SOURCEFILE --SourceFilename
	,@TODAY --AddedAt
	,USER_NAME() --AddedBy
FROM 
	ODW..PD01_PDM_INF PD01
	INNER JOIN ODW..BR03_BR_REF BR03
		ON BR03.DF_PRS_ID_BR = PD01.DF_PRS_ID
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.BF_SSN = PD01.DF_PRS_ID
	INNER JOIN ODW..GA10_LON_APP GA10
		ON GA10.AF_APL_ID = LN10.LF_LON_ALT
		AND GA10.AF_APL_ID_SFX = RIGHT('00' + CAST(LN10.LN_LON_ALT_SEQ AS VARCHAR(2)),2)
	INNER JOIN ODW..GA14_LON_STA GA14
		ON GA14.AF_APL_ID = GA10.AF_APL_ID
		AND GA14.AF_APL_ID_SFX = GA10.AF_APL_ID_SFX
	LEFT JOIN ODW..LK01_LGS_CDE_LKP LK01
		ON LK01.PM_ATR = 'BC-HME-CNC-RSL'
		AND LK01.PX_CDE_VAL = BR03.BC_HME_CNC_RSL
	LEFT JOIN
	(--exclude if address hasn't been updated since last MFRGN
		SELECT
			DF_PRS_ID,
			MAX(BD_ATY_PRF) AS max_BD_ATY_PRF
		FROM
			ODW..AY01_BR_ATY
		WHERE
			PF_ACT = @PFACT
		GROUP BY
			DF_PRS_ID
	) AY01
		ON AY01.DF_PRS_ID = PD01.DF_PRS_ID
		AND COALESCE(AY01.max_BD_ATY_PRF, CONVERT(DATE,'19000101')) >= COALESCE(BR03.BD_ADR_DAT_EFF, CONVERT(DATE,'19000102')) --reference address last update date
	LEFT JOIN ODW..CT30_CALL_QUE CT30  --exclude if does not have the KFRGNADD queue for the associated reference
		ON CT30.DF_PRS_ID_BR = LN10.BF_SSN
		AND CT30.IF_WRK_GRP = @QUEUE
		AND CT30.IC_TSK_STA IN ('A','W')
		AND CT30.DX_EVT_CMT_QUE LIKE CONCAT(BR03.DF_PRS_ID_RFR,'%')
	LEFT JOIN OLS.olqtskbldr.Queues Q
		ON Q.TargetId = LN10.BF_SSN
		AND Q.QueueName = @QUEUE
		AND Q.ProcessedAt IS NULL
		AND Q.DeletedAt IS NULL
		AND CAST(Q.AddedAt AS DATE) > CentralData.dbo.AddBusinessDays(@TODAY, -5) --Gives 5 days for any errors to be fixed before adding a dupe record
WHERE 
	BR03.BC_RFR_ST = 'FC'
	AND AY01.DF_PRS_ID IS NULL --exclude if address hasn't been updated since last MFRGN
	AND CT30.DF_PRS_ID_BR IS NULL --does not have the KFRGNADD queue for the associated reference
	AND Q.TargetId IS NULL --Check if the record is active in the Queues table so there are no dups
	AND BR03.BC_STA_BR03 = 'A' --active
	AND BR03.BI_VLD_ADR = 'Y'
	AND GA10.AA_CUR_PRI > 0.00
	AND GA14.AC_LON_STA_TYP IN ('CP','CR','DA','FB','IA','ID','IG','IM','RF','RP','UA','UB')
	AND GA14.AC_STA_GA14 = 'A' --active
	AND LN10.LC_STA_LON10 = 'R'
	AND LN10.LA_CUR_PRI > 0.00
	AND (LK01.PX_CDE_VAL IN ('01','02','03') OR RTRIM(BR03.BC_HME_CNC_RSL) = '')