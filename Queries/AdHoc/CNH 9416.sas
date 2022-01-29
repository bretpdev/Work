LIBNAME XL 'T:\Copy of June-XXXX Transfers.xlsx';

DATA WORK.GREAT_LAKES (KEEP=BF_SSN);
	SET XL.'GREAT LAKES$'N;
	BF_SSN=PUT(BORROWER_SSN, X.);
RUN;

DATA WORK.SOURCE;
      SET
		XL.'GRANITE STATE$'N 
/*		XL.'GREAT LAKES$'N*/
		WORK.GREAT_LAKES
		XL.'CORNERSTONE$'N 
		XL.'ESA$'N 
		XL.'MOHELA$'N 
		XL.'NELNET$'N 
		XL.'OSLA$'N 
		XL.'PHEAA$'N
		XL.'SLMA$'N 
		XL.'VSAC$'N
	;
      BF_SSN = STRIP(COMPRESS(BORROWER_SSN,'-'));
      FORMAT BF_SSN $X.;
      KEEP BF_SSN;
RUN;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
DATA LEGEND.SOURCE; *Send data to legend;
SET SOURCE;
RUN;

RSUBMIT LEGEND;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
LIBNAME PKUS DBX DATABASE=DNFPUTDL OWNER=PKUS;
PROC SQL;
	CREATE TABLE POP AS
		SELECT DISTINCT	
			S.BF_SSN,
			PDXX.DM_PRS_X,
			PDXX.DM_PRS_LST,
			LNXX.LN_SEQ,
			LNXX.IC_LON_PGM,
			LNXX.LA_CUR_PRI + DWXX.LA_NSI_OTS + DWXX.LA_NSI_ACR AS LA_LON_BAL,
			RPS.LC_TYP_SCH_DIS,
			BAL.LA_TOT_BAL,
			CASE 
				WHEN DWXXS.WC_DW_LON_STA IN ('XX','XX') THEN X
				ELSE X
			END AS IN_BNK,
			CASE 
				WHEN DWXXS.WC_DW_LON_STA IN ('XX','XX') THEN X
				ELSE X
			END AS IN_TPD,
			CASE 
				WHEN DWXXS.WC_DW_LON_STA IN ('XX','XX') THEN X
				ELSE X
			END AS IN_DTH,
			CASE
				WHEN AYXX.BF_SSN IS NOT NULL THEN AYXX.PF_REQ_ACT
				ELSE ''
			END AS OTHER_SPCL,
			DELQ.LN_DLQ_MAX,
			CASE
				WHEN CONS.BF_SSN IS NOT NULL THEN X
				ELSE X
			END AS IS_CONS
		FROM
			SOURCE S
			INNER JOIN PKUB.PDXX_PRS_NME PDXX
				ON S.BF_SSN = PDXX.DF_PRS_ID
			INNER JOIN PKUB.LNXX_LON LNXX
				ON S.BF_SSN = LNXX.BF_SSN
				AND LNXX.LA_CUR_PRI > X
				AND LNXX.LC_STA_LONXX = 'R'
			INNER JOIN PKUB.DWXX_DW_CLC_CLU DWXX
				ON LNXX.BF_SSN = DWXX.BF_SSN
				AND LNXX.LN_SEQ = DWXX.LN_SEQ
			LEFT JOIN 
				(
					SELECT DISTINCT
						RSXX.BF_SSN,
						LNXX.LN_SEQ,
						LNXX.LC_TYP_SCH_DIS
					FROM
						PKUB.RSXX_BR_RPD RSXX
						INNER JOIN PKUB.LNXX_LON_RPS LNXX
							ON RSXX.BF_SSN = LNXX.BF_SSN
							AND RSXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
					WHERE 
						RSXX.LC_STA_RPSTXX = 'A'
						AND LNXX.LC_STA_LONXX = 'A'
				) RPS
				ON LNXX.BF_SSN = RPS.BF_SSN
				AND LNXX.LN_SEQ = RPS.LN_SEQ
			INNER JOIN 
				(
					SELECT DISTINCT
						LNXX.BF_SSN,
						SUM(LNXX.LA_CUR_PRI + DWXX.LA_NSI_OTS + DWXX.LA_NSI_ACR) AS LA_TOT_BAL
					FROM
						PKUB.LNXX_LON LNXX
						INNER JOIN PKUB.DWXX_DW_CLC_CLU DWXX
							ON LNXX.BF_SSN = DWXX.BF_SSN
							AND LNXX.LN_SEQ = DWXX.LN_SEQ
					GROUP BY
						LNXX.BF_SSN
				) BAL
				ON LNXX.BF_SSN = BAL.BF_SSN
			LEFT JOIN PKUB.AYXX_BR_LON_ATY AYXX
				ON LNXX.BF_SSN = AYXX.BF_SSN
				AND AYXX.PF_REQ_ACT IN ('CSFSA','TLFSA','FCFSA')
			LEFT JOIN 
				(
					SELECT DISTINCT 
						BF_SSN,
						WC_DW_LON_STA
					FROM 
						PKUB.DWXX_DW_CLC_CLU
					WHERE 
						WC_DW_LON_STA IN ('XX','XX','XX','XX','XX','XX')	
				) DWXXS
				ON LNXX.BF_SSN = DWXXS.BF_SSN
			LEFT JOIN
				(
					SELECT DISTINCT
						BF_SSN,
						MAX(LN_DLQ_MAX) AS LN_DLQ_MAX
					FROM
						PKUB.LNXX_LON_DLQ_HST
					WHERE
						LC_STA_LONXX = 'X'
						AND LD_STA_LONXX = (TODAY() -X)
					GROUP BY
						BF_SSN
				) DELQ
				ON DELQ.BF_SSN = LNXX.BF_SSN
			LEFT JOIN
				(
					SELECT DISTINCT
						BF_SSN
					FROM
						PKUB.AYXX_BR_LON_ATY
					WHERE
						PF_REQ_ACT = 'DLCON'
						AND LD_ATY_REQ_RCV >= TODAY() - XXX
				) CONS
				ON LNXX.BF_SSN = CONS.BF_SSN
	;
