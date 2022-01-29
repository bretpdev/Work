%MACRO CRX_TROR_FILES(FED_PGM,RNO);
	/*get data for the loan program being processed*/
	DATA RDAT;
		SET T.TRORFED;
		WHERE LC_FED_PGM_YR = "&FED_PGM";
	RUN;

	PROC SQL;
	/*	get a distinct list of loan level data for those reports that need it (RDAT is at the LNXX transaction level, some items need loan level data)*/
		CREATE TABLE RDAT_LN AS
			SELECT DISTINCT
				RD.LID,
				RD.LA_BIL_PAS_DU,
				RD.NUM_DAYS_DELQ,
				RD.LA_OTS_PRI_ELG,
				RD.LA_NSI_ACR,
				RD.LA_OTS_LTE_FEE,
				RD.DFLT_SNT,
				WC_DW_LON_STA,
				WA_PRI_INC_CRD,
				WA_INT_INC_CRD,
				WD_INC_CRD_RPT_IRS
			FROM
				RDAT RD
		;

	/*this section gets data that needs to be stored separately for populating data for items XX and XX*/	
		/*	get data for items X and X, (A) X-XX Days*/
		CREATE TABLE AGDAT_X_X AS 
			SELECT
/*				DISTINCT LID*/
				'(A) X-XX Days' AS ROW,
				RD.*,
				RD.LA_BIL_PAS_DU AS Dollars
			FROM 
				RDAT_LN RD
			WHERE 
				X <= RD.NUM_DAYS_DELQ <= XX
				AND SUM(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR) > X
				AND RD.LA_BIL_PAS_DU > X
		;

		/*	get data for items X and X, (B) XX-XXX Days*/
		CREATE TABLE AGDAT_X_X AS 
			SELECT
/*				DISTINCT LID*/
				'(B) XX-XXX Days' AS ROW,
				RD.*,
				RD.LA_BIL_PAS_DU AS Dollars
			FROM 
				RDAT_LN RD
			WHERE 
				XX <= RD.NUM_DAYS_DELQ <= XXX
				AND SUM(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR) > X
				AND RD.LA_BIL_PAS_DU > X
		;

		/*	get data for items X and X, (C) XXX-XXX Days*/
		CREATE TABLE AGDAT_X_X AS
			SELECT
/*				DISTINCT LID*/
				'(C) XXX-XXX Days' AS ROW,
				RD.*,
				CASE 
					WHEN RD.DFLT_SNT = X THEN RD.LA_BIL_PAS_DU
/*					ELSE COALESCE(SUM(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR,RD.LA_OTS_LTE_FEE),X)*/
					ELSE COALESCE(RD.LA_OTS_PRI_ELG + RD.LA_NSI_ACR + RD.LA_OTS_LTE_FEE,X)
				 END AS Dollars
			FROM 
				RDAT_LN RD
			WHERE 
				XXX <= RD.NUM_DAYS_DELQ <= XXX
				AND SUM(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR) > X
				AND 
					(
						(RD.DFLT_SNT = X AND RD.LA_BIL_PAS_DU > X)
						OR
/*						COALESCE(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR,RD.LA_OTS_LTE_FEE,X) > X*/
						COALESCE(RD.LA_OTS_PRI_ELG + RD.LA_NSI_ACR + RD.LA_OTS_LTE_FEE,X) > X
					)
		;

		/*	get data for items X and XX, (D) X-X Years*/
		CREATE TABLE AGDAT_X_XX AS 
			SELECT
/*				DISTINCT LID*/
				'(D) X-X Years' AS ROW,
				RD.*,
				COALESCE(RD.LA_OTS_PRI_ELG + RD.LA_NSI_ACR + RD.LA_OTS_LTE_FEE,X) AS Dollars
			FROM 
				RDAT_LN RD
			WHERE 
				XXX <= RD.NUM_DAYS_DELQ <= XXX
				AND SUM(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR) > X
				AND COALESCE(RD.LA_OTS_PRI_ELG + RD.LA_NSI_ACR + RD.LA_OTS_LTE_FEE,X) > X
		;

	QUIT;

	%MACRO TO_FILE(DS,SHT);
		PROC EXPORT
				DATA=&DS
				OUTFILE="T:\SAS\TROR_DET_&FED_PGM..XLSX"
				REPLACE;
			SHEET=&SHT;
		RUN;
	%MEND;

	%TO_FILE(AGDAT_X_X,A);
	%TO_FILE(AGDAT_X_X,B);
	%TO_FILE(AGDAT_X_X,C);
	%TO_FILE(AGDAT_X_XX,D);

%MEND CRX_TROR_FILES;

%CRX_TROR_FILES(DLO,X);
/*%CRX_TROR_FILES(FAF,X);*/
/*%CRX_TROR_FILES(FAL,X);*/
/*%CRX_TROR_FILES(FBR,X);*/
/*%CRX_TROR_FILES(FCD,X);*/
/*%CRX_TROR_FILES(FCO,X);*/
/*%CRX_TROR_FILES(FIS,X);*/
/*%CRX_TROR_FILES(GAF,X);*/
/*%CRX_TROR_FILES(GAL,XX);*/
/*%CRX_TROR_FILES(LNC,XX);*/
/*%CRX_TROR_FILES(LPX,XX);*/
/*%CRX_TROR_FILES(LPX,XX);*/
/*%CRX_TROR_FILES(PIX,XX);*/
/*%CRX_TROR_FILES(PIX,XX);*/
/*%CRX_TROR_FILES(TPL,XX);*/
/*%CRX_TROR_FILES(UNP,XX);*/
