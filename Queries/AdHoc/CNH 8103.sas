PROC IMPORT OUT= WORK.SEPT
            DATAFILE= "T:\Closed School OPE ID Sept.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="SheetX$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

PROC IMPORT OUT= WORK.AUG
            DATAFILE= "T:\OPE ID List Aug.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="SheetX$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;



LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
DATA LEGEND.SEPT;
SET SEPT;
RUN;

DATA LEGEND.AUG;
SET AUG;
RUN;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
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
	CREATE TABLE AUG_OUTPUT AS
		SELECT DISTINCT
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LF_DOE_SCL_ORG
		FROM
			PKUB.LNXX_LON LNXX
			INNER JOIN AUG A
				ON A.ope_id = LNXX.LF_DOE_SCL_ORG
;

	CREATE TABLE SEPT_OUTPUT AS
		SELECT DISTINCT
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LF_DOE_SCL_ORG
		FROM
			PKUB.LNXX_LON LNXX
			INNER JOIN SEPT S
				ON S.ope_id = LNXX.LF_DOE_SCL_ORG
;


QUIT;

ENDRSUBMIT;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = LEGEND.AUG_OUTPUT 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="AUG"; 
RUN;

PROC EXPORT DATA = LEGEND.SEPT_OUTPUT 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SEPT"; 
RUN;


