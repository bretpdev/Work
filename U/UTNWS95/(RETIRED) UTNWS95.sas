/*set job specific values*/
/*	%LET ARCTYPEID = 0;		*Atd22ByLoan - Add arc by sequence number;*/
	%LET ARCTYPEID = 1;		*Atd22AllLoans - Add arc to all loans;
/*	%LET ARCTYPEID = 2;		*Atd22ByBalance - Add arc for all loans with a balance;*/
/*	%LET ARCTYPEID = 3;		*Atd22ByLoanProgram - Add arc by loan program;*/
/*	%LET ARCTYPEID = 4;		*Atd22AllLoansRegards - Add arc to all loans with regards to information;*/
/*	%LET ARCTYPEID = 5;		*Atd22ByLoanRegards - Add arc by sequence number with regards to information;*/

	%LET ARC = 'CODFP';
	%LET COMMENT = ' ';
	%LET SASID = 'UTNWS95';

/*set up library to SQL Server and include common code*/
	*TEST CONNECTIONS;
	LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn; update_lock_typ=nolock; bl_keepnulls=no";
	%INCLUDE "Y:\Codebase\SAS\ArcAdd Common.SAS";
	%LET DSN = 'FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn;';

	*LIVE CONNECTIONS;
/*	LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS.dsn; update_lock_typ=nolock; bl_keepnulls=no";*/
/*	%INCLUDE "Z:\Codebase\SAS\ArcAdd Common.SAS";*/
/*	%LET DSN = 'FILEDSN=X:\PADR\ODBC\CLS.dsn;';*/

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is VUK1 test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;

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

	CREATE TABLE REMOTE_DATA AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						PD10.DF_SPE_ACC_ID
/*						,LN10.LN_SEQ*/
					FROM
						PKUB.PD10_PRS_NME PD10
						INNER JOIN PKUB.LN10_LON LN10
							ON PD10.DF_PRS_ID = LN10.BF_SSN
						INNER JOIN PKUB.DW01_DW_CLC_CLU DW01
							ON PD10.DF_PRS_ID = DW01.BF_SSN
							AND LN10.LN_SEQ = DW01.LN_SEQ
						INNER JOIN PKUB.LN16_LON_DLQ_HST LN16
							ON PD10.DF_PRS_ID = LN16.BF_SSN
							AND LN10.LN_SEQ = LN16.LN_SEQ
							AND LN16.LC_STA_LON16 = '1'
							AND LN16.LN_DLQ_MAX BETWEEN 1 AND 5
/*Filter for borrowers who have received their first bill within the last week*/
						INNER JOIN 
						(/*gets only those whose minimum bill date is within last 7 days*/
							SELECT
								LN80_MIN.BF_SSN
								,LN80_MIN.LN_SEQ
							FROM 
								(/*gets minimum bill date for all loans*/
									SELECT
										LN80A.BF_SSN
										,LN80A.LN_SEQ
										,MIN(LN80A.LD_BIL_DU_LON) AS LD_BIL_DU_LON
									FROM 
										PKUB.LN80_LON_BIL_CRF LN80A
									WHERE 
										LN80A.LC_STA_LON80 = 'A'
										AND LN80A.LC_BIL_TYP_LON = 'P'
									GROUP BY
										LN80A.BF_SSN
										,LN80A.LN_SEQ
								) LN80_MIN
							WHERE 
								DAYS(LN80_MIN.LD_BIL_DU_LON) BETWEEN DAYS(CURRENT_DATE) - 7 AND DAYS(CURRENT_DATE)
					 	) LN80
							ON PD10.DF_PRS_ID = LN80.BF_SSN
							AND LN10.LN_SEQ = LN80.LN_SEQ
						INNER JOIN PKUB.LN90_FIN_ATY LN90
							ON PD10.DF_PRS_ID = LN90.BF_SSN
							AND LN10.LN_SEQ = LN90.LN_SEQ
							AND LN90.LC_STA_LON90 = 'A'
							AND LN90.LC_FAT_REV_REA = ' '
							AND LN90.PC_FAT_TYP = '01'
							AND LN90.PC_FAT_SUB_TYP = '01'
						INNER JOIN PKUB.PD40_PRS_PHN PD40
							ON PD10.DF_PRS_ID = PD40.DF_PRS_ID
							AND PD40.DI_PHN_VLD = 'Y'
							AND PD40.DC_PHN IN ('A','H','W')
							AND PD40.DC_ALW_ADL_PHN IN ('L','P','X')
/*Find ARCs left on account in last 20 days ON LOAN LEVEL*/
						LEFT JOIN 
						(
							SELECT 
								LN85.BF_SSN
								,LN85.LN_SEQ
							FROM
								PKUB.LN85_LON_ATY LN85
								INNER JOIN PKUB.AY10_BR_LON_ATY AY10
									ON LN85.BF_SSN = AY10.BF_SSN
									AND LN85.LN_ATY_SEQ = AY10.LN_ATY_SEQ
							WHERE 
								AY10.PF_REQ_ACT = 'CODFP'
								AND DAYS(AY10.LD_ATY_REQ_RCV) BETWEEN DAYS(CURRENT_DATE) - 20 AND DAYS(CURRENT_DATE) 
						) ARC
							ON PD10.DF_PRS_ID = ARC.BF_SSN
							AND LN10.LN_SEQ = ARC.LN_SEQ
					WHERE
						LN10.LA_CUR_PRI > 0
						AND LN10.LC_STA_LON10 = 'R'
						AND DW01.WC_DW_LON_STA = '03'
						AND ARC.BF_SSN IS NULL 
						AND ARC.LN_SEQ IS NULL

					FOR READ ONLY WITH UR
				)
	;
	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA REMOTE_DATA; 
	SET LEGEND.REMOTE_DATA; 
RUN;

/*call macro or put data step here to add job specific data to LEGEND data*/
%CREATE_GENERIC_ARCADD_DATA;
/*end job specific code*/

/*call ARC add common processing*/
/*%ARC_ADD_PROCESSING; *CAUTION: WRITES TO ARC ADD PROCESSING! COMMENT OUT FOR TESTING;*/
