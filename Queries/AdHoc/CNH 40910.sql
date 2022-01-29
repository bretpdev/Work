/****************CHECK ATTENDANCE********************/
USE CDW
GO

DECLARE @OPEIDX VARCHAR(X) = 'XXXXXXXX',
		@OPEIDX VARCHAR(X) = 'XXXXXXXX',
		@OPEIDX VARCHAR(X) = 'XXXXXXXX',
		@ARC VARCHAR(X) = 'DISCK'
;

--generic query
SELECT * FROM SCXX_SCH_DMO WHERE IF_DOE_SCL IN (@OPEIDX, @OPEIDX, @OPEIDX);
SELECT * FROM LNXX_LON WHERE LF_DOE_SCL_ORG IN (@OPEIDX, @OPEIDX, @OPEIDX);
SELECT * FROM SDXX_STU_SPR WHERE LF_DOE_SCL_ENR_CUR IN (@OPEIDX, @OPEIDX, @OPEIDX);

SELECT DISTINCT
	PDXX.DM_PRS_X,
	PDXX.DM_PRS_LST,
	PDXX.DF_PRS_ID, 
	AYXX.PF_REQ_ACT,
	LNXX.LF_DOE_SCL_ORG
FROM
	PDXX_PRS_NME PDXX
	INNER JOIN LNXX_LON LNXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
	INNER JOIN AYXX_BR_LON_ATY AYXX
		ON AYXX.BF_SSN = LNXX.BF_SSN
WHERE
	AYXX.PF_REQ_ACT = @ARC
	AND LNXX.LF_DOE_SCL_ORG IN (@OPEIDX, @OPEIDX, @OPEIDX)
;

--more involved query
DECLARE @OPEID TABLE (OPEID VARCHAR(X));
INSERT INTO @OPEID VALUES (@OPEIDX),(@OPEIDX), (@OPEIDX);
--select * from @OPEID;

--enrolled
SELECT DISTINCT
	O.OPEID,
	PDXX.DM_PRS_X AS FirstName,
	PDXX.DM_PRS_LST AS LastName,
	LNXX.BF_SSN AS SSN,
	LNXX.LD_TRM_BEG,
	AYXX.LD_ATY_REQ_RCV,
	AYXX.LC_STA_ACTYXX,
	AYXX.PF_REQ_ACT
FROM
	LNXX_LON LNXX
	INNER JOIN PDXX_PRS_NME PDXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
	INNER JOIN AYXX_BR_LON_ATY AYXX
		ON LNXX.BF_SSN = AYXX.BF_SSN
	INNER JOIN @OPEID O
		ON LNXX.LF_DOE_SCL_ORG = O.OPEID
WHERE
	AYXX.PF_REQ_ACT = @ARC
ORDER BY
	AYXX.LD_ATY_REQ_RCV	
;

--ever attended:
DECLARE @SQLQUERY NVARCHAR(MAX) =
N'
	SELECT	* FROM OPENQUERY(LEGEND,
	''
		SELECT DISTINCT
			PDXX.DM_PRS_X AS FirstName,
			PDXX.DM_PRS_LST AS LastName,
			LNXX.BF_SSN AS SSN,
			LNXX.LD_TRM_BEG,
			LNXX.LF_DOE_SCL_ORG AS LNXX_LF_DOE_SCL_ORG,
			MRXX.LF_DOE_SCL_ENR_CUR AS MRXX_LF_DOE_SCL_ENR_CUR,
			CASE
				WHEN LNXX.LF_DOE_SCL_ORG IN
				(
				 
					  '''''+@OPEIDX+'''''
					 ,'''''+@OPEIDX+'''''
					 ,'''''+@OPEIDX+'''''
				)
					AND MRXX.LF_DOE_SCL_ENR_CUR IN
				(
					 '''''+@OPEIDX+'''''
					,'''''+@OPEIDX+'''''
					,'''''+@OPEIDX+'''''
				)
				THEN ''''BOTH''''
				WHEN LNXX.LF_DOE_SCL_ORG IN
				(
					  '''''+@OPEIDX+'''''
					 ,'''''+@OPEIDX+'''''
					 ,'''''+@OPEIDX+'''''
				)
				THEN ''''LNXX_LF_DOE_SCL_ORG''''
				WHEN MRXX.LF_DOE_SCL_ENR_CUR IN
				(
					 '''''+@OPEIDX+'''''
					,'''''+@OPEIDX+'''''
					,'''''+@OPEIDX+'''''
				)
				THEN ''''MRXX_LF_DOE_SCL_ENR_CUR''''
			END AS MATCH,
			AYXX.PF_REQ_ACT,
			AYXX.LD_ATY_REQ_RCV,
			AYXX.LC_STA_ACTYXX
		FROM
			PKUB.LNXX_LON LNXX
			INNER JOIN PKUB.PDXX_PRS_NME PDXX
				ON LNXX.BF_SSN = PDXX.DF_PRS_ID
			INNER JOIN PKUB.AYXX_BR_LON_ATY AYXX
				ON LNXX.BF_SSN = AYXX.BF_SSN
			LEFT JOIN PKUR.MRXX_MGT_RPT_LON MRXX
				ON LNXX.BF_SSN = MRXX.BF_SSN
		WHERE		
			AYXX.PF_REQ_ACT = ('''''+@ARC+''''')
			AND 
			(
				LNXX.LF_DOE_SCL_ORG IN
					(
						 '''''+@OPEIDX+'''''
						,'''''+@OPEIDX+'''''
						,'''''+@OPEIDX+'''''
					) 
				OR MRXX.LF_DOE_SCL_ENR_CUR IN
					(
						 '''''+@OPEIDX+'''''
						,'''''+@OPEIDX+'''''
						,'''''+@OPEIDX+'''''
					)				
			)
	'')
';

EXECUTE SP_EXECUTESQL @SQLQUERY;
