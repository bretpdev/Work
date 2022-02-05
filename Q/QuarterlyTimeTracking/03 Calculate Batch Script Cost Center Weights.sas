LIBNAME BSYS ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\BSYS.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME CSYS ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\CSYS.dsn; update_lock_typ=nolock; bl_keepnulls=no");

%LET EndDate = '30apr2021'd; *last day of quarter being reported;

LIBNAME XL 'T:\SAS\Batch Script Time Tracking Average.xlsx';

DATA BatchScriptTimeTracking;
	SET XL.'Sheet1$'N (DROP=F1 Time_Tracking_Id);
	Time_Tracking_Id = _N_;
	WHERE Sacker_Name NE '';
RUN;

LIBNAME XL CLEAR;

PROC SQL;
/*	add cost centers based on business unit assignment*/
	CREATE TABLE ONE AS
		SELECT
			TT.*,
			SU.Unit,
			CASE
				WHEN UPCASE(TT.Sacker_Name) LIKE ('%FED') 
					OR UPCASE(TT.Sacker_Name) LIKE ('%FED)') 
					OR SR.IsFED = 1 
				THEN 13 /*CornerStone Portfolio Servicing*/
				WHEN TT.Sacker_Name = 'ARC Add Database'
					AND SU.Unit = 'Systems Support'
				THEN 8 /*LPP Portfolio Servicing*/
				ELSE CC.CostCenterId
			END AS CostCenterId,
			CASE
				WHEN UPCASE(TT.Sacker_Name) LIKE ('%FED') 
					OR UPCASE(TT.Sacker_Name) LIKE ('%FED)') 
					OR SR.IsFED = 1 
				THEN 100.0
				WHEN CC.CostCenterId IS NULL 
				THEN 100.0
				ELSE CC.Weight
			END AS Weight
		FROM
			BatchScriptTimeTracking TT
			INNER JOIN BSYS.SCKR_REF_Unit SU
				ON TT.Sacker_Name = SU.Program
			INNER JOIN CSYS.GENR_LST_BusinessUnits BU
				ON SU.Unit = BU.Name
			LEFT JOIN CSYS.COST_DAT_BusinessUnitCostCenters CC
				ON BU.ID = CC.BusinessUnitId
			LEFT JOIN BSYS.SCKR_DAT_Scripts SR
				ON TT.Sacker_Name = SR.Script
		WHERE
/*			&EndDate BETWEEN INPUT(CC.EffectiveBegin, YYMMDD10.) AND COALESCE(INPUT(CC.EffectiveEnd, YYMMDD10.), &EndDate)*/
			&EndDate BETWEEN CC.EffectiveBegin AND COALESCE(CC.EffectiveEnd, &EndDate)
	;

/*	calculate weighted percent for each script*/
	CREATE TABLE TWO AS
		SELECT
			O.*,
			S.TotalWeight,
			O.Weight/S.TotalWeight AS WeightedWeight,
			O.Weight/S.TotalWeight*O.Percent_of_Total_Minutes*100.0 AS WeightedPercent
		FROM
			ONE O
			INNER JOIN
			(
				SELECT
					Time_Tracking_Id,
					SUM(Weight) AS TotalWeight
				FROM
					ONE
				GROUP BY
					Time_Tracking_Id
			) S
			ON O.Time_Tracking_Id = S.Time_Tracking_Id
	;

/*	sum weighted percents to get total for each cost center*/
	CREATE TABLE THREE AS
		SELECT
			CostCenterId,
			SUM(WeightedPercent) AS Weight
		FROM
			TWO
		GROUP BY
			CostCenterId
	;

/*	add cost center name to data set THREE*/
	CREATE TABLE FOUR AS
		SELECT
			CASE
				WHEN CC.CostCenter IS NULL 
				THEN 'Overhead'
				ELSE CC.CostCenter
			END AS CostCenter,
			TH.*
		FROM
			THREE TH
			LEFT JOIN CSYS.COST_DAT_CostCenters CC
				ON TH.CostCenterId = CC.CostCenterId
	;

/*	create checksum (should equal 100)*/
	CREATE TABLE CHECKSUM AS
		SELECT
			SUM(Weight) AS TotalWeights
		FROM
			THREE
	;
QUIT;

/*data for review*/
PROC EXPORT 
	DATA = FOUR
    OUTFILE = "T:\SAS\BatchScriptCostCenters.xlsx"
    DBMS = EXCEL
	REPLACE;
	SHEET = "Review";
RUN;

/*data detail*/
PROC EXPORT 
	DATA = TWO
    OUTFILE = "T:\SAS\BatchScriptCostCenters.xlsx"
    DBMS = EXCEL
	REPLACE;
	SHEET = "Detail";
RUN;
