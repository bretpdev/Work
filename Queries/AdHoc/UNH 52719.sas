/**============= UTLWS14 BILLING STATEMENT ==================;*/
/**Duster;*/
%LET SRVR = DUSTER;
%LET DB = DLGSUTWH;
/**----------------------------------------------;*/
*uncomment for TESTING, comment for PROMOTION;
	%LET RPTLIB = T:\SAS;
	%LET RPTLIBX = T:\SAS; 
	%SYSLPUT SRVR = &SRVR;
	%SYSLPUT DB = &DB;
/**----------------------------------------------;*/

%LET days_ago_7 = '01/01/2016';*begin date range;
%LET days_ago_0 = '07/20/2017';*end date range;

%SYSLPUT days_ago_7 = &days_ago_7;
%SYSLPUT days_ago_0 = &days_ago_0;
%LET SSN = '1%';
%LET SSN =
 '100468620'
,'100508066'
,'100542751'
,'100662197'
,'100669436'
,'100704505'
,'100707549'
,'100708783'
,'100709579'
,'100720821'
,'100723181'
,'100724075'
,'100743459'
,'100746519'
,'100764424'
,'100766643'
,'100768581'
,'100769557'
,'100786152'
,'100843246'
,'100866170'
,'101663262'
,'101665800'
,'101682412'
,'101686289'
,'101700301'
,'101724103'
,'101741491'
,'101760857'
,'101765496'
,'101786333'
,'101806506'
,'101824660'
,'101844473'
,'101846462'
,'101885468'
,'101900540'
,'101964354'
,'102405250'
,'102505942'
,'102589432'
,'102609159'
,'102668394'
,'102705550'
,'102741430'
,'102746414'
,'102747676'
,'102749779'
,'102760994'
,'102783745'
,'102786716'
,'102788372'
;

%SYSLPUT SSN = &SSN;
LIBNAME  DUSTER  REMOTE  SERVER=&SRVR SLIBREF=WORK;
RSUBMIT;
LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1;
PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB); 
/*	gets all fees and bills due within specified time period*/
	CREATE TABLE DUES AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
			(
				SELECT DISTINCT
					LN10.BF_SSN
					,LN80A.LN_SEQ
					,LN80A.LC_STA_LON80						AS LN80
					,BL10.LD_BIL_CRT
					,BL10.LD_BIL_DU
					,COALESCE(LN80A.LA_BIL_PAS_DU,0)		AS PAST_DU
					,COALESCE(LN80A.LA_BIL_CUR_DU,0)		AS CUR_DU
					,CASE
						WHEN BL10.LC_BIL_TYP = 'C'
						THEN COALESCE(LN80A.LA_BIL_CUR_DU,0)
						ELSE COALESCE(LN80A.LA_BIL_DU_PRT,0)
						END	AS BL_CUR_DU
					,COALESCE(LN80A.LA_LTE_FEE_OTS_PRT,0)	AS FEES
					,BL10.LD_LST_PAY_LST_STM
					,COALESCE(BL10.LA_INT_PD_LST_STM,0) 	AS LA_INT_PD_LST_STM
					,COALESCE(BL10.LA_FEE_PD_LST_STM,0)		AS LA_FEE_PD_LST_STM
					,COALESCE(BL10.LA_PRI_PD_LST_STM,0)		AS LA_PRI_PD_LST_STM
					,case
						when bl10.lc_bil_typ = 'C'
						then bl10.lc_bil_typ
						else ''
					end as bil_typ_flag /*put C into Wave 5*/
				FROM
					OLWHRM1.LN10_LON LN10
					INNER JOIN OLWHRM1.BL10_BR_BIL BL10
						ON LN10.BF_SSN = BL10.BF_SSN
					INNER JOIN OLWHRM1.LN80_LON_BIL_CRF LN80
						ON LN10.BF_SSN = LN80.BF_SSN
						AND LN10.LN_SEQ = LN80.LN_SEQ
					INNER JOIN OLWHRM1.LN80_LON_BIL_CRF LN80A
						ON LN80A.BF_SSN = BL10.BF_SSN
						AND DAYS(LN80A.LD_BIL_CRT) = DAYS(BL10.LD_BIL_CRT)
						AND LN80A.LN_SEQ_BIL_WI_DTE = BL10.LN_SEQ_BIL_WI_DTE
				WHERE
					LN10.BF_SSN IN (&SSN)
/*					LN10.BF_SSN LIKE &SSN*/
					AND LN10.LA_CUR_PRI + COALESCE(LN10.LA_NSI_OTS,0) > 0.00
					AND BL10.LC_STA_BIL10 = 'A'
					AND BL10.LC_BIL_TYP IN ('P','I','C')
					AND BL10.LC_IND_BIL_SNT IN ('6','1','2','4','7','G','A','B','C','D','E','F','H','I','J','K','L','M','P','Q','R','8','T')
					AND DAYS(BL10.LD_BIL_CRT) BETWEEN DAYS(&days_ago_7) AND DAYS(&days_ago_0)

			FOR READ ONLY WITH UR
			)
	;
/*	gets when past-due bills were satisfied*/
	CREATE TABLE SATISFIED_BILL AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
			(
				SELECT
					LN80.BF_SSN
					,LN80.LD_BIL_CRT
					,LN80.LN_SEQ_BIL_WI_DTE
					,LN80.LN_BIL_OCC_SEQ
					,SUM(LN75.LA_BIL_STS) AS SUM_LA_BIL_STS
					,LN80.LA_BIL_CUR_DU
					,MAX(LN90.LD_FAT_PST) AS MAX_LD_FAT_PST
				FROM
					OLWHRM1.BL10_BR_BIL BL10
					INNER JOIN OLWHRM1.LN80_LON_BIL_CRF LN80
						ON BL10.BF_SSN = LN80.BF_SSN
						AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
						AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
					INNER JOIN OLWHRM1.LN75_BIL_LON_FAT LN75
						ON LN75.BF_SSN = LN80.BF_SSN
						AND LN75.LN_SEQ = LN80.LN_SEQ
						AND LN75.LD_BIL_CRT = LN80.LD_BIL_CRT
						AND LN75.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
						AND LN75.LN_BIL_OCC_SEQ = LN80.LN_BIL_OCC_SEQ
					INNER JOIN OLWHRM1.LN90_FIN_ATY LN90
						ON LN75.BF_SSN = LN90.BF_SSN
						AND LN75.LN_SEQ = LN90.LN_SEQ
						AND LN75.LN_FAT_SEQ = LN90.LN_FAT_SEQ
				WHERE
					BL10.BF_SSN IN (&SSN)
/*					BL10.BF_SSN LIKE &SSN*/
					AND BL10.LC_STA_BIL10 = 'A'
					AND BL10.LC_BIL_TYP IN ('P','I','C')
					AND BL10.LC_IND_BIL_SNT IN ('6','1','2','4','7','G','A','B','C','D','E','F','H','I','J','K','L','M','P','Q','R','8','T')
					AND DAYS(BL10.LD_BIL_CRT) BETWEEN DAYS(&days_ago_7) AND DAYS(&days_ago_0)
					AND LN90.PC_FAT_TYP = '10'
					AND LN90.LC_FAT_REV_REA = ' '
					AND LN90.LC_STA_LON90 = 'A'
				GROUP BY
					LN80.BF_SSN
					,LN80.LN_SEQ
					,LN80.LD_BIL_CRT
					,LN80.LN_SEQ_BIL_WI_DTE
					,LN80.LN_BIL_OCC_SEQ
					,LN80.LA_BIL_CUR_DU
			FOR READ ONLY WITH UR
			)
	;
