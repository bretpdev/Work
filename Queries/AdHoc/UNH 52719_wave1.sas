*============= UTLWS14 BILLING STATEMENT ==================;
*Duster;
%LET SRVR = DUSTER;
%LET DB = DLGSUTWH;
*UHEAA Test region;
/*%LET SRVR = QADBD004;*/
/*%LET DB = DLGSWQUT;*/
*----------------------------------------------;
*uncomment for PROMOTION, comment for TESTING;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
/*%LET RPTLIBX = /sas/whse/progrevw; */
*----------------------------------------------;
*uncomment for TESTING, comment for PROMOTION;
	%LET RPTLIB = T:\SAS;
	%LET RPTLIBX = T:\SAS; 
	%SYSLPUT SRVR = &SRVR;
	%SYSLPUT DB = &DB;
*----------------------------------------------;
* borrowers ;
FILENAME REPORT2 "&RPTLIBX/ULWS14.LWS14R2";*installment bill;
FILENAME REPORT4 "&RPTLIBX/ULWS14.LWS14R4";*interest statement;
FILENAME REPORT6 "&RPTLIB/ULWS14.LWS14R6";*error report >28 loans;
FILENAME REPORT7 "&RPTLIB/ULWS14.LWS14R7";*error report nonmatch;
FILENAME REPORT8 "&RPTLIB/ULWS14.LWS14R8";*duplicate bills;
FILENAME REPORT10 "&RPTLIB/ULWS14.LWS14R10";*TILP installment;
FILENAME REPORT12 "&RPTLIB/ULWS14.LWS14R12";*due diligence 1;
FILENAME REPORT13 "&RPTLIB/ULWS14.LWS14R13";*due diligence 2;
FILENAME REPORT15 "&RPTLIB/ULWS14.LWS14R15";*due diligence 3;
FILENAME REPORT16 "&RPTLIB/ULWS14.LWS14R16";*due diligence 4;
FILENAME REPORT17 "&RPTLIB/ULWS14.LWS14R17";*due diligence 5;
FILENAME REPORT18 "&RPTLIB/ULWS14.LWS14R18";*due diligence 6;
FILENAME REPORT19 "&RPTLIB/ULWS14.LWS14R19";*due diligence 7;
FILENAME REPORT20 "&RPTLIB/ULWS14.LWS14R20";*due diligence 8;
FILENAME REPORT21 "&RPTLIB/ULWS14.LWS14R21";*reduced payment;
* endorsers ;
FILENAME REPORT27 "&RPTLIB/ULWS14.LWS14R27";*installment bill;
FILENAME REPORT29 "&RPTLIB/ULWS14.LWS14R29";*interest statement;
FILENAME REPORT31 "&RPTLIB/ULWS14.LWS14R31";*due diligence 1;
FILENAME REPORT32 "&RPTLIB/ULWS14.LWS14R32";*due diligence 2;
FILENAME REPORT33 "&RPTLIB/ULWS14.LWS14R33";*due diligence 3;
FILENAME REPORT34 "&RPTLIB/ULWS14.LWS14R34";*due diligence 4;
FILENAME REPORT35 "&RPTLIB/ULWS14.LWS14R35";*due diligence 5;
FILENAME REPORT36 "&RPTLIB/ULWS14.LWS14R36";*due diligence 6;
FILENAME REPORT37 "&RPTLIB/ULWS14.LWS14R37";*due diligence 7;
FILENAME REPORT38 "&RPTLIB/ULWS14.LWS14R38";*due diligence 8;
FILENAME REPORT39 "&RPTLIB/ULWS14.LWS14R39";*reduced payment;
FILENAME REPORT99 "&RPTLIB/ULWS14.LWS14R99";*invalid address & no ecorr;
/***********************************************************************
* IF THIS JOB NEEDS TO BE RUN FOR A SPECIFIC SSN ENTER IT BELOW AND 
* UNNCOMMENT THE APPROPRIATE CODE LINES
************************************************************************/
%LET SSN = '100342722';*single borrower;
/*%LET SSN = 'xxxxxxxxx','xxxxxxxxx','xxxxxxxxx';*multiple borrowers;*/
%SYSLPUT SSN = &SSN;
/************************************************************************/
%LET MAX_LN=28; *SET MAX# OF LOANS TO WRITE TO REPORT;
DATA _NULL_;
	CALL SYMPUT('CURDATE',PUT(INTNX('DAY',TODAY(),0,'BEGINNING'), MMDDYY10.));
/*	CALL SYMPUT('DAYS_AGO_7',"'"||PUT(INTNX('DAY',TODAY(),-7,'BEGINNING'), MMDDYY10.)||"'");*wave1;*/
/*	CALL SYMPUT('DAYS_AGO_0',"'"||PUT(INTNX('DAY',TODAY(),0,'BEGINNING'), MMDDYY10.)||"'");*wave1;*/
RUN;

*wave 1 range;
%LET days_ago_7 = '12/01/2016';*begin date range;
%LET days_ago_0 = '07/20/2017';*end date range;

*---------------------------------------------------;
*uncomment for TESTING, comment for PROMOTION;
	%SYSLPUT DAYS_AGO_0 = &DAYS_AGO_0;
	%SYSLPUT DAYS_AGO_7 = &DAYS_AGO_7;
	LIBNAME  WORKLOCL  REMOTE  SERVER=&SRVR SLIBREF=WORK;
	RSUBMIT;
	LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1;
*----------------------------------------------------;
LIBNAME SAS_TAB V8 '/sas/whse/progrevw';
PROC SQL NOPRINT;
	SELECT "'"||TRIM(LENDER_ID)||"'"
		INTO :UHEAA_LIST SEPARATED BY ","
	FROM SAS_TAB.LDR_AFF
	WHERE AFFILIATION = 'UHEAA';
QUIT;
LIBNAME SAS_TAB CLEAR;
/*INPUT LOAN TYPES FOR PRIVATE AND FFEL LOANS*/
DATA LOAN_TYPES;
	FORMAT LN_TYP LN_PGM $50.;
	INFILE "/sas/whse/progrevw/GENR_REF_LoanTypes.txt" DLM=',' MISSOVER DSD;
	INFORMAT LN_TYP LN_PGM $50.;
	INPUT LN_TYP LN_PGM ;
	LN_PGM = UPCASE(LN_PGM);
RUN;
/*CREATE MACRO VARIABLE LISTS OF LOAN PROGRAMS(FFEL AND PRIVATE LOANS)*/
PROC SQL NOPRINT;
	SELECT "'"||TRIM(LN_TYP)||"'" 
		INTO :PRIVATE_LIST SEPARATED BY "," /*PRIVATE LOAN LIST*/
	FROM LOAN_TYPES
	WHERE LN_PGM ^= 'FFEL';
QUIT;
PROC SQL NOPRINT;
	SELECT "'"||TRIM(LN_TYP)||"'" 
		INTO :FFEL_LIST SEPARATED BY "," /*PRIVATE LOAN LIST*/
	FROM LOAN_TYPES
	WHERE LN_PGM = 'FFEL';
QUIT;
PROC SQL STIMER;
	CONNECT TO DB2 (DATABASE=&DB); 
	CREATE TABLE BLSTMNT AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
			(
				SELECT DISTINCT 
					BL10.BF_SSN
					,BL10.LD_BIL_CRT
					,BL10.LN_SEQ_BIL_WI_DTE
					,BL10.LC_IND_BIL_SNT
					,BL10.LD_RBL_LST
					,LN10.LN_SEQ
					,AY10.LD_ATY_REQ_RCV
					,RS10.LD_RPS_1_PAY_DU
					,LN10.LF_LON_CUR_OWN	
					,LN10.LD_LON_1_DSB								
					,LN10.IC_LON_PGM		
					,CASE
						 WHEN LN80.LC_LON_STA_BIL = '1' THEN 'REPAY'
						 WHEN LN80.LC_LON_STA_BIL = '2' THEN 'FORB'
						 WHEN LN80.LC_LON_STA_BIL = '3' THEN 'DEFER'
						 WHEN LN80.LC_LON_STA_BIL = '4' THEN 'SCHOOL'
						 WHEN LN80.LC_LON_STA_BIL = '5' THEN 'GRACE'
					 END AS LC_LON_STA_BIL
					,LN80.LR_INT_BIL
					,COALESCE(LN80.LA_CUR_PRN_BIL,0) AS LA_CUR_PRN_BIL
					,LN80.LA_BIL_PAS_DU
					,LN80.LA_BIL_CUR_DU
					,LN80.LA_LTE_FEE_OTS_PRT AS LN_LTE_FEE
					,CASE
						WHEN BL10.LC_IND_BIL_SNT = '4'
						THEN COALESCE(LN80.LA_BIL_CUR_DU,0)
						WHEN BL10.LC_IND_BIL_SNT <> '4'
						THEN COALESCE(LN80.LA_BIL_DU_PRT,0)
					END AS LA_BIL_DU_PRT
					,BL10.LC_BIL_TYP
					,BL10.LD_BIL_DU
					,BL10.LN_SEQ_BIL_WI_DTE
					,CASE
					 	WHEN BL10.LN_SEQ_BIL_WI_DTE < 10 
					  	THEN RTRIM('0')||CHAR(BL10.LN_SEQ_BIL_WI_DTE)
					 	ELSE CHAR(BL10.LN_SEQ_BIL_WI_DTE)
					 END AS BLSQ
					,CASE
					 	WHEN BL10.LD_RBL_LST IS NULL 
					  	THEN BL10.LD_BIL_CRT
						ELSE BL10.LD_RBL_LST
					 END AS BLL_DT
					,BL10.LC_BIL_MTD
					,BL10.LC_IND_BIL_SNT AS SENT_IND_GROUP
					/*for R2-99 processing*/
					,CASE
						WHEN PH05A.DI_CNC_EBL_OPI = 'Y'
							AND DI_VLD_CNC_EML_ADR = 'Y'
							THEN 'Y'
						ELSE 'N'
					END AS DI_CNC_EBL_OPI
					,CASE
						WHEN LN10.LC_LON_SND_CHC IN ('B','E','Y')
							THEN 'Y'
						ELSE 'N'
					END AS LC_LON_SND_CHC
				FROM 
					OLWHRM1.LN10_LON LN10
					INNER JOIN OLWHRM1.LN80_LON_BIL_CRF LN80
						ON LN10.BF_SSN = LN80.BF_SSN
						AND LN10.LN_SEQ = LN80.LN_SEQ
					INNER JOIN OLWHRM1.BL10_BR_BIL BL10
						ON LN80.BF_SSN = BL10.BF_SSN
						AND LN80.LD_BIL_CRT = BL10.LD_BIL_CRT
						AND LN80.LN_SEQ_BIL_WI_DTE = BL10.LN_SEQ_BIL_WI_DTE
					LEFT JOIN 
					(
						SELECT 
							BF_SSN
							,MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
						FROM 
							OLWHRM1.AY10_BR_LON_ATY 
						WHERE 
							PF_REQ_ACT IN ('BILLS','BILLC')
							AND LC_STA_ACTY10 = 'A'
						GROUP BY 
							BF_SSN
					) AY10
						ON LN10.BF_SSN = AY10.BF_SSN
					LEFT JOIN OLWHRM1.LN65_LON_RPS LN65
						ON LN10.BF_SSN = LN65.BF_SSN
						AND LN10.LN_SEQ = LN65.LN_SEQ
						AND LN65.LC_STA_LON65 = 'A'
					LEFT JOIN OLWHRM1.RS10_BR_RPD	RS10
						ON LN65.BF_SSN = RS10.BF_SSN
						AND LN65.LN_RPS_SEQ = RS10.LN_RPS_SEQ
						AND RS10.LC_STA_RPST10 = 'A'
					/*for R2-99 processing*/
					LEFT JOIN
					(
						SELECT
							PD10.DF_PRS_ID
							,PH05.DI_CNC_EBL_OPI
							,PH05.DI_VLD_CNC_EML_ADR
						FROM
							OLWHRM1.PD10_PRS_NME PD10
							INNER JOIN OLWHRM1.PH05_CNC_EML PH05
								ON CAST(PD10.DF_SPE_ACC_ID AS DECIMAL(10)) = PH05.DF_SPE_ID
								AND PD10.DF_PRS_ID NOT LIKE 'P%'	
					) PH05A
						ON PH05A.DF_PRS_ID = LN10.BF_SSN
				WHERE 
					LN10.LC_STA_LON10 = 'R'
					AND LN10.LA_CUR_PRI + COALESCE(LN10.LA_NSI_OTS,0) > 0
					AND LN80.LC_STA_LON80 = 'A'
					AND BL10.LC_STA_BIL10 = 'A'
					AND BL10.LC_BIL_TYP IN ('P','I','C')
					AND 
					(
/*						BL10.LD_BIL_CRT >= &DAYS_AGO_7*/
/*						OR BL10.LD_RBL_LST >= &DAYS_AGO_7*/
						BL10.LD_BIL_CRT between &DAYS_AGO_7 and &days_ago_0
						OR BL10.LD_RBL_LST between &DAYS_AGO_7 and &days_ago_0
					)
/*					USE FOR TESTING*/
					AND LN10.BF_SSN IN (&SSN)

				FOR READ ONLY WITH UR
			);
DISCONNECT FROM DB2;

/*DATA BLSTMNT;*/
/*	SET BLSTMNT;*/
/*	WHERE LC_IND_BIL_SNT IN */
/*		(*/
/*			 '1' /*Normal Paper bill is printed and sent to borrower*/*/
/*			,'2' /*Reprint of normal bill*/*/
/*			,'4' /*Use due date minus current date > = 5 days (insufficient lead time)*/*/
/*			,'7' /*Paid ahead bill*/*/
/*			,'G' /*When a C bill is printed (a C bill = EFT bill printed and sent to borrower without an amount due.  1st time notice and letter will not be extracted)*/*/
/*			,'A' /*ACH AND/OR EBILL BILL-NOT PRINTED*/*/
/*			,'B' /*ACH AND/OR EBILL BILL NOT PRINTED < 15 DAYS NOTICE*/*/
/*			,'C' /*ACH AND/OR EBILL 1ST NOTICE NOT PRINTED*/*/
/*			,'D' /*ACH AND/OR EBILL INSUFFICIENT LEAD TIME NOT PRINTED*/*/
/*			,'E' /*MONTHLY ACH AND/OR EBILL BILL NOT PRINTED*/*/
/*			,'F' /*ACH AND/OR EBILL BILL PRINTED NOT SENT<15 DAYS NOTICE*/*/
/*			,'H' /*ACH AND/OR EBILL INSUFFICIENT LEAD TIME NOT PRINTED*/*/
/*			,'I' /*MONTHLY ACH AND/OR EBILL BILL NOT PRINTED*/*/
/*			,'J' /*<15 DAYS REPRINT REQUEST NOT PRINTED*/*/
/*			,'K' /*ACH AND/OR EBILL 1ST NOTICE REPRINT RQST NOT PRINTD*/*/
/*			,'L' /*ACH AND/OR EBILL INSUFFICIENT LEAD TIME RPQ NOT PRNTD*/*/
/*			,'M' /*MONTHLY ACH AND/OR EBILL BILL REPRINT RQST NOT PRINTD*/*/
/*			,'P' /*ACH AND/OR EBILL REPRINT REQUEST NOT PRINTED*/*/
/*			,'Q' /*ACH AND/OR EBILL PAID AHEAD REPRINT REQST NOT PRINTED*/*/
/*			,'R' /*ACH AND/OR EBILL EVALUATED BY LATE FEES PROCESS*/*/
/*			,'8' /*INACTIVE LOANS INCLUDED IN BILL*/*/
/*			,'T' /*Reduced Payment Bill (Borrower in a Reduced Payment Forbearance)*/*/
/*		);*/
/*	IF LD_ATY_REQ_RCV = . THEN  /*1ST TIME BILL*/*/
/*		OUTPUT;*/
/*	ELSE IF LD_ATY_REQ_RCV < LD_BIL_CRT THEN  /*STANDARD BILL*/*/
/*		OUTPUT;*/
/*	ELSE IF LD_ATY_REQ_RCV < LD_RBL_LST THEN  /*REBILL*/*/
/*		OUTPUT;*/
/*RUN;*/
;PROC SORT DATA=BLSTMNT NODUPKEY;
	BY BF_SSN
		LD_BIL_CRT 
		LN_SEQ_BIL_WI_DTE 
		LN_SEQ;
RUN;
/*****************************************************
* GATHER INFO FOR BILLS SELECTED ABOVE 
******************************************************/
PROC SQL STIMER;
CONNECT TO DB2 (DATABASE=&DB);
/*****************************************************
* BILLING ADDRESS
******************************************************/
CREATE TABLE BILL_ADR AS
	SELECT DISTINCT 
		PD30A.*
	FROM 
		BLSTMNT BL
		INNER JOIN CONNECTION TO DB2 
		(
			SELECT 
				LN10.BF_SSN
				,CASE
					WHEN PD30_B.DC_DOM_ST_B <> ' ' 
						AND SUBSTR(PD30_B.DF_ZIP_CDE_B,6,1) = ' '
					THEN SUBSTR(PD30_B.DF_ZIP_CDE_B,1,5)
					WHEN PD30_B.DC_DOM_ST_B <> ' ' 
						AND SUBSTR(PD30_B.DF_ZIP_CDE_B,6,1) <> ' '
					THEN SUBSTR(PD30_B.DF_ZIP_CDE_B,1,5)||'-'||SUBSTR(PD30_B.DF_ZIP_CDE_B,6,4)
					WHEN PD30_B.DC_DOM_ST_B = ' '
					THEN PD30_B.DF_ZIP_CDE_B
				 END AS DF_ZIP_CDE_B
				,PD30_B.DM_CT_B
				,PD30_B.DX_STR_ADR_3_B
				,PD30_B.DX_STR_ADR_2_B
				,PD30_B.DX_STR_ADR_1_B
				,PD30_B.DC_ADR_B
				,CASE 
				 	WHEN PD30_B.DC_DOM_ST_B <> ' ' 
					THEN PD30_B.DC_DOM_ST_B
					WHEN PD30_B.DC_DOM_ST_B = ' ' 
					THEN PD30_B.DM_FGN_ST_B
				 END AS STATE_B
				,PD30_B.DM_FGN_CNY_B
				/*for R2-99 processing*/
				,PD30_B.DI_VLD_ADR_B_borr
			FROM 
				OLWHRM1.PD10_PRS_NME PD10
				INNER JOIN OLWHRM1.LN10_LON LN10
					ON PD10.DF_PRS_ID = LN10.BF_SSN
				INNER JOIN 
				(
					SELECT 
						DF_PRS_ID
						,DF_ZIP_CDE AS DF_ZIP_CDE_B
						,DM_CT AS DM_CT_B
						,DX_STR_ADR_3 AS DX_STR_ADR_3_B
						,DX_STR_ADR_2 AS DX_STR_ADR_2_B
						,DX_STR_ADR_1 AS DX_STR_ADR_1_B
						,DC_ADR AS DC_ADR_B
						,DC_DOM_ST AS DC_DOM_ST_B
						,DM_FGN_ST AS DM_FGN_ST_B
						,DM_FGN_CNY AS DM_FGN_CNY_B
						/*for R2-99 processing*/
						,DI_VLD_ADR AS DI_VLD_ADR_B_borr
					 FROM 
						OLWHRM1.PD30_PRS_ADR
					 WHERE 
						DC_ADR = 'B'
						AND DI_VLD_ADR = 'Y'
				) PD30_B
				ON PD10.DF_PRS_ID = PD30_B.DF_PRS_ID
			WHERE 
				LN10.LC_STA_LON10 = 'R'
/*				USE FOR TESTING*/
				AND LN10.BF_SSN IN (&SSN)

			FOR READ ONLY WITH UR
		) PD30A
			ON BL.BF_SSN = PD30A.BF_SSN
