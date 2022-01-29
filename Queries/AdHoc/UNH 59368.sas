%LET RPTLIB = T:\SAS\UNH 59368.xlsx;
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;

PROC SQL;
	CONNECT TO DB2 (DATABASE=DLGSUTWH);

	CREATE TABLE LP01 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP01_CAP_OPT_RUL	WHERE YEAR(PF_LST_DTS_LP01) = '2018');
	CREATE TABLE LP02 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP02_DFR_PAR		WHERE YEAR(PF_LST_DTS_LP02) = '2018');
	CREATE TABLE LP03 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP03_RDI			WHERE YEAR(PF_LST_DTS_LP03) = '2018');
	CREATE TABLE LP04 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP04_UPD_SQR_ORD	WHERE YEAR(PF_LST_DTS_LP04) = '2018');
	CREATE TABLE LP05 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP05_FOR_PAR		WHERE YEAR(PF_LST_DTS_LP05) = '2018');
	CREATE TABLE LP06 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP06_ITR_AND_TYP	WHERE YEAR(PF_LST_DTS_LP06) = '2018');
	CREATE TABLE LP08 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP08_PAY_APL_PAR	WHERE YEAR(PF_LST_DTS_LP08) = '2018');
	CREATE TABLE LP09 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP09_OWN_DSB_PAR	WHERE YEAR(PF_LST_DTS_LP09) = '2018');
	CREATE TABLE LP10 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP10_RPY_PAR		WHERE YEAR(PF_LST_DTS_LP10) = '2018');
	CREATE TABLE LP11 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP11_WUP_RFD_PAR	WHERE YEAR(PF_LST_DTS_LP11) = '2018');
	CREATE TABLE LP12 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP12_GTR_DSB_PAR	WHERE YEAR(PF_LST_DTS_LP12) = '2018');
	CREATE TABLE LP13 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP13_DD_ACT_STA		WHERE YEAR(PF_LST_DTS_LP13) = '2018');
/*	CREATE TABLE LP14 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP14_DD_ACT_WDO		WHERE YEAR(PF_LST_DTS_LP14) = '2018');*/
/*	CREATE TABLE LP15 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP15_DD_ACT_STE		WHERE YEAR(PF_LST_DTS_LP15) = '2018');*/
	CREATE TABLE LP17 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP17_STP_PUR		WHERE YEAR(PF_LST_DTS_LP17) = '2018');
/*	CREATE TABLE LP18 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP18_DD_CYC_PAR		WHERE YEAR(PF_LST_DTS_LP18) = '2018');*/
/*	CREATE TABLE LP19 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP19_DD_SKP_PAR		WHERE YEAR(PF_LST_DTS_LP19) = '2018');*/
	CREATE TABLE LP20 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP20_GTR_OWN_FEE	WHERE YEAR(PF_LST_DTS_LP20) = '2018');
/*	CREATE TABLE LP21 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP21_TIR_SCL_FEE	WHERE YEAR(PF_LST_DTS_LP21) = '2018');*/
/*	CREATE TABLE LP23 AS SELECT * FROM CONNECTION TO DB2 (SELECT * FROM OLWHRM1.LP23_TIR_SCL_ITR	WHERE YEAR(PF_LST_DTS_LP23) = '2018');*/

	DISCONNECT FROM DB2;
QUIT;
ENDRSUBMIT;

PROC EXPORT DATA = DUSTER.LP01 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;
PROC EXPORT DATA = DUSTER.LP02 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;
PROC EXPORT DATA = DUSTER.LP03 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;
PROC EXPORT DATA = DUSTER.LP04 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;
PROC EXPORT DATA = DUSTER.LP05 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;
PROC EXPORT DATA = DUSTER.LP06 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;
PROC EXPORT DATA = DUSTER.LP08 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;
PROC EXPORT DATA = DUSTER.LP09 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;
PROC EXPORT DATA = DUSTER.LP10 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;
PROC EXPORT DATA = DUSTER.LP11 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;
PROC EXPORT DATA = DUSTER.LP12 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;
PROC EXPORT DATA = DUSTER.LP13 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;
/*PROC EXPORT DATA = DUSTER.LP14 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;*/
/*PROC EXPORT DATA = DUSTER.LP15 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;*/
PROC EXPORT DATA = DUSTER.LP17 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;
/*PROC EXPORT DATA = DUSTER.LP18 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;*/
/*PROC EXPORT DATA = DUSTER.LP19 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;*/
PROC EXPORT DATA = DUSTER.LP20 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;
/*PROC EXPORT DATA = DUSTER.LP21 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;*/
/*PROC EXPORT DATA = DUSTER.LP23 OUTFILE = "&RPTLIB" DBMS = EXCEL REPLACE; RUN;*/
