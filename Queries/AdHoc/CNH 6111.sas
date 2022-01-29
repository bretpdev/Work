PROC IMPORT OUT= WORK.SOURCE
            DATAFILE= "T:\data dump.xlsx" 
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
	BF_SSN = PUT(BORROWERS, $X.);
	DROP BORROWERS;
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

PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE PDXX AS
		SELECT	
			A.*
		FROM	
			SOURCE S
			INNER JOIN PKUB.PDXX_PRS_ADR A
				ON S.BF_SSN = A.DF_PRS_ID
	;

	CREATE TABLE PDXX AS
		SELECT	
			A.*
		FROM	
			SOURCE S
			INNER JOIN PKUB.PDXX_PRS_INA A
				ON S.BF_SSN = A.DF_PRS_ID
	;

	CREATE TABLE FRXX AS
		SELECT	
			A.*
		FROM	
			SOURCE S
			INNER JOIN PKUB.FRXX_VIT_CHG_HST A
				ON S.BF_SSN = A.BF_SSN
	;


	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA PDXX; SET LEGEND.PDXX; RUN;
DATA PDXX; SET LEGEND.PDXX; RUN;
DATA FRXX; SET LEGEND.FRXX; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.PDXX
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="PDXX"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.PDXX
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="PDXX"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.PDXX
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="FRXX"; 
RUN;
