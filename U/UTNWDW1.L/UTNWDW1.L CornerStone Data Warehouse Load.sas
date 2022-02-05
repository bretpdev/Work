%LET MAXRNO = 42;

/*TEST*/
/*%LET TESTMSG = - THIS IS A TEST;*/
/*%LET RPTLIB = Y:\Batch\FTP;*/
/*%LET LOGLIB = Y:\Batch\Logs;*/
/*%LET CODELIB = Y:\Codebase\SAS\CDW;*/
/*%LET ODBCFILE = BSYS_TEST;*/
/*%LET DB = CDW_test;*/
/*%LET ODBCFILE_CSYS = CSYSTest;*/

/*LIVE*/
%LET TESTMSG = - THIS IS LIVE;
%LET RPTLIB = Z:\Batch\FTP;
%LET LOGLIB = Z:\Batch\Logs;
%LET CODELIB = Z:\Codebase\SAS\CDW;
%LET ODBCFILE = BSYS;
%LET DB = CDW;
%LET ODBCFILE_CSYS = CSYS;


/*	Gather the email addresses for error reports*/
PROC SQL NOPRINT;
	CONNECT TO ODBC AS BSYS (REQUIRED="FILEDSN=X:\PADR\ODBC\&ODBCFILE..dsn;");
	CREATE TABLE TEST AS
		SELECT 
			*
		FROM 
			CONNECTION TO BSYS 
				(
					SELECT
						A.WINUNAME 
						,A.TYPEKEY AS TYPE
					FROM
						GENR_REF_MISCEMAILNOTIF A
					WHERE
						A.TYPEKEY IN ('SAS Error')
					ORDER BY 
						WINUNAME
				)
	;
	DISCONNECT FROM BSYS;

	SELECT 
		TRIM(WINUNAME) || '@UTAHSBR.EDU' INTO :ERR SEPARATED BY '" "'
	FROM
		TEST
	WHERE
		TYPE = 'SAS Error'
	;
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
/*UNCOMMENT FOR LIVE*/
/*	SELECT */
/*		path  INTO :SASEXE */
/*	FROM*/
/*		SAS_Path*/
/*	;*/

	select
		'C:\Program Files\SASHome2\x86\SASFoundation\9.4\sas.exe' INTO :SASEXE
	FROM
		SAS_Path

	;
QUIT;

%PUT &SASEXE;

%PUT &ERR;

FILENAME ERRMESS EMAIL TO=("&ERR") ;

DATA _NULL_;
	CALL SYMPUT('START',PUT(TIME(),TIME9.));
	CALL SYMPUT('DAY_OF_YEAR',CATT('',"#",(TODAY() - INTNX('YEAR',TODAY(),-1,'E')),"*"));
RUN;

%LET R=0;
%MACRO FILECHECK;
%DO I = 2 %TO &MAXRNO;
	FILENAME REPORT&I "&RPTLIB/UNWDW1.NWDW1R&I..*";
	%IF %SYSFUNC(FEXIST(REPORT&I)) = 0 %THEN
		%DO;
			%PUT "File &I was missing";
			%LET R = %EVAL(&R+1);
			%IF &R = 1 %THEN %LET K = &I;
			%ELSE %LET K = &K.,&I;
		%END;
%END;
	%IF &R > 0 %THEN %DO;
		DATA _NULL_;
		FILE ERRMESS SUBJECT="CDW Error - Missing Files&TESTMSG";
			PUT "The CDW Update Process was Aborted!";
			PUT "&R file(s) were missing:";
			PUT "The missing file(s) are: &K";
		RUN;
		ENDSAS;
	%END;
	%ELSE %DO;
		DATA _NULL_;
		FILE ERRMESS subject="CDW Update Commencing&TESTMSG";
			PUT "The Cornerstone Data Warehouse Update Process Began Without Errors!";
			PUT "If Errors are found within individual files, a subsequent email will be sent.";
			PUT "START =         &START ";
		RUN;
	%END;
%MEND;
%FILECHECK;

OPTIONS NOXWAIT XSYNC;

X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.Endorser.sas"" 
	-LOG ""&LOGLIB\CDW.Endorser.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *2;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.Reference.sas"" 
	-LOG ""&LOGLIB\CDW.Reference.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *3;
