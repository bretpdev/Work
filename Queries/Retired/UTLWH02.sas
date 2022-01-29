*---------------------------------------------------------*
|UTLWH02 PRE-CERTIFIED LOANS NEEDING SCHOOL CERTIFICATION |
*---------------------------------------------------------*;
LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
/*%LET RPTLIB = T:\SAS;*/
FILENAME REPORTZ "&RPTLIB/ULWH02.LWH02RZ";
FILENAME REPORT2 "&RPTLIB/ULWH02.LWH02R2";
FILENAME REPORT3 "&RPTLIB/ULWH02.LWH02R3";
FILENAME REPORT4 "&RPTLIB/ULWH02.LWH02R4";
FILENAME REPORT5 "&RPTLIB/ULWH02.LWH02R5";
FILENAME REPORT6 "&RPTLIB/ULWH02.LWH02R6";
FILENAME REPORT7 "&RPTLIB/ULWH02.LWH02R7";
FILENAME REPORT8 "&RPTLIB/ULWH02.LWH02R8";
FILENAME REPORT9 "&RPTLIB/ULWH02.LWH02R9";
FILENAME REPORT10 "&RPTLIB/ULWH02.LWH02R10";
FILENAME REPORT11 "&RPTLIB/ULWH02.LWH02R11";
FILENAME REPORT12 "&RPTLIB/ULWH02.LWH02R12";
FILENAME REPORT13 "&RPTLIB/ULWH02.LWH02R13";
FILENAME REPORT14 "&RPTLIB/ULWH02.LWH02R14";
FILENAME REPORT15 "&RPTLIB/ULWH02.LWH02R15";
FILENAME REPORT16 "&RPTLIB/ULWH02.LWH02R16";
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
CREATE TABLE PCLNSC AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT B.AC_LON_TYP
	,A.AD_APL_CRT
	,A.AX_BR_REQ_IAA AS CAX_BR_REQ_IAA
	,CASE 
		WHEN A.AC_BR_CTZ = 'A' THEN 'CTZ'
		WHEN A.AC_BR_CTZ = 'B' THEN 'ELG NON CTZ'
		WHEN A.AC_BR_CTZ = 'C' THEN 'INELLIG'
	END AS AC_BR_CTZ
	,A.AF_CUR_APL_OPS_LDR
	,A.AC_CRD_CHK_PRF
/*BORROWER INFO */
	,A.DF_PRS_ID_BR
	,CASE 
		WHEN C.DM_PRS_MID = ''
		THEN RTRIM(C.DM_PRS_LST)||','||RTRIM(C.DM_PRS_1)
		ELSE RTRIM(C.DM_PRS_LST)||','||RTRIM(C.DM_PRS_1)||','||C.DM_PRS_MID 
	 END AS BNAME
	,C.DD_BRT AS BDATE
/*STUDENT INFO*/
	,A.DF_PRS_ID_STU
	,CASE 
		WHEN D.DM_PRS_MID = '' 
		THEN RTRIM(D.DM_PRS_LST)||','||RTRIM(D.DM_PRS_1)
		ELSE RTRIM(D.DM_PRS_LST)||','||RTRIM(D.DM_PRS_1)||','||D.DM_PRS_MID 
	 END AS SNAME
	,D.DD_BRT AS SDATE
	,E.IM_IST_FUL
	,CASE 
		WHEN E.IF_IST IN ('01009801','01009800') THEN '01009800 - 01009801'
		ELSE E.IF_IST
	 END AS REPNAME
	,CASE 
		WHEN E.IF_IST IN ('01009801','01009800') THEN '01009800'
		ELSE E.IF_IST
	 END AS IF_IST

FROM OLWHRM1.GA01_APP A
INNER JOIN OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
INNER JOIN OLWHRM1.PD01_PDM_INF C
	ON A.DF_PRS_ID_BR = C.DF_PRS_ID
INNER JOIN OLWHRM1.PD01_PDM_INF D
	ON A.DF_PRS_ID_STU = D.DF_PRS_ID
