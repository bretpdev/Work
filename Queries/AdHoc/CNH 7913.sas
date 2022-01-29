

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;


PROC SQL;
	CREATE TABLE RECRT_DUE AS
		SELECT DISTINCT
			BF_SSN,
			MAX(BD_ANV_QLF_IBR) AS BD_ANV_QLF_IBR,
			'X' AS CAT	
		FROM
			PKUB.RSXX_IBR_RPS
		WHERE
			BD_ANV_QLF_IBR BETWEEN INPUT('XX/XX/XXXX', MMDDYYXX.) AND INPUT('XX/XX/XXXX', MMDDYYXX.)
		GROUP BY
			BF_SSN,
			'X'
;
	CREATE TABLE RECRT_FAILED AS
		SELECT DISTINCT
			DUE.BF_SSN,
			DUE.BD_ANV_QLF_IBR,
			'X' AS CAT
			
		FROM
			RECRT_DUE DUE
			LEFT JOIN PKUB.RSXX_IBR_RPS RSXX
				ON RSXX.BF_SSN = DUE.BF_SSN
				AND RSXX.BC_STA_RSXX = 'A'
		WHERE
			COALESCE(RSXX.BD_CRT_RSXX, INPUT('XX/XX/XXXX', MMDDYYXX.)) > DUE.BD_ANV_QLF_IBR 
;
	CREATE TABLE FAIL_DEF_FORB AS
		SELECT DISTINCT
			FAILED.BF_SSN,
			FAILED.BD_ANV_QLF_IBR,
			'X' AS CAT
		FROM
			RECRT_FAILED FAILED
		LEFT JOIN PKUB.FBXX_BR_FOR_REQ FBXX
			ON FBXX.BF_SSN = FAILED.BF_SSN 
		LEFT JOIN PKUB.LNXX_BR_FOR_APV LNXX
			ON LNXX.BF_SSN = FBXX.BF_SSN
			AND LNXX.LF_FOR_CTL_NUM = FBXX.LF_FOR_CTL_NUM
		LEFT JOIN PKUB.DFXX_BR_DFR_REQ DFXX
				ON DFXX.BF_SSN = FAILED.BF_SSN
		LEFT JOIN PKUB.LNXX_BR_DFR_APV LNXX
			ON LNXX.BF_SSN = DFXX.BF_SSN
			AND LNXX.LF_DFR_CTL_NUM = DFXX.LF_DFR_CTL_NUM
		WHERE
			(FBXX.LC_FOR_TYP IN ('XX', 'XX', 'XX', 'XX', 'XX', 'XX', 'XX', 'XX', 'XX') AND LNXX.LD_FOR_APL > FAILED.BD_ANV_QLF_IBR)
			OR
			(DFXX.LC_DFR_TYP IN ('XX','XX') AND LNXX.LD_DFR_APL > FAILED.BD_ANV_QLF_IBR)
;
	CREATE TABLE RECRT_LATE_X AS
		SELECT DISTINCT
			DUE.BF_SSN,
			DUE.BD_ANV_QLF_IBR,
			'X' AS CAT
			
		FROM
			RECRT_FAILED DUE
			LEFT JOIN PKUB.RSXX_IBR_RPS RSXX
				ON RSXX.BF_SSN = DUE.BF_SSN
				AND RSXX.BC_STA_RSXX = 'A'
		WHERE
			COALESCE(RSXX.BD_CRT_RSXX, INPUT('XX/XX/XXXX', MMDDYYXX.)) > DUE.BD_ANV_QLF_IBR 
			AND COALESCE(RSXX.BD_CRT_RSXX, INPUT('XX/XX/XXXX', MMDDYYXX.)) < INTNX('month', DUE.BD_ANV_QLF_IBR, X, 'same')
