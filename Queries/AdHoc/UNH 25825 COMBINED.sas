%LET BANA = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\EA27_BANA.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME BANA ODBC &BANA ;

/*TEST*/
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK  ;*/

/*LIVE*/
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;

RSUBMIT;  
/*%let DB = DLGSWQUT;  *This is test;*/
%let DB = DLGSUTWH;  *This is live;

LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1;
PROC SQL;
	CREATE TABLE POP AS
		SELECT
			LN30.*
		FROM
			 OLWHRM1.LN30_LON_ERR LN30
		INNER JOIN
		(
			select
				*
			from 
				OLWHRM1.ln10_lon
			where
				PF_MAJ_BCH in ( '2016047001','2016047002','2016047003','2016047004')
				AND LA_CUR_PRI > 0
		)LN10
			ON LN30.BF_SSN = LN10.BF_SSN
			AND LN30.LN_SEQ = LN10.LN_SEQ
		WHERE
			LN30.PF_ERR_MSG = '03866'
			order by
				bf_ssn
;
QUIT;
ENDRSUBMIT;

DATA POP;
SET DUSTER.POP;
RUN;

PROC SQL;
	CREATE TABLE LOANS AS 
		SELECT DISTINCT
			BF_SSN,
			MAP.LN_SEQ,
			MAP.LOAN_NUMBER
		FROM
			POP P
		INNER JOIN BANA.COMPASSLOANMAPPING MAP
			ON P.BF_SSN = MAP.BORROWERSSN
			AND P.LN_SEQ = MAP.LN_SEQ
;
QUIT;

PROC SQL;
	CREATE TABLE DATA AS 
		SELECT
			L.*,
			COALESCE(F.PFHBeginDate, D.FirstDueDateCurrentRps) as PFHBeginDate,
			COALESCE(F.PFHEndDate,D.FirstDueDateCurrentRps) as PFHEndDate,
			coalesce(D.FirstDueDateCurrentRPS, datetime()) as FirstDueDateCurrentRPS,
			D.NextPaymentDueDate,
			T.RepaymentPlanCode,
			p.PartialFinancialHardshipAmount,
			p.PermanentStandardPayAmount,
			DC.InterestRate,
			A.BORROWERSTATE,
			BAL.PrincipalBalanceOutstanding,
			S.IBRForgiveStartDate,
			Term1CurrentRPS,
		    Amount1CurrentRPS,
		    Term2CurrentRPS,
		    Amount2CurrentRPS,
		    Term3CurrentRPS,
		    Amount3CurrentRPS,
		    Term4CurrentRPS,
		    Amount4CurrentRPS,
		    Term5CurrentRPS,
		    Amount5CurrentRPS,	
			Term6CurrentRPS,
      		Amount6CurrentRPS,
		    Term7CurrentRPS,
		    Amount7CurrentRPS,
		    Term8CurrentRPS,
		    Amount8CurrentRPS,
		    Term9CurrentRPS,
		    Amount9CurrentRPS,
		    Term10CurrentRPS,
		    Amount10CurrentRPS
		FROM
			LOANS L
			INNER JOIN BANA._03PaymentDataRecord T
				ON T.BORROWERSSN = L.BF_SSN
				AND T.LOAN_NUMBER = L.LOAN_NUMBER
			INNER JOIN BANA._05SupplementalBorrowerRecord S
				ON S.BORROWERSSN = L.BF_SSN
				AND S.LOAN_NUMBER = L.LOAN_NUMBER
			INNER JOIN BANA._07_08DisbClaimEnrollRecord DC
				ON DC.BORROWERSSN = L.BF_SSN
				AND DC.LOAN_NUMBER = L.LOAN_NUMBER
			INNER JOIN BANA._06BorrowerAddressRecord A
				ON A.BORROWERSSN = T.BORROWERSSN 
			INNER JOIN
			(
				SELECT
					BORROWERSSN,
					LOAN_NUMBER,
					SUM(PrincipalBalanceOutstanding) AS PrincipalBalanceOutstanding
				FROM
					BANA._07_08DisbClaimEnrollRecord
				GROUP BY
					BORROWERSSN,
					LOAN_NUMBER
			)BAL
				ON BAL.BORROWERSSN = DC.BORROWERSSN
				AND BAL.LOAN_NUMBER = DC.LOAN_NUMBER
			INNER JOIN
			(
				SELECT
					BORROWERSSN,
					SUM(PartialFinancialHardshipAmount) AS PartialFinancialHardshipAmount,
					SUM(PermanentStandardPayAmount) as PermanentStandardPayAmount
				FROM
					BANA._05SupplementalBorrowerRecord
				GROUP BY
					BORROWERSSN
			)p
				ON p.BORROWERSSN = DC.BORROWERSSN
			LEFT JOIN
			(
				SELECT
					BORROWERSSN,
					LOAN_NUMBER,
					MIN(PFHBeginDate) AS PFHBeginDate,
					MIN(PFHEndDate) as PFHEndDate
				FROM
					BANA._05SupplementalBorrowerRecord 
				GROUP BY
					BORROWERSSN,
					LOAN_NUMBER
			) F
				ON F.BORROWERSSN = L.BF_SSN
				AND F.LOAN_NUMBER = L.LOAN_NUMBER
			LEFT JOIN
			(
				SELECT
					BORROWERSSN,
					MIN(NextPaymentDueDate) AS NextPaymentDueDate,
					min(FirstDueDateCurrentRPS) as FirstDueDateCurrentRPS
				FROM
					BANA._03PaymentDataRecord
				GROUP BY
					BORROWERSSN
			)D
				ON D.BORROWERSSN = L.BF_SSN	
		
