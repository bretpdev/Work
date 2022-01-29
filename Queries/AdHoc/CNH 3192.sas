%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE RX AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						LNXX.LC_FED_PGM_YR,
						LNXX.LF_FED_CLC_RSK,
						'IRB_AMT                       ' AS TYPE,
						TO_CHAR(SUM(LNXX.LA_FAT_NSI)) AS AMOUNT
					FROM
						PKUB.LNXX_LON LNXX
					INNER JOIN PKUB.LNXX_FIN_ATY LNXX
						ON LNXX.BF_SSN = LNXX.BF_SSN
						AND LNXX.LN_SEQ = LNXX.LN_SEQ
					WHERE
						LNXX.PC_FAT_TYP = 'XX'
						AND LNXX.PC_FAT_SUB_TYP = 'XX'
						AND LNXX.LC_FAT_REV_REA = ' '
						AND LNXX.LC_STA_LONXX = 'A'
						AND DAYS(LNXX.LD_FAT_APL) = DAYS('XX/XX/XXXX')
						AND DAYS(LNXX.LD_FAT_EFF) = DAYS('XX/XX/XXXX') 
					GROUP BY 
						LNXX.LC_FED_PGM_YR,
						LNXX.LF_FED_CLC_RSK

					UNION ALL

					SELECT DISTINCT
						LNXX.LC_FED_PGM_YR,
						LNXX.LF_FED_CLC_RSK,
						'PBO_AMT                       ' AS TYPE,
						TO_CHAR(SUM(LNXX.LA_FAT_CUR_PRI)) AS AMOUNT
					FROM
						PKUB.LNXX_LON LNXX
					INNER JOIN PKUB.LNXX_FIN_ATY LNXX
						ON LNXX.BF_SSN = LNXX.BF_SSN
						AND LNXX.LN_SEQ = LNXX.LN_SEQ
					WHERE
						LNXX.PC_FAT_TYP = 'XX'
						AND LNXX.PC_FAT_SUB_TYP = 'XX'
						AND LNXX.LC_FAT_REV_REA = ' '
						AND LNXX.LC_STA_LONXX = 'A'
						AND DAYS(LNXX.LD_FAT_APL) = DAYS('XX/XX/XXXX')
						AND DAYS(LNXX.LD_FAT_EFF) = DAYS('XX/XX/XXXX') 
					GROUP BY 
						LNXX.LC_FED_PGM_YR,
						LNXX.LF_FED_CLC_RSK
					ORDER BY 
						LC_FED_PGM_YR,
						TYPE,
						LF_FED_CLC_RSK





					FOR READ ONLY WITH UR
				)
	;
QUIT;

