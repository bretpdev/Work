/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWS70.LWS70RZ";
FILENAME REPORT2 "&RPTLIB/ULWS70.LWS70R2";
FILENAME REPORT3 "&RPTLIB/ULWS70.LWS70R3";
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;
/*LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
LIBNAME PROGREVW '/sas/whse/progrevw';

DATA _NULL_;
/*	SET PROGREVW.lastrun_jobs;*/
/*	WHERE JOB = 'UTLWS70';*/
/*Easy way to change date for dev/test purposes*/
	LAST_RUN = today()-7;
	CALL SYMPUTX('LASTRUN',"'"||PUT(LAST_RUN,MMDDYY10.)||"'" );
RUN;
%PUT &LAST_RUN;

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
	CONNECT TO DB2 (DATABASE=DLGSUTWH);

	CREATE TABLE CMPLT AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						AY10.BF_SSN
						,AY10.PF_REQ_ACT
						,PD10.DF_SPE_ACC_ID
						,PD10.DM_PRS_1
						,PD10.DM_PRS_MID
						,PD10.DM_PRS_LST
						,PD30.DX_STR_ADR_1
						,PD30.DX_STR_ADR_2
						,PD30.DX_STR_ADR_3
						,PD30.DM_CT
						,PD30.DC_DOM_ST
						,PD30.DF_ZIP_CDE
						,PD30.DM_FGN_CNY
						,PD30.DM_FGN_ST
						,AY10.LD_ATY_REQ_RCV
					FROM	
						OLWHRM1.AY10_BR_LON_ATY AY10
						INNER JOIN OLWHRM1.PD10_PRS_NME PD10
							ON AY10.BF_SSN = PD10.DF_PRS_ID
						INNER JOIN OLWHRM1.PD30_PRS_ADR PD30
							ON AY10.BF_SSN = PD30.DF_PRS_ID
							AND PD30.DC_ADR = 'L'
					WHERE	
						AY10.PF_REQ_ACT IN ('BCTPA','BCACH')
						AND AY10.LD_ATY_REQ_RCV >= &LASTRUN

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

/*DATA PROGREVW.LASTRUN_JOBS;*/
/*	SET PROGREVW.LASTRUN_JOBS;*/
/*	IF JOB = 'UTLWS70' THEN LAST_RUN = TODAY();*/
/*RUN;*/

ENDRSUBMIT;

DATA CMPLT;
	SET DUSTER.CMPLT;
RUN;

/*Insert Keyline*/
DATA CMPLT (DROP = KEYSSN MODAY KEYLINE CHKDIG DIG I CHKDIG CHK1 CHK2 CHK3 CHKDIGIT CHECK);
	SET CMPLT;
	KEYSSN = TRANSLATE(BF_SSN,'MYLAUGHTER','0987654321');
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

DATA R2 R3;
	SET CMPLT;
	IF PF_REQ_ACT = 'BCACH' THEN OUTPUT R2;
	IF PF_REQ_ACT = 'BCTPA' THEN OUTPUT R3;
RUN;

/*Because blank fields go to top when sorted, sorting by state codes first then country is how to make this work*/
PROC SORT DATA=R2; 
	BY DC_DOM_ST DM_FGN_CNY;
RUN;

PROC SORT DATA=R3;
	BY DC_DOM_ST DM_FGN_CNY;
RUN;

/*write to comma delimited file*/
DATA _NULL_;
	SET  WORK.R2;
	FILE REPORT2 delimiter=',' DSD DROPOVER lrecl=32767;

	FORMAT BF_SSN $9. ;
	FORMAT PF_REQ_ACT $5. ;
	FORMAT DF_SPE_ACC_ID $10. ;
	FORMAT LD_ATY_REQ_RCV YYMMDDd10. ;
	FORMAT COST_CENTER $6. ;

	IF _N_ = 1 THEN        /* write column names */
		DO;
			PUT
			'SSN'
			','
			'ARC'
			','
			'ACCOUNT NUMBER'
			','
			'FIRST NAME'
			','
			'MIDDLE INITIAL'
			','
			'LAST NAME'
			','
			'STREET 1'
			','
			'STREET 2'
			','
			'STREET 3'
			','
			'CITY'
			','
			'STATE'
			','
			'ZIP'
			','
			'FOREIGN COUNTRY'
			','
			'FOREIGN STATE'
			','
			'ACS KEYLINE'
			','
			'ARC DATE'
			','
			'COST CENTER';
		END;
				
	DO;
		PUT BF_SSN $ @;
		PUT PF_REQ_ACT $ @;
		PUT DF_SPE_ACC_ID $ @;
		PUT DM_PRS_1 $ @;
		PUT DM_PRS_MID $ @;
		PUT DM_PRS_LST $ @;
		PUT DX_STR_ADR_1 $ @;
		PUT DX_STR_ADR_2 $ @;
		PUT DX_STR_ADR_3 $ @;
		PUT DM_CT $ @;
		PUT DC_DOM_ST $ @;
		PUT DF_ZIP_CDE $ @;
		PUT DM_FGN_CNY $ @;
		PUT DM_FGN_ST $ @;
		PUT ACSKEY $ @;
		PUT LD_ATY_REQ_RCV @;
		PUT 'MA2324' ;
		;
	END;
RUN;

DATA _NULL_;
	SET  WORK.R3;
	FILE REPORT3 delimiter=',' DSD DROPOVER lrecl=32767;

	FORMAT BF_SSN $9. ;
	FORMAT PF_REQ_ACT $5. ;
	FORMAT DF_SPE_ACC_ID $10. ;
	FORMAT LD_ATY_REQ_RCV YYMMDDd10. ;
	FORMAT COST_CENTER $6. ;

	IF _N_ = 1 THEN        /* write column names */
		DO;
			PUT
			'SSN'
			','
			'ARC'
			','
			'ACCOUNT NUMBER'
			','
			'FIRST NAME'
			','
			'MIDDLE INITIAL'
			','
			'LAST NAME'
			','
			'STREET 1'
			','
			'STREET 2'
			','
			'STREET 3'
			','
			'CITY'
			','
			'STATE'
			','
			'ZIP'
			','
			'FOREIGN COUNTRY'
			','
			'FOREIGN STATE'
			','
			'ACS KEYLINE'
			','
			'ARC DATE'
			','
			'COST CENTER';
		END;
				
	DO;
		PUT BF_SSN $ @;
		PUT PF_REQ_ACT $ @;
		PUT DF_SPE_ACC_ID $ @;
		PUT DM_PRS_1 $ @;
		PUT DM_PRS_MID $ @;
		PUT DM_PRS_LST $ @;
		PUT DX_STR_ADR_1 $ @;
		PUT DX_STR_ADR_2 $ @;
		PUT DX_STR_ADR_3 $ @;
		PUT DM_CT $ @;
		PUT DC_DOM_ST $ @;
		PUT DF_ZIP_CDE $ @;
		PUT DM_FGN_CNY $ @;
		PUT DM_FGN_ST $ @;
		PUT ACSKEY $ @;
		PUT LD_ATY_REQ_RCV @;
		PUT 'MA2324' ;
		;
	END;
RUN;
