LIBNAME BSYS ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\BSYS.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME CSYS ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\CSYS.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME XL 'T:\SAS\Batch Script Time Tracking Average.xlsx';

DATA BatchScriptTimeTracking;
	SET XL.'Sheet1$'N (DROP=F1 Time_Tracking_Id);
	Time_Tracking_Id = _N_;
	WHERE Sacker_Name NE '';
RUN;

LIBNAME XL CLEAR;

PROC SQL;
	CREATE TABLE BatchScriptNameErrors AS
		SELECT
			BS.Sacker_Name
			,SR.Script
			,SR.ID
		FROM
			BatchScriptTimeTracking BS
			LEFT JOIN BSYS.SCKR_DAT_Scripts SR
				ON BS.Sacker_Name = SR.Script
		WHERE
			SR.Script IS NULL
	;

	CREATE TABLE BatchScriptsNoCostCenter AS
		SELECT
			TT.Sacker_Name,
			SU.Unit AS Unit_Name,
			BU.ID AS CSYS_Id,
			CC.CostCenterId
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
			CC.BusinessUnitId IS NULL
			AND UPCASE(TT.Sacker_Name) NOT LIKE ('%FED')
			AND UPCASE(TT.Sacker_Name) NOT LIKE ('%FED)')
			AND SR.IsFED ^= 1
	;
QUIT;

PROC EXPORT 
	DATA = BatchScriptNameErrors
    OUTFILE = "T:\SAS\Batch Script Name Errors.xlsx" 
    DBMS = EXCEL
	REPLACE;
RUN;

PROC EXPORT 
	DATA = BatchScriptsNoCostCenter
    OUTFILE = "T:\SAS\Batch Scripts Allocated to Overhead.xlsx" 
    DBMS = EXCEL
	REPLACE;
RUN;
