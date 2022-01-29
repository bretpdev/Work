USE UDW;
GO

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @SchoolCodes TABLE (SchoolCode CHAR(8),	SchoolCloseDate DATE);
INSERT INTO	@SchoolCodes (SchoolCode, SchoolCloseDate) VALUES
--OPE ID	Closure Date
('04195100','3/6/2020  '),
('00372604','1/26/2018 '),
('00372601','1/26/2018 '),
('00372606','1/26/2018 '),
('00372603','1/26/2018 '),
('00211900','7/31/2011 '),
('04226600','2/24/2020 '),
('00165409','12/21/2018'),
('02617000','11/30/2019'),
('04215300','2/7/2020  '),
('03682401','2/5/2020  '),
('03682403','2/5/2020  '),
('04167602','9/20/2019 '),
('04167601','9/20/2019 '),
('00215013','3/31/2020 '),
('00109804','12/13/2019'),
('00717103','7/1/2019  '),
('00350213','5/7/2015  '),
('00350214','12/12/2014'),
('02100300','4/5/2019  '),
('00405720','2/28/2019 '),
('00405723','11/30/2019'),
('00405718','11/30/2019'),
('00405707','5/31/2019 '),
('00405722','5/31/2019 '),
('00405721','8/31/2019 '),
('10173310','3/4/2020  '),
('20173300','3/4/2020  '),
('20173326','3/4/2020  '),
('02166400','4/25/2019 '),
('01300300','1/21/2020 '),
('00407260','3/2/2020  '),
('00303016','2/26/2020 '),
('00249874','11/13/2019'),
('00249878','11/13/2019'),
('00249870','11/13/2019'),
('00249879','11/13/2019'),
('00249873','11/13/2019'),
('00249883','11/13/2019'),
('00249880','11/13/2019'),
('04136500','12/20/2019'),
('00174615','2/22/2020 '),
('00174617','2/22/2020 '),
('01219801','5/31/2019 '),
('00394103','12/31/2018'),
('00205303','5/8/2015  '),
('02503903','2/27/2020 ')
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

SELECT * 
--INTO #SCHOOLS
FROM OPENQUERY (DUSTER,
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



/**** digging deeper ****/
--select * from SD10_STU_SPR where LF_STU_SSN = '298821419'
--select * from LN10_LON where LF_DOE_SCL_ORG = '00228400'

--SELECT DISTINCT
--	LN10.BF_SSN,
--	--LN10.LN_SEQ,
--	LN10.LF_DOE_SCL_ORG,
--	SD10.LD_SCL_SPR
--FROM
--	LN10_LON LN10
--	INNER JOIN SD10_STU_SPR SD10 
--		ON SD10.LF_STU_SSN = LN10.BF_SSN 
--	LEFT JOIN #SCHOOLS SCHOOLS
--		ON LN10.BF_SSN = SCHOOLS.LF_STU_SSN
--WHERE
--	LN10.LA_CUR_PRI > 0.00
--	AND LN10.LC_STA_LON10 = 'R'
--	AND (
--			SD10.LF_DOE_SCL_ENR_CUR IN (SELECT SchoolCode FROM @SchoolCodes)
--			OR LN10.LF_DOE_SCL_ORG IN (SELECT SchoolCode FROM @SchoolCodes)
--		)
--;

--SELECT DISTINCT
--	LN10.BF_SSN,
--	--LN10.LN_SEQ,
--	LN10.LF_DOE_SCL_ORG,
--	SCHOOLS.LF_DOE_SCL_ORG,
--	SCHOOLS.LF_DOE_SCL_ENR_CUR,
--	SD10.LD_SCL_SPR	,
--	SchoolCodes.SchoolCloseDate
--FROM
--	LN10_LON LN10
--	INNER JOIN #SCHOOLS SCHOOLS
--		ON LN10.BF_SSN = SCHOOLS.LF_STU_SSN
--	LEFT JOIN SD10_STU_SPR SD10 
--		ON SD10.LF_STU_SSN = LN10.BF_SSN 
--	LEFT JOIN @SchoolCodes SchoolCodes
--		ON SCHOOLS.LF_DOE_SCL_ORG = SchoolCodes.SchoolCode
--		OR SCHOOLS.LF_DOE_SCL_ENR_CUR = SchoolCodes.SchoolCode
--WHERE
--	LN10.LA_CUR_PRI > 0.00
--	AND LN10.LC_STA_LON10 = 'R'
----	AND DATEADD(DAY, 121, ISNULL(SD10.LD_SCL_SPR, GETDATE())) > SchoolCodes.SchoolCloseDate
--ORDER BY
--	BF_SSN
--;


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
