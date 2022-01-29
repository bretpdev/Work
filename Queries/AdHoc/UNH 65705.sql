USE UDW
GO

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @SchoolCodes TABLE (SchoolCode CHAR(8),	SchoolCloseDate DATE);
INSERT INTO	@SchoolCodes (SchoolCode, SchoolCloseDate) VALUES
--OPE ID	Closure Date
('00756010','12/31/2019'),
('00270401','8/10/2019 '),
('00270403','8/10/2019 '),
('00270404','8/10/2019 '),
('00270405','8/10/2019 '),
('00270406','8/10/2019 '),
('01254401','6/30/2018 '),
('00239714','10/31/2019'),
('72098866','12/18/2019'),
('00798701','2/8/2019  '),
('00341802','12/12/2018'),
('00343608','7/23/2018 '),
('10174145','5/31/2017 '),
('10174186','10/18/2017'),
('20174147','12/12/2017'),
('20174164','5/8/2018  '),
('30174110','4/5/2017  '),
('30174116','4/5/2017  '),
('30174117','8/5/2017  '),
('30174121','12/13/2018'),
('30174123','4/18/2018 '),
('30174124','6/14/2018 '),
('00174177','4/5/2018  '),
('00174183','12/14/2017'),
('00174188','4/23/2019 '),
('00180302','12/31/2017'),
('00234311','12/22/2015'),
('00302900','1/20/2020 '),
('00304606','12/18/2019'),
('00304613','8/29/2018 '),
('01303922','12/19/2019'),
('00239710','10/31/2019'),
('00365204','5/8/2017  '),
('00172269','12/29/2018'),
('00251801','12/14/2012'),
('00251803','5/17/2019 '),
('00251804','7/28/2017 '),
('00239701','10/31/2019'),
('02098869','11/21/2019'),
('00224946','12/31/2019'),
('00228400','12/17/2019'),
('00407247','12/23/2019'),
('00407248','12/23/2019'),
('00407282','12/23/2019'),
('00230722','12/13/2016'),
('00230723','12/13/2016'),
('00230731','12/14/2013'),
('00230737','12/15/2015'),
('02098860','11/21/2019'),
('32098839','11/26/2019'),
('04051300','12/13/2019')

;
--select * from @SchoolCodes

/**** UNH 23008 query **/

SELECT DISTINCT
	LN10.BF_SSN,
	LN10.LN_SEQ,
	LN10.LF_DOE_SCL_ORG
FROM
	LN10_LON LN10
WHERE
	LN10.LF_DOE_SCL_ORG IN (SELECT SchoolCode FROM @SchoolCodes)
;

/**** digging deeper ****/
select * from SD10_STU_SPR where LF_STU_SSN = '298821419'
select * from LN10_LON where LF_DOE_SCL_ORG = '00228400'

SELECT DISTINCT
	LN10.BF_SSN,
	--LN10.LN_SEQ,
	LN10.LF_DOE_SCL_ORG,
	SD10.LD_SCL_SPR
FROM
	LN10_LON LN10
	INNER JOIN SD10_STU_SPR SD10 
		ON SD10.LF_STU_SSN = LN10.BF_SSN 
	LEFT JOIN #SCHOOLS SCHOOLS
		ON LN10.BF_SSN = SCHOOLS.LF_STU_SSN
WHERE
	LN10.LA_CUR_PRI > 0.00
	AND LN10.LC_STA_LON10 = 'R'
	AND (
			SD10.LF_DOE_SCL_ENR_CUR IN (SELECT SchoolCode FROM @SchoolCodes)
			OR LN10.LF_DOE_SCL_ORG IN (SELECT SchoolCode FROM @SchoolCodes)
		)
;


SELECT * INTO #SCHOOLS FROM OPENQUERY (DUSTER,
'
	SELECT 
		LF_STU_SSN,
		LF_DOE_SCL_ORG,
		LF_DOE_SCL_ENR_CUR
	FROM
		OLWHRM1.MR01_MGT_RPT_LON
') DUST 
WHERE
	DUST.LF_DOE_SCL_ORG IN (SELECT SchoolCode FROM @SchoolCodes)
	OR DUST.LF_DOE_SCL_ENR_CUR IN (SELECT SchoolCode FROM @SchoolCodes)
;
--select * from #SCHOOLS