/*	gets delinquency dates*/
	CREATE TABLE DAYS_DQ AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
			(
				SELECT DISTINCT 
					LN80.BF_SSN
					,BL10.LD_BIL_CRT
					,BL10.LD_BIL_DU
					,CASE
						WHEN LN80A.LA_BIL_PAS_DU <> 0.00
						THEN 'DQ'
						ELSE NULL
					END AS DQ_FLAG
					,CASE
						WHEN LN80A.LA_BIL_PAS_DU = 0.00
						THEN BL10.LD_BIL_DU
						ELSE NULL
					END AS DQ_DATE
				FROM
					OLWHRM1.LN10_LON LN10
					INNER JOIN OLWHRM1.BL10_BR_BIL BL10
						ON LN10.BF_SSN = BL10.BF_SSN
					INNER JOIN OLWHRM1.LN80_LON_BIL_CRF LN80
						ON LN10.BF_SSN = LN80.BF_SSN
						AND LN10.LN_SEQ = LN80.LN_SEQ
					INNER JOIN OLWHRM1.LN80_LON_BIL_CRF LN80A
						ON LN80A.BF_SSN = BL10.BF_SSN
						AND LN80A.LD_BIL_CRT = BL10.LD_BIL_CRT
						AND LN80A.LN_SEQ_BIL_WI_DTE = BL10.LN_SEQ_BIL_WI_DTE
				WHERE
					BL10.BF_SSN IN (&SSN)
/*					BL10.BF_SSN LIKE &SSN*/
					AND BL10.LD_BIL_CRT > '2015-01-01'/*looks this far back to get comprehensive dq history*/
					AND LN10.LA_CUR_PRI + COALESCE(LN10.LA_NSI_OTS,0) > 0.00
					AND LN80.LC_STA_LON80 = 'A'
					AND BL10.LC_STA_BIL10 = 'A'
					AND BL10.LC_BIL_TYP IN ('P','I','C')
					AND BL10.LC_IND_BIL_SNT IN ('6','1','2','4','7','G','A','B','C','D','E','F','H','I','J','K','L','M','P','Q','R','8','T')					
			FOR READ ONLY WITH UR
			)
	;
	/*borrowers needing special handling due to converting a loan onto the system with a delq.*/
	/*flag to eliminate from Wave 3 and deal with in Wave 5*/
	/*comment out if running Waves 1 & 2*/
	create table prev_servicer_dq as
		select distinct
			*
		from
			connection to db2
			(
				select
					bl10.bf_ssn
/*					,bl10.LD_BIL_DU*/
/*					,bl10.LD_BIL_CRT*/
/*					,case*/
/*						when bl10.ld_bil_du < bl10.ld_bil_crt*/
/*						and bl10.ld_bil_crt < ln10.ld_lon_acl_add*/
/*						then 1*/
/*						else 0*/
/*					end as prev_servicer_flag*/
				from
					OLWHRM1.LN10_LON LN10
					INNER JOIN OLWHRM1.BL10_BR_BIL BL10
						ON LN10.BF_SSN = BL10.BF_SSN
					INNER JOIN OLWHRM1.LN80_LON_BIL_CRF LN80
						ON LN10.BF_SSN = LN80.BF_SSN
						AND LN10.LN_SEQ = LN80.LN_SEQ
					INNER JOIN OLWHRM1.LN80_LON_BIL_CRF LN80A
						ON LN80A.BF_SSN = BL10.BF_SSN
						AND LN80A.LD_BIL_CRT = BL10.LD_BIL_CRT
						AND LN80A.LN_SEQ_BIL_WI_DTE = BL10.LN_SEQ_BIL_WI_DTE
				where
					BL10.BF_SSN IN (&SSN)
/*					BL10.BF_SSN LIKE &SSN*/
					AND days(bl10.ld_bil_du) < days(bl10.ld_bil_crt)
					and days(bl10.ld_bil_crt) < days(ln10.ld_lon_acl_add)
					AND LN10.LA_CUR_PRI + COALESCE(LN10.LA_NSI_OTS,0) > 0.00
					AND LN80.LC_STA_LON80 = 'A'
					AND BL10.LC_STA_BIL10 = 'A'
					AND BL10.LC_BIL_TYP IN ('P','I','C')
			)
	;
DISCONNECT FROM DB2;
QUIT;
ENDRSUBMIT;

DATA DUSTER_DAYS_DQ; SET DUSTER.DAYS_DQ; RUN;
DATA DUSTER_DUES; SET DUSTER.DUES; RUN;
DATA DUSTER_SATISFIED_BILL; SET DUSTER.SATISFIED_BILL; RUN;
data duster_prev_servicer_dq; set duster.prev_servicer_dq; run;*needed only for Waves 3 & 5;

/********************************************************************
	flag these different populations, handle each separately

	WAVES 1-5:

	WAVE 1) Past Due = N & RPS Change = N
	WAVE 3) Past Due = Y & RPS Change = N

	WAVE 2) Past Due = N & RPS Change = Y
	WAVE 4) Past Due = Y & RPS Change = Y

	WAVE 5) Other special circumstances that might arise

*******************************************************************/


/*****************************  WAVE 1: ALL GOOD!  *******************************************************/

DATA DAYS_DQ; SET DUSTER_DAYS_DQ; RUN;
DATA DUES; SET DUSTER_DUES; RUN;
DATA SATISFIED_BILL; SET DUSTER_SATISFIED_BILL; RUN;

*summary of all past-due bills;
PROC SQL;
	CREATE TABLE _DUES_TOTAL AS
	SELECT
		BF_SSN
		,LN80
		,LD_BIL_CRT
		,LD_BIL_DU
		,SUM(PAST_DU) AS PAST_DU
		,SUM(CUR_DU) AS CUR_DU
		,SUM(BL_CUR_DU) AS BL_CUR_DU
		,SUM(PAST_DU + BL_CUR_DU) AS TOT_DU
		,SUM(FEES) AS FEES
		,SUM(PAST_DU + BL_CUR_DU + FEES) AS TOT_W_FEES
		,(LA_INT_PD_LST_STM + LA_FEE_PD_LST_STM + LA_PRI_PD_LST_STM) AS PMT_AMT
	FROM
		DUES
	GROUP BY
		BF_SSN
		,LN80
		,LD_BIL_CRT
		,LD_BIL_DU
		,LA_INT_PD_LST_STM
		,LA_FEE_PD_LST_STM
		,LA_PRI_PD_LST_STM
	ORDER BY
		BF_SSN
		,LD_BIL_CRT DESC
	;
QUIT;