;	
	CREATE TABLE RECRT_LATE_X AS
		SELECT DISTINCT
			DUE.BF_SSN,
			DUE.BD_ANV_QLF_IBR,
			'X' AS CAT
			
		FROM
			RECRT_FAILED DUE
			LEFT JOIN PKUB.RSXX_IBR_RPS RSXX
				ON RSXX.BF_SSN = DUE.BF_SSN
				AND RSXX.BC_STA_RSXX = 'A'
		WHERE
			COALESCE(RSXX.BD_CRT_RSXX, INPUT('XX/XX/XXXX', MMDDYYXX.)) > DUE.BD_ANV_QLF_IBR 
			AND COALESCE(RSXX.BD_CRT_RSXX, INPUT('XX/XX/XXXX', MMDDYYXX.))BETWEEN (INTNX('month', DUE.BD_ANV_QLF_IBR, X, 'same')+X) AND INTNX('month', DUE.BD_ANV_QLF_IBR, X, 'same')
;	
	CREATE TABLE RECRT_LATE_X AS
		SELECT DISTINCT
			DUE.BF_SSN,
			DUE.BD_ANV_QLF_IBR,
			'X' AS CAT
			
		FROM
			RECRT_FAILED DUE
			LEFT JOIN PKUB.RSXX_IBR_RPS RSXX
				ON RSXX.BF_SSN = DUE.BF_SSN
				AND RSXX.BC_STA_RSXX = 'A'
		WHERE
			COALESCE(RSXX.BD_CRT_RSXX, INPUT('XX/XX/XXXX', MMDDYYXX.)) > DUE.BD_ANV_QLF_IBR 
			AND COALESCE(RSXX.BD_CRT_RSXX, INPUT('XX/XX/XXXX', MMDDYYXX.)) BETWEEN (INTNX('month', DUE.BD_ANV_QLF_IBR, X, 'same')+X) AND INTNX('month', DUE.BD_ANV_QLF_IBR, X, 'same')
;	
	CREATE TABLE RECRT_LATE_X AS
		SELECT DISTINCT
			DUE.BF_SSN,
			DUE.BD_ANV_QLF_IBR,
			'X' AS CAT
			
		FROM
			RECRT_FAILED DUE
			LEFT JOIN PKUB.RSXX_IBR_RPS RSXX
				ON RSXX.BF_SSN = DUE.BF_SSN
				AND RSXX.BC_STA_RSXX = 'A'
		WHERE
			COALESCE(RSXX.BD_CRT_RSXX, INPUT('XX/XX/XXXX', MMDDYYXX.)) > DUE.BD_ANV_QLF_IBR 
			AND COALESCE(RSXX.BD_CRT_RSXX, INPUT('XX/XX/XXXX', MMDDYYXX.)) BETWEEN (INTNX('month', DUE.BD_ANV_QLF_IBR, X, 'same')+X) AND INTNX('month', DUE.BD_ANV_QLF_IBR, X, 'same')
;
	CREATE TABLE RECRT_LATE_X AS
		SELECT DISTINCT
			DUE.BF_SSN,
			DUE.BD_ANV_QLF_IBR,
			'X' AS CAT
			
		FROM
			RECRT_FAILED DUE
			LEFT JOIN PKUB.RSXX_IBR_RPS RSXX
				ON RSXX.BF_SSN = DUE.BF_SSN
				AND RSXX.BC_STA_RSXX = 'A'
		WHERE
			COALESCE(RSXX.BD_CRT_RSXX, INPUT('XX/XX/XXXX', MMDDYYXX.)) > DUE.BD_ANV_QLF_IBR 
			AND COALESCE(RSXX.BD_CRT_RSXX, INPUT('XX/XX/XXXX', MMDDYYXX.)) BETWEEN (INTNX('month', DUE.BD_ANV_QLF_IBR, X, 'same')+X) AND INTNX('month', DUE.BD_ANV_QLF_IBR, X, 'same')
;	
	CREATE TABLE RECRT_LATE_X AS
		SELECT DISTINCT
			DUE.BF_SSN,
			DUE.BD_ANV_QLF_IBR,
			'X' AS CAT
		FROM
			RECRT_FAILED DUE
			LEFT JOIN PKUB.RSXX_IBR_RPS RSXX
				ON RSXX.BF_SSN = DUE.BF_SSN
				AND RSXX.BC_STA_RSXX = 'A'
		WHERE
			COALESCE(RSXX.BD_CRT_RSXX, INPUT('XX/XX/XXXX', MMDDYYXX.)) > DUE.BD_ANV_QLF_IBR 
			AND COALESCE(RSXX.BD_CRT_RSXX, INPUT('XX/XX/XXXX', MMDDYYXX.)) BETWEEN (INTNX('month', DUE.BD_ANV_QLF_IBR, X, 'same')+X) AND INTNX('month', DUE.BD_ANV_QLF_IBR, X, 'same')
