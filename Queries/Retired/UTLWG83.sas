/*===========================================*/
/*UTLWG83 - CONSOLIDATION SUMMARY INFORMATION*/
/*===========================================*/
LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
/*%LET RPTLIB = T:\SAS;*/
FILENAME REPORT2 "&RPTLIB/ULWG83.LWG83R2";
FILENAME REPORT3 "&RPTLIB/ULWG83.LWG83R3";
FILENAME REPORT4 "&RPTLIB/ULWG83.LWG83R4";
FILENAME REPORT5 "&RPTLIB/ULWG83.LWG83R5";
FILENAME REPORT6 "&RPTLIB/ULWG83.LWG83R6";
FILENAME REPORTZ "&RPTLIB/ULWG83.LWG83RZ";

/*LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;*/
/*RSUBMIT;*/

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
CREATE TABLE CS1A AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT LC10.DF_LCO_PRS_SSN_BR
	,LC10.AN_LCO_APL_SEQ
	,LC10.LN_LCO_UND_LN_SEQ
	,COALESCE(LC10.LA_UND_LN_ANT_PAY,0) AS ANT_PAYOFF
	,LC10.LD_UND_LN_ANT_PAY AS ANT_PAYOFF_DATE
	,CHAR(MONTH(LC10.LD_UND_LN_ANT_PAY)) AS MONTH
	,CHAR(DAY(LC10.LD_UND_LN_ANT_PAY)) AS DAY
	,CHAR(YEAR(LC10.LD_UND_LN_ANT_PAY)) AS YEAR
	,LC10.LD_LVC_RTN AS LVC_RETUN_DATE
	,LC10.LD_LVC_CMP_CDR AS LVC_COMPLETE_DATE
	,LC10.LI_UND_LN_CON AS TWOBCON
	,LC10.LA_UND_LN_BAL
	,AP1A.BD_LCO_BR_GRC_END
	,AP1A.IC_LCO_DSB_APV_STA
	,AP1A.AD_LCO_APL_DSB 
	,AP1A.AD_LCO_APL_RCV
	,AP1A.IC_LCO_LN_GTR_STA
	,AP1A.AF_USR_UPD_DSB_APV
	,AP1A.ID_LCO_LN_GTR_STA
	,RTRIM(PD6A.DM_LCO_PRS_LST)||', '||PD6A.DM_LCO_PRS_1 AS NAME
	,PD6A.DF_SPE_ACC_ID AS ACCTNUM

FROM OLWHRM1.AP1A_LCO_APL AP1A
INNER JOIN OLWHRM1.LC10_UND_LN_INF LC10
	ON AP1A.DF_LCO_PRS_SSN_BR = LC10.DF_LCO_PRS_SSN_BR
	AND AP1A.AN_LCO_APL_SEQ = LC10.AN_LCO_APL_SEQ
	AND AP1A.AC_LCO_ACC_STA NOT IN ('D1','D3','D4','D6','D7','D8')
INNER JOIN OLWHRM1.PD6A_LCO_PRS_DMO PD6A
	ON AP1A.DF_LCO_PRS_SSN_BR = PD6A.DF_LCO_PRS_SSN

WHERE AP1A.AD_LCO_APL_DSB IS NULL
AND AP1A.AD_LCO_APL_RCV IS NOT NULL
AND LC10.LI_UND_LN_CON = 'Y'

FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;
%SQLCHECK (SQLRPT=ULWG83.LWG83RZ);
QUIT;
/*ENDRSUBMIT;*/
/**/
/*DATA CS1A;SET WORKLOCL.CS1A;RUN;*/
/*===========================================================================*/
/*CALCULATE LVC STATUS */
/*===========================================================================*/
DATA LVC (KEEP=DF_LCO_PRS_SSN_BR AN_LCO_APL_SEQ LVC_REC);
SET CS1A;
WHERE (LVC_RETUN_DATE EQ . AND TWOBCON EQ 'Y')
OR (LVC_RETUN_DATE NE . AND TWOBCON EQ 'Y' AND ANT_PAYOFF EQ 0);
LVC_REC = 'N';
RUN;

