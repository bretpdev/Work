/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

PROC IMPORT OUT= WORK.LNXX
            DATAFILE= "T:\SAS\SOURCE.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="SheetX$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;

DATA LEGEND.SSNS;
	KEEP BF_SSN;
	SET LNXX;
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
			S.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.BN_EFT_SEQ,
			LNXX.LF_EFT_OCC_DTS AS LF_EFT_OCC_DTS_TEMP,
			LNXX.LC_STA_LNXX,
			LNXX.LR_EFT_RDC
		FROM
			SSNS S
		INNER JOIN
			PKUB.LNXX_EFT_TO_LON LNXX
			ON	S.BF_SSN = LNXX.BF_SSN
		INNER JOIN
			PKUB.LNXX_LON LNXX
			ON	S.BF_SSN = LNXX.BF_SSN
			AND	LNXX.LN_SEQ = LNXX.LN_SEQ
		WHERE 
				LNXX.LC_STA_LNXX = 'A'
			AND LNXX.LR_EFT_RDC = X
	;
	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

DATA DEMOX;
	SET DEMO;
	FORMAT LF_EFT_OCC_DTS_TEMP DATETIMEXX.X;
RUN;

DATA DEMOX;
	SET DEMO;
	LF_EFT_OCC_DTS = TRANSLATE(PUT(LF_EFT_OCC_DTS_TEMP,ISXXXXDTXX.X),'-','T');
	DROP LF_EFT_OCC_DTS_TEMP;
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMOX
            OUTFILE = "T:\SAS\NHXXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="lnXX"; 
RUN;
