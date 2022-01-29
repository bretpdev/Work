/*	%LET ARCTYPEID = 0;		*Atd22ByLoan - Add arc by sequence number;*/
/*  %LET ARCTYPEID = 1;		*Atd22AllLoans - Add arc to all loans;*/
	%LET ARCTYPEID = 2;		*Atd22ByBalance - Add arc for all loans with a balance;
/*	%LET ARCTYPEID = 3;		*Atd22ByLoanProgram - Add arc by loan program;*/
/*	%LET ARCTYPEID = 4;		*Atd22AllLoansRegards - Add arc to all loans with regards to information;*/
/*	%LET ARCTYPEID = 5;		*Atd22ByLoanRegards - Add arc by sequence number with regards to information;*/

%LET ARC = 'DIFST';
%LET COMMENT = 'Borrower has loan sequences that are currently in different statuses. Review account.';
%LET SASID = 'UTNWS97';

/*set up library to SQL Server and include common code*/

LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn; update_lock_typ=nolock; bl_keepnulls=no";
%INCLUDE "Y:\Codebase\SAS\ArcAdd Common.SAS";
/*LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS.dsn; update_lock_typ=nolock; bl_keepnulls=no";
%INCLUDE "Z:\Codebase\SAS\ArcAdd Common.SAS";*/

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
%let DB = DNFPUTDL;  *This is live;
/*LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE 0  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @01 " ********************************************************************* "
              / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @01 " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @01 " ****  &SQLXMSG   **** "
              / @01 " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT DISTINCT
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						PD10.DF_SPE_ACC_ID,
						CASE WHEN LN10.IC_LON_PGM IN('DLSPCN','DLSSPL','DLUSPL','SPCNSL') AND DW01Diff.IC_LON_PGM IN('DLPLGB','DLPCNS','PLUSGB','DLPLUS','PLUS','CNSLDN','DLUCNS','DLSCNS')
							    THEN 1 /*excluded for spousal consol with grad plus or parent plus*/
							 WHEN LN10.IC_LON_PGM IN('CNSLDN','DLUCNS','DLSCNS') AND DW01Diff.IC_LON_PGM IN('DLPLGB','DLPCNS','PLUSGB','DLPLUS','PLUS')
							 	THEN 1 /*excluded for non spousal with grad plus or parent plus*/
							 WHEN LN10.IC_LON_PGM IN('DLPLUS','PLUS','DLPCNS') AND DW01Diff.IC_LON_PGM IN('DLPLGB','DLPCNS','PLUSGB')
							 	THEN 1 /*excluded for parent plus with grad plus*/
							 WHEN POP8.BF_SSN IS NOT NULL 
							 	THEN 1 /*Include new scenario POP8 below*/
							 ELSE 0 END AS Exclude
					FROM
						PKUB.PD10_PRS_NME PD10
						INNER JOIN PKUB.LN10_LON LN10
							ON PD10.DF_PRS_ID = LN10.BF_SSN
						INNER JOIN PKUB.DW01_DW_CLC_CLU DW01
							ON LN10.BF_SSN = DW01.BF_SSN
							AND LN10.LN_SEQ = DW01.LN_SEQ
						/*This is grabbing loans of different statuses for the same borrower*/
						INNER JOIN 
							(
								SELECT
									DW01Inner.BF_SSN,
									DW01Inner.LN_SEQ,
									DW01Inner.WC_DW_LON_STA,
									LN10Inner.IC_LON_PGM
								FROM
									PKUB.DW01_DW_CLC_CLU DW01Inner
									INNER JOIN PKUB.LN10_LON LN10Inner
										ON DW01Inner.BF_SSN = LN10Inner.BF_SSN
										AND DW01Inner.LN_SEQ = LN10Inner.LN_SEQ
										AND LN10Inner.LA_CUR_PRI > 0
										AND LN10Inner.LC_STA_LON10 = 'R'
							) DW01Diff
							ON DW01.BF_SSN = DW01Diff.BF_SSN
							AND DW01.LN_SEQ <> DW01Diff.LN_SEQ
							AND DW01.WC_DW_LON_STA <> DW01Diff.WC_DW_LON_STA
						LEFT JOIN
							(
								SELECT
									AY10.BF_SSN,
									AY10.LD_ATY_REQ_RCV
								FROM
									PKUB.AY10_BR_LON_ATY AY10
								WHERE
									AY10.PF_REQ_ACT = 'DIFST'
							) DIFST /*exclude borrowers with this arc in the last 30 days*/
							ON DIFST.BF_SSN = LN10.BF_SSN
							AND DAYS(CURRENT DATE) - DAYS(DIFST.LD_ATY_REQ_RCV) <= 30
/*If the borrowers loans that are in different statuses have a combination status In-School and Deferment pop8*/
						LEFT JOIN 
							(
								SELECT
									DW0102.BF_SSN
								FROM 
									PKUB.DW01_DW_CLC_CLU DW0102
									INNER JOIN PKUB.DW01_DW_CLC_CLU DW0104
										ON DW0104.BF_SSN = DW0102.BF_SSN
										AND DW0104.WC_DW_LON_STA = '04'
									INNER JOIN PKUB.DF10_BR_DFR_REQ DF10
										ON DF10.BF_SSN = DW0104.BF_SSN
										AND DF10.LC_DFR_STA = 'A'
										AND DF10.LC_STA_DFR10 = 'A'
										AND DF10.LC_DFR_TYP IN ('15','18') /*the deferment type is either half-time or full-time */
									INNER JOIN PKUB.LN50_BR_DFR_APV LN50
										ON LN50.BF_SSN = DF10.BF_SSN
										AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
										AND LN50.LC_STA_LON50 = 'A'
										AND Current Date BETWEEN LN50.LD_DFR_BEG AND LN50.LD_DFR_END
								WHERE 
									DW0102.WC_DW_LON_STA IN ('02')
							) POP8
							ON POP8.BF_SSN = LN10.BF_SSN
						LEFT JOIN
							(
								SELECT
									*
								FROM
									PKUB.WQ20_TSK_QUE WQ20
								WHERE
									WQ20.WF_QUE = '36'
									AND WQ20.WF_SUB_QUE = '01'
									AND WQ20.WC_STA_WQUE20 IN('A','H','P','U','W','X')
							) ActiveQueue
							ON ActiveQueue.BF_SSN = LN10.BF_SSN
					WHERE
						LN10.LC_STA_LON10 = 'R'
						AND LN10.LA_CUR_PRI > 0
						AND DIFST.BF_SSN IS NULL
						AND ActiveQueue.BF_SSN IS NULL /*Doesnt have an open task to be worked already*/

					FOR READ ONLY WITH UR
				)
	;
	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

PROC SQL;
	CREATE TABLE REMOTE_DATA AS
		SELECT DISTINCT
			D.DF_SPE_ACC_ID
		FROM 
			DEMO D
		GROUP BY 
			D.DF_SPE_ACC_ID
		HAVING
			SUM(D.EXCLUDE) = 0
	;
QUIT;

%CREATE_GENERIC_ARCADD_DATA;
%ARC_ADD_PROCESSING;
