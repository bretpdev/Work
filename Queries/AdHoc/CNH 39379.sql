/****************CHECK ATTENDANCE********************/

USE CDW
GO

--generic query
SELECT
	PDXX.DM_PRS_X,
	PDXX.DM_PRS_LST,
	PDXX.DF_PRS_ID, 
	AYXX.PF_REQ_ACT
FROM
	PDXX_PRS_NME PDXX
	INNER JOIN LNXX_LON LNXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
	INNER JOIN AYXX_BR_LON_ATY AYXX
		ON AYXX.BF_SSN = LNXX.BF_SSN
WHERE
	AYXX.PF_REQ_ACT = 'DIFCR'
	AND LNXX.LF_DOE_SCL_ORG IN 
		(
			'XXXXXXXX',
			'XXXXXXXX',
			'XXXXXXXX',
			'XXXXXXXX',
			'XXXXXXXX',
			'XXXXXXXX',
			'XXXXXXXX',
			'XXXXXXXX',
			'XXXXXXXX',
			'XXXXXXXX',
			'XXXXXXXX',
			'XXXXXXXX',
			'XXXXXXXX',
			'XXXXXXXX',
			'XXXXXXXX',
			'XXXXXXXX',
			'XXXXXXXX',
			'XXXXXXXX',
			'XXXXXXXX',
			'XXXXXXXX',
			'XXXXXXXX',
			'XXXXXXXX',
			'XXXXXXXX',
			'XXXXXXXX',
			'XXXXXXXX',
			'XXXXXXXX',
			'XXXXXXXX'		
		)
;

--more involved query
DECLARE @OPEID TABLE (OPEID VARCHAR(X));
INSERT INTO @OPEID
VALUES
	('XXXXXXXX'),
	('XXXXXXXX'),
	('XXXXXXXX'),
	('XXXXXXXX'),
	('XXXXXXXX'),
	('XXXXXXXX'),
	('XXXXXXXX'),
	('XXXXXXXX'),
	('XXXXXXXX'),
	('XXXXXXXX'),
	('XXXXXXXX'),
	('XXXXXXXX'),
	('XXXXXXXX'),
	('XXXXXXXX'),
	('XXXXXXXX'),
	('XXXXXXXX'),
	('XXXXXXXX'),
	('XXXXXXXX'),
	('XXXXXXXX'),
	('XXXXXXXX'),
	('XXXXXXXX'),
	('XXXXXXXX'),
	('XXXXXXXX'),
	('XXXXXXXX'),
	('XXXXXXXX'),
	('XXXXXXXX'),
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
	AYXX.PF_REQ_ACT = 'DIFCR'
ORDER BY
	AYXX.LD_ATY_REQ_RCV	
;

--ever attended: --uncomment one @OPEIDX at a time and run from here down for each
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
--DECLARE @OPEIDX VARCHAR(X) ='XXXXXXXX';
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
				)
					AND MRXX.LF_DOE_SCL_ENR_CUR IN
				(
					'''''+@OPEIDX+'''''
				)
				THEN ''''BOTH''''
				WHEN LNXX.LF_DOE_SCL_ORG IN
				(
					 '''''+@OPEIDX+'''''
				)
				THEN ''''LNXX_LF_DOE_SCL_ORG''''
				WHEN MRXX.LF_DOE_SCL_ENR_CUR IN
				(
					'''''+@OPEIDX+'''''
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
			AYXX.PF_REQ_ACT = (''''DIFCR'''')
			AND 
			(
				LNXX.LF_DOE_SCL_ORG IN
					(
						'''''+@OPEIDX+'''''
					) 
				OR MRXX.LF_DOE_SCL_ENR_CUR IN
					(
						'''''+@OPEIDX+'''''
					)				
			)
	'')
';

EXECUTE SP_EXECUTESQL @SQLQUERY;
