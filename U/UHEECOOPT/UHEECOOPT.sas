/*TEST*/
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK  ;*/
/*LIVE*/
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER SLIBREF=WORK  ;


RSUBMIT;

/*%let DB = DLGSWQUT;  *This is test;*/
%let DB = DLGSUTWH;  *This is live;

LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1;

PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE POP AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						PD10.DF_SPE_ACC_ID AS AccountNumber,
						(TRIM(PD10.DM_PRS_1)|| ' ' || TRIM(PD10.DM_PRS_LST)) AS Name,
						COALESCE(PD32H.DX_ADR_EML,PD32A.DX_ADR_EML, PD32W.DX_ADR_EML) AS Recipient
					FROM
						OLWHRM1.PD10_PRS_NME PD10
						INNER JOIN OLWHRM1.LN10_LON LN10
							ON LN10.BF_SSN = PD10.DF_PRS_ID
						LEFT JOIN
						(
							SELECT
								DF_PRS_ID,
								DX_ADR_EML
							FROM
								OLWHRM1.PD32_PRS_ADR_EML
							WHERE
								DC_ADR_EML = 'H'
								AND DI_VLD_ADR_EML = 'Y'		
								AND DC_STA_PD32 = 'A'
						)PD32H
							ON PD32H.DF_PRS_ID = PD10.DF_PRS_ID
						LEFT JOIN
						(
							SELECT
								DF_PRS_ID,
								DX_ADR_EML
							FROM
								OLWHRM1.PD32_PRS_ADR_EML
							WHERE
								DC_ADR_EML = 'A'
								AND DI_VLD_ADR_EML = 'Y'	
								AND DC_STA_PD32 = 'A'	
						)PD32A
							ON PD32A.DF_PRS_ID = PD10.DF_PRS_ID
						LEFT JOIN
						(
							SELECT
								DF_PRS_ID,
								DX_ADR_EML
							FROM
								OLWHRM1.PD32_PRS_ADR_EML
							WHERE
								DC_ADR_EML = 'W'
								AND DI_VLD_ADR_EML = 'Y'
								AND DC_STA_PD32 = 'A'	
						)PD32W
							ON PD32W.DF_PRS_ID = PD10.DF_PRS_ID
						LEFT JOIN
						(
							SELECT
								BF_SSN
							FROM 
								OLWHRM1.DW01_DW_CLC_CLU
							WHERE
								WC_DW_LON_STA IN ('21', '17', '19')
						)DW01
							ON DW01.BF_SSN = LN10.BF_SSN
					WHERE
						LN10.LA_CUR_PRI > 0
						AND LN10.LC_STA_LON10 = 'R'
						AND DW01.BF_SSN IS NULL
						AND LN10.IC_LON_PGM != 'COMPLT'
						AND (PD32H.DF_PRS_ID IS NOT NULL OR PD32A.DF_PRS_ID IS NOT NULL OR PD32W.DF_PRS_ID IS NOT NULL) 
						

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;


QUIT;

ENDRSUBMIT;
DATA POP; SET DUSTER.POP; RUN;

PROC EXPORT DATA = WORK.pop 
            OUTFILE = "T:\SAS\UHEECOOPT_&sysdate.txt" 
            DBMS = CSV 
			REPLACE;
     PUTNAMES = YES;
RUN;

