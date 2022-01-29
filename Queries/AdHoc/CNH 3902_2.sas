PROC IMPORT OUT= WORK.SOURCE
            DATAFILE= "T:\LNXX.xls" 
            DBMS=EXCEL REPLACE;
     RANGE="A$"; 
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
	KEEP BF_SSN LN_SEQ;
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
			S.BF_SSN
			,S.LN_SEQ
			,LNXX.BN_EFT_SEQ
			,LNXX.LF_EFT_OCC_DTS
			,LNXX.LC_STA_LNXX
			,LNXX.LR_EFT_RDC
		FROM
			SOURCE S
			INNER JOIN PKUB.LNXX_EFT_TO_LON LNXX
				ON S.BF_SSN = LNXX.BF_SSN
				AND S.LN_SEQ = LNXX.LN_SEQ
		WHERE
			LNXX.LC_STA_LNXX = 'A'
			AND 
			LNXX.LR_EFT_RDC = X

;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

/*/*export to Excel spreadsheet*/*/
/*PROC EXPORT DATA = WORK.DEMO */
/*            OUTFILE = "T:\SAS\NH XXXX.xls" */
/*            DBMS = EXCEL*/
/*			REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
;
/*export to comma delimited file*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NH XXXX.CSV" 
            DBMS = CSV 
			REPLACE;
     PUTNAMES = YES;
RUN;