INNER JOIN (
	SELECT DISTINCT IF_IST
		,IM_IST_FUL
	FROM OLWHRM1.SC01_LGS_SCL_INF 
	WHERE IC_WEB_APL_PRT_OPT = ''
	) E
	ON A.AF_APL_OPS_SCL = E.IF_IST

WHERE B.AC_PRC_STA NOT IN ('A','R','X')
AND A.AD_BR_SIG IS NOT NULL
AND A.AD_SCL_SIG IS NULL
AND (
	B.AC_LON_TYP IN ('SF','SU')
	OR ( 
		B.AC_LON_TYP IN ('PL','GB')
		AND A.AC_CRD_CHK_PRF IN ('Y',' ')
		)	
	)
AND (
	B.AC_APL_SPS_REA_1 != 'A'
	AND B.AC_APL_SPS_REA_2 != 'A'
	AND B.AC_APL_SPS_REA_3 != 'A'
	AND B.AC_APL_SPS_REA_4 != 'A'
	AND B.AC_APL_SPS_REA_5 != 'A'
	)
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;
%SQLCHECK (SQLRPT=ULWH02.LWH02RZ);
QUIT;
/*ENDRSUBMIT;*/
/*DATA PCLNSC;*/
/*SET WORKLOCL.PCLNSC;*/
/*RUN;*/
DATA PCLNSC;
SET PCLNSC;
AX_BR_REQ_IAA = INT(CAX_BR_REQ_IAA);
RUN;

%MACRO LNSC_REP(SCL_ID,REPNO);

%LET SCL_NM = ;

DATA OB_TEST;
SET PCLNSC;
WHERE IF_IST = "&SCL_ID";
RUN;

%LET DSID = %SYSFUNC(OPEN(OB_TEST));
%LET HASOBS=%SYSFUNC(ATTRN(&DSID,ANY));
%LET RC=%SYSFUNC(CLOSE(&DSID));
%IF &HASOBS = 1 %THEN %DO;
	DATA _NULL_;
	SET OB_TEST;
	CALL SYMPUT('SCL_NM',TRIM(IM_IST_FUL));
	CALL SYMPUT('SCL_REP_ID',TRIM(REPNAME));
	RUN;
%END;
%ELSE %DO;
	%LET SCL_NM = ;
	%LET SCL_REP_ID = &SCL_ID;
%END;

DATA STAF PLUS;
SET OB_TEST;
IF AC_LON_TYP IN ('PL','GB') THEN OUTPUT PLUS;
ELSE IF AC_LON_TYP IN ('SF','SU') THEN OUTPUT STAF;
RUN;

PROC PRINTTO PRINT=REPORT&REPNO NEW;
RUN;
/*-----------------------------------------------------*
|                    STAFFORD REPORT                   |
*-----------------------------------------------------*/
OPTIONS ORIENTATION = LANDSCAPE PAGENO=1 NODATE CENTER;
OPTIONS PS=39 LS=133;
TITLE 'LOANS NEEDING SCHOOL CERTIFICATION - STAFFORD LOANS';
TITLE2 "SCHOOL ID: &SCL_REP_ID";
TITLE3	"&RUNDATE - &RUNTIME";
PROC CONTENTS DATA=STAF OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 131*'-';
	PUT      //////
		@51 '**** NO OBSERVATIONS FOUND ****';
	PUT //////
		@57 '-- END OF REPORT --';
	PUT /////////////
		@46 "JOB = UTLWH02  	 REPORT = ULWH02.LWH02R&REPNO";
	END;
RETURN;
RUN;

OPTIONS ORIENTATION = LANDSCAPE PAGENO=1 NODATE CENTER;
OPTIONS PS=39 LS=133;
TITLE 'LOANS NEEDING SCHOOL CERTIFICATION - STAFFORD LOANS';
TITLE2 "&SCL_NM: &SCL_REP_ID  ";
TITLE3	"&RUNDATE - &RUNTIME";
FOOTNOTE "JOB = UTLWH02  	 REPORT = ULWH02.LWH02R&REPNO";
PROC PRINT NOOBS SPLIT='/' DATA=STAF WIDTH=UNIFORM WIDTH=MIN;
FORMAT AD_APL_CRT BDATE MMDDYY10. AX_BR_REQ_IAA COMMA10.;
VAR DF_PRS_ID_BR
	BNAME
	BDATE
	AD_APL_CRT
	AX_BR_REQ_IAA
	AF_CUR_APL_OPS_LDR;
