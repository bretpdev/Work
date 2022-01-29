/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

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
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						LNXX.BF_SSN
						,MDSB.MIN_DSB
						,CASE
						 WHEN YEAR(MDSB.MIN_DSB) = XXXX THEN X
						 ELSE X
						 END AS "FOURTEEN"
						,CASE
						 WHEN YEAR(MDSB.MIN_DSB) = XXXX THEN X
						 ELSE X
						 END AS "FIFTEEN"
					FROM
						PKUB.LNXX_LON LNXX
						JOIN (
								SELECT
									LNXX.BF_SSN
									,MIN(LNXX.LD_LON_X_DSB) AS MIN_DSB
								FROM PKUB.LNXX_LON LNXX
								GROUP BY 
									LNXX.BF_SSN
								) MDSB
							ON LNXX.BF_SSN = MDSB.BF_SSN
							AND LNXX.LD_LON_X_DSB = MDSB.MIN_DSB
					WHERE YEAR(MDSB.MIN_DSB) IN (XXXX, XXXX)

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

PROC SQL;
	CREATE TABLE SMRY AS
		SELECT
			SUM(D.FOURTEEN) AS TOT_XXXX
			,SUM(D.FIFTEEN) AS TOT_XXXX
		FROM 
			DEMO D
	;
QUIT;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="DETAIL"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.SMRY 
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SUMMARY"; 
RUN;

