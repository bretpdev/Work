/*LIBNAME DNFPUTDL DBX DATABASE=DNFPUTDL OWNER=PKUB;*/
/*LIBNAME DNFPUTDM DBX DATABASE=DNFPUTDL OWNER=PKUS;*/

/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

FILENAME REPORTX "&RPTLIB/UNWTXX.NWTXXRX";
FILENAME REPORTX "&RPTLIB/UNWTXX.NWTXXRX";
FILENAME REPORTX "&RPTLIB/UNWTXX.NWTXXRX";
FILENAME REPORTX "&RPTLIB/UNWTXX.NWTXXRX";
FILENAME REPORTX "&RPTLIB/UNWTXX.NWTXXRX";
FILENAME REPORTX "&RPTLIB/UNWTXX.NWTXXRX";
FILENAME REPORTX "&RPTLIB/UNWTXX.NWTXXRX";
FILENAME REPORTX "&RPTLIB/UNWTXX.NWTXXRX";
FILENAME REPORTXX "&RPTLIB/UNWTXX.NWTXXRXX";
FILENAME REPORTXX "&RPTLIB/UNWTXX.NWTXXRXX";
FILENAME REPORTXX "&RPTLIB/UNWTXX.NWTXXRXX";
FILENAME REPORTXX "&RPTLIB/UNWTXX.NWTXXRXX";
FILENAME REPORTXX "&RPTLIB/UNWTXX.NWTXXRXX";
FILENAME REPORTXX "&RPTLIB/UNWTXX.NWTXXRXX";
FILENAME REPORTXX "&RPTLIB/UNWTXX.NWTXXRXX";
FILENAME REPORTXX "&RPTLIB/UNWTXX.NWTXXRXX";
FILENAME REPORTZ "&RPTLIB/UNWTXX.NWTXXRZ";

DATA _NULL_;
	CALL SYMPUTX('RUN_DT',"'"||PUT(INTNX('DAY',TODAY(),X,'beginning'), MMDDYYXX.)||"'");
	CALL SYMPUTX('cPREV_BEG',"'"||PUT(INTNX('MONTH',TODAY(),-X,'BEGINNING'), MMDDYYXX.)||"'");
	CALL SYMPUTX('cPREV_END',"'"||PUT(INTNX('MONTH',TODAY(),-X,'END'), MMDDYYXX.)||"'");
	CALL SYMPUTX('TITLE_DT',PUT(INTNX('MONTH',TODAY(),-X,'END'), MMDDYYXX.));
	CALL SYMPUT('PREV_BEG',INTNX('MONTH',TODAY(),-X,'BEGINNING'));
	CALL SYMPUT('PREV_END',INTNX('MONTH',TODAY(),-X,'END'));
	CALL SYMPUT('PREV_YR_BEG',INTNX('YEAR',TODAY(),-X,'BEGINNING'));
	CALL SYMPUT('PREV_YR_END',INTNX('YEAR',TODAY(),-X,'END'));
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
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;

PROC SQL;
	CONNECT TO DBX (DATABASE=DNFPUTDL);

	CREATE TABLE INIT_POP AS
		SELECT *
		FROM 
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						A.BF_SSN
						,A.LN_SEQ
						,A.BF_SSN||CHAR(A.LN_SEQ) AS LID
						,B.LN_FAT_SEQ
						,B.TRX_TYP
						,B.LD_FAT_EFF
						,B.LD_FAT_APL
						,B.LD_FAT_PST
						,COALESCE(E.LA_OTS_PRI_ELG,X) AS LA_OTS_PRI_ELG
						,COALESCE(E.LA_NSI_ACR,X) AS LA_NSI_ACR
						,COALESCE(E.LA_OTS_LTE_FEE,X) AS LA_OTS_LTE_FEE
						,COALESCE(B.LA_FAT_CUR_PRI,X) AS LA_FAT_CUR_PRI
						,COALESCE(B.LA_FAT_NSI,X) AS LA_FAT_NSI
						,COALESCE(B.LA_FAT_LTE_FEE,X) AS LA_FAT_LTE_FEE
						,LNXX.LD_BIL_CRT
						,COALESCE(LNXX.LA_BIL_CUR_DU,X) + COALESCE(LNXX.LA_BIL_PAS_DU,X) AS TOT_LA_BIL_DU
						,COALESCE(LNXX.LA_TOT_BIL_STS,X) AS LA_TOT_BIL_STS
						,LNXX.LD_BIL_DU_LON
						,LNXX.LA_BIL_CUR_DU
						,LNXX.LA_BIL_PAS_DU AS LA_BIL_PAS_DU_IP
