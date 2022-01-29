/*IDENTIFY ALL USU DISBURSEMENTS SCHEDULED FOR 12/31/02*/

libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;
OPTIONS NOCENTER NODATE NONUMBER LS=132;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE USUDISBS AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT A.DF_PRS_ID_BR				as SSN
	,B.AF_APL_ID
	,B.AF_APL_ID_SFX
	,B.AF_CUR_LON_OPS_LDR
FROM OLWHRM1.GA01_APP A 
INNER JOIN OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
INNER JOIN OLWHRM1.GA11_LON_DSB_ATY C
	ON C.AF_APL_ID = B.AF_APL_ID
	AND C.AF_APL_ID_SFX = B.AF_APL_ID_SFX
WHERE B.AC_PRC_STA = 'A' /*APPROVED LOAN*/
AND A.AF_APL_OPS_SCL = '00367700'
AND C.AC_DSB_ADJ_STA = 'A' /*ACTIVE GA11 ROW*/
AND C.AD_DSB_ADJ = '2002-12-31'
AND C.AC_DSB_ADJ IN ('B','D','E','I','N','R')
ORDER BY A.DF_PRS_ID_BR, B.AF_APL_ID, B.AF_APL_ID_SFX
);
DISCONNECT FROM DB2;
endrsubmit  ;

DATA USUDISBS; 
SET WORKLOCL.USUDISBS; 
RUN;

*WRITE LPP FILE;
data _null_ ;
set  WORK.Usudisbs                                end=EFIEOD;
where AF_CUR_LON_OPS_LDR NE '817455';
%let _EFIERR_ = 0; /* set the ERROR detection macro variable */
%let _EFIREC_ = 0;     /* clear export record count macro variable */
file 'T:\SAS\USUDisb - LPP.txt' delimiter=',' DSD DROPOVER lrecl=32767;
   format SSN $9. ;
   format AF_APL_ID $17. ;
   format AF_APL_ID_SFX $2.;
 do;
   EFIOUT + 1;
   put SSN $ @;
   put AF_APL_ID $ @;
   put AF_APL_ID_SFX $;
   ;
 end;
if _ERROR_ then call symput('_EFIERR_',1);  /* set ERROR detection macro variable */
If EFIEOD then
   call symput('_EFIREC_',EFIOUT);
run;

*WRITE ZIONS FILE;
data _null_;
set  WORK.Usudisbs                                end=EFIEOD;
where AF_CUR_LON_OPS_LDR EQ '817455';
%let _EFIERR_ = 0; /* set the ERROR detection macro variable */
%let _EFIREC_ = 0;     /* clear export record count macro variable */
file 'T:\SAS\USUDisb - ZIONS.txt' delimiter=',' DSD DROPOVER lrecl=32767;
   format SSN $9. ;
   format AF_APL_ID $17. ;
   format AF_APL_ID_SFX $2.;
 do;
   EFIOUT + 1;
   put SSN $ @;
   put AF_APL_ID $ @;
   put AF_APL_ID_SFX $;
   ;
 end;
if _ERROR_ then call symput('_EFIERR_',1);  /* set ERROR detection macro variable */
If EFIEOD then
   call symput('_EFIREC_',EFIOUT);
run;

