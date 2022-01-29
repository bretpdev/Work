/*********************************/
/*UTLWG55 RESCREENED APLICATIONS*/
/*********************************/
LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
/*%LET RPTLIB = T:\SAS;*/
FILENAME REPORT2 "&RPTLIB/ULWG55.LWG55R2";
DATA _NULL_;
     CALL SYMPUT('PREVDAY',"'"||PUT(INTNX('DAY',TODAY(),-1,'BEGINNING'), MMDDYYD10.)||"'");
     CALL SYMPUT('RUNDT',PUT(INTNX('DAY',TODAY(),0,'BEGINNING'), MMDDYYS10.));
RUN;
/*%SYSLPUT PREVDAY = &PREVDAY;*/
/*LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;*/
/*RSUBMIT;*/
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE ILTAR AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.DF_PRS_ID_BR
	,A.AF_APL_ID
	,C.DM_PRS_1
	,C.DM_PRS_MID
	,C.DM_PRS_LST	
	,C.DF_SPE_ACC_ID
FROM OLWHRM1.GA01_APP A
INNER JOIN OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
INNER JOIN OLWHRM1.PD01_PDM_INF C
	ON A.DF_PRS_ID_BR = C.DF_PRS_ID
WHERE B.AD_PRC = &PREVDAY
AND B.AC_PRC_STA = 'A'
AND B.AI_GTE_RSC = 'Y'
);
DISCONNECT FROM DB2;
/*ENDRSUBMIT;*/

/*DATA ILTAR;*/
/*SET WORKLOCL.ILTAR;*/
/*RUN;*/

PROC SORT DATA=ILTAR;
BY DF_PRS_ID_BR;
RUN;

DATA _NULL_;
SET ILTAR ;
LENGTH DESCRIPTION $600.;
USER = ' ';
ACT_DT = &PREVDAY ;
DESCRIPTION = CATX(',',
		DM_PRS_1
		,DM_PRS_MID
		,DM_PRS_LST
		,DF_SPE_ACC_ID
		,AF_APL_ID
	
);
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT USER $10. ;
/*FORMAT ACT_DT MMDDYY10. ;*/
FORMAT DESCRIPTION $600. ;
IF _N_ = 1 THEN DO;
	PUT "USER,ACT_DT,DESCRIPTION";
END;
DO;
   PUT USER $ @;
   PUT ACT_DT @;
   PUT DESCRIPTION $ ;
END;
RUN;