/*===============================================*/
/*UTLWS12 - DEFERMENT FORBEARANCE ABOUT TO EXPIRE*/
/*===============================================*/

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = /sas/whse/progrevw;*/
/*FILENAME REPORT2 "&RPTLIB/ULWS12.LWS12R2";*/
/*FILENAME REPORTZ "&RPTLIB/ULWS12.LWS12RZ";*/

OPTIONS SYMBOLGEN;

/*MACRO DATE INITIALIZATION DONE IN DATA NULL TO ENHANCE PERFORMANCE*/
DATA _NULL_;
	CALL SYMPUT('MNS60',"'"||PUT(INTNX('DAY',TODAY(),-60,'BEGINNING'), MMDDYYD10.)||"'");
	CALL SYMPUT('MNS8',"'"||PUT(INTNX('DAY',TODAY(),-8,'BEGINNING'), MMDDYYD10.)||"'");
	CALL SYMPUT('TODAY',"'"||PUT(INTNX('DAY',TODAY(),0,'BEGINNING'), MMDDYYD10.)||"'");
	CALL SYMPUT('DFDBEGIN',"'"||PUT(INTNX('DAY',TODAY(),+1,'BEGINNING'), MMDDYYD10.)||"'");
	CALL SYMPUT('DFDEND',"'"||PUT(INTNX('DAY',TODAY(),+15,'BEGINNING'), MMDDYYD10.)||"'");
RUN;

%SYSLPUT MNS8 = &MNS8;
%SYSLPUT MNS60 = &MNS60;
%SYSLPUT TODAY = &TODAY;
%SYSLPUT DFDBEGIN = &DFDBEGIN;
%SYSLPUT DFDEND = &DFDEND;

FILENAME REPORT2 "C:\WINDOWS\Temp\ULWS12.LWS12R2";

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
%MACRO DFATX(DATA1,DATA2,VALS);
PROC SORT DATA=&DATA1;
	BY &VALS;
RUN;
PROC SORT DATA=&DATA2;
	BY &VALS;
RUN;
DATA &DATA1;
	MERGE &DATA1 (IN=A) &DATA2 (IN=B);
	BY &VALS;
	IF A;
RUN;
%MEND;
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

PROC SQL STIMER;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
/*FORBEARANCE MASTER TABLE*/
CREATE TABLE FORB AS		
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.BF_SSN AS SSN	
FROM OLWHRM1.FB10_BR_FOR_REQ A
INNER JOIN OLWHRM1.LN60_BR_FOR_APV I
	ON A.BF_SSN = I.BF_SSN
	AND A.LF_FOR_CTL_NUM = I.LF_FOR_CTL_NUM
INNER JOIN OLWHRM1.LN10_LON B
	ON I.BF_SSN = B.BF_SSN
	AND I.LN_SEQ = B.LN_SEQ
INNER JOIN OLWHRM1.PD42_PRS_PHN C
	ON B.BF_SSN = C.DF_PRS_ID
INNER JOIN OLWHRM1.SD10_STU_SPR D
	ON B.LF_STU_SSN = D.LF_STU_SSN
INNER JOIN OLWHRM1.DW01_DW_CLC_CLU E
	ON B.BF_SSN = E.BF_SSN
	AND B.LN_SEQ = E.LN_SEQ
/*REPAYMENT TABLES*/
INNER JOIN OLWHRM1.LN65_LON_RPS F	
	ON B.BF_SSN = F.BF_SSN
	AND B.LN_SEQ = F.LN_SEQ
INNER JOIN OLWHRM1.LN66_LON_RPS_SPF G	
	ON F.BF_SSN = G.BF_SSN
	AND F.LN_SEQ = G.LN_SEQ
	AND F.LN_RPS_SEQ = G.LN_RPS_SEQ
INNER JOIN OLWHRM1.RS10_BR_RPD H
	ON F.BF_SSN = H.BF_SSN
	AND F.LN_RPS_SEQ = H.LN_RPS_SEQ