PROC SQL;
CONNECT TO DBX (DATABASE=&DB);
	CREATE TABLE RX AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						LNXX.BF_SSN,
						LNXX.LC_FED_PGM_YR,
						FSXX.LF_FED_AWD  AS LOAN_ID, 
						FSXX.LN_FED_AWD_SEQ AS LOAN_ID_SEQ,
						LNXX.LF_FED_CLC_RSK,
						CASE
							WHEN LNXX.IC_LON_PGM = 'DLSTFD' THEN 'DX'
							WHEN LNXX.IC_LON_PGM = 'DLUNST' THEN 'DX'
							WHEN LNXX.IC_LON_PGM = 'DLPLGB' THEN 'DX'
							WHEN LNXX.IC_LON_PGM = 'DLPLUS' THEN 'DX'
							WHEN LNXX.IC_LON_PGM IN ('DLUCNS', 'DLUSPL') THEN 'DX'
							WHEN LNXX.IC_LON_PGM IN ('DLSCNS', 'DLSSPL') THEN 'DX'
							WHEN LNXX.IC_LON_PGM = 'DLPCNS' THEN 'DX'
							WHEN LNXX.IC_LON_PGM = 'PLUS' 	THEN 'PL'
							WHEN LNXX.IC_LON_PGM = 'PLUSGB' THEN 'GB'
							WHEN LNXX.IC_LON_PGM = 'SLS' 	THEN 'SL'
							WHEN LNXX.IC_LON_PGM = 'STFFRD' THEN 'SF'
							WHEN LNXX.IC_LON_PGM = 'UNSTRD' THEN 'SU'
							WHEN LNXX.IC_LON_PGM = 'FISL' 	THEN 'FI'
							WHEN LNXX.IC_LON_PGM IN ('CNSLDN', 'UNCNS', 'SUBCNS', 'SUBSPC', 'UNSPC') THEN 'CL'
						END AS IC_LON_PGM,
						'PBO_AMT                       ' AS TYPE,
						SUM(LNXX.LA_FAT_CUR_PRI) AS AMOUNT,
						LNXX.LD_LON_X_DSB
					FROM
						PKUB.LNXX_LON LNXX
					INNER JOIN PKUB.LNXX_FIN_ATY LNXX
						ON LNXX.BF_SSN = LNXX.BF_SSN
						AND LNXX.LN_SEQ = LNXX.LN_SEQ
					INNER JOIN PKUB.FSXX_DL_LON FSXX
						ON FSXX.BF_SSN = LNXX.BF_SSN
						AND FSXX.LN_SEQ = LNXX.LN_SEQ
					WHERE
						LNXX.PC_FAT_TYP = 'XX'
						AND LNXX.PC_FAT_SUB_TYP = 'XX'
						AND LNXX.LC_FAT_REV_REA = ' '
						AND LNXX.LC_STA_LONXX = 'A'
						AND DAYS(LNXX.LD_FAT_APL) = DAYS('XX/XX/XXXX')
						AND DAYS(LNXX.LD_FAT_EFF) = DAYS('XX/XX/XXXX') 
					GROUP BY 
						LNXX.BF_SSN,
						LNXX.IC_LON_PGM,
						LNXX.LF_FED_CLC_RSK,
						LNXX.LC_FED_PGM_YR,
						FSXX.LF_FED_AWD,
						FSXX.LN_FED_AWD_SEQ,
						LNXX.LD_LON_X_DSB

				UNION ALL

					SELECT DISTINCT
						LNXX.BF_SSN,
						LNXX.LC_FED_PGM_YR,
						FSXX.LF_FED_AWD  AS LOAN_ID, 
						FSXX.LN_FED_AWD_SEQ AS LOAN_ID_SEQ,
						LNXX.LF_FED_CLC_RSK,
						CASE
							WHEN LNXX.IC_LON_PGM = 'DLSTFD' THEN 'DX'
							WHEN LNXX.IC_LON_PGM = 'DLUNST' THEN 'DX'
							WHEN LNXX.IC_LON_PGM = 'DLPLGB' THEN 'DX'
							WHEN LNXX.IC_LON_PGM = 'DLPLUS' THEN 'DX'
							WHEN LNXX.IC_LON_PGM IN ('DLUCNS', 'DLUSPL') THEN 'DX'
							WHEN LNXX.IC_LON_PGM IN ('DLSCNS', 'DLSSPL') THEN 'DX'
							WHEN LNXX.IC_LON_PGM = 'DLPCNS' THEN 'DX'
							WHEN LNXX.IC_LON_PGM = 'PLUS' 	THEN 'PL'
							WHEN LNXX.IC_LON_PGM = 'PLUSGB' THEN 'GB'
							WHEN LNXX.IC_LON_PGM = 'SLS' 	THEN 'SL'
							WHEN LNXX.IC_LON_PGM = 'STFFRD' THEN 'SF'
							WHEN LNXX.IC_LON_PGM = 'UNSTRD' THEN 'SU'
							WHEN LNXX.IC_LON_PGM = 'FISL' 	THEN 'FI'
							WHEN LNXX.IC_LON_PGM IN ('CNSLDN', 'UNCNS', 'SUBCNS', 'SUBSPC', 'UNSPC') THEN 'CL'
						END AS IC_LON_PGM,
						'IRB_AMT                       ' AS TYPE,
						SUM(LNXX.LA_FAT_NSI) AS AMOUNT,
						LNXX.LD_LON_X_DSB
					FROM
						PKUB.LNXX_LON LNXX
					INNER JOIN PKUB.LNXX_FIN_ATY LNXX
						ON LNXX.BF_SSN = LNXX.BF_SSN
						AND LNXX.LN_SEQ = LNXX.LN_SEQ
					INNER JOIN PKUB.FSXX_DL_LON FSXX
						ON FSXX.BF_SSN = LNXX.BF_SSN
						AND FSXX.LN_SEQ = LNXX.LN_SEQ
					WHERE
						LNXX.PC_FAT_TYP = 'XX'
						AND LNXX.PC_FAT_SUB_TYP = 'XX'
						AND LNXX.LC_FAT_REV_REA = ' '
						AND LNXX.LC_STA_LONXX = 'A'
						AND DAYS(LNXX.LD_FAT_APL) = DAYS('XX/XX/XXXX')
						AND DAYS(LNXX.LD_FAT_EFF) = DAYS('XX/XX/XXXX') 
					GROUP BY 
						LNXX.BF_SSN,
						LNXX.IC_LON_PGM,
						LNXX.LF_FED_CLC_RSK,
						LNXX.LC_FED_PGM_YR,
						FSXX.LF_FED_AWD,
						FSXX.LN_FED_AWD_SEQ,
						LNXX.LD_LON_X_DSB
					ORDER BY 
						BF_SSN,
						LOAN_ID, 
						LOAN_ID_SEQ,
						TYPE
					ASC


					FOR READ ONLY WITH UR
				)
	;
