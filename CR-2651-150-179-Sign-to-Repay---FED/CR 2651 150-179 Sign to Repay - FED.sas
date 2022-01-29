/*%LET RPTLIB = %SYSGET(reportdir);*/
/*LIBNAME SAS_TAB V8 '/sas/whse/progrevw';*/
LIBNAME SAS_TAB V8 'Y:\Development\SAS Test Files\progrevw';
%LET RPTLIB = T:\SAS;

FILENAME REPORTZ "&RPTLIB/CR 2651 150-179 Sign to Repay - FED.RZ";
FILENAME REPORT2 "&RPTLIB/CR 2651 150-179 Sign to Repay - FED.R2";
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
			AND CAMP.CAMPAIGN_NAME = 'CR 2651'
	;
QUIT;

DATA LEGEND.CURRENT_CAMPAIGN_BORROWERS; SET CURRENT_CAMPAIGN_BORROWERS; RUN;

RSUBMIT;
LIBNAME PKUB DB2 DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE LOANS AS
		SELECT DISTINCT
			LN10.BF_SSN,
			LN10.LN_SEQ,
			PD10.DF_PRS_ID,
			PD10.DF_SPE_ACC_ID,
			PD10.DM_PRS_1,
			PD10.DM_PRS_LST,
			PD30.DX_STR_ADR_1,
			PD30.DX_STR_ADR_2,
			PD30.DM_CT,
			PD30.DC_DOM_ST,
			PD30.DF_ZIP_CDE,
			PD30.DM_FGN_CNY,
			PD30.DM_FGN_ST,
			PD30.DI_VLD_ADR
		FROM
			CURRENT_CAMPAIGN_BORROWERS CAMP
			JOIN PKUB.PD10_PRS_NME PD10
				ON CAMP.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
			JOIN PKUB.PD30_PRS_ADR PD30
				ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
			JOIN PKUB.LN10_LON LN10
				ON PD10.DF_PRS_ID = LN10.BF_SSN
				AND LN10.LC_STA_LON10 = 'R'
				AND LN10.LA_CUR_PRI > 0
				AND LN10.IC_LON_PGM NOT IN ('DLPLUS', 'DPLUS', 'DLPCNS')
				AND LN10.LC_TL4_IBR_ELG <> 'I'
			JOIN PKUB.LN16_LON_DLQ_HST LN16
				ON LN10.BF_SSN = LN16.BF_SSN
				AND LN10.LN_SEQ = LN16.LN_SEQ
				AND LN16.LC_STA_LON16 = '1'
				AND LN16.LN_DLQ_MAX + 1 BETWEEN 150 AND 179
			JOIN PKUB.DW01_DW_CLC_CLU DW01
				ON LN10.BF_SSN = DW01.BF_SSN
				AND LN10.LN_SEQ = DW01.LN_SEQ
				AND DW01.WC_DW_LON_STA IN ('03', '13', '14')
	;

	CREATE TABLE BRWS AS
		SELECT DISTINCT
			DF_PRS_ID,
			DF_SPE_ACC_ID,
			DM_PRS_1,
			DM_PRS_LST,
			DX_STR_ADR_1,
			DX_STR_ADR_2,
			DM_CT,
			DC_DOM_ST,
			DF_ZIP_CDE,
			DM_FGN_CNY,
			DM_FGN_ST
		FROM
			LOANS
		WHERE
			DI_VLD_ADR = 'Y'
	;

	CREATE TABLE COMKRS AS
		SELECT DISTINCT
			PD10.DF_PRS_ID,
			PD10.DF_SPE_ACC_ID,
			PD10.DM_PRS_1,
			PD10.DM_PRS_LST,
			PD30.DX_STR_ADR_1,
			PD30.DX_STR_ADR_2,
			PD30.DM_CT,
			PD30.DC_DOM_ST,
			PD30.DF_ZIP_CDE,
			PD30.DM_FGN_CNY,
			PD30.DM_FGN_ST
		FROM
			LOANS
			JOIN PKUB.LN20_EDS LN20
				ON LOANS.BF_SSN = LN20.BF_SSN
				AND LOANS.LN_SEQ = LN20.LN_SEQ
				AND LN20.LC_EDS_TYP = 'M'
			JOIN PKUB.PD10_PRS_NME PD10
				ON PD10.DF_PRS_ID = LN20.LF_EDS
			JOIN PKUB.PD30_PRS_ADR PD30
				ON PD30.DF_PRS_ID = LN20.LF_EDS
				AND PD30.DI_VLD_ADR = 'Y'
	;
QUIT;

ENDRSUBMIT;
DATA BRWS; SET LEGEND.BRWS; RUN;
DATA COMKRS; SET LEGEND.COMKRS; RUN;

DATA RECIPS;
	SET BRWS COMKRS;
RUN;

PROC SORT DATA=RECIPS;
	BY DC_DOM_ST;
RUN;

DATA RECIPS (DROP = KEYSSN MODAY KEYLINE CHKDIG DIG I CHKDIG CHK1 CHK2 CHK3 CHKDIGIT CHECK);
	SET RECIPS;
	KEYSSN = TRANSLATE(DF_PRS_ID,'MYLAUGHTER','0987654321');
	MODAY = PUT(DATE(),MMDDYYN4.);
	KEYLINE = "P"||KEYSSN||MODAY||"L";
	CHKDIG = 0;
	LENGTH DIG $2.;
	DO I = 1 TO LENGTH(KEYLINE);
		IF I/2 NE ROUND(I/2,1) 
			THEN DIG = PUT(INPUT(SUBSTR(KEYLINE,I,1),BITS4.4) * 2, 2.);
		ELSE DIG = PUT(INPUT(SUBSTR(KEYLINE,I,1),BITS4.4), 2.);
		IF SUBSTR(DIG,1,1) = " " 
			THEN CHKDIG = CHKDIG + INPUT(SUBSTR(DIG,2,1),1.);
			ELSE DO;
				CHK1 = INPUT(SUBSTR(DIG,1,1),1.);
				CHK2 = INPUT(SUBSTR(DIG,2,1),1.);
				IF CHK1 + CHK2 >= 10
					THEN DO;
						CHK3 = PUT(CHK1 + CHK2,2.);
						CHK1 = INPUT(SUBSTR(CHK3,1,1),1.);
						CHK2 = INPUT(SUBSTR(CHK3,2,1),1.);
					END;
				CHKDIG = CHKDIG + CHK1 + CHK2;
			END;
	END;
	CHKDIGIT = 10 - INPUT(SUBSTR((RIGHT(PUT(CHKDIG,3.))),3,1),3.);
	IF CHKDIGIT = 10 THEN CHKDIGIT = 0;
	CHECK = PUT(CHKDIGIT,1.);
	ACSKEY = "#"||KEYLINE||CHECK||"#";
RUN;

DATA _NULL_;
	SET RECIPS;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;

	IF _N_ = 1 THEN PUT 'AccountNumber,FirstName,LastName,Address1,Address2,City,State,Zip,Country,ForeignState,Keyline,Cost_Center_Code';

	DO;
		PUT DF_SPE_ACC_ID @;  			
		PUT DM_PRS_1 @;
		PUT DM_PRS_LST @;
		PUT DX_STR_ADR_1 @;
		PUT DX_STR_ADR_2 @;
		PUT DM_CT @;
		PUT DC_DOM_ST @;
		PUT DF_ZIP_CDE @;
		PUT DM_FGN_CNY @;
		PUT DM_FGN_ST @;
		PUT ACSKEY @;
		PUT 'MA4481';
	END;
RUN;