PROC SQL;
*flag past due sums if 0 or >0 for time period >= 12-01-2016;
	CREATE TABLE PAST_DUE_FLAGS AS
	SELECT DISTINCT
		DT.BF_SSN
		,CASE
			WHEN SUM_PAST.PAST_DU_TOT = 0
			THEN '0'
			ELSE '>0'
		END AS PAST_DUE_FLAG
	FROM 
		_DUES_TOTAL DT
		LEFT JOIN
		(/*add up all past due amounts*/
			SELECT
				BF_SSN
				,SUM(PAST_DU) AS PAST_DU_TOT
			FROM
				_DUES_TOTAL
			WHERE
				LN80 = 'A'
				AND LD_BIL_CRT >= MDY(12,01,2016)
			GROUP BY
				BF_SSN
		)SUM_PAST
			ON DT.BF_SSN = SUM_PAST.BF_SSN
	;

*check for RPS changes for time period >= 12-01-2016;
	CREATE TABLE _RPS_FLAGS AS
	SELECT
		MIN_RPS.BF_SSN
		,MIN_RPS.MIN_BL_CUR_DU
		,MAX_RPS.MAX_BL_CUR_DU
	FROM
	(
		SELECT
			BF_SSN
			,MIN(BL_CUR_DU) AS MIN_BL_CUR_DU
		FROM
			_DUES_TOTAL
		WHERE
			LN80 = 'A'
			AND LD_BIL_CRT >= MDY(12,01,2016)
		GROUP BY
			BF_SSN	
	) MIN_RPS
		INNER JOIN
	(
		SELECT
			BF_SSN
			,MAX(BL_CUR_DU) AS MAX_BL_CUR_DU
		FROM
			_DUES_TOTAL
		WHERE
			LN80 = 'A'
			AND LD_BIL_CRT >= MDY(12,01,2016)
		GROUP BY
			BF_SSN	
	) MAX_RPS
		ON MIN_RPS.BF_SSN = MAX_RPS.BF_SSN
	;
QUIT;

*changes min and max numbers to characters for comparison because of a weird SAS number thing;
DATA RPS_FLAGS;
	SET _RPS_FLAGS;
		_MAX = PUT(MAX_BL_CUR_DU,Z10.2);
		_MIN = PUT(MIN_BL_CUR_DU,Z10.2);
	IF _MAX = _MIN
	THEN RPS_FLAG = 'NO CHANGES';
	ELSE RPS_FLAG = 'CHANGED';
RUN;

*get only Wave 1) Past Due = N & RPS Change = N;
PROC SQL;
	CREATE TABLE DUES_TOTAL AS
	SELECT
		DT.*
	FROM
		_DUES_TOTAL DT
		INNER JOIN PAST_DUE_FLAGS PDF
			ON DT.BF_SSN = PDF.BF_SSN
		INNER JOIN RPS_FLAGS RF
			ON DT.BF_SSN = RF.BF_SSN
	WHERE
		PDF.PAST_DUE_FLAG = '0'
		AND RF.RPS_FLAG = 'NO CHANGES'
		AND DT.LN80 = 'A'
		AND DT.LD_BIL_CRT >= MDY(12,01,2016)
	;
QUIT;

*puts date satisfied onto summary of past due bills;
PROC SQL;
	CREATE TABLE DUES_TOTAL_2 AS
	SELECT DISTINCT
		DT.*,
		SB.MAX_LD_FAT_PST AS DT_SATIS
	FROM
		DUES_TOTAL DT
		LEFT JOIN SATISFIED_BILL SB
			ON DT.BF_SSN = SB.BF_SSN
			AND DT.LD_BIL_CRT = SB.LD_BIL_CRT
	ORDER BY
		DT.BF_SSN
		,DT.LD_BIL_CRT DESC
	;
QUIT;
PROC SORT DATA=DUES_TOTAL_2;
	BY BF_SSN LD_BIL_CRT;
QUIT;

*get previous bill's due date to use as delinquency start date when 1st delinquency occurs;
PROC SORT DATA=DAYS_DQ;
	BY BF_SSN LD_BIL_CRT;
RUN;
DATA DAYS_DQ;
	SET DAYS_DQ;
	COUNT+1;
	BY BF_SSN LD_BIL_CRT;
	IF FIRST.BF_SSN THEN COUNT=1;
RUN;
PROC SQL;
	CREATE TABLE DAYS_DQ_2 AS
	SELECT
		A.BF_SSN
		,A.LD_BIL_CRT
		,A.LD_BIL_DU
		,A.DQ_FLAG
		,CASE
			WHEN A.DQ_FLAG IS NULL 
			THEN .
			ELSE B.DQ_DATE
		END AS DQ_DATE FORMAT=DATE9.
	FROM 
		DAYS_DQ A
		LEFT JOIN DAYS_DQ B
			ON A.BF_SSN = B.BF_SSN
			AND A.COUNT = B.COUNT + 1
	;
QUIT;

*puts delinquency start date onto bill summary table;
PROC SQL;
	CREATE TABLE DUES_DQ AS
	SELECT 
		DT.*
		,DQ.DQ_DATE
	FROM
		DUES_TOTAL_2 DT
		LEFT JOIN DAYS_DQ_2 DQ
			ON DT.BF_SSN = DQ.BF_SSN
			AND DT.LD_BIL_CRT = DQ.LD_BIL_CRT
	;
QUIT;
PROC SORT DATA=DUES_DQ;
	BY BF_SSN LD_BIL_CRT;
QUIT;

*number of months to add to date of delq at date billed;
DATA MONTHS_TO_MOVE;
	SET DUES_DQ;
	*counter for ease of testing;
	COUNT+1;
	BY BF_SSN LD_BIL_CRT;
	IF FIRST.BF_SSN THEN COUNT=1;

	*If payment is more than current, then move delinquency date forward by the number
	of times exceeded. Any partially-satisfied gets moved forward;
	PCT_SATIS_BASE = DIVIDE(PMT_AMT,BL_CUR_DU);
	MONTH_MOVE = INT(PCT_SATIS_BASE);
	REMAINDER = PCT_SATIS_BASE - INT(PCT_SATIS_BASE);

	*cumulative totals;
	MONTH_MOVE_CUM + MONTH_MOVE;
	REMAINDER_CUM + REMAINDER;

	*reset cumulative counts based on bf_ssn;
	IF FIRST.BF_SSN THEN MONTH_MOVE_CUM=0;
	IF FIRST.BF_SSN THEN REMAINDER_CUM=0;
	IF PAST_DU = 0.00 THEN MONTH_MOVE = 0;
	IF PAST_DU = 0.00 THEN REMAINDER = 0;
	IF PAST_DU = 0.00 THEN MONTH_MOVE_CUM = 0;
	IF PAST_DU = 0.00 THEN REMAINDER_CUM = 0;
RUN;

*setting base DQ date for use in adding months to move;
DATA MONTHS_TO_MOVE_2;
	DROP TEMP;
	SET MONTHS_TO_MOVE;
	BY BF_SSN;
	RETAIN TEMP;
	IF FIRST.BF_SSN THEN TEMP=.;
	IF DQ_DATE NE . THEN TEMP=DQ_DATE;
	ELSE IF DQ_DATE=. THEN DQ_DATE=TEMP;
	IF PAST_DU = 0.00 THEN DQ_DATE = .;
