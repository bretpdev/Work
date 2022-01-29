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
						PKUB.LNXX_LON_BIL_CRF LNXX
						INNER JOIN (
								SELECT
									LNXX.BF_SSN
									,COUNT(LNXX.LN_SEQ) AS NUM_LNS
								FROM 
									PKUB.LNXX_LON LNXX
								WHERE
									LNXX.LA_CUR_PRI > X
									AND LNXX.LC_STA_LONXX = 'R'
								GROUP BY 
									LNXX.BF_SSN
									) LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
						LEFT JOIN (
								SELECT
									LNXX.BF_SSN
									,SUM(LNXX.LA_BIL_DU_PRT) AS LA_BIL_DU_PRT
									,SUM(LNXX.LA_TOT_BIL_STS) AS LA_TOT_BIL_STS
								FROM 
									PKUB.LNXX_LON_BIL_CRF LNXX
								WHERE 
									DAYS(LNXX.LD_BIL_CRT) BETWEEN DAYS('XX/XX/XXXX') AND DAYS('XX/XX/XXXX') 
									AND LNXX.LC_STA_LONXX = 'A'
								GROUP BY 
									LNXX.BF_SSN
									) PD_AHD
							ON LNXX.BF_SSN = PD_AHD.BF_SSN
						INNER JOIN PKUB.BLXX_BR_BIL BLXX
							ON LNXX.BF_SSN = BLXX.BF_SSN
							AND LNXX.LD_BIL_CRT = BLXX.LD_BIL_CRT
							AND LNXX.LN_SEQ_BIL_WI_DTE = BLXX.LN_SEQ_BIL_WI_DTE
						INNER JOIN PKUB.LNXX_LON LNXXX
							ON LNXX.BF_SSN = LNXXX.BF_SSN
							AND LNXX.LN_SEQ = LNXXX.LN_SEQ
					WHERE
						LNXX.NUM_LNS > X
						AND DAYS(LNXX.LD_BIL_CRT) BETWEEN DAYS('XX/XX/XXXX') AND DAYS('XX/XX/XXXX') 
						AND LNXX.LC_STA_LONXX = 'A'
						AND PD_AHD.LA_BIL_DU_PRT > X 
						AND BLXX.LC_STA_BILXX = 'A'
						AND LNXXX.LA_CUR_PRI > X
						AND LNXXX.LC_STA_LONXX = 'R'
						AND LNXXX.LC_LON_SND_CHC != 'Y'
						AND (BLXX.LC_BIL_TYP = 'P' 
						OR (BLXX.LC_BIL_TYP = 'C' AND BLXX.LC_IND_BIL_SNT = 'T'))

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
