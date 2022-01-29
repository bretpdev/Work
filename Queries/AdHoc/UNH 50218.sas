
DATA REMOTE_DATA;
	INFILE 'T:\ULWS74.LWS74R2.*' DSD DLM = ','  MISSOVER DSD lrecl=32767 ;
	INPUT DF_SPE_ACC_ID :$10. ;
RUN;

DATA REMOTE_DATA;
SET REMOTE_DATA;
IF DF_SPE_ACC_ID = 'AccountNum' THEN DO; /*DELETE THE HEADER*/
	DELETE;
END;
RUN;

%LET ARCTYPEID = 2;		*Atd22ByBalance - Add arc for all loans with a balance;
%LET ARC = 'BANAW';
%LET COMMENT = 'BANA Welcome letter sent to borrower on ';
%LET SASID = 'BANADCR';

/*set up library to SQL Server and include common code*/
/*TEST*/
/*LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\ULS_TEST.dsn; update_lock_typ=nolock; bl_keepnulls=no";*/
/*%INCLUDE "X:\PADU\Test Sessions\Local SAS Schedule\ArcAdd Common.SAS";*/
/*%LET DSN = 'FILEDSN=X:\PADR\ODBC\ULS_TEST.dsn;';*/
/*LIVE*/
LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\ULS.dsn; update_lock_typ=nolock; bl_keepnulls=no";
%INCLUDE "X:\Sessions\Local SAS Schedule\ArcAdd Common.SAS";
%LET DSN = 'FILEDSN=X:\PADR\ODBC\ULS.dsn;';

/*call macro or put data step here to add job specific data to LEGEND data*/
%CREATE_GENERIC_ARCADD_DATA;
/*end job specific code*/

/*call ARC add common processing*/
%ARC_ADD_PROCESSING;

