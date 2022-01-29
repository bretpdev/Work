/*THIS SPECIFICIES WHAT DIRECTORY TO READ REPORTS FROM AND WHICH DATABASE TO UPDATE.*/
/*LIVE*/
%INCLUDE "X:\Sessions\Local SAS Schedule\UDW\UTLWDW1 UHEAA Data Warehouse Load.Folder and Database.sas";
%LET REPORT = X:\PADD\FTP;

/*TEST*/
/*%INCLUDE "X:\PADU\SAS\UDW\UTLWDW1 UHEAA Data Warehouse Load.Folder and Database.sas" ;
%LET REPORT = T:\SAS;*/

%FILECHECK(22);

DATA BR30 (drop=trunc);
	INFILE REPORT22 DSD DLM = ',' FIRSTOBS=1 MISSOVER end = eof;
	INPUT DF_SPE_ACC_ID :$10. BN_EFT_SEQ :2. BF_EFT_ABA :$9. BF_EFT_ACC :$17.
		 BC_EFT_STA :$1. BD_EFT_STA :$10. BA_EFT_ADD_WDR :9.2 BN_EFT_NSF_CTR :2.
		 BC_EFT_DNL_REA :$1. ;
%TRUNCAT(22,DF_SPE_ACC_ID);
RUN;
%copyerror;

PROC SORT DATA=BR30 ; BY DF_SPE_ACC_ID BN_EFT_SEQ; RUN;
DATA BR30; SET BR30; BY DF_SPE_ACC_ID; IF LAST.DF_SPE_ACC_ID; RUN;

%ENCHILADA(BR30,BR30_AUTOPAY,DF_SPE_ACC_ID,BN_EFT_SEQ BF_EFT_ABA BF_EFT_ACC BC_EFT_STA
	BD_EFT_STA BA_EFT_ADD_WDR BN_EFT_NSF_CTR BC_EFT_DNL_REA,WHERE BC_EFT_STA = 'A');

%GOODBYE_NULL(BR30_AUTOPAY,BN_EFT_SEQ,0);
%GOODBYE_NULL(BR30_AUTOPAY,BF_EFT_ABA,'');
%GOODBYE_NULL(BR30_AUTOPAY,BF_EFT_ACC,'');
%GOODBYE_NULL(BR30_AUTOPAY,BC_EFT_STA,'');
%GOODBYE_NULL(BR30_AUTOPAY,BD_EFT_STA,'');
%GOODBYE_NULL(BR30_AUTOPAY,BA_EFT_ADD_WDR,0);
%GOODBYE_NULL(BR30_AUTOPAY,BN_EFT_NSF_CTR,0);
%GOODBYE_NULL(BR30_AUTOPAY,BC_EFT_DNL_REA,'');

%FINISH;