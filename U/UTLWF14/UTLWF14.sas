/*UTLWF14 Detail Suspense Report*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWF14.LWF14R2" ;

OPTIONS
        nodate
        ERROR = 0
        MISSING = ' '
        LS=96
        PS=54
        ;

DATA _NULL_;
    CALL SYMPUT('END',"'"||put(INTNX('DAY',DATE(),-1),mmddyy10.)||"'");
	CALL SYMPUT('RUNDT',PUT(DATE()-1,MMDDYY10.));
RUN;

%SYSLPUT END = &END;

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;

libname say '/sas/whse/finmgmt'; 

  DATA NULLRPT;
   set say.sayings;
   if monum = day(date());
       RUN;

proc sql;
connect to db2(database=dlgsutwh);
create table test as
select *
from connection to db2(
select a.bf_ssn
      ,b.lf_clm_id
      ,a.ld_rmt_sus
      ,a.lc_trx_typ
      ,a.la_trx
	  ,C.DF_SPE_ACC_ID AS ACCTNUM
from olwhrm1.dc11_lon_fat a inner join
     olwhrm1.dc01_lon_clm_inf b on
      a.af_apl_id = b.af_apl_id and
      a.af_apl_id_sfx = b.af_apl_id_sfx and
      a.lf_crt_dts_dc10 = b.lf_crt_dts_dc10 and
      a.bd_trx_pst_hst = &end
INNER JOIN OLWHRM1.PD01_PDM_INF C
	ON A.BF_SSN = C.DF_PRS_ID
where ld_rmt_sus is not null);
disconnect from db2;
quit;

ENDRSUBMIT;

DATA TEST; SET WORKLOCL.TEST; RUN;

data test;
set test;
format ld_rmt_sus mmddyy10.;
label ld_rmt_sus = 'Date Suspended';

proc sort data=test;
by ld_rmt_sus lc_trx_typ bf_ssn lf_clm_id;

proc printto print=report2 NEW;
proc report data=test headline headskip nowd;
  Title 'Detail Suspense Report'; 
  title2 "For &rundt";
column lc_trx_typ ACCTNUM lf_clm_id la_trx;

define lc_trx_typ /group format=$2. width=11 'Transaction Type';
define ACCTNUM /display format=$10. width=10 'Acct #';
define lf_clm_id /display format=$4. width=8 'Claim ID';
define la_trx /sum format=dollar14.2 width=14 'Amount Posted';
by ld_rmt_sus ;

break after lc_trx_typ / ol skip summarize suppress;

rbreak after / dol dul summarize ;

run; 
PROC PRINT noobs split='/' DATA=NULLRPT;
var string;
label string = 'Quote of the Day';

RUN;

PROC PRINTTO;
RUN;
