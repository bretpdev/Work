/*

Metro II Credit Status for Bankputcy Loans

Lists bankruptcy loans with invalid collection costs and credit statuses.  Used to
monitor status settings on bankruptcy accounts which impact Metro II reporting.

*/


/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWQA2.LWQA2R2";*/

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT

	A.bf_ssn,
	a.af_apl_id||a.af_apl_id_sfx as clid,
	A.lc_pcl_rea,
	a.li_col_cst_ass,
	a.lc_crb_sta,
	a.lf_clm_id
FROM	OLWHRM1.DC01_LON_CLM_INF A
WHERE	A.lc_pcl_rea in ('BC', 'BH', 'BO') and
		a.lc_sta_dc10 = '03' and
		(a.li_col_cst_ass not in (' ', 'N') or
		 a.lc_crb_sta = '093')
order by lc_crb_sta, li_col_cst_ass, a.bf_ssn
);
DISCONNECT FROM DB2;
ENDRSUBMIT;

/*PROC PRINTTO PRINT=REPORT2;
RUN;*/
OPTIONS PAGENO=1 LS=80;
PROC PRINT NOOBS SPLIT='/' DATA=WORKLOCL.DEMO n='Number of Loans = ';

VAR bf_ssn
	lf_clm_id
	clid
	lc_pcl_rea
	li_col_cst_ass
	lc_crb_sta;

TITLE	'Metro II Credit Status for Bankputcy Loans';
RUN;
