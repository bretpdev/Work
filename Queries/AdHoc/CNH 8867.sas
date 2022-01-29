%LET RPTLIB = T:\SAS;
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;

%LET DB = DNFPUTDL; /*live*/
/*%LET DB = DLGSWQUT; /*test*/
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

PROC SQL;
CONNECT TO DBX (DATABASE=&DB); 
CREATE TABLE BORROWER AS
	SELECT *
	FROM CONNECTION TO DBX 
		(
		SELECT DISTINCT
			PDXX.DF_SPE_ACC_ID	AS Account_Number 
			,PDXX.DM_PRS_X		AS First_Name
			,PDXX.DM_PRS_LST	AS Last_Name 
			,PDXX.DX_STR_ADR_X	AS Street_X
			,PDXX.DX_STR_ADR_X	AS Street_X
			,PDXX.DM_CT			AS City 
			,PDXX.DC_DOM_ST		AS State
			,PDXX.DF_ZIP_CDE	AS Zip 
			,PDXX.DM_FGN_CNY	AS Foreign_Country
			,PDXX.DM_FGN_ST		AS Foreign_State
			,PDXX.DX_ADR_EML	AS Email
			,PDXX.DI_VLD_ADR_EML AS Email_Validity
			,LNXX.LN_DLQ_MAX 	AS Days_Delinquent
		FROM 
			PKUB.LNXX_LON LNXX
			INNER JOIN PKUB.PDXX_PRS_NME PDXX
				ON LNXX.BF_SSN = PDXX.DF_PRS_ID
			INNER JOIN PKUB.PDXX_PRS_ADR PDXX
				ON LNXX.BF_SSN = PDXX.DF_PRS_ID
			INNER JOIN PKUB.PDXX_PRS_ADR_EML PDXX
				ON LNXX.BF_SSN = PDXX.DF_PRS_ID
			INNER JOIN PKUB.DWXX_DW_CLC_CLU DWXX
				ON LNXX.BF_SSN = DWXX.BF_SSN
				AND LNXX.LN_SEQ = DWXX.LN_SEQ
			INNER JOIN PKUB.LNXX_LON_DLQ_HST LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
		WHERE 
			LNXX.IC_LON_PGM IN ('DLPCNS', 'DLPLUS', 'DLSCPL', 'DPLUS')
			AND LNXX.LC_STA_LONXX = 'R'
			AND DAYS(LNXX.LD_LON_X_DSB) > DAYS('XX-XX-XXXX')
			AND DWXX.WC_DW_LON_STA = 'XX'
			AND	PDXX.DI_VLD_ADR = 'Y' 
			AND PDXX.DC_ADR = 'L'
			AND LNXX.LN_DLQ_MAX ^= X
			AND LNXX.LC_STA_LONXX = 'X'
/*			AND DAYS(LNXX.LD_STA_LONXX) = DAYS(CURRENT DATE) - X*/
		)
;
DISCONNECT FROM DBX;
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  

QUIT;

ENDRSUBMIT;

DATA BORROWER;
	SET LEGEND.BORROWER;
RUN;

PROC EXPORT
		DATA=BORROWER
		OUTFILE="&RPTLIB\CNH XXXX.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="PLUS";
RUN;
