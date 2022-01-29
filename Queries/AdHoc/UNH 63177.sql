USE UDW
GO

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @SchoolCodes TABLE (SchoolCode CHAR(8),	SchoolCloseDate DATE);
INSERT INTO	@SchoolCodes (SchoolCode, SchoolCloseDate) VALUES
--OPE ID	Closure Date
('00368700','7/15/2019'),
('00146910','5/1/2019'),
('00146916','5/1/2019'),
('00270400','8/10/2019'),
('00146908','12/15/2017'),
('00146923','10/31/2017'),
('00456800','8/16/2019'),
('00456801','1/15/1995'),
('00173709','12/10/2016'),
('00405708','4/21/2019'),
('00405736','12/16/2018'),
('00110001','8/16/2019'),
('01201545','5/31/2019'),
('01114521','10/16/2017'),
('04231202','6/11/2019'),
('03400301','2/14/2016'),
('00184715','3/1/2018'),
('00527302','5/8/2016'),
('00405704','5/31/2019'),
('00349613','8/18/2017'),
('00889619','6/30/2019'),
('00226031','5/9/2014'),
('03778300','6/30/2019'),
('03017800','4/5/2019')
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
