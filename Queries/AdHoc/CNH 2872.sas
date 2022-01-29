LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;
PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE FORB AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						PDXX.DF_SPE_ACC_ID,
						LNXX.LN_SEQ,
						FBXX.LD_FOR_REQ_END,
						FBXX.LC_FOR_TYP,
						LNXX.LD_DLQ_OCC,
						LNXX.LC_DLQ_TYP,
						GRXX.WN_CUM_MTH_FOR,
						GRXX.WC_STA_GRXX 

					FROM
						PKUB.LNXX_LON LNXX
					LEFT JOIN PKUB.FBXX_BR_FOR_REQ FBXX
						ON FBXX.BF_SSN = LNXX.BF_SSN
					LEFT JOIN PKUB.PDXX_PRS_NME PDXX
						ON PDXX.DF_PRS_ID = LNXX.BF_SSN
					LEFT JOIN PKUB.LNXX_LON_DLQ_HST LNXX
						ON LNXX.BF_SSN = LNXX.BF_SSN
						AND LNXX.LC_DLQ_TYP <> 'I'
					LEFT JOIN PKUB.GRXX_RPT_LON_APL GRXX
						ON GRXX.BF_SSN = LNXX.BF_SSN
						AND GRXX.LN_SEQ = LNXX.LN_SEQ

					WHERE
						LNXX.LD_LON_EFF_ADD  = 'XX/XX/XXXX'
						AND DAYS(FBXX.LD_FOR_REQ_END) BETWEEN DAYS('XX/XX/XXXX') AND DAYS('XX/XX/XXXX')
						AND FBXX.LC_FOR_TYP IN ('XX','XX','XX','XX','XX','XX','XX')


					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;


/*export to Excel spreadsheet*/
PROC EXPORT DATA= LEGEND.FORB 
            OUTFILE= "T:\SAS\Request_X.xls" 
            DBMS=EXCEL REPLACE;
     SHEET="FORB"; 
RUN;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;
PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DELQ AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						PDXX.DF_SPE_ACC_ID,
						LNXX.LN_SEQ,
						FBXX.LD_FOR_REQ_END,
						FBXX.LC_FOR_TYP,
						LNXX.LD_DLQ_OCC,
						GRXX.WN_CUM_MTH_FOR
					FROM
						PKUB.LNXX_LON LNXX
					LEFT JOIN PKUB.FBXX_BR_FOR_REQ FBXX
						ON FBXX.BF_SSN = LNXX.BF_SSN
					LEFT JOIN PKUB.PDXX_PRS_NME PDXX
						ON PDXX.DF_PRS_ID = LNXX.BF_SSN
					LEFT JOIN PKUB.LNXX_LON_DLQ_HST LNXX
						ON LNXX.BF_SSN = LNXX.BF_SSN
						AND LNXX.LC_DLQ_TYP <> 'I'
					LEFT JOIN PKUB.GRXX_RPT_LON_APL GRXX
						ON GRXX.BF_SSN = LNXX.BF_SSN
						AND GRXX.LN_SEQ = LNXX.LN_SEQ

					WHERE
						LNXX.LD_LON_EFF_ADD  = 'XX/XX/XXXX'
						AND DAYS(FBXX.LD_FOR_REQ_END) BETWEEN DAYS('XX/XX/XXXX') AND DAYS('XX/XX/XXXX')
						AND FBXX.LC_FOR_TYP IN ('XX','XX','XX','XX','XX','XX','XX')
						AND DAYS(CURRENT DATE) - DAYS(LNXX.LD_DLQ_OCC) > XX
						AND LNXX.LC_STA_LONXX = 'X'

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

/*export to Excel spreadsheet*/
PROC EXPORT DATA= LEGEND.DELQ 
            OUTFILE= "T:\SAS\Request_X.xls" 
            DBMS=EXCEL REPLACE;
     SHEET="DELQ"; 
RUN;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;
PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE FORBX AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						PDXX.DF_SPE_ACC_ID,
						LNXX.LN_SEQ,
						FBXX.LD_FOR_REQ_END,
						FBXX.LC_FOR_TYP,
						LNXX.LD_DLQ_OCC,
						GRXX.WN_CUM_MTH_FOR

					FROM
						PKUB.LNXX_LON LNXX
					LEFT JOIN PKUB.FBXX_BR_FOR_REQ FBXX
						ON FBXX.BF_SSN = LNXX.BF_SSN
					LEFT JOIN PKUB.PDXX_PRS_NME PDXX
						ON PDXX.DF_PRS_ID = LNXX.BF_SSN
					LEFT JOIN PKUB.LNXX_LON_DLQ_HST LNXX
						ON LNXX.BF_SSN = LNXX.BF_SSN
						AND LNXX.LC_DLQ_TYP <> 'I'
					LEFT JOIN PKUB.GRXX_RPT_LON_APL GRXX
						ON GRXX.BF_SSN = LNXX.BF_SSN
						AND GRXX.LN_SEQ = LNXX.LN_SEQ

					WHERE
						LNXX.LD_LON_EFF_ADD  = 'XX/XX/XXXX'
						AND DAYS(FBXX.LD_FOR_REQ_END) = DAYS('XX/XX/XXXX')
						AND FBXX.LC_FOR_TYP IN ('XX','XX','XX','XX','XX','XX','XX')
						AND DAYS(CURRENT DATE) - DAYS(LNXX.LD_DLQ_OCC) > XX
						AND LNXX.LC_STA_LONXX = 'X'

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
/*export to Excel spreadsheet*/
PROC EXPORT DATA= LEGEND.FORBX 
            OUTFILE= "T:\SAS\Request_X.xls" 
            DBMS=EXCEL REPLACE;
     SHEET="FORBX"; 
RUN;
