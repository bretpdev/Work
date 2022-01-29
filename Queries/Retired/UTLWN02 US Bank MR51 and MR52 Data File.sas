/*====================================*/
/*UTLWN02 US BANK MR51, MR52 DATA FILE*/
/*====================================*/
/*PLEASE NOTE: THIS DIRECTORY HAS BEEN CHANGED TO PROGREVW BECAUSE OF THE SIZE OF THE OUTPUT FILE*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = /sas/whse/progrevw   ; */
/*FILENAME REPORTZ "&RPTLIB/ULWN02.LWN02RZ";*/
/*FILENAME REPORT2 "&RPTLIB/ULWN02.LWN02R2";*/

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;

%MACRO SQLCHECK (SQLRPT= );
%IF &SQLXRC NE 0 %THEN %DO;
	FILENAME REPORTZ "&RPTLIB/&SQLRPT";
	DATA _NULL_;
    FILE REPORTZ NOTITLES;
    PUT @01 " ********************************************************************* "
      / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
      / @01 " ****  THE SAS LOG IN &SQLRPT SHOULD BE REVIEWED.          **** "       
      / @01 " ********************************************************************* "
      / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
      / @01 " ****  &SQLXMSG   **** "
      / @01 " ********************************************************************* ";
	RUN;
%END;
%MEND;

PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE USBMRDF AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT 
A.IF_OWN_PRN  		AS MR51_IF_OWN_PRN  		
,A.BF_SSN  					AS MR51_BF_SSN  			
,A.LN_SEQ  					AS MR51_LN_SEQ  			
,A.IF_OWN  					AS MR51_IF_OWN  			
,A.IF_BND_ISS  				AS MR51_IF_BND_ISS  		
,A.IC_LON_PGM  				AS MR51_IC_LON_PGM  		
,A.IF_GTR  					AS MR51_IF_GTR  			
,A.LF_CUR_POR  				AS MR51_LF_CUR_POR  		
,A.WC_TYP_SCL_ORG  			AS MR51_WC_TYP_SCL_ORG  	
,A.WR_ITR_1  				AS MR51_WR_ITR_1  			
,A.WC_ITR_TYP_1  			AS MR51_WC_ITR_TYP_1  		
,A.LD_LON_1_DSB  			AS MR51_LD_LON_1_DSB  		
,A.WC_ELG_SIN_1  			AS MR51_WC_ELG_SIN_1  		
,A.WA_CUR_PRI  				AS MR51_WA_CUR_PRI  		
,A.WA_CUR_BR_INT  			AS MR51_WA_CUR_BR_INT  		
,A.WA_CUR_GOV_INT  			AS MR51_WA_CUR_GOV_INT  	
,A.WA_CUR_OTH_CHR  			AS MR51_WA_CUR_OTH_CHR  	
,A.WD_SAR_ATY_EFF  			AS MR51_WD_SAR_ATY_EFF  	
,A.WA_AVG_DAY_BAL  			AS MR51_WA_AVG_DAY_BAL  	
,A.WN_SQ_FOR_APL  			AS MR51_WN_SQ_FOR_APL  		
,A.WX_TYP_REC_CLP_LON  		AS MR51_WX_TYP_REC_CLP_LON  
,A.WF_SUP_ATY_RPT_LN1  		AS MR51_WF_SUP_ATY_RPT_LN1  
,A.WF_SUP_ATY_RPT_LN2  		AS MR51_WF_SUP_ATY_RPT_LN2  
,A.WF_SUP_ATY_RPT_LN3  		AS MR51_WF_SUP_ATY_RPT_LN3  
,A.WF_SUP_ATY_RPT_LN4  		AS MR51_WF_SUP_ATY_RPT_LN4  
,A.WF_SUP_ATY_RPT_LN5  		AS MR51_WF_SUP_ATY_RPT_LN5  
,A.WF_SUP_ATY_RPT_LN6  		AS MR51_WF_SUP_ATY_RPT_LN6  
,A.WF_SUP_ATY_RPT_LN7  		AS MR51_WF_SUP_ATY_RPT_LN7  
,A.WF_SUP_ATY_RPT_LN8  		AS MR51_WF_SUP_ATY_RPT_LN8  
,A.WF_SUP_ATY_RPT_LN9  		AS MR51_WF_SUP_ATY_RPT_LN9  
,A.WF_SAR_LN10  			AS MR51_WF_SAR_LN10  		
,A.LC_SCY_PGA_PGM_YR  		AS MR51_LC_SCY_PGA_PGM_YR  	
,A.LC_CU_LIA  				AS MR51_LC_CU_LIA  			
,A.WA_PRI_CU_BEG  			AS MR51_WA_PRI_CU_BEG  		
,A.WA_BRI_ACR_CU_BEG  		AS MR51_WA_BRI_ACR_CU_BEG  	
,A.WA_CLM_PRI_BKR  			AS MR51_WA_CLM_PRI_BKR  	
,A.WA_CLM_INT_BKR  			AS MR51_WA_CLM_INT_BKR  	
,A.WA_CLM_PRI_DTH  			AS MR51_WA_CLM_PRI_DTH  	
,A.WA_CLM_INT_DTH  			AS MR51_WA_CLM_INT_DTH  	
,A.WA_CLM_PRI_DSA  			AS MR51_WA_CLM_PRI_DSA  	
,A.WA_CLM_INT_DSA  			AS MR51_WA_CLM_INT_DSA  	
,A.WA_CLM_PRI_ILG  			AS MR51_WA_CLM_PRI_ILG  	
,A.WA_CLM_INT_ILG  			AS MR51_WA_CLM_INT_ILG  	
,A.WA_CLM_PRI_ISL_DLQ  		AS MR51_WA_CLM_PRI_ISL_DLQ  
,A.WA_CLM_INT_ISL_DLQ  		AS MR51_WA_CLM_INT_ISL_DLQ  
,A.WA_CLM_PRI_INT_DLQ  		AS MR51_WA_CLM_PRI_INT_DLQ  
,A.WA_CLM_INT_INT_DLQ  		AS MR51_WA_CLM_INT_INT_DLQ  
,A.WA_CLM_PRI_CLS  			AS MR51_WA_CLM_PRI_CLS  	
,A.WA_CLM_INT_CLS  			AS MR51_WA_CLM_INT_CLS  	
,A.WA_CLM_PRI_CU  			AS MR51_WA_CLM_PRI_CU  		
,A.WA_CLM_INT_CU  			AS MR51_WA_CLM_INT_CU  		
,A.WA_CLM_PRI_SUP  			AS MR51_WA_CLM_PRI_SUP  	
,A.WA_CLM_INT_SUP  			AS MR51_WA_CLM_INT_SUP  	
,A.WA_CLM_PRI_SKP  			AS MR51_WA_CLM_PRI_SKP  	
,A.WA_CLM_INT_SKP  			AS MR51_WA_CLM_INT_SKP  	
,A.WA_CLM_PRI_OTH  			AS MR51_WA_CLM_PRI_OTH  	
,A.WA_CLM_INT_OTH  			AS MR51_WA_CLM_INT_OTH  	
,A.WA_ORG_CHK  				AS MR51_WA_ORG_CHK  		
,A.WA_ORG_EFT  				AS MR51_WA_ORG_EFT  		
,A.WN_DSB_CHK_SQ  			AS MR51_WN_DSB_CHK_SQ  		
,A.WA_DSB_CHK_SQ  			AS MR51_WA_DSB_CHK_SQ  		
,A.WA_DSB_CAN_EFT_1  		AS MR51_WA_DSB_CAN_EFT_1  	
,A.WA_DSB_CAN_CHK_1  		AS MR51_WA_DSB_CAN_CHK_1  	
,A.WN_DSB_CAN_EFT_SQ  		AS MR51_WN_DSB_CAN_EFT_SQ  	
,A.WA_DSB_CAN_EFT_SQ  		AS MR51_WA_DSB_CAN_EFT_SQ  	
,A.WN_DSB_CAN_CHK_SQ  		AS MR51_WN_DSB_CAN_CHK_SQ  	
,A.WA_DSB_CAN_CHK_SQ  		AS MR51_WA_DSB_CAN_CHK_SQ  	
,A.WA_FAT_CUR_PRI_DCV  		AS MR51_WA_FAT_CUR_PRI_DCV  
,A.WA_FAT_NSI_DCV  			AS MR51_WA_FAT_NSI_DCV  	
,A.WA_FAT_CUR_PRI_CVN  		AS MR51_WA_FAT_CUR_PRI_CVN  
,A.WA_FAT_NSI_CVN  			AS MR51_WA_FAT_NSI_CVN  	
,A.WA_FAT_CUR_PRI_TRF  		AS MR51_WA_FAT_CUR_PRI_TRF  
,A.WA_FAT_NSI_TRF  			AS MR51_WA_FAT_NSI_TRF  	
,A.WA_FAT_CUR_PRI_RPR  		AS MR51_WA_FAT_CUR_PRI_RPR  
,A.WA_FAT_NSI_RPR  			AS MR51_WA_FAT_NSI_RPR  	
,A.WA_FAT_CUR_PRI_EXT  		AS MR51_WA_FAT_CUR_PRI_EXT  
,A.WA_FAT_NSI_EXT  			AS MR51_WA_FAT_NSI_EXT  	
,A.WA_FAT_CUR_PRI_NEW  		AS MR51_WA_FAT_CUR_PRI_NEW  
,A.WA_FAT_NSI_NEW  			AS MR51_WA_FAT_NSI_NEW  	
,A.WA_DSB_EFT_1  			AS MR51_WA_DSB_EFT_1  		
,A.WA_DSB_CHK_1  			AS MR51_WA_DSB_CHK_1  		
,A.WN_DSB_EFT_SQ  			AS MR51_WN_DSB_EFT_SQ  		
,A.WA_DSB_EFT_SQ  			AS MR51_WA_DSB_EFT_SQ  		
,A.LF_DOE_SCL_ORG  			AS MR51_LF_DOE_SCL_ORG  	
,A.WF_TIR_PCE_LN35  		AS MR51_WF_TIR_PCE_LN35  	
,A.LD_CU_ENT  				AS MR51_LD_CU_ENT  			
,A.WD_CLM_BKR_SBM  			AS MR51_WD_CLM_BKR_SBM  	
,A.WD_CLM_DTH_SBM  			AS MR51_WD_CLM_DTH_SBM  	
,A.WD_CLM_DSA_SBM  			AS MR51_WD_CLM_DSA_SBM  	
,A.WD_CLM_ILG_SBM  			AS MR51_WD_CLM_ILG_SBM  	
,A.WD_CLM_ISL_DLQ_SBM  		AS MR51_WD_CLM_ISL_DLQ_SBM  
,A.WD_CLM_INT_DLQ_SBM  		AS MR51_WD_CLM_INT_DLQ_SBM  
,A.WD_CLM_CLS_SBM  			AS MR51_WD_CLM_CLS_SBM  	
,A.WD_CLM_SKP_SBM  			AS MR51_WD_CLM_SKP_SBM  	
,A.WD_CLM_CU_SBM  			AS MR51_WD_CLM_CU_SBM  		
,A.WD_CLM_SUP_SBM  			AS MR51_WD_CLM_SUP_SBM  	
,A.WD_CLM_OTH_SBM  			AS MR51_WD_CLM_OTH_SBM  	
,A.LD_FOR_BEG  				AS MR51_LD_FOR_BEG  		
,A.LD_END_GRC_PRD  			AS MR51_LD_END_GRC_PRD  	
,A.WD_DCV_EFF  				AS MR51_WD_DCV_EFF  		
,A.WD_ORG_EFT_EFF  			AS MR51_WD_ORG_EFT_EFF  	
,A.WD_ORG_CHK_EFF  			AS MR51_WD_ORG_CHK_EFF  	
,A.WD_TRF_EFF  				AS MR51_WD_TRF_EFF  		
,A.WD_CVN_EFF  				AS MR51_WD_CVN_EFF  		
,A.WD_RPR_EFF  				AS MR51_WD_RPR_EFF  		
,A.WD_EXT_ORG_EFF  			AS MR51_WD_EXT_ORG_EFF  	
,A.WD_NEW_ORG_EFF  			AS MR51_WD_NEW_ORG_EFF  	
,A.WD_DSB_EFT_1  			AS MR51_WD_DSB_EFT_1  		
,A.WD_DSB_CHK_1  			AS MR51_WD_DSB_CHK_1  		
,A.WD_DSB_EFT_SQ  			AS MR51_WD_DSB_EFT_SQ  		
,A.WD_DSB_CHK_SQ  			AS MR51_WD_DSB_CHK_SQ  		
,A.WD_DSB_CAN_EFT_1  		AS MR51_WD_DSB_CAN_EFT_1  	
,A.WD_DSB_CAN_CHK_1  		AS MR51_WD_DSB_CAN_CHK_1  	
,A.WD_DSB_CAN_EFT_SQ  		AS MR51_WD_DSB_CAN_EFT_SQ  	
,A.WD_DSB_CAN_CHK_SQ  		AS MR51_WD_DSB_CAN_CHK_SQ  	
,A.WI_PPR_SCL_ORG  			AS MR51_WI_PPR_SCL_ORG  	

