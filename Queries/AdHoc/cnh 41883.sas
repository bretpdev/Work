/*query based on CNH 32770*/
LIBNAME SQL_CDW ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CDW.dsn; bl_keepnulls=no; READ_ISOLATION_LEVEL=RU" SCHEMA= DBO;

%LET TODAY = '15may2020'D;

PROC SQL NOPRINT;
CREATE TABLE SSAE18_pulldown AS
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID
		,LN10.LN_SEQ
		,DW01.WC_DW_LON_STA
		,CASE
			WHEN WC_DW_LON_STA = '01' THEN 'In Grace'
			WHEN WC_DW_LON_STA = '02' THEN 'In School'
			WHEN WC_DW_LON_STA = '03' THEN 'In Repayment'
			WHEN WC_DW_LON_STA = '04' THEN 'In Deferment'
			WHEN WC_DW_LON_STA = '05' THEN 'In Forbearance'
			WHEN WC_DW_LON_STA = '06' THEN 'In Cure'
			WHEN WC_DW_LON_STA = '07' THEN 'Claim Pending'
			WHEN WC_DW_LON_STA = '08' THEN 'Claim Submitted'
			WHEN WC_DW_LON_STA = '09' THEN 'Claim Cancelled'
			WHEN WC_DW_LON_STA = '10' THEN 'Claim Reject'
			WHEN WC_DW_LON_STA = '11' THEN 'Claim Returned'
			WHEN WC_DW_LON_STA = '12' THEN 'Claim Paid'
			WHEN WC_DW_LON_STA = '13' THEN 'Pre-claim Pending'
			WHEN WC_DW_LON_STA = '14' THEN 'Preclaim submitted'
			WHEN WC_DW_LON_STA = '15' THEN 'Pre-claim Cancelled'
			WHEN WC_DW_LON_STA = '16' THEN 'Death Alleged'
			WHEN WC_DW_LON_STA = '17' THEN 'Death Verified'
			WHEN WC_DW_LON_STA = '18' THEN 'Disability Alleged'
			WHEN WC_DW_LON_STA = '19' THEN 'Disability Verified'
			WHEN WC_DW_LON_STA = '20' THEN 'Bankruptcy Alleged'
			WHEN WC_DW_LON_STA = '21' THEN 'Bankruptcy Verified' 
			WHEN WC_DW_LON_STA = '22' THEN 'Paid In Full'
			WHEN WC_DW_LON_STA = '23' THEN 'Not Fully Originated'
			WHEN WC_DW_LON_STA = '88' THEN 'Processing Error'
			WHEN WC_DW_LON_STA = '98' THEN 'Unknown'
		ELSE 'DW01 STATUS CODE ERROR'
		END AS Status
		,CASE
			WHEN LN83.LC_STA_LN83 = 'A'
			THEN PUT(LN83.LR_EFT_RDC,z5.3)
			ELSE 'N'
		END AS EFT
		,LN10.LD_LON_1_DSB AS FirstDisbDate
		,LN72.LR_ITR AS InterestRate
	FROM	
		SQL_CDW.LN10_LON LN10
		INNER JOIN SQL_CDW.PD10_PRS_NME PD10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		INNER JOIN SQL_CDW.LN72_INT_RTE_HST LN72
			ON LN10.BF_SSN = LN72.BF_SSN
			AND LN10.LN_SEQ = LN72.LN_SEQ
		INNER JOIN SQL_CDW.DW01_DW_CLC_CLU DW01
			ON LN10.BF_SSN = DW01.BF_SSN
			AND LN10.LN_SEQ = DW01.LN_SEQ
		LEFT JOIN SQL_CDW.LN83_EFT_TO_LON LN83
			ON LN10.BF_SSN = LN83.BF_SSN
			AND LN10.LN_SEQ = LN83.LN_SEQ
		LEFT JOIN
		(/*flag for exclusion: direct consol loans with fixed rate*/
			SELECT
				LN10.BF_SSN
				,LN10.LN_SEQ
			FROM
				SQL_CDW.LN10_LON LN10
				INNER JOIN SQL_CDW.LN72_INT_RTE_HST LN72
					ON LN10.BF_SSN = LN72.BF_SSN
					AND LN10.LN_SEQ = LN72.LN_SEQ
			WHERE 
				LN10.LC_STA_LON10 = 'R'
				AND LN10.LA_CUR_PRI > 0.00
				AND &TODAY BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
				AND LN72.LC_STA_LON72 = 'A'
				AND LN72.LC_ITR_TYP = 'F1'/*fixed rate*/
				AND LN10.IC_LON_PGM IN 
				(/*direct consolidation loans*/
					'DLCNSL', 
					'DLPCNS', 
					'DLSCCN',
					'DLSCNS', 
					'DLSCPG', 
					'DLSCPL', 
					'DLSCSC', 
					'DLSCSL', 
					'DLSCST', 
					'DLSCUC', 
					'DLSCUN', 
					'DLSPCN', 
					'DLSSPL', 
					'DLUCNS',
					'DLUSPL'
				)
		) EXCLUDE
			ON EXCLUDE.BF_SSN = LN10.BF_SSN
			AND EXCLUDE.LN_SEQ = LN10.LN_SEQ
	WHERE
		EXCLUDE.BF_SSN IS NULL
		AND LN10.LC_STA_LON10 = 'R'
		AND LN10.LA_CUR_PRI > 0.00
		AND &TODAY BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
		AND LN72.LC_STA_LON72 = 'A'
