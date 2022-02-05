LIBNAME dLib 'Y:\Development\ExternalDataSources\MonthlyDataSets';
%INCLUDE "Y:\Codebase\SAS\Common Macros.sas";

DATA _NULL_;
	CALL SYMPUTX('MOYR',PUT(TODAY(),MMYYn6.));
RUN;

/*CREATE A LOCAL COPY*/
%LET CDW = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\CDW.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME CDW ODBC &CDW ;

PROC SQL;
CREATE TABLE dLib.LOCAL_PKUB_DW01_DW_CLC_CLU_&MOYR AS 
	SELECT 
		*
	FROM
		CDW.DW01_DW_CLC_CLU
;
QUIT;