,B.LD_DSB_CAN 				AS MR52_LD_DSB_CAN 				
,B.LA_DSB_CAN 				AS MR52_LA_DSB_CAN 				
,B.LD_DSB 					AS MR52_LD_DSB 					
,B.LA_DSB 					AS MR52_LA_DSB 					
,B.LC_DSB_TYP 				AS MR52_LC_DSB_TYP 				
,B.WX_DSB_TYP 				AS MR52_WX_DSB_TYP 				
,B.LC_DSB_CAN_TYP 			AS MR52_LC_DSB_CAN_TYP 			
,B.WX_DSB_CAN_TYP 			AS MR52_WX_DSB_CAN_TYP 			
,B.LC_DSB_FEE_1 			AS MR52_LC_DSB_FEE_1 			
,B.WX_DSB_FEE_1 			AS MR52_WX_DSB_FEE_1 			
,B.LA_DSB_FEE_1 			AS MR52_LA_DSB_FEE_1 			
,B.LC_DSB_FEE_2 			AS MR52_LC_DSB_FEE_2 			
,B.WX_DSB_FEE_2 			AS MR52_WX_DSB_FEE_2 			
,B.LA_DSB_FEE_2 			AS MR52_LA_DSB_FEE_2 			
,B.LC_DSB_FEE_3 			AS MR52_LC_DSB_FEE_3 			
,B.WX_DSB_FEE_3 			AS MR52_WX_DSB_FEE_3 			
,B.LA_DSB_FEE_3 			AS MR52_LA_DSB_FEE_3 			
,B.LF_DSB_CHK 				AS MR52_LF_DSB_CHK 				
,B.LC_LDR_DSB_MDM 			AS MR52_LC_LDR_DSB_MDM 			
,B.WX_LDR_DSB_MDM 			AS MR52_WX_LDR_DSB_MDM 			
,B.WX_POR_RCC_RPT_LN1 		AS MR52_WX_POR_RCC_RPT_LN1 		
,B.WX_POR_RCC_RPT_LN2 		AS MR52_WX_POR_RCC_RPT_LN2 		
,B.AF_DSB_RPT 				AS MR52_AF_DSB_RPT 				
,B.DM_PRS_1 				AS MR52_DM_PRS_1 				
,B.DM_PRS_MID 				AS MR52_DM_PRS_MID 				
,B.DM_PRS_LST 				AS MR52_DM_PRS_LST 				
,B.DM_PRS_LST_SFX 			AS MR52_DM_PRS_LST_SFX 			
,B.WF_OWN_RSB_ORG_FEE 		AS MR52_WF_OWN_RSB_ORG_FEE 		
,B.WF_BND_ISS_RSB_ORG 		AS MR52_WF_BND_ISS_RSB_ORG 		
,B.WN_DSB_TOT 				AS MR52_WN_DSB_TOT 				
,B.WN_CAN 					AS MR52_WN_CAN 					
,B.WN_CAN_TOT 				AS MR52_WN_CAN_TOT 				
,B.WX_POR_RCC_RPT_LN3 		AS MR52_WX_POR_RCC_RPT_LN3 		
,B.WA_DSB_FEE_CAN_1 		AS MR52_WA_DSB_FEE_CAN_1 		
,B.WA_DSB_FEE_CAN_2 		AS MR52_WA_DSB_FEE_CAN_2 		
,B.WR_ITR_1 				AS MR52_WR_ITR_1 				
,B.WC_ITR_TYP_1 			AS MR52_WC_ITR_TYP_1 			
,B.LF_CUR_POR 				AS MR52_LF_CUR_POR 				
,B.WC_TYP_SCL_ORG 			AS MR52_WC_TYP_SCL_ORG 			
,B.WC_ELG_SIN_1 			AS MR52_WC_ELG_SIN_1 			
,B.LC_SCY_PGA_PGM_YR 		AS MR52_LC_SCY_PGA_PGM_YR 		
,B.LF_DOE_SCL_ORG 			AS MR52_LF_DOE_SCL_ORG 			
,B.WF_TIR_PCE_MR52 			AS MR52_WF_TIR_PCE_MR52 		
,B.WI_DSB_ORG 				AS MR52_WI_DSB_ORG 				
,B.WD_DSB_CAN_REV 			AS MR52_WD_DSB_CAN_REV 			
,B.WA_DSB_CAN_REV 			AS MR52_WA_DSB_CAN_REV 			
,B.WA_FEE_CAN_REV_1 		AS MR52_WA_FEE_CAN_REV_1 		
,B.WA_FEE_CAN_REV_2 		AS MR52_WA_FEE_CAN_REV_2 		
,B.WN_CAN_REV 				AS MR52_WN_CAN_REV 				
,B.WN_CAN_REV_TOT 			AS MR52_WN_CAN_REV_TOT 			
,B.WX_POR_RCC_RPT_LN4 		AS MR52_WX_POR_RCC_RPT_LN4 		
,B.LC_DSB_CAN_REA 			AS MR52_LC_DSB_CAN_REA 			
,B.WX_DSB_CAN_REA 			AS MR52_WX_DSB_CAN_REA 			
,B.AF_LON_ALT 				AS MR52_AF_LON_ALT 				
,B.AN_SEQ_COM_LN_APL 		AS MR52_AN_SEQ_COM_LN_APL 		
 
