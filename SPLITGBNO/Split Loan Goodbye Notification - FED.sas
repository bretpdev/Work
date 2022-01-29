%LET RPTLIB = T:\SAS;

LIBNAME XL "&RPTLIB\Cornerstone Transfers.xlsx";
LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
/*LIBNAME PROGREVW 'Y:\Development\SAS Test Files\progrevw';*/
LIBNAME PROGREVW 'Z:\SAS\progrevw';

RSUBMIT;
LIBNAME PKUB DB2 DATABASE=DNFPUTDL OWNER=PKUB;

/*get master lists of demographics*/
PROC SQL;
/*	all addresses and e-mail addresses that meet criteria to be used*/
	CREATE TABLE ALLDEMOS AS
		SELECT
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
			PD32.DX_ADR_EML
		FROM
			PKUB.PD10_PRS_NME PD10
			LEFT JOIN PKUB.PD30_PRS_ADR PD30
				ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
				AND PD30.DI_VLD_ADR = 'Y'
			LEFT JOIN PKUB.PD32_PRS_ADR_EML PD32
				ON PD10.DF_PRS_ID = PD32.DF_PRS_ID
				AND PD32.DC_STA_PD32 = 'A'
				AND PD32.DI_VLD_ADR_EML = 'Y'
	;

/*	borrowers with a valid address (from ALLDEMOS) and no valid e-mail*/
	CREATE TABLE LETTERS AS
		SELECT
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
			ALLDEMOS
		WHERE
			DX_STR_ADR_1 IS NOT NULL
			AND DX_ADR_EML IS NULL
	;

/*	borrowers with a valid e-mail address*/
	CREATE TABLE EMAILS AS
		SELECT
			DF_PRS_ID,
			DF_SPE_ACC_ID,
			DM_PRS_1,
			DM_PRS_LST,		
			DX_ADR_EML
		FROM
			ALLDEMOS
		WHERE
			DX_ADR_EML IS NOT NULL
	;
QUIT;

ENDRSUBMIT;

/*create files for each servicer*/
%MACRO PROCESSOR(LRN,ERN,SVC,TAB);
/*	get list of borrowers for the servicer from the spreadsheet*/
	DATA LEGEND.&SVC (KEEP=BF_SSN SVC);
		SET XL."&TAB$"N;
		LENGTH BF_SSN $ 9;
		LENGTH SVC $ 10;
		BF_SSN = COMPRESS(TRANWRD(Borrower_SSN,'-',''));	*remove hyphen from SSN;
		SVC = "&SVC"; 										*add the servicer code so it can be used later to join the data set to the SERVICES data set;
	RUN;

	%SYSLPUT SVC = &SVC;
	
/*	join borrowers for the servicer to the demographic data on LEGEND*/
	RSUBMIT;
	PROC SQL;
/*		letters*/
		CREATE TABLE ADRS AS
			SELECT
				S.SVC,
				L.DF_PRS_ID,
				L.DF_SPE_ACC_ID,
				L.DM_PRS_1,
				L.DM_PRS_LST,
				L.DX_STR_ADR_1,
				L.DX_STR_ADR_2,
				L.DM_CT,
				L.DC_DOM_ST,
				L.DF_ZIP_CDE,
				L.DM_FGN_CNY,
				L.DM_FGN_ST
			FROM
				LETTERS L
				JOIN &SVC S
					ON L.DF_PRS_ID = S.BF_SSN
		;

/*		e-mails*/
		CREATE TABLE EMLS AS
			SELECT
				E.DF_SPE_ACC_ID,
				E.DM_PRS_1,
				E.DM_PRS_LST,
				E.DX_ADR_EML
			FROM
				EMAILS E
				JOIN &SVC S
					ON E.DF_PRS_ID = S.BF_SSN
		;
	QUIT;
	ENDRSUBMIT;

	PROC SQL;
		CREATE TABLE BRW_SVC AS
			SELECT
				ADR.DF_PRS_ID,
				ADR.DF_SPE_ACC_ID,
				ADR.DM_PRS_1,
				ADR.DM_PRS_LST,
				ADR.DX_STR_ADR_1,
				ADR.DX_STR_ADR_2,
				ADR.DM_CT,
				ADR.DC_DOM_ST,
				ADR.DF_ZIP_CDE,
				ADR.DM_FGN_CNY,
				ADR.DM_FGN_ST,
				SVC.SVC_NAME,
				SVC.SVC_ADR_1,
				SVC.SVC_ADR_2,
				SVC.SVC_CT,
				SVC.SVC_ST,
				SVC.SVC_ZIP,
				SVC.SVC_PHN,
				SVC.SVC_WEB,
				SVC.SVC_EML,
				PMT.SVC_NAME AS PMT_NAME,
				PMT.SVC_ADR_1 AS PMT_ADR_1,
				PMT.SVC_ADR_2 AS PMT_ADR_2,
				PMT.SVC_CT AS PMT_CT,
				PMT.SVC_ST AS PMT_ST,
				PMT.SVC_ZIP AS PMT_ZIP
			FROM
				LEGEND.ADRS ADR
				JOIN PROGREVW.SERVICERS SVC
					ON ADR.SVC = SVC.SVC
					AND SVC.TYP = 'SVC'
				LEFT JOIN PROGREVW.SERVICERS PMT
					ON ADR.SVC = PMT.SVC
					AND PMT.TYP = 'PMT'
			ORDER BY
				ADR.DC_DOM_ST
		;
	QUIT;

