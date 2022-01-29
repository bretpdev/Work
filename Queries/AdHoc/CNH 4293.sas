LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;

PROC SQL;
	CREATE TABLE FSA AS
		SELECT DISTINCT
			PDXX.DF_SPE_ACC_ID
		FROM
			PKUB.PDXX_PRS_NME PDXX
			JOIN PKUB.LNXX_LON LNXX
				ON PDXX.DF_PRS_ID = LNXX.BF_SSN
				AND LNXX.LA_CUR_PRI > X.XX
			JOIN PKUB.LNXX_LON_RPS ICR
				ON LNXX.BF_SSN = ICR.BF_SSN
				AND LNXX.LN_SEQ = ICR.LN_SEQ
				AND ICR.LC_TYP_SCH_DIS IN ('CX','CQ')
            	AND ICR.LC_STA_LONXX = 'A'   
			JOIN PKUB.LNXX_LON_RPS IBR
				ON LNXX.BF_SSN = IBR.BF_SSN
				AND LNXX.LN_SEQ = IBR.LN_SEQ
				AND IBR.LC_TYP_SCH_DIS IN ('PG', 'PL', 'IB', 'IL')
            	AND IBR.LC_STA_LONXX = 'I'   
	;
QUIT;

ENDRSUBMIT;

PROC EXPORT
		DATA=LEGEND.FSA
		OUTFILE='T:\SAS\FSA CR XXXX.xlsx'
		REPLACE;
RUN;
