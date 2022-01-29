/*UTLWM04 - SPECIAL CAMPAIGN - HIGH BALANCE BORROWERS*/
LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWM04.LWM04R2";
FILENAME REPORT3 "&RPTLIB/ULWM04.LWM04R3";
/*FILENAME REPORT2 "C:\WINDOWS\TEMP\ULWM04.LWM04R2";*/
/*FILENAME REPORT3 "C:\WINDOWS\TEMP\ULWM04.LWM04R3";*/
/*libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;*/
/*RSUBMIT;*/
PROC SQL STIMER;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE SCHB1A AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	A.DF_PRS_ID_BR
	,RTRIM(PD01.DM_PRS_1) AS DM_PRS_1
	,RTRIM(PD01.DM_PRS_LST) AS DM_PRS_LST
	,A.AF_APL_OPS_SCL
	,SC01.IM_IST_FUL
	,BRBAL.BWR_BAL
	,GA15.MAX_CLC_RPD
	,DATE(DAYS(GA15.MAX_CLC_RPD) + 30) AS APRX_1ST_DUE
	,A.DF_PRS_ID_STU
	,PD03.DI_VLD_ADR
	,PD03.DI_PHN_VLD
	,PD03.DM_FGN_CNY
	,PD03.DC_DOM_ST
	,'MA2328' AS COST_CENTER_CODE
FROM  OLWHRM1.GA01_APP A 
INNER JOIN OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
INNER JOIN OLWHRM1.GA14_LON_STA GA14
	ON B.AF_APL_ID = GA14.AF_APL_ID
	AND B.AF_APL_ID_SFX = GA14.AF_APL_ID_SFX
INNER JOIN
	(SELECT X.DF_PRS_ID_BR
	,SUM(Y.AA_GTE_LON_AMT - COALESCE(Y.AA_TOT_CAN,0) - Y.AA_TOT_RFD - Y.AA_TOT_AMT_PD) AS BWR_BAL
	FROM OLWHRM1.GA01_APP X
	INNER JOIN OLWHRM1.GA10_LON_APP Y
		ON X.AF_APL_ID = Y.AF_APL_ID
	INNER JOIN OLWHRM1.GA14_LON_STA GA14
		ON Y.AF_APL_ID = GA14.AF_APL_ID
		AND Y.AF_APL_ID_SFX = GA14.AF_APL_ID_SFX
	WHERE GA14.AC_STA_GA14 = 'A'
	AND GA14.AC_LON_STA_TYP IN ('CR','DA','FB','IA','ID','IG','IM','RP','UA','UB')
	GROUP BY X.DF_PRS_ID_BR
	HAVING SUM(Y.AA_GTE_LON_AMT - COALESCE(Y.AA_TOT_CAN,0) - Y.AA_TOT_RFD - Y.AA_TOT_AMT_PD) >= 25000
	)BRBAL
	ON A.DF_PRS_ID_BR = BRBAL.DF_PRS_ID_BR
INNER JOIN 
	(SELECT DF_PRS_ID_STU_NDS
	,MAX(AD_NDS_CLC_ENT_RPD) AS MAX_CLC_RPD
	FROM OLWHRM1.GA15_NDS_ID GA15
	INNER JOIN OLWHRM1.GA14_LON_STA GA14
		ON GA15.AF_APL_ID = GA14.AF_APL_ID
		AND GA15.AF_APL_ID_SFX = GA14.AF_APL_ID_SFX
		AND GA14.AC_LON_STA_TYP NOT IN ('CA','PF','PN','CP','DN','DP')
		AND GA15.AC_STA_GA15 = 'A'
	GROUP BY DF_PRS_ID_STU_NDS
	)GA15
	ON A.DF_PRS_ID_STU = GA15.DF_PRS_ID_STU_NDS
INNER JOIN OLWHRM1.PD01_PDM_INF PD01
	ON A.DF_PRS_ID_BR = PD01.DF_PRS_ID
INNER JOIN OLWHRM1.PD03_PRS_ADR_PHN PD03
	ON A.DF_PRS_ID_BR = PD03.DF_PRS_ID
	AND PD03.DC_ADR = 'L'
INNER JOIN
	(SELECT DISTINCT 
	IF_IST
	,IM_IST_FUL
	FROM OLWHRM1.SC01_LGS_SCL_INF
	)SC01
	ON A.AF_APL_OPS_SCL = SC01.IF_IST

WHERE GA14.AC_STA_GA14 = 'A'
AND B.AC_PRC_STA = 'A'
AND A.AF_APL_OPS_SCL IN 
	('02178500','02298500','02360800','00367300','00367400',
	 '00367401','00367403','00367404','00367405','03003000',
	 '03030601','03030602','03030606','03030603','03030605',
	 '03030604','03030600','00367400','00367405','00367404',
	 '00367403','00367401')
