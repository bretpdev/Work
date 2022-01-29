USE CDW
GO

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @SchoolCodes TABLE
(
	SchoolCode CHAR(X),
	SchoolCloseDate DATE
)

INSERT INTO
	@SchoolCodes
VALUES
	 ('XXXXXXXX', 'XX/XX/XXXX')
	,('XXXXXXXX', 'XX/XX/XXXX')
	,('XXXXXXXX', 'X/X/XXXX')
	,('XXXXXXXX', 'X/XX/XXXX')
	,('XXXXXXXX', 'XX/XX/XXXX')
	,('XXXXXXXX', 'XX/XX/XXXX')
	,('XXXXXXXX', 'XX/XX/XXXX')
	,('XXXXXXXX', 'XX/XX/XXXX')
	,('XXXXXXXX', 'XX/XX/XXXX')

--SELECT * FROM @SchoolCodes

SELECT
	*
FROM
	(
		SELECT DISTINCT
			PDXX.DF_SPE_ACC_ID,
			PDXX.DM_PRS_X,
			PDXX.DM_PRS_MID,
			PDXX.DM_PRS_LST,
			PDXX.DX_STR_ADR_X,
			PDXX.DX_STR_ADR_X,
			PDXX.DX_STR_ADR_X,
			PDXX.DM_CT,
			PDXX.DC_DOM_ST,
			PDXX.DF_ZIP_CDE,
			PDXX.DI_VLD_ADR,
			LNXX.LF_DOE_SCL_ORG,
			SDXX.LF_DOE_SCL_ENR_CUR,
			SDXX.LC_REA_SCL_SPR,
			SDXX.LD_NTF_SCL_SPR,
			SDXX.LD_SCL_SPR [LastDayOfEnrollment],
			School.SchoolCloseDate
		FROM
			PDXX_PRS_NME PDXX
			INNER JOIN PDXX_PRS_ADR PDXX ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
			INNER JOIN 
			(
				SELECT
					LNXX.BF_SSN,
					SC.SchoolCode,
					SC.SchoolCloseDate
				FROM
					@SchoolCodes SC
					INNER JOIN LNXX_LON LNXX ON LNXX.LF_DOE_SCL_ORG = SC.SchoolCode

				UNION

				SELECT DISTINCT
					SDXX.LF_STU_SSN [BF_SSN],
					SC.SchoolCode,
					SC.SchoolCloseDate
				FROM
					@SchoolCodes SC
					INNER JOIN SDXX_STU_SPR SDXX ON SDXX.LF_DOE_SCL_ENR_CUR = SC.SchoolCode
				WHERE
					SDXX.LC_STA_STUXX = 'A'
			) School ON School.BF_SSN = PDXX.DF_PRS_ID
			INNER JOIN LNXX_LON LNXX ON LNXX.BF_SSN = PDXX.DF_PRS_ID
			LEFT JOIN SDXX_STU_SPR SDXX ON SDXX.LF_STU_SSN = PDXX.DF_PRS_ID AND SDXX.LF_DOE_SCL_ENR_CUR = School.SchoolCode AND SDXX.LC_STA_STUXX = 'A'
		WHERE
			DATEADD(DAY, XXX, SDXX.LD_SCL_SPR) > School.SchoolCloseDate
	) X
WHERE
	DATEADD(DAY, XXX,ISNULL( X.LastDayOfEnrollment, GETDATE())) > X.SchoolCloseDate
	AND
	(
		X.LF_DOE_SCL_ENR_CUR IN (SELECT SchoolCode FROM @SchoolCodes)
		OR
		X.LF_DOE_SCL_ORG IN (SELECT SchoolCode FROM @SchoolCodes)
	)
ORDER BY
	X.DF_SPE_ACC_ID
