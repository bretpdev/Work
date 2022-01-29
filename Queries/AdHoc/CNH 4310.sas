LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;

PROC SQL;
	CREATE TABLE BILL AS
		SELECT DISTINCT
			A.BF_SSN
		FROM
/*			borrower has a loan with a Xst pay due date prior to X/X/XX*/
			(
				SELECT
					LNXX.BF_SSN,
					LNXX.LN_SEQ
				FROM 
					PKUB.LNXX_LON LNXX
					JOIN PKUB.LNXX_LON_RPS LNXX
						ON LNXX.BF_SSN = LNXX.BF_SSN
						AND LNXX.LN_SEQ = LNXX.LN_SEQ
					JOIN PKUB.RSXX_BR_RPD RSXX
						ON LNXX.BF_SSN = RSXX.BF_SSN
						AND LNXX.LN_RPS_SEQ = RSXX.LN_RPS_SEQ
				WHERE
					LNXX.LA_CUR_PRI > X
/*					AND LNXX.LC_STA_LONXX = 'R'*/
					AND RSXX.LD_RPS_X_PAY_DU < 'XXJULXXXX'D /*Xst pay due date prior to X/X/XX*/
			) A
/*			AND borrower has a loan with a Xst pay due date on or after X/X/XX and is not enrolled in e-bill, but is enrolled in Autopay*/
			JOIN 
			(
				SELECT
					LNXX.BF_SSN,
					LNXX.LN_SEQ,
					LNXX.LC_STA_LNXX,
					BLXX.LC_STA_BILXX
				FROM 
					PKUB.LNXX_LON LNXX
					JOIN PKUB.LNXX_LON_RPS LNXX
						ON LNXX.BF_SSN = LNXX.BF_SSN
						AND LNXX.LN_SEQ = LNXX.LN_SEQ
					JOIN PKUB.RSXX_BR_RPD RSXX
						ON LNXX.BF_SSN = RSXX.BF_SSN
						AND LNXX.LN_RPS_SEQ = RSXX.LN_RPS_SEQ
						AND RSXX.LD_RPS_X_PAY_DU >= 'XXJULXXXX'D /*Xst pay due date on or after to X/X/XX*/
					/*e-bill*/
					LEFT JOIN PKUB.BLXX_BR_BIL BLXX
						ON LNXX.BF_SSN = BLXX.BF_SSN
						AND BLXX.LC_STA_BILXX = 'A'
						AND BLXX.LC_BIL_MTD = 'X'
					/*autopay*/
					JOIN PKUB.LNXX_EFT_TO_LON LNXX	
						ON LNXX.BF_SSN = LNXX.BF_SSN
						AND LNXX.LN_SEQ = LNXX.LN_SEQ
						AND LNXX.LC_STA_LNXX = 'A'
						AND COALESCE(LNXX.LD_EFT_EFF_END,TODAY()+X) > TODAY() /*if end date is null, set it to a future date so it is treated as future date and included*/
				WHERE
					LNXX.LA_CUR_PRI > X
/*					AND LNXX.LC_STA_LONXX = 'R'*/
					AND BLXX.BF_SSN IS NULL /*only include borrower if there are no e-bill bills*/
			) B
				ON A.BF_SSN = B.BF_SSN
				AND A.LN_SEQ <> B.LN_SEQ
	;
QUIT;

PROC EXPORT	
		DATA=LEGEND.BILL
		OUTFILE='T:\SAS\AES Billing Issue.xlsx'
		DBMS=EXCEL
		REPLACE;
RUN;
