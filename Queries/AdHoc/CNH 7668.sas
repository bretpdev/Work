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
					SELECT 
						LNXX.BF_SSN
						,LNXX.LN_SEQ
						,LNXX.LA_CUR_PRI
						,CASE
							WHEN DWXX.BF_SSN IS NOT NULL AND DWXX.LN_SEQ IS NOT NULL THEN 'Y'
							ELSE ' '
						 END AS STATI
						,DWXX.WC_DW_LON_STA
						,CASE
							WHEN (LNXXA.BF_SSN IS NOT NULL AND LNXXA.LN_SEQ IS NOT NULL) OR 
								 (LNXXI.BF_SSN IS NOT NULL AND LNXXI.LN_SEQ IS NOT NULL) THEN 'Y'
							ELSE ' '
						 END AS IDR
						 ,LNXXA.LC_TYP_SCH_DIS AS ACTIVE_LC_TYP_SCH_DIS
						 ,LNXXI.LC_TYP_SCH_DIS AS INACTIVE_LC_TYP_SCH_DIS
					FROM
						PKUB.LNXX_LON LNXX
						LEFT JOIN 
							PKUB.DWXX_DW_CLC_CLU DWXX
							ON LNXX.BF_SSN = DWXX.BF_SSN
							AND LNXX.LN_SEQ = DWXX.LN_SEQ
							AND DWXX.WC_DW_LON_STA NOT IN ('XX','XX','XX','XX','XX','XX','XX','XX')
						LEFT JOIN (
								SELECT
									LNXX.BF_SSN
									,LNXX.LN_SEQ
									,LNXX.LC_TYP_SCH_DIS
								FROM 
									PKUB.LNXX_LON_RPS LNXX
								WHERE 
									LNXX.LC_STA_LONXX = 'A'
									AND 
									LNXX.LC_TYP_SCH_DIS IN ('CQ','CX','CX','CX','IB','IL','IS','IX','CA','CP','IP')
									) LNXXA
							ON LNXX.BF_SSN = LNXXA.BF_SSN
							AND LNXX.LN_SEQ = LNXXA.LN_SEQ
						LEFT JOIN (
								SELECT
									LNXX.BF_SSN
									,LNXX.LN_SEQ
									,LNXX.LC_TYP_SCH_DIS
								FROM 
									PKUB.LNXX_LON_RPS LNXX
								JOIN (
										SELECT
											LNXX.BF_SSN
											,LNXX.LN_SEQ
											,MAX(LNXX.LN_RPS_SEQ) AS LN_RPS_SEQ
										FROM 
											PKUB.LNXX_LON_RPS LNXX
											JOIN (
													SELECT
														LNXX.BF_SSN
														,LNXX.LN_SEQ
													FROM 
														PKUB.LNXX_LON_RPS LNXX
													WHERE 
														LNXX.LC_STA_LONXX = 'A'
													) ACT
												ON LNXX.BF_SSN = ACT.BF_SSN
												AND LNXX.LN_SEQ = ACT.LN_SEQ
										WHERE 
											ACT.BF_SSN IS NULL 
											AND 
											ACT.LN_SEQ IS NULL
										GROUP BY
											LNXX.BF_SSN
											,LNXX.LN_SEQ
											) INACT
									ON LNXX.BF_SSN = INACT.BF_SSN
									AND LNXX.LN_SEQ = INACT.LN_SEQ
									AND LNXX.LN_RPS_SEQ = INACT.LN_RPS_SEQ
								WHERE 
									LNXX.LC_TYP_SCH_DIS IN ('CQ','CX','CX','CX','IB','IL','IS','IX','CA','CP','IP')
									) LNXXI
							ON LNXX.BF_SSN = LNXXI.BF_SSN
							AND LNXX.LN_SEQ = LNXXI.LN_SEQ
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

PROC SORT DATA=DEMO; BY BF_SSN LN_SEQ; RUN;

PROC SQL;
	CREATE TABLE TOTAL AS
		SELECT
			SUM(LA_CUR_PRI) AS TOTAL_PRINCIPAL_BALANCE
		FROM 
			DEMO
	;

	CREATE TABLE CERTAIN_STATUSES AS
		SELECT
			SUM(LA_CUR_PRI) AS TOTAL_BALANCE_SELECTED_STATUSES
		FROM 
			DEMO
		WHERE 
			STATI = 'Y'
	;

	CREATE TABLE IDR AS
		SELECT
			SUM(LA_CUR_PRI) AS TOTAL_BALANCE_IDR
		FROM 
			DEMO
		WHERE 
			IDR = 'Y'
	;
QUIT;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.TOTAL 
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="TOTAL"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.CERTAIN_STATUSES 
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SELECTED_STATUSES"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.IDR 
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="IDR"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NH XXXX TEST.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
