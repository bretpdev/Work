/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS43.NWS43RZ";
FILENAME REPORT2 "&RPTLIB/UNWS43.NWS43R2";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;

RSUBMIT LEGEND;
/*%LET DB = DNFPRQUT;  *This is test;*/
/*%LET DB = DNFPRUUT;  *This is VUK3 test;*/
%LET DB = DNFPUTDL;  *This is live;
LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;
LIBNAME SAS_TAB V8 '/sas/whse/progrevw';
/*LIBNAME SAS_TAB V8 'Y:\Development\SAS Test Files\progrevw';*/

/*calculate the end of the current quarter*/
/*DATA _NULL_;*/
/*	CALL SYMPUT('EOQ',INTNX('QTR',TODAY(),0,'E'));*/
/*RUN;*/

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

%LET REPORT_DATE = TODAY();

/*assign default values to macro variables so the variables are always resolved even when there are no campaigns for the report date*/
DATA _NULL_;
	CALL SYMPUT('BEG_DATE',&REPORT_DATE);
	CALL SYMPUT('END_DATE',&REPORT_DATE);
RUN;


DATA _NULL_;
	SET SAS_TAB.CAMPAIGNS;
	CALL SYMPUT('BEG_DATE',BEGIN_DATE);
	CALL SYMPUT('END_DATE',END_DATE);
	WHERE 
		&REPORT_DATE BETWEEN BEGIN_DATE AND END_DATE
		AND CAMPAIGN_NAME LIKE ('%Late Stage Resolution')
	;
RUN;

%PUT &REPORT_DATE &BEG_DATE &END_DATE;

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

/*Get Current Campaign End Date and Create Macro for it*/
/*PROC SQL;*/
/*	SELECT DISTINCT*/
/*		END_DATE INTO :EOC*/
/*		SEPARATED BY ' '*/
/*	FROM*/
/*		SAS_TAB.CAMPAIGNS CAMP*/
/*	WHERE TODAY() BETWEEN BEGIN_DATE AND END_DATE*/
/*	AND CAMP.CAMPAIGN_NAME LIKE '%Late Stage Resolution'*/
/*	;*/
/*QUIT;*/

PROC SQL;
	CREATE TABLE DEMO AS
		SELECT DISTINCT
			PD10.DF_SPE_ACC_ID,
			LN10.LN_SEQ,
			FB10.LC_FOR_TYP
		FROM
			CURRENT_CAMPAIGN_BORROWERS CCBR
			JOIN PKUB.PD10_PRS_NME PD10
				ON CCBR.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
			JOIN PKUB.LN10_LON LN10
				ON PD10.DF_PRS_ID = LN10.BF_SSN
			JOIN PKUB.FB10_BR_FOR_REQ FB10
				ON LN10.BF_SSN = FB10.BF_SSN
			JOIN PKUB.LN60_BR_FOR_APV LN60
				ON LN10.BF_SSN = LN60.BF_SSN
				AND LN10.LN_SEQ = LN60.LN_SEQ
				AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
		WHERE
			LN10.LA_CUR_PRI > 0
			AND LN10.LC_STA_LON10 = 'R'
			AND FB10.LC_FOR_STA = 'A'
			AND FB10.LC_STA_FOR10 = 'A'
			AND LN60.LC_STA_LON60 = 'A'
			AND LN60.LD_FOR_END > &END_DATE
		ORDER BY
			PD10.DF_SPE_ACC_ID
	;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/

QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

DATA _NULL_;
	SET DEMO;
	FILE REPORT2 DSD DROPOVER;

	IF _N_ = 1 THEN PUT 'Account Number,Loan Sequence,Forbearance Type';

	PUT DF_SPE_ACC_ID LN_SEQ LC_FOR_TYP;
RUN;
