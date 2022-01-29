/*NOTE: 	open Q:\CS Systems Support\Data Mart Validation Reports\NSLDS file compare XXXXXX\Copy of Cornerstone Details April X.xlsx using password R$XpQyiZ%X#m*/
/*			before running this job*/

LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
LIBNAME CS_DISB 'Q:\CS Systems Support\Data Mart Validation Reports\X-XX-XX\XXXX-XX-XX_TWICE_DISB.xlsx';
LIBNAME CS_REPAY 'Q:\CS Systems Support\Data Mart Validation Reports\X-XX-XX\XXXX-XX-XX_In_Repay_XXX.xlsx';
LIBNAME FSA 'Q:\CS Systems Support\Data Mart Validation Reports\NSLDS file compare XXXXXX\Copy of Cornerstone Details April X.xlsx';


/***********************************************************************************/
/*                        Twice_Disb VS Loans Doubled TAB                          */
/***********************************************************************************/

DATA LEGEND.FSA_DOUBLED; SET FSA.'LOANS DOUBLED$'N(FIRSTOBS=X); RUN;

RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE FSA AS
		SELECT DISTINCT
			FSXX.BF_SSN,
			FSXX.LN_SEQ,
			FSA.FX AS AWARD_ID,
			FSA.OUT AS PRIN_BAL,
			FSA.OUTX AS INT_BAL,
			FSA.CURR AS GUAR_CD,
			FSA.FX AS LOAN_TYP,
			FSA.CURRX AS LOAN_STA,
			FSA.FX AS TOT_DIS
		FROM
			FSA_DOUBLED FSA
			JOIN PKUB.FSXX_DL_LON FSXX
				ON SUBSTR(FSA.FX,X,XX) = FSXX.LF_FED_AWD
				AND INPUT(SUBSTR(FSA.FX,XX,X),X.) = FSXX.LN_FED_AWD_SEQ
	;
QUIT;
ENDRSUBMIT;

DATA FSA; SET LEGEND.FSA; RUN;

PROC SQL;
	CREATE TABLE IN_FSA_NOT_CS AS
		SELECT
			FSA.*
		FROM
			FSA
			LEFT JOIN CS_DISB.'WITH_STATUS$'N CS
				ON FSA.BF_SSN = CS.BF_SSN
				AND FSA.LN_SEQ = CS.LN_SEQ
		WHERE
			CS.BF_SSN IS NULL
	;

	CREATE TABLE IN_CS_NOT_FSA AS
		SELECT
			CS.*
		FROM
			CS_DISB.'WITH_STATUS$'N CS
			LEFT JOIN FSA
				ON FSA.BF_SSN = CS.BF_SSN
				AND FSA.LN_SEQ = CS.LN_SEQ
		WHERE
			FSA.BF_SSN IS NULL
	;
QUIT;

DATA LEGEND.CS; SET IN_CS_NOT_FSA; RUN;

RSUBMIT;
PROC SQL;
	CREATE TABLE IN_CS_NOT_FSA AS
		SELECT
			CS.BF_SSN,
			FSXX.LF_FED_AWD||PUT(FSXX.LN_FED_AWD_SEQ,ZX.) AS AWARD_ID,
			CS.LD_LON_X_DSB,
			CS.STA,
			CS.LN_BAL,
			CS.DISB_AMT
		FROM
			CS
			JOIN PKUB.FSXX_DL_LON FSXX
				ON CS.BF_SSN = FSXX.BF_SSN
				AND CS.LN_SEQ = FSXX.LN_SEQ
	;
QUIT;
ENDRSUBMIT;

DATA IN_CS_NOT_FSA; SET LEGEND.IN_CS_NOT_FSA; RUN;

PROC EXPORT
		DATA=IN_FSA_NOT_CS
		OUTFILE='T:\SAS\TWICE_DISB Compare.xlsx'
		REPLACE;
RUN;
	
PROC EXPORT
		DATA=IN_CS_NOT_FSA
		OUTFILE='T:\SAS\TWICE_DISB Compare.xlsx'
		REPLACE;
