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
						,LNXX.LN_SEQ
						,LNXX.LC_TYP_SCH_DIS
						,ALM_MAX.LN_GRD_RPS_SEQ AS ALM_SEQ
						,ALM_MAX.LA_RPS_ISL AS NORMAL_AMT
						,MAX_RPS.LN_GRD_RPS_SEQ AS MAX_SEQ
						,MAX_RPS.LA_RPS_ISL AS BALLOON_AMT
					FROM
						PKUB.LNXX_LON LNXX
						INNER JOIN PKUB.LNXX_LON_RPS LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
						INNER JOIN PKUB.LNXX_LON_RPS_SPF LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
							AND LNXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
						INNER JOIN (
								SELECT
									A.BF_SSN
									,A.LN_SEQ
									,B.LA_RPS_ISL
									,MAX(B.LN_GRD_RPS_SEQ) AS LN_GRD_RPS_SEQ
								FROM 
									PKUB.LNXX_LON_RPS A
									INNER JOIN PKUB.LNXX_LON_RPS_SPF B
										ON A.BF_SSN = B.BF_SSN
										AND A.LN_SEQ = B.LN_SEQ
										AND A.LN_RPS_SEQ = B.LN_RPS_SEQ
								WHERE 
									A.LC_STA_LONXX = 'A'
								GROUP BY 
									A.BF_SSN
									,A.LN_SEQ
									,B.LA_RPS_ISL
									) MAX_RPS
							ON LNXX.BF_SSN = MAX_RPS.BF_SSN
							AND LNXX.LN_SEQ = MAX_RPS.LN_SEQ
							AND LNXX.LN_GRD_RPS_SEQ = MAX_RPS.LN_GRD_RPS_SEQ
							AND MAX_RPS.LN_GRD_RPS_SEQ > X
						INNER JOIN (
								SELECT
									A.BF_SSN
									,A.LN_SEQ
									,B.LA_RPS_ISL
									,MAX(B.LN_GRD_RPS_SEQ) AS LN_GRD_RPS_SEQ
								FROM 
									PKUB.LNXX_LON_RPS A
									INNER JOIN PKUB.LNXX_LON_RPS_SPF B
										ON A.BF_SSN = B.BF_SSN
										AND A.LN_SEQ = B.LN_SEQ
										AND A.LN_RPS_SEQ = B.LN_RPS_SEQ
								WHERE 
									A.LC_STA_LONXX = 'A'
								GROUP BY 
									A.BF_SSN
									,A.LN_SEQ
									,B.LA_RPS_ISL
									) ALM_MAX
							ON LNXX.BF_SSN = ALM_MAX.BF_SSN
							AND LNXX.LN_SEQ = ALM_MAX.LN_SEQ
							AND MAX_RPS.LN_GRD_RPS_SEQ - X = ALM_MAX.LN_GRD_RPS_SEQ
					WHERE
						LNXX.LA_CUR_PRI > X
						AND LNXX.LC_STA_LONXX = 'R'
						AND LNXX.LC_STA_LONXX = 'A'
						AND LNXX.LN_RPS_TRM = X

					FOR READ ONLY WITH UR
				)
	;


CREATE TABLE MAX AS
SELECT
	D.BF_SSN
	,D.LN_SEQ
	,D.LC_TYP_SCH_DIS
	,D.NORMAL_AMT
	,D.BALLOON_AMT
FROM
	DEMO D
	INNER JOIN (
				SELECT
					D.BF_SSN
					,D.LN_SEQ
					,MAX(D.MAX_SEQ) AS MAX_SEQ
				FROM
					DEMO D
				GROUP BY
					D.BF_SSN
					,D.LN_SEQ
				) MX
		ON D.BF_SSN = MX.BF_SSN
		AND D.LN_SEQ = MX.LN_SEQ
		AND D.MAX_SEQ = MX.MAX_SEQ
WHERE 
	D.BALLOON_AMT >= X * (D.NORMAL_AMT)
;
	
	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

/*DATA DEMO; SET LEGEND.DEMO; RUN;*/
DATA MAX; SET LEGEND.MAX; RUN;

PROC SORT DATA=MAX; BY BF_SSN; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.MAX
            OUTFILE = "T:\SAS\UPDATED NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
