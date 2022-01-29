LIBNAME dLib 'Y:\Development\ExternalDataSources\MonthlyDataSets';
%INCLUDE "Y:\Codebase\SAS\Common Macros.sas";
DATA _NULL_;
	CALL SYMPUTX('MOYR',PUT(TODAY(),MMYYn6.));
RUN;

/*CREATE A LOCAL COPY*/
%LET CDW = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\CDW.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME CDW ODBC &CDW ;

PROC SQL;
CREATE TABLE dLib.LOCAL_PKUS_LN16_LON_DLQ_HST_&MOYR AS 
	SELECT 
		*
	FROM
		CDW.LN16_LON_DLQ_HST
;
QUIT;
