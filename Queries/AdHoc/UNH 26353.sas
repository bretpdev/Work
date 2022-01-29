/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;
/*LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/

PROC SQL;
	CONNECT TO DB2 (DATABASE=DLGSUTWH);

	CREATE TABLE RS10_Change AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
							RS10.BF_SSN,
							RS10.LN_RPS_SEQ,
							RS10.LD_RPS_1_PAY_DU AS OLD_LD_RPS_1_PAY_DU,
							LN09.LD_NPD_PCV AS NEW_LD_RPS_1_PAY_DU
					FROM	
						OLWHRM1.RS10_BR_RPD RS10
						INNER JOIN OLWHRM1.LN10_LON LN10
							ON LN10.BF_SSN = RS10.BF_SSN
						INNER JOIN OLWHRM1.LN09_RPD_PIO_CVN LN09
							ON LN10.BF_SSN = LN09.BF_SSN
							AND LN10.LN_SEQ = LN09.LN_SEQ
					WHERE 
						LN10.LF_LON_CUR_OWN LIKE '829769%'
						AND LN10.LC_STA_LON10 = 'P'
						AND LN09.LD_NPD_PCV <= '03/18/2016'
					FOR READ ONLY WITH UR
				)
	;
	
	CREATE TABLE LN66_Change AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						LN66.BF_SSN,
						LN66.LN_SEQ,
						LN66.LN_RPS_SEQ,
						LN66.LN_GRD_RPS_SEQ,
						LN66.LN_RPS_TRM AS OLD_LN_RPS_TRM,
						LN66.LN_RPS_TRM+CEIL((DAYS('03/18/2016') - DAYS(LN09.LD_NPD_PCV)) / 31) AS NEW_LN_RPS_TRM
					FROM	
						OLWHRM1.LN66_LON_RPS_SPF LN66
						INNER JOIN OLWHRM1.LN10_LON LN10
							ON LN10.BF_SSN = LN66.BF_SSN
							AND LN10.LN_SEQ = LN66.LN_SEQ
						INNER JOIN OLWHRM1.LN09_RPD_PIO_CVN LN09
							ON LN10.BF_SSN = LN09.BF_SSN
							AND LN10.LN_SEQ = LN09.LN_SEQ
					WHERE
						LN10.LF_LON_CUR_OWN LIKE '829769%'
						AND LN10.LC_STA_LON10 = 'P'
						AND LN66.LN_GRD_RPS_SEQ = 1
						AND LN09.LD_NPD_PCV <= '03/18/2016'

					FOR READ ONLY WITH UR
				)
	;
ENDRSUBMIT;
DATA RS10_Change;
	SET DUSTER.RS10_Change;
RUN;
DATA LN66_Change;
	SET DUSTER.LN66_Change;
RUN;

PROC EXPORT DATA = RS10_Change 
            OUTFILE = "T:\SAS\UNH 26353.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RS10_Change"; 
RUN;
PROC EXPORT DATA = LN66_Change 
            OUTFILE = "T:\SAS\UNH 26353.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LN66_Change"; 
RUN;