;
QUIT;

PROC SQL;
	CREATE TABLE TERMS_ERROR AS
		SELECT
			BORROWERSSN
		FROM
			BANA._03PaymentDataRecord
		WHERE
			INTNX('month',DATEPART(TODAY()),6,'same') >= NextPaymentDueDate
;
QUIT;
		


DATA DUSTER.DATA;
SET DATA;
RUN;

RSUBMIT;  
LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1;
PROC SQL;
	CREATE TABLE RS10 AS
		SELECT DISTINCT
			D.BF_SSN,
			MAX(COALESCE(RS10.LN_RPS_SEQ,0) + 1) AS LN_RPS_SEQ
		FROM
			 DATA D
		LEFT JOIN OLWHRM1.RS10_BR_RPD RS10
			ON D.BF_SSN = RS10.BF_SSN
		GROUP BY 
			D.BF_SSN
		ORDER BY 
			D.BF_SSN
;
QUIT;
ENDRSUBMIT;		

DATA RS10;
SET DUSTER.RS10;
RUN;


PROC SQL;
	CREATE TABLE RS10_FINAL AS 
		SELECT DISTINCT
			D.BF_SSN,
			R.LN_RPS_SEQ,
			CASE
				WHEN D.RepaymentPlanCode = 'IB' THEN DATEPART(D.PFHBeginDate)
				WHEN D.RepaymentPlanCode = 'IP' THEN INTNX('month',DATEPART(D.PFHBeginDate),1,'same')
				ELSE DATEPART(D.NextPaymentDueDate)
			END AS LD_STA_RPST10,
			'A' AS LC_STA_RPST10,
			'M' AS LC_FRQ_PAY,
			'N' AS LI_SIG_RPD_DIS,
			CASE
				
				WHEN D.RepaymentPlanCode IN ('IB','IP') AND  DAY(DATEPART(D.PFHBeginDate)) = 31 THEN INTNX('day' ,DATEPART(D.PFHBeginDate), -3, 'same')
				WHEN D.RepaymentPlanCode IN ('IB','IP') AND DAY(DATEPART(D.PFHBeginDate)) = 30 THEN INTNX('day' ,DATEPART(D.PFHBeginDate), -2, 'same')
				WHEN D.RepaymentPlanCode IN ('IB','IP') AND DAY(DATEPART(D.PFHBeginDate)) = 29 THEN INTNX('day' ,DATEPART(D.PFHBeginDate), -1, 'same')
				WHEN D.RepaymentPlanCode IN ('IB','IP') THEN DATEPART(D.PFHBeginDate)
				ELSE DATEPART(D.NextPaymentDueDate)
			END AS LD_RPS_1_PAY_DU,
			CASE
				WHEN D.RepaymentPlanCode IN ('IB','IP') THEN D.RepaymentPlanCode 
				WHEN D.RepaymentPlanCode IN ('D4','G') THEN 'G'
				ELSE 'L'
			END AS LC_RPD_DIS,
			CASE
				WHEN D.RepaymentPlanCode IN ('IP','IB') THEN INTNX('month',DATEPART(D.PFHBeginDate),-1,'same')
				ELSE INTNX('month',DATEPART(D.NextPaymentDueDate),-1,'same')
			END AS LD_SNT_RPD_DIS,
			'NULL' as LD_RTN_RPD_DIS,
			'DIG TIMESTAMP' AS LF_LST_DTS_RS10,
			'NULL' AS LC_RPS_OPT_PRT,
			'DCR' AS LF_USR_RPS_REQ ,
			MONTH(DATEPART(COALESCE(D.NextPaymentDueDate, D.PFHBeginDate))) AS LN_BR_REQ_DU_DAY,
			'NULL' as BD_CRT_RS05,
			'NULL' as BN_IBR_SEQ,
			'NULL' AS LC_RPY_FIX_TRM_AMT,
			'HM' AS LC_CAP_TRG_LVE_PFH 
		FROM
			DATA D
			INNER JOIN RS10 R
				ON RS10.BF_SSN = D.BF_SSN