LABEL DF_PRS_ID_BR = 'SSN'
BNAME = 'NAME'
BDATE = 'DOB' 
AD_APL_CRT = 'APP CREATE DATE'
AX_BR_REQ_IAA = 'BORR REQ AMT'
AF_CUR_APL_OPS_LDR = 'LENDER' 
;
RUN;
/*-----------------------------------------------------*
|                    PLUS REPORT                       |
*-----------------------------------------------------*/
TITLE 'LOANS NEEDING SCHOOL CERTIFICATION - PLUS LOANS';
TITLE2 "SCHOOL ID: &SCL_REP_ID";
TITLE3	"&RUNDATE - &RUNTIME";
PROC CONTENTS DATA=PLUS OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 131*'-';
	PUT      //////
		@51 '**** NO OBSERVATIONS FOUND ****';
	PUT //////
		@57 '-- END OF REPORT --';
	PUT /////////////
		@46 "JOB = UTLWH02  	 REPORT = ULWH02.LWH02R&REPNO";
	END;
RETURN;
RUN;

TITLE 'LOANS NEEDING SCHOOL CERTIFICATION - PLUS LOANS';
TITLE2 "&SCL_NM: &SCL_REP_ID  ";
TITLE3	"&RUNDATE - &RUNTIME";
PROC REPORT NOWD DATA=PLUS SPACING=1 SPLIT='/' HEADSKIP;
COLUMN DF_PRS_ID_BR	BNAME BDATE	AD_APL_CRT AX_BR_REQ_IAA AF_CUR_APL_OPS_LDR AC_BR_CTZ
	DF_PRS_ID_STU SNAME	SDATE;
DEFINE DF_PRS_ID_BR / DISPLAY "SSN" FORMAT=$9. WIDTH=9;
DEFINE BNAME / DISPLAY "NAME" FORMAT=$30. WIDTH=25 ;
DEFINE BDATE / DISPLAY "DOB" FORMAT=MMDDYY8. WIDTH=8;
DEFINE AD_APL_CRT / DISPLAY "APP CREATE DATE" FORMAT=MMDDYY8. WIDTH=8;
DEFINE AX_BR_REQ_IAA / DISPLAY "BORR REQ AMT" FORMAT=COMMA10. WIDTH=12;
DEFINE AF_CUR_APL_OPS_LDR / DISPLAY "LENDER" FORMAT=$8. WIDTH=8;
DEFINE AC_BR_CTZ /DISPLAY "CITIZENSHIP" FORMAT=$11.WIDTH=11;
DEFINE DF_PRS_ID_STU / DISPLAY "STUDENT SSN" FORMAT=$9. WIDTH=9;
DEFINE SNAME / DISPLAY "STUDENT NAME" FORMAT=$30. WIDTH=25;
DEFINE SDATE / DISPLAY "STUDENT DOB" FORMAT=MMDDYY8. WIDTH=8;
RUN;
PROC PRINTTO;
RUN;
%MEND LNSC_REP;

%LNSC_REP(01009800,2);
%LNSC_REP(00367400,3);
%LNSC_REP(00367401,4);
%LNSC_REP(00367403,5);
%LNSC_REP(00367404,6);
%LNSC_REP(00367405,7);
%LNSC_REP(00161900,8);
%LNSC_REP(02270800,9);
%LNSC_REP(02270802,10);
%LNSC_REP(00161700,11);
%LNSC_REP(04072300,12);
%LNSC_REP(00368100,13);
%LNSC_REP(01116601,14);
%LNSC_REP(01116600,15);
%LNSC_REP(01116602,16);
