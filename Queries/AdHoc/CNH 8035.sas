/*set job specific values*/
/*	%LET ARCTYPEID = X;		*AtdXXByLoan - Add arc by sequence number;*/
/*	%LET ARCTYPEID = X;		*AtdXXAllLoans - Add arc to all loans;*/
	%LET ARCTYPEID = X;		*AtdXXByBalance - Add arc for all loans with a balance;
/*	%LET ARCTYPEID = X;		*AtdXXByLoanProgram - Add arc by loan program;*/
/*	%LET ARCTYPEID = X;		*AtdXXAllLoansRegards - Add arc to all loans with regards to information;*/
/*	%LET ARCTYPEID = X;		*AtdXXByLoanRegards - Add arc by sequence number with regards to information;*/

	%LET ARC = 'NSCRA';
	%LET COMMENT = NULL;
	%LET SASID = 'SCRAIUDFED';
	%LET ISREFERENCE = X;
	%LET INSENDORSER = X;


/*set up library to SQL Server and include common code*/
/*	LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn; update_lock_typ=nolock; bl_keepnulls=no";*/
/*	%INCLUDE "Y:\Codebase\SAS\ArcAdd Common.SAS";*/
/*	%LET DSN = 'FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn;';*/
	LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS.dsn; update_lock_typ=nolock; bl_keepnulls=no";
	%INCLUDE "Z:\Codebase\SAS\ArcAdd Common.SAS";
	%LET DSN = 'FILEDSN=X:\PADR\ODBC\CLS.dsn;';

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

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

PROC SQL ;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						PDXX.DF_SPE_ACC_ID
/*						,LNXX.LN_SEQ*/
/*						,LNXX.LC_INT_RDC_PGM*/
/*						,LNXX.LD_ITR_EFF_BEG*/
/*						,LNXX.LD_ITR_EFF_END*/
/*						,LNXX.LC_STA_LONXX*/
/*						,LNXX.LR_ITR*/
					FROM
						PKUB.PDXX_PRS_NME PDXX
						JOIN PKUB.LNXX_LON LNXX
							ON PDXX.DF_PRS_ID = LNXX.BF_SSN
						JOIN PKUB.LNXX_INT_RTE_HST LNXX
							ON PDXX.DF_PRS_ID = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
					WHERE
						LNXX.LA_CUR_PRI > X
						AND LNXX.LC_STA_LONXX = 'R'
						AND DAYS(CURRENT DATE) BETWEEN DAYS(LNXX.LD_ITR_EFF_BEG) AND DAYS(LNXX.LD_ITR_EFF_END)
						AND LNXX.LC_STA_LONXX = 'A'
						AND LNXX.LC_INT_RDC_PGM = 'M'
						AND LNXX.LR_ITR <= X

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA REMOTE_DATA; SET LEGEND.DEMO; RUN;

/*/*export to Excel spreadsheet*/*/
/*PROC EXPORT DATA = WORK.DEMO */
/*            OUTFILE = "T:\SAS\NH XXXX Test File.xlsx" */
/*            DBMS = EXCEL*/
/*			REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
;
/*call macro or put data step here to add job specific data to LEGEND data*/
%CREATE_GENERIC_ARCADD_DATA;
/*end job specific code*/

/*call ARC add common processing*/
%ARC_ADD_PROCESSING;