;
QUIT;

DATA RS10_FINAL;
SET RS10_FINAL;
FORMAT LD_STA_RPST10 MMDDYY10.;
FORMAT LD_RPS_1_PAY_DU MMDDYY10.;
FORMAT LD_SNT_RPD_DIS MMDDYY10.;
RUN;

PROC SQL;
	CREATE TABLE RS05_FINAL AS
		SELECT DISTINCT
			D.BF_SSN,
			CASE WHEN D.repaymentPlanCode = 'IB' THEN DATEPART(D.PFHBeginDate)
				 WHEN D.repaymentPlanCode = 'IP' THEN INTNX('month',DATEPART(D.PFHEndDate),1) 
			END AS BD_CRT_RS05,
			1 AS BN_IBR_SEQ,
			'DCR' AS BF_CRT_USE_RS05,
			YEAR(DATEPART(D.PFHBeginDate)) AS BF_CRY_YR,
			D.BorrowerState AS BC_ST_IBR,
			'A' AS BC_STA_RS05,
			CASE WHEN D.repaymentPlanCode = 'IB' AND D.BorrowerState = 'AK' THEN (((D.PartialFinancialHardshipAmount*12)/0.15)+29880)   
                 WHEN D.repaymentPlanCode = 'IB' AND D.BorrowerState = 'HI' THEN (((D.PartialFinancialHardshipAmount*12)/0.15)+27495)   
				 WHEN D.repaymentPlanCode= 'IB' THEN (((D.PartialFinancialHardshipAmount*12)/0.15)+23895)   
				 WHEN D.repaymentPlanCode = 'IP' THEN 10000 END AS BA_AGI,
			2 AS BN_MEM_HSE_HLD,
			D.PermanentStandardPayAmount AS BA_PMN_STD_TOT_PAY,
			'TAX' AS BC_IBR_INF_SRC_VER,
			'DIG Timestamp' AS BF_LST_DTS_RS05,
			'NULL' AS BF_SSN_SPO,
			'NULL' AS BC_IRS_TAX_FIL_STA,
			'NULL' AS BI_JNT_BR_SPO_RPY,
			INTNX('month',DATEPART(D.PFHBeginDate),12) AS BD_ANV_QLF_IBR,
			'NULL' AS BC_DOC_SNT_BR_IDR
		FROM
			DATA D
		WHERE
			D.repaymentPlanCode IN ('IB','IP')
;
QUIT;

DATA RS05_FINAL;
SET RS05_FINAL;
FORMAT BD_CRT_RS05 MMDDYY10.;
FORMAT BD_ANV_QLF_IBR MMDDYY10.;
FORMAT BA_AGI 12.2;
FORMAT BA_PMN_STD_TOT_PAY 12.2;
RUN;

PROC SQL;
	CREATE TABLE RS20_FINAL AS
		SELECT DISTINCT
			D.BF_SSN,
			CASE WHEN D.repaymentPlanCode = 'IB' THEN DATEPART(D.PFHBeginDate)
				 WHEN D.repaymentPlanCode = 'IP' THEN INTNX('month',DATEPART(D.PFHEndDate),1) 
			END AS BD_CRT_RS05,
			1 AS BN_IBR_SEQ,
			D.LN_SEQ AS LN_SEQ,
			D.PartialFinancialHardshipAmount AS LA_PFH_PAY,
			D.PermanentStandardPayAmount AS LA_PMT_STD_PAY,
			'DIG Timestamp' AS BF_LST_DTS_RS20,
			D.PrincipalBalanceOutstanding AS LA_OTS_PRI_IBR_ADD,
			'NULL' AS LA_OTS_NSI_IBR_ADD,
			'NULL' AS LA_ANN_PAY_IBR_CLC
		FROM
			DATA D
		WHERE
			D.PrincipalBalanceOutstanding > 0
			and D.repaymentPlanCode IN ('IB','IP')
