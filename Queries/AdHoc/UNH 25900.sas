/*TEST*/
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK  ;*/

/*LIVE*/
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;

%LET BANA = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\EA27_BANA.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME BANA ODBC &BANA ;

PROC SQL;
	CREATE TABLE CompassLoanMapping AS 
		SELECT 
			*
		FROM
			BANA.CompassLoanMapping;

	CREATE TABLE PaymentDataRecord AS 
		SELECT 
			*
		FROM
			BANA._03PaymentDataRecord;

	CREATE TABLE PRECLAIMS AS
		SELECT DISTINCT
			SSN,
			LOANIDENT,
			PCASTATUS,
			PCADATE
		FROM
			BANA.PRECLAIMS;

	CREATE TABLE PRECLAIMS_MAXDATE AS
		SELECT DISTINCT
			SSN,
			LOANIDENT,
			MAX(PCADATE) AS PCADATE
		FROM
			BANA.PRECLAIMS
		GROUP BY
			SSN,
			LOANIDENT;
QUIT;

DATA DUSTER.CompassLoanMapping;
	SET CompassLoanMapping;
RUN;
DATA DUSTER.PaymentDataRecord;
	SET PaymentDataRecord;
RUN;
DATA DUSTER.PRECLAIMS;
	SET PRECLAIMS;
RUN;
DATA DUSTER.PRECLAIMS_MAXDATE;
	SET PRECLAIMS_MAXDATE;
RUN;


