/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;
LIBNAME AES DBX DATABASE=&DB OWNER=AES;

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

	CREATE TABLE POP AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						PDXX.DF_SPE_ACC_ID,
						PDXX.DF_PRS_ID
					FROM
						PKUB.PDXX_PRS_NME PDXX
						INNER JOIN PKUB.LNXX_LON LNXX
							ON PDXX.DF_PRS_ID = LNXX.BF_SSN
						INNER JOIN PKUB.RSXX_BR_RPD RSXX
							ON RSXX.BF_SSN = LNXX.BF_SSN
							AND RSXX.LC_STA_RPSTXX = 'A'
						INNER JOIN PKUB.LNXX_LON_RPS LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
							AND LNXX.LN_RPS_SEQ = RSXX.LN_RPS_SEQ
						INNER JOIN PKUB.LNXX_LON_DLQ_HST LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
						INNER JOIN PKUB.DWXX_DW_CLC_CLU DWXX
							ON DWXX.BF_SSN = LNXX.BF_SSN
							AND DWXX.LN_SEQ = LNXX.LN_SEQ
							AND DWXX.WC_DW_LON_STA = 'XX'
						LEFT JOIN
						(
							SELECT
								BF_SSN
							FROM
								PKUB.LNXX_LON_DLQ_HST
							WHERE
								LC_STA_LONXX = 'X'
						)LNXX_CUR
							ON LNXX_CUR.BF_SSN = LNXX.BF_SSN
						LEFT JOIN PKUB.BRXX_BR_EFT BRXX
							ON BRXX.BF_SSN = LNXX.BF_SSN
						LEFT JOIN
						(
							SELECT
								BF_SSN
							FROM
								PKUB.LNXX_LON_RPS
							WHERE
								LC_TYP_SCH_DIS IN ('CQ','CX','CX','CX','IB','IL','IS','IX','CA','CP','IP','PG','PL','FS','FG')
								AND LC_STA_LONXX = 'A'
						)IDR
							ON IDR.BF_SSN = LNXX.BF_SSN
						INNER JOIN
						(
							SELECT DISTINCT
								BLXX.BF_SSN,
								BLXX.LD_BIL_DU
							FROM
								PKUB.BLXX_BR_BIL BLXX
							INNER JOIN
							(
								SELECT
									B.BF_SSN,
									MAX(B.LD_BIL_DU) AS LD_BIL_DU
								FROM
									PKUB.BLXX_BR_BIL B
								WHERE
									B.LC_STA_BILXX = 'A'
								GROUP BY 
									BF_SSN
							)MAX_BILL
								ON MAX_BILL.BF_SSN = BLXX.BF_SSN
								AND MAX_BILL.LD_BIL_DU = BLXX.LD_BIL_DU
							WHERE
								DAY(BLXX.LD_BIL_DU) BETWEEN XX AND XX
						)BLXX
							ON BLXX.BF_SSN = LNXX.BF_SSN
					WHERE
						LNXX.LA_CUR_PRI > X
						AND LNXX.LC_STA_LONXX = 'R'
						AND LNXX.LC_STA_LONXX = 'A'
						AND LNXX.LC_TYP_SCH_DIS NOT IN ('CQ','CX','CX','CX','IB','IL','IS','IX','CA','CP','IP','PG','PL','FS','FG')
						AND (BRXX.BC_EFT_STA IS NULL OR BRXX.BC_EFT_STA != 'A')
						AND LNXX.LD_DLQ_OCC BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
						AND LNXX_CUR.BF_SSN IS NULL
						AND LNXX.LN_DLQ_MAX > X
						AND IDR.BF_SSN IS NULL
						

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;
QUIT;

ENDRSUBMIT;

DATA POP; SET LEGEND.POP; RUN;

			
DATA _NULL_;
	SET POP;
	FILE "&RPTLIB/NH XXXX_SCRIPT_POP.txt" DELIMITER=',' DSD DROPOVER LRECL=XXXXX;

	IF _N_ = X THEN
		DO;
			PUT
				'DF_SPE_ACC_ID'
				','
				'DUE_DATE'
				;

		END;
	DO;
		PUT DF_SPE_ACC_ID $ @;
		PUT 'XX' ;

	;
	END;
RUN;

/*LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn; update_lock_typ=nolock; bl_keepnulls=no";*/
/*%LET DSN = 'FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn;';*/
/*LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS.dsn; update_lock_typ=nolock; bl_keepnulls=no";*/
/*%LET DSN = 'FILEDSN=X:\PADR\ODBC\CLS.dsn;';*/

/*PROC SQL;*/
/*	INSERT INTO	SQL.DueDateChange(Ssn,AccountNumber,DueDate)*/
/*	SELECT*/
/*		DF_PRS_ID,*/
/*		DF_SPE_ACC_ID,*/
/*		'XX'*/
/*	FROM*/
/*		POP*/
/*;*/
/*QUIT;*/
