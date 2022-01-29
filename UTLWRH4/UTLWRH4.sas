/*%LET RPTLIB = %SYSGET(reportdir);*/
/*%LET TBLLIB = /sas/whse/progrevw;*/
%LET RPTLIB = T:\SAS;
%LET TBLLIB = Q:\Process Automation\TabSAS;
FILENAME REPORTZ "&RPTLIB/ULWRH4.LWRH4RZ";
FILENAME REPORT2 "&RPTLIB/ULWRH4.LWRH4R2";

/***************************************************************
* DUSTER BOND ID TABLE - THIS IS THE SAME TABLE USED IN UTLWRH1 
****************************************************************/
DATA _NULL_ (WHERE=(BOND_STAT = 'A'));
	FORMAT BOND_ID $20. BOND_STAT $1.;
	INFILE "&TBLLIB/UTLWRH1.Bond.txt" DLM=',' MISSOVER FIRSTOBS=2;
	INFORMAT BOND_ID $20. BOND_STAT $1.; 
	INPUT BOND_ID $ BOND_STAT $;
	DO;
		CALL SYMPUT('BOND_ID',BOND_ID);
	END;
RUN;

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
%SYSLPUT BOND_ID = &BOND_ID;
RSUBMIT;
/*LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/

LIBNAME PROGREVW '/sas/whse/progrevw';

PROC SQL;
	CREATE TABLE BOND_IDS AS 
		SELECT DISTINCT
			IF_BND_ISS_OLD,
			IF_BND_ISS_NEW
		FROM
			PROGREVW.BOND_IDS
;
QUIT;


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

	CREATE TABLE RHB AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						DC01.BF_SSN
						,TRIM(PD01.DM_PRS_1) || ' ' || TRIM(PD01.DM_PRS_LST) AS NAME
						,DC01.AF_APL_ID || DC01.AF_APL_ID_SFX AS COMMONLINE_ID
						,GA10.AC_LON_TYP
						,DC02.LA_CLM_BAL
						,DC01.LA_PRI_COL
						,DC02.LA_CLM_PRJ_COL_CST
						,DC02.LD_ETR
						,DC02.LR_CUR_INT
						,'828476' AS REPURCHASE_LENDER
						,BND.IF_BND_ISS
					FROM	
						OLWHRM1.DC01_LON_CLM_INF DC01
						LEFT JOIN OLWHRM1.PD01_PDM_INF PD01
							ON DC01.BF_SSN = PD01.DF_PRS_ID
						LEFT JOIN OLWHRM1.GA10_LON_APP GA10
							ON DC01.AF_APL_ID = GA10.AF_APL_ID
							AND DC01.AF_APL_ID_SFX = GA10.AF_APL_ID_SFX
						LEFT JOIN  OLWHRM1.DC02_BAL_INT DC02
							ON DC01.AF_APL_ID = DC02.AF_APL_ID
							AND DC01.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
							AND DC01.LF_CRT_DTS_DC10 = DC02.LF_CRT_DTS_DC10
						LEFT JOIN
							(
								SELECT
									LN10.BF_SSN,
									LN10.LN_SEQ,
									LN10.LF_LON_ALT,
									LN10.LN_LON_ALT_SEQ,
									LN35.IF_BND_ISS
								FROM	
									OLWHRM1.LN10_LON LN10 
									INNER JOIN
											(
												SELECT
													BF_SSN,
													LF_LON_ALT, 
													LN_LON_ALT_SEQ,
													MAX(LF_LST_DTS_LN10) AS LAST_UPDATE
												FROM
													OLWHRM1.LN10_LON 
												GROUP BY
													BF_SSN,
													LF_LON_ALT, 
													LN_LON_ALT_SEQ
											)LN10_POP
										ON LN10_POP.BF_SSN = LN10.BF_SSN
										AND LN10_POP.LF_LON_ALT = LN10.LF_LON_ALT
										AND LN10_POP.LN_LON_ALT_SEQ = LN10.LN_LON_ALT_SEQ
										AND LN10_POP.LAST_UPDATE = LN10.LF_LST_DTS_LN10
									INNER JOIN
											(
												SELECT
													LN35.BF_SSN,
													LN35.LN_SEQ,
													IF_BND_ISS
												FROM
													OLWHRM1.LN35_LON_OWN LN35
													INNER JOIN
													(
														SELECT
															BF_SSN,
															LN_SEQ,
															IF_OWN,
															LN_LON_OWN_SEQ,
															MAX(LD_OWN_EFF_SR) AS LD_OWN_EFF_SR
														FROM
															OLWHRM1.LN35_LON_OWN
														WHERE
															LC_STA_LON35 = 'A'
															AND LD_OWN_EFF_END IS NULL
														GROUP BY 
															BF_SSN,
															LN_SEQ,
															IF_OWN,
															LN_LON_OWN_SEQ
													) POP
														ON POP.BF_SSN = LN35.BF_SSN
														AND POP.LN_SEQ = LN35.LN_SEQ
														AND POP.IF_OWN = LN35.IF_OWN
														AND POP.LN_LON_OWN_SEQ = LN35.LN_LON_OWN_SEQ
											) LN35
										ON LN35.BF_SSN = LN10.BF_SSN
										AND LN35.LN_SEQ = LN10.LN_SEQ 
									)BND
										ON DC01.AF_APL_ID = BND.LF_LON_ALT
										AND DC01.AF_APL_ID_SFX = BND.LN_LON_ALT_SEQ
					WHERE	
						DC01.LC_RHB_CON IN ('02','09')
						AND 
						DC01.LC_STA_DC10 = '03'

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA RHB; SET DUSTER.RHB; RUN;
DATA BOND_IDS; SET DUSTER.BOND_IDS; RUN;

PROC SQL;
	CREATE TABLE RHB AS
		SELECT
			R.*,
			COALESCE(B.IF_BND_ISS_NEW,"&BOND_ID") AS BOND_ID
		FROM
			RHB R
			LEFT JOIN BOND_IDS B
				ON B.IF_BND_ISS_OLD = R.IF_BND_ISS
;
QUIT;

DATA RHB2;
	SET RHB;
	PERIOD = INTCK('DAY',LD_ETR,INTNX('MONTH',DATE(),0)+19); /*NUMBER OF DAYS FROM CURRENT DATE TO 20TH*/
	INTACRL = ROUND(LA_PRI_COL * PERIOD * (LR_CUR_INT/100) / 365, .01);/*INTEREST TO ACCRUE FROM CURRENT DATE TO 20TH*/
	BAL = LA_CLM_BAL + INTACRL - LA_CLM_PRJ_COL_CST; 							/* TOTAL BALANCE AS OF 20TH. */		
