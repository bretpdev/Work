/*get parameters passed from calling job*/
DATA _NULL_;
	CALL SYMPUT('TESTMSG',SCAN("&SYSPARM",1,'|'));
	CALL SYMPUT('RPTLIB',SCAN("&SYSPARM",2,'|'));
	CALL SYMPUT('CODELIB',SCAN("&SYSPARM",3,'|'));
	CALL SYMPUT('ODBCFILE',SCAN("&SYSPARM",4,'|'));
	CALL SYMPUT('DB',SCAN("&SYSPARM",5,'|'));
RUN;

%INCLUDE "&CODELIB\UTNWDW1 CDW Daily Update.Folder and Database.sas"  ;

%LET K = 0;
%MACRO LOCALFILECHECK();
	FILENAME REPORT2 "&RPTLIB\UNWS74.NWS74R2.*";
	%IF %SYSFUNC(FEXIST(REPORT2)) = 0 %THEN %DO;
			%put "File UNWS74.NWS74R2 was missing";
			%let k = 2;
	%END;
	%IF &k > 0 %THEN %DO;
		DATA _NULL_;
			FILE ERRMESS subject='CDW Error - Missing File&TESTMSG';
			PUT "The CDW Update Process was Aborted for Report UNWS74.NWS74R2!";
			PUT "The file was missing!";
		RUN;
		ENDSAS;
	%END;
%MEND;
%LOCALFILECHECK();

DATA PH05(drop=trunc) ;
	INFILE REPORT2 DSD DLM = ',' FIRSTOBS=1 MISSOVER end = eof;
		INPUT DF_SPE_ACC_ID :$10. DX_CNC_EML_ADR :$254. DF_DTS_EML_ADR_EFF :DATETIME25.6 DF_LST_USR_EML_ADR :$8. DI_VLD_CNC_EML_ADR :1.
			DI_CNC_ELT_OPI :1. DC_ELT_OPI_SRC :$2. DI_CNC_EBL_OPI :1. DC_EBL_OPI_SRC :$2. DI_CNC_TAX_OPI :1. DC_TAX_OPI_SRC :$2.;
%TRUNCAT(2);
RUN;
%copyerror;

PROC SORT DATA=PH05 ; BY DF_SPE_ACC_ID; RUN;
DATA PH05; SET PH05; BY DF_SPE_ACC_ID; IF LAST.DF_SPE_ACC_ID; RUN;


%ENCHILADA
	(
		PH05,
		PH05_ContactEmail,
		DF_SPE_ACC_ID,
		DX_CNC_EML_ADR DF_DTS_EML_ADR_EFF DF_LST_USR_EML_ADR DI_VLD_CNC_EML_ADR DI_CNC_ELT_OPI DC_ELT_OPI_SRC DI_CNC_EBL_OPI DC_EBL_OPI_SRC DI_CNC_TAX_OPI DC_TAX_OPI_SRC,

	);

%GOODBYE_NULL(PH05_ContactEmail,DC_ELT_OPI_SRC,'');
%GOODBYE_NULL(PH05_ContactEmail,DC_EBL_OPI_SRC,'');
%GOODBYE_NULL(PH05_ContactEmail,DC_TAX_OPI_SRC,'');

x "del ""&RPTLIB\UNWS74.NWS74R2.*""" ;

%FINISH;

DATA _NULL_;
	FILE ERRMESS SUBJECT="CDW Ecorr Update Complete&TESTMSG";
	PUT "The CDW Ecorr Update Process is Complete";
RUN;