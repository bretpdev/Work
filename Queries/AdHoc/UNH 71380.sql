USE UDW
GO

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @SchoolCodes TABLE
(
	SchoolCode CHAR(8),
	SchoolCloseDate DATE
);

INSERT INTO
	@SchoolCodes
VALUES
--OPE ID	Closure Date
('03205400','4/21/2021'),
('02254000','4/23/2021'),
('02254002','1/25/1986'),
('02254004','1/25/1986'),
('02254005','1/25/1986'),
('02254006','4/5/2008'),
('02118000','3/30/2021'),
('00294902','6/28/2020'),
('00294903','6/28/2020'),
('00294905','6/28/2020'),
('00294911','6/28/2020'),
('00294913','6/28/2020'),
('00343807','11/1/2019'),
('20182262','12/7/2009'),
('40182259','11/17/2019'),
('00366314','12/19/2020'),
('00366339','12/19/2020'),
('00366315','10/31/2015'),
('00357309','5/11/2017'),
('02527501','6/19/2020'),
('00355806','5/12/2012'),
('00366306','3/2/2020'),
('00366326','8/10/2019'),
('00366333','5/16/2020'),
('30182286','2/23/2015'),
('30182289','3/7/2012'),
('40182263','7/31/2006'),
('00542405','12/18/2020'),
('00254102','6/6/2018'),
('00254103','3/12/2020'),
('00254104','6/11/2018'),
('00254105','12/15/2017'),
('00254106','12/15/2017'),
('00535906','6/30/2018'),
('00535907','6/30/2018'),
('00528304','4/23/2021'),
('00226023','8/12/2020'),
('02254007','8/21/2008')

--select * from @SchoolCodes

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
			SD10.LD_NTF_SCL_SPR,
			SD10.LC_REA_SCL_SPR,
			SD10.LD_SCL_SPR [LastDayOfEnrollment],
			School.SchoolCloseDate
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

				UNION

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
		--	DATEADD(DAY, 121, SD10.LD_SCL_SPR) > School.SchoolCloseDate
			 LN10.LA_CUR_PRI > 0.00
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

 