RUN;

*uses data set from above in final dq date calc;
DATA MONTHS_TO_MOVE_3;
	SET MONTHS_TO_MOVE_2;
	FORMAT DQ_DATE_ADJ DATE9.;

	*accounts for cumulative remainders greater than 1;
	REMAINDER_GE_1 = INT(REMAINDER_CUM);
	REMAINDER_LT_1 = REMAINDER_CUM - INT(REMAINDER_CUM);
	MONTH_MOVE_FINAL = REMAINDER_GE_1 + MONTH_MOVE_CUM;

	*delinquent date and days calc;
	IF DQ_DATE ^= . THEN DQ_DATE_ADJ = INTNX("MONTH",DQ_DATE,MONTH_MOVE_FINAL,"SAME");
	DAYS_DQ = LD_BIL_CRT - DQ_DATE_ADJ;
RUN;

/*prep output for excel*/
PROC SQL;
	CREATE TABLE PRELIM AS
		SELECT DISTINCT
			BF_SSN
			,LN80
			,LD_BIL_CRT 	FORMAT=MMDDYY10.
			,LD_BIL_DU 		FORMAT=MMDDYY10.
			,PAST_DU
			,BL_CUR_DU
			,TOT_DU
			,FEES
			,TOT_W_FEES
			,PMT_AMT
			,MAX(DT_SATIS) AS DT_SATIS FORMAT=MMDDYY10.
			,DQ_DATE_ADJ 	FORMAT=MMDDYY10.
			,DAYS_DQ
		FROM
			MONTHS_TO_MOVE_3
		WHERE
			LN80 = 'A'
			AND LD_BIL_CRT >= MDY(12,01,2016)
		GROUP BY
			BF_SSN
			,LN80
			,LD_BIL_CRT
			,LD_BIL_DU
			,PAST_DU
			,BL_CUR_DU
			,TOT_DU
			,FEES
			,TOT_W_FEES
			,PMT_AMT
			,DQ_DATE_ADJ
			,DAYS_DQ
	;
QUIT;

/**view output;*/
/*PROC REPORT DATA=PRELIM;*/
/*	COMPUTE BF_SSN;*/
/*	COUNT+1;*/
/*	IF MOD(COUNT,2) THEN */
/*		DO;*/
/*			CALL DEFINE(_ROW_, "STYLE", "STYLE=[BACKGROUND=#DCDCDC");*/
/*		END;*/
/*	ENDCOMP;*/
/*RUN;*/

PROC EXPORT
	DATA = PRELIM
	OUTFILE = "T:\SAS\Wave_1_billing_&SSN._SSN.xlsx" 
/*	OUTFILE = "T:\SAS\billing_original_pop.xlsx"*/
	DBMS = EXCEL
	REPLACE;
	SHEET="Wave_1_SSN_&SSN.";
RUN;

PROC DATASETS NOPRINT;
	DELETE 
		DAYS_DQ
		DAYS_DQ_2
		DUES
		DUES_DQ
		DUES_TOTAL
		DUES_TOTAL_2
		MONTHS_TO_MOVE
		MONTHS_TO_MOVE_2
		MONTHS_TO_MOVE_3
		PAST_DUE_FLAGS
		PRELIM
		RPS_FLAGS
		SATISFIED_BILL
		_DUES_TOTAL
		_RPS_FLAGS
		_DAYS_DQ
		;
QUIT;

/********************************************************************
	flag these different populations, handle each separately

	WAVES 1-5:

	WAVE 1) Past Due = N & RPS Change = N
	WAVE 3) Past Due = Y & RPS Change = N

	WAVE 2) Past Due = N & RPS Change = Y
	WAVE 4) Past Due = Y & RPS Change = Y

	WAVE 5) Other special circumstances that might arise

*******************************************************************/


/*****************************  WAVE 2: ALL GOOD!  *******************************************************/

DATA DAYS_DQ; SET DUSTER_DAYS_DQ; RUN;
DATA DUES; SET DUSTER_DUES; RUN;
DATA SATISFIED_BILL; SET DUSTER_SATISFIED_BILL; RUN;

*summary of all past-due bills;
PROC SQL;
	CREATE TABLE _DUES_TOTAL AS
	SELECT
		BF_SSN
		,LN80
		,LD_BIL_CRT
		,LD_BIL_DU
		,SUM(PAST_DU) AS PAST_DU
		,SUM(CUR_DU) AS CUR_DU
		,SUM(BL_CUR_DU) AS BL_CUR_DU
		,SUM(PAST_DU + BL_CUR_DU) AS TOT_DU
		,SUM(FEES) AS FEES
		,SUM(PAST_DU + BL_CUR_DU + FEES) AS TOT_W_FEES
		,(LA_INT_PD_LST_STM + LA_FEE_PD_LST_STM + LA_PRI_PD_LST_STM) AS PMT_AMT
	FROM
		DUES
	GROUP BY
		BF_SSN
		,LN80
		,LD_BIL_CRT
		,LD_BIL_DU
		,LA_INT_PD_LST_STM
		,LA_FEE_PD_LST_STM
		,LA_PRI_PD_LST_STM
	ORDER BY
		BF_SSN
		,LD_BIL_CRT DESC
	;
QUIT;

PROC SQL;
*flag past due sums if 0 or >0 for time period >= 12-01-2016;
	CREATE TABLE PAST_DUE_FLAGS AS
	SELECT DISTINCT
		DT.BF_SSN
		,CASE
			WHEN SUM_PAST.PAST_DU_TOT = 0
			THEN '0'
			ELSE '>0'
		END AS PAST_DUE_FLAG
	FROM 
		_DUES_TOTAL DT
		LEFT JOIN
		(/*add up all past due amounts*/
			SELECT
				BF_SSN
				,SUM(PAST_DU) AS PAST_DU_TOT
			FROM
				_DUES_TOTAL
			WHERE
				LN80 = 'A'
				AND LD_BIL_CRT >= MDY(12,01,2016)
			GROUP BY
				BF_SSN
		)SUM_PAST
			ON DT.BF_SSN = SUM_PAST.BF_SSN
	;

*check for RPS changes for time period >= 12-01-2016;
	CREATE TABLE _RPS_FLAGS AS
	SELECT
		MIN_RPS.BF_SSN
		,MIN_RPS.MIN_BL_CUR_DU
		,MAX_RPS.MAX_BL_CUR_DU
	FROM
	(
		SELECT
			BF_SSN
			,MIN(BL_CUR_DU) AS MIN_BL_CUR_DU
		FROM
			_DUES_TOTAL
		WHERE
			LN80 = 'A'
			AND LD_BIL_CRT >= MDY(12,01,2016)
		GROUP BY
			BF_SSN	
	) MIN_RPS
		INNER JOIN
	(
		SELECT
			BF_SSN
			,MAX(BL_CUR_DU) AS MAX_BL_CUR_DU
		FROM
			_DUES_TOTAL
		WHERE
			LN80 = 'A'
			AND LD_BIL_CRT >= MDY(12,01,2016)
		GROUP BY
			BF_SSN	
	) MAX_RPS
		ON MIN_RPS.BF_SSN = MAX_RPS.BF_SSN
	;
