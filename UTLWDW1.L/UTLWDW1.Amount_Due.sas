/*THIS SPECIFICIES WHAT DIRECTORY TO READ REPORTS FROM AND WHICH DATABASE TO UPDATE.*/
/*LIVE*/
%INCLUDE "X:\Sessions\Local SAS Schedule\UDW\UTLWDW1 UHEAA Data Warehouse Load.Folder and Database.sas";
%LET REPORT = X:\PADD\FTP;

/*TEST*/
/*%INCLUDE "X:\PADU\SAS\UDW\UTLWDW1 UHEAA Data Warehouse Load.Folder and Database.sas" ;
%LET REPORT = T:\SAS;*/

%FILECHECK(31);

DATA CUR_DUE (drop=trunc);
	INFILE REPORT31 DSD DLM = ',' FIRSTOBS=1 MISSOVER end = eof;
	INPUT DF_SPE_ACC_ID :$10. CUR_DUE :9.2 PAST_DUE :9.2 TOT_DUE :9.2 TOT_DUE_FEE :9.2 ;
%TRUNCAT(31,DF_SPE_ACC_ID);
RUN;
%copyerror;

PROC SORT DATA=CUR_DUE ; BY DF_SPE_ACC_ID ; RUN;
DATA CUR_DUE; SET CUR_DUE; BY DF_SPE_ACC_ID ; IF LAST.DF_SPE_ACC_ID; RUN;

%ENCHILADA(CUR_DUE,BORR_AMOUNTDUE,DF_SPE_ACC_ID,CUR_DUE PAST_DUE TOT_DUE TOT_DUE_FEE,);

%GOODBYE_NULL(BORR_AMOUNTDUE,CUR_DUE,0);
%GOODBYE_NULL(BORR_AMOUNTDUE,PAST_DUE,0);
%GOODBYE_NULL(BORR_AMOUNTDUE,TOT_DUE,0);
%GOODBYE_NULL(BORR_AMOUNTDUE,TOT_DUE_FEE,0);

%FINISH;
