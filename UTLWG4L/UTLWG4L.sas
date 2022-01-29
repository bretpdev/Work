options nocenter LS=150 PS=60 ;

/*libname dlgsutwh db2 database=dlgsutwh owner=olwhrm1 ;*/
/*%let  rptlib = %sysget(reportdir)  ;*/
%let  rptlib = T:\SAS;
filename report2  "&rptlib/ULWG4L.LWG4LR2"  ;

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;

proc sql  ;
CONNECT TO DB2 (database=dlgsutwh);
create table L140 as
SELECT *
FROM CONNECTION TO DB2  (
select distinct
		A.af_apl_id||A.af_apl_id_sfx	AS UID,
		A.AD_LON_STA					AS LNST,
		/*  SUBSTR(B.DF_PRS_ID_STU,1,3)||'-'||SUBSTR(B.DF_PRS_ID_STU,4,2)||*/
		/*         '-'||SUBSTR(B.DF_PRS_ID_STU,6,4) AS SSNS,*/
		E.DF_SPE_ACC_ID					AS SSNS,
		B.AF_CUR_APL_OPS_LDR			AS LNDR,
		B.AF_APL_OPS_SCL				AS SCHL,
		C.AD_LST_RPT_MNF				AS RPTDATE,
		C.ac_org_int_typ				AS INT,
		C.AD_PRC						AS DPROC,
		C.AF_CUR_LON_SER_AGY			AS SERV,
		D.LD_ENR_CER					AS CERTDATE,
		D.LD_STU_ENR_STA				AS ENRLDATE
FROM
		OLWHRM1.ga14_lon_sta A 
		INNER JOIN OLWHRM1.GA01_APP B
			ON A.AF_APL_ID = B.AF_APL_ID
        INNER JOIN OLWHRM1.ga10_lon_app C
        	ON A.AF_APL_ID = C.AF_APL_ID
       	INNER JOIN OLWHRM1.SD02_STU_ENR D
            ON D.DF_PRS_ID_STU = B.DF_PRS_ID_STU
		INNER JOIN OLWHRM1.PD01_PDM_INF E
			ON B.DF_PRS_ID_BR = E.DF_PRS_ID

WHERE
		A.ac_sta_ga14 = 'A'
		AND A.ac_lon_sta_typ = 'ID'
		AND C.AC_LON_TYP IN ('SF','SU')
		AND C.AD_LST_RPT_MNF = '10/31/2000'
		/*AND C.AD_LST_RPT_MNF IS NOT NULL */
		AND D.LC_STA_SD02 = 'A'
		AND D.LC_STU_ENR_STA = 'O'
)  ;

    
   DISCONNECT FROM DB2 ;

  %PUT &SQLXRC& &SQLXMSG& ;

DATA ERROR ;
 SET L140 ;

 IF INT = '7' THEN INTRATE = '9' ;
 ELSE INTRATE = '6' ;

 EOG = INTNX('MONTH',ENRLDATE,INTRATE)+DAY(ENRLDATE) ;

FORMAT EOG RPTDATE MMDDYY10. ;

  IF EOG <= RPTDATE ;
RUN ;

PROC SORT DATA=ERROR;
 BY SERV ;
RUN ;

ENDRSUBMIT;
DATA ERROR; SET WORKLOCL.ERROR; RUN;

DATA NULLRPT  ;  STRING= "***   END OF OBS OR NO OBSERVATIONS FOUND   ***"  ;  RUN ;


proc printto print=report2 NEW;
run  ;

PROC PRINT NOOBS N DATA=ERROR SPLIT='/' ;
 BY SERV ;
 PAGEBY SERV ;
 VAR LNDR SSNS UID DPROC LNST CERTDATE SCHL ;
 FORMAT dproc lnst certdate mmddyy10. ;
 LABEL SSNS     = 'STUDENT/ACCOUNT/NUMBER'
       UID      = 'UNIQUE ID'
       DPROC    = 'DATE/GUARANTEED'
       LNDR     = 'LENDER/CODE'
       LNST     = 'LOAN STATUS/DATE'
       CERTDATE = 'DATE/CERTIFIED'
       SCHL     = 'SCHOOL/CODE'
       ;
TITLE 'Accounts that have an L140 error code on them' ;
FOOTNOTE 'JOB=UTLWG4L, REPORT=ULWG4LR2';
RUN ;

PROC PRINT DATA = NULLRPT ; RUN ;

PROC PRINTTO;
RUN;