WHERE B.LA_CUR_PRI > 0
AND B.LC_STA_LON10 = 'R'
AND A.LC_FOR_STA = 'A'
AND A.LC_STA_FOR10 = 'A'
AND I.LC_STA_LON60 = 'A'
AND C.DI_PHN_VLD = 'Y'
AND C.DC_PHN = 'H'
AND C.DN_DOM_PHN_ARA <> ' '
AND C.DN_DOM_PHN_XCH <> ' '
AND C.DN_DOM_PHN_LCL <> ' '
AND E.WC_DW_LON_STA <> '06'
AND A.LD_FOR_REQ_END BETWEEN &DFDBEGIN AND &DFDEND /*FORBEARANCE END DATE WITHIN 1-15 DAYS*/
AND D.LD_SCL_SPR < &TODAY /*SCHOOL SEP DATE LESS THAN TODAY*/
AND E.WD_LON_RPD_SR > &MNS60 /*REPAYMENT START DATE > TODAY - 60*/
FOR READ ONLY WITH UR
);

/*DEFERMENT MASTER TABLE*/
CREATE TABLE DEFR AS		
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.BF_SSN AS SSN
FROM OLWHRM1.DF10_BR_DFR_REQ A
INNER JOIN OLWHRM1.LN50_BR_DFR_APV I
	ON A.BF_SSN = I.BF_SSN
	AND A.LF_DFR_CTL_NUM = I.LF_DFR_CTL_NUM
INNER JOIN OLWHRM1.LN10_LON B
	ON I.BF_SSN = B.BF_SSN
	AND I.LN_SEQ = B.LN_SEQ
INNER JOIN OLWHRM1.PD42_PRS_PHN C
	ON B.BF_SSN = C.DF_PRS_ID
INNER JOIN OLWHRM1.SD10_STU_SPR D
	ON B.LF_STU_SSN = D.LF_STU_SSN
INNER JOIN OLWHRM1.DW01_DW_CLC_CLU E
	ON B.BF_SSN = E.BF_SSN
	AND B.LN_SEQ = E.LN_SEQ
/*REPAYMENT TABLES*/
INNER JOIN OLWHRM1.LN65_LON_RPS F	
	ON B.BF_SSN = F.BF_SSN
	AND B.LN_SEQ = F.LN_SEQ
INNER JOIN OLWHRM1.LN66_LON_RPS_SPF G	
	ON F.BF_SSN = G.BF_SSN
	AND F.LN_SEQ = G.LN_SEQ
	AND F.LN_RPS_SEQ = G.LN_RPS_SEQ
INNER JOIN OLWHRM1.RS10_BR_RPD H
	ON F.BF_SSN = H.BF_SSN
	AND F.LN_RPS_SEQ = H.LN_RPS_SEQ
WHERE B.LA_CUR_PRI > 0
AND B.LC_STA_LON10 = 'R'
AND A.LC_DFR_STA = 'A'
AND A.LC_STA_DFR10 = 'A'
AND I.LC_STA_LON50 = 'A'
AND C.DI_PHN_VLD = 'Y'
AND C.DC_PHN = 'H'
AND C.DN_DOM_PHN_ARA <> ' '
AND C.DN_DOM_PHN_XCH <> ' '
AND C.DN_DOM_PHN_LCL <> ' '
AND E.WC_DW_LON_STA <> '06'
AND A.LD_DFR_REQ_END BETWEEN &DFDBEGIN AND &DFDEND /*DEFERMENT END DATE WITHIN 1-15 DAYS*/
AND D.LD_SCL_SPR < &TODAY /*SCHOOL SEP DATE LESS THAN TODAY*/
AND E.WD_LON_RPD_SR > &MNS60 /*REPAYMENT START DATE > TODAY - 60*/
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWS12.LWS12RZ);*/
/*QUIT;*/

/*ENDRSUBMIT;*/
/*DATA FORB;SET WORKLOCL.FORB;RUN;*/
/*DATA DEFR;SET WORKLOCL.DEFR;RUN;*/

/*EXCLUSION QUERIES*/
PROC SQL STIMER;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
/*EXCLUDE BORROWERS WITH ANY CONTACT WITHIN 45 DAYS*/
CREATE TABLE EX1 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT AY10.BF_SSN AS SSN
FROM OLWHRM1.AY10_BR_LON_ATY AY10
WHERE AY10.PF_RSP_ACT = 'CNTCT' 
AND AY10.LC_STA_ACTY10 = 'A'
AND ABS(DAYS(AY10.LD_REQ_RSP_ATY_PRF) - DAYS(CURRENT DATE)) < 45
FOR READ ONLY WITH UR
);