;
/*****************************************************
* LEGAL ADDRESS
******************************************************/
CREATE TABLE LEGL_ADR AS
	SELECT DISTINCT 
		PD.*
	FROM 
		BLSTMNT BL
	INNER JOIN CONNECTION TO DB2 
	(
		SELECT DISTINCT 
			LN10.BF_SSN	
			,PD10.DF_SPE_ACC_ID
			,CASE 
				WHEN PD10.DM_PRS_MID = ' ' 
				THEN RTRIM(PD10.DM_PRS_1)||' '||PD10.DM_PRS_LST
				WHEN PD10.DM_PRS_MID <> ' ' 
				THEN RTRIM(PD10.DM_PRS_1)||' '||RTRIM(PD10.DM_PRS_MID)||' '||PD10.DM_PRS_LST
			 END AS NAME
			,PD10.DM_PRS_1
			,PD10.DM_PRS_MID
			,PD10.DM_PRS_LST
		/*LEGAL ADDRESS INFO*/
			,CASE
				WHEN PD30_L.DC_DOM_ST <> ' ' 
					AND SUBSTR(PD30_L.DF_ZIP_CDE,6,1) = ' '
				THEN SUBSTR(PD30_L.DF_ZIP_CDE,1,5)
				WHEN PD30_L.DC_DOM_ST <> ' ' 
					AND SUBSTR(PD30_L.DF_ZIP_CDE,6,1) <> ' '
				THEN SUBSTR(PD30_L.DF_ZIP_CDE,1,5)||'-'||SUBSTR(PD30_L.DF_ZIP_CDE,6,4)
				WHEN PD30_L.DC_DOM_ST = ' '
				THEN PD30_L.DF_ZIP_CDE
			 END AS DF_ZIP_CDE_L
			,PD30_L.DM_CT AS DM_CT_L
			,PD30_L.DX_STR_ADR_3 AS DX_STR_ADR_3_L
			,PD30_L.DX_STR_ADR_2 AS DX_STR_ADR_2_L
			,PD30_L.DX_STR_ADR_1 AS DX_STR_ADR_1_L
			,PD30_L.DC_ADR AS DC_ADR_L
			,CASE 
			 	WHEN PD30_L.DC_DOM_ST <> ' ' 
				THEN PD30_L.DC_DOM_ST
			 	WHEN PD30_L.DC_DOM_ST = ' ' 
				THEN PD30_L.DM_FGN_ST
			 END AS STATE_L
			,PD30_L.DM_FGN_CNY AS DM_FGN_CNY_L
			,PD32A.DI_VLD_ADR_EML
			/*for R2-99 processing*/
			,PD30_L.DI_VLD_ADR AS DI_VLD_ADR_L_borr
		FROM 
			OLWHRM1.PD10_PRS_NME PD10
			INNER JOIN OLWHRM1.PD30_PRS_ADR PD30_L
				ON PD10.DF_PRS_ID = PD30_L.DF_PRS_ID
			INNER JOIN OLWHRM1.LN10_LON LN10
				ON PD10.DF_PRS_ID = LN10.BF_SSN
			LEFT JOIN 
			(
				SELECT DISTINCT 
					DF_PRS_ID	
					,DI_VLD_ADR_EML
				FROM 
					OLWHRM1.PD32_PRS_ADR_EML 
				WHERE 
					DI_VLD_ADR_EML = 'N'
					AND DC_STA_PD32 = 'A'
			) PD32A
			ON PD10.DF_PRS_ID = PD32A.DF_PRS_ID
		WHERE 
			LN10.LC_STA_LON10 = 'R'
			AND PD30_L.DC_ADR = 'L'
/*			USE FOR TESTING*/
			AND LN10.BF_SSN IN (&SSN)

		FOR READ ONLY WITH UR
	) PD
		ON BL.BF_SSN = PD.BF_SSN
;
/*****************************************************
* GET MOST RECENT TRANSACTION INFO AT BORROWER LEVEL
******************************************************/
CREATE TABLE BOR_TRX AS
	SELECT DISTINCT 
		LN90C.*
	FROM 
		BLSTMNT BL
		INNER JOIN CONNECTION TO DB2 
		(/*modified to get only month-by-month transactions along with bl10 to use as PK*/
			SELECT 
				bl10.ld_bil_crt,
				LN90A.BF_SSN 
				,LN90A.PC_FAT_TYP||LN90A.PC_FAT_SUB_TYP AS TX_TYP
				,LN90A.LD_FAT_EFF 
				,ABS(SUM(LN90A.LA_FAT_CUR_PRI)) AS LA_FAT_CUR_PRI
				,ABS(SUM(LN90A.LA_FAT_NSI)) AS LA_FAT_NSI
				,ABS(SUM(LN90A.LA_FAT_LTE_FEE)) AS LA_FAT_LTE_FEE
			FROM
				olwhrm1.LN10_LON LN10
				INNER JOIN olwhrm1.LN75_BIL_LON_FAT LN75
				    ON LN10.BF_SSN = LN75.BF_SSN
				    AND LN10.LN_SEQ = LN75.LN_SEQ
				inner join olwhrm1.ln90_fin_aty ln90a
					on ln75.bf_ssn = ln90a.bf_ssn
					and ln75.ln_seq = ln90a.ln_seq
					and ln75.ln_fat_seq = ln90a.ln_fat_seq
				INNER JOIN olwhrm1.BL10_BR_BIL BL10
				    ON LN10.BF_SSN = BL10.BF_SSN
				    AND LN75.LD_BIL_CRT = BL10.LD_BIL_CRT
				    AND LN75.LN_SEQ_BIL_WI_DTE = BL10.LN_SEQ_BIL_WI_DTE
			WHERE 
				LN90A.PC_FAT_TYP = '10'
				AND LN90A.PC_FAT_SUB_TYP IN ('10','11','12','41')
				AND LN90A.LC_FAT_REV_REA = ' ' 
				AND LN90A.LC_STA_LON90 = 'A'
				and days(ln90a.ld_sta_lon90) between days(&days_ago_7) and days(&days_ago_0)
				AND LN90A.BF_SSN IN (&SSN)
			GROUP BY 
				bl10.ld_bil_crt,
				LN90A.BF_SSN
				,LN90A.PC_FAT_TYP
				,LN90A.PC_FAT_SUB_TYP
				,LN90A.LD_FAT_EFF 
		) LN90C
			ON BL.BF_SSN = LN90C.BF_SSN
;
/**/
/*CREATE TABLE BOR_TRX AS*/
/*	SELECT DISTINCT */
/*		LN90C.**/
/*	FROM */
/*		BLSTMNT BL*/
/*		INNER JOIN CONNECTION TO DB2 */
/*		(*/
/*			SELECT */
/*				LN90A.BF_SSN */
/*				,LN90A.PC_FAT_TYP||LN90A.PC_FAT_SUB_TYP AS TX_TYP*/
/*				,LN90A.LD_FAT_EFF */
/*				,ABS(SUM(LN90A.LA_FAT_CUR_PRI)) AS LA_FAT_CUR_PRI*/
/*				,ABS(SUM(LN90A.LA_FAT_NSI)) AS LA_FAT_NSI*/
/*				,ABS(SUM(LN90A.LA_FAT_LTE_FEE)) AS LA_FAT_LTE_FEE*/
/*			FROM */
/*				OLWHRM1.LN90_FIN_ATY LN90A*/
/*				INNER JOIN */
/*				(*/
/*					SELECT */
/*						BF_SSN*/
/*						,MAX(LD_FAT_EFF) AS LD_FAT_EFF*/
/*					FROM */
/*						OLWHRM1.LN90_FIN_ATY */
/*					WHERE */
/*						LC_FAT_REV_REA = ''*/
/*						AND LC_STA_LON90 = 'A'*/
/*						AND PC_FAT_TYP = '10'*/
/*						AND PC_FAT_SUB_TYP IN ('10','11','12','41')*/
/*					GROUP BY */
/*						BF_SSN*/
/*				) LN90B*/
/*				ON LN90A.BF_SSN = LN90B.BF_SSN*/
/*				AND LN90A.LD_FAT_EFF = LN90B.LD_FAT_EFF*/
/*			WHERE */
/*				LN90A.PC_FAT_TYP = '10'*/
/*				AND LN90A.PC_FAT_SUB_TYP IN ('10','11','12','41')*/
/*				AND LN90A.LC_FAT_REV_REA = ' ' */
/*				AND LN90A.LC_STA_LON90 = 'A'*/
/*				/*USE FOR TESTING*/*/
/*				AND LN90A.BF_SSN IN (&SSN)*/
/*			GROUP BY */
/*				LN90A.BF_SSN*/
/*				,LN90A.PC_FAT_TYP*/
/*				,LN90A.PC_FAT_SUB_TYP*/
/*				,LN90A.LD_FAT_EFF */
/*			FOR READ ONLY WITH UR*/
/*		) LN90C*/
/*			ON BL.BF_SSN = LN90C.BF_SSN*/
;
/*****************************************************
* GET TOTAL OF LATE FEES ASSESSED AT LOAN LEVEL
******************************************************/
CREATE TABLE LATE_FEES AS
	SELECT DISTINCT 
		LN80A.*
	FROM 
		BLSTMNT BL
		INNER JOIN CONNECTION TO DB2 
		(
			SELECT DISTINCT 
				BF_SSN
				,LN_SEQ
				,LD_BIL_CRT
				,LN_SEQ_BIL_WI_DTE
				,SUM(COALESCE(LA_LTE_FEE_OTS_PRT,0)) AS BOR_LTE_FEE
			FROM 
				OLWHRM1.LN80_LON_BIL_CRF
			WHERE 
				LC_STA_LON80 = 'A'
/*				USE FOR TESTING*/
				AND BF_SSN IN (&SSN)
			GROUP BY 
				BF_SSN
				,LN_SEQ
				,LD_BIL_CRT
				,LN_SEQ_BIL_WI_DTE
			FOR READ ONLY WITH UR
		) LN80A
			ON BL.BF_SSN = LN80A.BF_SSN
			AND BL.LN_SEQ = LN80A.LN_SEQ
			AND BL.LD_BIL_CRT = LN80A.LD_BIL_CRT
			AND BL.LN_SEQ_BIL_WI_DTE = LN80A.LN_SEQ_BIL_WI_DTE
;
/*****************************************************
* CO-BORROWER INFO
******************************************************/
CREATE TABLE COBRW_A AS
	SELECT 
		*
	FROM 
		CONNECTION TO DB2 
	(
		SELECT DISTINCT 
			LN20.BF_SSN
			,LN20.LF_EDS
			,LN20.LN_SEQ
			,PD10.DF_SPE_ACC_ID
			,CASE
				WHEN PD10.DM_PRS_MID = ' ' 
				THEN RTRIM(PD10.DM_PRS_1)||' '||PD10.DM_PRS_LST
				WHEN PD10.DM_PRS_MID <> ' ' 
				THEN RTRIM(PD10.DM_PRS_1)||' '||RTRIM(PD10.DM_PRS_MID)||' '||PD10.DM_PRS_LST
			 END AS NAME
			,PD10.DM_PRS_1
			,PD10.DM_PRS_MID
			,PD10.DM_PRS_LST
		/*CO-BORROWER BILLING ADDRESS*/
			,CASE
				WHEN PD30_B.DC_DOM_ST_B <> ' ' 
					AND SUBSTR(PD30_B.DF_ZIP_CDE_B,6,1) = ' '
				THEN SUBSTR(PD30_B.DF_ZIP_CDE_B,1,5)
				WHEN PD30_B.DC_DOM_ST_B <> ' ' 
					AND SUBSTR(PD30_B.DF_ZIP_CDE_B,6,1) <> ' '
				THEN SUBSTR(PD30_B.DF_ZIP_CDE_B,1,5)||'-'||SUBSTR(PD30_B.DF_ZIP_CDE_B,6,4)
				WHEN PD30_B.DC_DOM_ST_B = ' '
				THEN PD30_B.DF_ZIP_CDE_B
			 END AS DF_ZIP_CDE_B
			,PD30_B.DM_CT_B
			,PD30_B.DX_STR_ADR_3_B
			,PD30_B.DX_STR_ADR_2_B
			,PD30_B.DX_STR_ADR_1_B
			,PD30_B.DC_ADR_B
			,CASE 
			 	WHEN PD30_B.DC_DOM_ST_B <> ' ' 
				THEN PD30_B.DC_DOM_ST_B
				WHEN PD30_B.DC_DOM_ST_B = ' ' 
				THEN PD30_B.DM_FGN_ST_B
			 END AS STATE_B
			,PD30_B.DM_FGN_CNY_B
		/*LEGAL ADDRESS INFO*/
			,CASE
				WHEN PD30_L.DC_DOM_ST <> ' ' 
					AND SUBSTR(PD30_L.DF_ZIP_CDE,6,1) = ' '
				THEN SUBSTR(PD30_L.DF_ZIP_CDE,1,5)
				WHEN PD30_L.DC_DOM_ST <> ' ' 
					AND SUBSTR(PD30_L.DF_ZIP_CDE,6,1) <> ' '
				THEN SUBSTR(PD30_L.DF_ZIP_CDE,1,5)||'-'||SUBSTR(PD30_L.DF_ZIP_CDE,6,4)
				WHEN PD30_L.DC_DOM_ST = ' '
				THEN PD30_L.DF_ZIP_CDE
			 END AS DF_ZIP_CDE_L
			,PD30_L.DM_CT AS DM_CT_L
			,PD30_L.DX_STR_ADR_3 AS DX_STR_ADR_3_L
			,PD30_L.DX_STR_ADR_2 AS DX_STR_ADR_2_L
			,PD30_L.DX_STR_ADR_1 AS DX_STR_ADR_1_L
			,PD30_L.DC_ADR AS DC_ADR_L
			,CASE 
			 	WHEN PD30_L.DC_DOM_ST <> ' ' 
				THEN PD30_L.DC_DOM_ST
			 	WHEN PD30_L.DC_DOM_ST = ' ' 
				THEN PD30_L.DM_FGN_ST
			 END AS STATE_L
			,PD30_L.DM_FGN_CNY AS DM_FGN_CNY_L
			,PD32A.DI_VLD_ADR_EML
			/*for R2-99 processing*/
			,NULL AS DI_CNC_EBL_OPI /*will use borrower's flags*/
			,NULL AS LC_LON_SND_CHC /*will use borrower's flags*/
			,CASE
				WHEN PD30_B.DI_VLD_ADR_B_endo = 'Y'
					THEN 'Y'
				ELSE 'N'
			END AS DI_VLD_ADR_B_endo
			,CASE
				WHEN PD30_L.DI_VLD_ADR_L_endo = 'Y'
					THEN 'Y'
				ELSE 'N'
			END AS DI_VLD_ADR_L_endo
		FROM 
			OLWHRM1.LN20_EDS LN20
			LEFT JOIN 
			(
				SELECT 
					DF_PRS_ID
					,DF_ZIP_CDE 	AS DF_ZIP_CDE_B
					,DM_CT 			AS DM_CT_B
					,DX_STR_ADR_3 	AS DX_STR_ADR_3_B
					,DX_STR_ADR_2 	AS DX_STR_ADR_2_B
					,DX_STR_ADR_1 	AS DX_STR_ADR_1_B
					,DC_ADR 		AS DC_ADR_B
					,DC_DOM_ST 		AS DC_DOM_ST_B
					,DM_FGN_ST 		AS DM_FGN_ST_B
					,DM_FGN_CNY 	AS DM_FGN_CNY_B
					/*for R2-99 processing*/
					,DI_VLD_ADR 	AS DI_VLD_ADR_B_endo
				FROM 
					OLWHRM1.PD30_PRS_ADR 
				WHERE 
					DC_ADR = 'B'
					AND DI_VLD_ADR = 'Y'
			) PD30_B
				ON LN20.LF_EDS = PD30_B.DF_PRS_ID
			LEFT JOIN
			(
				SELECT 
					DF_PRS_ID
					,DF_ZIP_CDE 	
					,DM_CT 			
					,DX_STR_ADR_3 	
					,DX_STR_ADR_2 	
					,DX_STR_ADR_1 	
					,DC_ADR 		
					,DC_DOM_ST 		
					,DM_FGN_ST 		
					,DM_FGN_CNY
					/*for R2-99 processing*/
					,DI_VLD_ADR 	AS DI_VLD_ADR_L_endo
				FROM 
					OLWHRM1.PD30_PRS_ADR 
				WHERE 
					DC_ADR = 'L'
					AND DI_VLD_ADR = 'Y'
			) PD30_L
				ON LN20.LF_EDS = PD30_L.DF_PRS_ID
			INNER JOIN OLWHRM1.PD10_PRS_NME PD10
				ON LN20.LF_EDS = PD10.DF_PRS_ID
			INNER JOIN OLWHRM1.LN10_LON LN10
				ON LN20.BF_SSN = LN10.BF_SSN
				AND LN20.LN_SEQ = LN10.LN_SEQ
			LEFT JOIN 
			(
				SELECT DISTINCT 
					DF_PRS_ID	
					,DI_VLD_ADR_EML
				FROM 
					OLWHRM1.PD32_PRS_ADR_EML 
				WHERE 
					DI_VLD_ADR_EML = 'N'
					AND DC_STA_PD32 = 'A'
			) PD32A
				ON PD10.DF_PRS_ID = PD32A.DF_PRS_ID
		WHERE 
			LN20.LC_STA_LON20 = 'A'
			AND 
			(
				(
					LN20.LC_EDS_TYP = 'M'
					AND LN10.IC_LON_PGM IN (&FFEL_LIST)
				)
			OR
				(
					LN20.LC_EDS_TYP = 'S'
					AND LN10.IC_LON_PGM IN (&PRIVATE_LIST)
				)
			)
/*			USE FOR TESTING*/
			AND LN20.BF_SSN IN (&SSN)

		FOR READ ONLY WITH UR
	);
