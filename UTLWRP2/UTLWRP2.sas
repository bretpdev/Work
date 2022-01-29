/*	List loans where Aux Status = 01 and Aux Status Date is
	within previous month. */

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWRP2.LWRP2R2";*/

DATA _NULL_;
/*Date variable that displays the first of the previous month.*/
CALL SYMPUT('BEGIN',"'"||PUT(INTNX('MONTH',DATE(),-1), MMDDYY10.)||"'");
/*Date variable that displays the last date of the previous month.*/
CALL SYMPUT('END',"'"||PUT(INTNX('MONTH',DATE(),0)-1, MMDDYY10.)||"'");
RUNDT = INTNX('DAY',INTNX('MONTH',DATE(),0),-1);
/*Month and year for title.*/
CALL SYMPUT('RUNDT',TRIM(LEFT(UPCASE(PUT(RUNDT,MONNAME.))))||' '||PUT(RUNDT,YEAR.));
RUN;
%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT END = &END;

libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;
OPTIONS NOCENTER NODATE NONUMBER LS=132;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE REPMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	a.lc_pcl_rea					as CLMTYP,
		A.IF_OPS_LDR					AS CLMLDR,
		integer(A.bf_ssn)				as SSN,
		RTRIM(D.DM_PRS_LST) 			AS NAME,
		A.af_apl_id||A.af_apl_id_sfx	as CLUID,
		A.LF_CLM_ID 					AS CLMID,
		a.la_clm_int + a.la_pri_agy_clc	AS CLMPDAMT,
		A.LD_LDR_POF 					AS CLMPDDT,
		B.LA_TRX 	 					AS RPAMT,
		B.BD_TRX_PST_HST 				AS RPDT
FROM  OLWHRM1.DC01_LON_CLM_INF A left outer join OLWHRM1.DC11_LON_FAT B
	on a.lf_crt_dts_dc10 = b.lf_crt_dts_dc10
	AND B.LC_TRX_TYP = 'RP' 
INNER join OLWHRM1.PD01_PDM_INF D
	on A.bf_ssn = D.DF_PRS_ID
WHERE A.lc_aux_sta = '01'
AND A.ld_aux_sta_upd BETWEEN &BEGIN AND &END
ORDER BY A.bf_ssn, A.LF_CLM_ID
);
DISCONNECT FROM DB2;
endrsubmit  ;

DATA REPMO; 
SET WORKLOCL.REPMO; 
ATUT = (month(RPDT) - month(CLMPDDT)) + (year(RPDT) - year(CLMPDDT))*12;
by ssn notsorted;
if first.ssn then count=1;
else count=0;
RUN;

/*PROC PRINTTO PRINT=REPORT2;
RUN;*/
%macro rptprnt(dsname);
%let dsid=%sysfunc(open(&dsname));
%let hasobs=%sysfunc(attrn(&dsid,any));
%let rc=%sysfunc(close(&dsid));
%if &hasobs=1 %then
	%do;
OPTIONS NOCENTER NODATE NUMBER PAGENO=1 LS=126;
PROC REPORT DATA=&dsname NOWD SPACING=1 HEADSKIP;
TITLE "MONTHLY REPURCHASES - &RUNDT";
FOOTNOTE  'JOB = UTLWRP2     REPORT = ULWRP2.LWRP2R2';
COLUMN CLMTYP CLMLDR SSN NAME CLUID CLMID CLMPDAMT CLMPDDT 
ATUT RPAMT RPDT N COUNT BWRS AVGMO;
DEFINE CLMTYP / DISPLAY "Claim Type" WIDTH=5;
DEFINE CLMLDR / DISPLAY "Claim Lender";* WIDTH=6;
DEFINE SSN / DISPLAY ORDER FORMAT=SSN11.;
DEFINE NAME / DISPLAY ORDER WIDTH=12 "Name";
DEFINE CLUID / DISPLAY "Unique ID";
DEFINE CLMID / DISPLAY "Claim ID" WIDTH=5;
DEFINE CLMPDAMT / ANALYSIS SUM FORMAT=COMMA12.2 "Claim Paid Amount";
DEFINE CLMPDDT / DISPLAY FORMAT=MMDDYY8. "Claim Paid Date";
DEFINE ATUT / ANALYSIS MEAN FORMAT=3. "Months at UHEAA" WIDTH=6;
DEFINE RPAMT / ANALYSIS SUM FORMAT=COMMA12.2 "Repurchase Amount";
DEFINE RPDT / DISPLAY FORMAT=MMDDYY8. "Repurchase Date" WIDTH=10;
DEFINE N /  NOPRINT WIDTH=5;
DEFINE COUNT / NOPRINT;
DEFINE BWRS / NOPRINT COMPUTED FORMAT=COMMA5.;
DEFINE AVGMO / NOPRINT COMPUTED FORMAT=3.;
BREAK AFTER SSN / SUMMARIZE SKIP OL SUPPRESS;
RBREAK AFTER  / SUMMARIZE SKIP DOL SUPPRESS;

COMPUTE BWRS;
BWRS = COUNT.SUM;
ENDCOMP;
COMPUTE AVGMO;
AVGMO = ATUT.MEAN;
ENDCOMP;

COMPUTE AFTER;
LINE @1 "Number of Borrowers:  " @35 BWRS COMMA6.;
LINE @1 "Number of Loans:  " @35 N COMMA6.;
LINE @1 "Average Number of Months at UHEAA:  " @39 AVGMO 4.1;
ENDCOMP;
RUN;
	%end;
%else %if &hasobs=0 %then
	%do;
    	data _null_;
            file print notitles;
            put @01 "No repurchases found for &RUNDT";
        run;
    %end;
%mend rptprnt;
%rptprnt(REPMO);
