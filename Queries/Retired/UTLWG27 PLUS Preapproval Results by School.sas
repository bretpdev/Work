/*UTLWG27 PLUS PREAPPROVAL RESULTS BY SCHOOL*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT3 "&RPTLIB/ULWG27.LWG27R3";
FILENAME REPORT4 "&RPTLIB/ULWG27.LWG27R4";
FILENAME REPORT5 "&RPTLIB/ULWG27.LWG27R5";
FILENAME REPORT6 "&RPTLIB/ULWG27.LWG27R6";
FILENAME REPORT7 "&RPTLIB/ULWG27.LWG27R7";
FILENAME REPORT8 "&RPTLIB/ULWG27.LWG27R8";
FILENAME REPORT9 "&RPTLIB/ULWG27.LWG27R9";
FILENAME REPORT10 "&RPTLIB/ULWG27.LWG27R10";
FILENAME REPORT11 "&RPTLIB/ULWG27.LWG27R11";
FILENAME REPORT12 "&RPTLIB/ULWG27.LWG27R12";
FILENAME REPORT13 "&RPTLIB/ULWG27.LWG27R13";
FILENAME REPORT14 "&RPTLIB/ULWG27.LWG27R14";
FILENAME REPORT15 "&RPTLIB/ULWG27.LWG27R15";
FILENAME REPORT16 "&RPTLIB/ULWG27.LWG27R16";
FILENAME REPORT17 "&RPTLIB/ULWG27.LWG27R17";
FILENAME REPORT18 "&RPTLIB/ULWG27.LWG27R18";
FILENAME REPORT19 "&RPTLIB/ULWG27.LWG27R19";
FILENAME REPORT20 "&RPTLIB/ULWG27.LWG27R20";
FILENAME REPORT21 "&RPTLIB/ULWG27.LWG27R21";
FILENAME REPORT22 "&RPTLIB/ULWG27.LWG27R22";
FILENAME REPORT23 "&RPTLIB/ULWG27.LWG27R23";
FILENAME REPORT24 "&RPTLIB/ULWG27.LWG27R24";
FILENAME REPORT25 "&RPTLIB/ULWG27.LWG27R25";
FILENAME REPORT26 "&RPTLIB/ULWG27.LWG27R26";
FILENAME REPORT27 "&RPTLIB/ULWG27.LWG27R27";
FILENAME REPORT28 "&RPTLIB/ULWG27.LWG27R28";
FILENAME REPORT29 "&RPTLIB/ULWG27.LWG27R29";
FILENAME REPORT31 "&RPTLIB/ULWG27.LWG27R31";
FILENAME REPORT32 "&RPTLIB/ULWG27.LWG27R32";
FILENAME REPORT33 "&RPTLIB/ULWG27.LWG27R33";
FILENAME REPORT35 "&RPTLIB/ULWG27.LWG27R35";
FILENAME REPORT37 "&RPTLIB/ULWG27.LWG27R37";
FILENAME REPORT39 "&RPTLIB/ULWG27.LWG27R39";
FILENAME REPORT41 "&RPTLIB/ULWG27.LWG27R41";
FILENAME REPORT43 "&RPTLIB/ULWG27.LWG27R43";
FILENAME REPORT45 "&RPTLIB/ULWG27.LWG27R45";
FILENAME REPORT47 "&RPTLIB/ULWG27.LWG27R47";
FILENAME REPORT49 "&RPTLIB/ULWG27.LWG27R49";
FILENAME REPORT51 "&RPTLIB/ULWG27.LWG27R51";
FILENAME REPORT52 "&RPTLIB/ULWG27.LWG27R52";
FILENAME REPORT53 "&RPTLIB/ULWG27.LWG27R53";
FILENAME REPORT55 "&RPTLIB/ULWG27.LWG27R55";
FILENAME REPORT57 "&RPTLIB/ULWG27.LWG27R57";
FILENAME REPORT59 "&RPTLIB/ULWG27.LWG27R59";
FILENAME REPORT61 "&RPTLIB/ULWG27.LWG27R61";
FILENAME REPORT63 "&RPTLIB/ULWG27.LWG27R63";
FILENAME REPORT65 "&RPTLIB/ULWG27.LWG27R65";
FILENAME REPORT67 "&RPTLIB/ULWG27.LWG27R67";
FILENAME REPORT69 "&RPTLIB/ULWG27.LWG27R69";
FILENAME REPORT71 "&RPTLIB/ULWG27.LWG27R71";
FILENAME REPORT73 "&RPTLIB/ULWG27.LWG27R73";
FILENAME REPORT75 "&RPTLIB/ULWG27.LWG27R75";
FILENAME REPORT76 "&RPTLIB/ULWG27.LWG27R76";
FILENAME REPORT77 "&RPTLIB/ULWG27.LWG27R77";
FILENAME REPORT78 "&RPTLIB/ULWG27.LWG27R78";
FILENAME REPORT79 "&RPTLIB/ULWG27.LWG27R79";
FILENAME REPORT80 "&RPTLIB/ULWG27.LWG27R80";
FILENAME REPORT81 "&RPTLIB/ULWG27.LWG27R81";
FILENAME REPORT82 "&RPTLIB/ULWG27.LWG27R82";
FILENAME REPORT84 "&RPTLIB/ULWG27.LWG27R84";
FILENAME REPORT85 "&RPTLIB/ULWG27.LWG27R85";
FILENAME REPORT86 "&RPTLIB/ULWG27.LWG27R86";
FILENAME REPORT87 "&RPTLIB/ULWG27.LWG27R87";
FILENAME REPORT88 "&RPTLIB/ULWG27.LWG27R88";
FILENAME REPORT89 "&RPTLIB/ULWG27.LWG27R89";
FILENAME REPORT90 "&RPTLIB/ULWG27.LWG27R90";
FILENAME REPORT91 "&RPTLIB/ULWG27.LWG27R91";
FILENAME REPORT92 "&RPTLIB/ULWG27.LWG27R92";
FILENAME REPORT93 "&RPTLIB/ULWG27.LWG27R93";
FILENAME REPORT94 "&RPTLIB/ULWG27.LWG27R94";
FILENAME REPORT95 "&RPTLIB/ULWG27.LWG27R95";
FILENAME REPORT96 "&RPTLIB/ULWG27.LWG27R96";
FILENAME REPORT97 "&RPTLIB/ULWG27.LWG27R97";
FILENAME REPORT98 "&RPTLIB/ULWG27.LWG27R98"; /*BANK ONE*/
FILENAME REPORT99 "&RPTLIB/ULWG27.LWG27R99"; /*COMPREHESIVE BY SCHOOLS*/
FILENAME REPORT2 "&RPTLIB/ULWG27.LWG27R2";   /*DENIALS WITH INVALID ADDRESSES*/
FILENAME REPORTZ "&RPTLIB/ULWG27.LWG27RZ";

