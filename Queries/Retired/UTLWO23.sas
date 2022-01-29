/*CREDIT UNION MEMBERSHIP APPLICATION HOLDS*/

LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
/*%LET RPTLIB = T:\SAS;*/
FILENAME REPORTZ "&RPTLIB/ULWO23.LWO23RZ";
FILENAME REPORT2 "&RPTLIB/ULWO23.LWO23R2";
FILENAME REPORT3 "&RPTLIB/ULWO23.LWO23R3";
FILENAME REPORT5 "&RPTLIB/ULWO23.LWO23R5";
FILENAME REPORT6 "&RPTLIB/ULWO23.LWO23R6";
FILENAME REPORT7 "&RPTLIB/ULWO23.LWO23R7";
FILENAME REPORT9 "&RPTLIB/ULWO23.LWO23R9";
FILENAME REPORT10 "&RPTLIB/ULWO23.LWO23R10";
FILENAME REPORT11 "&RPTLIB/ULWO23.LWO23R11";
FILENAME REPORT12 "&RPTLIB/ULWO23.LWO23R12";
FILENAME REPORT13 "&RPTLIB/ULWO23.LWO23R13";
FILENAME REPORT14 "&RPTLIB/ULWO23.LWO23R14";
FILENAME REPORT15 "&RPTLIB/ULWO23.LWO23R15";
FILENAME REPORT16 "&RPTLIB/ULWO23.LWO23R16";
FILENAME REPORT17 "&RPTLIB/ULWO23.LWO23R17";
FILENAME REPORT18 "&RPTLIB/ULWO23.LWO23R18";


DATA _NULL_;
CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'beginning'), MMDDYY10.));
CALL SYMPUT('RUNTIME',PUT(TIME(), TIME.));
RUN;

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
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	'  *'																				AS NEW_REC,
	''																					AS SP_HLD,
	''																					AS LN_HLD,
	A.DF_PRS_ID 																		AS SSN,
	B.DF_PRS_ID_STU			 															AS SSN_STU,
	C.AF_APL_ID||C.AF_APL_ID_SFX 														AS APL_ID,
	RTRIM(A.DM_PRS_LST)||', '||RTRIM(A.DM_PRS_1)||' '||A.DM_PRS_MID 					AS DM_PRS_NAME,
	RTRIM(A.DX_STR_ADR_1)||' '||RTRIM(A.DX_STR_ADR_2) 									AS DM_PRS_ADD1,
	RTRIM(A.DM_CT)||' '||A.DC_DOM_ST||'  '||A.DF_ZIP 									AS DM_PRS_ADD2,
	'('||SUBSTR(A.DN_PHN,1,3)||') '||SUBSTR(A.DN_PHN,4,3)||'-'||SUBSTR(A.DN_PHN,7,4)	AS DN_PHN,
	C.AD_PRC,
	D.IF_IST,
	D.IM_IST_FUL

FROM
	OLWHRM1.PD01_PDM_INF A
	INNER JOIN OLWHRM1.GA01_APP B
	ON A.DF_PRS_ID = B.DF_PRS_ID_BR
	AND B.AF_CUR_APL_OPS_LDR IN 
		('817440', '817545', '820200', '822373', 
		 '829123', '830132', '830146', '830791',
		 '833828', '817575', '834122', '832241',
		 '833577', '817546', '834265')
	INNER JOIN OLWHRM1.GA10_LON_APP C
	ON B.AF_APL_ID = C.AF_APL_ID
	AND (C.AC_APL_SPS_REA_1 = 'A' 
		 OR C.AC_APL_SPS_REA_2 = 'A'
		 OR C.AC_APL_SPS_REA_3 = 'A'
		 OR C.AC_APL_SPS_REA_4 = 'A'
		 OR C.AC_APL_SPS_REA_5 = 'A')
	AND AC_PRC_STA NOT IN ('A','R')
	INNER JOIN OLWHRM1.LR01_LGS_LDR_INF D
	ON B.AF_CUR_APL_OPS_LDR = D.IF_IST

ORDER BY
	A.DF_PRS_ID,
	C.AF_APL_ID||C.AF_APL_ID_SFX
);
DISCONNECT FROM DB2;
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>;  *INCLUDES ERROR MESSAGES TO SAS LOG  ;
%SQLCHECK (SQLRPT=ULWO23.LWO23RZ);
QUIT;
/*ENDRSUBMIT;*/

/*DATA DEMO;*/
/*SET WORKLOCL.DEMO;*/
/*RUN;*/

DATA DEMO;
SET DEMO;
DF_PRS_ID = INT(SSN);
DF_PRS_ID_STU = INT(SSN_STU);
RUN;

/*DATA _NULL_;*/
/*CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'beginning'), MMDDYY10.));*/
/*FOOTNOTE ;*/
/*RUN;*/

OPTIONS NODATE NOBYLINE ORIENTATION=LANDSCAPE;
OPTIONS LS=127 PS=39;

%MACRO PRINT(LENDER=,REPNO=);

OPTIONS PAGENO=1;
OPTIONS NOCENTER;
PROC PRINTTO PRINT=REPORT&REPNO NEW;
RUN;

DATA DEMOPR;
	SET DEMO;
	WHERE IF_IST = "&LENDER";
RUN;

