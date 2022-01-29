LIBNAME DNFPUTDL DB2 DATABASE=DNFPUTDL OWNER=PKUB;
LIBNAME DNFPUTDM DB2 DATABASE=DNFPUTDL OWNER=PKUS;

/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

FILENAME REPORT2 "&RPTLIB/UNWT01.NWT01R2";
FILENAME REPORT3 "&RPTLIB/UNWT01.NWT01R3";
FILENAME REPORT4 "&RPTLIB/UNWT01.NWT01R4";
FILENAME REPORT5 "&RPTLIB/UNWT01.NWT01R5";
FILENAME REPORT6 "&RPTLIB/UNWT01.NWT01R6";
FILENAME REPORT7 "&RPTLIB/UNWT01.NWT01R7";
FILENAME REPORT8 "&RPTLIB/UNWT01.ndr";
FILENAME REPORT9 "&RPTLIB/UNWT01.NWT01R9";
FILENAME REPORT10 "&RPTLIB/UNWT01.NWT01R10";
FILENAME REPORT11 "&RPTLIB/UNWT01.NWT01R11";
FILENAME REPORT12 "&RPTLIB/UNWT01.NWT01R12";
FILENAME REPORT13 "&RPTLIB/UNWT01.NWT01R13";
FILENAME REPORT14 "&RPTLIB/UNWT01.NWT01R14";
FILENAME REPORT15 "&RPTLIB/UNWT01.NWT01R15";
FILENAME REPORT16 "&RPTLIB/UNWT01.NWT01R16";
FILENAME REPORT17 "&RPTLIB/UNWT01.NWT01R17";
FILENAME REPORTZ "&RPTLIB/UNWT01.NWT01RZ";

DATA _NULL_;
	CALL SYMPUTX('RUN_DT',"'"||PUT(INTNX('DAY',TODAY(),0,'BEGINNING'), MMDDYY10.)||"'");
	CALL SYMPUTX('cPREV_BEG',"'"||PUT(INTNX('MONTH',TODAY(),-1,'BEGINNING'), MMDDYY10.)||"'");
	CALL SYMPUTX('cPREV_END',"'"||PUT(INTNX('MONTH',TODAY(),-1,'END'), MMDDYY10.)||"'");
	CALL SYMPUTX('TITLE_DT',PUT(INTNX('MONTH',TODAY(),-1,'END'), MMDDYY10.));
	CALL SYMPUT('PREV_BEG',INTNX('MONTH',TODAY(),-1,'BEGINNING'));
	CALL SYMPUT('PREV_END',INTNX('MONTH',TODAY(),-1,'END'));
	CALL SYMPUT('PREV_YR_BEG',INTNX('YEAR',TODAY(),-1,'BEGINNING'));
	CALL SYMPUT('PREV_YR_END',INTNX('YEAR',TODAY(),-1,'END'));
RUN;

%SYSLPUT RUN_DT = &RUN_DT;
%SYSLPUT cPREV_BEG = &cPREV_BEG;
%SYSLPUT cPREV_END = &cPREV_END;
%SYSLPUT PREV_BEG = &PREV_BEG;
%SYSLPUT PREV_END = &PREV_END;
%SYSLPUT PREV_YR_BEG = &PREV_YR_BEG;
%SYSLPUT PREV_YR_END = &PREV_YR_END;

LIBNAME  WORKLOCL  REMOTE  SERVER=LEGEND  SLIBREF=WORK;
RSUBMIT;

/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
%let DB = DNFPUTDL;  *This is live;


LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;

PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE INIT_POP AS
		SELECT *
		FROM 
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						LN10.BF_SSN,
						LN10.LN_SEQ,
						LN10.BF_SSN||CHAR(LN10.LN_SEQ) AS LID,
						PAYMENT.LN_FAT_SEQ,
						PAYMENT.TRX_TYP,
						PAYMENT.LD_FAT_EFF,
						PAYMENT.LD_FAT_APL,
						PAYMENT.LD_FAT_PST,
						COALESCE(LN37.LA_OTS_PRI_ELG,0) AS LA_OTS_PRI_ELG,
						COALESCE(LN37.LA_NSI_ACR,0) AS LA_NSI_ACR,
						COALESCE(LN37.LA_OTS_LTE_FEE,0) AS LA_OTS_LTE_FEE,
						COALESCE(PAYMENT.LA_FAT_CUR_PRI,0) AS LA_FAT_CUR_PRI,
						COALESCE(PAYMENT.LA_FAT_NSI,0) AS LA_FAT_NSI,
						COALESCE(PAYMENT.LA_FAT_LTE_FEE,0) AS LA_FAT_LTE_FEE,
						LN80.LD_BIL_CRT,
						COALESCE(LN80.LA_BIL_CUR_DU,0) + COALESCE(LN80.LA_BIL_PAS_DU,0) AS TOT_LA_BIL_DU,
						COALESCE(LN80.LA_TOT_BIL_STS,0) AS LA_TOT_BIL_STS,
						LN80.LD_BIL_DU_LON,
						LN80.LA_BIL_CUR_DU,
						LN80.LA_BIL_PAS_DU AS LA_BIL_PAS_DU_IP,
						DELQ.NUM_DAYS_DELQ,
						DW01.WC_DW_LON_STA,
						LN10.LC_WOF_WUP_REA,
						LN10.LC_FED_PGM_YR,
						CASE 
							WHEN AY10_30.BF_SSN IS NULL THEN 0
							ELSE 1
						END AS DFLT_SNT_30,
						CASE 
							WHEN AY10_125.BF_SSN IS NULL THEN 0
							ELSE 1
						END AS DFLT_SNT_125,
						COALESCE(MR65.WD_INC_CRD_RPT_IRS,CURRENT_DATE) AS WD_INC_CRD_RPT_IRS,
						COALESCE(MR65.WA_PRI_INC_CRD,0) AS WA_PRI_INC_CRD,
						COALESCE(MR65.WA_INT_INC_CRD,0) AS WA_INT_INC_CRD,
						CASE
							WHEN COALESCE(MR5A.WN_DAY_DLQ_ISL,0) >= 181 AND MR5A.WC_LON_SUB_STA = '21' THEN 2
							WHEN COALESCE(MR5A.WN_DAY_DLQ_ISL,0) >= 121 AND MR5A.WC_LON_SUB_STA = '21' THEN 1
							WHEN COALESCE(MR5A.WN_DAY_DLQ_ISL,0) > 270 AND (COALESCE(LN37.LA_OTS_PRI_ELG,0) + COALESCE(LN37.LA_NSI_ACR,0) +	COALESCE(LN37.LA_OTS_LTE_FEE,0)) < 25 THEN 3
							WHEN COALESCE(MR5A.WN_DAY_DLQ_ISL,0) > 270 AND MR5A.BF_SSN IS NOT NULL AND SUBSTR(MR5A.BF_SSN,1,1) = '9' THEN 4
							WHEN COALESCE(MR5A.WN_DAY_DLQ_ISL,0) > 270 AND (PD24.DN_BKR_ADS IS NOT NULL AND PD24.DN_BKR_ADS != '') THEN 5
							WHEN COALESCE(MR5A.WN_DAY_DLQ_ISL,0) > 270 AND MR5A.WC_LON_SUB_STA = '05' AND MR5A.LC_FOR_TYP IN ('01','04') AND DAYS(CURRENT_DATE) BETWEEN DAYS(MR5A.LD_FOR_BEG) AND DAYS(MR5A.LD_FOR_END) THEN 6
							ELSE 0
						END AS TOP_HEIRARCHY
					FROM 
						PKUB.LN10_LON LN10
						LEFT OUTER JOIN
						(
							SELECT 
								BF_SSN,
								LN_SEQ,
								LN_FAT_SEQ,
								PC_FAT_TYP||PC_FAT_SUB_TYP AS TRX_TYP,
								LD_FAT_EFF,
								LD_FAT_APL,
								LD_FAT_PST,
								LA_FAT_CUR_PRI,
								LA_FAT_NSI,
								LA_FAT_LTE_FEE
							FROM
								PKUB.LN90_FIN_ATY 
							WHERE 
								LC_FAT_REV_REA = ''
								AND LC_STA_LON90 = 'A'
								AND 
								(
									(PC_FAT_TYP = '02' AND PC_FAT_SUB_TYP IN ('90','91')) 
									OR (PC_FAT_TYP IN ('50','60') AND PC_FAT_SUB_TYP IN ('02','03')) 
									OR (PC_FAT_TYP = '10')
								) 
						) PAYMENT
							ON LN10.BF_SSN = PAYMENT.BF_SSN
							AND LN10.LN_SEQ = PAYMENT.LN_SEQ
						LEFT OUTER JOIN 
						(
							SELECT 
								BF_SSN,
								LN_SEQ,
								DAYS(CURRENT DATE) - DAYS(LD_DLQ_OCC) - 5 AS NUM_DAYS_DELQ
							FROM 
								PKUB.LN16_LON_DLQ_HST
							WHERE 
								LC_STA_LON16 = '1'
								AND LC_DLQ_TYP = 'P'
						) DELQ
							ON LN10.BF_SSN = DELQ.BF_SSN 
							AND LN10.LN_SEQ = DELQ.LN_SEQ
						LEFT OUTER JOIN PKUB.DW01_DW_CLC_CLU DW01
							ON LN10.BF_SSN = DW01.BF_SSN
							AND LN10.LN_SEQ = DW01.LN_SEQ
						LEFT OUTER JOIN PKUS.LN37_LON_MTH_BAL LN37
							ON LN10.BF_SSN = LN37.BF_SSN
							AND LN10.LN_SEQ = LN37.LN_SEQ
							AND LN37.LC_STA_LON37 = 'A'
							AND LN37.LD_EFF_MTH_BAL = &cPREV_END
						LEFT OUTER JOIN PKUB.LN80_LON_BIL_CRF LN80
							ON LN10.BF_SSN = LN80.BF_SSN
							AND LN10.LN_SEQ = LN80.LN_SEQ
							AND LN80.LC_STA_LON80 = 'A'
							AND LN80.LD_BIL_CRT BETWEEN &cPREV_BEG AND &cPREV_END
							AND DAYS(LN80.LD_BIL_CRT) > DAYS(LN80.LD_BIL_DU_LON) - 30
						LEFT OUTER JOIN PKUB.AY10_BR_LON_ATY AY10_30
							ON LN10.BF_SSN = AY10_30.BF_SSN
							AND AY10_30.PF_REQ_ACT = 'DLFDL'
							AND AY10_30.PF_RSP_ACT = 'PRNTD'
							AND DAYS(AY10_30.LD_ATY_RSP) >= DAYS(CURRENT_DATE) - 30
						LEFT OUTER JOIN PKUB.AY10_BR_LON_ATY AY10_125
							ON LN10.BF_SSN = AY10_125.BF_SSN
							AND AY10_125.PF_REQ_ACT = 'DLFDL'
							AND AY10_125.PF_RSP_ACT = 'PRNTD'
							AND DAYS(AY10_125.LD_ATY_RSP) >= DAYS(CURRENT_DATE) - 125
						LEFT OUTER JOIN PKUB.MR65_MSC_TAX_RPT MR65
							ON LN10.BF_SSN = MR65.BF_SSN
							AND LN10.LN_SEQ = MR65.LN_SEQ
						LEFT OUTER JOIN
						(
							SELECT
								M.*
							FROM
								PKUR.MR5A_LON_MTH_SSH_12 M
								INNER JOIN
								(
									SELECT
										BF_SSN,
										LN_SEQ,
										MAX(WD_CRT_MR5A) AS WD_CRT_MR5A
									FROM
										PKUR.MR5A_LON_MTH_SSH_12
									GROUP BY
										BF_SSN,
										LN_SEQ		
								)MS
									ON M.BF_SSN = MS.BF_SSN
									AND M.LN_SEQ = MS.LN_SEQ
									AND M.WD_CRT_MR5A = MS.WD_CRT_MR5A
						) MR5A
							ON MR5A.BF_SSN = LN10.BF_SSN
							AND MR5A.LN_SEQ = LN10.LN_SEQ
						LEFT OUTER JOIN PKUB.PD24_PRS_BKR PD24
							ON PD24.DF_PRS_ID = LN10.BF_SSN
					WHERE 
						LN10.LD_LON_ACL_ADD <= CURRENT DATE 
						AND LN10.LA_CUR_PRI > 0
						
					FOR READ ONLY WITH UR
				) X
	;

	DISCONNECT FROM DB2;

