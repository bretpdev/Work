%LET Noble = %STR(REQUIRED="DSN=APPX");
LIBNAME Noble ODBC &Noble ;

DATA _NULL_;
	CALL SYMPUTX('MOYR',PUT(TODAY(),DATEX.));
RUN;

PROC SQL;
	CREATE TABLE INBOUND_CALLS_&MOYR AS
		SELECT
			*
		FROM
			Noble.inboundlog
;
QUIT;

LIBNAME FILES 'Y:\Development\SAS Test Files\CR XXXX\';

DATA FILES.INBOUND_CALLS_&MOYR;
SET INBOUND_CALLS_&MOYR;
RUN;

PROC EXPORT DATA = WORK.INBOUND_CALLS_&MOYR 
            OUTFILE = "Y:\Development\SAS Test Files\CR XXXX\INBOUND_CALLS_&MOYR..xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SHEETX"; 
RUN;
