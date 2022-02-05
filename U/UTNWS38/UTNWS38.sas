/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS38.NWS38RZ";
FILENAME REPORT2 "&RPTLIB/UNWS38.NWS38R2";
FILENAME REPORT3 "&RPTLIB/UNWS38.NWS38R3";
FILENAME REPORT4 "&RPTLIB/UNWS38.NWS38R4";

/*LIBNAME SAS_TAB V8 '/sas/whse/progrevw';*/
LIBNAME SAS_TAB V8 'Y:\Development\SAS Test Files\progrevw';

DATA _NULL_;
	SET SAS_TAB.CAMPAIGNS;
	CALL SYMPUT('BEG_DATE',BEGIN_DATE);
	CALL SYMPUT('END_DATE',END_DATE);
	WHERE TODAY() BETWEEN BEGIN_DATE AND END_DATE;
	WHERE CAMPAIGN_NAME LIKE '%Late Stage Resolution';
RUN;

%SYSLPUT BEG_DATE = &BEG_DATE;
%SYSLPUT END_DATE = &END_DATE;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work;

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

RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
%LET DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;

PROC SQL;
	CREATE TABLE MANC AS
		SELECT DISTINCT
			PD10.DF_SPE_ACC_ID,
			PD10.DF_PRS_ID,
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
			CURRENT_CAMPAIGN_BORROWERS CCB
			JOIN PKUB.PD10_PRS_NME PD10
				ON CCB.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
			JOIN PKUB.LN10_LON LN10
				ON PD10.DF_PRS_ID = LN10.BF_SSN
				AND LN10.LA_CUR_PRI > 0
				AND LN10.LC_STA_LON10 = 'R'
			JOIN
				(
					SELECT
						BF_SSN,
						COUNT(PF_REQ_ACT) AS PF_REQ_ACT_CNT
					FROM
						PKUB.AY10_BR_LON_ATY
					WHERE
						PF_REQ_ACT LIKE ('P199%')
						AND PF_RSP_ACT = 'NOCTC'
						AND LD_ATY_REQ_RCV > &BEG_DATE
					GROUP BY
						BF_SSN
				) AY10
				ON PD10.DF_PRS_ID = AY10.BF_SSN
				AND AY10.PF_REQ_ACT_CNT > 10
			LEFT JOIN PKUB.PD30_PRS_ADR PD30
				ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
				AND PD30.DI_VLD_ADR = 'Y'
			LEFT JOIN PKUB.PD32_PRS_ADR_EML PD32
				ON PD10.DF_PRS_ID = PD32.DF_PRS_ID
				AND PD32.DI_VLD_ADR_EML = 'Y'
				AND PD32.DC_STA_PD32 = 'A'
	;

/*	for testing*/
/*	CREATE TABLE ARCS AS*/
/*		SELECT*/
/*			AY10.BF_SSN,*/
/*			AY10.PF_REQ_ACT,*/
/*			AY10.PF_RSP_ACT,*/
/*			AY10.LD_ATY_REQ_RCV*/
/*		FROM*/
/*			MANC*/
/*			JOIN PKUB.AY10_BR_LON_ATY AY10*/
/*				ON MANC.DF_PRS_ID = AY10.BF_SSN*/
/*		WHERE*/
/*			PF_REQ_ACT LIKE ('P199%')*/
/*			AND PF_RSP_ACT = 'NOCTC'*/
/*			AND LD_ATY_REQ_RCV > &BEG_DATE*/
/*		ORDER BY*/
/*			BF_SSN,*/
/*			LD_ATY_REQ_RCV*/
/*	;*/

	CREATE TABLE R2 AS
		SELECT DISTINCT
			DF_SPE_ACC_ID
		FROM
			MANC
	;

	CREATE TABLE R3 AS
		SELECT DISTINCT
			DF_SPE_ACC_ID,
			DF_PRS_ID,
			DM_PRS_1,
			DM_PRS_LST,
			DX_STR_ADR_1,
			DX_STR_ADR_2,
			DM_CT,
			DC_DOM_ST,
			DF_ZIP_CDE,
			DM_FGN_CNY,
			DM_FGN_ST,
			'MA4481' AS COST_CENTER
		FROM
			MANC
		WHERE
			DX_STR_ADR_1 <> ''
			AND DX_ADR_EML IS NULL
		ORDER BY
			DC_DOM_ST
	;	

	CREATE TABLE R4 AS
		SELECT DISTINCT
			DF_SPE_ACC_ID,
			DM_PRS_1,
			DM_PRS_LST,
			DX_ADR_EML
		FROM
			MANC
		WHERE
			DX_ADR_EML <> ''
	;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/	
QUIT;

DATA R3 (DROP = KEYSSN MODAY KEYLINE CHKDIG DIG I CHKDIG CHK1 CHK2 CHK3 CHKDIGIT CHECK);
	SET R3;
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

ENDRSUBMIT;

/*DATA ARCS; SET LEGEND.ARCS; RUN;*/
/*DATA MANC; SET LEGEND.MANC; RUN;*/
DATA R2; SET LEGEND.R2; RUN;
DATA R3; SET LEGEND.R3; RUN;
DATA R4; SET LEGEND.R4; RUN;

/*export to queue builder (fed) file*/
DATA _NULL_;
	SET R2 ;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
	PUT DF_SPE_ACC_ID 'MRCNC,,,,,,,ALL,Review account and phone number. More than 10 attempts have been made to contact with no success.' ;
RUN;

/*export to comman delimited file for the Special E-mail Campaign - FED script*/
DATA _NULL_;
	SET R3 ;
	FILE REPORT3 DELIMITER=',' DSD DROPOVER LRECL=32767;

	IF _N_ = 1 THEN
		PUT 
			'Borrower Account #,'
			'Borrower First Name,'
			'Last Name,'
			'Street 1,'
			'Street 2,'
			'City,'
			'State,'
			'ZIP,'
			'Foreign Country,'
			'Foreign State,'
			'ACSKeyline,'
			'Costcenter'
	;

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
	PUT COST_CENTER  $ ;
RUN;


/*write to comma delimited file for the Email Batch Script - FED script*/
DATA _NULL_;
	SET	WORK.R4;
	FILE REPORT4 delimiter=',' DSD DROPOVER lrecl=32767;

	/* write column names, remove this to create a file without a header row */
	IF _N_ = 1 THEN
		DO;
			PUT	
				'DF_SPE_ACC_ID'
				','
				'DM_PRS_1'
				','
				'DM_PRS_LST'
				','
				'DX_ADR_EML'
			;
		END;

	/* write data*/	
	DO;
		PUT DF_SPE_ACC_ID $ @;
		PUT DM_PRS_1 $ @;
		PUT DM_PRS_LST $ @;
		PUT DX_ADR_EML $;
		;
	END;
RUN;