RSUBMIT;  
/*%let DB = DLGSWQUT;  *This is test;*/
%let DB = DLGSUTWH;  *This is live;
LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1;
PROC SQL;
	CREATE TABLE LN40Rows AS
		SELECT DISTINCT
			LN10.BF_SSN,
			LN10.LN_SEQ,
			COALESCE(LN40.LN_SEQ_CLM_PCL,0) AS LN_SEQ_CLM_PCL, 
			/*•This field will need to pull the max LN_SEQ_CLM_PCL on the borrower level 
			they have ever had and increment it by one for each loan sequence we are creating 
			an LN40 record for. If the borrower has no LN_SEQ_CLM_PCL in their history this 
			can start at ‘1’ and increment by one for each additional loan sequence the borrower has*/
			LN10.LA_CUR_PRI AS LA_SBM_CLM_PCL_PRI,
			COALESCE(DW01.WA_TOT_BRI_OTS,DW01.LA_NSI_OTS,0.00) AS LA_SBM_CLM_PCL_INT,
			'NULL' AS LI_CLM_PKG_RTN_RCV,
			'NULL' AS LC_CLM_REJ_RTN_LIB,
			'NULL' AS LI_GTR_ACK_CLM_CAN,
			'NULL' AS LC_REA_CAN_CLM_PCL,
			PC.PCADATE AS LD_CRT_CLM_PCL,
			'NULL' AS LD_CLM_REJ_RTN_ACL,
			'NULL' AS LD_CLM_REJ_TRN_EFF,
			'NULL' AS LD_CLM_REJ_RTN_MAX,
			PC.PCADATE AS LD_SBM_CLM_PCL,
			'NULL' AS LD_CAN_CLM_PCL,
			'NULL' AS LC_TYP_REJ_RTN,
			'DIG Timestamp' AS LF_LST_DTS_LN40,
			'3' AS LC_TYP_REC_CLP_LON,
			'06' AS LC_REA_CLP_LON,
			'NULL' AS LI_TSK_CRT_RSI_BAL,
			COALESCE(LN40.LN_SEQ_CLM_PCL,0) AS LN_SEQ_CLM_PCL_ORG,
			/*•This field will need to pull the max LN_SEQ_CLM_PCL on the borrower level 
			they have ever had and increment it by one for each loan sequence we are creating 
			an LN40 record for. If the borrower has no LN_SEQ_CLM_PCL in their history this 
			can start at ‘1’ and increment by one for each additional loan sequence the borrower has*/
			DATEPART(PR.PriorServicerFirstUnpaidInstall) AS LD_CND_OCC,
			'NULL' AS LD_CLM_PD_PCV,
			'NULL' AS LA_CLM_PD_PCV,
			'NULL' AS LD_CLM_ORG_CRT,
			'NULL' AS LD_CLM_ORG_SBM,
			'NULL' AS LI_PCL_CLM_PCV,
			'NULL' AS LC_REA_CLM_REJ_RTN,
			'NULL' AS LC_SUP_PCA,
			'NULL' AS LD_OSD_CLM,
			'NULL' AS LD_NTF_OSD_CLM,
			'NULL' AS LI_RPD_CHG_CLM,
			'NULL' AS LD_1_PAY_DU_CLM,
			'NULL' AS LA_TOT_BR_PAY_CLM,
			'NULL' AS LN_MTH_PAY_CLM,
			'NULL' AS LN_MTH_DFR_CLM,
			'NULL' AS LN_MTH_FOR_CLM,
			'NULL' AS LN_MTH_VIO_CLM,
			'NULL' AS LN_DFR_FOR_EVT_CLM,
			'NULL' AS LN_MTH_RNV_CLM,
			'NULL' AS LD_PAY_DU_CLM,
			'NULL' AS LA_TOT_DSB_CLM,
			'NULL' AS LA_CAP_INT_CLM,
			'NULL' AS LA_PRI_RPD_CLM,
			'NULL' AS LA_CU_INT_CAP_CLM,
			'NULL' AS LD_INT_PD_THU_CLM,
			'NULL' AS LD_CLM_INT_CLM,
			'NULL' AS LA_UNP_INT_NO_CAP,
			'NULL' AS LD_CLM_REJ_LTR,
			'NULL' AS LA_DSD_RFD_CLM,
			'NULL' AS LD_CLM_PD_LTR,
			'NULL' AS LN_CCI_CLM_SEQ,
			'NULL' AS LD_CCI_LON_SLD,
			'NULL' AS LD_CCI_SER_RSB,
			'NULL' AS LD_XCP_PRF,
			'NULL' AS LA_CCI_UNP_FEE,
			'NULL' AS LA_CCI_UNP_INT,
			'NULL' AS LA_ITL_STD_PAY_CLM,
			'NULL' AS LA_PMN_STD_PAY_CLM,
			'NULL' AS LD_25_YR_FGV_CLM,
			'NULL' AS LN_MTH_QLF_FGV_CLM,
			'NULL' AS LD_IBR_SR_CLM,
			'NULL' AS LN_DAY_EHD_DFR_CLM
		FROM
			WORK.PRECLAIMS_MAXDATE PC 
			INNER JOIN OLWHRM1.LN10_LON LN10
				ON LN10.BF_SSN = PC.SSN
				AND LN10.LF_GTR_RFR_XTN = PC.LOANIDENT
			INNER JOIN WORK.CompassLoanMapping CLM
				ON CLM.BorrowerSSN = LN10.BF_SSN
				AND CLM.LN_SEQ = LN10.LN_SEQ
			INNER JOIN WORK.PaymentDataRecord PR
				ON PR.BorrowerSSN = CLM.BorrowerSSN
				AND PR.loan_number = CLM.loan_number
			LEFT OUTER JOIN OLWHRM1.DW01_DW_CLC_CLU DW01
				ON DW01.BF_SSN = LN10.BF_SSN
				AND DW01.LN_SEQ = LN10.LN_SEQ
			LEFT OUTER JOIN 
				(
					SELECT
						MAX(LN_SEQ_CLM_PCL) AS LN_SEQ_CLM_PCL,
						BF_SSN
					FROM
						OLWHRM1.LN40_LON_CLM_PCL
					GROUP BY
						BF_SSN
				)LN40 
				ON LN40.BF_SSN = LN10.BF_SSN
			LEFT OUTER JOIN OLWHRM1.CL10_CLM_PCL CL10
				ON CL10.BF_SSN = LN40.BF_SSN
				AND CL10.LN_SEQ_CLM_PCL = LN40.LN_SEQ_CLM_PCL