;
	CREATE TABLE DID_NOT_RECRT_DELQ AS
		SELECT DISTINCT
			F.BF_SSN,
			F.BD_ANV_QLF_IBR,
			'XX' AS CAT
		FROM
			RECRT_FAILED F
			LEFT JOIN RECRT_LATE_X LX
				ON LX.BF_SSN = F.BF_SSN
			LEFT JOIN RECRT_LATE_X LX
				ON LX.BF_SSN = F.BF_SSN
			LEFT JOIN RECRT_LATE_X LX
				ON LX.BF_SSN = F.BF_SSN
			LEFT JOIN RECRT_LATE_X LX
				ON LX.BF_SSN = F.BF_SSN
			LEFT JOIN RECRT_LATE_X LX
				ON LX.BF_SSN = F.BF_SSN
			LEFT JOIN RECRT_LATE_X LX
				ON LX.BF_SSN = F.BF_SSN
			INNER JOIN PKUB.LNXX_LON_DLQ_HST LNXX
				ON LNXX.BF_SSN = F.BF_SSN
				AND LNXX.LC_STA_LONXX = 'X'
				AND LNXX.LN_DLQ_MAX > XX
		WHERE
			LX.BF_SSN IS NULL
			AND LX.BF_SSN IS NULL 
			AND LX.BF_SSN IS NULL 
			AND LX.BF_SSN IS NULL 
			AND LX.BF_SSN IS NULL 
			AND LX.BF_SSN IS NULL
			AND LNXX.LN_DLQ_MAX IS NOT NULL
;
	CREATE TABLE DID_NOT_RECRT_CURRENT AS
		SELECT DISTINCT
			F.BF_SSN,
			F.BD_ANV_QLF_IBR,
			'XX' AS CAT
		FROM
			RECRT_FAILED F
			LEFT JOIN RECRT_LATE_X LX
				ON LX.BF_SSN = F.BF_SSN
			LEFT JOIN RECRT_LATE_X LX
				ON LX.BF_SSN = F.BF_SSN
			LEFT JOIN RECRT_LATE_X LX
				ON LX.BF_SSN = F.BF_SSN
			LEFT JOIN RECRT_LATE_X LX
				ON LX.BF_SSN = F.BF_SSN
			LEFT JOIN RECRT_LATE_X LX
				ON LX.BF_SSN = F.BF_SSN
			LEFT JOIN RECRT_LATE_X LX
				ON LX.BF_SSN = F.BF_SSN
			INNER JOIN PKUB.LNXX_LON LNXX
				ON LNXX.BF_SSN = F.BF_SSN
			INNER JOIN PKUB.DWXX_DW_CLC_CLU DWXX
				ON LNXX.BF_SSN = DWXX.BF_SSN
				AND LNXX.LN_SEQ = DWXX.LN_SEQ
			LEFT JOIN DID_NOT_RECRT_DELQ DELQ
				ON DELQ.BF_SSN = F.BF_SSN
		WHERE
			LX.BF_SSN IS NULL
			AND LX.BF_SSN IS NULL 
			AND LX.BF_SSN IS NULL 
			AND LX.BF_SSN IS NULL 
			AND LX.BF_SSN IS NULL 
			AND LX.BF_SSN IS NULL
			AND DELQ.BF_SSN IS NULL
			AND WC_DW_LON_STA = 'XX' 
;		
QUIT;

DATA DID_NOT_RECRT_CURRENT; SET LEGEND.DID_NOT_RECRT_CURRENT; RUN;

DATA FINAL;
	SET DID_NOT_RECRT_CURRENT DID_NOT_RECRT_DELQ RECRT_DUE RECRT_FAILED FAIL_DEF_FORB RECRT_LATE_X RECRT_LATE_X RECRT_LATE_X RECRT_LATE_X RECRT_LATE_X RECRT_LATE_X;
RUN;

ENDRSUBMIT;

DATA FINAL; SET LEGEND.FINAL; RUN;