PROC SORT DATA=LVC NODUPKEY;BY DF_LCO_PRS_SSN_BR AN_LCO_APL_SEQ;RUN;
PROC SORT DATA=CS1A;BY DF_LCO_PRS_SSN_BR AN_LCO_APL_SEQ;RUN;

DATA CS1A;
MERGE LVC CS1A;
BY DF_LCO_PRS_SSN_BR AN_LCO_APL_SEQ;
IF LVC_REC NE '' THEN LVC_REC = LVC_REC;
ELSE LVC_REC = 'Y';
RUN;
/*===========================================================================*/
/*GET MAX ANT_PAYOFF_DATE DATE FOR FURTHER PROCESSING */
/*===========================================================================*/
PROC SQL;
CREATE TABLE MAX_DT AS 
SELECT DF_LCO_PRS_SSN_BR 
	,MAX(ANT_PAYOFF_DATE) AS ANT_PAYOFF_DATE FORMAT=MMDDYY10.
FROM CS1A
GROUP BY DF_LCO_PRS_SSN_BR;
QUIT;
RUN;
/*===========================================================================*/
/*SEPARATE APPROVED FROM NOT APPROVED STATUS*/
/*===========================================================================*/
DATA CSAP (DROP=ANT_PAYOFF_DATE) CSNAP (DROP=ANT_PAYOFF_DATE);
SET CS1A;
IF IC_LCO_DSB_APV_STA = 'A' THEN OUTPUT CSAP;
ELSE OUTPUT CSNAP;
RUN;
/*===========================================================================*/
/*MERGE MAX ANT_PAYOFF_DATE*/
/*===========================================================================*/
%MACRO MAXDATE(DS1,DS2);
PROC SORT DATA=&DS1;BY DF_LCO_PRS_SSN_BR;RUN;
PROC SORT DATA=&DS2;BY DF_LCO_PRS_SSN_BR;RUN;
DATA &DS1;
MERGE &DS1 (IN=A) &DS2 (IN=B);
BY DF_LCO_PRS_SSN_BR;
IF A AND B;
RUN;
%MEND MAXDATE;
%MAXDATE(CSAP,MAX_DT);
%MAXDATE(CSNAP,MAX_DT);
/*==========BORROWER CHECK==========*/
/*PROC SQL;*/
/*CREATE TABLE DBR AS */
/*SELECT distinct A.**/
/*FROM CSAP A*/
/*INNER JOIN CSNAP B*/
/*ON A.DF_LCO_PRS_SSN_BR = B.DF_LCO_PRS_SSN_BR ;*/
/*QUIT;*/
/*RUN;*/
/*PROC SORT DATA=DBR;*/
/*BY DF_LCO_PRS_SSN_BR ANT_PAYOFF_DATE;*/
/*RUN;*/
/*===========================================================================*/
/*APPROVED PROCESSING*/
/*===========================================================================*/
%MACRO DT_CONCAT(DS,CRT_DT);
DATA &DS (DROP=MONTH DAY YEAR);
SET &DS;
IF LENGTH(LEFT(MONTH(&CRT_DT))) EQ 1 THEN MONTH = '0'||TRIM(LEFT(MONTH(&CRT_DT)));
ELSE MONTH = TRIM(LEFT(MONTH(&CRT_DT)));
IF LENGTH(LEFT(DAY(&CRT_DT))) EQ 1 THEN DAY = '0'||TRIM(LEFT(DAY(&CRT_DT)));
ELSE DAY = TRIM(LEFT(DAY(&CRT_DT)));
YEAR = TRIM(LEFT(YEAR(&CRT_DT)));
CHAR_&CRT_DT = LEFT(TRIM(MONTH)||'/'||TRIM(DAY)||'/'||TRIM(YEAR));
IF CHAR_ANT_PAYOFF_DATE = '0./0./.' THEN CHAR_ANT_PAYOFF_DATE = 'NONE';
RUN;
%MEND DT_CONCAT;
%DT_CONCAT(CSAP,ANT_PAYOFF_DATE); 