;
QUIT;

DATA RS20_FINAL;
SET RS20_FINAL;
FORMAT BD_CRT_RS05 MMDDYY10.;
FORMAT LA_PFH_PAY 12.2;
FORMAT LA_PMT_STD_PAY 12.2;
RUN;

PROC EXPORT DATA = WORK.RS20_FINAL 
            OUTFILE = "T:\NH 25825.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RS20 ADD"; 
RUN;


PROC EXPORT DATA = WORK.RS05_FINAL 
            OUTFILE = "T:\NH 25825.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RS05 ADD"; 
RUN;

PROC SQL;
	CREATE TABLE LN65 AS 
		SELECT DISTINCT
			D.BF_SSN,
			D.LN_SEQ,
			R.LN_RPS_SEQ,
			'NULL' AS LA_RPD_INT_DIS,
			D.INTERESTRATE AS LR_APR_RPD_DIS ,
			'NULL' AS LA_TOT_RPD_DIS ,
			D.PrincipalBalanceOutstanding AS LA_CPI_RPD_DIS ,
			'NULL' AS LR_INT_RPD_DIS ,
			'NULL' AS LA_ANT_CAP ,
			'NULL' AS LD_GRC_PRD_END ,
			CASE 
				WHEN D.RepaymentPlanCode = 'IB' THEN DATEPART(D.PFHBeginDate)
				WHEN D.RepaymentPlanCode = 'IP' THEN intnx('month',DATEPART(D.PFHEndDate ),1, 'same')
				ELSE DATEPART(D.NextPaymentDueDate)
			END AS LD_CRT_LON65,
			'A' AS LC_STA_LON65,
			'DIG TIMESTAMP' AS LF_LST_DTS_LN65,
			CASE
				WHEN D.RepaymentPlanCode IN ('IB','IP') THEN D.RepaymentPlanCode 
				WHEN D.RepaymentPlanCode IN ('D4','G') THEN 'G'
				ELSE 'L'
			END AS LC_TYP_SCH_DIS,
			'NULL' AS LA_ACR_INT_RPD,
			'NULL' AS LA_ANT_SUP_FEE,
			'NULL' AS LN_RPD_MAX_TRM_REQ,
			'NULL' AS LD_RPD_MAX_TRM_SR,
			'NULL' AS LC_RPD_INA_REA,
			1 AS LC_RPD_DIS,
			'NULL' as LR_CLC_INC_SCH,
			'NULL' as LA_CLC_RPY_SCH,
			'N' AS LI_ICR_RPD_NEG_AMR 
		FROM
			DATA D
			INNER JOIN RS10 R
				ON RS10.BF_SSN = D.BF_SSN 
;
QUIT;

DATA LN65_FINAL;
	SET LN65;
	FORMAT LD_CRT_LON65 MMDDYY10.;
	FORMAT LA_CPI_RPD_DIS 12.2;
RUN;

