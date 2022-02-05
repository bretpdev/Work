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

%REMOTE_DATA_SAVE(PKUB,DW01_DW_CLC_CLU);
%REMOTE_DATA_SAVE(PKUS,LN10_LON);
%REMOTE_DATA_SAVE(PKUS,LN16_LON_DLQ_HST);
%REMOTE_DATA_SAVE(PKUB,LN10_LON );
/***************************************************************************************
PUT THE DATASET OUT ON LEGEND SO IT CAN BE USED IN PRODUCTION REPORTING
****************************************************************************************/
RSUBMIT;
LIBNAME lgnd '/sas/whse/progrevw';
DATA lgnd.DW01_PREV_MONTH;
	SET DW01_DW_CLC_CLU;
RUN;
ENDRSUBMIT;
