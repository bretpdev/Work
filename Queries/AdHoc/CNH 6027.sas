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
						COUNT(DISTINCT LNXX.BF_SSN) AS TOT_CNT
						,COUNT(DISTINCT LNXX.BF_SSN) AS TOT_XX_DLQ
					FROM
						PKUB.LNXX_LON LNXX
						INNER JOIN PKUB.LNXX_RPD_PIO_CVN LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
							AND LNXX.LD_LON_RHB_PCV IS NOT NULL 
						LEFT JOIN (
								SELECT DISTINCT
									LNXX.BF_SSN
								FROM
									PKUB.LNXX_LON_DLQ_HST LNXX
									INNER JOIN (
											SELECT
												LNXX.BF_SSN
												,MAX(LNXX.LN_DLQ_MAX) AS LN_DLQ_MAX
											FROM 
												PKUB.LNXX_LON_DLQ_HST LNXX
											WHERE LNXX.LC_STA_LONXX = 'X'
											GROUP BY LNXX.BF_SSN
												) MX
										ON LNXX.BF_SSN = MX.BF_SSN
								WHERE MX.LN_DLQ_MAX >= XX
									) LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN	
					WHERE
						LNXX.LA_CUR_PRI > X
						AND LNXX.LC_STA_LONXX = 'R'

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
            OUTFILE = "T:\SAS\EXCEL OUTPUT.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