/*****************************************************
* MESSAGE TEXT QUERY
******************************************************/
CREATE TABLE SV30 AS
	SELECT 
		*
	FROM 
		CONNECTION TO DB2 
	(
		SELECT 
			* 
		FROM 
			OLWHRM1.SV30_SER_TXT
		WHERE 
			PC_MSG_TYP = 'T'
			AND PC_MSG_BIL_TYP IN ('A','B','P','I','N')

		FOR READ ONLY WITH UR
	);
/*****************************************************
* DELINQUENCY TABLE QUERY
******************************************************/
CREATE TABLE DELQ AS
	SELECT DISTINCT 
		BL.BF_SSN
		,BL.LN_SEQ
		,LN.LD_DLQ_OCC
		,LN.LD_FOR_END
		,LN.LD_FOR_BEG
	FROM 
		BLSTMNT BL
		INNER JOIN CONNECTION TO DB2 
		(
			SELECT 
				LN16.BF_SSN
				,LN16.LN_SEQ
				,LN16.LD_DLQ_OCC
				,LN60.LD_FOR_END
				,LN60.LD_FOR_BEG
			FROM 
				OLWHRM1.LN16_LON_DLQ_HST LN16
				LEFT JOIN 
				(
					SELECT 
						BF_SSN
						,LN_SEQ
						,LD_FOR_END
						,LD_FOR_BEG
					FROM 
						OLWHRM1.LN60_BR_FOR_APV 
					WHERE 
						&DAYS_AGO_0 BETWEEN LD_FOR_BEG AND LD_FOR_END
						AND LC_STA_LON60 = 'A'
				) LN60
					ON LN16.BF_SSN = LN60.BF_SSN
					AND LN16.LN_SEQ = LN60.LN_SEQ
			WHERE 
				LN16.LC_STA_LON16 = '1'
				AND LN16.LC_DLQ_TYP = 'P'
/*				USE FOR TESTING*/
				AND LN16.BF_SSN IN (&SSN)

			FOR READ ONLY WITH UR
		) LN
			ON BL.BF_SSN = LN.BF_SSN
			AND BL.LN_SEQ = LN.LN_SEQ
;
/*****************************************************
* ORIGINAL PRINCIPAL FOR LOANS 
******************************************************/
CREATE TABLE LN15 AS
	SELECT DISTINCT 
		BL.BF_SSN
		,BL.LN_SEQ
		,LN15A.LN_BR_DSB_SEQ
	    ,LN15A.LA_DSB - LN15A.LA_DSB_CAN AS LA_DSB
	FROM 
		BLSTMNT BL
		INNER JOIN CONNECTION TO DB2 
		(
			SELECT 
				BF_SSN
				,LN_BR_DSB_SEQ
				,LN_SEQ
			    ,LA_DSB 
				,COALESCE(LA_DSB_CAN,0) AS LA_DSB_CAN
			FROM 
				OLWHRM1.LN15_DSB
			WHERE 
				LC_DSB_TYP = '2'
				AND LC_STA_LON15 IN ('1','3')

			FOR READ ONLY WITH UR
		) LN15A
			ON BL.BF_SSN = LN15A.BF_SSN
			AND BL.LN_SEQ = LN15A.LN_SEQ
;
/*****************************************************
* GET PRINCIPAL, INTEREST, AND AGGEGRATE AMOUNTS PAID
******************************************************/
CREATE TABLE LN90 AS
	SELECT DISTINCT 
		BL.BF_SSN
		,BL.LN_SEQ
		,LN90A.LN_FAT_SEQ
		,LN90A.TYP
	    ,LN90A.LA_FAT_CUR_PRI
		,LN90A.LA_FAT_NSI
		,LN90A.LA_FAT_PRI_NSI
		,LN90A.LA_FAT_LTE_FEE /*cumulative late fees paid*/
	FROM 
		BLSTMNT BL
		INNER JOIN CONNECTION TO DB2 
		(
			SELECT 
				BF_SSN
				,LN_SEQ
			    ,LN_FAT_SEQ
				,PC_FAT_TYP||PC_FAT_SUB_TYP AS TYP
			    ,ABS(COALESCE(LA_FAT_CUR_PRI,0)) AS LA_FAT_CUR_PRI
				,ABS(COALESCE(LA_FAT_NSI,0)) AS LA_FAT_NSI
				,ABS(COALESCE(LA_FAT_LTE_FEE,0)) AS LA_FAT_LTE_FEE /*cumulative late fees paid*/
				,ABS(COALESCE(LA_FAT_CUR_PRI,0) + COALESCE(LA_FAT_NSI,0) + COALESCE(LA_FAT_LTE_FEE,0)) AS LA_FAT_PRI_NSI
			FROM 
				OLWHRM1.LN90_FIN_ATY
			WHERE 
				PC_FAT_TYP = '10'
				AND LC_FAT_REV_REA = ''
				AND LC_STA_LON90 = 'A'

			FOR READ ONLY WITH UR
		) LN90A
			ON BL.BF_SSN = LN90A.BF_SSN
			AND BL.LN_SEQ = LN90A.LN_SEQ
;
DISCONNECT FROM DB2;
/**************************************************************
* GET APPLICABLE BILLS WITH A CO-BORROWER
***************************************************************/
PROC SORT DATA=BLSTMNT OUT=BILL_KEY 
	(KEEP=BF_SSN LN_SEQ LD_BIL_CRT LN_SEQ_BIL_WI_DTE);
	BY BF_SSN LN_SEQ;
RUN;
PROC SORT DATA=COBRW_A;
	BY BF_SSN LN_SEQ;
RUN;
DATA COBRW_A (DROP=LN_SEQ);
	MERGE BILL_KEY (IN=A) 
		  COBRW_A (IN=B);
	BY BF_SSN LN_SEQ;
	IF A=B;
RUN;





/*test join:

data bor_trx;
set worklocl.bor_trx;
run;

data blstmnt;
set worklocl.blstmnt;
run;

proc sql;
select * 
from blstmnt bl
left join(
			SELECT 
			ld_bil_crt,
				BF_SSN
				,LD_FAT_EFF 
				,SUM(LA_FAT_CUR_PRI) AS LA_FAT_CUR_PRI
				,SUM(LA_FAT_NSI) AS LA_FAT_NSI 
				,SUM(LA_FAT_LTE_FEE) AS LA_FAT_LTE_FEE
			FROM 
				BOR_TRX
			GROUP BY 
				BF_SSN
				,LD_FAT_EFF 
)trx
	on bl.bf_ssn = trx.bf_ssn
	and bl.ld_bil_crt = trx.ld_bil_crt
;quit;


*/












/**************************************************************
* COMBINE INFO FOR BLSTMNT DATA SET
***************************************************************/
PROC SQL;
CREATE TABLE BLSTMNT_TEMP AS 
	SELECT DISTINCT 
		BL.*
		,BUS.DF_ZIP_CDE_B
		,BUS.DM_CT_B 
		,BUS.DX_STR_ADR_3_B 
		,BUS.DX_STR_ADR_2_B 
		,BUS.DX_STR_ADR_1_B 
		,BUS.DC_ADR_B 
		,BUS.STATE_B 
		,BUS.DM_FGN_CNY_B
		,LEG.DF_SPE_ACC_ID 
		,LEG.NAME
		,LEG.DM_PRS_1 
		,LEG.DM_PRS_MID 
		,LEG.DM_PRS_LST 
		,LEG.DF_ZIP_CDE_L 
		,LEG.DM_CT_L 
		,LEG.DX_STR_ADR_3_L 
		,LEG.DX_STR_ADR_2_L 
		,LEG.DX_STR_ADR_1_L 
		,LEG.DC_ADR_L 
		,LEG.STATE_L 
		,LEG.DM_FGN_CNY_L
		,LEG.DI_VLD_ADR_EML


/*		,. as LD_FAT_EFF */
/*		,. as LA_FAT_CUR_PRI */
/*		,. as LA_FAT_NSI */
/*		,. as LA_FAT_LTE_FEE*/
		,TRX.LD_FAT_EFF 
		,TRX.LA_FAT_CUR_PRI 
		,TRX.LA_FAT_NSI 
		,TRX.LA_FAT_LTE_FEE


		,LFEE.BOR_LTE_FEE 
		,LN15.OPAFEL
		,LN90.TAPTP
		,LN90.TAPTI
		,LN90.TAGAP
		,LN90.CUM_LTE_FEE_PD /*cumulative late fees paid*/
		/*for R2-99 processing*/
		,BUS.DI_VLD_ADR_B_borr
		,LEG.DI_VLD_ADR_L_borr
	FROM 
		BLSTMNT BL
		LEFT JOIN BILL_ADR BUS
			ON BL.BF_SSN = BUS.BF_SSN
		LEFT JOIN LEGL_ADR LEG
			ON BL.BF_SSN = LEG.BF_SSN
		LEFT JOIN 
		(
			SELECT
ld_bil_crt,
				BF_SSN
				,LD_FAT_EFF 
				,SUM(LA_FAT_CUR_PRI) AS LA_FAT_CUR_PRI
				,SUM(LA_FAT_NSI) AS LA_FAT_NSI 
				,SUM(LA_FAT_LTE_FEE) AS LA_FAT_LTE_FEE
			FROM 
				BOR_TRX
			GROUP BY 
				BF_SSN
				,LD_FAT_EFF 
		) TRX
			ON BL.BF_SSN = TRX.BF_SSN
and bl.ld_bil_crt = trx.ld_bil_crt
		LEFT JOIN LATE_FEES LFEE
			ON BL.BF_SSN = LFEE.BF_SSN
			AND BL.LN_SEQ = LFEE.LN_SEQ
			AND BL.LD_BIL_CRT = LFEE.LD_BIL_CRT
			AND BL.LN_SEQ_BIL_WI_DTE = LFEE.LN_SEQ_BIL_WI_DTE
		LEFT JOIN 
		(
			SELECT 
				BF_SSN
				,LN_SEQ
				,SUM(LA_DSB) AS OPAFEL 
			FROM
				LN15
			GROUP BY 
				BF_SSN
				,LN_SEQ
		) LN15
			ON BL.BF_SSN = LN15.BF_SSN
			AND BL.LN_SEQ = LN15.LN_SEQ
		LEFT JOIN 
		(
			SELECT 
				BF_SSN
				,LN_SEQ
				,SUM(LA_FAT_CUR_PRI) AS TAPTP
				,SUM(LA_FAT_NSI) AS TAPTI
				,SUM(LA_FAT_PRI_NSI) AS TAGAP
				,SUM(LA_FAT_LTE_FEE) AS CUM_LTE_FEE_PD /*cumulative late fees paid*/
			FROM 
				LN90
			GROUP BY 
				BF_SSN
				,LN_SEQ
		) LN90
			ON BL.BF_SSN = LN90.BF_SSN
			AND BL.LN_SEQ = LN90.LN_SEQ
	;
QUIT;
/*OVERWRITE OLD BLSTMNT DATA SET*/
DATA BLSTMNT;
	SET BLSTMNT_TEMP;
RUN;
/*CLEAN UP DATA SETS*/
PROC DATASETS NOPRINT;
	DELETE BLSTMNT_TEMP BILL_ADR LEGL_ADR BOR_TRX LATE_FEES LN15 LN90;	
QUIT;
*----------------------------------------------;
*uncomment for TESTING, comment for PROMOTION;
	%SYSRPUT PRIVATE_LIST = &PRIVATE_LIST;
	%SYSRPUT UHEAA_LIST = &UHEAA_LIST;
	ENDRSUBMIT;
	DATA BLSTMNT;
		SET WORKLOCL.BLSTMNT;
	RUN;
	DATA COBRW_A;
		SET WORKLOCL.COBRW_A;
	RUN;
	DATA SV30;
		SET WORKLOCL.SV30;
	RUN;
	DATA DELQ;
		SET WORKLOCL.DELQ;
	RUN;
*----------------------------------------------;
PROC SORT DATA=BLSTMNT;
	BY BF_SSN LN_SEQ;
RUN;
/********************************************************************************
* IF BORROWER HAS MORE THAN 28 LOANS REMOVE FROM PROCESSING AND WRITE TO FILE 6
*********************************************************************************/
DATA GRAL (KEEP=BF_SSN LN_ID);
	SET BLSTMNT;
	LN_ID = TRIM(LEFT(BF_SSN))||' '||TRIM(LEFT(LN_SEQ));
RUN;
PROC SORT DATA=GRAL NODUPKEY;
	BY BF_SSN LN_ID;
RUN;
PROC SQL;
	CREATE TABLE GRALA AS 
		SELECT 
			COUNT(*) AS COUNT
			,BF_SSN
		FROM 
			GRAL
		GROUP BY 
			BF_SSN
		HAVING 
			COUNT(*) > &MAX_LN;
QUIT;
/*********************************************************************************************
* REMOVE BORROWERS SET INDICATOR FOR BORROWERS WITH MORE THAN 28 LOANS AND SET TILP/COMPLT INDICATOR
**********************************************************************************************/
PROC SQL;
	CREATE TABLE BLSTMNTA AS 
		SELECT DISTINCT 
			A.*
			,C.GTMAX_LOANS
		FROM 
			BLSTMNT A
		LEFT JOIN 
		(
			SELECT 
				BF_SSN
				,'X' AS GTMAX_LOANS
			FROM 
				GRALA
		) C
		ON A.BF_SSN = C.BF_SSN ;
QUIT;
PROC SORT DATA=BLSTMNTA OUT=BLSTMNT;
	BY BF_SSN LN_SEQ_BIL_WI_DTE LD_BIL_CRT;
RUN;
DATA BLSTMNT;
	SET BLSTMNT;
	IF IC_LON_PGM = 'COMPLT'
		THEN COMPLT_BILL = 'X';
	IF IC_LON_PGM = 'TILP'
		THEN TILP_BILL = 'X';
RUN;
/********************************************************************************
* CALCULATE DAYS DELINQUENT AND CREATE DAYS DELINQUENT LOAN DATA SETS
*********************************************************************************/
DATA DELQ;
	SET DELQ;
	IF LD_FOR_BEG > LD_DLQ_OCC 
	THEN DAYS_DELQ = (TODAY() - LD_DLQ_OCC) - (LD_FOR_END - LD_FOR_BEG);
	ELSE DAYS_DELQ = TODAY() - LD_DLQ_OCC;
RUN;
PROC SQL;
	CREATE TABLE _BLSTMNT AS
		SELECT 
			BL.*
			,DQ.DAYS_DELQ
		FROM 
			BLSTMNT BL
			LEFT JOIN DELQ DQ
				ON BL.BF_SSN = DQ.BF_SSN
				AND BL.LN_SEQ = DQ.LN_SEQ
		ORDER BY 
			BL.BF_SSN 
			,BL.LN_SEQ_BIL_WI_DTE 
			,BL.LD_BIL_CRT
			,BL.LN_SEQ;
QUIT;
PROC SORT DATA=_BLSTMNT OUT=BLSTMNT;
	BY BF_SSN LN_SEQ_BIL_WI_DTE LD_BIL_CRT COMPLT_BILL TILP_BILL;
RUN;
/********************************************************************************
* CREATE BILL LEVEL DATASET CONTAINING LOAN INFO
*********************************************************************************/
%MACRO LN_BIL(DS,NEWDS,NEWCOL);
	PROC TRANSPOSE DATA=&DS OUT=BLSTMNT&NEWDS 
		(DROP=_NAME_) 
		PREFIX=&NEWCOL;
		VAR &NEWCOL;
		BY BF_SSN LN_SEQ_BIL_WI_DTE LD_BIL_CRT COMPLT_BILL TILP_BILL;
	RUN;
%MEND LN_BIL;
%LN_BIL(BLSTMNT,1,LN_SEQ);
%LN_BIL(BLSTMNT,2,LD_LON_1_DSB);
%LN_BIL(BLSTMNT,4,LC_LON_STA_BIL);
%LN_BIL(BLSTMNT,5,LR_INT_BIL);
%LN_BIL(BLSTMNT,6,LA_CUR_PRN_BIL);
%LN_BIL(BLSTMNT,7,LA_BIL_PAS_DU);
%LN_BIL(BLSTMNT,8,LN_LTE_FEE);
%LN_BIL(BLSTMNT,9,DAYS_DELQ);
%LN_BIL(BLSTMNT,10,OPAFEL);
%LN_BIL(BLSTMNT,11,TAPTP);
%LN_BIL(BLSTMNT,12,TAPTI);
%LN_BIL(BLSTMNT,13,TAGAP); 
%LN_BIL(BLSTMNT,16,BOR_LTE_FEE); 
%LN_BIL(BLSTMNT,17,CUM_LTE_FEE_PD);/*cumulative late fees paid*/
%LN_BIL(BLSTMNT,18,LC_LON_SND_CHC);/*for R99 processing at loan level*/
%MACRO LN_BIL(DS,NEWDS,NEWCOL,CRIT);
	PROC TRANSPOSE DATA=&DS OUT=BLSTMNT&NEWDS 
		(DROP=_NAME_) 
		PREFIX=&NEWCOL;
		VAR &NEWCOL;
		BY BF_SSN LN_SEQ_BIL_WI_DTE LD_BIL_CRT COMPLT_BILL TILP_BILL;
		&CRIT;
	RUN;
%MEND LN_BIL;
%LN_BIL(BLSTMNT,3,IC_LON_PGM,WHERE COMPLT_BILL ^= 'X' AND TILP_BILL ^= 'X');
%LN_BIL(BLSTMNT,14,IC_LON_PGM,WHERE COMPLT_BILL = 'X' AND TILP_BILL ^= 'X');
%LN_BIL(BLSTMNT,15,IC_LON_PGM,WHERE COMPLT_BILL ^= 'X' AND TILP_BILL = 'X');
DATA BLSTMNT_LN (DROP=_LABEL_);
	MERGE BLSTMNT1 BLSTMNT2 BLSTMNT3 BLSTMNT4 BLSTMNT5 BLSTMNT6 
		  BLSTMNT7 BLSTMNT8 BLSTMNT9 BLSTMNT10 BLSTMNT11 BLSTMNT12 
		  BLSTMNT13 BLSTMNT14 BLSTMNT15 BLSTMNT16 
		  BLSTMNT17  /*cumulative late fees paid*/
		  BLSTMNT18; /*for R99 processing at loan level*/
	BY BF_SSN LN_SEQ_BIL_WI_DTE LD_BIL_CRT COMPLT_BILL TILP_BILL;
RUN;
PROC DATASETS NOPRINT;
	DELETE BLSTMNT1-BLSTMNT18 _BLSTMNT;
QUIT;
/********************************************************************************
* CREATE BORROWER LEVEL DATA SET
*********************************************************************************/
DATA BLSTMNT_DEM 
	(KEEP= BF_SSN DF_ZIP_CDE_L DM_CT_L DX_STR_ADR_3_L DX_STR_ADR_2_L DX_STR_ADR_1_L 
		DC_ADR_L STATE_L DM_FGN_CNY_L DF_ZIP_CDE_B DM_CT_B DX_STR_ADR_3_B DX_STR_ADR_2_B DX_STR_ADR_1_B
		DC_ADR_B STATE_B DM_FGN_CNY_B DF_SPE_ACC_ID DM_PRS_1 DM_PRS_MID DM_PRS_LST DI_VLD_ADR_EML
		/*for R2-99 processing*/
		DI_VLD_ADR_B_borr DI_VLD_ADR_L_borr DI_CNC_EBL_OPI
	);
	SET BLSTMNT;
