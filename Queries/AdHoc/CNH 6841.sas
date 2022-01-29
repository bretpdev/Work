LIBNAME  LEGEND  REMOTE  SERVER=LEGEND  SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;

PROC SQL;
	CREATE TABLE DEMO AS
		SELECT DISTINCT	
			A.BF_SSN
		FROM	
			PKUB.LNXX_LON A
			INNER JOIN PKUB.PDXX_PRS_NME B
				ON A.BF_SSN = B.DF_PRS_ID
			INNER JOIN PKUB.DWXX_DW_CLC_CLU C
				ON A.BF_SSN = C.BF_SSN
				AND A.LN_SEQ = C.LN_SEQ
		WHERE	
			A.LC_STA_LONXX = 'R'
			AND A.LA_CUR_PRI > X
			AND	C.WC_DW_LON_STA IN ('XX','XX','XX','XX','XX','XX','XX','XX','XX','XX')
			AND A.IC_LON_PGM NOT IN ('DLUCNS', 'DLSCNS', 'CNSLDN', 'DSCON', 'DLUSPL', 'DUCON', 'SPCNSL', 
							'SUBCNS', 'SUBSPC', 'UNCNS', 'DLSPCN', 'DLSSPL', 'DLCNSL', 'DLPCNS', 
							'UNSPC', 'DLSCST', 'DLSCUN', 'DLSCPL', 'DLSCPG', 'DLSCSL', 'DLSCSC', 'DLSCUC', 'DLSCCN')
	;
ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

PROC EXPORT DATA= WORK.DEMO
            OUTFILE= "T:\SAS\SSAE XX Audit Account Accruing Interest.xlsx" 
            DBMS=EXCEL REPLACE;
RUN;
