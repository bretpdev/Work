LIBNAME DUSTER REMOTE SERVER=DUSTER SLIBREF=WORK;
RSUBMIT;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
PROC SQL;
	CREATE TABLE RS05 AS
		SELECT
			*
		FROM
			OLWHRM1.RS05_IBR_RPS
		WHERE
			BF_SSN IN ('520253428','520788537','520708607','585948384','517604828','524086288','520132065','520708606','520721406','334383717','267714810','338848025')
	;

	CREATE TABLE RS10 AS
		SELECT
			*
		FROM
			OLWHRM1.RS10_BR_RPD
		WHERE
			BF_SSN IN ('520253428','520788537','520708607','585948384','517604828','524086288','520132065','520708606','520721406','334383717','267714810','338848025')
	;

	CREATE TABLE RS20 AS
		SELECT
			*
		FROM
			OLWHRM1.RS20_IBR_IRL_LON
		WHERE
			BF_SSN IN ('520253428','520788537','520708607','585948384','517604828','524086288','520132065','520708606','520721406','334383717','267714810','338848025')
	;
QUIT;
ENDRSUBMIT;

PROC EXPORT
		DATA=DUSTER.RS05
		OUTFILE='T:\Data Dump RS05, RS10 and RS20.XLSX'
		REPLACE;
RUN;

PROC EXPORT
		DATA=DUSTER.RS10
		OUTFILE='T:\Data Dump RS05, RS10 and RS20.XLSX'
		REPLACE;
RUN;

PROC EXPORT
		DATA=DUSTER.RS20
		OUTFILE='T:\Data Dump RS05, RS10 and RS20.XLSX'
		REPLACE;
RUN;