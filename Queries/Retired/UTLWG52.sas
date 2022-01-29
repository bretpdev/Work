/*UTLWG52*/
/*TWO-STEP CONSOLIDATION LOANS*/

LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWG52.LWG52R2";
/**/
/*LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;*/
/*RSUBMIT;*/

PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);

CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT LC10.DF_LCO_PRS_SSN_BR AS SSN
FROM OLWHRM1.LC10_UND_LN_INF LC10

WHERE LI_UND_LN_SER_ORG = 'N'
AND LC_UND_LN_PGM IN ('HEAL','DPLUS','PERK','DSS','DUS'
							,'SUBCNS','UNCNS','DSCON','DUCON','SUBSPC','UNSPC')
AND LI_UND_LN_CON = 'Y'
);

CREATE TABLE DEMO2 AS
SELECT *
FROM CONNECTION TO DB2 (

SELECT LN10.BF_SSN AS SSN
	,LN85.LN_ATY_SEQ
	,AY10.LD_REQ_RSP_ATY_PRF AS PRF2
	,DAYS(AY10.LD_REQ_RSP_ATY_PRF) AS DYS2
FROM	OLWHRM1.LN10_LON LN10
LEFT OUTER JOIN OLWHRM1.LN90_FIN_ATY LN90
ON LN10.BF_SSN = LN90.BF_SSN
AND LN10.LN_SEQ = LN90.LN_SEQ
LEFT OUTER JOIN OLWHRM1.LN85_LON_ATY LN85
ON LN10.BF_SSN = LN85.BF_SSN
AND LN10.LN_SEQ = LN85.LN_SEQ
LEFT OUTER JOIN OLWHRM1.AY10_BR_LON_ATY AY10
ON LN85.BF_SSN = AY10.BF_SSN
AND LN85.LN_ATY_SEQ = AY10.LN_ATY_SEQ
WHERE LN10.IC_LON_PGM IN ('SUBCNS','UNCNS')
AND LN90.LN_FAT_SEQ_REV IS NULL
AND LN90.PC_FAT_TYP = '01'
AND LN90.PC_FAT_SUB_TYP = '01'
AND LN90.LC_CSH_ADV = 'A'
AND AY10.PF_REQ_ACT IN ('OC308','OC318')
);


CREATE TABLE DEMO3 AS
SELECT *
FROM CONNECTION TO DB2 (

SELECT ZZ.BF_SSN AS SSN
	,ZZ.LN_ATY_SEQ
	,ZZ.LD_REQ_RSP_ATY_PRF AS PRF3
	,DAYS(ZZ.LD_REQ_RSP_ATY_PRF) AS DYS3
FROM OLWHRM1.AY10_BR_LON_ATY ZZ
WHERE ZZ.PF_REQ_ACT IN ('OC350')
);

DISCONNECT FROM DB2;
/*ENDRSUBMIT;*/
/*DATA DEMO; SET WORKLOCL.DEMO; RUN;*/
/*DATA DEMO2; SET WORKLOCL.DEMO2; RUN;*/
/*DATA DEMO3; SET WORKLOCL.DEMO3; RUN;*/

PROC SQL;
CREATE TABLE DEMO4 AS
SELECT * 
FROM DEMO2
LEFT OUTER JOIN DEMO3
ON DEMO2.SSN = DEMO3.SSN
AND DEMO2.LN_ATY_SEQ = DEMO3.LN_ATY_SEQ
WHERE NOT DEMO3.DYS3 > DEMO2.DYS2 
;
QUIT;

PROC SQL;
CREATE TABLE DEMO5 AS
SELECT DISTINCT DEMO4.SSN
FROM DEMO4
INNER JOIN DEMO
ON DEMO4.SSN = DEMO.SSN
;
QUIT;

PROC SORT DATA=DEMO5;
BY SSN;
RUN;

PROC PRINTTO PRINT=REPORT2;
RUN;
/*For landscape reports:*/
/*OPTIONS ORIENTATION = LANDSCAPE;*/
/*OPTIONS CENTER NODATE NUMBER PAGENO=1 PS=39 LS=127;*/

/*For portrait reports;*/
OPTIONS ORIENTATION = PORTRAIT;
OPTIONS CENTER NODATE NUMBER PAGENO=1 PS=52 LS=96;

PROC PRINT NOOBS SPLIT='/' DATA=DEMO5;
VAR SSN;
TITLE 'Two Step Consolidated Loans';
FOOTNOTE  'JOB = UTLWG52     REPORT = ULWG52.LWG52R2';
RUN;


/*PROC EXPORT DATA=WORKLOCL.DEMO*/
/*            OUTFILE= "T:\SAS\DEMO.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/