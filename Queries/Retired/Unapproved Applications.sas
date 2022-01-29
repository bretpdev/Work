/* lists unapproved applications

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWAB1.LWAB1R2";*/

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT
	A.af_apl_id||A.af_apl_id_sfx	AS CLID,
	B.df_prs_id_br					AS SSN,
	rtrim(C.DM_PRS_1)||' '||rtrim(C.DM_PRS_LST)		AS NAME,
	rtrim(C.dx_str_adr_1)||' '||rtrim(C.dx_str_adr_2)	AS ADDRESS,
	rtrim(C.dm_ct)							AS CITY,
	C.dc_dom_st						AS STATE,
	C.df_zip						AS ZIP
FROM	OLWHRM1.GA10_LON_APP A INNER JOIN
		OLWHRM1.GA01_APP B ON
			A.af_apl_id = B.af_apl_id INNER JOIN
		OLWHRM1.PD01_PDM_INF C ON
			B.df_prs_id_br = C.df_prs_id
WHERE	A.ad_prc = '2001-08-01' AND
		A.ac_prc_sta in ('P', 'I', 'S') and
		((a.ac_lon_typ = 'SF' and b.ax_scl_sub_cer_iaa is not null) or (a.ac_lon_typ = 'SU' and b.ax_scl_usb_cer_iaa is not null) or a.ac_lon_typ in ('PL', 'CL', 'SL'))
);
DISCONNECT FROM DB2;
ENDRSUBMIT;
PROC SORT DATA=WORKLOCL.DEMO;
BY SSN;
RUN;
/*PROC PRINTTO PRINT=REPORT2;
RUN;*/
OPTIONS PAGENO=1 LS=132;
PROC PRINT NOOBS SPLIT='/' DATA=WORKLOCL.DEMO WIDTH = MIN;
VAR SSN
	CLID
	NAME
	ADDRESS
	CITY
	STATE
	ZIP;
title 'Unapproved Guarantees for August 1, 2001';
footnote;
RUN;
proc sql;
create table demo2 as
select distinct ssn
from worklocl.demo;
quit;
