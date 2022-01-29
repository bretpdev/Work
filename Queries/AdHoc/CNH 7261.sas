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
						PDXX.DF_SPE_ACC_ID,
						LNXX.BF_SSN,
						LNXX.LN_SEQ,
						COALESCE(LNXX.LD_END_GRC_PRD, LNXX.LD_DSB) AS DATE_ENTERED_REPAYMENT,
						COALESCE(LNXX.LN_IBR_FGV_MTH_CTR,X) AS LN_IBR_FGV_MTH_CTR,
						COALESCE(LNXX.LN_ICR_FGV_MTH_CTR,X) AS LN_ICR_FGV_MTH_CTR,
						MONTHS_BETWEEN(CURRENT_DATE,COALESCE(LNXX.LD_END_GRC_PRD, LNXX.LD_LON_X_DSB)) AS TOTAL_MONTHS_IN_REPAYMENT,
						COALESCE(FORB.MONTHS_USED, X) AS FORB_MONTHS_USED,
						COALESCE(DEF.MONTHS_USED, X) AS DEF_MONTHS_USED
					FROM
						PKUB.PDXX_PRS_NME PDXX
						INNER JOIN PKUB.LNXX_LON LNXX
							ON PDXX.DF_PRS_ID = LNXX.BF_SSN
						INNER JOIN PKUB.LNXX_RPD_PIO_CVN LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
						INNER JOIN PKUB.RSXX_BR_RPD RSXX
							ON RSXX.BF_SSN = LNXX.BF_SSN
						INNER JOIN PKUB.LNXX_LON_RPS LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
							AND RSXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
						LEFT JOIN
						(
							SELECT
								BF_SSN,
								LN_SEQ,
								MAX(LD_DSB) AS LD_DSB
							FROM
								PKUB.LNXX_DSB
							WHERE
								LA_DSB - COALESCE(LA_DSB_CAN,X) > X
							GROUP BY
								BF_SSN,
								LN_SEQ

						)LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ 
						LEFT JOIN
						(
							SELECT
								LNXX.BF_SSN,
								LNXX.LN_SEQ,
								SUM(MONTHS_BETWEEN(POP.LD_FOR_END,POP.LD_FOR_BEG)) AS MONTHS_USED
							FROM
								PKUB.LNXX_BR_FOR_APV LNXX
							INNER JOIN
							(
								SELECT
									LNXX.BF_SSN,
									LNXX.LN_SEQ,
									LNXX.LF_FOR_CTL_NUM,
									LNXX.LN_FOR_OCC_SEQ,
									LNXX.LD_FOR_BEG,
									CASE
										WHEN DAYS(LNXX.LD_FOR_END) > DAYS(CURRENT_DATE) THEN CURRENT_DATE
										ELSE LNXX.LD_FOR_END
									END AS LD_FOR_END
								FROM 
									PKUB.LNXX_BR_FOR_APV LNXX
									INNER JOIN PKUB.FBXX_BR_FOR_REQ FBXX
										ON FBXX.BF_SSN = LNXX.BF_SSN
										AND FBXX.LF_FOR_CTL_NUM = LNXX.LF_FOR_CTL_NUM
								WHERE
									LNXX.LC_STA_LONXX = 'A'
									AND FBXX.LC_FOR_STA = 'A'
									AND FBXX.LC_STA_FORXX = 'A'
							)POP
								ON POP.BF_SSN = LNXX.BF_SSN
								AND POP.LN_SEQ = LNXX.LN_SEQ
								AND POP.LF_FOR_CTL_NUM = LNXX.LF_FOR_CTL_NUM
								AND POP.LN_FOR_OCC_SEQ = LNXX.LN_FOR_OCC_SEQ
							GROUP BY 
								LNXX.BF_SSN,
								LNXX.LN_SEQ
								
						)FORB
							ON FORB.BF_SSN = LNXX.BF_SSN
							AND FORB.LN_SEQ = LNXX.LN_SEQ
						LEFT JOIN
						(
							SELECT
								LNXX.BF_SSN,
								LNXX.LN_SEQ,
								SUM(MONTHS_BETWEEN(POP.LD_DFR_END, POP.LD_DFR_BEG)) AS MONTHS_USED
							FROM
								PKUB.LNXX_BR_DFR_APV LNXX
							INNER JOIN
							(
								SELECT
									LNXX.BF_SSN,
									LNXX.LN_SEQ,
									LNXX.LF_DFR_CTL_NUM,
									LNXX.LN_DFR_OCC_SEQ,
									LNXX.LD_DFR_BEG,
									CASE
										WHEN DAYS(LNXX.LD_DFR_END) > DAYS(CURRENT_DATE) THEN CURRENT_DATE
										ELSE LNXX.LD_DFR_END
									END AS LD_DFR_END
								FROM PKUB.LNXX_BR_DFR_APV LNXX
									INNER JOIN PKUB.DFXX_BR_DFR_REQ DFXX
										ON DFXX.BF_SSN = LNXX.BF_SSN
										AND DFXX.LF_DFR_CTL_NUM = LNXX.LF_DFR_CTL_NUM
								WHERE
									LNXX.LC_STA_LONXX = 'A'
									AND DFXX.LC_STA_DFRXX = 'A'
									AND DFXX.LC_DFR_STA = 'A'
								
							) POP
								ON POP.BF_SSN = LNXX.BF_SSN
								AND POP.LN_SEQ = LNXX.LN_SEQ
								AND POP.LF_DFR_CTL_NUM = LNXX.LF_DFR_CTL_NUM
								AND POP.LN_DFR_OCC_SEQ = LNXX.LN_DFR_OCC_SEQ
							GROUP BY 
								LNXX.BF_SSN,
								LNXX.LN_SEQ
						)DEF
							ON DEF.BF_SSN = LNXX.BF_SSN
							AND DEF.LN_SEQ = LNXX.LN_SEQ
					WHERE
						LNXX.LC_TYP_SCH_DIS IN ('IB', 'IL', 'CX', 'CX', 'CX', 'CQ', 'CA', 'CP', 'IX', 'IP') 
						AND LNXX.LC_STA_LONXX = 'A'
						AND RSXX.LC_STA_RPSTXX = 'A'
					ORDER BY 
						PDXX.DF_SPE_ACC_ID

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
	CREATE TABLE FINAL AS 
		SELECT
			*,
			(TOTAL_MONTHS_IN_REPAYMENT - FORB_MONTHS_USED - DEF_MONTHS_USED) AS ACTUAL_MONTHS_IN_REPAYMENT
		FROM
			DEMO
		WHERE
			LN_IBR_FGV_MTH_CTR > (TOTAL_MONTHS_IN_REPAYMENT - FORB_MONTHS_USED - DEF_MONTHS_USED)
			OR LN_ICR_FGV_MTH_CTR > (TOTAL_MONTHS_IN_REPAYMENT - FORB_MONTHS_USED - DEF_MONTHS_USED)
;
QUIT;

DATA TEST;
	SET FINAL;
	WHERE ACTUAL_MONTHS_IN_REPAYMENT < X;
RUN;
/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.FINAL 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SheetX"; 
RUN;

