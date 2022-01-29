*----------------------------------------------------------*
| UTLWD34 INCREASE COLLECTION REVENUE - TRACK  PIF RESULTS |
*----------------------------------------------------------*;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWD34.LWD34R2";
FILENAME REPORT3 "&RPTLIB/ULWD34.LWD34R3";
FILENAME REPORTZ "&RPTLIB/ULWD34.LWD34RZ";
DATA _NULL_;
	CALL SYMPUT('cMONTHS_AGO_6',"'"||PUT(INTNX('MONTH',TODAY(),-6,'S'), MMDDYYD10.)||"'");
	CALL SYMPUT('cMONTHS_AGO_2_BEG',"'"||PUT(INTNX('MONTH',TODAY(),-2,'BEGINNING'), MMDDYYD10.)||"'");
	CALL SYMPUT('BEGIN',INTNX('MONTH',TODAY(),-1,'BEGINNING'));
	CALL SYMPUT('END',INTNX('MONTH',TODAY(),-1,'END'));
	CALL SYMPUT('MONTHS_AGO_2_BEG',INTNX('MONTH',TODAY(),-2,'BEGINNING'));
	CALL SYMPUT('MONTHS_AGO_2_END',INTNX('MONTH',TODAY(),-2,'END'));
	CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'BEGINNING'), MMDDYY10.));
	CALL SYMPUT('RUNTIME',PUT(TIME(), TIME.));
RUN;
%SYSLPUT cMONTHS_AGO_6 = &cMONTHS_AGO_6;
%SYSLPUT cMONTHS_AGO_2_BEG = &cMONTHS_AGO_2_BEG;
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
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
CREATE TABLE ICRTPR AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.DF_PRS_ID
	,B.DF_SPE_ACC_ID
	,C.AF_APL_ID||C.AF_APL_ID_SFX AS CLUID
	,A.PF_ACT
	,A.BD_ATY_PRF
	,E.LC_TRX_TYP
	,E.LD_TRX_EFF
	,E.LA_PRI_AT_PST
	,(
		C.LA_CLM_PRI 
		+ C.LA_CLM_INT 
		- C.LA_PRI_COL 
		+ C.LA_INT_ACR 
		+ COALESCE(D.LA_CLM_INT_ACR,0)
		- C.LA_INT_COL 
	 ) + 
	 (
		C.LA_LEG_CST_ACR 
		-C.LA_LEG_CST_COL 
		+C.LA_OTH_CHR_ACR 
		-C.LA_OTH_CHR_COL 
		+C.LA_COL_CST_ACR 
		-C.LA_COL_CST_COL 
	 ) AS LN_AMT

FROM OLWHRM1.AY01_BR_ATY A
INNER JOIN OLWHRM1.PD01_PDM_INF B
	ON A.DF_PRS_ID = B.DF_PRS_ID
INNER JOIN OLWHRM1.DC01_LON_CLM_INF C
	ON A.DF_PRS_ID = C.BF_SSN
INNER JOIN OLWHRM1.DC11_LON_FAT E
	ON C.AF_APL_ID = E.AF_APL_ID
	AND C.AF_APL_ID_SFX = E.AF_APL_ID_SFX
	AND C.LF_CRT_DTS_DC10 = E.LF_CRT_DTS_DC10
LEFT OUTER JOIN OLWHRM1.DC02_BAL_INT D
	ON E.AF_APL_ID = D.AF_APL_ID
	AND E.AF_APL_ID_SFX = D.AF_APL_ID_SFX

WHERE C.LC_WOF_CPR = '02'
	AND A.BD_ATY_PRF > &cMONTHS_AGO_6
	AND A.PF_ACT IN 
	(
		'DLMN1','DLMN3'
	)
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWD34.LWD34RZ);*/
/*QUIT;*/
ENDRSUBMIT;

DATA ICRTPR;
SET WORKLOCL.ICRTPR;
RUN;