RUN;

PROC SORT DATA=RHB2; BY BF_SSN COMMONLINE_ID; RUN;

/*create printed report*/
PROC PRINTTO PRINT=REPORT2 NEW; RUN;

OPTIONS ORIENTATION=LANDSCAPE PS=39 LS=127;
TITLE 		'Pre-Rehabilitation Report';
TITLE2		"RUNDATE &SYSDATE9";
FOOTNOTE1  	"THIS DOCUMENT MAY CONTAIN BORROWERS' SENSITIVE INFORMATION THAT UHEAA HAS PLEDGED TO PROTECT.";
FOOTNOTE2	'PLEASE TAKE APPROPRIATE PRECAUTIONS TO SAFEGUARD THIS INFORMATION.';
FOOTNOTE3	;
FOOTNOTE4   'JOB = UTLWRH4  	 REPORT = ULWRH4.LWRH4R2';

PROC PRINT 
		NOOBS SPLIT = '/' 
		DATA = RHB2 
		WIDTH = UNIFORM 
		WIDTH = MIN 
		LABEL;

	FORMAT
		BF_SSN $9.
		NAME $47.
		COMMONLINE_ID $19.
		AC_LON_TYP $2.
		REPURCHASE_LENDER $6.
		BOND_ID $8.
	;

	VAR 
		BF_SSN 
		NAME 
		COMMONLINE_ID 
		AC_LON_TYP 
		BAL 
		REPURCHASE_LENDER 
		BOND_ID 
	;

	LABEL
		BF_SSN = 'SSN'
		NAME = 'Name'
		COMMONLINE_ID = 'CommonLine ID'
		AC_LON_TYP = 'Loan Type'
		BAL = 'Principle Amount as of Repurchase Date'
		REPURCHASE_LENDER = 'Repurchase Lender'
		BOND_ID  = 'Bond ID'
	;
RUN;

PROC PRINTTO; RUN;