RUN;
/********************************************************************************************
* IF BORROWER OR COBORROWER HAS VALID BILLING ADDRESS USE IT OTHERWISE USE THE LEGAL ADDRESS
*********************************************************************************************/
%MACRO AVERDICT(DS,type,KLIST);
	DATA &DS (KEEP=&KLIST);
	SET &DS;
		IF DC_ADR_B = 'B' 
		THEN DC_ADR = DC_ADR_B; 
		ELSE DC_ADR = DC_ADR_L;
		IF DC_ADR_B = 'B' 
		THEN DF_ZIP_CDE = DF_ZIP_CDE_B;
		ELSE DF_ZIP_CDE = DF_ZIP_CDE_L;
		IF DC_ADR_B = 'B' 
		THEN DM_CT = DM_CT_B;
		ELSE DM_CT = DM_CT_L;
		IF DC_ADR_B = 'B' 
		THEN DX_STR_ADR_3 = DX_STR_ADR_3_B;
		ELSE DX_STR_ADR_3 = DX_STR_ADR_3_L;
		IF DC_ADR_B = 'B' 
		THEN DX_STR_ADR_2 = DX_STR_ADR_2_B;
		ELSE DX_STR_ADR_2 = DX_STR_ADR_2_L;
		IF DC_ADR_B = 'B' 
		THEN DX_STR_ADR_1 = DX_STR_ADR_1_B;
		ELSE DX_STR_ADR_1 = DX_STR_ADR_1_L;
		IF DC_ADR_B = 'B' 
		THEN DC_ADR = DC_ADR_B;
		ELSE DC_ADR = DC_ADR_L;
		IF DC_ADR_B = 'B' 
		THEN STATE = STATE_B;
		ELSE STATE = STATE_L;
		IF DC_ADR_B = 'B' 
		THEN DM_FGN_CNY = DM_FGN_CNY_B;
		ELSE DM_FGN_CNY = DM_FGN_CNY_L;
		/*for R2-99 processing*/
		IF DC_ADR_B = 'B'
		THEN DI_VLD_ADR&type = DI_VLD_ADR_B&type;
		ELSE DI_VLD_ADR&type = DI_VLD_ADR_L&type;
	RUN;
%MEND AVERDICT;
%AVERDICT(BLSTMNT_DEM,_borr,BF_SSN DF_SPE_ACC_ID DM_PRS_1 DM_PRS_MID DM_PRS_LST DF_ZIP_CDE 
				DM_CT DX_STR_ADR_3 DX_STR_ADR_2 DX_STR_ADR_1 DC_ADR STATE DM_FGN_CNY DI_VLD_ADR_EML
				/*for R2-99 processing*/
				DI_VLD_ADR_borr	DI_CNC_EBL_OPI
		 );
%AVERDICT(COBRW_A,_endo,BF_SSN LF_EDS LD_BIL_CRT LN_SEQ_BIL_WI_DTE DF_SPE_ACC_ID DM_PRS_1 DM_PRS_MID DM_PRS_LST 
				DF_ZIP_CDE DM_CT DX_STR_ADR_3 DX_STR_ADR_2 DX_STR_ADR_1 DC_ADR STATE DM_FGN_CNY NAME DI_VLD_ADR_EML
				/*for R2-99 processing*/
				DI_VLD_ADR_endo	DI_CNC_EBL_OPI
		 );
PROC SORT DATA=BLSTMNT_DEM NODUPKEY;
	BY BF_SSN;
RUN;
PROC SORT DATA=COBRW_A NODUPKEY;
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE;
RUN;
/**************************************************************
* CALCULATE KEYLINE FOR BORROWER AND CO-BORROWER
***************************************************************/
%MACRO KEYLINE(DS,SSN);
	DATA &DS (DROP = KEYSSN MODAY KEYLINE CHKDIG DIG I 
		CHKDIG CHK1 CHK2 CHK3 CHKDIGIT CHECK);
	SET &DS;
	KEYSSN = TRANSLATE(&SSN,'MYLAUGHTER','0987654321');
	MODAY = PUT(DATE(),MMDDYYN4.);
	KEYLINE = "P"||KEYSSN||MODAY||DC_ADR;
	CHKDIG = 0;
	LENGTH DIG $2.;
	DO I = 1 TO LENGTH(KEYLINE);
		IF I/2 NE ROUND(I/2,1) 
		THEN DIG = PUT(INPUT(SUBSTR(KEYLINE,I,1),BITS4.4) * 2, 2.);
		ELSE DIG = PUT(INPUT(SUBSTR(KEYLINE,I,1),BITS4.4), 2.);
		IF SUBSTR(DIG,1,1) = " " 
		THEN CHKDIG = CHKDIG + INPUT(SUBSTR(DIG,2,1),1.);
		ELSE 
			DO;
				CHK1 = INPUT(SUBSTR(DIG,1,1),1.);
				CHK2 = INPUT(SUBSTR(DIG,2,1),1.);
				IF CHK1 + CHK2 >= 10
					THEN 
						DO;
							CHK3 = PUT(CHK1 + CHK2,2.);
							CHK1 = INPUT(SUBSTR(CHK3,1,1),1.);
							CHK2 = INPUT(SUBSTR(CHK3,2,1),1.);
						END;
				CHKDIG = CHKDIG + CHK1 + CHK2;
			END;
	END;
	CHKDIGIT = 10 - INPUT(SUBSTR((RIGHT(PUT(CHKDIG,3.))),3,1),3.);
	IF CHKDIGIT = 10 
	THEN CHKDIGIT = 0;
	CHECK = PUT(CHKDIGIT,1.);
	ACSKEY = "#"||KEYLINE||CHECK||"#";
	RUN;
%MEND KEYLINE;
%KEYLINE(BLSTMNT_DEM,BF_SSN);
%KEYLINE(COBRW_A,LF_EDS);
/**************************************************************
* CREATE DATA SET TO DETERMINE MESSAGE TEXT
***************************************************************/
DATA MESTXT
	/*dropping these R2-99 processing indicators just in case it messes with message text*/
	(DROP=DI_CNC_EBL_OPI LC_LON_SND_CHC DI_VLD_ADR_B_borr DI_VLD_ADR_L_borr);
	SET BLSTMNT;
RUN;
/**************************************************************
* REMOVE LOAN LEVEL AND BORROWER LEVEL INFO FROM MAIN DATA SET
***************************************************************/
DATA BLSTMNT 
	(DROP = DM_PRS_1 DM_PRS_MID DM_PRS_LST DF_ZIP_CDE_L 
		DM_CT_L DX_STR_ADR_3_L DX_STR_ADR_2_L DX_STR_ADR_1_L DC_ADR_L STATE_L DM_FGN_CNY_L 
		DF_ZIP_CDE_B DM_CT_B DX_STR_ADR_3_B DX_STR_ADR_2_B DX_STR_ADR_1_B DC_ADR_B STATE_B 
		DM_FGN_CNY_B DM_PRS_1 DM_PRS_MID DM_PRS_LST LN_SEQ LD_LON_1_DSB IC_LON_PGM 
		LC_LON_STA_BIL LR_INT_BIL LA_CUR_PRN_BIL LN_LTE_FEE OPAFEL TAPTP TAPTI TAGAP
		CUM_LTE_FEE_PD/*cumulative late fees paid*/
		/*for R2-99 processing*/
		DI_CNC_EBL_OPI LC_LON_SND_CHC DI_VLD_ADR_B_borr DI_VLD_ADR_L_borr
	);
	SET BLSTMNT;
RUN;
PROC SQL;
	CREATE TABLE BLL AS 
		SELECT DISTINCT 
			BF_SSN 
			,LN_SEQ_BIL_WI_DTE 
			,LD_BIL_CRT
			,COMPLT_BILL
			,TILP_BILL
			,SUM(COALESCE(LA_BIL_PAS_DU,0)) AS LA_BIL_PAS_DU 
			,SUM(COALESCE(LA_BIL_DU_PRT,0)) AS LA_BIL_DU_PRT
			,SUM(COALESCE(LA_BIL_CUR_DU,0)) AS LA_BIL_CUR_DU
			,SUM(COALESCE(BOR_LTE_FEE,0)) AS BOR_LTE_FEE
		FROM 
			BLSTMNT
		GROUP BY 
			BF_SSN 
			,LN_SEQ_BIL_WI_DTE 
			,LD_BIL_CRT
			,COMPLT_BILL
			,TILP_BILL;
QUIT;
/**************************************************************
* GET INHOUSE INDICATOR FOR COST CODE CALCULATION
***************************************************************/
DATA CSTCD (KEEP=BF_SSN LN_SEQ_BIL_WI_DTE LD_BIL_CRT COMPLT_BILL TILP_BILL INHOUSE);
	SET BLSTMNT;
	WHERE LF_LON_CUR_OWN IN (&UHEAA_LIST);
	INHOUSE = 'Y';
RUN;
PROC SORT DATA=CSTCD NODUPKEY;
	BY BF_SSN LN_SEQ_BIL_WI_DTE LD_BIL_CRT COMPLT_BILL TILP_BILL;
RUN;
DATA BLSTMNT (DROP=LA_BIL_PAS_DU LA_BIL_CUR_DU LA_BIL_DU_PRT LF_LON_CUR_OWN);
	SET BLSTMNT;
RUN;
PROC SORT DATA=BLSTMNT NODUPKEY;
	BY BF_SSN LN_SEQ_BIL_WI_DTE LD_BIL_CRT COMPLT_BILL TILP_BILL;
RUN;
DATA BLSTMNT;
	MERGE BLSTMNT BLL CSTCD;
	BY BF_SSN LN_SEQ_BIL_WI_DTE LD_BIL_CRT COMPLT_BILL TILP_BILL;
RUN;
DATA BLSTMNT;
	SET BLSTMNT;
	TAP = ABS(SUM(LA_FAT_CUR_PRI,LA_FAT_NSI,LA_FAT_LTE_FEE));
	IF TAP = . THEN TAP = 0;
RUN;
/**************************************************************
* GET MESSAGE TEXT
***************************************************************/
DATA MESTXT (KEEP=BF_SSN LN_SEQ_BIL_WI_DTE LD_BIL_CRT COMPLT_BILL TILP_BILL BLL_DT JVAR LF_LON_CUR_OWN IC_LON_PGM LC_BIL_TYP);
	SET MESTXT;
	JVAR = 1;
RUN;
DATA SV30 (DROP=BF_SSN);
	SET SV30;
	JVAR_SV30 = 1;
RUN;
PROC SQL;
	CREATE TABLE TXT AS 
		SELECT 
			A.LD_BIL_CRT
			,A.BF_SSN
			,A.LN_SEQ_BIL_WI_DTE 
			,A.COMPLT_BILL
			,TILP_BILL
			,A.LF_LON_CUR_OWN
			,A.IC_LON_PGM 
			,A.LC_BIL_TYP
			,A.BLL_DT
			,B.*
		FROM 
			MESTXT A
			INNER JOIN SV30 B
				ON A.JVAR = B.JVAR_SV30;
QUIT;
DATA TXT (KEEP=BF_SSN LN_SEQ_BIL_WI_DTE LD_BIL_CRT COMPLT_BILL TILP_BILL PX_MSG_BIL_ARA BLL_DT);
	SET TXT;
	IF BLL_DT GE PD_EFF_BEG AND BLL_DT LE PD_EFF_END 
		AND LF_LON_CUR_OWN EQ PF_MSG_BIL_OWN
		AND IC_LON_PGM EQ PF_MSG_BIL_LON_PGM
		AND (
				PC_MSG_BIL_TYP = 'A'
				OR (LC_BIL_TYP IN ('I','C') AND PC_MSG_BIL_TYP = 'B')
				OR (LC_BIL_TYP = 'P' AND PC_MSG_BIL_TYP = 'P')
				OR (LC_BIL_TYP = 'C' AND PC_MSG_BIL_TYP = 'I')
				OR (LC_BIL_TYP = 'I' AND PC_MSG_BIL_TYP = 'N')
			)
		THEN OUTPUT TXT;
RUN;
PROC SORT DATA=TXT NODUPRECS;
	BY BF_SSN COMPLT_BILL TILP_BILL;
RUN;
/*END MESSAGE TXT CALCULATION*/

/*CALCULATE THE AMOUNT DUE*/
PROC SQL;
	CREATE TABLE AMT AS
		SELECT DISTINCT 
			BF_SSN
			,LD_BIL_CRT
			,LN_SEQ_BIL_WI_DTE
			,COMPLT_BILL
			,TILP_BILL
			,CASE
				WHEN LC_IND_BIL_SNT = '4'
				THEN SUM(COALESCE(LA_BIL_CUR_DU,0))
				WHEN LC_IND_BIL_SNT ^= '4'
				THEN SUM(COALESCE(LA_BIL_PAS_DU,0)) + 
					 SUM(COALESCE(LA_BIL_DU_PRT,0)) + 
					 SUM(COALESCE(BOR_LTE_FEE,0))
				ELSE .
			 END AS AMTDU
		FROM 
			BLSTMNT
		GROUP BY 
			BF_SSN
			,LD_BIL_CRT
			,LN_SEQ_BIL_WI_DTE
			,COMPLT_BILL
			,TILP_BILL;
QUIT;
PROC SORT DATA=AMT;
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE COMPLT_BILL TILP_BILL;
RUN; 
PROC SORT DATA=BLSTMNT;
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE COMPLT_BILL TILP_BILL;
RUN; 
DATA BLSTMNT;
	MERGE AMT BLSTMNT;
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE COMPLT_BILL TILP_BILL;
RUN;
PROC SORT DATA=BLSTMNT NODUPRECS;
	BY BF_SSN LN_SEQ_BIL_WI_DTE LD_BIL_CRT COMPLT_BILL TILP_BILL;
RUN;
/*********************************************************
* BEGIN SCAN LINE CALCULATION
**********************************************************/
DATA SCAN;
	SET BLSTMNT;
RUN;
DATA SCAN (KEEP=BF_SSN YEAR JDATE STTC PAI SI FD AMTDU BLSQ LN_SEQ_BIL_WI_DTE LD_BIL_CRT COMPLT_BILL TILP_BILL DF_SPE_ACC_ID);
	SET SCAN;
	FORMAT YEAR $ 4. JDATE $ 3. /*CHKDGT $ 1.*/;
	YEAR = YEAR(LD_BIL_CRT);/*YEAR*/
	JDATE = LEFT(INTCK('DAY',INTNX('YEAR',LD_BIL_CRT,0,'BEGINNING'),LD_BIL_CRT)+1);/*JULIAN DATE*/
	STTC = '10';/*SUB TYPE TRX CODE*/
	PAI = '0';/*PAID AHEAD INDICATOR*/
	SI = '0';/*SYSTEM INDICATOR*/
	FD = '00000000000000';/*FILLER DIGITS*/
RUN;
/*FORMAT JULIAN DATE*/
DATA SCAN;
	SET SCAN;
	X = LENGTH(JDATE);
	IF X = 3 THEN JDATE = JDATE;
	ELSE IF X = 2 THEN JDATE = '0'||JDATE;
	ELSE IF X = 1 THEN JDATE = '00'||JDATE;
RUN;
PROC SORT DATA=SCAN NODUPKEY;
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE COMPLT_BILL TILP_BILL;
RUN;
DATA SCAN;
	SET SCAN;
	LENGTH ZN $ 5.;
	IF 10000 <= AMTDU < 100000 THEN ZN='0';
	ELSE IF 1000 <= AMTDU < 10000 THEN ZN='00';
	ELSE IF 100 <= AMTDU < 1000 THEN ZN='000';
	ELSE IF 10 <= AMTDU < 100 THEN ZN='0000';
	ELSE IF AMTDU <= 10 THEN ZN='00000';
RUN;
DATA SCAN (KEEP=BF_SSN LD_BIL_CRT YEAR JDATE STTC PAI SI FD BLSQ LN_SEQ_BIL_WI_DTE COMPLT_BILL TILP_BILL PW DF_SPE_ACC_ID);
	SET SCAN;
	LENGTH Y $ 2. PW $ 8.;
	X=INDEX(LEFT(AMTDU),'.');
	IF X NE 0 THEN DO;
		Y=SUBSTR(LEFT(AMTDU),X+1,2);
		L=LENGTH(Y);
		END;
	IF L = . THEN Y = '00';
		ELSE IF L = 1 THEN Y=TRIM(LEFT(Y))||'0';
		ELSE IF L = 2 THEN Y = Y;
	IF L = . THEN A = AMTDU;
		ELSE IF L NE . THEN A=SUBSTR(LEFT(AMTDU),1,X-1);
	V=LEFT(TRIM(A)||TRIM(Y));
	PW=TRIM(ZN)||TRIM(V);
RUN;
DATA SCAN (KEEP=BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE COMPLT_BILL TILP_BILL SCAN);
	LENGTH SCAN $ 45;
	SET SCAN;
	SCAN = TRIM(YEAR)||TRIM(JDATE)||TRIM(BLSQ)||TRIM(DF_SPE_ACC_ID)||TRIM(STTC)||
		   TRIM(PW)||TRIM(PAI)||TRIM(SI)||TRIM(FD);
RUN;

/*BEGIN CHECK DIGIT 2 CALCULATIONS*/
DATA SCAN2;
	SET SCAN;
RUN;
DATA SCAN2A;
	SET SCAN2;
	/*DIGIT VALUES*/
	DV1=INT(SUBSTR(SCAN,1,1));DV2=INT(SUBSTR(SCAN,2,1));DV3=INT(SUBSTR(SCAN,3,1));
	DV4=INT(SUBSTR(SCAN,4,1));DV5=INT(SUBSTR(SCAN,5,1));DV6=INT(SUBSTR(SCAN,6,1));
	DV7=INT(SUBSTR(SCAN,7,1));DV8=INT(SUBSTR(SCAN,8,1));DV9=INT(SUBSTR(SCAN,9,1));
	DV10=INT(SUBSTR(SCAN,10,1));DV11=INT(SUBSTR(SCAN,11,1));DV12=INT(SUBSTR(SCAN,12,1));
	DV13=INT(SUBSTR(SCAN,13,1));DV14=INT(SUBSTR(SCAN,14,1));DV15=INT(SUBSTR(SCAN,15,1));
	DV16=INT(SUBSTR(SCAN,16,1));DV17=INT(SUBSTR(SCAN,17,1));DV18=INT(SUBSTR(SCAN,18,1));
	DV19=INT(SUBSTR(SCAN,19,1));DV20=INT(SUBSTR(SCAN,20,1));DV21=INT(SUBSTR(SCAN,21,1));
	DV22=INT(SUBSTR(SCAN,22,1));DV23=INT(SUBSTR(SCAN,23,1));DV24=INT(SUBSTR(SCAN,24,1));
	DV25=INT(SUBSTR(SCAN,25,1));DV26=INT(SUBSTR(SCAN,26,1));DV27=INT(SUBSTR(SCAN,27,1));
	DV28=INT(SUBSTR(SCAN,28,1));DV29=INT(SUBSTR(SCAN,29,1));DV30=INT(SUBSTR(SCAN,30,1));
	DV31=INT(SUBSTR(SCAN,31,1));DV32=INT(SUBSTR(SCAN,32,1));DV33=INT(SUBSTR(SCAN,33,1));
	DV34=INT(SUBSTR(SCAN,34,1));DV35=INT(SUBSTR(SCAN,35,1));DV36=INT(SUBSTR(SCAN,36,1));
	DV37=INT(SUBSTR(SCAN,37,1));DV38=INT(SUBSTR(SCAN,38,1));DV39=INT(SUBSTR(SCAN,39,1));
	DV40=INT(SUBSTR(SCAN,40,1));DV41=INT(SUBSTR(SCAN,41,1));DV42=INT(SUBSTR(SCAN,42,1));
	DV43=INT(SUBSTR(SCAN,43,1));DV44=INT(SUBSTR(SCAN,44,1));DV45=INT(SUBSTR(SCAN,45,1));
