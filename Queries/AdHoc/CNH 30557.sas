LIBNAME OCT 'T:\October Closed School.xlsx';
LIBNAME NOV 'T:\November Closed School.xlsx';

DATA WORK.XLSOURCE (DROP=OPE_ID);
	SET 
		OCT.'SheetX$'N 
		NOV.'SheetX$'N;
	IF OPE_ID ^= .;
	OPEID = PUT(OPE_ID, X.);
RUN;

LIBNAME OCT CLEAR;
LIBNAME NOV CLEAR;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;

DATA LEGEND.XLSOURCE;
SET XLSOURCE;
RUN;

RSUBMIT LEGEND;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE CLOSED_SCHOOLS AS
		SELECT DISTINCT
			PDXX.DF_SPE_ACC_ID 					AS Account_number  
			,LNXX.LN_SEQ 						AS Loan_Seq 
			,CATX(' ', PDXX.DM_PRS_X, PDXX.DM_PRS_LST)	AS Borrower_name 
			,PDXX.DX_STR_ADR_X 					AS Borrower_street_add_X  
			,PDXX.DX_STR_ADR_X 					AS Borrower_street_add_X  
			,PDXX.DM_CT 						AS Bororwer_City  
			,PDXX.DC_DOM_ST 					AS Borrower_State 
			,PDXX.DF_ZIP_CDE 					AS Borrower_Zip 
			,PDXX.DM_FGN_CNY 					AS Borrower_Foreign_Country  
			,PDXX.DI_VLD_ADR 					AS Borrower_add_validity  
			,LNXX.LF_DOE_SCL_ORG 				AS Original_school_code  
			,SDXX.LF_DOE_SCL_ENR_CUR 			AS Current_school_code  
			,SDXX.LD_SCL_SPR 					AS Separation_Date   
			,SDXX.LC_REA_SCL_SPR 				AS Separation_Reason    
		FROM
			PKUB.LNXX_LON LNXX
			INNER JOIN PKUB.PDXX_PRS_NME PDXX
				ON LNXX.BF_SSN = PDXX.DF_PRS_ID
			INNER JOIN PKUB.PDXX_PRS_ADR PDXX
				ON LNXX.BF_SSN = PDXX.DF_PRS_ID
			INNER JOIN PKUB.SDXX_STU_SPR SDXX
				ON LNXX.BF_SSN = SDXX.LF_STU_SSN
				AND SDXX.LC_STA_STUXX = 'A'
			INNER JOIN XLSOURCE XL
				ON (
						LNXX.LF_DOE_SCL_ORG = XL.OPEID
						OR SDXX.LF_DOE_SCL_ENR_CUR = XL.OPEID
					)
/*		WHERE*/
/*			LNXX.LF_DOE_SCL_ORG in */
/*				(select opeid from xlsource)*/
/*			OR SDXX.LF_DOE_SCL_ENR_CUR in */
/*				(select opeid from xlsource)*/
		;
QUIT;
ENDRSUBMIT;

DATA CLOSED_SCHOOLS; 
	SET LEGEND.CLOSED_SCHOOLS; 
RUN;

PROC EXPORT
		DATA=CLOSED_SCHOOLS
		OUTFILE='T:\SAS\CNH XXXXX.xlsx'
		REPLACE;
RUN;	
