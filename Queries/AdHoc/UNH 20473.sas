/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

PROC IMPORT OUT= WORK.LN66
            DATAFILE= "T:\LN66 DCR.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="LN66$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

DATA WORK.DATA;
	KEEP BF_SSN LN_SEQ LN_RPS_SEQ LN_GRD_RPS_SEQ Old_LA_RPS_ISL OLD_LN_RPS_TRM;
	SET LN66;
RUN;

PROC SQL ;
	CREATE TABLE DEMO AS
		SELECT DISTINCT
			D.BF_SSN,
			D.LN_SEQ,
			D.LN_RPS_SEQ,
			D.LN_GRD_RPS_SEQ,
			D.Old_LA_RPS_ISL,
			CASE
				WHEN D.LN_GRD_RPS_SEQ = 1 THEN TWOS.Old_LA_RPS_ISL
				WHEN D.LN_GRD_RPS_SEQ = 2 THEN ONES.Old_LA_RPS_ISL
				ELSE -1
			END AS NEW_LA_RPS_ISL,
			D.OLD_LN_RPS_TRM,
			CASE
				WHEN D.LN_GRD_RPS_SEQ = 2 THEN ONES.OLD_LN_RPS_TRM
				WHEN D.LN_GRD_RPS_SEQ = 1 THEN TWOS.OLD_LN_RPS_TRM
				ELSE -1
			END AS NEW_LN_RPS_TRM
		FROM
			DATA D 
			LEFT JOIN (
						SELECT
							BF_SSN,
							LN_SEQ,
							LN_RPS_SEQ,
							LN_GRD_RPS_SEQ,
							Old_LA_RPS_ISL,
							OLD_LN_RPS_TRM
						FROM
							DATA
						WHERE
							LN_GRD_RPS_SEQ = 1
					  ) ONES
				ON D.BF_SSN = ONES.BF_SSN
				AND D.LN_SEQ = ONES.LN_SEQ
			LEFT JOIN (
						SELECT
							BF_SSN,
							LN_SEQ,
							LN_RPS_SEQ,
							LN_GRD_RPS_SEQ,
							Old_LA_RPS_ISL,
							OLD_LN_RPS_TRM
						FROM
							DATA
						WHERE
							LN_GRD_RPS_SEQ = 2
					   ) TWOS
				ON D.BF_SSN = TWOS.BF_SSN
				AND D.LN_SEQ = TWOS.LN_SEQ
		ORDER BY D.BF_SSN, D.LN_GRD_RPS_SEQ, D.LN_SEQ
	;
QUIT;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO
            OUTFILE = "T:\SAS\LN66.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LN66"; 
RUN;
