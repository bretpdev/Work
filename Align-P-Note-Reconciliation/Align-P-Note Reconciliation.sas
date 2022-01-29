%LET ADD_DATE = '04JUN2001'D;

/*%LET RPTLIB = T:\SAS;*/
%LET RPTLIB = Q:\Support Services\Test Files\SAS\SASR 3852\;
FILENAME REPORT2 "&RPTLIB\Align-P-Note Reconciliation.R2.txt";

%SYSLPUT ADD_DATE = &ADD_DATE;

LIBNAME DUSTER REMOTE SERVER=DUSTER SLIBREF=WORK;
RSUBMIT;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;

PROC SQL;
	CREATE TABLE R2 AS
		SELECT DISTINCT
			PD10.DF_SPE_ACC_ID
		FROM
			OLWHRM1.LN10_LON LN10
			JOIN OLWHRM1.PD10_PRS_NME PD10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
		WHERE
			LN10.IF_DOE_LDR IN ('826717','830248')
/*			LN10.IF_DOE_LDR = '829123'*/
			AND LN10.LD_LON_ACL_ADD = &ADD_DATE
	;
QUIT;

ENDRSUBMIT;

DATA R2; SET DUSTER.R2; RUN;

DATA _NULL_;
	SET R2;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;;

	PUT DF_SPE_ACC_ID 'WYMPN,,,,,,,ALL,';
RUN;

/*rsubmit;*/
/*LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*proc sql;*/
/*	create table adddates as*/
/*		select*/
/*			LD_LON_ACL_ADD,*/
/*			count(distinct bf_ssn) as cnt*/
/*		from*/
/*			OLWHRM1.LN10_LON LN10*/
/*		group by*/
/*			LD_LON_ACL_ADD*/
/*	;*/
/*quit;*/
/*endrsubmit;*/
/**/
/*RSUBMIT;*/
/*PROC SQL;*/
/*	SELECT*/
/*		IF_DOE_LDR*/
/*	FROM*/
/*		OLWHRM1.LN10_LON LN10*/
/*	WHERE*/
/*		LD_LON_ACL_ADD='04JUN2001'D*/
/*	;*/
/*QUIT;*/
/*ENDRSUBMIT;	*/