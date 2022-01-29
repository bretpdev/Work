/*TEST*/
/*%LET TESTMSG = - THIS IS A TEST;
%LET RPTLIB = T:\SAS;
%LET LOGLIB = Y:\Batch\Logs;
%LET CODELIB = Y:\Codebase\SAS\CDW;
%LET ODBCFILE = BSYS_TEST;
%LET DB = CDW_test;
%LET ODBCFILE_CSYS = CSYSTest;*/

/*LIVE*/
%LET TESTMSG = - THIS IS LIVE;
%LET RPTLIB = Z:\Batch\FTP;
%LET LOGLIB = Z:\Batch\Logs;
%LET CODELIB = Z:\Codebase\SAS\CDW;
%LET ODBCFILE = BSYS;
%LET DB = CDW;
%LET ODBCFILE_CSYS = CSYS;

%INCLUDE "&CODELIB\UTNWDW1 CDW DAILY UPDATE.FOLDER AND DATABASE.SAS"  ;
%FILECHECK(52);


DATA LN99(drop=trunc);
	INFILE REPORT52 DSD DLM = ',' FIRSTOBS=1 MISSOVER end = eof LRECL = 32767;
	INPUT 
		BF_SSN :$9.
		LN_SEQ :2.
		LN_FAT_SEQ :2.
		IF_LON_SLE :$7.
		LF_LST_DTS_LN99 :DATETIME25.
		IF_SLL_OWN_SLD :$8.
		IF_BUY_OWN_SLD :$8.
		LA_STD_STD_ISL_DCV :12.2
	;
	%TRUNCAT1(52, BF_SSN);
RUN;

%COPYERROR;

%ENCHILADA
	(
		LN99,
		LN99_LON_SLE_FAT,
		BF_SSN  
		LN_SEQ  
		LN_FAT_SEQ  
		IF_LON_SLE,
		LF_LST_DTS_LN99  
		IF_SLL_OWN_SLD  
		IF_BUY_OWN_SLD  
		LA_STD_STD_ISL_DCV  
/*		no filter*/
	);
	
%FINISH(SOURCE=LN99_LON_SLE_FAT);
