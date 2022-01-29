LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
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
						LNXX.BF_SSN,
						LNXX.LN_SEQ,
						LNXX.LD_FAT_EFF
					FROM
						PKUB.LNXX_FIN_ATY LNXX
					INNER JOIN PKUB.AYXX_BR_LON_ATY AYXX
						ON AYXX.BF_SSN = LNXX.BF_SSN
					INNER JOIN PKUB.AYXX_ATY_TXT AYXX
						ON AYXX.BF_SSN = AYXX.BF_SSN
						AND AYXX.LN_ATY_SEQ = AYXX.LN_ATY_SEQ
						
					WHERE
						(LNXX.PC_FAT_TYP = 'XX' AND LNXX.PC_FAT_SUB_TYP = 'XX')
						AND DAYS(AYXX.LD_ATY_RSP) > DAYS('XX/XX/XXXX')
						AND AYXX.PF_REQ_ACT = 'AXDCV'
						AND AYXX.LX_ATY LIKE '%PSLF%'

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SheetX"; 
RUN;

