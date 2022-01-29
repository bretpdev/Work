LIBNAME DUSTER REMOTE SERVER=DUSTER SLIBREF=WORK;
RSUBMIT;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;

PROC SQL;
	CREATE TABLE ADDRS AS
		SELECT DISTINCT
			PD10.DF_SPE_ACC_ID,
			PD10.DF_PRS_ID,
			PD30.DX_STR_ADR_1,
			PD30.DX_STR_ADR_2, 
			PD30.DM_CT, 
			PD30.DC_DOM_ST, 
			PD30.DF_ZIP_CDE,
			CATX(' ',PD30.DX_STR_ADR_1,PD30.DX_STR_ADR_2,PD30.DM_CT,PD30.DC_DOM_ST,PD30.DF_ZIP_CDE) AS ADDR
		FROM
			OLWHRM1.PD10_PRS_NME PD10
			JOIN OLWHRM1.PD30_PRS_ADR PD30
				ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
		WHERE
			PD30.DI_VLD_ADR = 'Y'
			AND SUBSTR(PD10.DF_PRS_ID,1,1) NE 'P'
	;
	
	CREATE TABLE CNTS AS
		SELECT
			COUNT(DISTINCT DF_SPE_ACC_ID) AS CNT,
			ADDR
		FROM
			ADDRS
		GROUP BY
			ADDR
	;

	CREATE TABLE MULTS AS
		SELECT
			ADDRS.DF_SPE_ACC_ID,
			ADDRS.DX_STR_ADR_1,
			ADDRS.DX_STR_ADR_2, 
			ADDRS.DM_CT, 
			ADDRS.DC_DOM_ST, 
			ADDRS.DF_ZIP_CDE
		FROM
			CNTS
			JOIN ADDRS
				ON CNTS.ADDR = ADDRS.ADDR
		WHERE
			CNTS.CNT GE 4
		ORDER BY
			ADDRS.ADDR,
			ADDRS.DF_SPE_ACC_ID
	;
QUIT;

ENDRSUBMIT;

DATA MULTS; SET DUSTER.MULTS; RUN;

PROC EXPORT
		DATA=MULTS
		OUTFILE='T:\SAS\UHEAA COMMON ADDRESSES.XLSX'
		REPLACE;
RUN;