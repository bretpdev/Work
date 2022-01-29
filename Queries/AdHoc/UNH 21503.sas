LIBNAME DUSTER REMOTE SERVER=DUSTER SLIBREF=WORK;
RSUBMIT;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
PROC SQL;
	CREATE TABLE DEFS AS
		SELECT
			LN50.BF_SSN,
			LN50.LN_SEQ,
			LN50.LF_DFR_CTL_NUM,
			LN50.LN_DFR_OCC_SEQ,
			INTCK('DAY',LN50.LD_DFR_BEG,LN50.LD_DFR_END) AS DF_DAYS
		FROM
			OLWHRM1.DF10_BR_DFR_REQ DF10
			JOIN OLWHRM1.LN50_BR_DFR_APV LN50
				ON DF10.BF_SSN = LN50.BF_SSN
				AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
		WHERE
			DF10.LC_DFR_TYP = '29'
			AND DF10.LC_DFR_STA = 'A' 
			AND DF10.LC_STA_DFR10 = 'A' 
			AND LN50.LC_STA_LON50 = 'A' 
			AND LN50.LC_DFR_RSP ^= '003'
		ORDER BY
			LN50.BF_SSN,
			LN50.LN_SEQ
	;
QUIT;

PROC SQL;
	CREATE TABLE EH_DEF_48 AS
		SELECT DISTINCT 
			BF_SSN
		FROM 
			DEFS
		GROUP BY
			BF_SSN,
			LN_SEQ
		HAVING
			SUM(DF_DAYS) > 1095
		ORDER BY
			BF_SSN
	;
QUIT;

ENDRSUBMIT; 

DATA EH_DEF_48; SET DUSTER.EH_DEF_48; RUN;

PROC EXPORT
		DATA=EH_DEF_48
		OUTFILE='T:\EH Deferment - 48 Months Used.XLSX'
		REPLACE;
RUN;