/*	get a distinct list of loans and bills to be used by next query*/
	CREATE TABLE LNS AS
		SELECT DISTINCT
			BF_SSN,
			LN_SEQ,
			LD_BIL_CRT
		FROM
			INIT_POP
	;

/*	get payments between bill create date and previous month end*/
	CREATE TABLE PMTS AS
		SELECT DISTINCT
			LNS.BF_SSN,
			LNS.LN_SEQ,
			SUM(COALESCE(LN90.LA_FAT_CUR_PRI,0) + COALESCE(LN90.LA_FAT_NSI,0) + COALESCE(LN90.LA_FAT_LTE_FEE,0)) AS PMTS
		FROM
			LNS
			LEFT JOIN PKUB.LN90_FIN_ATY	LN90
				ON LNS.BF_SSN = LN90.BF_SSN
				AND LNS.LN_SEQ = LN90.LN_SEQ
				AND LN90.LD_FAT_PST BETWEEN LNS.LD_BIL_CRT AND &PREV_END
				AND LN90.PC_FAT_TYP = '10'
		GROUP BY
			LNS.BF_SSN,
			LNS.LN_SEQ
	;

/*	calculate past due amount and add it to data from initial population*/
	CREATE TABLE TRORFED AS
		SELECT
			IP.*,
			CASE
				WHEN IP.LD_BIL_DU_LON < &PREV_END AND SUM(IP.TOT_LA_BIL_DU,PMTS.PMTS) <= 0 THEN 0 /*LN90.PMTS is negative so the amount gets subtracted from the total amount due on the bill*/
				WHEN IP.LD_BIL_DU_LON < &PREV_END AND SUM(IP.TOT_LA_BIL_DU,PMTS.PMTS) > 0 THEN SUM(IP.TOT_LA_BIL_DU,PMTS.PMTS)
				WHEN COALESCE(IP.LA_BIL_PAS_DU_IP,0) - COALESCE(IP.LA_TOT_BIL_STS,0) <= 0 THEN 0
				ELSE COALESCE(IP.LA_BIL_PAS_DU_IP,0) - COALESCE(IP.LA_TOT_BIL_STS,0)
			END AS LA_BIL_PAS_DU  
		FROM
			INIT_POP IP
			JOIN PMTS
				ON IP.BF_SSN = PMTS.BF_SSN
				AND IP.LN_SEQ = PMTS.LN_SEQ
		;
