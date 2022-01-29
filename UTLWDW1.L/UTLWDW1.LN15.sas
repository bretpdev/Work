/*THIS SPECIFICIES WHAT DIRECTORY TO READ REPORTS FROM AND WHICH DATABASE TO UPDATE.*/
/*LIVE*/
%INCLUDE "X:\Sessions\Local SAS Schedule\UDW\UTLWDW1 UHEAA Data Warehouse Load.Folder and Database.sas";
%LET REPORT = X:\PADD\FTP;

/*TEST*/
/*%INCLUDE "X:\PADU\SAS\UDW\UTLWDW1 UHEAA Data Warehouse Load.Folder and Database.sas" ;
%LET REPORT = T:\SAS;*/

%FILECHECK(35);

DATA LN15;
	INFILE REPORT35 DSD DLM = ',' FIRSTOBS=1 MISSOVER END = EOF;
	INPUT 
		DF_SPE_ACC_ID :$10.
		LN_BR_DSB_SEQ :6.
		LA_DSB :8.2
		LD_DSB :$10.
		LC_DSB_TYP :$1.
		LC_STA_LON15 :$1.
		LN_SEQ :6.
		LA_DL_REBATE :12.2
	;
	%TRUNCAT(35,DF_SPE_ACC_ID);
RUN;
%copyerror;

PROC SORT DATA=LN15; BY DF_SPE_ACC_ID LN_BR_DSB_SEQ; RUN;
DATA LN15; SET LN15; BY DF_SPE_ACC_ID LN_BR_DSB_SEQ; IF LAST.LN_BR_DSB_SEQ; RUN;

%ENCHILADA
	(
		LN15,
		LN15_Disbursement,
		DF_SPE_ACC_ID LN_BR_DSB_SEQ,
		LA_DSB LD_DSB LC_DSB_TYP LC_STA_LON15 LN_SEQ LA_DL_REBATE,
	)
;

%GOODBYE_NULL(LN15_Disbursement,LN_SEQ,0);
%GOODBYE_NULL(LN15_Disbursement,LA_DL_REBATE,0);

%FINISH;