/*FILENAME REPORT2 "C:/WINDOWS/TEMP/ULWG27.LWG27R2";*/
/*FILENAME REPORT97 "C:/WINDOWS/TEMP/ULWG27.LWG27R97";*/
/*FILENAME REPORT98 "C:/WINDOWS/TEMP/ULWG27.LWG27R98";*/

options symbolgen;
DATA _NULL_;	
/*THIS WILL CREATE 2 VALUES "BEGIN" AND "END". "END" WILL ALWAYS BE YESTERDAYS DATE.*/
/*"BEGIN" WILL BE YESTERDAYS DATE OR IF IT WAS A HOLIDAY IT WILL BE THE HOLIDAYS DATE, */
/* OR IF IT WAS A WEEK END IT WILL BE THE SATURDAY BEFOR.*/	
	EFFDT = TODAY() - 1;
    CALL SYMPUT('EFFDATE',put(EFFDT,MMDDYY10.));
	CALL SYMPUT('END',"'"||put(EFFDT,MMDDYY10.)||"'"); /*END DATE WILL ALWAYS BE YESTERDAY*/ 
	START = TODAY() - 1; /*START DATE IS SET TO YESTERDAY*/
	CALL SYMPUT('BEGIN',"'"||put(START,MMDDYY10.)||"'");

RUN;
%SYSLPUT EFFDATE = "&EFFDATE";	/*YESTERDAY WITH OUT TICK MARKS*/
%SYSLPUT END = &END;			/*YESTERDAY WITH TICK MARKS*/
%SYSLPUT BEGIN = &BEGIN;		/*LAST DAY NOT */

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
CREATE TABLE DEMO2 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT
	DISTINCT

	CASE
		WHEN A.AC_CRD_CHK_PRF = 'D' THEN 'Denied'
		WHEN A.AC_CRD_CHK_PRF = 'Y' THEN 'Approved'
		WHEN (A.AC_CRD_CHK_PRF = '' AND G.PF_ACT = 'GCCKS' AND G.BD_ATY_PRF = B.AD_PRC) THEN 'Staff Review'
	END AS CREDITCHECKRESULT
	
	,A.AC_CRD_CHK_PRF
	,A.AD_CRD_CHK_PRF AS DATECREDITCHECKEDPERFORMED
	,G.PF_ACT

	,A.DF_PRS_ID_BR AS BORROWERSSN
	,RTRIM(F.DM_PRS_1) || ' ' || F.DM_PRS_MID || ' ' || RTRIM(F.DM_PRS_LST) AS BORROWERNAME
	,RTRIM(A.AX_BR_REQ_IAA) AS REQUESTEDLOANAMOUNT
	,A.DF_PRS_ID_STU AS STUDENTSSN
	,RTRIM(C.DM_PRS_1) || ' ' || C.DM_PRS_MID || ' ' || RTRIM(C.DM_PRS_LST) AS STUDENTNAME
	,CASE 
		WHEN A.AF_APL_OPS_SCL = '01009801' THEN '01009800' 
		ELSE A.AF_APL_OPS_SCL 
	 END AS SCHOOLCODE
	,D.IM_IST_FUL AS SCHOOL
	,A.AF_ORG_APL_OPS_LDR AS LENDERCODE
	,A.AX_APL_SRC_CDE AS SOURCECODE
	,F.DM_PRS_1 AS FNAME
	,F.DM_PRS_LST AS LNAME
	,F.DX_STR_ADR_1 AS ADD1
	,F.DX_STR_ADR_2 AS ADD2
	,F.DI_VLD_ADR AS ADDV
	
	,F.DM_CT AS CITY
	,F.DC_DOM_ST AS STATE
	,F.DF_ZIP AS ZIP
	,F.DM_FGN_CNY AS COUNTRY
	,F.DF_SPE_ACC_ID AS ACCOUNT
	,C.DF_SPE_ACC_ID AS ACCOUNTSTU
	,F.DC_ADR 