%MACRO CREATE(NEWDS, TITLE, CAT);
PROC SQL;
	CREATE TABLE &NEWDS AS 
		SELECT DISTINCT
			&TITLE AS TITLE,
			A.NOV_XX,
			B.DEC_XX,
			C.JAN_XX,
			D.FEB_XX,
			E.MAR_XX,
			F.APR_XX,
			G.MAY_XX,
			H.JUN_XX,
			I.JUL_XX,
			J.AUG_XX,
			K.TOTAL
		FROM
			FINAL F
		LEFT JOIN (
				SELECT 
					&TITLE AS TITLE,
					COUNT(BF_SSN) AS NOV_XX
				FROM 
					FINAL 
				WHERE 
					CAT = &CAT 
					AND BD_ANV_QLF_IBR BETWEEN INPUT('XX/XX/XXXX',MMDDYYXX.) AND INPUT('XX/XX/XXXX',MMDDYYXX.)
				)A
					ON A.TITLE = &TITLE
		LEFT JOIN (
					SELECT
						&TITLE AS TITLE,
						COUNT(BF_SSN) AS DEC_XX
					FROM
						FINAL
					WHERE
						CAT = &CAT
						AND BD_ANV_QLF_IBR BETWEEN INPUT('XX/XX/XXXX',MMDDYYXX.) AND INPUT('XX/XX/XXXX',MMDDYYXX.)
				)B
					ON B.TITLE = &TITLE
		LEFT JOIN (
					SELECT
						&TITLE AS TITLE,
						COUNT(BF_SSN) AS JAN_XX
					FROM
						FINAL
					WHERE
						CAT = &CAT
						AND BD_ANV_QLF_IBR BETWEEN INPUT('XX/XX/XXXX',MMDDYYXX.) AND INPUT('XX/XX/XXXX',MMDDYYXX.) 
					) C
						ON C.TITLE = &TITLE
		LEFT JOIN (
					SELECT
						&TITLE AS TITLE,
						COUNT(BF_SSN) AS FEB_XX
					FROM
						FINAL
					WHERE
						CAT = &CAT
						AND BD_ANV_QLF_IBR BETWEEN INPUT('XX/XX/XXXX',MMDDYYXX.) AND INPUT('XX/XX/XXXX',MMDDYYXX.)
					)D
						ON D.TITLE = &TITLE
		LEFT JOIN (
					SELECT
						&TITLE AS TITLE,
						COUNT(BF_SSN) AS MAR_XX
					FROM
						FINAL
					WHERE
						CAT = &CAT
						AND BD_ANV_QLF_IBR BETWEEN INPUT('XX/XX/XXXX',MMDDYYXX.) AND INPUT('XX/XX/XXXX',MMDDYYXX.)
				)E
					ON E.TITLE = &TITLE
		LEFT JOIN (
					SELECT
						&TITLE AS TITLE,
						COUNT(BF_SSN) AS APR_XX
					FROM
						FINAL
					WHERE
						CAT = &CAT
						AND BD_ANV_QLF_IBR BETWEEN INPUT('XX/XX/XXXX',MMDDYYXX.) AND INPUT('XX/XX/XXXX',MMDDYYXX.)
				)F
					ON F.TITLE = &TITLE
		LEFT JOIN (
					SELECT
						&TITLE AS TITLE,
						COUNT(BF_SSN) AS MAY_XX
					FROM
						FINAL
					WHERE
						CAT = &CAT
						AND BD_ANV_QLF_IBR BETWEEN INPUT('XX/XX/XXXX',MMDDYYXX.) AND INPUT('XX/XX/XXXX',MMDDYYXX.)
				) G
					ON G.TITLE = &TITLE
		LEFT JOIN (
					SELECT
						&TITLE AS TITLE,
						COUNT(BF_SSN) AS JUN_XX
					FROM
						FINAL
					WHERE
						CAT = &CAT
						AND BD_ANV_QLF_IBR BETWEEN INPUT('XX/XX/XXXX',MMDDYYXX.) AND INPUT('XX/XX/XXXX',MMDDYYXX.)
				)H
					ON H.TITLE = &TITLE
		LEFT JOIN (
					SELECT
						&TITLE AS TITLE,
						COUNT(BF_SSN) AS JUL_XX
					FROM
						FINAL
					WHERE
						CAT = &CAT
						AND BD_ANV_QLF_IBR BETWEEN INPUT('XX/XX/XXXX',MMDDYYXX.) AND INPUT('XX/XX/XXXX',MMDDYYXX.)
				)I
					ON I.TITLE = &TITLE
		LEFT JOIN (
					SELECT
						&TITLE AS TITLE,
						COUNT(BF_SSN) AS AUG_XX
					FROM
						FINAL
					WHERE
						CAT = &CAT
						AND BD_ANV_QLF_IBR BETWEEN INPUT('XX/XX/XXXX',MMDDYYXX.) AND INPUT('XX/XX/XXXX',MMDDYYXX.)
				)J
					ON J.TITLE = &TITLE
		LEFT JOIN (
					SELECT
						&TITLE AS TITLE,
						COUNT(BF_SSN) AS TOTAL
					FROM
						FINAL
					WHERE
						CAT = &CAT
				)K
					ON K.TITLE = &TITLE
					
