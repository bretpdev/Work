/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
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
					SELECT DISTINCT 
						LN65.*				
					FROM	
						OLWHRM1.LN65_LON_RPS LN65
						INNER JOIN OLWHRM1.PD10_PRS_NME PD10
							ON PD10.DF_PRS_ID = LN65.BF_SSN
					WHERE PD10.DF_SPE_ACC_ID = '5367367791'

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE RS05 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT 
						RS05.*				
					FROM	
						OLWHRM1.RS05_IBR_RPS RS05
						INNER JOIN OLWHRM1.PD10_PRS_NME PD10
							ON PD10.DF_PRS_ID = RS05.BF_SSN
					WHERE PD10.DF_SPE_ACC_ID = '5367367791'

					FOR READ ONLY WITH UR
				)
	;

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
						INNER JOIN OLWHRM1.PD10_PRS_NME PD10
							ON PD10.DF_PRS_ID = RS10.BF_SSN
					WHERE PD10.DF_SPE_ACC_ID = '5367367791'

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE RS20 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT 
						RS20.*				
					FROM	
						OLWHRM1.RS20_IBR_IRL_LON RS20
						INNER JOIN OLWHRM1.PD10_PRS_NME PD10
							ON PD10.DF_PRS_ID = RS20.BF_SSN
					WHERE PD10.DF_SPE_ACC_ID = '5367367791'

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA LN65;
	SET DUSTER.LN65;
RUN;
DATA RS05;
	SET DUSTER.RS05;
RUN;
DATA RS10;
	SET DUSTER.RS10;
RUN;
DATA RS20;
	SET DUSTER.RS20;
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.LN65 
            OUTFILE = "T:\NH 25418.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LN65"; 
RUN;

PROC EXPORT DATA = WORK.RS05 
            OUTFILE = "T:\NH 25418.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RS05"; 
RUN;

PROC EXPORT DATA = WORK.RS10
            OUTFILE = "T:\NH 25418.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RS10"; 
RUN;

PROC EXPORT DATA = WORK.RS20
            OUTFILE = "T:\NH 25418.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RS20"; 
RUN;
