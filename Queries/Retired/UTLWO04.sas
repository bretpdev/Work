/*DISBURSEMENTS IN THE PRIOR MONTH CONVERTED TO SERVICING IN CURRENT MONTH*/
LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
/*%LET RPTLIB = T:\SAS;*/
FILENAME REPORT2 "&RPTLIB/ULWO04.LWO04R2";
FILENAME REPORT3 "&RPTLIB/ULWO04.LWO04R3";
FILENAME REPORT4 "&RPTLIB/ULWO04.LWO04R4";
FILENAME REPORT5 "&RPTLIB/ULWO04.LWO04R5";
FILENAME REPORT6 "&RPTLIB/ULWO04.LWO04R6";
FILENAME REPORT7 "&RPTLIB/ULWO04.LWO04R7";
FILENAME REPORT8 "&RPTLIB/ULWO04.LWO04R8";
FILENAME REPORT9 "&RPTLIB/ULWO04.LWO04R9";
FILENAME REPORT10 "&RPTLIB/ULWO04.LWO04R10";
FILENAME REPORT11 "&RPTLIB/ULWO04.LWO04R11";
FILENAME REPORT12 "&RPTLIB/ULWO04.LWO04R12";
FILENAME REPORT13 "&RPTLIB/ULWO04.LWO04R13";
FILENAME REPORT14 "&RPTLIB/ULWO04.LWO04R14";
FILENAME REPORT15 "&RPTLIB/ULWO04.LWO04R15";
FILENAME REPORT16 "&RPTLIB/ULWO04.LWO04R16";
FILENAME REPORT17 "&RPTLIB/ULWO04.LWO04R17";
FILENAME REPORT18 "&RPTLIB/ULWO04.LWO04R18";
FILENAME REPORT19 "&RPTLIB/ULWO04.LWO04R19";
FILENAME REPORT20 "&RPTLIB/ULWO04.LWO04R20";
FILENAME REPORT21 "&RPTLIB/ULWO04.LWO04R21";
FILENAME REPORT22 "&RPTLIB/ULWO04.LWO04R22";
FILENAME REPORT23 "&RPTLIB/ULWO04.LWO04R23";
FILENAME REPORT24 "&RPTLIB/ULWO04.LWO04R24";
FILENAME REPORT25 "&RPTLIB/ULWO04.LWO04R25";
FILENAME REPORT26 "&RPTLIB/ULWO04.LWO04R26";
FILENAME REPORT27 "&RPTLIB/ULWO04.LWO04R27";
FILENAME REPORT28 "&RPTLIB/ULWO04.LWO04R28";
FILENAME REPORT29 "&RPTLIB/ULWO04.LWO04R29";
FILENAME REPORTZ "&RPTLIB/ULWO04.LWO04RZ";
OPTIONS SYMBOLGEN;
*FIND THE NEXT-TO-LAST WORKING DAY OF THE MONTH AND FIRST DAY OF CURRENT MONTH;
DATA _NULL_;
*TODAY()+3 IS USED IN CASE EOM HAPPENS PRIOR TO THE LAST CALENDAR DAY;
NTLWD = INTNX('MONTH',TODAY()+3,-2,'end');
IF WEEKDAY(NTLWD) = 1 THEN NTLWD = NTLWD - 3;
ELSE IF WEEKDAY(NTLWD) = 2 THEN NTLWD = NTLWD - 3;
ELSE IF WEEKDAY(NTLWD) = 3 THEN NTLWD = NTLWD - 1;
ELSE IF WEEKDAY(NTLWD) = 4 THEN NTLWD = NTLWD - 1;
ELSE IF WEEKDAY(NTLWD) = 5 THEN NTLWD = NTLWD - 1;
ELSE IF WEEKDAY(NTLWD) = 6 THEN NTLWD = NTLWD - 1;
ELSE IF WEEKDAY(NTLWD) = 7 THEN NTLWD = NTLWD - 2;
CALL SYMPUT('NTLWD',"'"||PUT(NTLWD,MMDDYY10.)||"'");
FOMDT = INTNX('MONTH',TODAY()+3,-1,'BEGINNING');
CALL SYMPUT('FOMDT',"'"||PUT(FOMDT,MMDDYY10.)||"'");
CALL SYMPUT('CUMO',TRIM(LEFT(UPCASE(PUT(FOMDT,MONNAME9.))))||' '||PUT(YEAR(FOMDT),4.));
RUN;

