/****************CHECK ATTENDANCE********************/

USE CDW
GO

DECLARE @BEGINDATE DATE = CONVERT(DATE,'XXXXXXXX');
DECLARE @ENDDATE DATE = CONVERT(DATE,'XXXXXXXX');
--select @BEGINDATE,@ENDDATE

DECLARE @OPEID TABLE (OPEID VARCHAR(X));
INSERT INTO @OPEID VALUES 
 ('XXXXXXXX')
;
--select * from @OPEID;

--enrolled
SELECT DISTINCT
	O.OPEID,
	PDXX.DM_PRS_X AS FirstName,
	PDXX.DM_PRS_LST AS LastName,
	LNXX.BF_SSN AS SSN,
	--LNXX.LD_TRM_BEG,
	IIF(AYXX.PF_REQ_ACT IN ('DIFCR','DICSK'), AYXX.PF_REQ_ACT, 'no') AS 'HAS_DIFCR/DICSK',
	AYXX.LD_ATY_REQ_RCV,
	AYXX.LC_STA_ACTYXX
FROM
	LNXX_LON LNXX
	INNER JOIN PDXX_PRS_NME PDXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
	INNER JOIN @OPEID O
		ON LNXX.LF_DOE_SCL_ORG = O.OPEID
	INNER JOIN AYXX_BR_LON_ATY AYXX
		ON LNXX.BF_SSN = AYXX.BF_SSN
WHERE
	AYXX.LD_ATY_REQ_RCV >= @BEGINDATE 
	AND AYXX.LD_ATY_REQ_RCV <= @ENDDATE
	--AND AYXX.PF_REQ_ACT IN ('DIFCR','DICSK')
	AND AYXX.LC_STA_ACTYXX = 'A'
ORDER BY
	AYXX.LD_ATY_REQ_RCV
	,IIF(AYXX.PF_REQ_ACT IN ('DIFCR','DICSK'), AYXX.PF_REQ_ACT, 'no') DESC
;

--ever attended:
DECLARE @SQLQUERY VARCHAR(MAX) = 
'
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
				 ''''XXXXXXXX''''
			)
				AND MRXX.LF_DOE_SCL_ENR_CUR IN
			(
				 ''''XXXXXXXX''''
			)
			THEN ''''BOTH''''
			WHEN LNXX.LF_DOE_SCL_ORG IN
			(
				 ''''XXXXXXXX''''
			)
			THEN ''''LNXX_LF_DOE_SCL_ORG''''
			WHEN MRXX.LF_DOE_SCL_ENR_CUR IN
			(
				 ''''XXXXXXXX''''
			)
			THEN ''''MRXX_LF_DOE_SCL_ENR_CUR''''
		END AS MATCH
		--AYXX.PF_REQ_ACT,
		--AYXX.LD_ATY_REQ_RCV,
		--AYXX.LC_STA_ACTYXX
	FROM
		PKUB.LNXX_LON LNXX
		INNER JOIN PKUB.PDXX_PRS_NME PDXX
			ON LNXX.BF_SSN = PDXX.DF_PRS_ID
		INNER JOIN PKUB.AYXX_BR_LON_ATY AYXX
			ON LNXX.BF_SSN = AYXX.BF_SSN
		LEFT JOIN PKUR.MRXX_MGT_RPT_LON MRXX
			ON LNXX.BF_SSN = MRXX.BF_SSN
	WHERE
		--DAYS(AYXX.LD_ATY_REQ_RCV) BETWEEN DAYS(''''' + CAST(@BEGINDATE AS VARCHAR) + ''''') AND DAYS(''''' + CAST(@ENDDATE AS VARCHAR)+ ''''')
		--AND
		 --AYXX.PF_REQ_ACT IN (''''DIFCR'''',''''DICSK'''')
		--AND AYXX.LC_STA_ACTYXX = ''''A''''
		--AND 
		(
			LNXX.LF_DOE_SCL_ORG IN
				(
					 ''''XXXXXXXX''''
				) 
			OR MRXX.LF_DOE_SCL_ENR_CUR IN
				(
				 ''''XXXXXXXX''''
				)				
		)
'')'

EXEC(@SQLQUERY);
