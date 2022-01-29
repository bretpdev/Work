LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;

DATA PBC;
	INFILE 'T:\UTXXXXX.MBXX.EXTRACT.KUDLRGNP.#XXXXXXX.ALLOANS_DLO';
	INPUT BF_SSN $ XXX-XXX LN_SEQ XX-XX LA_INT XX-XXX;
RUN;

DATA LEGEND.PBC; SET PBC; RUN;

RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
LIBNAME PKUR DBX DATABASE=DNFPUTDL OWNER=PKUR;
LIBNAME AES DBX DATABASE=DNFPUTDL OWNER=AES;

/*The sum of all active/unreversed financial activity from LNXX <> the outstanding non-subsidized interest from LNXX */
PROC SQL;
	CREATE TABLE LNXXL AS
		SELECT DISTINCT
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			SUM(LNXX.LA_FAT_NSI) AS LA_INT_CAL
		FROM
			PKUB.LNXX_FIN_ATY LNXX
		WHERE
			LNXX.LC_STA_LONXX = 'A'
			AND LNXX.LC_FAT_REV_REA = ''
		GROUP BY	
			LNXX.BF_SSN,
			LNXX.LN_SEQ			
	;
QUIT;

PROC SQL;
	CREATE TABLE LNXX_LNXX AS
		SELECT DISTINCT
			PUT(PHXX.DF_SPE_ID,ZXX.) AS DF_SPE_ID,
			LNXX.LN_SEQ,
			LNXX.LC_STA_LONXX,
			LNXX.LA_INT_CAL,
			LNXX.LA_NSI_OTS AS LA_INT,
			LNXX.LA_INT_CAL - LNXX.LA_NSI_OTS AS LA_INT_DIF
		FROM
			AES.PHXX_SUPER_ID PHXX
			JOIN PKUB.LNXX_LON LNXX
				ON PHXX.DF_PRS_ID = LNXX.BF_SSN
			JOIN LNXXL LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
		WHERE
			LNXX.LA_INT_CAL NE LNXX.LA_NSI_OTS
	;
QUIT;

/*The sum of all active/unreversed activity from LNXX + sum of the FRXX�s <> PBC loan balance	*/
PROC SQL;
	CREATE TABLE LNXXF AS
		SELECT DISTINCT
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			SUM(LNXX.LA_FAT_NSI) AS LA_FAT_NSI
		FROM
			PKUB.LNXX_FIN_ATY LNXX
		WHERE
			LNXX.LC_STA_LONXX = 'A'
			AND LNXX.LC_FAT_REV_REA = ''
			AND LNXX.LD_FAT_APL < INTNX('MONTH',TODAY(),X,'BEGINNING')
		GROUP BY	
			LNXX.BF_SSN,
			LNXX.LN_SEQ
	;

	CREATE TABLE FRXX AS
		SELECT DISTINCT
			FRXX.BF_SSN,
			FRXX.LN_SEQ,
			SUM(FRXX.LA_FMS_MTH_INT_ADJ + FRXX.LA_FMS_MTH_INT_ACR) AS LA_FRXX
		FROM
			PKUR.FRXX_MTH_INT_RPT FRXX
		GROUP BY
			FRXX.BF_SSN,
			FRXX.LN_SEQ
	;

	CREATE TABLE LNXX_FRXX_PBC AS
		SELECT DISTINCT
			PUT(PHXX.DF_SPE_ID,ZXX.) AS DF_SPE_ID,
			LNXX.LN_SEQ,
			LNXX.LC_STA_LONXX,
			COALESCE(LNXX.LA_FAT_NSI,X) + COALESCE(FRXX.LA_FRXX,X) AS LA_INT_CAL,
			PBC.LA_INT,
			COALESCE(LNXX.LA_FAT_NSI,X) + COALESCE(FRXX.LA_FRXX,X) - COALESCE(PBC.LA_INT,X) AS LA_INT_DIF
			,LNXX.LA_FAT_NSI
			,FRXX.LA_FRXX
		FROM
			AES.PHXX_SUPER_ID PHXX
			JOIN PKUB.LNXX_LON LNXX
				ON PHXX.DF_PRS_ID = LNXX.BF_SSN
			JOIN PBC
				ON LNXX.BF_SSN = PBC.BF_SSN
				AND LNXX.LN_SEQ = PBC.LN_SEQ
			JOIN LNXXF LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
			JOIN FRXX
				ON LNXX.BF_SSN = FRXX.BF_SSN
				AND LNXX.LN_SEQ = FRXX.LN_SEQ
		WHERE
			COALESCE(LNXX.LA_FAT_NSI,X) + COALESCE(FRXX.LA_FRXX,X) NE COALESCE(PBC.LA_INT,X)
	;
QUIT;

DATA DATA_OUT;
	SET LNXX_LNXX LNXX_FRXX_PBC;
RUN;
ENDRSUBMIT;

/*DATA DATA_OUT; SET LEGEND.DATA_OUT (OBS=XXXX); RUN;*/
DATA LNXX_LNXX; SET LEGEND.LNXX_LNXX (OBS=XXXX); RUN;
DATA LNXX_FRXX_PBC; SET LEGEND.LNXX_FRXX_PBC (OBS=XXXX); RUN;
