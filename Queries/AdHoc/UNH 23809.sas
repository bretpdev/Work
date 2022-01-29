
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWAB1.LWAB1RZ";
FILENAME REPORT2 "&RPTLIB/ULWAB1.LWAB1R2";
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;

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
         RUN;
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
						PD01.DF_SPE_ACC_ID AS AccountNumber,
						AY01.BF_CRT_DTS_AY01 AS DFDL1_Date,
						AY01.PF_ACT AS DFDL1_ARC,
						AY012.BF_CRT_DTS_AY01 AS Repayment_Arrangement_Date,
						AY012.PF_ACT AS ARC,
						BR01.BC_CLM_STA AS Claim_Status,
						SUM(DC01.LA_COL_CST_ACR) AS Collection_Cost_Accrued,
						SUM(DC02.LA_CLM_PRJ_COL_CST) AS Collection_Cost_Projected
					FROM
						OLWHRM1.DC01_LON_CLM_INF DC01
						JOIN OLWHRM1.PD01_PDM_INF PD01
							ON DC01.BF_SSN = PD01.DF_PRS_ID
						JOIN OLWHRM1.AY01_BR_ATY AY01
							ON DC01.BF_SSN = AY01.DF_PRS_ID
						LEFT JOIN OLWHRM1.AY01_BR_ATY AY012
							ON DC01.BF_SSN = AY012.DF_PRS_ID
						LEFT JOIN OLWHRM1.BR01_BR_CRF BR01
							ON DC01.BF_SSN = BR01.BF_SSN
						LEFT JOIN OLWHRM1.DC02_BAL_INT DC02
							ON DC01.AF_APL_ID = DC02.AF_APL_ID
							AND DC01.AF_APL_ID_SFX = DC02.AF_APL_ID_SFX
					WHERE
						AY01.PF_ACT = 'DLDF1'
						AND AY012.PF_ACT IN ('DD136', 'DD137', 'DRWTN')
						AND DAYS(AY012.BF_CRT_DTS_AY01) - DAYS(AY01.BF_CRT_DTS_AY01) <= 60
						AND DAYS(AY012.BF_CRT_DTS_AY01) - DAYS(AY01.BF_CRT_DTS_AY01) > 0
					GROUP BY
						PD01.DF_SPE_ACC_ID,
						AY01.BF_CRT_DTS_AY01,
						AY01.PF_ACT,
						BR01.BC_CLM_STA,
						AY012.BF_CRT_DTS_AY01,
						AY012.PF_ACT
						
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
            OUTFILE = "T:\SAS\NH 23809.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