/*	,G.BD_ATY_PRF AS RAWDATE*/
	
	,B.AD_PRC AS OLDDATE
	,SUBSTR(G.BX_CMT,1,3) AS CMT_CHAR
	
	,A.AD_CRD_CHK_PRF
	,A.AC_APL_TYP
	,B.AC_PRC_STA
	,B.AF_APL_ID || B.AF_APL_ID_SFX AS CLUID
	,B.AC_LON_TYP

	FROM	OLWHRM1.GA01_APP A 
	INNER JOIN OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
	INNER JOIN OLWHRM1.PD01_PDM_INF C
	ON C.DF_PRS_ID = A.DF_PRS_ID_STU
	INNER JOIN OLWHRM1.SC01_LGS_SCL_INF D
	ON D.IF_IST = A.AF_APL_OPS_SCL
	INNER JOIN OLWHRM1.PD01_PDM_INF F
	ON F.DF_PRS_ID = A.DF_PRS_ID_BR
	INNER JOIN OLWHRM1.AY01_BR_ATY G
	ON G.DF_PRS_ID = A.DF_PRS_ID_BR

	WHERE A.AD_CRD_CHK_PRF BETWEEN &BEGIN AND &END
	AND A.AC_APL_TYP = 'P'
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWAB1.LWAB1RZ);*/
/*QUIT;*/
ENDRSUBMIT;
DATA DEMO2; SET WORKLOCL.DEMO2; RUN;

DATA DEMO2;
LENGTH REQUESTEDLOANAMOUNT $15.;
SET DEMO2;
FORMAT DOL 15. REQAMOUNT $15. REQUESTEDLOANAMOUNT $15. DATECREDITCHECKEDPERFORMED MMDDYY10.;
DOL = REQUESTEDLOANAMOUNT;
REQAMOUNT = DOL; 
REQAMOUNT = LEFT(REQAMOUNT);
REQAMOUNT = TRIM(REQAMOUNT);

NUM = LENGTH(REQAMOUNT);
ZERO = '.00';

IF REQAMOUNT = '999999' THEN REQAMOUNT = 'MAXIMUM';
ELSE IF REQAMOUNT = . THEN REQAMOUNT = '$0';
ELSE IF NUM < 4 THEN REQAMOUNT = '$'||REQAMOUNT;
ELSE IF NUM >= 4 THEN REQAMOUNT = '$'||SUBSTR(REQAMOUNT,1, NUM - 3)||','||SUBSTR(REQAMOUNT,NUM - 2);

RUN;
DATA WORK.DEMO2;
SET WORK.DEMO2;
LENGTH REQUESTEDLOANAMOUNT $15.;
FORMAT REQUESTEDLOANAMOUNT $15. REQ $15.;
IF REQAMOUNT = 'MAXIMUM' THEN REQ = 'MAXIMUM';
ELSE REQ = TRIM(REQAMOUNT) || ZERO;
REQUESTEDLOANAMOUNT = REQ;
RUN;

PROC SORT DATA=WORK.DEMO2 nodupkey;
BY SCHOOLCODE BORROWERSSN;
RUN;

DATA DATAKEY;
SET DEMO2;
WHERE CREDITCHECKRESULT = 'Denied' AND ADDV = 'Y' AND CMT_CHAR <> 'MAN';
RUN;

*CALCULATE KEYLINE;
DATA DATAKEY (DROP = KEYSSN MODAY KEYLINE CHKDIG DIG I 
	CHKDIG CHK1 CHK2 CHK3 CHKDIGIT CHECK);