RUN;

/***********************************************************************************/
/*                        In_Repay_XXX VS Repay Plans TAB                          */
/***********************************************************************************/

DATA LEGEND.FSA_REPAY; SET FSA.'REPAY PLANS$'N(FIRSTOBS=X); RUN;

RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE FSA AS
		SELECT DISTINCT
			FSXX.BF_SSN,
			FSXX.LN_SEQ,
			FSA.AWARD AS AWARD_ID,
			FSA.FX AS TYPE
		FROM
			FSA_REPAY FSA
			JOIN PKUB.FSXX_DL_LON FSXX
				ON SUBSTR(FSA.AWARD,X,XX) = FSXX.LF_FED_AWD
				AND INPUT(SUBSTR(FSA.AWARD,XX,X),X.) = FSXX.LN_FED_AWD_SEQ
	;
QUIT;
ENDRSUBMIT;

DATA FSA; SET LEGEND.FSA; RUN;

PROC SQL;
	CREATE TABLE IN_FSA_NOT_CS AS
		SELECT
			FSA.*
		FROM
			FSA
			LEFT JOIN CS_REPAY.'BYSTATUS$'N CS
				ON FSA.BF_SSN = CS.BF_SSN
				AND FSA.LN_SEQ = CS.LN_SEQ
		WHERE
			CS.BF_SSN IS NULL
	;

	CREATE TABLE IN_CS_NOT_FSA AS
		SELECT
			CS.*
		FROM
			CS_REPAY.'BYSTATUS$'N CS
			LEFT JOIN FSA
				ON FSA.BF_SSN = CS.BF_SSN
				AND FSA.LN_SEQ = CS.LN_SEQ
		WHERE
			FSA.BF_SSN IS NULL
	;
QUIT;

/*for the loans in the FSA file not in the CS file, please look at Compass and provide the repayment plan type, and LNXX loan status adn whether the loan is XXX+. */
DATA LEGEND.IN_FSA_NOT_CS; SET IN_FSA_NOT_CS; RUN;
RSUBMIT;
PROC SQL;
	CREATE TABLE IN_FSA_NOT_CS_X AS
		SELECT
			CS.*,
			LNXX.LC_TYP_SCH_DIS,
			LNXX.LC_STA_LONXX,
			COALESCE(LNXX.LN_DLQ_MAX,X) AS LN_DLQ_MAX,
			CASE
				WHEN COALESCE(LNXX.LN_DLQ_MAX,X) > XXX THEN 'YES'
				ELSE 'NO'
			END AS IS_XXX_PLUS
		FROM
			IN_FSA_NOT_CS CS
			LEFT JOIN PKUB.LNXX_LON LNXX
				ON CS.BF_SSN = LNXX.BF_SSN
				AND CS.LN_SEQ = LNXX.LN_SEQ
			LEFT JOIN PKUB.LNXX_LON_RPS LNXX
				ON CS.BF_SSN = LNXX.BF_SSN
				AND CS.LN_SEQ = LNXX.LN_SEQ
				AND LNXX.LC_STA_LONXX = 'A'
			LEFT JOIN PKUB.LNXX_LON_DLQ_HST LNXX
				ON CS.BF_SSN = LNXX.BF_SSN
				AND CS.LN_SEQ = LNXX.LN_SEQ
				AND LNXX.LC_STA_LONXX = 'X'
	;
QUIT;
ENDRSUBMIT;

DATA IN_FSA_NOT_CS; SET LEGEND.IN_FSA_NOT_CS_X; RUN;

PROC EXPORT
		DATA=IN_FSA_NOT_CS
		OUTFILE='T:\SAS\IN_REPAY Compare.xlsx'
		REPLACE;
RUN;
	
PROC EXPORT
		DATA=IN_CS_NOT_FSA
		OUTFILE='T:\SAS\IN_REPAY Compare.xlsx'
		REPLACE;
RUN;

/*libname _all_ clear;*/
