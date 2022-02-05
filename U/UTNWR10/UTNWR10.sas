/*CALL SQL SPROC InsertCreditBalance TO INSERT INTO ServicerInventoryMetrics*/ 
/*LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\ServicerInventoryMetrics_TEST.dsn; bl_keepnulls=no; READ_ISOLATION_LEVEL=RU" SCHEMA= dbo;*/
LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\ServicerInventoryMetrics.dsn; bl_keepnulls=no; READ_ISOLATION_LEVEL=RU" SCHEMA= dbo;

PROC SQL;
/*	CONNECT TO ODBC (REQUIRED="FILEDSN=X:\PADR\ODBC\ServicerInventoryMetrics_TEST.dsn; bl_keepnulls=no; READ_ISOLATION_LEVEL=RU");*/
	CONNECT TO ODBC (REQUIRED="FILEDSN=X:\PADR\ODBC\ServicerInventoryMetrics.dsn; bl_keepnulls=no; READ_ISOLATION_LEVEL=RU");
	EXECUTE(InsertCreditBalance) BY ODBC;
	DISCONNECT FROM ODBC;
QUIT;


