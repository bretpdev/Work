*---------------------------------------*
| UTLWA22 Monthly Rolling Forecast Data |
*---------------------------------------*;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWA22.LWA22R2";
FILENAME REPORTZ "&RPTLIB/ULWA22.LWA22RZ";
/*************** DEFINE DATE RANGE ***************/
%LET BEGIN = '07/01/2008';
%LET END = '07/01/2009';
/*************************************************/
%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT END = &END;
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
CREATE TABLE MRFD AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT A.BF_SSN
	,A.LN_SEQ
	,A.LD_LON_1_DSB
	,A.IC_LON_PGM
	,A.LA_LON_AMT_GTR
	,A.LC_ACA_GDE_LEV
	,B.LN_BR_DSB_SEQ
	,B.LN_LON_DSB_SEQ
	,B.LA_DSB
	,COALESCE(B.LA_DSB_CAN,0) AS LA_DSB_CAN
	,B.LD_DSB
	,B.LC_STA_LON15
	,B.LC_DSB_TYP
FROM OLWHRM1.LN10_LON A
INNER JOIN OLWHRM1.LN15_DSB B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
WHERE A.LC_STA_LON10 = 'R'
	AND A.LA_CUR_PRI > 0
	AND A.LD_LON_1_DSB BETWEEN &BEGIN AND &END
	AND A.IC_LON_PGM IN 
	(
		'STFFRD','UNSTFD','PLUS','PLUSGB'
	)
	AND B.LC_STA_LON15 IN 
	(
		'1','3'
	)
	AND B.LC_DSB_TYP = '2'
	AND NOT EXISTS (
		SELECT 1
		FROM OLWHRM1.LN15_DSB Y
		WHERE Y.BF_SSN = A.BF_SSN
			AND Y.LN_SEQ = A.LN_SEQ
			AND Y.LD_DSB > '09/30/2009'
			AND Y.LC_STA_LON15 IN ('1','3')
			AND Y.LC_DSB_TYP = '1'
			AND 
			(
					Y.LA_DSB_CAN IS NULL OR 
					Y.LA_DSB_CAN <> Y.LA_DSB
			)
		)
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWA22.LWA22RZ);*/
/*QUIT;*/
ENDRSUBMIT;
DATA MRFD ;
	SET WORKLOCL.MRFD;
RUN;
PROC SORT DATA=MRFD OUT=MRFD;
	BY BF_SSN LN_SEQ LD_LON_1_DSB LD_DSB;
RUN;
/*****************************************************************
* INITIALIZE VARIABLES
******************************************************************/
DATA MRFD ;
	SET MRFD;
	BY BF_SSN LN_SEQ LD_LON_1_DSB;
	IF FIRST.LN_SEQ THEN DO;
		LA_LON_AMT_GTR = LA_LON_AMT_GTR;
		MO_1_NUM = 1;
	END;
	ELSE DO;
		LA_LON_AMT_GTR = 0;
		MO_1_NUM = 0;
	END;
	TPMON = MONTH(LD_DSB);
	TPYEAR = YEAR(LD_DSB);
	MON_1ST_DSB = MONTH(LD_LON_1_DSB);
	YR_1ST_DSB = YEAR(LD_LON_1_DSB);
	NET_DISB = LA_DSB - LA_DSB_CAN;
	SELECT;
		WHEN
		(
			IC_LON_PGM IN ('STFFRD') 
			AND LC_ACA_GDE_LEV IN ('1','2','3','4','5')
		) IC_LON_PGM = 'STAFFU';
		WHEN
		(
			IC_LON_PGM IN ('STFFRD') 
			AND LC_ACA_GDE_LEV IN ('A','B','C','D')
		) IC_LON_PGM = 'STAFFG';
		OTHERWISE IC_LON_PGM = IC_LON_PGM;
	END;
	DO;
		IF NET_DISB = 0 THEN
			DELETE;
		ELSE 
			OUTPUT ;
	END;	