RUN;
DATA SCAN2B;
	SET SCAN2;
	/*DIGIT MULTIPLIERS*/
	DIG1MTPLR=1;DIG2MTPLR=2;DIG3MTPLR=1;DIG4MTPLR=2;DIG5MTPLR=1;DIG6MTPLR=2;
	DIG7MTPLR=1;DIG8MTPLR=2;DIG9MTPLR=1;DIG10MTPLR=2;DIG11MTPLR=1;DIG12MTPLR=2;
	DIG13MTPLR=1;DIG14MTPLR=2;DIG15MTPLR=1;DIG16MTPLR=2;DIG17MTPLR=1;DIG18MTPLR=2;
	DIG19MTPLR=1;DIG20MTPLR=2;DIG21MTPLR=1;DIG22MTPLR=2;DIG23MTPLR=1;DIG24MTPLR=2;
	DIG25MTPLR=1;DIG26MTPLR=2;DIG27MTPLR=1;DIG28MTPLR=2;DIG29MTPLR=1;DIG30MTPLR=2;
	DIG31MTPLR=1;DIG32MTPLR=2;DIG33MTPLR=1;DIG34MTPLR=2;DIG35MTPLR=1;DIG36MTPLR=2;
	DIG37MTPLR=1;DIG38MTPLR=2;DIG39MTPLR=1;DIG40MTPLR=2;DIG41MTPLR=1;DIG42MTPLR=2;
	DIG43MTPLR=1;DIG44MTPLR=2;DIG45MTPLR=1;
RUN;
PROC SORT DATA=SCAN2A;
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE;
RUN;
PROC SORT DATA=SCAN2B;
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE;
RUN;
PROC TRANSPOSE DATA=SCAN2A OUT=SCAN2A (DROP=_NAME_) PREFIX=DV;
	VAR DV1 DV2 DV3 DV4 DV5 DV6 DV7 DV8 DV9 DV10 DV11 DV12 DV13 DV14 DV15 
	DV16 DV17 DV18 DV19 DV20 DV21 DV22 DV23 DV24 DV25 DV26 DV27 DV28 DV29
	DV30 DV31 DV32 DV33 DV34 DV35 DV36 DV37 DV38 DV39 DV40 DV41 DV42 DV43 
	DV44 DV45;
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE COMPLT_BILL TILP_BILL;
RUN;
PROC TRANSPOSE DATA=SCAN2B OUT=SCAN2B (DROP=_NAME_) PREFIX=DM;
	VAR DIG1MTPLR DIG2MTPLR DIG3MTPLR DIG4MTPLR DIG5MTPLR DIG6MTPLR DIG7MTPLR
	DIG8MTPLR DIG9MTPLR DIG10MTPLR DIG11MTPLR DIG12MTPLR DIG13MTPLR DIG14MTPLR
	DIG15MTPLR DIG16MTPLR DIG17MTPLR DIG18MTPLR DIG19MTPLR DIG20MTPLR DIG21MTPLR
	DIG22MTPLR DIG23MTPLR DIG24MTPLR DIG25MTPLR DIG26MTPLR DIG27MTPLR DIG28MTPLR
	DIG29MTPLR DIG30MTPLR DIG31MTPLR DIG32MTPLR DIG33MTPLR DIG34MTPLR DIG35MTPLR
	DIG36MTPLR DIG37MTPLR DIG38MTPLR DIG39MTPLR DIG40MTPLR DIG41MTPLR DIG42MTPLR
	DIG43MTPLR DIG44MTPLR DIG45MTPLR;
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE COMPLT_BILL TILP_BILL;
RUN;
DATA SCANTB;
	MERGE SCAN2A SCAN2B;
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE COMPLT_BILL TILP_BILL;
RUN;
DATA SCANTB;
	SET SCANTB;
	CKDVR = TRIM(LEFT(BF_SSN))||TRIM(LEFT(PUT(LD_BIL_CRT,DATE9.)))||TRIM(LEFT(LN_SEQ_BIL_WI_DTE))||TRIM(LEFT(COMPLT_BILL));
RUN;
PROC SORT DATA=SCANTB;
	BY CKDVR;
RUN;
DATA SCANTB ;
	SET SCANTB;
	BY CKDVR;
	ACC1 = 0;
	ACC2 = 0;
	IF FIRST.CKDVR THEN DPOS = 0;
	DPOS+1;
RUN;
PROC SORT DATA=SCANTB;
	BY CKDVR DESCENDING DPOS;
RUN;
/*USE ACC2A FOR ACCUMULATOR 1*/
DATA SCANTB (KEEP=BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE COMPLT_BILL TILP_BILL CD1);
	SET SCANTB;
	BY CKDVR;
	ACC2=DV1*DM1;
	IF ACC2 <= 9 
	THEN ACC2A=ACC1 + ACC2;
	ELSE IF ACC2 > 9 
	THEN 
		DO;
			X=SUBSTR(LEFT(ACC2),1,1);
			Y=SUBSTR(LEFT(ACC2),2,1);
			ACC2A=X+Y;
		END;
	IF FIRST.CKDVR 
	THEN CD1=0;
	CD1+ACC2A;
	IF LAST.CKDVR 
	THEN OUTPUT;
RUN;
DATA SCANTB (KEEP=BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE COMPLT_BILL TILP_BILL CKDGT);
	SET SCANTB;
	IF CD1 >= 100 
	THEN LDIGT = SUBSTR(LEFT(CD1),3,1);
	ELSE LDIGT = SUBSTR(LEFT(CD1),2,1);
	IF 10 - LDIGT < 10 
	THEN CKDGT = 10 - LDIGT;
	ELSE IF 10 - LDIGT = 10 
	THEN CKDGT = 0;
RUN;
/*END CHECK DIGIT 2 CALCULATION*/

PROC SORT DATA=SCAN;
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE COMPLT_BILL TILP_BILL;
RUN;
PROC SORT DATA=SCANTB;
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE COMPLT_BILL TILP_BILL;
RUN;
/*CREATE FINAL SCAN LINE*/
DATA SCAN;
	MERGE SCAN SCANTB;
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE COMPLT_BILL TILP_BILL;
RUN;
DATA SCAN (KEEP=BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE COMPLT_BILL TILP_BILL SCANLN);
	SET SCAN;
	LENGTH SCANLN $ 46.;
	SCANLN = LEFT(TRIM(SCAN))||LEFT(TRIM(CKDGT));
RUN;
/*********************************************************
* END OF SCANLINE CALCULATIONS
**********************************************************/

/*SORT DATA SETS FOR MERGE*/
PROC SORT DATA=SCAN;
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE COMPLT_BILL TILP_BILL;
RUN;
PROC SORT DATA=BLSTMNT;
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE COMPLT_BILL TILP_BILL;
RUN;
PROC SORT DATA=TXT;
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE COMPLT_BILL TILP_BILL;
RUN;
PROC SORT DATA=BLSTMNT_LN;
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE COMPLT_BILL TILP_BILL;
RUN;
/*MERGE SCANLINE WITH OTHER INFO*/
DATA BLSTMNT;
	MERGE SCAN BLSTMNT;
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE COMPLT_BILL TILP_BILL;
RUN;
/*PUT MESSAGE TEXT WITH OTHER DATA*/
DATA BLSTMNT;
	MERGE TXT (IN=A) BLSTMNT (IN=B);
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE COMPLT_BILL TILP_BILL;
RUN;
/*MERGE LOAN LEVEL INFO*/
DATA BLSTMNT;
	MERGE BLSTMNT BLSTMNT_LN;
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE COMPLT_BILL TILP_BILL;
RUN;
/*MERGE DEMOGRAPHIC INFO*/
DATA BLSTMNT;
	MERGE BLSTMNT BLSTMNT_DEM;
	BY BF_SSN;
RUN;
PROC SORT DATA=BLSTMNT;
	BY DM_PRS_LST BF_SSN LN_SEQ_BIL_WI_DTE LD_BIL_CRT COMPLT_BILL TILP_BILL;
RUN;
DATA BLSTMNT;
	SET BLSTMNT;
	IF LC_IND_BIL_SNT = 'G' 
	THEN PX_MSG_BIL_ARA = ' ';
	ELSE PX_MSG_BIL_ARA = PX_MSG_BIL_ARA;
RUN;
/*****************************************************************************
* MERGE CO-BORROWER INFO FOR BILLS
******************************************************************************
* FIRST CHECK TO SEE IF THE CO-BORROWER AND THE BORROWER HAVE THE SAME ADDRESS. 
* IF THEY DO THEN ADD THE CO-BORROWER NAME AND SSN BACK TO THE BLSTMNT DATA SET
******************************************************************************/
PROC SQL;
	CREATE TABLE COBRW AS 
	SELECT DISTINCT 
		 A.BF_SSN
		,A.LD_BIL_CRT
		,A.LN_SEQ_BIL_WI_DTE
		,B.NAME AS EDR_NM 
		,B.DM_PRS_1 AS EDR_DM_PRS_1
		,B.DM_PRS_MID AS EDR_DM_PRS_MID
		,B.DM_PRS_LST AS EDR_DM_PRS_LST
		,B.DF_SPE_ACC_ID AS EDS_ACCT
		,B.LF_EDS
		,'N' AS SMADI
		,B.DI_VLD_ADR_endo
	FROM 
		BLSTMNT A
		INNER JOIN COBRW_A B
			ON A.BF_SSN = B.BF_SSN
			AND A.LD_BIL_CRT = B.LD_BIL_CRT
			AND A.LN_SEQ_BIL_WI_DTE = B.LN_SEQ_BIL_WI_DTE;
QUIT;
PROC SORT DATA=BLSTMNT;
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE COMPLT_BILL TILP_BILL;
RUN;
PROC SORT DATA=COBRW;
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE;
RUN;
DATA BLSTMNT;
	MERGE BLSTMNT COBRW;
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE;
RUN;
* OTHERWISE ADD A NEW ROW FOR THE CO-BORROWER ;
DATA BOR (DROP= DF_SPE_ACC_ID DM_PRS_1 DM_PRS_MID DM_PRS_LST DF_ZIP_CDE DM_CT DX_STR_ADR_3
		DX_STR_ADR_2 DX_STR_ADR_1 DC_ADR STATE DM_FGN_CNY ACSKEY NAME LF_EDS);
	SET BLSTMNT;
	WHERE SMADI = 'N';
	LABEL EDS_ACCT = 'EDS_ACCT'
		  EDR_DM_PRS_1 = 'EDR_DM_PRS_1'
		  EDR_DM_PRS_MID = 'EDR_DM_PRS_MID'
		  EDR_DM_PRS_LST = 'EDR_DM_PRS_LST';
RUN;
PROC SORT DATA=BOR;
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE COMPLT_BILL TILP_BILL;
RUN;
PROC SORT DATA=COBRW_A;
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE;
RUN;
DATA COBRW_A;
	MERGE COBRW_A (IN=A) BOR (IN=B);
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE;
	IF SMADI = 'N' ;
RUN;
DATA BLSTMNT;
	SET BLSTMNT COBRW_A;
RUN;
/*****************************************************************************
*MERGE DAYS DELINQUENT INFORMATION AT THE BORROWER LEVEL
******************************************************************************/
PROC SORT DATA=BLSTMNT;
	BY BF_SSN;
RUN;
PROC SORT DATA=DELQ (KEEP=BF_SSN DAYS_DELQ) NODUPKEY;
	BY BF_SSN DAYS_DELQ;
RUN;
DATA BLSTMNT;
	MERGE BLSTMNT DELQ;
	BY BF_SSN;
RUN;
/****************************************************************************
****************************** IMPORTANT NOTE! ******************************
* BECAUSE A BORROWER COULD HAVE MORE THAN ONE DAYS DELINQUENT VALUE, A BILL 
* COULD APPEAR MORE THAN ONCE PER BORROWER/CO-BORROWER
******************************************************************************/
PROC SORT DATA=BLSTMNT NODUPKEY;
	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE DF_SPE_ACC_ID COMPLT_BILL TILP_BILL LF_EDS DAYS_DELQ;
RUN;
/*=======CALCULATE COST CENTER AND MASTER SORT VARIABLES=======*/
DATA BLSTMNT;
	SET BLSTMNT;
	IF INHOUSE = 'Y' 
	THEN CSTCNTR = 'MA2324';
	ELSE CSTCNTR = 'MA2327';
RUN;
DATA BLSTMNT;
	SET BLSTMNT;
	IF STATE IN ('FC',' ') 
	THEN MSTRSRT = 1;
	ELSE MSTRSRT = 2;
RUN;
/*=============================================================*/
/*CREATE ERROR REPORT DATA SETS*/
PROC SQL;
	CREATE TABLE BLLERR AS 
		SELECT 
			*
		FROM 
			BLSTMNT
		WHERE 
			LC_BIL_TYP NOT IN ('P','C','I') 
			AND LC_IND_BIL_SNT NOT IN ('1','2','4','7','G');
QUIT;
PROC SQL;
	CREATE TABLE GRALB AS 
		SELECT 
			B.*
		FROM 
			GRALA A
			INNER JOIN BLSTMNT B
			ON A.BF_SSN = B.BF_SSN;
QUIT;
/*===================================================================*
* COMMENT CODE BLOCK 3 FOR PRODUCTION
*====================================================================*
* CODE BLOCK 3
*====================================================================*/
	DATA UTLWS14_TODAY;
		SET BLSTMNT;
	RUN;
	RSUBMIT;
		LIBNAME PROGREVW V8 '/sas/whse/progrevw'; *DUSTER;
		DATA UTLWS14_LAST_RUN;
			SET PROGREVW.UTLWS14_LAST_RUN;
		RUN;
	ENDRSUBMIT;
	DATA UTLWS14_LAST_RUN;
		SET WORKLOCL.UTLWS14_LAST_RUN;
	RUN;
	PROC SORT DATA = UTLWS14_TODAY;
		BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE ;
	RUN;
	PROC SORT DATA = UTLWS14_LAST_RUN;
		BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE ;
	RUN;
	DATA BILL_DUPS;
		MERGE UTLWS14_LAST_RUN (IN=B) 
			  UTLWS14_TODAY (IN=A);
		BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE ;
		IF A AND B;
	RUN;
	PROC SORT DATA=BILL_DUPS;
		BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE ;
	RUN;
/*===================================================================*
*END CODE BLOCK 3
*====================================================================*
*UNCOMMENT CODE BLOCK 4 FOR PRODUCTION
*====================================================================*
*CODE BLOCK 4
*====================================================================*/
/**CREATE DATA SET FOR THE LAST BILLING STATEMENT RUN;*/
/*LIBNAME PROGREVW V8 '/sas/whse/progrevw'; *DUSTER;*/
/*DATA UTLWS14_TODAY;*/
/*	SET BLSTMNT;*/
/*RUN;*/
/*/*THIS OVERWRITES THE EXISTING DATA SET ON DUSTER*/*/
/*DATA UTLWS14_LAST_RUN;*/
/*	SET PROGREVW.UTLWS14_LAST_RUN;*/
/*RUN;*/
/*PROC SORT DATA = UTLWS14_TODAY;*/
/*	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE;*/
/*RUN;*/
/*PROC SORT DATA = UTLWS14_LAST_RUN;*/
/*	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE;*/
/*RUN;*/
/*DATA BILL_DUPS;*/
/*	MERGE UTLWS14_LAST_RUN (IN=B)*/
/*		  UTLWS14_TODAY (IN=A);*/
/*	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE;*/
/*	IF A AND B;*/
/*RUN;*/
/*PROC SORT DATA=BILL_DUPS;*/
/*	BY BF_SSN LD_BIL_CRT LN_SEQ_BIL_WI_DTE;*/
/*RUN;*/
/*/*SAVE HISTORY DATA SET IN CASE IT NEEDS TO BE REFERENCED IN A SPECIAL RUN*/*/
/*DATA PROGREVW.UTLWS14_LAST_RUN;*/
/*	SET UTLWS14_TODAY;*/
/*RUN;*/
/*=================================================================
*END CODE BLOCK 4
===================================================================*/
;
/******************************************************************
*PROCESSING SPLIT DELINQUENCIES
*******************************************************************/
/*counts number of loan sequences since it varies per run*/
PROC CONTENTS 
	DATA=BLSTMNT 
	OUT=BLSTMNT_COLUMNS
	NOPRINT;
RUN;
PROC SQL NOPRINT;
	SELECT COUNT(NAME)
	INTO :COLUMNS
	FROM BLSTMNT_COLUMNS
	WHERE NAME LIKE 'LN_SEQ__';
QUIT;
%PUT &COLUMNS;
%LET COLUMN_COUNT = &COLUMNS;
*creates empty data set in case there are no split delinquencies;
DATA BLSTMNT_DLQ;
	SET BLSTMNT;
	WHERE 1=0;
RUN;
/*identifies split delinquencies*/
%MACRO DLQSPLIT;
	PROC SQL NOPRINT;
		CREATE TABLE BLSTMNT_DLQ AS
			%DO I=3 %TO &COLUMN_COUNT ;
				SELECT * 
				FROM BLSTMNT
				WHERE 
					DAYS_DELQ1 > 0 
					AND 
					(
						DAYS_DELQ2 > 0
						OR DAYS_DELQ&I. > 0
					)
					AND 
					(
						   (DAYS_DELQ1 ^= DAYS_DELQ2 AND DAYS_DELQ2 ^= .)
						OR (DAYS_DELQ1 ^= DAYS_DELQ&I. AND DAYS_DELQ&I. ^= .)
					)
				ORDER BY BF_SSN;
			%END;
	QUIT;