PROC SQL;
	CREATE TABLE LN66_FINAL AS 
		SELECT DISTINCT
			D.BF_SSN,
			D.LN_SEQ,
			R.LN_RPS_SEQ,
			1 AS LN_GRD_RPS_SEQ,
			CASE
				WHEN D.RepaymentPlanCode IN ('IB','IP') THEN D.PartialFinancialHardshipAmount 
				ELSE D.Amount1CurrentRPS
			END AS LA_RPS_ISL,
			CASE 
				WHEN D.RepaymentPlanCode = 'IB' THEN DATEPART(D.PFHBeginDate)
				WHEN D.RepaymentPlanCode = 'IP' THEN intnx('month',DATEPART(D.PFHEndDate ),1, 'same')
				ELSE DATEPART(D.NextPaymentDueDate)
			END AS LD_CRT_LON66,
			CASE 
				WHEN D.RepaymentPlanCode IN ('IB','IP') THEN 12
				ELSE D.Term1CurrentRPS
			END AS LN_RPS_TRM,
			'DIG TIMESTAMP' AS LF_LST_DTS_LN66,
			'NULL' AS LA_PRI_RDC_GRD,
			'NULL' AS LN_PRI_RDC_GRD_TRM,
			'NULL' as LA_PRI_ATU_PAY,
			'NULL' as LD_RPYE_FGV

		FROM
			DATA D
			INNER JOIN RS10 R
				ON RS10.BF_SSN = D.BF_SSN 

 	UNION ALL

		SELECT DISTINCT
			D.BF_SSN,
			D.LN_SEQ,
			R.LN_RPS_SEQ,
			2 AS LN_GRD_RPS_SEQ,
			CASE
				WHEN D.RepaymentPlanCode IN ('IB','IP') THEN D.PermanentStandardPayAmount
				ELSE D.Amount2CurrentRPS
			END AS LA_RPS_ISL,
			CASE 
				WHEN D.RepaymentPlanCode = 'IB' THEN DATEPART(D.PFHBeginDate)
				WHEN D.RepaymentPlanCode = 'IP' THEN intnx('month',DATEPART(D.PFHEndDate ),1, 'same')
				ELSE DATEPART(D.NextPaymentDueDate)
			END AS LD_CRT_LON66,
			CASE 
				WHEN D.RepaymentPlanCode IN ('IB','IP') THEN  CEIL((300 - ((TODAY() - DATEPART(D.IBRForgiveStartDate)) /365 * 12)))
				ELSE D.Term2CurrentRPS
			END AS LN_RPS_TRM,
			'DIG TIMESTAMP' AS LF_LST_DTS_LN66,
			'NULL' AS LA_PRI_RDC_GRD,
			'NULL' AS LN_PRI_RDC_GRD_TRM,
			'NULL' as LA_PRI_ATU_PAY,
			'NULL' as LD_RPYE_FGV

		FROM
			DATA D
			INNER JOIN RS10 R
				ON RS10.BF_SSN = D.BF_SSN 


	 UNION ALL

		SELECT DISTINCT
			D.BF_SSN,
			D.LN_SEQ,
			R.LN_RPS_SEQ,
			3 AS LN_GRD_RPS_SEQ,
			CASE
				WHEN D.RepaymentPlanCode IN ('IB','IP') THEN D.PermanentStandardPayAmount
				ELSE D.Amount3CurrentRPS
			END AS LA_RPS_ISL,
			CASE 
				WHEN D.RepaymentPlanCode = 'IB' THEN DATEPART(D.PFHBeginDate)
				WHEN D.RepaymentPlanCode = 'IP' THEN intnx('month',DATEPART(D.PFHEndDate ),1, 'same')
				ELSE DATEPART(D.NextPaymentDueDate)
			END AS LD_CRT_LON66,
			CASE 
				WHEN D.RepaymentPlanCode IN ('IB','IP') THEN  CEIL((300 - ((TODAY() - DATEPART(D.IBRForgiveStartDate)) /365 * 12)))
				ELSE D.Term3CurrentRPS
			END AS LN_RPS_TRM,
			'DIG TIMESTAMP' AS LF_LST_DTS_LN66,
			'NULL' AS LA_PRI_RDC_GRD,
			'NULL' AS LN_PRI_RDC_GRD_TRM,
			'NULL' as LA_PRI_ATU_PAY,
			'NULL' as LD_RPYE_FGV

		FROM
			DATA D
			INNER JOIN RS10 R
				ON RS10.BF_SSN = D.BF_SSN 
		WHERE
			D.RepaymentPlanCode IN ('D4','G')
			AND D.Amount3CurrentRPS > 0

	UNION ALL

		SELECT DISTINCT
			D.BF_SSN,
			D.LN_SEQ,
			R.LN_RPS_SEQ,
			4 AS LN_GRD_RPS_SEQ,
			CASE
				WHEN D.RepaymentPlanCode IN ('IB','IP') THEN D.PermanentStandardPayAmount
				ELSE D.Amount4CurrentRPS
			END AS LA_RPS_ISL,
			CASE 
				WHEN D.RepaymentPlanCode = 'IB' THEN DATEPART(D.PFHBeginDate)
				WHEN D.RepaymentPlanCode = 'IP' THEN intnx('month',DATEPART(D.PFHEndDate ),1, 'same')
				ELSE DATEPART(D.NextPaymentDueDate)
			END AS LD_CRT_LON66,
			CASE 
				WHEN D.RepaymentPlanCode IN ('IB','IP') THEN  CEIL((300 - ((TODAY() - DATEPART(D.IBRForgiveStartDate)) /365 * 12)))
				ELSE D.Term4CurrentRPS
			END AS LN_RPS_TRM,
			'DIG TIMESTAMP' AS LF_LST_DTS_LN66,
			'NULL' AS LA_PRI_RDC_GRD,
			'NULL' AS LN_PRI_RDC_GRD_TRM,
			'NULL' as LA_PRI_ATU_PAY,
			'NULL' as LD_RPYE_FGV

		FROM
			DATA D
			INNER JOIN RS10 R
				ON RS10.BF_SSN = D.BF_SSN 
		WHERE
			D.RepaymentPlanCode IN ('D4','G')
			AND D.Amount4CurrentRPS > 0

	UNION ALL

		SELECT DISTINCT
			D.BF_SSN,
			D.LN_SEQ,
			R.LN_RPS_SEQ,
			5 AS LN_GRD_RPS_SEQ,
			CASE
				WHEN D.RepaymentPlanCode IN ('IB','IP') THEN D.PermanentStandardPayAmount
				ELSE D.Amount5CurrentRPS
			END AS LA_RPS_ISL,
			CASE 
				WHEN D.RepaymentPlanCode = 'IB' THEN DATEPART(D.PFHBeginDate)
				WHEN D.RepaymentPlanCode = 'IP' THEN intnx('month',DATEPART(D.PFHEndDate ),1, 'same')
				ELSE DATEPART(D.NextPaymentDueDate)
			END AS LD_CRT_LON66,
			CASE 
				WHEN D.RepaymentPlanCode IN ('IB','IP') THEN  CEIL((300 - ((TODAY() - DATEPART(D.IBRForgiveStartDate)) /365 * 12)))
				ELSE D.Term5CurrentRPS
			END AS LN_RPS_TRM,
			'DIG TIMESTAMP' AS LF_LST_DTS_LN66,
			'NULL' AS LA_PRI_RDC_GRD,
			'NULL' AS LN_PRI_RDC_GRD_TRM,
			'NULL' as LA_PRI_ATU_PAY,
			'NULL' as LD_RPYE_FGV

		FROM
			DATA D
			INNER JOIN RS10 R
				ON RS10.BF_SSN = D.BF_SSN 
		WHERE
			D.RepaymentPlanCode IN ('D4','G')
			AND D.Amount5CurrentRPS > 0

	UNION ALL

		SELECT DISTINCT
			D.BF_SSN,
			D.LN_SEQ,
			R.LN_RPS_SEQ,
			6 AS LN_GRD_RPS_SEQ,
			CASE
				WHEN D.RepaymentPlanCode IN ('IB','IP') THEN D.PermanentStandardPayAmount
				ELSE D.Amount6CurrentRPS
			END AS LA_RPS_ISL,
			CASE 
				WHEN D.RepaymentPlanCode = 'IB' THEN DATEPART(D.PFHBeginDate)
				WHEN D.RepaymentPlanCode = 'IP' THEN intnx('month',DATEPART(D.PFHEndDate ),1, 'same')
				ELSE DATEPART(D.NextPaymentDueDate)
			END AS LD_CRT_LON66,
			CASE 
				WHEN D.RepaymentPlanCode IN ('IB','IP') THEN  CEIL((300 - ((TODAY() - DATEPART(D.IBRForgiveStartDate)) /365 * 12)))
				ELSE D.Term6CurrentRPS
			END AS LN_RPS_TRM,
			'DIG TIMESTAMP' AS LF_LST_DTS_LN66,
			'NULL' AS LA_PRI_RDC_GRD,
			'NULL' AS LN_PRI_RDC_GRD_TRM,
			'NULL' as LA_PRI_ATU_PAY,
			'NULL' as LD_RPYE_FGV

		FROM
			DATA D
			INNER JOIN RS10 R
				ON RS10.BF_SSN = D.BF_SSN 
		WHERE
			D.RepaymentPlanCode IN ('D4','G')
			AND D.Amount6CurrentRPS > 0

	UNION ALL

		SELECT DISTINCT
			D.BF_SSN,
			D.LN_SEQ,
			R.LN_RPS_SEQ,
			7 AS LN_GRD_RPS_SEQ,
			CASE
				WHEN D.RepaymentPlanCode IN ('IB','IP') THEN D.PermanentStandardPayAmount
				ELSE D.Amount7CurrentRPS
			END AS LA_RPS_ISL,
			CASE 
				WHEN D.RepaymentPlanCode = 'IB' THEN DATEPART(D.PFHBeginDate)
				WHEN D.RepaymentPlanCode = 'IP' THEN intnx('month',DATEPART(D.PFHEndDate ),1, 'same')
				ELSE DATEPART(D.NextPaymentDueDate)
			END AS LD_CRT_LON66,
			CASE 
				WHEN D.RepaymentPlanCode IN ('IB','IP') THEN  CEIL((300 - ((TODAY() - DATEPART(D.IBRForgiveStartDate)) /365 * 12)))
				ELSE D.Term7CurrentRPS
			END AS LN_RPS_TRM,
			'DIG TIMESTAMP' AS LF_LST_DTS_LN66,
			'NULL' AS LA_PRI_RDC_GRD,
			'NULL' AS LN_PRI_RDC_GRD_TRM,
			'NULL' as LA_PRI_ATU_PAY,
			'NULL' as LD_RPYE_FGV

		FROM
			DATA D
			INNER JOIN RS10 R
				ON RS10.BF_SSN = D.BF_SSN 
		WHERE
			D.RepaymentPlanCode IN ('D4','G')
			AND D.Amount7CurrentRPS > 0

	UNION ALL

		SELECT DISTINCT
			D.BF_SSN,
			D.LN_SEQ,
			R.LN_RPS_SEQ,
			8 AS LN_GRD_RPS_SEQ,
			CASE
				WHEN D.RepaymentPlanCode IN ('IB','IP') THEN D.PermanentStandardPayAmount
				ELSE D.Amount8CurrentRPS
			END AS LA_RPS_ISL,
			CASE 
				WHEN D.RepaymentPlanCode = 'IB' THEN DATEPART(D.PFHBeginDate)
				WHEN D.RepaymentPlanCode = 'IP' THEN intnx('month',DATEPART(D.PFHEndDate ),1, 'same')
				ELSE DATEPART(D.NextPaymentDueDate)
			END AS LD_CRT_LON66,
			CASE 
				WHEN D.RepaymentPlanCode IN ('IB','IP') THEN  CEIL((300 - ((TODAY() - DATEPART(D.IBRForgiveStartDate)) /365 * 12)))
				ELSE D.Term8CurrentRPS
			END AS LN_RPS_TRM,
			'DIG TIMESTAMP' AS LF_LST_DTS_LN66,
			'NULL' AS LA_PRI_RDC_GRD,
			'NULL' AS LN_PRI_RDC_GRD_TRM,
			'NULL' as LA_PRI_ATU_PAY,
			'NULL' as LD_RPYE_FGV

		FROM
			DATA D
			INNER JOIN RS10 R
				ON RS10.BF_SSN = D.BF_SSN 
		WHERE
			D.RepaymentPlanCode IN ('D4','G')
			AND D.Amount8CurrentRPS > 0

	UNION ALL

		SELECT DISTINCT
			D.BF_SSN,
			D.LN_SEQ,
			R.LN_RPS_SEQ,
			8 AS LN_GRD_RPS_SEQ,
			CASE
				WHEN D.RepaymentPlanCode IN ('IB','IP') THEN D.PermanentStandardPayAmount
				ELSE D.Amount8CurrentRPS
			END AS LA_RPS_ISL,
			CASE 
				WHEN D.RepaymentPlanCode = 'IB' THEN DATEPART(D.PFHBeginDate)
				WHEN D.RepaymentPlanCode = 'IP' THEN intnx('month',DATEPART(D.PFHEndDate ),1, 'same')
				ELSE DATEPART(D.NextPaymentDueDate)
			END AS LD_CRT_LON66,
			CASE 
				WHEN D.RepaymentPlanCode IN ('IB','IP') THEN  CEIL((300 - ((TODAY() - DATEPART(D.IBRForgiveStartDate)) /365 * 12)))
				ELSE D.Term8CurrentRPS
			END AS LN_RPS_TRM,
			'DIG TIMESTAMP' AS LF_LST_DTS_LN66,
			'NULL' AS LA_PRI_RDC_GRD,
			'NULL' AS LN_PRI_RDC_GRD_TRM,
			'NULL' as LA_PRI_ATU_PAY,
			'NULL' as LD_RPYE_FGV

		FROM
			DATA D
			INNER JOIN RS10 R
				ON RS10.BF_SSN = D.BF_SSN 
		WHERE
			D.RepaymentPlanCode IN ('D4','G')
			AND D.Amount8CurrentRPS > 0

	UNION ALL

		SELECT DISTINCT
			D.BF_SSN,
			D.LN_SEQ,
			R.LN_RPS_SEQ,
			9 AS LN_GRD_RPS_SEQ,
			CASE
				WHEN D.RepaymentPlanCode IN ('IB','IP') THEN D.PermanentStandardPayAmount
				ELSE D.Amount9CurrentRPS
			END AS LA_RPS_ISL,
			CASE 
				WHEN D.RepaymentPlanCode = 'IB' THEN DATEPART(D.PFHBeginDate)
				WHEN D.RepaymentPlanCode = 'IP' THEN intnx('month',DATEPART(D.PFHEndDate ),1, 'same')
				ELSE DATEPART(D.NextPaymentDueDate)
			END AS LD_CRT_LON66,
			CASE 
				WHEN D.RepaymentPlanCode IN ('IB','IP') THEN  CEIL((300 - ((TODAY() - DATEPART(D.IBRForgiveStartDate)) /365 * 12)))
				ELSE D.Term9CurrentRPS
			END AS LN_RPS_TRM,
			'DIG TIMESTAMP' AS LF_LST_DTS_LN66,
			'NULL' AS LA_PRI_RDC_GRD,
			'NULL' AS LN_PRI_RDC_GRD_TRM,
			'NULL' as LA_PRI_ATU_PAY,
			'NULL' as LD_RPYE_FGV

		FROM
			DATA D
			INNER JOIN RS10 R
				ON RS10.BF_SSN = D.BF_SSN 
		WHERE
			D.RepaymentPlanCode IN ('D4','G')
			AND D.Amount9CurrentRPS > 0

	UNION ALL

		SELECT DISTINCT
			D.BF_SSN,
			D.LN_SEQ,
			R.LN_RPS_SEQ,
			10 AS LN_GRD_RPS_SEQ,
			CASE
				WHEN D.RepaymentPlanCode IN ('IB','IP') THEN D.PermanentStandardPayAmount
				ELSE D.Amount10CurrentRPS
			END AS LA_RPS_ISL,
			CASE 
				WHEN D.RepaymentPlanCode = 'IB' THEN DATEPART(D.PFHBeginDate)
				WHEN D.RepaymentPlanCode = 'IP' THEN intnx('month',DATEPART(D.PFHEndDate ),1, 'same')
				ELSE DATEPART(D.NextPaymentDueDate)
			END AS LD_CRT_LON66,
			CASE 
				WHEN D.RepaymentPlanCode IN ('IB','IP') THEN  CEIL((300 - ((TODAY() - DATEPART(D.IBRForgiveStartDate)) /365 * 12)))
				ELSE D.Term10CurrentRPS
			END AS LN_RPS_TRM,
			'DIG TIMESTAMP' AS LF_LST_DTS_LN66,
			'NULL' AS LA_PRI_RDC_GRD,
			'NULL' AS LN_PRI_RDC_GRD_TRM,
			'NULL' as LA_PRI_ATU_PAY,
			'NULL' as LD_RPYE_FGV

		FROM
			DATA D
			INNER JOIN RS10 R
				ON RS10.BF_SSN = D.BF_SSN 
		WHERE
			D.RepaymentPlanCode IN ('D4','G')
			AND D.Amount10CurrentRPS > 0