X """&SASEXE"" -icon  
	-SYSIN ""&CODELIB\UTNWDW1.Bill.sas"" 
	-LOG ""&LOGLIB\CDW.Bill.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *4;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.Financialtran.sas"" 
	-LOG ""&LOGLIB\CDW.Financialtran.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *5;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.Deferment.sas"" 
	-LOG ""&LOGLIB\CDW.Deferment.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *6;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.Forbearance.sas"" 
	-LOG ""&LOGLIB\CDW.Forbearance.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *7;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.Borrower.sas"" 
	-LOG ""&LOGLIB\CDW.Borrower.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *8;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.DL200_Letters.sas"" 
	-LOG ""&LOGLIB\CDW.DL200_Letters.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *9;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.Waived_Fees.sas"" 
	-LOG ""&LOGLIB\CDW.Waived_Fees.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *10;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.Loan.sas""  
	-LOG ""&LOGLIB\CDW.Loan.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *11;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.EFT.sas"" 
	-LOG ""&LOGLIB\CDW.EFT.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *12;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.Rehabilitation.sas"" 
	-LOG ""&LOGLIB\CDW.Rehabilitation.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *13;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.Disbursement.sas"" 
	-LOG ""&LOGLIB\CDW.Disbursement.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *14;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.Borrower_Benefit.sas"" 
	-LOG ""&LOGLIB\CDW.Borrower_Benefit.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *15;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.Arc_Indicators.sas"" 
	-LOG ""&LOGLIB\CDW.Arc_Indicators.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *16;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.RPS.sas"" 
	-LOG ""&LOGLIB\CDW.RPS.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *17;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.Int_Rate.sas"" 
	-LOG ""&LOGLIB\CDW.Int_Rate.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *18;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.School.sas"" 
	-LOG ""&LOGLIB\CDW.School.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *19;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.Delinquency.sas"" 
	-LOG ""&LOGLIB\CDW.Delinquency.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *20;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.K0PHN.sas"" 
	-LOG ""&LOGLIB\CDW.K0PHN.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *21;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.Autopay.sas"" 
	-LOG ""&LOGLIB\CDW.Autopay.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *22;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.Email.sas"" 
	-LOG ""&LOGLIB\CDW.Email.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *23;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.Suspense.sas"" 
	-LOG ""&LOGLIB\CDW.Suspense.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *24;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.Address.sas"" 
	-LOG ""&LOGLIB\CDW.Address.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *25;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.Phone.sas"" 
	-LOG ""&LOGLIB\CDW.Phone.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *26;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.K0ADD.sas"" 
	-LOG ""&LOGLIB\CDW.K0ADD.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *27;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.Activity_History.sas"" 
	-LOG ""&LOGLIB\CDW.Activity_History.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *28;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.Loan2.sas"" 
	-LOG ""&LOGLIB\CDW.Loan2.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *29;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.Credit_Reporting.sas"" 
	-LOG ""&LOGLIB\CDW.Credit_Reporting.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *30;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.Amount_Due.sas"" 
	-LOG ""&LOGLIB\CDW.Amount_Due.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *31;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.M1411.sas"" 
	-LOG ""&LOGLIB\CDW.M1411.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *32;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.TransactionHistory.sas"" 
	-LOG ""&LOGLIB\CDW.TransactionHistory.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *33;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.LT20LetterRequests.sas"" 
	-LOG ""&LOGLIB\CDW.LT20LetterRequests.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *34;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.BenefitProgramTiers.sas"" 
	-LOG ""&LOGLIB\CDW.BenefitProgramTiers.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *35;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.BenefitProgram.sas"" 
	-LOG ""&LOGLIB\CDW.BenefitProgram.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *36;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.FinancialActivityAdjustment.sas"" 
	-LOG ""&LOGLIB\CDW.FinancialActivityAdjustment.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *37;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.FS12NegAm.sas"" 
	-LOG ""&LOGLIB\CDW.FS12NegAm.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *38;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.DF10DefermentLetter.sas"" 
	-LOG ""&LOGLIB\CDW.DF10DefermentLetter.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *39;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.FB10ForbearanceLetter.sas"" 
	-LOG ""&LOGLIB\CDW.FB10ForbearanceLetter.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *40;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.RS10RepaymentSummaryLetter.sas"" 
	-LOG ""&LOGLIB\CDW.RS10RepaymentSummaryLetter.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *41;
X """&SASEXE"" -icon 
	-SYSIN ""&CODELIB\UTNWDW1.RS05IDRRepayment.sas"" 
	-LOG ""&LOGLIB\CDW.UTNWDW1.RS05IDRRepayment.LOG"" 
	-SYSPARM ""&TESTMSG|&RPTLIB|&CODELIB|&ODBCFILE|&DB"""; *42;
