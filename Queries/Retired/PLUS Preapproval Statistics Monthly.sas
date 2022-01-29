/*DSASG12 PLUS PREAPPROVAL STATISTICS*/

/*THIS JOB PRODUCES THREE REPORTS THAT WILL BE WRITTEN TO T:\SAS.*/
/*USE WORD TO PRINT THESE REPORTS AND CHANGE THE FONT TO 'COURIER NEW' THE FONT SIZE*/
/*TO 8 AND THE ORIENTATION TO LANDSCAPE*/

/*INPUT MASTER TEXT FILE*/
DATA PLUS_TXT;
	INFILE 'X:\ARCHIVE\WEBAPPS\MASTERPLUSPREAPP.TXT' DLM = ',' DSD;
	LENGTH DF_PRS_ID_BR $9 /*VAR20 $10;*/;
	INFORMAT WEB_DT MMDDYY10.;
	FORMAT WEB_DT MMDDYY10.;
	INPUT VAR1 $ DF_PRS_ID_BR $ VAR3 $ VAR4 $ VAR5 $ VAR6 $ VAR7 $ VAR8 $ VAR9 $ VAR10 $ VAR11 $ VAR12 $ VAR13 $ VAR14 $ VAR15 $ IF_IST $ VAR17 $ VAR18 $ VAR19 $ WEB_DT $ VAR21 $ VAR22 $ VAR23 $ PORTAL $;
RUN;

DATA _NULL_;
     CALL SYMPUT('BEGIN',"'"||PUT(INTNX('MONTH',TODAY(),-1,'beginning'), MMDDYYD10.)||"'");
     CALL SYMPUT('END',"'"||PUT(INTNX('MONTH',TODAY(),-1,'end'), MMDDYYD10.)||"'");
	 CALL SYMPUT('PRV_BEGIN',INTNX('MONTH',TODAY(),-1,'beginning'));
     CALL SYMPUT('PRV_END',INTNX('MONTH',TODAY(),-1,'end'));
	 CALL SYMPUT('EFFDATE',TRIM(LEFT(UPCASE(
		PUT(INTNX('MONTH',TODAY(),-1), MONNAME9.)||' '||
		PUT(INTNX('MONTH',TODAY(),-1), YEAR4.)))));
RUN;
%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT END = &END;

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;

/*GET INFO FOR CREDIT CHECKED PERFROMED IN THE PREVIOUS MONTH*/
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DEN AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.DF_PRS_ID_BR
	,A.AF_APL_ID
	,B.AD_PRC
	,B.AC_PRC_STA
	,B.AC_LON_TYP
	,A.AD_CRD_CHK_PRF
	,A.AC_CRD_CHK_PRF
	,D.AF_APL_OPS_SCL AS IF_IST
	,F.DC_DOM_ST
FROM OLWHRM1.GA01_APP A
INNER JOIN OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
INNER JOIN 
	(SELECT K.AF_APL_ID
	 	,K.AF_APL_OPS_SCL
	 FROM OLWHRM1.GA01_APP K
	 INNER JOIN OLWHRM1.GA10_LON_APP J
	 	ON K.AF_APL_ID = J.AF_APL_ID
	 WHERE J.AC_LON_TYP = 'PL'
	 AND J.AD_PRC = 
 			(SELECT MAX(Y.AD_PRC)
			 FROM OLWHRM1.GA01_APP X
			 INNER JOIN OLWHRM1.GA10_LON_APP Y
			 	ON X.AF_APL_ID = Y.AF_APL_ID
			 WHERE Y.AF_APL_ID = J.AF_APL_ID
			 AND Y.AC_LON_TYP = J.AC_LON_TYP
			 AND X.AD_CRD_CHK_PRF BETWEEN &BEGIN AND &END)
	 ) D
	ON A.AF_APL_ID = D.AF_APL_ID
INNER JOIN OLWHRM1.SC01_LGS_SCL_INF E
	ON D.AF_APL_OPS_SCL = E.IF_IST
LEFT OUTER JOIN OLWHRM1.PD03_PRS_ADR_PHN F
	ON A.DF_PRS_ID_BR = F.DF_PRS_ID	
	AND F.DC_ADR = 'L'

WHERE A.AD_CRD_CHK_PRF BETWEEN &BEGIN AND &END
AND B.AC_LON_TYP = 'PL'
);

