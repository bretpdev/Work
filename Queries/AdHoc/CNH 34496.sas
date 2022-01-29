/*query based on CNH XXXXX*/
LIBNAME SQL_CDW ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CDW.dsn; bl_keepnulls=no; READ_ISOLATION_LEVEL=RU" SCHEMA= DBO;

%LET TODAY = 'XX/XX/XXXX';

PROC SQL NOPRINT;
CREATE TABLE SSAEXX_pulldown AS
	SELECT DISTINCT
		PDXX.DF_SPE_ACC_ID
		,LNXX.LN_SEQ
		,DWXX.WC_DW_LON_STA
		,CASE
			WHEN WC_DW_LON_STA = 'XX' THEN 'In Grace'
			WHEN WC_DW_LON_STA = 'XX' THEN 'In School'
			WHEN WC_DW_LON_STA = 'XX' THEN 'In Repayment'
			WHEN WC_DW_LON_STA = 'XX' THEN 'In Deferment'
			WHEN WC_DW_LON_STA = 'XX' THEN 'In Forbearance'
			WHEN WC_DW_LON_STA = 'XX' THEN 'In Cure'
			WHEN WC_DW_LON_STA = 'XX' THEN 'Claim Pending'
			WHEN WC_DW_LON_STA = 'XX' THEN 'Claim Submitted'
			WHEN WC_DW_LON_STA = 'XX' THEN 'Claim Cancelled'
			WHEN WC_DW_LON_STA = 'XX' THEN 'Claim Reject'
			WHEN WC_DW_LON_STA = 'XX' THEN 'Claim Returned'
			WHEN WC_DW_LON_STA = 'XX' THEN 'Claim Paid'
			WHEN WC_DW_LON_STA = 'XX' THEN 'Pre-claim Pending'
			WHEN WC_DW_LON_STA = 'XX' THEN 'Preclaim submitted'
			WHEN WC_DW_LON_STA = 'XX' THEN 'Pre-claim Cancelled'
			WHEN WC_DW_LON_STA = 'XX' THEN 'Death Alleged'
			WHEN WC_DW_LON_STA = 'XX' THEN 'Death Verified'
			WHEN WC_DW_LON_STA = 'XX' THEN 'Disability Alleged'
			WHEN WC_DW_LON_STA = 'XX' THEN 'Disability Verified'
			WHEN WC_DW_LON_STA = 'XX' THEN 'Bankruptcy Alleged'
			WHEN WC_DW_LON_STA = 'XX' THEN 'Bankruptcy Verified' 
			WHEN WC_DW_LON_STA = 'XX' THEN 'Paid In Full'
			WHEN WC_DW_LON_STA = 'XX' THEN 'Not Fully Originated'
			WHEN WC_DW_LON_STA = 'XX' THEN 'Processing Error'
			WHEN WC_DW_LON_STA = 'XX' THEN 'Unknown'
		ELSE 'DWXX STATUS CODE ERROR'
		END AS Status
		,CASE
			WHEN LNXX.LC_STA_LNXX = 'A'
			THEN PUT(LNXX.LR_EFT_RDC,zX.X)
			ELSE 'N'
		END AS EFT
		,LNXX.LD_LON_X_DSB AS FirstDisbDate
		,LNXX.LR_ITR AS InterestRate
	FROM	
		SQL_CDW.LNXX_LON LNXX
		INNER JOIN SQL_CDW.PDXX_PRS_NME PDXX
			ON LNXX.BF_SSN = PDXX.DF_PRS_ID
		INNER JOIN SQL_CDW.LNXX_INT_RTE_HST LNXX
			ON LNXX.BF_SSN = LNXX.BF_SSN
			AND LNXX.LN_SEQ = LNXX.LN_SEQ
		INNER JOIN SQL_CDW.DWXX_DW_CLC_CLU DWXX
			ON LNXX.BF_SSN = DWXX.BF_SSN
			AND LNXX.LN_SEQ = DWXX.LN_SEQ
		LEFT JOIN SQL_CDW.LNXX_EFT_TO_LON LNXX
			ON LNXX.BF_SSN = LNXX.BF_SSN
			AND LNXX.LN_SEQ = LNXX.LN_SEQ
		LEFT JOIN
		(/*flag for exclusion: direct consol loans with fixed rate*/
			SELECT
				LNXX.BF_SSN
				,LNXX.LN_SEQ
			FROM
				SQL_CDW.LNXX_LON LNXX
				INNER JOIN SQL_CDW.LNXX_INT_RTE_HST LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
			WHERE 
				LNXX.LC_STA_LONXX = 'R'
				AND LNXX.LA_CUR_PRI > X.XX
				AND &TODAY BETWEEN LNXX.LD_ITR_EFF_BEG AND LNXX.LD_ITR_EFF_END
				AND LNXX.LC_STA_LONXX = 'A'
				AND LNXX.LC_ITR_TYP = 'FX'/*fixed rate*/
				AND LNXX.IC_LON_PGM IN 
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
			ON EXCLUDE.BF_SSN = LNXX.BF_SSN
			AND EXCLUDE.LN_SEQ = LNXX.LN_SEQ
	WHERE
		EXCLUDE.BF_SSN IS NULL
		AND LNXX.LC_STA_LONXX = 'R'
		AND LNXX.LA_CUR_PRI > X.XX
		AND &TODAY BETWEEN LNXX.LD_ITR_EFF_BEG AND LNXX.LD_ITR_EFF_END
		AND LNXX.LC_STA_LONXX = 'A'
/*	ORDER BY*/
/*		PDXX.DF_SPE_ACC_ID*/
/*		,LNXX.LN_SEQ*/
	;
QUIT;

*format account id to preserve leading zeros when exporting;
DATA _SSAEXX (DROP=DF_SPE_ACC_ID);
	SET SSAEXX_pulldown;
	AccountNumber = '="' || DF_SPE_ACC_ID || '"';
RUN;

PROC SQL NOPRINT;
CREATE TABLE SSAEXX AS
	SELECT
		AccountNumber
		,LN_SEQ AS Loan
		,InterestRate
		,EFT
		,WC_DW_LON_STA
		,Status		
		,FirstDisbDate
	FROM
		_SSAEXX;
QUIT;

/*export spreadsheets*/

PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='In Grace'))
		OUTFILE="T:\SAS\In Grace.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='In School'))
		OUTFILE="T:\SAS\In School.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='In Repayment'))
		OUTFILE="T:\SAS\In Repayment.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='In Deferment'))
		OUTFILE="T:\SAS\In Deferment.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='In Forbearance'))
		OUTFILE="T:\SAS\In Forbearance.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='In Cure'))
		OUTFILE="T:\SAS\In Cure.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='Claim Pending'))
		OUTFILE="T:\SAS\Claim Pending.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='Claim Submitted'))
		OUTFILE="T:\SAS\Claim Submitted.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='Claim Cancelled'))
		OUTFILE="T:\SAS\Claim Cancelled.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='Claim Reject'))
		OUTFILE="T:\SAS\Claim Reject.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='Claim Returned'))
		OUTFILE="T:\SAS\Claim Returned.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='Claim Paid'))
		OUTFILE="T:\SAS\Claim Paid.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='Pre-claim Pending'))
		OUTFILE="T:\SAS\Pre-claim Pending.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='Preclaim submitted'))
		OUTFILE="T:\SAS\Preclaim submitted.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='Pre-claim Cancelled'))
		OUTFILE="T:\SAS\Pre-claim Cancelled.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='Death Alleged'))
		OUTFILE="T:\SAS\Death Alleged.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='Death Verified'))
		OUTFILE="T:\SAS\Death Verified.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='Disability Alleged'))
		OUTFILE="T:\SAS\Disability Alleged.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='Disability Verified'))
		OUTFILE="T:\SAS\Disability Verified.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='Bankruptcy Alleged'))
		OUTFILE="T:\SAS\Bankruptcy Alleged.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='Bankruptcy Verified'))
		OUTFILE="T:\SAS\Bankruptcy Verified.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='Paid In Full'))
		OUTFILE="T:\SAS\Paid In Full.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='Not Fully Originated'))
		OUTFILE="T:\SAS\Not Fully Originated.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='Processing Error'))
		OUTFILE="T:\SAS\Processing Error.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='Unknown'))
		OUTFILE="T:\SAS\Unknown.csv"
		DBMS = CSV
		REPLACE;
RUN;
PROC EXPORT
		DATA=SSAEXX
		(WHERE=(STATUS='DWXX STATUS CODE ERROR'))
		OUTFILE="T:\SAS\DWXX STATUS CODE ERROR.csv"
		DBMS = CSV
		REPLACE;
RUN;
