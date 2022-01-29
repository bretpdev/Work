/*UTLWM38 - VWA Payment Tracking*/
/*-----Production settings-----*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/

/*-----Development settings-----*/
%LET RPTLIB = T:\SAS;
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;

FILENAME REPORTZ "&RPTLIB/ULWM38.LWM38RZ";
FILENAME REPORT2 "&RPTLIB/ULWM38.LWM38R2";

/*-----Development settings-----*/
RSUBMIT;

/*-----Production settings-----*/
/*%macro sqlcheck ;*/
/*  %if  &sqlxrc ne 0  %then  %do  ;*/
/*    data _null_  ;*/
/*            file reportz notitles  ;*/
/*            put @01 " ********************************************************************* "*/
/*              / @01 " ****  The SQL code above has experienced an error.               **** "*/
/*              / @01 " ****  The SAS should be reviewed.                                **** "       */
/*              / @01 " ********************************************************************* "*/
/*              / @01 " ****  The SQL error code is  &sqlxrc  and the SQL error message  **** "*/
/*              / @01 " ****  &sqlxmsg   **** "*/
/*              / @01 " ********************************************************************* "*/
/*            ;*/
/*         run  ;*/
/*  %end  ;*/
/*%mend  ;*/

PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	PD01.DF_SPE_ACC_ID
	,PD01.DM_PRS_1 || ' ' || PD01.DM_PRS_LST AS NAME
	,DC11.LD_TRX_EFF
FROM	OLWHRM1.PD01_PDM_INF PD01
INNER JOIN OLWHRM1.GA01_APP GA01
	ON GA01.DF_PRS_ID_BR = PD01.DF_PRS_ID
INNER JOIN OLWHRM1.DC01_LON_CLM_INF DC01
	ON DC01.AF_APL_ID = GA01.AF_APL_ID
INNER JOIN OLWHRM1.DC11_LON_FAT DC11
	ON DC11.AF_APL_ID = DC01.AF_APL_ID
	AND DC11.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
INNER JOIN OLWHRM1.BR01_BR_CRF BR01
	ON PD01.DF_PRS_ID = BR01.BF_SSN
WHERE DC11.LC_TRX_TYP = 'EP'
	AND DC01.LC_STA_DC10 = '03'
	AND DC01.LC_AUX_STA = ''
	AND DC01.LC_PCL_REA IN ('DB', 'DQ', 'DF')
	AND DC01.LD_CLM_ASN_DOE IS NULL
	AND DC01.LC_REA_CLM_ASN_DOE = ''
	AND BR01.BN_RHB_PAY_CTR > 7
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*-----Production settings-----*/
/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

/*-----Development settings-----*/
ENDRSUBMIT;
DATA DEMO; SET WORKLOCL.DEMO; RUN;

/*Some borrowers have multiple payments in some months, so get the results down to a per-month basis.*/
DATA DEMO (DROP = LD_TRX_EFF); SET DEMO;
PAYMENT_MONTH = MONTH(LD_TRX_EFF);
PAYMENT_YEAR = YEAR(LD_TRX_EFF);
RUN;
PROC SORT DATA=DEMO NODUPKEY;
BY DF_SPE_ACC_ID DESCENDING PAYMENT_YEAR DESCENDING PAYMENT_MONTH;
RUN;

/*Whittle the data down to consecutive months.*/
DATA DEMO; SET DEMO;
BY DF_SPE_ACC_ID;
RETAIN PREVIOUS_PAYMENT_MONTH;
IF FIRST.DF_SPE_ACC_ID THEN DO;
	PREVIOUS_PAYMENT_MONTH = PAYMENT_MONTH;
	OUTPUT;
END;
ELSE IF PAYMENT_MONTH = PREVIOUS_PAYMENT_MONTH - 1 OR (PAYMENT_MONTH = 12 AND PREVIOUS_PAYMENT_MONTH = 1) THEN DO;
	PREVIOUS_PAYMENT_MONTH = PAYMENT_MONTH;
	OUTPUT;
END;
RUN;

/*Get the borrowers who have at least 8 records.*/
PROC MEANS NOPRINT DATA = DEMO (KEEP = DF_SPE_ACC_ID NAME);
BY DF_SPE_ACC_ID NAME;
OUTPUT OUT = DEMO;
RUN;
DATA DEMO (DROP = _TYPE_); SET DEMO;
WHERE _FREQ_ >= 8;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS CENTER PAGENO=1 ORIENTATION=PORTRAIT ;
OPTIONS LS=96 PS=52;
PROC PRINT NOOBS SPLIT='/' DATA=DEMO WIDTH=UNIFORM WIDTH=MIN;
VAR DF_SPE_ACC_ID
	NAME
	_FREQ_;
LABEL	DF_SPE_ACC_ID = 'Account Number'
		NAME = 'Borrower Name'
		_FREQ_ = 'Number of VWA Payments';
TITLE	'VWA Payment Tracking';
FOOTNOTE1  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE2	"Please take appropriate precautions to safeguard this information.";
FOOTNOTE3	;
FOOTNOTE4 	'JOB = UTLWM38     REPORT = ULWM38.LWM38R2';
RUN;
PROC PRINTTO;
RUN;
