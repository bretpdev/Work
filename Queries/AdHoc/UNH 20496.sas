LIBNAME DUSTER REMOTE SERVER=DUSTER SLIBREF=WORK;
RSUBMIT;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;

PROC SQL;
	CREATE TABLE FNLBLL AS
		SELECT DISTINCT
			LN80.BF_SSN,
			LN80.LN_SEQ,
			LN80.LD_BIL_CRT
		FROM
			OLWHRM1.LN80_LON_BIL_CRF LN80
			JOIN OLWHRM1.BL10_BR_BIL BL10
				ON LN80.BF_SSN = BL10.BF_SSN
				AND LN80.LD_BIL_CRT = BL10.LD_BIL_CRT
				AND LN80.LN_SEQ_BIL_WI_DTE = BL10.LN_SEQ_BIL_WI_DTE
		WHERE
			LN80.LI_FNL_BIL_LON = 'Y'
			AND BL10.LC_IND_BIL_SNT = 'I'
	;
QUIT;
ENDRSUBMIT;

DATA FNLBLL; SET DUSTER.FNLBLL; RUN;

PROC EXPORT
		DATA=FNLBLL
		OUTFILE='T:\SAS\Final Bill Borrower Query.xlsx'
		REPLACE;
RUN;
/**/
/*	We need to locate loans where LN80, value LI_FNL_BIL_LON final bill indicator = Y. Each record will have within the row field LD_BIL_CRT which is a date. With those dates, we need to go to the BL10 table and in the corresponding row (LD_BIL_CRT), we need to find all borrowers where LC_IND_BIL_SNT = I. */
/**/
/*This is to find all borrowers where a final bill indicator exists in LN80, but the bill was created with a sent indicator (LC_IND_BIL_SNT) of I, rather than F. Let me know if this needs clarification.*/
