*  DEFAULT AVERSION REBATE REPORT (monthly) mc;

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWDV2.LWDV2R2";*/

options nodate pageno=1 ls=132 errors=0 mprint symbolgen;
libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;
data _null_;
   begin1=intnx('month',today(),0)-6  ; *Five days prior to end of previous month;
   call symput  ( 'begin', "'"||put(begin1,yymmdd10.)||"'" )   ;

   end1=intnx('month',today(),0)+5  ; *Fifth day of current month;
   call symput  ( 'end', "'"||put(end1,yymmdd10.)||"'" )   ;
run;
OPTIONS NOCENTER NODATE NONUMBER LS=132;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DVREB AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	A.af_apl_id||A.af_apl_id_sfx	as CLUID,
		integer(A.bf_ssn)				as SSN,
		A.lf_clm_id,
		A.ld_ldr_pof,
		B.la_pri,
		B.la_int,
		B.la_trf,
		B.ld_trf,
case 
	when RTRIM(D.DM_PRS_MID) <> ' ' 
		then RTRIM(D.DM_PRS_1)||' '||RTRIM(D.DM_PRS_MID)||' '||RTRIM(D.DM_PRS_LST)
	when RTRIM(D.DM_PRS_MID) = ' ' 
		then RTRIM(D.DM_PRS_1)||' '||RTRIM(D.DM_PRS_LST)
end										AS NAME,
		date(B.lf_crt_dts_dc20)			AS lf_crt_dts_dc20
FROM  OLWHRM1.DC01_LON_CLM_INF A inner join OLWHRM1.DC20_FEE_TRF B
	on a.af_apl_id = b.af_apl_id
	and a.af_apl_id_sfx = b.af_apl_id_sfx
left outer join OLWHRM1.PD01_PDM_INF D
	on A.bf_ssn = D.DF_PRS_ID
WHERE date(B.lf_crt_dts_dc20) between &begin and &end
AND B.lc_rec_sta = 'O'
AND A.ld_ldr_pof = 
	(select max(X.ld_ldr_pof)
	from OLWHRM1.DC01_LON_CLM_INF X
	where a.af_apl_id = x.af_apl_id
	and a.af_apl_id_sfx = x.af_apl_id_sfx
	and x.lc_pcl_rea IN ('DF','DQ','IN','DB','DU'))
);
DISCONNECT FROM DB2;

PROC SORT data=DVREB;BY SSN CLUID;RUN;
endrsubmit  ;

DATA DVREB; 
SET WORKLOCL.DVREB; 
RUN;

DATA _NULL_;
     EFFMO = PUT(INTNX('MONTH',TODAY(),-1), MONNAME9.);
	 EFFYR = PUT(INTNX('MONTH',TODAY(),-1), YEAR4.);
     CALL SYMPUT('EFFDATE',TRIM(LEFT(EFFMO||' '||EFFYR)));
RUN;

/*PROC PRINTTO PRINT=REPORT2;
RUN;*/

OPTIONS NOCENTER DATE NUMBER LINESIZE=126 PAGENO=1;

PROC REPORT DATA=DVREB NOWD SPACING=1 HEADSKIP SPLIT='/' ;
COLUMN SSN NAME CLUID lf_clm_id ld_ldr_pof la_pri la_int la_trf ld_trf N CSUM;
DEFINE SSN / FORMAT=SSN11. DISPLAY LEFT;
DEFINE NAME / 'Name' WIDTH=25 ;
DEFINE CLUID / 'Unique ID' ;
DEFINE LF_CLM_ID / 'Clm/ID' WIDTH=4 ;
DEFINE ld_ldr_pof / 'Clm Pd/Date' DISPLAY FORMAT=MMDDYY8. CENTER;
DEFINE LA_PRI / 'Outstanding/Principal' ANALYSIS SUM FORMAT=COMMA14.2 ;
DEFINE LA_INT / 'Outstanding/Interest' ANALYSIS SUM FORMAT=COMMA12.2 ;
DEFINE LA_TRF / '1%/Fee' ANALYSIS SUM FORMAT=COMMA12.2 ;
DEFINE LD_TRF / 'Bill/Date' DISPLAY FORMAT=MMDDYY8. CENTER;
DEFINE N /  NOPRINT WIDTH=5;
DEFINE CSUM / NOPRINT COMPUTED WIDTH = 5;
RBREAK AFTER / SUMMARIZE SKIP DOL;
COMPUTE CSUM;
CSUM = N;
ENDCOMP;
COMPUTE AFTER;
LINE ' ';
LINE ' ';
LINE "TOTAL NUMBER OF LOANS:  "CSUM COMMA6.;
ENDCOMP;
TITLE "Default Aversion Rebate Detail Report, &EFFDATE";
FOOTNOTE  'JOB = UTLWDV2     REPORT = ULWDV2.LWDV2R2';
RUN;
