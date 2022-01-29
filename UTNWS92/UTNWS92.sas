/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS92.NWS92RZ";
FILENAME REPORT2 "&RPTLIB/UNWS92.NWS92R2";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE 0  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @01 " ********************************************************************* "
              / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @01 " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @01 " ****  &SQLXMSG   **** "
              / @01 " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL NOPRINT;
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE TOTAL_POP AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						PD10.DF_SPE_ACC_ID,
						LN10.BF_SSN,
						LN10.LN_SEQ,
						LN10.LA_CUR_PRI,
						LN10.IC_LON_PGM,
						COALESCE(DW01.WA_TOT_BRI_OTS,DW01.LA_NSI_OTS) AS INTEREST
					FROM
						PKUB.PD10_PRS_NME PD10
						INNER JOIN PKUB.LN10_LON LN10
							ON PD10.DF_PRS_ID = LN10.BF_SSN
						INNER JOIN PKUB.LN90_FIN_ATY LN90
							ON LN90.BF_SSN = PD10.DF_PRS_ID
							AND LN90.LN_SEQ = LN10.LN_SEQ
							AND (LN90.PC_FAT_TYP = '01' AND LN90.PC_FAT_SUB_TYP = '01' AND LN90.LC_FAT_REV_REA = '' AND LN90.LC_STA_LON90 = 'A') 
						LEFT JOIN PKUB.DW01_DW_CLC_CLU DW01
							ON DW01.BF_SSN = LN10.BF_SSN
							AND DW01.LN_SEQ = LN10.LN_SEQ
					WHERE
						LN10.LA_CUR_PRI > 0
						AND LN10.LC_STA_LON10 = 'R'

					FOR READ ONLY WITH UR
				)
;
	DISCONNECT FROM DB2;

	SELECT
		COUNT(DISTINCT DF_SPE_ACC_ID) INTO :TOTAL_COUNT /*SAVE THE TOTAL COUNT IN A VARIABLE*/
	FROM
		TOTAL_POP
;

	CREATE TABLE IN_SCHOOL AS
		SELECT DISTINCT
			TP.DF_SPE_ACC_ID,
			TP.LN_SEQ,
			TP.LA_CUR_PRI,
			TP.INTEREST
		FROM
			TOTAL_POP TP
			LEFT JOIN /*INSCHOOL BORROWERS*/
			(
				SELECT
					BF_SSN,
					LN_SEQ
				FROM
					PKUB.DW01_DW_CLC_CLU DW01
				WHERE
					DW01.WC_DW_LON_STA = '02'
			)SCHOOL
				ON SCHOOL.BF_SSN = TP.BF_SSN
				AND SCHOOL.LN_SEQ = TP.LN_SEQ
			LEFT JOIN /*SCHOOL DEFEREMNT BORROWERS*/
			(
				SELECT 
					LN50.BF_SSN,
					LN50.LN_SEQ
				FROM
					PKUB.LN50_BR_DFR_APV LN50
				INNER JOIN PKUB.DF10_BR_DFR_REQ DF10
					ON DF10.BF_SSN = LN50.BF_SSN
					AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
				WHERE
					DF10.LC_DFR_TYP IN ('15','18')
					AND DF10.LC_DFR_STA = 'A'
					AND LN50.LC_STA_LON50 = 'A'
					AND TODAY() BETWEEN LN50.LD_DFR_BEG AND LN50.LD_DFR_END

			) DEFERMENT
				ON DEFERMENT.BF_SSN = TP.BF_SSN
				AND DEFERMENT.LN_SEQ = TP.LN_SEQ
		WHERE
			(SCHOOL.BF_SSN AND SCHOOL.LN_SEQ IS NOT NULL) OR (DEFERMENT.BF_SSN AND DEFERMENT.LN_SEQ IS NOT NULL) 
;

	CREATE TABLE IN_GRACE AS
		SELECT DISTINCT
			TP.DF_SPE_ACC_ID,
			TP.LN_SEQ,
			TP.LA_CUR_PRI,
			TP.INTEREST
		FROM
			TOTAL_POP TP
		INNER JOIN PKUB.DW01_DW_CLC_CLU DW01
			ON DW01.BF_SSN = TP.BF_SSN
			AND DW01.LN_SEQ = TP.LN_SEQ
		WHERE
			DW01.WC_DW_LON_STA = '01'
