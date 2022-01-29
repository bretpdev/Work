DATA BKY;
	INFILE '\\FSUHEAADOCS\DomainUsersData\jlwright\Desktop\MR-1\Bankruptcy.txt';
	INPUT
		SSN $ 1-11 
		CLAIM_ID $16-19
		PAID_LENDER_ED $ 20-38
		TOTAL_PAID $ 39-56
		COMPLEMENT $ 57-74
		MR1OTHER $ 75-92
		FileDate $ 93-99
		;
RUN;

DATA BKY;
	SET BKY;
	SSN = COMPRESS(TRANSLATE(SSN,'','-'));
	PAID_LENDER_ED = COMPRESS(TRANSLATE(PAID_LENDER_ED,'','$,'));
	TOTAL_PAID = COMPRESS(TRANSLATE(TOTAL_PAID,'','$,'));
	COMPLEMENT = COMPRESS(TRANSLATE(COMPLEMENT,'','$,'));
	MR1OTHER = COMPRESS(TRANSLATE(MR1OTHER,'','$,'));
RUN;

DATA DEFAULT;
	INFILE '\\FSUHEAADOCS\DomainUsersData\jlwright\Desktop\MR-1\Default.txt';
	INPUT
		SSN $ 1-11 
		CLAIM_ID $16-19
		PAID_LENDER_ED $ 20-38
		TOTAL_PAID $ 39-56
		COMPLEMENT $ 57-74
		MR1OTHER $ 75-92
		FileDate $ 93-99
		;
RUN;

DATA DEFAULT;
	SET DEFAULT;
	SSN = COMPRESS(TRANSLATE(SSN,'','-'));
	PAID_LENDER_ED = COMPRESS(TRANSLATE(PAID_LENDER_ED,'','$,'));
	TOTAL_PAID = COMPRESS(TRANSLATE(TOTAL_PAID,'','$,'));
	COMPLEMENT = COMPRESS(TRANSLATE(COMPLEMENT,'','$,'));
	MR1OTHER = COMPRESS(TRANSLATE(MR1OTHER,'','$,'));
RUN;

DATA DTHDIS;
	INFILE '\\FSUHEAADOCS\DomainUsersData\jlwright\Desktop\MR-1\DeathDis.txt';
	INPUT
		SSN $ 1-11 
		CLAIM_ID $16-19
		PAID_LENDER_ED $ 20-38
		TOTAL_PAID $ 39-56
		COMPLEMENT $ 57-74
		MR1OTHER $ 75-92
		FileDate $ 93-99
		;
RUN;

DATA DTHDIS;
	SET DTHDIS;
	SSN = COMPRESS(TRANSLATE(SSN,'','-'));
	PAID_LENDER_ED = COMPRESS(TRANSLATE(PAID_LENDER_ED,'','$,'));
	TOTAL_PAID = COMPRESS(TRANSLATE(TOTAL_PAID,'','$,'));
	COMPLEMENT = COMPRESS(TRANSLATE(COMPLEMENT,'','$,'));
	MR1OTHER = COMPRESS(TRANSLATE(MR1OTHER,'','$,'));
RUN;

DATA DISCHARGE;
	INFILE '\\FSUHEAADOCS\DomainUsersData\jlwright\Desktop\MR-1\Discharge.txt';
	INPUT
		SSN $ 1-11 
		CLAIM_ID $16-19
		PAID_LENDER_ED $ 20-38
		TOTAL_PAID $ 39-56
		COMPLEMENT $ 57-74
		MR1OTHER $ 75-92
		FileDate $ 93-99
		;
RUN;

DATA DISCHARGE;
	SET DISCHARGE;
	SSN = COMPRESS(TRANSLATE(SSN,'','-'));
	PAID_LENDER_ED = COMPRESS(TRANSLATE(PAID_LENDER_ED,'','$,'));
	TOTAL_PAID = COMPRESS(TRANSLATE(TOTAL_PAID,'','$,'));
	COMPLEMENT = COMPRESS(TRANSLATE(COMPLEMENT,'','$,'));
	MR1OTHER = COMPRESS(TRANSLATE(MR1OTHER,'','$,'));
RUN;