;
QUIT;
%MEND;

%CREATE(OUTPUTX, 'Number of borrowers whose IDR recertification was due', 'X');
%CREATE(OUTPUTX, 'Number of borrowers that failed to recertify on time', 'X');
%CREATE(OUTPUTX, 'Number of borrowers, after failing to recertify on time, went into a hardship related forbearance or deferment', 'X');
%CREATE(OUTPUTX, 'Number of borrowers, after failing to recertify on time, successfully recertified within X month after the deadline:', 'X');
%CREATE(OUTPUTX, '...within X months', 'X');
%CREATE(OUTPUTX, '...within X months', 'X');
%CREATE(OUTPUTX, '...within X months', 'X');
%CREATE(OUTPUTX, '...within X months', 'X');
%CREATE(OUTPUTX, '...within X months', 'X');
%CREATE(OUTPUTXX, 'Number of borrowers, after failing to recertify on time, failed to recertify within X months and are delinquent (>XX days)', 'XX');
%CREATE(OUTPUTXX, 'Number of borrowers, after failing to recertify on time, failed to recertify within X months and are current in active repayment (not in deferment, not in forbearance, and are XX days delinquent or less)', 'XX');




PROC SQL NOPRINT;
CONNECT TO ODBC AS IDR (REQUIRED="FILEDSN=X:\PADR\ODBC\Income_Driven_Repayment.dsn; update_lock_typ=nolock; bl_keepnulls=no");
CREATE TABLE ALT AS
SELECT *
FROM CONNECTION TO IDR (
			SELECT  
				*
 			FROM 
				[dbo].[Applications]
   			WHERE 
				created_at BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
);
DISCONNECT FROM IDR;
QUIT;


PROC SQL;
	CREATE TABLE ALT_TOTAL AS 
		SELECT
			'TOTAL' AS LABEL,
			COUNT(*) AS TOTAL_COUNT
		FROM
			ALT
;
	CREATE TABLE ALT_ALT AS
		SELECT
			'TOTAL' AS LABEL,
			COUNT(*) AS ALT_COUNT
		FROM
			ALT
		WHERE
			income_source_id = X
;
	CREATE TABLE ALT_FINAL AS 
		SELECT
			'Overall, over these ten months, what percentage of IDR applications needed to use alternative documentation of income?' AS TITLE,
			((A.ALT_COUNT / T.TOTAL_COUNT)) AS NOV_XX FORMAT PERCENTXX.X
		FROM
			ALT_TOTAL T
		INNER JOIN ALT_ALT A
			ON A.LABEL = T.LABEL
;		
QUIT;

DATA TOTALS;
	LENGTH TITLE $XXX.;
	SET OUTPUTX OUTPUTX OUTPUTX OUTPUTX OUTPUTX OUTPUTX OUTPUTX OUTPUTX OUTPUTX OUTPUTXX OUTPUTXX ALT_FINAL;
RUN; 


PROC EXPORT DATA = WORK.TOTALS 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SHEETX"; 
RUN;

PROC SQL;
	CREATE TABLE TEST AS 
		SELECT
			*
		FROM 	
			DID_NOT_RECRT_CURRENT
		ORDER BY 
			BD_ANV_QLF_IBR
;
QUIT;
