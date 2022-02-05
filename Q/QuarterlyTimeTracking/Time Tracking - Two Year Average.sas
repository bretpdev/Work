*get allocations data from archive;
%LET FILEPATH = Q:\Support Services\Time tracking\ARCHIVE\Quarterly Time Tracking;

LIBNAME TIMESHT "&FILEPATH\FY18 Q3\Allocations.xlsx";
DATA FY18Q3;
	Quarter = 'FY18Q3';
	SET TIMESHT.Final;
RUN;
LIBNAME TIMESHT CLEAR;
LIBNAME TIMESHT "&FILEPATH\FY18 Q4\Allocations FY 2018 Q4- Manual Reallocation vs3 .xlsx";
DATA FY18Q4;
	Quarter = 'FY18Q4';
	SET TIMESHT.Final;
RUN;
LIBNAME TIMESHT CLEAR;
LIBNAME TIMESHT "&FILEPATH\FY19 Q1\Allocations.xlsx";
DATA FY19Q1;
	Quarter = 'FY19Q1';
	SET TIMESHT.Final;
RUN;
LIBNAME TIMESHT CLEAR;
LIBNAME TIMESHT "&FILEPATH\FY19 Q2\Allocations.xlsx";
DATA FY19Q2;
	Quarter = 'FY19Q2';
	SET TIMESHT.Final;
RUN;
LIBNAME TIMESHT CLEAR;
LIBNAME TIMESHT "&FILEPATH\FY19 Q3\Final Run\Allocations.xlsx";
DATA FY19Q3;
	Quarter = 'FY19Q3';
	SET TIMESHT.Final;
RUN;
LIBNAME TIMESHT CLEAR;
LIBNAME TIMESHT "&FILEPATH\FY19 Q4\Allocations.xlsx";
DATA FY19Q4;
	Quarter = 'FY19Q4';
	SET TIMESHT.Final;
RUN;
LIBNAME TIMESHT CLEAR;
LIBNAME TIMESHT "&FILEPATH\FY20 Q1\Final Results\Allocations.xlsx";
DATA FY20Q1;
	Quarter = 'FY20Q1';
	SET TIMESHT.Final;
RUN;
LIBNAME TIMESHT CLEAR;
LIBNAME TIMESHT "&FILEPATH\FY20 Q2\Allocations.xlsx";
DATA FY20Q2;
	Quarter = 'FY20Q2';
	SET TIMESHT.Final;
RUN;
LIBNAME TIMESHT CLEAR;

PROC SQL NOPRINT;
	CREATE TABLE _TWOYEAR AS
	SELECT * FROM
	(
		SELECT * FROM FY18Q3 UNION ALL
		SELECT * FROM FY18Q4 UNION ALL
		SELECT * FROM FY19Q1 UNION ALL
		SELECT * FROM FY19Q2 UNION ALL
		SELECT * FROM FY19Q3 UNION ALL
		SELECT * FROM FY19Q4 UNION ALL
		SELECT * FROM FY20Q1 UNION ALL
		SELECT * FROM FY20Q2
	)
	ORDER BY 
		COSTCENTERID;
QUIT;

PROC SORT DATA=_TWOYEAR;
	BY COSTCENTER;
RUN;

PROC FREQ NOPRINT;
	TABLES COSTCENTER / OUT=COSTCENTERCOUNTS;
RUN;

PROC SQL NOPRINT;
	CREATE TABLE TWOYEAR AS
	SELECT
		CCC.COUNT AS CountQuartersBilled
		,TY.*
	FROM
		_TWOYEAR TY
		LEFT JOIN COSTCENTERCOUNTS CCC
			ON CCC.CostCenter = TY.CostCenter;
QUIT;

*final ouptut;
PROC SQL NOPRINT;
	CREATE TABLE TWOYEARAVERAGE AS
	SELECT
/*		CostCenterId,*/
		CostCenter,
		CountQuartersBilled,
		SUM(Hours) AS TotalHours,
		SUM(Hours) / CountQuartersBilled AS AverageForQuartersBilled,
		SUM(Hours) / 8 AS TwoYearAverage
	FROM
		TWOYEAR
	GROUP BY
/*		CostCenterId,*/
		CostCenter,
		CountQuartersBilled;
QUIT;

PROC EXPORT
		DATA=TWOYEARAVERAGE
		OUTFILE="T:\SAS\TWOYEARAVERAGE.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="2YrAvg";
RUN;
