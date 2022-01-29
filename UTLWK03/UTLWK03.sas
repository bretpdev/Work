/*UTLWK03*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
/*FILENAME REPORT2 "&RPTLIB/ULWK03.LWK03R2";*/

FILENAME REPORT2 'T:\SAS\ULWK03.LWK03R2';

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
options symbolgen;
DATA _NULL_;		/*GETS THE PREVIOUS DAY*/
	EFFDT = TODAY() - 1;
    CALL SYMPUT('EFFDATE',put(EFFDT,MMDDYY10.));
	CALL SYMPUT('RUNDATE',"'"||put(EFFDT,MMDDYY10.)||"'");
RUN;

%SYSLPUT RUNDATE = &RUNDATE;	
RSUBMIT;
/*ONELINK*/
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DEMOO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT							
	DISTINCT
	A.DF_PRS_ID AS SSN
	,A.DM_PRS_LST AS LASTNAME
	,B.BF_LST_USR_AY01 AS USERID
FROM OLWHRM1.PD01_PDM_INF A 
INNER JOIN OLWHRM1.AY01_BR_ATY B
	ON A.DF_PRS_ID = B.DF_PRS_ID
WHERE (A.DI_PHN_VLD = 'N'
	   OR A.DI_VLD_ADR = 'N') 
AND B.BC_ATY_TYP IN ('TC','TE')
AND B.BC_ATY_CNC_TYP IN ('03','04')
AND B.BD_ATY_PRF = &RUNDATE 
AND B.BF_LST_USR_AY01 LIKE 'UT%'
);


/*COMPASS*/
CREATE TABLE DEMOC AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT
	DISTINCT
	P.DF_PRS_ID AS SSN
	,P.DM_PRS_LST AS LASTNAME
	,S.LF_PRF_BY AS USERID
FROM OLWHRM1.PD10_PRS_NME P
INNER JOIN OLWHRM1.PD42_PRS_PHN Q
	ON P.DF_PRS_ID = Q.DF_PRS_ID
INNER JOIN OLWHRM1.PD30_PRS_ADR R
	ON Q.DF_PRS_ID = R.DF_PRS_ID
INNER JOIN OLWHRM1.AY10_BR_LON_ATY S
	ON P.DF_PRS_ID = S.BF_SSN
INNER JOIN OLWHRM1.AC10_ACT_REQ T
	ON S.PF_REQ_ACT = T.PF_REQ_ACT

WHERE S.PF_RSP_ACT = 'CNTCT'
AND LD_ATY_REQ_RCV = &RUNDATE
AND T.PC_TYP_REQ_ACT = 'C'
AND T.PC_REQ_ACT_RCP = 'B'
AND Q.DC_PHN = 'H'
AND (Q.DI_PHN_VLD = 'N'
	 OR R.DI_VLD_ADR = 'N')
AND S.LF_PRF_BY LIKE 'UT%'
);

DISCONNECT FROM DB2;
ENDRSUBMIT;

PROC SORT DATA=WORKLOCL.DEMOO;
BY USERID;
RUN;
PROC SORT DATA=WORKLOCL.DEMOC;
BY USERID;
RUN;
/*PROC PRINTTO PRINT=REPORT2;
RUN;*/
OPTIONS PAGENO=1 LS=80;
PROC PRINT NOOBS SPLIT='/' DATA=WORKLOCL.DEMOO;
BY USERID;
TITLE	"Locate Services, SKIP Borrower Contact-No Demo Update";
TITLE2 "OneLink";
TITLE3  "For &EFFDATE";
FOOTNOTE  'JOB = UTLWK03     REPORT = ULWF01.LWK03R2';
RUN;
PROC PRINT NOOBS SPLIT='/' DATA=WORKLOCL.DEMOC;
BY USERID;
TITLE	"Locate Services, SKIP Borrower Contact-No Demo Update";
TITLE2 "Compass";
TITLE3  "For &EFFDATE";
FOOTNOTE  'JOB = UTLWK03     REPORT = ULWF01.LWK03R2';
RUN;

/*PROC EXPORT DATA=WORKLOCL.DEMOV*/
/*            OUTFILE= "T:\SAS\DEMO.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/

proc printto;
run;