/*						,CASE*/
/*							WHEN COALESCE(LNXX.LA_BIL_PAS_DU,X) - COALESCE(LNXX.LA_TOT_BIL_STS,X) <= X THEN X*/
/*							ELSE COALESCE(LNXX.LA_BIL_PAS_DU,X) - COALESCE(LNXX.LA_TOT_BIL_STS,X)*/
/*						 END AS LA_BIL_PAS_DU*/
						,C.NUM_DAYS_DELQ
						,C.LD_DLQ_OCC /*??*/
						,C.LN_DLQ_MAX /*??*/
						,D.WC_DW_LON_STA
						,A.LC_WOF_WUP_REA
						,A.LC_FED_PGM_YR
						,CASE 
							WHEN AYXX.BF_SSN IS NULL THEN X
							ELSE X
						 END AS DFLT_SNT
						,COALESCE(MRXX.WD_INC_CRD_RPT_IRS,CURRENT_DATE) AS WD_INC_CRD_RPT_IRS
						,COALESCE(MRXX.WA_PRI_INC_CRD,X) AS WA_PRI_INC_CRD
						,COALESCE(MRXX.WA_INT_INC_CRD,X) AS WA_INT_INC_CRD
					FROM 
						PKUB.LNXX_LON A
						LEFT OUTER JOIN
							(
								SELECT 
									BF_SSN
									,LN_SEQ
									,LN_FAT_SEQ
									,PC_FAT_TYP||PC_FAT_SUB_TYP AS TRX_TYP
									,LD_FAT_EFF
									,LD_FAT_APL
									,LD_FAT_PST
									,LA_FAT_CUR_PRI
									,LA_FAT_NSI
									,LA_FAT_LTE_FEE
								FROM
									PKUB.LNXX_FIN_ATY 
								WHERE 
									LC_FAT_REV_REA = ''
									AND LC_STA_LONXX = 'A'
									AND 
									(
										(PC_FAT_TYP = 'XX' AND PC_FAT_SUB_TYP IN ('XX','XX')) OR
										(PC_FAT_TYP IN ('XX','XX') AND PC_FAT_SUB_TYP IN ('XX','XX')) OR
										(PC_FAT_TYP = 'XX')
									) 
							) B
							ON A.BF_SSN = B.BF_SSN
							AND A.LN_SEQ = B.LN_SEQ
						LEFT OUTER JOIN 
							(
								SELECT 
									BF_SSN
									,LN_SEQ
									,DAYS(CURRENT DATE) - DAYS(LD_DLQ_OCC) - X AS NUM_DAYS_DELQ
									,LD_DLQ_OCC /*??*/
									,LN_DLQ_MAX /*??*/
								FROM 
									PKUB.LNXX_LON_DLQ_HST
								WHERE 
									LC_STA_LONXX = 'X'
									AND LC_DLQ_TYP = 'P'
							) C
							ON A.BF_SSN = C.BF_SSN 
							AND A.LN_SEQ = C.LN_SEQ
						LEFT OUTER JOIN PKUB.DWXX_DW_CLC_CLU D
							ON A.BF_SSN = D.BF_SSN
							AND A.LN_SEQ = D.LN_SEQ
						LEFT OUTER JOIN PKUS.LNXX_LON_MTH_BAL E
							ON A.BF_SSN = E.BF_SSN
							AND A.LN_SEQ = E.LN_SEQ
							AND E.LC_STA_LONXX = 'A'
							AND E.LD_EFF_MTH_BAL = &cPREV_END
						LEFT OUTER JOIN PKUB.LNXX_LON_BIL_CRF LNXX
							ON A.BF_SSN = LNXX.BF_SSN
							AND A.LN_SEQ = LNXX.LN_SEQ
							AND LNXX.LC_STA_LONXX = 'A'
							AND LNXX.LD_BIL_CRT BETWEEN &cPREV_BEG AND &cPREV_END
							AND DAYS(LNXX.LD_BIL_CRT) > DAYS(LNXX.LD_BIL_DU_LON) - XX
						LEFT OUTER JOIN PKUB.AYXX_BR_LON_ATY AYXX
							ON A.BF_SSN = AYXX.BF_SSN
							AND AYXX.PF_RSP_ACT = 'DLFDL'
							AND DAYS(AYXX.LD_ATY_RSP) >= DAYS(CURRENT_DATE) - XX
						LEFT OUTER JOIN PKUB.MRXX_MSC_TAX_RPT MRXX
							ON A.BF_SSN = MRXX.BF_SSN
							AND A.LN_SEQ = MRXX.LN_SEQ
					WHERE 
						A.LD_LON_ACL_ADD <= CURRENT DATE 
						AND A.LA_CUR_PRI > X