%MEND DLQSPLIT;
%DLQSPLIT;
/*cleans out nonmatching bill data*/
%MACRO DLQCLEANSE;
	DATA BLSTMNT_DLQ;
		SET BLSTMNT_DLQ;
		%DO I=1 %TO &COLUMN_COUNT ;
			IF  DAYS_DELQ ^= DAYS_DELQ&I. 
			THEN 
				DO;
					LD_LON_1_DSB&I. = . ;
					LN_SEQ&I. = . ;
					IC_LON_PGM&I. = ' ' ;
					LC_LON_STA_BIL&I. = ' ' ;
					LR_INT_BIL&I. = . ;
					LA_CUR_PRN_BIL&I. = . ;
					LA_BIL_PAS_DU&I. = . ;
					LN_LTE_FEE&I. = . ;
					DAYS_DELQ&I. = . ;
					OPAFEL&I. = . ;
					TAPTP&I. = . ;
					TAPTI&I. = . ;
					TAGAP&I. = . ;
					BOR_LTE_FEE&I. = . ;
					CUM_LTE_FEE_PD&I. = . ;
					LC_LON_SND_CHC&I. = ' ' ;
				END;
		%END;
	RUN;
%MEND DLQCLEANSE;
%DLQCLEANSE;
/*shifts non-missing values to the left*/
%MACRO LEFT_SHIFT;
	DATA BLSTMNT_DLQ (DROP=J I
		LN_SEQ1 - LN_SEQ&COLUMN_COUNT
		LD_LON_1_DSB1 - LD_LON_1_DSB&COLUMN_COUNT
		IC_LON_PGM1 - IC_LON_PGM&COLUMN_COUNT
		LC_LON_STA_BIL1 - LC_LON_STA_BIL&COLUMN_COUNT 
		LR_INT_BIL1 - LR_INT_BIL&COLUMN_COUNT
		LA_CUR_PRN_BIL1 - LA_CUR_PRN_BIL&COLUMN_COUNT 
		LA_BIL_PAS_DU1 - LA_BIL_PAS_DU&COLUMN_COUNT
		LN_LTE_FEE1 - LN_LTE_FEE&COLUMN_COUNT
		DAYS_DELQ1 - DAYS_DELQ&COLUMN_COUNT 
		OPAFEL1 - OPAFEL&COLUMN_COUNT
		TAPTP1 - TAPTP&COLUMN_COUNT 
		TAPTI1 - TAPTI&COLUMN_COUNT 
		TAGAP1 - TAGAP&COLUMN_COUNT 
		BOR_LTE_FEE1 - BOR_LTE_FEE&COLUMN_COUNT 
		CUM_LTE_FEE_PD1 - CUM_LTE_FEE_PD&COLUMN_COUNT
		LC_LON_SND_CHC1 - LC_LON_SND_CHC&COLUMN_COUNT
		) ;
	SET BLSTMNT_DLQ;
		/*creates flexible number of new columns to hold old data*/
		ARRAY IN1 LN_SEQ1 - LN_SEQ&COLUMN_COUNT ;
		ARRAY OUT1 _LN_SEQ1 - _LN_SEQ&COLUMN_COUNT ;
		ARRAY IN2 LD_LON_1_DSB1 - LD_LON_1_DSB&COLUMN_COUNT ;
		ARRAY OUT2 _LD_LON_1_DSB1 - _LD_LON_1_DSB&COLUMN_COUNT ;
		ARRAY IN3 $ IC_LON_PGM1 - IC_LON_PGM&COLUMN_COUNT ;
		ARRAY OUT3 $6 _IC_LON_PGM1 - _IC_LON_PGM&COLUMN_COUNT ;
		ARRAY IN4 $ LC_LON_STA_BIL1 - LC_LON_STA_BIL&COLUMN_COUNT ;
		ARRAY OUT4 $6 _LC_LON_STA_BIL1 - _LC_LON_STA_BIL&COLUMN_COUNT ;
		ARRAY IN5 LR_INT_BIL1 - LR_INT_BIL&COLUMN_COUNT ;
		ARRAY OUT5 _LR_INT_BIL1 - _LR_INT_BIL&COLUMN_COUNT ;
		ARRAY IN6 LA_CUR_PRN_BIL1 - LA_CUR_PRN_BIL&COLUMN_COUNT ;
		ARRAY OUT6 _LA_CUR_PRN_BIL1 - _LA_CUR_PRN_BIL&COLUMN_COUNT ;
		ARRAY IN7 LA_BIL_PAS_DU1 - LA_BIL_PAS_DU&COLUMN_COUNT ;
		ARRAY OUT7 _LA_BIL_PAS_DU1 - _LA_BIL_PAS_DU&COLUMN_COUNT ;
		ARRAY IN8 LN_LTE_FEE1 - LN_LTE_FEE&COLUMN_COUNT ;
		ARRAY OUT8 _LN_LTE_FEE1 - _LN_LTE_FEE&COLUMN_COUNT ;
		ARRAY IN9 DAYS_DELQ1 - DAYS_DELQ&COLUMN_COUNT ;
		ARRAY OUT9 _DAYS_DELQ1 - _DAYS_DELQ&COLUMN_COUNT ;
		ARRAY IN10 OPAFEL1 - OPAFEL&COLUMN_COUNT ;
		ARRAY OUT10 _OPAFEL1 - _OPAFEL&COLUMN_COUNT ;
		ARRAY IN11 TAPTP1 - TAPTP&COLUMN_COUNT ;
		ARRAY OUT11 _TAPTP1 - _TAPTP&COLUMN_COUNT ;
		ARRAY IN12 TAPTI1 - TAPTI&COLUMN_COUNT ;
		ARRAY OUT12 _TAPTI1 - _TAPTI&COLUMN_COUNT ;
		ARRAY IN13 TAGAP1 - TAGAP&COLUMN_COUNT ;
		ARRAY OUT13 _TAGAP1 - _TAGAP&COLUMN_COUNT ;
		ARRAY IN14 BOR_LTE_FEE1 - BOR_LTE_FEE&COLUMN_COUNT ;
		ARRAY OUT14 _BOR_LTE_FEE1 - _BOR_LTE_FEE&COLUMN_COUNT ;
		ARRAY IN15 CUM_LTE_FEE_PD1 - CUM_LTE_FEE_PD&COLUMN_COUNT ;
		ARRAY OUT15 _CUM_LTE_FEE_PD1 - _CUM_LTE_FEE_PD&COLUMN_COUNT ;
		ARRAY IN16 $ LC_LON_SND_CHC1 - LC_LON_SND_CHC&COLUMN_COUNT ;
		ARRAY OUT16 $2 _LC_LON_SND_CHC1 - _LC_LON_SND_CHC&COLUMN_COUNT ;
		%DO A=1 %TO 16;
			J=1;
			DO I=1 TO &COLUMN_COUNT;
				IF IN&A.(I) NE ' ' 
				THEN 
					DO;
						OUT&A.(J)=IN&A.(I);
						J+1;
					END;
			END;
		%END;
	RUN;
	DATA BLSTMNT_DLQ;
		SET BLSTMNT_DLQ;
			%DO I=1 %TO &COLUMN_COUNT;
				RENAME	
					_LN_SEQ&I. = LN_SEQ&I.
					_LD_LON_1_DSB&I. = LD_LON_1_DSB&I.
					_IC_LON_PGM&I. = IC_LON_PGM&I.
					_LC_LON_STA_BIL&I. = LC_LON_STA_BIL&I.
					_LR_INT_BIL&I. = LR_INT_BIL&I.
					_LA_CUR_PRN_BIL&I. = LA_CUR_PRN_BIL&I.
					_LA_BIL_PAS_DU&I. = LA_BIL_PAS_DU&I.
					_LN_LTE_FEE&I. = LN_LTE_FEE&I.
					_DAYS_DELQ&I. = DAYS_DELQ&I.
					_OPAFEL&I. = OPAFEL&I.
					_TAPTP&I. = TAPTP&I.
					_TAPTI&I. = TAPTI&I. 
					_TAGAP&I. = TAGAP&I.
					_BOR_LTE_FEE&I. = BOR_LTE_FEE&I.
					_CUM_LTE_FEE_PD&I. = CUM_LTE_FEE_PD&I.
					_LC_LON_SND_CHC&I. = LC_LON_SND_CHC&I. ;
			%END;
	RUN;
	DATA BLSTMNT_DLQ;
		SET BLSTMNT_DLQ;
			%DO I=1 %TO &COLUMN_COUNT;
				FORMAT 	LD_LON_1_DSB&I. DATE9.
						IC_LON_PGM&I. $6. 
						LC_LON_STA_BIL&I. $6. 
						LC_LON_SND_CHC&I. $2. ;
			  INFORMAT  LD_LON_1_DSB&I. DATE9.
						IC_LON_PGM&I. $6. 
						LC_LON_STA_BIL&I. $6.
						LC_LON_SND_CHC&I. $2. ;
			%END;
	RUN;
%MEND LEFT_SHIFT;
%LEFT_SHIFT;
/*get correct value for LA_BIL_DU_PRT*/
DATA TARGET (KEEP=BF_SSN);
	SET BLSTMNT_DLQ;
RUN;
PROC SORT DATA=TARGET NODUPRECS;
    BY BF_SSN;
RUN;
*----------------------------------------------;
*comment out for PROMOTION, uncomment for TESTING;
	LIBNAME DUSTER REMOTE SERVER=&SRVR SLIBREF=WORK;
	DATA DUSTER.TARGET; *send data to duster;
		SET TARGET;
	RUN;
	RSUBMIT;
*----------------------------------------------;
LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1;
PROC SQL;
CREATE TABLE BLSTMNT_TARGET AS
	SELECT DISTINCT 
		LN80.BF_SSN
		,LN80.LN_SEQ
		,LN80_MAX.LD_BIL_CRT
		,LN80.LA_BIL_DU_PRT
	FROM 
		OLWHRM1.LN10_LON LN10
		INNER JOIN OLWHRM1.LN80_LON_BIL_CRF LN80
			ON LN10.BF_SSN = LN80.BF_SSN
			AND LN10.LN_SEQ = LN80.LN_SEQ
		INNER JOIN 
		(
			SELECT DISTINCT
				BF_SSN
				,LN_SEQ
				,MAX(LD_BIL_CRT) AS LD_BIL_CRT
			FROM
				OLWHRM1.LN80_LON_BIL_CRF
			GROUP BY
				BF_SSN
				,LN_SEQ
		)LN80_MAX
			ON LN80.BF_SSN = LN80_MAX.BF_SSN
			AND LN80.LN_SEQ = LN80_MAX.LN_SEQ	
			AND LN80.LD_BIL_CRT = LN80_MAX.LD_BIL_CRT
	WHERE 
		LN10.LC_STA_LON10 = 'R'
		AND LN10.LA_CUR_PRI + COALESCE(LN10.LA_NSI_OTS,0) > 0
		AND LN80.LC_STA_LON80 = 'A'
		AND LN10.BF_SSN IN (SELECT BF_SSN FROM TARGET)
	ORDER BY 
		LN80.BF_SSN
		,LN80.LN_SEQ;
QUIT;
*----------------------------------------------;
*comment out for PROMOTION, uncomment for TESTING;
	ENDRSUBMIT;
	DATA BLSTMNT_TARGET;
		SET DUSTER.BLSTMNT_TARGET;
	RUN;
*----------------------------------------------;
/*clears out incorrect amounts*/
DATA BLSTMNT_DLQ ;
	ARRAY IN1 LA_BIL_DU_PRT1 - LA_BIL_DU_PRT&COLUMN_COUNT ;
	SET BLSTMNT_DLQ;
		LA_BIL_DU_PRT = . ;
		LA_BIL_PAS_DU = . ;
		BOR_LTE_FEE = . ;
		AMTDU = . ;
RUN;
/*inserts correct LA_BIL_DU_PRT by loan sequence*/
%MACRO LA_BIL_DU_PRT;
	%DO I=1 %TO &COLUMN_COUNT ;
		PROC SQL NOPRINT;
			UPDATE BLSTMNT_DLQ A
			SET LA_BIL_DU_PRT&I. = (
				SELECT 
					LA_BIL_DU_PRT
				FROM 
					BLSTMNT_TARGET B
				WHERE 
					A.BF_SSN = B.BF_SSN
					AND A.LN_SEQ&I. = B.LN_SEQ
				);
		QUIT;
	%END;
RUN;
%MEND LA_BIL_DU_PRT;
%LA_BIL_DU_PRT;
DATA BLSTMNT_DLQ (DROP=LA_BIL_DU_PRT1 - LA_BIL_DU_PRT&COLUMN_COUNT);
	SET BLSTMNT_DLQ;
	LA_BIL_DU_PRT = SUM(OF LA_BIL_DU_PRT1 - LA_BIL_DU_PRT&COLUMN_COUNT);
RUN;
/*calculates correct values for LA_BIL_PAS_DU, BOR_LTE_FEE, AMTDU */
DATA BLSTMNT_DLQ;
	SET BLSTMNT_DLQ;
		LA_BIL_PAS_DU = SUM(OF LA_BIL_PAS_DU1 - LA_BIL_PAS_DU&COLUMN_COUNT);
		BOR_LTE_FEE = SUM(OF LN_LTE_FEE1 - LN_LTE_FEE&COLUMN_COUNT);
		AMTDU = SUM(LA_BIL_PAS_DU + LA_BIL_DU_PRT + BOR_LTE_FEE);
RUN;
/*********************************************************************
*CREATES BORROWER & ENDORSER DATA SETS FOR BILL PROCESSING
**********************************************************************/
/*removes split delinquent borrowers*/
PROC SQL NOPRINT;
	CREATE TABLE _BLSTMNT_BOR AS
		SELECT 
			A.* 
		FROM 
			BLSTMNT A
			LEFT JOIN BLSTMNT_DLQ B
				ON A.BF_SSN = B.BF_SSN
		WHERE 
			B.BF_SSN IS NULL;
	CREATE TABLE _BLSTMNT_EDR AS
		SELECT 
			C.* 
		FROM 
			BLSTMNT C
			LEFT JOIN BLSTMNT_DLQ D
				ON C.BF_SSN = D.BF_SSN
		WHERE 
			D.BF_SSN IS NULL;;
QUIT;
/*adds clean split delinquent borrowers*/
DATA _BLSTMNT_BOR;
	SET BLSTMNT_DLQ
		_BLSTMNT_BOR ;
RUN;
DATA _BLSTMNT_EDR;
	SET BLSTMNT_DLQ
		_BLSTMNT_EDR ;
RUN;
/************adds special message to bill**********/
*----------------------------------------------;
*comment out appropriate one for TESTING or PROMOTION;
LIBNAME PROGREVW 'Q:\Support Services\Test Files\SAS\SASR 4106'; *TESTING;
/*LIBNAME PROGREVW V8 '/sas/whse/progrevw'; *PROMOTION;*/
*----------------------------------------------;
PROC SQL;
	CREATE TABLE BLSTMNT_BOR AS
	SELECT 
		A.*,
		B.MESSAGE
	FROM 
		_BLSTMNT_BOR A
		LEFT JOIN PROGREVW.SPECIAL_MESSAGE B
			ON 1=1
	WHERE
		B.MESSAGE_ID = '1' /*message to borrower*/
	;
	CREATE TABLE BLSTMNT_EDR AS
	SELECT 
		C.*,
		D.MESSAGE
	FROM 
		_BLSTMNT_EDR C
		LEFT JOIN PROGREVW.SPECIAL_MESSAGE D
			ON 2=2
	WHERE
		D.MESSAGE_ID = '2' /*message to endorser*/
	;
QUIT;
LIBNAME PROGREVW CLEAR;
/*******************************************************************/
* OVERWRITE BORROWER SSN WITH ENDORSER SSN IF APPLICABLE AND 
SET FLAGS TO DISTINGUISH BETWEEN BORROWERS AND ENDORSERS ;
/*******************************************************************/

/****************  BORROWER BILL PROCESSING  **********************/

/******************************************************************/
DATA BLSTMNT;
	SET BLSTMNT_BOR;
	CUM_LTE_FEE_PD = SUM(OF CUM_LTE_FEE_PD1 - CUM_LTE_FEE_PD&COLUMN_COUNT);/*cumulative late fees paid*/
	TAPDFALILF = COALESCE(AMTDU,0) + COALESCE(BOR_LTE_FEE,0);
	LATE_FEE_APP_DATE = LD_BIL_DU + 15;
	DO;
		IF COMPLT_BILL = 'X' AND ROUND(LA_BIL_DU_PRT * .05,.01) LE 15 
		THEN 
			DO;
				EST_LATE_FEE = 15;
			END;
		ELSE IF COMPLT_BILL = 'X' AND ROUND(LA_BIL_DU_PRT * .05,.01) > 15 
		THEN DO;
				EST_LATE_FEE = ROUND(LA_BIL_DU_PRT * .05,.01);
			END;
		ELSE 
			DO;
				EST_LATE_FEE = ROUND(LA_BIL_DU_PRT * .06,.01);
			END;
	END;
	AMTDUPLSELF = AMTDU + EST_LATE_FEE;
	DO;
		IF SUBSTR(SCANLN,10,10) ^= DF_SPE_ACC_ID 
		THEN 
			DO; /*ENDORSER @ DIFFERENT ADDRESS*/
				BF_SSN = LF_EDS;
				BRW = 'N';
				EDR = 'Y';
				IS_EDR = 1;
				EDR_SMADD = 0;
				EDR_NM = '';
			END;
		ELSE IF SMADI = 'Y' 
		THEN 
			DO; /*ENDORSER @ SAME ADDRESS*/
				BF_SSN = BF_SSN;
				BRW = 'Y';
				EDR = 'N';
				IS_EDR = 0;
				EDR_SMADD = 1;
			END;
		ELSE 
			DO;
				BF_SSN = BF_SSN; /*NO ENDORSER*/
				BRW = 'Y';
				EDR = 'N';
				IS_EDR = 0;
				EDR_SMADD = 0;
			END;
	END;
RUN;
/******************************************************************
* FORCE A ZERO VALUE FOR APPROPRIATE VARIABLES
*******************************************************************/
%MACRO FORCE_0_VALUE(VAR2FORCE,MULTI);
	DATA BLSTMNT ;
	SET BLSTMNT;
		%IF &MULTI 
		%THEN 
			%DO;
				%DO I=1 %TO &MAX_LN;
					&VAR2FORCE&I = COALESCE(&VAR2FORCE&I,0);
				%END;
			%END;
		%ELSE 
			%DO;
				&VAR2FORCE = COALESCE(&VAR2FORCE,0);
			%END;
	RUN;
%MEND FORCE_0_VALUE;
%FORCE_0_VALUE(LA_FAT_CUR_PRI,0);
%FORCE_0_VALUE(LA_FAT_NSI,0); 
%FORCE_0_VALUE(LA_FAT_LTE_FEE,0); 
%FORCE_0_VALUE(TAP,0); 
%FORCE_0_VALUE(LA_BIL_PAS_DU,0);
%FORCE_0_VALUE(LA_BIL_DU_PRT,0);
%FORCE_0_VALUE(BOR_LTE_FEE,0);
%FORCE_0_VALUE(AMTDU,0); 
%FORCE_0_VALUE(CUM_LTE_FEE_PD,0);
PROC DATASETS NOPRINT;
	DELETE AMT BLL BLSTMNTA BLSTMNT_DEM BLSTMNT_LN BOR COBRW COBRW_A DELQ GRAL GRALA 
		MESTXT SCAN SCAN2 SCAN2A SCAN2B SCANTB SV30 TXT ;	