;

	CREATE TABLE LN40RowsNull AS
		SELECT DISTINCT
			PC.SSN AS BF_SSN,
			LN10.LN_SEQ,
			COALESCE(LN40.LN_SEQ_CLM_PCL,0) AS LN_SEQ_CLM_PCL, 
			/*•This field will need to pull the max LN_SEQ_CLM_PCL on the borrower level 
			they have ever had and increment it by one for each loan sequence we are creating 
			an LN40 record for. If the borrower has no LN_SEQ_CLM_PCL in their history this 
			can start at ‘1’ and increment by one for each additional loan sequence the borrower has*/
			LN10.LA_CUR_PRI AS LA_SBM_CLM_PCL_PRI,
			COALESCE(DW01.WA_TOT_BRI_OTS,DW01.LA_NSI_OTS,0.00) AS LA_SBM_CLM_PCL_INT,
			'NULL' AS LI_CLM_PKG_RTN_RCV,
			'NULL' AS LC_CLM_REJ_RTN_LIB,
			'NULL' AS LI_GTR_ACK_CLM_CAN,
			'NULL' AS LC_REA_CAN_CLM_PCL,
			PC.PCADATE AS LD_CRT_CLM_PCL,
			'NULL' AS LD_CLM_REJ_RTN_ACL,
			'NULL' AS LD_CLM_REJ_TRN_EFF,
			'NULL' AS LD_CLM_REJ_RTN_MAX,
			PC.PCADATE AS LD_SBM_CLM_PCL,
			'NULL' AS LD_CAN_CLM_PCL,
			'NULL' AS LC_TYP_REJ_RTN,
			'DIG Timestamp' AS LF_LST_DTS_LN40,
			'3' AS LC_TYP_REC_CLP_LON,
			'06' AS LC_REA_CLP_LON,
			'NULL' AS LI_TSK_CRT_RSI_BAL,
			COALESCE(LN40.LN_SEQ_CLM_PCL,0) AS LN_SEQ_CLM_PCL_ORG,
			/*•This field will need to pull the max LN_SEQ_CLM_PCL on the borrower level 
			they have ever had and increment it by one for each loan sequence we are creating 
			an LN40 record for. If the borrower has no LN_SEQ_CLM_PCL in their history this 
			can start at ‘1’ and increment by one for each additional loan sequence the borrower has*/
			DATEPART(PR.PriorServicerFirstUnpaidInstall) AS LD_CND_OCC,
			'NULL' AS LD_CLM_PD_PCV,
			'NULL' AS LA_CLM_PD_PCV,
			'NULL' AS LD_CLM_ORG_CRT,
			'NULL' AS LD_CLM_ORG_SBM,
			'NULL' AS LI_PCL_CLM_PCV,
			'NULL' AS LC_REA_CLM_REJ_RTN,
			'NULL' AS LC_SUP_PCA,
			'NULL' AS LD_OSD_CLM,
			'NULL' AS LD_NTF_OSD_CLM,
			'NULL' AS LI_RPD_CHG_CLM,
			'NULL' AS LD_1_PAY_DU_CLM,
			'NULL' AS LA_TOT_BR_PAY_CLM,
			'NULL' AS LN_MTH_PAY_CLM,
			'NULL' AS LN_MTH_DFR_CLM,
			'NULL' AS LN_MTH_FOR_CLM,
			'NULL' AS LN_MTH_VIO_CLM,
			'NULL' AS LN_DFR_FOR_EVT_CLM,
			'NULL' AS LN_MTH_RNV_CLM,
			'NULL' AS LD_PAY_DU_CLM,
			'NULL' AS LA_TOT_DSB_CLM,
			'NULL' AS LA_CAP_INT_CLM,
			'NULL' AS LA_PRI_RPD_CLM,
			'NULL' AS LA_CU_INT_CAP_CLM,
			'NULL' AS LD_INT_PD_THU_CLM,
			'NULL' AS LD_CLM_INT_CLM,
			'NULL' AS LA_UNP_INT_NO_CAP,
			'NULL' AS LD_CLM_REJ_LTR,
			'NULL' AS LA_DSD_RFD_CLM,
			'NULL' AS LD_CLM_PD_LTR,
			'NULL' AS LN_CCI_CLM_SEQ,
			'NULL' AS LD_CCI_LON_SLD,
			'NULL' AS LD_CCI_SER_RSB,
			'NULL' AS LD_XCP_PRF,
			'NULL' AS LA_CCI_UNP_FEE,
			'NULL' AS LA_CCI_UNP_INT,
			'NULL' AS LA_ITL_STD_PAY_CLM,
			'NULL' AS LA_PMN_STD_PAY_CLM,
			'NULL' AS LD_25_YR_FGV_CLM,
			'NULL' AS LN_MTH_QLF_FGV_CLM,
			'NULL' AS LD_IBR_SR_CLM,
			'NULL' AS LN_DAY_EHD_DFR_CLM
		FROM
			WORK.PRECLAIMS_MAXDATE PC 
			INNER JOIN WORK.CompassLoanMapping CLM
				ON CLM.BorrowerSSN = PC.SSN
			INNER JOIN WORK.PaymentDataRecord PR
				ON PR.BorrowerSSN = CLM.BorrowerSSN
				AND PR.loan_number = CLM.loan_number
			LEFT OUTER JOIN OLWHRM1.LN10_LON LN10
				ON LN10.BF_SSN = CLM.BorrowerSSN
				AND LN10.LN_SEQ = CLM.LN_SEQ
			LEFT OUTER JOIN OLWHRM1.DW01_DW_CLC_CLU DW01
				ON DW01.BF_SSN = LN10.BF_SSN
				AND DW01.LN_SEQ = LN10.LN_SEQ
			LEFT OUTER JOIN 
				(
					SELECT
						MAX(LN_SEQ_CLM_PCL) AS LN_SEQ_CLM_PCL,
						BF_SSN
					FROM
						OLWHRM1.LN40_LON_CLM_PCL
					GROUP BY
						BF_SSN
				)LN40 
				ON LN40.BF_SSN = LN10.BF_SSN
			LEFT OUTER JOIN OLWHRM1.CL10_CLM_PCL CL10
				ON CL10.BF_SSN = LN40.BF_SSN
				AND CL10.LN_SEQ_CLM_PCL = LN40.LN_SEQ_CLM_PCL
		WHERE
			LN10.BF_SSN IS NULL
