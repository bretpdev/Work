/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK; /*live*/
RSUBMIT;

%LET DB = DLGSUTWH; /*live*/

/*Please find all borrowers with at least one BANA loan 
and a valid email address to be included in an email campaign about e-bill participation.

BANA = LN10.LF_LON_CUR_OWN = 829769XX (any branch or no branch is fine)
Valid email = PD32.DI_VLD_ADR_EML = Y

Output = comma delimited text file

Acct # = PD10.DF_SPE_ACC_ID
Borrower Name = PD10.DM_PRS_1
Borrower Last Name = PD10.DM_PRS_LST
Email address = PD32.DX_ADR_EML
PH05.DI_CNC_EBL_OPI = N or blank
*/

PROC SQL;
CONNECT TO DB2 (DATABASE=&DB); 
	CREATE TABLE Demos AS
		SELECT 
			*
		FROM CONNECTION TO DB2 
			(
			SELECT DISTINCT
				PD10.DF_SPE_ACC_ID,
				PD10.DM_PRS_1,
				PD10.DM_PRS_LST,
				PD32.DX_ADR_EML
			FROM	
				OLWHRM1.PD10_PRS_NME PD10
				INNER JOIN OLWHRM1.LN10_LON LN10 
					ON LN10.BF_SSN = PD10.DF_PRS_ID
				INNER JOIN OLWHRM1.PD32_PRS_ADR_EML PD32
					ON PD32.DF_PRS_ID = PD10.DF_PRS_ID
					AND PD32.DI_VLD_ADR_EML = 'Y'
				LEFT OUTER JOIN OLWHRM1.DW01_DW_CLC_CLU DW01
					ON DW01.BF_SSN = LN10.BF_SSN
					AND DW01.LN_SEQ = LN10.LN_SEQ
					AND DW01.WC_DW_LON_STA IN ('17','19','21')
				INNER JOIN
					(
						SELECT
							SUM(LN10.LA_CUR_PRI) AS Bal,
							PD10.DF_SPE_ACC_ID
						FROM OLWHRM1.LN10_LON LN10
							INNER JOIN OLWHRM1.PD10_PRS_NME PD10
								ON PD10.DF_PRS_ID = LN10.BF_SSN
						GROUP BY
							PD10.DF_SPE_ACC_ID
					) PaidOff ON PaidOff.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
				LEFT OUTER JOIN OLWHRM1.PH05_CNC_EML PH05
					ON LPAD(CAST(PH05.DF_SPE_ID AS VARCHAR(10)),10,'0') = PD10.DF_SPE_ACC_ID
			WHERE	
				LN10.LF_LON_CUR_OWN LIKE '829769%'
				AND (PH05.DF_SPE_ID IS NULL 
					OR PH05.DI_CNC_EBL_OPI = 'N' 
					OR PH05.DI_CNC_EBL_OPI = '')
				AND DW01.BF_SSN IS NULL /*exclude disability, death, and bankruptcy*/
				AND PaidOff.Bal > 0 /*exclude paid off accounts*/
				
			)
	;
DISCONNECT FROM DB2;
QUIT;

ENDRSUBMIT;

PROC EXPORT
		DATA=DUSTER.Demos
		OUTFILE="&RPTLIB\UNH 26278.csv"
		DBMS = CSV
		REPLACE;
RUN;
