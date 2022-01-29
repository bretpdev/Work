LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;

PROC SQL;

	CREATE TABLE LNXX AS
		SELECT DISTINCT
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LN_RPS_SEQ,
			LNXX.LA_CUR_PRI,
			AYXX.PF_REQ_ACT
		FROM
			PKUB.LNXX_LON_RPS LNXX
			LEFT JOIN 
				(
					SELECT DISTINCT
						BF_SSN,
						SUM(LA_CUR_PRI) AS LA_CUR_PRI
					FROM
						PKUB.LNXX_LON
					GROUP BY
						BF_SSN
				) LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
			LEFT JOIN PKUB.AYXX_BR_LON_ATY AYXX
				ON LNXX.BF_SSN = AYXX.BF_SSN
				AND AYXX.PF_REQ_ACT IN ('IBADI', 'IBRDF', 'DRICR', 'IDRPR', 'DILIB', 'DRIBR', 'IBREA', 'CODCA', 'CODPA')
		WHERE
			 LNXX.LC_TYP_SCH_DIS IN ('IB', 'IL', 'CX', 'CX', 'CX', 'CQ', 'CA', 'CP') 
			 AND (LNXX.LA_CUR_PRI = X OR AYXX.BF_SSN IS NOT NULL)
	;

/*	lnXX records with no RSXX record*/
	CREATE TABLE _X_RSXX AS
		SELECT DISTINCT
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LN_RPS_SEQ
		FROM
			LNXX
			JOIN PKUB.RSXX_BR_RPD RSXX
				ON LNXX.BF_SSN = RSXX.BF_SSN
				AND LNXX.LN_RPS_SEQ = RSXX.LN_RPS_SEQ
			LEFT JOIN PKUB.RSXX_IBR_IRL_LON RSXX
				ON LNXX.BF_SSN = RSXX.BF_SSN
				AND LNXX.LN_SEQ = RSXX.LN_SEQ
				AND RSXX.BN_IBR_SEQ = RSXX.BN_IBR_SEQ
		WHERE
			RSXX.BF_SSN IS NULL
	;

/*	lnXX records with no RSXX record*/
	CREATE TABLE _X_RSXX AS
		SELECT DISTINCT
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LN_RPS_SEQ
		FROM
			LNXX
			JOIN PKUB.RSXX_BR_RPD RSXX
				ON LNXX.BF_SSN = RSXX.BF_SSN
				AND LNXX.LN_RPS_SEQ = RSXX.LN_RPS_SEQ
			LEFT JOIN PKUB.RSXX_IBR_RPS RSXX
				ON LNXX.BF_SSN = RSXX.BF_SSN
				AND RSXX.BD_CRT_RSXX = RSXX.BD_CRT_RSXX
				AND RSXX.BN_IBR_SEQ = RSXX.BN_IBR_SEQ
		WHERE
			RSXX.BF_SSN IS NULL
	;

	CREATE TABLE _X_RSXX AS
		SELECT DISTINCT
			RSXX.BF_SSN,
			RSXX.BD_CRT_RSXX,
			RSXX.BN_IBR_SEQ,
			RSXX.LN_SEQ
		FROM
			LNXX
			JOIN PKUB.RSXX_IBR_IRL_LON RSXX
				ON LNXX.BF_SSN = RSXX.BF_SSN
				AND LNXX.LN_SEQ = RSXX.LN_SEQ
			LEFT JOIN PKUB.RSXX_IBR_RPS RSXX
				ON RSXX.BF_SSN = RSXX.BF_SSN
				AND RSXX.BD_CRT_RSXX = RSXX.BD_CRT_RSXX
				AND RSXX.BN_IBR_SEQ = RSXX.BN_IBR_SEQ
		WHERE
			RSXX.BF_SSN IS NULL
	;

	CREATE TABLE _X_RSXX AS
		SELECT DISTINCT
			RSXX.BF_SSN,
			RSXX.BD_CRT_RSXX,
			RSXX.BN_IBR_SEQ
		FROM
			_X_RSXX RSXX
			JOIN PKUB.RSXX_IBR_RPS RSXX
				ON RSXX.BF_SSN = RSXX.BF_SSN

	;

	CREATE TABLE _X_RSXX AS
		SELECT DISTINCT
			RSXX.BF_SSN,
			RSXX.BD_CRT_RSXX,
			RSXX.BN_IBR_SEQ
		FROM
			LNXX
			JOIN PKUB.RSXX_IBR_RPS RSXX
				ON LNXX.BF_SSN = RSXX.BF_SSN
			LEFT JOIN PKUB.RSXX_IBR_IRL_LON RSXX
				ON RSXX.BF_SSN = RSXX.BF_SSN
				AND RSXX.BD_CRT_RSXX = RSXX.BD_CRT_RSXX
				AND RSXX.BN_IBR_SEQ = RSXX.BN_IBR_SEQ
		WHERE
			RSXX.BF_SSN IS NULL
	;

	CREATE TABLE _X_RSXX AS
		SELECT DISTINCT
			RSXX.BF_SSN,
			RSXX.BD_CRT_RSXX,
			RSXX.BN_IBR_SEQ
		FROM
			_X_RSXX RSXX
			JOIN PKUB.RSXX_IBR_IRL_LON RSXX
				ON RSXX.BF_SSN = RSXX.BF_SSN
	;

	CREATE TABLE _X_LNXX AS
		SELECT DISTINCT
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LN_RPS_SEQ
		FROM
			LNXX IDR
			JOIN PKUB.LNXX_LON_RPS LNXX
				ON IDR.BF_SSN = LNXX.BF_SSN
				AND IDR.LN_SEQ = LNXX.LN_SEQ
			LEFT JOIN PKUB.RSXX_BR_RPD RSXX
				ON LNXX.BF_SSN = RSXX.BF_SSN
				AND LNXX.LN_RPS_SEQ = RSXX.LN_RPS_SEQ
		WHERE
			RSXX.BF_SSN IS NULL
	;

	CREATE TABLE _X_RSXX AS
		SELECT DISTINCT
			RSXX.BF_SSN,
			RSXX.LN_RPS_SEQ
		FROM
			_X_LNXX LNXX
			JOIN PKUB.RSXX_BR_RPD RSXX
				ON LNXX.BF_SSN = RSXX.BF_SSN
	;

	CREATE TABLE _X_RSXX AS
		SELECT DISTINCT
			RSXX.BF_SSN,
			RSXX.LN_RPS_SEQ
		FROM
			LNXX IDR
			JOIN PKUB.LNXX_LON_RPS LNXX
				ON IDR.BF_SSN = LNXX.BF_SSN
				AND IDR.LN_SEQ = LNXX.LN_SEQ
			LEFT JOIN PKUB.RSXX_BR_RPD RSXX
				ON LNXX.BF_SSN = RSXX.BF_SSN
				AND LNXX.LN_RPS_SEQ = RSXX.LN_RPS_SEQ
		WHERE
			LNXX.BF_SSN IS NULL
	;

	CREATE TABLE _X_LNXX AS
		SELECT DISTINCT
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LN_RPS_SEQ
		FROM
			_X_RSXX RSXX
			JOIN PKUB.LNXX_LON_RPS LNXX
				ON RSXX.BF_SSN = LNXX.BF_SSN
	;

