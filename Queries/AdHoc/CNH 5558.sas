LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;

PROC SQL;
	CREATE TABLE RX_Paid_Ahead AS
		SELECT DISTINCT
			LNXX.BF_SSN,
			LNXX.LN_SEQ
		FROM
			PKUB.LNXX_LON LNXX
			JOIN PKUB.LNXX_LON_BIL_CRF LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
			JOIN PKUB.BLXX_BR_BIL BLXX
				ON LNXX.BF_SSN = BLXX.BF_SSN
		WHERE
			LNXX.LC_STA_LONXX = 'R'
			AND LNXX.LA_CUR_PRI > X
			AND BLXX.LC_IND_BIL_SNT IN ('N', 'X')
			AND BLXX.LC_STA_BILXX = 'A'
			AND LNXX.LC_STA_LONXX = 'A'
			AND LNXX.LD_BIL_DU_LON > TODAY() + XX
		ORDER BY
			LNXX.BF_SSN,
			LNXX.LN_SEQ
	;

	CREATE TABLE RX_Split_Delinquencies AS
		SELECT
			BF_SSN
		FROM
			(
				SELECT
					BF_SSN,
					COUNT(DISTINCT LN_DLQ_MAX) AS CNT
				FROM
					(
						SELECT DISTINCT
							LNXX.BF_SSN,
							LNXX.LN_SEQ,
							COALESCE(LNXX.LN_DLQ_MAX,X) AS LN_DLQ_MAX
						FROM
							PKUB.LNXX_LON LNXX
							LEFT JOIN PKUB.LNXX_LON_DLQ_HST LNXX
								ON LNXX.BF_SSN = LNXX.BF_SSN
								AND LNXX.LN_SEQ = LNXX.LN_SEQ
								AND LNXX.LC_STA_LONXX = 'X'
								AND LD_DLQ_MAX = TODAY() - X
						WHERE
							LNXX.LC_STA_LONXX = 'R'
							AND LNXX.LA_CUR_PRI > X
					)
				GROUP BY
					BF_SSN
			)
		WHERE
			CNT > X
	;	

	CREATE TABLE RX_Multiple_Status AS
		SELECT
			BF_SSN
		FROM
			(
				SELECT
					BF_SSN,
					COUNT(DISTINCT WC_DW_LON_STA) AS CNT
				FROM
					(
						SELECT DISTINCT
							LNXX.BF_SSN,
							LNXX.LN_SEQ,
							DWXX.WC_DW_LON_STA
						FROM
							PKUB.LNXX_LON LNXX
							JOIN PKUB.DWXX_DW_CLC_CLU DWXX
								ON LNXX.BF_SSN = DWXX.BF_SSN
								AND LNXX.LN_SEQ = DWXX.LN_SEQ
						WHERE
							LNXX.LC_STA_LONXX = 'R'
							AND LNXX.LA_CUR_PRI > X
					)
				GROUP BY
					BF_SSN
			)
		WHERE
			CNT > X
	;	

	CREATE TABLE RX_GT_XX_Months_For_Used AS
		SELECT
			BF_SSN,
			LN_SEQ,
			MO_USED
		FROM
			(
				SELECT
					LNXX.BF_SSN,
					LNXX.LN_SEQ,
					SUM((LNXX.LD_FOR_END - LNXX.LD_FOR_BEG + X) / XXX * XX) AS MO_USED
				FROM
					PKUB.LNXX_LON LNXX
					JOIN PKUB.LNXX_BR_FOR_APV LNXX
						ON LNXX.BF_SSN = LNXX.BF_SSN
						AND LNXX.LN_SEQ = LNXX.LN_SEQ
					JOIN PKUB.FBXX_BR_FOR_REQ FBXX
						ON LNXX.BF_SSN = FBXX.BF_SSN
						AND LNXX.LF_FOR_CTL_NUM = FBXX.LF_FOR_CTL_NUM
				WHERE
					LNXX.LC_STA_LONXX = 'R'
					AND LNXX.LA_CUR_PRI > X
					AND FBXX.LC_FOR_TYP = 'XX'
					AND FBXX.LC_FOR_STA = 'A'
					AND FBXX.LC_STA_FORXX = 'A'
					AND LNXX.LC_STA_LONXX = 'A'
				GROUP BY
					LNXX.BF_SSN,
					LNXX.LN_SEQ
			)
		WHERE
			MO_USED > XX
	;

	CREATE TABLE LOANS AS
		SELECT DISTINCT
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			DWXX.WC_DW_LON_STA
		FROM
			PKUB.LNXX_LON LNXX
			JOIN PKUB.DWXX_DW_CLC_CLU DWXX
				ON LNXX.BF_SSN = DWXX.BF_SSN
				AND LNXX.LN_SEQ = DWXX.LN_SEQ
		WHERE
			LNXX.LC_STA_LONXX = 'R'
			AND LNXX.LA_CUR_PRI > X
	;

	CREATE TABLE RX_In_a_Forbearance AS
		SELECT 
			BF_SSN,
			LN_SEQ
		FROM
			LOANS
		WHERE
			WC_DW_LON_STA = 'XX'
	;

	CREATE TABLE RX_In_a_Deferment AS
		SELECT 
			BF_SSN,
			LN_SEQ
		FROM
			LOANS
		WHERE
			WC_DW_LON_STA = 'XX'
	;

	CREATE TABLE RX_In_School_Status AS
		SELECT 
			BF_SSN,
			LN_SEQ
		FROM
			LOANS
		WHERE
			WC_DW_LON_STA = 'XX'
	;

	CREATE TABLE SCHDS AS
		SELECT DISTINCT
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LC_TYP_SCH_DIS
		FROM
			PKUB.LNXX_LON LNXX
			JOIN PKUB.LNXX_LON_RPS LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
		WHERE
			LNXX.LC_STA_LONXX = 'R'
			AND LNXX.LA_CUR_PRI > X
			AND LNXX.LC_STA_LONXX = 'A'
			AND LNXX.LC_TYP_SCH_DIS IN ('IB', 'IL', 'CQ', 'CX', 'CX', 'CX')
	;

	CREATE TABLE RX_IBR_PFH_Perm_Standard AS
		SELECT
			*
		FROM
			SCHDS
		WHERE
			LC_TYP_SCH_DIS IN ('IB', 'IL')
	;

	CREATE TABLE RXX_ICR_PFH_Perm_Standard AS
		SELECT
			*
		FROM
			SCHDS
		WHERE
			LC_TYP_SCH_DIS IN ('CQ', 'CX', 'CX', 'CX')
	;

	CREATE TABLE RXX_RPF_Status AS
		SELECT
			LNXX.BF_SSN,
			LNXX.LN_SEQ
		FROM
			PKUB.LNXX_LON LNXX
			JOIN PKUB.LNXX_BR_FOR_APV LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
			JOIN PKUB.FBXX_BR_FOR_REQ FBXX
				ON LNXX.BF_SSN = FBXX.BF_SSN
				AND LNXX.LF_FOR_CTL_NUM = FBXX.LF_FOR_CTL_NUM
		WHERE
			LNXX.LC_STA_LONXX = 'R'
			AND LNXX.LA_CUR_PRI > X
			AND FBXX.LC_FOR_TYP = 'XX'
			AND FBXX.LC_FOR_STA = 'A'
			AND FBXX.LC_STA_FORXX = 'A'
			AND LNXX.LC_STA_LONXX = 'A'
			AND LNXX.LD_FOR_BEG < TODAY()
			AND LNXX.LD_FOR_END > TODAY()
	;

