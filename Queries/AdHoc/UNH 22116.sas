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

	CREATE TABLE BKR AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						PD01.DF_SPE_ACC_ID
						,DC01.AF_APL_ID
						,DC01.AF_APL_ID_SFX
						,DC18.LD_BKR_FIL
					FROM	
						OLWHRM1.DC01_LON_CLM_INF DC01
						INNER JOIN OLWHRM1.DC18_BKR DC18
							ON DC01.AF_APL_ID = DC18.AF_APL_ID
							AND DC01.AF_APL_ID_SFX = DC18.AF_APL_ID_SFX
						INNER JOIN OLWHRM1.GA01_APP GA01
							ON DC01.AF_APL_ID = GA01.AF_APL_ID
						INNER JOIN OLWHRM1.PD01_PDM_INF PD01
							ON GA01.DF_PRS_ID_BR = PD01.DF_PRS_ID
					WHERE	
						DC01.LC_AUX_STA = '07'
						AND DC18.LC_BKR_STA = '01'
						AND DC18.LD_BKR_FIL >= '01/01/2009'

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE DSM AS
		SELECT
			*
		FROM 
			CONNECTION TO DB2
				( 
					SELECT DISTINCT
						PD01.DF_SPE_ACC_ID
						,DC01.AF_APL_ID
						,DC01.AF_APL_ID_SFX
						,DC18.LD_BKR_FIL
						,DC18.LD_BKR_DSM
					FROM 
						OLWHRM1.DC01_LON_CLM_INF DC01
						INNER JOIN OLWHRM1.DC18_BKR DC18
							ON DC01.AF_APL_ID = DC18.AF_APL_ID
							AND DC01.AF_APL_ID_SFX = DC18.AF_APL_ID_SFX
						INNER JOIN OLWHRM1.GA01_APP GA01
							ON DC01.AF_APL_ID = GA01.AF_APL_ID
						INNER JOIN OLWHRM1.PD01_PDM_INF PD01
							ON GA01.DF_PRS_ID_BR = PD01.DF_PRS_ID
					WHERE
						DC01.LC_AUX_STA = '01'
						AND DC18.LD_BKR_DSM >= '01/01/2009'

				   FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE DCH AS
		SELECT
			*
		FROM 
			CONNECTION TO DB2
				( 
					SELECT DISTINCT
						PD01.DF_SPE_ACC_ID
						,DC01.AF_APL_ID
						,DC01.AF_APL_ID_SFX
						,DC18.LD_BKR_FIL
						,DC18.LD_BKR_DCH
					FROM 
						OLWHRM1.DC01_LON_CLM_INF DC01
						INNER JOIN OLWHRM1.DC18_BKR DC18
							ON DC01.AF_APL_ID = DC18.AF_APL_ID
							AND DC01.AF_APL_ID_SFX = DC18.AF_APL_ID_SFX
						INNER JOIN OLWHRM1.GA01_APP GA01
							ON DC01.AF_APL_ID = GA01.AF_APL_ID
						INNER JOIN OLWHRM1.PD01_PDM_INF PD01
							ON GA01.DF_PRS_ID_BR = PD01.DF_PRS_ID
					WHERE
						DC01.LC_AUX_STA = '01'
						AND DC18.LD_BKR_DCH >= '01/01/2009'

				   FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA BKR; SET DUSTER.BKR; RUN;
DATA DSM; SET DUSTER.DSM; RUN;
DATA DCH; SET DUSTER.DCH; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.BKR 
            OUTFILE = "T:\SAS\NH 22116.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="BANKRUPTCIES"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DSM 
            OUTFILE = "T:\SAS\NH 22116.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="DISMISSED"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DCH
            OUTFILE = "T:\SAS\NH 22116.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="DISCHARGED"; 
RUN;
