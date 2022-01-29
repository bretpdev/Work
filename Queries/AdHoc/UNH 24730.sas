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
						LN80.BF_SSN,
						LN80.LN_SEQ,
						LN80.LD_BIL_CRT,
						LN80.LN_SEQ_BIL_WI_DTE,
						LN80.LN_BIL_OCC_SEQ,
						LN80.LI_FNL_BIL_LON
					FROM	
						OLWHRM1.LN80_LON_BIL_CRF LN80
						JOIN OLWHRM1.PD10_PRS_NME PD10
							ON LN80.BF_SSN = PD10.DF_PRS_ID
					WHERE
						PD10.DF_SPE_ACC_ID IN ('3763266410',
												'6032443789',
												'1820403469',
												'9376267735',
												'3495426149',
												'4523384487',
												'1116685434',
												'2149634668',
												'4575498701',
												'1096727009',
												'1573752755',
												'8247493192',
												'6200291179',
												'7186873264',
												'5081241068')

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
PROC EXPORT DATA = DEMO 
            OUTFILE = "T:\SAS\LN80.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LN80"; 
RUN;
