/*set job specific values*/
	/*%LET ARCTYPEID = 0;	*Atd22ByLoan - Add arc by sequence number;*/
	%LET ARCTYPEID = 1;		*Atd22AllLoans - Add arc to all loans;
	/*%LET ARCTYPEID = 2;	*Atd22ByBalance - Add arc for all loans with a balance;*/
	/*%LET ARCTYPEID = 3;	*Atd22ByLoanProgram - Add arc by loan program;*/
	/*%LET ARCTYPEID = 4;	*Atd22AllLoansRegards - Add arc to all loans with regards to information;*/
	/*%LET ARCTYPEID = 5;	*Atd22ByLoanRegards - Add arc by sequence number with regards to information;*/

	%LET ARC = 'BKMOC';
	%LET COMMENT = 'Review MOC and Pacer for Chapter 13';
	%LET SASID = 'UTLWS60';

/*set up library to SQL Server and include common code*/
/*	TEST*/
/*	LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\ULS_TEST.dsn; update_lock_typ=nolock; bl_keepnulls=no";*/
/*	%INCLUDE "X:\PADU\Test Sessions\Local SAS Schedule\ArcAdd Common.SAS";*/
/*	%LET DSN = 'FILEDSN=X:\PADR\ODBC\ULS_TEST.dsn;';*/
/*	LIVE*/
	LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\ULS.dsn; update_lock_typ=nolock; bl_keepnulls=no";
	%INCLUDE "X:\Sessions\Local SAS Schedule\ArcAdd Common.SAS";
	%LET DSN = 'FILEDSN=X:\PADR\ODBC\ULS.dsn;';

%LET RPTLIB = X:\PADD\FTP;
FILENAME REPORTZ "&RPTLIB/ULWS60.LWS60RZ";
FILENAME REPORT2 "&RPTLIB/ULWS60.LWS60R2";
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;

DATA _NULL_;
	CALL SYMPUT('BKDATE',"'"||PUT(INTNX('WEEKDAY',TODAY(),-5,'BEGINNING'), MMDDYYD10.)||"'");
RUN;

%SYSLPUT BKDATE = &BKDATE;
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

	CREATE TABLE S60 AS
		SELECT *
		FROM 
			CONNECTION TO DB2 
				(
				SELECT DISTINCT
					PD10.DF_SPE_ACC_ID
/*					,A.DF_PRS_ID		AS DF_PRS_ID_A*/
/*					,B.DF_PRS_ID		AS DF_PRS_ID_B	*/
/*					,A.PF_ACT			AS PF_ACT_A*/
/*					,B.PF_ACT			AS PF_ACT_B	*/
/*					,A.BF_LST_DTS_AY01	AS BF_LST_DTS_AY01_A*/
/*					,B.BF_LST_DTS_AY01	AS BF_LST_DTS_AY01_B*/
/*					,PD24.DD_BKR_STA*/
/*					,A.BX_CMT*/
/*					,LN10.LC_STA_LON10*/
/*					,LN10.LA_CUR_PRI*/
/*					,DW01.WC_DW_LON_STA*/
/*					,PD24.DC_BKR_TYP*/
/*					,PD24.DC_BKR_STA*/
				FROM 
					OLWHRM1.AY01_BR_ATY A
					INNER JOIN OLWHRM1.AY01_BR_ATY B
						ON A.DF_PRS_ID = B.DF_PRS_ID
					INNER JOIN OLWHRM1.LN10_LON LN10
						ON A.DF_PRS_ID = LN10.BF_SSN
					INNER JOIN OLWHRM1.DW01_DW_CLC_CLU DW01
						ON LN10.BF_SSN = DW01.BF_SSN
						AND LN10.LN_SEQ = DW01.LN_SEQ
					INNER JOIN OLWHRM1.PD24_PRS_BKR PD24
						ON LN10.BF_SSN = PD24.DF_PRS_ID
					INNER JOIN OLWHRM1.PD10_PRS_NME PD10
						ON LN10.BF_SSN = PD10.DF_PRS_ID
				WHERE 
					A.PF_ACT = 'MDCID'
					AND	B.PF_ACT = 'DBKRW'
					AND (SUBSTR(A.BX_CMT,65,5)) = 'ASMOC'
					AND A.BF_LST_DTS_AY01 < B.BF_LST_DTS_AY01 
					AND DAYS(A.BF_LST_DTS_AY01) > DAYS(PD24.DD_BKR_STA)
					AND LN10.LC_STA_LON10 = 'R'
					AND LN10.LA_CUR_PRI > 0
					AND DW01.WC_DW_LON_STA = '21'
					AND PD24.DC_BKR_TYP = '07'
					AND PD24.DC_BKR_STA = '06'
					AND DAYS(A.BF_LST_DTS_AY01) = DAYS(&BKDATE)

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
	/*QUIT;*/
QUIT;

ENDRSUBMIT;

DATA REMOTE_DATA; 
	SET DUSTER.S60; 
RUN;

/*PROC EXPORT*/
/*		DATA=S60*/
/*		OUTFILE="&RPTLIB\SASR_3686.txt"*/
/*		DBMS = TAB*/
/*		REPLACE;*/
/*RUN;*/

%CREATE_GENERIC_ARCADD_DATA;
/*end job specific code*/

/*call ARC add common processing*/
%ARC_ADD_PROCESSING;
