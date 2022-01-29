/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE X  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @XX " ********************************************************************* "
              / @XX " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @XX " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @XX " ********************************************************************* "
              / @XX " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @XX " ****  &SQLXMSG   **** "
              / @XX " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						PDXX.DF_SPE_ACC_ID
						,LNXX.LN_SEQ
						,LNXX.LD_LON_EFF_ADD
						,DTE.LST_SYS_DTE
						,MIL.LD_ITR_EFF_BEG
						,MIL.LD_ITR_EFF_END
						,CASE
							WHEN LNXX.LD_LON_EFF_ADD <= 'XX-XX-XXXX' AND DTE.LST_SYS_DTE >= 'XX-XX-XXXX' THEN X
							ELSE X
						 END AS FYXX
						 ,CASE
							WHEN LNXX.LD_LON_EFF_ADD <= 'XX-XX-XXXX' AND DTE.LST_SYS_DTE >= 'XX-XX-XXXX' THEN X
							ELSE X
						 END AS FYXX
						 ,CASE
							WHEN LNXX.LD_LON_EFF_ADD <= 'XX-XX-XXXX' AND DTE.LST_SYS_DTE >= 'XX-XX-XXXX' THEN X
							ELSE X
						 END AS FYXX
						 ,CASE
							WHEN LNXX.LD_LON_EFF_ADD <= 'XX-XX-XXXX' AND DTE.LST_SYS_DTE >= 'XX-XX-XXXX' THEN X
							ELSE X
						 END AS FYXX
						 ,CASE
							WHEN LNXX.LD_LON_EFF_ADD <= 'XX-XX-XXXX' AND DTE.LST_SYS_DTE >= 'XX-XX-XXXX' THEN X
							ELSE X
						 END AS FYXX
						 ,CASE
							WHEN MIL.LD_ITR_EFF_BEG <= 'XX-XX-XXXX' AND MIL.LD_ITR_EFF_END >= 'XX-XX-XXXX' THEN X
							ELSE X
						 END AS MILXX
						 ,CASE
							WHEN MIL.LD_ITR_EFF_BEG <= 'XX-XX-XXXX' AND MIL.LD_ITR_EFF_END >= 'XX-XX-XXXX' THEN X
							ELSE X
						 END AS MILXX
						 ,CASE
							WHEN MIL.LD_ITR_EFF_BEG <= 'XX-XX-XXXX' AND MIL.LD_ITR_EFF_END >= 'XX-XX-XXXX' THEN X
							ELSE X
						 END AS MILXX
						 ,CASE
							WHEN MIL.LD_ITR_EFF_BEG <= 'XX-XX-XXXX' AND MIL.LD_ITR_EFF_END >= 'XX-XX-XXXX' THEN X
							ELSE X
						 END AS MILXX
						 ,CASE
							WHEN MIL.LD_ITR_EFF_BEG <= 'XX-XX-XXXX' AND MIL.LD_ITR_EFF_END >= 'XX-XX-XXXX' THEN X
							ELSE X
						 END AS MILXX
					FROM
						PKUB.LNXX_LON LNXX
						JOIN (
								SELECT
									LNXX.BF_SSN
									,LNXX.LN_SEQ
									,CASE
										WHEN LNXX.LD_PIF_RPT IS NOT NULL THEN LNXX.LD_PIF_RPT
										WHEN LNXX.LC_STA_LONXX = 'D' AND LNXX.LD_PIF_RPT IS NULL THEN LNXX.LD_STA_LONXX
										ELSE CURRENT_DATE
									 END AS LST_SYS_DTE
								FROM PKUB.LNXX_LON LNXX
									) DTE
							ON LNXX.BF_SSN = DTE.BF_SSN
							AND LNXX.LN_SEQ = DTE.LN_SEQ
						LEFT JOIN (
								SELECT
									LNXX.BF_SSN
									,LNXX.LN_SEQ
									,LNXX.LD_ITR_EFF_BEG
									,LNXX.LD_ITR_EFF_END
								FROM PKUB.LNXX_INT_RTE_HST LNXX
								WHERE LNXX.LC_INT_RDC_PGM = 'M'
										AND LNXX.LC_STA_LONXX = 'A'
									) MIL
							ON LNXX.BF_SSN = MIL.BF_SSN
							AND LNXX.LN_SEQ = MIL.LN_SEQ
						JOIN 
							PKUB.PDXX_PRS_NME PDXX
							ON LNXX.BF_SSN = PDXX.DF_PRS_ID

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