/*GET INFO FOR GCCKS IN THE PREVIOUS MONTH*/
CREATE TABLE DEN2 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.DF_PRS_ID_BR
	,A.AF_APL_ID
	,B.AD_PRC
	,B.AC_PRC_STA
	,B.AC_LON_TYP
	,C.PF_ACT
	,C.BF_CRT_DTS_AY01 AS BD_ATY_PRF
	,D.AF_APL_OPS_SCL AS IF_IST
	,F.DC_DOM_ST
FROM OLWHRM1.GA01_APP A
INNER JOIN OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
INNER JOIN OLWHRM1.AY01_BR_ATY C
	ON A.DF_PRS_ID_BR = C.DF_PRS_ID
INNER JOIN 
	(SELECT K.AF_APL_ID
	 	,K.AF_APL_OPS_SCL
	 FROM OLWHRM1.GA01_APP K
	 INNER JOIN OLWHRM1.GA10_LON_APP J
	 	ON K.AF_APL_ID = J.AF_APL_ID
	 WHERE J.AC_LON_TYP = 'PL'
	 AND J.AD_PRC = 
 			(SELECT MAX(Y.AD_PRC)
			 FROM OLWHRM1.GA01_APP X
			 INNER JOIN OLWHRM1.GA10_LON_APP Y
			 	ON X.AF_APL_ID = Y.AF_APL_ID
			 WHERE Y.AF_APL_ID = J.AF_APL_ID
			 AND Y.AC_LON_TYP = J.AC_LON_TYP)
	 ) D
	ON A.AF_APL_ID = D.AF_APL_ID
LEFT OUTER JOIN OLWHRM1.PD03_PRS_ADR_PHN F
	ON A.DF_PRS_ID_BR = F.DF_PRS_ID	
	AND F.DC_ADR = 'L'

WHERE C.PF_ACT IN ('GCCKA','GCCKD','GCCKS')
AND C.BD_ATY_PRF BETWEEN &BEGIN AND &END
AND EXISTS
	(SELECT *
	 FROM OLWHRM1.AY01_BR_ATY X
	 WHERE X.DF_PRS_ID = C.DF_PRS_ID
	 AND X.PF_ACT = 'GCCKS'
	 AND X.BD_ATY_PRF BETWEEN &BEGIN AND &END)
);

/*SCHOOL NAMES*/
CREATE TABLE SC01 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT IF_IST
	,IM_IST_FUL
FROM OLWHRM1.SC01_LGS_SCL_INF
);
DISCONNECT FROM DB2;
ENDRSUBMIT;

DATA DEN;SET WORKLOCL.DEN;RUN;
DATA DEN2;SET WORKLOCL.DEN2;RUN;
DATA SC01;SET WORKLOCL.SC01;RUN;

PROC SORT DATA=DEN;BY DF_PRS_ID_BR AF_APL_ID;RUN;
PROC SORT DATA=DEN2;BY DF_PRS_ID_BR AF_APL_ID;RUN;

DATA DEN;
MERGE DEN DEN2;
BY DF_PRS_ID_BR AF_APL_ID;
RUN;

PROC SORT DATA=SC01;BY IF_IST;RUN;
PROC SORT DATA=DEN;BY IF_IST;RUN;

DATA DEN;
MERGE DEN (IN=A) SC01 (IN=B);
BY IF_IST;
IF A;
RUN;

/*BEGIN R2 CALCULATIONS*/
/*===============================================================================*/
%MACRO R2A(TBL1,PRF_IND,CRIT_IND);
PROC SQL;
CREATE TABLE &TBL1 AS
SELECT IM_IST_FUL
	,IF_IST
	,COUNT(DISTINCT DF_PRS_ID_BR) AS &PRF_IND._COUNT
FROM DEN
WHERE AC_CRD_CHK_PRF = &CRIT_IND
GROUP BY IM_IST_FUL
	,IF_IST;
QUIT;
RUN;
%MEND;
%R2A(R2A1,Y,'Y');
%R2A(R2A2,D,'D');

PROC SQL;
CREATE TABLE R2B AS 
SELECT IM_IST_FUL
	,IF_IST
	,COUNT(DISTINCT DF_PRS_ID_BR) AS BR_GCCKS_CT
