/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";


LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn; update_lock_typ=nolock; bl_keepnulls=no";
%LET DSN = 'FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn;';
/*LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS.dsn; update_lock_typ=nolock; bl_keepnulls=no";*/
/*%LET DSN = 'FILEDSN=X:\PADR\ODBC\CLS.dsn;';*/

PROC SQL;
	CREATE TABLE SOURCE AS
		SELECT distinct
			SSN
		FROM
			SQL.DUEDATECHANGE
		
;
QUIT;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
DATA LEGEND.SOURCE; *Send data to Duster;
SET SOURCE;
RUN;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
%let DB = DNFPRQUT;  *This is test;
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
/*%let DB = DNFPUTDL;  *This is live;*/

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE X  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @XX " ********************************************************************* "
              / @XX " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @XX " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @XX " ********************************************************************* "
              / @XX " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @XX " ****  &SQLXMSG   **** "
              / @XX " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL;
	CREATE TABLE DEMO AS
					SELECT DISTINCT
						RSXX.*
					FROM
						PKUB.RSXX_BR_RPD rsXX
					inner join source s
						on s.ssn = rsXX.bf_ssn
					inner join
					(	
						SELECT DISTINCT
							bf_ssn,
							LD_RPS_X_PAY_DU
						FROM
							PKUB.RSXX_BR_RPD
						where
							LC_STA_RPSTXX = 'A'
					) pop
						on pop.bf_ssn = rsXX.bf_ssn
					where
						RSXX.LC_STA_RPSTXX = 'A'
						and DAY(rsXX.LD_RPS_X_PAY_DU) ^= DAY(pop.LD_RPS_X_PAY_DU)


;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

