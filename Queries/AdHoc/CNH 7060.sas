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
						LNXX.BF_SSN,
						LNXX.LN_RPS_SEQ,
						LNXX.LC_STA_LONXX,
						LNXX.LC_TYP_SCH_DIS,
						RSXX.LD_RPS_X_PAY_DU
					FROM
						PKUB.RSXX_BR_RPD RSXX
						INNER JOIN PKUB.LNXX_LON_RPS LNXX
							ON LNXX.BF_SSN = RSXX.BF_SSN
							AND LNXX.LN_RPS_SEQ = RSXX.LN_RPS_SEQ
					WHERE
						LNXX.LC_TYP_SCH_DIS = 'IB'
						AND DAYS(RSXX.LD_RPS_X_PAY_DU) < DAYS(CURRENT_DATE)

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

PROC SQL;
	CREATE TABLE NUMBERS AS
		SELECT
			D.*
		FROM
			DEMO D
		WHERE
			LD_RPS_X_PAY_DU BETWEEN INPUT('XX/XX/XXXX', MMDDYYXX.) AND INPUT('XX/XX/XXXX', MMDDYYXX.)
;
QUIT;


PROC EXPORT DATA = WORK.NUMBERS 
            OUTFILE = "T:\NH XXXX.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="MAY - JUNE RENEWALS"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\NH XXXX.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RAW DATA"; 
RUN;

