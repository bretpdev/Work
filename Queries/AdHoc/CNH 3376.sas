LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

PROC SQL inobs = XXXX;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						LNXX.BF_SSN,
						PDXX.DD_BRT,
						DWXX.WC_DW_LON_STA,
						WBXX.DN_USR_ID,
						WBXX.DX_USR_PSW,
						WBXX.DX_USR_EML,
						WBXX.DX_USR_QST_X,
						WBXX.DX_USR_ANS_X,
						WBXX.DX_USR_QST_X,
						WBXX.DX_USR_ANS_X
					FROM
						PKUB.PDXX_PRS_NME PDXX
						JOIN PKUB.LNXX_LON LNXX
							ON PDXX.DF_PRS_ID = LNXX.BF_SSN
						JOIN PKUB.DWXX_DW_CLC_CLU DWXX
							ON LNXX.BF_SSN = DWXX.BF_SSN
							AND LNXX.LN_SEQ = DWXX.LN_SEQ
						LEFT JOIN WEBFLSX.WBXX_SIT_ACS_CTL WBXX
							ON LNXX.BF_SSN = WBXX.DN_SSN
					WHERE
						LNXX.LA_CUR_PRI > X
						AND LNXX.LC_STA_LONXX = 'R'
						AND DWXX.WC_DW_LON_STA IN ('XX','XX')

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NH XXXX.xls" 
            DBMS = EXCEL
			REPLACE;
RUN;
