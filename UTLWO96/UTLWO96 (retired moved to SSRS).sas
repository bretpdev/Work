/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWO96.LWO96RZ";
FILENAME REPORT2 "&RPTLIB/ULWO96.LWO96R2";
FILENAME REPORT3 "&RPTLIB/ULWO96.LWO96R3";
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
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	DISTINCT
		A.DF_SPE_ACC_ID
		,A.DF_PRS_ID
		,'XX-XXX-'||SUBSTR(A.DF_PRS_ID,6,4) AS SSN
		,TRIM(A.DM_PRS_1)||' '||TRIM(A.DM_PRS_LST) AS DM_PRS
		,B.IF_OPS_SCL_RPT
		,B.LC_CUR_SCL_ID_SRC
		,B.LC_STU_ENR_TYP
		,B.LC_ENR_STA_SRC
		,B.LD_STU_ENR_STA
		,B.LC_ENR_STA_DTE_SRC
		,B.LD_XPC_GRD
		,B.LC_XPC_GRD_DTE_SRC
		,B.LD_ENR_CER
		,B.LC_ENR_DTE_CER_SRC

FROM	OLWHRM1.PD01_PDM_INF A
		INNER JOIN OLWHRM1.SD02_STU_ENR B
			ON A.DF_PRS_ID = B.DF_PRS_ID_STU
			AND B.LC_STA_SD02 = 'A'
		INNER JOIN (SELECT	Z.DF_PRS_ID_STU
							,COUNT(Z.DF_PRS_ID_STU) AS STU_CNT
					FROM	OLWHRM1.SD02_STU_ENR Z
					WHERE	Z.LC_STA_SD02 = 'A'
					GROUP BY DF_PRS_ID_STU) C
			ON A.DF_PRS_ID = C.DF_PRS_ID_STU
			AND C.STU_CNT > 1
ORDER BY DF_SPE_ACC_ID

FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;
DATA DEMO;
	SET WORKLOCL.DEMO;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;

OPTIONS CENTER PAGENO=1 ORIENTATION=LANDSCAPE;
OPTIONS LS=127 PS=39;

PROC PRINT NOOBS SPLIT='/' DATA=DEMO WIDTH=UNIFORM WIDTH=MIN;
VAR 	DF_SPE_ACC_ID
		SSN
/*		DM_PRS_LST*/
		DM_PRS
		IF_OPS_SCL_RPT
		LC_CUR_SCL_ID_SRC
		LC_STU_ENR_TYP
		LC_ENR_STA_SRC
		LD_STU_ENR_STA
		LC_ENR_STA_DTE_SRC
		LD_XPC_GRD
		LC_XPC_GRD_DTE_SRC
		LD_ENR_CER
		LC_ENR_DTE_CER_SRC;
LABEL	DF_SPE_ACC_ID = 'Account Number'
		SSN	= 'SSN'
		DM_PRS = 'Name'
		IF_OPS_SCL_RPT = 'Current School ID'
		LC_CUR_SCL_ID_SRC = 'ID/Scr/1'
		LC_STU_ENR_TYP = 'Enr/Sta/2'
		LC_ENR_STA_SRC = 'Sta/Src/3'
		LD_STU_ENR_STA = 'Enrollment Effective Date'
		LC_ENR_STA_DTE_SRC = 'Enr/Src/4'
		LD_XPC_GRD = 'Expected Graduation Date'
		LC_XPC_GRD_DTE_SRC = 'Grd/Src/5'
		LD_ENR_CER = 'Certification Date'
		LC_ENR_DTE_CER_SRC = 'Crt/Src/6';
FORMAT  LD_STU_ENR_STA MMDDYY10.
		LD_XPC_GRD MMDDYY10.
		LD_ENR_CER MMDDYY10.;
TITLE	'Multiple Active Enrollments';

FOOTNOTE1	"1=School ID Source 2=Enrollment Status 3=Enrollment Status Source 4=Enrollment Effective Source";
FOOTNOTE2	"5=Graduation Date Source 6=Certification Date Source";
FOOTNOTE3  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE4	"Please take appropriate precautions to safeguard this information.";
FOOTNOTE5	;
FOOTNOTE6 	'JOB = UTLWO96     REPORT = ULWO96.LWO96R2';
RUN;
PROC PRINTTO PRINT=REPORT3 NEW;
RUN;

PROC PRINT NOOBS SPLIT='/' DATA=DEMO WIDTH=UNIFORM WIDTH=MIN;
VAR 	DF_SPE_ACC_ID
		SSN
/*		DM_PRS_LST*/
		DM_PRS
		IF_OPS_SCL_RPT
		LC_CUR_SCL_ID_SRC
		LC_STU_ENR_TYP
		LC_ENR_STA_SRC
		LD_STU_ENR_STA
		LC_ENR_STA_DTE_SRC
		LD_XPC_GRD
		LC_XPC_GRD_DTE_SRC
		LD_ENR_CER
		LC_ENR_DTE_CER_SRC;
WHERE LD_XPC_GRD < TODAY();
LABEL	DF_SPE_ACC_ID = 'Account Number'
		SSN	= 'SSN'
		DM_PRS = 'Name'
		IF_OPS_SCL_RPT = 'Current School ID'
		LC_CUR_SCL_ID_SRC = 'ID/Scr/1'
		LC_STU_ENR_TYP = 'Enr/Sta/2'
		LC_ENR_STA_SRC = 'Sta/Src/3'
		LD_STU_ENR_STA = 'Enrollment Effective Date'
		LC_ENR_STA_DTE_SRC = 'Enr/Src/4'
		LD_XPC_GRD = 'Expected Graduation Date'
		LC_XPC_GRD_DTE_SRC = 'Grd/Src/5'
		LD_ENR_CER = 'Certification Date'
		LC_ENR_DTE_CER_SRC = 'Crt/Src/6';
FORMAT  LD_STU_ENR_STA MMDDYY10.
		LD_XPC_GRD MMDDYY10.
		LD_ENR_CER MMDDYY10.;
TITLE	'Multiple Active Enrollments With Past Expected Graduation Date';

FOOTNOTE1	"1=School ID Source 2=Enrollment Status 3=Enrollment Status Source 4=Enrollment Effective Source";
FOOTNOTE2	"5=Graduation Date Source 6=Certification Date Source";
FOOTNOTE3  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE4	"Please take appropriate precautions to safeguard this information.";
FOOTNOTE5	;
FOOTNOTE6 	'JOB = UTLWO96     REPORT = ULWO96.LWO96R2';
RUN;
PROC PRINTTO;
RUN;