RUN;
/*****************************************************************
* CREATE TABLE THAT CONTAINS A ROW FOR EACH MONTH AND LOAN TYPE 
* FROM AUGUST 2008 TO SEPTEMBER 2009
******************************************************************/
DATA TPS (DROP=START I1 I2 TPBEG);
	START = '01AUG2008'D;
	DO I1=0 TO 13;		
		TPBEG = INTNX('MONTH',START,I1,'B');
		TPMON = MONTH(TPBEG);
		TPYEAR = YEAR(TPBEG);
		DO I2=1 TO 5;
			SELECT(I2);
				WHEN (1) IC_LON_PGM = 'STAFFU';
				WHEN (2) IC_LON_PGM = 'STAFFG';
				WHEN (3) IC_LON_PGM = 'UNSTFD';
				WHEN (4) IC_LON_PGM = 'PLUS';
				WHEN (5) IC_LON_PGM = 'PLUSGB';
			END;
			OUTPUT;
		END;
	END;
RUN;
/*****************************************************************
* PUT EVERYTHING TOGETHER, NOTE THE JOIN CONDITIONS
******************************************************************/
PROC SQL;
CREATE TABLE TPS_REPORT AS
SELECT DISTINCT A.TPMON
	,A.TPYEAR
	,A.IC_LON_PGM
	,COALESCE(B.DISB_COUNT,0) AS DISB_COUNT
	,COALESCE(C.NET_DISB,0) AS NET_DISB
	,COALESCE(D.MONTHLY_1ST_DISB,0) AS MONTHLY_1ST_DISB
	,COALESCE(E.OG_TOT,0) AS OG_TOT
FROM TPS A
LEFT OUTER JOIN ( /*# DISBURSED*/
	SELECT TPMON
		,TPYEAR
		,IC_LON_PGM
		,COUNT(*) AS DISB_COUNT
	FROM MRFD 
	GROUP BY TPMON
		,TPYEAR
		,IC_LON_PGM
	) B
	ON A.TPMON = B.TPMON
	AND A.TPYEAR = B.TPYEAR
	AND A.IC_LON_PGM = B.IC_LON_PGM
LEFT OUTER JOIN ( /*$ DISBURSED*/
	SELECT TPMON
		,TPYEAR
		,IC_LON_PGM
		,SUM(NET_DISB) AS NET_DISB
	FROM MRFD 
	GROUP BY TPMON
		,TPYEAR
		,IC_LON_PGM
	) C
	ON A.TPMON = C.TPMON
	AND A.TPYEAR = C.TPYEAR
	AND A.IC_LON_PGM = C.IC_LON_PGM
LEFT OUTER JOIN ( /*# 1ST DISBURSED*/
	SELECT MON_1ST_DSB
		,YR_1ST_DSB
		,IC_LON_PGM
		,SUM(MO_1_NUM) AS MONTHLY_1ST_DISB
	FROM MRFD 
	GROUP BY MON_1ST_DSB
		,YR_1ST_DSB
		,IC_LON_PGM
	) D
	ON A.TPMON = D.MON_1ST_DSB
	AND A.TPYEAR = D.YR_1ST_DSB
	AND A.IC_LON_PGM = D.IC_LON_PGM
LEFT OUTER JOIN ( /*$ ORIGINATED*/
	SELECT MON_1ST_DSB
		,YR_1ST_DSB
		,IC_LON_PGM
		,SUM(LA_LON_AMT_GTR) AS OG_TOT
	FROM MRFD 
	GROUP BY MON_1ST_DSB
		,YR_1ST_DSB
		,IC_LON_PGM
	) E
	ON A.TPMON = E.MON_1ST_DSB
	AND A.TPYEAR = E.YR_1ST_DSB
	AND A.IC_LON_PGM = E.IC_LON_PGM
ORDER BY A.TPYEAR
	,A.TPMON
	,A.IC_LON_PGM
