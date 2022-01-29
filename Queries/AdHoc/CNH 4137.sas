LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
%let DB = DNFPUTDL;  *This is live;
LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						PDXX.DF_SPE_ACC_ID AS ACCOUNT_NUMBER, 
						PDXX.DM_PRS_X || ' ' || PDXX.DM_PRS_LST AS NAME,
						PDXX.DD_BKR_STA AS DISCHARGE_DATE
					FROM
						PKUB.PDXX_PRS_NME PDXX
						INNER JOIN PKUB.PDXX_PRS_BKR PDXX
							ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID 
					WHERE
						PDXX.DA_BKR_DCH IS NOT NULL
						AND PDXX.DD_BKR_STA >= 'XXXX-XX-XX'
					ORDER BY 
						PDXX.DD_BKR_STA

					FOR READ ONLY WITH UR
				);

DISCONNECT FROM DBX;
QUIT;
ENDRSUBMIT;

PROC EXPORT
		DATA=LEGEND.DEMO
		OUTFILE='T:\SAS\NHCS XXXX.XLSX'
		REPLACE;
RUN;