QUIT;
ENDRSUBMIT;

DATA POP; SET LEGEND.POP; RUN;

DATA REASONS;
	SET POP;
	IF LA_TOT_BAL <= XX THEN
		REASONX = 'Total Balance <= $XX';
	IF LN_DLQ_MAX >= XXX THEN
		REASONX = 'Active Delinquency =XXX';
	IF IS_CONS = X THEN
		REASONX = 'Consolidation in Process';
	IF IN_BNK = X THEN
		REASONX = 'Specialty Claim/Bankruptcy';
	IF IN_TPD = X THEN
		REASONX = 'Specialty Claim/TPD';
	IF IN_DTH = X THEN
		REASONX = 'Specialty Claim/Death';
	IF OTHER_SPCL ^= '' THEN
		REASONX = CATS('Specialty Claim/',OTHER_SPCL);

	REASON = CATX('|',REASONX,REASONX,REASONX,REASONX,REASONX,REASONX,REASONX);
RUN;

PROC SQL;
	CREATE TABLE EXLD AS
		SELECT DISTINCT
			BF_SSN,
			DM_PRS_X,
			DM_PRS_LST,
			REASON
		FROM
			REASONS
		WHERE 
			REASON ^= ''
	;

	CREATE TABLE NONEXLD AS
		SELECT DISTINCT
			BF_SSN,
			DM_PRS_X,
			DM_PRS_LST,
			IC_LON_PGM,
			LA_LON_BAL,
			LC_TYP_SCH_DIS
		FROM
			REASONS
		WHERE 
			REASON = ''
	;
QUIT;

PROC EXPORT
		DATA=EXLD
		OUTFILE='T:\Split Loan Transfer XX-XX-XXXX.xlsx'
		REPLACE;
RUN;

PROC EXPORT
		DATA=NONEXLD
		OUTFILE='T:\Split Loan Transfer XX-XX-XXXX.xlsx'
		REPLACE;
RUN;
