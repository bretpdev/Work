LIBNAME EA27BANA ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\EA27_BANA.dsn; bl_keepnulls=no; READ_ISOLATION_LEVEL=RU" SCHEMA= dbo;

PROC SQL;
CREATE TABLE Cycle AS
	SELECT
		SSN
		,Cycle_Dt
	FROM EA27BANA.ACH_DATA
;
QUIT;

%LET RPTLIB = T:\SAS;

	*gets only the one column needed from external file;
DATA ACH;
	INFILE 'X:\Archive\BANA\Production Files Wave 3\Supplemental Files\ACH_20160305140001.CSV'
	FIRSTOBS=2;
	INPUT @1 Borrower_SSN $CHAR9.;
RUN;

	*removes duplicates;
PROC SORT 
	DATA=ACH 
	OUT=Source 
	NODUPKEY;
	BY Borrower_SSN;
RUN;

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;/*live*/

	*Send data to Duster;
DATA DUSTER.Source; 
	SET Source;
RUN;

DATA DUSTER.Cycle; 
	SET Cycle;
RUN;

RSUBMIT;

%LET DB = DLGSUTWH; *live;

	
LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1; *needed for SQL queries, but not for DB2 queries;

PROC SQL;
CREATE TABLE ACHfile AS
	SELECT DISTINCT
		S.Borrower_SSN AS SSN
		,PD10.DM_PRS_1 AS First_Name
		,PD10.DM_PRS_LST AS Last_Name
		,COALESCE(BL10.DUE_DAY, LN65.DUE_DAY, DAY(DATEPART(Cycle.DUE_DAY))) AS DUE_DATE
		,PD30.DX_STR_ADR_1 AS Address_Line_1
		,PD30.DX_STR_ADR_2 AS Address_Line_2
		,PD30.DM_CT AS City
		,PD30.DC_DOM_ST AS State
		,PD30.DF_ZIP_CDE AS Zip_Code
		,PD30.DM_FGN_ST AS Foreign_State
		,PD30.DM_FGN_CNY AS Foreign_Country
		,PD32.DX_ADR_EML AS Email_Address
	FROM
		WORK.Source S
		INNER JOIN OLWHRM1.PD10_PRS_NME PD10
			ON S.Borrower_SSN = PD10.DF_PRS_ID
		INNER JOIN OLWHRM1.PD30_PRS_ADR PD30
			ON S.Borrower_SSN = PD30.DF_PRS_ID
		INNER JOIN OLWHRM1.PD32_PRS_ADR_EML PD32
			ON S.Borrower_SSN = PD32.DF_PRS_ID
		LEFT JOIN 
			(
				SELECT
					BF_SSN,
					MAX(DAY(LD_RPS_1_PAY_DU)) AS DUE_DAY
				FROM
					OLWHRM1.RS10_BR_RPD RS10
				WHERE
					LC_STA_RPST10 = 'A'
				GROUP BY
					BF_SSN
			)LN65
				ON LN65.BF_SSN = S.Borrower_SSN
			LEFT JOIN
			(
				SELECT
					BF_SSN,
					MAX(DAY(LD_BIL_DU)) AS DUE_DAY
				FROM
					OLWHRM1.BL10_BR_BIL 
				WHERE
					LC_STA_BIL10 = 'A'
				GROUP BY
					BF_SSN
			)BL10
				ON BL10.BF_SSN = S.Borrower_SSN
			LEFT JOIN
			(
				SELECT
					SSN,
					MAX(DAY(DATEPART(Cycle_Dt))) AS DUE_DAY
				FROM
					WORK.Cycle 
				GROUP BY
					SSN
			)Cycle
				ON Cycle.SSN = S.Borrower_SSN
;

QUIT;

ENDRSUBMIT;

DATA ACHfile;
	SET DUSTER.ACHfile;
RUN;

PROC EXPORT
		DATA=ACHfile
		OUTFILE="&RPTLIB\UNH 26430.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="ACH_Wave3";
RUN;
