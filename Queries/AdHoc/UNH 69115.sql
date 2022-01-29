USE UDW
GO

DECLARE @SchoolCodes TABLE (SchoolCode CHAR(8),	SchoolCloseDate DATE);
INSERT INTO	@SchoolCodes (SchoolCode, SchoolCloseDate) VALUES
--OPE ID	Closure Date
('00396505','7/31/2020 '),
('01077905','8/31/2020 '),
('01077914','8/11/2020 '),
('00331701','8/8/2017  '),
('00325990','1/12/2020 '),
('00325991','12/16/2018'),
('00327227','12/12/2019'),
('32098818','7/22/2020 '),
('32098820','3/30/2020 '),
('00158802','7/9/2020  '),
('00158804','7/9/2020  '),
('00158808','7/9/2020  '),
('00158810','7/9/2020  '),
('00158811','7/9/2020  '),
('00158813','7/9/2020  '),
('00158814','7/9/2020  '),
('00158815','7/9/2020  '),
('00158817','7/9/2020  '),
('00158820','7/9/2020  '),
('00158821','7/9/2020  '),
('00158822','7/9/2020  '),
('00158823','7/31/2017 '),
('00158824','7/23/2019 '),
('00158826','7/9/2020  '),
('00158828','7/9/2020  '),
('00158831','7/9/2020  '),
('00492400','6/19/2020 '),
('00893900','8/31/2020 '),
('00177901','7/18/2017 '),
('00177903','7/18/2017 '),
('00177904','7/18/2017 '),
('00177909','7/18/2017 '),
('00177914','7/18/2017 '),
('02074407','7/18/2017 '),
('02074409','7/18/2017 '),
('02074412','7/18/2017 '),
('01163100','4/25/2020 '),
('02218810','6/1/2018  '),
('02491501','5/5/2020  '),
('00318465','12/15/2017'),
('00135202','5/15/2020 '),
('00359208','9/14/2020 '),
('00359304','5/5/2020  '),
('52098893','4/21/2020 '),
('00194812','3/13/2020 '),
('00757303','8/21/2020 '),
('03088800','6/30/2020 '),
('02594301','9/13/2020 '),
('02594302','9/13/2020 '),
('00407103','6/15/2020 '),
('00367401','9/13/2020 '),
('00367405','9/13/2020 '),
('00367411','9/13/2020 '),
('00108307','5/10/2018 '),
('00108317','12/19/2019'),
('01146020','3/14/2020 '),
('01146026','3/14/2020 '),
('01146029','3/14/2020 '),
('01146031','3/14/2020 '),
('01146039','3/14/2020 '),
('01146046','3/14/2020 '),
('52098887','12/11/2014'),
('82098867','2/19/2020 '),
('02541901','8/26/2020 '),
('02143401','12/6/2019 '),
('02594300','9/13/2020 ')
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