/*						AND A.BF_SSN IN ('XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX')*/
/*						AND A.BF_SSN IN ('XXXXXXXXX','XXXXXXXXX')*/
						AND A.BF_SSN IN
							(
								'XXXXXXXXX',
								'XXXXXXXXX',
								'XXXXXXXXX',
								'XXXXXXXXX',
								'XXXXXXXXX',
								'XXXXXXXXX',
								'XXXXXXXXX',
								'XXXXXXXXX',
								'XXXXXXXXX',
								'XXXXXXXXX',
								'XXXXXXXXX',
								'XXXXXXXXX',
								'XXXXXXXXX',
								'XXXXXXXXX',
								'XXXXXXXXX'
							)

					FOR READ ONLY WITH UR
				) X 
	;

	DISCONNECT FROM DBX;

	CREATE TABLE LNS AS
		SELECT DISTINCT
			BF_SSN,
			LN_SEQ,
			LD_BIL_CRT
		FROM
			INIT_POP
	;

	CREATE TABLE PMTS AS
		SELECT DISTINCT
			LNS.BF_SSN,
			LNS.LN_SEQ,
			SUM(COALESCE(LNXX.LA_FAT_CUR_PRI,X) + COALESCE(LNXX.LA_FAT_NSI,X) + COALESCE(LNXX.LA_FAT_LTE_FEE,X)) AS PMTS
/*COALESCE(LNXX.LA_FAT_CUR_PRI,X) + COALESCE(LNXX.LA_FAT_NSI,X) + COALESCE(LNXX.LA_FAT_LTE_FEE,X) AS PMTS,*/
/*LNXX.LA_FAT_CUR_PRI AS PLA_FAT_CUR_PRI,*/
/*LNXX.LA_FAT_NSI AS PPLA_FAT_NSI,*/
/*LNXX.LA_FAT_LTE_FEE AS PLA_FAT_LTE_FEE,*/
/*LNXX.LD_FAT_PST AS PLD_FAT_PST*/
		FROM
			LNS
			LEFT JOIN PKUB.LNXX_FIN_ATY	LNXX
				ON LNS.BF_SSN = LNXX.BF_SSN
				AND LNS.LN_SEQ = LNXX.LN_SEQ
				AND LNXX.LD_FAT_PST BETWEEN LNS.LD_BIL_CRT AND &PREV_END
				AND LNXX.PC_FAT_TYP = 'XX'
/*				AND LNXX.PC_FAT_SUB_TYP*/
		GROUP BY
			LNS.BF_SSN,
			LNS.LN_SEQ
	;

	CREATE TABLE TRORFED AS
		SELECT
			IP.*,
			CASE
/*				WHEN IP.LD_BIL_DU_LON < &PREV_END THEN SUM(IP.TOT_LA_BIL_DU,PMTS.PMTS)*/
				WHEN IP.LD_BIL_DU_LON < &PREV_END AND SUM(IP.TOT_LA_BIL_DU,PMTS.PMTS) <= X THEN X /*LNXX.PMTS is negative so the amount gets subtracted from the total amount due on the bill*/
				WHEN IP.LD_BIL_DU_LON < &PREV_END AND SUM(IP.TOT_LA_BIL_DU,PMTS.PMTS) > X THEN SUM(IP.TOT_LA_BIL_DU,PMTS.PMTS)
				WHEN COALESCE(IP.LA_BIL_PAS_DU_IP,X) - COALESCE(IP.LA_TOT_BIL_STS,X) <= X THEN X
				ELSE COALESCE(IP.LA_BIL_PAS_DU_IP,X) - COALESCE(IP.LA_TOT_BIL_STS,X)
			END AS LA_BIL_PAS_DU  
		FROM
			INIT_POP IP
			JOIN PMTS
				ON IP.BF_SSN = PMTS.BF_SSN
				AND IP.LN_SEQ = PMTS.LN_SEQ
	;

