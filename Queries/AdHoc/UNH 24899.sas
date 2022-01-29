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
						LN16.LD_DLQ_OCC
						,PD10.DF_SPE_ACC_ID
						,LN10.LN_SEQ
					FROM	
						OLWHRM1.LN10_LON LN10
						JOIN OLWHRM1.LN16_LON_DLQ_HST LN16
							ON LN10.BF_SSN = LN16.BF_SSN
							AND LN10.LN_SEQ = LN16.LN_SEQ
							AND LN16.LC_STA_LON16 = '1'
						JOIN OLWHRM1.PD10_PRS_NME PD10
							ON LN10.BF_SSN = PD10.DF_PRS_ID
					WHERE	
						LN10.LA_CUR_PRI > 0
						AND LN10.LC_STA_LON10 = 'R'
						AND LN10.IF_GTR = '000800'
						AND LN16.LD_DLQ_OCC BETWEEN '05-30-2015' AND '09-13-2015'
					ORDER BY LN16.LD_DLQ_OCC
							,LN10.BF_SSN
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

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NH 24899 V2.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