;
QUIT;
/*****************************************************************
* CREATE PRODUCTION REPORT
******************************************************************/
DATA _NULL_;
	SET WORK.TPS_REPORT;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
	FORMAT TPMON BEST12. ;
	FORMAT TPYEAR BEST12. ;
	FORMAT IC_LON_PGM $6. ;
	FORMAT DISB_COUNT BEST12. ;
	FORMAT NET_DISB 20.2 ;
	FORMAT MONTHLY_1ST_DISB BEST12. ;
	FORMAT OG_TOT 20.2 ;
	IF _N_ = 1 THEN DO;
		PUT 'IC_LON_PGM,YEAR,MONTH,DISB_COUNT,AMOUNT_DISB,MONTHLY_1ST_DISB,OG_TOT';
	END;
	DO;
		PUT IC_LON_PGM $ @;
		PUT TPYEAR @;
		PUT TPMON @;
		PUT DISB_COUNT @;
		PUT NET_DISB @;
		PUT MONTHLY_1ST_DISB @;
		PUT OG_TOT ;
	END;
RUN;
/*****************************************************************
* DETAIL FILE
******************************************************************/
DATA _NULL_;
	SET  WORK.MRFD ;
	FILE 'T:\SAS\UTLWA22.R2.Detail.txt' DELIMITER=',' DSD DROPOVER LRECL=32767;
	FORMAT BF_SSN $9. ;
	FORMAT LN_SEQ 6. ;
	FORMAT LD_LON_1_DSB DATE9. ;
	FORMAT IC_LON_PGM $6. ;
	FORMAT LA_LON_AMT_GTR 10.2 ;
	FORMAT LC_ACA_GDE_LEV $2. ;
	FORMAT LN_BR_DSB_SEQ 6. ;
	FORMAT LN_LON_DSB_SEQ 6. ;
	FORMAT LA_DSB 10.2 ;
	FORMAT LA_DSB_CAN 15.2 ;
	FORMAT LD_DSB DATE9. ;
	FORMAT LC_STA_LON15 $1. ;
	FORMAT LC_DSB_TYP $1. ;
	FORMAT TPMON BEST12. ;
	FORMAT TPYEAR BEST12. ;
	FORMAT MON_1ST_DSB BEST12. ;
	FORMAT YR_1ST_DSB BEST12. ;
	FORMAT NET_DISB BEST12. ;
	IF _N_ = 1 THEN DO;
	PUT 'BF_SSN'
		','
		'LN_SEQ'
		','
		'LD_LON_1_DSB'
		','
		'IC_LON_PGM'
		','
		'LA_LON_AMT_GTR'
		','
		'LC_ACA_GDE_LEV'
		','
		'LN_BR_DSB_SEQ'
		','
		'LN_LON_DSB_SEQ'
		','
		'LA_DSB'
		','
		'LA_DSB_CAN'
		','
		'LD_DSB'
		','
		'LC_STA_LON15'
		','
		'LC_DSB_TYP'
		','
		'TPMON'
		','
		'TPYEAR'
		','
		'MON_1ST_DSB'
		','
		'YR_1ST_DSB'
		','
		'AMOUNT_DISB';
	END;
	DO;   
		PUT BF_SSN $ @;
		PUT LN_SEQ @;
		PUT LD_LON_1_DSB @;
		PUT IC_LON_PGM $ @;
		PUT LA_LON_AMT_GTR @;
		PUT LC_ACA_GDE_LEV $ @;
		PUT LN_BR_DSB_SEQ @;
		PUT LN_LON_DSB_SEQ @;
		PUT LA_DSB @;
		PUT LA_DSB_CAN @;
		PUT LD_DSB @;
		PUT LC_STA_LON15 $ @;
		PUT LC_DSB_TYP $ @;
		PUT TPMON @;
		PUT TPYEAR @;
		PUT MON_1ST_DSB @;
		PUT YR_1ST_DSB @;
		PUT NET_DISB ;
	END;
RUN;
