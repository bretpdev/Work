/*THIS SPECIFICIES WHAT DIRECTORY TO READ REPORTS FROM AND WHICH DATABASE TO UPDATE.*/
/*LIVE*/
%INCLUDE "X:\Sessions\Local SAS Schedule\UDW\UTLWDW1 UHEAA Data Warehouse Load.Folder and Database.sas";
%LET REPORT = X:\PADD\FTP;

/*TEST*/
/*%INCLUDE "X:\PADU\SAS\UDW\UTLWDW1 UHEAA Data Warehouse Load.Folder and Database.sas" ;
%LET REPORT = T:\SAS;*/

%FILECHECK(46);

DATA PD24;
	INFILE REPORT46 DSD DLM = ',' FIRSTOBS=1 MISSOVER END = EOF;
	INPUT 
		DF_PRS_ID :$9.
		DD_BKR_NTF :DATE9.
		DD_BKR_FIL :DATE9.
		DC_BKR_TYP :$2.
		DF_ATT :$9.
		DF_COU_DKT :$12.
		DD_BKR_VER :DATE9.
		DC_BKR_DCH_NDC :$1.
		DM_BKR_CT :$20.
		DC_BKR_ST :$2.
		DD_BKR_COR_1_RCV :DATE9.
		DA_BKR_DCH :9.2
		DD_BKR_STA :DATE9.
		DD_BKR_POO_ACK :DATE9.
		DD_BKR_POO :DATE9.
		DD_BKR_DCH_RCV :DATE9.
		DD_BKR_CDR_RCV :DATE9.
		DD_BKR_ADS_RCV :DATE9.
		DN_BKR_ADS :$10.
		DC_BKR_STA :$2.
		DF_LST_DTS_PD24 :DATETIME25.
		IF_IST :$8.
		DM_FGN_CNY_BKR_FIL :$15.
		DM_FGN_ST_BKR_FIL :$15.
		DD_BKR_RAF :DATE9.
		DD_COU_LST_CNC :DATE9.
		DD_BKR_CAE_CLO :DATE9.
		DD_BKR_CHP_CVN :DATE9.
		DD_BKR_COR_LST_RCV :DATE9.

	;
	%TRUNCAT(46, DF_PRS_ID);
RUN;
%copyerror;

DATA PD24;
SET PD24;
	DD_BKR_NTF = DD_BKR_NTF * 86400;
	DD_BKR_FIL = DD_BKR_FIL * 86400;
	DD_BKR_VER = DD_BKR_VER * 86400;
	DD_BKR_COR_1_RCV = DD_BKR_COR_1_RCV * 86400;
	DD_BKR_STA = DD_BKR_STA * 86400;
	DD_BKR_POO_ACK = DD_BKR_POO_ACK * 86400;
	DD_BKR_POO = DD_BKR_POO * 86400;
	DD_BKR_DCH_RCV = DD_BKR_DCH_RCV * 86400;
	DD_BKR_CDR_RCV = DD_BKR_CDR_RCV * 86400;
	DD_BKR_ADS_RCV = DD_BKR_ADS_RCV * 86400;
	DD_BKR_RAF = DD_BKR_RAF * 86400;
	DD_COU_LST_CNC = DD_COU_LST_CNC * 86400;
	DD_BKR_CAE_CLO = DD_BKR_CAE_CLO * 86400;
	DD_BKR_CHP_CVN = DD_BKR_CHP_CVN * 86400;
	DD_BKR_COR_LST_RCV = DD_BKR_COR_LST_RCV * 86400;
RUN;

PROC SORT DATA=PD24; BY DF_PRS_ID DD_BKR_NTF; RUN;

%ENCHILADA
	(
		PD24,
		PD24_PRS_BKR,
		DF_PRS_ID  
		DD_BKR_NTF,  
		DD_BKR_FIL  
		DC_BKR_TYP  
		DF_ATT  
		DF_COU_DKT  
		DD_BKR_VER  
		DC_BKR_DCH_NDC  
		DM_BKR_CT  
		DC_BKR_ST  
		DD_BKR_COR_1_RCV  
		DA_BKR_DCH  
		DD_BKR_STA  
		DD_BKR_POO_ACK  
		DD_BKR_POO  
		DD_BKR_DCH_RCV  
		DD_BKR_CDR_RCV  
		DD_BKR_ADS_RCV  
		DN_BKR_ADS  
		DC_BKR_STA  
		DF_LST_DTS_PD24  
		IF_IST  
		DM_FGN_CNY_BKR_FIL  
		DM_FGN_ST_BKR_FIL  
		DD_BKR_RAF  
		DD_COU_LST_CNC  
		DD_BKR_CAE_CLO  
		DD_BKR_CHP_CVN  
		DD_BKR_COR_LST_RCV  

	)
;

%FINISH;