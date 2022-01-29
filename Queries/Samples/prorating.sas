LIBNAME Align ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\AlignImport.dsn; update_lock_typ=nolock; bl_keepnulls=no") ;
LIBNAME DCRDATA 'Q:\Support Services\Align\DCR DATA\Error 02291.xlsx';

DATA SOURCE (KEEP=BF_SSN LOAN_SEQ);
	SET DCRDATA.'Sheet1$'n; 
	BF_SSN = PUT(BORROWER_SSN, Z9.);
RUN;

/*get data from align database for loans from source file*/
PROC SQL;
	CREATE TABLE GRGMPC AS
		SELECT DISTINCT
			S.BF_SSN,
			S.LOAN_SEQ AS LN_SEQ,		
			INPUT(LNS.ln_curr_principal,9.2) AS CURR_PRIN,
			INPUT(GR.Gr_Monthly_Pmt_Amt_Curr,9.2) AS LA_RPS_ISL,
			GR.GR_ID
		FROM
			SOURCE S
			JOIN Align.CompassLoanMapping MAP
				ON S.BF_SSN = MAP.br_ssn
				AND S.LOAN_SEQ = MAP.CommpassLoanSeq
			JOIN Align.ITELSQLDF_Loan LNS
				ON MAP.br_ssn = LNS.br_ssn
				AND MAP.NelNetLoanSeq = LNS.ln_num
			JOIN Align.ITEKSQLDF_Group GR
				ON LNS.br_ssn = GR.br_ssn
				AND LNS.gr_id = GR.gr_id	
	;
QUIT;

/*get total principal for each borrower/group*/
PROC SQL;
	CREATE TABLE PRIN_TTLS AS
		SELECT
			BF_SSN,
			GR_ID,
			SUM(CURR_PRIN) AS TTL_PRIN
		FROM
			GRGMPC
		GROUP BY
			BF_SSN,
			GR_ID
	;
QUIT;

/*add the total principal field to the other data fields*/
PROC SQL;
	CREATE TABLE ALL_DATA AS
		SELECT
			GR.BF_SSN,
			GR.LN_SEQ,
			GR.LA_RPS_ISL,
			GR.CURR_PRIN,
			PR.TTL_PRIN,
			GR.GR_ID
		FROM
			GRGMPC GR
			JOIN PRIN_TTLS PR
				ON GR.BF_SSN = PR.BF_SSN
				AND GR.GR_ID = PR.GR_ID
		ORDER BY
			BF_SSN,
			GR_ID
	;
QUIT;

/*prorate LA_RPS_ISL based on loan principal balance*/
DATA WA;
	SET ALL_DATA;
	BY BF_SSN GR_ID;

	RETAIN AMT_ALLOCATED 0;

/*	no proration needed if there is only one loan in the group*/
	IF FIRST.GR_ID AND LAST.GR_ID THEN LA_RPS_ISL_WA = LA_RPS_ISL;
/*	for the first loan in the group, calculate the prorated amount and set the amount allocated from the total amount to be allocated*/
	ELSE IF FIRST.GR_ID THEN 
		DO;
			LA_RPS_ISL_WA = ROUND(LA_RPS_ISL * CURR_PRIN/TTL_PRIN,.01);
			AMT_ALLOCATED = LA_RPS_ISL_WA;
		END;
/*	for the last loan, calcualte the prorated amount as the total amount to be prorated less the amount already prorated*/
	ELSE IF LAST.GR_ID THEN LA_RPS_ISL_WA = LA_RPS_ISL - AMT_ALLOCATED;
/*	for loans which are not first or last, calculate the prorated amount and add it to the amount allocated*/
	ELSE 
		DO;
			LA_RPS_ISL_WA = ROUND(LA_RPS_ISL * CURR_PRIN/TTL_PRIN,.01);
			AMT_ALLOCATED = AMT_ALLOCATED + LA_RPS_ISL_WA;				
		END;
RUN;

/*select and rename fields and order data for the final output*/
PROC SQL;
	CREATE TABLE WAF AS
		SELECT
			BF_SSN,
			LN_SEQ,
			LA_RPS_ISL_WA AS LA_RPS_ISL
		FROM
			WA
		ORDER BY
			BF_SSN,
			LN_SEQ
	;
QUIT;

PROC EXPORT
		DATA=WAF
		OUTFILE='T:\SAS\ERROR 02291.XLSX'
		REPLACE;
	SHEET='LA_PRS_ISL';
RUN;
