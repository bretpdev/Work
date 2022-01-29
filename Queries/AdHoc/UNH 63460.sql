--	CREATE TABLE NONSKIPS AS
SELECT 
	* 
FROM 
	OPENQUERY
	(
		DUSTER,
		'
			SELECT DISTINCT
				PD03.DF_PRS_ID
			FROM
				OLWHRM1.PD03_PRS_ADR_PHN PD03
				INNER JOIN OLWHRM1.CT30_CALL_QUE CT30
					ON PD03.DF_PRS_ID = CT30.DF_PRS_ID_BR
			WHERE
				CT30.IF_WRK_GRP IN (''MREFRADD'',''KLOANAPP'',''SKCOR'',''SKREF'',''SKSCH'')
				AND PD03.DI_VLD_ADR = ''Y''
				AND PD03.DI_PHN_VLD = ''Y''
		'
	)

--	CREATE TABLE PIFS AS
SELECT
	*
FROM
	OPENQUERY
		(
			DUSTER,
			'
				SELECT DISTINCT
					CT30.DF_PRS_ID_BR
				FROM
					OLWHRM1.CT30_CALL_QUE CT30
					LEFT JOIN
					(
						SELECT
							GA01.DF_PRS_ID_BR,
							GA14.AC_LON_STA_TYP
						FROM 
							OLWHRM1.GA01_APP GA01
							INNER JOIN OLWHRM1.GA14_LON_STA GA14
								ON GA01.AF_APL_ID = GA14.AF_APL_ID
								AND GA14.AC_LON_STA_TYP NOT IN (''PF'',''PN'')
								AND GA14.AC_STA_GA14 = ''A''
					) STA
						ON 	STA.DF_PRS_ID_BR = CT30.DF_PRS_ID_BR
				WHERE
					CT30.IF_WRK_GRP IN (''MREFRADD'',''KLOANAPP'',''SKCOR'',''SKREF'',''SKSCH'')
					AND STA.DF_PRS_ID_BR IS NULL
			'
		)


--	CREATE TABLE KNAMEDIS_PIF AS
SELECT
	*
FROM
	OPENQUERY
		(
			DUSTER,
			'
				SELECT DISTINCT
					CT30.DF_PRS_ID_BR
				FROM
					OLWHRM1.CT30_CALL_QUE CT30
					LEFT JOIN
						(
							SELECT
								GA01.DF_PRS_ID_BR,
								GA14.AC_LON_STA_TYP
							FROM 
								OLWHRM1.GA01_APP GA01
								INNER JOIN OLWHRM1.GA14_LON_STA GA14
									ON GA01.AF_APL_ID = GA14.AF_APL_ID
									AND GA14.AC_LON_STA_TYP NOT IN (''PF'',''PN'')
									AND GA14.AC_STA_GA14 = ''A''
						) STA
						ON 	STA.DF_PRS_ID_BR = CT30.DF_PRS_ID_BR
				WHERE
					CT30.IF_WRK_GRP IN (''KNAMEDIS'')
					AND STA.DF_PRS_ID_BR IS NULL
			'
		)


--	CREATE TABLE KNAMEDIS_NO_CONFLICT AS
SELECT
	*
FROM
	OPENQUERY
		(
			DUSTER,
			'
				SELECT DISTINCT
					PD01.DF_PRS_ID
				FROM
					OLWHRM1.PD01_PDM_INF PD01
					INNER JOIN OLWHRM1.PD10_PRS_NME PD10
						ON PD01.DF_PRS_ID = PD10.DF_PRS_ID
					INNER JOIN OLWHRM1.CT30_CALL_QUE CT30
						ON PD01.DF_PRS_ID = CT30.DF_PRS_ID_BR
				WHERE
					CT30.IF_WRK_GRP IN (''KNAMEDIS'')
					AND PD01.DM_PRS_1 = PD10.DM_PRS_1
					AND PD01.DM_PRS_MID = PD10.DM_PRS_MID
					AND PD01.DM_PRS_LST = PD10.DM_PRS_LST
			'
		)

--	CREATE TABLE KNAMEDIS_MI_CONFLICT AS
SELECT
	*
FROM
	OPENQUERY
		(
			DUSTER,
			'SELECT DISTINCT
				PD01.DF_PRS_ID
			FROM
				OLWHRM1.PD01_PDM_INF PD01
				INNER JOIN OLWHRM1.PD10_PRS_NME PD10
					ON PD01.DF_PRS_ID = PD10.DF_PRS_ID
				INNER JOIN OLWHRM1.CT30_CALL_QUE CT30
					ON PD01.DF_PRS_ID = CT30.DF_PRS_ID_BR
			WHERE
				CT30.IF_WRK_GRP IN (''KNAMEDIS'')
				AND PD01.DM_PRS_1 = PD10.DM_PRS_1
				AND PD01.DM_PRS_MID <> PD10.DM_PRS_MID
				AND PD01.DM_PRS_LST = PD10.DM_PRS_LST
			'
		)