/*	Query for all SSNs where there is a CP payment transaction
with an effective date greater than 9/30/2000.

DC11_LON_FAT
bf_ssn M Ib1 X(9) Borrower SSN. Alternate index.
lc_trx_typ MD X(2) Transaction type code = 'CP'
ld_trx_eff O X(10) Transaction effective date
la_apl_pri MD S9(7)V9(2) COMP-3Amount applied to principal
la_apl_int MD S9(7)V9(2) COMP-3Amount applied to interest
la_apl_leg_cst MD S9(5)V9(2) COMP-3Amount applied to legal costs
la_apl_oth_chr MD S9(5)V9(2) COMP-3Amount applied to other charges
la_apl_col_cst MD S9(5)V9(2) COMP-3Amount applied to collection costs
*/

libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;
OPTIONS NOCENTER NODATE NONUMBER LS=132;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	integer(A.bf_ssn)							AS BSSN,
		RTRIM(D.DM_PRS_LST)							AS LNAME,
		A.ld_trx_eff								AS TRXEFF,
		A.lc_trx_typ								AS TRXTYP,
		A.la_apl_pri + A.la_apl_int + A.la_apl_leg_cst 
		+ A.la_apl_oth_chr + A.la_apl_col_cst		AS PSTAMT

FROM  OLWHRM1.DC11_LON_FAT A left outer join OLWHRM1.PD01_PDM_INF D
	on A.bf_ssn = D.DF_PRS_ID
WHERE A.ld_trx_eff > '09-30-2000'
AND A.lc_trx_typ = 'CP'
ORDER BY A.ld_trx_eff, A.bf_ssn
);
DISCONNECT FROM DB2;
endrsubmit  ;

DATA DEMO; 
SET WORKLOCL.DEMO; 
MM = MONTH(TRXEFF);
YY = YEAR(TRXEFF);
RUN;

OPTIONS NOCENTER NODATE NONUMBER LS=126;* PS=40;
PROC PRINT NOOBS SPLIT='/' DATA=DEMO WIDTH=UNIFORM WIDTH=MIN;
VAR BSSN LNAME TRXEFF TRXTYP PSTAMT;
LABEL LNAME='Last Name' TRXEFF='CP Effective Date' 
	TRXTYP='Payment Type' PSTAMT='Amount Posted';
FORMAT BSSN SSN11. TRXEFF MMDDYY10. PSTAMT DOLLAR12.2	;
BY YY MM;
SUM PSTAMT;
SUMBY MM;
TITLE 'Consolidations Paid by FFEL - October 2001 to Present';
FOOTNOTE  'JOB = QUERY     REPORT = FFEL Consols June 01 + (58)';
RUN;