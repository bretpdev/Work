LIBNAME DUSTER REMOTE SERVER=DUSTER SLIBREF=WORK;
RSUBMIT;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
PROC SQL;
	CREATE TABLE FEDOFF AS
		SELECT
			AF_APL_ID,
			AF_APL_ID_SFX,
			BF_SSN,
			LD_TRX_EFF,
			LA_TRX,
			LC_TRX_TYP
		FROM
			OLWHRM1.DC11_LON_FAT DC11
		WHERE
			LC_TRX_TYP = 'FO'
			AND (LA_TRX BETWEEN 6000 AND 6500 OR LD_TRX_EFF BETWEEN '01DEC2013'D AND '31DEC2013'D)
	;
QUIT;
ENDRSUBMIT;
DATA FEDOFF; SET DUSTER.FEDOFF; RUN;

PROC EXPORT
		DATA=FEDOFF
		OUTFILE='T:\SAS\Federal Offset Payment.XLSX'
		REPLACE;
RUN;
