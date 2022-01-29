/*delete the first row of the Excel file attached to the Need Help ticket so the second row will be used for column names*/

LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
LIBNAME XL 'T:\Trigger file for compare XXXXXX.XLSX';
LIBNAME INREPAY 'Q:\CS Systems Support\Data Mart Validation Reports\X-X-XX\XXXX-XX-XX_IN_REPAY not Defaulted.XLSX';
LIBNAME STATUS 'Q:\CS Systems Support\Data Mart Validation Reports\X-X-XX\XXXX-XX-XX_STATUS.XLSX';

DATA REPAY; 
	SET INREPAY.'BYSTATUS$'N; 
RUN;
DATA LOAN_DETAIL_FSA; 
	SET STATUS.'LOAN_DETAIL_FSA$'N; 
	FORMAT BORR_TYPE $X.;

	IF IC_LON_PGM IN ('DLUCNS','DLPCNS','DLSCNS','DLUSPL','DLSSPL','SUBCNS','UNCNS','CNSDLN','SUBSPC','UNSPC') 
		THEN BORR_TYPE = 'CON_BOR';
	ELSE IF IC_LON_PGM IN ('DLPLUS','PLUS') 
		THEN BORR_TYPE = 'PLUS_BOR';
	ELSE IF IC_LON_PGM IN ('DLSTFD','DLUNST','DLPLGB','STFFRD','UNSTFD','PLUSGB','SLS','FISL') 
		THEN BORR_TYPE = 'STU_BOR';
	ELSE BORR_TYPE = '';
RUN;

DATA CSPORTCSV;
	SET XL.'CSPortCSV SepX$'N;
	FORMAT SCHD_TYP $XX.;
	FORMAT SCHL_TYP $X.;

	IF PAY_PLAN IN ('JX','JX','JX','JX') THEN SCHD_TYP = 'Alternative';
	ELSE IF PAY_PLAN IN ('SF','CS','FF') THEN SCHD_TYP = 'Standard';
	ELSE IF PAY_PLAN IN ('CG','GR','SG') THEN SCHD_TYP = 'Graduated';
	ELSE IF PAY_PLAN IN ('FE','EF','EG') THEN SCHD_TYP = 'Extended';
	ELSE IF PAY_PLAN IN ('CX','CX','CX') THEN SCHD_TYP = 'ICR';
	ELSE IF PAY_PLAN IN ('IB','IL','IX') THEN SCHD_TYP = 'IBR';
	ELSE IF PAY_PLAN IN ('PA') THEN SCHD_TYP = 'Other';
	ELSE SCHD_TYP = '';

	IF SCH_TYPE = X THEN SCHL_TYP = 'C';
	ELSE SCHL_TYP = PUT(SCH_TYPE,X.);
RUN;

DATA LEGEND.CSPORTCSV; SET CSPORTCSV; RUN;

RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;

PROC SQL;
	CREATE TABLE NSLDS AS
		SELECT DISTINCT
			FSXX.BF_SSN,
			FSXX.LN_SEQ,
			CATX('',FSXX.BF_SSN,PUT(FSXX.LN_SEQ,ZX.)) AS LID,
			CS.*,
			SCXX.IF_DOE_SCL,
			SCXX.IC_SCL_OWN_CTL_TYP
		FROM
			CSPORTCSV CS
			INNER JOIN PKUB.FSXX_DL_LON FSXX
				ON CS.AWARD_ID = CATX('',FSXX.LF_FED_AWD,PUT(FSXX.LN_FED_AWD_SEQ,ZX.))
			INNER JOIN PKUB.LNXX_LON LNXX
				ON FSXX.BF_SSN = LNXX.BF_SSN
				AND FSXX.LN_SEQ = LNXX.LN_SEQ
			LEFT JOIN PKUB.SCXX_SCH_DMO SCXX
				ON LNXX.LF_DOE_SCL_ORG = SCXX.IF_DOE_SCL
	;
QUIT;
ENDRSUBMIT;

DATA NSLDS; 
	SET LEGEND.NSLDS; 
RUN;

