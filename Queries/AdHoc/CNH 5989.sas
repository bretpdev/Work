/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
%let DB = DNFPRQUT;  *This is test;
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
/*%let DB = DNFPUTDL;  *This is live;*/

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

	CREATE TABLE RX AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						PDXX.DF_SPE_ACC_ID
						,LNXX.BF_SSN
						,PDXX.DD_BRT
						,PDXX.DM_PRS_X
						,PDXX.DM_PRS_LST
					FROM
						PKUB.LNXX_LON LNXX
						INNER JOIN PKUB.PDXX_PRS_NME PDXX
							ON LNXX.BF_SSN = PDXX.DF_PRS_ID
						LEFT JOIN WEBFLSX.WBXX_CSM_USR_ACC WBXX
							ON LNXX.BF_SSN = WBXX.DF_USR_SSN
					WHERE
						LNXX.LC_STA_LONXX = 'D'
						AND WBXX.DF_USR_SSN IS NULL

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE RX AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						PDXX.DF_SPE_ACC_ID
						,LNXX.BF_SSN
						,PDXX.DD_BRT
						,PDXX.DM_PRS_X
						,PDXX.DM_PRS_LST
					FROM
						PKUB.LNXX_LON LNXX
						INNER JOIN PKUB.PDXX_PRS_NME PDXX
							ON LNXX.BF_SSN = PDXX.DF_PRS_ID
						INNER JOIN PKUB.DWXX_DW_CLC_CLU DWXX
							ON LNXX.BF_SSN = DWXX.BF_SSN
							AND LNXX.LN_SEQ = DWXX.LN_SEQ
						LEFT JOIN WEBFLSX.WBXX_CSM_USR_ACC WBXX
							ON LNXX.BF_SSN = WBXX.DF_USR_SSN
					WHERE
						LNXX.LC_STA_LONXX = 'R'
						AND LNXX.LA_CUR_PRI > X
						AND WBXX.DF_USR_SSN IS NULL
						AND DWXX.WC_DW_LON_STA IN ('XX','XX')

					FOR READ ONLY WITH UR
				)
	;
	
	CREATE TABLE RX AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						PDXX.DF_SPE_ACC_ID
						,LNXX.BF_SSN
						,PDXX.DD_BRT
						,PDXX.DM_PRS_X
						,PDXX.DM_PRS_LST
					FROM
						PKUB.LNXX_LON LNXX
						INNER JOIN PKUB.PDXX_PRS_NME PDXX
							ON LNXX.BF_SSN = PDXX.DF_PRS_ID
						INNER JOIN PKUB.DWXX_DW_CLC_CLU DWXX
							ON LNXX.BF_SSN = DWXX.BF_SSN
							AND LNXX.LN_SEQ = DWXX.LN_SEQ
						LEFT JOIN WEBFLSX.WBXX_CSM_USR_ACC WBXX
							ON LNXX.BF_SSN = WBXX.DF_USR_SSN
					WHERE
						LNXX.LC_STA_LONXX = 'R'
						AND LNXX.LA_CUR_PRI > X
						AND WBXX.DF_USR_SSN IS NULL
						AND DWXX.WC_DW_LON_STA NOT IN ('XX','XX','XX')

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA RX; SET LEGEND.RX; RUN;
DATA RX; SET LEGEND.RX; RUN;
DATA RX; SET LEGEND.RX; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.RX 
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RX"; 
RUN;

PROC EXPORT DATA = WORK.RX 
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RX"; 
RUN;


PROC EXPORT DATA = WORK.RX
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RX"; 
RUN;