QUIT;

*changes min and max numbers to characters for comparison because of a weird SAS number thing;
DATA RPS_FLAGS;
	SET _RPS_FLAGS;
		_MAX = PUT(MAX_BL_CUR_DU,Z10.2);
		_MIN = PUT(MIN_BL_CUR_DU,Z10.2);
	IF _MAX = _MIN
	THEN RPS_FLAG = 'NO CHANGES';
	ELSE RPS_FLAG = 'CHANGED';
RUN;

* get only Wave 2) Past Due = N & RPS Change = Y;
PROC SQL;
	CREATE TABLE DUES_TOTAL AS
	SELECT
		DT.*
	FROM
		_DUES_TOTAL DT
		INNER JOIN PAST_DUE_FLAGS PDF
			ON DT.BF_SSN = PDF.BF_SSN
		INNER JOIN RPS_FLAGS RF
			ON DT.BF_SSN = RF.BF_SSN
	WHERE
		PDF.PAST_DUE_FLAG = '0'
		AND RF.RPS_FLAG = 'CHANGED'
		AND DT.LN80 = 'A'
/*		AND DT.LD_BIL_CRT >= MDY(12,01,2016)*/
	;
QUIT;

*puts date satisfied onto summary of past due bills;
PROC SQL;
	CREATE TABLE DUES_TOTAL_2 AS
	SELECT DISTINCT
		DT.*,
		SB.MAX_LD_FAT_PST AS DT_SATIS
	FROM
		DUES_TOTAL DT
		LEFT JOIN SATISFIED_BILL SB
			ON DT.BF_SSN = SB.BF_SSN
			AND DT.LD_BIL_CRT = SB.LD_BIL_CRT
	ORDER BY
		DT.BF_SSN
		,DT.LD_BIL_CRT DESC
	;
QUIT;
PROC SORT DATA=DUES_TOTAL_2;
	BY BF_SSN LD_BIL_CRT;
QUIT;

*get previous bill's due date to use as delinquency start date when 1st delinquency occurs;
PROC SORT DATA=DAYS_DQ;
	BY BF_SSN LD_BIL_CRT;
RUN;
DATA DAYS_DQ;
	SET DAYS_DQ;
	COUNT+1;
	BY BF_SSN LD_BIL_CRT;
	IF FIRST.BF_SSN THEN COUNT=1;
RUN;
PROC SQL;
	CREATE TABLE DAYS_DQ_2 AS
	SELECT
		A.BF_SSN
		,A.LD_BIL_CRT
		,A.LD_BIL_DU
		,A.DQ_FLAG
		,CASE
			WHEN A.DQ_FLAG IS NULL 
			THEN .
			ELSE B.DQ_DATE
		END AS DQ_DATE FORMAT=DATE9.
	FROM 
		DAYS_DQ A
		LEFT JOIN DAYS_DQ B
			ON A.BF_SSN = B.BF_SSN
			AND A.COUNT = B.COUNT + 1
	;
QUIT;

*puts delinquency start date onto bill summary table;
PROC SQL;
	CREATE TABLE DUES_DQ AS
	SELECT 
		DT.*
		,DQ.DQ_DATE
	FROM
		DUES_TOTAL_2 DT
		LEFT JOIN DAYS_DQ_2 DQ
			ON DT.BF_SSN = DQ.BF_SSN
			AND DT.LD_BIL_CRT = DQ.LD_BIL_CRT
	;
QUIT;
PROC SORT DATA=DUES_DQ;
	BY BF_SSN LD_BIL_CRT;
QUIT;

*number of months to add to date of delq at date billed;
DATA MONTHS_TO_MOVE;
	SET DUES_DQ;
	*counter for ease of testing;
	COUNT+1;
	BY BF_SSN LD_BIL_CRT;
	IF FIRST.BF_SSN THEN COUNT=1;

	*If payment is more than current, then move delinquency date forward by the number
	of times exceeded. Any partially-satisfied gets moved forward;
	PCT_SATIS_BASE = DIVIDE(PMT_AMT,BL_CUR_DU);
	MONTH_MOVE = INT(PCT_SATIS_BASE);
	REMAINDER = PCT_SATIS_BASE - INT(PCT_SATIS_BASE);

	*cumulative totals;
	MONTH_MOVE_CUM + MONTH_MOVE;
	REMAINDER_CUM + REMAINDER;

	*reset cumulative counts based on bf_ssn;
	IF FIRST.BF_SSN THEN MONTH_MOVE_CUM=0;
	IF FIRST.BF_SSN THEN REMAINDER_CUM=0;
	IF PAST_DU = 0.00 THEN MONTH_MOVE = 0;
	IF PAST_DU = 0.00 THEN REMAINDER = 0;
	IF PAST_DU = 0.00 THEN MONTH_MOVE_CUM = 0;
	IF PAST_DU = 0.00 THEN REMAINDER_CUM = 0;
RUN;

*setting base DQ date for use in adding months to move;
DATA MONTHS_TO_MOVE_2;
	DROP TEMP;
	SET MONTHS_TO_MOVE;
	BY BF_SSN;
	RETAIN TEMP;
	IF FIRST.BF_SSN THEN TEMP=.;
	IF DQ_DATE NE . THEN TEMP=DQ_DATE;
	ELSE IF DQ_DATE=. THEN DQ_DATE=TEMP;
	IF PAST_DU = 0.00 THEN DQ_DATE = .;
RUN;

*uses data set from above in final dq date calc;
DATA MONTHS_TO_MOVE_3;
	SET MONTHS_TO_MOVE_2;
	FORMAT DQ_DATE_ADJ DATE9.;

	*accounts for cumulative remainders greater than 1;
	REMAINDER_GE_1 = INT(REMAINDER_CUM);
	REMAINDER_LT_1 = REMAINDER_CUM - INT(REMAINDER_CUM);
	MONTH_MOVE_FINAL = REMAINDER_GE_1 + MONTH_MOVE_CUM;

	*delinquent date and days calc;
	IF DQ_DATE ^= . THEN DQ_DATE_ADJ = INTNX("MONTH",DQ_DATE,MONTH_MOVE_FINAL,"SAME");
	DAYS_DQ = LD_BIL_CRT - DQ_DATE_ADJ;
RUN;

/*prep output for excel*/
PROC SQL;
	CREATE TABLE PRELIM AS
		SELECT DISTINCT
			BF_SSN
			,LN80
			,LD_BIL_CRT 	FORMAT=MMDDYY10.
			,LD_BIL_DU 		FORMAT=MMDDYY10.
			,PAST_DU
			,BL_CUR_DU
			,TOT_DU
			,FEES
			,TOT_W_FEES
			,PMT_AMT
			,MAX(DT_SATIS) AS DT_SATIS FORMAT=MMDDYY10.
			,DQ_DATE_ADJ 	FORMAT=MMDDYY10.
			,DAYS_DQ
		FROM
			MONTHS_TO_MOVE_3
		WHERE
			LN80 = 'A'
			AND LD_BIL_CRT >= MDY(12,01,2016)
		GROUP BY
			BF_SSN
			,LN80
			,LD_BIL_CRT
			,LD_BIL_DU
			,PAST_DU
			,BL_CUR_DU
			,TOT_DU
			,FEES
			,TOT_W_FEES
			,PMT_AMT
			,DQ_DATE_ADJ
			,DAYS_DQ
	;
