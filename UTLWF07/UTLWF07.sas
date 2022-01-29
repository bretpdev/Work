OPTION
        ERRORS  = 0
        MISSING = '0'
        LS      =96
        PS      =54
                ;

       %LET RPTLIB = %SYSGET(reportdir);
        FILENAME REPORT2 "&RPTLIB/ULWF07.LWF07R2" ;

                DATA NULLRPT;
                        STRING = "END OF REPORT";
                        RUN;

 DATA _NULL_;
        IF WEEKDAY(DATE()) =1 THEN DO;
        CALL SYMPUT('END',"'"||put(INTNX('DAY',DATE(),-2),mmddyy10.)||"'");
        END;
        ELSE DO;
        CALL SYMPUT('END',"'"||put(INTNX('DAY',DATE(),-1),mmddyy10.)||"'");
        END;
        CALL SYMPUT('RUNDT',PUT(DATE()-1,MMDDYY10.));

proc sql;
connect to db2(database=dlgsutwh);
create table dr01 as
select *
from connection to db2(
select count(bn_dly_rci_seq) as counts
      ,sum(ba_trx) as totals
      ,bf_usr_pst_dr01
      ,bc_dly_rci_typ
      ,bc_trx_typ
from olwhrm1.dr01_dly_rci
where bd_trx_pst = &end and
      substr(bf_usr_pst_dr01,1,2) = 'PH'
group by bf_usr_pst_dr01
        ,bc_dly_rci_typ
        ,bc_trx_typ
order by bf_usr_pst_dr01);

disconnect from db2;
quit;

proc printto print=report2;

proc print noobs split='/' data=dr01;
by bf_usr_pst_dr01;
id bc_dly_rci_typ;
var bc_trx_typ counts totals;
label bf_usr_pst_dr01 = 'Posted by'
      bc_dly_rci_typ = 'Record Type'
      bc_trx_typ = 'Transaction Type'
      counts = 'Transactions Posted'
      totals = 'Total Posted';
format counts comma7.
       totals dollar14.2;
sum counts totals;
Title1 'Default Team Daily Posting Statistics';
Title2 "&rundt";

PROC PRINT DATA=NULLRPT;
    RUN;
