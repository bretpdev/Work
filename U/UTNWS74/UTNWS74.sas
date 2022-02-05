%LET RPTLIB = %SYSGET(reportdir);
/*%LET RPTLIB = T:\SAS;*/
FILENAME REPORT2 "&RPTLIB/UNWS74.NWS74R2";
FILENAME REPORTZ "&RPTLIB/UNWS74.NWS74RZ";

LIBNAME SAS_TAB V8 '/sas/whse/progrevw';
/*LIBNAME SAS_TAB V8 'Y:\Development\SAS Test Files\progrevw';*/

DATA _NULL_; 
	SET SAS_TAB.LASTRUN_JOBS;
	/*If the job must be run manually set this macro to the last day it successfully ran(last business day).*/
/*	LAST_RUN = TODAY() - 365;	*COMMENT FOR PROD, IT WILL READ THE DATE FROM A TABLE;*/
	IF JOB = 'UTNWS74' THEN DO;
		CALL SYMPUT('LAST_RUN',"'"||TRIM(LEFT(PUT(LAST_RUN,DATE10.)))|| "'D");
		CALL SYMPUT('LAST_RUNPASS',"'"|| PUT(LAST_RUN,MMDDYY10.) || "'");
	END;
RUN;

/*%SYSLPUT LAST_RUN = &LAST_RUN;*/
/**/
/*LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;*/
/*RSUBMIT; */
/*%LET DB = DNFPRQUT;  *This is test;*/
%LET DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB; 
LIBNAME AES DB2 DATABASE=&DB OWNER=AES; 

PROC SQL;
	CREATE TABLE PH05 AS
		SELECT DISTINCT
			PUT(PH05.DF_SPE_ID,Z10.) AS DF_SPE_ACC_ID,
			PH05.DX_CNC_EML_ADR,
			PH05.DF_DTS_EML_ADR_EFF,
			PH05.DF_LST_USR_EML_ADR,
			CASE
				WHEN PH05.DI_VLD_CNC_EML_ADR = 'Y' THEN 1
				ELSE 0
			END AS DI_VLD_CNC_EML_ADR,
			CASE
				WHEN PH05.DI_CNC_ELT_OPI = 'Y' THEN 1
				ELSE 0
			END AS DI_CNC_ELT_OPI,
			PH05.DC_ELT_OPI_SRC,
			CASE
				WHEN PH05.DI_CNC_EBL_OPI = 'Y' THEN 1
				ELSE 0
			END AS DI_CNC_EBL_OPI,
			PH05.DC_EBL_OPI_SRC,
			CASE
				WHEN PH05.DI_CNC_TAX_OPI = 'Y' THEN 1
				ELSE 0
			END AS DI_CNC_TAX_OPI,
			PH05.DC_TAX_OPI_SRC
		FROM
			PKUB.PD10_PRS_NME PD10
			JOIN PKUB.LN10_LON LN10
				ON PD10.DF_PRS_ID = LN10.BF_SSN
			JOIN AES.PH05_CNC_EML PH05
				ON PD10.DF_SPE_ACC_ID = PUT(PH05.DF_SPE_ID,Z10.)
		WHERE
			LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0
			AND DATEPART(PH05.DF_LST_DTS_PH05) >= &LAST_RUN
	;
QUIT;

/*uncomment for prod so the data set gets updated*/
DATA SAS_TAB.LASTRUN_JOBS;
SET SAS_TAB.LASTRUN_JOBS;
IF JOB = 'UTNWS74' THEN LAST_RUN = TODAY();
RUN;
/*ENDRSUBMIT;*/

/*DATA PH05; SET LEGEND.PH05; RUN; *2;*/

DATA _NULL_;
	SET PH05 END = EOF;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
	IF _N_ = 1 THEN PUT "-Begin-";
	DO;
		PUT DF_SPE_ACC_ID @;
		PUT DX_CNC_EML_ADR @;
		PUT DF_DTS_EML_ADR_EFF @;
		PUT DF_LST_USR_EML_ADR @;
		PUT DI_VLD_CNC_EML_ADR @;
		PUT DI_CNC_ELT_OPI @;
		PUT DC_ELT_OPI_SRC @;
		PUT DI_CNC_EBL_OPI @;
		PUT DC_EBL_OPI_SRC @;
		PUT DI_CNC_TAX_OPI @;
		PUT DC_TAX_OPI_SRC ;
	END;
	IF EOF THEN PUT "-End-";
RUN;