PROC SORT DATA=CSAP;
BY ANT_PAYOFF_DATE CHAR_ANT_PAYOFF_DATE DF_LCO_PRS_SSN_BR AN_LCO_APL_SEQ LN_LCO_UND_LN_SEQ;
RUN;

DATA CSAPB;
SET CSAP;
BY ANT_PAYOFF_DATE CHAR_ANT_PAYOFF_DATE DF_LCO_PRS_SSN_BR AN_LCO_APL_SEQ LN_LCO_UND_LN_SEQ;
IF FIRST.DF_LCO_PRS_SSN_BR THEN BR_CT = 1;
	ELSE BR_CT = 0;
IF FIRST.ANT_PAYOFF_DATE THEN DO;
	AMT_TOT = 0;
	BOR_TOT = 0;
	END;
AMT_TOT+ANT_PAYOFF;
BOR_TOT+BR_CT;
RUN;

DATA CSAPC (KEEP=ANT_PAYOFF_DATE CHAR_ANT_PAYOFF_DATE AMT_TOT BOR_TOT);
SET CSAPB;
BY ANT_PAYOFF_DATE;
IF LAST.ANT_PAYOFF_DATE THEN OUTPUT;
RUN;

PROC SORT DATA=CSAPC;
BY ANT_PAYOFF_DATE CHAR_ANT_PAYOFF_DATE;
RUN;

DATA CSAPC;
SET CSAPC;
IF _N_ = 1 THEN DISP_VAR = 0;
DISP_VAR+1;
RUN;
/*===========================================================================*/
/*NOT APPROVED PROCESSING*/
/*===========================================================================*/
DATA GDT NOGDT;
SET CSNAP;
IF BD_LCO_BR_GRC_END NE . THEN OUTPUT GDT;
ELSE OUTPUT NOGDT;
RUN;
/*===========================================================================*/
/*GRACE DATE POPULATED PROCESSING*/
/*===========================================================================*/
PROC SORT DATA=GDT;
BY BD_LCO_BR_GRC_END DF_LCO_PRS_SSN_BR AN_LCO_APL_SEQ LN_LCO_UND_LN_SEQ;
RUN;

PROC SORT DATA=GDT;BY LVC_REC DF_LCO_PRS_SSN_BR;RUN;

DATA GDTC;
SET GDT;
BY LVC_REC DF_LCO_PRS_SSN_BR;
IF FIRST.DF_LCO_PRS_SSN_BR THEN BR_CT = 1;
ELSE BR_CT = 0;
IF FIRST.LVC_REC THEN DO;
	AMT_TOT = 0;
	BOR_TOT = 0;
	END;
AMT_TOT+LA_UND_LN_BAL;
BOR_TOT+BR_CT;
RUN;

DATA GDTD (KEEP=LVC_REC AMT_TOT BOR_TOT);
SET GDTC;
BY LVC_REC;
IF LAST.LVC_REC THEN OUTPUT;
RUN;
/*===========================================================================*/
/*GRACE DATE POPULATED PROCESSING BY MONTH/YEAR*/
/*===========================================================================*/
DATA GDT_MO;
SET GDT;
MONTH = LEFT(MONTH(BD_LCO_BR_GRC_END));
DAY = LEFT(DAY(BD_LCO_BR_GRC_END)); 
YEAR = LEFT(YEAR(BD_LCO_BR_GRC_END));
YRMO = TRIM(LEFT(MONTH(BD_LCO_BR_GRC_END)))||'/'||LEFT(YEAR(BD_LCO_BR_GRC_END));
RUN;

