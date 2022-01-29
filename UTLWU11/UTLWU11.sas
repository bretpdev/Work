*----------------------------------------------------*
|UTLWU11 QC FOR REPAYMENT ACCOUNT NOT HELD BY UHEAA  |
*----------------------------------------------------*;

/******************************************************************/
/*BE SURE TO COMMENT AND UNCOMMENT THE NESSESARY CODE BLOCKS BELOW*/
/******************************************************************/

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWU11.LWU11R2";
FILENAME REPORTZ "&RPTLIB/ULWU11.LWU11RZ";
DATA _NULL_;
	 CALL SYMPUT('DAYS_60',"'"||PUT(INTNX('DAY',TODAY(),-60,'BEGINNING'), MMDDYYD10.)||"'");
     CALL SYMPUT('DAYS_30',"'"||PUT(INTNX('DAY',TODAY(),-30,'BEGINNING'), MMDDYYD10.)||"'");
	 CALL SYMPUT('TODAY',"'"||PUT(INTNX('DAY',TODAY(),0,'BEGINNING'), MMDDYYD10.)||"'");
RUN;
%SYSLPUT DAYS_60 = &DAYS_60;
%SYSLPUT DAYS_30 = &DAYS_30;
%SYSLPUT TODAY = &TODAY;
LIBNAME  WORKLOCL  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;
LIBNAME SAS_TAB V8 '/sas/whse/progrevw';
PROC SQL NOPRINT;
	SELECT "'"||TRIM(LENDER_ID)||"'"
		INTO :UHEAA_LIST SEPARATED BY ","
	FROM SAS_TAB.LDR_AFF
	WHERE AFFILIATION = 'UHEAA';
QUIT;

%MACRO SQLCHECK (SQLRPT= );
%IF &SQLXRC NE 0 %THEN %DO;
	DATA _NULL_;
    FILE REPORTZ NOTITLES;
    PUT @01 " ********************************************************************* "
      / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
      / @01 " ****  THE SAS LOG IN &SQLRPT SHOULD BE REVIEWED.          **** "       
      / @01 " ********************************************************************* "
      / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
      / @01 " ****  &SQLXMSG   **** "
      / @01 " ********************************************************************* ";
	RUN;
%END;
%MEND;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE QCRANUQ1 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.DF_SPE_ACC_ID
	,X.BF_SSN
	,X.IC_LON_PGM
	,X.LN_SEQ
	,X.LF_LON_CUR_OWN
	,X.WD_LON_RPD_SR
	,X.WC_DW_LON_STA
	,X.LA_CUR_PRI
