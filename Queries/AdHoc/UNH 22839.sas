/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS46.NWS46RZ";
FILENAME REPORT2 "&RPTLIB/UNWS46.NWS46R2";

PROC IMPORT OUT = WORK.SOURCE
            DATAFILE = "T:\JoshuaBentley_DCR1.xlsx" 
            DBMS = xlsx REPLACE;
   			SHEET = 'Sheet1'; 
RUN;

data source;
	set source;
	format ssn z9.	accountnumber z10.;
run;

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER SLIBREF=WORK;
DATA DUSTER.SOURCE;
SET SOURCE;
RUN;

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
						AF_APL_ID,
						AF_APL_ID_SFX,
						LF_CRT_DTS_DC10
					FROM	
						OLWHRM1.DC01_LON_CLM_INF
					WHERE	
						BF_SSN = '529738721'
						AND AF_APL_ID IN ('0040270000B041411','0040270000B035087','0007490000A133875','0036800000U010914')
						AND LC_STA_DC10 = '03'
						AND AF_APL_ID_SFX = '01'

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

PROC EXPORT DATA= WORK.DEMO 
            OUTFILE= "T:\NH 22839.xlsx" 
            DBMS=EXCEL REPLACE;
     SHEET="Sheet1"; 
RUN;
