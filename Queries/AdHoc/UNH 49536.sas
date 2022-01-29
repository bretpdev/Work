%LET RPTLIB = T:\SAS;
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK; *live;
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK ; *test;*/

RSUBMIT;

%LET DB = DLGSUTWH; *live;
/*%LET DB = DLGSWQUT; *test;*/

PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);
	CREATE TABLE DEMO AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						PH05.DF_CNC_SYS_ID
						,RIGHT('0000000000' || CAST(PH05.DF_SPE_ID AS VARCHAR(10)), 10) AS DF_SPE_ID
						,CASE
							WHEN PH05.DI_CNC_ELT_OPI IN('Y','N')
								THEN 'NA'
							WHEN TRIM(COALESCE(PH05.DI_CNC_ELT_OPI,'')) != ''
								THEN 'NULL'
							ELSE 'NULL'
						END AS DI_CNC_ELT_OPI
						,CASE
							WHEN PH05.DI_CNC_ELT_OPI IS NULL
								OR PH05.DI_CNC_ELT_OPI = ' '
								THEN 'Y'
							ELSE 'NA'
						END AS DI_CNC_ELT_OPI_NEW
						,CASE 
							WHEN TRIM(COALESCE(PH05.DC_ELT_OPI_APL_SRC,'')) != ''
								THEN 'NA'
							ELSE 'NULL' 
						END AS DC_ELT_OPI_APL_SRC
						,CASE 
							WHEN TRIM(COALESCE(PH05.DC_ELT_OPI_APL_SRC,'')) != ''
								THEN 'NA'
							ELSE 'C' 
						END AS DC_ELT_OPI_APL_SRC_NEW
						,CASE 
							WHEN TRIM(COALESCE(PH05.DC_ELT_OPI_SRC,'')) != ''
								THEN 'NA'
							ELSE 'NULL'
						END AS DC_ELT_OPI_SRC
						,CASE 
							WHEN TRIM(COALESCE(PH05.DC_ELT_OPI_SRC,'')) != ''
								THEN 'NA'
							ELSE 'NB'
						END AS DC_ELT_OPI_SRC_NEW
						,CASE 
							WHEN COALESCE(CAST(PH05.DF_DTS_ELT_OPI_EFF AS VARCHAR(20)),'') != ''
								THEN 'NA'
							ELSE 'NULL'
						END AS DF_DTS_ELT_OPI_EFF
						,CASE 
							WHEN COALESCE(CAST(PH05.DF_DTS_ELT_OPI_EFF AS VARCHAR(20)),'') != ''
								THEN 'NA'
							ELSE 'DIG Timestamp'
						END AS DF_DTS_ELT_OPI_EFF_NEW
						,CASE
							WHEN TRIM(COALESCE(PH05.DF_LST_USR_ELT_OPI,'')) != ''
								THEN 'NA'
							ELSE 'NULL'
						END AS DF_LST_USR_ELT_OPI
						,CASE
							WHEN TRIM(COALESCE(PH05.DF_LST_USR_ELT_OPI,'')) != ''
								THEN 'NA'
							ELSE 'DCR'
						END AS DF_LST_USR_ELT_OPI_NEW
						,CASE
							WHEN PH05.DI_CNC_EBL_OPI IN('Y','N')
								THEN 'NA'
							WHEN TRIM(COALESCE(PH05.DI_CNC_EBL_OPI,'')) != ''
								THEN 'NULL'
							ELSE 'NULL'
						END AS DI_CNC_EBL_OPI
						,CASE
							WHEN PH05.DI_CNC_EBL_OPI IN('Y','N')
								THEN 'NA'
							WHEN TRIM(COALESCE(PH05.DI_CNC_EBL_OPI,'')) != ''
								THEN 'Y'
							ELSE 'Y'
						END AS DI_CNC_EBL_OPI_NEW
						,CASE 
							WHEN TRIM(COALESCE(PH05.DC_EBL_OPI_APL_SRC,'')) != ''
								THEN 'NA'
							ELSE 'NULL'
						END AS DC_EBL_OPI_APL_SRC
						,CASE 
							WHEN TRIM(COALESCE(PH05.DC_EBL_OPI_APL_SRC,'')) != ''
								THEN 'NA'
							ELSE 'C'
						END AS DC_EBL_OPI_APL_SRC_NEW
						,CASE 
							WHEN TRIM(COALESCE(PH05.DC_EBL_OPI_SRC,'')) != ''
								THEN 'NA'
							ELSE 'NULL'
						END AS DC_EBL_OPI_SRC
						,CASE 
							WHEN TRIM(COALESCE(PH05.DC_EBL_OPI_SRC,'')) != ''
								THEN 'NA'
							ELSE 'NB'
						END AS DC_EBL_OPI_SRC_NEW
						,CASE 
							WHEN COALESCE(CAST(PH05.DF_DTS_EBL_OPI_EFF AS VARCHAR(20)),'') != ''
								THEN 'NA'
							ELSE 'NULL'
						END AS DF_DTS_EBL_OPI_EFF
						,CASE 
							WHEN COALESCE(CAST(PH05.DF_DTS_EBL_OPI_EFF AS VARCHAR(20)),'') != ''
								THEN 'NA'
							ELSE 'DIG Timestamp'
						END AS DF_DTS_EBL_OPI_EFF_NEW
						,CASE 
							WHEN TRIM(COALESCE(PH05.DF_LST_USR_EBL_OPI,'')) != ''
								THEN 'NA'
							ELSE 'NULL'
						END AS DF_LST_USR_EBL_OPI
						,CASE 
							WHEN TRIM(COALESCE(PH05.DF_LST_USR_EBL_OPI,'')) != ''
								THEN 'NA'
							ELSE 'DCR'
						END AS DF_LST_USR_EBL_OPI_NEW
					FROM 
						OLWHRM1.LN10_LON LN10
						INNER JOIN OLWHRM1.PD10_PRS_NME PD10
							ON LN10.BF_SSN = PD10.DF_PRS_ID
						INNER JOIN OLWHRM1.PH05_CNC_EML PH05
							ON PD10.DF_SPE_ACC_ID = PH05.DF_SPE_ID
						LEFT JOIN 
							(
								SELECT DISTINCT
									AY10.BF_SSN
								FROM
									OLWHRM1.AY10_BR_LON_ATY AY10
								WHERE
									AY10.PF_REQ_ACT = 'ECORO'									
							)AY10
							ON LN10.BF_SSN = AY10.BF_SSN
					WHERE
						LN10.LA_CUR_PRI > 0
						AND LN10.LC_STA_LON10 = 'R'
						AND PH05.DI_VLD_CNC_EML_ADR = 'Y'
						AND AY10.BF_SSN IS NULL

					FOR READ ONLY WITH UR
				)
	;
	DISCONNECT FROM DB2;
QUIT;
ENDRSUBMIT;

DATA DEMO;
	SET DUSTER.DEMO;
RUN;

PROC EXPORT DATA= DEMO
            OUTFILE= "&RPTLIB\PH05.xlsx" 
            DBMS=EXCEL REPLACE;
RUN;
