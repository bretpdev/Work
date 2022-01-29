/*

MONTHLY PRECLAIMS

These reports give totals and detail for the preclaims filed during the previous month.

*/

options nodate pageno=1 ps=54 ls=120 errors=0 mprint;
libname dlgsutwh db2 database=dlgsutwh owner=olwhrm1 ;
%let  rptlib = %sysget(reportdir)  ;
filename report2  "&rptlib/ULWDPM.LWDPMR2"  ;
filename report3  "&rptlib/ULWDPM.LWDPMR3"  ;
filename report4  "&rptlib/ULWDPM.LWDPMR4"  ;


*LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
*RSUBMIT;
data _null_;
   begin1=intnx('month',today(),-1)  ;
   call symput  ( 'begin', "'"||put(begin1,yymmdd10.)||"'" )   ;

   end1=intnx('month',today(),0)-1  ;
   call symput  ( 'end', "'"||put(end1,yymmdd10.)||"'" )   ;
run;

proc sql ;
connect to db2 (database=dlgsutwh);
create table preclm as
select *
from connection to db2
   (
   select  distinct
           bf_ssn                               as      ssno,
           af_apl_id||af_apl_id_sfx             as      appid,
           lf_crt_dts_dc10                      as      timestmp,
           lc_pca_crt_src                       as      creatype,
           la_clm_pri+la_clm_int                as      balance,
           lf_cur_lon_ser_agy                   as      servicer,
           case
              when lf_cur_lon_ser_agy='700141'
              then 'SLSC '
              else 'UHEAA'
           end as serv
from olwhrm1.zd01_dft_pcl
where   date(lf_crt_dts_dc10) between &begin and &end and
        lc_pcl_rea not in('DE','DI','BC','BH','BO','IN','CS','DU','FC')
   );

create table cancels as
select *
from connection to db2
   (
   select  distinct
           bf_ssn                               as      ssno,
           af_apl_id||af_apl_id_sfx             as      appid,
           lf_crt_dts_dc10                      as      timestmp,
           lc_pca_crt_src                       as      creatype,
           la_clm_pri+la_clm_int                as      balance,
           lf_cur_lon_ser_agy                   as      servicer,
           case
              when lf_cur_lon_ser_agy='700141'
              then 'SLSC '
              else 'UHEAA'
           end as serv
from olwhrm1.zd02_dft_can
where   date(lf_crt_dts_dc10) between &begin and &end and
        lc_pcl_rea not in('DE','DI','BC','BH','BO','IN','CS','DU','FC')
   );

create table deft as
select *
from connection to db2
   (
   select  distinct
           bf_ssn                               as      ssno,
           af_apl_id||af_apl_id_sfx             as      appid,
           lf_crt_dts_dc10                      as      timestmp,
           'I'                                  as      creatype,
           la_clm_pri+la_clm_int                as      balance,
           lf_cur_lon_ser_agy                   as      servicer,
           case
              when lf_cur_lon_ser_agy='700141'
              then 'SLSC '
              else 'UHEAA'
           end as serv
from olwhrm1.zd03_dft_clm
where   date(lf_crt_dts_dc10) between &begin and &end and
        lc_pcl_rea not in('DE','DI','BC','BH','BO','IN','CS','DU','FC')
   );
disconnect from db2;
quit;
*ENDRSUBMIT;
proc sort data=work.preclm nodupkey;
        by ssno appid timestmp;
run;

proc sort data=work.cancels nodupkey;
        by ssno appid timestmp;
run;

proc sort data=work.deft nodupkey;
        by ssno appid timestmp;
run;

data work.combine(drop=ssno);
   set work.preclm work.cancels work.deft;
   ssn=ssno+0;
run;

proc sort data=work.combine;
   by ssn;
run;

data work.pcaaccnt;
        set work.combine;
        by ssn;
        if first.ssn then baltot=0;
        baltot+balance;
        if last.ssn then output work.pcaaccnt;
run;

PROC SORT DATA=work.combine;
        BY SERV servicer;;
RUN;

proc format;
        value $lndrfmt
        'I'='On-Line'
        'R'='Remote '
        'U'='Batch  ';
run;
DATA _NULL_;
     EFFMO = PUT(INTNX('MONTH',TODAY(),-1), MONNAME9.);
	 EFFYR = PUT(INTNX('MONTH',TODAY(),-1), YEAR4.);
     CALL SYMPUT('EFFDATE',EFFMO||' '||EFFYR);
RUN;
proc printto print=report3 ;
run;

%macro rptprnt(dsname);
   %let dsid=%sysfunc(open(&dsname));
   %let hasobs=%sysfunc(attrn(&dsid,any));
   %let rc=%sysfunc(close(&dsid));
   %if &hasobs=1 %then
      %do;
         PROC TABULATE DATA=&dsname;
               CLASS CREATYPE;
               VAR BALANCE;
               TABLE ALL,
                    (CREATYPE all)*(N*F=COMMA8. BALANCE*F=DOLLAR17.2) / rts=12 ;
               LABEL CREATYPE='Create Type'
                     BALANCE='$ Amount';
               KEYLABEL SUM=' ' ALL='Total' n='Number';
               FORMAT CREATYPE $LNDRFMT.;
               TITLE1 "Number and Dollar of Preclaims Created in &EFFDATE";
               TITLE2 "(Note Level)  Run Date:  &sysdate9 LWDP0R3";
               FOOTNOTE1;
         RUN;
      %end;
   %else %if &hasobs=0 %then
      %do;
         data _null_;
            file print notitles;
            put @01 'No records found for LWDP0R3';
         run;
      %end;
%mend rptprnt;
%rptprnt(work.combine);

proc printto print=report2 ;
run;

options pageno=1;

%macro rptprnt(dsname);
   %let dsid=%sysfunc(open(&dsname));
   %let hasobs=%sysfunc(attrn(&dsid,any));
   %let rc=%sysfunc(close(&dsid));
   %if &hasobs=1 %then
      %do;
         proc tabulate data=&dsname;
            class serv servicer;
            var baltot;
            table serv*(servicer all) all,
                  n*f=comma8.
                  baltot*f=dollar16.2;
            label baltot='Total $ Accounts'
                  servicer='Detail'
                  serv='Servicer';
            keylabel n='Total # Accounts' all='Total' sum=' ';
            TITLE1 "Number and Dollar of Preclaims Created in &EFFDATE";
            TITLE2 "(Account Level)  Run Date:  &sysdate9 LWDP0R2";
            FOOTNOTE1 'Creation Reason is Not:  DE, DI, BC, BH, BO,, IN, CS, DU, FC';
         RUN;
      %end;
   %else %if &hasobs=0 %then
      %do;
         data _null_;
            file print notitles;
            put @01 'No records found for LWDP0R2';
         run;
      %end;
%mend rptprnt;

%rptprnt(work.pcaaccnt);
options pageno = 1;
proc printto print=report4 ;
run;
proc print data = work.combine split ='/';
var 
	ssn
	appid
	timestmp
	creatype
	balance
	servicer;
label	ssn = 'SSN'
		appid = 'Unique ID'
		timestmp = 'Create Date and Time'
		creatype = 'Create Type'
		balance = 'Balance'
		servicer = 'Servicer ID';
title	"Preclaims Detail for the Month of &EFFDATE";
run;