PROC SORT DATA=GDT_MO;
BY LVC_REC YRMO DF_LCO_PRS_SSN_BR;
RUN;

DATA GDTC_MO;
SET GDT_MO;
BY LVC_REC YRMO DF_LCO_PRS_SSN_BR;
IF FIRST.DF_LCO_PRS_SSN_BR THEN BR_CT = 1;
ELSE BR_CT = 0;
IF FIRST.YRMO THEN DO;
	AMT_TOT = 0;
	BOR_TOT = 0;
	END;
AMT_TOT+LA_UND_LN_BAL;
BOR_TOT+BR_CT;
RUN;

PROC SORT DATA=GDTC_MO;
BY  LVC_REC YRMO;
RUN;

DATA GDTD_MO (KEEP=LVC_REC BD_LCO_BR_GRC_END YRMO AMT_TOT BOR_TOT);
SET GDTC_MO;
BY  LVC_REC YRMO;
IF LAST.YRMO THEN OUTPUT;
RUN;

PROC SORT DATA=GDTD_MO;
BY LVC_REC BD_LCO_BR_GRC_END YRMO;
RUN;

DATA GDTD_MO ;
SET GDTD_MO ;
IF _N_ = 1 THEN DISP_VAR = 0;
DISP_VAR+1;
RUN;
/*===========================================================================*/
/*NO GRACE DATE POPULATED PROCESSING*/
/*===========================================================================*/
PROC SORT DATA=NOGDT;
BY BD_LCO_BR_GRC_END DF_LCO_PRS_SSN_BR AN_LCO_APL_SEQ LN_LCO_UND_LN_SEQ;
RUN;

PROC SORT DATA=NOGDT;BY LVC_REC DF_LCO_PRS_SSN_BR;RUN;

DATA NOGDTC;
SET NOGDT;
BY LVC_REC DF_LCO_PRS_SSN_BR;
IF FIRST.DF_LCO_PRS_SSN_BR THEN BR_CT = 1;
ELSE BR_CT = 0;
IF FIRST.LVC_REC THEN DO;
	AMT_TOT = 0;
	BOR_TOT = 0;
	END;
AMT_TOT+LA_UND_LN_BAL;
BOR_TOT+BR_CT;
RUN;

DATA NOGDTD (KEEP=LVC_REC AMT_TOT BOR_TOT);
SET NOGDTC;
BY LVC_REC;
IF LAST.LVC_REC THEN OUTPUT;
RUN;
/*===========================================================================*/
/*GET DATA FOR ERROR REPORTS*/
/*===========================================================================*/
PROC SQL;
CREATE TABLE BRWR_ERR AS 
SELECT DISTINCT A.DF_LCO_PRS_SSN_BR
	,A.NAME
	,A.AN_LCO_APL_SEQ
	,A.AF_USR_UPD_DSB_APV
	,A.ID_LCO_LN_GTR_STA
	,A.ACCTNUM
FROM CS1A A
WHERE A.ANT_PAYOFF_DATE = .
AND A.TWOBCON = 'Y'
AND A.IC_LCO_DSB_APV_STA = 'A'
OR 
A.IC_LCO_LN_GTR_STA= 'S'
;
QUIT;
RUN;

PROC SORT DATA=BRWR_ERR NODUPRECS;
BY DF_LCO_PRS_SSN_BR /*ANT_PAYOFF_DATE*/;
RUN;

DATA ERR_SRT (KEEP=DF_LCO_PRS_SSN_BR LVC_REC);
SET GDT NOGDT CSAP;
RUN;

PROC SORT DATA=ERR_SRT NODUPKEY;BY DF_LCO_PRS_SSN_BR;RUN;
PROC SORT DATA=BRWR_ERR;BY DF_LCO_PRS_SSN_BR;RUN;

DATA BRWR_ERR;
MERGE BRWR_ERR (IN=A) ERR_SRT (IN=B);
BY DF_LCO_PRS_SSN_BR;
IF A AND B;
RUN;

