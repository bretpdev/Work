/*	Originally a PHEAA-created report.  Modified to add '04' claims
	and to eliminate loans counted more than once in totals.*/

OPTIONS
        ERROR = 0
        MISSING = ' '
        LS=96
        PS=54
        ;
libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;

DATA _NULL_;
CALL SYMPUT('END',"'"||put(INTNX('month',DATE(),0)-1,mmddyy10.)||"'");
RUN;
%syslput END = &END;

RSUBMIT;
proc sql;
connect to db2(database=dlgsutwh);
create table aver as
select *
from connection to db2(
select a.af_apl_id||a.af_apl_id_sfx as CLUID
      ,a.lf_crt_dts_dc10
      ,a.bf_ssn
      ,a.ld_pcl_rcv
      ,b.ld_trf
      ,b.la_trf
from olwhrm1.dc20_fee_trf b inner join
     olwhrm1.dc01_lon_clm_inf a on
     b.af_apl_id = a.af_apl_id and
     b.af_apl_id_sfx = a.af_apl_id_sfx
where b.lc_rec_sta = 'O' and
      a.lc_sta_dc10 in ('03','04') and
      b.ld_trf = &end
and a.ld_pcl_rcv = 	
	(select max(y.ld_pcl_rcv)
	from olwhrm1.dc20_fee_trf x inner join olwhrm1.dc01_lon_clm_inf y
	on x.af_apl_id = y.af_apl_id 
	and x.af_apl_id_sfx = y.af_apl_id_sfx
	where x.af_apl_id||x.af_apl_id_sfx = a.af_apl_id||a.af_apl_id_sfx
	and x.lc_rec_sta = 'O' 
	and y.lc_sta_dc10 in ('03','04') 
	and x.ld_trf = &end)
order by a.ld_pcl_rcv);
disconnect from db2;
quit;

endrsubmit;
data aver;
set worklocl.aver;
run;

data aver;
set aver;
yr=intnx('month',ld_pcl_rcv,0);
srtdt2=put(yr,monyy7.);
srtdt=year(ld_pcl_rcv)||month(ld_pcl_rcv);

proc sort;
by srtdt;

proc means noprint sum;
by srtdt;
id srtdt2;
var la_trf;
output out=sums sum=la_trf;
run;

proc print noobs split='/' data=sums;
var srtdt2 _freq_ la_trf;
label srtdt2 = 'Year and Month/Preclaim Received'
      _freq_ = 'Claims'
      la_trf = 'Amount Transfered';
sum la_trf _freq_;
format la_trf dollar14.2
       _freq_ comma7.;
title1 'Default Aversion Fee';
Title2 'Funds Transfered Out';
title3 "Month Ending &end";
run;
