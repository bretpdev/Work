%LET INPUTDATE = '02/11/2016';

/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWAB1.LWAB1RZ";
FILENAME REPORT2 "&RPTLIB/ULWAB1.LWAB1R2";

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;

%SYSLPUT INPUTDATE = &INPUTDATE;

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
						PD10.DF_SPE_ACC_ID
						,LN10.LN_SEQ
						,LN16.LD_DLQ_OCC 
						,LN10.LD_LON_ACL_ADD
					FROM	
						OLWHRM1.PD10_PRS_NME PD10
						INNER JOIN OLWHRM1.LN10_LON LN10
							ON PD10.DF_PRS_ID = LN10.BF_SSN
						INNER JOIN OLWHRM1.LN16_LON_DLQ_HST LN16
							ON LN10.BF_SSN = LN16.BF_SSN
							AND LN10.LN_SEQ = LN16.LN_SEQ
					WHERE	
						DAYS(LN10.LD_LON_ACL_ADD) = DAYS(&INPUTDATE)
						AND DAYS(CURRENT_DATE) - DAYS(LN16.LD_DLQ_OCC) <= 60

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
/*DATA DEMO; SET DUSTER.DEMO; RUN;*/

PROC EXPORT DATA= DUSTER.DEMO
            OUTFILE= "X:\PADD\FTP\Transfer Forbearance UHEAA.CSV" 
            DBMS=CSV REPLACE;
RUN;