QUIT;

ENDRSUBMIT;

/*LIBNAME FILE 'C:\SERF_FILE\';*/
/*DATA TRORFED; SET FILE.TRORFED; RUN;*/

DATA TRORFED; SET WORKLOCL.TRORFED; RUN;

DATA SPACER;
	RPT_ROW = 0.5; OUTPUT;
	RPT_ROW = 1.5; OUTPUT;
	RPT_ROW = 3.25; OUTPUT;
	RPT_ROW = 3.5; OUTPUT;
	RPT_ROW = 13.25; OUTPUT;
	RPT_ROW = 13.5; OUTPUT;
	RPT_ROW = 13.75; OUTPUT;
	RPT_ROW = 15.25; OUTPUT;
	RPT_ROW = 15.5; OUTPUT;
	RPT_ROW = 16.5; OUTPUT;
/*see note (1)*/
/*	RPT_ROW = 11; OUTPUT;*/
RUN;
OPTIONS MISSING=' ';

PROC FORMAT;
	VALUE fCAT 
		.5 = 'Section A'
		1.5 = '(6) Amounts Written Off'
		3='(B) Written Off and Closed Out'
		3.25 = 'Section C'
		3.5 = '(1) Delinquencies by Age'
		4='(A) 1-30 Days'
		5='(B) 31-60 Days'
		6='(C) 61-90 Days'
		7='(D) 91-120 Days'
		8='(E) 121-150 Days'
		9='(F) 151-180 Days'
		10='(G) 181-270 Days'
		11='(H) 271-365 Days'
		11.5='(I) 1-2 Years'
		12='(J) 2-6 Years'
		13='(L) Total Delinquency by Age'
		13.25 = 'Part II - Debt Management Tool'
		13.5 = 'Section A'
		13.75 = '(1) Delinquencies 1-120 Days'
		14='(A) In Bankruptcy'
		15.25 = 'Section C'
		15.5 = '(1) Collections on Delinquent Debt'
		16='(D) By Third Party'
		16.5='Part III TOP'
		17='Debt Balance < $25'
		17.5='Invalid SSN'
		18='Adversary Proceedings'
		18.5='Active Military'
		19='Bankruptcy 121+'
		19.5='Bankruptcy 181+';

/*see note (1)*/
/*		11='Section E'*/
/*		12='(1) Debts Closed out During the Previous Calendar Year'*/
/*		13='(A) Reported to IRS on Form 1099-C';*/
RUN;

TITLE;
FOOTNOTE;