;
QUIT; 
ENDRSUBMIT;

DATA LN40Rows;
SET DUSTER.LN40Rows;
FORMAT LD_CND_OCC MMDDYY10.;
FORMAT LA_SBM_CLM_PCL_INT 8.2;
RUN;

PROC SORT DATA=LN40Rows; BY BF_SSN LN_SEQ; RUN;

DATA LN40Rows;
SET LN40Rows;
BY BF_SSN;
IF FIRST. BF_SSN THEN N=LN_SEQ_CLM_PCL+1;
   ELSE N+1;
RUN;

DATA LN40Rows (drop=N);
SET LN40Rows;
LN_SEQ_CLM_PCL = N;
LN_SEQ_CLM_PCL_ORG = N;
RUN;

DATA LN40RowsNull;
SET DUSTER.LN40RowsNull;
FORMAT LD_CND_OCC MMDDYY10.;
FORMAT LA_SBM_CLM_PCL_INT 8.2;
RUN;

PROC SORT DATA=LN40RowsNull; BY BF_SSN LN_SEQ; RUN;

DATA LN40RowsNull;
SET LN40RowsNull;
BY BF_SSN;
IF FIRST. BF_SSN THEN N=LN_SEQ_CLM_PCL+1;
   ELSE N+1;
RUN;

DATA LN40RowsNull (drop=N);
SET LN40RowsNull;
LN_SEQ_CLM_PCL = N;
LN_SEQ_CLM_PCL_ORG = N;
RUN;

/* SEND MANIPULATED DATA BACK TO DUSTER*/
DATA DUSTER.LN40Rows;
SET LN40Rows;
RUN;

DATA DUSTER.LN40RowsNull;
SET LN40RowsNull;
RUN;

RSUBMIT;
PROC SQL;
CREATE TABLE CL10Rows AS
		SELECT DISTINCT
			LN40.BF_SSN,
			LN40.LN_SEQ_CLM_PCL AS LN_SEQ_CLM_PCL, 
			'06' AS LC_REA_CLM_PCL,
			'3' AS LC_TYP_REC_CLM_PCL,
			'NULL' AS LF_USR_ASN_CLM_PCL,
			'NULL' AS LI_CLM_GTR_RCV,
			INTNX('day',DATEPART(PR.PriorServicerFirstUnpaidInstall),120) AS LD_CLM_RQR,
			'NULL' AS LF_CLM_BCH,
			'DIG Timestamp' AS LF_LST_DTS_CL10,
			'NULL' AS LI_CLM_QA,
			CASE WHEN PC.PCASTATUS = 'A' THEN LN40.LD_CRT_CLM_PCL ELSE 'NULL' END AS LD_GTR_CLM_RCI,
			CASE WHEN PC.PCASTATUS = 'A' THEN 'A'
				 WHEN PC.PCASTATUS = 'S' THEN 'NULL'
				 WHEN PC.PCASTATUS = 'R' THEN 'J'
				 WHEN PC.PCASTATUS = 'C' THEN 'NULL'
				 ELSE 'NULL' END AS LC_GTR_CLM_ACK,
			'NULL' AS LC_CAN_STA_CCI,
			'NULL' AS LI_CLM_CLL_RCV,
			'NULL' AS LC_XCP_PRF
		FROM
			WORK.PRECLAIMS_MAXDATE PCD
			INNER JOIN WORK.PRECLAIMS PC
				ON PCD.SSN = PC.SSN
				AND PCD.LOANIDENT = PC.LOANIDENT
				AND PCD.PCADATE = PC.PCADATE
			INNER JOIN OLWHRM1.LN10_LON LN10
				ON LN10.BF_SSN = PCD.SSN
				AND LN10.LF_GTR_RFR_XTN = PCD.LOANIDENT
			INNER JOIN WORK.LN40Rows LN40
				ON LN40.BF_SSN = LN10.BF_SSN	
				AND LN40.LN_SEQ = LN10.LN_SEQ	
			INNER JOIN WORK.CompassLoanMapping CLM
				ON CLM.BorrowerSSN = LN40.BF_SSN
				AND CLM.LN_SEQ = LN40.LN_SEQ
			INNER JOIN WORK.PaymentDataRecord PR
				ON PR.BorrowerSSN = CLM.BorrowerSSN
				AND PR.loan_number = CLM.loan_number
