/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS46.NWS46RZ";
FILENAME REPORT2 "&RPTLIB/UNWS46.NWS46R2";

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER SLIBREF=work  ;
RSUBMIT DUSTER;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
%let DB = DLGSUTWH;  *This is live;

LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1;

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
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						PD10.DF_SPE_ACC_ID
						,LN10.LN_SEQ
						,LN10.LD_LON_EFF_ADD
						,DTE.LST_SYS_DTE
						,MIL.LD_ITR_EFF_BEG
						,MIL.LD_ITR_EFF_END
						 ,CASE
							WHEN LN10.LD_LON_EFF_ADD <= '09-30-2015' AND DTE.LST_SYS_DTE >= '10-01-2014' THEN 1
							ELSE 0
						 END AS FY15
						 ,CASE
							WHEN LN10.LD_LON_EFF_ADD <= '12-31-2015' AND DTE.LST_SYS_DTE >= '10-01-2015' THEN 1
							ELSE 0
						 END AS FY16
						 ,CASE
							WHEN MIL.LD_ITR_EFF_BEG <= '09-30-2015' AND MIL.LD_ITR_EFF_END >= '10-01-2014' THEN 1
							ELSE 0
						 END AS MIL15
						 ,CASE
							WHEN MIL.LD_ITR_EFF_BEG <= '12-31-2015' AND MIL.LD_ITR_EFF_END >= '10-01-2015' THEN 1
							ELSE 0
						 END AS MIL16
					FROM
						OLWHRM1.LN10_LON LN10
						JOIN (
								SELECT
									LN10.BF_SSN
									,LN10.LN_SEQ
									,CASE
										WHEN LN10.LC_STA_LON10 = 'D' THEN LN10.LD_STA_LON10
										ELSE CURRENT_DATE
									 END AS LST_SYS_DTE
								FROM OLWHRM1.LN10_LON LN10
									) DTE
							ON LN10.BF_SSN = DTE.BF_SSN
							AND LN10.LN_SEQ = DTE.LN_SEQ
						LEFT JOIN (
								SELECT
									LN72.BF_SSN
									,LN72.LN_SEQ
									,LN72.LD_ITR_EFF_BEG
									,LN72.LD_ITR_EFF_END
								FROM OLWHRM1.LN72_INT_RTE_HST LN72
								WHERE LN72.LC_INT_RDC_PGM = 'M'
										AND LN72.LC_STA_LON72 = 'A'
									) MIL
							ON LN10.BF_SSN = MIL.BF_SSN
							AND LN10.LN_SEQ = MIL.LN_SEQ
						JOIN 
							OLWHRM1.PD10_PRS_NME PD10
							ON LN10.BF_SSN = PD10.DF_PRS_ID
					WHERE
						LN10.IC_LON_PGM = 'COMPLT'

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE CUR_CMPLT AS 
		SELECT
			COUNT(DISTINCT LN10.BF_SSN) AS NUM_BORS
			,COUNT(LN10.LN_SEQ) AS NUM_LNS
			,SUM(DSB.LA_DSB) AS TOT_DSB
			,SUM(LN10.LA_CUR_PRI) AS TOT_BAL
		FROM 
			OLWHRM1.LN10_LON LN10
			JOIN (
					SELECT
						LN15.BF_SSN
						,LN15.LN_SEQ
						,SUM(LN15.LA_DSB) AS LA_DSB
					FROM 
						OLWHRM1.LN15_DSB LN15
					GROUP BY
						LN15.BF_SSN
						,LN15.LN_SEQ 
					) DSB
				ON LN10.BF_SSN = DSB.BF_SSN
				AND LN10.LN_SEQ = DSB.LN_SEQ
		WHERE 
			LN10.IC_LON_PGM = 'COMPLT'
	;
	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET DUSTER.DEMO; RUN;
DATA CUR_CMPLT; SET DUSTER.CUR_CMPLT; RUN;

PROC SQL;
	CREATE TABLE SMRY AS
		SELECT
			,COUNT(DISTINCT Y15.DF_SPE_ACC_ID) AS FY15_BOR
			,SUM(D.FY15) AS FY15_LNS
			,COUNT(DISTINCT Y16.DF_SPE_ACC_ID) AS FY16_BOR
			,SUM(D.FY16) AS FY16_LNS
			,COUNT(DISTINCT M15.DF_SPE_ACC_ID) AS MIL15_BOR
			,SUM(D.MIL15) AS MIL15_LNS
			,COUNT(DISTINCT M16.DF_SPE_ACC_ID) AS MIL16_BOR
			,SUM(D.MIL16) AS MIL16_LNS
		FROM DEMO D
			LEFT JOIN (
					SELECT DISTINCT
						D.DF_SPE_ACC_ID
					FROM DEMO D
					WHERE D.FY15 = 1
						) Y15
				ON D.DF_SPE_ACC_ID = Y15.DF_SPE_ACC_ID
			LEFT JOIN (
					SELECT DISTINCT
						D.DF_SPE_ACC_ID
					FROM DEMO D
					WHERE D.FY16 = 1
						) Y16
				ON D.DF_SPE_ACC_ID = Y16.DF_SPE_ACC_ID
			LEFT JOIN (
					SELECT DISTINCT
						D.DF_SPE_ACC_ID
					FROM DEMO D
					WHERE D.MIL15 = 1
						) M15
				ON D.DF_SPE_ACC_ID = M15.DF_SPE_ACC_ID
			LEFT JOIN (
					SELECT DISTINCT
						D.DF_SPE_ACC_ID
					FROM DEMO D
					WHERE D.MIL16 = 1
						) M16
				ON D.DF_SPE_ACC_ID = M16.DF_SPE_ACC_ID
	;
QUIT;

/*Setting military borrowers and loans at zero from query results above*/
DATA CUR_CMPLT;
	SET CUR_CMPLT;
	BORROWERS_WITH_MILITARY_RATE = 0;
	LOANS_WITH_MILITARY_RATE = 0;
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.CUR_CMPLT 
            OUTFILE = "T:\SAS\NH 25916.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SUMMARY";  
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NH 25916.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="DETAIL"; 
RUN;

