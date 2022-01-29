USE CDW;
GO

DECLARE @SchoolCodes TABLE
(
	SchoolCode CHAR(X),
	SchoolCloseDate DATE
);

INSERT INTO
	@SchoolCodes (SchoolCode,SchoolCloseDate)
VALUES 
--OPE ID	Closure Date
('XXXXXXXX','X/XX/XXXX '),
('XXXXXXXX','X/X/XXXX  '),
('XXXXXXXX','X/XX/XXXX '),
('XXXXXXXX','X/X/XXXX  '),
('XXXXXXXX','X/X/XXXX  '),
('XXXXXXXX','X/XX/XXXX '),
('XXXXXXXX','X/XX/XXXX '),
('XXXXXXXX','X/XX/XXXX '),
('XXXXXXXX','XX/XX/XXXX'),
('XXXXXXXX','X/XX/XXXX '),
('XXXXXXXX','X/XX/XXXX '),
('XXXXXXXX','X/XX/XXXX '),
('XXXXXXXX','XX/XX/XXXX'),
('XXXXXXXX','X/X/XXXX  '),
('XXXXXXXX','XX/X/XXXX '),
('XXXXXXXX','X/XX/XXXX '),
('XXXXXXXX','X/X/XXXX  '),
('XXXXXXXX','X/XX/XXXX '),
('XXXXXXXX','X/XX/XXXX '),
('XXXXXXXX','X/XX/XXXX '),
('XXXXXXXX','X/XX/XXXX '),
('XXXXXXXX','X/XX/XXXX '),
('XXXXXXXX','X/XX/XXXX '),
('XXXXXXXX','X/XX/XXXX '),
('XXXXXXXX','X/XX/XXXX '),
('XXXXXXXX','X/XX/XXXX '),
('XXXXXXXX','X/XX/XXXX '),
('XXXXXXXX','X/XX/XXXX '),
('XXXXXXXX','XX/X/XXXX ')

;
--select * from @SchoolCodes

/*****************  MODIFIED QUERY  ***********************/
SELECT
	*
FROM
	(
		SELECT DISTINCT
			PDXXB.DF_SPE_ACC_ID,
			LNXX.LN_SEQ,
			LNXX.IC_LON_PGM,
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
			SDXX.LF_DOE_SCL_ENR_CUR,
			LNXX.LF_DOE_SCL_ORG,
			SDXX.LC_REA_SCL_SPR,
			MAX(SDXX.LD_NTF_SCL_SPR) OVER(PARTITION BY SDXX.LF_STU_SSN) AS LD_NTF_SCL_SPR,
			--SDXX.LD_NTF_SCL_SPR,
			CASE 
				WHEN IC_LON_PGM NOT IN ('DLPLUS', 'DLPLGB') THEN  ISNULL(SDXX.LD_SCL_SPR,'XX-XX-XXXX') 
				ELSE SDXX.LD_SCL_SPR 
			END [LastDayOfEnrollment],
			School.SchoolCloseDate,
			LNXX.LA_CUR_PRI
		FROM
			PDXX_PRS_NME PDXX
			
			INNER JOIN 
			(
				SELECT
					LNXX.LF_STU_SSN AS BF_SSN,
					SC.SchoolCode,
					SC.SchoolCloseDate
				FROM
					@SchoolCodes SC
					INNER JOIN LNXX_LON LNXX 
						ON LNXX.LF_DOE_SCL_ORG = SC.SchoolCode

				UNION ALL

				SELECT
					SDXX.LF_STU_SSN [BF_SSN],
					SC.SchoolCode,
					SC.SchoolCloseDate
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
			LEFT JOIN PDXX_PRS_ADR PDXX 
				ON PDXX.DF_PRS_ID = lnXX.BF_SSN
			LEFT JOIN PDXX_PRS_NME PDXXB
				ON PDXXB.DF_PRS_ID = lnXX.BF_SSN
			LEFT JOIN SDXX_STU_SPR SDXX 
				ON SDXX.LF_STU_SSN = PDXX.DF_PRS_ID 
				AND SDXX.LF_DOE_SCL_ENR_CUR = School.SchoolCode 
				AND DATEDIFF(DAY, ISNULL(SDXX.LD_STA_STUXX, GETDATE()), GETDATE()) <= XXX
				AND DATEDIFF(DAY, ISNULL(SDXX.LD_SCL_SPR, GETDATE()), GETDATE()) <= XXX
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
					) ML
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
				OR
				LNXX.BF_SSN IS NOT NULL
			)
	) X
WHERE
	DATEADD(DAY, XXX, ISNULL(X.LastDayOfEnrollment, GETDATE())) > X.SchoolCloseDate
	AND	(
			X.LF_DOE_SCL_ENR_CUR IN (SELECT SchoolCode FROM @SchoolCodes)
			OR	X.LF_DOE_SCL_ORG IN (SELECT SchoolCode FROM @SchoolCodes)
		)
ORDER BY
	X.DF_SPE_ACC_ID,
	X.LN_SEQ
;