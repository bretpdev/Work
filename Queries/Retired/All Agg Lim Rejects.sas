/*  This one-time query prints one report which displays all loans with
	a GA10 Date Processed of 6/1/2001 or greater and a reject
	code of 'G'.									mc 9/26/01
*/
libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;
OPTIONS NOCENTER NODATE NONUMBER LS=132;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE REJECTS AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT
	c.DF_PRS_ID												AS SSN1,
	RTRIM(c.dm_prs_1)||' '||RTRIM(c.dm_prs_lst)				AS NAME,
	c.df_zip												AS ZIP,
	a.af_apl_id||a.af_apl_id_sfx 							AS CLUID,
	a.ad_prc												AS PROCDT,
	a.AC_APL_REJ_REA_1										AS REJ1,
	a.AC_APL_REJ_REA_2										AS REJ2,
	a.AC_APL_REJ_REA_3										AS REJ3,
	a.AC_APL_REJ_REA_4										AS REJ4,
	a.AC_APL_REJ_REA_5										AS REJ5
FROM  OLWHRM1.GA10_LON_APP a JOIN OLWHRM1.GA01_APP b
ON a.af_apl_id = b.af_apl_id
JOIN OLWHRM1.PD01_PDM_INF c
ON c.DF_PRS_ID = b.df_prs_id_br
WHERE a.ad_prc >= '06-01-2001'
AND (a.AC_APL_REJ_REA_1 = 'G'
OR a.AC_APL_REJ_REA_2 = 'G'
OR a.AC_APL_REJ_REA_3 = 'G'
OR a.AC_APL_REJ_REA_4 = 'G'
OR a.AC_APL_REJ_REA_5 = 'G')
);
DISCONNECT FROM DB2;
endrsubmit  ;

DATA REJECTS;
SET WORKLOCL.REJECTS;
RUN;

DATA REJECTS (DROP = ZIP);
SET REJECTS;
ZIP5 = SUBSTR(ZIP,1,5);
ZIP4 = SUBSTR(ZIP,6,4);
SSN = INPUT(SSN1, 9.);
FORMAT PROCDT MMDDYY8. SSN SSN11.;
RUN;

PROC SORT DATA = REJECTS;
BY ZIP5 ZIP4;
RUN;

DATA REJECTS (DROP = ZIP5 ZIP4);
SET REJECTS;
IF ZIP4 <> ' ' THEN ZIP = TRIM(ZIP5)||"-"||TRIM(ZIP4);
ELSE ZIP = ZIP5;
RUN;

OPTIONS NOCENTER DATE NUMBER PAGENO=1 LS=120;
PROC PRINT DATA = REJECTS NOOBS N='Total:  ' SPLIT='/' WIDTH = MIN WIDTH=UNIFORM;
VAR ZIP SSN NAME PROCDT CLUID;
LABEL ZIP = 'ZIP Code' 
NAME = 'Name' PROCDT = 'Date Processed' CLUID = 'Commonline Unique ID';
TITLE 'All Aggregate Limit Rejects';
FOOTNOTE  'JOB = MANUAL     REPORT = All Agg Lim Rejects';
RUN;