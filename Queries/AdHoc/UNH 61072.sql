USE UDW
GO

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @SchoolCodes TABLE (SchoolCode CHAR(8),	SchoolCloseDate DATE);
INSERT INTO	@SchoolCodes (SchoolCode, SchoolCloseDate) VALUES
--OPE ID	Closure Date
('00747000','3/8/2019  '),
('02179903','3/8/2019  '),
('02179909','3/8/2019  '),
('02151918','8/31/2018 '),
('00152643','8/19/2018 '),
('04106301','4/30/2018 '),
('00152666','10/17/2016'),
('02179902','3/8/2019  '),
('02179951','3/8/2019  '),
('02158414','9/16/2018 '),
('02179936','3/8/2019  '),
('02179901','3/8/2019  '),
('01016800','8/31/2018 '),
('00531101','6/30/1986 '),
('02179919','3/8/2019  '),
('00186002','10/1/2011 '),
('00186017','12/8/2013 '),
('00186028','4/26/2014 '),
('00186029','12/8/2013 '),
('00186034','3/1/2019  '),
('00826001','12/31/2018'),
('00224948','12/31/2018'),
('00227901','7/23/2009 '),
('00227906','7/25/2013 '),
('00227908','7/25/2013 '),
('00227909','7/25/2009 '),
('00227910','5/7/2004  '),
('00227911','12/14/2017'),
('00227912','5/8/2015  '),
('00227913','5/9/2014  '),
('02179935','3/8/2019  '),
('02179949','3/8/2019  '),
('02179907','3/8/2019  '),
('02179950','3/8/2019  '),
('02179900','3/8/2019  '),
('02179908','3/8/2019  '),
('02179928','3/8/2019  '),
('02179938','3/8/2019  '),
('02179940','3/8/2019  '),
('02179947','3/8/2019  '),
('03516303','7/23/2018 '),
('04156100','3/28/2019 '),
('04221800','3/2/2019  '),
('02179905','3/8/2019  '),
('02179910','3/8/2019  '),
('02179911','3/8/2019  '),
('02179913','3/8/2019  '),
('02291300','3/8/2019  '),
('02291302','3/8/2019  '),
('02310502','8/31/2011 ')
;

--select * from @SchoolCodes



/**** UNH 23008 query **/

--SELECT DISTINCT
--	LN10.BF_SSN,
--	LN10.LN_SEQ,
--	LN10.LF_DOE_SCL_ORG
--FROM
--	LN10_LON LN10
--WHERE
--	LN10.LF_DOE_SCL_ORG IN (SELECT SchoolCode FROM @SchoolCodes)
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
			--AND LN10.LA_CUR_PRI > 0.00
			--AND LN10.LA_LON_AMT_GTR > 0.00
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

