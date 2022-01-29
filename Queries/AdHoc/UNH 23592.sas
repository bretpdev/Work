/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWAB1.LWAB1RZ";
FILENAME REPORT2 "&RPTLIB/ULWAB1.LWAB1R2";
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;
/*LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE 0  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @01 " ********************************************************************* "
              / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @01 " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @01 " ****  &SQLXMSG   **** "
              / @01 " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL;
	CONNECT TO DB2 (DATABASE=DLGSUTWH);

	CREATE TABLE DEMO AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						GA10.AF_APL_ID || GA10.AF_APL_ID_SFX,
						GA10.AD_PRC
					FROM	
						OLWHRM1.GA10_LON_APP GA10
					WHERE
						GA10.AC_PRC_STA = 'A'

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;	
run;

DATA DEMO; SET DUSTER.DEMO; RUN;

DATA OUT1 OUT2 OUT3;
	SET DEMO;
	IF _N_ < 600000 THEN OUTPUT OUT1;
	IF _N_ >= 600000 AND _N_ < 1200000 THEN OUTPUT OUT2;
	IF _N_ >= 1200000 THEN OUTPUT OUT3;
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = OUT1
            OUTFILE = "T:\SAS\Guaranteed Loans.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = OUT2
            OUTFILE = "T:\SAS\Guaranteed Loans.xlsx" 
            DBMS = EXCEL;
     SHEET="B"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = OUT3
            OUTFILE = "T:\SAS\Guaranteed Loans.xlsx" 
            DBMS = EXCEL;
     SHEET="C"; 
RUN;
