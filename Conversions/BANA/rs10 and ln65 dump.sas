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

	CREATE TABLE RS10 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						RS10.*
					FROM	
						OLWHRM1.RS10_BR_RPD RS10
						INNER JOIN OLWHRM1.LN10_LON LN10
							ON LN10.BF_SSN = RS10.BF_SSN
					WHERE 
						LN10.LF_LON_CUR_OWN LIKE '829769%'
						AND LN10.LC_STA_LON10 = 'P' 
					FOR READ ONLY WITH UR
				)
	;
	
	CREATE TABLE LN65 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						LN65.*
					FROM	
						OLWHRM1.LN65_LON_RPS LN65
						INNER JOIN OLWHRM1.LN10_LON LN10
							ON LN10.BF_SSN = LN65.BF_SSN
							AND LN10.LN_SEQ = LN65.LN_SEQ
					WHERE
						LN10.LF_LON_CUR_OWN LIKE '829769%'
						AND LN10.LC_STA_LON10 = 'P'
					FOR READ ONLY WITH UR
				)
	;

QUIT;

ENDRSUBMIT;
DATA RS10;
	SET DUSTER.RS10;
RUN;
DATA LN65;
	SET DUSTER.LN65;
RUN;

PROC EXPORT DATA = LN65 
            OUTFILE = "T:\SAS\RS10 and LN65.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LN65"; 
RUN;
PROC EXPORT DATA = RS10 
            OUTFILE = "T:\SAS\RS10 and LN65.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RS10"; 
RUN;
