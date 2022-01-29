/*GO PLUS Apps not yet guaranteed for Weber and Dixie*/

libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;
OPTIONS NOCENTER NODATE NONUMBER LS=132;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE PROVAPP AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT
		integer(A.df_prs_id_br)			as SSN
		,b.AF_APL_ID||b.AF_APL_ID_SFX	as CLUID
		,RTRIM(D.DM_PRS_LST)||', '||RTRIM(D.DM_PRS_1)||' '||RTRIM(D.DM_PRS_MID)
										AS NAME
		,b.AC_PRC_STA
		,b.AD_PRC
		,a.AF_APL_OPS_SCL
		,C.IM_IST_FUL
		,E.AC_CNL_PRC_TYP
FROM  OLWHRM1.GA01_APP A inner join OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
inner join 
	(SELECT DISTINCT 
	IF_IST
	,IM_IST_FUL
	FROM OLWHRM1.SC01_LGS_SCL_INF) C
	ON a.AF_APL_OPS_SCL = C.IF_IST
inner join OLWHRM1.PD01_PDM_INF D
	on A.df_prs_id_br = D.DF_PRS_ID
INNER JOIN OLWHRM1.GA20_CNL_DAT E
	ON A.AF_APL_ID = E.AF_APL_ID
WHERE b.AC_PRC_STA in ('P','H','M','S')
AND A.AC_APL_TYP = 'P'
AND A.AF_APL_OPS_SCL IN ('00367100','00368000')
AND E.AC_CNL_PRC_TYP = 'GO'
);
DISCONNECT FROM DB2;

PROC SORT DATA = PROVAPP;
BY IM_IST_FUL AD_PRC SSN CLUID;
RUN;

endrsubmit  ;
DATA PROVAPP; 
SET WORKLOCL.PROVAPP; 
IF AC_PRC_STA = 'H'	THEN PRIN_STA = 'APPLICATION HOLD';
ELSE IF AC_PRC_STA = 'P'	THEN PRIN_STA = 'PROVISIONAL';
ELSE IF AC_PRC_STA = 'M'	THEN PRIN_STA = 'ACCOUNT HOLD';
ELSE IF AC_PRC_STA = 'S'	THEN PRIN_STA = 'STAFF REVIEW';
RUN;

DATA _NULL_;
CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'beginning'), MMDDYY10.));
RUN;

OPTIONS CENTER NODATE NUMBER PAGENO=1 LS=126;
PROC REPORT DATA=PROVAPP NOWD /*SPACING=1*/ HEADSKIP SPLIT='/';
TITLE 'WEBER AND DIXIE PLUS LOANS';
TITLE2 'GO - NOT GUARANTEED';
TITLE3 "RUN DATE: &RUNDATE";
FOOTNOTE  'JOB = QUERY 		REPORT = GO PLUS APPS, WEBER & DIXIE';
COLUMN IM_IST_FUL SSN NAME CLUID PRIN_STA AD_PRC AC_CNL_PRC_TYP N;
DEFINE IM_IST_FUL / GROUP NOPRINT;
DEFINE SSN / DISPLAY LEFT FORMAT=SSN11. "BORROWER/SSN";
DEFINE NAME / DISPLAY WIDTH=30 "BORROWER/NAME";
DEFINE CLUID / DISPLAY "COMMONLINE/UNIQUE ID";
DEFINE PRIN_STA / DISPLAY "APP/STATUS";
DEFINE AD_PRC / DISPLAY FORMAT=MMDDYY10. "DATE PROCESSED";
DEFINE AC_CNL_PRC_TYP / DISPLAY "PROC/TYPE/CODE" WIDTH=4;
DEFINE N / NOPRINT;
COMPUTE BEFORE IM_IST_FUL;
	LINE " ";
	LINE "SCHOOL:   " IM_IST_FUL $40.;
	LINE " ";
ENDCOMP;
COMPUTE AFTER IM_IST_FUL;
	LINE " ";
	LINE "OUTSTANDING LOAN APPLICATIONS FOR ";
	LINE IM_IST_FUL $30. ":  " N COMMA7.;
	LINE " ";
ENDCOMP;
COMPUTE AFTER;
	LINE " ";
	LINE "TOTAL OUTSTANDING LOAN APPLICATIONS:  " N COMMA7.;
	LINE " ";
ENDCOMP;
RUN;
