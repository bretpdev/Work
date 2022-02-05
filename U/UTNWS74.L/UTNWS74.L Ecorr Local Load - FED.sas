/*TEST*/
/*%LET TESTMSG = - THIS IS A TEST;*/
/*%LET RPTLIB = Y:\Batch\FTP;*/
/*%LET LOGLIB = Y:\Batch\Logs;*/
/*%LET CODELIB = Y:\Codebase\SAS\CDW;*/
/*%LET ODBCFILE = BSYS_TEST;*/
/*%LET ODBCFILE_CSYS = CSYSTest;*/
/*%LET DB = CDW_test;*/

/*LIVE*/
%LET TESTMSG = THIS IS PROD;
%LET RPTLIB = Z:\Batch\FTP;
%LET LOGLIB = Z:\Batch\Logs;
%LET CODELIB = Z:\Codebase\SAS\CDW;
%LET ODBCFILE = BSYS;
%LET ODBCFILE_CSYS = CSYS;
%LET DB = CDW;

FILENAME REPORT2 "&RPTLIB\UNWS74.NWS74R2.*";

/*	Gather the email addresses for error reports*/
PROC SQL NOPRINT;
CONNECT TO ODBC AS BSYS (REQUIRED="FILEDSN=X:\PADR\ODBC\&ODBCFILE..dsn;");
CREATE TABLE test AS
SELECT *
FROM CONNECTION TO BSYS (
SELECT a.WINUNAME 
	,A.TYPEKEY as type
FROM GENR_REF_MISCEMAILNOTIF a
where A.TYPEKEY in ('SAS Error')
order by WINUNAME
);
DISCONNECT FROM BSYS;

SELECT TRIM(WINUNAME) || '@UTAHSBR.EDU' INTO :err SEPARATED BY '" "'
FROM test
WHERE TYPE = 'SAS Error';
QUIT;


PROC SQL NOPRINT;
	CONNECT TO ODBC AS CSYS (REQUIRED="FILEDSN=X:\PADR\ODBC\&ODBCFILE_CSYS..dsn;");
	CREATE TABLE SAS_Path AS
		SELECT 
			*
		FROM 
			CONNECTION TO CSYS 
				(
					SELECT
						path
					FROM
						GENR_DAT_EnterpriseFileSystem
					WHERE
						[KEY] = 'SAS'
				)
	;
	DISCONNECT FROM CSYS;

	SELECT 
		path  INTO :SASEXE 
	FROM
		SAS_Path

	;
QUIT;


%put &err;
%PUT &SASEXE;
FILENAME ERRMESS EMAIL to=("&err") ;

data _null_;
	call symput('start',put(time(),time9.));
	call symput('day_of_year',catt('',"#",(today() - intnx('year',today(),-1,'e')),"*"));
run;


%let k = 0;
%MACRO FILECHECK();
	%IF %SYSFUNC(FEXIST(REPORT2)) = 0 %THEN %DO;
			%put "File UNWS74.NWS74R2 was missing";
			%let k = 2;
	%END;
	%IF &k > 0 %THEN %DO;
		DATA _NULL_;
			FILE ERRMESS subject="CDW Ecorr Error - Missing File&TESTMSG";
			PUT "The CDW Ecorr Update Process was Aborted for Report UNWS74.NWS74R2!";
			PUT "The file was missing!";
		RUN;
		ENDSAS;
	%END;
	%ELSE %DO;
		DATA _NULL_;
			FILE ERRMESS subject="CDW Ecorr Update Commencing&TESTMSG";
			PUT "The CDW Ecorr Update Process Began Without Errors!";
			PUT "If Errors are found within individual files, a subsequent email will be sent.";
			PUT "START =         &START ";
		RUN;
	%END;
%MEND;
%FILECHECK();

options noxwait noxsync;

x """&SASEXE"" -icon 
	-CONFIG ""&CODELIB\win7config.cfg"" 
	-sysin ""&CODELIB\UTNWS74.Ecorr.sas"" 
	-LOG ""&LOGLIB\CDW.Ecorr.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB""";
