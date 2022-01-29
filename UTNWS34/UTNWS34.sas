/*%LET RPTLIB = %SYSGET(reportdir);*/
/*LIBNAME SAS_TAB V8 '/sas/whse/progrevw';*/
LIBNAME SAS_TAB V8 'Y:\Development\SAS Test Files\progrevw';
%LET RPTLIB = T:\SAS;

FILENAME REPORTZ "&RPTLIB/UNWS34.NWS34RZ";
FILENAME REPORT2 "&RPTLIB/UNWS34.NWS34R2";
FILENAME REPORT3 "&RPTLIB/UNWS34.NWS34R3";
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND  SLIBREF=WORK;

PROC SQL;
	CREATE TABLE CURRENT_CAMPAIGN_BORROWERS AS
		SELECT
			BORR.DF_SPE_ACC_ID
		FROM
			SAS_TAB.CAMPAIGNS CAMP
			JOIN SAS_TAB.CAMPAIGN_BORROWERS BORR
				ON CAMP.CAMPAIGN_ID = BORR.CAMPAIGN_ID
		WHERE
			TODAY() BETWEEN BEGIN_DATE AND END_DATE
			AND CAMP.CAMPAIGN_NAME LIKE '%Late Stage Resolution'
	;
QUIT;

DATA LEGEND.CURRENT_CAMPAIGN_BORROWERS; SET CURRENT_CAMPAIGN_BORROWERS; RUN;
RSUBMIT;

LIBNAME PKUB DB2 DATABASE=DNFPUTDL OWNER=PKUB;

PROC SQL;
	CREATE TABLE S34_DS1 AS
		SELECT DISTINCT 
			LN10.BF_SSN,
			TRIM(PD10.DM_PRS_1) || ', ' || PD10.DM_PRS_LST AS BOR_NAME,
			CASE
				WHEN COALESCE(LN16.LN_DLQ_MAX,0) = 0 THEN 0
				ELSE COALESCE(LN16.LN_DLQ_MAX,0) + 1 
			END AS LN_DLQ_MAX,
			CASE
				WHEN HPHN.DC_ALW_ADL_PHN IN ('L','Q','P') THEN 'Y'
				WHEN HPHN.DC_ALW_ADL_PHN IN ('N','U','X') THEN 'N'
				ELSE ''
			END AS PHN_H_CON,
			HPHN.DN_DOM_PHN_ARA||HPHN.DN_DOM_PHN_XCH||HPHN.DN_DOM_PHN_LCL AS PHN_H,
			CASE
				WHEN APHN.DC_ALW_ADL_PHN IN ('L','Q','P') THEN 'Y'
				WHEN APHN.DC_ALW_ADL_PHN IN ('N','U','X') THEN 'N'
				ELSE ''
			END AS PHN_A_CON,
			APHN.DN_DOM_PHN_ARA||APHN.DN_DOM_PHN_XCH||APHN.DN_DOM_PHN_LCL AS PHN_A,
			CASE
				WHEN WPHN.DC_ALW_ADL_PHN IN ('L','Q','P') THEN 'Y'
				WHEN WPHN.DC_ALW_ADL_PHN IN ('N','U','X') THEN 'N'
				ELSE ''
			END AS PHN_W_CON,
			WPHN.DN_DOM_PHN_ARA||WPHN.DN_DOM_PHN_XCH||WPHN.DN_DOM_PHN_LCL AS PHN_W,
			PD30.DX_STR_ADR_1,
			PD30.DX_STR_ADR_2,
			PD30.DX_STR_ADR_3,
			PD30.DM_CT,
			PD30.DC_DOM_ST,
			PD30.DF_ZIP_CDE,
			LN10.LF_LON_CUR_OWN,
			BAL.LA_CUR_PRI
		FROM
			CURRENT_CAMPAIGN_BORROWERS CCBR
			JOIN PKUB.PD10_PRS_NME PD10
				ON CCBR.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
			JOIN PKUB.LN10_LON LN10
				ON PD10.DF_PRS_ID = LN10.BF_SSN
				AND LN10.LA_CUR_PRI > 0
				AND LN10.LC_STA_LON10 = 'R'
			JOIN PKUB.DW01_DW_CLC_CLU DW01
				ON LN10.BF_SSN = DW01.BF_SSN
				AND LN10.LN_SEQ = DW01.LN_SEQ
				AND DW01.WC_DW_LON_STA NOT IN ('16','17','18','19','20','21')
			LEFT JOIN (
					SELECT
						LN16.BF_SSN,
						MAX(LN16.LN_DLQ_MAX) AS LN_DLQ_MAX
					FROM PKUB.LN16_LON_DLQ_HST LN16
					WHERE LN16.LC_STA_LON16 = '1'
					GROUP BY LN16.BF_SSN
						) LN16
				ON LN10.BF_SSN = LN16.BF_SSN
