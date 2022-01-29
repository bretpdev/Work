options error=0 nocenter;

libname dlgsutwh db2 database=dlgsutwh owner=olwhrm1 ;

%let  rptlib = %sysget(reportdir)  ;

filename report2  "&rptlib/ULWG30.LWG30R2"  ;
filename report3  "&rptlib/ULWG30.LWG30R3"  ;

proc sql  ;
CONNECT TO DB2 (database=dlgsutwh);
create table ONE as
SELECT *
FROM CONNECTION TO DB2  (
select
    DEC(A.DF_PRS_ID_STU,9)   as ssna,
    A.AC_LON_TYP                 AS LTYPE
FROM
   olwhrm1.ZG01_ICP_APP A inner join olwhrm1.GA30_APL_PRC_CTL B
          ON  A.AF_APL_ID = B.AF_APL_ID
where
   A.AD_PRC >= (CURRENT DATE-180 DAYS)
   AND B.AC_SCL_ICP = 'Y'
   AND B.AC_STU_BR_ICP <> 'Y'
   AND B.AC_STA_GA30 = 'A'
    ) ;
     
     DISCONNECT FROM DB2 ;

  %PUT &SQLXRC& &SQLXMSG& ;

 RUN;

DATA STAFFORD PLUS ;
 SET ONE;

   IF LTYPE IN ('SF','SU') THEN OUTPUT STAFFORD ;

   IF LTYPE = 'PL' THEN OUTPUT PLUS ;
      ;

proc printto print=report2 ;
run  ;

PROC TABULATE DATA=STAFFORD;
   VAR SSNA;
   TABLE (SSNA='TOTAL'*N='NUMBER'*F=COMMA10.);
   TITLE1 'APPS WAITING FOR SCHOOL CERTIFICATION AND SCHOOL HAS ';
   TITLE2 'REQUESTED ONE-TIME NOTIFICATION ONLY';
   TITLE3 'STAFFORD ONLY';
   FOOTNOTE 'MEM, JOB=LWG30, REPORT=LWG30R2';
 RUN;


proc printto print=report3 ;
run  ;

PROC TABULATE DATA=PLUS ;
   VAR SSNA;
   TABLE (SSNA='TOTAL'*N='NUMBER'*F=COMMA10.);
   TITLE1 'APPS WAITING FOR SCHOOL CERTIFICATION AND SCHOOL HAS ';
   TITLE2 'REQUESTED ONE-TIME NOTIFICATION ONLY';
   TITLE3 'PLUS/SLS ONLY';
   FOOTNOTE 'MEM, JOB=LWG30, REPORT=LWG30R3';
RUN;
