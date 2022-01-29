/*change the name of the data file in the FILENAME DATAF statement*/
/*you may also need to change the RANGE in the PROC IMPORT statement*/
/*NOTE:  'Adjustment' is misspelled in the folder name, just FYI in case it gets fixed and breaks this code*/


FILENAME REPORTF "Q:\CS Loan Servicing\Manual Adjsutment Query\Manual Adjustment Data.xls";
FILENAME DATAF "Q:\CS Loan Servicing\Manual Adjsutment Query\Manual Adjustment Query Sept XXXX.xlsx";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK  ;

PROC IMPORT OUT = LEGEND.MAQD 
            DATAFILE = DATAF
            DBMS = EXCEL REPLACE;
     RANGE="'Account #$'"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

RSUBMIT LEGEND;
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE MAQR AS
		SELECT	
			DW.DF_SPE_ACC_ID,
			DW.LC_ACA_GDE_LEV,
			DW.LC_DL_STU_DEP_STA,
			DW.LI_DL_XED_USB_LMT,
			DW.LR_DL_DSB_REB_PER,
			DW.LF_DL_FIN_AWD_YR,
			DW.LD_DL_STU_ACA_BEG,
			DW.LD_DL_STU_ACA_END,
			DW.LI_DL_BR_ELG_HPA,
			DW.LF_DL_MPN,
			DW.LC_DL_STA_MPN,
			DW.LD_MPN_EXP
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						LNXX.BF_SSN,
						LNXX.LN_SEQ,
						PDXX.DF_SPE_ACC_ID,
						LNXX.LC_ACA_GDE_LEV,
						FSXX.LC_DL_STU_DEP_STA,
						FSXX.LI_DL_XED_USB_LMT,
						FSXX.LR_DL_DSB_REB_PER,
						FSXX.LF_DL_FIN_AWD_YR,
						FSXX.LD_DL_STU_ACA_BEG,
						FSXX.LD_DL_STU_ACA_END,
						FSXX.LI_DL_BR_ELG_HPA,
						FSXX.LF_DL_MPN,
						FSXX.LC_DL_STA_MPN,
						LNXX.LD_MPN_EXP
					FROM
						PKUB.PDXX_PRS_NME PDXX
						JOIN PKUB.LNXX_LON LNXX
							ON PDXX.DF_PRS_ID = LNXX.BF_SSN
						JOIN PKUB.FSXX_DL_LON FSXX
							ON LNXX.BF_SSN = FSXX.BF_SSN
							AND LNXX.LN_SEQ = FSXX.LN_SEQ

					FOR READ ONLY WITH UR
				) DW
			JOIN MAQD
				ON DW.BF_SSN = MAQD.SSN
				AND DW.LN_SEQ = MAQD.LOAN_SEQ
	;

	DISCONNECT FROM DBX;

QUIT;

ENDRSUBMIT;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = LEGEND.MAQR 
            OUTFILE = REPORTF 
            DBMS = EXCEL
			REPLACE;
RUN;
