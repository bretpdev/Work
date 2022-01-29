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

	CREATE TABLE SV10 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						*
					FROM
						OLWHRM1.SV10_SER_MST

					FOR READ ONLY WITH UR
				)
	;

		CREATE TABLE SV25 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						*
					FROM
						OLWHRM1.SV25_SER_DPT

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA SV10;
	SET DUSTER.SV10;
RUN;
DATA SV25;
	SET DUSTER.SV25;
RUN;

PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\UNH 26243.xlsx" 
            DBMS = EXCEL
     SHEET="SV10"; 
RUN;

PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\UNH 26243.xlsx" 
            DBMS = EXCEL
     SHEET="SV25"; 
RUN;
