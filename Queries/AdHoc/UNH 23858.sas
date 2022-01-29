/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWAB1.LWAB1RZ";
FILENAME REPORT2 "&RPTLIB/ULWAB1.LWAB1R2";
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;
/*LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE 0  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @01 " ********************************************************************* "
              / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @01 " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @01 " ****  &SQLXMSG   **** "
              / @01 " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL;
	CONNECT TO DB2 (DATABASE=DLGSUTWH);

	CREATE TABLE DEMO AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						PD10.DF_SPE_ACC_ID,
						PD10.DM_PRS_1,
						PD32.DX_ADR_EML,
						PD30.DX_STR_ADR_1,
						PD30.DX_STR_ADR_2,
						PD30.DX_STR_ADR_3
					FROM	
						OLWHRM1.PD10_PRS_NME PD10 
						INNER JOIN OLWHRM1.LN10_LON LN10
							ON PD10.DF_PRS_ID = LN10.BF_SSN
						INNER JOIN OLWHRM1.PD32_PRS_ADR_EML PD32
							ON PD10.DF_PRS_ID = PD32.DF_PRS_ID
							AND PD32.DI_VLD_ADR_EML = 'Y'
							AND PD32.DC_STA_PD32 = 'A'
						LEFT JOIN OLWHRM1.PD30_PRS_ADR PD30
							ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
							AND PD30.DI_VLD_ADR = 'Y'
							AND PD30.DC_ADR = 'B'
							AND PD30.DC_DOM_ST = 'UT'
						LEFT JOIN 
							(
								SELECT DISTINCT 
									LN10I.BF_SSN
								FROM
									OLWHRM1.LN10_LON LN10I
									LEFT JOIN OLWHRM1.PD24_PRS_BKR PD24I
										ON PD24I.DF_PRS_ID = LN10I.BF_SSN
										AND PD24I.DC_BKR_STA = '06'
									LEFT JOIN OLWHRM1.CL10_CLM_PCL CL10I
										ON CL10I.BF_SSN = LN10I.BF_SSN
								WHERE
									LN10I.DD_DSA_VER IS NOT NULL /*verified disability*/
									OR LN10I.DD_DTH_VER IS NOT NULL /*verified death*/
									OR PD24I.DF_PRS_ID IS NOT NULL /*current bky*/
									OR LN10I.LD_CLM_PD IS NOT NULL /*default claim paid*/
									OR CL10I.BF_SSN IS NOT NULL /*installment deliquency*/									
							) EXCLU 
								ON EXCLU.BF_SSN = PD10.DF_PRS_ID
					WHERE
						EXCLU.BF_SSN IS NULL
						AND (PD30.DF_PRS_ID IS NOT NULL 
						OR SUBSTR(LN10.LF_DOE_SCL_ORG,1,6) IN('003670','003675','003677','003681','004027','033394','003680','003678','003679','003671','005220','040653'))



					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA DEMO;
	SET DUSTER.DEMO;
RUN;
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\NH 23858.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="Sheet1"; 
RUN;
