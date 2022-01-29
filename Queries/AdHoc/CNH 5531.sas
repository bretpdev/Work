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
						LNXX.*
					FROM
						PKUB.LNXX_LON LNXX
						INNER JOIN PKUB.LNXX_INT_RTE_HST LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
						INNER JOIN (
								SELECT
									LNXX.BF_SSN
									,LNXX.LN_SEQ
									,MAX(LNXX.LN_ITR_SEQ) AS LN_ITR_SEQ
								FROM PKUB.LNXX_INT_RTE_HST LNXX
								WHERE LNXX.LC_STA_LONXX = 'A'
								GROUP BY LNXX.BF_SSN
										,LNXX.LN_SEQ
									) MX
							ON LNXX.BF_SSN = MX.BF_SSN
							AND LNXX.LN_SEQ = MX.LN_SEQ
							AND LNXX.LN_ITR_SEQ = MX.LN_ITR_SEQ
					WHERE
						LNXX.LD_ITR_EFF_BEG != 'XX/XX/XXXX'
						AND LNXX.LC_ITR_TYP = 'SV'
						AND LNXX.LC_STA_LONXX = 'R'
						AND LNXX.LA_CUR_PRI > X

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

PROC SORT DATA=DEMO; BY BF_SSN LN_SEQ; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