QUIT;

PROC SQL;
CONNECT TO DBX (DATABASE=&DB);
	CREATE TABLE RX AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						LNXX.BF_SSN,
						LNXX.LC_FED_PGM_YR,
						FSXX.LF_FED_AWD  AS LOAN_ID, 
						FSXX.LN_FED_AWD_SEQ AS LOAN_ID_SEQ,
						LNXX.LF_FED_CLC_RSK,
						CASE
							WHEN LNXX.IC_LON_PGM = 'DLSTFD' THEN 'STFFRD'
							WHEN LNXX.IC_LON_PGM = 'DLUNST' THEN 'UNSTFD'
							WHEN LNXX.IC_LON_PGM = 'DLPLGB' THEN 'PLUSGB'
							WHEN LNXX.IC_LON_PGM = 'DLPLUS' THEN 'PLUS'
							WHEN LNXX.IC_LON_PGM IN ('DLUCNS', 'DLUSPL', 'DLPCNS')  THEN 'UNCNS'
							WHEN LNXX.IC_LON_PGM IN ('DLSCNS', 'DLSSPL') THEN 'SUBCNS'
							WHEN LNXX.IC_LON_PGM = 'PLUS' 	THEN 'PL'
							WHEN LNXX.IC_LON_PGM = 'PLUSGB' THEN 'GB'
							WHEN LNXX.IC_LON_PGM = 'SLS' 	THEN 'SL'
							WHEN LNXX.IC_LON_PGM = 'STFFRD' THEN 'SF'
							WHEN LNXX.IC_LON_PGM = 'UNSTRD' THEN 'SU'
							WHEN LNXX.IC_LON_PGM = 'FISL' 	THEN 'FI'
							WHEN LNXX.IC_LON_PGM IN ('CNSLDN', 'UNCNS', 'SUBCNS', 'SUBSPC', 'UNSPC') THEN 'CL'
							ELSE 'FIX' || LNXX.IC_LON_PGM
						END AS IC_LON_PGM,
						LNXX.LC_FED_PGM_YR,
						COALESCE(LNXX.LF_STU_SSN,LNXX.BF_SSN) AS STUDENTS_SSN,
						'PBO_AMT                       ' AS TYPE,
						LNXX.LA_FAT_CUR_PRI AS PBO,
						LNXX.LA_FAT_NSI AS IRB,
						LNXX.LN_SEQ
					FROM
						PKUB.LNXX_LON LNXX
					INNER JOIN PKUB.LNXX_FIN_ATY LNXX
						ON LNXX.BF_SSN = LNXX.BF_SSN
						AND LNXX.LN_SEQ = LNXX.LN_SEQ
					INNER JOIN PKUB.FSXX_DL_LON FSXX
						ON FSXX.BF_SSN = LNXX.BF_SSN
						AND FSXX.LN_SEQ = LNXX.LN_SEQ
					WHERE
						LNXX.PC_FAT_TYP = 'XX'
						AND LNXX.PC_FAT_SUB_TYP = 'XX'
						AND LNXX.LC_FAT_REV_REA = ' '
						AND LNXX.LC_STA_LONXX = 'A'
						AND DAYS(LNXX.LD_FAT_APL) = DAYS('XX/XX/XXXX')
						AND DAYS(LNXX.LD_FAT_EFF) = DAYS('XX/XX/XXXX') 

				UNION ALL
					
					SELECT DISTINCT
							LNXX.BF_SSN,
							LNXX.LC_FED_PGM_YR,
							FSXX.LF_FED_AWD  AS LOAN_ID, 
							FSXX.LN_FED_AWD_SEQ AS LOAN_ID_SEQ, /*ADD THIS JOIN*/
							LNXX.LF_FED_CLC_RSK,
							CASE
								WHEN LNXX.IC_LON_PGM = 'DLSTFD' THEN 'STFFRD'
								WHEN LNXX.IC_LON_PGM = 'DLUNST' THEN 'UNSTFD'
								WHEN LNXX.IC_LON_PGM = 'DLPLGB' THEN 'PLUSGB'
								WHEN LNXX.IC_LON_PGM = 'DLPLUS' THEN 'PLUS'
								WHEN LNXX.IC_LON_PGM IN ('DLUCNS', 'DLUSPL', 'DLPCNS')  THEN 'UNCNS'
								WHEN LNXX.IC_LON_PGM IN ('DLSCNS', 'DLSSPL') THEN 'SUBCNS'
								WHEN LNXX.IC_LON_PGM = 'PLUS' 	THEN 'PL'
								WHEN LNXX.IC_LON_PGM = 'PLUSGB' THEN 'GB'
								WHEN LNXX.IC_LON_PGM = 'SLS' 	THEN 'SL'
								WHEN LNXX.IC_LON_PGM = 'STFFRD' THEN 'SF'
								WHEN LNXX.IC_LON_PGM = 'UNSTRD' THEN 'SU'
								WHEN LNXX.IC_LON_PGM = 'FISL' 	THEN 'FI'
								WHEN LNXX.IC_LON_PGM IN ('CNSLDN', 'UNCNS', 'SUBCNS', 'SUBSPC', 'UNSPC') THEN 'CL'
								ELSE 'FIX' || LNXX.IC_LON_PGM
							END AS IC_LON_PGM,
							LNXX.LC_FED_PGM_YR,
							COALESCE(LNXX.LF_STU_SSN,LNXX.BF_SSN) AS STUDENTS_SSN,  /*Coaleces to bwr ssn*/
							'IRB_AMT                       ' AS TYPE,
							LNXX.LA_FAT_CUR_PRI AS PBO,
							LNXX.LA_FAT_NSI AS IRB,
							LNXX.LN_SEQ
						FROM
							PKUB.LNXX_LON LNXX
						INNER JOIN PKUB.LNXX_FIN_ATY LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
						INNER JOIN PKUB.FSXX_DL_LON FSXX
							ON FSXX.BF_SSN = LNXX.BF_SSN
							AND FSXX.LN_SEQ = LNXX.LN_SEQ
						WHERE
							LNXX.PC_FAT_TYP = 'XX'
							AND LNXX.PC_FAT_SUB_TYP = 'XX'
							AND LNXX.LC_FAT_REV_REA = ' '
							AND LNXX.LC_STA_LONXX = 'A'
							AND DAYS(LNXX.LD_FAT_APL) = DAYS('XX/XX/XXXX')
							AND DAYS(LNXX.LD_FAT_EFF) = DAYS('XX/XX/XXXX') 
						ORDER BY 
							BF_SSN,
							LOAN_ID, 
							LOAN_ID_SEQ

					FOR READ ONLY WITH UR
				)
	;
	DISCONNECT FROM DBX;