;

	CREATE TABLE IN_REPAYMENT AS
		SELECT DISTINCT
			TP.DF_SPE_ACC_ID,
			TP.LN_SEQ,
			TP.LA_CUR_PRI,
			TP.INTEREST,
			TP.IC_LON_PGM,
			CASE
				WHEN SD10.LC_REA_SCL_SPR NOT IN ('01','00','10','11','19') THEN 'N'
				ELSE 'Y'
			END AS GRADUATED
		FROM
			TOTAL_POP TP
		INNER JOIN PKUB.DW01_DW_CLC_CLU DW01
			ON DW01.BF_SSN = TP.BF_SSN
			AND DW01.LN_SEQ = TP.LN_SEQ
		LEFT JOIN PKUB.LN13_LON_STU_OSD LN13
			ON LN13.LF_STU_SSN = TP.BF_SSN
			AND LN13.LN_SEQ = TP.LN_SEQ
			AND LN13.LC_STA_LON13 = 'A'
		LEFT JOIN PKUB.SD10_STU_SPR SD10
			ON SD10.LF_STU_SSN = TP.BF_SSN
			AND SD10.LC_STA_STU10 = 'A'
			AND LN13.LN_STU_SPR_SEQ = SD10.LN_STU_SPR_SEQ
		WHERE
			DW01.WC_DW_LON_STA = '03'
;
	CREATE TABLE SKIPS AS 
		SELECT DISTINCT
			TP.DF_SPE_ACC_ID,
			TP.LN_SEQ,
			TP.LA_CUR_PRI,
			TP.INTEREST
		FROM
			TOTAL_POP TP
		INNER JOIN PKUB.PD26_PRS_SKP_PRC PD26
			ON PD26.BF_SSN = TP.BF_SSN
		WHERE PD26.DC_STA_SKP = '2'
;
	CREATE TABLE DELINQUENCY AS 
		SELECT DISTINCT
			TP.DF_SPE_ACC_ID,
			TP.LN_SEQ,
			TP.LA_CUR_PRI,
			TP.INTEREST,
			(LN16.LN_DLQ_MAX + 1) AS DAYS_DELQ
		FROM
			TOTAL_POP TP
		INNER JOIN PKUB.LN16_LON_DLQ_HST LN16
			ON LN16.BF_SSN = TP.BF_SSN
			AND LN16.LN_SEQ = TP.LN_SEQ
			AND LN16.LC_STA_LON16 = '1'
			AND (LN16.LN_DLQ_MAX + 1) BETWEEN 5 AND 361
;

QUIT;

%MACRO DELQ_BUCKETS(NEWSET, BEGIN_DAYS, END_DAYS);
	DATA &NEWSET;
		SET DELINQUENCY;
		WHERE DAYS_DELQ BETWEEN &BEGIN_DAYS AND &END_DAYS;
	RUN;
%MEND DELQ_BUCKETS;

%DELQ_BUCKETS(D5T30, 5,30);
%DELQ_BUCKETS(D31T60, 31,60);
%DELQ_BUCKETS(D61T90, 61,90);
%DELQ_BUCKETS(D91T120, 91,120);
%DELQ_BUCKETS(D121T150, 121,150);
%DELQ_BUCKETS(D151T180, 151,180);
%DELQ_BUCKETS(D181T210, 181,210);
%DELQ_BUCKETS(D211T240, 211,240);
%DELQ_BUCKETS(D241T270, 241,270);
%DELQ_BUCKETS(D271T300, 271,300);
%DELQ_BUCKETS(D301T330, 301,330);
%DELQ_BUCKETS(D331T360, 331,360);

