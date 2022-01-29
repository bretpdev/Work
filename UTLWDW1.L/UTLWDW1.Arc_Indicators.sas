/*THIS SPECIFICIES WHAT DIRECTORY TO READ REPORTS FROM AND WHICH DATABASE TO UPDATE.*/
/*LIVE*/
%INCLUDE "X:\Sessions\Local SAS Schedule\UDW\UTLWDW1 UHEAA Data Warehouse Load.Folder and Database.sas";
%LET REPORT = X:\PADD\FTP;

/*TEST*/
/*%INCLUDE "X:\PADU\SAS\UDW\UTLWDW1 UHEAA Data Warehouse Load.Folder and Database.sas" ;
%LET REPORT = T:\SAS;*/

%FILECHECK(34);

DATA ARCS(drop=trunc);
	INFILE REPORT34 DSD DLM = ',' FIRSTOBS=1 MISSOVER end = eof;
	INPUT DF_SPE_ACC_ID :$10. LN_ATY_SEQ :6. PF_REQ_ACT :$5. LC_STA_ACTY10 :$1.;
%TRUNCAT(34,DF_SPE_ACC_ID);
RUN;
%copyerror;

PROC SORT DATA=ARCS ; BY DF_SPE_ACC_ID LN_ATY_SEQ; RUN;
DATA ARCS; SET ARCS; BY DF_SPE_ACC_ID LN_ATY_SEQ; IF LAST.LN_ATY_SEQ; RUN;

%ENCHILADA(ARCS,AY10_ARCINDICATORS,DF_SPE_ACC_ID LN_ATY_SEQ,PF_REQ_ACT LC_STA_ACTY10,WHERE LC_STA_ACTY10 = 'A');

%GOODBYE_NULL(AY10_ARCINDICATORS,LC_STA_ACTY10,'');

%FINISH;