%MACRO CR8_TROR_FILES(FED_PGM,RNO);
	/*get data for the loan program being processed*/
	DATA RDAT;
		SET TRORFED;
		WHERE LC_FED_PGM_YR = "&FED_PGM";
	RUN;

	PROC SQL;

	/*	get a distinct list of loan level data for those reports that need it (RDAT is at the LN90 transaction level, some items need loan level data)*/
		CREATE TABLE RDAT_LN AS
			SELECT DISTINCT
				RD.LID,
				RD.LA_BIL_PAS_DU,
				RD.NUM_DAYS_DELQ,
				RD.LA_OTS_PRI_ELG,
				RD.LA_NSI_ACR,
				RD.LA_OTS_LTE_FEE,
				RD.DFLT_SNT_30,
				RD.DFLT_SNT_125,
				WC_DW_LON_STA,
				WA_PRI_INC_CRD,
				WA_INT_INC_CRD,
				WD_INC_CRD_RPT_IRS,
				TOP_HEIRARCHY
			FROM
				RDAT RD
		;

	/*this section gets data that needs to be stored separately for populating data for items 11 and 12*/	
		/*	get data for items 3 and 4, (A) 1-90 Days*/
		CREATE TABLE AGDAT_3_4 AS 
			SELECT
				4 AS RPT_ROW
				,COUNT(DISTINCT RD.LID) AS Number
				,SUM(RD.LA_BIL_PAS_DU) AS Dollars
			FROM 
				RDAT_LN RD
			WHERE 
				1 <= RD.NUM_DAYS_DELQ <= 30
				AND SUM(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR) > 0
				AND RD.LA_BIL_PAS_DU > 0
		;

		CREATE TABLE AGDAT_4_5 AS 
			SELECT
				5 AS RPT_ROW
				,COUNT(DISTINCT RD.LID) AS Number
				,SUM(RD.LA_BIL_PAS_DU) AS Dollars
			FROM 
				RDAT_LN RD
			WHERE 
				31 <= RD.NUM_DAYS_DELQ <= 60
				AND SUM(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR) > 0
				AND RD.LA_BIL_PAS_DU > 0
		;

		CREATE TABLE AGDAT_5_6 AS 
			SELECT
				6 AS RPT_ROW
				,COUNT(DISTINCT RD.LID) AS Number
				,SUM(RD.LA_BIL_PAS_DU) AS Dollars
			FROM 
				RDAT_LN RD
			WHERE 
				61 <= RD.NUM_DAYS_DELQ <= 90
				AND SUM(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR) > 0
				AND RD.LA_BIL_PAS_DU > 0
		;

		CREATE TABLE AGDAT_6_7 AS 
			SELECT
				7 AS RPT_ROW
				,COUNT(DISTINCT RD.LID) AS Number
				,SUM(RD.LA_BIL_PAS_DU) AS Dollars
			FROM 
				RDAT_LN RD
			WHERE 
				91 <= RD.NUM_DAYS_DELQ <= 120
				AND SUM(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR) > 0
				AND RD.LA_BIL_PAS_DU > 0
		;

		CREATE TABLE AGDAT_7_8 AS 
			SELECT
				8 AS RPT_ROW
				,COUNT(DISTINCT RD.LID) AS Number
				,SUM(RD.LA_BIL_PAS_DU) AS Dollars
			FROM 
				RDAT_LN RD
			WHERE 
				121 <= RD.NUM_DAYS_DELQ <= 150
				AND SUM(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR) > 0
				AND RD.LA_BIL_PAS_DU > 0
		;

		CREATE TABLE AGDAT_8_9 AS 
			SELECT
				9 AS RPT_ROW
				,COUNT(DISTINCT RD.LID) AS Number
				,SUM(RD.LA_BIL_PAS_DU) AS Dollars
			FROM 
				RDAT_LN RD
			WHERE 
				151 <= RD.NUM_DAYS_DELQ <= 180
				AND SUM(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR) > 0
				AND RD.LA_BIL_PAS_DU > 0
		;

		CREATE TABLE AGDAT_9_10 AS 
			SELECT
				10 AS RPT_ROW
				,COUNT(DISTINCT RD.LID) AS Number
				,SUM(RD.LA_BIL_PAS_DU) AS Dollars
			FROM 
				RDAT_LN RD
			WHERE 
				180 <= RD.NUM_DAYS_DELQ <= 270
				AND SUM(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR) > 0
				AND RD.LA_BIL_PAS_DU > 0
		;

		/*	get data for items 7 and 8, (C) 181-365 Days*/
		CREATE TABLE DAT_10_11 AS
			SELECT
				11 AS RPT_ROW
				,RD.LID
				,CASE 
					WHEN RD.DFLT_SNT_125 = 0 THEN RD.LA_BIL_PAS_DU
					ELSE COALESCE(SUM(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR,RD.LA_OTS_LTE_FEE),0)
				 END AS Dollars
			FROM 
				RDAT_LN RD
			WHERE 
				271 <= RD.NUM_DAYS_DELQ <= 365
				AND SUM(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR) > 0
				AND 
					(
						(RD.DFLT_SNT_125 = 0 AND RD.LA_BIL_PAS_DU > 0)
						OR
						COALESCE(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR,RD.LA_OTS_LTE_FEE,0) > 0
					)
		;

		CREATE TABLE AGDAT_10_11 AS 
			SELECT
				RPT_ROW
				,COUNT(DISTINCT LID) AS Number
				,COALESCE(SUM(Dollars),0) AS Dollars
			FROM 
				DAT_10_11
		;

		/*	get data for items 9 and 10, (D) 1-2 Years*/
		CREATE TABLE AGDAT_11_12 AS 
			SELECT
				11.5 AS RPT_ROW
				,COUNT(DISTINCT RD.LID) AS Number
				,COALESCE(SUM(RD.LA_OTS_PRI_ELG + RD.LA_NSI_ACR + RD.LA_OTS_LTE_FEE),0) AS Dollars
			FROM 
				RDAT_LN RD
			WHERE 
				366 <= RD.NUM_DAYS_DELQ <= 730
				AND SUM(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR) > 0
				AND COALESCE(RD.LA_OTS_PRI_ELG + RD.LA_NSI_ACR + RD.LA_OTS_LTE_FEE,0) > 0
		;

		CREATE TABLE AGDAT_12_13 AS 
			SELECT
				12 AS RPT_ROW
				,COUNT(DISTINCT RD.LID) AS Number
				,COALESCE(SUM(RD.LA_OTS_PRI_ELG + RD.LA_NSI_ACR + RD.LA_OTS_LTE_FEE),0) AS Dollars
			FROM 
				RDAT_LN RD
			WHERE 
				731 <= RD.NUM_DAYS_DELQ <= 2190
				AND SUM(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR) > 0
				AND COALESCE(RD.LA_OTS_PRI_ELG + RD.LA_NSI_ACR + RD.LA_OTS_LTE_FEE,0) > 0
		;

		/*	get data for items 11 and 12, (E) Total Delinquency by Age*/
		CREATE TABLE AGDAT_13_14 AS 
			SELECT
				13 AS RPT_ROW
				,COALESCE(SUM(Number),0) AS Number
				,COALESCE(SUM(Dollars),0) AS Dollars
			FROM
				(
					SELECT * FROM AGDAT_3_4
					UNION
					SELECT * FROM AGDAT_4_5
					UNION
					SELECT * FROM AGDAT_5_6
					UNION
					SELECT * FROM AGDAT_6_7
					UNION
					SELECT * FROM AGDAT_7_8
					UNION
					SELECT * FROM AGDAT_8_9
					UNION
					SELECT * FROM AGDAT_9_10
					UNION
					SELECT * FROM AGDAT_10_11
					UNION
					SELECT * FROM AGDAT_11_12
					UNION
					SELECT * FROM AGDAT_12_13
					
				)
		;

	/*aggregate data for all items*/
		/*	get data for items 1 and 2, (B) Written Off and Closed Out*/
		CREATE TABLE AGDAT AS 
			SELECT
				3 AS RPT_ROW
				,COUNT(DISTINCT RD.LID) AS Number
				,COALESCE(SUM(RD.LA_FAT_CUR_PRI + RD.LA_FAT_NSI + RD.LA_FAT_LTE_FEE),0) AS Dollars
			FROM 
				RDAT RD
				LEFT JOIN
				(
					SELECT
						 LID
					FROM
						RDAT
					WHERE
						LC_WOF_WUP_REA = 'Z'
						AND SUM(LA_FAT_CUR_PRI,LA_FAT_NSI) >= 25
						AND TRX_TYP IN ('5002','5003')
				) EX
					ON RD.LID = EX.LID
			WHERE 
				RD.TRX_TYP IN ('5002','5003','6002','6003')
				AND &PREV_BEG <= RD.LD_FAT_PST <= &PREV_END
				AND EX.LID IS NULL
		UNION
		/*	get data for items 3 and 4, (A) 1-30 Days*/
			SELECT
				*
			FROM 
				AGDAT_3_4
		UNION
		/*	get data for items 3 and 4, (A) 31-60 Days*/
			SELECT
				*
			FROM 
				AGDAT_4_5
		UNION
		/*	get data for items 3 and 4, (A) 61-91 Days*/
			SELECT
				*
			FROM 
				AGDAT_5_6

		UNION
		/*	get data for items 3 and 4, (A) 91-120 Days*/
			SELECT
				*
			FROM 
				AGDAT_6_7
		UNION
		/*	get data for items 3 and 4, (A) 121-150 Days*/
			SELECT
				*
			FROM 
				AGDAT_7_8
		UNION
		/*	get data for items 3 and 4, (A) 151-180 Days*/
			SELECT
				*
			FROM 
				AGDAT_8_9
		UNION
		/*	get data for items 3 and 4, (A) 181-270 Days*/
			SELECT
				*
			FROM 
				AGDAT_9_10

		UNION
		/*	get data for items 3 and 4, (A) 271-365 Days*/
			SELECT
				*
			FROM 
				AGDAT_10_11
		UNION
		/*	get data for items 3 and 4, (A) 1-2 Years*/
			SELECT
				*
			FROM 
				AGDAT_11_12
		UNION
			/*	get data for items 3 and 4, (A) 2-6 Years*/
			SELECT
				*
			FROM 
				AGDAT_12_13
		
		UNION
		/*	get data for items 11 and 12, (E) Total Delinquency by Age*/
			SELECT
				*
			FROM 
				AGDAT_13_14
		UNION
		/*	get data for items 13 and 14, (A) In Bankruptcy*/
			SELECT
				14 AS RPT_ROW
				,COUNT(DISTINCT RD.LID) AS Number
				,COALESCE(SUM(RD.LA_BIL_PAS_DU),0) AS Dollars
			FROM 
				RDAT_LN RD
			WHERE 
				1 <= RD.NUM_DAYS_DELQ <= 120
				AND RD.WC_DW_LON_STA = '21'
				AND SUM(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR) > 0
				AND RD.LA_BIL_PAS_DU > 0
		UNION
		/*	get data for items 15 and 16, (D) By Third Party*/
			SELECT
				16 AS RPT_ROW,
				COUNT(DISTINCT RD.LID) AS Number,
				COALESCE(SUM(RD.LA_FAT_CUR_PRI + RD.LA_FAT_NSI + RD.LA_FAT_LTE_FEE),0) AS Dollars
			FROM 
				RDAT RD
			WHERE 
				RD.NUM_DAYS_DELQ > 0
				AND SUBSTR(RD.TRX_TYP,1,2) = '10'
				AND &PREV_BEG <= RD.LD_FAT_APL <= &PREV_END
				AND SUM(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR) > 0
	  	UNION
	  		SELECT
				17 AS RPT_ROW,
				COUNT(DISTINCT RD.LID) AS Number,
				COALESCE(SUM(RD.LA_OTS_PRI_ELG + RD.LA_NSI_ACR + RD.LA_OTS_LTE_FEE),0) AS Dollars
			FROM 
				RDAT_LN RD
				WHERE TOP_HEIRARCHY = 3
		UNION
			SELECT
				17.5 AS RPT_ROW,
				COUNT(DISTINCT RD.LID) AS Number,
				COALESCE(SUM(RD.LA_OTS_PRI_ELG + RD.LA_NSI_ACR + RD.LA_OTS_LTE_FEE),0) AS Dollars
			FROM 
				RDAT_LN RD
				WHERE TOP_HEIRARCHY = 4
		UNION
			SELECT
				18 AS RPT_ROW,
				COUNT(DISTINCT RD.LID) AS Number,
				COALESCE(SUM(RD.LA_OTS_PRI_ELG + RD.LA_NSI_ACR + RD.LA_OTS_LTE_FEE),0) AS Dollars
			FROM 
				RDAT_LN RD
				WHERE TOP_HEIRARCHY = 5
		UNION
			SELECT
				18.5 AS RPT_ROW,
				COUNT(DISTINCT RD.LID) AS Number,
				COALESCE(SUM(RD.LA_OTS_PRI_ELG + RD.LA_NSI_ACR + RD.LA_OTS_LTE_FEE),0) AS Dollars
			FROM 
				RDAT_LN RD
				WHERE TOP_HEIRARCHY = 6
		UNION
			SELECT
				19 AS RPT_ROW,
				COUNT(DISTINCT RD.LID) AS Number,
				COALESCE(SUM(RD.LA_OTS_PRI_ELG + RD.LA_NSI_ACR + RD.LA_OTS_LTE_FEE),0) AS Dollars
			FROM 
				RDAT_LN RD
				WHERE TOP_HEIRARCHY in (1,2)
		UNION
			SELECT
				19.5 AS RPT_ROW,
				COUNT(DISTINCT RD.LID) AS Number,
				COALESCE(SUM(RD.LA_OTS_PRI_ELG + RD.LA_NSI_ACR + RD.LA_OTS_LTE_FEE),0) AS Dollars
			FROM 
				RDAT_LN RD
				WHERE TOP_HEIRARCHY = 2

