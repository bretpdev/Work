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
							WHEN LN10.LD_LON_EFF_ADD <= '09-30-2009' AND DTE.LST_SYS_DTE >= '10-01-2008' THEN 1
							ELSE 0
						 END AS FY09
						 ,CASE
							WHEN LN10.LD_LON_EFF_ADD <= '09-30-2010' AND DTE.LST_SYS_DTE >= '10-01-2009' THEN 1
							ELSE 0
						 END AS FY10
						 ,CASE
							WHEN LN10.LD_LON_EFF_ADD <= '09-30-2011' AND DTE.LST_SYS_DTE >= '10-01-2010' THEN 1
							ELSE 0
						 END AS FY11
						,CASE
							WHEN LN10.LD_LON_EFF_ADD <= '09-30-2012' AND DTE.LST_SYS_DTE >= '10-01-2011' THEN 1
							ELSE 0
						 END AS FY12
						 ,CASE
							WHEN LN10.LD_LON_EFF_ADD <= '09-30-2013' AND DTE.LST_SYS_DTE >= '10-01-2012' THEN 1
							ELSE 0
						 END AS FY13
						 ,CASE
							WHEN LN10.LD_LON_EFF_ADD <= '09-30-2014' AND DTE.LST_SYS_DTE >= '10-01-2013' THEN 1
							ELSE 0
						 END AS FY14
						 ,CASE
							WHEN LN10.LD_LON_EFF_ADD <= '09-30-2015' AND DTE.LST_SYS_DTE >= '10-01-2014' THEN 1
							ELSE 0
						 END AS FY15
						 ,CASE
							WHEN LN10.LD_LON_EFF_ADD <= '12-31-2015' AND DTE.LST_SYS_DTE >= '10-01-2015' THEN 1
							ELSE 0
						 END AS FY16
						 ,CASE
							WHEN MIL.LD_ITR_EFF_BEG <= '09-30-2009' AND MIL.LD_ITR_EFF_END >= '10-01-2008' THEN 1
							ELSE 0
						 END AS MIL09
						 ,CASE
							WHEN MIL.LD_ITR_EFF_BEG <= '09-30-2010' AND MIL.LD_ITR_EFF_END >= '10-01-2009' THEN 1
							ELSE 0
						 END AS MIL10
						 ,CASE
							WHEN MIL.LD_ITR_EFF_BEG <= '09-30-2011' AND MIL.LD_ITR_EFF_END >= '10-01-2010' THEN 1
							ELSE 0
						 END AS MIL11
						 ,CASE
							WHEN MIL.LD_ITR_EFF_BEG <= '09-30-2012' AND MIL.LD_ITR_EFF_END >= '10-01-2011' THEN 1
							ELSE 0
						 END AS MIL12
						 ,CASE
							WHEN MIL.LD_ITR_EFF_BEG <= '09-30-2013' AND MIL.LD_ITR_EFF_END >= '10-01-2012' THEN 1
							ELSE 0
						 END AS MIL13
						 ,CASE
							WHEN MIL.LD_ITR_EFF_BEG <= '09-30-2014' AND MIL.LD_ITR_EFF_END >= '10-01-2013' THEN 1
							ELSE 0
						 END AS MIL14
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
										WHEN LN10.LD_PIF_RPT IS NOT NULL THEN LN10.LD_PIF_RPT
										WHEN LN10.LC_STA_LON10 = 'D' AND LN10.LD_PIF_RPT IS NULL THEN LN10.LD_STA_LON10
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
						LN10.IC_LON_PGM ^= 'COMPLT'

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET DUSTER.DEMO; RUN;

