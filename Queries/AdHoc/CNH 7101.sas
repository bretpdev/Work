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
						LNXX.BF_SSN
						,LNXX.LN_RPS_SEQ
						,LNXX.LC_TYP_SCH_DIS
/*						,GRDS.ALM_LN_GRD_RPS_SEQ*/
/*						,GRDS.MAX_LN_GRD_RPS_SEQ*/
						,LNXX_ALM.LA_RPS_ISL AS NORMAL_AMT
						,LNXX_MAX.LA_RPS_ISL AS BALLOON_AMT
					FROM
						PKUB.LNXX_LON_RPS LNXX
						INNER JOIN PKUB.LNXX_LON LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
						INNER JOIN (
								SELECT
									LNXX.BF_SSN
									,LNXX.LN_RPS_SEQ
									,LNXX.LN_GRD_RPS_SEQ AS ALM_LN_GRD_RPS_SEQ
									,MAX_GRD.LN_GRD_RPS_SEQ AS MAX_LN_GRD_RPS_SEQ
								FROM PKUB.LNXX_LON_RPS LNXX
									INNER JOIN PKUB.LNXX_LON_RPS_SPF LNXX
										ON LNXX.BF_SSN = LNXX.BF_SSN
										AND LNXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
									INNER JOIN (
											SELECT
												LNXX.BF_SSN
												,LNXX.LN_RPS_SEQ
												,MAX(LNXX.LN_GRD_RPS_SEQ) AS LN_GRD_RPS_SEQ
											FROM PKUB.LNXX_LON_RPS LNXX	
												INNER JOIN PKUB.LNXX_LON_RPS_SPF LNXX	
													ON LNXX.BF_SSN = LNXX.BF_SSN
													AND LNXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
											WHERE LNXX.LC_STA_LONXX = 'A'
												AND LNXX.LN_GRD_RPS_SEQ > X
												AND LNXX.LN_RPS_TRM = X
/*												AND LNXX.BF_SSN LIKE 'XXXX%'*/
											GROUP BY LNXX.BF_SSN
													,LNXX.LN_RPS_SEQ
												) MAX_GRD
										ON LNXX.BF_SSN = MAX_GRD.BF_SSN
										AND LNXX.LN_RPS_SEQ = MAX_GRD.LN_RPS_SEQ
								WHERE LNXX.LN_GRD_RPS_SEQ = MAX_GRD.LN_GRD_RPS_SEQ - X
									) GRDS
							ON LNXX.BF_SSN = GRDS.BF_SSN
							AND LNXX.LN_RPS_SEQ = GRDS.LN_RPS_SEQ
						INNER JOIN (
								SELECT
									LNXX.BF_SSN
									,LNXX.LN_RPS_SEQ
									,LNXX.LN_GRD_RPS_SEQ
									,SUM(LNXX.LA_RPS_ISL) AS LA_RPS_ISL
								FROM PKUB.LNXX_LON_RPS_SPF LNXX
								GROUP BY LNXX.BF_SSN
										,LNXX.LN_RPS_SEQ
										,LNXX.LN_GRD_RPS_SEQ
									) LNXX_MAX
							ON LNXX.BF_SSN = LNXX_MAX.BF_SSN
							AND LNXX.LN_RPS_SEQ = LNXX_MAX.LN_RPS_SEQ
							AND GRDS.MAX_LN_GRD_RPS_SEQ = LNXX_MAX.LN_GRD_RPS_SEQ
						INNER JOIN (
								SELECT
									LNXX.BF_SSN
									,LNXX.LN_RPS_SEQ
									,LNXX.LN_GRD_RPS_SEQ
									,SUM(LNXX.LA_RPS_ISL) AS LA_RPS_ISL
								FROM PKUB.LNXX_LON_RPS_SPF LNXX
								GROUP BY LNXX.BF_SSN
										,LNXX.LN_RPS_SEQ
										,LNXX.LN_GRD_RPS_SEQ
									) LNXX_ALM
							ON LNXX.BF_SSN = LNXX_ALM.BF_SSN
							AND LNXX.LN_RPS_SEQ = LNXX_ALM.LN_RPS_SEQ
							AND GRDS.ALM_LN_GRD_RPS_SEQ = LNXX_ALM.LN_GRD_RPS_SEQ
					WHERE
						LNXX_MAX.LA_RPS_ISL >= (LNXX_ALM.LA_RPS_ISL * X)

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

PROC SORT DATA=DEMO; BY BF_SSN LN_RPS_SEQ; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
