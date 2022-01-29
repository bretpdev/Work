/*APPLY WHERE APPROPRIATE*/

/*%LET TBLLIB = /sas/whse/progrevw;*/
%LET TBLLIB = Q:\Process Automation\TabSAS;

/*INPUT LOAN TYPES FOR PRIVATE AND FFEL LOANS*/
DATA LOAN_TYPES;
	FORMAT LN_TYP LN_PGM $50.;
	INFILE "&TBLLIB/GENR_REF_LoanTypes.txt" DLM=',' MISSOVER DSD;
	INFORMAT LN_TYP LN_PGM $50.;
	INPUT LN_TYP LN_PGM ;
	LN_PGM = UPCASE(LN_PGM);
RUN;
/*CREATE MACRO VARIALBE LISTS OF LOAN PROGRAMS(FFEL AND PRIVATE LOANS)*/
PROC SQL NOPRINT;
	SELECT "'"||TRIM(LN_TYP)||"'" 
		INTO :FFELP_LIST SEPARATED BY "," /*FFEL LOAN LIST*/
	FROM LOAN_TYPES
	WHERE LN_PGM = 'FFEL';

	SELECT "'"||TRIM(LN_TYP)||"'" 
		INTO :PRIVATE_LIST SEPARATED BY "," /*PRIVATE LOAN LIST*/
	FROM LOAN_TYPES
	WHERE LN_PGM ^= 'FFEL';
QUIT;

/*MAKE LOAN PROGRAM ASSIGNMENT*/
/*	IF IC_LON_PGM IN (&FFELP_LIST) THEN*/
/*		LN_PGM = 'FFELP';*/
/*	ELSE */
/*		LN_PGM = 'ALT';*/