QUIT;

ENDRSUBMIT;

DATA RX;
SET LEGEND.RX;
TRANS_ID = _N_;
RUN;
DATA RX;
SET LEGEND.RX;
RUN;
DATA RX;
SET LEGEND.RX;
RUN;

PROC SQL;
CREATE TABLE RX_FINAL AS
	SELECT
		RX_DATA.*,
		RX_DATA.TRANS_ID
	FROM
		RX RX_DATA
	INNER JOIN RX RX_DATA
		ON RX_DATA.TYPE = RX_DATA.TYPE
		AND RX_DATA.LC_FED_PGM_YR = RX_DATA.LC_FED_PGM_YR
		AND RX_DATA.LF_FED_CLC_RSK = RX_DATA.LF_FED_CLC_RSK
		;
QUIT;

PROC SQL;
CREATE TABLE RX_FINAL AS
	SELECT
		RX_DATA.*,
		RX_DATA.TRANS_ID
	FROM
		RX RX_DATA
	INNER JOIN RX RX_DATA
		ON RX_DATA.TYPE = RX_DATA.TYPE
		AND RX_DATA.LC_FED_PGM_YR = RX_DATA.LC_FED_PGM_YR
		AND RX_DATA.LF_FED_CLC_RSK = RX_DATA.LF_FED_CLC_RSK
		;