PROC SQL;
	CREATE TABLE SMRY AS
		SELECT
			COUNT(DISTINCT Y09.DF_SPE_ACC_ID) AS FY09_BOR
			,SUM(D.FY09) AS FY09_LNS
			,COUNT(DISTINCT Y10.DF_SPE_ACC_ID) AS FY10_BOR
			,SUM(D.FY10) AS FY10_LNS
			,COUNT(DISTINCT Y11.DF_SPE_ACC_ID) AS FY11_BOR
			,SUM(D.FY11) AS FY11_LNS
			,COUNT(DISTINCT Y12.DF_SPE_ACC_ID) AS FY12_BOR
			,SUM(D.FY12) AS FY12_LNS
			,COUNT(DISTINCT Y13.DF_SPE_ACC_ID) AS FY13_BOR
			,SUM(D.FY13) AS FY13_LNS
			,COUNT(DISTINCT Y14.DF_SPE_ACC_ID) AS FY14_BOR
			,SUM(D.FY14) AS FY14_LNS
			,COUNT(DISTINCT Y15.DF_SPE_ACC_ID) AS FY15_BOR
			,SUM(D.FY15) AS FY15_LNS
			,COUNT(DISTINCT Y16.DF_SPE_ACC_ID) AS FY16_BOR
			,SUM(D.FY16) AS FY16_LNS
			,COUNT(DISTINCT M09.DF_SPE_ACC_ID) AS MIL09_BOR
			,SUM(D.MIL09) AS MIL09_LNS
			,COUNT(DISTINCT M10.DF_SPE_ACC_ID) AS MIL10_BOR
			,SUM(D.MIL10) AS MIL10_LNS
			,COUNT(DISTINCT M11.DF_SPE_ACC_ID) AS MIL11_BOR
			,SUM(D.MIL11) AS MIL11_LNS
			,COUNT(DISTINCT M12.DF_SPE_ACC_ID) AS MIL12_BOR
			,SUM(D.MIL12) AS MIL12_LNS
			,COUNT(DISTINCT M13.DF_SPE_ACC_ID) AS MIL13_BOR
			,SUM(D.MIL13) AS MIL13_LNS
			,COUNT(DISTINCT M14.DF_SPE_ACC_ID) AS MIL14_BOR
			,SUM(D.MIL14) AS MIL14_LNS
			,COUNT(DISTINCT M15.DF_SPE_ACC_ID) AS MIL15_BOR
			,SUM(D.MIL15) AS MIL15_LNS
			,COUNT(DISTINCT M16.DF_SPE_ACC_ID) AS MIL16_BOR
			,SUM(D.MIL16) AS MIL16_LNS
		FROM DEMO D
			LEFT JOIN (
					SELECT DISTINCT
						D.DF_SPE_ACC_ID
					FROM DEMO D
					WHERE D.FY09 = 1
						) Y09
				ON D.DF_SPE_ACC_ID = Y09.DF_SPE_ACC_ID
			LEFT JOIN (
					SELECT DISTINCT
						D.DF_SPE_ACC_ID
					FROM DEMO D
					WHERE D.FY10 = 1
						) Y10
				ON D.DF_SPE_ACC_ID = Y10.DF_SPE_ACC_ID
			LEFT JOIN (
					SELECT DISTINCT
						D.DF_SPE_ACC_ID
					FROM DEMO D
					WHERE D.FY11 = 1
						) Y11
				ON D.DF_SPE_ACC_ID = Y11.DF_SPE_ACC_ID
			LEFT JOIN (
					SELECT DISTINCT
						D.DF_SPE_ACC_ID
					FROM DEMO D
					WHERE D.FY12 = 1
						) Y12
				ON D.DF_SPE_ACC_ID = Y12.DF_SPE_ACC_ID
			LEFT JOIN (
					SELECT DISTINCT
						D.DF_SPE_ACC_ID
					FROM DEMO D
					WHERE D.FY13 = 1
						) Y13
				ON D.DF_SPE_ACC_ID = Y13.DF_SPE_ACC_ID
			LEFT JOIN (
					SELECT DISTINCT
						D.DF_SPE_ACC_ID
					FROM DEMO D
					WHERE FY14 = 1
						) Y14
				ON D.DF_SPE_ACC_ID = Y14.DF_SPE_ACC_ID
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
					WHERE D.MIL09 = 1
						) M09
				ON D.DF_SPE_ACC_ID = M09.DF_SPE_ACC_ID
			LEFT JOIN (
					SELECT DISTINCT
						D.DF_SPE_ACC_ID
					FROM DEMO D
					WHERE D.MIL10 = 1
						) M10
				ON D.DF_SPE_ACC_ID = M10.DF_SPE_ACC_ID
			LEFT JOIN (
					SELECT DISTINCT
						D.DF_SPE_ACC_ID
					FROM DEMO D
					WHERE D.MIL11 = 1
						) M11
				ON D.DF_SPE_ACC_ID = M11.DF_SPE_ACC_ID
			LEFT JOIN (
					SELECT DISTINCT
						D.DF_SPE_ACC_ID
					FROM DEMO D
					WHERE D.MIL12 = 1
						) M12
				ON D.DF_SPE_ACC_ID = M12.DF_SPE_ACC_ID
			LEFT JOIN (
					SELECT DISTINCT
						D.DF_SPE_ACC_ID
					FROM DEMO D
					WHERE D.MIL13 = 1
						) M13
				ON D.DF_SPE_ACC_ID = M13.DF_SPE_ACC_ID
			LEFT JOIN (
					SELECT DISTINCT
						D.DF_SPE_ACC_ID
					FROM DEMO D
					WHERE D.MIL14 = 1
						) M14
				ON D.DF_SPE_ACC_ID = M14.DF_SPE_ACC_ID
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

DATA DEMO1 DEMO2;
	SET DEMO;
	IF DF_SPE_ACC_ID =: '0' 
		THEN OUTPUT DEMO1;
	IF DF_SPE_ACC_ID =: '1' 
		THEN OUTPUT DEMO1;
	IF DF_SPE_ACC_ID =: '2' 
		THEN OUTPUT DEMO1;
	IF DF_SPE_ACC_ID =: '3' 
		THEN OUTPUT DEMO1;
	IF DF_SPE_ACC_ID =: '4' 
		THEN OUTPUT DEMO1;
	IF DF_SPE_ACC_ID =: '5' 
		THEN OUTPUT DEMO2;
	IF DF_SPE_ACC_ID =: '6' 
		THEN OUTPUT DEMO2;
	IF DF_SPE_ACC_ID =: '7' 
		THEN OUTPUT DEMO2;
	IF DF_SPE_ACC_ID =: '8' 
		THEN OUTPUT DEMO2;
	IF DF_SPE_ACC_ID =: '9' 
		THEN OUTPUT DEMO2;
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.SMRY 
            OUTFILE = "T:\SAS\NH 25909 v2.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SUMMARY";  
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO1 
            OUTFILE = "T:\SAS\NH 25909 v2.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="DETAIL1"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO2 
            OUTFILE = "T:\SAS\NH 25909 v2.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="DETAIL2"; 
RUN;

