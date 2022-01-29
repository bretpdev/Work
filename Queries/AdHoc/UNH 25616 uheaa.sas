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

	CREATE TABLE ARCS AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						AY10.*
					FROM	
						OLWHRM1.AY10_BR_LON_ATY AY10
					WHERE	
						AY10.LF_USR_REQ_ATY IN ('UT01310','UT01311')
						AND
						DAYS(AY10.LD_ATY_REQ_RCV) BETWEEN DAYS('04-29-2015') AND DAYS(CURRENT_DATE)

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE PD42 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						PD42.*
					FROM	
						OLWHRM1.PD42_PRS_PHN PD42
					WHERE	
						PD42.DF_LST_USR_PD42 IN ('UT01310','UT01311')
						AND 
						DAYS(PD42.DD_PHN_VER) BETWEEN DAYS('04-29-2015') AND DAYS(CURRENT_DATE)

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE PD41 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						PD41.*
					FROM	
						OLWHRM1.PD41_PHN_HST PD41
					WHERE	
						PD41.DF_LST_USR_PD41 IN ('UT01310','UT01311')
						AND 
						DAYS(PD41.DD_PHN_VER_HST) BETWEEN DAYS('04-29-2015') AND DAYS(CURRENT_DATE)

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE PD31 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						PD31.*
					FROM	
						OLWHRM1.PD31_PRS_INA PD31
					WHERE	
						PD31.DF_LST_USR_PD31 IN ('UT01310','UT01311')
						AND 
						DAYS(PD31.DD_VER_ADR_HST) BETWEEN DAYS('04-29-2015') AND DAYS(CURRENT_DATE)

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE PD30 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						PD30.*
					FROM	
						OLWHRM1.PD30_PRS_ADR PD30
					WHERE	
						PD30.DF_LST_USR_PD30 IN ('UT01310','UT01311')
						AND 
						DAYS(PD30.DD_VER_ADR) BETWEEN DAYS('04-29-2015') AND DAYS(CURRENT_DATE)

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA ARCS;
	SET DUSTER.ARCS;
RUN;

DATA PD42;
	SET DUSTER.PD42;
RUN;

DATA PD41;
	SET DUSTER.PD41;
RUN;

DATA PD31;
	SET DUSTER.PD31;
RUN;

DATA PD30;
	SET DUSTER.PD30;
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.ARCS 
            OUTFILE = "T:\SAS\NH 25616 uheaa.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="ARCS"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.PD42 
            OUTFILE = "T:\SAS\NH 25616 uheaa.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="PD42"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.PD41 
            OUTFILE = "T:\SAS\NH 25616 uheaa.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="PD41"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.PD31 
            OUTFILE = "T:\SAS\NH 25616 uheaa.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="PD31"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.PD30 
            OUTFILE = "T:\SAS\NH 25616 uheaa.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="PD30"; 
RUN;

