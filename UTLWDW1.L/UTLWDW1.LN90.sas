/*THIS SPECIFICIES WHAT DIRECTORY TO READ REPORTS FROM AND WHICH DATABASE TO UPDATE.*/
/*LIVE*/
%INCLUDE "X:\Sessions\Local SAS Schedule\UDW\UTLWDW1 UHEAA Data Warehouse Load.Folder and Database.sas";
%LET REPORT = X:\PADD\FTP;

/*TEST*/
/*%INCLUDE "X:\PADU\SAS\UDW\UTLWDW1 UHEAA Data Warehouse Load.Folder and Database.sas" ;
%LET REPORT = T:\SAS;*/

%FILECHECK(51);

DATA LN90;
	INFILE REPORT51 DSD DLM = ',' FIRSTOBS=1 MISSOVER END = EOF;
	INPUT
		BF_SSN :$9.
		LN_SEQ :2.
		LN_FAT_SEQ :2.
		LC_FAT_REV_REA :$1.
		LD_FAT_APL :DATE9.
		LD_FAT_PST :DATE9.
		LD_FAT_EFF :DATE9.
		LD_FAT_DPS :DATE9.
		LC_CSH_ADV :$1.
		LD_STA_LON90 :DATE9.
		LC_STA_LON90 :$1.
		LA_FAT_PCL_FEE :7.2
		LA_FAT_NSI :7.2
		LA_FAT_LTE_FEE :7.2
		LA_FAT_ILG_PRI :8.2
		LA_FAT_CUR_PRI :8.2
		LF_LST_DTS_LN90 :DATETIME25.
		PC_FAT_TYP :$2.
		PC_FAT_SUB_TYP :$2.
		LA_FAT_NSI_ACR :7.2
		LI_FAT_RAP :$1.
		LN_FAT_SEQ_REV :2.
		LI_EFT_NSF_OVR :$1.
		LF_USR_EFT_NSF_OVR :$8.
		LA_FAT_MSC_FEE :12.2
		LA_FAT_MSC_FEE_PCV :12.2
		LA_FAT_DL_REB :12.2

	;
	%TRUNCAT(51, BF_SSN);
RUN;
%copyerror;

DATA LN90;
SET LN90;
	LD_FAT_APL = LD_FAT_APL * 86400;
	LD_FAT_PST = LD_FAT_PST * 86400;
	LD_FAT_EFF = LD_FAT_EFF * 86400;
	LD_FAT_DPS = LD_FAT_DPS * 86400;
	LD_STA_LON90 = LD_STA_LON90 * 86400;

RUN;

PROC SORT DATA=LN90; BY BF_SSN LN_SEQ LN_FAT_SEQ; RUN;

%ENCHILADA
	(
		LN90,
		LN90_FIN_ATY,
		BF_SSN  
		LN_SEQ  
		LN_FAT_SEQ,  
		LC_FAT_REV_REA  
		LD_FAT_APL  
		LD_FAT_PST  
		LD_FAT_EFF  
		LD_FAT_DPS  
		LC_CSH_ADV  
		LD_STA_LON90  
		LC_STA_LON90  
		LA_FAT_PCL_FEE  
		LA_FAT_NSI  
		LA_FAT_LTE_FEE  
		LA_FAT_ILG_PRI  
		LA_FAT_CUR_PRI  
		LF_LST_DTS_LN90  
		PC_FAT_TYP  
		PC_FAT_SUB_TYP  
		LA_FAT_NSI_ACR  
		LI_FAT_RAP  
		LN_FAT_SEQ_REV  
		LI_EFT_NSF_OVR  
		LF_USR_EFT_NSF_OVR  
		LA_FAT_MSC_FEE  
		LA_FAT_MSC_FEE_PCV  
		LA_FAT_DL_REB  

	)
;

%FINISH;