QUIT;

ENDRSUBMIT;

/*DATA INIT_POP; SET WORKLOCL.INIT_POP; RUN;*/
DATA PMTS; SET WORKLOCL.PMTS; RUN;

DATA TRORFED; SET WORKLOCL.TRORFED; RUN;



DATA SPACER;
	RPT_ROW = X.X; OUTPUT;
	RPT_ROW = X.X; OUTPUT;
	RPT_ROW = X.XX; OUTPUT;
	RPT_ROW = X.X; OUTPUT;
	RPT_ROW = X.XX; OUTPUT;
	RPT_ROW = X.X; OUTPUT;
	RPT_ROW = X.XX; OUTPUT;
	RPT_ROW = X.XX; OUTPUT;
	RPT_ROW = X.X; OUTPUT;
/*see note (X)*/
/*	RPT_ROW = XX; OUTPUT;*/
RUN;
OPTIONS MISSING=' ';

PROC FORMAT;
	VALUE fCAT 
		.X = 'Section A'
		X.X = '(X) Amounts Written Off'
		X='(B) Written Off and Closed Out'
		X.XX = 'Section C'
		X.X = '(X) Delinquencies by Age'
		X='(A) X-XX Days'
		X='(B) XX-XXX Days'
		X='(C) XXX-XXX Days'
		X.X='(D) X-X Years'
		X='(E) Total Delinquency by Age'
		X.XX = 'Part II - Debt Management Tool'
		X.X = 'Section A'
		X.XX = '(X) Delinquencies X-XXX Days'
		X='(A) In Bankruptcy'
		X.XX = 'Section C'
		X.X = '(X) Collections on Delinquent Debt'
		XX='(D) By Third Party'
/*see note (X)*/
/*		XX='Section E'*/
/*		XX='(X) Debts Closed out During the Previous Calendar Year'*/
/*		XX='(A) Reported to IRS on Form XXXX-C';*/
RUN;

TITLE;
FOOTNOTE;

