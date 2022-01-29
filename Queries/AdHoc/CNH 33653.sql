USE CDW
GO

DECLARE @BEGINDATE DATE = 'X/X/XXXX';
DECLARE @ENDDATE DATE = 'XX/XX/XXXX';
--SELECT @BEGINDATE,@ENDDATE

DECLARE @OPEID TABLE (OPEID VARCHAR(X));
INSERT INTO @OPEID VALUES 
 ('XXXXXXXX')
,('XXXXXXXX')
,('XXXXXXXX')
,('XXXXXXXX')
,('XXXXXXXX')
,('XXXXXXXX')
,('XXXXXXXX')
,('XXXXXXXX')
,('XXXXXXXX')
,('XXXXXXXX')
,('XXXXXXXX')
,('XXXXXXXX')
,('XXXXXXXX')
,('XXXXXXXX')
,('XXXXXXXX')
,('XXXXXXXX')
,('XXXXXXXX')
,('XXXXXXXX')
,('XXXXXXXX')
,('XXXXXXXX')
,('XXXXXXXX')
;
--SELECT * FROM @OPEID;

--enrolled
SELECT DISTINCT
	PDXX.DF_SPE_ACC_ID,
	LNXX.LN_SEQ,
	LNXX.LD_TRM_BEG,
	O.OPEID,
	LNXX.LD_TRM_BEG,
	AYXX.PF_REQ_ACT,
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
	AYXX.LD_ATY_REQ_RCV BETWEEN @BEGINDATE AND @ENDDATE
	AND	AYXX.PF_REQ_ACT IN ('DICSK','DIDTH','DIFCR','DIPSF','DITLF','DIUPR','DITPD')
	AND AYXX.LC_STA_ACTYXX = 'A'
ORDER BY
	PDXX.DF_SPE_ACC_ID,
	LNXX.LD_TRM_BEG


--ever attended:
DECLARE @SQLQUERY VARCHAR(MAX) = 
'
SELECT	* FROM OPENQUERY(LEGEND,
''
	SELECT DISTINCT
		PDXX.DF_SPE_ACC_ID,
		LNXX.LN_SEQ,
		LNXX.LD_TRM_BEG,
		LNXX.LF_DOE_SCL_ORG AS LNXX_LF_DOE_SCL_ORG,
		MRXX.LF_DOE_SCL_ENR_CUR AS MRXX_LF_DOE_SCL_ENR_CUR,
		CASE
			WHEN LNXX.LF_DOE_SCL_ORG IN
			(
				 ''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''		
			)
				AND MRXX.LF_DOE_SCL_ENR_CUR IN
			(
				 ''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
			)
			THEN ''''BOTH''''
			WHEN LNXX.LF_DOE_SCL_ORG IN
			(
				 ''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
			)
			THEN ''''LNXX_LF_DOE_SCL_ORG''''
			WHEN MRXX.LF_DOE_SCL_ENR_CUR IN
			(
				 ''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
				,''''XXXXXXXX''''
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
		DAYS(AYXX.LD_ATY_REQ_RCV) BETWEEN DAYS(''''' + CAST(@BEGINDATE AS VARCHAR) + ''''') AND DAYS(''''' + CAST(@ENDDATE AS VARCHAR)+ ''''')
		AND	AYXX.PF_REQ_ACT IN (''''DICSK'''',''''DIDTH'''',''''DIFCR'''',''''DIPSF'''',''''DITLF'''',''''DIUPR'''',''''DITPD'''')
		AND AYXX.LC_STA_ACTYXX = ''''A''''
		AND 
		(
			LNXX.LF_DOE_SCL_ORG IN
				(
					 ''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
				) 
			OR MRXX.LF_DOE_SCL_ENR_CUR IN
				(
					 ''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
					,''''XXXXXXXX''''
				)				
		)
'')'

EXEC(@SQLQUERY);