/*	ORDER BY*/
/*		PD10.DF_SPE_ACC_ID*/
/*		,LN10.LN_SEQ*/
	;
QUIT;

*format account id to preserve leading zeros when exporting;
DATA _SSAE18 (DROP=DF_SPE_ACC_ID);
	SET SSAE18_pulldown;
	AccountNumber = '="' || DF_SPE_ACC_ID || '"';
RUN;

PROC SQL NOPRINT;
CREATE TABLE SSAE18 AS
	SELECT
		AccountNumber
		,LN_SEQ AS Loan
		,InterestRate
		,EFT
		,WC_DW_LON_STA
		,Status		
		,FirstDisbDate
	FROM
		_SSAE18;
QUIT;

/*export spreadsheets*/

PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='In Grace'))
		OUTFILE="T:\SAS\In Grace.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='In School'))
		OUTFILE="T:\SAS\In School.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='In Repayment'))
		OUTFILE="T:\SAS\In Repayment.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='In Deferment'))
		OUTFILE="T:\SAS\In Deferment.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='In Forbearance'))
		OUTFILE="T:\SAS\In Forbearance.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='In Cure'))
		OUTFILE="T:\SAS\In Cure.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='Claim Pending'))
		OUTFILE="T:\SAS\Claim Pending.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='Claim Submitted'))
		OUTFILE="T:\SAS\Claim Submitted.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='Claim Cancelled'))
		OUTFILE="T:\SAS\Claim Cancelled.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='Claim Reject'))
		OUTFILE="T:\SAS\Claim Reject.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='Claim Returned'))
		OUTFILE="T:\SAS\Claim Returned.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='Claim Paid'))
		OUTFILE="T:\SAS\Claim Paid.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='Pre-claim Pending'))
		OUTFILE="T:\SAS\Pre-claim Pending.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='Preclaim submitted'))
		OUTFILE="T:\SAS\Preclaim submitted.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='Pre-claim Cancelled'))
		OUTFILE="T:\SAS\Pre-claim Cancelled.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='Death Alleged'))
		OUTFILE="T:\SAS\Death Alleged.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='Death Verified'))
		OUTFILE="T:\SAS\Death Verified.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='Disability Alleged'))
		OUTFILE="T:\SAS\Disability Alleged.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='Disability Verified'))
		OUTFILE="T:\SAS\Disability Verified.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='Bankruptcy Alleged'))
		OUTFILE="T:\SAS\Bankruptcy Alleged.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='Bankruptcy Verified'))
		OUTFILE="T:\SAS\Bankruptcy Verified.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='Paid In Full'))
		OUTFILE="T:\SAS\Paid In Full.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='Not Fully Originated'))
		OUTFILE="T:\SAS\Not Fully Originated.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='Processing Error'))
		OUTFILE="T:\SAS\Processing Error.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='Unknown'))
		OUTFILE="T:\SAS\Unknown.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAE18
		(WHERE=(STATUS='DW01 STATUS CODE ERROR'))
		OUTFILE="T:\SAS\DW01 STATUS CODE ERROR.csv"
		DBMS = CSV
		REPLACE;
RUN;
