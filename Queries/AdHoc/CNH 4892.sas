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
						,DSB.LA_CUR_PRI + IST.WA_TOT_BRI_OTS AS PRI_AND_INT
						,LNXX.LI_BR_DET_RPD_XTN
						,DWXX.WC_DW_LON_STA
					FROM
						PKUB.LNXX_LON LNXX
						INNER JOIN PKUB.DWXX_DW_CLC_CLU DWXX
							ON LNXX.BF_SSN = DWXX.BF_SSN
							AND LNXX.LN_SEQ = DWXX.LN_SEQ
						INNER JOIN (
									SELECT
										LNXX.BF_SSN
										,SUM(LNXX.LA_CUR_PRI) AS LA_CUR_PRI
										,MIN(LNXX.LD_LON_X_DSB) AS FST_DSB
									FROM 
										PKUB.LNXX_LON LNXX
									GROUP BY
										LNXX.BF_SSN
									) DSB
							ON LNXX.BF_SSN = DSB.BF_SSN
						INNER JOIN (
									SELECT
										DWXX.BF_SSN
										,SUM(DWXX.WA_TOT_BRI_OTS) AS WA_TOT_BRI_OTS
									FROM
										PKUB.DWXX_DW_CLC_CLU DWXX
									GROUP BY
										DWXX.BF_SSN
									) IST
							ON LNXX.BF_SSN = IST.BF_SSN
					WHERE
						(DSB.LA_CUR_PRI + IST.WA_TOT_BRI_OTS) >= XXXXX
						AND 
						DWXX.WC_DW_LON_STA IN ('XX','XX')
						AND
						LNXX.LI_BR_DET_RPD_XTN != 'Y'
						AND 
						DSB.FST_DSB > 'XX/XX/XXXX'

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
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
