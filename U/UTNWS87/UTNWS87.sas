/*set job specific values*/
/*	%LET ARCTYPEID = 0;		*Atd22ByLoan - Add arc by sequence number;*/
	%LET ARCTYPEID = 1;		*Atd22AllLoans - Add arc to all loans;
/*	%LET ARCTYPEID = 2;		*Atd22ByBalance - Add arc for all loans with a balance;*/
/*	%LET ARCTYPEID = 3;		*Atd22ByLoanProgram - Add arc by loan program;*/
/*	%LET ARCTYPEID = 4;		*Atd22AllLoansRegards - Add arc to all loans with regards to information;*/
/*	%LET ARCTYPEID = 5;		*Atd22ByLoanRegards - Add arc by sequence number with regards to information;*/

	%LET ARC = 'PHMPN';
	%LET COMMENT = 'Phone consent received per MPN';
	%LET SASID = 'UTNWS87';

/*set up library to SQL Server and include common code*/
/*	LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn; update_lock_typ=nolock; bl_keepnulls=no";*/
/*	%INCLUDE "Y:\Codebase\SAS\ArcAdd Common.SAS";*/
/*	%LET DSN = 'FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn;';*/
	LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS.dsn; update_lock_typ=nolock; bl_keepnulls=no";
	%INCLUDE "Z:\Codebase\SAS\ArcAdd Common.SAS";
	%LET DSN = 'FILEDSN=X:\PADR\ODBC\CLS.dsn;';


/*begin job specific code*/
LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME SAS_TAB V8 '/sas/whse/progrevw';
DATA _NULL_; 
	SET SAS_TAB.LASTRUN_JOBS;
/*	LAST_RUN = '15DEC2014'D;	*COMMENT FOR PROD, IT WILL READ THE DATE FROM A TABLE;*/
/*	IF JOB = 'UTNWS87' THEN DO;*/
		CALL SYMPUT('LAST_RUN',"'"||TRIM(LEFT(PUT(LAST_RUN,DATE10.)))|| "'D");
		CALL SYMPUT('LAST_RUNPASS',"'"|| PUT(LAST_RUN,MMDDYY10.) || "'");
/*	END;*/
RUN;
%PUT &LAST_RUN &LAST_RUNPASS;

PROC SQL;
	CONNECT TO DB2 (DATABASE=DNFPUTDL);

	CREATE TABLE REMOTE_DATA AS
		SELECT
			*
		FROM
			CONNECTION TO DB2
				(
					SELECT DISTINCT
						PD10.DF_SPE_ACC_ID
					FROM	
						PKUB.PD10_PRS_NME PD10
						JOIN PKUB.LN10_LON LN10
							ON PD10.DF_PRS_ID = LN10.BF_SSN
						LEFT JOIN PKUB.AY10_BR_LON_ATY AY10
							ON PD10.DF_PRS_ID = AY10.BF_SSN
							AND  AY10.PF_REQ_ACT= 'PHMPN'
						INNER JOIN PKUB.LN90_FIN_ATY LN90
							ON LN90.BF_SSN = PD10.DF_PRS_ID
							AND (LN90.PC_FAT_TYP = '01' AND LN90.PC_FAT_SUB_TYP = '01' AND LN90.LC_FAT_REV_REA = '' AND LN90.LC_STA_LON90 = 'A') 
					WHERE
						LN10.LD_LON_ACL_ADD >= &LAST_RUNPASS
						AND AY10.BF_SSN IS NULL
				)
	;

	DISCONNECT FROM DB2;
QUIT;

DATA SAS_TAB.LASTRUN_JOBS;
	SET SAS_TAB.LASTRUN_JOBS;
	IF JOB = 'UTNWS87' THEN LAST_RUN = TODAY();
RUN;
ENDRSUBMIT;

DATA REMOTE_DATA; SET LEGEND.REMOTE_DATA; RUN;

/*call macro or put data step here to add job specific data to LEGEND data*/
%CREATE_GENERIC_ARCADD_DATA;
/*end job specific code*/

/*call ARC add common processing*/
%ARC_ADD_PROCESSING;
