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
						lnXX.bf_ssn
					FROM
						 PKUB.LNXX_LON LNXX
					INNER JOIN PKUB.DWXX_DW_CLC_CLU DWXX
						ON DWXX.BF_SSN = LNXX.BF_SSN
						AND DWXX.LN_SEQ = LNXX.LN_SEQ
					INNER JOIN PKUB.LNXX_LON_RPS LNXX
						ON LNXX.BF_SSN = LNXX.BF_SSN
						AND LNXX.LN_SEQ = LNXX.LN_SEQ
					WHERE
						(LNXX.LA_CUR_PRI + DWXX.WA_TOT_BRI_OTS) > X
						AND LNXX.LC_STA_LONXX = 'R'
						AND LNXX.LC_TYP_SCH_DIS IN ('IB', 'IL', 'CX', 'CX', 'CX', 'CQ', 'CA', 'CP','IX', 'IP')
						AND LNXX.LC_STA_LONXX = 'A'
						and DWXX.WC_DW_LON_STA = 'XX'

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;


QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;
/**/
/**/
/*/*export to Excel spreadsheet*/*/
/*PROC EXPORT DATA = WORK.DEMO */
/*            OUTFILE = "T:\SAS\EXCEL OUTPUT.xls" */
/*            DBMS = EXCEL*/
/*			REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