PROC CONTENTS DATA=DEMOPR OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
   OPTIONS CENTER;
   TITLE  "CREDIT UNION MEMBERSHIP APPLICATION HOLDS";
   TITLE2 "LENDER: &LENDER";
   TITLE3 "&RUNDATE - &RUNTIME";
   PUT // 127*'-';
   PUT      //////
       @49 '**** NO OBSERVATIONS FOUND ****';
   PUT //////
       @54 '-- END OF REPORT --';
   PUT /////////////
   		@43 "JOB = UTLWO23     REPORT = ULWO23.LWO23R&REPNO";
   END;
RETURN;
RUN;

PROC REPORT DATA=DEMOPR NOWD SPACING=2 HEADSKIP HEADLINE WRAP SPLIT='/' FORMCHAR(2)='-';
TITLE; 
TITLE2;
COLUMN
	IF_IST
	IM_IST_FUL
	NEW_REC
	DF_PRS_ID
	DM_PRS_NAME
	DM_PRS_ADD1
	AD_PRC
	SP_HLD
	DF_PRS_ID_STU
	APL_ID
	DM_PRS_ADD2
	DN_PHN
	LN_HLD;
DEFINE IF_IST / GROUP NOPRINT;
DEFINE IM_IST_FUL / GROUP NOPRINT;
DEFINE NEW_REC / DISPLAY RIGHT "*" WIDTH=3;
DEFINE DF_PRS_ID / DISPLAY LEFT "BORROWER SSN" FORMAT=SSN11. WIDTH=14;
DEFINE DM_PRS_NAME / DISPLAY LEFT "BORROWER NAME" WIDTH=36;
DEFINE DM_PRS_ADD1 / DISPLAY LEFT "ADDRESS" WIDTH=50;
DEFINE AD_PRC / DISPLAY LEFT "DATE PROCESSED" FORMAT=MMDDYY10. WIDTH=14;
DEFINE SP_HLD / DISPLAY "" WIDTH=3;
DEFINE DF_PRS_ID_STU / DISPLAY LEFT "STUDENT SSN" FORMAT=SSN11. WIDTH=14;
DEFINE APL_ID / DISPLAY LEFT "LOAN ID" WIDTH=36;
DEFINE DM_PRS_ADD2 / DISPLAY LEFT "CITY STATE ZIP" WIDTH=50;
DEFINE DN_PHN / DISPLAY LEFT "PHONE" WIDTH=14;
DEFINE LN_HLD / DISPLAY "" WIDTH=127;

COMPUTE BEFORE _PAGE_;	
	LINE "CREDIT UNION MEMBERSHIP APPLICATION HOLDS";
	LINE IM_IST_FUL $40.;
	LINE "LENDER: &LENDER";
	LINE "&RUNDATE - &RUNTIME";
	LINE ' ';
ENDCOMP;

COMPUTE AFTER _PAGE_;
	LINE "JOB = UTLWO23     REPORT = ULWO23.LWO23R&REPNO";
ENDCOMP;

BREAK AFTER IF_IST / OL SUMMARIZE PAGE;

RUN;

%MEND PRINT;

%PRINT(LENDER=817440,REPNO=2);
%PRINT(LENDER=817545,REPNO=3);
%PRINT(LENDER=820200,REPNO=5);
%PRINT(LENDER=829123,REPNO=7);
%PRINT(LENDER=830132,REPNO=9);
%PRINT(LENDER=830146,REPNO=10);
%PRINT(LENDER=830791,REPNO=11);
%PRINT(LENDER=833828,REPNO=12);
%PRINT(LENDER=817575,REPNO=13);
%PRINT(LENDER=834122,REPNO=14);
%PRINT(LENDER=832241,REPNO=15);
%PRINT(LENDER=833577,REPNO=16);
%PRINT(LENDER=817546,REPNO=17);
%PRINT(LENDER=834265,REPNO=18);


%MACRO CDILIM(LENDER=, REPNO=); 

DATA CMDLM;
SET DEMO;
BLANK = ' ';
WHERE IF_IST = "&LENDER";
RUN;

DATA _NULL_;
SET  WORK.CMDLM;
FILE REPORT&REPNO DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT NEW_REC $3. ;
FORMAT SP_HLD $1. ;
FORMAT LN_HLD $1. ;
FORMAT SSN 9. ;
FORMAT SSN_STU 9. ;
FORMAT APL_ID $19. ;
FORMAT DM_PRS_NAME $51. ;
FORMAT DM_PRS_ADD1 $71. ;
FORMAT DM_PRS_ADD2 $49. ;
FORMAT DN_PHN $14. ;
FORMAT AD_PRC MMDDYY10. ;
FORMAT IF_IST $8. ;
FORMAT IM_IST_FUL $40. ;
FORMAT BLANK $1. ;
DO;
PUT BLANK $ @;
PUT IF_IST $ @;
PUT SSN @;
PUT DM_PRS_NAME $ @;
PUT DM_PRS_ADD1 $ @;
PUT DM_PRS_ADD2 $ @;
PUT AD_PRC @;
PUT SSN_STU @;
PUT APL_ID $ @;
PUT DN_PHN $ ;
END;
RUN;
%MEND CDILIM;

%CDILIM(LENDER=822373,REPNO=6);

PROC PRINTTO;
RUN;
