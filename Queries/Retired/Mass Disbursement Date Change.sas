/*IDENTIFY ALL DISBURSEMENTS FOR THE SPECIFIED SCHOOL 
SCHEDULED FOR THE SPECIFIED DATE.  THE OUTPUT IS USED BY THE 
MASS DISBURSEMENT CHANGE SCRIPT TO CHANGE THE DISBURSEMENT
DATES.*/

%LET SCHLID = '00522000'; /*ENTER DESIRED SCHOOL ID*/
%LET DSBDT = '01/06/2006'; /*ENTER DESIRED DISBURSEMENT DATE*/

%SYSLPUT SCHLID = &SCHLID;
%SYSLPUT DSBDT = &DSBDT;

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;
OPTIONS NOCENTER NODATE NONUMBER LS=132;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE MDDCH AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.DF_PRS_ID_BR AS SSN
	,RTRIM(B.AF_APL_ID)||B.AF_APL_ID_SFX AS CLUID
	,B.AF_CUR_LON_OPS_LDR
	,D.AF_APL_ID
	,D.AN_SEQ
	,D.AF_DOE_SCL
	,F.IM_SCL_FUL
	,E.LD_DSB
FROM OLWHRM1.GA01_APP A 
INNER JOIN OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
INNER JOIN OLWHRM1.GA11_LON_DSB_ATY C
	ON C.AF_APL_ID = B.AF_APL_ID
	AND C.AF_APL_ID_SFX = B.AF_APL_ID_SFX
LEFT OUTER JOIN OLWHRM1.AP03_MASTER_APL D
	ON B.AF_APL_ID = D.AF_CNL
	AND B.AF_APL_ID_SFX = D.AF_CNL_SFX
LEFT OUTER JOIN OLWHRM1.LN15_DSB E
	ON D.AF_APL_ID = E.AF_APL_ID
	AND E.LD_DSB = &DSBDT
LEFT OUTER JOIN OLWHRM1.SC10_SCH_DMO F
	ON D.AF_DOE_SCL = F.IF_DOE_SCL
WHERE B.AC_PRC_STA = 'A' /*APPROVED LOAN*/
AND A.AF_APL_OPS_SCL = &SCHLID
AND C.AC_DSB_ADJ_STA = 'A' /*ACTIVE GA11 ROW*/
AND C.AD_DSB_ADJ = &DSBDT
AND C.AC_DSB_ADJ IN ('B','D','E','I','N','R')
ORDER BY A.DF_PRS_ID_BR, RTRIM(B.AF_APL_ID)||B.AF_APL_ID_SFX
);
DISCONNECT FROM DB2;
ENDRSUBMIT  ;

DATA MDDCH; 
SET WORKLOCL.MDDCH; 
RUN;

PROC SORT DATA = MDDCH;
BY SSN CLUID;
RUN;

DATA _NULL_;
SET  WORK.MDDCH;
FILE 'T:\SAS\MassDisbChange.txt' DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT SSN $9. ;
FORMAT CLUID $19. ;
FORMAT AF_APL_ID $9. ;
FORMAT AN_SEQ 6. ;
FORMAT AF_DOE_SCL $8. ;
FORMAT IM_SCL_FUL $40. ;
FORMAT LD_DSB MMDDYY10. ;
DO;
   PUT SSN $ @;
   PUT CLUID $ @;
   PUT AF_APL_ID $ @;
   PUT AN_SEQ @;
   PUT LD_DSB @;
   PUT AF_DOE_SCL $ @;
   PUT IM_SCL_FUL $ ;
END;
RUN;
