LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE IP AS
		SELECT DISTINCT
			AYXX.BF_SSN,
			AYXX.PF_REQ_ACT,
			AYXX.LD_ATY_REQ_RCV,
			AYXX.LX_ATY,
			CASE 
				WHEN AYXX.PF_REQ_ACT = 'DCSLD' THEN 'DMCS'
				WHEN AYXX.PF_REQ_ACT = 'AXDCV' AND AYXX.LX_ATY LIKE ('PSLF TRANS%') THEN 'PSLF'
				WHEN AYXX.PF_REQ_ACT = 'AXDCV' AND AYXX.LX_ATY LIKE ('TPD TRANS%') THEN 'TPD'
				WHEN AYXX.PF_REQ_ACT = 'AXDCV' AND AYXX.LX_ATY LIKE ('SPLIT LOAN TRANSFER TO PHEAA%') THEN 'Split to PHEAA'
				WHEN AYXX.PF_REQ_ACT = 'AXDCV' AND AYXX.LX_ATY LIKE ('SPLIT LOAN TRANSFER TO NELNET%') THEN 'Split to NLNT'
				WHEN AYXX.PF_REQ_ACT = 'AXDCV' AND AYXX.LX_ATY LIKE ('SPLIT LOAN TRANSFER TO SLMA%') THEN 'Split to SLMA'
				WHEN AYXX.PF_REQ_ACT = 'AXDCV' AND (AYXX.LX_ATY LIKE ('SPLIT LOAN TRANSFER TO GREAT LAKES%') OR AYXX.LX_ATY LIKE ('GREAT LAKES%')) THEN 'Split to GL'
				ELSE ''
			END AS SENTTO,
			CASE
				WHEN FSXX.BF_SSN IS NOT NULL THEN X
				ELSE X
			END AS HASNEGAM
		FROM
			PKUB.AYXX_BR_LON_ATY AYXX
			JOIN PKUB.AYXX_ATY_TXT AYXX
				ON AYXX.BF_SSN = AYXX.BF_SSN
				AND AYXX.LN_ATY_SEQ = AYXX.LN_ATY_SEQ
			LEFT JOIN PKUB.FSXX_DL_LON FSXX
				ON AYXX.BF_SSN = FSXX.BF_SSN
				AND (FSXX.LA_ICR_NEG_AMR_IC IS NOT NULL OR FSXX.LA_PAE_NEG_AMR_IC IS NOT NULL)
				AND FSXX.LF_LST_DTS_FSXX GE 'XXJULXXXX'D
		WHERE
			AYXX.PF_REQ_ACT IN ('DCSLD','AXDCV')
			AND AYXX.LD_ATY_REQ_RCV GE 'XXJULXXXX'D
		ORDER BY
			BF_SSN
	;
QUIT;
ENDRSUBMIT;

DATA IP; SET LEGEND.IP; RUN;

PROC SQL;
	CREATE TABLE CNTS AS
		SELECT
			PF_REQ_ACT,
			LD_ATY_REQ_RCV,
			SENTTO,
			HASNEGAM,
			COUNT(DISTINCT BF_SSN) AS CNT
		FROM
			IP
		GROUP BY
			PF_REQ_ACT,
			LD_ATY_REQ_RCV,
			SENTTO,
			HASNEGAM
		ORDER BY
			PF_REQ_ACT,
			SENTTO,
			LD_ATY_REQ_RCV,
			HASNEGAM
	;

	CREATE TABLE DISTCNTS AS
		SELECT DISTINCT
			SENTTO,
			LD_ATY_REQ_RCV
		FROM
			CNTS
	;

	CREATE TABLE CNTSX AS
		SELECT
			CNTS.SENTTO AS TRANSFER_TO,
			CNTS.LD_ATY_REQ_RCV AS TRANSFER_DATE,
			COALESCE(HASNOT.CNT,X) AS NO_NEG_AM_CNT,
			COALESCE(NEGAM.CNT,X) AS NEG_AM_CNT,
			COALESCE(HASNOT.CNT,X) + COALESCE(NEGAM.CNT,X) AS GRAND_TOTAL,
			COALESCE(NEGAM.CNT,X)/CALCULATED GRAND_TOTAL AS PERCENT_NEG_AM
		FROM
			DISTCNTS CNTS
			LEFT JOIN
				(
					SELECT
						SENTTO,
						LD_ATY_REQ_RCV,
						COALESCE(CNT,X) AS CNT
					FROM
						CNTS
					WHERE
						HASNEGAM = X
				) HASNOT
				ON CNTS.SENTTO = HASNOT.SENTTO
				AND CNTS.LD_ATY_REQ_RCV = HASNOT.LD_ATY_REQ_RCV
			LEFT JOIN
				(
					SELECT
						SENTTO,
						LD_ATY_REQ_RCV,
						COALESCE(CNT,X) AS CNT
					FROM
						CNTS
					WHERE
						HASNEGAM = X
				) NEGAM
				ON CNTS.SENTTO = NEGAM.SENTTO
				AND CNTS.LD_ATY_REQ_RCV = NEGAM.LD_ATY_REQ_RCV
	;

	CREATE TABLE CNTS_FINAL AS
		SELECT
			TRANSFER_TO,
			TRANSFER_DATE,
			GRAND_TOTAL,
			PERCENT_NEG_AM
		FROM
			CNTSX
	;
QUIT;

PROC SQL;
	CREATE TABLE BLANKS AS
		SELECT *
		FROM IP
		WHERE SENTTO = ''
	;
QUIT;

PROC EXPORT
		DATA=CNTS_FINAL
		OUTFILE='T:\SAS\NH XXXX.XLSX'
		REPLACE;
	SHEET = 'REPORT';
RUN;

PROC EXPORT
		DATA=IP
		OUTFILE='T:\SAS\NH XXXX.XLSX'
		REPLACE;
	SHEET = 'DETAIL';
RUN;

PROC EXPORT
		DATA=BLANKS
		OUTFILE='T:\SAS\NH XXXX.XLSX'
		REPLACE;
	SHEET = 'BLANKS';
RUN;

