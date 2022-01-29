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
%FILECHECK(58);


DATA RP30(drop=trunc);
	INFILE REPORT58 DSD DLM = ',' FIRSTOBS=1 MISSOVER end = eof;
	INPUT 
		IF_OWN :$8.
		PN_EFT_RIR_OWN_SEQ :2.
		IC_LON_PGM :$6.
		IF_GTR :$6.
		PD_LON_1_DSB :DATE9.
		PF_DOE_SCL_ORG :$8.
		PC_ST_BR_RSD_APL :$2.
		PD_EFT_RIR_EFF_BEG :DATE9.
		PD_EFT_RIR_EFF_END :DATE9.
		PC_EFT_RIR_STA :$1.
		PD_EFT_RIR_STA :DATE9.
		PI_EFT_RIR_PRC :$1.
		PC_EFT_NSF_LTR_REQ :$5.
		PR_EFT_RIR :5.3
		PF_LST_USR_RP30 :$8.
		PF_LST_DTS_RP30 :DATETIME25.
		PC_EFT_RIR_PNT_YR :$7.
		PD_EFT_BBS_LOT_BEG :DATE9.
		PD_EFT_BBS_GTE_DTE :DATE9.
		PD_EFT_BBS_RPD_SR :DATE9.
		PD_EFT_BBS_LCO_RCV :DATE9.
		PN_EFT_BBS_NSF_LMT :3.0
		PC_EFT_BBS_NSF_PRC :$1.
		PN_EFT_BBS_NSF_MTH :3.0
		PC_EFT_BBS_FED :$3.
		PI_EFT_RIR_RPY_0 :$1.;
%TRUNCAT1(58, IF_OWN);
RUN;
%COPYERROR;

DATA RP30;
SET RP30;
	PD_LON_1_DSB = PD_LON_1_DSB * 86400;
	PD_EFT_RIR_EFF_BEG = PD_EFT_RIR_EFF_BEG * 86400;
	PD_EFT_RIR_EFF_END = PD_EFT_RIR_EFF_END * 86400;
	PD_EFT_RIR_STA = PD_EFT_RIR_STA * 86400;
	PD_EFT_BBS_LOT_BEG = PD_EFT_BBS_LOT_BEG * 86400;
	PD_EFT_BBS_GTE_DTE = PD_EFT_BBS_GTE_DTE * 86400;
	PD_EFT_BBS_RPD_SR = PD_EFT_BBS_RPD_SR * 86400;
	PD_EFT_BBS_LCO_RCV = PD_EFT_BBS_LCO_RCV * 86400;
RUN;

PROC SORT DATA=RP30; BY IF_OWN PN_EFT_RIR_OWN_SEQ; RUN;

%ENCHILADA(
RP30,
RP30_EFT_RIR_PAR,
IF_OWN  
PN_EFT_RIR_OWN_SEQ,  
IC_LON_PGM  
IF_GTR  
PD_LON_1_DSB  
PF_DOE_SCL_ORG  
PC_ST_BR_RSD_APL  
PD_EFT_RIR_EFF_BEG  
PD_EFT_RIR_EFF_END  
PC_EFT_RIR_STA  
PD_EFT_RIR_STA  
PI_EFT_RIR_PRC  
PC_EFT_NSF_LTR_REQ  
PR_EFT_RIR  
PF_LST_USR_RP30  
PF_LST_DTS_RP30  
PC_EFT_RIR_PNT_YR  
PD_EFT_BBS_LOT_BEG  
PD_EFT_BBS_GTE_DTE  
PD_EFT_BBS_RPD_SR  
PD_EFT_BBS_LCO_RCV  
PN_EFT_BBS_NSF_LMT  
PC_EFT_BBS_NSF_PRC  
PN_EFT_BBS_NSF_MTH  
PC_EFT_BBS_FED  
PI_EFT_RIR_RPY_0 
);

%FINISH(SOURCE=RP30);
