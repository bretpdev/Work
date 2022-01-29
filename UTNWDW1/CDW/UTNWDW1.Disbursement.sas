/*TEST*/
/*%LET TESTMSG = - THIS IS A TEST;*/
/*%LET RPTLIB = T:\SAS;*/
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

%INCLUDE "&CODELIB\UTNWDW1 CDW DAILY UPDATE.FOLDER AND DATABASE.SAS";
%FILECHECK(14);

DATA LN15(drop=trunc) ;
	INFILE REPORT14 DSD DLM = ',' FIRSTOBS=1 MISSOVER end = eof;
	INPUT DF_SPE_ACC_ID :$10. LN_BR_DSB_SEQ :6. LA_DSB :9.2 LD_DSB :$10. LC_DSB_TYP :$1.  
		LC_STA_LON15 :$1. LN_SEQ :6. LA_DL_REBATE :9.2 ;
%TRUNCAT(14);
RUN;
%COPYERROR;
PROC SORT DATA=LN15 ; BY DF_SPE_ACC_ID LN_BR_DSB_SEQ; RUN;
DATA LN15; SET LN15; BY DF_SPE_ACC_ID LN_BR_DSB_SEQ; IF LAST.LN_BR_DSB_SEQ; RUN;

%ENCHILADA(LN15,LN15_DISBURSEMENT,DF_SPE_ACC_ID LN_BR_DSB_SEQ,LA_DSB LD_DSB LC_DSB_TYP LC_STA_LON15 
	LN_SEQ LA_DL_REBATE,WHERE LC_STA_LON15 = '1' AND LA_DSB > 0);

%GOODBYE_NULL(LN15_DISBURSEMENT,LA_DL_REBATE,0);

%FINISH(SOURCE=Disbursement);
