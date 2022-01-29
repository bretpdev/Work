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
					SELECT DISTINCT
						LNXX.BF_SSN,
						LNXX.LN_SEQ,
						LNXX.LF_FED_CLC_RSK,
						FRXX.LF_FED_CLC_RSK_OLD,
						FRXX.LF_FED_CLC_RSK_NEW
					FROM
						PKUB.LNXX_LON LNXX
						LEFT JOIN PKUB.FRXX_VIT_CHG_HST FRXX
							ON LNXX.BF_SSN = FRXX.BF_SSN
							AND LNXX.LN_SEQ = FRXX.LN_SEQ
					WHERE (LNXX.BF_SSN = 'XXXXXXXXX' AND LNXX.LN_SEQ = 'X')
						OR	(LNXX.BF_SSN = 'XXXXXXXXX' AND LNXX.LN_SEQ = 'X')
						OR	(LNXX.BF_SSN = 'XXXXXXXXX' AND LNXX.LN_SEQ = 'X')
						OR	(LNXX.BF_SSN = 'XXXXXXXXX' AND LNXX.LN_SEQ = 'X')
						OR	(LNXX.BF_SSN = 'XXXXXXXXX' AND LNXX.LN_SEQ = 'X')
						OR	(LNXX.BF_SSN = 'XXXXXXXXX' AND LNXX.LN_SEQ = 'X')
						OR	(LNXX.BF_SSN = 'XXXXXXXXX' AND LNXX.LN_SEQ = 'X')
						OR	(LNXX.BF_SSN = 'XXXXXXXXX' AND LNXX.LN_SEQ = 'X')
						OR	(LNXX.BF_SSN = 'XXXXXXXXX' AND LNXX.LN_SEQ = 'X')
						OR	(LNXX.BF_SSN = 'XXXXXXXXX' AND LNXX.LN_SEQ = 'X')
						OR	(LNXX.BF_SSN = 'XXXXXXXXX' AND LNXX.LN_SEQ = 'X')
						OR	(LNXX.BF_SSN = 'XXXXXXXXX' AND LNXX.LN_SEQ = 'X')
						OR	(LNXX.BF_SSN = 'XXXXXXXXX' AND LNXX.LN_SEQ = 'X')
						OR	(LNXX.BF_SSN = 'XXXXXXXXX' AND LNXX.LN_SEQ = 'X')
						OR	(LNXX.BF_SSN = 'XXXXXXXXX' AND LNXX.LN_SEQ = 'X')
						OR	(LNXX.BF_SSN = 'XXXXXXXXX' AND LNXX.LN_SEQ = 'X')
						OR	(LNXX.BF_SSN = 'XXXXXXXXX' AND LNXX.LN_SEQ = 'X')
						OR	(LNXX.BF_SSN = 'XXXXXXXXX' AND LNXX.LN_SEQ = 'X')
						OR	(LNXX.BF_SSN = 'XXXXXXXXX' AND LNXX.LN_SEQ = 'X')
						OR	(LNXX.BF_SSN = 'XXXXXXXXX' AND LNXX.LN_SEQ = 'X')


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
            OUTFILE = "T:\SAS\CNH XXXXX.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