FROM DEN
WHERE PF_ACT = 'GCCKS'
GROUP BY IM_IST_FUL
	,IF_IST;
QUIT;
RUN;

PROC SORT DATA=R2A1;BY IM_IST_FUL IF_IST;RUN;
PROC SORT DATA=R2A2;BY IM_IST_FUL IF_IST;RUN;
PROC SORT DATA=R2B;BY IM_IST_FUL IF_IST;RUN;

DATA R2;
MERGE R2A1 R2A2 R2B;
BY IM_IST_FUL IF_IST;
RUN;

DATA R2;
SET R2;
IF Y_COUNT = . THEN Y_COUNT = 0;
	ELSE Y_COUNT = Y_COUNT;
IF D_COUNT = . THEN D_COUNT = 0;
	ELSE D_COUNT = D_COUNT;
IF BR_GCCKS_CT = . THEN BR_GCCKS_CT = 0;
	ELSE BR_GCCKS_CT = BR_GCCKS_CT;
RUN;
/*===============================================================================*/
/*END R2 CALCULATIONS*/

/*BEGIN R3 CALCULATIONS*/
/*===============================================================================*/
DATA PLUS_TXT (KEEP=DF_PRS_ID_BR WEB_DT PORTAL IF_IST VAR19);
SET PLUS_TXT;
WHERE WEB_DT GE &PRV_BEGIN AND WEB_DT LE &PRV_END;
RUN;

PROC SORT DATA=SC01; BY IF_IST; RUN;
PROC SORT DATA=PLUS_TXT; BY IF_IST; RUN;

DATA R3A;
MERGE SC01 (IN=A) PLUS_TXT (IN=B);
BY IF_IST;
IF B;
RUN;

PROC SQL;
CREATE TABLE R3B AS 
SELECT DISTINCT A.IM_IST_FUL
	,A.IF_IST
	,COALESCE(B.STU_PRT_CT,0) AS STU_PRT_CT
	,COALESCE(C.SCHL_CT,0) AS SCHL_CT
	,COALESCE(D.INHS_CT,0) AS INHS_CT
FROM R3A A
LEFT OUTER JOIN 
	(SELECT IF_IST
		,PORTAL AS STU_PRT
		,COUNT(DISTINCT DF_PRS_ID_BR) AS STU_PRT_CT
	 FROM R3A
	 WHERE PORTAL = 'StuPort'
	 GROUP BY IF_IST
		,PORTAL 
	 ) B
	ON A.IF_IST = B.IF_IST
LEFT OUTER JOIN 
	(SELECT IF_IST
		,PORTAL 
		,COUNT(DISTINCT DF_PRS_ID_BR) AS SCHL_CT
	 FROM R3A
	 WHERE PORTAL = 'school'
	 GROUP BY IF_IST
		,PORTAL 
	 ) C
	ON A.IF_IST = C.IF_IST
LEFT OUTER JOIN 
	(SELECT IF_IST
		,PORTAL 
		,COUNT(DISTINCT DF_PRS_ID_BR) AS INHS_CT
	 FROM R3A
	 WHERE PORTAL = 'inhouse'
	 GROUP BY IF_IST
		,PORTAL 
	 ) D
	ON A.IF_IST = D.IF_IST
;
QUIT;
RUN;

PROC SORT DATA=R3B; BY IM_IST_FUL IF_IST; RUN;

%MACRO R3A(TBL1,NM_IND,CRIT_IND);
PROC SQL;
CREATE TABLE &TBL1 AS
SELECT IM_IST_FUL
	,IF_IST
	,COUNT(DISTINCT DF_PRS_ID_BR) AS &NM_IND._COUNT
FROM R3A
WHERE VAR19 = &CRIT_IND
GROUP BY IM_IST_FUL
	,IF_IST;
QUIT;
RUN;
%MEND;
%R3A(R3C,N,'N');
%R3A(R3D,S,'S');

DATA R3;
MERGE R3B R3C R3D;
BY IM_IST_FUL IF_IST; 
RUN;

DATA R3;
SET R3;
IF N_COUNT = . THEN N_COUNT = 0;
	ELSE N_COUNT = N_COUNT;
IF S_COUNT = . THEN S_COUNT = 0;
	ELSE S_COUNT = S_COUNT;
