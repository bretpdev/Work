DECLARE @QUE_TASK VARCHAR(8) = 'KFRGNADD'
DECLARE @BeginningOfTime DATE = '1900-01-01';
DECLARE @TODAY DATETIME = GETDATE()

INSERT INTO OLS.olqtskbldr.Queues(TargetId, QueueName, InstitutionId, InstitutionType, DateDue, TimeDue, Comment, AddedAt, AddedBy)
SELECT DISTINCT
	PD03.DF_PRS_ID, --TargetId
	@QUE_TASK, --QueueName
	NULL, --InstitutionId
	NULL, --InstitutionType
	NULL, --DateDue
	NULL, --TimeDue
	RTRIM(CONCAT(PD03.DF_PRS_ID, ' - Review Foreign Address Demographics for Hygiene Country ',PD03.DM_FGN_CNY)), --Comment
	@TODAY, --AddedAt
	USER_NAME() --AddedBy
FROM
	ODW..PD01_PDM_INF PD01
	INNER JOIN ODW..GA01_APP GA01
		ON GA01.DF_PRS_ID_BR = PD01.DF_PRS_ID
	INNER JOIN ODW..GA10_LON_APP GA10
		ON GA01.AF_APL_ID = GA10.AF_APL_ID
	INNER JOIN ODW..DC01_LON_CLM_INF DC01
		ON DC01.AF_APL_ID = GA10.AF_APL_ID
		AND DC01.AF_APL_ID_SFX = GA10.AF_APL_ID_SFX
--PD03: date of most recent foreign address update for each borrower/country
	INNER JOIN
	(
		SELECT
			PD03.DF_PRS_ID,
			PD03.DM_FGN_CNY,
			MAX(PD03.DD_LST_UPD_ADR) AS MX_UPDT
		FROM
			ODW..PD03_PRS_ADR_PHN PD03
		WHERE
			PD03.DI_VLD_ADR = 'Y'
			AND
			(
				PD03.DC_DOM_ST = 'FC'
				OR ISNULL(PD03.DM_FGN_CNY,'') != '' 
			)
		GROUP BY
			PD03.DF_PRS_ID,
			PD03.DM_FGN_CNY
	) PD03
		ON PD03.DF_PRS_ID = PD01.DF_PRS_ID
--AY01: date of most recnet MFRGN activity record for each borrower
	LEFT JOIN 
	(
		SELECT
			AY01.DF_PRS_ID,
			MAX(AY01.BD_ATY_PRF) AS MFRGN_MX_DT
		FROM
			ODW..AY01_BR_ATY AY01
		WHERE
			AY01.PF_ACT = 'MFRGN'
			--Doesnt look like this table has active flags
		GROUP BY
			AY01.DF_PRS_ID
	) AY01 
		ON AY01.DF_PRS_ID = PD03.DF_PRS_ID		
	LEFT JOIN ODW..CT30_CALL_QUE CT30 --borrower with a 'KFRGNADD' queue task in OneLINK to be excluded
		ON CT30.DF_PRS_ID_BR = PD03.DF_PRS_ID
		AND CT30.IF_WRK_GRP = @QUE_TASK
		AND CT30.IC_TSK_STA IN ('A','W') --task is open
WHERE
	ISNULL(AY01.MFRGN_MX_DT,@BeginningOfTime) < PD03.MX_UPDT --No arc after most recent address change
	AND CT30.DF_PRS_ID_BR IS NULL --no open queue tasks
	AND GA10.AA_CUR_PRI > 0.00 --has a balance
	AND RTRIM(DC01.LC_REA_CLM_ASN_DOE) = '' --not subrogated