QUIT;
PROC SORT DATA=BLSTMNT;
	BY CSTCNTR MSTRSRT;
RUN;
PROC SORT DATA=GRALB;
	BY DM_PRS_LST;
RUN;
PROC SORT DATA=BLLERR;
	BY DM_PRS_LST;
RUN;
PROC SORT DATA=BILL_DUPS;
	BY DM_PRS_LST;
RUN;
*file for R99;
DATA BLSTMNT_R99_BOR;
	SET BLSTMNT;
RUN;
%MACRO REPTS(RPNO,DTST,TITLE1,CRIT);
	PROC PRINTTO PRINT=REPORT&RPNO NEW;
	RUN;
	OPTIONS ORIENTATION = PORTRAIT PAGENO=1 NODATE;
	OPTIONS PS=52 LS=96;
	TITLE	'BILLING STATEMENTS';
	TITLE2 	"&TITLE1";
	TITLE3	"RUNDATE: &CURDATE";
	FOOTNOTE  "JOB = UTLWS14  	 REPORT = ULWS14.LWS14R&RPNO";
	PROC CONTENTS DATA=&DTST OUT=EMPTYSET NOPRINT;
	DATA _NULL_;
		SET EMPTYSET;
		FILE PRINT;
		IF  NOBS=0 AND _N_ =1 
		THEN 
			DO;
				PUT // 96*'-';
				PUT      ////////
					@35 '**** NO OBSERVATIONS FOUND ****';
				PUT ////////
					@39 '-- END OF REPORT --';
				PUT ////////////////
					@29 "JOB = UTLWS14  	 REPORT = ULWS14.LWS14R&RPNO";
			END;
		RETURN;
	RUN;
	PROC PRINT NOOBS SPLIT='/' DATA=&DTST WIDTH=MIN;
		VAR DF_SPE_ACC_ID NAME LD_BIL_CRT LN_SEQ_BIL_WI_DTE AMTDU;
		&CRIT; /*needed for R2-99 processing*/
		LABEL DF_SPE_ACC_ID = 'ACCT #' LD_BIL_CRT = 'BILL CREATE DATE' LN_SEQ_BIL_WI_DTE = 'BILL SEQUENCE' AMTDU = 'TOTAL DUE';
		FORMAT LD_BIL_CRT MMDDYY10. LN_SEQ_BIL_WI_DTE 6. AMTDU DOLLAR10.2;
	RUN;
	PROC PRINTTO;
	RUN;
%MEND REPTS;
%MACRO CREATE_BILL_FILES(RPNO,CRIT);
	DATA BILL_REP_DS;
		SET BLSTMNT;
		&CRIT;
		IF &RPNO = 21 
		THEN 
			DO;
				AMTDU = AMTDU - LA_BIL_PAS_DU;
				LA_BIL_PAS_DU = 0;
			END;
		/*R2-39 processing*/
		%DO I=1 %TO &MAX_LN;
			IF DI_VLD_ADR_borr = 'Y'
				OR DI_CNC_EBL_OPI = 'Y'
				OR LC_LON_SND_CHC&I. = 'Y';
		%END;
	RUN;
	PROC SORT DATA=BILL_REP_DS;
		BY CSTCNTR MSTRSRT STATE;
	RUN;
	DATA _NULL_;
		SET  WORK.BILL_REP_DS;
		FILE REPORT&RPNO DELIMITER=',' DSD DROPOVER LRECL=32767;
		FORMAT ACSKEY $18. ;
		FORMAT BF_SSN $9. ;
		FORMAT DF_SPE_ACC_ID $10. ;
		FORMAT DM_PRS_1 $13. ;
		FORMAT DM_PRS_MID $13. ;
		FORMAT DM_PRS_LST $23. ;
		FORMAT DX_STR_ADR_1 $30. ;
		FORMAT DX_STR_ADR_2 $30. ;
		FORMAT DX_STR_ADR_3 $30. ;
		FORMAT DM_CT $20. ;
		FORMAT STATE $15. ;
		FORMAT DF_ZIP_CDE $18. ;
		FORMAT DM_FGN_CNY $15. ;
		FORMAT LD_LON_1_DSB1-LD_LON_1_DSB&MAX_LN MMDDYY10. ;
		FORMAT LN_SEQ1-LN_SEQ&MAX_LN 6. ;
		FORMAT IC_LON_PGM1-IC_LON_PGM&MAX_LN $6. ;
		FORMAT LC_LON_STA_BIL1-LC_LON_STA_BIL&MAX_LN $6. ;
		FORMAT LR_INT_BIL1-LR_INT_BIL&MAX_LN 7.3 ;
		FORMAT LA_CUR_PRN_BIL1-LA_CUR_PRN_BIL&MAX_LN DOLLAR15.2 ;
		FORMAT LA_BIL_PAS_DU1-LA_BIL_PAS_DU&MAX_LN DOLLAR15.2 ;
		FORMAT LN_LTE_FEE1-LN_LTE_FEE&MAX_LN DOLLAR15.2 ;
		FORMAT CUM_LTE_FEE_PD1-CUM_LTE_FEE_PD&MAX_LN DOLLAR15.2 ; /*cumulative late fees paid*/
		FORMAT DAYS_DELQ1-DAYS_DELQ&MAX_LN BEST12. ;
		FORMAT TAPTP1-TAPTP&MAX_LN DOLLAR15.2 ;
		FORMAT TAPTI1-TAPTI&MAX_LN DOLLAR15.2 ;
		FORMAT TAGAP1-TAGAP&MAX_LN DOLLAR15.2 ;
		FORMAT OPAFEL1-OPAFEL&MAX_LN DOLLAR15.2 ;
		FORMAT LD_FAT_EFF mmddyy10. ;
		FORMAT LA_FAT_CUR_PRI DOLLAR15.2 ;
		FORMAT LA_FAT_NSI DOLLAR15.2 ;
		FORMAT LA_FAT_LTE_FEE DOLLAR15.2 ;
		FORMAT TAP DOLLAR15.2 ;
		FORMAT LD_BIL_CRT MMDDYY10. ; /*bill create date*/
		FORMAT LN_SEQ_BIL_WI_DTE 6. ; /*bill sequence*/
		FORMAT LD_BIL_DU mmddyy10. ;
		FORMAT LA_BIL_PAS_DU DOLLAR15.2 ;
		FORMAT BOR_LTE_FEE DOLLAR15.2 ;
		FORMAT LA_BIL_DU_PRT DOLLAR15.2 ;
		FORMAT AMTDU DOLLAR15.2;
		FORMAT PX_MSG_BIL_ARA $79. ;
		FORMAT SCANLN $46. ;
		FORMAT CSTCNTR $6. ;
		FORMAT LF_EDS $9. ;
		FORMAT DAYS_DELQ BEST12. ;
		FORMAT TAPDFALILF DOLLAR15.2 ;
		FORMAT AMTDUPLSELF DOLLAR15.2 ;
		FORMAT IS_EDR BEST12. ;
		FORMAT EDR_SMADD BEST12.;
		FORMAT LATE_FEE_APP_DATE MMDDYY10. ;
		FORMAT EST_LATE_FEE DOLLAR15.2 ;
		FORMAT CUM_LTE_FEE_PD DOLLAR15.2 ; /*cumulative late fees paid*/
		FORMAT MESSAGE $255. ; /*special message*/
	IF _N_ = 1 THEN        /* WRITE COLUMN NAMES */
	DO;
	   PUT
	   	'ACSKEY' ','
		'BF_SSN' ','
		'DF_SPE_ACC_ID' ',' 
		'DM_PRS_1' ',' 
		'DM_PRS_MID' ',' 
		'DM_PRS_LST' ',' 
		'DX_STR_ADR_1' ',' 
		'DX_STR_ADR_2' ',' 
		'DX_STR_ADR_3' ',' 
		'DM_CT' ',' 
		'STATE' ',' 
		'DF_ZIP_CDE' ',' 
		'DM_FGN_CNY' ',' @;
		DO I=1 TO &MAX_LN;
			PUT 'LD_LON_1_DSB' I @;
			PUT 'LN_SEQ' I @;
			PUT 'IC_LON_PGM' I  @;
			PUT 'LC_LON_STA_BIL' I @;
			PUT 'LR_INT_BIL' I @;
			PUT 'LA_CUR_PRN_BIL' I @;
			PUT 'LA_BIL_PAS_DU' I @;
			PUT 'LN_LTE_FEE' I @;
			PUT 'CUM_LTE_FEE_PD' I @; /*cumulative late fees paid*/
			PUT 'DAYS_DELQ' I @;
			PUT 'TAPTP' I @;
			PUT 'TAPTI' I @;
			PUT 'TAGAP' I @;
			PUT 'OPAFEL' I @;
		END;
		PUT
		'LATE_FEE_APP_DATE' ','
		'EST_LATE_FEE' ',' 
		'CUM_LTE_FEE_PD' ',' /*cumulative late fees paid*/
		'LD_FAT_EFF' ','
		'LA_FAT_CUR_PRI' ','
		'LA_FAT_NSI' ','
		'LA_FAT_LTE_FEE' ','
		'TAP' ','
		'BILL_CREATE_DATE' ',' /*formerly BLL_DT*/
		'BILL_SEQUENCE' ',' /*bill sequence*/
		'DAYS_DELQ' ','
		'TAPDFALILF' ','
		'AMTDUPLSELF' ','
		'LD_BIL_DU' ','
		'LA_BIL_PAS_DU' ','
		'LA_BIL_DU_PRT' ','
		'BOR_LTE_FEE' ','
		'AMTDU' ','
		'PX_MSG_BIL_ARA' ','
		'LF_EDS' ','
		'IS_EDR' ','
		'EDR_SMADD' ','
		'SCANLN' ','
		'EDR_NM' ','
		'STATE_IND' ','
		'COST_CENTER_CODE' ','
		'MESSAGE'; /*special message*/
	END;
	DO;
		PUT ACSKEY $ @;
		PUT BF_SSN $ @;
		PUT DF_SPE_ACC_ID $ @;
		PUT DM_PRS_1 $ @;
		PUT DM_PRS_MID $ @;
		PUT DM_PRS_LST $ @;
		PUT DX_STR_ADR_1 $ @;
		PUT DX_STR_ADR_2 $ @;
		PUT DX_STR_ADR_3 $ @;
		PUT DM_CT $ @;
		PUT STATE $ @;
		PUT DF_ZIP_CDE $ @;
		PUT DM_FGN_CNY $ @;
		%DO I=1 %TO &MAX_LN;
			PUT LD_LON_1_DSB&I @;
			PUT LN_SEQ&I @;
			PUT IC_LON_PGM&I  @;
			PUT LC_LON_STA_BIL&I @;
			PUT LR_INT_BIL&I @;
			PUT LA_CUR_PRN_BIL&I @;
			PUT LA_BIL_PAS_DU&I @;
			PUT LN_LTE_FEE&I @;
			PUT CUM_LTE_FEE_PD&I @; /*cumulative late fees paid*/
			PUT DAYS_DELQ&I @;
			PUT TAPTP&I @;
			PUT TAPTI&I @;
			PUT TAGAP&I @;
			PUT OPAFEL&I @;
		%END;
		PUT LATE_FEE_APP_DATE @;
		PUT EST_LATE_FEE @;
		PUT CUM_LTE_FEE_PD @; /*cumulative late fees paid*/
		PUT LD_FAT_EFF @;
		PUT LA_FAT_CUR_PRI @;
		PUT LA_FAT_NSI @;
		PUT LA_FAT_LTE_FEE @;
		PUT TAP @;
		PUT LD_BIL_CRT @; /*bill create date*/
		PUT LN_SEQ_BIL_WI_DTE @; /*bill sequence*/
		PUT DAYS_DELQ @;
		PUT TAPDFALILF @;
		PUT AMTDUPLSELF @;
		PUT LD_BIL_DU @;
		PUT LA_BIL_PAS_DU @;
		PUT LA_BIL_DU_PRT @;
		PUT BOR_LTE_FEE @;
		PUT AMTDU @;
		PUT PX_MSG_BIL_ARA $ @;
		PUT LF_EDS $ @;
		PUT IS_EDR @;
		PUT EDR_SMADD @;
		PUT SCANLN $ @;
		PUT EDR_NM $ @;
		PUT STATE $ @;
		PUT CSTCNTR $ @;
		PUT MESSAGE $; /*special message*/
	END;
	RUN;
%MEND CREATE_BILL_FILES;
/**********BORROWER BILLS*************/
/*installment bill*/
%CREATE_BILL_FILES(2,
	WHERE LC_BIL_TYP EQ 'P' 
		AND TILP_BILL NE 'X' 
		AND GTMAX_LOANS NE 'X'
		AND DAYS_DELQ <= 31 /*excludes borrowers 271+ days delinquent*/
		AND COMPLT_BILL NE 'X'
		AND IS_EDR = 0
);
/*interest statement*/
%CREATE_BILL_FILES(4,
	WHERE LC_BIL_TYP EQ 'I'
		AND GTMAX_LOANS NE 'X'
		AND COMPLT_BILL NE 'X'
		AND TILP_BILL NE 'X'
		AND IS_EDR = 0
);
/*billing error report more than 28 loans*/
%REPTS(6,GRALB,BORROWERS WITH MORE THAN &MAX_LN LOANS);
/*billing error report nonmatching criteria*/
%REPTS(7,BLLERR,BILL DOES NOT MATCH CRITERIA);
/*duplicate billing statement*/
%REPTS(8,BILL_DUPS,DUPLICATE BILLING STATEMENTS);
/*TILP installment*/
%CREATE_BILL_FILES(10,
	WHERE LC_BIL_TYP EQ 'P' 
		AND TILP_BILL EQ 'X'
		AND COMPLT_BILL NE 'X'
		AND GTMAX_LOANS NE 'X'
);
/*due diligence 1*/
%CREATE_BILL_FILES(12,
	WHERE LC_BIL_TYP EQ 'P' 
		AND GTMAX_LOANS NE 'X'
		AND COMPLT_BILL NE 'X'
		AND TILP_BILL NE 'X'
		AND 31<=DAYS_DELQ<=60
		AND IS_EDR = 0
);
/*due diligence 2*/
%CREATE_BILL_FILES(13,
	WHERE LC_BIL_TYP EQ 'P' 
		AND GTMAX_LOANS NE 'X'
		AND COMPLT_BILL NE 'X'
		AND TILP_BILL NE 'X'
		AND 61<=DAYS_DELQ<=90
		AND IS_EDR = 0
);
/*due diligence 3*/
%CREATE_BILL_FILES(15,
	WHERE LC_BIL_TYP EQ 'P' 
		AND GTMAX_LOANS NE 'X'
		AND COMPLT_BILL NE 'X'
		AND TILP_BILL NE 'X'
		AND 91<=DAYS_DELQ<=120
		AND IS_EDR = 0
);
/*due diligence 4*/
%CREATE_BILL_FILES(16,
	WHERE LC_BIL_TYP EQ 'P' 
		AND GTMAX_LOANS NE 'X'
		AND COMPLT_BILL NE 'X'
		AND TILP_BILL NE 'X'
		AND 121<=DAYS_DELQ<=150
		AND IS_EDR = 0
);
/*due diligence 5*/
%CREATE_BILL_FILES(17,
	WHERE LC_BIL_TYP EQ 'P' 
		AND GTMAX_LOANS NE 'X'
		AND COMPLT_BILL NE 'X'
		AND TILP_BILL NE 'X'
		AND 151<=DAYS_DELQ<=180
		AND IS_EDR = 0
);
/*due diligence 6*/
%CREATE_BILL_FILES(18,
	WHERE LC_BIL_TYP EQ 'P' 
		AND GTMAX_LOANS NE 'X'
		AND COMPLT_BILL NE 'X'
		AND TILP_BILL NE 'X'
		AND 181<=DAYS_DELQ<=210
		AND IS_EDR = 0
);
/*due diligence 7*/
%CREATE_BILL_FILES(19,
	WHERE LC_BIL_TYP EQ 'P' 
		AND GTMAX_LOANS NE 'X'
		AND COMPLT_BILL NE 'X'
		AND TILP_BILL NE 'X'
		AND 211<=DAYS_DELQ<=240
		AND IS_EDR = 0
);
/*due diligence 8*/
%CREATE_BILL_FILES(20,
	WHERE LC_BIL_TYP EQ 'P' 
		AND GTMAX_LOANS NE 'X'
		AND COMPLT_BILL NE 'X'
		AND TILP_BILL NE 'X'
		AND DAYS_DELQ >= 241 /*includes borrowers 271+ days delinquent*/
		AND IS_EDR = 0
);
/*reduced payment statement*/
%CREATE_BILL_FILES(21,
	WHERE LC_BIL_TYP IN ('P','C')
		AND SENT_IND_GROUP EQ 'T' 
		AND GTMAX_LOANS NE 'X'
		AND TILP_BILL NE 'X'
		AND COMPLT_BILL NE 'X'
		AND IS_EDR = 0
);
/******************************************************************/
* OVERWRITE BORROWER SSN WITH ENDORSER SSN IF APPLICABLE AND 
SET FLAGS TO DISTINGUISH BETWEEN BORROWERS AND ENDORSERS ;
/******************************************************************/

/****************  ENDORSER BILL PROCESSING  **********************/

/******************************************************************/
DATA BLSTMNT;
	SET BLSTMNT_EDR;
	CUM_LTE_FEE_PD = SUM(OF CUM_LTE_FEE_PD1 - CUM_LTE_FEE_PD&COLUMN_COUNT);/*cumulative late fees paid*/
	TAPDFALILF = COALESCE(AMTDU,0) + COALESCE(BOR_LTE_FEE,0);
	LATE_FEE_APP_DATE = LD_BIL_DU + 15;
	DO;
		IF COMPLT_BILL = 'X' AND ROUND(LA_BIL_DU_PRT * .05,.01) LE 15 
		THEN 
			DO;
				EST_LATE_FEE = 15;
			END;
		ELSE IF COMPLT_BILL = 'X' AND ROUND(LA_BIL_DU_PRT * .05,.01) > 15 
		THEN 
			DO;
				EST_LATE_FEE = ROUND(LA_BIL_DU_PRT * .05,.01);
			END;
		ELSE 
			DO;
				EST_LATE_FEE = ROUND(LA_BIL_DU_PRT * .06,.01);
			END;
	END;
	AMTDUPLSELF = AMTDU + EST_LATE_FEE;
	DO;
		IF SUBSTR(SCANLN,10,10) ^= DF_SPE_ACC_ID 
		THEN 
			DO; /*ENDORSER @ DIFFERENT ADDRESS*/
				BRW = 'N';
				EDR = 'Y';
				IS_EDR = 1;
				EDR_SMADD = 0;
				EDR_NM = '';
				DF_SPE_ACC_ID = EDS_ACCT;
			END;
		ELSE IF SMADI = 'Y' 
		THEN 
			DO; /*ENDORSER @ SAME ADDRESS*/
				BRW = 'Y';
				EDR = 'N';
				IS_EDR = 1;
				EDR_SMADD = 1;
				DM_PRS_1 = EDR_DM_PRS_1;
				DM_PRS_MID = EDR_DM_PRS_MID;
				DM_PRS_LST = EDR_DM_PRS_LST;
				EDR_NM = '';
				DF_SPE_ACC_ID = EDS_ACCT;
			END;
		ELSE 
			DO;
				BF_SSN = BF_SSN; /*NO ENDORSER*/
				BRW = 'Y';
				EDR = 'N';
				IS_EDR = 0;
				EDR_SMADD = 0;
			END;
	END;
