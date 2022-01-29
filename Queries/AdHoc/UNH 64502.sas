*gets last day of prior fiscal year based on today's date;
DATA _NULL_;
	CALL SYMPUT('FYEND',"'"||PUT(INTNX('YEAR.07', TODAY(), -1, 'END'),MMDDYY10.)||"'");
RUN;
%PUT &FYEND;
%SYSLPUT FYEND = &FYEND;
%LET RPTLIB = T:\SAS;
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;
/*LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
PROC SQL;
	CONNECT TO DB2 (DATABASE=DLGSUTWH);
	CREATE TABLE DEMO AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
			(
				SELECT DISTINCT
					PD10.DF_SPE_ACC_ID AS AwardedAccount
					,YEAR(LN15.LD_DSB) AS Year
					,MONTH(LN15.LD_DSB) AS Month
				FROM	
					OLWHRM1.LN10_LON LN10
					INNER JOIN OLWHRM1.LN15_DSB LN15
						ON LN10.BF_SSN = LN15.BF_SSN
						AND LN10.LN_SEQ = LN15.LN_SEQ
						AND DAYS(LN15.LD_DSB) BETWEEN DAYS('07/01/2005') AND DAYS(&FYEND)
					LEFT JOIN OLWHRM1.SC10_SCH_DMO SC10
						ON LN10.LF_DOE_SCL_ORG = SC10.IF_DOE_SCL
					INNER JOIN OLWHRM1.PD10_PRS_NME PD10
						ON LN10.BF_SSN = PD10.DF_PRS_ID
				WHERE	
					LN10.IC_LON_PGM = 'TILP'
					AND (LN15.LA_DSB - COALESCE(LN15.LA_DSB_CAN,0)) != 0

				FOR READ ONLY WITH UR
			)
	;
	DISCONNECT FROM DB2;
QUIT;
ENDRSUBMIT;

/*After receiving this info, you need to group the output by FISCAL YEAR and then remove duplicate account numbers and count the remaining rows*/
DATA DEMO;
	SET DUSTER.DEMO;
RUN;

*counts per FY;
PROC SQL;
*FY13 (July 2012 - June 2013);
	CREATE TABLE FY2013 AS
	SELECT
		COUNT(DISTINCT AWARDEDACCOUNT) AS TOTAL
	FROM 
		DEMO
	WHERE 
		( 
			YEAR = 2012
			AND MONTH IN (7,8,9,10,11,12)
		)
		OR
		(
			YEAR = 2013
			AND MONTH IN (1,2,3,4,5,6)
		)
	;

*FY14 (July 2013 - June 2014);
	CREATE TABLE FY2014 AS
	SELECT
		COUNT(DISTINCT AWARDEDACCOUNT) AS TOTAL
	FROM 
		DEMO
	WHERE 
		( 
			YEAR = 2013
			AND MONTH IN (7,8,9,10,11,12)
		)
		OR
		(
			YEAR = 2014
			AND MONTH IN (1,2,3,4,5,6)
		)
	;

*FY15 (July 2014 - June 2015);
	CREATE TABLE FY2015 AS
	SELECT
		COUNT(DISTINCT AWARDEDACCOUNT) AS TOTAL
	FROM 
		DEMO
	WHERE 
		( 
			YEAR = 2014
			AND MONTH IN (7,8,9,10,11,12)
		)
		OR
		(
			YEAR = 2015
			AND MONTH IN (1,2,3,4,5,6)
		)
	;

*FY16 (July 2015 - June 2016);
	CREATE TABLE FY2016 AS
	SELECT 
		COUNT(DISTINCT AWARDEDACCOUNT) AS TOTAL
	FROM
		DEMO
	WHERE 
		( 
			YEAR = 2015
			AND MONTH IN (7,8,9,10,11,12)
		)
		OR
		(
			YEAR = 2016
			AND MONTH IN (1,2,3,4,5,6)
		)
	;

