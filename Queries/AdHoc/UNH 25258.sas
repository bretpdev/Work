/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
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
						DC01.BF_SSN						
					FROM	
						OLWHRM1.GA10_LON_APP GA10 
						INNER JOIN 
							(
								SELECT DISTINCT
									DC01.BF_SSN,
									DC01.AF_APL_ID,
									DC01.AF_APL_ID_SFX,
									SUM(DC01.LA_CLM_PRI 
										+ DC01.LA_CLM_INT
										- DC01.LA_PRI_COL
										+ DC01.LA_INT_ACR
										+ DC02.LA_CLM_INT_ACR
										- DC01.LA_INT_COL)
									+ SUM(DC01.LA_LEG_CST_ACR) AS Total
								FROM
									OLWHRM1.DC01_LON_CLM_INF DC01
									LEFT OUTER JOIN OLWHRM1.DC02_BAL_INT DC02
										ON DC02.AF_APL_ID = DC01.AF_APL_ID
										AND DC02.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
										AND DC02.LF_CRT_DTS_DC10 = DC01.LF_CRT_DTS_DC10
								GROUP BY
									DC01.BF_SSN,
									DC01.AF_APL_ID,
									DC01.AF_APL_ID_SFX
							) DC01
							ON DC01.AF_APL_ID = GA10.AF_APL_ID
							AND DC01.AF_APL_ID_SFX = GA10.AF_APL_ID_SFX
						INNER JOIN OLWHRM1.GA01_APP GA01
		 					ON GA01.AF_APL_ID = GA10.AF_APL_ID
						INNER JOIN OLWHRM1.DC18_BKR DC18
							ON DC18.AF_APL_ID = GA10.AF_APL_ID
							AND DC18.AF_APL_ID_SFX = GA10.AF_APL_ID_SFX 
						INNER JOIN OLWHRM1.AY10_BR_LON_ATY AY10
							ON AY10.BF_SSN = DC01.BF_SSN
						INNER JOIN OLWHRM1.AY20_ATY_TXT AY20
							ON AY20.BF_SSN = AY10.BF_SSN
							AND AY20.LN_ATY_SEQ = AY20.LN_ATY_SEQ
					WHERE	
						DC18.LC_BKR_STA = '06'
						AND DC01.Total > 0
						AND AY10.PF_REQ_ACT = 'DBKRW'
						AND AY20.LX_ATY LIKE '%DISMISSED%'

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
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\NH 25258.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="Sheet1"; 
RUN;