/*				AND LN10.LN_SEQ = LN16.LN_SEQ*/
/*				AND LN16.LC_STA_LON16 = '1'*/
			LEFT JOIN PKUB.PD40_PRS_PHN HPHN
				ON PD10.DF_PRS_ID = HPHN.DF_PRS_ID
				AND HPHN.DC_PHN = 'H'
				AND HPHN.DI_PHN_VLD = 'Y'
			LEFT JOIN PKUB.PD40_PRS_PHN APHN
				ON PD10.DF_PRS_ID = APHN.DF_PRS_ID
				AND APHN.DC_PHN = 'A'
				AND APHN.DI_PHN_VLD = 'Y'
			LEFT JOIN PKUB.PD40_PRS_PHN WPHN
				ON PD10.DF_PRS_ID = WPHN.DF_PRS_ID
				AND WPHN.DC_PHN = 'W'
				AND WPHN.DI_PHN_VLD = 'Y'
			LEFT JOIN PKUB.PD30_PRS_ADR PD30
				ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
				AND PD30.DC_ADR = 'L'
			INNER JOIN 
				(
					SELECT 
						LN10.BF_SSN,
						LN10.LF_LON_CUR_OWN,
						100 * (SUM(LN10.LA_CUR_PRI) + SUM(DW01.LA_NSI_ACR)) /** 100*/ AS LA_CUR_PRI
					FROM
						PKUB.LN10_LON LN10
						INNER JOIN PKUB.DW01_DW_CLC_CLU DW01
							ON LN10.BF_SSN = DW01.BF_SSN
							AND LN10.LN_SEQ = DW01.LN_SEQ
					WHERE 
						LN10.LC_STA_LON10 = 'R'
						AND LN10.LA_CUR_PRI > 0
					GROUP BY 
						LN10.BF_SSN,
						LN10.LF_LON_CUR_OWN
				) BAL
				ON LN10.BF_SSN = BAL.BF_SSN
		WHERE 
			CALCULATED PHN_H <> ''
			OR CALCULATED PHN_A <> ''
			OR CALCULATED PHN_W <> ''
	;

	CREATE TABLE S34 AS
		SELECT DISTINCT 
			DS1.*
			,COALESCE(DUE.AMOUNT_DUE,0) * 100 AS AMOUNT_DUE
		FROM
			S34_DS1 DS1
			LEFT OUTER JOIN
				(
					SELECT 
						LN80.BF_SSN,
						SUM(COALESCE(LN80.LA_BIL_PAS_DU,0) + COALESCE(LN80.LA_BIL_CUR_DU,0) + COALESCE(LN80.LA_LTE_FEE_OTS_PRT,0)) AS AMOUNT_DUE
					FROM 
						PKUB.LN80_LON_BIL_CRF LN80
						INNER JOIN 
							(
								SELECT 
									BIL_SEQ.BF_SSN,
									BIL_SEQ.LD_BIL_CRT,
									MAX(BIL_SEQ.LN_SEQ_BIL_WI_DTE) AS LN_SEQ_BIL_WI_DTE
								FROM
									PKUB.LN80_LON_BIL_CRF BIL_SEQ
									INNER JOIN 
										(
											SELECT 
												BF_SSN,
												MAX(LD_BIL_CRT) AS LD_BIL_CRT
											FROM
												PKUB.LN80_LON_BIL_CRF 
											WHERE 
												LC_STA_LON80 = 'A'
												AND LC_BIL_TYP_LON = 'P'
												AND LD_BIL_CRT > TODAY() - 31
											GROUP BY BF_SSN
										) BIL_DT
										ON BIL_DT.BF_SSN = BIL_SEQ.BF_SSN
										AND BIL_DT.LD_BIL_CRT = BIL_SEQ.LD_BIL_CRT
								WHERE 
									BIL_SEQ.LC_STA_LON80 = 'A'
									AND BIL_SEQ.LC_BIL_TYP_LON = 'P'
								GROUP BY 
									BIL_SEQ.BF_SSN,
									BIL_SEQ.LD_BIL_CRT
							) THE_BILL
							ON LN80.BF_SSN = THE_BILL.BF_SSN
							AND LN80.LD_BIL_CRT = THE_BILL.LD_BIL_CRT
							AND LN80.LN_SEQ_BIL_WI_DTE = THE_BILL.LN_SEQ_BIL_WI_DTE
						LEFT JOIN PKUB.LN10_LON LN10
							ON LN80.BF_SSN = LN10.BF_SSN
							AND LN80.LN_SEQ = LN10.LN_SEQ
						WHERE 
							LN80.LC_STA_LON80 = 'A'
							AND LN10.LA_CUR_PRI > 0
							AND LN10.LC_STA_LON10 = 'R'
						GROUP BY 
							LN80.BF_SSN
				) DUE
				ON DS1.BF_SSN = DUE.BF_SSN
	;
