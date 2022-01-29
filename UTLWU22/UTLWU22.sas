/*UTLWU22 - Ref Add KLOANAPP QC*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWU22.LWU22RZ";
FILENAME REPORT2 "&RPTLIB/ULWU22.LWU22R2";

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;

%macro sqlcheck ;
  %if  &sqlxrc ne 0  %then  %do  ;
    data _null_  ;
            file reportz notitles  ;
            put @01 " ********************************************************************* "
              / @01 " ****  The SQL code above has experienced an error.               **** "
              / @01 " ****  The SAS should be reviewed.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  The SQL error code is  &sqlxrc  and the SQL error message  **** "
              / @01 " ****  &sqlxmsg   **** "
              / @01 " ********************************************************************* "
            ;
         run  ;
  %end  ;
%mend  ;


PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE QUERY AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	D.DF_SPE_ACC_ID,
	C.BF_LST_USR_AY01,
	C.BD_ATY_PRF
FROM	OLWHRM1.GA14_LON_STA A
	INNER JOIN OLWHRM1.GA10_LON_APP B
		ON A.AF_APL_ID = B.AF_APL_ID
		AND A.AF_APL_ID_SFX = B.AF_APL_ID_SFX
	INNER JOIN OLWHRM1.GA01_APP J
		ON A.AF_APL_ID = J.AF_APL_ID
	INNER JOIN OLWHRM1.AY01_BR_ATY C
		ON C.DF_PRS_ID = J.DF_PRS_ID_BR
	INNER JOIN OLWHRM1.PD01_PDM_INF D
	 	ON D.DF_PRS_ID = J.DF_PRS_ID_BR
WHERE	A.AC_LON_STA_TYP IN ('CR', 'DA', 'FB', 'IA', 'ID', 'IG', 'IM', 'RF', 'RP', 'UA', 'UB')
	AND B.AA_CUR_PRI > 0
	AND C.PF_ACT = 'K1APP'
	AND DAYS(C.BD_ATY_PRF) = DAYS(CURRENT DATE) - 1
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;
DATA QUERY;
SET WORKLOCL.QUERY;
RUN;
PROC SORT DATA=QUERY;
	BY DF_SPE_ACC_ID;
RUN;
DATA _NULL_;
	SET QUERY  ;
	USER = BF_LST_USR_AY01;
	ACT_DT = BD_ATY_PRF;
	DESCRIPTION = DF_SPE_ACC_ID;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
	FORMAT USER $10. ;
	FORMAT ACT_DT MMDDYY10. ;
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


