/*QC - Payroll Deduction Payments*/
/*UTLWO29*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWO29.LWO29R2";
FILENAME REPORTZ "&RPTLIB/ULWO29.LWO29RZ";
options symbolgen;
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
SELECT 
	BF_SSN AS SSN
	,DF_SPE_ACC_ID
	,LD_FAT_EFF
	,TRIM(DM_PRS_1) || ' ' || TRIM(DM_PRS_LST) AS NAME
	,LN_SEQ
	,LC_FAT_REV_REA
	,PC_FAT_TYP
	,PC_FAT_SUB_TYP
FROM	OLWHRM1.LN90_FIN_ATY A 
INNER JOIN OLWHRM1.PD10_PRS_NME B
ON A.BF_SSN = B.DF_PRS_ID
WHERE LC_FAT_REV_REA = ''
AND PC_FAT_TYP = '10'
AND PC_FAT_SUB_TYP = '14'
AND LC_STA_LON90 = 'A'
FOR READ ONLY WITH UR

);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;
DATA DEMO; SET WORKLOCL.DEMO; RUN;

DATA _NULL_;
SET DEMO ;
LENGTH DESCRIPTION $600.;
USER = ' ';
ACT_DT = LD_FAT_EFF;
DESCRIPTION = CATX(',',
	DF_SPE_ACC_ID,
	NAME,
	LN_SEQ);
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
