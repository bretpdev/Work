/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWAB1.LWAB1RZ";
FILENAME REPORT2 "&RPTLIB/ULWAB1.LWAB1R2";
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;


PROC SQL;
	CONNECT TO DB2 (DATABASE=DLGSUTWH);

	CREATE TABLE DEMO AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						PD01.DF_SPE_ACC_ID,
						GA10.AD_PRC
					FROM
						OLWHRM1.PD01_PDM_INF PD01
						JOIN OLWHRM1.DC01_LON_CLM_INF DC01
							ON PD01.DF_PRS_ID = DC01.BF_SSN
							AND DC01.LC_REA_CLM_ASN_DOE = '07'
						JOIN OLWHRM1.GA01_APP GA01
							ON DC01.BF_SSN = GA01.DF_PRS_ID_BR
							AND GA01.AI_STU_SIG = 'Y'
							AND GA01.AC_ELS_LON = 'E'
						JOIN OLWHRM1.GA10_LON_APP GA10
							ON GA01.AF_APL_ID = GA10.AF_APL_ID

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

QUIT;

ENDRSUBMIT;
DATA DEMO;
	SET DUSTER.DEMO;
RUN;