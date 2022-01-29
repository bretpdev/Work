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
%FILECHECK(27);


DATA K0ADD(drop=trunc);
	INFILE REPORT27 DSD DLM = ',' FIRSTOBS=1 MISSOVER end = eof;
	INPUT DF_SPE_ACC_ID :$10. LN_ATY_SEQ :6. LC_STA_ACTY10 :$1. DX_STR_ADR_1 :$30. DX_STR_ADR_2 :$30. DM_CT :$20. DC_DOM_ST :$2. 
	DF_ZIP_CDE :$17. DM_FGN_CNY :$25. COMMENTS :$300.;
%TRUNCAT(27);
RUN;
%COPYERROR;
PROC SORT DATA=K0ADD ; BY DF_SPE_ACC_ID LN_ATY_SEQ; RUN;
DATA K0ADD; SET K0ADD; BY DF_SPE_ACC_ID LN_ATY_SEQ; IF LAST.LN_ATY_SEQ; RUN;

%ENCHILADA(K0ADD,AY10_K0ADD,DF_SPE_ACC_ID LN_ATY_SEQ,LC_STA_ACTY10 DX_STR_ADR_1 DX_STR_ADR_2 DM_CT 
	DC_DOM_ST DF_ZIP_CDE DM_FGN_CNY COMMENTS,WHERE LC_STA_ACTY10 = 'A');

%GOODBYE_NULL(AY10_K0ADD,LC_STA_ACTY10,'');
%GOODBYE_NULL(AY10_K0ADD,DX_STR_ADR_1,'');
%GOODBYE_NULL(AY10_K0ADD,DX_STR_ADR_2,'');
%GOODBYE_NULL(AY10_K0ADD,DM_CT,'');
%GOODBYE_NULL(AY10_K0ADD,DC_DOM_ST,'');
%GOODBYE_NULL(AY10_K0ADD,DF_ZIP_CDE,'');
%GOODBYE_NULL(AY10_K0ADD,DM_FGN_CNY,'');
%GOODBYE_NULL(AY10_K0ADD,COMMENTS,'');

%FINISH(SOURCE=K0ADD);