QUIT;

ENDRSUBMIT;

DATA S34; SET LEGEND.S34; RUN;

DATA _NULL_;
SET S34;
FILE REPORT2 DROPOVER LRECL=32767;
DO;
	PUT @1 BF_SSN   			
		@15 BOR_NAME
		@70 LN_DLQ_MAX
		@159 PHN_H_CON
		@160 PHN_H
		@175 PHN_A_CON
		@176 PHN_A
		@191 PHN_W_CON
		@192 PHN_W
		@260 DX_STR_ADR_1
		@290 DX_STR_ADR_2
		@320 DX_STR_ADR_3
		@350 DM_CT
		@370 DC_DOM_ST
		@372 DF_ZIP_CDE
		@414 LF_LON_CUR_OWN
		@422 AMOUNT_DUE
		@429 LA_CUR_PRI;	
	END;
RUN;

DATA _NULL_;
	SET S34;
	FILE REPORT3 DELIMITER=',' DSD DROPOVER LRECL=32767;

	IF _N_ = 1 THEN
		DO;
			PUT 'SSN,Name,Days Delinquent,Consent for Home Phone,Home Phone,Consent for Alternate Phone,Alternate Phone,Consent for Work Phone,Work Phone,
				Street 1,Street 2,Street 3,City,State,Zip Code,Owner Code,Amount Due,Total balance of all loans';
		END;

	DO;
		PUT BF_SSN @;  			
		PUT BOR_NAME @;
		PUT LN_DLQ_MAX @;
		PUT PHN_H_CON @;
		PUT PHN_H @;
		PUT PHN_A_CON @;
		PUT PHN_A @;
		PUT PHN_W_CON @;
		PUT PHN_W @;
		PUT DX_STR_ADR_1 @;
		PUT DX_STR_ADR_2 @;
		PUT DX_STR_ADR_3 @;
		PUT DM_CT @;
		PUT DC_DOM_ST @;
		PUT DF_ZIP_CDE @;
		PUT LF_LON_CUR_OWN @;
		PUT AMOUNT_DUE @;
		PUT LA_CUR_PRI $;
	END;
RUN;
