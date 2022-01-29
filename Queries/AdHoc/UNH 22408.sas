/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWAB1.LWAB1RZ";
FILENAME REPORT2 "&RPTLIB/ULWAB1.LWAB1R2";
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
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

	CREATE TABLE DEMO AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						
						LN10.LF_LON_CUR_OWN AS Lender_Id,
						LN35.IF_BND_ISS AS Bond_Id,
						LN10.IC_LON_PGM AS Loan_Program,
						DW01.WC_DW_LON_STA AS DW01_Status,
						COUNT(*) AS Group_Count
					FROM	
						OLWHRM1.LN10_LON LN10
					INNER JOIN OLWHRM1.LN35_LON_OWN LN35
						ON LN35.BF_SSN = LN10.BF_SSN
						AND LN35.LN_SEQ = LN10.LN_SEQ
						AND LN35.IF_OWN = LN10.LF_LON_CUR_OWN
					INNER JOIN OLWHRM1.DW01_DW_CLC_CLU DW01
						ON DW01.BF_SSN = LN10.BF_SSN
						AND DW01.LN_SEQ = LN10.LN_SEQ
					WHERE	
						LN10.LA_CUR_PRI > 0
						AND LN10.LC_STA_LON10 = 'R'
						AND LN35.LD_OWN_EFF_END IS NULL
						AND LN35.LC_STA_LON35 = 'A'
					GROUP BY
						LN10.LF_LON_CUR_OWN,
						LN35.IF_BND_ISS,
						LN10.IC_LON_PGM,
						DW01.WC_DW_LON_STA

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO;
SET DUSTER.DEMO;
RUN;

/*PROC SQL;*/
/*CREATE TABLE TEST AS */
/*	SELECT*/
/*		SUM(GROUP_COUNT)*/
/*	FROM*/
/*		DEMO*/
/*;*/
/*QUIT;*/

PROC EXPORT DATA = DUSTER.DEMO 
            OUTFILE = "T:\NH 22408.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="Sheet1"; 
RUN;
