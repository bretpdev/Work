/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";


PROC IMPORT datafile = 'T:\Cornerstone AM backfill.xlsx'
	DBMS = excel
	OUT = Backfill
	REPLACE; 
	GETNAMES=YES;
RUN;

DATA Backfill;
	SET Backfill;
	BF_SSN = PUT(BF_SSN, $X.);
RUN;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;

DATA LEGEND.Backfill;
	SET Backfill;
RUN;

RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE X  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @XX " ********************************************************************* "
              / @XX " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @XX " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @XX " ********************************************************************* "
              / @XX " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @XX " ****  &SQLXMSG   **** "
              / @XX " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL ;

	CREATE TABLE DEMO AS
		SELECT DISTINCT
			GRXX.BF_SSN,
			GRXX.LN_SEQ,
			GRXX.WN_SEQ_GRXX,
			GRXX.WC_NDS_RPD_TRM_RPT
		FROM
			Backfill B
			INNER JOIN PKUB.GRXX_RPT_LON_APL GRXX
				ON GRXX.BF_SSN = B.BF_SSN
				AND GRXX.LN_SEQ = INPUT(B.LN_SEQ,X.)
				AND GRXX.WN_SEQ_GRXX = INPUT(B.WN_SEQ_GRXX,X.)
;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SHEETX"; 
RUN;

