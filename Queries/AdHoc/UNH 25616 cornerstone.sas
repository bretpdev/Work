/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS46.NWS46RZ";
FILENAME REPORT2 "&RPTLIB/UNWS46.NWS46R2";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;

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

PROC SQL ;
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE ARCS AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						AY10.*
					FROM	
						PKUB.AY10_BR_LON_ATY AY10
					WHERE	
						AY10.LF_USR_REQ_ATY IN ('UT01310','UT01311')
						AND
						DAYS(AY10.LD_ATY_REQ_RCV) BETWEEN DAYS('04-29-2015') AND DAYS(CURRENT_DATE)

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE PD40 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						PD40.*
					FROM	
						PKUB.PD40_PRS_PHN PD40
					WHERE	
						PD40.DF_LST_USR_PD40 IN ('UT01310','UT01311')
						AND 
						DAYS(PD40.DD_PHN_VER) BETWEEN DAYS('04-29-2015') AND DAYS(CURRENT_DATE)

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
						PKUB.PD41_PHN_HST PD41
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
						PKUB.PD31_PRS_INA PD31
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
						PKUB.PD30_PRS_ADR PD30
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
	SET LEGEND.ARCS;
RUN;

DATA PD40;
	SET LEGEND.PD40;
RUN;

DATA PD41;
	SET LEGEND.PD41;
RUN;

DATA PD31;
	SET LEGEND.PD31;
RUN;

DATA PD30;
	SET LEGEND.PD30;
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.ARCS 
            OUTFILE = "T:\SAS\NH 25616 cornerstone.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="ARCS"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.PD40 
            OUTFILE = "T:\SAS\NH 25616 cornerstone.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="PD40"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.PD41 
            OUTFILE = "T:\SAS\NH 25616 cornerstone.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="PD41"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.PD31 
            OUTFILE = "T:\SAS\NH 25616 cornerstone.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="PD31"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.PD30 
            OUTFILE = "T:\SAS\NH 25616 cornerstone.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="PD30"; 
RUN;