%MACRO CRX_TROR_FILES(FED_PGM,RNO);
	/*get data for the loan program being processed*/
	DATA RDAT;
		SET TRORFED;
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
				X AS RPT_ROW
				,COUNT(DISTINCT RD.LID) AS Number
				,SUM(RD.LA_BIL_PAS_DU) AS Dollars
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
				X AS RPT_ROW
				,COUNT(DISTINCT RD.LID) AS Number
				,SUM(RD.LA_BIL_PAS_DU) AS Dollars
			FROM 
				RDAT_LN RD
			WHERE 
				XX <= RD.NUM_DAYS_DELQ <= XXX
				AND SUM(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR) > X
				AND RD.LA_BIL_PAS_DU > X
		;

		/*	get data for items X and X, (C) XXX-XXX Days*/
		CREATE TABLE DAT_X_X AS
			SELECT
				X AS RPT_ROW
				,RD.LID
				,CASE 
					WHEN RD.DFLT_SNT = X THEN RD.LA_BIL_PAS_DU
					ELSE COALESCE(SUM(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR,RD.LA_OTS_LTE_FEE),X)
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
						COALESCE(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR,RD.LA_OTS_LTE_FEE,X) > X
					)
		;

		CREATE TABLE AGDAT_X_X AS 
			SELECT
				RPT_ROW
				,COUNT(DISTINCT LID) AS Number
				,COALESCE(SUM(Dollars),X) AS Dollars
			FROM 
				DAT_X_X
		;

		/*	get data for items X and XX, (D) X-X Years*/
		CREATE TABLE AGDAT_X_XX AS 
			SELECT
				X.X AS RPT_ROW
				,COUNT(DISTINCT RD.LID) AS Number
				,COALESCE(SUM(RD.LA_OTS_PRI_ELG + RD.LA_NSI_ACR + RD.LA_OTS_LTE_FEE),X) AS Dollars
			FROM 
				RDAT_LN RD
			WHERE 
				XXX <= RD.NUM_DAYS_DELQ <= XXX
				AND SUM(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR) > X
				AND COALESCE(RD.LA_OTS_PRI_ELG + RD.LA_NSI_ACR + RD.LA_OTS_LTE_FEE,X) > X
		;

		/*	get data for items XX and XX, (E) Total Delinquency by Age*/
		CREATE TABLE AGDAT_XX_XX AS 
			SELECT
				X AS RPT_ROW
				,COALESCE(SUM(Number),X) AS Number
				,COALESCE(SUM(Dollars),X) AS Dollars
			FROM
				(
					SELECT * FROM AGDAT_X_X
					UNION
					SELECT * FROM AGDAT_X_X
					UNION
					SELECT * FROM AGDAT_X_X
					UNION
					SELECT * FROM AGDAT_X_XX
				)
		;

	/*aggregate data for all items*/
		/*	get data for items X and X, (B) Written Off and Closed Out*/
		CREATE TABLE AGDAT AS 
			SELECT
				X AS RPT_ROW
				,COUNT(DISTINCT RD.LID) AS Number
				,COALESCE(SUM(RD.LA_FAT_CUR_PRI + RD.LA_FAT_NSI + RD.LA_FAT_LTE_FEE),X) AS Dollars
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
							AND SUM(LA_FAT_CUR_PRI,LA_FAT_NSI) >= XX
							AND TRX_TYP IN ('XXXX','XXXX')
					) EX
					ON RD.LID = EX.LID
			WHERE 
				RD.TRX_TYP IN ('XXXX','XXXX','XXXX','XXXX')
				AND &PREV_BEG <= RD.LD_FAT_PST <= &PREV_END
				AND EX.LID IS NULL
		UNION
		/*	get data for items X and X, (A) X-XX Days*/
			SELECT
				*
			FROM 
				AGDAT_X_X
		UNION
		/*	get data for items X and X, (B) XX-XXX Days*/
			SELECT
				*
			FROM 
				AGDAT_X_X
		UNION
		/*	get data for items X and X, (C) XXX-XXX Days*/
			SELECT
				*
			FROM 
				AGDAT_X_X
		UNION
		/*	get data for items X and XX, (D) X-X Years*/
			SELECT
				*
			FROM 
				AGDAT_X_XX
		UNION
		/*	get data for items XX and XX, (E) Total Delinquency by Age*/
			SELECT
				*
			FROM 
				AGDAT_XX_XX
		UNION
		/*	get data for items XX and XX, (A) In Bankruptcy*/
			SELECT
				X AS RPT_ROW
				,COUNT(DISTINCT RD.LID) AS Number
				,COALESCE(SUM(RD.LA_BIL_PAS_DU),X) AS Dollars
			FROM 
				RDAT_LN RD
			WHERE 
				X <= RD.NUM_DAYS_DELQ <= XXX
				AND RD.WC_DW_LON_STA = 'XX'
				AND SUM(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR) > X
				AND RD.LA_BIL_PAS_DU > X
		UNION
		/*	get data for items XX and XX, (D) By Third Party*/
			SELECT
				XX AS RPT_ROW
				,COUNT(DISTINCT RD.LID) AS Number
				,COALESCE(SUM(RD.LA_FAT_CUR_PRI + RD.LA_FAT_NSI + RD.LA_FAT_LTE_FEE),X) AS Dollars
			FROM 
				RDAT RD
			WHERE 
				RD.NUM_DAYS_DELQ > X
				AND SUBSTR(RD.TRX_TYP,X,X) = 'XX'
				AND &PREV_BEG <= RD.LD_FAT_APL <= &PREV_END
				AND SUM(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR) > X
