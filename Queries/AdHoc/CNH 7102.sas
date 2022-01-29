%LET RPTLIB = T:\SAS;
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

/*%MACRO SQLCHECK ;
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
%MEND  ;*/

PROC SQL;

	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE TEST AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
				SELECT DISTINCT
					PDXX.DF_SPE_ACC_ID AS ACCOUNT,
					LNXX.LN_SEQ AS LOAN_SEQUENCE,
					LNXX.LA_BIL_CUR_DU AS MONTHLY_PAYMENT_DUE,
					LNXX.LA_NSI_BIL AS MONTHLY_ACCRUEING_INTEREST,
					LNXX.LD_BIL_CRT AS BILL_DATE,
					LNXX.LN_SEQ_BIL_WI_DTE AS DATE_SEQ_NO,
					LNXX.LN_BIL_OCC_SEQ AS BILL_OCC_NO,
					LNXX.LC_TYP_SCH_DIS AS REPAYMENT_SCHEDULE
				FROM
					PKUB.LNXX_LON_BIL_CRF LNXX
					INNER JOIN 
						PKUB.LNXX_LON LNXX
						ON LNXX.BF_SSN = LNXX.BF_SSN
						AND LNXX.LN_SEQ = LNXX.LN_SEQ
						AND LNXX.LC_STA_LONXX = 'R' 
						AND LNXX.LA_CUR_PRI > X	
					INNER JOIN
						PKUB.PDXX_PRS_NME PDXX
						ON PDXX.DF_PRS_ID = LNXX.BF_SSN
					LEFT JOIN	
						PKUB.LNXX_LON_RPS LNXX
						ON LNXX.BF_SSN = LNXX.BF_SSN
						AND LNXX.LN_SEQ = LNXX.LN_SEQ
						AND LNXX.LC_STA_LONXX = 'A'
				WHERE
					LNXX.LA_BIL_CUR_DU < LNXX.LA_NSI_BIL 
					AND LNXX.LC_STA_LONXX = 'A' 
					AND LNXX.LC_BIL_TYP_LON = 'P'
					AND DAYS(CURRENT DATE) - DAYS(LNXX.LD_BIL_CRT) <= XX
					FOR READ ONLY WITH UR
				);

	CREATE TABLE FINAL AS
		SELECT
			ACCOUNT,
			LOAN_SEQUENCE,
			MONTHLY_PAYMENT_DUE,
			MONTHLY_ACCRUEING_INTEREST,
			REPAYMENT_SCHEDULE,
			MAX(BILL_DATE) as BILLDATE,
			MAX(DATE_SEQ_NO) as DATE_SEQ_NO,
			MAX(BILL_OCC_NO) as BILL_OCC_NO
		FROM
			Work.TEST
		GROUP BY
			ACCOUNT,
			LOAN_SEQUENCE,
			MONTHLY_PAYMENT_DUE,
			MONTHLY_ACCRUEING_INTEREST,
			REPAYMENT_SCHEDULE
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;
	%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA FINAL; SET LEGEND.FINAL; RUN;
/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.FINAL 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SheetX"; 
RUN;
