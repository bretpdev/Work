options nodate pageno=1 ps=58 ls=80 errors=0 ;

libname dlgsutwh db2 database=dlgsutwh owner=olwhrm1 ;

%let  rptlib = %sysget(reportdir)  ;

filename report2  "&rptlib/ULWDPC.LWDPCR2"  ;

data _null_  ;
   begin1=intnx('week',today()+3,-1,'beginning')  ;
   call symput  ( 'begin', "'"||put(begin1,yymmdd10.)||"'" )   ;

   end1=intnx('week',today()+3,-1,'end')  ;
   call symput  ( 'end', "'"||put(end1,yymmdd10.)||"'" )   ;
run;

proc sql;
connect to db2 (database=dlgsutwh);
create table preclm as
select 	input(ssno,9.) 		    as 	ssn,
 	appid,
 	timestmp,
 	prin+intr 		            as 	balance,
    datepart(timestmp)          as    create,
 	INTNX('WEEK',datepart(timestmp),0,'end') 	as      weekend
from connection to db2
   (
   select  distinct
 	   bf_ssn 			        as      ssno,
           af_apl_id||af_apl_id_sfx 	as      appid,
           lf_crt_dts_dc10      as      timestmp,
           la_clm_pri 			as     	prin,
 	   la_clm_int               as      intr
   from olwhrm1.zd01_dft_pcl
   where   lc_aux_sta in('  ','04') and
           date(lf_crt_dts_dc10) between &begin and &end and
           lc_pcl_rea not in('DE','DI','BC','BH','BO','IN','CS','DU','FC')
   );

create table cancel as
select 	input(ssno,9.) 			as 	ssn,
 	appid,
 	timestmp,
 	prin+intr 			as 	balance,
    datepart(timestmp)  as  create,
 	INTNX('WEEK',datepart(timestmp),0,'end')         as      weekend
from connection to db2
   (
   select  distinct
 	   bf_ssn 			as      ssno,
           af_apl_id||af_apl_id_sfx 	as      appid,
           lf_crt_dts_dc10 		as      timestmp,
           la_clm_pri 			as 	prin,
 	   la_clm_int                   as      intr        
   from olwhrm1.zd02_dft_can
   where   lc_aux_sta in('  ','04') and
           date(lf_crt_dts_dc10) between &begin and &end and
           lc_pcl_rea not in('DE','DI','BC','BH','BO','IN','CS','DU','FC')
   );

create table deft as
select 	input(ssno,9.) 			as 	ssn,
 	appid,
 	timestmp,
 	prin+intr 			as 	balance,
     datepart(timestmp)  as  create,
 	INTNX('WEEK',datepart(timestmp),0,'end')         as      weekend
from connection to db2
   (
   select  distinct
 	   bf_ssn 			as      ssno,
           af_apl_id||af_apl_id_sfx 	as      appid,
           lf_crt_dts_dc10 		as      timestmp,
           la_clm_pri 			as 	prin,
 	   la_clm_int                   as      intr        
   from olwhrm1.zd03_dft_clm
   where   lc_aux_sta='  ' and
           date(lf_crt_dts_dc10) between &begin and &end and
           lc_pcl_rea not in('DE','DI','BC','BH','BO','IN','CS','DU','FC')
   );

create table activity as
select 	input(ssno,9.) 		as 	ssn,
        entry,
        datepart(created)  as  created,
        userid
from connection to db2
   (
   select  df_prs_id 		as      ssno,
           pf_act           as      entry,
           bf_crt_dts_ay01  as      created,
           bf_lst_usr_ay01  as      userid
   from olwhrm1.AY01_BR_ATY

   where   pf_act='ADPCA' and
           date(bf_crt_dts_ay01) between &begin and &end
           and substr(bf_lst_usr_ay01,1,2)='DV'
                 
   );
disconnect from db2;
quit;

proc sort data=activity nodup;
   by ssn created;
run;

proc sort data=preclm nodupkey;
   by ssn appid;
run;

proc sort data=cancel nodupkey;
   by ssn appid;
run;

proc sort data=deft nodupkey;
   by ssn appid;
run;

data allcreate;
   set preclm cancel deft;
run;

proc sort data=allcreate nodupkey;
   by ssn appid;
run;

data combine;
   merge activity(in=a)
         allcreate(in=b);
   by ssn;
   if a=1 and create=created;
run;

proc sort data=combine;
   by create ssn;
run;

proc printto print=report2 ;
run;

%macro rptprnt(dsname);
   %let dsid=%sysfunc(open(&dsname));
   %let hasobs=%sysfunc(attrn(&dsid,any));
   %let rc=%sysfunc(close(&dsid));
   %if &hasobs=1 %then
      %do;
         proc tabulate data=&dsname;
            class weekend;
   	    var balance;
   	    table weekend,
         	  n*f=comma8.
            balance*f=dollar16.2 / rts=14;
            format weekend mmddyy10.;
            label weekend='Week Ending';
            keylabel sum=' ' n='#' all=' ';
            title1 'Batch creates for the last week';
            title2 "Run Date: &sysdate9  (LWDPCR2)";
         run;
      %end;
   %else %if &hasobs=0 %then
      %do;
         data _null_;
            file print notitles;
            put @01 'No records found for LWDPCR2';
         run;
      %end;
%mend rptprnt;

%rptprnt(combine);
