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
						BLXX.BF_SSN
						,BLXX.LD_BIL_CRT
						,PDXX.DF_SPE_ACC_ID
					FROM
						PKUB.BLXX_BR_BIL BLXX
							INNER JOIN PKUB.LNXX_LON_BIL_CRF LNXX
								ON BLXX.BF_SSN = LNXX.BF_SSN
								AND BLXX.LD_BIL_CRT = LNXX.LD_BIL_CRT
								AND BLXX.LN_SEQ_BIL_WI_DTE = LNXX.LN_SEQ_BIL_WI_DTE
							INNER JOIN PKUB.LNXX_LON LNXX
								ON BLXX.BF_SSN = LNXX.BF_SSN
								AND LNXX.LN_SEQ = LNXX.LN_SEQ
							INNER JOIN PKUB.PDXX_PRS_NME PDXX
								ON BLXX.BF_SSN = PDXX.DF_PRS_ID
					WHERE
						BLXX.LC_STA_BILXX = 'A'
					AND LNXX.LC_STA_LONXX = 'A'
					AND LNXX.LA_CUR_PRI > X
					AND LNXX.LC_STA_LONXX = 'R'
/*					AND LNXX.LC_LON_SND_CHC != 'Y'*/
					AND BLXX.LD_BIL_CRT BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'

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
CREATE TABLE CHK AS
	SELECT
		D.*
	FROM DEMO D
		INNER JOIN (
				SELECT
					D.BF_SSN
					,COUNT(D.BF_SSN) AS CNT
				FROM DEMO D
				GROUP BY D.BF_SSN
					) MULT
			ON D.BF_SSN = MULT.BF_SSN
	WHERE MULT.CNT > X
;
QUIT;

PROC SORT DATA=CHK; BY DF_SPE_ACC_ID; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\EXCEL OUTPUT.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
