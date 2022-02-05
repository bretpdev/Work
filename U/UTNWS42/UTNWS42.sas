/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS42.NWS42RZ";
FILENAME REPORT2 "&RPTLIB/UNWS42.NWS42R2";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;

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
	CREATE TABLE INIT_POP AS
		SELECT DISTINCT
			PD10.DF_SPE_ACC_ID,
			PD10.DF_PRS_ID,
			PD26.DC_SKP_TYP
		FROM
			PKUB.LN10_LON LN10
			JOIN PKUB.PD26_PRS_SKP_PRC PD26
				ON LN10.BF_SSN = PD26.BF_SSN
			JOIN PKUB.PD10_PRS_NME PD10
				ON PD26.DF_PRS_ID = PD10.DF_PRS_ID
			LEFT JOIN PKUB.AY10_BR_LON_ATY SKQ71
				ON PD26.DF_PRS_ID = SKQ71.BF_SSN
				AND SKQ71.PF_REQ_ACT = 'SKQ71'
				AND SKQ71.LD_ATY_REQ_RCV > PD26.DD_SKP_BEG
			LEFT JOIN PKUB.AY10_BR_LON_ATY KOTHR
				ON PD26.DF_PRS_ID = KOTHR.BF_SSN
				AND KOTHR.PF_REQ_ACT = 'KOTHR'
				AND KOTHR.LD_ATY_REQ_RCV >= TODAY() - 45
		WHERE
			LN10.LA_CUR_PRI > 0
			AND LN10.LC_STA_LON10 = 'R'
			AND PD26.DC_SKP_PRS = '1'
			AND PD26.DC_STA_SKP = '2'
			AND SKQ71.BF_SSN IS NULL /*exclude accounts that have a 2N/AD task requested*/
			AND KOTHR.BF_SSN IS NULL
	;

	CREATE TABLE ADD_SKIPS AS
		SELECT DISTINCT
			IP.DF_SPE_ACC_ID,
			CATX(',',PD30.DC_ADR,PD30.DX_STR_ADR_1,PD30.DX_STR_ADR_2,PD30.DM_CT,PD30.DC_DOM_ST,PD30.DF_ZIP_CDE,PD30.DM_FGN_ST,PD30.DM_FGN_CNY) AS COMMENT,
			'A' AS SKIP_TYP
		FROM
			INIT_POP IP
			JOIN PKUB.PD30_PRS_ADR PD30
				ON IP.DF_PRS_ID = PD30.DF_PRS_ID
		WHERE
			IP.DC_SKP_TYP IN ('A','B')
			AND PD30.DC_ADR IN ('B','D')
			AND PD30.DX_STR_ADR_1 IS NOT NULL
			AND PD30.DI_VLD_ADR = 'Y'
	;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

DATA SKIPS;
	SET ADD_SKIPS;
	WHERE COMMENT IS NOT NULL;
RUN;

PROC SORT DATA=SKIPS;
	BY DF_SPE_ACC_ID SKIP_TYP;
RUN;

/*combine all comments for each account into one record*/
DATA R2 (DROP=COMMENT SKIP_TYP);
	SET SKIPS;
	BY DF_SPE_ACC_ID;

	LENGTH COMMENTS $ 256;
	RETAIN COMMENTS;

	IF FIRST.DF_SPE_ACC_ID THEN COMMENTS = ''; /*reset COMMENTS accumulator variable*/
	COMMENTS = CATX(',',COMMENTS,COMMENT); /*concatenate COMMENTS with COMMENT from record being processed*/
	IF LAST.DF_SPE_ACC_ID; /*output data to the data set when the last record for the account number is reached*/
RUN;

ENDRSUBMIT;

DATA R2; SET LEGEND.R2; RUN;

DATA _NULL_;
	SET R2 ;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;

	PUT DF_SPE_ACC_ID 'SKQ71,,,,,,,ALL,' COMMENTS ;
RUN;
