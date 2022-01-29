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
						*
					FROM	
						OLWHRM1.MR64_BR_TAX
					WHERE
						LF_TAX_YR = '2015'

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE DEMO2 AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						*
					FROM
						OLWHRM1.MR67_ORG_ISS_DSU
					WHERE
						LF_TAX_YR = '2015'

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE DEMO3 AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						*
					FROM
						OLWHRM1.MR68_TAX_RPT_NOT
					WHERE
						LF_TAX_YR = '2015'

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO1; SET LEGEND.DEMO1; RUN;

DATA DEMO2; SET LEGEND.DEMO2; RUN;

DATA DEMO3; SET LEGEND.DEMO3; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO1 
            OUTFILE = "T:\SAS\UNH 25555.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="MR64"; 
RUN;

PROC EXPORT DATA = WORK.DEMO2 
            OUTFILE = "T:\SAS\UNH 25555.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="MR67"; 
RUN;

PROC EXPORT DATA = WORK.DEMO3 
            OUTFILE = "T:\SAS\UNH 25555.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="MR68"; 
RUN;
