%LET RPTLIB = T:\SAS;
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME AES DBX DATABASE=&DB OWNER=AES;
LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

PROC SQL;

	CONNECT TO DBX (DATABASE=&DB);
		
		CREATE TABLE SchoolIds AS
			SELECT
				LNXX.BF_SSN,
				LNXX.LN_SEQ,
				LNXX.LF_DOE_SCL_ORG
			FROM
				PKUB.LNXX_LON LNXX
				LEFT OUTER JOIN PKUB.AYXX_BR_LON_ATY AYXX
				ON AYXX.BF_SSN = LNXX.BF_SSN
				AND AYXX.PF_REQ_ACT = 'MXXXX'
				AND AYXX.LD_ATY_REQ_RCV = 'XXMAYXXXX'd
			WHERE
				LNXX.LF_DOE_SCL_ORG IN ('XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX')
				AND AYXX.BF_SSN IS NULL;
QUIT;

ENDRSUBMIT;


DATA SchoolIds; SET LEGEND.SchoolIds; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.SchoolIds 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SheetX"; 
RUN;