USE UDW
GO

DECLARE @SchoolCodes TABLE (SchoolCode CHAR(8),	SchoolCloseDate DATE);
INSERT INTO	@SchoolCodes (SchoolCode, SchoolCloseDate) VALUES
--OPE ID	Closure Date
('00757301','10/23/2020'),
('00330960','7/3/2018'),
('02053002','12/14/2018'),
('04192000','10/31/2020'),
('00167413','6/5/2015'),
('00172229','3/9/2018'),
('02339601','3/17/2020'),
('03956317','3/17/2014'),
('00970802','10/23/2020'),
('00354916','8/21/2020'),
('01300510','11/14/2020'),
('01067401','6/14/2018'),
('00974309','11/16/2016'),
('00974340','1/24/2017'),
('00348716','12/31/2019'),
('01070001','8/26/2020'),
('00134629','12/13/2016'),
('00377405','6/14/2019'),
('00717806','4/13/2020')
;
--select * from @SchoolCodes

/*****************  MODIFIED QUERY  ***********************/
SELECT
	*
FROM
	(
		SELECT DISTINCT
			PD10B.DF_SPE_ACC_ID,
			PD10B.DM_PRS_1,
			PD10B.DM_PRS_MID,
			PD10B.DM_PRS_LST,
			PD30.DX_STR_ADR_1,
			PD30.DX_STR_ADR_2,
			PD30.DX_STR_ADR_3,
			PD30.DM_CT,
			PD30.DC_DOM_ST,
			PD30.DF_ZIP_CDE,
			PD30.DI_VLD_ADR,
			SD10.LF_DOE_SCL_ENR_CUR,
			SD10.LC_REA_SCL_SPR,
			MAX(SD10.LD_NTF_SCL_SPR) OVER(PARTITION BY SD10.LF_STU_SSN) AS LD_NTF_SCL_SPR,
			--SD10.LD_NTF_SCL_SPR,
			CASE 
				WHEN IC_LON_PGM NOT IN ('DLPLUS', 'DLPLGB') THEN  ISNULL(SD10.LD_SCL_SPR,'01-01-1900') 
				ELSE SD10.LD_SCL_SPR 
			END [LastDayOfEnrollment],
			School.SchoolCloseDate,
			ln10.LF_DOE_SCL_ORG,
			LN10.IC_LON_PGM,
			LN10.LA_CUR_PRI,
			LN10.LN_SEQ
		FROM
			PD10_PRS_NME PD10
			
			INNER JOIN 
			(
				SELECT
					LN10.LF_STU_SSN AS BF_SSN,
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
			) School 
				ON School.BF_SSN = PD10.DF_PRS_ID
			INNER JOIN 
			(
				SELECT DISTINCT
					LF_STU_SSN,
					BF_SSN,
					LA_CUR_PRI AS LA_CUR_PRI,
					LA_LON_AMT_GTR AS LA_LON_AMT_GTR,
					LF_DOE_SCL_ORG,
					IC_LON_PGM,
					LN_SEQ
				FROM
					LN10_LON 
			)LN10 
				ON LN10.LF_STU_SSN = PD10.DF_PRS_ID
			left JOIN PD30_PRS_ADR PD30 
				ON PD30.DF_PRS_ID = ln10.BF_SSN
			left JOIN PD10_PRS_NME PD10B
				ON PD10B.DF_PRS_ID = ln10.BF_SSN
			LEFT JOIN SD10_STU_SPR SD10 
				ON SD10.LF_STU_SSN = PD10.DF_PRS_ID 
				AND SD10.LF_DOE_SCL_ENR_CUR = School.SchoolCode 
				AND DATEDIFF(DAY, ISNULL(SD10.LD_STA_STU10, GETDATE()), GETDATE()) <= 120
				AND DATEDIFF(DAY, ISNULL(SD10.LD_SCL_SPR, GETDATE()), GETDATE()) <= 120
			LEFT JOIN
			(
				SELECT DISTINCT
					LN90.BF_SSN
				FROM
					LN90_FIN_ATY LN90
					INNER JOIN 
					(
						SELECT
							BF_SSN,
							MAX(LN_FAT_SEQ) AS LN_FAT_SEQ
						FROM
							LN90_FIN_ATY
						WHERE
							LC_STA_LON90 = 'A'
							AND ISNULL(LC_FAT_REV_REA,'') = ''
						GROUP BY
							BF_SSN
					)ML
						ON ML.BF_SSN = LN90.BF_SSN
						AND ML.LN_FAT_SEQ = LN90.LN_FAT_SEQ
				WHERE
					LN90.PC_FAT_TYP = '10'
					AND LN90.PC_FAT_SUB_TYP = '10'
			) LN90
				ON LN90.BF_SSN = LN10.BF_SSN
				
		WHERE 
			LN10.LA_LON_AMT_GTR > 0.00
			AND
			(
				LN10.LA_CUR_PRI > 0.00
				OR
				LN90.BF_SSN IS NOT NULL
			)
	) X
WHERE
	DATEADD(DAY, 121, ISNULL(X.LastDayOfEnrollment, GETDATE())) > X.SchoolCloseDate
	AND	(
			X.LF_DOE_SCL_ENR_CUR IN (SELECT SchoolCode FROM @SchoolCodes)
			OR	X.LF_DOE_SCL_ORG IN (SELECT SchoolCode FROM @SchoolCodes)
		)
ORDER BY
	X.DF_SPE_ACC_ID,
	X.LN_SEQ
;