/*%SYSLPUT NTLWD = &NTLWD;*/
/*%SYSLPUT FOMDT = &FOMDT;*/
/*libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;*/
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
CREATE TABLE OLDDSB AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT INTEGER(A.BF_SSN) AS BF_SSN
	,B.AN_SEQ
	,COALESCE(A.LA_DSB,0) - COALESCE(A.LA_DSB_CAN,0) AS DISBAMT
	,COALESCE(D.LA_DSB_FEE,0) AS ORIGFEE
	,COALESCE(I.LA_DSB_FEE,0) AS GTYFEE
	,A.LA_DSB_ORG_CHK_EFT AS NETDISB
	,A.LD_DSB_ROS_PRT
	,A.LD_DSB
	,A.IC_LON_PGM
	,B.AF_DOE_SCL
    ,C.IF_DOE_LDR
	,E.IM_LDR_FUL
	,H.LD_FAT_PST
	,C.IF_TIR_PCE
	,B.AF_RGL_CAT_LP20
	,PL_DISB.DSB_IND
FROM OLWHRM1.LN15_DSB A
INNER JOIN OLWHRM1.AP03_MASTER_APL B 
	ON A.AF_APL_ID = B.AF_APL_ID
INNER JOIN OLWHRM1.LN10_LON C
    ON C.BF_SSN = A.BF_SSN
    AND C.LN_SEQ = A.LN_SEQ
    AND C.IC_LON_PGM = A.IC_LON_PGM
INNER JOIN OLWHRM1.LN18_DSB_FEE D
    ON D.BF_SSN = A.BF_SSN
    AND D.LN_BR_DSB_SEQ = A.LN_BR_DSB_SEQ
    AND D.LC_DSB_FEE = '02'	/*ORG FEE*/
LEFT OUTER JOIN OLWHRM1.LN18_DSB_FEE I
    ON I.BF_SSN = A.BF_SSN
    AND I.LN_BR_DSB_SEQ = A.LN_BR_DSB_SEQ
    AND I.LC_DSB_FEE = '21'	/*GTY FEE*/
INNER JOIN OLWHRM1.LR10_LDR_DMO E
	ON E.IF_DOE_LDR = C.IF_DOE_LDR
INNER JOIN OLWHRM1.LN93_DSB_FIN_TRX G
	ON G.BF_SSN = A.BF_SSN
	AND G.LN_BR_DSB_SEQ = A.LN_BR_DSB_SEQ
INNER JOIN OLWHRM1.LN90_FIN_ATY H
	ON H.BF_SSN = G.BF_SSN
	AND H.LN_SEQ = G.LN_SEQ
	AND H.LN_FAT_SEQ = G.LN_FAT_SEQ
	AND H.LC_STA_LON90 = 'A'
LEFT OUTER JOIN (
	SELECT BF_SSN 
		,LN_SEQ
		,'X' AS DSB_IND
	FROM OLWHRM1.LN15_DSB
	WHERE DAYS(LD_DSB) BETWEEN DAYS('07/01/2006') AND DAYS('08/07/2006')
	AND IC_LON_PGM IN ('PLUS','PLUSGB')
	AND LC_STA_LON15 IN ('1','3')
	AND LC_DSB_TYP = '2'
	AND LA_DSB <> COALESCE(LA_DSB_CAN,0)
	) PL_DISB
	ON A.BF_SSN = PL_DISB.BF_SSN
	AND A.LN_SEQ = PL_DISB.LN_SEQ
WHERE A.LC_DSB_RLS_STA = 'R'
AND (A.LA_DSB_CAN IS NULL
    OR A.LA_DSB_CAN <> A.LA_DSB)
AND H.PC_FAT_TYP = '01'
AND H.PC_FAT_SUB_TYP = '01'
AND A.LD_DSB_ROS_PRT <= &NTLWD
AND H.LD_FAT_PST > &FOMDT
AND A.LC_LDR_DSB_MDM NOT IN ('M','F','E')
/*EXCLUDE REVERSALS*/
AND H.LN_FAT_SEQ_REV IS NULL
ORDER BY C.IF_DOE_LDR, A.IC_LON_PGM, A.BF_SSN
);
DISCONNECT FROM DB2;
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;
%SQLCHECK (SQLRPT=UTLWO04.LWO04RZ);
QUIT;
DATA OLDDSB;
SET OLDDSB;
/*NETDISB = SUM(DISBAMT,-ORIGFEE);*/
IF IF_DOE_LDR = '813760UT' THEN IF_DOE_LDR = '813760';/*Key Bank*/
ELSE IF IF_DOE_LDR = '820043' THEN IF_DOE_LDR = '829505';/*Washington Mutual lender merge;*/
ELSE IF_DOE_LDR = IF_DOE_LDR;
RUN;
/*ENDRSUBMIT;*/
/*DATA OLDDSB; */
/*SET WORKLOCL.OLDDSB; */
/*RUN;*/