SET DATAKEY;
KEYSSN = TRANSLATE(BORROWERSSN,'MYLAUGHTER','0987654321');
MODAY = PUT(DATE(),MMDDYYN4.);
KEYLINE = "P"||KEYSSN||MODAY||DC_ADR;
CHKDIG = 0;
LENGTH DIG $2.;
DO I = 1 TO LENGTH(KEYLINE);
	IF I/2 NE ROUND(I/2,1) 
		THEN DIG = PUT(INPUT(SUBSTR(KEYLINE,I,1),BITS4.4) * 2, 2.);
	ELSE DIG = PUT(INPUT(SUBSTR(KEYLINE,I,1),BITS4.4), 2.);
	IF SUBSTR(DIG,1,1) = " " 
		THEN CHKDIG = CHKDIG + INPUT(SUBSTR(DIG,2,1),1.);
		ELSE DO;
			CHK1 = INPUT(SUBSTR(DIG,1,1),1.);
			CHK2 = INPUT(SUBSTR(DIG,2,1),1.);
			IF CHK1 + CHK2 >= 10
				THEN DO;
					CHK3 = PUT(CHK1 + CHK2,2.);
					CHK1 = INPUT(SUBSTR(CHK3,1,1),1.);
					CHK2 = INPUT(SUBSTR(CHK3,2,1),1.);
				END;
			CHKDIG = CHKDIG + CHK1 + CHK2;
		END;
END;
CHKDIGIT = 10 - INPUT(SUBSTR((RIGHT(PUT(CHKDIG,3.))),3,1),3.);
IF CHKDIGIT = 10 THEN CHKDIGIT = 0;
CHECK = PUT(CHKDIGIT,1.);
ACSKEY = "#"||KEYLINE||CHECK||"#";
RUN;

PROC SQL;
CREATE TABLE DATAKEY_1 AS
SELECT DISTINCT BORROWERSSN, FNAME, LNAME, CLUID, STUDENTSSN
FROM DATAKEY;
QUIT;

PROC SORT DATA=WORK.DATAKEY_1;
BY LNAME;
RUN;

DATA DEMO2;
SET DEMO2;
WHERE CMT_CHAR <> 'WEB';
RUN;



PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
PROC REPORT DATA=WORK.DEMO2 NOWD HEADSKIP SPLIT='/';
WHERE CREDITCHECKRESULT = 'Denied' AND ADDV = 'N';
TITLE 'PLUS Preapproval Results by School';
TITLE2 'Denials with Invalid Addresses';
FOOTNOTE "JOB = UTLWG27     REPORT = ULWG27.LWG27R2";
COLUMN BORROWERSSN FNAME LNAME ADD1 ADD2 CITY STATE ZIP COUNTRY AC_LON_TYP;


DEFINE BORROWERSSN / DISPLAY "SSN" WIDTH = 15;
DEFINE FNAME / DISPLAY "First Name" WIDTH = 12;
DEFINE LNAME / DISPLAY "Last Name" WIDTH = 12;
DEFINE ADD1 / DISPLAY "Addr 1" WIDTH = 20;
DEFINE ADD2 / DISPLAY "Addr 2" WIDTH = 10;
DEFINE CITY / DISPLAY "City" WIDTH = 15;
DEFINE STATE / DISPLAY "State" WIDTH = 5;
DEFINE ZIP / DISPLAY "Zip" WIDTH = 9;
DEFINE COUNTRY / DISPLAY "Country" WIDTH = 10;
DEFINE AC_LON_TYP / DISPLAY "LOAN/TYPE" WIDTH = 5;
RUN;

%MACRO PRINTEM(SCH=, SCHNAME=, RPNO=);
PROC PRINTTO PRINT=REPORT&RPNO NEW;
RUN;

OPTIONS PAGENO=1 ORIENTATION=LANDSCAPE ;/*NOBYLINE*/
OPTIONS PS=39 LS=127;

DATA SCHOOLDATA;
SET WORK.DEMO2;
WHERE SCHOOLCODE = "&SCH";
BY SCHOOLCODE SCHOOL;
TYPESORT = BORROWERSSN;
RUN;

DATA _NULL_;		
SET WORK.SCHOOLDATA;
	CALL SYMPUT('SNAME', "'"||PUT(SCHOOL, $30.)||"'");
	CALL SYMPUT('SCODE', SCHOOLCODE);
RUN;

%IF &RPNO = 75 %THEN %DO;
/*CREATE DATASET FOR REPORT76*/
DATA _NULL_;
CALL SYMPUT('TS1',"'NO'");
RUN;

DATA _NULL_;
SET SCHOOLDATA;
CALL SYMPUT('TS1',"'YES'");
RUN;

DATA TB76;
FORMAT TS $3.;
TS = &TS1;
RUN;
%END;