QUIT;
ENDRSUBMIT;

DATA RX_Paid_Ahead; SET LEGEND.RX_Paid_Ahead; RUN;
DATA RX_Split_Delinquencies; SET LEGEND.RX_Split_Delinquencies; RUN;
DATA RX_Multiple_Status; SET LEGEND.RX_Multiple_Status; RUN;
DATA RX_GT_XX_Months_For_Used; SET LEGEND.RX_GT_XX_Months_For_Used; RUN;
DATA RX_In_a_Forbearance; SET LEGEND.RX_In_a_Forbearance; RUN;
DATA RX_In_a_Deferment; SET LEGEND.RX_In_a_Deferment; RUN;
DATA RX_In_School_Status; SET LEGEND.RX_In_School_Status; RUN;
DATA RX_IBR_PFH_Perm_Standard; SET LEGEND.RX_IBR_PFH_Perm_Standard; RUN;
DATA RXX_ICR_PFH_Perm_Standard; SET LEGEND.RXX_ICR_PFH_Perm_Standard; RUN;
DATA RXX_RPF_Status; SET LEGEND.RXX_RPF_Status; RUN;

PROC EXPORT
		DATA=RX_Paid_Ahead
		OUTFILE='T:\SAS\SSN Query for Test Migration.xlsx'
		REPLACE;
RUN;

PROC EXPORT
		DATA=RX_Split_Delinquencies
		OUTFILE='T:\SAS\SSN Query for Test Migration.xlsx'
		REPLACE;
RUN;

PROC EXPORT
		DATA=RX_Multiple_Status
		OUTFILE='T:\SAS\SSN Query for Test Migration.xlsx'
		REPLACE;
RUN;

PROC EXPORT
		DATA=RX_GT_XX_Months_For_Used
		OUTFILE='T:\SAS\SSN Query for Test Migration.xlsx'
		REPLACE;
RUN;

PROC EXPORT
		DATA=RX_In_a_Forbearance
		OUTFILE='T:\SAS\SSN Query for Test Migration.xlsx'
		REPLACE;
RUN;

PROC EXPORT
		DATA=RX_In_a_Deferment
		OUTFILE='T:\SAS\SSN Query for Test Migration.xlsx'
		REPLACE;
RUN;

PROC EXPORT
		DATA=RX_In_School_Status
		OUTFILE='T:\SAS\SSN Query for Test Migration.xlsx'
		REPLACE;
RUN;

PROC EXPORT
		DATA=RX_IBR_PFH_Perm_Standard
		OUTFILE='T:\SAS\SSN Query for Test Migration.xlsx'
		REPLACE;
RUN;

PROC EXPORT
		DATA=RXX_ICR_PFH_Perm_Standard
		OUTFILE='T:\SAS\SSN Query for Test Migration.xlsx'
		REPLACE;
RUN;

PROC EXPORT
		DATA=RXX_RPF_Status
		OUTFILE='T:\SAS\SSN Query for Test Migration.xlsx'
		REPLACE;
RUN;