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

PROC SQL ;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						LNXX.BF_SSN
					FROM
						PKUB.LNXX_LON LNXX
						INNER JOIN PKUB.LNXX_LON_DLQ_HST LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
						INNER JOIN PKUB.AYXX_BR_LON_ATY AYXX
							ON AYXX.BF_SSN = LNXX.BF_SSN
					WHERE
						LNXX.LA_CUR_PRI > X
						AND LNXX.LC_STA_LONXX = 'R'
						AND LNXX.LC_STA_LONXX = 'X' 
						AND LNXX.LN_DLQ_MAX >= XXX
						AND AYXX.PF_REQ_ACT = 'DLFDL'
						AND DAYS(AYXX.LD_ATY_REQ_RCV) >= DAYS('XX/XX/XXXX')

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;


/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SSAEXX FED Final Demand.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="Results"; 
RUN;

