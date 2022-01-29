/*UTLWM01 - CHRONIC DELINQUENTS*/

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
/*FILENAME REPORT2 "&RPTLIB/ULWM01.LWM01R2";*/

FILENAME REPORT2 "T:\SAS\ULWM01.LWM01R2";
libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;

PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE CHRDLQ AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	A.DF_PRS_ID_BR
	,RTRIM(D.DM_PRS_LST)||', '||RTRIM(D.DM_PRS_1) AS NAME
	,A.AF_APL_OPS_SCL
	,E.IM_IST_FUL
FROM  OLWHRM1.GA01_APP A 
INNER JOIN OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
INNER JOIN OLWHRM1.GA14_LON_STA C
	ON B.AF_APL_ID = C.AF_APL_ID
	AND B.AF_APL_ID_SFX = C.AF_APL_ID_SFX
INNER JOIN OLWHRM1.PD01_PDM_INF D
	ON A.DF_PRS_ID_BR = D.DF_PRS_ID
INNER JOIN 
	(SELECT DISTINCT IF_IST
		,IM_IST_FUL
	FROM OLWHRM1.SC01_LGS_SCL_INF
	)E
	ON A.AF_APL_OPS_SCL = E.IF_IST

WHERE A.AF_APL_OPS_SCL IN ('02178500','02298500','02360800'
,'00367400','00367401','00367403','00367404','00367405')
AND C.AC_STA_GA14 = 'A'
AND C.AC_LON_STA_TYP = 'RP'
AND NOT EXISTS
	(SELECT *
	FROM OLWHRM1.AY01_BR_ATY X
	WHERE X.DF_PRS_ID = A.DF_PRS_ID_BR
	AND X.PF_ACT = 'ACRDL'
	AND DAYS(X.BD_ATY_PRF) > DAYS(CURRENT DATE) - 365)

ORDER BY A.DF_PRS_ID_BR
);
DISCONNECT FROM DB2;

ENDRSUBMIT;
DATA CHRDLQ; 
SET WORKLOCL.CHRDLQ; 
RUN;

DATA _NULL_;
SET  WORK.CHRDLQ;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT DF_PRS_ID_BR $9. ;
FORMAT NAME $49. ;
FORMAT AF_APL_OPS_SCL $8. ;
FORMAT IM_IST_FUL $40.;
DO;
	PUT DF_PRS_ID_BR $ @;
	PUT NAME $ @;
	PUT AF_APL_OPS_SCL $ @;
	PUT IM_IST_FUL $;
	;
END;
RUN;