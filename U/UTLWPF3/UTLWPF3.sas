/*
NON GARN SMALL BALANCE

Report 2 lists accounts without loans in garnishment status with a balance > $25
and principal and interest < $25 and other costs (legal, other, coll costs) < $100.
Report 3 lists accounts with loans in garnishment status with principal and interest
< $25 and other costs (legal, other, coll costs) < $100.
Collections uses this report to identify small balance accounts so advices can be
submitted to zero the balance and expedite the satisfaction process.

*/

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWPF3.LWPF3R2";
FILENAME REPORT3 "&RPTLIB/ULWPF3.LWPF3R3";
*/

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE LNBALS AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	A.bf_ssn										AS SSN,
	A.af_apl_id||A.af_apl_id_sfx					AS CLID,
	RTRIM(B.DM_PRS_1)||' '||RTRIM(B.dm_prs_lst)		AS NAME,
	C.la_clm_bal									AS BAL,
	A.la_clm_pri + A.la_clm_int	- A.la_pri_col +
		A.la_int_acr + C.la_clm_int_acr -
		A.la_int_col								AS PI,
	A.la_leg_cst_acr - A.la_leg_cst_col +
		A.la_oth_chr_acr - A.la_oth_chr_col +
		A.la_col_cst_acr - A.la_col_cst_col + 
		C.la_clm_prj_col_cst						AS OTHER,
		A.lc_grn 									AS GARN

FROM	OLWHRM1.DC01_LON_CLM_INF A INNER JOIN
		OLWHRM1.PD01_PDM_INF B ON
			A.bf_ssn = B.df_prs_id AND
			A.lc_sta_dc10 = '03' AND
			A.ld_clm_asn_doe IS NULL INNER JOIN
		OLWHRM1.DC02_BAL_INT C ON
			A.af_apl_id = C.af_apl_id AND
			A.af_apl_id_sfx = C.af_apl_id_sfx
);

DISCONNECT FROM DB2;

PROC SQL;
CREATE TABLE ACBALS AS
SELECT DISTINCT
	SSN,
	NAME,
	SUM(BAL)	AS BAL,
	SUM(PI)		AS PI,
	SUM(OTHER)	AS OTHER,
	GARN
FROM LNBALS
GROUP BY SSN, NAME, GARN;

PROC SQL;
CREATE TABLE SMBAL AS
SELECT DISTINCT *
FROM ACBALS
WHERE	BAL > 25 AND
		PI < 25 AND
		OTHER <100 AND
		GARN NOT IN ('06', '07');
PROC SQL;
CREATE TABLE SMBALGL AS
SELECT DISTINCT *
FROM ACBALS
WHERE	PI < 25 AND
		OTHER <100 AND
		GARN IN ('06', '07');
RUN;
ENDRSUBMIT;

/*PROC PRINTTO PRINT=REPORT2;
RUN;*/
OPTIONS CENTER PAGENO=1 LS=80;
PROC PRINT NOOBS SPLIT='/' DATA=WORKLOCL.SMBAL WIDTH=MIN N='NUMBER OF ACCOUNTS = ';
VAR SSN
	NAME
	BAL
	PI
	OTHER;
LABEL	BAL = 'TOTAL BALANCE'
		PI = 'PRINCIPAL AND INTEREST'
		OTHER = 'OTHER COSTS';
FORMAT BAL DOLLAR10. PI DOLLAR10. OTHER DOLLAR10.;
TITLE1 'OPEN SMALL BALANCE ACCOUNTS';
TITLE2 'NO GARNISHMENT';
FOOTNOTE 'JOB = UTLWPF3     REPORT = ULWPF3.LWPF3R2';
RUN;

/*PROC PRINTTO PRINT=REPORT3;
RUN;*/
OPTIONS CENTER PAGENO=1 LS=80;
PROC PRINT NOOBS SPLIT='/' DATA=WORKLOCL.SMBALGL WIDTH=MIN N='NUMBER OF ACCOUNTS = ';
VAR SSN
	NAME
	BAL
	PI
	OTHER;
LABEL	BAL = 'TOTAL BALANCE'
		PI = 'PRINCIPAL AND INTEREST'
		OTHER = 'OTHER COSTS';
FORMAT BAL DOLLAR10. PI DOLLAR10. OTHER DOLLAR10.;
TITLE1 'OPEN SMALL BALANCE ACCOUNTS';
TITLE2 'WITH GARNISHMENT';
FOOTNOTE 'JOB = UTLWPF3     REPORT = ULWPF3.LWPF3R3';
RUN;
