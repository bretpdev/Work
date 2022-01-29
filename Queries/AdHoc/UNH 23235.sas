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
						,DW01.WC_DW_LON_STA
						,LN16.LN_DLQ_MAX
						,LN10.IF_GTR
					FROM	
						OLWHRM1.LN10_LON LN10
						INNER JOIN OLWHRM1.DW01_DW_CLC_CLU DW01
							ON DW01.BF_SSN = LN10.BF_SSN
							AND DW01.LN_SEQ = LN10.LN_SEQ
						INNER JOIN OLWHRM1.LN16_LON_DLQ_HST LN16
							ON LN16.BF_SSN = LN10.BF_SSN
							AND LN16.LN_SEQ = LN10.LN_SEQ
					WHERE	
						DW01.WC_DW_LON_STA NOT IN('07','08')
						AND LN16.LN_DLQ_MAX >= 60
						AND LN10.LA_CUR_PRI > 0
						AND LN16.LC_STA_LON16 = '1'

					ORDER BY
						LN10.IF_GTR
						,LN10.BF_SSN
						,LN10.LN_SEQ

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
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\NH 23235.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="Sheet1"; 
RUN;
