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
--OPE ID'Closure Date
('XXXXXXXX')
--select * from @SchoolCodes

/*****************  MODIFIED QUERY  ***********************/
SELECT
	*
FROM
	(
		SELECT DISTINCT
			COUNT(DISTINCT CSREQ.BF_SSN) AS AppSentToBorrower,
			COUNT(DISTINCT CSDNY.BF_SSN) AS DeniedRequests
		FROM
			PDXX_PRS_NME PDXX
			INNER JOIN PDXX_PRS_ADR PDXX 
				ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
			INNER JOIN 
			(
				SELECT
					LNXX.BF_SSN,
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
				WHERE
					SDXX.LC_STA_STUXX = 'A'
			) School 
				ON School.BF_SSN = PDXX.DF_PRS_ID
			INNER JOIN LNXX_LON LNXX 
				ON LNXX.BF_SSN = PDXX.DF_PRS_ID
			LEFT JOIN SDXX_STU_SPR SDXX 
				ON SDXX.LF_STU_SSN = PDXX.DF_PRS_ID 
				AND SDXX.LF_DOE_SCL_ENR_CUR = School.SchoolCode 
				AND SDXX.LC_STA_STUXX = 'A'
			LEFT JOIN 
			(
				SELECT DISTINCT
					BF_SSN
				FROM
					CDW..AYXX_BR_LON_ATY AYXX
				WHERE
					PF_REQ_ACT = 'CSREQ'
					AND LC_STA_ACTYXX = 'A'
				GROUP BY
					BF_SSN
			) CSREQ
				ON CSREQ.BF_SSN = LNXX.BF_SSN
			LEFT JOIN 
			(
				SELECT DISTINCT
					BF_SSN
				FROM
					CDW..AYXX_BR_LON_ATY AYXX
				WHERE
					PF_REQ_ACT = 'CSDNY'
					AND LC_STA_ACTYXX = 'A'
				GROUP BY
					BF_SSN
			) CSDNY
				ON CSDNY.BF_SSN = LNXX.BF_SSN
		WHERE
			LNXX.LA_CUR_PRI > X.XX
			AND LNXX.LA_LON_AMT_GTR > X.XX
			AND LF_DOE_SCL_ENR_CUR IN (SELECT SchoolCode FROM @SchoolCodes)
			OR	LF_DOE_SCL_ORG IN (SELECT SchoolCode FROM @SchoolCodes)
	) X

;
