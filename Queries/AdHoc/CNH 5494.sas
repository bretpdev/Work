LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE ITEMX AS
	    SELECT
			LNXXA.BF_SSN,
			LNXXA.LN_SEQ
	     FROM
			PKUB.LNXX_LON_BIL_CRF LNXXA
	    	INNER JOIN PKUB.LNXX_LON LNXXA
				ON  LNXXA.BF_SSN               = LNXXA.BF_SSN
	      		AND LNXXA.LN_SEQ               = LNXXA.LN_SEQ
	      		AND LNXXA.LC_STA_LONXX         = 'R'
	      		AND LNXXA.LA_CUR_PRI           > X
		    INNER JOIN PKUB.LNXX_LON_BBS_TIR LNXXA
				ON LNXXA.BF_SSN               = LNXXA.BF_SSN
				AND LNXXA.LN_SEQ               = LNXXA.LN_SEQ
				AND LNXXA.LC_STA_LNXX          = 'A'
				AND LNXXA.PM_BBS_PGM           = 'DLR'
				AND LNXXA.LC_LON_BBT_STA       = 'A'
				AND LNXXA.LN_BBT_PAY_DLQ_MOT   = XX
		    INNER JOIN PKUB.LNXX_LON_BBS LNXXA
				ON LNXXA.BF_SSN               = LNXXA.BF_SSN
				AND LNXXA.LN_SEQ               = LNXXA.LN_SEQ
				AND LNXXA.LC_STA_LNXX          = 'A'
	    WHERE 
			LNXXA.LC_STA_LONXX = 'A'
			AND LNXXA.LC_BIL_TYP_LON = 'P'
			AND (XX -  COALESCE(LNXXA.LN_BBS_PCV_PAY_MOT,X)) = 
				(
					SELECT
						COUNT(*)
					FROM 
						PKUB.LNXX_LON_BIL_CRF LNXXB
					WHERE 
						LNXXA.BF_SSN       = LNXXB.BF_SSN
						AND LNXXA.LN_SEQ       = LNXXB.LN_SEQ
						AND LNXXB.LC_STA_LONXX = 'A'
						AND LNXXB.LC_BIL_TYP_LON = 'P'
						AND LNXXB.LA_BIL_CUR_DU = X
						AND LNXXA.LD_BIL_DU_LON >  LNXXB.LD_BIL_DU_LON
				)
			AND LNXXA.LD_BBT_STS_PAY < LNXXA.LD_BIL_DU_LON
	;
QUIT;

PROC SQL;
	CREATE TABLE ITEMX AS
		SELECT 
			LNXXA.BF_SSN,
			LNXXA.LN_SEQ
		FROM 
			PKUB.LNXX_LON_BIL_CRF LNXXA
			INNER JOIN PKUB.LNXX_LON LNXXA
				ON  LNXXA.BF_SSN               = LNXXA.BF_SSN
	      		AND LNXXA.LN_SEQ               = LNXXA.LN_SEQ
				AND LNXXA.LC_STA_LONXX         = 'R'
				AND LNXXA.LA_CUR_PRI           > X
			INNER JOIN PKUB.LNXX_LON_BBS_TIR LNXXA
				ON LNXXA.BF_SSN               = LNXXA.BF_SSN
				AND LNXXA.LN_SEQ               = LNXXA.LN_SEQ
				AND LNXXA.LC_STA_LNXX          = 'A'
				AND LNXXA.PM_BBS_PGM           = 'DLR'
				AND LNXXA.LC_LON_BBT_STA       IN ('P','D')
			INNER JOIN PKUB.LNXX_LON_BBS LNXXA
				ON LNXXA.BF_SSN               = LNXXA.BF_SSN
				AND LNXXA.LN_SEQ               = LNXXA.LN_SEQ
				AND LNXXA.LC_STA_LNXX          = 'A'
				AND LNXXA.LN_BBS_PCV_PAY_MOT   IS NOT NULL
		WHERE 
			LNXXA.LC_STA_LONXX = 'A'
			AND LNXXA.LC_BIL_TYP_LON = 'P'
			AND LNXXA.LD_BBS_DSQ_APL > LNXXA.LD_BIL_DU_LON
			AND (XX - LNXXA.LN_BBS_PCV_PAY_MOT) =
				(
					SELECT 
						COUNT(*)
					FROM 
						PKUB.LNXX_LON_BIL_CRF LNXXB
					WHERE 
						LNXXA.BF_SSN       = LNXXB.BF_SSN
						AND LNXXA.LN_SEQ       = LNXXB.LN_SEQ
						AND LNXXB.LC_STA_LONXX = 'A'
						AND LNXXB.LC_BIL_TYP_LON = 'P'
						AND LNXXA.LD_BIL_DU_LON > LNXXB.LD_BIL_DU_LON
				)
	;
