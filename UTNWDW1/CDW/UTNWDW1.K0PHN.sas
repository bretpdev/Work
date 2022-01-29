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

%INCLUDE "&CODELIB\UTNWDW1 CDW DAILY UPDATE.FOLDER AND DATABASE.SAS";
%FILECHECK(21);


DATA K0PHN(drop=trunc);
	INFILE REPORT21 DSD DLM = ',' FIRSTOBS=1 MISSOVER end = eof;
	INPUT DF_SPE_ACC_ID :$10. LN_ATY_SEQ :6. LC_STA_ACTY10 :$1. PHN1 :$20. PHN2 :$20. PHN3 :$20. COMMENTS :$300.;
%TRUNCAT(21);
RUN;
%COPYERROR;
PROC SORT DATA=K0PHN ; BY DF_SPE_ACC_ID LN_ATY_SEQ; RUN;
DATA K0PHN; SET K0PHN; BY DF_SPE_ACC_ID LN_ATY_SEQ; IF LAST.LN_ATY_SEQ; RUN;

%ENCHILADA(K0PHN,AY10_K0PHN,DF_SPE_ACC_ID LN_ATY_SEQ,LC_STA_ACTY10 PHN1 PHN2 PHN3 COMMENTS,WHERE LC_STA_ACTY10 = 'A');

%GOODBYE_NULL(AY10_K0PHN,LC_STA_ACTY10,'');
%GOODBYE_NULL(AY10_K0PHN,PHN1,'');
%GOODBYE_NULL(AY10_K0PHN,PHN2,'');
%GOODBYE_NULL(AY10_K0PHN,PHN3,'');
%GOODBYE_NULL(AY10_K0PHN,COMMENTS,'');

%FINISH(SOURCE=K0PHN);