PROC SORT DATA=BRWR_ERR;BY LVC_REC DF_LCO_PRS_SSN_BR;RUN;

/*===========================================================================*/
/*APPROVED FOR DISB REPORT*/
/*===========================================================================*/
PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS CENTER DATE NUMBER PAGENO=1 LS=128 PS=39;
PROC REPORT DATA=CSAPC NOWD SPACING=1 HEADSKIP SPLIT='/';
TITLE 'APPROVED FOR DISBURSEMENT';
TITLE2 'WITH A BLANK DISCLOSURE DATE';
FOOTNOTE 'JOB=UTLWG83	 REPORT=ULWG83.LWG83R2';
COLUMN DISP_VAR CHAR_ANT_PAYOFF_DATE AMT_TOT BOR_TOT;
DEFINE DISP_VAR / ORDER NOPRINT;
DEFINE CHAR_ANT_PAYOFF_DATE / ORDER "ANTICIPATED PAYOFF DATE" WIDTH=20 ;
DEFINE AMT_TOT / ANALYSIS "TOTAL ANTICIPATED PAYOFF AMOUNT" WIDTH=20 FORMAT=DOLLAR18.2;
DEFINE BOR_TOT / ANALYSIS "BORROWER TOTAL" WIDTH=20;
COMPUTE AFTER;
	LINE @57 '__________________' @91 '_____';
	LINE ' ';
	LINE @34 'TOTALS' @57 AMT_TOT.SUM DOLLAR18.2 @91 BOR_TOT.SUM COMMA5. ;
	LINE ' ';
ENDCOMP;
RUN;
/*===========================================================================*/
/*GRACE DATE POPULATED LVC REPORT*/
/*===========================================================================*/
PROC PRINTTO PRINT=REPORT3 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS CENTER DATE NUMBER PAGENO=1 LS=128 PS=39;
PROC REPORT DATA=GDTD NOWD SPACING=1 HEADSKIP SPLIT='/';
TITLE 'LVC STATUS FOR BORROWERS WITH ';
TITLE2 'GRACE END POPULATED';
FOOTNOTE 'JOB=UTLWG83	 REPORT=ULWG83.LWG83R3';
COLUMN LVC_REC AMT_TOT BOR_TOT;
DEFINE LVC_REC / ORDER "LVC RECEIVED" WIDTH=20 ;
DEFINE AMT_TOT / ANALYSIS "TOTAL UNDERLYING/LOAN AMOUNT" WIDTH=20 FORMAT=DOLLAR18.2;
DEFINE BOR_TOT / ANALYSIS "BORROWER TOTAL" WIDTH=20;
COMPUTE AFTER;
	LINE @57 '__________________' @90 '______';
	LINE ' ';
	LINE @34 'TOTALS' @57 AMT_TOT.SUM DOLLAR18.2 @91 BOR_TOT.SUM COMMA5. ;
	LINE ' ';
ENDCOMP;
RUN;
/*===========================================================================*/
/*GRACE DATE POPULATED LVC REPORT BY MONTH/YEAR*/
/*===========================================================================*/
PROC PRINTTO PRINT=REPORT4 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS CENTER DATE NUMBER PAGENO=1 LS=128 PS=39;
PROC REPORT DATA=GDTD_MO NOWD SPACING=1 HEADSKIP SPLIT='*';
TITLE 'LVC STATUS FOR BORROWERS WITH ';
TITLE2 'GRACE END POPULATED BY MONTH';
FOOTNOTE 'JOB=UTLWG83	 REPORT=ULWG83.LWG83R4';

COLUMN LVC_REC DISP_VAR YRMO AMT_TOT BOR_TOT;
DEFINE DISP_VAR / ORDER NOPRINT;
DEFINE LVC_REC / ORDER "LVC RECEIVED" WIDTH=20 ;
DEFINE YRMO / DISPLAY "MONTH/YEAR" WIDTH=20 ;
DEFINE AMT_TOT / ANALYSIS "TOTAL UNDERLYING*LOAN AMOUNT" WIDTH=20 FORMAT=DOLLAR18.2;
DEFINE BOR_TOT / ANALYSIS "BORROWER TOTAL" WIDTH=20;