QUIT;

/**view output;*/
/*PROC REPORT DATA=PRELIM;*/
/*	COMPUTE BF_SSN;*/
/*	COUNT+1;*/
/*	IF MOD(COUNT,2) THEN */
/*		DO;*/
/*			CALL DEFINE(_ROW_, "STYLE", "STYLE=[BACKGROUND=#DCDCDC");*/
/*		END;*/
/*	ENDCOMP;*/
/*RUN;*/

PROC EXPORT
	DATA = PRELIM
	OUTFILE = "T:\SAS\Wave_2_billing_&SSN._SSN.xlsx" 
/*	OUTFILE = "T:\SAS\billing_original_pop.xlsx"*/
	DBMS = EXCEL
	REPLACE;
	SHEET="Wave_2_SSN_&SSN.";
RUN;
;
PROC DATASETS NOPRINT;
	DELETE 
		DAYS_DQ
		DAYS_DQ_2
		DUES
		DUES_DQ
		DUES_TOTAL
		DUES_TOTAL_2
		MONTHS_TO_MOVE
		MONTHS_TO_MOVE_2
		MONTHS_TO_MOVE_3
		PAST_DUE_FLAGS
		PRELIM
		RPS_FLAGS
		SATISFIED_BILL
		_DUES_TOTAL
		_RPS_FLAGS
		_DAYS_DQ
		;
QUIT;











/********************************************************************
	flag these different populations, handle each separately

	WAVES 1-5:

	WAVE 1) Past Due = N & RPS Change = N
	WAVE 3) Past Due = Y & RPS Change = N

	WAVE 2) Past Due = N & RPS Change = Y
	WAVE 4) Past Due = Y & RPS Change = Y

	WAVE 5) Other special circumstances that might arise

*******************************************************************/


/*****************************  WAVE 3: WORK IN PROGRESS  *******************************************************/

* removes borrowers who converted a loan onto the system with a delq.;
PROC SQL;
	CREATE TABLE DAYS_DQ AS
	SELECT
		dq.*
	FROM
		DUSTER_DAYS_DQ dq
		left join duster_prev_servicer_dq psd
			on dq.bf_ssn = psd.bf_ssn
	WHERE
		PSD.BF_SSN IS NULL
	;

	CREATE TABLE DUES AS
	SELECT
		dd.*
	FROM
		DUSTER_DUES dd
		left join duster_prev_servicer_dq psd
			on dd.bf_ssn = psd.bf_ssn
	WHERE
		PSD.BF_SSN IS NULL
	;

	CREATE TABLE SATISFIED_BILL AS
	SELECT
		dsb.*
	FROM
		DUSTER_SATISFIED_BILL dsb
		left join duster_prev_servicer_dq psd
			on dsb.bf_ssn = psd.bf_ssn
	WHERE
		PSD.BF_SSN IS NULL
	;
QUIT;

*summary of all past-due bills;
PROC SQL;
	CREATE TABLE _DUES_TOTAL AS
	SELECT
		BF_SSN
		,LN80
		,LD_BIL_CRT
		,LD_BIL_DU
		,SUM(PAST_DU) AS PAST_DU
		,SUM(CUR_DU) AS CUR_DU
		,SUM(BL_CUR_DU) AS BL_CUR_DU
		,SUM(PAST_DU + BL_CUR_DU) AS TOT_DU
		,SUM(FEES) AS FEES
		,SUM(PAST_DU + BL_CUR_DU + FEES) AS TOT_W_FEES
		,(LA_INT_PD_LST_STM + LA_FEE_PD_LST_STM + LA_PRI_PD_LST_STM) AS PMT_AMT
		,bil_typ_flag
	FROM
		DUES
	GROUP BY
		BF_SSN
		,LN80
		,LD_BIL_CRT
		,LD_BIL_DU
		,LA_INT_PD_LST_STM
		,LA_FEE_PD_LST_STM
		,LA_PRI_PD_LST_STM
		,bil_typ_flag
	ORDER BY
		BF_SSN
		,LD_BIL_CRT DESC
	;
QUIT;

PROC SQL;
*flag past due sums if 0 or >0 for time period >= 12-01-2016;
	CREATE TABLE PAST_DUE_FLAGS AS
	SELECT DISTINCT
		DT.BF_SSN
		,CASE
			WHEN SUM_PAST.PAST_DU_TOT = 0
			THEN '0'
			ELSE '>0'
		END AS PAST_DUE_FLAG
	FROM 
		_DUES_TOTAL DT
		LEFT JOIN
		(/*add up all past due amounts*/
			SELECT
				BF_SSN
				,SUM(PAST_DU) AS PAST_DU_TOT
			FROM
				_DUES_TOTAL
			WHERE
				LN80 = 'A'
				AND LD_BIL_CRT >= MDY(12,01,2016)
			GROUP BY
				BF_SSN
		)SUM_PAST
			ON DT.BF_SSN = SUM_PAST.BF_SSN
	;

*check for RPS changes for time period >= 12-01-2016;
	CREATE TABLE _RPS_FLAGS AS
	SELECT
		MIN_RPS.BF_SSN
		,MIN_RPS.MIN_BL_CUR_DU
		,MAX_RPS.MAX_BL_CUR_DU
	FROM
	(
		SELECT
			BF_SSN
			,MIN(BL_CUR_DU) AS MIN_BL_CUR_DU
		FROM
			_DUES_TOTAL
		WHERE
			LN80 = 'A'
			AND LD_BIL_CRT >= MDY(12,01,2016)
		GROUP BY
			BF_SSN	
	) MIN_RPS
		INNER JOIN
	(
		SELECT
			BF_SSN
			,MAX(BL_CUR_DU) AS MAX_BL_CUR_DU
		FROM
			_DUES_TOTAL
		WHERE
			LN80 = 'A'
			AND LD_BIL_CRT >= MDY(12,01,2016)
		GROUP BY
			BF_SSN	
	) MAX_RPS
		ON MIN_RPS.BF_SSN = MAX_RPS.BF_SSN
	;
QUIT;

*changes min and max numbers to characters for comparison because of a weird SAS number thing;
DATA RPS_FLAGS;
	SET _RPS_FLAGS;
		_MAX = PUT(MAX_BL_CUR_DU,Z10.2);
		_MIN = PUT(MIN_BL_CUR_DU,Z10.2);
	IF _MAX = _MIN
	THEN RPS_FLAG = 'NO CHANGES';
	ELSE RPS_FLAG = 'CHANGED';
RUN;