;
QUIT;

DATA LN66_FINAL;
	SET LN66_FINAL;
	FORMAT LD_CRT_LON66 MMDDYY10.;
	FORMAT LA_RPS_ISL 12.2;
RUN;

PROC SQL;
	CREATE TABLE LN66_ERROR AS 
		SELECT DISTINCT
			bf_ssn,
			ln_seq
		FROM
			DATA
		WHERE 
			Amount1CurrentRPS = 0
			AND REPAYMENTPLANCODE NOT IN ('IB','IP') 	
;
QUIT;


PROC SQL;
	CREATE TABLE RS10_FINAL AS 
		SELECT DISTINCT
			RS10.*
		FROM
			RS10_FINAL RS10
			INNER JOIN LN66_FINAL LN66
				ON LN66.BF_SSN = RS10.BF_SSN
				AND LN66.LN_RPS_SEQ = RS10.LN_RPS_SEQ
;
	CREATE TABLE LN65_FINAL AS 
		SELECT DISTINCT
			LN65.*
		FROM
			LN65_FINAL LN65
			INNER JOIN LN66_FINAL LN66
				ON LN66.BF_SSN = LN65.BF_SSN
				AND LN66.LN_SEQ = LN65.LN_SEQ
				AND LN66.LN_RPS_SEQ = LN65.LN_RPS_SEQ
;
QUIT;



PROC EXPORT DATA = WORK.RS10_FINAL 
            OUTFILE = "T:\NH 25825.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RS10 ADD"; 
RUN;

PROC EXPORT DATA = WORK.LN65_FINAL 
            OUTFILE = "T:\NH 25825.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LN65 ADD"; 
RUN;

PROC EXPORT DATA = WORK.LN66_FINAL 
            OUTFILE = "T:\NH 25825.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LN66 ADD"; 
RUN;

PROC EXPORT DATA = WORK.LN66_ERROR 
            OUTFILE = "T:\NH 25825 LN66 ZERO PAYMENT.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LN66"; 
RUN;