PROC SQL;
CREATE TABLE ICRTPRX(DROP=LN_AMT) AS 
	SELECT A.DF_PRS_ID
		,A.DF_SPE_ACC_ID
		,A.LD_TRX_EFF
		,A.PF_ACT
		,A.BD_ATY_PRF
		,SUM(A.LN_AMT) AS LN_AMT
		,SUM(COALESCE(C.LA_PRI_AT_PST,0)) AS BR_AMT_PD_OFF
	FROM ICRTPR A
	INNER JOIN (
		SELECT DF_PRS_ID
			,MAX(LD_TRX_EFF) AS BR_MX_DATE
		FROM ICRTPR
		GROUP BY DF_PRS_ID
		) B
		ON A.DF_PRS_ID = B.DF_PRS_ID
		AND A.LD_TRX_EFF = B.BR_MX_DATE	
	LEFT OUTER JOIN (
		SELECT LJA.DF_PRS_ID
			,LJA.CLUID
			,LJA.LA_PRI_AT_PST
		FROM ICRTPR LJA
		INNER JOIN (
			SELECT DF_PRS_ID
				,MAX(LD_TRX_EFF) AS LD_TRX_EFF
			FROM ICRTPR
			WHERE LD_TRX_EFF BETWEEN &MONTHS_AGO_2_BEG AND &MONTHS_AGO_2_END
			GROUP BY DF_PRS_ID
			) LJB
			ON LJA.DF_PRS_ID = LJB.DF_PRS_ID
			AND LJA.LD_TRX_EFF = LJB.LD_TRX_EFF
		WHERE LJA.LC_TRX_TYP IN 
			(
				'BR','EP','CS'
			)
		GROUP BY LJA.DF_PRS_ID
		) C
		ON A.CLUID = C.CLUID
	WHERE A.LD_TRX_EFF BETWEEN &BEGIN AND &END
		AND A.LC_TRX_TYP IN 
		(
			'BR','EP','CS'
		)
	GROUP BY A.DF_PRS_ID
		,A.DF_SPE_ACC_ID
		,A.LD_TRX_EFF
		,A.PF_ACT
		,A.BD_ATY_PRF
	HAVING SUM(A.LN_AMT) = 0
	;
QUIT;
/*CREATE SUMMARY DATA SET FOR R3*/
PROC SQL;
CREATE TABLE RLUPS AS 
SELECT COUNT(DISTINCT DF_PRS_ID) AS BR_CT
	,COALESCE(SUM(BR_AMT_PD_OFF),0) AS TOT_AMT
	,COALESCE(SUM(BR_AMT_PD_OFF) / COUNT(DISTINCT DF_PRS_ID),0) AS AVE_COL
FROM ICRTPRX
;
QUIT;
/*CREATE REPORTS*/
PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 NODATE PAGENO=2 CENTER;
TITLE 'INCREASE COLLECTION REVENUE - BORROWER LEVEL  PIF RESULTS';
TITLE2	"&RUNDATE - &RUNTIME";
FOOTNOTE   'JOB = UTLWD34  	 REPORT = ULWD34.LWD34R2';
PROC CONTENTS DATA=ICRTPRX OUT=EMPTYSET NOPRINT;
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
		@46 "JOB = UTLWD34  	 REPORT = ULWD34.LWD34R2";
	END;
RETURN;
RUN;
PROC PRINT NOOBS SPLIT='/' DATA=ICRTPRX WIDTH=UNIFORM WIDTH=MIN LABEL;
FORMAT BD_ATY_PRF LD_TRX_EFF MMDDYY10. BR_AMT_PD_OFF DOLLAR20.2;
VAR DF_SPE_ACC_ID PF_ACT BD_ATY_PRF LD_TRX_EFF BR_AMT_PD_OFF;
LABEL DF_SPE_ACC_ID = 'ACCOUNT NUMBER'
PF_ACT = 'ACTION CODE'
BD_ATY_PRF = 'ACTION CODE DATE'
LD_TRX_EFF = 'LAST PAYMENT DATE'
BR_AMT_PD_OFF = 'BORROWER AMOUNT PAID OFF'
;
RUN;
PROC PRINTTO;
RUN;

PROC PRINTTO PRINT=REPORT3 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 NODATE PAGENO=2 CENTER;
TITLE 'INCREASE COLLECTION REVENUE - ROLL UP OF  PIF TOTALS';
TITLE2	"&RUNDATE - &RUNTIME";
FOOTNOTE   'JOB = UTLWD34  	 REPORT = ULWD34.LWD34R3';
PROC PRINT NOOBS SPLIT='/' DATA=RLUPS WIDTH=UNIFORM WIDTH=MIN LABEL;
FORMAT BR_CT COMMA7. TOT_AMT AVE_COL DOLLAR20.2;
VAR TOT_AMT BR_CT AVE_COL;
LABEL TOT_AMT = 'TOTAL PAID OFF'
	BR_CT = 'NUMBER OF BORROWERS SELECTED'
	AVE_COL = 'AVERAGE COLLECTION PER BORROWER'
;
RUN;
PROC PRINTTO;
RUN;
