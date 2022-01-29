/*%LET RPTLIB = %SYSGET(reportdir);*/
/*LIBNAME SAS_TAB V8 '/sas/whse/progrevw';*/
LIBNAME SAS_TAB V8 'Y:\Development\SAS Test Files\progrevw';
%LET RPTLIB = T:\SAS;

FILENAME REPORTZ "&RPTLIB/UNWS76.NWS76RZ";
FILENAME REPORT2 "&RPTLIB/UNWS76.NWS76R2";
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
			AND CAMP.CAMPAIGN_NAME LIKE '%CR 2651%'
			AND CAMP.CAMPAIGN_NAME NE 'CR 2651 Control'
	;
QUIT;

DATA LEGEND.CURRENT_CAMPAIGN_BORROWERS; SET CURRENT_CAMPAIGN_BORROWERS; RUN;

RSUBMIT;
LIBNAME PKUB DB2 DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE S76 AS
		SELECT DISTINCT
			PD10.DF_SPE_ACC_ID,
			CATX(' ',PD10.DM_PRS_1,PD10.DM_PRS_LST) AS NAME,
			PD10.DD_BRT,
			CATX(' ',PD30.DX_STR_ADR_1, PD30.DX_STR_ADR_2, PD30.DM_CT, PD30.DC_DOM_ST, PD30.DF_ZIP_CDE) AS ADDRESS,
			PD30.DM_FGN_CNY,
			PD30.DI_VLD_ADR,
			CATX('',PD4H.DN_DOM_PHN_ARA,PD4H.DN_DOM_PHN_XCH,PD4H.DN_DOM_PHN_LCL,PD4H.DN_PHN_XTN) AS DN_PHN_H_DOM,
			CATX('',PD4H.DN_FGN_PHN_CNY,PD4H.DN_FGN_PHN_CT,PD4H.DN_FGN_PHN_LCL,PD4H.DN_PHN_XTN) AS DN_PHN_H_FGN,
			PD4H.DI_PHN_VLD AS DI_PHN_VLDH,
			CATX('',PD4A.DN_DOM_PHN_ARA,PD4A.DN_DOM_PHN_XCH,PD4A.DN_DOM_PHN_LCL,PD4A.DN_PHN_XTN) AS DN_PHN_A_DOM,
			CATX('',PD4A.DN_FGN_PHN_CNY,PD4A.DN_FGN_PHN_CT,PD4A.DN_FGN_PHN_LCL,PD4A.DN_PHN_XTN) AS DN_PHN_A_FGN,
			PD4A.DI_PHN_VLD AS DI_PHN_VLDA,
			CATX('',PD4A.DN_DOM_PHN_ARA,PD4W.DN_DOM_PHN_XCH,PD4W.DN_DOM_PHN_LCL,PD4W.DN_PHN_XTN) AS DN_PHN_W_DOM,
			CATX('',PD4W.DN_FGN_PHN_CNY,PD4W.DN_FGN_PHN_CT,PD4W.DN_FGN_PHN_LCL,PD4W.DN_PHN_XTN) AS DN_PHN_W_FGN,
			PD4W.DI_PHN_VLD AS DI_PHN_VLDW,
			UPPER(PD3H.DX_ADR_EML) AS DX_ADR_EMLH,
			PD3H.DI_VLD_ADR_EML AS DI_VLD_ADR_EMLH,
			UPPER(PD3A.DX_ADR_EML) AS DX_ADR_EMLA,
			PD3A.DI_VLD_ADR_EML AS DI_VLD_ADR_EMLA,
			UPPER(PD3W.DX_ADR_EML) AS DX_ADR_EMLW,
			PD3W.DI_VLD_ADR_EML AS DI_VLD_ADR_EMLW,
			CATX(' ',LN10.LF_DOE_SCL_ORG,SC10.IM_SCL_FUL) AS SCHOOL
		FROM
			CURRENT_CAMPAIGN_BORROWERS CAMP
			JOIN PKUB.PD10_PRS_NME PD10
				ON CAMP.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
			JOIN PKUB.PD26_PRS_SKP_PRC PD26
				ON PD10.DF_PRS_ID = PD26.DF_PRS_ID
				AND PD26.DC_STA_SKP = '2'
			JOIN PKUB.LN10_LON LN10
				ON PD26.BF_SSN = LN10.BF_SSN
				AND PD26.LN_SEQ = LN10.LN_SEQ
				AND LN10.LC_STA_LON10 = 'R'
				AND LN10.LA_CUR_PRI > 0
				AND LN10.LC_SST_LON10 <> '5'
			JOIN PKUB.DW01_DW_CLC_CLU DW01
				ON LN10.BF_SSN = DW01.BF_SSN
				AND LN10.LN_SEQ = DW01.LN_SEQ
				AND DW01.WC_DW_LON_STA NOT IN ('07', '13', '16', '17', '18', '19', '20', '21')
			LEFT JOIN PKUB.PD30_PRS_ADR PD30
				ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
				AND PD30.DC_ADR = 'L'
			LEFT JOIN PKUB.PD40_PRS_PHN PD4H
				ON PD10.DF_PRS_ID = PD4H.DF_PRS_ID
				AND PD4H.DC_PHN = 'H'
			LEFT JOIN PKUB.PD40_PRS_PHN PD4A
				ON PD10.DF_PRS_ID = PD4A.DF_PRS_ID
				AND PD4A.DC_PHN = 'A'
			LEFT JOIN PKUB.PD40_PRS_PHN PD4W
				ON PD10.DF_PRS_ID = PD4W.DF_PRS_ID
				AND PD4W.DC_PHN = 'W'
			LEFT JOIN PKUB.PD32_PRS_ADR_EML PD3H
				ON PD10.DF_PRS_ID = PD3H.DF_PRS_ID
				AND PD3H.DC_STA_PD32 = 'A'
				AND PD3H.DC_ADR_EML = 'H'
			LEFT JOIN PKUB.PD32_PRS_ADR_EML PD3A
				ON PD10.DF_PRS_ID = PD3A.DF_PRS_ID
				AND PD3A.DC_STA_PD32 = 'A'
				AND PD3A.DC_ADR_EML = 'A'
			LEFT JOIN PKUB.PD32_PRS_ADR_EML PD3W
				ON PD10.DF_PRS_ID = PD3W.DF_PRS_ID
				AND PD3W.DC_STA_PD32 = 'A'
				AND PD3W.DC_ADR_EML = 'W'
			LEFT JOIN PKUB.SC10_SCH_DMO SC10
				ON LN10.LF_DOE_SCL_ORG = SC10.IF_DOE_SCL
		ORDER BY 
			PD10.DF_SPE_ACC_ID
	;
