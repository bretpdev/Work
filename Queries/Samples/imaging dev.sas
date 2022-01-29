/*Specify the folder where the report file(s) are:*/
%LET RPTLIB = X:\PADD\FTP;
/*%LET RPTLIBX = T:\sas;*/

/*Specify a back-up location for the report files here:*/
%LET RPTLIBX = X:\archive\sas;

/*Specify the database dsn file here:*/
%LET IMG = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\ImgUheaaDevSql.dsn; update_lock_typ=nolock; bl_keepnulls=no");
/*LIBNAME &IMG ;*/


PROC SQL inobs = 1000;
CONNECT TO ODBC AS IMG (&IMG);
CREATE TABLE test AS
SELECT *
FROM CONNECTION TO IMG (
SELECT *
FROM DOCSTORAGE
);
DISCONNECT FROM IMG;

;
QUIT;
