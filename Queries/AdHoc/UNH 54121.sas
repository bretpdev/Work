%LET bsys = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\bsys.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME bsys ODBC &bsys ;

PROC SQL;
	CREATE TABLE RESULTS AS 
		SELECT	
			I.*,
			BU.BU
		FROM	
			BSYS.QCTR_DAT_Issue I
			INNER JOIN BSYS.QCTR_DAT_BU BU ON BU.IssueID = I.ID
		WHERE	
			I.Requested >= INPUT('01JAN2017:00:00:00.000', DATETIME22.3)
;
QUIT;

PROC EXPORT DATA = WORK.RESULTS 
            OUTFILE = "T:\SSAE18 COM - QC errors.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RESULTS"; 
RUN;