PROC SQL;
	CREATE TABLE NSLDS_PLAN_SMRY AS
		SELECT
			SCHD_TYP,
			COUNT(BF_SSN) AS CNT
		FROM
			NSLDS
		GROUP BY
			SCHD_TYP
	;

	CREATE TABLE REPAY_PLAN_SMRY AS
		SELECT
			SCHD_TYP,
			COUNT(BF_SSN) AS CNT
		FROM
			REPAY
		GROUP BY
			SCHD_TYP
	; 

	CREATE TABLE PLAN_SMRY AS
		SELECT
			NSLDS.SCHD_TYP AS NSLDS_SCHD_TYP,
			REPAY.SCHD_TYP AS CS_SCHD_TYP,
			NSLDS.CNT AS NSLDS_COUNT,
			REPAY.CNT AS CS_COUNT
		FROM
			NSLDS_PLAN_SMRY NSLDS
			FULL OUTER JOIN REPAY_PLAN_SMRY REPAY
				ON NSLDS.SCHD_TYP = REPAY.SCHD_TYP
	;

	CREATE TABLE PLAN_MATCH AS
		SELECT
			NSLDS.BF_SSN,
			NSLDS.LN_SEQ,
			NSLDS.AWARD_ID
		FROM
			NSLDS
			INNER JOIN REPAY
				ON REPAY.LID = NSLDS.LID
				AND REPAY.SCHD_TYP = NSLDS.SCHD_TYP
	;

	CREATE TABLE PLAN_NO_MATCH AS
		SELECT
			NSLDS.BF_SSN,
			NSLDS.LN_SEQ,
			NSLDS.AWARD_ID,
			REPAY.SCHD_TYP AS CS_SCHD_TYP,
			NSLDS.SCHD_TYP AS NSLDS_SCHD_TYP,
			NSLDS.PAY_PLAN
		FROM
			NSLDS
			INNER JOIN REPAY
				ON REPAY.LID = NSLDS.LID
		WHERE
			NSLDS.PAY_PLAN NE '-'
			AND REPAY.SCHD_TYP NE NSLDS.SCHD_TYP
	;

	CREATE TABLE PLAN_NO_CS AS
		SELECT
			NSLDS.BF_SSN,
			NSLDS.LN_SEQ,
			NSLDS.AWARD_ID,
			REPAY.SCHD_TYP AS CS_SCHD_TYP,
			NSLDS.SCHD_TYP AS NSLDS_SCHD_TYP,
			NSLDS.PAY_PLAN
		FROM
			NSLDS
			LEFT JOIN REPAY
				ON REPAY.LID = NSLDS.LID
		WHERE
			NSLDS.PAY_PLAN NE '-'
			AND REPAY.BF_SSN IS NULL
	;

	CREATE TABLE NSLDS_SCHL_SMRY AS
		SELECT
			SCHL_TYP,
			COUNT(BF_SSN) AS CNT
		FROM
			NSLDS
		GROUP BY
			SCHL_TYP
	;

	CREATE TABLE CS_SCHL_SMRY AS
		SELECT
			SCH_TYP,
			COUNT(BF_SSN) AS CNT
		FROM
			LOAN_DETAIL_FSA
		GROUP BY
			SCH_TYP
	; 

	CREATE TABLE SCHL_SMRY AS
		SELECT
			NSLDS.SCHL_TYP AS NSLDS_SCHL_TYP,
			CS.SCH_TYP AS CS_SCHL_TYP,
			NSLDS.CNT AS NSLDS_COUNT,
			CS.CNT AS CS_COUNT
		FROM
			NSLDS_SCHL_SMRY NSLDS
			FULL OUTER JOIN CS_SCHL_SMRY CS
				ON NSLDS.SCHL_TYP = CS.SCH_TYP
	;

	CREATE TABLE SCHL_MATCH AS
		SELECT
			NSLDS.BF_SSN,
			NSLDS.LN_SEQ,
			NSLDS.AWARD_ID
		FROM
			NSLDS
			INNER JOIN LOAN_DETAIL_FSA DET
				ON NSLDS.LID = DET.LID
				AND NSLDS.SCHL_TYP = DET.SCH_TYP
	;

	CREATE TABLE SCHL_NO_MATCH AS
		SELECT
			NSLDS.BF_SSN,
			NSLDS.LN_SEQ,
			NSLDS.AWARD_ID,
			NSLDS.SCHL_TYP AS NSLDS_SCHL_TYP,
			DET.SCH_TYP AS CS_SCHL_TYP
		FROM
			NSLDS
			INNER JOIN LOAN_DETAIL_FSA DET
				ON NSLDS.LID = DET.LID
		WHERE
			NSLDS.SCHL_TYP NE DET.SCH_TYP
	;

	CREATE TABLE SCHL_NO_CS AS
		SELECT
			NSLDS.BF_SSN,
			NSLDS.LN_SEQ,
			NSLDS.AWARD_ID,
			NSLDS.SCHL_TYP AS NSLDS_SCHL_TYP,
			DET.SCH_TYP AS CS_SCHL_TYP
		FROM
			NSLDS
			LEFT JOIN LOAN_DETAIL_FSA DET
				ON NSLDS.LID = DET.LID
		WHERE
			DET.BF_SSN IS NULL
	;

	CREATE TABLE NSLDS_BOR_TYP_SMRY AS
		SELECT
			BORR_TYPE,
			COUNT(BF_SSN) AS CNT
		FROM
			NSLDS
		GROUP BY
			BORR_TYPE
	;

	CREATE TABLE CS_BOR_TYP_SMRY AS
		SELECT
			BORR_TYPE,
			COUNT(BF_SSN) AS CNT
		FROM
			LOAN_DETAIL_FSA
		GROUP BY
			BORR_TYPE
	; 

	CREATE TABLE BOR_TYP_SMRY AS
		SELECT
			NSLDS.BORR_TYPE AS NSLDS_BORR_TYPE,
			CS.BORR_TYPE AS CS_BORR_TYPE,
			NSLDS.CNT AS NSLDS_COUNT,
			CS.CNT AS CS_COUNT
		FROM
			NSLDS_BOR_TYP_SMRY NSLDS
			FULL OUTER JOIN CS_BOR_TYP_SMRY CS
				ON NSLDS.BORR_TYPE = CS.BORR_TYPE
	;

	CREATE TABLE BOR_TYP_MATCH AS
		SELECT
			NSLDS.BF_SSN,
			NSLDS.LN_SEQ,
			NSLDS.AWARD_ID
		FROM
			NSLDS
			INNER JOIN LOAN_DETAIL_FSA DET
				ON NSLDS.LID = DET.LID
				AND NSLDS.BORR_TYPE = DET.BORR_TYPE
	;

	CREATE TABLE BOR_TYP_NO_MATCH AS
		SELECT
			NSLDS.BF_SSN,
			NSLDS.LN_SEQ,
			NSLDS.AWARD_ID,
			NSLDS.BORR_TYPE AS NSLDS_BORR_TYPE,
			DET.BORR_TYPE AS CS_BORR_TYPE_TYPE
		FROM
			NSLDS
			INNER JOIN LOAN_DETAIL_FSA DET
				ON NSLDS.LID = DET.LID
		WHERE
			NSLDS.BORR_TYPE NE DET.BORR_TYPE
	;

	CREATE TABLE BOR_TYP_NO_CS AS
		SELECT
			NSLDS.BF_SSN,
			NSLDS.LN_SEQ,
			NSLDS.AWARD_ID,
			NSLDS.BORR_TYPE AS NSLDS_BORR_TYPE,
			DET.BORR_TYPE AS CS_BORR_TYPE_TYPE
		FROM
			NSLDS
			LEFT JOIN LOAN_DETAIL_FSA DET
				ON NSLDS.LID = DET.LID
		WHERE
			DET.BF_SSN IS NULL 
	;
