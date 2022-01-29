%LET BANA = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\EA27_BANA.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME BANA ODBC &BANA ;

PROC SQL;
	CREATE TABLE DATA AS 
		SELECT DISTINCT
			P.BORROWERSSN,
			SUM(S.PartialFinancialHardshipAmount) as PartialFinancialHardshipAmount,
			RepaymentPlanCode
		FROM	
			BANA._03PaymentDataRecord P
			INNER JOIN BANA._05SupplementalBorrowerRecord S
				ON S.BORROWERSSN = P.BORROWERSSN
				AND S.LOAN_NUMBER = P.LOAN_NUMBER
		WHERE
			P.RepaymentPlanCode IN ('IB','IP')
		GROUP BY
			P.BORROWERSSN,
			RepaymentPlanCode
;
QUIT;

/*TEST*/
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK  ;*/

/*LIVE*/
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;

DATA DUSTER.DATA;
SET DATA;
RUN;

RSUBMIT;  
/*%let DB = DLGSWQUT;  *This is test;*/
%let DB = DLGSUTWH;  *This is live;
LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1;
PROC SQL;
	CREATE TABLE RS10 AS
		SELECT DISTINCT
			LN66.BF_SSN,
			LN66.LN_SEQ,
			LN66.LN_RPS_SEQ,
			LN66.LN_GRD_RPS_SEQ,
			RS10.LC_RPD_DIS AS NEW_REPAYMENT_PLAN,
			d.RepaymentPlanCode as OLD_REPAYMENT_PLAN,
			D.PartialFinancialHardshipAmount AS EA27_PAYMENT,
			LN66.LA_RPS_ISL AS NEW_PAYMENT,
			LN10.LA_CUR_PRI,
            PD30.DC_DOM_ST,
            LN72.LR_ITR
		FROM
			 DATA D
		INNER JOIN OLWHRM1.RS10_BR_RPD RS10
			ON D.BORROWERSSN = RS10.BF_SSN
		INNER JOIN OLWHRM1.LN65_LON_RPS LN65
			ON LN65.BF_SSN = RS10.BF_SSN
			AND LN65.LN_RPS_SEQ = RS10.LN_RPS_SEQ
		INNER JOIN OLWHRM1.LN66_LON_RPS_SPF LN66
			ON LN66.BF_SSN = LN65.BF_SSN
			AND LN65.LN_SEQ = LN66.LN_SEQ
			AND LN65.LN_RPS_SEQ = LN66.LN_RPS_SEQ
		INNER JOIN OLWHRM1.LN10_LON LN10
        	ON LN66.BF_SSN = LN10.BF_SSN 
       		AND LN66.LN_SEQ = LN10.LN_SEQ
		JOIN OLWHRM1.LN72_INT_RTE_HST LN72
	        ON LN66.BF_SSN = LN72.BF_SSN
	        AND LN66.LN_SEQ = LN72.LN_SEQ
	        AND LN72.LC_STA_LON72 = 'A'
	        AND TODAY() BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
		LEFT JOIN OLWHRM1.PD30_PRS_ADR PD30
	        ON RS10.BF_SSN = PD30.DF_PRS_ID
	        AND PD30.DC_ADR = 'L'
		WHERE
			RS10.LC_RPD_DIS = 'L'
			AND RS10.LC_STA_RPST10 = 'A'
		
;
QUIT;
ENDRSUBMIT;	

DATA RS10;
SET DUSTER.RS10;
RUN;

PROC SQL;
	CREATE TABLE BWR_LEVEL AS 
		SELECT
			BF_SSN,
			SUM(EA27_PAYMENT) AS EA27_PAYMENT,
			SUM(NEW_PAYMENT) AS NEW_PAYMENT
		FROM
			RS10
		GROUP BY
			BF_SSN
;
QUIT;

PROC EXPORT DATA = WORK.RS10 
            OUTFILE = "T:\IBR BORROWRS ON LEVEL WITH LOAN INFO.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LOAN LEVEL"; 
RUN;

PROC EXPORT DATA = WORK.BWR_LEVEL 
            OUTFILE = "T:\IBR BORROWRS ON LEVEL WITH LOAN INFO.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="BORROWER LEVEL"; 
RUN;
