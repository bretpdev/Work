LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;

PROC SQL;
	CREATE TABLE LPXX AS
		SELECT
			*
		FROM
			PKUB.LPXX_DD_ACT_STA
		WHERE
			IC_LON_PGM = 'DLSTFD'
			AND PF_RGL_CAT = 'XXXXXXX'
			AND IF_GTR = 'XXXXXX'
			AND IF_OWN = 'XXXXXXXX'
			AND PC_DD_STA = 'D'
			AND PC_DD_SCH_TYP = 'I'			
	;

	CREATE TABLE LPXX AS
		SELECT
			*
		FROM
			PKUB.LPXX_DD_ACT_WDO
		WHERE
			IC_LON_PGM = 'DLSTFD'
			AND PF_RGL_CAT = 'XXXXXXX'
			AND IF_GTR = 'XXXXXX'
			AND IF_OWN = 'XXXXXXXX'
			AND PC_DD_STA = 'D'
			AND PC_DD_SCH_TYP = 'I'			
	;

	CREATE TABLE LPXX AS
		SELECT
			*
		FROM
			PKUB.LPXX_DD_ACT_STE
		WHERE
			IC_LON_PGM = 'DLSTFD'
			AND PF_RGL_CAT = 'XXXXXXX'
			AND IF_GTR = 'XXXXXX'
			AND IF_OWN = 'XXXXXXXX'
			AND PC_DD_STA = 'D'
			AND PC_DD_SCH_TYP = 'I'			
	;
QUIT;

ENDRSUBMIT;

PROC EXPORT
		DATA=LEGEND.LPXX
		OUTFILE='T:\SAS\NH XXXX.XLSX'
		REPLACE;
RUN;

PROC EXPORT
		DATA=LEGEND.LPXX
		OUTFILE='T:\SAS\NH XXXX.XLSX'
		REPLACE;
RUN;

PROC EXPORT
		DATA=LEGEND.LPXX
		OUTFILE='T:\SAS\NH XXXX.XLSX'
		REPLACE;
RUN;