RUN;
/*===============================================================================*/
/*END R3 CALCULATIONS*/

/*BEGIN R4 CALCULATIONS*/
/*===============================================================================*/
PROC SQL;
CREATE TABLE R4A AS 
SELECT * 
FROM DEN A
WHERE EXISTS
	(SELECT *
	 FROM DEN X
	 WHERE X.DF_PRS_ID_BR = A.DF_PRS_ID_BR
	 AND X.PF_ACT = 'GCCKS');
QUIT;
RUN;

PROC SQL;
CREATE TABLE R4B AS 
SELECT DISTINCT 
	A.DF_PRS_ID_BR 
	,A.AF_APL_ID 
	,A.DC_DOM_ST 
	,A.IM_IST_FUL 
	,A.IF_IST
	,A.PF_ACT AS GCCKS_ID
	,A.BD_ATY_PRF AS GCCKS_DT
	,B.GCCKA_ID
	,B.GCCKA_DT
	,C.GCCKD_ID
	,C.GCCKD_DT
FROM R4A A
LEFT OUTER JOIN 
	(SELECT DISTINCT DF_PRS_ID_BR 
		,PF_ACT AS GCCKA_ID
		,BD_ATY_PRF AS GCCKA_DT
	 FROM R4A
	 WHERE PF_ACT = 'GCCKA'
	 ) B
	ON A.DF_PRS_ID_BR  = B.DF_PRS_ID_BR 
LEFT OUTER JOIN 
	(SELECT DISTINCT DF_PRS_ID_BR  
		,PF_ACT AS GCCKD_ID
		,BD_ATY_PRF AS GCCKD_DT
	 FROM R4A
	 WHERE PF_ACT = 'GCCKD'
	 ) C
	ON A.DF_PRS_ID_BR  = C.DF_PRS_ID_BR 
WHERE A.PF_ACT = 'GCCKS'
;
QUIT;
RUN;

PROC SORT DATA = R4B NODUPRECS;BY DF_PRS_ID_BR;RUN;

%MACRO R4B(DSNM,AT,CRIT1,CRIT_C,CRIT2);
PROC SQL;
CREATE TABLE R4A&DSNM AS 
SELECT IM_IST_FUL
	,IF_IST
	,COUNT(DISTINCT DF_PRS_ID_BR) AS &AT._COUNT
FROM R4B
WHERE &CRIT1 &CRIT_C &CRIT2
GROUP BY IM_IST_FUL
	,IF_IST
;
QUIT;
RUN;
%MEND;
%R4B(_LA,LA,GCCKA_DT,GT,GCCKS_DT);
%R4B(_LD,LD,GCCKD_DT,GT,GCCKS_DT);
%R4B(_CA,CA,DC_DOM_ST,EQ,'CA');
%R4B(_TX,TX,DC_DOM_ST,EQ,'TX');

PROC SQL;
CREATE TABLE R4A_IP AS 
SELECT A.*
FROM R4B A
WHERE NOT EXISTS
	(SELECT * 
	 FROM R4B Y
	 WHERE Y.DF_PRS_ID_BR = A.DF_PRS_ID_BR
	 AND (GCCKS_DT < GCCKA_DT
	 	  OR
		  GCCKS_DT < GCCKD_DT));
QUIT;
RUN;

PROC SQL;
CREATE TABLE R4A_IP AS 
SELECT IM_IST_FUL
	,IF_IST
	,COUNT(DISTINCT DF_PRS_ID_BR) AS IP_COUNT
FROM R4A_IP
GROUP BY IM_IST_FUL
	,IF_IST
;
QUIT;
RUN;

%MACRO SORT(DSET);
PROC SORT DATA=&DSET;BY IM_IST_FUL IF_IST;RUN;
%MEND;
%SORT(R4A_IP);
%SORT(R4A_LA);
%SORT(R4A_LD);
%SORT(R4A_TX);
%SORT(R4A_CA);

DATA R4;
MERGE R4A_IP R4A_LA R4A_LD R4A_TX R4A_CA;
BY IM_IST_FUL IF_IST;
RUN;

DATA R4;
SET R4;
IF IP_COUNT = . THEN IP_COUNT = 0;
	ELSE IP_COUNT = IP_COUNT;
IF LA_COUNT = . THEN LA_COUNT = 0;
	ELSE LA_COUNT = LA_COUNT;
