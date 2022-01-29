%macro truncat(r);
	retain trunc X;
	if df_spe_acc_id = '-Begin-' then do;
		if trunc = X then do;
			trunc = X;
			delete;
		end;
		else do;
				call symput('ERROR',"&r");
		end;
		delete;
	end;
	else if df_spe_acc_id = '-End-' then do;
		if trunc = X then trunc = X;
		else do;
				call symput('ERROR',"&r");
		end;
		delete;
	end;
if eof and df_spe_acc_id ^= '-End-' then do;
		call symput('ERROR',"&r");
end;
%MEND;


DATA LTXX(drop=trunc);
	INFILE "C:\LTXX\*.*" DSD DLM = ',' FIRSTOBS=X MISSOVER end = eof LRECL = XXXXX;

	INPUT 
		DF_SPE_ACC_ID :$XX.
		RT_RUN_SRT_PRC :$XX.
		RN_SEQ_LTR_CRT_PRC :XX.
		RN_SEQ_REC_PRC :X.
		RT_RUN_SRT_DTS_PRC :DATETIMEXX.X
		RM_DSC_LTR_PRC :$XX.
		RC_TYP_SBJ_PRC :$X.
		RF_SBJ_PRC :$X.
		RN_ENT_REQ_PRC  :X. 
		RN_ATY_SEQ_PRC :XX.
		RI_REC_PRC :$X.
		RX_REQ_ARA_X_PRC :$XXX.
		RI_LTR_REQ_DEL_PRC :$X.
		RC_LTR_REQ_SRC_PRC :$X.
		RI_PRV_RUN_ERR_PRC :$X.
		RF_COR_DOC_PRC :$XX.
		RI_LTR_OPT_ENC_PRC :$X.
	;
	%TRUNCAT(XX);

RUN;

                  

LIBNAME DB ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CDW.dsn; update_lock_typ=nolock; bl_keepnulls=no";


PROC SQL;
	CREATE TABLE DATA AS
		SELECT
			L.*
		FROM
			LTXX L 
		LEFT JOIN DB.LTXX_LetterRequests CDW
			ON L.DF_SPE_ACC_ID = CDW.DF_SPE_ACC_ID
			AND L.RT_RUN_SRT_PRC = CDW.RT_RUN_SRT_PRC
			AND L.RN_SEQ_LTR_CRT_PRC = CDW.RN_SEQ_LTR_CRT_PRC
			AND L.RN_SEQ_REC_PRC = CDW.RN_SEQ_REC_PRC
			AND L.RM_DSC_LTR_PRC = CDW.RM_DSC_LTR_PRC
			AND L.RF_SBJ_PRC = CDW.RF_SBJ_PRC
			AND L.RN_ENT_REQ_PRC = CDW.RN_ENT_REQ_PRC
			AND L.RN_ATY_SEQ_PRC = CDW.RN_ATY_SEQ_PRC
		WHERE
			CDW.LTXX_LETTER_REQUEST_ID IS NULL
		
		ORDER BY
			L.RT_RUN_SRT_DTS_PRC
;

CREATE TABLE T AS 
	SELECT DISTINCT
		RN_SEQ_LTR_CRT_PRC AS LetterSeq,
		DF_SPE_ACC_ID as AccountNumber,
		RM_DSC_LTR_PRC as LetterId,
		RF_SBJ_PRC as Ssn,
		RX_REQ_ARA_X_PRC as TextArea,
		RT_RUN_SRT_PRC AS SentDate
	FROM
		DATA
	WHERE RM_DSC_LTR_PRC IN ('TSXXBSAPPM',
'TSXXBSAPPV',
'TSXXBSCRAA',
'TSXXBSCRAE',
'TSXXBFNBIL',
'TSXXBGLBX',
'TSXXBTMXC',
'TSXXBDMIL')
;
QUIT;


PROC SQL;
	CREATE TABLE COUNT AS 
		SELECT
			LetterId,
			COUNT(*) AS MissingCount
		FROM
			T
		WHERE LETTERID  IN ('TSXXBSAPPM',
'TSXXBSAPPV',
'TSXXBSCRAA',
'TSXXBSCRAE',
'TSXXBFNBIL',
'TSXXBGLBX',
'TSXXBTMXC',
'TSXXBDMIL')
		GROUP BY 
			LetterId
;
QUIT;


%LET BSYS = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\BSYS.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME BSYS ODBC &BSYS ;

PROC SQL;
	CREATE TABLE COUNT_WITH_DESCRIPTION AS
		SELECT
			C.*,
			B.DocName
		FROM
			COUNT C
		LEFT JOIN BSYS.LTDB_DAT_DocDetail B
			ON B.ID = C.LETTERID
;
QUIT;