LIBNAME WORKLOCL REMOTE SERVER=DUSTER SLIBREF=WORK;
DATA WORKLOCL.BKY; SET BKY; RUN;
DATA WORKLOCL.DEFAULT; SET DEFAULT; RUN;
DATA WORKLOCL.DTHDIS; SET DTHDIS; RUN;
DATA WORKLOCL.DISCHARGE; SET DISCHARGE; RUN;

/*SUBMIT CODE TO REMOTE SERVER*/
RSUBMIT DUSTER;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%let DB = DLGSUTWH;
%let OWNR = OLWHRM1;

PROC SQL STIMER;
	CONNECT TO DB2 (DATABASE=&DB); 

/*	get data for all loans from DB2 warehouse*/
	CREATE TABLE MR_and_DB2_DATA AS
		SELECT
			B.FileDate
			,'MR-1-E' AS Form2000
			,DB2D.DM_PRS_LST
			,DB2D.DM_PRS_1
			,B.SSN
			,DB2D.AF_APL_ID || REPEAT('0',1-LENGTH(DB2D.AF_APL_ID_SFX))||DB2D.AF_APL_ID_SFX AS LoanId
			,DB2D.IF_OPS_LDR
			,DB2D.LD_LDR_POF
			,B.PAID_LENDER_ED
			,B.PAID_LENDER_ED AS AMOUNT
			,'749' AS GACode
		FROM 
			CONNECTION TO DB2 
			(
				SELECT DISTINCT
					DC01.AF_APL_ID
					,DC01.AF_APL_ID_SFX
					,PD01.DM_PRS_LST
					,PD01.DM_PRS_1
					,PD01.DF_PRS_ID
					,DC01.IF_OPS_LDR
					,COALESCE(DC01.LD_AUX_STA_UPD, DC01.LD_LDR_POF) AS LD_LDR_POF
					,DC01.LF_CLM_ID
				FROM 
					&OWNR..PD01_PDM_INF PD01
					INNER JOIN &OWNR..DC01_LON_CLM_INF DC01
						ON PD01.DF_PRS_ID = DC01.BF_SSN
				FOR READ ONLY WITH UR
			) DB2D
			INNER JOIN BKY B
				ON DB2D.DF_PRS_ID = B.SSN
				AND DB2D.LF_CLM_ID = B.CLAIM_ID

		UNION ALL

		SELECT
			B.FileDate
			,'MR-1-A' AS Form2000
			,DB2D.DM_PRS_LST
			,DB2D.DM_PRS_1
			,B.SSN
			,DB2D.AF_APL_ID || REPEAT('0',1-LENGTH(DB2D.AF_APL_ID_SFX))||DB2D.AF_APL_ID_SFX AS LoanId
			,DB2D.IF_OPS_LDR
			,DB2D.LD_LDR_POF
			,B.PAID_LENDER_ED
			,B.PAID_LENDER_ED AS AMOUNT
			,'749' AS GACode
		FROM 
			CONNECTION TO DB2 
			(
				SELECT DISTINCT
					DC01.AF_APL_ID
					,DC01.AF_APL_ID_SFX
					,PD01.DM_PRS_LST
					,PD01.DM_PRS_1
					,PD01.DF_PRS_ID
					,DC01.IF_OPS_LDR
					,COALESCE(DC01.LD_AUX_STA_UPD, DC01.LD_LDR_POF) AS LD_LDR_POF
					,DC01.LF_CLM_ID
				FROM 
					&OWNR..PD01_PDM_INF PD01
					INNER JOIN &OWNR..DC01_LON_CLM_INF DC01
						ON PD01.DF_PRS_ID = DC01.BF_SSN
	
				FOR READ ONLY WITH UR
			) DB2D
			INNER JOIN DEFAULT B
				ON DB2D.DF_PRS_ID = B.SSN
				AND DB2D.LF_CLM_ID = B.CLAIM_ID

		UNION ALL

		SELECT
			B.FileDate
			,'MR-1-C' AS Form2000
			,DB2D.DM_PRS_LST
			,DB2D.DM_PRS_1
			,B.SSN
			,DB2D.AF_APL_ID || REPEAT('0',1-LENGTH(DB2D.AF_APL_ID_SFX))||DB2D.AF_APL_ID_SFX AS LoanId
			,DB2D.IF_OPS_LDR
			,DB2D.LD_LDR_POF
			,B.PAID_LENDER_ED
			,B.PAID_LENDER_ED AS AMOUNT
			,'749' AS GACode
		FROM 
			CONNECTION TO DB2 
			(
				SELECT DISTINCT
					DC01.AF_APL_ID
					,DC01.AF_APL_ID_SFX
					,PD01.DM_PRS_LST
					,PD01.DM_PRS_1
					,PD01.DF_PRS_ID
					,DC01.IF_OPS_LDR
					,COALESCE(DC01.LD_AUX_STA_UPD, DC01.LD_LDR_POF) AS LD_LDR_POF
					,DC01.LF_CLM_ID
				FROM 
					&OWNR..PD01_PDM_INF PD01
					INNER JOIN &OWNR..DC01_LON_CLM_INF DC01
						ON PD01.DF_PRS_ID = DC01.BF_SSN
	
				FOR READ ONLY WITH UR
			) DB2D
			INNER JOIN DTHDIS B
				ON DB2D.DF_PRS_ID = B.SSN
				AND DB2D.LF_CLM_ID = B.CLAIM_ID

		UNION ALL

		SELECT
			B.FileDate
			,'MR-1-G' AS Form2000
			,DB2D.DM_PRS_LST
			,DB2D.DM_PRS_1
			,B.SSN
			,DB2D.LF_LON_ALT || REPEAT('0',1-LENGTH(DB2D.LN_LON_ALT_SEQ))||DB2D.LN_LON_ALT_SEQ AS LoanId
			,DB2D.IF_OPS_LDR
			,DB2D.LD_LDR_POF
			,B.PAID_LENDER_ED
			,B.PAID_LENDER_ED AS AMOUNT
			,'749' AS GACode
		FROM 
			CONNECTION TO DB2 
			(
				SELECT DISTINCT
					LN10.LF_LON_ALT
					,CAST(LN10.LN_LON_ALT_SEQ AS VARCHAR(2)) AS LN_LON_ALT_SEQ
					,PD01.DM_PRS_LST
					,PD01.DM_PRS_1
					,PD01.DF_PRS_ID
					,LN10.LF_LON_CUR_OWN AS IF_OPS_LDR
					,LN90.LD_FAT_EFF AS LD_LDR_POF
				FROM 
					&OWNR..PD01_PDM_INF PD01
					INNER JOIN &OWNR..LN90_FIN_ATY LN90
						ON PD01.DF_PRS_ID = LN90.BF_SSN
						AND LN90.PC_FAT_TYP = '10'
						AND LN90.PC_FAT_SUB_TYP = '50'
					INNER JOIN &OWNR..LN10_LON LN10
						ON LN10.BF_SSN = LN90.BF_SSN
						AND LN10.LN_SEQ = LN90.LN_SEQ
	
				FOR READ ONLY WITH UR
			) DB2D
			INNER JOIN DISCHARGE B
				ON DB2D.DF_PRS_ID = B.SSN
	;
	DISCONNECT FROM DB2;

QUIT;

ENDRSUBMIT;

/*copy down data set of remote data joined with local data to local WORK folder*/
DATA MR_and_DB2_DATA; SET WORKLOCL.MR_and_DB2_DATA; RUN;


/*create an csv copy of the data for testing and review*/
ODS CSV FILE="T:\SAS\UNH 71353.csv";
PROC PRINT DATA=WORK.MR_and_DB2_DATA;
RUN;
ODS CSV CLOSE;


/*write data to flat file*/
DATA _NULL_;
	SET MR_and_DB2_DATA;
	FILE 'T:\SAS\UNH 71353.txt';
	FORMAT LD_LDR_POF YYMMDDn8.;

	DO;

	PUT 
		@1 FileDate
		@8 Form2000
		@15 DM_PRS_LST
		@47 DM_PRS_1
		@79 SSN
		@88 LoanId
		@118 IF_OPS_LDR
		@128 LD_LDR_POF
		@136 PAID_LENDER_ED $15. -r
		@151 AMOUNT $15. -r
		@166 GACode
	;
	END;
RUN;