DATA OLDDSB;
SET OLDDSB;
LENGTH LOC1 $ 4. LOC2 $ 40.;
IF INPUT(AF_RGL_CAT_LP20,BEST12.) <= 1999030 THEN
	LOC1 = '3%';
ELSE IF INPUT(AF_RGL_CAT_LP20,BEST12.) = 2006020 AND (
	DSB_IND = 'X' OR IC_LON_PGM IN ('STFFRD','UNSTFD')) THEN 
	LOC1 = '2%';
ELSE IF INPUT(AF_RGL_CAT_LP20,BEST12.) = 2006020 AND DSB_IND = '' AND IC_LON_PGM IN ('PLUS','PLUSGB') THEN 
	LOC1 = '3%';
ELSE IF INPUT(AF_RGL_CAT_LP20,BEST12.) = 2007020 AND IC_LON_PGM IN ('STFFRD','UNSTFD') THEN 
	LOC1 = '1.5%';
ELSE IF INPUT(AF_RGL_CAT_LP20,BEST12.) = 2008020 AND IC_LON_PGM IN ('STFFRD','UNSTFD') THEN 
	LOC1 = '1%';
ELSE IF INPUT(AF_RGL_CAT_LP20,BEST12.) = 2009020 AND IC_LON_PGM IN ('STFFRD','UNSTFD') THEN 
	LOC1 = '0.5%';
ELSE IF INPUT(AF_RGL_CAT_LP20,BEST12.) = 2009020 AND IC_LON_PGM IN ('PLUS','PLUSGB') THEN 
	LOC1 = '3%';

IF IF_TIR_PCE ^= '' THEN DO;
	IF INPUT(AF_RGL_CAT_LP20,BEST12.) = 2008020 THEN 
		LOC2 = 'UHEAA PAID ORIGINATION FEE';
	ELSE 
		LOC2 = 'LOAN WITH ZERO ORIGINATION FEE';
END;

IF ORIGFEE > 0 THEN DO;
	IF INPUT(AF_RGL_CAT_LP20,BEST12.) = 2009020 AND IC_LON_PGM IN ('PLUS','PLUSGB') THEN DO;
		IF GTYFEE > 0 THEN
			LOC2 = 'LOANS WITH ORIG / GUAR FEE ED 3% / 1%';
		ELSE 
			LOC2 = 'LOAN WITH ORIGINATION FEE 3%';
	END;
	ELSE DO;
		IF GTYFEE > 0 THEN
			LOC2 = 'LOANS WITH ORIG / GUAR FEE ED 0.5% / 1%';
		ELSE 
			LOC2 = 'LOAN WITH ORIGINATION FEE 3%';
	END;
END;
RUN;

%MACRO PRINT(LENDER=,REPNO=);

%LET TTL = ;

DATA OLDDSBLDR;
SET OLDDSB;
WHERE IF_DOE_LDR = "&LENDER";
RUN;

DATA _NULL_;
SET OLDDSBLDR;
CALL SYMPUT('TTL',TRIM(IM_LDR_FUL));
RUN;

PROC PRINTTO PRINT=REPORT&REPNO NEW;
RUN;
OPTIONS CENTER NODATE NUMBER PAGENO=1 LS=132;
PROC CONTENTS DATA=OLDDSBLDR OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 126*'-';
	PUT      ////////
	   @51 '**** NO OBSERVATIONS FOUND ****';
	PUT ////////
	   @57 '-- END OF REPORT --';
	PUT ///////////
	   @46 "JOB = UTLWO04     REPORT = ULWO04.LWO04R&REPNO";
END;
RETURN;
TITLE "UTAH HIGHER EDUCATION ASSISTANCE AUTHORITY";
TITLE2 "DISBURSEMENTS IN PRIOR MONTH";
TITLE3 "CONVERTED TO SERVICING IN &CUMO";
TITLE4 "LENDER ID # &LENDER";
run;

