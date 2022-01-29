/***************************************************************************************
CREATE A MONTLY SNAP SHOT OF THE DW01 AT THE BEGGINING OF EACH MONTH. THIS MUST BE RUN
ON THE FIRST BUSINESS DAY OF THE MONTH
****************************************************************************************/
LIBNAME dLib 'Y:\Development\ExternalDataSources\MonthlyDataSets';
LIBNAME TLib 'T:\SAS\DataSources';
%INCLUDE "Y:\Codebase\SAS\Common Macros.sas";
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
DATA _NULL_;
	CALL SYMPUTX('MOYR',PUT(TODAY(),MMYYn6.));
RUN;

DATA _NULL_;
	CALL SYMPUTX('LMOYR',PUT(intnx('Month',TODAY(),-1,'s'),MONYY7.));
RUN;

%LET CDW = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\CDW.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME CDW ODBC &CDW ;

%LET AUDITCDW = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\AUDITCDW.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME AUDITCDW ODBC &AUDITCDW ;


PROC SQL;
	CREATE TABLE dLib.PKUB_DW01_DW_CLC_CLU_&MOYR AS
	SELECT
		*
	FROM
		CDW.DW01_DW_CLC_CLU
	;
	QUIT;
PROC SQL;
	CREATE TABLE dLib.PKUB_LN10_LON_&MOYR AS
	SELECT
		*
	FROM
		CDW.LN10_LON
	;
	QUIT;

PROC SQL;
	CREATE TABLE dLib.PKUs_LN10_LON_&MOYR AS
	SELECT
		*
	FROM
		AUDITCDW.LN10_LON_&LMOYR
	;
	QUIT;


%REMOTE_DATA_SAVE(PKUS,LN16_LON_DLQ_HST);

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
DATA LEGEND.DW01_DW_CLC_CLU; *Send data to Duster;
SET dLib.PKUB_DW01_DW_CLC_CLU_&MOYR;
RUN;


/***************************************************************************************
PUT THE DATASET OUT ON LEGEND SO IT CAN BE USED IN PRODUCTION REPORTING
****************************************************************************************/
RSUBMIT;
LIBNAME lgnd '/sas/whse/progrevw';


DATA lgnd.DW01_PREV_MONTH;
	SET DW01_DW_CLC_CLU;
RUN;
ENDRSUBMIT;
