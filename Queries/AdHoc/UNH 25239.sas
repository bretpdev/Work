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
						LP06.PC_STA_LPD06,
						LP06.IC_LON_PGM,
						LP06.PF_RGL_CAT,
						LP06.IF_GTR,
						LP06.IF_OWN,
						LP06.IF_BND_ISS,
						LP06.PC_ITR_TYP,
						LP06.PD_EFF_SR_LPD06, 
						LP06.PD_EFF_END_LPD06
					FROM
						OLWHRM1.LP06_ITR_AND_TYP LP06
					WHERE
						LP06.PC_STA_LPD06 = 'A'
						AND LP06.IF_GTR = '000749'
						AND LP06.IF_OWN = '00000000'
						AND LP06.IF_BND_ISS = '00000000'

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
	FORMAT PD_EFF_SR_LPD06 MMDDYY10.;
	FORMAT PD_EFF_END_LPD06 MMDDYY10.;
RUN;

PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\NH 25239.txt" 
            DBMS = CSV 
			REPLACE;
     PUTNAMES = YES;
RUN;