PROC CONTENTS DATA=WORK.SCHOOLDATA OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
   PUT // 126*'-';
   PUT      ////////
       @53 '**** NO RECORD FOUND ****';
   PUT ////////
       @56 '-- END OF REPORT --';
   PUT /////
   		@44 "JOB = UTLWG27     REPORT = ULWG27.LWG27R&RPNO";
   END;
RETURN;
TITLE "PLUS Preapproval Results by School";
TITLE2 "&SCHNAME - &SCH";
TITLE4 "Run Date : &EFFDATE";
run;
/*REPORTS BY SCHOOL, PAGE 1*/
PROC REPORT DATA=WORK.SCHOOLDATA NOWD HEADSKIP SPLIT='/';
BY SCHOOLCODE SCHOOL;
TITLE 'PLUS Preapproval Results by School';
TITLE2 '#BYVAL2 - #BYVAL1';
FOOTNOTE "JOB = UTLWG27     REPORT = ULWG27.LWG27R&RPNO";
COLUMN CREDITCHECKRESULT DATECREDITCHECKEDPERFORMED LENDERCODE BORROWERSSN
		BORROWERNAME REQ STUDENTSSN STUDENTNAME AC_LON_TYP;
DEFINE CREDITCHECKRESULT / DISPLAY "CC Result" WIDTH = 6;
DEFINE DATECREDITCHECKEDPERFORMED / DISPLAY "Date of CC" WIDTH = 12;
DEFINE BORROWERSSN / DISPLAY "Borrower SSN" WIDTH = 11;
DEFINE BORROWERNAME / DISPLAY "Borrower Name" WIDTH = 20;
DEFINE REQ / DISPLAY "Req Ln Amt" WIDTH = 15;
DEFINE STUDENTSSN / DISPLAY "Student SSN" WIDTH = 11;
DEFINE STUDENTNAME / DISPLAY "Student Name" WIDTH = 20;
DEFINE LENDERCODE / DISPLAY "Lender ID" WIDTH = 7;
DEFINE AC_LON_TYP / DISPLAY "Loan Type" WIDTH=5;

RUN;


/*REPORTS BY SCHOOL AND BY BORROWER, PAGE 2*/
PROC REPORT DATA=WORK.SCHOOLDATA NOWD HEADSKIP SPLIT='/';
BY BORROWERSSN;
TITLE "PLUS Preapproval Results by Borrower";
FOOTNOTE "JOB = UTLWG27     REPORT = ULWG27.LWG27R&RPNO";
COLUMN CREDITCHECKRESULT DATECREDITCHECKEDPERFORMED LENDERCODE BORROWERSSN
		BORROWERNAME REQ STUDENTSSN STUDENTNAME AC_LON_TYP;
DEFINE CREDITCHECKRESULT / DISPLAY "CC Result" WIDTH = 6;
DEFINE DATECREDITCHECKEDPERFORMED / DISPLAY "Date of CC" WIDTH = 12;
DEFINE BORROWERSSN / DISPLAY "Borrower SSN" WIDTH = 11;
DEFINE BORROWERNAME / DISPLAY "Borrower Name" WIDTH = 20;
DEFINE REQ / DISPLAY "Req Ln Amt" WIDTH = 15;
DEFINE STUDENTSSN / DISPLAY "Student SSN" WIDTH = 11;
DEFINE STUDENTNAME / DISPLAY "Student Name" WIDTH = 20;
DEFINE LENDERCODE / DISPLAY "Lender ID" WIDTH = 7;
DEFINE AC_LON_TYP / DISPLAY "Loan Type" WIDTH=5;
RUN;
%MEND PRINTEM;

%MACRO PRINTALL(RPNO=);
PROC PRINTTO PRINT=REPORT&RPNO NEW;
RUN;

OPTIONS PAGENO=1 ORIENTATION=LANDSCAPE NOBYLINE ;
OPTIONS PS=39 LS=127;

DATA _NULL_;		
SET WORK.DEMO2;
	CALL SYMPUT('SNAME', "'"||PUT(SCHOOL, $30.)||"'");
	CALL SYMPUT('SCODE', SCHOOLCODE);
RUN;

PROC CONTENTS DATA=WORK.DEMO2 OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
   PUT // 126*'-';
   PUT      ////////
       @53 '**** NO RECORD FOUND ****';
   PUT ////////
       @56 '-- END OF REPORT --';
   PUT /////
   		@44 "JOB = UTLWG27     REPORT = ULWG27.LWG27R&RPNO";
   END;
RETURN;
TITLE "Loan Origination PLUS Preapproval Results by School";
TITLE3 'School Name - School Code';
TITLE4 "Run Date : &EFFDATE";
run;