/*EXCLUDE PAUTO ARC WITHIN 60 DAYS*/
CREATE TABLE EX2 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT AY10.BF_SSN AS SSN
FROM OLWHRM1.AY10_BR_LON_ATY AY10
WHERE AY10.PF_REQ_ACT = 'PAUTO' 
AND AY10.LC_STA_ACTY10 = 'A'
AND ABS(DAYS(AY10.LD_REQ_RSP_ATY_PRF) - DAYS(CURRENT DATE)) < 60
FOR READ ONLY WITH UR
);

/*MORE EXCLUSION INFO*/
CREATE TABLE EX3 AS		
SELECT *
FROM CONNECTION TO DB2 (	
SELECT DISTINCT BF_SSN AS SSN
FROM OLWHRM1.WQ20_TSK_QUE
WHERE WF_QUE IN ('X1','X2')
FOR READ ONLY WITH UR
);

CREATE TABLE EX4 AS		
SELECT *
FROM CONNECTION TO DB2 (	
SELECT DISTINCT BF_SSN AS SSN
FROM OLWHRM1.AY10_BR_LON_ATY 
WHERE PF_REQ_ACT = 'XWXPR' 
AND DAYS(LD_REQ_RSP_ATY_PRF) > DAYS(CURRENT DATE) - 8
FOR READ ONLY WITH UR
);

CREATE TABLE EX5 AS		
SELECT *
FROM CONNECTION TO DB2 (	
SELECT DISTINCT X.BF_SSN AS SSN
FROM OLWHRM1.AY10_BR_LON_ATY X
WHERE X.PF_REQ_ACT = 'XWXPR'
AND X.LD_ATY_RSP < 
		(SELECT MAX(XX.LD_ATY_RSP)
		 FROM OLWHRM1.AY10_BR_LON_ATY XX
		 WHERE X.BF_SSN = XX.BF_SSN
		 AND XX.PF_REQ_ACT = 'P200C')
FOR READ ONLY WITH UR
);

/*EXCLUDE IF BORROWER HAS MORE THAN 4 XWXPR ARCS*/
CREATE TABLE EX6 AS		
SELECT *
FROM CONNECTION TO DB2 (
SELECT A.BF_SSN AS SSN
FROM OLWHRM1.AY10_BR_LON_ATY A
INNER JOIN OLWHRM1.FB10_BR_FOR_REQ B
	ON A.BF_SSN = B.BF_SSN
WHERE A.PF_REQ_ACT = 'XWXPR'
AND A.PF_RSP_ACT NOT IN ('CNTCT','COMPL')
AND ABS(DAYS(A.LD_REQ_RSP_ATY_PRF)- DAYS(B.LD_FOR_REQ_END)) < 45
GROUP BY A.BF_SSN
HAVING COUNT(*) > 4
FOR READ ONLY WITH UR
);

/*EXCLUDE IF BORROWER HAS MORE THAN 4 XWXPR ARCS*/
CREATE TABLE EX7 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT A.BF_SSN AS SSN
FROM OLWHRM1.AY10_BR_LON_ATY A
INNER JOIN OLWHRM1.DF10_BR_DFR_REQ C
	ON A.BF_SSN = C.BF_SSN
WHERE A.PF_REQ_ACT = 'XWXPR'
AND A.PF_RSP_ACT NOT IN ('CNTCT','COMPL')
AND ABS(DAYS(A.LD_REQ_RSP_ATY_PRF)- DAYS(C.LD_DFR_REQ_END)) < 45
GROUP BY A.BF_SSN
HAVING COUNT(*) > 4
FOR READ ONLY WITH UR
);

CREATE TABLE EX8 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT Y.BF_SSN AS SSN
FROM OLWHRM1.DF10_BR_DFR_REQ Y
INNER JOIN OLWHRM1.LN50_BR_DFR_APV Z
	ON Z.BF_SSN = Y.BF_SSN
	AND Z.LF_DFR_CTL_NUM = Y.LF_DFR_CTL_NUM
WHERE DAYS(LD_DFR_REQ_END) > DAYS(CURRENT DATE) + 45
AND Y.LC_DFR_STA = 'A'
AND Y.LC_STA_DFR10 = 'A'
AND Z.LC_STA_LON50 = 'A'
FOR READ ONLY WITH UR
);

