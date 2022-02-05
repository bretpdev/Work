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
LIBNAME PKUS DB2 DATABASE=&DB OWNER=PKUS;

PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);
	CREATE TABLE LN16_LON_DLQ_HST AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT
						*
					FROM
						PKUS.LN16_LON_DLQ_HST 

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;
QUIT;

ENDRSUBMIT;

/*UNCOMMENT FOR FILEZILLA RUN*/
/*LIBNAME LEGEND 'T:\SAS\'*/

DATA dLib.PKUS_LN16_LON_DLQ_HST_&MOYR;
SET LEGEND.LN16_LON_DLQ_HST;
RUN;