PROC REPORT DATA=WORK.DEMO2 NOWD HEADSKIP SPLIT='/';
BY SCHOOLCODE SCHOOL;
TITLE "PLUS Preapproval Results by School";
TITLE2 '#BYVAL2 - #BYVAL1';
FOOTNOTE "JOB = UTLWG27     REPORT = ULWG27.LWG27R&RPNO";
COLUMN CREDITCHECKRESULT DATECREDITCHECKEDPERFORMED LENDERCODE BORROWERSSN
		BORROWERNAME REQ STUDENTSSN STUDENTNAME;
DEFINE CREDITCHECKRESULT / DISPLAY "CC Result" WIDTH = 6;
DEFINE DATECREDITCHECKEDPERFORMED / DISPLAY "Date of CC" WIDTH = 12;
DEFINE BORROWERSSN / DISPLAY "Borrower SSN" WIDTH = 11;
DEFINE BORROWERNAME / DISPLAY "Borrower Name" WIDTH = 20;
DEFINE REQ / DISPLAY "Req Ln Amt" WIDTH = 15;
DEFINE STUDENTSSN / DISPLAY "Student SSN" WIDTH = 11;
DEFINE STUDENTNAME / DISPLAY "Student Name" WIDTH = 20;
DEFINE LENDERCODE / DISPLAY "Lender ID" WIDTH = 7;
RUN;

%MEND PRINTALL;
%MACRO PRINTLENDER(RPNO=,LNDR=);
PROC PRINTTO PRINT=REPORT&RPNO NEW;
RUN;


DATA LENDERDATA;
SET WORK.DEMO2;
WHERE LENDERCODE = "&LNDR";
BY SCHOOLCODE SCHOOL;
TYPESORT = BORROWERSSN;
RUN;


OPTIONS PAGENO=1 ORIENTATION=LANDSCAPE NOBYLINE;
OPTIONS PS=39 LS=127;


PROC CONTENTS DATA=LENDERDATA OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
   PUT // 126*'-';
   PUT      ////////
       @53 '**** NO RECORD FOUND ****';
   PUT ////////
       @56 '-- END OF REPORT --';
   PUT /////
   		@44 "JOB = UTLWG27     REPORT = ULWG27.LWG27R&RPNO";
   END;
RETURN;
TITLE "PLUS Preapproval Results for Bank One";
TITLE3 "Run Date : &EFFDATE";
run;

PROC REPORT DATA=LENDERDATA NOWD HEADSKIP SPLIT='/';
TITLE "PLUS Preapproval Results for Bank One";
FOOTNOTE "JOB = UTLWG27     REPORT = ULWG27.LWG27R&RPNO";
COLUMN CREDITCHECKRESULT DATECREDITCHECKEDPERFORMED SCHOOLCODE BORROWERSSN
		BORROWERNAME REQ STUDENTSSN STUDENTNAME;
DEFINE CREDITCHECKRESULT / DISPLAY "CC Result" WIDTH = 12;
DEFINE DATECREDITCHECKEDPERFORMED / DISPLAY "Date of CC" WIDTH = 12;
DEFINE SCHOOLCODE / "School ID" WIDTH = 8;
DEFINE BORROWERSSN / DISPLAY "Borrower SSN" WIDTH = 11;
DEFINE BORROWERNAME / DISPLAY "Borrower Name" WIDTH = 20;
DEFINE REQ / DISPLAY "Req Ln Amt" WIDTH = 15;
DEFINE STUDENTSSN / DISPLAY "Student SSN" WIDTH = 11;
DEFINE STUDENTNAME / DISPLAY "Student Name" WIDTH = 20;


RUN;
%MEND PRINTLENDER;

PROC SQL;
CREATE TABLE DEMO2_1 AS
SELECT DISTINCT CREDITCHECKRESULT
	,DATECREDITCHECKEDPERFORMED 
	,LENDERCODE 
	,BORROWERSSN
	,BORROWERNAME 
	,REQ 
	,STUDENTSSN 
	,STUDENTNAME 
	,SCHOOLCODE 
	,SCHOOL
FROM DEMO2;
QUIT;

PROC SORT DATA=WORK.DEMO2_1 nodupkey;
BY SCHOOLCODE BORROWERSSN;
RUN;

%PRINTALL(RPNO=99);	
%PRINTLENDER(RPNO=98,LNDR=819628);/*1068*/

