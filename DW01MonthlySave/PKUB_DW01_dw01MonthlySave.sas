/***************************************************************************************
CREATE A MONTLY SNAP SHOT OF THE DW01 AT THE BEGGINING OF EACH MONTH. THIS MUST BE RUN
ON THE FIRST BUSINESS DAY OF THE MONTH
****************************************************************************************/
LIBNAME dLib 'Y:\Development\ExternalDataSources\MonthlyDataSets';
%INCLUDE "Y:\Codebase\SAS\Common Macros.sas";
DATA _NULL_;
	CALL SYMPUTX('MOYR',PUT(TODAY(),MMYYn6.));
RUN;


RSUBMIT;
%let DB = DNFPUTDL;  *This is live;
LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;

PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);
	CREATE TABLE DW01_DW_CLC_CLU AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT
						*
					FROM
						PKUB.DW01_DW_CLC_CLU 

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;
QUIT;

/*/****************************************************************************************/
/*PUT THE DATASET OUT ON LEGEND SO IT CAN BE USED IN PRODUCTION REPORTING*/
/*****************************************************************************************/*/;
LIBNAME lgnd '/sas/whse/progrevw';
DATA lgnd.DW01_PREV_MONTH;
	SET DW01_DW_CLC_CLU;
RUN;

ENDRSUBMIT;


/*UNCOMMENT FOR FILEZILLA RUN*/
/*LIBNAME LEGEND 'T:\SAS\'*/

DATA dLib.PKUB_DW01_DW_CLC_CLU_&MOYR;
SET LEGEND.DW01_DW_CLC_CLU;
RUN;
