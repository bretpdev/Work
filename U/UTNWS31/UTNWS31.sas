/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS31.NWS31RZ";
FILENAME REPORT2 "&RPTLIB/UNWS31.NWS31R2";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
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
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE INITPOP AS
		SELECT	
			BF_SSN,
			DI_PHN_VLD,
			DC_ALW_ADL_PHN,
			LD_DLQ_OCC,
			CASE
				WHEN TODAY() - LD_DLQ_OCC < 15 THEN 'CURR'
				ELSE 'DELQ'
			END AS DELQ_STA
		FROM	
			CONNECTION TO DB2 
				(
					SELECT	
						DISTINCT
						LN10.BF_SSN,
						PD40.DI_PHN_VLD,
						PD40.DC_ALW_ADL_PHN,
						MAX(LN16.LD_DLQ_OCC) AS LD_DLQ_OCC
					FROM
						PKUB.LN10_LON LN10
						LEFT OUTER JOIN PKUB.PD40_PRS_PHN PD40
							ON LN10.BF_SSN = PD40.DF_PRS_ID
						LEFT OUTER JOIN 
							(
								SELECT
									BF_SSN,
									LN_SEQ,
									MAX(LD_DLQ_OCC) AS LD_DLQ_OCC
								FROM
									PKUB.LN16_LON_DLQ_HST
								WHERE
									LC_STA_LON16 = '1'
								GROUP BY
									BF_SSN,
									LN_SEQ
							) LN16
							ON LN10.BF_SSN = LN16.BF_SSN
							AND LN10.LN_SEQ = LN16.LN_SEQ
						
					WHERE
						LN10.LA_CUR_PRI > 0
						AND LN10.LC_STA_LON10 = 'R'
					GROUP BY
						LN10.BF_SSN,
						PD40.DI_PHN_VLD,
						PD40.DC_ALW_ADL_PHN

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA INITPOP; SET LEGEND.INITPOP; RUN;


/*get total borrower and valid phone counts*/
PROC SQL;
/*get count of all borrowers*/
	CREATE TABLE BRWS AS
		SELECT
			COUNT(DISTINCT BF_SSN) AS BRWS
		FROM
			INITPOP
	;

/*get count of all borrowers with a valid phone*/
	CREATE TABLE BRW_VLD_PHN AS
		SELECT
			COUNT(DISTINCT BF_SSN) AS BRWVLDPHN
		FROM
			INITPOP
		WHERE
			DI_PHN_VLD = 'Y'
	;

/*get count of all delinquent borrowers*/
	CREATE TABLE DLQS AS
		SELECT
			COUNT(DISTINCT BF_SSN) AS DLQS
		FROM
			INITPOP
		WHERE
			DELQ_STA = 'DELQ'
	;

/*get count of all delinquent borrowers with a valid phone*/
	CREATE TABLE DLQ_VLD_PHN AS
		SELECT
			COUNT(DISTINCT BF_SSN) AS DLQVLDPHN
		FROM
			INITPOP
		WHERE
			DELQ_STA = 'DELQ'
			AND DI_PHN_VLD = 'Y'
	;
QUIT;


/*get other borrower counts*/
PROC SQL;
/*all borrowers with valid mobile with consent, needed in query below to determine all borrowers with valid land and no mobile with consent*/
	CREATE TABLE BRW_CONSENT_DET AS
		SELECT
			DISTINCT 
			BF_SSN
		FROM
			INITPOP
		WHERE
			DI_PHN_VLD = 'Y'
			AND DC_ALW_ADL_PHN = 'P'
	;

/*count of all borrowers with valid mobile with consent*/
	CREATE TABLE BRW_CONSENT AS
		SELECT
			COUNT(BF_SSN) AS BRWCONSENT
		FROM
			BRW_CONSENT_DET
	;

/*count of all borrowers with valid land and no mobile with consent*/
	CREATE TABLE BRW_LAND AS
		SELECT
			COUNT(DISTINCT I.BF_SSN) AS BRWLAND
		FROM
			INITPOP I
			LEFT OUTER JOIN BRW_CONSENT_DET C
				ON I.BF_SSN = C.BF_SSN
		WHERE
			I.DI_PHN_VLD = 'Y'
			AND I.DC_ALW_ADL_PHN = 'L'
			AND C.BF_SSN IS NULL
	;
QUIT;


/*get other delinquent counts*/
PROC SQL;
/*delinquent borrowers with valid mobile with consent, needed in query below to determine delinquent borrowers with valid land and no mobile with consent*/
	CREATE TABLE DLQ_CONSENT_DET AS
		SELECT
			DISTINCT 
			BF_SSN
		FROM
			INITPOP
		WHERE
			DELQ_STA = 'DELQ'
			AND DI_PHN_VLD = 'Y'
			AND DC_ALW_ADL_PHN = 'P'
	;

/*count of delinquent borrowers with valid mobile with consent*/
	CREATE TABLE DLQ_CONSENT AS
		SELECT
			COUNT(BF_SSN) AS DLQCONSENT
		FROM
			DLQ_CONSENT_DET
	;

/*count of delinquent borrowers with valid land and no mobile with consent*/
	CREATE TABLE DLQ_LAND AS
		SELECT
			COUNT(DISTINCT I.BF_SSN) AS DLQLAND
		FROM
			INITPOP I
			LEFT OUTER JOIN DLQ_CONSENT_DET C
				ON I.BF_SSN = C.BF_SSN
		WHERE
			I.DELQ_STA = 'DELQ'
			AND I.DI_PHN_VLD = 'Y'
			AND I.DC_ALW_ADL_PHN = 'L'
			AND C.BF_SSN IS NULL
	;
QUIT;


/*merge datasets and calculate calculated counts and percentages*/
DATA COUNTS;
	MERGE BRWS BRW_VLD_PHN DLQS DLQ_VLD_PHN BRW_CONSENT BRW_LAND DLQ_CONSENT DLQ_LAND;
	BRWINVLDPHN = BRWS - BRWVLDPHN;
	DLQINVLDPHN = DLQS - DLQVLDPHN;
	BRWNOCONSENT = BRWVLDPHN - BRWCONSENT - BRWLAND;
	DLQNOCONSENT = DLQVLDPHN - DLQCONSENT - DLQLAND;
	BRWCONSENTPCT = BRWCONSENT/BRWS;
	DLQCONSENTPCT = DLQCONSENT/DLQS;
	BRWLANDPCT = BRWLAND/BRWS;
	DLQLANDPCT = DLQLAND/DLQS;
	BRWNOCONSENTPCT = BRWNOCONSENT/BRWS;
	DLQNOCONSENTPCT = DLQNOCONSENT/DLQS;
	BRWINVLDPHNPCT = BRWINVLDPHN/BRWS;
	DLQINVLDPHNPCT = DLQINVLDPHN/DLQS;
RUN;

/*create printed report*/
DATA _NULL_;
	SET		WORK.COUNTS;
	FILE	REPORT2 delimiter='/' DSD DROPOVER lrecl=32767;

	FORMAT
		BRWS
		BRWVLDPHN
		BRWINVLDPHN
		BRWCONSENT
		BRWLAND
		BRWNOCONSENT
		DLQS
		DLQVLDPHN
		DLQINVLDPHN
		DLQCONSENT
		DLQLAND
		DLQNOCONSENT
		COMMA10.
	;

	FORMAT 	
		BRWCONSENTPCT
		DLQCONSENTPCT
		BRWLANDPCT
		DLQLANDPCT
		BRWNOCONSENTPCT
		DLQNOCONSENTPCT
		BRWINVLDPHNPCT
		DLQINVLDPHNPCT 
		PERCENT8.2
	;

	PUT '                                                   TCPA Consent Reporting';
	PUT ;
	PUT ;
	PUT 'Total Amount of Open Released Accounts in the CornerStone Portfolio:                                                    	' BRWS;
	PUT 'Number of borrowers with a valid phone number:                                                                          	' BRWVLDPHN;
	PUT 'Number of borrowers with an invalid phone number:                                                                       	' BRWINVLDPHN;
	PUT 'Number of borrowers with a valid mobile phone number and who have given consent to be contacted with an auto-dialer:    	' BRWCONSENT;
	PUT 'Number of borrowers with a valid landline phone number and don’t have a valid mobile phone number with consent:         	' BRWLAND;
	PUT 'Number of borrowers with a valid mobile phone number and who have not given consent to be contacted with an auto-dialer:   ' BRWNOCONSENT;
	PUT 'Percentage of borrowers who have given consent to be contacted with an auto-dialer on their valid mobile phone number:  	' BRWCONSENTPCT;
	PUT 'Percentage of borrowers with a valid landline phone number and don’t have a valid mobile phone number with consent:     	' BRWLANDPCT;
	PUT 'Percentage of borrowers who have not given consent to be contacted with an auto-dialer on their valid phone number:     	' BRWNOCONSENTPCT;
	PUT 'Percentage of borrowers who have an invalid phone number:                                                               	' BRWINVLDPHNPCT;
	PUT ;
	PUT 'Total Amount of Delinquent Accounts in the CornerStone Portfolio:                                                       	' DLQS;
	PUT 'Number of delinquent borrowers with a valid phone number:                                                               	' DLQVLDPHN;
	PUT 'Number of delinquent borrowers with an invalid phone number:                                                            	' DLQINVLDPHN;
	PUT 'Number of delinquent borrowers with a valid mobile phone number and have given consent to be contacted with an';
	PUT 'auto-dialer:                                                                                                            	' DLQCONSENT;
	PUT 'Number of delinquent borrowers with a valid landline phone number and don’t have a valid mobile phone number with';
	PUT 'consent:                                                                                                                	' DLQLAND;
	PUT 'Number of delinquent borrowers with a valid mobile phone number and have not given consent to be contacted with an';
	PUT 'auto-dialer:                                                                                                            	' DLQNOCONSENT;
	PUT 'Percentage of delinquent borrowers who have given consent to be contacted with an auto-dialer on their valid mobile';
	PUT 'phone number:                                                                                                           	' DLQCONSENTPCT;
	PUT 'Percentage of delinquent borrowers with a valid landline phone number and don’t have a valid mobile phone number with';
	PUT 'consent:                                                                                                                	' DLQLANDPCT;
	PUT 'Percentage of delinquent borrowers who have not given consent to be contacted with an auto-dialer on their valid phone';
	PUT 'number:                                                                                                                 	' DLQNOCONSENTPCT;
	PUT 'Percentage of delinquent borrowers who have an invalid phone number:                                                    	' DLQINVLDPHNPCT;
	PUT ;
	PUT ;
	PUT '                                 JOB = UTNWS31                        REPORT = UNWS31.NWS31R2';

RUN;

/*Compass detail files for testing*/
/*PROC SQL;*/
/*	CREATE TABLE BRWS_DET AS*/
/*		SELECT*/
/*			DISTINCT BF_SSN*/
/*		FROM*/
/*			INITPOP*/
/*	;*/
/**/
/*	CREATE TABLE BRW_VLD_PHN_DET AS*/
/*		SELECT*/
/*			DISTINCT BF_SSN*/
/*		FROM*/
/*			INITPOP*/
/*		WHERE*/
/*			DI_PHN_VLD = 'Y'*/
/*	;*/
/**/
/*	CREATE TABLE DLQS_DET AS*/
/*		SELECT*/
/*			DISTINCT BF_SSN*/
/*		FROM*/
/*			INITPOP*/
/*		WHERE*/
/*			DELQ_STA = 'DELQ'*/
/*	;*/
/**/
/*	CREATE TABLE DLQ_VLD_PHN_DET AS*/
/*		SELECT*/
/*			DISTINCT BF_SSN*/
/*		FROM*/
/*			INITPOP*/
/*		WHERE*/
/*			DELQ_STA = 'DELQ'*/
/*			AND DI_PHN_VLD = 'Y'*/
/*	;*/
/**/
/*	CREATE TABLE BRW_CONSENT_DET AS*/
/*		SELECT*/
/*			DISTINCT */
/*			BF_SSN*/
/*		FROM*/
/*			INITPOP*/
/*		WHERE*/
/*			DI_PHN_VLD = 'Y'*/
/*			AND DC_ALW_ADL_PHN = 'P'*/
/*	;*/
/**/
/*	CREATE TABLE BRW_LAND_DET AS*/
/*		SELECT*/
/*			DISTINCT I.BF_SSN*/
/*		FROM*/
/*			INITPOP I*/
/*			LEFT OUTER JOIN BRW_CONSENT_DET C*/
/*				ON I.BF_SSN = C.BF_SSN*/
/*		WHERE*/
/*			I.DI_PHN_VLD = 'Y'*/
/*			AND I.DC_ALW_ADL_PHN = 'L'*/
/*			AND C.BF_SSN IS NULL*/
/*	;*/
/**/
/*	CREATE TABLE DLQ_CONSENT_DET AS*/
/*		SELECT*/
/*			DISTINCT */
/*			BF_SSN*/
/*		FROM*/
/*			INITPOP*/
/*		WHERE*/
/*			DELQ_STA = 'DELQ'*/
/*			AND DI_PHN_VLD = 'Y'*/
/*			AND DC_ALW_ADL_PHN = 'P'*/
/*	;*/
/**/
/*	CREATE TABLE DLQ_LAND_DET AS*/
/*		SELECT*/
/*			DISTINCT I.BF_SSN*/
/*		FROM*/
/*			INITPOP I*/
/*			LEFT OUTER JOIN DLQ_CONSENT_DET C*/
/*				ON I.BF_SSN = C.BF_SSN*/
/*		WHERE*/
/*			I.DELQ_STA = 'DELQ'*/
/*			AND I.DI_PHN_VLD = 'Y'*/
/*			AND I.DC_ALW_ADL_PHN = 'L'*/
/*			AND C.BF_SSN IS NULL*/
/*	;*/
/*QUIT;*/
/**/
/*PROC EXPORT DATA= WORK.BRWS_DET */
/*            OUTFILE= "T:\SAS\BRWS_DET.xls" */
/*            DBMS=EXCEL REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
/**/
/*PROC EXPORT DATA= WORK.BRW_VLD_PHN_DET */
/*            OUTFILE= "T:\SAS\BRW_VLD_PHN_DET.xls" */
/*            DBMS=EXCEL REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
/**/
/*PROC EXPORT DATA= WORK.DLQS_DET */
/*            OUTFILE= "T:\SAS\DLQS_DET.xls" */
/*            DBMS=EXCEL REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
/**/
/*PROC EXPORT DATA= WORK.DLQ_VLD_PHN_DET */
/*            OUTFILE= "T:\SAS\DLQ_VLD_PHN_DET.xls" */
/*            DBMS=EXCEL REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
/**/
/*PROC EXPORT DATA= WORK.BRW_CONSENT_DET */
/*            OUTFILE= "T:\SAS\BRW_CONSENT_DET.xls" */
/*            DBMS=EXCEL REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
/**/
/*PROC EXPORT DATA= WORK.BRW_LAND_DET */
/*            OUTFILE= "T:\SAS\BRW_LAND_DET.xls" */
/*            DBMS=EXCEL REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
/**/
/*PROC EXPORT DATA= WORK.DLQ_CONSENT_DET */
/*            OUTFILE= "T:\SAS\DLQ_CONSENT_DET.xls" */
/*            DBMS=EXCEL REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
/**/
/*PROC EXPORT DATA= WORK.DLQ_LAND_DET */
/*            OUTFILE= "T:\SAS\DLQ_LAND_DET.xls" */
/*            DBMS=EXCEL REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