QUIT;

PROC EXPORT
		DATA=PLAN_MATCH
		OUTFILE='T:\SAS\Trigger File Compare X-XX-XX.XLSX'
		REPLACE;
RUN;

PROC EXPORT
		DATA=PLAN_NO_MATCH
		OUTFILE='T:\SAS\Trigger File Compare X-XX-XX.XLSX'
		REPLACE;
RUN;

PROC EXPORT
		DATA=PLAN_NO_CS
		OUTFILE='T:\SAS\Trigger File Compare X-XX-XX.XLSX'
		REPLACE;
RUN;

PROC EXPORT
		DATA=SCHL_MATCH
		OUTFILE='T:\SAS\Trigger File Compare X-XX-XX.XLSX'
		REPLACE;
RUN;

PROC EXPORT
		DATA=SCHL_NO_MATCH
		OUTFILE='T:\SAS\Trigger File Compare X-XX-XX.XLSX'
		REPLACE;
RUN;

PROC EXPORT
		DATA=SCHL_NO_CS
		OUTFILE='T:\SAS\Trigger File Compare X-XX-XX.XLSX'
		REPLACE;
RUN;

PROC EXPORT
		DATA=BOR_TYP_MATCH
		OUTFILE='T:\SAS\Trigger File Compare X-XX-XX.XLSX'
		REPLACE;
RUN;

PROC EXPORT
		DATA=BOR_TYP_NO_MATCH
		OUTFILE='T:\SAS\Trigger File Compare X-XX-XX.XLSX'
		REPLACE;
RUN;

PROC EXPORT
		DATA=BOR_TYP_NO_CS
		OUTFILE='T:\SAS\Trigger File Compare X-XX-XX.XLSX'
		REPLACE;
RUN;

PROC EXPORT
		DATA=PLAN_SMRY
		OUTFILE='T:\SAS\Trigger File Compare X-XX-XX.XLSX'
		REPLACE;
RUN;

PROC EXPORT
		DATA=SCHL_SMRY
		OUTFILE='T:\SAS\Trigger File Compare X-XX-XX.XLSX'
		REPLACE;
RUN;

PROC EXPORT
		DATA=BOR_TYP_SMRY
		OUTFILE='T:\SAS\Trigger File Compare X-XX-XX.XLSX'
		REPLACE;
RUN;