FROM OLWHRM1.PD10_PRS_NME A
INNER JOIN (
		SELECT A.BF_SSN
			,A.IC_LON_PGM
			,A.LN_SEQ
			,A.LF_LON_CUR_OWN
			,A.LA_CUR_PRI
			,B.WD_LON_RPD_SR
			,B.WC_DW_LON_STA
		FROM OLWHRM1.LN10_LON A
		INNER JOIN OLWHRM1.DW01_DW_CLC_CLU B
			ON A.BF_SSN = B.BF_SSN
			AND A.LN_SEQ = B.LN_SEQ
		INNER JOIN OLWHRM1.SD10_STU_SPR C
			ON A.LF_STU_SSN = C.LF_STU_SSN
		WHERE A.LA_CUR_PRI > 0
		AND A.LF_LON_CUR_OWN NOT IN (&UHEAA_LIST)
		AND B.WD_LON_RPD_SR <= &DAYS_30
		AND C.LC_STA_STU10 = 'A'
		AND C.LD_SCL_SPR <= &DAYS_30
		AND A.IC_LON_PGM NOT IN ('TILP','GATEUG','GATEGL','GATEMD')
		AND A.LC_STA_LON10 = 'R'
	UNION
		SELECT A.BF_SSN
			,A.IC_LON_PGM
			,A.LN_SEQ
			,A.LF_LON_CUR_OWN
			,A.LA_CUR_PRI
			,B.WD_LON_RPD_SR
			,B.WC_DW_LON_STA
		FROM OLWHRM1.LN10_LON A
		INNER JOIN OLWHRM1.SD10_STU_SPR C
			ON A.LF_STU_SSN = C.LF_STU_SSN
		INNER JOIN  (
				SELECT BF_SSN
					,LN_SEQ
					,WD_LON_RPD_SR
					,WC_DW_LON_STA
				FROM OLWHRM1.DW01_DW_CLC_CLU
				WHERE WC_DW_LON_STA = '03'
				AND WD_LON_RPD_SR >= &DAYS_30
			UNION
				SELECT DW01.BF_SSN
					,DW01.LN_SEQ
					,DW01.WD_LON_RPD_SR
					,DW01.WC_DW_LON_STA
				FROM OLWHRM1.DW01_DW_CLC_CLU DW01
				INNER JOIN (
					SELECT BF_SSN
						,LN_SEQ
						,MAX(LD_DFR_BEG) AS MX_DFR_BEG
					FROM OLWHRM1.LN50_BR_DFR_APV 
					WHERE LC_STA_LON50 = 'A'
					GROUP BY BF_SSN
						,LN_SEQ
					) LN50
					ON DW01.BF_SSN = LN50.BF_SSN
					AND DW01.LN_SEQ = LN50.LN_SEQ
				WHERE DW01.WC_DW_LON_STA = '04'
				AND LN50.MX_DFR_BEG >= &DAYS_30
			UNION
				SELECT DW01.BF_SSN
					,DW01.LN_SEQ
					,DW01.WD_LON_RPD_SR
					,DW01.WC_DW_LON_STA
				FROM OLWHRM1.DW01_DW_CLC_CLU DW01
				INNER JOIN (
				SELECT BF_SSN
					,LN_SEQ
					,MAX(LD_FOR_BEG) AS MX_FRB_BEG
				FROM OLWHRM1.LN60_BR_FOR_APV 
				WHERE LC_STA_LON60 = 'A'
				GROUP BY BF_SSN
					,LN_SEQ
					) LN60
					ON DW01.BF_SSN = LN60.BF_SSN
					AND DW01.LN_SEQ = LN60.LN_SEQ
				WHERE DW01.WC_DW_LON_STA = '05'
				AND LN60.MX_FRB_BEG >= &DAYS_30
			) B
			ON A.BF_SSN = B.BF_SSN
			AND A.LN_SEQ = B.LN_SEQ
		WHERE A.LA_CUR_PRI > 0
		AND A.LF_LON_CUR_OWN NOT IN (&UHEAA_LIST)
		AND C.LC_STA_STU10 = 'A'
		AND C.LD_SCL_SPR <= &DAYS_30
		AND A.IC_LON_PGM NOT IN ('TILP','GATEUG','GATEGL','GATEMD')
		) X
		ON A.DF_PRS_ID = X.BF_SSN

FOR READ ONLY WITH UR
);

CREATE TABLE EMIGRE AS
SELECT DISTINCT A.*
FROM CONNECTION TO DB2 (
	SELECT A.BF_SSN
		,A.LN_SEQ
	FROM OLWHRM1.LN10_LON A
	INNER JOIN OLWHRM1.DW01_DW_CLC_CLU B
		ON A.BF_SSN = B.BF_SSN
		AND A.LN_SEQ = B.LN_SEQ
	WHERE B.WD_LON_RPD_SR >= &DAYS_60
	AND A.IC_LON_PGM IN ('PLUS','PLUSGB')
FOR READ ONLY WITH UR
) A;
DISCONNECT FROM DB2;
PROC SQL;
CREATE TABLE QCRANU AS
SELECT DISTINCT A.*
FROM QCRANUQ1 A
WHERE NOT EXISTS (
	SELECT *
	FROM EMIGRE X
	WHERE X.BF_SSN = A.BF_SSN
	AND X.LN_SEQ = A.LN_SEQ
	)
;
QUIT;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWU11.LWU11RZ);*/
/*QUIT;*/

LIBNAME HLDIR V8 '/sas/whse/progrevw';

/***********************************************************************************/
/***********************     BLOCK 1   *********************************************/
/***********************************************************************************/
/*****************COMMENT OUT CODE BLOCK 1 FOR PRODUCTION **************************/
%LET LOGDS = DTLOG_TEST;
/***********************************************************************************/
/*********************** END BLOCK 1   *********************************************/
/***********************************************************************************/