QUIT;

/*data dumps*/
%MACRO DATA_DUMPS(DS,TB);
	PROC SQL;
		CREATE TABLE RSXX&DS AS
			SELECT DISTINCT
				DB.*
			FROM
				&TB DS
				JOIN PKUB.RSXX_IBR_RPS DB
					ON DS.BF_SSN = DB.BF_SSN
			ORDER BY
				BF_SSN
		;

		CREATE TABLE RSXX&DS AS
			SELECT DISTINCT
				DB.*
			FROM
				&TB DS
				JOIN PKUB.RSXX_BR_RPD DB
					ON DS.BF_SSN = DB.BF_SSN
			ORDER BY
				BF_SSN
		;

		CREATE TABLE RSXX&DS AS
			SELECT DISTINCT
				DB.*
			FROM
				&TB DS
				JOIN PKUB.RSXX_IBR_IRL_LON DB
					ON DS.BF_SSN = DB.BF_SSN
			ORDER BY
				BF_SSN
		;

		CREATE TABLE LNXX&DS AS
			SELECT DISTINCT
				DB.*
			FROM
				&TB DS
				JOIN PKUB.LNXX_LON_RPS DB
					ON DS.BF_SSN = DB.BF_SSN
			ORDER BY
				BF_SSN
		;

		CREATE TABLE RSXX&DS AS
			SELECT DISTINCT
				DB.*
			FROM
				&TB DS
				JOIN PKUB.RSXX_IBR_EXT_LON  DB
					ON DS.BF_SSN = DB.BF_SSN
			ORDER BY
				BF_SSN
		;

		CREATE TABLE LNXX&DS AS
			SELECT DISTINCT
				DB.*
			FROM
				&TB DS
				JOIN PKUB.LNXX_LON_RPS_SPF DB
					ON DS.BF_SSN = DB.BF_SSN
			ORDER BY
				BF_SSN
		;
	QUIT;
%MEND;

%DATA_DUMPS(_X_,_X_RSXX);
%DATA_DUMPS(_X_,_X_RSXX);

ENDRSUBMIT;

%MACRO TO_FILE(DS);
	DATA &DS; SET LEGEND.&DS; RUN;

	PROC EXPORT
			DATA=&DS
			OUTFILE='T:\SAS\NHCS XXXX QUERY X.XLSX'
			REPLACE;
	RUN;
%MEND;

%TO_FILE(_X_RSXX);
%TO_FILE(_X_RSXX);
%TO_FILE(_X_RSXX);
%TO_FILE(_X_RSXX);
%TO_FILE(_X_LNXX);
%TO_FILE(_X_RSXX);
%TO_FILE(_X_RSXX);
%TO_FILE(_X_LNXX);
%TO_FILE(_X_RSXX);
%TO_FILE(_X_RSXX);

%MACRO DD_FILE(DS);
	PROC EXPORT
			DATA=LEGEND.&DS
			OUTFILE='T:\SAS\NHCS XXXX QUERY X DATA DUMPS.XLSX'
			REPLACE;
	RUN;
%MEND;

%DD_FILE(RSXX_X_);
%DD_FILE(RSXX_X_);
%DD_FILE(RSXX_X_);
%DD_FILE(LNXX_X_);
%DD_FILE(RSXX_X_);
%DD_FILE(LNXX_X_);
%DD_FILE(RSXX_X_);
%DD_FILE(RSXX_X_);
%DD_FILE(RSXX_X_);
%DD_FILE(LNXX_X_);
%DD_FILE(RSXX_X_);
%DD_FILE(LNXX_X_);
