LIBNAME Align ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\AlignImport.dsn; update_lock_typ=nolock; bl_keepnulls=no") ;

/*get data from align database for loans from source file*/
PROC SQL;
	CREATE TABLE GRGMPC AS
		SELECT DISTINCT
			MAP.br_ssn AS BF_SSN,
			MAP.CommpassLoanSeq AS LN_SEQ,		
			INPUT(LNS.ln_curr_principal,9.2) AS CURR_PRIN,
			INPUT(GR.Gr_Monthly_Pmt_Amt_Curr,9.2) AS LA_RPS_ISL,
			GR.GR_ID,
			GR.GR_NUM_TERM_PERIODS_ORIG
		FROM
			Align.CompassLoanMapping MAP
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

PROC SQL;
	CREATE TABLE WAF AS
		SELECT DISTINCT
			WA.BF_SSN,
			WA.LN_SEQ,
			WA.LA_RPS_ISL_WA,
			G.GR_NUM_TERM_PERIODS_ORIG AS REPAY_TRM
		FROM
			WA
			LEFT JOIN GRGMPC G
				ON WA.BF_SSN = G.BF_SSN
				AND WA.LN_SEQ = G.LN_SEQ
		ORDER BY
			BF_SSN,
			LN_SEQ
	;
QUIT;

PROC IMPORT OUT= WORK.SOURCE
            DATAFILE= "T:\LN66 INSERT - Align.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="LN66 insert$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

PROC SQL;
CREATE TABLE COMBO AS
	SELECT 
		S.*
		,W.LA_RPS_ISL_WA AS CURRENT_PAYMENT_AMT
		,W.REPAY_TRM
	FROM SOURCE S
		LEFT JOIN WAF W
			ON S.BF_SSN = W.BF_SSN
			AND S.LN_SEQ = W.LN_SEQ
;
QUIT;

/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWAB1.LWAB1RZ";
FILENAME REPORT2 "&RPTLIB/ULWAB1.LWAB1R2";
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;

DATA DUSTER.COMBO;
	SET COMBO;
RUN;

RSUBMIT;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;

PROC SQL;
	CONNECT TO DB2 (DATABASE=DLGSUTWH);

	CREATE TABLE FINAL AS
		SELECT DISTINCT
			C.*
			,LN65.LC_TYP_SCH_DIS
		FROM COMBO C
			INNER JOIN OLWHRM1.LN65_LON_RPS LN65
				ON C.BF_SSN = LN65.BF_SSN
				AND C.LN_SEQ = LN65.LN_SEQ
		WHERE LN65.LC_STA_LON65 = 'A'
;
	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA FINAL;
	SET DUSTER.FINAL;
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.FINAL
            OUTFILE = "T:\SAS\NH 20115.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