* get only Wave 3) Past Due = Y & RPS Change = N;
* also removes bil typ C;
PROC SQL;
	CREATE TABLE DUES_TOTAL AS
	SELECT
		DT.*
	FROM
		_DUES_TOTAL DT
		INNER JOIN PAST_DUE_FLAGS PDF
			ON DT.BF_SSN = PDF.BF_SSN
		INNER JOIN RPS_FLAGS RF
			ON DT.BF_SSN = RF.BF_SSN
		left join
		(/*removes C bil types from pop*/
			select
				bf_ssn
			from
				_dues_total
			where
				bil_typ_flag = 'C'
				and LD_BIL_CRT >= MDY(12,01,2016)
		)c_bil
			on dt.bf_ssn = c_bil.bf_ssn
	WHERE
		c_bil.bf_ssn is null
		and PDF.PAST_DUE_FLAG = '>0'
		AND RF.RPS_FLAG = 'NO CHANGES'
		AND DT.LN80 = 'A'
	;
QUIT;

*puts date satisfied onto summary of past due bills;
PROC SQL;
	CREATE TABLE DUES_TOTAL_2 AS
	SELECT DISTINCT
		DT.*,
		SB.MAX_LD_FAT_PST AS DT_SATIS
	FROM
		DUES_TOTAL DT
		LEFT JOIN SATISFIED_BILL SB
			ON DT.BF_SSN = SB.BF_SSN
			AND DT.LD_BIL_CRT = SB.LD_BIL_CRT
	ORDER BY
		DT.BF_SSN
		,DT.LD_BIL_CRT DESC
	;
QUIT;
PROC SORT DATA=DUES_TOTAL_2;
	BY BF_SSN LD_BIL_CRT;
QUIT;

*prep to get previous bill's due date to use as delinquency start date when 1st delinquency occurs;
PROC SORT DATA=DAYS_DQ;
	BY BF_SSN LD_BIL_CRT;
RUN;

*flag and remove duplicate bills;
DATA _DAYS_DQ;
	SET DAYS_DQ;
	BY BF_SSN LD_BIL_CRT;
	IF LAST.LD_BIL_CRT THEN KEEPER=1; 
	IF KEEPER=1;
RUN;

*put counter to use in next data step;
DATA _DAYS_DQ;
	SET _DAYS_DQ;
	COUNT+1;
	BY BF_SSN LD_BIL_CRT;
	IF FIRST.BF_SSN THEN COUNT=1;
RUN;

*get previous bill's due date to use as delinquency start date when 1st delinquency occurs;
PROC SQL;
	CREATE TABLE DAYS_DQ_2 AS
	SELECT DISTINCT
		A.BF_SSN
		,A.LD_BIL_CRT
		,A.LD_BIL_DU
		,A.DQ_FLAG
		,CASE
			WHEN A.DQ_FLAG IS NULL 
			THEN .
			ELSE B.DQ_DATE
		END AS DQ_DATE FORMAT=DATE9.
	FROM 
		_DAYS_DQ A
		LEFT JOIN _DAYS_DQ B
			ON A.BF_SSN = B.BF_SSN
			AND A.COUNT = B.COUNT + 1
	;
QUIT;

*puts delinquency start date onto bill summary table, and accounts for missing bill create months;
proc sql;
	create table dues_dq as
	select
		 dq.bf_ssn
		,tot.ln80
		,dq.ld_bil_crt
		,dq.ld_bil_du
		,tot.past_du
		,tot.cur_du
		,tot.bl_cur_du
		,tot.tot_du
		,tot.fees
		,tot.tot_w_fees
		,tot.pmt_amt
		,tot.dt_satis
		,dq.dq_date
	from
		days_dq_2 dq
		left join dues_total_2 tot
			on dq.bf_ssn = tot.bf_ssn
			and dq.ld_bil_crt = tot.ld_bil_crt
	;
quit;
PROC SORT DATA=DUES_DQ;
	BY BF_SSN LD_BIL_CRT;
QUIT;

*setting base DQ date for use in adding months to move;
DATA MONTHS_TO_MOVE;
	DROP TEMP;
	SET dues_dq;
	count_pk+1;
	BY BF_SSN;
	RETAIN TEMP;
	IF FIRST.BF_SSN THEN TEMP=.;
	IF DQ_DATE NE . THEN TEMP=DQ_DATE;
	ELSE IF DQ_DATE=. THEN DQ_DATE=TEMP;
	dq_date_pk = dq_date;
	if dq_date_pk =. then dq_date_pk = ld_bil_du;
	IF PAST_DU = 0.00 THEN DQ_DATE = .;
	IF FIRST.BF_SSN THEN COUNT_pk=1;
RUN;






/*trying to get payment amount to apply to earliest past due bill*/

proc print; where bf_ssn = '100743459';quit;

proc sql; 
select *
from satisfied_bill
where bf_ssn = '100743459'
order by ld_bil_crt
;quit;


proc sql;
select * 
from months_to_move
where bf_ssn = '100743459'
order by ld_bil_crt
;quit;


proc sql;
create table past_ as
select
	count_pk
	,dq_date_pk
	,bf_ssn
/*	,ld_bil_crt*/
	,past_du
/*	,pmt_amt*/
from
	months_to_move
where 
	past_du ^=0
	and past_du ^=.
/*	and bf_ssn = '100743459'*/
;
create table pmt_ as
select
	count_pk
	,dq_date_pk
	,bf_ssn
/*	,ld_bil_crt*/
/*	,past_du*/
	,pmt_amt
from
	months_to_move
where 
	pmt_amt ^=0
	and pmt_amt ^=.
/*	and bf_ssn = '100743459'*/
;quit;

proc sort data=past_; by bf_ssn dq_date_pk count_pk; run;
proc sort data=pmt_; by bf_ssn dq_date_pk count_pk; run;
proc print; run;


DATA _past_;
	SET past_;
	sub_pk+1;
	BY bf_ssn dq_date_pk count_pk;
	IF first.dq_date_pk THEN sub_pk=1;
RUN;

DATA _pmt_;
	SET pmt_;
	sub_pk+1;
	BY bf_ssn dq_date_pk count_pk;
	IF first.dq_date_pk THEN sub_pk=1;
RUN;
proc print; where bf_ssn = '100743459';run;