CREATE TABLE EX9 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT Y.BF_SSN AS SSN
FROM OLWHRM1.FB10_BR_FOR_REQ Y
INNER JOIN OLWHRM1.LN60_BR_FOR_APV Z
	ON Z.BF_SSN = Y.BF_SSN
	AND Z.LF_FOR_CTL_NUM = Y.LF_FOR_CTL_NUM
WHERE DAYS(LD_FOR_REQ_END) > DAYS(CURRENT DATE) + 45
AND Y.LC_FOR_STA = 'A'
AND Y.LC_STA_FOR10 = 'A'
AND Z.LC_STA_LON60 = 'A'
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*ENDRSUBMIT;*/
/*DATA EX1;SET WORKLOCL.EX1;RUN;*/
/*DATA EX2;SET WORKLOCL.EX2;RUN;*/
/*DATA EX3;SET WORKLOCL.EX3;RUN;*/
/*DATA EX4;SET WORKLOCL.EX4;RUN;*/
/*DATA EX5;SET WORKLOCL.EX5;RUN;*/
/*DATA EX6;SET WORKLOCL.EX6;RUN;*/
/*DATA EX7;SET WORKLOCL.EX7;RUN;*/
/*DATA EX8;SET WORKLOCL.EX8;RUN;*/
/*DATA EX9;SET WORKLOCL.EX9;RUN;*/

/*CREATE COMBINED DATA SETS*/
DATA DFR_FRB;
SET FORB DEFR;
RUN;

DATA EXCLUDE;
SET EX1 EX2 EX3 EX4 EX5 EX6 EX7 EX8 EX9;
RUN;

PROC SORT DATA=DFR_FRB NODUPKEY;BY SSN;RUN;
PROC SORT DATA=EXCLUDE NODUPKEY;BY SSN;RUN;

DATA DFR_FRB;
MERGE DFR_FRB (IN=A) EXCLUDE (IN=B);
BY SSN;
IF A AND NOT B;
RUN;

/*ENDRSUBMIT;*/
/*DATA DFR_FRB;*/
/*SET WORKLOCL.DFR_FRB;*/
/*RUN;*/

/*LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;*/
/*RSUBMIT;*/
/*GET EXISTING BF_SSNS IN X2 QUEUE*/
PROC SQL STIMER;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE X2Q AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT 
	 WQ20.BF_SSN AS SSN
	,MAX(AY10.LD_REQ_RSP_ATY_PRF) AS LD_REQ_RSP_ATY_PRF
	,'X' AS WQ20_IND
FROM OLWHRM1.WQ20_TSK_QUE WQ20 
LEFT OUTER JOIN OLWHRM1.AY10_BR_LON_ATY AY10
	ON WQ20.BF_SSN = AY10.BF_SSN 
	AND AY10.PF_REQ_ACT = 'XWXPR'
	AND AY10.LC_STA_ACTY10 = 'A'
WHERE WQ20.WF_QUE = 'X2'
GROUP BY WQ20.BF_SSN
FOR READ ONLY WITH UR
);

