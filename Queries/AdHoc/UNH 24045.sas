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

	CREATE TABLE RS05 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						*
					FROM	
						OLWHRM1.RS05_IBR_RPS RS05
					WHERE	
						RS05.BF_SSN in( '520742719','529451028')

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

PROC SQL;
	CONNECT TO DB2 (DATABASE=DLGSUTWH);

	CREATE TABLE RS10 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						*
					FROM	
						OLWHRM1.RS10_BR_RPD RS10
					WHERE	
						RS10.BF_SSN in( '520742719','529451028')

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

PROC SQL;
	CONNECT TO DB2 (DATABASE=DLGSUTWH);

	CREATE TABLE LN65 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						*
					FROM	
						OLWHRM1.LN65_LON_RPS LN65
					WHERE	
						LN65.BF_SSN in( '520742719','529451028')

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA RS05;
	SET DUSTER.RS05;
RUN;

DATA RS10;
	SET DUSTER.RS10;
RUN;

DATA LN65;
	SET DUSTER.LN65;
RUN;

PROC EXPORT DATA = WORK.RS05 
            OUTFILE = "T:\NH 24045.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RS05"; 
RUN;

PROC EXPORT DATA = WORK.RS10 
            OUTFILE = "T:\NH 24045.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RS10"; 
RUN;

PROC EXPORT DATA = WORK.LN65 
            OUTFILE = "T:\NH 24045.xlsx"
            DBMS = EXCEL
			REPLACE;
     SHEET="LN65"; 
RUN;