/*join up past_ and pmt_ data sets and 




select * from openquery (duster,'
	select
		ln75.bf_ssn
		,ln75.ln_seq
		,ln75.ld_bil_crt
		,ln75.ln_seq_bil_wi_dte
		,ln75.ln_bil_occ_seq
		,ln75.la_bil_sts
		--,sum(ln75.la_bil_sts) as la_bil_sts
		,ln90.ld_fat_pst
	from 
		olwhrm1.ln75_bil_lon_fat ln75
		inner join olwhrm1.ln90_fin_aty ln90
			on ln75.bf_ssn = ln90.bf_ssn
			and ln75.ln_seq = ln90.ln_seq
			and ln75.ln_fat_seq = ln90.ln_fat_seq
	where
		ln90.pc_fat_typ = ''10''
		and (
				ln90.lc_fat_rev_rea = ''''
				or ln90.lc_fat_rev_rea is null
			)
		and ln90.lc_sta_lon90 = ''A''
		and ln75.bf_ssn = ''100743459''
	--group by
	--	ln75.bf_ssn
	--	,ln75.ln_seq
	--	,ln75.ld_bil_crt
	--	,ln75.ln_seq_bil_wi_dte
	--	,ln75.ln_bil_occ_seq
');
	

select * from openquery (duster,'
select
	 ln80.bf_ssn
	,ln80.ld_bil_crt
	,sum(ln75.la_bil_sts)
	,ln80.la_bil_cur_du
from 
	olwhrm1.ln80_lon_bil_crf ln80
	inner join
	(
		select
			ln75.bf_ssn
			,ln75.ln_seq
			,ln75.ld_bil_crt
			,ln75.ln_seq_bil_wi_dte
			,ln75.ln_bil_occ_seq
			,ln75.la_bil_sts
			--,sum(ln75.la_bil_sts) as la_bil_sts
		from 
			olwhrm1.ln90_fin_aty ln90
			inner join olwhrm1.ln75_bil_lon_fat ln75
				on ln75.bf_ssn = ln90.bf_ssn
				and ln75.ln_seq = ln90.ln_seq
				and ln75.ln_fat_seq = ln90.ln_fat_seq
		where
			ln90.pc_fat_typ = ''10''
			and (
					ln90.lc_fat_rev_rea = ''''
					or ln90.lc_fat_rev_rea is null
				)
			and ln90.lc_sta_lon90 = ''A''
			and ln75.bf_ssn = ''100743459''
		--group by
		--	ln75.bf_ssn
		--	,ln75.ln_seq
		--	,ln75.ld_bil_crt
		--	,ln75.ln_seq_bil_wi_dte
		--	,ln75.ln_bil_occ_seq
	)ln75
		on ln75.bf_ssn = ln80.bf_ssn 
		and ln75.ln_seq = ln80.ln_seq
		and ln75.ld_bil_crt = ln80.ld_bil_crt
		and ln75.ln_seq_bil_wi_dte = ln80.ln_seq_bil_wi_dte
		and ln75.ln_bil_occ_seq = ln80.ln_bil_occ_seq
where
	ln80.bf_ssn = ''100743459''
group by
	 ln80.bf_ssn
	,ln80.ld_bil_crt
	,ln80.la_bil_cur_du
');
	










































*number of months to add to date of delq at date billed;
DATA MONTHS_TO_MOVE_2;
	SET months_to_move;
	*counter for ease of testing;
	COUNT+1;
	BY BF_SSN LD_BIL_CRT;
	IF FIRST.BF_SSN THEN COUNT=1;

	*If payment is more than current, then move delinquency date forward by the number
	of times exceeded. Any partially-satisfied gets moved forward;
	PCT_SATIS_BASE = DIVIDE(PMT_AMT,BL_CUR_DU);
	MONTH_MOVE = INT(PCT_SATIS_BASE);
	REMAINDER = PCT_SATIS_BASE - INT(PCT_SATIS_BASE);

	*cumulative totals;
	MONTH_MOVE_CUM + MONTH_MOVE;
	REMAINDER_CUM + REMAINDER;

	*reset cumulative counts based on bf_ssn;
	IF FIRST.BF_SSN THEN MONTH_MOVE_CUM=0;
	IF FIRST.BF_SSN THEN REMAINDER_CUM=0;
	IF PAST_DU = 0.00 THEN MONTH_MOVE = 0;
	IF PAST_DU = 0.00 THEN REMAINDER = 0;
	IF PAST_DU = 0.00 THEN MONTH_MOVE_CUM = 0;
	IF PAST_DU = 0.00 THEN REMAINDER_CUM = 0;
RUN;


*uses data set from above in final dq date calc;
DATA MONTHS_TO_MOVE_3;
	SET MONTHS_TO_MOVE_2;
	FORMAT DQ_DATE_ADJ DATE9.;

	*accounts for cumulative remainders greater than 1;
	REMAINDER_GE_1 = INT(REMAINDER_CUM);
	REMAINDER_LT_1 = REMAINDER_CUM - INT(REMAINDER_CUM);
	MONTH_MOVE_FINAL = REMAINDER_GE_1 + MONTH_MOVE_CUM;

	*delinquent date and days calc;
	IF DQ_DATE ^= . THEN DQ_DATE_ADJ = INTNX("MONTH",DQ_DATE,MONTH_MOVE_FINAL,"SAME");
	DAYS_DQ = LD_BIL_CRT - DQ_DATE_ADJ;
RUN;











/*proc print data=months_to_move_3; where bf_ssn in ('100743459'); run;*/
/*prep output for excel*/
PROC SQL;
	CREATE TABLE PRELIM AS
		SELECT DISTINCT
			BF_SSN
			,LN80
			,LD_BIL_CRT 	FORMAT=MMDDYY10.
			,LD_BIL_DU 		FORMAT=MMDDYY10.
			,PAST_DU
			,BL_CUR_DU
			,TOT_DU
			,FEES
			,TOT_W_FEES
			,PMT_AMT
			,MAX(DT_SATIS) AS DT_SATIS FORMAT=MMDDYY10.
			,DQ_DATE_ADJ 	FORMAT=MMDDYY10.
			,DAYS_DQ
		FROM
			MONTHS_TO_MOVE_3
		WHERE
			LN80 = 'A'
			AND LD_BIL_CRT >= MDY(12,01,2016)
		GROUP BY
			BF_SSN
			,LN80
			,LD_BIL_CRT
			,LD_BIL_DU
			,PAST_DU
			,BL_CUR_DU
			,TOT_DU
			,FEES
			,TOT_W_FEES
			,PMT_AMT
			,DQ_DATE_ADJ
			,DAYS_DQ
	;
QUIT;

proc print ; where bf_ssn in ('100743459','101806506'); run;
/*problem:100743459,101806506*/
101846462
101724103
102746414




*view output;
PROC REPORT DATA=PRELIM;
	COMPUTE BF_SSN;
	COUNT+1;
	IF MOD(COUNT,2) THEN 
		DO;
			CALL DEFINE(_ROW_, "STYLE", "STYLE=[BACKGROUND=#DCDCDC");
		END;
	ENDCOMP;
RUN;

PROC EXPORT
	DATA = PRELIM
/*	OUTFILE = "T:\SAS\Wave_3_billing_&SSN._SSN.xlsx" */
	OUTFILE = "T:\SAS\test.xlsx" 
	DBMS = EXCEL
	REPLACE;
/*	SHEET="Wave_3_SSN_&SSN.";*/
RUN;
;
PROC DATASETS NOPRINT;
	DELETE 
		DAYS_DQ
		DAYS_DQ_2
		DUES
		DUES_DQ
		DUES_TOTAL
		DUES_TOTAL_2
		MONTHS_TO_MOVE
		MONTHS_TO_MOVE_2
		MONTHS_TO_MOVE_3
		PAST_DUE_FLAGS
		PRELIM
		RPS_FLAGS
		SATISFIED_BILL
		_DUES_TOTAL
		_RPS_FLAGS
		_DAYS_DQ
		;
QUIT;
