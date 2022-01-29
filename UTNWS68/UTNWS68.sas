/*%LET RPTLIB = %SYSGET(reportdir);*/
/*LIBNAME SAS_TAB V8 '/sas/whse/progrevw';*/
%LET RPTLIB = T:\SAS;
LIBNAME SAS_TAB V8 'Y:\Development\SAS Test Files\progrevw';
FILENAME REPORTZ "&RPTLIB/UNWS68.NWS68RZ";
FILENAME REPORT2 "&RPTLIB/UNWS68.NWS68R2";
FILENAME REPORT3 "&RPTLIB/UNWS68.NWS68R3";
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

RSUBMIT;

LIBNAME PKUB DB2 DATABASE=DNFPUTDL OWNER=PKUB;


PROC SQL;
	CREATE TABLE POP AS 
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
			PD30.DM_FGN_ST,
			'MA4481' AS COST_CENTER_CODE,
			LN16.LN_DLQ_MAX,
			LN20.LF_EDS,
			CASE
				WHEN PD30.DC_DOM_ST IN ('FC','') THEN 1
				ELSE 2
	 		END AS SVAR
		FROM
			CURRENT_CAMPAIGN_BORROWERS CCBR
			INNER JOIN PKUB.PD10_PRS_NME PD10
				ON PD10.DF_SPE_ACC_ID = CCBR.DF_SPE_ACC_ID
			INNER JOIN PKUB.PD30_PRS_ADR PD30
				ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
				AND PD30.DI_VLD_ADR  = 'Y'
			JOIN PKUB.LN10_LON LN10	
				ON LN10.BF_SSN = PD10.DF_PRS_ID	
			JOIN PKUB.LN16_LON_DLQ_HST LN16
				ON LN10.BF_SSN = LN16.BF_SSN
/*				AND LN10.LN_SEQ = LN16.LN_SEQ*/
				AND LN10.LC_STA_LON10 = 'R'
				AND LN10.LA_CUR_PRI > 0
				AND LN10.IC_LON_PGM NOT IN ('DLPLUS', 'DPLUS', 'DLPCNS', 'PLUS')
				AND LN10.LC_TL4_IBR_ELG <> 'I'
				AND LN16.LN_DLQ_MAX + 1 BETWEEN 180 AND 270
				AND LN16.LC_STA_LON16 = '1'
			JOIN PKUB.DW01_DW_CLC_CLU DW01
				ON LN10.BF_SSN = DW01.BF_SSN
				AND LN10.LN_SEQ = DW01.LN_SEQ
				AND DW01.WC_DW_LON_STA IN ('03','13','14')
			LEFT JOIN PKUB.LN20_EDS LN20
				ON LN10.BF_SSN = LN20.BF_SSN
				AND LN10.LN_SEQ = LN20.LN_SEQ
				AND LN20.LC_STA_LON20 = 'A'	
				AND LN20.LC_EDS_TYP = 'M'
	;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

PROC SQL;
	CREATE TABLE COBWRS AS 
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
			PD30.DM_FGN_ST,
			'MA4481' AS COST_CENTER_CODE,
			POP.LN_DLQ_MAX,
			POP.LF_EDS,
			CASE
				WHEN PD30.DC_DOM_ST IN ('FC','') THEN 1
				ELSE 2
	 		END AS SVAR
		FROM 
			PKUB.LN20_EDS LN20
			INNER JOIN POP 
				ON POP.LF_EDS = LN20.LF_EDS
			INNER JOIN PKUB.PD10_PRS_NME PD10
				ON PD10.DF_PRS_ID = LN20.LF_EDS
			INNER JOIN PKUB.PD30_PRS_ADR PD30
				ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
				AND PD30.DI_VLD_ADR  = 'Y'
;

ENDRSUBMIT;

DATA BWRPOP;
SET LEGEND.POP;
RUN;

DATA FINALCOBWERS;
SET LEGEND.COBWRS;
RUN;

PROC SORT DATA=BWRPOP NODUP; BY DF_SPE_ACC_ID ; RUN;
PROC SORT DATA=FINALCOBWERS NODUP; BY DF_SPE_ACC_ID ; RUN;

DATA FINAL;
	MERGE BWRPOP FINALCOBWERS;
	BY DF_SPE_ACC_ID;
RUN;

DATA FINAL (DROP = KEYSSN MODAY KEYLINE CHKDIG DIG I 
	CHKDIG CHK1 CHK2 CHK3 CHKDIGIT CHECK);
SET FINAL;
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


DATA BWRSR2 (DROP=LN_DLQ_MAX);
SET FINAL;
WHERE LN_DLQ_MAX BETWEEN 180 AND 239;
RUN;

DATA BWRSR3 (DROP=LN_DLQ_MAX);
SET FINAL;
WHERE LN_DLQ_MAX BETWEEN 240 AND 270;
RUN;


PROC SORT DATA=BWRSR3 NODUP; BY SVAR DC_DOM_ST; RUN;
PROC SORT DATA=BWRSR2 NODUP; BY SVAR DC_DOM_ST; RUN;

DATA _NULL_;
	SET		BWRSR2;
	FILE	REPORT2 delimiter=',' DSD DROPOVER lrecl=32767;

	/* write column names, remove this to create a file without a header row */
	IF _N_ = 1 THEN
		DO;
			PUT	
				'AccountNumber'
				','
				'FirstName'
				','
				'LastName'
				','
				'Address1'
				','
				'Address2'
				','
				'City'
				','
				'State'
				','
				'ZIP'
				','
				'Country'
				','
				'ForeignState'
				','
				'KeyLine'
				','
				'Cost_Center_Code'
			;
		END;

	/* write data*/	
	DO;
		PUT DF_SPE_ACC_ID $ @;
		PUT DM_PRS_1 $ @;
		PUT DM_PRS_LST $ @;
		PUT DX_STR_ADR_1 @;
		PUT DX_STR_ADR_2 @;
		PUT DM_CT @;
		PUT DC_DOM_ST @;
		PUT DF_ZIP_CDE @;
		PUT DM_FGN_CNY @;
		PUT DM_FGN_ST @;
		PUT ACSKEY @;
		PUT COST_CENTER_CODE $;

		;
	END;
RUN;

DATA _NULL_;
	SET		BWRSR3;
	FILE	REPORT3 delimiter=',' DSD DROPOVER lrecl=32767;

	/* write column names, remove this to create a file without a header row */
	IF _N_ = 1 THEN
		DO;
			PUT	
				'AccountNumber'
				','
				'FirstName'
				','
				'LastName'
				','
				'Address1'
				','
				'Address2'
				','
				'City'
				','
				'State'
				','
				'ZIP'
				','
				'Country'
				','
				'ForeignState'
				','
				'KeyLine'
				','
				'Cost_Center_Code'
			;
		END;

	/* write data*/	
	DO;
		PUT DF_SPE_ACC_ID $ @;
		PUT DM_PRS_1 $ @;
		PUT DM_PRS_LST $ @;
		PUT DX_STR_ADR_1 @;
		PUT DX_STR_ADR_2 @;
		PUT DM_CT @;
		PUT DC_DOM_ST @;
		PUT DF_ZIP_CDE @;
		PUT DM_FGN_CNY @;
		PUT DM_FGN_ST @;
		PUT ACSKEY @;
		PUT COST_CENTER_CODE $;

		;
	END;
RUN;
