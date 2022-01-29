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
					SELECT 
						LN10.BF_SSN
						,LN10.LN_SEQ
						,LN10.LA_CUR_PRI
						,CASE
							WHEN DW01.BF_SSN IS NOT NULL AND DW01.LN_SEQ IS NOT NULL THEN 'Y'
							ELSE ' '
						 END AS STATI
						,DW01.WC_DW_LON_STA
						,CASE
							WHEN (LN65A.BF_SSN IS NOT NULL AND LN65A.LN_SEQ IS NOT NULL) OR 
								 (LN65I.BF_SSN IS NOT NULL AND LN65I.LN_SEQ IS NOT NULL) THEN 'Y'
							ELSE ' '
						 END AS IDR
						 ,LN65A.LC_TYP_SCH_DIS AS ACTIVE_LC_TYP_SCH_DIS
						 ,LN65I.LC_TYP_SCH_DIS AS INACTIVE_LC_TYP_SCH_DIS
					FROM
						OLWHRM1.LN10_LON LN10
						LEFT JOIN 
							OLWHRM1.DW01_DW_CLC_CLU DW01
							ON LN10.BF_SSN = DW01.BF_SSN
							AND LN10.LN_SEQ = DW01.LN_SEQ
							AND DW01.WC_DW_LON_STA NOT IN ('01','02','06','12','17','21','22','23')
						LEFT JOIN (
								SELECT
									LN65.BF_SSN
									,LN65.LN_SEQ
									,LN65.LC_TYP_SCH_DIS
								FROM 
									OLWHRM1.LN65_LON_RPS LN65
								WHERE 
									LN65.LC_STA_LON65 = 'A'
									AND 
									LN65.LC_TYP_SCH_DIS IN ('CQ','C1','C2','C3','IB','IL','IS','I3','CA','CP','IP')
									) LN65A
							ON LN10.BF_SSN = LN65A.BF_SSN
							AND LN10.LN_SEQ = LN65A.LN_SEQ
						LEFT JOIN (
								SELECT
									LN65.BF_SSN
									,LN65.LN_SEQ
									,LN65.LC_TYP_SCH_DIS
								FROM 
									OLWHRM1.LN65_LON_RPS LN65
								JOIN (
										SELECT
											LN65.BF_SSN
											,LN65.LN_SEQ
											,MAX(LN65.LN_RPS_SEQ) AS LN_RPS_SEQ
										FROM 
											OLWHRM1.LN65_LON_RPS LN65
											JOIN (
													SELECT
														LN65.BF_SSN
														,LN65.LN_SEQ
													FROM 
														OLWHRM1.LN65_LON_RPS LN65
													WHERE 
														LN65.LC_STA_LON65 = 'A'
													) ACT
												ON LN65.BF_SSN = ACT.BF_SSN
												AND LN65.LN_SEQ = ACT.LN_SEQ
										WHERE 
											ACT.BF_SSN IS NULL 
											AND 
											ACT.LN_SEQ IS NULL
										GROUP BY
											LN65.BF_SSN
											,LN65.LN_SEQ
											) INACT
									ON LN65.BF_SSN = INACT.BF_SSN
									AND LN65.LN_SEQ = INACT.LN_SEQ
									AND LN65.LN_RPS_SEQ = INACT.LN_RPS_SEQ
								WHERE 
									LN65.LC_TYP_SCH_DIS IN ('CQ','C1','C2','C3','IB','IL','IS','I3','CA','CP','IP')
									) LN65I
							ON LN10.BF_SSN = LN65I.BF_SSN
							AND LN10.LN_SEQ = LN65I.LN_SEQ
					WHERE
						LN10.LA_CUR_PRI > 0
						AND LN10.LC_STA_LON10 = 'R'

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET DUSTER.DEMO; RUN;

PROC SORT DATA=DEMO; BY BF_SSN LN_SEQ; RUN;

PROC SQL;
	CREATE TABLE TOTAL AS
		SELECT
			SUM(LA_CUR_PRI) AS TOTAL_PRINCIPAL_BALANCE
		FROM 
			DEMO
	;

	CREATE TABLE CERTAIN_STATUSES AS
		SELECT
			SUM(LA_CUR_PRI) AS TOTAL_BALANCE_SELECTED_STATUSES
		FROM 
			DEMO
		WHERE 
			STATI = 'Y'
	;

	CREATE TABLE IDR AS
		SELECT
			SUM(LA_CUR_PRI) AS TOTAL_BALANCE_IDR
		FROM 
			DEMO
		WHERE 
			IDR = 'Y'
	;
QUIT;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.TOTAL 
            OUTFILE = "T:\SAS\NH 23886.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="TOTAL"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.CERTAIN_STATUSES 
            OUTFILE = "T:\SAS\NH 23886.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SELECTED_STATUSES"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.IDR 
            OUTFILE = "T:\SAS\NH 23886.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="IDR"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NH 23886 TEST.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