PROC SQL;
	CREATE TABLE FINAL AS 
		SELECT 
			'TRANSFERRED FROM COD' AS TITLE,
			COUNT(DISTINCT DF_SPE_ACC_ID) AS BORROWER_COUNT,
			COUNT(*) AS LOAN_COUNT,
			SUM(LA_CUR_PRI + COALESCE(INTEREST,0)) AS TOTAL_DOLLAR_AMOUNT,
			(COUNT(DISTINCT DF_SPE_ACC_ID) / &TOTAL_COUNT) AS PERCENT
		FROM
			TOTAL_POP

		UNION ALL

		SELECT
			'IN SCHOOL' AS TITLE,
			COUNT(DISTINCT DF_SPE_ACC_ID) AS BORROWER_COUNT,
			COUNT(*) AS LOAN_COUNT,
			SUM(LA_CUR_PRI + COALESCE(INTEREST,0)) AS TOTAL_DOLLAR_AMOUNT,
			(COUNT(DISTINCT DF_SPE_ACC_ID) / &TOTAL_COUNT) AS PERCENT
		FROM
			IN_SCHOOL

		UNION ALL

		SELECT
			'IN GRACE' AS TITLE,
			COUNT(DISTINCT DF_SPE_ACC_ID) AS BORROWER_COUNT,
			COUNT(*) AS LOAN_COUNT,
			SUM(LA_CUR_PRI + COALESCE(INTEREST,0)) AS TOTAL_DOLLAR_AMOUNT,
			(COUNT(DISTINCT DF_SPE_ACC_ID) / &TOTAL_COUNT) AS PERCENT
		FROM
			IN_GRACE

		UNION ALL

		SELECT
			'IN REPAYMENT' AS TITLE,
			COUNT(DISTINCT DF_SPE_ACC_ID) AS BORROWER_COUNT,
			COUNT(*) AS LOAN_COUNT,
			SUM(LA_CUR_PRI + COALESCE(INTEREST,0)) AS TOTAL_DOLLAR_AMOUNT,
			(COUNT(DISTINCT DF_SPE_ACC_ID) / &TOTAL_COUNT) AS PERCENT
		FROM
			IN_REPAYMENT

		UNION ALL

		SELECT
			'IN REPAYMENT (NOT GRADUATED)' AS TITLE,
			COUNT(DISTINCT DF_SPE_ACC_ID) AS BORROWER_COUNT,
			COUNT(*) AS LOAN_COUNT,
			SUM(LA_CUR_PRI + COALESCE(INTEREST,0)) AS TOTAL_DOLLAR_AMOUNT,
			(COUNT(DISTINCT DF_SPE_ACC_ID) / &TOTAL_COUNT) AS PERCENT
		FROM
			IN_REPAYMENT
		WHERE
			GRADUATED = 'N'
			AND IC_LON_PGM NOT IN ('DLPLUS','DPLUS','DLPCNS','DLPLGB','DLSCNS','DLUCNS','DLUSPL','DLSSPL')

		UNION ALL

		SELECT
			'SKIPS' AS TITLE,
			COUNT(DISTINCT DF_SPE_ACC_ID) AS BORROWER_COUNT,
			COUNT(*) AS LOAN_COUNT,
			SUM(LA_CUR_PRI + COALESCE(INTEREST,0)) AS TOTAL_DOLLAR_AMOUNT,
			(COUNT(DISTINCT DF_SPE_ACC_ID) / &TOTAL_COUNT) AS PERCENT
		FROM
			SKIPS

		UNION ALL

		SELECT
			'DELQ DAYS BETWEEN 5 AND 360' AS TITLE,
			COUNT(DISTINCT DF_SPE_ACC_ID) AS BORROWER_COUNT,
			COUNT(*) AS LOAN_COUNT,
			SUM(LA_CUR_PRI + COALESCE(INTEREST,0)) AS TOTAL_DOLLAR_AMOUNT,
			(COUNT(DISTINCT DF_SPE_ACC_ID) / &TOTAL_COUNT) AS PERCENT
		FROM
			DELINQUENCY

		UNION ALL

		SELECT
			'DELQ (5-30) DAYS' AS TITLE,
			COUNT(DISTINCT DF_SPE_ACC_ID) AS BORROWER_COUNT,
			COUNT(*) AS LOAN_COUNT,
			SUM(LA_CUR_PRI + COALESCE(INTEREST,0)) AS TOTAL_DOLLAR_AMOUNT,
			(COUNT(DISTINCT DF_SPE_ACC_ID) / &TOTAL_COUNT) AS PERCENT
		FROM
			D5T30

		UNION ALL

		SELECT
			'DELQ (31-60) DAYS' AS TITLE,
			COUNT(DISTINCT DF_SPE_ACC_ID) AS BORROWER_COUNT,
			COUNT(*) AS LOAN_COUNT,
			SUM(LA_CUR_PRI + COALESCE(INTEREST,0)) AS TOTAL_DOLLAR_AMOUNT,
			(COUNT(DISTINCT DF_SPE_ACC_ID) / &TOTAL_COUNT) AS PERCENT
		FROM
			D31T60

		UNION ALL

		SELECT
			'DELQ (61-90) DAYS' AS TITLE,
			COUNT(DISTINCT DF_SPE_ACC_ID) AS BORROWER_COUNT,
			COUNT(*) AS LOAN_COUNT,
			COALESCE(SUM(LA_CUR_PRI + INTEREST),0)AS TOTAL_DOLLAR_AMOUNT,
			(COUNT(DISTINCT DF_SPE_ACC_ID) / &TOTAL_COUNT) AS PERCENT
		FROM
			D61T90

		UNION ALL

		SELECT
			'DELQ (91-120) DAYS' AS TITLE,
			COUNT(DISTINCT DF_SPE_ACC_ID) AS BORROWER_COUNT,
			COUNT(*) AS LOAN_COUNT,
			SUM(LA_CUR_PRI + COALESCE(INTEREST,0)) AS TOTAL_DOLLAR_AMOUNT,
			(COUNT(DISTINCT DF_SPE_ACC_ID) / &TOTAL_COUNT) AS PERCENT
		FROM
			D91T120

		UNION ALL

		SELECT
			'DELQ (121-150) DAYS' AS TITLE,
			COUNT(DISTINCT DF_SPE_ACC_ID) AS BORROWER_COUNT,
			COUNT(*) AS LOAN_COUNT,
			SUM(LA_CUR_PRI + COALESCE(INTEREST,0)) AS TOTAL_DOLLAR_AMOUNT,
			(COUNT(DISTINCT DF_SPE_ACC_ID) / &TOTAL_COUNT) AS PERCENT
		FROM
			D121T150

		UNION ALL

		SELECT
			'DELQ (151-180) DAYS' AS TITLE,
			COUNT(DISTINCT DF_SPE_ACC_ID) AS BORROWER_COUNT,
			COUNT(*) AS LOAN_COUNT,
			SUM(LA_CUR_PRI + COALESCE(INTEREST,0)) AS TOTAL_DOLLAR_AMOUNT,
			(COUNT(DISTINCT DF_SPE_ACC_ID) / &TOTAL_COUNT) AS PERCENT
		FROM
			D151T180

		UNION ALL

		SELECT
			'DELQ (181-210) DAYS' AS TITLE,
			COUNT(DISTINCT DF_SPE_ACC_ID) AS BORROWER_COUNT,
			COUNT(*) AS LOAN_COUNT,
			SUM(LA_CUR_PRI + COALESCE(INTEREST,0)) AS TOTAL_DOLLAR_AMOUNT,
			(COUNT(DISTINCT DF_SPE_ACC_ID) / &TOTAL_COUNT) AS PERCENT
		FROM
			D181T210

		UNION ALL

		SELECT
			'DELQ (211-240) DAYS' AS TITLE,
			COUNT(DISTINCT DF_SPE_ACC_ID) AS BORROWER_COUNT,
			COUNT(*) AS LOAN_COUNT,
			SUM(LA_CUR_PRI + COALESCE(INTEREST,0)) AS TOTAL_DOLLAR_AMOUNT,
			(COUNT(DISTINCT DF_SPE_ACC_ID) / &TOTAL_COUNT) AS PERCENT
		FROM
			D211T240

		UNION ALL

		SELECT
			'DELQ (241-270) DAYS' AS TITLE,
			COUNT(DISTINCT DF_SPE_ACC_ID) AS BORROWER_COUNT,
			COUNT(*) AS LOAN_COUNT,
			SUM(LA_CUR_PRI + COALESCE(INTEREST,0)) AS TOTAL_DOLLAR_AMOUNT,
			(COUNT(DISTINCT DF_SPE_ACC_ID) / &TOTAL_COUNT) AS PERCENT
		FROM
			D241T270

		UNION ALL

		SELECT
			'DELQ (271-300) DAYS' AS TITLE,
			COUNT(DISTINCT DF_SPE_ACC_ID) AS BORROWER_COUNT,
			COUNT(*) AS LOAN_COUNT,
			SUM(LA_CUR_PRI + COALESCE(INTEREST,0)) AS TOTAL_DOLLAR_AMOUNT,
			(COUNT(DISTINCT DF_SPE_ACC_ID) / &TOTAL_COUNT) AS PERCENT
		FROM
			D271T300

		UNION ALL

		SELECT
			'DELQ (301-330) DAYS' AS TITLE,
			COUNT(DISTINCT DF_SPE_ACC_ID) AS BORROWER_COUNT,
			COUNT(*) AS LOAN_COUNT,
			SUM(LA_CUR_PRI + COALESCE(INTEREST,0)) AS TOTAL_DOLLAR_AMOUNT,
			((COUNT(*) / &TOTAL_COUNT)) AS PERCENT
		FROM
			D301T330

		UNION ALL

		SELECT
			'DELQ (331-360) DAYS' AS TITLE,
			COUNT(DISTINCT DF_SPE_ACC_ID) AS BORROWER_COUNT,
			COUNT(*) AS LOAN_COUNT,
			SUM(LA_CUR_PRI + COALESCE(INTEREST,0)) AS TOTAL_DOLLAR_AMOUNT,
			(COUNT(DISTINCT DF_SPE_ACC_ID) / &TOTAL_COUNT) AS PERCENT
		FROM
			D331T360