;

CREATE TABLE CL10RowsNull AS
		SELECT DISTINCT
			LN40.BF_SSN,
			LN40.LN_SEQ_CLM_PCL AS LN_SEQ_CLM_PCL, 
			/*•This field will need to pull the max LN_SEQ_CLM_PCL on the borrower level 
			they have ever had and increment it by one for each loan sequence we are creating 
			an LN40 record for. If the borrower has no LN_SEQ_CLM_PCL in their history this 
			can start at ‘1’ and increment by one for each additional loan sequence the borrower has*/
			'06' AS LC_REA_CLM_PCL,
			'3' AS LC_TYP_REC_CLM_PCL,
			'NULL' AS LF_USR_ASN_CLM_PCL,
			'NULL' AS LI_CLM_GTR_RCV,
			'' AS LD_CLM_RQR,
			'NULL' AS LF_CLM_BCH,
			'DIG Timestamp' AS LF_LST_DTS_CL10,
			'NULL' AS LI_CLM_QA,
			CASE WHEN PC.PCASTATUS = 'A' THEN LN40.LD_CRT_CLM_PCL ELSE 'NULL' END AS LD_GTR_CLM_RCI,
			CASE WHEN PC.PCASTATUS = 'A' THEN 'A'
				 WHEN PC.PCASTATUS = 'S' THEN 'NULL'
				 WHEN PC.PCASTATUS = 'R' THEN 'J'
				 WHEN PC.PCASTATUS = 'C' THEN 'NULL'
				 ELSE 'NULL' END AS LC_GTR_CLM_ACK,
			'NULL' AS LC_CAN_STA_CCI,
			'NULL' AS LI_CLM_CLL_RCV,
			'NULL' AS LC_XCP_PRF
		FROM
			WORK.PRECLAIMS PC
			INNER JOIN WORK.PRECLAIMS_MAXDATE PCD
				ON PCD.SSN = PC.SSN
				AND PCD.LOANIDENT = PC.LOANIDENT
				AND PCD.PCADATE = PC.PCADATE
			INNER JOIN WORK.LN40RowsNull LN40
				ON LN40.BF_SSN = PC.SSN	
;

QUIT;
ENDRSUBMIT;

DATA CL10Rows;
SET DUSTER.CL10Rows;
FORMAT LD_CLM_RQR MMDDYY10.;
RUN;

PROC SORT DATA=CL10Rows; BY BF_SSN; RUN;

DATA CL10RowsNull;
SET DUSTER.CL10RowsNull;
RUN;

PROC SORT DATA=CL10RowsNull; BY BF_SSN; RUN;

PROC EXPORT DATA = WORK.LN40RowsNull 
            OUTFILE = "T:\NH 25900 LN40 Manual.csv" 
            DBMS = csv
			REPLACE;
RUN;

PROC EXPORT DATA = WORK.CL10RowsNull 
            OUTFILE = "T:\NH 25900 CL10 Manual.csv" 
            DBMS = csv
			REPLACE;
RUN;

PROC EXPORT DATA = WORK.LN40Rows 
            OUTFILE = "T:\NH 25900 LN40 DCR.csv" 
            DBMS = csv
			REPLACE;
RUN;

PROC EXPORT DATA = WORK.CL10Rows 
            OUTFILE = "T:\NH 25900 CL10 DCR.csv" 
            DBMS = csv
			REPLACE;
RUN;
