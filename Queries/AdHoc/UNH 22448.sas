/*LIBNAME CRI 'Q:\Systems Support\Debbie\2014 files for CRI';*/
LIBNAME CRI 'T:\';

%LET LENDERS = '826717','827283','828476','830248';

/*•	Total $$ Subsidized Stafford 	_______________________*/
/*•	Total $$ Unsubsidized Stfd	_______________________*/
/*•	Total $$ PLUS	_______________________*/
/*•	Total $$ Consolidation	_______________________*/
/**/
/*•	Total number of loans 	_______________________*/
/*•	Total number of borrowers	_______________________*/
/*•	Total # of serviced lenders	_______________________*/
/*•	Total # of guarantors 	_______________________*/

PROC SQL;
	CREATE TABLE CRIDET AS
		SELECT DISTINCT
			LN10.BF_SSN,
			LN10.LN_SEQ,
			MR5A.IF_GTR,
			COALESCE(MR5A.WA_CUR_PRI,0) + COALESCE(MR5A.WA_CUR_BR_INT,0) + COALESCE(MR5A.WA_CUR_GOV_INT,0) AS LA_BAL,
			CASE
				WHEN MR5A.IC_LON_PGM IN ('PLUS','PLUSGB') THEN 'PLUS'
				WHEN MR5A.IC_LON_PGM IN ('CNSLDN','SUBCNS','SUBSPC','UNCNS','UNSPC') THEN 'CONSOL'
				ELSE MR5A.IC_LON_PGM 
			END AS LOAN_TYPE,
			MR5A.IF_OWN
		FROM 
			CRI.Ln10_lon_eom LN10
			LEFT JOIN CRI.Mr5a_mr_lon_mth_01 MR5A
				ON LN10.BF_SSN = MR5A.BF_SSN
				AND LN10.LN_SEQ = MR5A.LN_SEQ
		WHERE
			SUBSTR(MR5A.IF_OWN,1,6) IN (&LENDERS)
			AND LN10.LC_STA_LON10 = 'R'
	;
QUIT;

PROC SQL;
	CREATE TABLE BALS AS
		SELECT DISTINCT
			IF_OWN,
			LOAN_TYPE,
			SUM(LA_BAL) AS TTL_BAL
		FROM
			CRIDET
		GROUP BY
			IF_OWN,
			LOAN_TYPE
	;
QUIT;

PROC SQL;
	CREATE TABLE CNTS AS
		SELECT DISTINCT
			IF_OWN AS LENDER,
			COUNT(DISTINCT BF_SSN) AS BORROWERS,
			COUNT(DISTINCT CAT(BF_SSN,PUT(LN_SEQ,Z4.))) AS LOANS,
			COUNT(DISTINCT IF_GTR) AS GUARANTORS
		FROM
			CRIDET
		GROUP BY
			IF_OWN
	;
QUIT;

PROC EXPORT
		DATA=BALS
		OUTFILE='T:\CRI Audit Loan Totals.XLSX'
		REPLACE;
RUN;


PROC EXPORT
		DATA=CNTS
		OUTFILE='T:\CRI Audit Loan Totals.XLSX'
		REPLACE;
RUN;