QUIT;

PROC EXPORT DATA = WORK.RX 
            OUTFILE = "T:\SAS\GL SUMMARY.CSV" 
            DBMS = CSV 
			REPLACE;
     PUTNAMES = YES;
RUN;

PROC EXPORT DATA = WORK.RX_FINAL 
            OUTFILE = "T:\SAS\PBC DOWNLOAD.CSV" 
            DBMS = CSV 
			REPLACE;
     PUTNAMES = YES;
RUN;

DATA RX_FINAL;
SET RX_FINAL;
FORMAT LD_LON_X_DSB MMDDYYXX.;
RUN;

PROC EXPORT DATA = WORK.RX_FINAL 
            OUTFILE = "T:\SAS\TXO FILE.CSV" 
            DBMS = CSV 
			REPLACE;
     PUTNAMES = YES;
RUN;

PROC EXPORT DATA = WORK.RX 
            OUTFILE = "T:\SAS\GL SUMMARY.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;

PROC EXPORT DATA = WORK.RX_FINAL 
            OUTFILE = "T:\SAS\TXO FILE.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;

PROC EXPORT DATA = WORK.RX_FINAL 
            OUTFILE = "T:\SAS\PBC DOWNLOAD.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;




/*DATA _NULL_;*/
/*SET RX;*/
/**/
/*V = ''; /*THIS WILL BE A FILLER CHARACTER*/*/
/*CURDATE = COMPRESS(PUT(TODAY(),YYMMDDXX.),'-');*/
/**/
/*FILE REPORTX DROPOVER LRECL=XXXXX;*/
/**/
/*/*FORMAT*/*/
/*/*CURDATE YYMMDDsXX.*/*/
/*/*;*/*/
/**/
/*IF _N_ = X THEN */
/*	DO;*/
/*		PUT*/
/*			'H     XXXXXX  '*/
/*		PUT CURDATE $@;*/
/*		PUT 'NCGL'*/
/*		PUT CURDATE $@;*/
/*		PUT 'XXXXXX.dat:      XXXX'*/
/*		PUT @XXX V;*/
/*	END;*/
/**/
/*	DO;*/
/*		PUT 'D'*/
/*		PUT LC_FED_PGM_YR $@;*/
/*		PUT 'XXXXXXTRSVOS'*/
/*		PUT CURDATE $@;*/
/*		PUT TYPE $@;*/
/*		PUT AMT $@;*/
/*		PUT @XXX V $@;*/
/*		PUT LF_FED_CLC_RSK $@;*/
/*		PUT 'XXXXND'*/
/**/
/**/
/*		;*/
/*	END;*/
/*RUN;*/