/*see note (X)*/
/*		UNION*/
/*		/*	get data for items XX and XX, Debts Closed out During the Previous Calendar Year*/
/*			SELECT*/
/*				XX AS RPT_ROW*/
/*				,COUNT(DISTINCT RD.LID) AS Number*/
/*				,COALESCE(SUM(RD.LA_FAT_CUR_PRI + RD.LA_FAT_NSI + RD.LA_FAT_LTE_FEE),X) AS Dollars*/
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
/*							AND SUM(LA_FAT_CUR_PRI,LA_FAT_NSI) >= XX*/
/*							AND TRX_TYP IN ('XXXX','XXXX')*/
/*					) EX*/
/*					ON RD.LID = EX.LID*/
/*			WHERE */
/*				RD.TRX_TYP IN ('XXXX','XXXX','XXXX','XXXX')*/
/*				AND &PREV_YR_BEG <= RD.LD_FAT_PST <= &PREV_YR_END*/
/*				AND SUM(RD.LA_OTS_PRI_ELG,RD.LA_NSI_ACR) = X*/
/*				AND EX.LID IS NULL*/
/*		UNION*/
/*		/*	get data for items XX and XX, (A) Reported to IRS on Form XXXX-C*/
/*			SELECT*/
/*				XX AS RPT_ROW*/
/*				,COUNT(DISTINCT RD.LID) AS Number*/
/*				,COALESCE(SUM(RD.WA_PRI_INC_CRD + RD.WA_INT_INC_CRD),X) AS Dollars*/
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

	TITLEX;
	TITLEX;
	TITLEX;
	ODS LISTING CLOSE;
	ODS TAGSETS.MSOFFICEXK STYLE=MINIMAL FILE=REPORT&RNO;
	ODS ESCAPECHAR='^';
	ODS TEXT="^S={font=('TIMES NEW ROMAN',XXpt,Bold) just=center}CornerStone Treasury Report on Receivables, Date: &TITLE_DT, Loan Program = &FED_PGM ";
	PROC REPORT NOWD DATA=AGDAT
		STYLE(HEADER)=[FONT=("TIMES NEW ROMAN",XXPT,BOLD ITALIC) JUST=LEFT BACKGROUND=BLACK FOREGROUND=WHITE] ;
		COLUMN RPT_ROW DISPLAYGRP Number Dollars;
		DEFINE RPT_ROW / ORDER NOPRINT ;
		DEFINE DISPLAYGRP / DISPLAY FORMAT=fCAT. 'Part I - Status of Receivables' 
			STYLE(COLUMN)=[FONT=("TIMES NEW ROMAN",XXPT,ITALIC) JUST=LEFT CELLWIDTH=XXXPT];; 
		DEFINE Number / DISPLAY LEFT FORMAT=COMMAXX.;
		DEFINE Dollars / DISPLAY LEFT FORMAT=COMMAXX.X;
		COMPUTE RPT_ROW;
			IF RPT_ROW=XX THEN DO;
				CALL DEFINE(_ROW_,"STYLE","STYLE=[FONT=('TIMES NEW ROMAN',XXPT,BOLD ITALIC) BACKGROUND=BLACK FOREGROUND=WHITE]" );
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

	ODS  TAGSETS.MSOFFICEXK CLOSE;
	ODS LISTING;

	PROC DATASETS;
		DELETE AGDAT;
	QUIT;
%MEND CRX_TROR_FILES;

%CRX_TROR_FILES(DLO,X);
%CRX_TROR_FILES(FAF,X);
%CRX_TROR_FILES(FAL,X);
%CRX_TROR_FILES(FBR,X);
%CRX_TROR_FILES(FCD,X);
%CRX_TROR_FILES(FCO,X);
%CRX_TROR_FILES(FIS,X);
%CRX_TROR_FILES(GAF,X);
%CRX_TROR_FILES(GAL,XX);
%CRX_TROR_FILES(LNC,XX);
%CRX_TROR_FILES(LPX,XX);
%CRX_TROR_FILES(LPX,XX);
%CRX_TROR_FILES(PIX,XX);
%CRX_TROR_FILES(PIX,XX);
%CRX_TROR_FILES(TPL,XX);
%CRX_TROR_FILES(UNP,XX);

OPTIONS MISSING='.';
/*Detail File*/
/*PROC EXPORT DATA= TRORFED */
/*            OUTFILE= "T:\SAS\TROR_Detail.xls" */
/*            DBMS=EXCELXXXX REPLACE;*/
/*RUN;*/

/*note (X)*/
/*The request for this information was removed from SASR XXXX at the last minute because the information */
/*was not what was expected and there is no time to fix it and they have another way of getting the information.*/
/*However, the plan is to submit a new request for the information so this code will be needed later.	See the*/
/*e-mail message attached to SASR XXXX for more information*/
