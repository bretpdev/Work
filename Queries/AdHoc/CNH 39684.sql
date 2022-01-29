USE CDW
GO

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @SchoolCodes TABLE
(
	SchoolCode CHAR(X)
);

INSERT INTO
	@SchoolCodes
VALUES	  
--OPE ID	Closure Date
('XXXXXXXX')

;
--select * from @SchoolCodes

/*****************  MODIFIED QUERY  ***********************/
SELECT DISTINCT
	*
FROM
	(
		SELECT DISTINCT
			PDXXB.DF_SPE_ACC_ID,
			PDXXB.DM_PRS_X,
			PDXXB.DM_PRS_MID,
			PDXXB.DM_PRS_LST,
			PDXX.DX_STR_ADR_X,
			PDXX.DX_STR_ADR_X,
			PDXX.DX_STR_ADR_X,
			PDXX.DM_CT,
			PDXX.DC_DOM_ST,
			PDXX.DF_ZIP_CDE,
			PDXX.DI_VLD_ADR,
			Separation.LF_DOE_SCL_ENR_CUR,
			Separation.LC_REA_SCL_SPR,
			MAX(Separation.LD_NTF_SCL_SPR) OVER(PARTITION BY Separation.LF_STU_SSN) AS LD_NTF_SCL_SPR,
			--SDXX.LD_NTF_SCL_SPR,
			CASE 
				WHEN IC_LON_PGM NOT IN ('DLPLUS', 'DLPLGB') THEN  ISNULL(Separation.LD_SCL_SPR,'XX-XX-XXXX') 
				ELSE Separation.LD_SCL_SPR 
			END [LastDayOfEnrollment],
			lnXX.LF_DOE_SCL_ORG,
			LNXX.IC_LON_PGM,
			LNXX.LA_CUR_PRI,
			LNXX.LN_SEQ
		FROM
			PDXX_PRS_NME PDXX
			
			INNER JOIN 
			(
				SELECT
					LNXX.LF_STU_SSN AS BF_SSN,
					SC.SchoolCode
				FROM
					@SchoolCodes SC
					INNER JOIN LNXX_LON LNXX 
						ON LNXX.LF_DOE_SCL_ORG = SC.SchoolCode

				UNION

				SELECT DISTINCT
					SDXX.LF_STU_SSN [BF_SSN],
					SC.SchoolCode
				FROM
					@SchoolCodes SC
					INNER JOIN SDXX_STU_SPR SDXX 
						ON SDXX.LF_DOE_SCL_ENR_CUR = SC.SchoolCode
			) School 
				ON School.BF_SSN = PDXX.DF_PRS_ID
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
					LNXX_LON 
			)LNXX 
				ON LNXX.LF_STU_SSN = PDXX.DF_PRS_ID
			INNER JOIN 
			(
				SELECT
					SDXX.LF_STU_SSN,
					SDXX.LF_DOE_SCL_ENR_CUR,
					SDXX.LD_SCL_SPR,
					MAX(SDXX.LC_REA_SCL_SPR) OVER(PARTITION BY SDXX.LF_STU_SSN, SDXX.LF_DOE_SCL_ENR_CUR) AS LC_REA_SCL_SPR,
					SDXX.LD_NTF_SCL_SPR
				FROM
					SDXX_STU_SPR SDXX
					INNER JOIN
					(
						SELECT
							LF_STU_SSN,
							LF_DOE_SCL_ENR_CUR,
							MIN(LD_SCL_SPR) AS LD_SCL_SPR
						FROM
							SDXX_STU_SPR
						WHERE
							CAST(ISNULL(LD_SCL_SPR, GETDATE()) AS DATE) >= 'XXXX-XX-XX'
							AND CAST(ISNULL(LD_SCL_SPR, GETDATE()) AS DATE) <= 'XXXX-XX-XX'
						GROUP BY
							LF_STU_SSN,
							LF_DOE_SCL_ENR_CUR
					)SDXXMin
						ON SDXX.LF_STU_SSN = SDXXMin.LF_STU_SSN
						AND SDXX.LF_DOE_SCL_ENR_CUR = SDXXMin.LF_DOE_SCL_ENR_CUR
						AND SDXX.LD_SCL_SPR = SDXXMin.LD_SCL_SPR
			) Separation
				ON Separation.LF_STU_SSN = PDXX.DF_PRS_ID 
				AND Separation.LF_DOE_SCL_ENR_CUR = School.SchoolCode 
				AND CAST(ISNULL(Separation.LD_SCL_SPR, GETDATE()) AS DATE) >= 'XXXX-XX-XX'
				AND CAST(ISNULL(LD_SCL_SPR, GETDATE()) AS DATE) <= 'XXXX-XX-XX'
			left JOIN PDXX_PRS_ADR PDXX 
				ON PDXX.DF_PRS_ID = lnXX.BF_SSN
			left JOIN PDXX_PRS_NME PDXXB
				ON PDXXB.DF_PRS_ID = lnXX.BF_SSN		
			LEFT JOIN
			(
				SELECT DISTINCT
					LNXX.BF_SSN
				FROM
					CDW..LNXX_FIN_ATY LNXX
					INNER JOIN 
					(
						SELECT
							BF_SSN,
							MAX(LN_FAT_SEQ) AS LN_FAT_SEQ
						FROM
							CDW..LNXX_FIN_ATY
						WHERE
							LC_STA_LONXX = 'A'
							AND ISNULL(LC_FAT_REV_REA,'') = ''
						GROUP BY
							BF_SSN
					)ML
						ON ML.BF_SSN = LNXX.BF_SSN
						AND ML.LN_FAT_SEQ = LNXX.LN_FAT_SEQ
				WHERE
					LNXX.PC_FAT_TYP = 'XX'
					AND LNXX.PC_FAT_SUB_TYP = 'XX'
			) LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				
		WHERE 
			LNXX.LA_LON_AMT_GTR > X.XX
			AND
			(
				LNXX.LA_CUR_PRI > X.XX
				OR LNXX.BF_SSN IS NOT NULL
			)
	) X
WHERE
	CAST(ISNULL(X.LastDayOfEnrollment, GETDATE()) AS DATE) >= 'XXXX-XX-XX'
	AND CAST(ISNULL(X.LastDayOfEnrollment, GETDATE()) AS DATE) <= 'XXXX-XX-XX'
	AND	(
			X.LF_DOE_SCL_ENR_CUR IN (SELECT SchoolCode FROM @SchoolCodes)
			OR	X.LF_DOE_SCL_ORG IN (SELECT SchoolCode FROM @SchoolCodes)
		)
ORDER BY
	X.DF_SPE_ACC_ID,
	X.LN_SEQ
;
