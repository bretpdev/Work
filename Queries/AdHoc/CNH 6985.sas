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
						WQXX.WF_QUE,
						WQXX.WF_SUB_QUE,
						WQXX.WN_CTL_TSK,
						WQXX.PF_REQ_ACT,
						PDXX.DF_SPE_ACC_ID,
						(PDXX.DM_PRS_X || ' ' || PDXX.DM_PRS_LST) AS NAME,
						LNXX.LN_SEQ,
						LNXX.LC_STA_LONXX
					FROM
						PKUB.WQXX_TSK_QUE WQXX
					INNER JOIN PKUB.PDXX_PRS_NME PDXX
						ON PDXX.DF_PRS_ID = WQXX.BF_SSN
					INNER JOIN PKUB.LNXX_LON_ATY LNXX
						ON LNXX.BF_SSN = WQXX.BF_SSN
						AND LNXX.LN_ATY_SEQ = WQXX.LN_ATY_SEQ
					INNER JOIN	PKUB.LNXX_LON LNXX
						ON LNXX.BF_SSN = WQXX.BF_SSN
						AND LNXX.LN_SEQ = LNXX.LN_SEQ
						AND LNXX.LA_CUR_PRI <= X
					WHERE
						WQXX.WF_QUE = 'FX'
						AND WQXX.WF_SUB_QUE = 'AX'
						

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

DATA ONE (DROP=WF_QUE WF_SUB_QUE WN_CTL_TSK PF_REQ_ACT);
	SET DEMO;
RUN;

DATA TWO (DROP=DF_SPE_ACC_ID NAME);
SET DEMO;
RUN;

PROC EXPORT DATA = WORK.ONE 
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="OUTPUTX"; 
RUN;

PROC EXPORT DATA = WORK.TWO 
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="OUTPUTX"; 
RUN;
