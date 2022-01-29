/*UTLWQ13 - DAILY FOREIGN SCHOOL ANTICIPATED DISBURSEMENTS*/

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
/*FILENAME REPORT2 "&RPTLIB/ULWQ13.LWQ13R2";*/
/*FILENAME REPORTZ "&RPTLIB/ULWQ13.LWQ13RZ";*/

/*CALCULATE HOW MANY DAYS AHEAD OF THE CURRENT DATE TO INCLUDE IN THE REPORT*/
DATA _NULL_;
N = WEEKDAY(DATE());
IF N = 1 THEN CALL SYMPUT('END',"'"||PUT(INTNX('DAY',TODAY(),+4), MMDDYYD10.)||"'");
ELSE IF N = 2 THEN CALL SYMPUT('END',"'"||PUT(INTNX('DAY',TODAY(),+4), MMDDYYD10.)||"'");
ELSE IF N = 3 THEN CALL SYMPUT('END',"'"||PUT(INTNX('DAY',TODAY(),+3), MMDDYYD10.)||"'");
ELSE IF N = 4 THEN CALL SYMPUT('END',"'"||PUT(INTNX('DAY',TODAY(),+5), MMDDYYD10.)||"'");
ELSE IF N = 5 THEN CALL SYMPUT('END',"'"||PUT(INTNX('DAY',TODAY(),+5), MMDDYYD10.)||"'");
ELSE IF N = 6 THEN CALL SYMPUT('END',"'"||PUT(INTNX('DAY',TODAY(),+5), MMDDYYD10.)||"'");
ELSE IF N = 7 THEN CALL SYMPUT('END',"'"||PUT(INTNX('DAY',TODAY(),+6), MMDDYYD10.)||"'");
RUN;

%SYSLPUT END = &END;
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWQ13.LWQ13R2";
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

PROC SQL STIMER;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE FCANTD AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT INTEGER(A.BF_SSN) AS SSN
	,PD10.DF_SPE_ACC_ID AS ACCTNUM
	,STU_PD10.DF_SPE_ACC_ID AS STU_ACCTNUM
	,RTRIM(PD10.DM_PRS_LST)||', '||RTRIM(PD10.DM_PRS_1)||' '||(PD10.DM_PRS_MID) AS NAME
	,PD30.DX_STR_ADR_1
	,PD30.DX_STR_ADR_2
	,PD30.DX_STR_ADR_3
	,PD30.DM_CT
	,PD30.DC_DOM_ST
	,PD30.DF_ZIP_CDE
	,PD30.DM_FGN_CNY
	,PD30.DM_FGN_ST
	,PD30.DI_VLD_ADR

	,PD42.DN_DOM_PHN_ARA
	,PD42.DN_DOM_PHN_XCH
	,PD42.DN_DOM_PHN_LCL
	,PD42.DN_FGN_PHN_INL
	,PD42.DN_FGN_PHN_CNY
	,PD42.DN_FGN_PHN_CT
	,PD42.DN_FGN_PHN_LCL
	,PD42.DI_PHN_VLD

	,PD32.DX_ADR_EML
	,PD10.DC_SEX

	,INTEGER(B.LF_STU_SSN) AS STU_SSN
	,RTRIM(STU_PD10.DM_PRS_LST)||', '||RTRIM(STU_PD10.DM_PRS_1)||' '||
		(STU_PD10.DM_PRS_MID) AS STU_NAME

	,B.IC_LON_PGM
	,A.LD_DSB
	,A.AN_SEQ
	,A.LN_LON_DSB_SEQ
	,A.LA_DSB - COALESCE(A.LA_DSB_CAN,0) AS DSB_AMT
	,B.AF_CNL||B.AF_CNL_SFX as CLUID
	,A.LC_CMN_LN_HLD_RLS

	,B.AF_DOE_SCL
	,SC10.IM_SCL_FUL
	,SC25.IX_SCL_STR_ADR_1
	,SC25.IX_SCL_STR_ADR_2
	,SC25.IX_SCL_STR_ADR_3
	,SC25.IM_SCL_CT
	,SC25.IC_SCL_DOM_ST
	,SC25.IM_SCL_FGN_ST
	,SC25.IM_SCL_FGN_CNY
	,SC25.IF_SCL_ZIP_CDE
	,SC25.II_SCL_VLD_ADR
	,SC20.II_COP
	,SC26.IC_DSB_MDM	
	,B.AF_DOE_LDR
	,LR10.IM_LDR_FUL

FROM OLWHRM1.LN15_DSB A
INNER JOIN OLWHRM1.AP03_MASTER_APL B
	ON A.BF_SSN = B.BF_SSN
	AND A.AN_SEQ = B.AN_SEQ 
INNER JOIN OLWHRM1.SC10_SCH_DMO SC10
	ON B.AF_DOE_SCL = SC10.IF_DOE_SCL
