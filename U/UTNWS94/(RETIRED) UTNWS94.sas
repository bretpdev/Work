/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS94.NWS94RZ";
FILENAME REPORT2 "&RPTLIB/UNWS94.NWS94R2";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
%let DB = DNFPRQUT;  *This is test;
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
/*%let DB = DNFPUTDL;  *This is live;*/

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

	CREATE TABLE STUS AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						PD10.DF_SPE_ACC_ID
						,PD10.DM_PRS_1
						,PD10.DM_PRS_LST
						,PD30.DX_STR_ADR_1
						,PD30.DX_STR_ADR_2
						,PD30.DM_CT
						,PD30.DC_DOM_ST
						,PD30.DF_ZIP_CDE
						,PD30.DM_FGN_ST
						,PD30.DM_FGN_CNY
						,LN10.BF_SSN
					FROM	
						PKUB.PD10_PRS_NME PD10
						INNER JOIN PKUB.LN10_LON LN10
							ON PD10.DF_PRS_ID = LN10.BF_SSN
							AND LN10.IC_LON_PGM NOT IN ('DLPLUS','DPLUS','DLPCNS','DLPLGB','DLSCNS','DLUCNS','DLUSPL','DLSSPL',
														'PLUS', 'FISL', 'SUBCNS', 'UNCNS', 'CNSDLN', 'SUBSPC', 'UNSPC', 'DLSCUC', 'DLSCSL','DLSCUN', 'DLSCPL', 'DLSCSC')
						INNER JOIN PKUB.SD10_STU_SPR SD10
							ON PD10.DF_PRS_ID = SD10.LF_STU_SSN
						LEFT JOIN (
								SELECT 
									SD10.LF_STU_SSN
								FROM 
									PKUB.SD10_STU_SPR SD10
								WHERE SD10.LC_REA_SCL_SPR IN ('00','01','10','11')
									AND SD10.LC_STA_STU10 = 'A'
									) EXCL
							ON PD10.DF_PRS_ID = EXCL.LF_STU_SSN
						LEFT JOIN PKUB.PD30_PRS_ADR PD30
							ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
							AND PD30.DC_ADR = 'L'
						LEFT JOIN AES.PH05_CNC_EML PH05
							ON PD10.DF_SPE_ACC_ID = PH05.DF_SPE_ID 
						LEFT JOIN PKUB.AY10_BR_LON_ATY AY10
							ON PD10.DF_PRS_ID = AY10.BF_SSN
							AND AY10.PF_REQ_ACT = 'COWGS'
					WHERE	
						LN10.LC_STA_LON10 = 'R'
						AND LN10.LA_CUR_PRI > 0
						AND SD10.LD_SCL_SPR < CURRENT_DATE
						AND (PD30.DI_VLD_ADR = 'Y' OR PH05.DI_CNC_ELT_OPI = 'Y')
						AND EXCL.LF_STU_SSN IS NULL
						AND AY10.BF_SSN IS NULL

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA STUS; SET LEGEND.STUS; RUN;

/*Insert Keyline*/
DATA CMPLT (DROP = KEYSSN MODAY KEYLINE CHKDIG DIG I CHKDIG CHK1 CHK2 CHK3 CHKDIGIT CHECK BF_SSN);
	SET STUS;
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
	COSTCENTER = 'MA4481';
RUN;

/*write to comma delimited file*/
DATA _NULL_;
	SET	WORK.CMPLT;
	FILE REPORT2
		DELIMITER = ','
		DSD
		DROPOVER
		LRECL = 32767
	;

	FORMAT
		DF_SPE_ACC_ID $10.
	;

	/* write column names, remove this to create a file without a header row */
	IF _N_ = 1 THEN
		DO;
			PUT	
				'ACCOUNT NUMBER'
				','
				'FIRST NAME'
				','
				'LAST NAME'
				','
				'STREET1'
				','
				'STREET2'
				','
				'CITY'
				','
				'STATE'
				','
				'ZIP'
				','
				'FOREIGN STATE'
				','
				'FOREIGN COUNTRY'
				','
				'ACSKEYLINE'
				','
				'COST CENTER'
			;
		END;

	/* write data*/	
	DO;
		PUT DF_SPE_ACC_ID $ @;
		PUT DM_PRS_1 $ @;
		PUT DM_PRS_LST $ @;
		PUT DX_STR_ADR_1 $ @;
		PUT DX_STR_ADR_2 $ @;
		PUT DM_CT $ @;
		PUT DC_DOM_ST $ @;
		PUT DF_ZIP_CDE $ @;
		PUT DM_FGN_ST $ @;
		PUT DM_FGN_CNY $ @;
		PUT ACSKEY $ @;
		PUT COSTCENTER $ ;
		;
	END;
RUN;
