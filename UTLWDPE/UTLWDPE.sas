/*	This report prints the total number of DAAR cure totals
    for the previous month, with total dollar amount, broken
	down by level of delinquency.
*/

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
/*FILENAME REPORT2 "&RPTLIB/ULWDPE.LWDPER2";*/


libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;
OPTIONS NOCENTER NODATE NONUMBER LS=132 SYMBOLGEN;

DATA _NULL_;
     CALL SYMPUT('BEGIN',"'"||PUT(INTNX('MONTH',TODAY(),-1,'beginning'), MMDDYYD10.)||"'");
     CALL SYMPUT('END',"'"||PUT(INTNX('MONTH',TODAY(),-1,'end'), MMDDYYD10.)||"'");
RUN;

PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	
		A.bf_ssn							as SSN,
		A.af_apl_id||A.af_apl_id_sfx		as CLUID,
		A.ld_pcl_rcv						as PCLRCV,
		A.la_clm_pri + A.la_clm_int			as TOTDFL,
		CASE
			WHEN A.lc_clm_lon_typ IN ('SF','SU') 
				THEN 'S'
				ELSE A.lc_clm_lon_typ END 	as LONTYP,
		CASE
			WHEN A.ld_org_dco is not null 
				THEN A.ld_org_dco
				ELSE A.ld_dco END			as DCODT,
		A.lf_cur_lon_ser_agy				as CURSER,
		A.ld_sta_upd_dc10					as STADT
FROM  OLWHRM1.DC01_LON_CLM_INF A
WHERE A.LC_PCL_REA IN ('DF','DQ')
AND A.lc_sta_dc10 = '02'
AND A.ld_sta_upd_dc10 BETWEEN &BEGIN AND &END
ORDER BY A.BF_SSN, A.af_apl_id||A.af_apl_id_sfx
);
DISCONNECT FROM DB2;

DATA DEMO;
SET DEMO;
DELDAYS = STADT - PCLRCV;
IF DELDAYS >= 0 AND DELDAYS <= 30 THEN INTERVAL = '000-030';
ELSE IF DELDAYS >= 31 AND DELDAYS <= 60 THEN INTERVAL = '031-060';
ELSE IF DELDAYS >= 61 AND DELDAYS <= 90 THEN INTERVAL = '061-090';
ELSE IF DELDAYS >= 91 AND DELDAYS <= 120 THEN INTERVAL = '091-120';
ELSE IF DELDAYS >= 121 AND DELDAYS <= 150 THEN INTERVAL = '121-150';
ELSE IF DELDAYS >= 151 AND DELDAYS <= 180 THEN INTERVAL = '151-180';
ELSE IF DELDAYS >= 181 AND DELDAYS <= 210 THEN INTERVAL = '181-210';
ELSE IF DELDAYS >= 211 AND DELDAYS <= 240 THEN INTERVAL = '211-240';
ELSE IF DELDAYS >= 241 AND DELDAYS <= 270 THEN INTERVAL = '241-270';
ELSE IF DELDAYS >= 271 AND DELDAYS <= 300 THEN INTERVAL = '271-300';
ELSE IF DELDAYS >= 301 AND DELDAYS <= 330 THEN INTERVAL = '301-330';
ELSE IF DELDAYS >= 331 AND DELDAYS <= 360 THEN INTERVAL = '331-360';
ELSE IF DELDAYS > 360 THEN INTERVAL = '360+';
ELSE INTERVAL = 'OTHER*';
RUN;

PROC SQL;
CREATE TABLE DEMOLON AS
SELECT 	INTERVAL, 
		COUNT(INTERVAL) AS LONCNT,
		SUM(TOTDFL)		AS TOTDFL
FROM DEMO
GROUP BY INTERVAL;

CREATE TABLE DEMOPCA AS
SELECT 	INTERVAL,
		COUNT(INTERVAL) AS PCACNT
FROM 
	(SELECT DISTINCT  	INTERVAL, 
						SSN, 
						LONTYP, 
						PCLRCV, 
						DCODT, 
						CURSER
	FROM DEMO A)
GROUP BY INTERVAL;

CREATE TABLE DEMOSUM AS
SELECT A.*,B.PCACNT /*"*"*/
FROM DEMOLON A JOIN DEMOPCA B
ON A.INTERVAL = B.INTERVAL;
QUIT;

endrsubmit  ;

DATA DEMOSUM; 
SET WORKLOCL.DEMOSUM; 
RUN;

OPTIONS CENTER NODATE NONUMBER LS=120 SYMBOLGEN;
DATA _NULL_;
     EFFMO = PUT(INTNX('MONTH',TODAY(),-1), MONNAME9.);
	 EFFYR = PUT(INTNX('MONTH',TODAY(),-1), YEAR4.);
     CALL SYMPUT('EFFDATE',EFFMO||' '||EFFYR);
RUN;
PROC PRINTTO PRINT=REPORT2;
RUN;
PROC PRINT DATA = DEMOSUM NOOBS LABEL SPLIT='/';
VAR INTERVAL LONCNT PCACNT TOTDFL;
SUM LONCNT PCACNT TOTDFL ;
LABEL INTERVAL= 'Days in Default Aversion'
LONCNT= 'Number of Loans' PCACNT= 'Number of PCAs'
TOTDFL= 'Total Dollar Amount';
FORMAT LONCNT COMMA8. PCACNT COMMA8. TOTDFL DOLLAR15.2;
TITLE "Monthly DAAR Cure Totals - &EFFDATE";
FOOTNOTE 'JOB = UTLWDPE     REPORT = ULWDPE.LWDPER2 (55)';
RUN;