/***********************************************************************************/
/***********************     BLOCK 2   *********************************************/
/***********************************************************************************/
/*************** UNCOMMENT CODE BLOCK 2 FOR PRODUCTION *************************/
/*%LET LOGDS = DTLOG;*/
/***********************************************************************************/
/********************** END BLOCK 2    *********************************************/
/***********************************************************************************/


/*******************************************************************************************
* THE DAY1 VARIABLE WILL HOLD THE FIRST DATE THAT THE LOAN WAS SELECTED. THE DAY30 VARIABLE 
* HOLDS THE TRIGGER DATE WHICH WILL SELECT THE LOAN FOR THE REPORT ON THE APPROPRIATE DATE 
********************************************************************************************/
PROC SQL;
CREATE TABLE DTLOG_TEMP AS 
SELECT DISTINCT A.BF_SSN
	,A.LN_SEQ
	,TODAY() AS DAY1 FORMAT=MMDDYY10.
	,TODAY()+29 AS DAY30 FORMAT=MMDDYY10.
FROM QCRANU A
WHERE NOT EXISTS (
	SELECT *
	FROM HLDIR.&LOGDS X
	WHERE X.BF_SSN = A.BF_SSN
	AND X.LN_SEQ = A.LN_SEQ
	)
;
QUIT;
/*******************************************************************************************
* PUT ALL THE DATE INFO TOGETHER IN ONE DATA SET
********************************************************************************************/
DATA HLDIR.&LOGDS;
SET HLDIR.&LOGDS DTLOG_TEMP;
RUN;
/*******************************************************************************************
* SELECT THE LOANS THAT HAVE APPEARED FOR THIRTY DAYS
********************************************************************************************/
DATA DESRES;
SET HLDIR.&LOGDS;
WHERE DAY30 <= TODAY();
RUN;

PROC SQL;
CREATE TABLE PRNT_TDY AS 
SELECT DISTINCT A.*
FROM QCRANU A
INNER JOIN DESRES B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
;
QUIT;
/*******************************************************************************************
* REMOVE LOANS FROM THE REPOSITORY FILE IF IT IS NOT SELECTED IN THE INITIAL SELECTION
********************************************************************************************/
PROC SQL;
CREATE TABLE UPDT_LOG AS 
SELECT DISTINCT *
FROM HLDIR.&LOGDS A
WHERE EXISTS (
	SELECT *
	FROM QCRANU X
	WHERE X.BF_SSN = A.BF_SSN
	AND X.LN_SEQ = A.LN_SEQ
	)
;
QUIT;

DATA HLDIR.&LOGDS;
SET UPDT_LOG;
RUN;
ENDRSUBMIT;

DATA PRNT_TDY;
SET WORKLOCL.PRNT_TDY;
RUN;

PROC SORT DATA=PRNT_TDY;
BY DF_SPE_ACC_ID;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 PAGENO=1 CENTER;
TITLE 'QC REPAYMENT ACCOUNTS NOT HELD BY UHEAA';
FOOTNOTE 'JOB = UTLWU11  	 REPORT = ULWU11.LWU11R2';
PROC CONTENTS DATA=PRNT_TDY OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 126*'-';
	PUT      //////
		@51 '**** NO OBSERVATIONS FOUND ****';
	PUT //////
		@57 '-- END OF REPORT --';
	PUT //////////////
		@46 "JOB = UTLWU11  	 REPORT = ULWU11.LWU11R2";
	END;
RETURN;
RUN;
PROC PRINT NOOBS SPLIT='/' DATA=PRNT_TDY WIDTH=UNIFORM WIDTH=MIN;
FORMAT WD_LON_RPD_SR MMDDYY10. LA_CUR_PRI DOLLAR10.2;
VAR DF_SPE_ACC_ID
	LN_SEQ
	WC_DW_LON_STA
	IC_LON_PGM
	LF_LON_CUR_OWN
	WD_LON_RPD_SR
	LA_CUR_PRI;
LABEL DF_SPE_ACC_ID = 'ACCOUNT NUMBER'
	LN_SEQ = 'LOAN SEQ'
	WC_DW_LON_STA = 'LOAN STATUS'
	IC_LON_PGM = 'LOAN TYPE'
	LF_LON_CUR_OWN = 'CURRENT OWNER'
	WD_LON_RPD_SR = 'REPAYMENT START DATE'
	LA_CUR_PRI = 'CURRENT BALANCE'
;
RUN;
PROC PRINTTO;
RUN;