/*	calculate ACS keyline*/
	DATA R&LRN (DROP = KEYSSN MODAY KEYLINE CHKDIG DIG I CHKDIG CHK1 CHK2 CHK3 CHKDIGIT CHECK);
		SET BRW_SVC;
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

	/*write to comma delimited file for the Batch Letters - FED script*/
	DATA _NULL_;
		SET	R&LRN;
		FILE "&RPTLIB\Goodbye Notification.&SVC Letter.R&LRN..txt" delimiter=',' DSD DROPOVER lrecl=32767;

		IF _N_ = 1 THEN PUT	'AccountNumber,FirstName,LastName,Street1,Street2,City,State,Zip,ForeignCountry,ForeignState,ACSKeyline,CostCenter,'
							'ServicerName,ServicerStreet1,ServicerStreet2,ServicerCity,ServicerState,ServicerZip,ServicerPhone,ServicerWebsite,ServicerEmail,'
							'PaymentName,PaymentStreet1,PaymentStreet2,PaymentCity,PaymentState,PaymentZip';

		PUT DF_SPE_ACC_ID $ @;
		PUT DM_PRS_1 $ @;
		PUT DM_PRS_LST $ @;
		PUT DX_STR_ADR_1 $ @;
		PUT DX_STR_ADR_2 $ @;
		PUT DM_CT $ @;
		PUT DC_DOM_ST $ @;
		PUT DF_ZIP_CDE $ @;
		PUT DM_FGN_CNY $ @;
		PUT DM_FGN_ST $ @;
		PUT ACSKEY $ @;
		PUT 'MA4481,' @;
		PUT SVC_NAME $ @;
		PUT SVC_ADR_1 $ @;
		PUT SVC_ADR_2 $ @;
		PUT SVC_CT $ @;
		PUT SVC_ST $ @;
		PUT SVC_ZIP $ @;
		PUT SVC_PHN $ @;
		PUT SVC_WEB $ @;
		PUT SVC_EML $ @;
		PUT PMT_NAME $ @;
		PUT PMT_ADR_1 $ @;
		PUT PMT_ADR_2 $ @;
		PUT PMT_CT $ @;
		PUT PMT_ST $ @;
		PUT PMT_ZIP $ ;
	RUN;


	/*write to comma delimited file for the Email Batch Script - FED script*/
	DATA _NULL_;
		SET		LEGEND.EMLS;
		FILE	"&RPTLIB\Goodbye Notification.&SVC Email.R&ERN..txt" delimiter=',' DSD DROPOVER lrecl=32767;

		IF _N_ = 1 THEN PUT	'DF_SPE_ACC_ID,DM_PRS_1,DM_PRS_LST,DX_ADR_EML';

		PUT DF_SPE_ACC_ID $ @;
		PUT DM_PRS_1 $ @;
		PUT DM_PRS_LST $ @;
		PUT DX_ADR_EML $;
	RUN;

%MEND;

%PROCESSOR(2,3,SMAE,To Sallie Mae);
%PROCESSOR(4,5,PHEAA,To Pheaa);
%PROCESSOR(6,7,NELNET,To Nelnet);
%PROCESSOR(8,9,GLAKES,To Great Lakes);
%PROCESSOR(10,11,ASPIRE,To Aspire);
%PROCESSOR(12,13,EDFIN,To EdfinancialED);
%PROCESSOR(14,15,GRANITE,To Granite);
%PROCESSOR(16,17,MOHELA,To MOHELA);
%PROCESSOR(18,19,OSLA,To OSLA);
%PROCESSOR(20,21,VSAC,To VSAC);


