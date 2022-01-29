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
%FILECHECK(57);


DATA LN55A(drop=trunc);
	INFILE REPORT57 DSD DLM = ',' FIRSTOBS=1 MISSOVER end = eof;
	INPUT 
		BF_SSN :$9.
		LN_SEQ :2.
		LN_LON_BBS_SEQ :2.
		LN_LON_BBT_SEQ :2.
		LF_LON_BBS_TIR :$2.
		LF_LON_BBS_SUB_TIR :$2.
		PM_BBS_PGM :$3.
		PN_BBS_PGM_SEQ :2.
		PF_BBS_PGM_TIR :$2.
		PN_BBS_PGM_TIR_SEQ :2.
		LN_BR_DSB_SEQ :2.
		LC_STA_LN55 :$1.
		LD_STA_LN55 :DATE9.
		LD_LON_BBT_CHK_ISS :DATE9.
		LD_BBT_DSQ_OVR_END :DATE9.
		LN_LON_BBT_PAY_OVR :3.0
		LD_LON_BBT_BEG :DATE9.
		LD_REB_MTD_LTR_SNT :DATE9.
		LC_LON_BBT_REB_MTD :$1.
		LN_BBT_PAY_PIF_MOT :3.0
		LN_BBT_PAY_DLQ_MOT :3.0
		LC_LON_BBT_DSQ_REA :$2.
		LC_LON_BBT_STA :$1.
		LN_LON_BBT_PAY :3.0
		LD_LON_BBT_STA :DATE9.
		LD_BBT_STS_PAY :DATE9.
		LF_LST_USR_LN55 :$8.
		LF_LST_DTS_LN55 :DATETIME25.
		LD_LON_BBT_ELG_FNL :DATE9.
		LD_BBT_DLQ_MOT_STS :DATE9.
		LD_BBT_PIF_MOT_STS :DATE9.
		LN_BBT_DLQ_MOT_OVR :3.0
		LN_BBT_PIF_MOT_OVR :3.0;
%TRUNCAT1(57, BF_SSN);
RUN;
%COPYERROR;

DATA LN55A;
SET LN55A;
	LD_STA_LN55 = LD_STA_LN55 * 86400;
	LD_LON_BBT_CHK_ISS = LD_LON_BBT_CHK_ISS * 86400;
	LD_BBT_DSQ_OVR_END = LD_BBT_DSQ_OVR_END * 86400;
	LD_LON_BBT_BEG = LD_LON_BBT_BEG * 86400;
	LD_REB_MTD_LTR_SNT = LD_REB_MTD_LTR_SNT * 86400;
	LD_LON_BBT_STA = LD_LON_BBT_STA * 86400;
	LD_BBT_STS_PAY = LD_BBT_STS_PAY * 86400;
	LD_LON_BBT_ELG_FNL = LD_LON_BBT_ELG_FNL * 86400;
	LD_BBT_DLQ_MOT_STS = LD_BBT_DLQ_MOT_STS * 86400;
	LD_BBT_PIF_MOT_STS = LD_BBT_PIF_MOT_STS * 86400;
RUN;

PROC SORT DATA=LN55A; BY BF_SSN LN_SEQ LN_LON_BBS_SEQ LN_LON_BBT_SEQ; RUN;

%ENCHILADA(
LN55A,
LN55_LON_BBS_TIR,
BF_SSN  
LN_SEQ  
LN_LON_BBS_SEQ  
LN_LON_BBT_SEQ,  
LF_LON_BBS_TIR  
LF_LON_BBS_SUB_TIR  
PM_BBS_PGM  
PN_BBS_PGM_SEQ  
PF_BBS_PGM_TIR  
PN_BBS_PGM_TIR_SEQ  
LN_BR_DSB_SEQ  
LC_STA_LN55  
LD_STA_LN55  
LD_LON_BBT_CHK_ISS  
LD_BBT_DSQ_OVR_END  
LN_LON_BBT_PAY_OVR  
LD_LON_BBT_BEG  
LD_REB_MTD_LTR_SNT  
LC_LON_BBT_REB_MTD  
LN_BBT_PAY_PIF_MOT  
LN_BBT_PAY_DLQ_MOT  
LC_LON_BBT_DSQ_REA  
LC_LON_BBT_STA  
LN_LON_BBT_PAY  
LD_LON_BBT_STA  
LD_BBT_STS_PAY  
LF_LST_USR_LN55  
LF_LST_DTS_LN55  
LD_LON_BBT_ELG_FNL  
LD_BBT_DLQ_MOT_STS  
LD_BBT_PIF_MOT_STS  
LN_BBT_DLQ_MOT_OVR  
LN_BBT_PIF_MOT_OVR 
);

%FINISH(SOURCE=LN55A);
