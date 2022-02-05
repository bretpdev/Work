/***************************************************************************************
CREATE A MONTLY SNAP SHOT OF THE DW01 AT THE BEGGINING OF EACH MONTH. THIS MUST BE RUN
ON THE FIRST BUSINESS DAY OF THE MONTH
****************************************************************************************/
LIBNAME dLib 'Y:\Development\ExternalDataSources\MonthlyDataSets\';
%INCLUDE "Y:\Codebase\SAS\Common Macros.sas";
DATA _NULL_;
	CALL SYMPUTX('MOYR',PUT(TODAY(),MMYYn6.));
RUN;


RSUBMIT;
%let DB = DNFPUTDL;  *This is live;
LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;

PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);
	CREATE TABLE LN10_LON AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT
						*
					FROM
						PKUB.LN10_LON 

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;
QUIT;

ENDRSUBMIT;

/*UNCOMMENT FOR FILEZILLA RUN*/
/*LIBNAME LEGEND 'T:\SAS\'*/

DATA dLib.PKUB_LN10_LON_&MOYR;
SET LEGEND.LN10_LON;
RUN;
