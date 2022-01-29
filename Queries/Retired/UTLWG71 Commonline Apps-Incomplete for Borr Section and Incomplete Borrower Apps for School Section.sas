/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB=T:\SAS;
FILENAME REPORT2 "&rptlib/ULWG71.LWG71R2";
OPTIONS YEARCUTOFF=1960 MISSING=' ' ERRORS=0 NOCENTER LS=132 PS=60;

DATA NULLRPT  ;  STRING= "***   NO OBSERVATIONS FOUND   ***"  ;  RUN ;

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;

proc sql;
connect to db2(database=dlgsutwh);
        create table one as
        select *
                from connection to db2
                (select
                  a.af_apl_id||b.af_apl_id_sfx as uid,
                  b.ac_prc_sta            as stat,
                  a.ac_apl_ent_mth            as meth,
             SUBSTR(A.DF_PRS_ID_BR,1,3)||'-'||SUBSTR(A.DF_PRS_ID_BR,4,2)||
         '-'||SUBSTR(A.DF_PRS_ID_BR,6,4) AS SSNB,
                  a.ad_apl_rcv            as recd,
                  b.ac_lon_typ            as ltype,
                  a.af_apl_ops_scl          as schl,
                  a.ai_br_sig             as bsign,
                  a.ai_scl_sig            as ssign,
				  C.DF_SPE_ACC_ID
        from olwhrm1.ga01_APP a inner join olwhrm1.GA10_LON_APP b
                 on a.af_apl_id= b.af_apl_id 
			INNER JOIN OLWHRM1.PD01_PDM_INF C
				ON A.DF_PRS_ID_BR = C.DF_PRS_ID
                where a.ad_apl_rcv > '04/01/2000'
			and b.ac_prc_sta in ('I','X','P')  
                        )  ;

DISCONNECT FROM DB2;
quit;
 %PUT &SQLXRC& &SQLXMSG&;
run;

Data schl(drop=uidb uid)  borr(drop=uids uid);
        set one;
        if meth in ('10', '1P', '1R') and ssign ='Y' then do;
        uids=uid;
           output schl;
        end;

        if meth not in ('10', '1P', '1R') and bsign='Y' then do;
         uidb=uid;
            output borr;
         end;
run;
proc sort data=schl; by ssnb schl ;
run;
proc sort data=borr; by ssnb schl;
run;

Data both;
Merge schl(IN=A keep=ssnb schl uids ltype DF_SPE_ACC_ID) borr(IN=B keep=ssnb schl uidb DF_SPE_ACC_ID);
by ssnb schl;
if a=b;

run;

ENDRSUBMIT;
DATA BOTH;
SET WORKLOCL.BOTH;
RUN;

proc sort data=both;
by schl;
Proc Printto file=report2 NEW;
Run;

proc print data=both split='/' ;
var DF_SPE_ACC_ID uidb uids ltype schl ;
label DF_SPE_ACC_ID= 'ACCT #' uids='Commonline/UniqID' uidb='Borr/UniqID' ltype='Loan/Type'
schl='School/Code';
title 'Commonline Apps-Incomplete for Borr Section and Incomplete Borrower Apps for School Section';
Footnote 'tmn-lwg71';
run;

PROC PRINT DATA = NULLRPT ; RUN ;

PROC PRINTTO;RUN;