IF LD_COUNT = . THEN LD_COUNT = 0;
	ELSE LD_COUNT = LD_COUNT;
IF TX_COUNT = . THEN TX_COUNT = 0;
	ELSE TX_COUNT = TX_COUNT;
IF CA_COUNT = . THEN CA_COUNT = 0;
	ELSE CA_COUNT = CA_COUNT;
RUN;

DATA R4;
SET R4;
TOTAL=SUM(IP_COUNT,LA_COUNT,LD_COUNT);
RUN;
/*===============================================================================*/
/*END R4 CALCULATIONS*/

%MACRO SCHL(DSET);
DATA &DSET;
SET &DSET;
SCHOOL_ID = TRIM(IM_IST_FUL)||' '||'('||TRIM(IF_IST)||')';
RUN;
%MEND;
%SCHL(R2);
%SCHL(R3);
%SCHL(R4);

/*CREATE TOTALS*/
%MACRO TOT1(DSET_I,DSET_N);
DATA &DSET_I;
SET &DSET_I;
JVAR='XYZ';
RUN;
DATA &DSET_N;
SET &DSET_I;
RUN;
%MEND TOT1;
 
%TOT1(R2,R2C);
%TOT1(R3,R3E);
%TOT1(R4,R4C);

%MACRO TOT2(DSET,KEEP_LIST,CRIT);
DATA &DSET (KEEP=&KEEP_LIST);
SET &DSET END=LAST;
&CRIT
IF LAST;
RUN;
%MEND TOT2;

%TOT2(R2C,
	  Y_COUNT_TOT D_COUNT_TOT BR_GCCKS_CT_TOT JVAR,
	  Y_COUNT_TOT+Y_COUNT;D_COUNT_TOT+D_COUNT;BR_GCCKS_CT_TOT+BR_GCCKS_CT;);
%TOT2(R3E,
	  STU_PRT_CT_TOT SCHL_CT_TOT INHS_CT_TOT N_COUNT_TOT S_COUNT_TOT JVAR,
	  STU_PRT_CT_TOT+STU_PRT_CT;
	  SCHL_CT_TOT+SCHL_CT;
	  INHS_CT_TOT+INHS_CT;
	  N_COUNT_TOT+N_COUNT;
	  S_COUNT_TOT+S_COUNT;);
%TOT2(R4C,
	  IP_COUNT_TOT LA_COUNT_TOT LD_COUNT_TOT TX_COUNT_TOT CA_COUNT_TOT TOTAL_TOT JVAR,
	  IP_COUNT_TOT+IP_COUNT;
	  LA_COUNT_TOT+LA_COUNT;
	  LD_COUNT_TOT+LD_COUNT;
	  TX_COUNT_TOT+TX_COUNT;
	  CA_COUNT_TOT+CA_COUNT;
	  TOTAL_TOT+TOTAL;);

%MACRO TOT3(DSET1,DSET2);
DATA &DSET1;
MERGE &DSET1 &DSET2;
BY JVAR;
RUN;
PROC SQL;
CREATE TABLE &DSET1.COUNT AS 
SELECT COUNT(*) AS COUNT
FROM &DSET1;
QUIT;
RUN;
%MEND TOT3;
%TOT3(R2,R2C);
%TOT3(R3,R3E);
%TOT3(R4,R4C);

DATA _NULL_;
SET R2COUNT;
CALL SYMPUT('R2C',COUNT);
RUN;

DATA _NULL_;
SET R3COUNT;
CALL SYMPUT('R3C',COUNT);
RUN;

DATA _NULL_;
SET R4COUNT;
CALL SYMPUT('R4C',COUNT);
RUN;


/*WRITE REPORTS*/
OPTIONS CENTER NODATE PAGENO=1 ORIENTATION = LANDSCAPE;
OPTIONS PS=50 LS=127;

DATA R2;
SET R2;
FILE 'T:\SAS\PLUS PREAPPROVAL RESULTS.TXT' PRINT;
TITLE1	'PLUS PREAPPROVAL USEAGE';
TITLE2	'CREDIT CHECK RESULTS BY SCHOOL';
TITLE3	"&EFFDATE";
IF _N_ = 1 THEN DO;
	PUT / @1 'SCHOOL NAME' @60 'APPROVED' @70 'DENIED' @80 'STAFF REVIEW'  / 120*'-';
	END;