COMPUTE AFTER LVC_REC;
	LINE @67 '__________________' @100 '______';
	LINE ' ';
	LINE @44 'TOTALS' @67 AMT_TOT.SUM DOLLAR18.2 @101 BOR_TOT.SUM COMMA5. ;
	LINE ' ';
ENDCOMP;

BREAK AFTER LVC_REC / PAGE SUPPRESS;
RUN;
/*===========================================================================*/
/*GRACE DATE NOT POPULATED LVC REPORT*/
/*===========================================================================*/
PROC PRINTTO PRINT=REPORT5 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS CENTER DATE NUMBER PAGENO=1 LS=128 PS=39;
PROC REPORT DATA=NOGDTD NOWD SPACING=1 HEADSKIP SPLIT='/';
TITLE 'LVC STATUS FOR BORROWERS WITH ';
TITLE2 'GRACE END NOT POPULATED';
FOOTNOTE 'JOB=UTLWG83	 REPORT=ULWG83.LWG83R5';
COLUMN LVC_REC AMT_TOT BOR_TOT;
DEFINE LVC_REC / ORDER "LVC RECEIVED" WIDTH=20 ;
DEFINE AMT_TOT / ANALYSIS "TOTAL UNDERLYING/LOAN AMOUNT" WIDTH=20 FORMAT=DOLLAR18.2;
DEFINE BOR_TOT / ANALYSIS "BORROWER TOTAL" WIDTH=20;
COMPUTE AFTER;
	LINE @57 '__________________' @90 '______';
	LINE ' ';
	LINE @34 'TOTALS' @57 AMT_TOT.SUM DOLLAR18.2 @91 BOR_TOT.SUM COMMA5. ;
	LINE ' ';
ENDCOMP;
RUN;
/*===========================================================================*/
/*DISCREPENCY REPORT*/
/*===========================================================================*/
PROC PRINTTO PRINT=REPORT6 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS CENTER DATE NUMBER PAGENO=1 LS=128 PS=39;
PROC REPORT DATA=BRWR_ERR NOWD SPACING=1 HEADSKIP SPLIT='/';
TITLE 'CONSOLIDATION LOAN DISCREPANCY REPORT';
FOOTNOTE 'JOB=UTLWG83	 REPORT=ULWG83.LWG83R6';
COLUMN LVC_REC ACCTNUM NAME AN_LCO_APL_SEQ AF_USR_UPD_DSB_APV ID_LCO_LN_GTR_STA;
DEFINE LVC_REC / ORDER NOPRINT;
DEFINE ACCTNUM / DISPLAY "ACCT NUM" WIDTH=15 ;
DEFINE NAME / DISPLAY "NAME" WIDTH=45 ;
DEFINE AN_LCO_APL_SEQ / DISPLAY "APP SEQ" WIDTH=15 ;
DEFINE AF_USR_UPD_DSB_APV / DISPLAY "USER ID" WIDTH=15 ;
DEFINE ID_LCO_LN_GTR_STA / DISPLAY "STATUS DATE" WIDTH=15 FORMAT=MMDDYY10.;
RUN;

PROC PRINTTO;
RUN;

/*===========================================================================*/
/*DETAIL FILES*/
/*===========================================================================*/
/*PROC EXPORT DATA=CSAP*/
/*            OUTFILE= "T:\SAS\APPROVED_DETAIL.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/
/*PROC EXPORT DATA=GDT*/
/*            OUTFILE= "T:\SAS\GRACE_END_DETAIL.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/
/*PROC EXPORT DATA=NOGDT*/
/*            OUTFILE= "T:\SAS\NOGRACE_END_DETAIL.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/