AND GA14.AC_LON_STA_TYP IN ('IG')
AND NOT EXISTS
	(SELECT *
	FROM OLWHRM1.AY01_BR_ATY X
	WHERE X.DF_PRS_ID = A.DF_PRS_ID_BR
	AND X.PF_ACT = 'ACONS')
);

/*ENROLLMENT RECORDS TABLE FOR REPORT2*/
CREATE TABLE SD02 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT X.DF_PRS_ID_STU
	,X.LF_STU_ENR_RPT_SEQ
	,X.LC_STU_ENR_TYP
	,X.LD_XPC_GRD
	,X.LD_ENR_CER
	,X.LD_STU_ENR_STA
	,X.IF_OPS_SCL_RPT
FROM OLWHRM1.SD02_STU_ENR X
WHERE X.IF_OPS_SCL_RPT IN 
	('02178500','02298500','02360800','00367300','00367400',
	 '00367401','00367403','00367404','00367405','03003000',
	 '03030601','03030602','03030606','03030603','03030605',
	 '03030604','03030600','00367400','00367405','00367404',
	 '00367403','00367401')
AND X.LC_STA_SD02 <> 'I'
);

/*ENROLLMENT RECORDS TABLE TO EXCLUDE BORROWERS WITH
MORE RECENTLY CERTIFIED SD02 ROWS AT NON TARGET SCHOOLS*/
CREATE TABLE SD02X1 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT X.DF_PRS_ID_STU
	,X.LF_STU_ENR_RPT_SEQ
	,X.LC_STU_ENR_TYP
	,X.LD_XPC_GRD
	,X.LD_ENR_CER
	,X.LD_STU_ENR_STA
	,X.IF_OPS_SCL_RPT
FROM OLWHRM1.SD02_STU_ENR X
WHERE X.IF_OPS_SCL_RPT NOT IN 
	('02178500','02298500','02360800','00367300','00367400',
	 '00367401','00367403','00367404','00367405','03003000',
	 '03030601','03030602','03030606','03030603','03030605',
	 '03030604','03030600','00367400','00367405','00367404',
	 '00367403','00367401')
AND X.LC_STA_SD02 <> 'I'
);

/*IDENTIFY BORROWERS WITH OPEN SLMA LOANS FOR EXCLUSION*/
CREATE TABLE SLMA AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT W.DF_PRS_ID_BR
FROM OLWHRM1.GA01_APP W
INNER JOIN OLWHRM1.GA10_LON_APP X
	ON W.AF_APL_ID = X.AF_APL_ID
INNER JOIN OLWHRM1.GA14_LON_STA Y
	ON X.AF_APL_ID = Y.AF_APL_ID
	AND X.AF_APL_ID_SFX = Y.AF_APL_ID_SFX
WHERE Y.AC_STA_GA14 = 'A'
AND Y.AC_LON_STA_TYP IN ('IA','ID','IG','IM','RP','CR','FB','DA')
AND (X.AF_CUR_LON_SER_AGY = '700191'
	OR X.AF_CUR_LON_OPS_LDR IN ('829587','830084','830151',
		'831053','831474','833253','888885','899993'))
);
DISCONNECT FROM DB2;

/*REMOVE RECORDS FROM CAMPAIGN IF THEY WERE IDENTIFIED IN SLMA TABLE ABOVE*/
CREATE TABLE SCHB1B AS
SELECT A.*
FROM SCHB1A A
WHERE A.DF_PRS_ID_BR NOT IN
(SELECT DF_PRS_ID_BR
FROM SLMA)
;

PROC SQL;
CREATE TABLE SD02X AS
SELECT X.*
FROM SD02X1 X
INNER JOIN SCHB1B Y
	ON X.DF_PRS_ID_STU = Y.DF_PRS_ID_STU
	AND X.IF_OPS_SCL_RPT NOT IN 
		('02178500','02298500','02360800','00367300','00367400',
	 	 '00367401','00367403','00367404','00367405','03003000',
	 	 '03030601','03030602','03030606','03030603','03030605',
	 	 '03030604','03030600','00367400','00367405','00367404',
	 	 '00367403','00367401')