INNER JOIN OLWHRM1.SC20_SCH_GTR SC20
	ON B.AF_DOE_SCL = SC20.IF_DOE_SCL
	AND B.IC_LON_PGM = SC20.IC_LON_PGM
	AND B.IF_GTR = SC20.IF_GTR
INNER JOIN OLWHRM1.SC25_SCH_DPT SC25
	ON B.AF_DOE_SCL = SC25.IF_DOE_SCL
	AND SC25.IC_SCL_DPT = '000'
INNER JOIN OLWHRM1.PD10_PRS_NME PD10 
	ON A.BF_SSN = PD10.DF_PRS_ID
INNER JOIN OLWHRM1.PD10_PRS_NME STU_PD10 
	ON B.LF_STU_SSN = STU_PD10.DF_PRS_ID
INNER JOIN OLWHRM1.PD30_PRS_ADR PD30
	ON A.BF_SSN = PD30.DF_PRS_ID
	AND PD30.DC_ADR = 'L'
LEFT OUTER JOIN OLWHRM1.PD32_PRS_ADR_EML PD32
	ON A.BF_SSN = PD32.DF_PRS_ID
	AND PD32.DC_ADR_EML = 'H'
	AND PD32.DC_STA_PD32 = 'A'
INNER JOIN OLWHRM1.PD42_PRS_PHN PD42
	ON A.BF_SSN = PD42.DF_PRS_ID
	AND PD42.DC_PHN = 'H'
LEFT OUTER JOIN OLWHRM1.SC26_DSB_SCH SC26
	ON B.AF_DOE_SCL = SC26.IF_DOE_SCL
INNER JOIN OLWHRM1.LR10_LDR_DMO LR10
	ON B.AF_DOE_LDR = LR10.IF_DOE_LDR
INNER JOIN OLWHRM1.SC22_SCH_STA_TRK SC22
	ON B.AF_DOE_SCL = SC22.IF_DOE_SCL

WHERE SC22.IC_STA_SCL_DOE = 'I' 
AND A.LC_DSB_TYP = '1'
AND A.LC_STA_LON15 IN ('1', '3')
AND (A.LA_DSB_CAN IS NULL
	OR A.LA_DSB <> A.LA_DSB_CAN)
AND A.LD_DSB <= &END
AND NOT EXISTS
		(SELECT *
		FROM OLWHRM1.AY10_BR_LON_ATY X
		WHERE X.BF_SSN = A.BF_SSN
		AND X.AN_SEQ = A.AN_SEQ
		AND X.LC_STA_ACTY10 = 'A'
		AND X.PF_REQ_ACT = 'LOFSA'
		AND DAYS(X.LD_ATY_REQ_RCV) BETWEEN (DAYS(A.LD_DSB) - 7) AND DAYS(A.LD_DSB)
		)
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;

DATA FCANTD; 
SET WORKLOCL.FCANTD; 
RUN;

PROC SORT DATA=FCANTD;
BY SSN AN_SEQ LN_LON_DSB_SEQ;
RUN;

PROC FORMAT LIBRARY=WORK;
VALUE $EFT
'1' = 'EFT DATA & TRANSMISSION'
'2' = 'EFT DATA ONLY'
'3' = 'ELM - CDA'
'4' = 'MASTERCHECK';
QUIT;

/*SET CURRENT RUN DATE VARIABLE*/
DATA _NULL_;
CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'beginning'), MMDDYY10.));
RUN;

/*FORMAT SOME VALUES FOR PRINTING*/
DATA FCANTD;
SET FCANTD;
CTSTZP = TRIM(DM_CT)||", "||TRIM(DC_DOM_ST)||" "||TRIM(DF_ZIP_CDE);
SCL_CTSTZP = TRIM(IM_SCL_CT)||", "||TRIM(IC_SCL_DOM_ST)||" "||TRIM(IF_SCL_ZIP_CDE);
LENGTH FGN $20.;
IF DM_FGN_ST = ' ' AND DM_FGN_CNY = ' ' THEN FGN = ' ';
ELSE IF DM_FGN_ST NE ' ' AND DM_FGN_CNY = ' ' THEN FGN = TRIM(DM_FGN_ST);
ELSE IF DM_FGN_ST = ' ' AND DM_FGN_CNY NE ' ' THEN FGN = TRIM(DM_FGN_CNY);
ELSE IF DM_FGN_ST NE ' ' AND DM_FGN_CNY NE ' ' 
	THEN FGN = TRIM(DM_FGN_ST)||", "||TRIM(DM_FGN_CNY);
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;

OPTIONS ORIENTATION = PORTRAIT;
OPTIONS CENTER NODATE NONUMBER PAGENO=1 LS=96 PS=52;

TITLE 'COMPASS ANTICIPATED DISBURSEMENTS AT FOREIGN SCHOOLS';
TITLE2 "RUN DATE:  &RUNDATE";
FOOTNOTE  'JOB = UTLWQ13     REPORT = ULWQ13.LWQ13R2';

