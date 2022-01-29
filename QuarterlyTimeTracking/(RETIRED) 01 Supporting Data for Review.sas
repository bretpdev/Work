LIBNAME BSYS ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\BSYS.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME CSYS ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\CSYS.dsn; update_lock_typ=nolock; bl_keepnulls=no");
/*LIBNAME CSYT ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\CSYSTEST.dsn; update_lock_typ=nolock; bl_keepnulls=no");*/

PROC SQL;
	CREATE TABLE BU_Cost_Centers AS
		SELECT
			BCC.BusinessUnitCostCenterId,
			BCC.BusinessUnitId,
			BU.Name,
			BCC.CostCenterId,
			CC.CostCenter,
			BCC.Weight
		FROM
			CSYS.COST_DAT_BusinessUnitCostCenters BCC
			INNER JOIN CSYS.GENR_LST_BusinessUnits BU
				ON BCC.BusinessUnitId = BU.ID
			INNER JOIN CSYS.COST_DAT_CostCenters CC
				ON BCC.CostCenterId = CC.CostCenterId
		WHERE
			BCC.EffectiveEnd IS NULL
		ORDER BY
			BusinessUnitId,
			CostCenterId
	;

	CREATE TABLE Cost_Centers AS
		SELECT
			CostCenterId,
			CostCenter,
			CASE
				WHEN IsBillable = 1 
				THEN 'True' 
				ELSE 'False'
			END AS IsBillable,
			CASE
				WHEN IsChargedOverHead = 1 
				THEN 'True' 
				ELSE 'False'
			END AS IsChargedOverHead
		FROM
			CSYS.COST_DAT_CostCenters
		ORDER BY
			CostCenter
	;

	CREATE TABLE Mail_Code_Cost_Centers AS
		SELECT
			MC.MailCodeCostCenterId,
			MC.MailCode,
			UCC.Name,
			MC.CostCenterId,
			CC.CostCenter,
			MC.Weight
		FROM
			CSYS.COST_DAT_MailCodeCostCenters MC
			INNER JOIN CSYS.COST_DAT_CostCenters CC
				ON MC.CostCenterId = CC.CostCenterId
			INNER JOIN BSYS.GENR_LST_UHEAACostCenters UCC
				ON MC.MailCode = UCC.Code
		WHERE
			MC.EffectiveEnd IS NULL
	; 

QUIT;


PROC EXPORT 
	DATA = BU_Cost_Centers
    OUTFILE = "T:\SAS\Supporting Data for Review.xlsx" 
    DBMS = EXCEL
	REPLACE;
	SHEET = "BU_Cost_Centers";
RUN;

PROC EXPORT 
	DATA = Cost_Centers
    OUTFILE = "T:\SAS\Supporting Data for Review.xlsx" 
    DBMS = EXCEL
	REPLACE;
	SHEET = "Cost_Centers";
RUN;

PROC EXPORT 
	DATA = Mail_Code_Cost_Centers
    OUTFILE = "T:\SAS\Supporting Data for Review.xlsx" 
    DBMS = EXCEL
	REPLACE;
	SHEET = "Mail_Code_Cost_Centers";
RUN;
