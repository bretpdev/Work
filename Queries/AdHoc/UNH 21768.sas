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
					SELECT DISTINCT
						DC01.AF_APL_ID
						,DC01.AF_APL_ID_SFX
/*						COUNT(DISTINCT DC01.AF_APL_ID || DC01.AF_APL_ID_SFX) AS LN_CNT*/
/*						,DC01.LC_STA_DC10*/
/*						,DC01.LC_AUX_STA*/
/*						,DC01.LD_STA_UPD_DC10*/
/*						,DC01.LA_TOT_ITL_CLM_PD*/
					FROM	
						OLWHRM1.DC01_LON_CLM_INF DC01
						INNER JOIN (
								SELECT
									DC01.AF_APL_ID
									,DC01.AF_APL_ID_SFX
									,DC01.LD_STA_UPD_DC10
								FROM 
									OLWHRM1.DC01_LON_CLM_INF DC01
								WHERE DC01.LC_STA_DC10 = '04'
									AND DC01.LC_AUX_STA = '01'
									AND DC01.LA_TOT_ITL_CLM_PD > 0
									)BKR
							ON DC01.AF_APL_ID = BKR.AF_APL_ID
							AND DC01.AF_APL_ID_SFX = BKR.AF_APL_ID_SFX
						INNER JOIN (
								SELECT
									DC01.AF_APL_ID
									,DC01.AF_APL_ID_SFX
									,DC01.LD_STA_UPD_DC10
								FROM 
									OLWHRM1.DC01_LON_CLM_INF DC01
								WHERE 
									DC01.LC_STA_DC10 = '03'
									AND DC01.LC_AUX_STA = ' '
									AND DC01.LA_TOT_ITL_CLM_PD > 0
									)DFL
							ON DC01.AF_APL_ID = DFL.AF_APL_ID
							AND DC01.AF_APL_ID_SFX = DFL.AF_APL_ID_SFX
						WHERE BKR.LD_STA_UPD_DC10 < DFL.LD_STA_UPD_DC10
/*						GROUP BY DC01.AF_APL_ID*/
/*								,DC01.AF_APL_ID_SFX*/

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE TOT AS
	SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
				SELECT
					COUNT (DISTINCT DC01.AF_APL_ID || DC01.AF_APL_ID_SFX) AS TOT_BKR
				FROM 
					OLWHRM1.DC01_LON_CLM_INF DC01
				WHERE DC01.LC_STA_DC10 = '04'
					AND DC01.LC_AUX_STA = '01'
					AND DC01.LA_TOT_ITL_CLM_PD > 0
				)
;
QUIT;

ENDRSUBMIT;
DATA DEMO2;
	SET DUSTER.DEMO;
RUN;

DATA TOT;
	SET DUSTER.TOT;
RUN;

DATA FINL;
	SET DEMO TOT;
	PERCENT = LN_CNT/TOT_LN_CNT;
	FORMAT PERCENT BEST12.;
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO2 
            OUTFILE = "T:\SAS\NH 21768 Detail.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
