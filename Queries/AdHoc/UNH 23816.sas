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
					SELECT
						LN10.BF_SSN
						,LN10.LN_SEQ
						,LN10.LD_LON_ACL_ADD
						,MAX(LN65.LD_CRT_LON65) AS LN65Date
						,BR30.BC_EFT_STA
					FROM	
						OLWHRM1.LN10_LON LN10
						INNER JOIN OLWHRM1.LN65_LON_RPS LN65
							ON LN65.BF_SSN = LN10.BF_SSN
							AND LN65.LN_SEQ = LN10.LN_SEQ
						LEFT OUTER JOIN OLWHRM1.BR30_BR_EFT BR30
							ON BR30.BF_SSN = LN10.BF_SSN
					WHERE	
						LN10.LA_CUR_PRI > 0
						AND LN10.LD_LON_ACL_ADD = '6/4/2013'
						AND LN10.IC_LON_PGM IN ('SUBCNS','SUBSPC','CNSLDN','UNCNS','UNSPC')
					GROUP BY
						LN10.BF_SSN
						,LN10.LN_SEQ
						,LN10.LD_LON_ACL_ADD
						,BR30.BC_EFT_STA

					FOR READ ONLY WITH UR
				)
		WHERE LN65Date < LD_LON_ACL_ADD
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO;
	SET DUSTER.DEMO;
RUN;

PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\NH 23816.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="Sheet1"; 
RUN;