QUIT;

PROC SQL;
	CREATE TABLE ITEMX AS
		SELECT 
			LNXXA.BF_SSN
			,LNXXA.LN_SEQ
			,LNXXA.LD_BIL_DU_LON
		FROM 
			PKUB.LNXX_LON_BIL_CRF LNXXA
			INNER JOIN PKUB.LNXX_LON LNXXA
				ON  LNXXA.BF_SSN               = LNXXA.BF_SSN
	      		AND LNXXA.LN_SEQ               = LNXXA.LN_SEQ
				AND LNXXA.LC_STA_LONXX         = 'R'
				AND LNXXA.LA_CUR_PRI           > X
			INNER JOIN PKUB.LNXX_LON_BBS_TIR LNXXA
				ON LNXXA.BF_SSN               = LNXXA.BF_SSN
				AND LNXXA.LN_SEQ               = LNXXA.LN_SEQ
				AND LNXXA.LC_STA_LNXX          = 'A'
				AND LNXXA.PM_BBS_PGM           = 'DLR'
				AND LNXXA.LC_LON_BBT_STA       = 'A'
				AND LNXXA.LN_BBT_PAY_DLQ_MOT   = XX
			INNER JOIN PKUB.LNXX_LON_BBS LNXXA
				ON LNXXA.BF_SSN               = LNXXA.BF_SSN
				AND LNXXA.LN_SEQ               = LNXXA.LN_SEQ
				AND LNXXA.LC_STA_LNXX          = 'A'
		WHERE 
			LNXXA.LC_STA_LONXX = 'A'
			AND LNXXA.LC_BIL_TYP_LON = 'P'
			AND (XX - COALESCE(LNXXA.LN_BBS_PCV_PAY_MOT,X)) >
				(
					SELECT 
						COUNT(*)
					FROM 
						PKUB.LNXX_LON_BIL_CRF LNXXB
					WHERE 
						LNXXA.BF_SSN       = LNXXB.BF_SSN
						AND LNXXA.LN_SEQ       = LNXXB.LN_SEQ
						AND LNXXB.LC_STA_LONXX = 'A'
						AND LNXXB.LC_BIL_TYP_LON = 'P'
						AND LNXXB.LA_BIL_CUR_DU > X
						AND LNXXA.LD_BIL_DU_LON >
						LNXXB.LD_BIL_DU_LON
				)
			AND LNXXA.LD_BIL_DU_LON + X < LNXXA.LD_BIL_STS_RIR_TOL
			AND LNXXA.LD_BIL_STS_RIR_TOL IS NOT NULL
		ORDER BY 
			LNXXA.BF_SSN
			,LNXXA.LN_SEQ
			,LNXXA.LD_BIL_DU_LON
	;
QUIT;
ENDRSUBMIT;

DATA ITEMX; SET LEGEND.ITEMX; RUN;
DATA ITEMX; SET LEGEND.ITEMX; RUN;
DATA ITEMX; SET LEGEND.ITEMX; RUN;

PROC EXPORT
		DATA=ITEMX
		OUTFILE='T:\SAS\DLR BB Disqualification - Detail.xlsx'
		REPLACE;
RUN;

PROC EXPORT
		DATA=ITEMX
		OUTFILE='T:\SAS\DLR BB Disqualification - Detail.xlsx'
		REPLACE;
RUN;

PROC EXPORT
		DATA=ITEMX
		OUTFILE='T:\SAS\DLR BB Disqualification - Detail.xlsx'
		REPLACE;
RUN;