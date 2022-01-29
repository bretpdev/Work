* reads external Excel spreadsheet using Dynamic Data Exchange (DDE) ;
OPTIONS NOXSYNC NOXWAIT;
X '"T:\Populations.xlsx"';
FILENAME POPS DDE 'Excel|T:\[Populations.xlsx]Total Pop, De-Dup!RXCX:RXXXXCX';
DATA Borrowers;
	INFILE POPS;
	LENGTH SSN $X;
	INPUT SSN;
RUN;

%LET RPTLIB = T:\SAS;
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;

*Send data to Legend;
DATA LEGEND.Borrowers; 
SET Borrowers;
RUN;

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
CREATE TABLE LoanDetail AS
	SELECT DISTINCT
		PDXX.DM_PRS_X 			AS FIRST_NAME
		,PDXX.DM_PRS_LST 		AS LAST_NAME
		,LNXX.BF_SSN 			AS SSN
		,LNXX.LN_SEQ 			AS LOAN
		,FSXX.LF_FED_AWD 		AS AWARD_ID
		,FSXX.LN_FED_AWD_SEQ 	AS AWARD_ID_SEQ
		,DWXX.WC_DW_LON_STA 	AS STATUS
		,LNXX.LA_CUR_PRI 		AS CURR_PRINCIPAL
		,DWXX.WA_TOT_BRI_OTS	AS CURR_INTEREST
		,LNXX.LD_CRT_LONXX		AS REDISCLOSURED
		,LNXX.LA_ACR_INT_RPD 	AS INTEREST_AT_REDISCLOSURE
	FROM
		WORK.Borrowers BORR
		INNER JOIN PKUB.LNXX_LON LNXX
			ON LNXX.BF_SSN = BORR.SSN
		INNER JOIN PKUB.PDXX_PRS_NME PDXX
			ON LNXX.BF_SSN = PDXX.DF_PRS_ID	
		INNER JOIN PKUB.FSXX_DL_LON	FSXX
			ON LNXX.BF_SSN = FSXX.BF_SSN
			AND LNXX.LN_SEQ = FSXX.LN_SEQ 
		INNER JOIN PKUB.DWXX_DW_CLC_CLU DWXX
			ON LNXX.BF_SSN = DWXX.BF_SSN
			AND LNXX.LN_SEQ = DWXX.LN_SEQ
		INNER JOIN PKUB.LNXX_LON_RPS LNXX
			ON LNXX.BF_SSN = LNXX.BF_SSN
			AND LNXX.LN_SEQ = LNXX.LN_SEQ
/*		INNER JOIN */
/*			(*/
/*              SELECT*/
/*                LNXX.BF_SSN,*/
/*				LNXX.LN_SEQ,*/
/*				LNXX.LN_RPS_SEQ,*/
/*                LNXX.LD_CRT_LONXX,*/
/*                LNXX.LC_STA_LONXX,*/
/*                LNXX.LC_TYP_SCH_DIS,*/
/*                ROW_NUMBER() OVER (PARTITION BY LNXX.BF_SSN ORDER BY LNXX.LD_CRT_LONXX) AS DateOrder*/
/*     	     FROM*/
/*                PKUB.LNXX_LON_RPS LNXX*/
/*			)PKUB.LNXX_LON_RPS LNXX*/
/*				ON LNXX.BF_SSN = LNXX.BF_SSN*/
/*				AND LNXX.LN_SEQ = LNXX.LN_SEQ*/

;
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  

QUIT;

ENDRSUBMIT;

DATA LoanDetail;
	SET LEGEND.LoanDetail;
RUN;

PROC EXPORT
	DATA=LoanDetail
	OUTFILE="&RPTLIB\NH XXXX.xlsx"
	DBMS = EXCEL
	REPLACE;
	SHEET = 'FSA';
RUN;
