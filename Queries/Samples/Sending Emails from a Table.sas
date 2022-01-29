%LET MD = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\OPSDEV.dsn;");
/*%LET MD = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\MD.dsn;");*/
LIBNAME DUDE ODBC USER="sqlserver;" PASSWORD="procauto;" &MD ;

PROC SQL NOPRINT;
SELECT EMAIL INTO :TARGET seperated by '" "'
FROM DUDE.ERROR_MESSAGE
WHERE TYPE = 'TO';
SELECT EMAIL INTO :CC seperated by '" "'
FROM DUDE.ERROR_MESSAGE
WHERE TYPE = 'CC';
QUIT;
%let target = &target;
%let cc = &cc;
%PUT &TARGET &CC;

FILENAME ERRMESS EMAIL to=("&CC");
/*FILENAME ERRMESS EMAIL "&target" cc="&cc";*/

DATA _NULL_;
FILE ERRMESS;
/*	put '!em_to! ("sshelp@utahsbr.edu")';*/
/*	put '!em_cc! ("nowens@utahsbr.edu")';*/
	PUT "This email is being sent from a live SAS session";
/*ABORT ABEND;*/
RUN;
