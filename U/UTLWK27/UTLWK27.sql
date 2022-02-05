
DECLARE @Queue VARCHAR(8) = 'KNAMEDIS'
DECLARE @SasId VARCHAR(25) = 'UTLWK27R2'
DECLARE @Today DATETIME = GETDATE()

INSERT INTO OLS.olqtskbldr.Queues(TargetId, QueueName, InstitutionId, InstitutionType, DateDue, TimeDue, Comment, SourceFilename, AddedAt, AddedBy)
SELECT DISTINCT
	NamesPop.DF_PRS_ID AS TargetId,
	@Queue AS QueueName,
	NULL AS InstitutionId,
	NULL AS InstitutionType,
	NULL AS DateDue,
	NULL AS TimeDue,
	CONCAT('OL = ', NamesPop.ONE, ', CO = ', NamesPop.COM) AS Comment,
	NULL AS SourceFileName,
	@Today AS AddedAt, 
	@SasId AS AddedBy
FROM
(
	SELECT DISTINCT
			PD10.DF_PRS_ID,
			CONCAT(RTRIM(PD01.DM_PRS_1), ' ', RTRIM(PD01.DM_PRS_MID), ' ', RTRIM(PD01.DM_PRS_LST)) AS ONE,
			CONCAT(RTRIM(PD10.DM_PRS_1), ' ', RTRIM(PD10.DM_PRS_MID), ' ', RTRIM(PD10.DM_PRS_LST), ' ', RTRIM(PD10.DM_PRS_LST_SFX)) AS COM
		FROM
			UDW..PD10_PRS_NME PD10
			INNER JOIN ODW..PD01_PDM_INF PD01
				ON PD01.DF_PRS_ID = PD10.DF_PRS_ID
			INNER JOIN UDW..LN10_LON LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
			INNER JOIN ODW..GA10_LON_APP GA10
				ON GA10.AF_APL_ID = LN10.LF_LON_ALT
				AND GA10.AF_APL_ID_SFX = RIGHT('00' + CAST(LN10.LN_LON_ALT_SEQ AS VARCHAR(2)),2)
			LEFT JOIN ODW..CT30_CALL_QUE CT30
				ON CT30.DF_PRS_ID_BR = PD10.DF_PRS_ID
				AND CT30.IF_WRK_GRP = @Queue
				AND CT30.IC_TSK_STA IN ('A','W')
			LEFT JOIN OLS.olqtskbldr.Queues ExistingData
				ON ExistingData.TargetId = PD10.DF_PRS_ID
				AND ExistingData.QueueName = @Queue
				AND
				(
					CAST(ExistingData.AddedAt AS DATE) = @TODAY
					OR ExistingData.ProcessedAt IS NULL
				)
				AND ExistingData.DeletedAt IS NULL
				AND ExistingData.DeletedBy IS NULL
		WHERE
			CT30.DF_PRS_ID_BR IS NULL --Filter out accounts that already have the queue task on them, if any exist
			AND ExistingData.TargetId IS NULL --Filter out matching, unprocessed tasks for OLQTSKBLDR, if any exist
			AND CONCAT(RTRIM(PD01.DM_PRS_1), ' ', RTRIM(PD01.DM_PRS_MID), ' ', RTRIM(PD01.DM_PRS_LST)) != CONCAT(RTRIM(PD10.DM_PRS_1), ' ', RTRIM(PD10.DM_PRS_MID), ' ', RTRIM(PD10.DM_PRS_LST), ' ', RTRIM(PD10.DM_PRS_LST_SFX))
			AND
			( 
				(
					LN10.LC_STA_LON10 = 'R' 
					AND LN10.LA_CUR_PRI > 0.00
				)
				OR
				(
					LN10.LC_STA_LON10 = 'D'
					AND LN10.LA_CUR_PRI = 0.00
					AND GA10.AC_PRC_STA = 'A'
					AND GA10.AD_PRC >= CAST(DATEADD(DAY, -3, @Today) AS DATE)
				)
			)
) NamesPop
ORDER BY
	NamesPop.DF_PRS_ID