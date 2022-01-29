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

	CREATE TABLE MR64 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						*
					FROM	
						OLWHRM1.MR64_BR_TAX
					WHERE
						LF_CRT_DTS_MR64 = '01/26/2016'

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE MR65 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						*
					FROM	
						OLWHRM1.MR65_MSC_TAX_RPT
					WHERE
						WF_CRT_DTS_MR65 = '01/26/2016'

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA MR64;
	SET DUSTER.MR64;
RUN;
DATA MR65;
	SET DUSTER.MR65;
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.MR64 
            OUTFILE = "T:\SAS\UNH 25732.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="MR64"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.MR65 
            OUTFILE = "T:\SAS\UNH 25732.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="MR65"; 
RUN;
