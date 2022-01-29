/*****************************************
* TILP TEACHING CREDIT
******************************************/
%LET RPTLIB = Q:\Support Services\Test Files\SAS;
FILENAME CACTUS "&RPTLIB/TILP Teaching Credit SOE File 12-12.csv";
FILENAME REPORT2 "&RPTLIB/TILPTeachingCredit.R2.txt";
DATA SOE (KEEP=CACTUS CONHRS TITLE1 PRCNT);
	INFILE CACTUS dlm=',' missover dsd lrecl=76000 firstobs=2;
	/*****************************************
	* Note when running this in the past we have had to move the cactus column to column C and insert dummy data into Column A
	******************************************/
	INFORMAT CACTUS $6. V2 $1. V3 $1. V4 $1. CONHRS BEST12. V8 $1. v7 mmddyy10. TITLE1 BEST12.;
	INPUT V2 $ V3 $ CACTUS $ V4 $ CONHRS V6 V7 V8 $ TITLE1;
	IF TITLE1 = 1 THEN DO;
		PRCNT = ROUND(CONHRS/720,.01) * 2;
	END;
	ELSE DO;
		PRCNT = ROUND(CONHRS/720,.01);
	END;
RUN;
/*GET COMPASS DATA*/

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT DUSTER;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
PROC SQL;
CREATE TABLE TOP_DOWN AS
SELECT B.BF_SSN
	,B.LN_SEQ
	,B.LA_CUR_PRI
	,SUM(ABS(COALESCE(LA_FAT_CUR_PRI,0))) AS LA_FAT_CUR_PRI
FROM OLWHRM1.LN10_LON B
LEFT OUTER join OLWHRM1.LN90_FIN_ATY A
	ON B.BF_SSN = A.BF_SSN
	AND B.LN_SEQ = A.LN_SEQ
	AND A.PC_FAT_TYP = '10'
	AND A.PC_FAT_SUB_TYP = '54'
	AND A.LC_FAT_REV_REA = ''
	AND A.LC_STA_LON90 = 'A'
WHERE B.IC_LON_PGM = 'TILP'
	AND B.LC_STA_LON10 = 'R'
/*	AND B.LA_CUR_PRI > 0 */
GROUP BY B.BF_SSN;

CREATE TABLE ARC_TEXT AS
SELECT DISTINCT A.BF_SSN
	,A.PF_REQ_ACT
	,SUBSTR(C.LX_ATY,1,9) AS LX_ATY
FROM  OLWHRM1.AY10_BR_LON_ATY A
INNER JOIN OLWHRM1.AY15_ATY_CMT B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_ATY_SEQ = B.LN_ATY_SEQ
INNER JOIN OLWHRM1.AY20_ATY_TXT C
	ON B.BF_SSN = C.BF_SSN
	AND B.LN_ATY_SEQ = C.LN_ATY_SEQ
	AND B.LN_ATY_CMT_SEQ = C.LN_ATY_CMT_SEQ
WHERE A.PF_REQ_ACT IN ('CACTS','OTLTC')	
	AND A.LC_STA_ACTY10 = 'A';

quit;

PROC SORT DATA=ARC_TEXT; BY BF_SSN DESCENDING PF_REQ_ACT; RUN;
DATA ARC_TEXT(DROP=PF_REQ_ACT LX_ATY CACTS);
SET ARC_TEXT;
LENGTH CACTS $9.;
BY BF_SSN;
RETAIN OTLTC ;
IF FIRST.BF_SSN THEN OTLTC = 0;
IF PF_REQ_ACT = 'OTLTC' THEN OTLTC + INPUT(LX_ATY,9.2);
IF PF_REQ_ACT = 'CACTS' THEN DO;
	CACTUS = substr(lx_aty,1,6);
	CACTS = SCAN(LX_ATY,1,' ');
	IF ANYDIGIT(CACTS) = 0 THEN DELETE;
	CACTUS = substr(lx_aty,1,6);
	OUTPUT;
END;
RUN;
PROC SORT DATA=ARC_TEXT NODUPKEY; BY BF_SSN CACTUS; RUN;

PROC SQL;
CREATE TABLE GCID AS 
SELECT D.DF_SPE_ACC_ID
	,D.DM_PRS_LST
	,A.CACTUS
	,B.LN_SEQ
	,B.LA_CUR_PRI
	,OTLTC + LA_FAT_CUR_PRI AS OFSUM
	,C.LD_DSB
	,C.LA_DSB
FROM ARC_TEXT A
INNER JOIN TOP_DOWN B
	ON A.BF_SSN = B.BF_SSN
INNER JOIN OLWHRM1.PD10_PRS_NME D
	ON A.BF_SSN = D.DF_PRS_ID
