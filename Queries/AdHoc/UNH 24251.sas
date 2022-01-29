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

	CREATE TABLE LP01 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						*
					FROM	
						OLWHRM1.LP01_CAP_OPT_RUL LP01
					WHERE
						LP01.IF_GTR = '000749'
						AND LP01.IF_OWN = '828476'

					FOR READ ONLY WITH UR
				);

	CREATE TABLE LP02 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						*
					FROM	
						OLWHRM1.LP02_DFR_PAR LP02
					WHERE
						LP02.IF_GTR = '000749'
						/*AND LP02.IF_OWN = '828476'*/

					FOR READ ONLY WITH UR
				);

	CREATE TABLE LP03 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						*
					FROM	
						OLWHRM1.LP03_RDI LP03
					WHERE
						LP03.IF_GTR = '000749'
						AND LP03.IF_OWN = '828476'

					FOR READ ONLY WITH UR
				);
	CREATE TABLE LP04 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						*
					FROM	
						OLWHRM1.LP04_UPD_SQR_ORD LP04
					WHERE
						LP04.IF_GTR = '000749'
						/*AND LP04.IF_OWN = '828476'*/

					FOR READ ONLY WITH UR
				);
	CREATE TABLE LP05 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						*
					FROM	
						OLWHRM1.LP05_FOR_PAR LP05
					WHERE
						LP05.IF_GTR = '000749'
						AND LP05.IF_OWN = '828476'

					FOR READ ONLY WITH UR
				);
	CREATE TABLE LP06 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						*
					FROM	
						OLWHRM1.LP06_ITR_AND_TYP LP06
					WHERE
						LP06.IF_GTR = '000749'
						AND LP06.IF_OWN = '828476'

					FOR READ ONLY WITH UR
				);
	CREATE TABLE LP08 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						*
					FROM	
						OLWHRM1.LP08_PAY_APL_PAR LP08
					WHERE
						LP08.IF_GTR = '000749'
						AND LP08.IF_OWN = '828476'

					FOR READ ONLY WITH UR
				);
	CREATE TABLE LP09 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						*
					FROM	
						OLWHRM1.LP09_OWN_DSB_PAR LP09
					WHERE
						/*LP09.IF_GTR = '000749'
						AND*/ LP09.IF_OWN = '828476'

					FOR READ ONLY WITH UR
				);
	CREATE TABLE LP10 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						*
					FROM	
						OLWHRM1.LP10_RPY_PAR LP10
					WHERE
						LP10.IF_GTR = '000749'
						AND LP10.IF_OWN = '828476'

					FOR READ ONLY WITH UR
				);
	CREATE TABLE LP11 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						*
					FROM	
						OLWHRM1.LP11_WUP_RFD_PAR LP11
					WHERE
						LP11.IF_GTR = '000749'
						AND LP11.IF_OWN = '828476'

					FOR READ ONLY WITH UR
				);
	CREATE TABLE LP12 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						*
					FROM	
						OLWHRM1.LP12_GTR_DSB_PAR LP12
					WHERE
						LP12.IF_GTR = '000749'
						/*AND LP12.IF_OWN = '828476'*/

					FOR READ ONLY WITH UR
				);
	CREATE TABLE LP13 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						*
					FROM	
						OLWHRM1.LP13_DD_ACT_STA LP13
					WHERE
						LP13.IF_GTR = '000749'
						AND LP13.IF_OWN = '828476'

					FOR READ ONLY WITH UR
				);
	CREATE TABLE LP14 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						*
					FROM	
						OLWHRM1.LP14_DD_ACT_WDO LP14
					WHERE
						LP14.IF_GTR = '000749'
						AND LP14.IF_OWN = '828476'

					FOR READ ONLY WITH UR
				);
	CREATE TABLE LP15 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						*
					FROM	
						OLWHRM1.LP15_DD_ACT_STE LP15
					WHERE
						LP15.IF_GTR = '000749'
						AND LP15.IF_OWN = '828476'

					FOR READ ONLY WITH UR
				);
	CREATE TABLE LP17 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						*
					FROM	
						OLWHRM1.LP17_STP_PUR LP17
					WHERE
						LP17.IF_GTR = '000749'
						/*AND LP17.IF_OWN = '828476'*/

					FOR READ ONLY WITH UR
				);
	CREATE TABLE LP18 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						*
					FROM	
						OLWHRM1.LP18_DD_CYC_PAR LP18
					WHERE
						LP18.IF_GTR = '000749'
						/*AND LP18.IF_OWN = '828476'*/

					FOR READ ONLY WITH UR
				);
	CREATE TABLE LP19 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						*
					FROM	
						OLWHRM1.LP19_DD_SKP_PAR LP19
					WHERE
						LP19.IF_GTR = '000749'
						/*AND LP19.IF_OWN = '828476'*/

					FOR READ ONLY WITH UR
				);
	CREATE TABLE LP20 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						*
					FROM	
						OLWHRM1.LP20_GTR_OWN_FEE LP20
					WHERE
						LP20.IF_GTR = '000749'
						AND LP20.IF_OWN = '828476'

					FOR READ ONLY WITH UR
				);

