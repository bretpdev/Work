/*Consolidation Payment Transaction UTLWR03*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWR03.LWR03R2";
FILENAME REPORTZ "&RPTLIB/ULWR03.LWR03RZ";

LIBNAME  WORKLOCL  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;

%macro sqlcheck ;
  %if  &sqlxrc ne 0  %then  %do  ;
    data _null_  ;
            file reportz notitles  ;
            put @01 " ********************************************************************* "
              / @01 " ****  The SQL code above has experienced an error.               **** "
              / @01 " ****  The SAS should be reviewed.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  The SQL error code is  &sqlxrc  and the SQL error message  **** "
              / @01 " ****  &sqlxmsg   **** "
              / @01 " ********************************************************************* "
            ;
         run  ;
  %end  ;
%mend  ;


PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (

SELECT DISTINCT
	PD10.DF_SPE_ACC_ID AS ACC
	,B.LN_SEQ
	,B.LD_FAT_EFF AS DT_107080
	,COALESCE(B.LA_FAT_CUR_PRI,0) + COALESCE(B.LA_FAT_NSI,0) AS AMT_107080
	,T0101.LD_FAT_EFF AS DT_0101
	,ABS(T0101.LA_FAT_CUR_PRI) AS AMT_0101
	,T0395.LD_FAT_EFF AS DT_0395
	,ABS(T0395.LA_FAT_CUR_PRI) AS AMT_0395
	,COALESCE(A.LA_CUR_PRI,0) as LA_CUR_PRI
	,A.IF_DOE_LDR
	,A.LF_LON_CUR_OWN
	,DW01.WC_DW_LON_STA

	,A.LD_LON_1_DSB
	,A.BF_SSN



FROM OLWHRM1.LN90_FIN_ATY B
INNER JOIN OLWHRM1.PD10_PRS_NME PD10
	ON PD10.DF_PRS_ID = B.BF_SSN
left outer JOIN (SELECT DISTINCT Z.BF_SSN, Z.LN_SEQ, Z.LA_FAT_CUR_PRI, Z.LD_FAT_EFF FROM OLWHRM1.LN90_FIN_ATY Z 
			WHERE Z.PC_FAT_TYP = '01' 
			AND Z.PC_FAT_SUB_TYP = '01'
			AND Z.LC_STA_LON90 = 'A'
			AND Z.LC_FAT_REV_REA = '' 
			AND Z.LA_FAT_CUR_PRI IS NOT NULL
			) T0101
	ON T0101.BF_SSN = B.BF_SSN
	AND T0101.LN_SEQ = B.LN_SEQ
	AND T0101.LD_FAT_EFF > B.LD_FAT_EFF
left outer JOIN (SELECT DISTINCT Z.BF_SSN, Z.LN_SEQ, Z.LA_FAT_CUR_PRI, Z.LD_FAT_EFF FROM OLWHRM1.LN90_FIN_ATY Z 
			WHERE Z.PC_FAT_TYP = '03' 
			AND Z.PC_FAT_SUB_TYP = '95'
			AND Z.LC_STA_LON90 = 'A'
			AND Z.LC_FAT_REV_REA = '' 
			AND Z.LA_FAT_CUR_PRI IS NOT NULL
			) T0395
	ON T0395.BF_SSN = B.BF_SSN
	AND T0395.LN_SEQ = B.LN_SEQ
	AND T0395.LD_FAT_EFF > B.LD_FAT_EFF
INNER JOIN OLWHRM1.LN10_LON A
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
INNER JOIN OLWHRM1.DW01_DW_CLC_CLU DW01
	ON DW01.BF_SSN = B.BF_SSN
	AND DW01.LN_SEQ = B.LN_SEQ
WHERE B.PC_FAT_TYP = '10'
AND B.PC_FAT_SUB_TYP IN ('70','80')
AND B.LC_STA_LON90 = 'A'
AND B.LC_FAT_REV_REA = ''
AND A.LA_CUR_PRI <> 0
AND A.LC_STA_LON10 = 'R'

FOR READ ONLY WITH UR

);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;


DATA DEMO; SET WORKLOCL.DEMO; RUN;

PROC SORT DATA=DEMO;
BY ACC;
RUN;


DATA DEMO2;
SET DEMO;
FORMAT CUR_STAT $13.;
IF WC_DW_LON_STA = '01' THEN CUR_STAT = 'IN GRACE ';
IF WC_DW_LON_STA = '02' THEN CUR_STAT = 'IN SCHOOL';
IF WC_DW_LON_STA = '03' THEN CUR_STAT = 'IN REPAY';
IF WC_DW_LON_STA = '04' THEN CUR_STAT = 'IN DEFER';
IF WC_DW_LON_STA = '05' THEN CUR_STAT = 'IN FORB';
IF WC_DW_LON_STA = '06' THEN CUR_STAT = 'CURE';
IF WC_DW_LON_STA = '07' THEN CUR_STAT = 'CLAIM PEND';
IF WC_DW_LON_STA = '08' THEN CUR_STAT = 'CLAIM SUB';
IF WC_DW_LON_STA = '09' THEN CUR_STAT = 'CLAIM CANC';
IF WC_DW_LON_STA = '10' THEN CUR_STAT = 'CLAIM REJ';
IF WC_DW_LON_STA = '11' THEN CUR_STAT = 'CLAIM RET';
IF WC_DW_LON_STA = '12' THEN CUR_STAT = 'CLAIM PAID';
IF WC_DW_LON_STA = '13' THEN CUR_STAT = 'PRE-CLAIM PEN';
IF WC_DW_LON_STA = '14' THEN CUR_STAT = 'PRE-CLAIM SUB';
IF WC_DW_LON_STA = '15' THEN CUR_STAT = 'PRE-CLAIM CAN';
IF WC_DW_LON_STA = '16' THEN CUR_STAT = 'DEATH ALGD';
IF WC_DW_LON_STA = '17' THEN CUR_STAT = 'DEATH VERF';
IF WC_DW_LON_STA = '18' THEN CUR_STAT = 'DISAB ALGD';
IF WC_DW_LON_STA = '19' THEN CUR_STAT = 'DISAB VERF';
IF WC_DW_LON_STA = '20' THEN CUR_STAT = 'BK ALGD';
IF WC_DW_LON_STA = '21' THEN CUR_STAT = 'BK VERF';
IF WC_DW_LON_STA = '22' THEN CUR_STAT = 'PIF';
IF WC_DW_LON_STA = '23' THEN CUR_STAT = 'NOT FULLY ORG';
IF WC_DW_LON_STA = '88' THEN CUR_STAT = 'ERROR';
IF WC_DW_LON_STA = '98' THEN CUR_STAT = 'UNKNOWN';

FORMAT LN $2.;
LN = LN_SEQ;
RUN;


PROC PRINTTO PRINT=REPORT2 NEW;
RUN;

/*For landscape reports:*/
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS CENTER NODATE NUMBER PAGENO=1 PS=39 LS=127;

