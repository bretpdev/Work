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
						PD30.DF_PRS_ID,
						PD30.DX_STR_ADR_1,
						PD30.DF_LST_USR_PD30,
						PD30.DD_STA_PDEM30
					FROM
						OLWHRM1.PD30_PRS_ADR PD30
					WHERE
						PD30.DX_STR_ADR_1 LIKE 'TT%'
						OR PD30.DX_STR_ADR_1 LIKE 'TC%'
						OR PD30.DX_STR_ADR_1 LIKE 'AM%'
						OR PD30.DX_STR_ADR_1 LIKE 'EM%'
						

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET DUSTER.DEMO; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\PD30_Commercial.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
