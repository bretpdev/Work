/*  This one-time query prints one report which displays all loans with
	a GA10 Date Processed of 6/1/2001 or greater, an approved 'A' status
	and a guarantee reduction code of '03'.					mc 9/26/01
*/
libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;
OPTIONS NOCENTER NODATE NONUMBER LS=132;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT
	C.DF_PRS_ID												AS SSN1,
	RTRIM(C.dm_prs_1)||' '||RTRIM(C.dm_prs_lst)				AS NAME,
	A.af_apl_id||A.af_apl_id_sfx 							AS CLUID,
	A.ad_prc												AS PROCDT,
	A.ac_gte_rdc											AS REDUXCD,
	A.AC_PRC_STA											AS STATUS
FROM  OLWHRM1.GA10_LON_APP A JOIN OLWHRM1.GA01_APP B
ON A.af_apl_id = B.af_apl_id
JOIN OLWHRM1.PD01_PDM_INF C
ON C.DF_PRS_ID = B.df_prs_id_br
WHERE A.ad_prc >= '06-01-2001'
AND A.AC_PRC_STA = 'A'
AND A.ac_gte_rdc = '03'
);
DISCONNECT FROM DB2;
endrsubmit  ;

DATA DEMO;
SET WORKLOCL.DEMO;
SSN = INPUT(SSN1, 9.);
RUN;

PROC SORT data=DEMO;
BY PROCDT SSN;
RUN;

OPTIONS NOCENTER PAGENO=1 LS=120;
PROC PRINT DATA = DEMO NOOBS N='Count:  ' LABEL WIDTH=UNIFORM WIDTH=MIN;
VAR SSN NAME PROCDT CLUID;
FORMAT SSN SSN11. PROCDT MMDDYY8.;
LABEL NAME = 'Name' CLUID = 'Commonline Unique ID' PROCDT = 'Date Processed';
TITLE 'All Aggregate Limit Reduced Loans';
FOOTNOTE  'JOB = MANUAL     REPORT = All Agg Lim Reduced';
RUN;

