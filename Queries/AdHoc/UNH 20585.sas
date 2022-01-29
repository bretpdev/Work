LIBNAME DUSTER REMOTE SERVER=DUSTER SLIBREF=WORK;
RSUBMIT;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;

PROC SQL;
	CREATE TABLE FNLBLL AS
		SELECT DISTINCT
			LN80.BF_SSN,
			LN80.LN_SEQ,
			LN80.LD_BIL_CRT,
			LN80.LA_BIL_CUR_DU,
			LN66.LA_RPS_ISL,
			LN80.LA_CUR_PRN_BIL
			,LN10.LD_LON_EFF_ADD
		FROM
			OLWHRM1.LN80_LON_BIL_CRF LN80
			JOIN OLWHRM1.BL10_BR_BIL BL10
				ON LN80.BF_SSN = BL10.BF_SSN
				AND LN80.LD_BIL_CRT = BL10.LD_BIL_CRT
				AND LN80.LN_SEQ_BIL_WI_DTE = BL10.LN_SEQ_BIL_WI_DTE
			JOIN OLWHRM1.LN10_LON LN10
				ON LN80.BF_SSN = LN10.BF_SSN
				AND LN80.LN_SEQ = LN10.LN_SEQ
			LEFT JOIN OLWHRM1.RS10_BR_RPD RS10
				ON LN80.BF_SSN = RS10.BF_SSN
				AND RS10.LC_STA_RPST10 = 'A'
			LEFT JOIN OLWHRM1.LN65_LON_RPS LN65
				ON LN80.BF_SSN = LN65.BF_SSN
				AND LN80.LN_SEQ = LN65.LN_SEQ
				AND LN65.LC_STA_LON65 = 'A'
				AND RS10.LN_RPS_SEQ = LN65.LN_RPS_SEQ
			LEFT JOIN OLWHRM1.LN66_LON_RPS_SPF LN66
				ON LN65.BF_SSN = LN66.BF_SSN
				AND LN65.LN_SEQ = LN66.LN_SEQ
				AND LN65.LN_RPS_SEQ = LN66.LN_RPS_SEQ				
		WHERE
			LN80.LI_FNL_BIL_LON = 'Y'
			AND BL10.LC_IND_BIL_SNT = 'I'
/*			AND LN10.LD_LON_EFF_ADD = '24JUN2014'D*/
	;
QUIT;
ENDRSUBMIT;

DATA FNLBLL; SET DUSTER.FNLBLL; RUN;

PROC EXPORT
		DATA=FNLBLL
		OUTFILE='T:\SAS\Expanded Final Bill Borrower Query.xlsx'
		REPLACE;
RUN;