;			
QUIT;

ENDRSUBMIT;

/*data total_pop;*/
/*set legend.total_pop;*/
/*run;*/
/**/
/*PROC EXPORT DATA = WORK.total_pop */
/*            OUTFILE = "T:\SAS\EXCEL OUTPUT.xlsx" */
/*            DBMS = EXCEL*/
/*			REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/

DATA FINAL;
	SET LEGEND.FINAL;
RUN;

DATA FINAL;
	SET FINAL;
	TOTAL_DOLLAR_AMOUNT = COALESCE(TOTAL_DOLLAR_AMOUNT,0);
RUN;


/*create printed report*/
PROC PRINTTO PRINT=REPORT2 NEW; RUN;

OPTIONS ORIENTATION=LANDSCAPE PS=39 LS=127;
TITLE 		'COD Weekly Summary Report - FED';
TITLE2		"RUNDATE &SYSDATE9";
FOOTNOTE1  	"THIS DOCUMENT MAY CONTAIN BORROWERS' SENSITIVE INFORMATION THAT UHEAA HAS PLEDGED TO PROTECT.";
FOOTNOTE2	'PLEASE TAKE APPROPRIATE PRECAUTIONS TO SAFEGUARD THIS INFORMATION.';
FOOTNOTE3	;
FOOTNOTE4   'JOB = UTNWS92  	 REPORT = UTNWS92.NWS92R2';

PROC PRINT 
		NOOBS SPLIT = '/' 
		DATA = FINAL 
		WIDTH = UNIFORM 
		WIDTH = MIN 
		LABEL;

	FORMAT
		TOTAL_DOLLAR_AMOUNT DOLLAR25.2
		PERCENT PERCENT10.3
;

	VAR 
		TITLE	
		BORROWER_COUNT
		LOAN_COUNT
		TOTAL_DOLLAR_AMOUNT
		PERCENT
	;

	LABEL
		TITLE = 'TITLE'
		BORROWER_COUNT = 'COUNT OF BORROWERS'
		LOAN_COUNT = 'COUNT OF LOANS'
		TOTAL_DOLLAR_AMOUNT = 'TOTAL DOLLAR AMOUNT'
		PERCENT = 'PERCENT OF TOTAL'
	;
RUN;

PROC PRINTTO; RUN;