PROC PRINTTO;
RUN;
%PRINTEM(SCH=02270801,SCHNAME=AMERICAN INSTITUTE OF MED & DENT,RPNO=3);
%PRINTEM(SCH=02178501,SCHNAME=EAGLE GATE COLLEGE DAVIS-WEBER,RPNO=4);	/*733*/
%PRINTEM(SCH=00160600,SCHNAME=BRIGHAM YOUNG UNIVERSITY-HAWAII,RPNO=5);
%PRINTEM(SCH=02531806,SCHNAME=Paul Mitchell The School - Sterling Heights,RPNO=6);
%PRINTEM(SCH=00162500,SCHNAME=BRIGHAM YOUNG UNIVERSITY-IDAHO,RPNO=7);
%PRINTEM(SCH=03490300,SCHNAME=UP Academy of Hair Design,RPNO=8);
%PRINTEM(SCH=00367000,SCHNAME=BRIGHAM YOUNG UNIVERSITY,RPNO=9);
%PRINTEM(SCH=00161700,SCHNAME=COLLEGE OF IDAHO,RPNO=10);
%PRINTEM(SCH=00367600,SCHNAME=COLLEGE OF EASTERN UTAH,RPNO=11);
%PRINTEM(SCH=02351700,SCHNAME=DONTA SCHOOL OF BEAUTY CULTURE,RPNO=12);
%PRINTEM(SCH=00367100,SCHNAME=DIXIE STATE COLLEGE OF UTAH,RPNO=13);
%PRINTEM(SCH=02149901,SCHNAME=PAUL MITCHELL THE SCHOOL - VIRGINIA,RPNO=14);
%PRINTEM(SCH=02178500,SCHNAME=EAGLE GATE COLLEGE,RPNO=15);
%PRINTEM(SCH=02179935,SCHNAME=ARGOSY UNIVERSITY,RPNO=16);
%PRINTEM(SCH=00367200,SCHNAME=LATTER DAY SAINTS BUSINESS COLLEGE,RPNO=17);
%PRINTEM(SCH=02360802,SCHNAME=PROVO COLLEGE - AMERICAN FORK,RPNO=18);
%PRINTEM(SCH=02298500,SCHNAME=MOUNTAIN WEST COLLEGE,RPNO=19);
%PRINTEM(SCH=01179900,SCHNAME=PAUL MITCHELL THE SCHOOL - FAYETTEVILLE,RPNO=20);
%PRINTEM(SCH=03082100,SCHNAME=MYOTHERAPY COLLEGE OF UTAH,RPNO=21);
%PRINTEM(SCH=01116602,SCHNAME=UTAH CAREER COLLEGE � OREM ,RPNO=22);
%PRINTEM(SCH=01009800,SCHNAME=NEUMONT UNIVERSITY,RPNO=23);
%PRINTEM(SCH=04145500,SCHNAME=SKIN SCIENCE INSTITUTE,RPNO=24);
%PRINTEM(SCH=02531801,SCHNAME=Paul Mitchell The School - CA,RPNO=25);
%PRINTEM(SCH=02556801,SCHNAME=Paul Mitchell The School - St George,RPNO=26);
%PRINTEM(SCH=02531800,SCHNAME=Paul Mitchell The School - UT,RPNO=27);
%PRINTEM(SCH=03490301,SCHNAME=Paul Mitchell The School - Chicago,RPNO=28);
%PRINTEM(SCH=02531802,SCHNAME=Paul Mitchell The School - RI,RPNO=29);
%PRINTEM(SCH=02360800,SCHNAME=PROVO COLLEGE,RPNO=31); 
%PRINTEM(SCH=02612200,SCHNAME=MARINELO SCHOOL OF BEAUTY,RPNO=32); 
%PRINTEM(SCH=00522000,SCHNAME=SALT LAKE COMMUNITY COLLEGE,RPNO=33);
%PRINTEM(SCH=00367404,SCHNAME=STEVENS HENAGER COLLEGE-BOUNTIFUL,RPNO=35);
%PRINTEM(SCH=00367405,SCHNAME=STEVENS HENAGER COLLEGE PROVIDENCE,RPNO=37);
%PRINTEM(SCH=00367403,SCHNAME=STEVENS-HENAGER COLLEGE OF BUSINESS,RPNO=39);
%PRINTEM(SCH=00367400,SCHNAME=STEVENS HENAGER COLLEGE OGDEN,RPNO=41);
%PRINTEM(SCH=00367401,SCHNAME=STEVENS-HENAGER COLLEGE PROVO,RPNO=43);
%PRINTEM(SCH=03829500,SCHNAME=SKINWORKS SCHOOL OF ADVANCED SKINCARE,RPNO=45);
%PRINTEM(SCH=00367900,SCHNAME=SNOW COLLEGE,RPNO=47);
%PRINTEM(SCH=00367800,SCHNAME=SOUTHERN UTAH UNIVERSITY,RPNO=49); 
%PRINTEM(SCH=03030606,SCHNAME=UTAH COLLEGE OF MASSAGE THERAPY DENVER,RPNO=51);
%PRINTEM(SCH=03030607,SCHNAME=UTAH COLLEGE OF MASSAGE THERAPY AURORA,RPNO=52); /*724*/
%PRINTEM(SCH=03030602,SCHNAME=UTAH COLLEGE OF MASSAGE THERAPY,RPNO=53);
%PRINTEM(SCH=03030605,SCHNAME=UTAH COLLEGE OF MASSAGE THERAPY-PHOENIX,RPNO=55);
%PRINTEM(SCH=03030600,SCHNAME=UTAH SCHOOL OF MASSAGE THERAPY,RPNO=57); 
%PRINTEM(SCH=03030604,SCHNAME=UTAH COLLEGE OF MASSAGE THERAPY-TEMPE,RPNO=59);
%PRINTEM(SCH=03030601,SCHNAME=UTAH COL OF MASSAGE THER--PROVO/ORE,RPNO=61);
%PRINTEM(SCH=03030603,SCHNAME=UTAH COLLEGE OF MASSAGE THERAPY-NV,RPNO=63); 
%PRINTEM(SCH=00367500,SCHNAME=UNIVERSITY OF UTAH,RPNO=65);
%PRINTEM(SCH=00367700,SCHNAME=UTAH STATE UNIVERSITY,RPNO=67);
%PRINTEM(SCH=00402700,SCHNAME=UTAH VALLEY STATE COLLEGE,RPNO=69); 
%PRINTEM(SCH=02573100,SCHNAME=EVANS HAIRSTYLING COLLEGE CEDAR CIT,RPNO=71);
%PRINTEM(SCH=00368000,SCHNAME=WEBER STATE UNIVERSITY,RPNO=73);
%PRINTEM(SCH=00368100,SCHNAME=WESTMINSTER COLLEGE,RPNO=75);
%PRINTEM(SCH=02270800,SCHNAME=AMERICAN INST OF MED & DENTAL,RPNO=77);
%PRINTEM(SCH=03838300,SCHNAME=OGDEN INSTITUTE OF MASSAGE THERAPY,RPNO=78);
%PRINTEM(SCH=02178502,SCHNAME=EAGLE GATE COLLEGE - SLC,RPNO=79);
%PRINTEM(SCH=02531804,SCHNAME=PAUL MITCHELL THE SCHOOL - SAN DIEGO CALIFORNIA,RPNO=80);
%PRINTEM(SCH=02531803,SCHNAME=PAUL MITCHELL THE SCHOOL - ORLANDO,RPNO=81);
%PRINTEM(SCH=03909300,SCHNAME=MAXIMUM STYLE TECHNOLOGY,RPNO=82);
%PRINTEM(SCH=02556800,SCHNAME=PAUL MITCHELL THE SCHOOL - SLC,RPNO=84);
%PRINTEM(SCH=02270802,SCHNAME=AMERICAN INST OF MED & DENTAL - DRAPER,RPNO=85);
%PRINTEM(SCH=00161900,SCHNAME=COLLEGE OF SOUTHERN IDAHO,RPNO=86);
%PRINTEM(SCH=02531805,SCHNAME=PAUL MITCHELL THE SCHOOL - TAMPA,RPNO=87);
%PRINTEM(SCH=02153100,SCHNAME=PAUL MITCHELL THE SCHOOL - NASHVILLE,RPNO=88);
%PRINTEM(SCH=02149900,SCHNAME=PAUL MITCHELL THE SCHOOL - JACKSONVILLE,RPNO=89);
%PRINTEM(SCH=01116600,SCHNAME=UTAH CAREER COLLEGE,RPNO=90);
%PRINTEM(SCH=01116601,SCHNAME=UTAH CAREER COLLEGE - LAYTON,RPNO=91);
%PRINTEM(SCH=03463300,SCHNAME=CAREERS UNLIMITED,RPNO=92);
%PRINTEM(SCH=02357700,SCHNAME=PAUL MITCHELL THE SCHOOL - HOUSTON,RPNO=93);
%PRINTEM(SCH=03598300,SCHNAME=PAUL MITCHELL THE SCHOOL - SANTA BARBARA,RPNO=94);
%PRINTEM(SCH=03907300,SCHNAME=GREAT LAKES ACADEMY OF HAIR DESIGN,RPNO=95);
%PRINTEM(SCH=04072300,SCHNAME=Dallas Roberts Academy of Hair Design and Aesthetics,RPNO=96);
%PRINTEM(SCH=03084602,SCHNAME=The Art Institute of Salt Lake City,RPNO=97);
OPTIONS SYMBOLGEN NOCENTER NODATE NONUMBER LS=132;
DATA _NULL_;
SET TB76;
FILE REPORT76 DELIMITER=',' DSD DROPOVER LRECL=32767;
PUT TS;
RUN;

PROC PRINTTO;
RUN;


/*PROC EXPORT DATA=DEMO2*/
/*            OUTFILE= "T:\SAS\UTLWG27_Detail.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/