QUIT;

ENDRSUBMIT;
DATA LP01;
	SET DUSTER.LP01;
RUN;
DATA LP02;
	SET DUSTER.LP02;
RUN;
DATA LP03;
	SET DUSTER.LP03;
RUN;
DATA LP04;
	SET DUSTER.LP04;
RUN;
DATA LP05;
	SET DUSTER.LP05;
RUN;
DATA LP06;
	SET DUSTER.LP06;
RUN;
DATA LP08;
	SET DUSTER.LP08;
RUN;
DATA LP09;
	SET DUSTER.LP09;
RUN;
DATA LP10;
	SET DUSTER.LP10;
RUN;
DATA LP11;
	SET DUSTER.LP11;
RUN;
DATA LP12;
	SET DUSTER.LP12;
RUN;
DATA LP13;
	SET DUSTER.LP13;
RUN;
DATA LP14;
	SET DUSTER.LP14;
RUN;
DATA LP15;
	SET DUSTER.LP15;
RUN;
DATA LP17;
	SET DUSTER.LP17;
RUN;
DATA LP18;
	SET DUSTER.LP18;
RUN;
DATA LP19;
	SET DUSTER.LP19;
RUN;
DATA LP20;
	SET DUSTER.LP20;
RUN;

PROC EXPORT DATA = LP01 
            OUTFILE = "T:\SAS\NH 24251.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LP01"; 
RUN;
PROC EXPORT DATA = LP02 
            OUTFILE = "T:\SAS\NH 24251.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LP02"; 
RUN;
PROC EXPORT DATA = LP03 
            OUTFILE = "T:\SAS\NH 24251.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LP03"; 
RUN;
PROC EXPORT DATA = LP04 
            OUTFILE = "T:\SAS\NH 24251.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LP04"; 
RUN;
PROC EXPORT DATA = LP05 
            OUTFILE = "T:\SAS\NH 24251.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LP05"; 
RUN;
PROC EXPORT DATA = LP06 
            OUTFILE = "T:\SAS\NH 24251.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LP06"; 
RUN;
PROC EXPORT DATA = LP08 
            OUTFILE = "T:\SAS\NH 24251.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LP08"; 
RUN;
PROC EXPORT DATA = LP09 
            OUTFILE = "T:\SAS\NH 24251.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LP09"; 
RUN;
PROC EXPORT DATA = LP10 
            OUTFILE = "T:\SAS\NH 24251.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LP10"; 
RUN;
PROC EXPORT DATA = LP11 
            OUTFILE = "T:\SAS\NH 24251.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LP11"; 
RUN;
PROC EXPORT DATA = LP12 
            OUTFILE = "T:\SAS\NH 24251.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LP12"; 
RUN;
PROC EXPORT DATA = LP13
            OUTFILE = "T:\SAS\NH 24251.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LP13"; 
RUN;
PROC EXPORT DATA = LP14
            OUTFILE = "T:\SAS\NH 24251.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LP14"; 
RUN;
PROC EXPORT DATA = LP15 
            OUTFILE = "T:\SAS\NH 24251.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LP15"; 
RUN;
PROC EXPORT DATA = LP17 
            OUTFILE = "T:\SAS\NH 24251.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LP17"; 
RUN;
PROC EXPORT DATA = LP18 
            OUTFILE = "T:\SAS\NH 24251.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LP18"; 
RUN;
PROC EXPORT DATA = LP19 
            OUTFILE = "T:\SAS\NH 24251.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LP19"; 
RUN;
PROC EXPORT DATA = LP20 
            OUTFILE = "T:\SAS\NH 24251.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LP20"; 
RUN;
