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

	CREATE TABLE LN65 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						LN65.*
					FROM	
						OLWHRM1.LN10_LON LN10
						INNER JOIN OLWHRM1.LN65_LON_RPS LN65
							ON LN10.BF_SSN = LN65.BF_SSN
							AND LN10.LN_SEQ = LN65.LN_SEQ
					WHERE	
						LN10.LF_LON_CUR_OWN IN ('826717','830248')
						AND LN65.LC_TYP_SCH_DIS IN ('IB','IL')


					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE LN66 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						LN66.*
					FROM	
						OLWHRM1.LN10_LON LN10
						INNER JOIN OLWHRM1.LN65_LON_RPS LN65
							ON LN10.BF_SSN = LN65.BF_SSN
							AND LN10.LN_SEQ = LN65.LN_SEQ
						INNER JOIN OLWHRM1.LN66_LON_RPS_SPF LN66
							ON LN10.BF_SSN = LN66.BF_SSN
							AND LN10.BF_SSN = LN66.LN_SEQ
							AND LN65.LN_RPS_SEQ = LN66.LN_RPS_SEQ
					WHERE	
						LN10.LF_LON_CUR_OWN IN ('826717','830248')
						AND LN65.LC_TYP_SCH_DIS IN ('IB','IL')


					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE RS05 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						RS05.*
					FROM	
						OLWHRM1.LN10_LON LN10
						INNER JOIN OLWHRM1.LN65_LON_RPS LN65
							ON LN10.BF_SSN = LN65.BF_SSN
							AND LN10.LN_SEQ = LN65.LN_SEQ
						INNER JOIN OLWHRM1.RS05_IBR_RPS RS05
							ON LN10.BF_SSN = RS05.BF_SSN
					WHERE	
						LN10.LF_LON_CUR_OWN IN ('826717','830248')
						AND LN65.LC_TYP_SCH_DIS IN ('IB','IL')


					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA LN65; SET DUSTER.LN65; RUN;
DATA LN66; SET DUSTER.LN66; RUN;
DATA RS05; SET DUSTER.RS05; RUN;

PROC EXPORT
		DATA=LN65
		OUTFILE='T:\SAS\NH 19938.XLSX'
		REPLACE;
		SHEET="LN65"; 
RUN;

PROC EXPORT
		DATA=LN66
		OUTFILE='T:\SAS\NH 19938.XLSX'
		REPLACE;
		SHEET="LN66"; 
RUN;

PROC EXPORT
		DATA=RS05
		OUTFILE='T:\SAS\NH 19938.XLSX'
		REPLACE;
		SHEET="RS05"; 
RUN;