RUN;
/******************************************************************
* FORCE A ZERO VALUE FOR APPROPRIATE VARIABLES
*******************************************************************/
%MACRO FORCE_0_VALUE(VAR2FORCE,MULTI);
	DATA BLSTMNT ;
	SET BLSTMNT;
		%IF &MULTI 
		%THEN 
			%DO;
				%DO I=1 %TO &MAX_LN;
					&VAR2FORCE&I = COALESCE(&VAR2FORCE&I,0);
				%END;
			%END;
		%ELSE 
			%DO;
				&VAR2FORCE = COALESCE(&VAR2FORCE,0);
			%END;
	RUN;
%MEND FORCE_0_VALUE;
%FORCE_0_VALUE(LA_FAT_CUR_PRI,0);
%FORCE_0_VALUE(LA_FAT_NSI,0); 
%FORCE_0_VALUE(LA_FAT_LTE_FEE,0); 
%FORCE_0_VALUE(TAP,0); 
%FORCE_0_VALUE(LA_BIL_PAS_DU,0);
%FORCE_0_VALUE(LA_BIL_DU_PRT,0);
%FORCE_0_VALUE(BOR_LTE_FEE,0);
%FORCE_0_VALUE(AMTDU,0);
*file for R99;
DATA BLSTMNT_R99_EDR;
	SET BLSTMNT;
RUN;
PROC SORT DATA=BLSTMNT;
	BY CSTCNTR MSTRSRT;
RUN;
%MACRO CREATE_BILL_FILES_EDR(RPNO,CRIT);
	DATA BILL_REP_DS;
		SET BLSTMNT;
		&CRIT;
		/*R2-99 processing*/
		%DO I=1 %TO &MAX_LN;
			IF DI_VLD_ADR_endo = 'Y'
				OR DI_CNC_EBL_OPI = 'Y'
				OR LC_LON_SND_CHC&I. = 'Y';
		%END;
	RUN;
	PROC SORT DATA=BILL_REP_DS;
		BY CSTCNTR MSTRSRT STATE;
	RUN;
	DATA _NULL_;
		SET WORK.BILL_REP_DS;
		FILE REPORT&RPNO DELIMITER=',' DSD DROPOVER LRECL=32767;
		FORMAT ACSKEY $18. ;
		FORMAT BF_SSN $9. ;
		FORMAT DF_SPE_ACC_ID $10. ;
		FORMAT DM_PRS_1 $13. ;
		FORMAT DM_PRS_MID $13. ;
		FORMAT DM_PRS_LST $23. ;
		FORMAT DX_STR_ADR_1 $30. ;
		FORMAT DX_STR_ADR_2 $30. ;
		FORMAT DX_STR_ADR_3 $30. ;
		FORMAT DM_CT $20. ;
		FORMAT STATE $15. ;
		FORMAT DF_ZIP_CDE $18. ;
		FORMAT DM_FGN_CNY $15. ;
		FORMAT LD_LON_1_DSB1-LD_LON_1_DSB&MAX_LN MMDDYY10. ;
		FORMAT LN_SEQ1-LN_SEQ&MAX_LN 6. ;
		FORMAT IC_LON_PGM1-IC_LON_PGM&MAX_LN $6. ;
		FORMAT LC_LON_STA_BIL1-LC_LON_STA_BIL&MAX_LN $6. ;
		FORMAT LR_INT_BIL1-LR_INT_BIL&MAX_LN 7.3 ;
		FORMAT LA_CUR_PRN_BIL1-LA_CUR_PRN_BIL&MAX_LN DOLLAR15.2 ;
		FORMAT LA_BIL_PAS_DU1-LA_BIL_PAS_DU&MAX_LN DOLLAR15.2 ;
		FORMAT LN_LTE_FEE1-LN_LTE_FEE&MAX_LN DOLLAR15.2 ;
		FORMAT CUM_LTE_FEE_PD1-CUM_LTE_FEE_PD&MAX_LN DOLLAR15.2 ; /*cumulative late fees paid*/
		FORMAT DAYS_DELQ1-DAYS_DELQ&MAX_LN BEST12. ;
		FORMAT TAPTP1-TAPTP&MAX_LN DOLLAR15.2 ;
		FORMAT TAPTI1-TAPTI&MAX_LN DOLLAR15.2 ;
		FORMAT TAGAP1-TAGAP&MAX_LN DOLLAR15.2 ;
		FORMAT OPAFEL1-OPAFEL&MAX_LN DOLLAR15.2 ;
		FORMAT LD_FAT_EFF mmddyy10. ;
		FORMAT LA_FAT_CUR_PRI DOLLAR15.2 ;
		FORMAT LA_FAT_NSI DOLLAR15.2 ;
		FORMAT LA_FAT_LTE_FEE DOLLAR15.2 ;
		FORMAT TAP DOLLAR15.2 ;
		FORMAT LD_BIL_CRT MMDDYY10. ; /*bill create date*/
		FORMAT LN_SEQ_BIL_WI_DTE 6. ; /*bill sequence*/
		FORMAT LD_BIL_DU mmddyy10. ;
		FORMAT LA_BIL_PAS_DU DOLLAR15.2 ;
		FORMAT BOR_LTE_FEE DOLLAR15.2 ;
		FORMAT LA_BIL_DU_PRT DOLLAR15.2 ;
		FORMAT AMTDU DOLLAR15.2;
		FORMAT PX_MSG_BIL_ARA $79. ;
		FORMAT SCANLN $46. ;
		FORMAT CSTCNTR $6. ;
		FORMAT LF_EDS $9. ;
		FORMAT DAYS_DELQ BEST12. ;
		FORMAT TAPDFALILF DOLLAR15.2 ;
		FORMAT AMTDUPLSELF DOLLAR15.2 ;
		FORMAT IS_EDR BEST12. ;
		FORMAT EDR_SMADD BEST12. ;
		FORMAT LATE_FEE_APP_DATE MMDDYY10. ;
		FORMAT EST_LATE_FEE DOLLAR15.2 ;
		FORMAT CUM_LTE_FEE_PD DOLLAR15.2 ; /*cumulative late fees paid*/
		FORMAT MESSAGE $255. ; /*special message*/
	IF _N_ = 1 THEN        /* WRITE COLUMN NAMES */
	DO;
	   PUT
	   	'ACSKEY' ','
		'BF_SSN' ','
		'DF_SPE_ACC_ID' ',' 
		'DM_PRS_1' ',' 
		'DM_PRS_MID' ',' 
		'DM_PRS_LST' ',' 
		'DX_STR_ADR_1' ',' 
		'DX_STR_ADR_2' ',' 
		'DX_STR_ADR_3' ',' 
		'DM_CT' ',' 
		'STATE' ',' 
		'DF_ZIP_CDE' ',' 
		'DM_FGN_CNY' ',' @;
		DO I=1 TO &MAX_LN;
			PUT 'LD_LON_1_DSB' I @;
			PUT 'LN_SEQ' I @;
			PUT 'IC_LON_PGM' I  @;
			PUT 'LC_LON_STA_BIL' I @;
			PUT 'LR_INT_BIL' I @;
			PUT 'LA_CUR_PRN_BIL' I @;
			PUT 'LA_BIL_PAS_DU' I @;
			PUT 'LN_LTE_FEE' I @;
			PUT 'CUM_LTE_FEE_PD' I @ ; /*cumulative late fees paid*/
			PUT 'DAYS_DELQ' I @;
			PUT 'TAPTP' I @;
			PUT 'TAPTI' I @;
			PUT 'TAGAP' I @;
			PUT 'OPAFEL' I @;
		END;
		PUT
		'LATE_FEE_APP_DATE' ','
		'EST_LATE_FEE' ',' 
		'CUM_LTE_FEE_PD' ','/*cumulative late fees paid*/
		'LD_FAT_EFF' ','
		'LA_FAT_CUR_PRI' ','
		'LA_FAT_NSI' ','
		'LA_FAT_LTE_FEE' ','
		'TAP' ','
		'BILL_CREATE_DATE' ',' /*formerly BLL_DT*/
		'BILL_SEQUENCE' ',' /*bill sequence*/
		'DAYS_DELQ' ','
		'TAPDFALILF' ','
		'AMTDUPLSELF' ','
		'LD_BIL_DU' ','
		'LA_BIL_PAS_DU' ','
		'LA_BIL_DU_PRT' ','
		'BOR_LTE_FEE' ','
		'AMTDU' ','
		'PX_MSG_BIL_ARA' ','
		'LF_EDS' ','
		'IS_EDR' ','
		'EDR_SMADD' ','
		'SCANLN' ','
		'EDR_NM' ','
		'STATE_IND' ','
		'COST_CENTER_CODE' ','
		'MESSAGE'; /*special message*/
	END;
	DO;
		PUT ACSKEY $ @;
		PUT BF_SSN $ @;
		PUT DF_SPE_ACC_ID $ @;
		PUT DM_PRS_1 $ @;
		PUT DM_PRS_MID $ @;
		PUT DM_PRS_LST $ @;
		PUT DX_STR_ADR_1 $ @;
		PUT DX_STR_ADR_2 $ @;
		PUT DX_STR_ADR_3 $ @;
		PUT DM_CT $ @;
		PUT STATE $ @;
		PUT DF_ZIP_CDE $ @;
		PUT DM_FGN_CNY $ @;
		%DO I=1 %TO &MAX_LN;
			PUT LD_LON_1_DSB&I @;
			PUT LN_SEQ&I @;
			PUT IC_LON_PGM&I  @;
			PUT LC_LON_STA_BIL&I @;
			PUT LR_INT_BIL&I @;
			PUT LA_CUR_PRN_BIL&I @;
			PUT LA_BIL_PAS_DU&I @;
			PUT LN_LTE_FEE&I @;
			PUT CUM_LTE_FEE_PD&I @; /*cumulative late fees paid*/
			PUT DAYS_DELQ&I @;
			PUT TAPTP&I @;
			PUT TAPTI&I @;
			PUT TAGAP&I @;
			PUT OPAFEL&I @;
		%END;
		PUT LATE_FEE_APP_DATE @;
		PUT EST_LATE_FEE @;
		PUT CUM_LTE_FEE_PD @; /*cumulative late fees paid*/
		PUT LD_FAT_EFF @;
		PUT LA_FAT_CUR_PRI @;
		PUT LA_FAT_NSI @;
		PUT LA_FAT_LTE_FEE @;
		PUT TAP @;
		PUT LD_BIL_CRT @; /*bill create date*/
		PUT LN_SEQ_BIL_WI_DTE @; /*bill sequence*/
		PUT DAYS_DELQ @;
		PUT TAPDFALILF @;
		PUT AMTDUPLSELF @;
		PUT LD_BIL_DU @;
		PUT LA_BIL_PAS_DU @;
		PUT LA_BIL_DU_PRT @;
		PUT BOR_LTE_FEE @;
		PUT AMTDU @;
		PUT PX_MSG_BIL_ARA $ @;
		PUT LF_EDS $ @;
		PUT IS_EDR @;
		PUT EDR_SMADD @;
		PUT SCANLN $ @;
		PUT EDR_NM $ @;
		PUT STATE $ @;
		PUT CSTCNTR $ @;
		PUT MESSAGE $; /*special message*/
	END;
	RUN;
%MEND CREATE_BILL_FILES_EDR;	
/*installment bill*/
%CREATE_BILL_FILES_EDR(27,
	WHERE LC_BIL_TYP EQ 'P' 
		AND TILP_BILL NE 'X' 
		AND GTMAX_LOANS NE 'X'
		AND DAYS_DELQ <= 31 /*excludes borrowers 271+ days delinquent*/
		AND COMPLT_BILL NE 'X'
		AND IS_EDR = 1
);
/*interest statement*/
%CREATE_BILL_FILES_EDR(29,
	WHERE LC_BIL_TYP EQ 'I'
		AND GTMAX_LOANS NE 'X'
		AND COMPLT_BILL NE 'X'
		AND TILP_BILL NE 'X'
		AND IS_EDR = 1
);
/*due diligence 1*/
%CREATE_BILL_FILES_EDR(31,
	WHERE LC_BIL_TYP EQ 'P' 
		AND GTMAX_LOANS NE 'X'
		AND COMPLT_BILL NE 'X'
		AND TILP_BILL NE 'X'
		AND 31<=DAYS_DELQ<=60
		AND IS_EDR = 1
);
/*due diligence 2*/
%CREATE_BILL_FILES_EDR(32,
	WHERE LC_BIL_TYP EQ 'P' 
		AND GTMAX_LOANS NE 'X'
		AND COMPLT_BILL NE 'X'
		AND TILP_BILL NE 'X'
		AND 61<=DAYS_DELQ<=90
		AND IS_EDR = 1
);
/*due diligence 3*/
%CREATE_BILL_FILES_EDR(33,
	WHERE LC_BIL_TYP EQ 'P' 
		AND GTMAX_LOANS NE 'X'
		AND COMPLT_BILL NE 'X'
		AND TILP_BILL NE 'X'
		AND 91<=DAYS_DELQ<=120
		AND IS_EDR = 1
);
/*due diligence 4*/
%CREATE_BILL_FILES_EDR(34,
	WHERE LC_BIL_TYP EQ 'P' 
		AND GTMAX_LOANS NE 'X'
		AND COMPLT_BILL NE 'X'
		AND TILP_BILL NE 'X'
		AND 121<=DAYS_DELQ<=150
		AND IS_EDR = 1
);
/*due diligence 5*/
%CREATE_BILL_FILES_EDR(35,
	WHERE LC_BIL_TYP EQ 'P' 
		AND GTMAX_LOANS NE 'X'
		AND COMPLT_BILL NE 'X'
		AND TILP_BILL NE 'X'
		AND 151<=DAYS_DELQ<=180
		AND IS_EDR = 1
);
/*due diligence 6*/
%CREATE_BILL_FILES_EDR(36,
	WHERE LC_BIL_TYP EQ 'P' 
		AND GTMAX_LOANS NE 'X'
		AND COMPLT_BILL NE 'X'
		AND TILP_BILL NE 'X'
		AND 181<=DAYS_DELQ<=210
		AND IS_EDR = 1
);
/*due diligence 7*/
%CREATE_BILL_FILES_EDR(37,
	WHERE LC_BIL_TYP EQ 'P' 
		AND GTMAX_LOANS NE 'X'
		AND COMPLT_BILL NE 'X'
		AND TILP_BILL NE 'X'
		AND 211<=DAYS_DELQ<=240
		AND IS_EDR = 1
);
/*due diligence 8*/
%CREATE_BILL_FILES_EDR(38,
	WHERE LC_BIL_TYP EQ 'P' 
		AND GTMAX_LOANS NE 'X'
		AND COMPLT_BILL NE 'X'
		AND TILP_BILL NE 'X'
		AND DAYS_DELQ >= 241 /*includes borrowers 271+ days delinquent*/
		AND IS_EDR = 1
);
/*reduced payment statement*/
%CREATE_BILL_FILES_EDR(39,
	WHERE LC_BIL_TYP IN ('P','C')
		AND SENT_IND_GROUP EQ 'T' 
		AND GTMAX_LOANS NE 'X'
		AND TILP_BILL NE 'X'
		AND COMPLT_BILL NE 'X'
		AND IS_EDR = 1
);
/*NonBilled Borrowers & Endorsers*/
DATA NNN_BOR;		
	SET BLSTMNT_R99_BOR;
	IF IS_EDR = 0;
	IF DI_VLD_ADR_borr = 'N';
	IF DI_CNC_EBL_OPI = 'N';
	APP_LN_SEQ = 'ALL';
RUN;
%MACRO R99_EDR;
	DATA _NNN_EDR;	
		SET BLSTMNT_R99_EDR;
		REGARDSTOCODE = 'E';
		REGARDSTOID = LF_EDS;
		IF IS_EDR = 1;
		IF DI_VLD_ADR_endo = 'N';
		IF DI_CNC_EBL_OPI = 'N';
		%DO I=1 %TO &COLUMN_COUNT;*gets every applicable loan sequence;
			APP_LN_SEQ = LN_SEQ&I.;
			OUTPUT;
		%END;
	RUN;
%MEND R99_EDR;
%R99_EDR;
PROC SORT DATA=NNN_BOR NODUPKEY;
	BY DF_SPE_ACC_ID IS_EDR CSTCNTR MSTRSRT STATE APP_LN_SEQ;
RUN;
PROC SORT DATA=_NNN_EDR NODUPKEY;
	BY DF_SPE_ACC_ID IS_EDR CSTCNTR MSTRSRT STATE APP_LN_SEQ;
	WHERE APP_LN_SEQ ^=.;
RUN;
*converts numeric LN_SEQ to character;
DATA NNN_EDR (RENAME=(_APP_LN_SEQ = APP_LN_SEQ));
	SET _NNN_EDR;
	_APP_LN_SEQ = PUT(APP_LN_SEQ,3.);
	DROP APP_LN_SEQ;
RUN;
*combines both data sets;
DATA BLSTMNT_R99;
	SET NNN_BOR
		NNN_EDR;
RUN;		
%MACRO CREATE_R99(RPNO);
	DATA BILL_REP_DS;
		SET BLSTMNT_R99;
	RUN;
	PROC SORT DATA=BILL_REP_DS;
		BY CSTCNTR MSTRSRT STATE;
	RUN;
	DATA _NULL_;
		SET  WORK.BILL_REP_DS;
		FILE REPORT&RPNO DELIMITER=',' DSD DROPOVER LRECL=32767;
		DO;
			PUT BF_SSN @;
			PUT 'BILLN,' @;
			PUT ',' @;
			PUT ',' @;
			PUT ',' @;
			PUT ',' @;
			PUT REGARDSTOCODE @;
			PUT REGARDSTOID @;
			PUT APP_LN_SEQ @;
			PUT 'Has invalid address and is not on ecorr' ;
		END;
	RUN;
%MEND CREATE_R99;
%CREATE_R99(99);