FROM OLWHRM1.MR51_MR_SUP_ATY A
INNER JOIN OLWHRM1.MR52_MR_DSB_MTH B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
	AND A.IC_LON_PGM = B.IC_LON_PGM 
	AND A.IF_GTR = B.IF_GTR 
	AND A.IF_BND_ISS = B.AF_BND_ISS
WHERE A.IF_OWN = '811698'
AND A.WA_CUR_PRI > 0
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWN01.LWN01RZ);*/
/*QUIT;*/

ENDRSUBMIT;

DATA USBMRDF;
SET WORKLOCL.USBMRDF;
RUN;

PROC SORT DATA=USBMRDF NODUPKEY;
BY MR51_BF_SSN MR51_LN_SEQ;
RUN;

DATA _NULL_;
SET  WORK.USBMRDF;
FILE 'T:\SAS\ULWN02.LWN02R2.TXT' DELIMITER=',' DSD DROPOVER LRECL=32767;
*FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT MR51_IF_OWN_PRN $8. ;
FORMAT MR51_BF_SSN $9. ;
FORMAT MR51_LN_SEQ 6. ;
FORMAT MR51_IF_OWN $8. ;
FORMAT MR51_IF_BND_ISS $8. ;
FORMAT MR51_IC_LON_PGM $6. ;
FORMAT MR51_IF_GTR $6. ;
FORMAT MR51_LF_CUR_POR $20. ;
FORMAT MR51_WC_TYP_SCL_ORG $2. ;
FORMAT MR51_WR_ITR_1 7.3 ;
FORMAT MR51_WC_ITR_TYP_1 $2. ;
FORMAT MR51_LD_LON_1_DSB MMDDYY10. ;
FORMAT MR51_WC_ELG_SIN_1 $1. ;
FORMAT MR51_WA_CUR_PRI 10.2 ;
FORMAT MR51_WA_CUR_BR_INT 10.2 ;
FORMAT MR51_WA_CUR_GOV_INT 9.2 ;
FORMAT MR51_WA_CUR_OTH_CHR 9.2 ;
FORMAT MR51_WD_SAR_ATY_EFF MMDDYY10. ;
FORMAT MR51_WA_AVG_DAY_BAL 10.2 ;
FORMAT MR51_WN_SQ_FOR_APL 4. ;
FORMAT MR51_WX_TYP_REC_CLP_LON $20. ;
FORMAT MR51_WF_SUP_ATY_RPT_LN1 $3. ;
FORMAT MR51_WF_SUP_ATY_RPT_LN2 $3. ;
FORMAT MR51_WF_SUP_ATY_RPT_LN3 $3. ;
FORMAT MR51_WF_SUP_ATY_RPT_LN4 $3. ;
FORMAT MR51_WF_SUP_ATY_RPT_LN5 $3. ;
FORMAT MR51_WF_SUP_ATY_RPT_LN6 $3. ;
FORMAT MR51_WF_SUP_ATY_RPT_LN7 $3. ;
FORMAT MR51_WF_SUP_ATY_RPT_LN8 $3. ;
FORMAT MR51_WF_SUP_ATY_RPT_LN9 $3. ;
FORMAT MR51_WF_SAR_LN10 $3. ;
FORMAT MR51_LC_SCY_PGA_PGM_YR $1. ;
FORMAT MR51_LC_CU_LIA $1. ;
FORMAT MR51_WA_PRI_CU_BEG 10.2 ;
FORMAT MR51_WA_BRI_ACR_CU_BEG 9.2 ;
FORMAT MR51_WA_CLM_PRI_BKR 10.2 ;
FORMAT MR51_WA_CLM_INT_BKR 9.2 ;
FORMAT MR51_WA_CLM_PRI_DTH 10.2 ;
FORMAT MR51_WA_CLM_INT_DTH 9.2 ;
FORMAT MR51_WA_CLM_PRI_DSA 10.2 ;
FORMAT MR51_WA_CLM_INT_DSA 9.2 ;
FORMAT MR51_WA_CLM_PRI_ILG 10.2 ;
FORMAT MR51_WA_CLM_INT_ILG 9.2 ;
FORMAT MR51_WA_CLM_PRI_ISL_DLQ 10.2 ;
FORMAT MR51_WA_CLM_INT_ISL_DLQ 9.2 ;
FORMAT MR51_WA_CLM_PRI_INT_DLQ 10.2 ;
FORMAT MR51_WA_CLM_INT_INT_DLQ 9.2 ;
FORMAT MR51_WA_CLM_PRI_CLS 10.2 ;
FORMAT MR51_WA_CLM_INT_CLS 9.2 ;
FORMAT MR51_WA_CLM_PRI_CU 10.2 ;
FORMAT MR51_WA_CLM_INT_CU 9.2 ;
FORMAT MR51_WA_CLM_PRI_SUP 10.2 ;
FORMAT MR51_WA_CLM_INT_SUP 9.2 ;
FORMAT MR51_WA_CLM_PRI_SKP 10.2 ;
FORMAT MR51_WA_CLM_INT_SKP 9.2 ;
FORMAT MR51_WA_CLM_PRI_OTH 10.2 ;
FORMAT MR51_WA_CLM_INT_OTH 9.2 ;
FORMAT MR51_WA_ORG_CHK 10.2 ;
FORMAT MR51_WA_ORG_EFT 10.2 ;
FORMAT MR51_WN_DSB_CHK_SQ 4. ;
FORMAT MR51_WA_DSB_CHK_SQ 10.2 ;
FORMAT MR51_WA_DSB_CAN_EFT_1 10.2 ;
FORMAT MR51_WA_DSB_CAN_CHK_1 10.2 ;
FORMAT MR51_WN_DSB_CAN_EFT_SQ 4. ;
FORMAT MR51_WA_DSB_CAN_EFT_SQ 10.2 ;
FORMAT MR51_WN_DSB_CAN_CHK_SQ 4. ;
FORMAT MR51_WA_DSB_CAN_CHK_SQ 10.2 ;
FORMAT MR51_WA_FAT_CUR_PRI_DCV 10.2 ;
FORMAT MR51_WA_FAT_NSI_DCV 9.2 ;
FORMAT MR51_WA_FAT_CUR_PRI_CVN 10.2 ;
FORMAT MR51_WA_FAT_NSI_CVN 9.2 ;
FORMAT MR51_WA_FAT_CUR_PRI_TRF 10.2 ;
FORMAT MR51_WA_FAT_NSI_TRF 9.2 ;
FORMAT MR51_WA_FAT_CUR_PRI_RPR 10.2 ;
FORMAT MR51_WA_FAT_NSI_RPR 9.2 ;
FORMAT MR51_WA_FAT_CUR_PRI_EXT 10.2 ;
FORMAT MR51_WA_FAT_NSI_EXT 9.2 ;
FORMAT MR51_WA_FAT_CUR_PRI_NEW 10.2 ;
FORMAT MR51_WA_FAT_NSI_NEW 9.2 ;
FORMAT MR51_WA_DSB_EFT_1 10.2 ;
FORMAT MR51_WA_DSB_CHK_1 10.2 ;
FORMAT MR51_WN_DSB_EFT_SQ 4. ;
FORMAT MR51_WA_DSB_EFT_SQ 10.2 ;
FORMAT MR51_LF_DOE_SCL_ORG $8. ;
FORMAT MR51_WF_TIR_PCE_LN35 $3. ;
FORMAT MR51_LD_CU_ENT MMDDYY10. ;
FORMAT MR51_WD_CLM_BKR_SBM MMDDYY10. ;
FORMAT MR51_WD_CLM_DTH_SBM MMDDYY10. ;
FORMAT MR51_WD_CLM_DSA_SBM MMDDYY10. ;
FORMAT MR51_WD_CLM_ILG_SBM MMDDYY10. ;
FORMAT MR51_WD_CLM_ISL_DLQ_SBM MMDDYY10. ;
FORMAT MR51_WD_CLM_INT_DLQ_SBM MMDDYY10. ;
FORMAT MR51_WD_CLM_CLS_SBM MMDDYY10. ;
FORMAT MR51_WD_CLM_SKP_SBM MMDDYY10. ;
FORMAT MR51_WD_CLM_CU_SBM MMDDYY10. ;
FORMAT MR51_WD_CLM_SUP_SBM MMDDYY10. ;
FORMAT MR51_WD_CLM_OTH_SBM MMDDYY10. ;
FORMAT MR51_LD_FOR_BEG MMDDYY10. ;
FORMAT MR51_LD_END_GRC_PRD MMDDYY10. ;
FORMAT MR51_WD_DCV_EFF MMDDYY10. ;
FORMAT MR51_WD_ORG_EFT_EFF MMDDYY10. ;
FORMAT MR51_WD_ORG_CHK_EFF MMDDYY10. ;
FORMAT MR51_WD_TRF_EFF MMDDYY10. ;
FORMAT MR51_WD_CVN_EFF MMDDYY10. ;
FORMAT MR51_WD_RPR_EFF MMDDYY10. ;
FORMAT MR51_WD_EXT_ORG_EFF MMDDYY10. ;
FORMAT MR51_WD_NEW_ORG_EFF MMDDYY10. ;
FORMAT MR51_WD_DSB_EFT_1 MMDDYY10. ;
FORMAT MR51_WD_DSB_CHK_1 MMDDYY10. ;
FORMAT MR51_WD_DSB_EFT_SQ MMDDYY10. ;
FORMAT MR51_WD_DSB_CHK_SQ MMDDYY10. ;
FORMAT MR51_WD_DSB_CAN_EFT_1 MMDDYY10. ;
FORMAT MR51_WD_DSB_CAN_CHK_1 MMDDYY10. ;
FORMAT MR51_WD_DSB_CAN_EFT_SQ MMDDYY10. ;
FORMAT MR51_WD_DSB_CAN_CHK_SQ MMDDYY10. ;
FORMAT MR51_WI_PPR_SCL_ORG $1. ;
FORMAT MR52_LD_DSB_CAN MMDDYY10. ;
FORMAT MR52_LA_DSB_CAN 10.2 ;
FORMAT MR52_LA_DSB 10.2 ;
FORMAT MR52_LC_DSB_TYP $1. ;
FORMAT MR52_WX_DSB_TYP $20. ;
FORMAT MR52_LC_DSB_CAN_TYP $1. ;
FORMAT MR52_WX_DSB_CAN_TYP $20. ;
FORMAT MR52_LC_DSB_FEE_1 $2. ;
FORMAT MR52_WX_DSB_FEE_1 $20. ;
FORMAT MR52_LA_DSB_FEE_1 9.2 ;
FORMAT MR52_LC_DSB_FEE_2 $2. ;
FORMAT MR52_WX_DSB_FEE_2 $20. ;
FORMAT MR52_LA_DSB_FEE_2 9.2 ;
FORMAT MR52_LC_DSB_FEE_3 $2. ;
FORMAT MR52_WX_DSB_FEE_3 $20. ;
FORMAT MR52_LA_DSB_FEE_3 9.2 ;
FORMAT MR52_LF_DSB_CHK $10. ;
FORMAT MR52_LC_LDR_DSB_MDM $1. ;
FORMAT MR52_WX_LDR_DSB_MDM $20. ;
FORMAT MR52_WX_POR_RCC_RPT_LN1 $3. ;
FORMAT MR52_WX_POR_RCC_RPT_LN2 $3. ;
FORMAT MR52_AF_DSB_RPT $8. ;
FORMAT MR52_DM_PRS_1 $13. ;
FORMAT MR52_DM_PRS_MID $13. ;
FORMAT MR52_DM_PRS_LST $23. ;
FORMAT MR52_DM_PRS_LST_SFX $4. ;
FORMAT MR52_WF_OWN_RSB_ORG_FEE $8. ;
FORMAT MR52_WF_BND_ISS_RSB_ORG $8. ;
FORMAT MR52_WN_DSB_TOT 4. ;
FORMAT MR52_WN_CAN 4. ;
FORMAT MR52_WN_CAN_TOT 4. ;
FORMAT MR52_WX_POR_RCC_RPT_LN3 $3. ;
FORMAT MR52_WA_DSB_FEE_CAN_1 9.2 ;
FORMAT MR52_WA_DSB_FEE_CAN_2 9.2 ;
FORMAT MR52_WR_ITR_1 7.3 ;
FORMAT MR52_WC_ITR_TYP_1 $2. ;
FORMAT MR52_LF_CUR_POR $20. ;
FORMAT MR52_WC_TYP_SCL_ORG $2. ;
FORMAT MR52_WC_ELG_SIN_1 $1. ;
FORMAT MR52_LC_SCY_PGA_PGM_YR $1. ;
FORMAT MR52_LF_DOE_SCL_ORG $8. ;
FORMAT MR52_WF_TIR_PCE_MR52 $3. ;
FORMAT MR52_WI_DSB_ORG $1. ;
FORMAT MR52_WD_DSB_CAN_REV MMDDYY10. ;
FORMAT MR52_WA_DSB_CAN_REV 10.2 ;
FORMAT MR52_WA_FEE_CAN_REV_1 9.2 ;
FORMAT MR52_WA_FEE_CAN_REV_2 9.2 ;
FORMAT MR52_WN_CAN_REV 4. ;
FORMAT MR52_WN_CAN_REV_TOT 4. ;
FORMAT MR52_WX_POR_RCC_RPT_LN4 $3. ;
FORMAT MR52_LC_DSB_CAN_REA $4. ;
FORMAT MR52_WX_DSB_CAN_REA $20. ;
FORMAT MR52_AF_LON_ALT $17. ;
FORMAT MR52_AN_SEQ_COM_LN_APL 6. ;
DO;
PUT MR51_IF_OWN_PRN $ @;
PUT MR51_BF_SSN $ @;
PUT MR51_LN_SEQ @;
PUT MR51_IF_OWN $ @;
PUT MR51_IF_BND_ISS $ @;
PUT MR51_IC_LON_PGM $ @;
PUT MR51_IF_GTR $ @;
PUT MR51_LF_CUR_POR $ @;
PUT MR51_WC_TYP_SCL_ORG $ @;
PUT MR51_WR_ITR_1 @;
PUT MR51_WC_ITR_TYP_1 $ @;
PUT MR51_LD_LON_1_DSB @;
PUT MR51_WC_ELG_SIN_1 $ @;
PUT MR51_WA_CUR_PRI @;
PUT MR51_WA_CUR_BR_INT @;
PUT MR51_WA_CUR_GOV_INT @;
PUT MR51_WA_CUR_OTH_CHR @;
PUT MR51_WD_SAR_ATY_EFF @;
PUT MR51_WA_AVG_DAY_BAL @;
PUT MR51_WN_SQ_FOR_APL @;
PUT MR51_WX_TYP_REC_CLP_LON $ @;
PUT MR51_WF_SUP_ATY_RPT_LN1 $ @;
PUT MR51_WF_SUP_ATY_RPT_LN2 $ @;
PUT MR51_WF_SUP_ATY_RPT_LN3 $ @;
PUT MR51_WF_SUP_ATY_RPT_LN4 $ @;
PUT MR51_WF_SUP_ATY_RPT_LN5 $ @;
PUT MR51_WF_SUP_ATY_RPT_LN6 $ @;
PUT MR51_WF_SUP_ATY_RPT_LN7 $ @;
PUT MR51_WF_SUP_ATY_RPT_LN8 $ @;
PUT MR51_WF_SUP_ATY_RPT_LN9 $ @;
PUT MR51_WF_SAR_LN10 $ @;
PUT MR51_LC_SCY_PGA_PGM_YR $ @;
PUT MR51_LC_CU_LIA $ @;
PUT MR51_WA_PRI_CU_BEG @;
PUT MR51_WA_BRI_ACR_CU_BEG @;
PUT MR51_WA_CLM_PRI_BKR @;
PUT MR51_WA_CLM_INT_BKR @;
PUT MR51_WA_CLM_PRI_DTH @;
PUT MR51_WA_CLM_INT_DTH @;
PUT MR51_WA_CLM_PRI_DSA @;
PUT MR51_WA_CLM_INT_DSA @;
PUT MR51_WA_CLM_PRI_ILG @;
PUT MR51_WA_CLM_INT_ILG @;
PUT MR51_WA_CLM_PRI_ISL_DLQ @;
PUT MR51_WA_CLM_INT_ISL_DLQ @;
PUT MR51_WA_CLM_PRI_INT_DLQ @;
PUT MR51_WA_CLM_INT_INT_DLQ @;
PUT MR51_WA_CLM_PRI_CLS @;
PUT MR51_WA_CLM_INT_CLS @;
PUT MR51_WA_CLM_PRI_CU @;
PUT MR51_WA_CLM_INT_CU @;
PUT MR51_WA_CLM_PRI_SUP @;
PUT MR51_WA_CLM_INT_SUP @;
PUT MR51_WA_CLM_PRI_SKP @;
PUT MR51_WA_CLM_INT_SKP @;
PUT MR51_WA_CLM_PRI_OTH @;
PUT MR51_WA_CLM_INT_OTH @;
PUT MR51_WA_ORG_CHK @;
PUT MR51_WA_ORG_EFT @;
PUT MR51_WN_DSB_CHK_SQ @;
PUT MR51_WA_DSB_CHK_SQ @;
PUT MR51_WA_DSB_CAN_EFT_1 @;
PUT MR51_WA_DSB_CAN_CHK_1 @;
PUT MR51_WN_DSB_CAN_EFT_SQ @;
PUT MR51_WA_DSB_CAN_EFT_SQ @;
PUT MR51_WN_DSB_CAN_CHK_SQ @;
PUT MR51_WA_DSB_CAN_CHK_SQ @;
PUT MR51_WA_FAT_CUR_PRI_DCV @;
PUT MR51_WA_FAT_NSI_DCV @;
PUT MR51_WA_FAT_CUR_PRI_CVN @;
PUT MR51_WA_FAT_NSI_CVN @;
PUT MR51_WA_FAT_CUR_PRI_TRF @;
PUT MR51_WA_FAT_NSI_TRF @;
PUT MR51_WA_FAT_CUR_PRI_RPR @;
PUT MR51_WA_FAT_NSI_RPR @;
PUT MR51_WA_FAT_CUR_PRI_EXT @;
PUT MR51_WA_FAT_NSI_EXT @;
PUT MR51_WA_FAT_CUR_PRI_NEW @;
PUT MR51_WA_FAT_NSI_NEW @;
PUT MR51_WA_DSB_EFT_1 @;
PUT MR51_WA_DSB_CHK_1 @;
PUT MR51_WN_DSB_EFT_SQ @;
PUT MR51_WA_DSB_EFT_SQ @;
PUT MR51_LF_DOE_SCL_ORG $ @;
PUT MR51_WF_TIR_PCE_LN35 $ @;
PUT MR51_LD_CU_ENT @;
PUT MR51_WD_CLM_BKR_SBM @;
PUT MR51_WD_CLM_DTH_SBM @;
PUT MR51_WD_CLM_DSA_SBM @;
PUT MR51_WD_CLM_ILG_SBM @;
PUT MR51_WD_CLM_ISL_DLQ_SBM @;
PUT MR51_WD_CLM_INT_DLQ_SBM @;
PUT MR51_WD_CLM_CLS_SBM @;
PUT MR51_WD_CLM_SKP_SBM @;
PUT MR51_WD_CLM_CU_SBM @;
PUT MR51_WD_CLM_SUP_SBM @;
PUT MR51_WD_CLM_OTH_SBM @;
PUT MR51_LD_FOR_BEG @;
PUT MR51_LD_END_GRC_PRD @;
PUT MR51_WD_DCV_EFF @;
PUT MR51_WD_ORG_EFT_EFF @;
PUT MR51_WD_ORG_CHK_EFF @;
PUT MR51_WD_TRF_EFF @;
PUT MR51_WD_CVN_EFF @;
PUT MR51_WD_RPR_EFF @;
PUT MR51_WD_EXT_ORG_EFF @;
PUT MR51_WD_NEW_ORG_EFF @;
PUT MR51_WD_DSB_EFT_1 @;
PUT MR51_WD_DSB_CHK_1 @;
PUT MR51_WD_DSB_EFT_SQ @;
PUT MR51_WD_DSB_CHK_SQ @;
PUT MR51_WD_DSB_CAN_EFT_1 @;
PUT MR51_WD_DSB_CAN_CHK_1 @;
PUT MR51_WD_DSB_CAN_EFT_SQ @;
PUT MR51_WD_DSB_CAN_CHK_SQ @;
PUT MR51_WI_PPR_SCL_ORG $ @;
PUT MR52_LD_DSB_CAN @;
PUT MR52_LA_DSB_CAN @;
PUT MR52_LA_DSB @;
PUT MR52_LC_DSB_TYP $ @;
PUT MR52_WX_DSB_TYP $ @;
PUT MR52_LC_DSB_CAN_TYP $ @;
PUT MR52_WX_DSB_CAN_TYP $ @;
PUT MR52_LC_DSB_FEE_1 $ @;
PUT MR52_WX_DSB_FEE_1 $ @;
PUT MR52_LA_DSB_FEE_1 @;
PUT MR52_LC_DSB_FEE_2 $ @;
PUT MR52_WX_DSB_FEE_2 $ @;
PUT MR52_LA_DSB_FEE_2 @;
PUT MR52_LC_DSB_FEE_3 $ @;
PUT MR52_WX_DSB_FEE_3 $ @;
PUT MR52_LA_DSB_FEE_3 @;
PUT MR52_LF_DSB_CHK $ @;
PUT MR52_LC_LDR_DSB_MDM $ @;
PUT MR52_WX_LDR_DSB_MDM $ @;
PUT MR52_WX_POR_RCC_RPT_LN1 $ @;
PUT MR52_WX_POR_RCC_RPT_LN2 $ @;
PUT MR52_AF_DSB_RPT $ @;
PUT MR52_DM_PRS_1 $ @;
PUT MR52_DM_PRS_MID $ @;
PUT MR52_DM_PRS_LST $ @;
PUT MR52_DM_PRS_LST_SFX $ @;
PUT MR52_WF_OWN_RSB_ORG_FEE $ @;
PUT MR52_WF_BND_ISS_RSB_ORG $ @;
PUT MR52_WN_DSB_TOT @;
PUT MR52_WN_CAN @;
PUT MR52_WN_CAN_TOT @;
PUT MR52_WX_POR_RCC_RPT_LN3 $ @;
PUT MR52_WA_DSB_FEE_CAN_1 @;
PUT MR52_WA_DSB_FEE_CAN_2 @;
PUT MR52_WR_ITR_1 @;
PUT MR52_WC_ITR_TYP_1 $ @;
PUT MR52_LF_CUR_POR $ @;
PUT MR52_WC_TYP_SCL_ORG $ @;
PUT MR52_WC_ELG_SIN_1 $ @;
PUT MR52_LC_SCY_PGA_PGM_YR $ @;
PUT MR52_LF_DOE_SCL_ORG $ @;
PUT MR52_WF_TIR_PCE_MR52 $ @;
PUT MR52_WI_DSB_ORG $ @;
PUT MR52_WD_DSB_CAN_REV @;
PUT MR52_WA_DSB_CAN_REV @;
PUT MR52_WA_FEE_CAN_REV_1 @;
PUT MR52_WA_FEE_CAN_REV_2 @;
PUT MR52_WN_CAN_REV @;
PUT MR52_WN_CAN_REV_TOT @;
PUT MR52_WX_POR_RCC_RPT_LN4 $ @;
PUT MR52_LC_DSB_CAN_REA $ @;
PUT MR52_WX_DSB_CAN_REA $ @;
PUT MR52_AF_LON_ALT $ @;
PUT MR52_AN_SEQ_COM_LN_APL ;
END;
RUN;