/*see note (1)*/
/*		UNION*/
/*		/*	get data for items 17 and 18, Debts Closed out During the Previous Calendar Year*/
/*			SELECT*/
/*				12 AS RPT_ROW*/
/*				,COUNT(DISTINCT RD.LID) AS Number*/
/*				,COALESCE(SUM(RD.LA_FAT_CUR_PRI + RD.LA_FAT_NSI + RD.LA_FAT_LTE_FEE),0) AS Dollars*/
/*			FROM */
/*				RDAT RD*/
/*				LEFT JOIN*/
/*					(*/
/*						SELECT*/
/*							 LID*/
/*						FROM*/
/*							RDAT*/
/*						WHERE*/
/*							LC_WOF_WUP_REA = 'Z'*/
/*							AND SUM(LA_FAT_CUR_PRI,LA_FAT_NSI) >= 25*/
/*							AND TRX_TYP IN ('5002','5003')*/
/*					) EX*/
/*					ON RD.LID = EX.LID*/
/*			WHERE */
/*				RD.TRX_TYP IN ('5002','5003','6002','6003')*/
/*				AND &PREV_YR_BEG <= RD.LD_FAT_PST <= &PREV_YR_END*/
/*				AND SUM(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR) = 0*/
/*				AND EX.LID IS NULL*/
/*		UNION*/
/*		/*	get data for items 19 and 20, (A) Reported to IRS on Form 1099-C*/
/*			SELECT*/
/*				13 AS RPT_ROW*/
/*				,COUNT(DISTINCT RD.LID) AS Number*/
/*				,COALESCE(SUM(RD.WA_PRI_INC_CRD + RD.WA_INT_INC_CRD),0) AS Dollars*/
/*			FROM */
/*				RDAT_LN RD*/
/*			WHERE */
/*				&PREV_YR_BEG <= RD.WD_INC_CRD_RPT_IRS <= &PREV_YR_END*/
		;

	QUIT;

	/*add header and subheader records to data records */
	DATA AGDAT;
		SET AGDAT SPACER;
		DisplayGrp = RPT_ROW;
	RUN;

	/*sort so rows are in the correct order for printing*/
	PROC SORT DATA=AGDAT;
		BY RPT_ROW;
	RUN;

	DATA AGDAT;
		SET AGDAT;
		RPT_ROW = _N_;
	RUN;

	TITLE1;
	TITLE2;
	TITLE3;
	ODS LISTING CLOSE;
	ODS TAGSETS.MSOFFICE2K STYLE=MINIMAL FILE=REPORT&RNO;
	ODS ESCAPECHAR='^';
	ODS TEXT="^S={font=('TIMES NEW ROMAN',14pt,Bold) just=center}CornerStone Treasury Report on Receivables, Date: &TITLE_DT, Loan Program = &FED_PGM ";
	PROC REPORT NOWD DATA=AGDAT
		STYLE(HEADER)=[FONT=("TIMES NEW ROMAN",12PT,BOLD ITALIC) JUST=LEFT BACKGROUND=BLACK FOREGROUND=WHITE] ;
		COLUMN RPT_ROW DISPLAYGRP Number Dollars;
		DEFINE RPT_ROW / ORDER NOPRINT ;
		DEFINE DISPLAYGRP / DISPLAY FORMAT=fCAT. 'Part I - Status of Receivables' 
			STYLE(COLUMN)=[FONT=("TIMES NEW ROMAN",12PT,ITALIC) JUST=LEFT CELLWIDTH=300PT];; 
		DEFINE Number / DISPLAY LEFT FORMAT=COMMA20.;
		DEFINE Dollars / DISPLAY LEFT FORMAT=COMMA22.2;
		COMPUTE RPT_ROW;
			IF RPT_ROW IN (17,24) THEN DO;
				CALL DEFINE(_ROW_,"STYLE","STYLE=[FONT=('TIMES NEW ROMAN',12PT,BOLD ITALIC) BACKGROUND=BLACK FOREGROUND=WHITE]" );
			END;
		ENDCOMP;
		COMPUTE Number;
			IF Number=' ' THEN
			CALL DEFINE(_COL_,"STYLE","STYLE=[BACKGROUND=BLACK]" );
		ENDCOMP;
		COMPUTE Dollars;
			IF Dollars=' ' THEN
			CALL DEFINE(_COL_,"STYLE","STYLE=[BACKGROUND=BLACK]" );
		ENDCOMP;
	RUN;

	ODS  TAGSETS.MSOFFICE2K CLOSE;
	ODS LISTING;

	PROC DATASETS;
		DELETE AGDAT;
	QUIT;