INNER JOIN OLWHRM1.LN15_DSB C
	ON B.BF_SSN = C.BF_SSN
	AND B.LN_SEQ = C.LN_SEQ;
QUIT;
ENDRSUBMIT;
DATA GCID;
SET DUSTER.GCID;
RUN;

PROC SORT DATA=GCID; BY CACTUS; RUN;
proc sort data=soe; by CACTUS; run;
data gcid;
merge gcid(in=a) soe(in=b);
by cactus;
if a and b;
run;
/*PROC SQL;*/
/*CREATE TABLE GCID AS*/
/*SELECT A.**/
/*FROM GCID A*/
/*INNER JOIN SOE B*/
/*ON A.CACTUS=B.CACTUS;*/
/*QUIT;*/

PROC SORT DATA=GCID NODUPKEY; 
	BY DF_SPE_ACC_ID LD_DSB LN_SEQ;
RUN;
DATA GCID;
	SET GCID;
	BY DF_SPE_ACC_ID LD_DSB LN_SEQ;
	RETAIN OFSUM1;
	IF FIRST.DF_SPE_ACC_ID THEN DO;
		OFSUM1 = OFSUM-LA_DSB;
		OFSUM = OFSUM1;
	END;
	ELSE DO;
		OFSUM = OFSUM1-LA_DSB;
		OFSUM1 = OFSUM;
	END;
	IF OFSUM < 0 THEN 
		ELIMINATED = 0;
	ELSE
		ELIMINATED = 1;
RUN;
DATA GCID;
	SET GCID;
	BY DF_SPE_ACC_ID DESCENDING ELIMINATED;
	IF FIRST.DF_SPE_ACC_ID THEN 
		PSEQ=0;
	IF ELIMINATED = 0 AND LA_CUR_PRI > 0 THEN DO;
		PSEQ + 1;
	END;
	ELSE DELETE;
/*		PSEQ = 0;*/
RUN;
DATA P2L (DROP=UB);
	SET SOE;
	WHERE PRCNT ^= . ;
	UB = CEIL(PRCNT);
	DO PSEQ=1 TO UB ;
		RETAIN TPRCNT;
		IF PSEQ=1 THEN 
			TPRCNT = PRCNT;
		ELSE 
			TPRCNT = TPRCNT - 1; 
		OUTPUT;
	END;
RUN;
DATA P2L;
	SET P2L;
	IF TPRCNT < 1 THEN 
		TPRCNT = TPRCNT;
	ELSE 
		TPRCNT = 1;
RUN;
/*GET AMOUNT TO APPLY*/
PROC SQL;
CREATE TABLE APAMT AS 
	SELECT AA.CACTUS
		,SUM(AA.APA) AS APA
	FROM (
		SELECT A.CACTUS
			,A.PSEQ
			,A.TPRCNT  
			,B.LD_DSB
			,B.LA_DSB
			,A.TPRCNT * B.LA_DSB AS APA
		FROM P2L A
		INNER JOIN GCID B
			ON A.CACTUS = B.CACTUS
			AND A.PSEQ = B.PSEQ
		) AA
	GROUP BY AA.CACTUS;
/*PUT DATA TOGETHER AND PROCESS*/
CREATE TABLE PRC_GCID AS
	SELECT A.*
		,B.APA
	FROM GCID A
	INNER JOIN APAMT B
		ON A.CACTUS = B.CACTUS
	WHERE A.LA_CUR_PRI > 0
	ORDER BY DF_SPE_ACC_ID
		,LD_DSB
		,LN_SEQ;
QUIT;
DATA PRC_GCID;
	SET PRC_GCID;
	BY DF_SPE_ACC_ID LD_DSB LN_SEQ;
	RETAIN APA1;
	IF FIRST.DF_SPE_ACC_ID THEN DO;
		APA1 = APA-LA_CUR_PRI;
	END;
	ELSE DO;
		APA = APA1;
		APA1 = APA-LA_CUR_PRI;
	END;
	PTYPE = '1054';
	PMT_EFF_DT = TODAY();
	IF APA1 > 0 THEN WOA = LA_CUR_PRI;
	ELSE IF APA1 <= 0 AND APA > 0 THEN WOA = APA;
	IF APA1 > 0 OR APA > 0 THEN OUTPUT;
RUN;
DATA _NULL_;
SET PRC_GCID;
WHERE round(WOA,.01) > 0;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
   FORMAT PMT_EFF_DT MMDDYY10. ;
   FORMAT WOA 10.2 ;
DO;
   PUT DF_SPE_ACC_ID @;
   PUT PTYPE @;
   PUT LN_SEQ @;
   PUT WOA @;
   PUT PMT_EFF_DT @;
   PUT DM_PRS_LST $ ;
;
END;
RUN;