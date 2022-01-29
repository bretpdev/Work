%LET RPTLIB = T:\SAS;
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
/*	%LET ARCTYPEID = 0;		*Atd22ByLoan - Add arc by sequence number;*/
/*  %LET ARCTYPEID = 1;		*Atd22AllLoans - Add arc to all loans;*/
	%LET ARCTYPEID = 2;		*Atd22ByBalance - Add arc for all loans with a balance;
/*	%LET ARCTYPEID = 3;		*Atd22ByLoanProgram - Add arc by loan program;*/
/*	%LET ARCTYPEID = 4;		*Atd22AllLoansRegards - Add arc to all loans with regards to information;*/
/*	%LET ARCTYPEID = 5;		*Atd22ByLoanRegards - Add arc by sequence number with regards to information;*/

	%LET ARC = 'BKNFB';
	%LET COMMENT = 'Borrower has a bankruptcy forbearance, but is not in a bankruptcy status.';
	%LET SASID = 'UTLWOO4';
/*set up library to SQL Server and include common code*/
/*	LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\ULS_TEST.dsn; update_lock_typ=nolock; bl_keepnulls=no";*/
	LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\ULS.dsn; update_lock_typ=nolock; bl_keepnulls=no";
	%INCLUDE "X:\Sessions\Local SAS Schedule\ArcAdd Common.SAS";
RSUBMIT;
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
	CONNECT TO DB2 (DATABASE=DLGSUTWH);

	CREATE TABLE REMOTE_DATA AS
		SELECT DISTINCT
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						PD10.DF_SPE_ACC_ID
					FROM
						OLWHRM1.PD10_PRS_NME PD10
						INNER JOIN OLWHRM1.LN10_LON LN10
							ON PD10.DF_PRS_ID = LN10.BF_SSN
						INNER JOIN OLWHRM1.LN60_BR_FOR_APV LN60
							ON LN10.BF_SSN = LN60.BF_SSN
							AND LN10.LN_SEQ = LN60.LN_SEQ
							AND LN60.LC_STA_LON60 = 'A'
						INNER JOIN OLWHRM1.FB10_BR_FOR_REQ FB10
							ON LN60.BF_SSN = FB10.BF_SSN
							AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
							AND FB10.LC_STA_FOR10 = 'A'
							AND FB10.LC_FOR_TYP = '10'
							AND FB10.LC_FOR_STA = 'A'
						INNER JOIN OLWHRM1.DW01_DW_CLC_CLU DW01
							ON LN10.BF_SSN = DW01.BF_SSN
							AND LN10.LN_SEQ = DW01.LN_SEQ
							AND DW01.WC_DW_LON_STA NOT IN('20','21')
						LEFT OUTER JOIN
									(
										SELECT
											AY10.BF_SSN,
											AY10.LD_ATY_REQ_RCV
										FROM
											OLWHRM1.AY10_BR_LON_ATY AY10
										WHERE
											AY10.PF_REQ_ACT = 'BKNFB'
									) BKNFB /*exclude borrowers with this arc after the forbearance apply date*/
									ON BKNFB.BF_SSN = LN10.BF_SSN
									AND BKNFB.LD_ATY_REQ_RCV >= LN60.LD_FOR_APL
					WHERE
						LN10.LC_STA_LON10 = 'R'
						AND LN10.LA_CUR_PRI > 0
						AND CURRENT DATE BETWEEN LN60.LD_FOR_BEG AND LN60.LD_FOR_END
						AND BKNFB.BF_SSN IS NULL

					FOR READ ONLY WITH UR
				)
	;
	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA REMOTE_DATA; SET DUSTER.REMOTE_DATA; RUN;

%CREATE_GENERIC_ARCADD_DATA;
%ARC_ADD_PROCESSING;