%MEND CR8_TROR_FILES;

%CR8_TROR_FILES(DLO,2);
%CR8_TROR_FILES(FAF,3);
%CR8_TROR_FILES(FAL,4);
%CR8_TROR_FILES(FBR,5);
%CR8_TROR_FILES(FCD,6);
%CR8_TROR_FILES(FCO,7);
%CR8_TROR_FILES(FIS,8);
%CR8_TROR_FILES(GAF,9);
%CR8_TROR_FILES(GAL,10);
%CR8_TROR_FILES(LNC,11);
%CR8_TROR_FILES(LP1,12);
%CR8_TROR_FILES(LP2,13);
%CR8_TROR_FILES(PI1,14);
%CR8_TROR_FILES(PI2,15);
%CR8_TROR_FILES(TPL,16);
%CR8_TROR_FILES(UNP,17);

OPTIONS MISSING='.';
/*Detail File*/
/*PROC EXPORT DATA= TRORFED */
/*            OUTFILE= "T:\SAS\TROR_Detail.CSV" */
/*            REPLACE;*/
/*RUN;*/

/*note (1)*/
/*The request for this information was removed from SASR 3761 at the last minute because the information */
/*was not what was expected and there is no time to fix it and they have another way of getting the information.*/
/*However, the plan is to submit a new request for the information so this code will be needed later.	See the*/
/*e-mail message attached to SASR 3761 for more information*/
