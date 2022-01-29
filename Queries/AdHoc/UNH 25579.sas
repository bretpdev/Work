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

	CREATE TABLE DTPRS AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						AY10.*
					FROM	
						OLWHRM1.AY10_BR_LON_ATY AY10
					WHERE	
						/*AY10.PF_REQ_ACT = 'DTPRS'*/
						/*AND*/ AY10.PF_RSP_ACT IS NOT NULL
						AND (
							DAYS(AY10.LD_ATY_RSP) = DAYS('06/23/2015')
							OR DAYS(AY10.LD_ATY_RSP) = DAYS('07/07/2015')
							OR DAYS(AY10.LD_ATY_RSP) = DAYS('07/09/2015')
							)

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DTPRS;
	SET DUSTER.DTPRS;
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DTPRS 
            OUTFILE = "T:\SAS\NH 25579.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="DTPRS"; 
RUN;
