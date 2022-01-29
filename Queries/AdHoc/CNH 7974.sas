/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE X  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @XX " ********************************************************************* "
              / @XX " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @XX " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @XX " ********************************************************************* "
              / @XX " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @XX " ****  &SQLXMSG   **** "
              / @XX " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL ;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						PDXX.DF_PRS_ID AS Borrower_SSN,
						FSXX.LF_FED_AWD AS Award_Id,
						FSXX.LN_FED_AWD_SEQ AS Award_Sequence,
						LNXX.LN_SEQ AS Loan_Sequence,
						LNXX.IC_LON_PGM AS Loan_Type, /*(translated to NSLDS value)*/
						LNXX.LA_CUR_PRI AS Current_Principal_Balance,
						LNXX.LR_ITR AS Current_Fixed_Interest_Rate,
						CASE WHEN LNXX.LD_PIF_RPT IS NULL THEN 'No' ELSE 'Yes' END AS PIF, 
						LNXX.LD_PIF_RPT,
						CONCAT(LNXX.PC_FAT_TYP, LNXX.PC_FAT_SUB_TYP) AS Trans_Type, /* of the last transaction */
						Disburse.LD_DSB AS Last_Disbursement_Date
					FROM
						PKUB.PDXX_PRS_NME PDXX
						INNER JOIN PKUB.LNXX_LON LNXX
							ON LNXX.BF_SSN = PDXX.DF_PRS_ID
						INNER JOIN PKUB.LNXX_INT_RTE_HST LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
						INNER JOIN PKUB.LNXX_DSB LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
						INNER JOIN PKUB.FSXX_DL_LON FSXX
							ON FSXX.BF_SSN = LNXX.BF_SSN
							AND FSXX.LN_SEQ = LNXX.LN_SEQ
						INNER JOIN /*Last LNXX transaction */
							(
								SELECT
									LNXXInner.BF_SSN,
									LNXXInner.LN_SEQ,
									LNXXInner.PC_FAT_TYP,
									LNXXInner.PC_FAT_SUB_TYP
								FROM
									PKUB.LNXX_FIN_ATY LNXXInner
								WHERE
									LNXXInner.LD_FAT_APL = (SELECT MAX(LNXXMax.LD_FAT_APL) FROM PKUB.LNXX_FIN_ATY LNXXMax WHERE LNXXMax.BF_SSN = LNXXInner.BF_SSN AND LNXXMax.LN_SEQ = LNXXInner.LN_SEQ)
							) LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
						INNER JOIN /*Get the latest LNXX disbursment date*/
							(
								SELECT
									LNXXInner.BF_SSN,
									LNXXInner.LN_SEQ,
									LNXXInner.LD_DSB
								FROM
									PKUB.LNXX_DSB LNXXInner
								WHERE
									LNXXInner.LD_DSB = (SELECT MAX(LNXXMax.LD_DSB) FROM PKUB.LNXX_DSB LNXXMax WHERE LNXXMax.BF_SSN = LNXXInner.BF_SSN AND LNXXMax.LN_SEQ = LNXXInner.LN_SEQ)
							) Disburse
							ON Disburse.BF_SSN = LNXX.BF_SSN
							AND Disburse.LN_SEQ = LNXX.LN_SEQ
					WHERE
						LNXX.LD_DSB BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
						AND LNXX.LC_ITR_TYP = 'FX'

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SHEETX"; 
RUN;

