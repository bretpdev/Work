LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE CLSD AS
		SELECT DISTINCT
			LNXX.BF_SSN,
			PDXX.DF_SPE_ACC_ID,
			PDXX.DM_PRS_X, 
			PDXX.DM_PRS_LST, 
			LNXX.LF_DOE_SCL_ORG,
			LNXX.LF_DL_SCL_ENR_DSB,
			AYXX.PF_REQ_ACT 
		FROM
			PKUB.LNXX_LON LNXX
			JOIN PKUB.AYXX_BR_LON_ATY AYXX
				ON LNXX.BF_SSN = AYXX.BF_SSN
			JOIN PKUB.PDXX_PRS_NME PDXX
				ON LNXX.BF_SSN = PDXX.DF_PRS_ID
			LEFT JOIN
				PKUB.LNXX_DSB LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
		WHERE
			AYXX.PF_REQ_ACT IN ('DISCK','DICSK')
/*			AND (LNXX.LF_DOE_SCL_ORG = 'XXXXXXXX' OR LNXX.LF_DL_SCL_ENR_DSB  = 'XXXXXXXX')*/
	;
QUIT;
ENDRSUBMIT;

DATA CLSD; SET LEGEND.CLSD; RUN;

PROC EXPORT
		DATA=CLSD
		OUTFILE='T:\Closed School Request for FSA.XLSX'
		REPLACE;
RUN;


