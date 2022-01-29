/*	This report shows all loans where the Lender Payoff field in LC05
	has been populated and there is no reinsurance bill record for that loan
	(date requested in LC28 is blank).  mc
*/

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWA02.LWA02R2";*/

libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;
OPTIONS NOCENTER NODATE NONUMBER LS=80;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE A02 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	A.af_apl_id||A.af_apl_id_sfx	as CLUID,
		integer(A.bf_ssn)				as SSN,
		A.lf_clm_id						as CLMID,
		A.ld_ldr_pof					as POFFDT
		,B.ld_agy_bil_doe
FROM  OLWHRM1.DC01_LON_CLM_INF A left outer join OLWHRM1.DC19_DOE_RIN B
	on a.af_apl_id = b.af_apl_id
	and a.af_apl_id_sfx = b.af_apl_id_sfx
WHERE A.ld_ldr_pof is not null
and B.ld_agy_bil_doe is null
and A.lc_sta_dc10 in ('03','04')
);
DISCONNECT FROM DB2;

PROC SORT data=A02;
BY SSN CLMID;
RUN;
endrsubmit  ;

DATA A02; 
SET WORKLOCL.A02; 
RUN;

/*PROC PRINTTO PRINT=REPORT2;
RUN;*/

OPTIONS NOCENTER NODATE NONUMBER LS=80;
PROC PRINT NOOBS SPLIT='/' DATA=A02 WIDTH=UNIFORM WIDTH=MIN n='Count:  ';
VAR SSN CLUID CLMID POFFDT;
LABEL CLUID = 'Commonline Unique ID' CLMID = 'Claim ID'
POFFDT = 'Lender Payoff Date';
FORMAT POFFDT MMDDYY8. SSN SSN11.;
TITLE 'Payoff Date Populated, No Reinsurance';
FOOTNOTE  'JOB = UTLWA02     REPORT = ULWA02.LWA02R2';
RUN;