SELECT DISTINCT
	LN10.BF_SSN,
	--LN10.LN_SEQ,
	LN10.LF_DOE_SCL_ORG,
	SCHOOLS.LF_DOE_SCL_ORG,
	SCHOOLS.LF_DOE_SCL_ENR_CUR,
	SD10.LD_SCL_SPR	,
	SchoolCodes.SchoolCloseDate
FROM
	LN10_LON LN10
	INNER JOIN #SCHOOLS SCHOOLS
		ON LN10.BF_SSN = SCHOOLS.LF_STU_SSN
	LEFT JOIN SD10_STU_SPR SD10 
		ON SD10.LF_STU_SSN = LN10.BF_SSN 
	LEFT JOIN @SchoolCodes SchoolCodes
		ON SCHOOLS.LF_DOE_SCL_ORG = SchoolCodes.SchoolCode
		OR SCHOOLS.LF_DOE_SCL_ENR_CUR = SchoolCodes.SchoolCode
WHERE
	LN10.LA_CUR_PRI > 0.00
	AND LN10.LC_STA_LON10 = 'R'
--	AND DATEADD(DAY, 121, ISNULL(SD10.LD_SCL_SPR, GETDATE())) > SchoolCodes.SchoolCloseDate
ORDER BY
	BF_SSN
;


/*****************  MODIFIED QUERY  ***********************/
SELECT
	*
FROM
	(
		SELECT DISTINCT
			PD10.DF_SPE_ACC_ID,
			PD10.DM_PRS_1,
			PD10.DM_PRS_MID,
			PD10.DM_PRS_LST,
			PD30.DX_STR_ADR_1,
			PD30.DX_STR_ADR_2,
			PD30.DX_STR_ADR_3,
			PD30.DM_CT,
			PD30.DC_DOM_ST,
			PD30.DF_ZIP_CDE,
			PD30.DI_VLD_ADR,
			LN10.LF_DOE_SCL_ORG,
			SD10.LF_DOE_SCL_ENR_CUR,
			SD10.LC_REA_SCL_SPR,
			SD10.LD_NTF_SCL_SPR,
			SD10.LD_SCL_SPR [LastDayOfEnrollment],
			School.SchoolCloseDate
			--,SUM(LN10.LA_CUR_PRI) OVER(PARTITION BY LN10.BF_SSN) AS sum_LA_CUR_PRI
			--,SUM(LN10.LA_LON_AMT_GTR) OVER(PARTITION BY LN10.BF_SSN) AS sum_LA_LON_AMT_GTR
		FROM
			PD10_PRS_NME PD10
			INNER JOIN PD30_PRS_ADR PD30 
				ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
			INNER JOIN 
			(
				SELECT
					LN10.BF_SSN,
					SC.SchoolCode,
					SC.SchoolCloseDate
				FROM
					@SchoolCodes SC
					INNER JOIN LN10_LON LN10 
						ON LN10.LF_DOE_SCL_ORG = SC.SchoolCode

				UNION ALL

				SELECT DISTINCT
					SD10.LF_STU_SSN [BF_SSN],
					SC.SchoolCode,
					SC.SchoolCloseDate
				FROM
					@SchoolCodes SC
					INNER JOIN SD10_STU_SPR SD10 
						ON SD10.LF_DOE_SCL_ENR_CUR = SC.SchoolCode
				WHERE
					SD10.LC_STA_STU10 = 'A'
			) School 
				ON School.BF_SSN = PD10.DF_PRS_ID
			INNER JOIN LN10_LON LN10 
				ON LN10.BF_SSN = PD10.DF_PRS_ID
			LEFT JOIN SD10_STU_SPR SD10 
				ON SD10.LF_STU_SSN = PD10.DF_PRS_ID 
				AND SD10.LF_DOE_SCL_ENR_CUR = School.SchoolCode 
				AND SD10.LC_STA_STU10 = 'A'
		WHERE
			DATEADD(DAY, 121, SD10.LD_SCL_SPR) > School.SchoolCloseDate
			AND LN10.LA_CUR_PRI > 0.00
			AND LN10.LA_LON_AMT_GTR > 0.00
	) X
WHERE
	DATEADD(DAY, 121, ISNULL(X.LastDayOfEnrollment, GETDATE())) > X.SchoolCloseDate
	AND	(
			X.LF_DOE_SCL_ENR_CUR IN (SELECT SchoolCode FROM @SchoolCodes)
			OR	X.LF_DOE_SCL_ORG IN (SELECT SchoolCode FROM @SchoolCodes)
		)
ORDER BY
	X.DF_SPE_ACC_ID
;
