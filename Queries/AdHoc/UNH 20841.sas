LIBNAME DUSTER REMOTE SERVER=DUSTER SLIBREF=WORK;
RSUBMIT;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
PROC SQL;
	CREATE TABLE ACH AS
		SELECT DISTINCT
			BR30.BF_SSN

		FROM
			OLWHRM1.BR30_BR_EFT BR30
			JOIN OLWHRM1.DF10_BR_DFR_REQ DF10
				ON BR30.BF_SSN = DF10.BF_SSN
		WHERE
			BR30.BC_EFT_STA = 'A'
			AND DF10.LD_DFR_REQ_BEG < TODAY()
			AND DF10.LD_DFR_REQ_END > TODAY()
	;
QUIT;
ENDRSUBMIT;

PROC EXPORT
		DATA=DUSTER.ACH
		OUTFILE='T:\SAS\Borrower on ACH and in Deferment.XLSX'
		REPLACE;
RUN;
