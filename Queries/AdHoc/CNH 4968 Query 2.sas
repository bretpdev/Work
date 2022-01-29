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
					INNER JOIN (
								SELECT DISTINCT
									LNXX.BF_SSN
									,LNXX.LN_SEQ
									,SUM(DAYS(DTES.END_DTE) - DAYS(DTES.BEG_DTE)) / XX AS MNTHS_USED 
								FROM
									PKUB.LNXX_BR_FOR_APV LNXX
									INNER JOIN (
												SELECT DISTINCT
													LNXX.BF_SSN
													,LNXX.LN_SEQ
													,LF_FOR_CTL_NUM
													,LN_FOR_OCC_SEQ
													,CASE
														WHEN LNXX.LD_FOR_BEG < 'XX/XX/XXXX' THEN LNXX.LD_FOR_BEG
														ELSE 'XX/XX/XXXX'
													END AS BEG_DTE
													,CASE
														WHEN LNXX.LD_FOR_END < 'XX/XX/XXXX' THEN LNXX.LD_FOR_END
														ELSE 'XX/XX/XXXX'
													END AS END_DTE
												FROM
													PKUB.LNXX_BR_FOR_APV LNXX
												WHERE
													LNXX.LC_STA_LONXX = 'A'
												) DTES
										ON LNXX.BF_SSN = DTES.BF_SSN
										AND LNXX.LN_SEQ = DTES.LN_SEQ
										AND LNXX.LF_FOR_CTL_NUM = DTES.LF_FOR_CTL_NUM
										AND LNXX.LN_FOR_OCC_SEQ = DTES.LN_FOR_OCC_SEQ
								GROUP BY 
									LNXX.BF_SSN
									,LNXX.LN_SEQ
								) FLT
						ON LNXX.BF_SSN = FLT.BF_SSN
						AND LNXX.LN_SEQ = FLT.LN_SEQ
						INNER JOIN (
								SELECT DISTINCT
									LNXX.BF_SSN
									,LNXX.LN_SEQ
									,LNXX.LF_FOR_CTL_NUM
									,LNXX.LN_FOR_OCC_SEQ
									,LNXX.LD_FOR_BEG
									,CRNT.LD_FOR_END
								FROM
									PKUB.LNXX_LON LNXX
										INNER JOIN PKUB.LNXX_BR_FOR_APV LNXX
											ON LNXX.BF_SSN = LNXX.BF_SSN
											AND LNXX.LN_SEQ = LNXX.LN_SEQ
									INNER JOIN (
												SELECT DISTINCT
													LNXX.BF_SSN
													,LNXX.LN_SEQ
													,LF_FOR_CTL_NUM
													,LN_FOR_OCC_SEQ
													,LNXX.LD_FOR_END
												FROM
													PKUB.LNXX_BR_FOR_APV LNXX
												WHERE
													LNXX.LC_STA_LONXX = 'A'
													AND
													LNXX.LD_FOR_END > 'XX/XX/XXXX'
												) CRNT
										ON LNXX.BF_SSN = CRNT.BF_SSN
										AND LNXX.LN_SEQ = CRNT.LN_SEQ
										AND LNXX.LF_FOR_CTL_NUM = CRNT.LF_FOR_CTL_NUM
										AND LNXX.LN_FOR_OCC_SEQ = CRNT.LN_FOR_OCC_SEQ
								WHERE LNXX.LA_CUR_PRI > X
									AND LNXX.LC_STA_LONXX = 'R'
									AND LNXX.LD_FOR_BEG <= 'XX/XX/XXXX'
									AND LNXX.LC_STA_LONXX = 'A'
									) CRNTX
							ON LNXX.BF_SSN = CRNTX.BF_SSN
							AND LNXX.LN_SEQ = CRNTX.LN_SEQ
					WHERE FLT.MNTHS_USED >= XX
						AND LNXX.LA_CUR_PRI > X
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
            OUTFILE = "T:\SAS\NH XXXX query X.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
