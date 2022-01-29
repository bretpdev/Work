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

PROC SQL inobs = XXXX;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT
						FRXX.*
					FROM
						PKUR.FRXX_MTH_INT_RPT FRXX
					WHERE
						(FRXX.BF_SSN = 'XXXXXXXXX' AND FRXX.LN_SEQ IN (X, X))
						 OR (FRXX.BF_SSN = 'XXXXXXXXX' AND FRXX.LN_SEQ IN(X, X))
						 OR (FRXX.BF_SSN = 'XXXXXXXXX' AND FRXX.LN_SEQ IN(X))
						 OR (FRXX.BF_SSN = 'XXXXXXXXX' AND FRXX.LN_SEQ IN(X, X, X, X, X, X, X, X, X, XX, XX))
						 OR (FRXX.BF_SSN = 'XXXXXXXXX' AND FRXX.LN_SEQ IN(X, X))
						 OR (FRXX.BF_SSN = 'XXXXXXXXX' AND FRXX.LN_SEQ IN(X, X))
						 OR (FRXX.BF_SSN = 'XXXXXXXXX' AND FRXX.LN_SEQ IN(X, X, X, X))
						 OR (FRXX.BF_SSN = 'XXXXXXXXX' AND FRXX.LN_SEQ IN(X))

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\FRXX.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
