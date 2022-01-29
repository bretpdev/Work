
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;

PROC SQL;
	CONNECT TO DB2 (DATABASE=DLGSUTWH);

	CREATE TABLE DEMO AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						TRIM(PD01.DM_PRS_1) || ' ' || TRIM(PD01.DM_PRS_LST) AS BRW_NAME,
						LN10.BF_SSN,
  						LN10.LN_SEQ,
  						LN10.IC_LON_PGM,
						LN65.LC_TYP_SCH_DIS,
						LN65.LD_CRT_LON65,
						LN65.LA_CPI_RPD_DIS,
						LN65.LA_ACR_INT_RPD

						,LN10.LD_LON_EFF_ADD
					FROM
						OLWHRM1.PD01_PDM_INF PD01
						JOIN OLWHRM1.LN10_LON LN10
							ON PD01.DF_PRS_ID = LN10.BF_SSN
						JOIN OLWHRM1.LN65_LON_RPS LN65
							ON LN10.BF_SSN = LN65.BF_SSN
							AND LN10.LN_SEQ = LN65.LN_SEQ
					WHERE
						LN10.IC_LON_PGM IN ('STFFRD','UNSTFD')
						AND LN65.LC_TYP_SCH_DIS IN ('IL', 'IB')
						AND LN65.LD_CRT_LON65 BETWEEN '10/01/2012' AND '09/30/2013'
						AND 
							(
								LN10.LD_LON_EFF_ADD <> '2013-05-16'
								OR
								(LN10.LD_LON_EFF_ADD = '2013-05-16' AND LN65.LD_CRT_LON65 > '2013-05-17' AND LN65.LD_CRT_LON65 <> '2013-06-04')
							)
					ORDER BY
						LN10.BF_SSN,
  						LN10.LN_SEQ

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

QUIT;

ENDRSUBMIT;

DATA DEMO; SET DUSTER.DEMO; RUN;

PROC EXPORT
		OUTFILE = 'T:\SAS\NHUH 17765.XLS'
		DATA = DEMO
		REPLACE
	;
QUIT;
