%LET RPTLIB = T:\UNH 72515.xlsx;				
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;				
RSUBMIT;				
				
PROC SQL;				
	CONNECT TO DB2 (DATABASE=DLGSUTWH);			
				
	CREATE TABLE LP01 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP01_CAP_OPT_RUL);		
	CREATE TABLE LP02 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP02_DFR_PAR	);	
	CREATE TABLE LP03 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP03_RDI		);
	CREATE TABLE LP04 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP04_UPD_SQR_ORD);		
	CREATE TABLE LP05 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP05_FOR_PAR	);	
	CREATE TABLE LP06_1 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP06_ITR_AND_TYP WHERE YEAR(PF_LST_DTS_LP06) < 2017);	
	CREATE TABLE LP06_2 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP06_ITR_AND_TYP WHERE YEAR(PF_LST_DTS_LP06) >= 2017);		
	CREATE TABLE LP08 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP08_PAY_APL_PAR);		
	CREATE TABLE LP09 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP09_OWN_DSB_PAR);		
	CREATE TABLE LP10 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP10_RPY_PAR	);	
	CREATE TABLE LP11 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP11_WUP_RFD_PAR);		
	CREATE TABLE LP12 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP12_GTR_DSB_PAR);		
	CREATE TABLE LP13 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP13_DD_ACT_STA	);	
	CREATE TABLE LP14 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP14_DD_ACT_WDO	);	
	CREATE TABLE LP15 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP15_DD_ACT_STE	);	
	CREATE TABLE LP17 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP17_STP_PUR	);	
	CREATE TABLE LP18 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP18_DD_CYC_PAR	);	
	CREATE TABLE LP19 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP19_DD_SKP_PAR	);	
	CREATE TABLE LP20 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP20_GTR_OWN_FEE);		
	CREATE TABLE LP21 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP21_TIR_SCL_FEE);		
	CREATE TABLE LP23 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP23_TIR_SCL_ITR);		
	CREATE TABLE LP51 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP51_CAP_RUL_ERR);		
	CREATE TABLE LP56 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP56_HST_LPD06	);	
	CREATE TABLE LP60 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP60_RPY_PAR_ERR);		
	CREATE TABLE LP68 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP68_DD_CYC_ERR	);	
	CREATE TABLE LP70 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP70_GTR_FEE_ERR);		
				
	DISCONNECT FROM DB2;			
QUIT;				
ENDRSUBMIT;				
				
PROC EXPORT DATA = DUSTER.LP01 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;				
PROC EXPORT DATA = DUSTER.LP02 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;				
PROC EXPORT DATA = DUSTER.LP03 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;				
PROC EXPORT DATA = DUSTER.LP04 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;				
PROC EXPORT DATA = DUSTER.LP05 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;				
PROC EXPORT DATA = DUSTER.LP06_1 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;	
PROC EXPORT DATA = DUSTER.LP06_2 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;	
PROC EXPORT DATA = DUSTER.LP08 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;				
PROC EXPORT DATA = DUSTER.LP09 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;				
PROC EXPORT DATA = DUSTER.LP10 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;				
PROC EXPORT DATA = DUSTER.LP11 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;				
PROC EXPORT DATA = DUSTER.LP12 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;				
PROC EXPORT DATA = DUSTER.LP13 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;				
PROC EXPORT DATA = DUSTER.LP14 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;				
PROC EXPORT DATA = DUSTER.LP15 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;				
PROC EXPORT DATA = DUSTER.LP17 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;				
PROC EXPORT DATA = DUSTER.LP18 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;				
PROC EXPORT DATA = DUSTER.LP19 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;				
PROC EXPORT DATA = DUSTER.LP20 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;				
PROC EXPORT DATA = DUSTER.LP21 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;				
PROC EXPORT DATA = DUSTER.LP23 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;				
PROC EXPORT DATA = DUSTER.LP51 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;				
PROC EXPORT DATA = DUSTER.LP56 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;				
PROC EXPORT DATA = DUSTER.LP60 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;				
PROC EXPORT DATA = DUSTER.LP68 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;				
PROC EXPORT DATA = DUSTER.LP70 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;				
