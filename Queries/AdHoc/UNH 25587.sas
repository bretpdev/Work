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
						PD10.DM_PRS_LST AS LastName,
						PD10.DM_PRS_1 AS FirstName,
						PD30.DX_STR_ADR_1 AS AddressLine1,
						PD30.DX_STR_ADR_2 AS AddressLine2,
						PD30.DM_CT AS City,
						PD30.DC_DOM_ST AS State,
						PD30.DF_ZIP_CDE AS ZipCode
					FROM
						OLWHRM1.PD10_PRS_NME PD10
						JOIN OLWHRM1.PD30_PRS_ADR PD30 ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
						JOIN OLWHRM1.LN10_LON LN10 ON PD10.DF_PRS_ID = LN10.BF_SSN
					WHERE
						LN10.LF_LON_CUR_OWN IN ('829769','82976901','82976902','82976903','82976904','82976905','82976906','82976907','82976908')
						AND LN10.LA_CUR_PRI > 0
						AND PD30.DI_VLD_ADR = 'Y'
						AND LN10.LD_LON_ACL_ADD > '01/01/1900'
						AND LN10.LC_STA_LON10 = 'R'
						AND PD30.DC_FGN_CNY = ''

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
            OUTFILE = "T:\SAS\UNH 25587.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
