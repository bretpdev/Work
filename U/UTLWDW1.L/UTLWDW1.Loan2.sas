/*THIS SPECIFICIES WHAT DIRECTORY TO READ REPORTS FROM AND WHICH DATABASE TO UPDATE.*/
/*LIVE*/
%INCLUDE "X:\Sessions\Local SAS Schedule\UDW\UTLWDW1 UHEAA Data Warehouse Load.Folder and Database.sas";
%LET REPORT = X:\PADD\FTP;

/*TEST*/
/*%INCLUDE "X:\PADU\SAS\UDW\UTLWDW1 UHEAA Data Warehouse Load.Folder and Database.sas" ;
%LET REPORT = T:\SAS;*/

%FILECHECK(29);

DATA DW01(DROP=WX_OVR_DW_LON_STA trunc) ;
	INFILE REPORT29 DSD DLM = ',' FIRSTOBS=1 MISSOVER end = eof;
	INPUT DF_SPE_ACC_ID :$10. LN_SEQ :6. WC_DW_LON_STA :$2. WA_TOT_BRI_OTS :14.2 WD_LON_RPD_SR :$10. WX_OVR_DW_LON_STA :$20. ;
	FORMAT DW_LON_STA $20.;
	if wx_ovr_dw_lon_sta = '' then DW_LON_STA = PUT(WC_DW_LON_STA,$LONSTA.);
	else DW_LON_STA = WX_OVR_DW_LON_STA; 
%TRUNCAT(29,DF_SPE_ACC_ID);
RUN;
%copyerror;

PROC SORT DATA=DW01 ; BY DF_SPE_ACC_ID LN_SEQ; RUN;
DATA DW01; SET DW01; BY DF_SPE_ACC_ID LN_SEQ; IF LAST.LN_SEQ; RUN;

%ENCHILADA(DW01,DW01_LOAN,DF_SPE_ACC_ID LN_SEQ,WC_DW_LON_STA WA_TOT_BRI_OTS WD_LON_RPD_SR DW_LON_STA,);

%GOODBYE_NULL(DW01_LOAN,WC_DW_LON_STA,'');
%GOODBYE_NULL(DW01_LOAN,DW_LON_STA,'');
%GOODBYE_NULL(DW01_LOAN,WA_TOT_BRI_OTS,0);
%GOODBYE_NULL(DW01_LOAN,WD_LON_RPD_SR,'');

%FINISH;