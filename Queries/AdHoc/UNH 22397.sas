LIBNAME DUSTER REMOTE SERVER=DUSTER SLIBREF=WORK;
RSUBMIT;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;

PROC SQL;
	CREATE TABLE ACHCNTSA AS
		SELECT DISTINCT
			BF_SSN
		FROM
			OLWHRM1.LN83_EFT_TO_LON LN83
		WHERE
			LN83.LC_STA_LN83 = 'A'
	;
QUIT;

PROC SQL;
	CREATE TABLE ACHCNTSA AS
		SELECT DISTINCT
			BF_SSN
		FROM
			OLWHRM1.LN83_EFT_TO_LON LN83
		WHERE
			LN83.LC_STA_LN83 = 'A'
			AND TODAY() BETWEEN LD_EFT_EFF_BEG AND LD_EFT_EFF_END
	;
QUIT;

ENDRSUBMIT;
