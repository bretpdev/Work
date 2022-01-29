PROC IMPORT OUT= WORK.SOURCE
            DATAFILE= "T:\SLMA X-X-XX Borrower List.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="SheetX$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;

DATA LEGEND.SOURCE;
	SET SOURCE;
	BF_SSN = COMPRESS(BORROWER_SSN,'','kd');
	KEEP BF_SSN BORROWER_SSN;
RUN;

DATA LEGEND.SOURCEX;
	SET SOURCE;
	LENGTH BF_SSN $X.;
	BF_SSN = STRIP(COMPRESS(BORROWER_SSN,'','kd'));
	FORMAT BF_SSN $X.;
	KEEP BF_SSN BORROWER_SSN;
RUN;

DATA SOURCEX;
	SET SOURCE;
	LENGTH BF_SSN $X.;
	BF_SSN = STRIP(COMPRESS(BORROWER_SSN,'','kd'));
	FORMAT BF_SSN $X.;
	KEEP BF_SSN BORROWER_SSN;
RUN;
DATA SOURCEX;
	SET SOURCEX;
	BR_SSN = BF_SSN;
	KEEP BORROWER_SSN BR_SSN;
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
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			S.BORROWER_SSN
			,LNXX.LN_SEQ
			,LNXX.LD_LON_GTR
			,FSXX.LF_FED_AWD || PUT(FSXX.LN_FED_AWD_SEQ,ZX.) AS AWARD_ID
		FROM	
			SOURCEX S
			INNER JOIN PKUB.LNXX_LON LNXX
				ON S.BF_SSN = LNXX.BF_SSN
			INNER JOIN PKUB.FSXX_DL_LON FSXX
				ON S.BF_SSN = FSXX.BF_SSN
				AND LNXX.LN_SEQ = FSXX.LN_SEQ
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

PROC SQL;
CREATE TABLE OUTPT AS
	SELECT
		S.*
		,D.LN_SEQ
		,D.LD_LON_GTR
		,D.AWARD_ID
	FROM SOURCE S
		INNER JOIN DEMO D
			ON S.BORROWER_SSN = D.BORROWER_SSN
;
QUIT;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.OUTPT
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
