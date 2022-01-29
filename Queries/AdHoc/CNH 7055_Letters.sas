%LET RPTLIB = T:\SAS;
/*LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn; update_lock_typ=nolock; bl_keepnulls=no";*/
/*%LET DSN = 'FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn;';*/
LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS.dsn; update_lock_typ=nolock; bl_keepnulls=no";
%LET DSN = 'FILEDSN=X:\PADR\ODBC\CLS.dsn;';

PROC SQL;
	CREATE TABLE SOURCE AS
		SELECT
			SSN
		FROM
			SQL.DUEDATECHANGE
		WHERE
			SUCCESSFUL = X
;
QUIT;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
DATA LEGEND.SOURCE; *Send data to Duster;
SET SOURCE;
RUN;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;


LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;
LIBNAME AES DBX DATABASE=&DB OWNER=AES;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE X  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @XX " ********************************************************************* "
              / @XX " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @XX " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @XX " ********************************************************************* "
              / @XX " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @XX " ****  &SQLXMSG   **** "
              / @XX " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL;	
	CREATE TABLE LETTERS AS 
		SELECT DISTINCT
			PDXX.DF_SPE_ACC_ID,
			PDXX.DF_PRS_ID,
			PDXX.DM_PRS_X,
			PDXX.DM_PRS_LST,
			PDXX.DX_STR_ADR_X,
			PDXX.DX_STR_ADR_X,
			PDXX.DM_CT,
			PDXX.DC_DOM_ST,
			PDXX.DF_ZIP_CDE,
			PDXX.DM_FGN_CNY,
			PDXX.DM_FGN_ST
		FROM
			SOURCE S
			INNER JOIN PKUB.PDXX_PRS_NME PDXX
				ON S.SSN = PDXX.DF_PRS_ID
			INNER JOIN PKUB.PDXX_PRS_ADR PDXX
				ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
			LEFT JOIN AES.PHXX_CNC_EML PHXX
				ON PHXX.DF_SPE_ID = INPUT(PDXX.DF_SPE_ACC_ID,BESTXX.)
			WHERE
				((PDXX.DI_VLD_ADR = 'Y' AND PDXX.DC_ADR = 'L') OR (PHXX.DI_VLD_CNC_EML_ADR = 'Y' AND DI_CNC_ELT_OPI = 'Y'))
;
QUIT;

%MACRO ACS(NEW_NAME,POP);
	DATA &NEW_NAME (DROP = KEYSSN MODAY KEYLINE CHKDIG DIG I CHKDIG CHKX CHKX CHKX CHKDIGIT CHECK);
		SET &POP;
		KEYSSN = TRANSLATE(DF_PRS_ID,'MYLAUGHTER','XXXXXXXXXX');
		MODAY = PUT(DATE(),MMDDYYNX.);
		KEYLINE = "P"||KEYSSN||MODAY||"L";
		CHKDIG = X;
		LENGTH DIG $X.;
		DO I = X TO LENGTH(KEYLINE);
			IF I/X NE ROUND(I/X,X) 
				THEN DIG = PUT(INPUT(SUBSTR(KEYLINE,I,X),BITSX.X) * X, X.);
			ELSE DIG = PUT(INPUT(SUBSTR(KEYLINE,I,X),BITSX.X), X.);
			IF SUBSTR(DIG,X,X) = " " 
				THEN CHKDIG = CHKDIG + INPUT(SUBSTR(DIG,X,X),X.);
				ELSE DO;
					CHKX = INPUT(SUBSTR(DIG,X,X),X.);
					CHKX = INPUT(SUBSTR(DIG,X,X),X.);
					IF CHKX + CHKX >= XX
						THEN DO;
							CHKX = PUT(CHKX + CHKX,X.);
							CHKX = INPUT(SUBSTR(CHKX,X,X),X.);
							CHKX = INPUT(SUBSTR(CHKX,X,X),X.);
						END;
					CHKDIG = CHKDIG + CHKX + CHKX;
				END;
		END;
		CHKDIGIT = XX - INPUT(SUBSTR((RIGHT(PUT(CHKDIG,X.))),X,X),X.);
		IF CHKDIGIT = XX THEN CHKDIGIT = X;
		CHECK = PUT(CHKDIGIT,X.);
		ACSKEY = "#"||KEYLINE||CHECK||"#";
	RUN;
%MEND;

%ACS(LETTERS,LETTERS);

ENDRSUBMIT;

DATA LETTERS; SET LEGEND.LETTERS; RUN;

DATA _NULL_;
	SET LETTERS;
	FILE "&RPTLIB/NH XXXX_LETTER_POP.txt" DELIMITER=',' DSD DROPOVER LRECL=XXXXX;

	IF _N_ = X THEN
		DO;
			PUT
				'DF_SPE_ACC_ID'
				','
				'DM_PRS_X'
				','
				'DM_PRS_LST'
				','
				'DX_STR_ADR_X'
				','
				'DX_STR_ADR_X'
				','
				'DM_CT'
				','
				'DC_DOM_ST'
				','
				'DF_ZIP_CDE'
				','
				'DM_FGN_CNY'
				','
				'DM_FGN_ST'
				','
				'ACSKeyline'
				','
				'CostCenter';

		END;
	DO;
		PUT DF_SPE_ACC_ID $ @;
		PUT DM_PRS_X $ @;
		PUT DM_PRS_LST $ @;
		PUT DX_STR_ADR_X $ @;
		PUT DX_STR_ADR_X $ @;
		PUT DM_CT $ @;
		PUT DC_DOM_ST $ @;
		PUT DF_ZIP_CDE $ @;
		PUT DM_FGN_CNY $ @;
		PUT DM_FGN_ST $ @;
		PUT ACSKEY $ @;
		PUT 'MAXXXX' ;

	;
	END;
RUN;