CREATE TABLE WQ10 AS
SELECT * 
FROM CONNECTION TO DB2 (
SELECT WN_DAY_PRD_GOA_WRK
	,'Y' AS X2_IND
FROM OLWHRM1.WQ10_TSK_QUE_DFN
WHERE WF_QUE = 'X2'
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*ENDRSUBMIT;*/
/*DATA X2Q;SET WORKLOCL.X2Q;RUN;*/
/*DATA WQ10;SET WORKLOCL.WQ10;RUN;*/

DATA DFR_FRB;
SET DFR_FRB;
X2_IND = 'Y';
RUN;

DATA DFR_FRB (DROP=X2_IND);
MERGE DFR_FRB (IN=A) WQ10 (IN=B);
BY X2_IND;
RUN;

DATA DFR_FRB;
SET DFR_FRB;
IF SSN = ' ' THEN DELETE;
RUN;

/*CREATE MASTER DFR_FRB DATA SET*/
DATA DFR_FRB;
SET DFR_FRB X2Q;
RUN;

PROC SORT DATA=DFR_FRB NODUPKEY;BY SSN;RUN;

/*GET DEMOGRAPHIC INFO*/
PROC SQL STIMER ;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DEMO_INFO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT 
	LN10.BF_SSN AS SSN
	,CASE 
		WHEN PD10.DM_PRS_MID = ' '
		THEN RTRIM(PD10.DM_PRS_1)||' '||PD10.DM_PRS_LST
		WHEN PD10.DM_PRS_MID <> ' '
		THEN RTRIM(PD10.DM_PRS_1)||' '||RTRIM(PD10.DM_PRS_MID)||' '||PD10.DM_PRS_LST
	 END AS NAME
	,SUBSTR(PD10.DM_PRS_LST,1,2) AS NAME_SORT
	,PD10.DD_BRT
/*ADDRESS INFO*/
	,PD30.DX_STR_ADR_1 	 
	,PD30.DX_STR_ADR_2 	 
	,PD30.DX_STR_ADR_3 	 
	,PD30.DM_CT
	,PD30.DC_DOM_ST
	,PD30.DF_ZIP_CDE
	,PD30.DM_FGN_CNY
	,PD30.DM_FGN_ST
	,PD30.DI_VLD_ADR
/*	PHONE INFO*/
	,PD42.DC_PHN
	,PD42.DN_DOM_PHN_ARA||PD42.DN_DOM_PHN_XCH||PD42.DN_DOM_PHN_LCL AS HME_PHN
FROM OLWHRM1.LN10_LON LN10
INNER JOIN OLWHRM1.PD10_PRS_NME PD10
	ON LN10.BF_SSN = PD10.DF_PRS_ID
INNER JOIN OLWHRM1.PD30_PRS_ADR PD30
	ON LN10.BF_SSN = PD30.DF_PRS_ID
	AND PD30.DC_ADR = 'L'
INNER JOIN OLWHRM1.PD42_PRS_PHN PD42
	ON LN10.BF_SSN = PD42.DF_PRS_ID
INNER JOIN OLWHRM1.DW01_DW_CLC_CLU DW01
	ON LN10.BF_SSN = DW01.BF_SSN
	AND LN10.LN_SEQ = DW01.LN_SEQ
WHERE LN10.LA_CUR_PRI > 0
AND LN10.LC_STA_LON10 = 'R'
AND DW01.WC_DW_LON_STA <> '06'
AND DW01.WD_LON_RPD_SR > &MNS60 
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
%DFATX(DFR_FRB,DEMO_INFO,SSN);

/*GET LOAN COUNT AND TOTAL INFO*/
PROC SQL STIMER;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE LN10_INFO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT A.BF_SSN AS SSN
	,COUNT(*) AS COUNT
	,SUM(A.LA_CUR_PRI) AS LA_CUR_PRI
FROM OLWHRM1.LN10_LON A
INNER JOIN OLWHRM1.DW01_DW_CLC_CLU B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
WHERE A.LA_CUR_PRI > 0
AND A.LC_STA_LON10 = 'R'
AND B.WC_DW_LON_STA <> '06'
AND B.WD_LON_RPD_SR > &MNS60 
GROUP BY A.BF_SSN
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
%DFATX(DFR_FRB,LN10_INFO,SSN);

/*GET AMOUNT DUE*/
PROC SQL STIMER;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE AMTDU AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT X.BF_SSN AS SSN
	,SUM(COALESCE(X.LA_BIL_PAS_DU,0) +
		 COALESCE(X.LA_BIL_DU_PRT,0) +
		 COALESCE(X.LA_LTE_FEE_OTS_PRT,0)) AS AMTDU
FROM OLWHRM1.LN80_LON_BIL_CRF X
INNER JOIN OLWHRM1.LN10_LON Y
	ON X.BF_SSN = Y.BF_SSN
	AND X.LN_SEQ = Y.LN_SEQ
INNER JOIN OLWHRM1.DW01_DW_CLC_CLU B
	ON Y.BF_SSN = B.BF_SSN
	AND Y.LN_SEQ = B.LN_SEQ
WHERE Y.LA_CUR_PRI > 0
AND Y.LC_STA_LON10 = 'R'
AND B.WC_DW_LON_STA <> '06'
AND B.WD_LON_RPD_SR > &MNS60 
AND X.LC_STA_LON80 = 'A'
AND X.LD_BIL_CRT = 
		(SELECT MAX(Y.LD_BIL_CRT) 
		 FROM OLWHRM1.BL10_BR_BIL Y
		 WHERE Y.BF_SSN = X.BF_SSN
		 AND Y.LC_STA_BIL10 = 'A'
		 AND Y.LC_BIL_TYP = 'P')
GROUP BY X.BF_SSN
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
%DFATX(DFR_FRB,AMTDU,SSN);

/*GET MAX EFF DATE FOR LN90 INFO*/
PROC SQL STIMER;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE TRX_DT AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT LN90.BF_SSN AS SSN
	,MAX(LN90.LD_FAT_EFF) AS MX_EFFDT
FROM OLWHRM1.LN10_LON Y
INNER JOIN OLWHRM1.DW01_DW_CLC_CLU DW01
	ON DW01.BF_SSN = Y.BF_SSN
	AND DW01.LN_SEQ = Y.LN_SEQ
INNER JOIN OLWHRM1.LN90_FIN_ATY LN90
	ON Y.BF_SSN = LN90.BF_SSN
	AND Y.LN_SEQ = LN90.LN_SEQ
WHERE Y.LA_CUR_PRI > 0
AND Y.LC_STA_LON10 = 'R'
AND DW01.WC_DW_LON_STA <> '06'
AND DW01.WD_LON_RPD_SR > &MNS60 
AND LN90.PC_FAT_TYP = '10'
AND LN90.PC_FAT_SUB_TYP IN ('10','11','12')
AND LN90.LC_FAT_REV_REA = ' ' 
AND LN90.LC_STA_LON90 = 'A'
GROUP BY LN90.BF_SSN
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
%DFATX(DFR_FRB,TRX_DT,SSN);

/*GET DAYS DELINQUENT*/
PROC SQL STIMER;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DLQ_DYS AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT LN16.BF_SSN AS SSN
	,MAX(LN16.LN_DLQ_MAX) AS LN_DLQ_MAX
FROM OLWHRM1.LN10_LON LN10
INNER JOIN OLWHRM1.DW01_DW_CLC_CLU DW01
	ON DW01.BF_SSN = LN10.BF_SSN
	AND DW01.LN_SEQ = LN10.LN_SEQ
INNER JOIN OLWHRM1.LN16_LON_DLQ_HST LN16
	ON LN10.BF_SSN = LN16.BF_SSN
	AND LN10.LN_SEQ = LN16.LN_SEQ
WHERE LN10.LA_CUR_PRI > 0
AND LN10.LC_STA_LON10 = 'R'
AND DW01.WC_DW_LON_STA <> '06'
AND DW01.WD_LON_RPD_SR > &MNS60 
AND LN16.LC_STA_LON16 = '1' 
GROUP BY LN16.BF_SSN
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
%DFATX(DFR_FRB,DLQ_DYS,SSN);

/*LAST ATTEMPTED CONTACT DATE*/
PROC SQL STIMER;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE ATY_ATT AS
SELECT * 
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.BF_SSN AS SSN
	,MAX(AY10.LD_ATY_RSP) AS LD_ATY_RSP_ATT
FROM OLWHRM1.LN10_LON A
INNER JOIN OLWHRM1.AY10_BR_LON_ATY AY10
	ON AY10.BF_SSN = A.BF_SSN
INNER JOIN OLWHRM1.DW01_DW_CLC_CLU B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
WHERE A.LA_CUR_PRI > 0
AND A.LC_STA_LON10 = 'R'
AND B.WC_DW_LON_STA <> '06'
AND B.WD_LON_RPD_SR > &MNS60 
AND AY10.PF_RSP_ACT IN ('NOCTC','INVPH')
GROUP BY A.BF_SSN
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
%DFATX(DFR_FRB,ATY_ATT,SSN);

/*LAST CONTACT DATE*/
PROC SQL STIMER;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE ATY_CTC AS
SELECT * 
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.BF_SSN AS SSN
	,MAX(AY10.LD_ATY_RSP) AS LD_ATY_RSP_CTC
FROM OLWHRM1.LN10_LON A
INNER JOIN OLWHRM1.AY10_BR_LON_ATY AY10
	ON AY10.BF_SSN = A.BF_SSN
INNER JOIN OLWHRM1.DW01_DW_CLC_CLU B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
WHERE A.LA_CUR_PRI > 0
AND A.LC_STA_LON10 = 'R'
AND B.WC_DW_LON_STA <> '06'
AND B.WD_LON_RPD_SR > &MNS60 
AND AY10.PF_RSP_ACT = 'CNTCT'
GROUP BY A.BF_SSN
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
%DFATX(DFR_FRB,ATY_CTC,SSN);
ENDRSUBMIT;

DATA DFR_FRB;
SET WORKLOCL.DFR_FRB;
RUN;

/*BREAK OUT AND REASSEMBLE PHONE INFO*/
%MACRO PHN(DSET,PHNVAL);
DATA PHN_&DSET (KEEP=SSN DC_PHN_&DSET PHN_&DSET);
SET DFR_FRB;
WHERE DC_PHN = &PHNVAL;
DC_PHN_&DSET = DC_PHN;
PHN_&DSET = HME_PHN;
RUN;
PROC SORT DATA = PHN_&DSET;
BY SSN;
RUN;
%MEND;
%PHN(H,'H');
%PHN(A,'A');
%PHN(W,'W');
DATA PHN;
MERGE PHN_H PHN_A PHN_W;
BY SSN;
RUN;

DATA DFR_FRB (DROP=DC_PHN HME_PHN);
SET DFR_FRB;
RUN;

PROC SORT DATA=PHN;BY SSN;RUN;
PROC SORT DATA=DFR_FRB NODUPRECS;BY SSN;RUN;

DATA DFR_FRB;
MERGE DFR_FRB PHN;
BY SSN;
RUN;

/*VARIABLE INITIALIZATION*/
DATA DFR_FRB;
SET DFR_FRB;
QUEUE = 'X2BS';
REGION = 'N';
LENDER_CODE = '        ';
CUR_SCL_CODE = '                    ';
PHN_IND = 'D';
REL = 'BORROWER';
REL_CODE = 'B';
GEO_PHN_IND = 'D';
BI_ATY_3_PTY = ' ';
FILLER = '                                                                                                                  ';
RUN;

/*CALCULATE DAYS LEFT TO COMPLETE*/
DATA DFR_FRB;
SET DFR_FRB;
IF WN_DAY_PRD_GOA_WRK NE . THEN DTC = WN_DAY_PRD_GOA_WRK;
ELSE IF LD_REQ_RSP_ATY_PRF NE . THEN DO;
	IF INTCK('DAY',TODAY(),LD_REQ_RSP_ATY_PRF) =< 0
	THEN DTC = 0;
	ELSE IF INTCK('DAY',TODAY(),LD_REQ_RSP_ATY_PRF) > 0
	THEN DTC = INTCK('DAY',TODAY(),LD_REQ_RSP_ATY_PRF);
END;
RUN;

*WRITE CURRENT RUN DATETIME FOR THIS JOB TO DATASET;
DATA DFR_FRB;
SET DFR_FRB;
FORMAT RUNDT DATETIME.;
RUNDT = DATETIME();
RUN;

PROC SQL;
CREATE TABLE DFR_FRB2 AS 
SELECT DISTINCT SSN
FROM DFR_FRB
WHERE WQ20_IND NE 'X';
QUIT;
RUN;

/*ONLY UNCOMMENT THIS IF YOU WANT TO OVERWRITE THE CYPRUS DATA SET FROM A LOCAL RUN!*/
/***********************************************************************************/
/*DATA WORKLOCL.DFR_FRB;*/
/*SET DFR_FRB;*/
/*RUN;*/
/*RSUBMIT;*/
/***********************************************************************************/
/*THIS MUST BE UNCOMMENTED FOR PRODUCTION*/
/***********************************************************************************/
*LIBNAME PROGRVW V8 '/sas/whse/progrevw'; /*CYPRUS DATA SET*/
/*DATA PROGRVW.ULWS12_LWS12R3;*/
/*SET DFR_FRB;*/
/*RUN;*/
/***********************************************************************************/
/*ENDRSUBMIT;*/
/***********************************************************************************/

/*FILE TO BUILD QUEUE*/
DATA _NULL_;
SET  WORK.DFR_FRB2;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT SSN 9.;
PUT SSN $ ;
RUN;

/*PROC EXPORT DATA=WORK.DFR_FRB*/
/*            OUTFILE= "C:\WINDOWS\TEMP\ULWS12_LWS12R3.XLS" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/