*FY17 (July 2016 - June 2017);
	CREATE TABLE FY2017 AS
	SELECT 
		COUNT(DISTINCT AWARDEDACCOUNT) AS TOTAL
	FROM
		DEMO
	WHERE 
		( 
			YEAR = 2016
			AND MONTH IN (7,8,9,10,11,12)
		)
		OR
		(
			YEAR = 2017
			AND MONTH IN (1,2,3,4,5,6)
		)
	;

*FY18 (July 2017 - June 2018);
	CREATE TABLE FY2018 AS
	SELECT 
		COUNT(DISTINCT AWARDEDACCOUNT) AS TOTAL
	FROM
		DEMO
	WHERE 
		( 
			YEAR = 2017
			AND MONTH IN (7,8,9,10,11,12)
		)
		OR
		(
			YEAR = 2018
			AND MONTH IN (1,2,3,4,5,6)
		)
	;

*FY18 (July 2018 - June 2019);
	CREATE TABLE FY2019 AS
	SELECT 
		COUNT(DISTINCT AWARDEDACCOUNT) AS TOTAL
	FROM
		DEMO
	WHERE 
		( 
			YEAR = 2018
			AND MONTH IN (7,8,9,10,11,12)
		)
		OR
		(
			YEAR = 2019
			AND MONTH IN (1,2,3,4,5,6)
		)
	;

*unions everything into one table;
	CREATE TABLE FY_COUNTS AS
	SELECT '2013' AS FY,* FROM FY2013
	UNION
	SELECT '2014' AS FY,* FROM FY2014
	UNION
	SELECT '2015' AS FY,* FROM FY2015
	UNION
	SELECT '2016' AS FY,* FROM FY2016
	UNION
	SELECT '2017' AS FY,* FROM FY2017
	UNION
	SELECT '2018' AS FY,* FROM FY2018
	UNION
	SELECT '2019' AS FY,* FROM FY2019
	;
QUIT;

*counts per FY quarter;
PROC SQL;
	CREATE TABLE FY19_Q1 AS
	SELECT 
		COUNT(AWARDEDACCOUNT) AS TOTAL
	FROM
		DEMO
	WHERE 
		YEAR = 2018
		AND MONTH IN (7,8,9)
	;
	CREATE TABLE FY19_Q2 AS
	SELECT 
		COUNT(AWARDEDACCOUNT) AS TOTAL
	FROM
		DEMO
	WHERE 
		YEAR = 2018
		AND MONTH IN (10,11,12)
	;
	CREATE TABLE FY19_Q3 AS
	SELECT 
		COUNT(AWARDEDACCOUNT) AS TOTAL
	FROM
		DEMO
	WHERE 
		YEAR = 2019
		AND MONTH IN (1,2,3)
	;
	CREATE TABLE FY19_Q4 AS
	SELECT 
		COUNT(AWARDEDACCOUNT) AS TOTAL
	FROM
		DEMO
	WHERE 
		YEAR = 2019
		AND MONTH IN (4,5,6)
	;
*unions everything into one table;
	CREATE TABLE FY19_QUARTER_COUNTS AS
	SELECT 'Q1' AS FY19,* FROM FY19_Q1
	UNION                         
	SELECT 'Q2' AS FY19,* FROM FY19_Q2
	UNION                         
	SELECT 'Q3' AS FY19,* FROM FY19_Q3
	UNION                         
	SELECT 'Q4' AS FY19,* FROM FY19_Q4
	;
QUIT;

/*export to Excel spreadsheet*/
PROC EXPORT 
	DATA = WORK.DEMO 
	OUTFILE = "T:\UNH 54062.xlsx" 
	DBMS = EXCEL
	REPLACE;
	SHEET="Awardees"; 
RUN;

PROC EXPORT 
	DATA = WORK.FY_COUNTS
	OUTFILE = "T:\UNH 54062.xlsx" 
	DBMS = EXCEL
	REPLACE;
	SHEET="FY_counts";
RUN;

PROC EXPORT 
	DATA = WORK.FY19_QUARTER_COUNTS
	OUTFILE = "T:\UNH 54062.xlsx" 
	DBMS = EXCEL
	REPLACE;
	SHEET="FY19_quarter_counts";
RUN;