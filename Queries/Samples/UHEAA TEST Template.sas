/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWS65.LWS65RZ";
FILENAME REPORT2 "&RPTLIB/ULWS65.LWS65R2";

LIBNAME  QADBD004  REMOTE  SERVER=QADBD004 SLIBREF=WORK  ;
RSUBMIT QADBD004;


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
	CONNECT TO DB2 (DATABASE=DLGSWQUT);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						PD10.DF_SPE_ACC_ID
					FROM
						OLWHRM1.PD10_PRS_NME PD10
						

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;


QUIT;

ENDRSUBMIT;
DATA DEMO; SET QADBD004.DEMO; RUN;