PROC SQL;
	CREATE TABLE SMRY AS
		SELECT
			COUNT(DISTINCT YXX.DF_SPE_ACC_ID) AS FYXX_BOR
			,SUM(D.FYXX) AS FYXX_LNS
			,COUNT(DISTINCT YXX.DF_SPE_ACC_ID) AS FYXX_BOR
			,SUM(D.FYXX) AS FYXX_LNS
			,COUNT(DISTINCT YXX.DF_SPE_ACC_ID) AS FYXX_BOR
			,SUM(D.FYXX) AS FYXX_LNS
			,COUNT(DISTINCT YXX.DF_SPE_ACC_ID) AS FYXX_BOR
			,SUM(D.FYXX) AS FYXX_LNS
			,COUNT(DISTINCT YXX.DF_SPE_ACC_ID) AS FYXX_BOR
			,SUM(D.FYXX) AS FYXX_LNS
			,COUNT(DISTINCT MXX.DF_SPE_ACC_ID) AS MILXX_BOR
			,SUM(D.MILXX) AS MILXX_LNS
			,COUNT(DISTINCT MXX.DF_SPE_ACC_ID) AS MILXX_BOR
			,SUM(D.MILXX) AS MILXX_LNS
			,COUNT(DISTINCT MXX.DF_SPE_ACC_ID) AS MILXX_BOR
			,SUM(D.MILXX) AS MILXX_LNS
			,COUNT(DISTINCT MXX.DF_SPE_ACC_ID) AS MILXX_BOR
			,SUM(D.MILXX) AS MILXX_LNS
			,COUNT(DISTINCT MXX.DF_SPE_ACC_ID) AS MILXX_BOR
			,SUM(D.MILXX) AS MILXX_LNS
		FROM DEMO D
			LEFT JOIN (
					SELECT DISTINCT
						D.DF_SPE_ACC_ID
					FROM DEMO D
					WHERE D.FYXX = X
						) YXX
				ON D.DF_SPE_ACC_ID = YXX.DF_SPE_ACC_ID
			LEFT JOIN (
					SELECT DISTINCT
						D.DF_SPE_ACC_ID
					FROM DEMO D
					WHERE D.FYXX = X
						) YXX
				ON D.DF_SPE_ACC_ID = YXX.DF_SPE_ACC_ID
			LEFT JOIN (
					SELECT DISTINCT
						D.DF_SPE_ACC_ID
					FROM DEMO D
					WHERE FYXX = X
						) YXX
				ON D.DF_SPE_ACC_ID = YXX.DF_SPE_ACC_ID
			LEFT JOIN (
					SELECT DISTINCT
						D.DF_SPE_ACC_ID
					FROM DEMO D
					WHERE D.FYXX = X
						) YXX
				ON D.DF_SPE_ACC_ID = YXX.DF_SPE_ACC_ID
			LEFT JOIN (
					SELECT DISTINCT
						D.DF_SPE_ACC_ID
					FROM DEMO D
					WHERE D.FYXX = X
						) YXX
				ON D.DF_SPE_ACC_ID = YXX.DF_SPE_ACC_ID
			LEFT JOIN (
					SELECT DISTINCT
						D.DF_SPE_ACC_ID
					FROM DEMO D
					WHERE D.MILXX = X
						) MXX
				ON D.DF_SPE_ACC_ID = MXX.DF_SPE_ACC_ID
			LEFT JOIN (
					SELECT DISTINCT
						D.DF_SPE_ACC_ID
					FROM DEMO D
					WHERE D.MILXX = X
						) MXX
				ON D.DF_SPE_ACC_ID = MXX.DF_SPE_ACC_ID
			LEFT JOIN (
					SELECT DISTINCT
						D.DF_SPE_ACC_ID
					FROM DEMO D
					WHERE D.MILXX = X
						) MXX
				ON D.DF_SPE_ACC_ID = MXX.DF_SPE_ACC_ID
			LEFT JOIN (
					SELECT DISTINCT
						D.DF_SPE_ACC_ID
					FROM DEMO D
					WHERE D.MILXX = X
						) MXX
				ON D.DF_SPE_ACC_ID = MXX.DF_SPE_ACC_ID
			LEFT JOIN (
					SELECT DISTINCT
						D.DF_SPE_ACC_ID
					FROM DEMO D
					WHERE D.MILXX = X
						) MXX
				ON D.DF_SPE_ACC_ID = MXX.DF_SPE_ACC_ID
	;
QUIT;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.SMRY 
            OUTFILE = "T:\SAS\NH XXXX vX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SUMMARY"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NH XXXX vX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="DETAIL"; 
RUN;
