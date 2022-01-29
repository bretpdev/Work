PROC IMPORT OUT= WORK.SOURCE
            DATAFILE= "T:\EAXX Prep file.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="SheetX$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

PROC SQL;
CREATE TABLE SOURCEX AS
	SELECT DISTINCT
		S.SSN AS BF_SSN
	FROM SOURCE S
;
QUIT;

/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;

DATA LEGEND.SOURCEX; 
	SET SOURCEX;
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

	CREATE TABLE DEMO AS
		SELECT	
			LNXX.LD_LON_GTR AS GUARANTY_DATE
			,S.BF_SSN AS SSN
			,PDXX.DM_PRS_LST AS LAST_NAME
			,PDXX.DM_PRS_X AS FIRST_NAME
			,FSXX.LF_FED_AWD || PUT(FSXX.LN_FED_AWD_SEQ, ZX.) AS AWARD_ID
			,LNXX.LC_FED_PGM_YR
		FROM SOURCEX S
			INNER JOIN PKUB.LNXX_LON LNXX
				ON S.BF_SSN = LNXX.BF_SSN
			INNER JOIN PKUB.PDXX_PRS_NME PDXX
				ON S.BF_SSN = PDXX.DF_PRS_ID
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

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