/*IF NO OUTPUT, PRINT THAT*/
PROC CONTENTS DATA=FCANTD OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
   PUT // 90*'-';
   PUT      ////////
       @31 '**** NO OBSERVATIONS FOUND ****';
   PUT ////////
       @37 '-- END OF REPORT --';
   PUT ////////////////////////////
   		@26 "JOB = UTLWQ13     REPORT = ULWQ13.LWQ13R2";
   END;
RETURN;
run;

DATA _NULL_;
SET FCANTD;
	FILE PRINT n=pagesize;
	TITLE;
	FOOTNOTE;
	BY SSN;
	/*BORROWER INFO*/
	IF FIRST.SSN THEN DO;

		PUT #1 @26 "COMPASS ANTICIPATED DISBURSEMENTS AT FOREIGN SCHOOLS"
			/@40 "RUN DATE:  &RUNDATE"
			/
			/
			/@1 "BORROWER INFORMATION:"
			/@1 "ACCT:" @45 ACCTNUM
			/@1 "NAME:" @45 NAME $25.
			/
			/@1 "BORROWER LEGAL ADDRESS  (VALID: " DI_VLD_ADR +(-1) "):"
			@45 DX_STR_ADR_1 $40.
			;
		IF DX_STR_ADR_2 NE " " THEN	PUT @45 DX_STR_ADR_2 $40.;
		IF DX_STR_ADR_3 NE " " THEN	PUT @45 DX_STR_ADR_3 $40.;
		PUT @45 CTSTZP $41.;
		IF FGN NE " " THEN PUT @45 FGN;
		PUT ;

		PUT @1 "BORROWER TELEPHONE NUMBER:" @;
		IF DN_DOM_PHN_ARA NE " " 
			THEN PUT @45 "HOME " DN_DOM_PHN_ARA +(-1) "-" DN_DOM_PHN_XCH +(-1) "-" DN_DOM_PHN_LCL;
		IF DN_FGN_PHN_INL NE " " THEN DO;
			PUT @45 "FOREIGN " DN_FGN_PHN_INL +(-1) "-" DN_FGN_PHN_CNY +(-1) "-" DN_FGN_PHN_CT @;
			IF DN_FGN_PHN_LCL NE " " 
				THEN PUT +(-1) "-" DN_FGN_PHN_LCL;
				ELSE PUT ' ';
			END;
		PUT ' ';

		END;

	/*DISBURSEMENT INFO*/

	PUT 90* '*'
		/
		;

	IF IC_LON_PGM = "PLUS" THEN DO;
		PUT	"PLUS LOAN STUDENT INFORMATION:"
			/"ACCT:" @45 STU_ACCTNUM
			/"NAME:" @45 STU_NAME $25.
			/
			;
		END;

	PUT "LOAN & DISBURSEMENT INFORMATION:"
		/"LOAN TYPE:  " @45 IC_LON_PGM $6.
		/"DISB DATE:  " @45 LD_DSB MMDDYY10.
		/"APP SEQ #:  " @42 AN_SEQ 4.
		/"DISB #:  " @43 LN_LON_DSB_SEQ 3.
		/"DISB AMOUNT:  "  @44 DSB_AMT DOLLAR10.2
		/"COMMONLINE UNIQUE ID:  " @45 CLUID $19.
		/"DISB HOLD/RELEASE INDICATOR:  "  @45 LC_CMN_LN_HLD_RLS $1.
		/
		;

	PUT "SCHOOL INFORMATION:"
		/"SCHOOL CODE:  " @45 AF_DOE_SCL $8.
		/"SCHOOL NAME:  " @45 IM_SCL_FUL $40.
		/"SCHOOL CO-PAYABLE INDICATOR ON COMPASS:  " @45 II_COP $1.
		/"SCHOOL DISBURSEMENT METHOD:  " @45 IC_DSB_MDM $EFT.
		/
		/"SCHOOL ADDRESS  (VALID: " II_SCL_VLD_ADR +(-1) "):"
		@45 IX_SCL_STR_ADR_1 $40.
		;
	IF IX_SCL_STR_ADR_2 NE " " THEN	PUT @45 IX_SCL_STR_ADR_2 $40.;
	IF IX_SCL_STR_ADR_3 NE " " THEN	PUT @45 IX_SCL_STR_ADR_3 $40.;
	PUT @45 SCL_CTSTZP $40.;
	IF IM_SCL_FGN_ST NE " " AND IM_SCL_FGN_ST NE "FC" THEN	PUT @45 IM_SCL_FGN_ST $40.;
    IF IM_SCL_FGN_CNY NE " " AND IM_SCL_FGN_CNY NE "FC" THEN	PUT @45 IM_SCL_FGN_CNY $40.
		/;

	PUT "LENDER INFORMATION:"
		/"LENDER CODE:  " @45 AF_DOE_LDR $8.
		/"LENDER NAME:  " @45 IM_LDR_FUL $40.
		;

	PUT ' ';
	IF LAST.SSN THEN PUT _PAGE_;
RUN;

PROC PRINTTO;
RUN;