PUT @1 SCHOOL_ID	@60 Y_COUNT	@70 D_COUNT	@80 BR_GCCKS_CT;

IF _N_ = &R2C THEN DO;
	PUT 120*'_'/;
	PUT @1 'TOTALS' @60 Y_COUNT_TOT @70 D_COUNT_TOT @80 BR_GCCKS_CT_TOT;
	PUT ////@50 'JOB=DSASG12			REPORT=DSASG12.R2';
	END;

RUN;

DATA R3;
SET R3;
FILE 'T:\SAS\PORTAL STATISTICS.TXT' PRINT;
TITLE1	'PLUS PREAPPROVAL USAGE';
TITLE2	'PORTAL STATISTICS - DENIED AND STAFF REVIEW';
TITLE3	"&EFFDATE";
IF _N_ = 1 THEN DO;
	PUT / @70 'PORTAL' @100 'RESULTS'
		// @1 'SCHOOL NAME' @60 'SCHOOL' @70 'BORROWER' @80 'INHOUSE' @95 'DENIED' @105 'STAFF REVIEW'
		/120*'-';
	END;
PUT @1 SCHOOL_ID	@60 SCHL_CT @70 STU_PRT_CT @80 INHS_CT @95 N_COUNT @105 S_COUNT;

IF _N_ = &R3C THEN DO;
	PUT 120*'_'/;
	PUT @1 'TOTALS' @60 SCHL_CT_TOT @70 STU_PRT_CT_TOT @80 INHS_CT_TOT 
		@95 N_COUNT_TOT @105 S_COUNT_TOT;
	PUT ////@50 'JOB=DSASG12			REPORT=DSASG12.R3';
	END;
RUN;

DATA R4;
SET R4;
FILE 'T:\SAS\STAFF REVIEW DETAIL.TXT' PRINT;
TITLE1	'PLUS PREAPPROVAL USAGE';
TITLE2	'STAFF REVIEW DETAIL';
TITLE3	"&EFFDATE";
IF _N_ = 1 THEN DO;
	PUT / @60 'LATER' @70 'LATER' @80 'IN' @100 'TX' @110 'CA';
	PUT / @1 'SCHOOL NAME' @60 'APPROVED' @70 'DENIED' @80 'PROCESS' @90 'TOTAL' @100 'RESIDENT' @110 'RESIDENT'
		/120*'-';
	END;
PUT @1 SCHOOL_ID @60 LA_COUNT @70 LD_COUNT @80 IP_COUNT @90 TOTAL 
	@100 TX_COUNT @110 CA_COUNT;

IF _N_ = &R4C THEN DO;
	PUT 120*'_'/;
	PUT @1 'TOTALS' @60 LA_COUNT_TOT @70 LD_COUNT_TOT @80 IP_COUNT_TOT @90 TOTAL_TOT 
	@100 TX_COUNT_TOT @110 CA_COUNT_TOT;
	PUT ////@50 'JOB=DSASG12			REPORT=DSASG12.R4';
	END;
RUN;

/*USED FOR TESTING*/
/*DATA PLUS_TXT_TST;*/
/*	INFILE 'X:\ARCHIVE\WEBAPPS\MASTERPLUSPREAPP.TXT' DLM = ',' DSD;*/
/*	LENGTH DF_PRS_ID_BR $9;*/
/*	INFORMAT WEB_DT MMDDYY10.;*/
/*	FORMAT WEB_DT MMDDYY10.;*/
/*	INPUT VAR1 $ DF_PRS_ID_BR $ VAR3 $ VAR4 $ VAR5 $ VAR6 $ VAR7 $ VAR8 $ VAR9 $ VAR10 $ VAR11 $ VAR12 $ VAR13 $ VAR14 $ VAR15 $ IF_IST $ VAR17 $ VAR18 $ VAR19 $ WEB_DT $ VAR21 $ VAR22 $ VAR23 $ PORTAL $;*/
/*RUN;*/
/**/
/*PROC EXPORT DATA= DEN */
/*            OUTFILE= "T:\SAS\WAREHOUSE.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/
/*PROC EXPORT DATA= PLUS_TXT_TST */
/*            OUTFILE= "T:\SAS\TXT_FILE.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/