PROC REPORT DATA=OLDDSBLDR NOWD HEADSKIP SPLIT='/' SPACING=1;
TITLE "UTAH HIGHER EDUCATION ASSISTANCE AUTHORITY";
TITLE2 "DISBURSEMENTS IN PRIOR MONTH";
TITLE3 "CONVERTED TO SERVICING IN &CUMO";
TITLE4 "LENDER ID # &LENDER - &TTL";
FOOTNOTE1  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE2	"Please take appropriate precautions to safeguard this information.";
FOOTNOTE3	;
FOOTNOTE4 "JOB = UTLWO04     REPORT = ULWO04.LWO04R&REPNO";
COLUMN LOC2 LOC1 BF_SSN AN_SEQ DISBAMT ORIGFEE NETDISB LD_DSB_ROS_PRT LD_DSB
IC_LON_PGM AF_DOE_SCL LD_FAT_PST /*N*/;
DEFINE LOC2 / GROUP "CATEGORY" WIDTH=15 FLOW;
DEFINE LOC1 / GROUP "%" WIDTH=4;
DEFINE BF_SSN / ORDER "SSN" FORMAT=SSN11. LEFT;
DEFINE AN_SEQ / ORDER "APP SEQ" WIDTH=3;
DEFINE DISBAMT / ANALYSIS FORMAT=COMMA12.2 "GROSS DISB/AMOUNT";
DEFINE ORIGFEE / ANALYSIS FORMAT=COMMA8.2 WIDTH=11 "ORIGINATION/FEE";
DEFINE NETDISB / ANALYSIS FORMAT=COMMA12.2 "NET DISB/AMOUNT" ;
DEFINE LD_DSB_ROS_PRT / ORDER FORMAT=MMDDYY10. "ROSTER/DATE" ;
DEFINE LD_DSB / ORDER FORMAT=MMDDYY10. "DISB/DATE" WIDTH=10;
DEFINE IC_LON_PGM / ORDER "LOAN/TYPE" WIDTH=6;
DEFINE AF_DOE_SCL / ORDER "SCHOOL/CODE" ;
DEFINE LD_FAT_PST / ORDER FORMAT=MMDDYY10. WIDTH=10 "CONVERTED/TO/SERVICING";
/*DEFINE N / NOPRINT;*/
BREAK AFTER LOC2 / SUMMARIZE SUPPRESS SKIP OL;
COMPUTE AFTER;
LINE ' ';
LINE @1 'TOTAL GROSS DISBBURSEMENTS:' @33 DISBAMT.SUM DOLLAR14.2;
LINE @1 'TOTAL ORIGINATION FEES:' @33 ORIGFEE.SUM DOLLAR14.2;
LINE @1 'TOTAL NET DISBBURSEMENTS:' @33 NETDISB.SUM DOLLAR14.2;
ENDCOMP;
RUN;
PROC PRINTTO;
RUN;
%MEND PRINT;

%PRINT(LENDER=811698,REPNO=2);
%PRINT(LENDER=813760,REPNO=3);
%PRINT(LENDER=813894,REPNO=4);
%PRINT(LENDER=817440,REPNO=5);
%PRINT(LENDER=817545,REPNO=6);
%PRINT(LENDER=817546,REPNO=7);
%PRINT(LENDER=819628,REPNO=8);
%PRINT(LENDER=829505,REPNO=9);
%PRINT(LENDER=820200,REPNO=10);
%PRINT(LENDER=822373,REPNO=11);
%PRINT(LENDER=829123,REPNO=12);
%PRINT(LENDER=829158,REPNO=13);
%PRINT(LENDER=830132,REPNO=14);
%PRINT(LENDER=830146,REPNO=15);
%PRINT(LENDER=830791,REPNO=16);
%PRINT(LENDER=832241,REPNO=17);
%PRINT(LENDER=833828,REPNO=18);
%PRINT(LENDER=817455,REPNO=19);
%PRINT(LENDER=817575,REPNO=20);
%PRINT(LENDER=834122,REPNO=21);
%PRINT(LENDER=833577,REPNO=22);
%PRINT(LENDER=834265,REPNO=23);
%PRINT(LENDER=828476,REPNO=24);
%PRINT(LENDER=834370,REPNO=25);
%PRINT(LENDER=834437,REPNO=26);
%PRINT(LENDER=834396,REPNO=27);
%PRINT(LENDER=82847601,REPNO=28);
%PRINT(LENDER=834493,REPNO=29);


/*PROC EXPORT DATA= OLDDSB*/
/*            OUTFILE= "T:\SAS\UTLWO04_DETAIL.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/