;
QUIT;
*RSUBMIT;
/*ORDER AND NUMBER ENROLLMENT ROWS*/
PROC SORT DATA=SCHB1B;
BY DF_PRS_ID_STU AF_APL_OPS_SCL;
RUN;
PROC SORT DATA=SD02;
BY DF_PRS_ID_STU IF_OPS_SCL_RPT DESCENDING LD_ENR_CER DESCENDING LF_STU_ENR_RPT_SEQ;
RUN;
DATA SD02;
SET SD02;
BY DF_PRS_ID_STU IF_OPS_SCL_RPT DESCENDING LD_ENR_CER DESCENDING LF_STU_ENR_RPT_SEQ;
IF FIRST.IF_OPS_SCL_RPT THEN ORDER = 1;
ELSE ORDER + 1;
RUN;
/*IDENTIFY MOST RECENT CERT ROW*/
DATA SD021;
SET SD02;
WHERE ORDER = 1;
RUN;
/*IDENTIFY NEXT PREVIOUSLY CERTIFIED A, F, OR H ROW*/
DATA SD022;
SET SD02;
WHERE ORDER NE 1
AND LC_STU_ENR_TYP IN ('A','F','H');
RUN;
DATA SD022;
SET SD022;
BY DF_PRS_ID_STU IF_OPS_SCL_RPT DESCENDING LD_ENR_CER DESCENDING LF_STU_ENR_RPT_SEQ;
IF FIRST.IF_OPS_SCL_RPT;
RUN;
/*PUT IT ALL TOGETHER*/
*RSUBMIT;
PROC SQL;
CREATE TABLE SCHB1 AS
SELECT A.*
	,B.LC_STU_ENR_TYP AS LC_STU_ENR_TYP_1
	,B.LD_STU_ENR_STA AS LD_STU_ENR_STA_1
	,C.LD_XPC_GRD AS LD_XPC_GRD_2
FROM SCHB1B A
LEFT OUTER JOIN SD021 B
	ON A.DF_PRS_ID_STU = B.DF_PRS_ID_STU
	AND A.AF_APL_OPS_SCL = B.IF_OPS_SCL_RPT
LEFT OUTER JOIN SD022 C
	ON A.DF_PRS_ID_STU = C.DF_PRS_ID_STU
	AND A.AF_APL_OPS_SCL = C.IF_OPS_SCL_RPT
;
QUIT;
DATA SCHB1A;
SET SCHB1;
WHERE LC_STU_ENR_TYP_1 = 'G'
OR (LC_STU_ENR_TYP_1 = 'W'
	AND LD_STU_ENR_STA_1 = LD_XPC_GRD_2);
RUN;
PROC SQL;
CREATE TABLE SCHB1 AS
SELECT A.*
FROM SCHB1A A
WHERE NOT EXISTS (SELECT *
		FROM SD02X C
		WHERE C.DF_PRS_ID_STU = A.DF_PRS_ID_BR
		AND C.LD_STU_ENR_STA > A.LD_STU_ENR_STA_1)
;
QUIT;

/*ENDRSUBMIT;*/
/*DATA SCHB1; */
/*SET WORKLOCL.SCHB1; */
/*RUN;*/

DATA SCHB1 IVAD;
SET SCHB1;
IF DI_PHN_VLD = 'N' THEN OUTPUT IVAD;
ELSE IF DI_VLD_ADR = 'N' THEN OUTPUT IVAD;
ELSE OUTPUT SCHB1;
RUN;

DATA _NULL_;
SET  WORK.SCHB1;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT DF_PRS_ID_BR $9. ;
FORMAT DM_PRS_1 $12. ;
FORMAT DM_PRS_LST $35. ;
FORMAT AF_APL_OPS_SCL $8. ;
FORMAT IM_IST_FUL $40. ;
FORMAT BWR_BAL 12.2 ;
FORMAT MAX_CLC_RPD MMDDYY10. ;
FORMAT APRX_1ST_DUE MMDDYY10. ;
DO;
PUT DF_PRS_ID_BR $ @;
PUT DM_PRS_1 $ @;
PUT DM_PRS_LST $ @;
PUT AF_APL_OPS_SCL $ @;
PUT IM_IST_FUL $ @;
PUT BWR_BAL @;
PUT MAX_CLC_RPD @;
PUT APRX_1ST_DUE @;
PUT DM_FGN_CNY @;
PUT DC_DOM_ST @;
PUT COST_CENTER_CODE ;

;
END;
RUN;

DATA _NULL_;                                      
CURRENT=TODAY();                                  
CALL SYMPUT('CURDATE',PUT(CURRENT,MMDDYY10.));       
RUN;  

PROC PRINTTO PRINT=REPORT3 NEW;
RUN;
OPTIONS PAGENO=1 LS=100 NODATE CENTER;
TITLE 'SC--HIGH BALANCE INVALID ADDRESS OR PHONE';
TITLE2 "RUN DATE: &CURDATE";
PROC CONTENTS DATA=IVAD OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 100*'-';
	PUT      ////////
		@45 '**** NO OBSERVATIONS FOUND ****';
	PUT ////////
		@49 '-- END OF REPORT --';
	PUT /////////////////////////
		@38 "JOB = UTLWM04  	 REPORT = ULWM04.LWM04R3";
	END;
RETURN;
RUN;
PROC PRINT NOOBS SPLIT='/' DATA=IVAD N="TOTAL: ";
VAR DF_PRS_ID_BR
	DI_VLD_ADR
	DI_PHN_VLD;
LABEL DF_PRS_ID_BR = 'SSN' 
	DI_PHN_VLD = 'VALID PHONE'
	DI_VLD_ADR = 'VALID ADDRESS';
FOOTNOTE  'JOB = UTLWM04 	 REPORT = ULWM04.LWM04R3';
RUN;

PROC PRINTTO;
RUN;