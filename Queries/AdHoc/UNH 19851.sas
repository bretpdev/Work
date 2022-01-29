LIBNAME DUSTER REMOTE SERVER=DUSTER SLIBREF=WORK;
RSUBMIT;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;

PROC FORMAT;
	VALUE $LONPGM
		'CNSLDN' = 'Consolidation Loan'
		'PLUS' = 'Federal PLUS Loan'
		'PLUSGB' = 'Graduate PLUS'
		'SLS' = 'Federal SLS Loan'
		'STFFRD' = 'Federal Stafford Loan'
		'SUBCNS' = 'Subsidize Consolidation Loan'
		'UNCNS' = 'Unsubsidized Consolidation Loan'
		'UNSPC' = 'Unsubsidized Spousal Consolidation Loan'
		'UNSTFD' = 'Federal Unsubsidized Stafford Loan'
	;
QUIT;

PROC SQL;
	CREATE TABLE ECMC AS
		SELECT
			IF_GTR AS Guarantor,
			BF_SSN AS SSN,
			PUT(IC_LON_PGM, $LONPGM.) AS Loan_Type,
			LD_LON_GTR AS Guarantee_Date,
			LA_LON_AMT_GTR AS Guarantee_Amount,
			LD_LON_1_DSB AS First_Disbursement_Date,
			LF_LON_CUR_OWN AS Current_Lender_ID,
			'5/12/2014' AS Date_Loan_Sold,
			'700126' AS Current_Servicer_ID,
			'5/12/2014' AS Servicer_Responsibility_Date
		FROM
			OLWHRM1.LN10_LON
		WHERE
			IF_GTR IN ('000706','000751','000951','000755','000708')
	;
QUIT;
ENDRSUBMIT;



DATA ECMC; 	SET DUSTER.ECMC; RUN;

PROC EXPORT
		DATA=ECMC
		OUTFILE='T:\SAS\ECMC DATA.XLSX'
		REPLACE;
QUIT;

	
