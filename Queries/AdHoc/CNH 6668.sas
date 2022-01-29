LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE EML AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						PDXX.DF_SPE_ACC_ID,
						PDXX.DM_PRS_X,
						PDXX.DM_PRS_LST,
						PDXX.DX_ADR_EML
					FROM
						PKUB.PDXX_PRS_NME PDXX
						JOIN PKUB.LNXX_LON LNXX
							ON PDXX.DF_PRS_ID = LNXX.BF_SSN
							AND LNXX.LA_CUR_PRI > X
							AND LNXX.LC_STA_LONXX = 'R'
						JOIN PKUB.PDXX_PRS_ADR_EML PDXX
							ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
							AND PDXX.DI_VLD_ADR_EML = 'Y'
							AND PDXX.DC_STA_PDXX = 'A'
						JOIN PKUB.DWXX_DW_CLC_CLU DWXX
							ON PDXX.DF_PRS_ID = DWXX.BF_SSN
							AND DWXX.WC_DW_LON_STA NOT IN ('XX', 'XX', 'XX', 'XX', 'XX', 'XX', 'XX', 'XX')
					ORDER BY
						DF_SPE_ACC_ID

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

QUIT;

ENDRSUBMIT;

DATA EML; SET LEGEND.EML; RUN;

DATA EMLX;
	SET EML;

	BRW_NAME = CATX(' ',DM_PRS_X,DM_PRS_LST);
RUN;

/*write to comma delimited file for the Email Batch Script - FED script*/
DATA _NULL_;
	SET		WORK.EMLX;
	FILE	'T:\NH XXXX.TXT' delimiter=',' DSD DROPOVER lrecl=XXXXX;

	/* write column names, remove this to create a file without a header row */
	IF _N_ = X THEN
		DO;
			PUT "ACCOUNT_NUMBER,BORROWER_NAME,EMAIL_ADDRESS";
		END;

	DO;
	   PUT DF_SPE_ACC_ID @;
	   PUT BRW_NAME @;
	   PUT DX_ADR_EML $ ;
	END;
RUN;