PROC SQL;
	CREATE TABLE MANUAL AS
		SELECT
			ACCOUNTNUMBER,
			LETTERID,
			SENTDATE
		FROM
			T
		WHERE
			LETTERID IN ('TSXXBTMXC')
;
QUIT;

PROC SQL;
	CREATE TABLE TSXXBSAPPM AS 
		SELECT
			ACCOUNTNUMBER
		FROM
			T
		WHERE
			LETTERID = 'TSXXBSAPPM'
;
	CREATE TABLE TSXXBSAPPV AS 
		SELECT
			ACCOUNTNUMBER
		FROM
			T
		WHERE LETTERID = 'TSXXBSAPPV'
;
	CREATE TABLE TSXXBSCRAA AS 
		SELECT
			ACCOUNTNUMBER
		FROM
			T
		WHERE
			LETTERID = 'TSXXBSCRAA'
;
	CREATE TABLE TSXXBGLBX AS 
		SELECT
			ACCOUNTNUMBER
		FROM
			T
		WHERE
			LETTERID = 'TSXXBGLBX'
;
QUIT;

PROC EXPORT DATA =  TSXXBSAPPM
            OUTFILE = "T:\LETTERS.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="TSXXBSAPPM"; 
RUN;
PROC EXPORT DATA =  TSXXBSAPPV
            OUTFILE = "T:\LETTERS.xlsx"
            DBMS = EXCEL
			REPLACE;
     SHEET="TSXXBSAPPV"; 
RUN;
PROC EXPORT DATA =  TSXXBSCRAA
            OUTFILE = "T:\LETTERS.xlsx"
            DBMS = EXCEL
			REPLACE;
     SHEET="TSXXBSCRAA"; 
RUN;
PROC EXPORT DATA =  TSXXBGLBX
            OUTFILE = "T:\LETTERS.xlsx"
            DBMS = EXCEL
			REPLACE;
     SHEET="TSXXBGLBX"; 
RUN;

PROC EXPORT DATA =  MANUAL
            OUTFILE = "T:\MANUAL PRINT LETTERS.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SheetX"; 
RUN;
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
DATA LEGEND.SOURCE; *Send data to Duster;
SET T;
RUN;

RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

PROC SQL;
	CREATE TABLE DEMO AS
					SELECT DISTINCT
						S.ACCOUNTNUMBER,
						S.LETTERID,
						S.SENTDATE,
						COALESCE(DELQ.DAYS_DELQ, X) AS DAYS_DELQ,
						CASE
							WHEN LNXX.BF_SSN IS NOT NULL THEN 'HAS OPEN LOANS'
							ELSE 'NO OPEN LOANS'
						END AS OPEN_LOANS,
						CASE
							WHEN LNXX.BF_SSN IS NOT NULL THEN 'IS COD'
							ELSE 'IS NOT COD'
						END AS COD,
						REPAY.REPAY_SCH,
						STATUS.LOAN_STATUS
						
					FROM
						SOURCE S 
					LEFT JOIN
					(
						SELECT
							BF_SSN,
							MAX(LN_DLQ_MAX) AS DAYS_DELQ
						FROM
							PKUB.LNXX_LON_DLQ_HST
						WHERE
							LC_STA_LONXX = 'X'
						GROUP BY
							BF_SSN
					) DELQ
						ON DELQ.BF_SSN = S.SSN
					LEFT JOIN PKUB.LNXX_LON LNXX
						ON LNXX.BF_SSN = S.SSN
						AND LNXX.LC_STA_LONXX = 'R'
					LEFT JOIN PKUB.LNXX_FIN_ATY LNXX
						ON LNXX.BF_SSN = S.SSN
						AND (LNXX.PC_FAT_TYP = 'XX' AND LNXX.PC_FAT_SUB_TYP = 'XX' AND LNXX.LC_FAT_REV_REA = '' AND LNXX.LC_STA_LONXX = 'A') 
					LEFT JOIN
					(
						SELECT DISTINCT
							BF_SSN,
							LC_TYP_SCH_DIS AS REPAY_SCH
						FROM
							PKUB.LNXX_LON_RPS
						WHERE
							LC_STA_LONXX = 'A'


					) REPAY
						ON REPAY.BF_SSN = S.SSN
					LEFT JOIN
					(
						SELECT DISTINCT
							BF_SSN,
							WC_DW_LON_STA AS LOAN_STATUS
						FROM
							PKUB.DWXX_DW_CLC_CLU

					) STATUS
						ON STATUS.BF_SSN = S.SSN
						
;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

data demo;
set legend.demo;
run;

PROC EXPORT DATA = demo 
            OUTFILE = "T:\Letters With Data.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SheetX"; 
RUN;