QUIT;

ENDRSUBMIT;
DATA S76; SET LEGEND.S76; RUN;

DATA S77 (DROP=DN_PHN_H_DOM DN_PHN_H_FGN DN_PHN_A_DOM DN_PHN_A_FGN DN_PHN_W_DOM DN_PHN_W_FGN SCHOOL);
	SET S76;
	BY DF_SPE_ACC_ID;

	RETAIN SCHOOLS;

	IF DN_PHN_H_DOM NE '' THEN DN_PHN_H = DN_PHN_H_DOM; ELSE DN_PHN_H = DN_PHN_H_FGN;
	IF DN_PHN_A_DOM NE '' THEN DN_PHN_A = DN_PHN_A_DOM; ELSE DN_PHN_A = DN_PHN_A_FGN;
	IF DN_PHN_W_DOM NE '' THEN DN_PHN_W = DN_PHN_W_DOM; ELSE DN_PHN_W = DN_PHN_W_FGN;

	IF FIRST.DF_SPE_ACC_ID AND LAST.DF_SPE_ACC_ID THEN
		DO;
			SCHOOLS = SCHOOL;
			OUTPUT;
		END;
	ELSE IF FIRST.DF_SPE_ACC_ID THEN SCHOOLS = SCHOOL;
	ELSE IF LAST.DF_SPE_ACC_ID THEN
		DO;
			SCHOOLS = CATX(',',SCHOOLS,SCHOOL);
			OUTPUT;
		END;
	ELSE SCHOOLS = CATX(',',SCHOOLS,SCHOOL);		
RUN;

DATA _NULL_;
	SET S77;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;

	FORMAT DD_BRT MMDDYY10.;

	IF _N_ = 1 THEN
		DO;
			PUT 'Borrower Account Number,Borrower Name,Date of Birth,Current Address,Current Foreign Country,Address Validity,Current Home Phone,Home Phone Validity,Current Alt Phone,
				Alt Phone Validity,Current Work Phone,Work Phone Validity,Current Home Email,Home Email Validity,Current Alt Email,
				Alt Email Validity,Current Work Email,Work Email Validity,School';
		END;

	DO;
		PUT DF_SPE_ACC_ID @;  			
		PUT NAME @;
		PUT DD_BRT @;
		PUT ADDRESS @;
		PUT DM_FGN_CNY @;
		PUT DI_VLD_ADR @;
		PUT DN_PHN_H @;
		PUT DI_PHN_VLDH @;
		PUT DN_PHN_A @;
		PUT DI_PHN_VLDA @;
		PUT DN_PHN_W @;
		PUT DI_PHN_VLDW @;
		PUT DX_ADR_EMLH @;
		PUT DI_VLD_ADR_EMLH @;
		PUT DX_ADR_EMLA @;
		PUT DI_VLD_ADR_EMLA @;
		PUT DX_ADR_EMLW @;
		PUT DI_VLD_ADR_EMLW @;
		PUT SCHOOLS ;
	END;
RUN;