PROC CONTENTS DATA=DEMO2 OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 132*'-';
	PUT      ////////
		@51 '**** NO RECORDS FOUND ****';
	PUT ////////
		@57 '-- END OF REPORT --';
	PUT ////////////////
		@40 "JOB = Consolidation Payment Transaction, Loan Not PIF     REPORT = R2";
	END;
RETURN;
TITLE 'CONSOLIDATION PAYMENT TRANSACTION';
TITLE2 'LOAN NOT PIF';
RUN;

PROC PRINT NOOBS SPLIT='/' DATA=DEMO2;
FORMAT DT_107080 MMDDYY10. AMT_107080 DOLLAR10.2 DT_0101 MMDDYY10. AMT_0101 DOLLAR10.2 DT_0395 MMDDYY10. AMT_0395 DOLLAR10.2 LA_CUR_PRI DOLLAR10.2;
VAR ACC LN DT_107080 AMT_107080 DT_0101 AMT_0101 DT_0395 AMT_0395 LA_CUR_PRI IF_DOE_LDR LF_LON_CUR_OWN CUR_STAT;
LABEL	ACC = 'Account Number'
		LN = 'Ln/Sq'
		DT_107080 = 'Effective Date'
		AMT_107080 = 'Trans Amount'
		DT_0101 = 'New Disb Trans Dt'
		AMT_0101 = 'New Disb Trans Amt'
		DT_0395 = 'Loan Sale Date'
		AMT_0395 = 'Loan Sale Amount'
		LA_CUR_PRI = 'Current Balance'
		IF_DOE_LDR = 'Original Lender'
		LF_LON_CUR_OWN = 'Currnet Owner'
		CUR_STAT = 'Current Status';

TITLE 'CONSOLIDATION PAYMENT TRANSACTION';
TITLE2 'LOAN NOT PIF';
FOOTNOTE  'JOB = UTLWR03     REPORT = ULWR03.LWR03R2';
RUN;

PROC PRINTTO;
run;

/*FOR TESTING ONLY*/
/**/
/*PROC SQL;*/
/*CREATE TABLE DEMO3 AS*/
/*SELECT DISTINCT BF_SSN, LN, LD_LON_1_DSB*/
/*FROM DEMO2*/
/*;*/
/*QUIT;*/
/**/
/**/
/*data _null_;*/
/*set  WORK.Demo3                                   end=EFIEOD;*/
/*file 'T:\SAS\Consolidation Payment Transaction.txt' delimiter=',' DSD DROPOVER lrecl=32767;*/
/*   format BF_SSN $9. ;*/
/*   format LN $2. ;*/
/*   format LD_LON_1_DSB MMDDYY8. ;*/
/*if _n_ = 1 then       */
/* do;*/
/*   put*/
/*   'BF_SSN'*/
/*   ','*/
/*   'LN'*/
/*   ','*/
/*   'LD_LON_1_DSB'*/
/*   ;*/
/* end;*/
/* do;*/
/*   EFIOUT + 1;*/
/*   put BF_SSN $ @;*/
/*   put LN $ @;*/
/*   put LD_LON_1_DSB ;*/
/*   ;*/
/* end;*/
/**/
/*run;*/
