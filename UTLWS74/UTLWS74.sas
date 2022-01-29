/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWS74.LWS74R2.&sysdate";
FILENAME REPORT3 "&RPTLIB/ULWS74.LWS74R3.&sysdate";


%LET EFF_ADD_DATE = '01/01/2016';
%SYSLPUT EFF_ADD_DATE = &EFF_ADD_DATE;

/*LIBNAME  DUSTER  REMOTE  SERVER=DUSTER SLIBREF=WORK  ;*/ /*LIVE*/
LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK  ; /*TEST*/
RSUBMIT;
/*%LET DB = DLGSUTWH ;*/ /*LIVE*/
%LET DB = DLGSWQUT ; /*TEST*/

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

    CREATE TABLE R2 AS
        SELECT  
            *
        FROM    
            CONNECTION TO DB2 
                (
                    SELECT DISTINCT
                        PD10.DF_SPE_ACC_ID,
						PD10.DF_PRS_ID AS BF_SSN,
                        PD10.DM_PRS_1,
                        PD10.DM_PRS_MID,
                        PD10.DM_PRS_LST,
                        PD30.DX_STR_ADR_1,
                        PD30.DX_STR_ADR_2,
                        PD30.DX_STR_ADR_3,
                        PD30.DM_CT,
                        PD30.DC_DOM_ST,
                        PD30.DF_ZIP_CDE,
                        PD30.DM_FGN_CNY,
                        PD30.DM_FGN_ST,
/*						ACS KEYLINE NEEDS TO BE INSERTED STILL*/
                        'MA2324' AS COST_CENTER_CODE
                    FROM OLWHRM1.LN10_LON LN10
                    	INNER JOIN OLWHRM1.PD30_PRS_ADR PD30
                        	ON LN10.BF_SSN = PD30.DF_PRS_ID
                        	AND PD30.DI_VLD_ADR = 'Y'
                        	AND PD30.DC_ADR = 'L'
                    	LEFT OUTER JOIN OLWHRM1.PD10_PRS_NME PD10
                        	ON LN10.BF_SSN = PD10.DF_PRS_ID
                    WHERE 
						LN10.LC_STA_LON10 = 'P'
                    	AND LN10.LF_LON_CUR_OWN IN ('829769','82976901','82976902','82976903','82976904','82976905','82976906','82976907','82976908')
                    	AND LN10.LA_CUR_PRI > 0
                        

                    FOR READ ONLY WITH UR
                )
    ;
        DISCONNECT FROM DB2;

QUIT;
PROC SQL ;
    CONNECT TO DB2 (DATABASE=&DB);

    CREATE TABLE R3 AS
        SELECT *
        FROM CONNECTION TO DB2
            (
                SELECT DISTINCT
                    PD10.DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
                    PD10.DM_PRS_1||PD10.DM_PRS_MID||PD10.DM_PRS_LST AS NAME,
                    COALESCE(PD32A.DX_ADR_EML,PD32B.DX_ADR_EML,PD32C.DX_ADR_EML) AS RECIPIENT
                FROM OLWHRM1.LN10_LON LN10
	                LEFT OUTER JOIN OLWHRM1.PD32_PRS_ADR_EML PD32A
	                    ON LN10.BF_SSN = PD32A.DF_PRS_ID
	                    AND PD32A.DC_ADR_EML = 'H'
	                    AND PD32A.DC_STA_PD32 = 'A'
	                    AND PD32A.DI_VLD_ADR_EML = 'Y'
	                LEFT OUTER JOIN OLWHRM1.PD32_PRS_ADR_EML PD32B
	                    ON LN10.BF_SSN = PD32B.DF_PRS_ID
	                    AND PD32B.DC_ADR_EML = 'A'
	                    AND PD32B.DC_STA_PD32 = 'A'
	                    AND PD32B.DI_VLD_ADR_EML = 'Y'
	                LEFT OUTER JOIN OLWHRM1.PD32_PRS_ADR_EML PD32C
	                    ON LN10.BF_SSN = PD32C.DF_PRS_ID
	                    AND PD32C.DC_ADR_EML = 'W'
	                    AND PD32C.DC_STA_PD32 = 'A'
	                    AND PD32C.DI_VLD_ADR_EML = 'Y'
	                LEFT OUTER JOIN OLWHRM1.PD10_PRS_NME PD10
	                    ON LN10.BF_SSN = PD10.DF_PRS_ID
                WHERE 
					LN10.LC_STA_LON10 = 'R'
	                AND LN10.LF_LON_CUR_OWN IN ('829769','82976901','82976902','82976903','82976904','82976905','82976906' ,'82976907','82976908')
	                AND LN10.LD_LON_EFF_ADD = &EFF_ADD_DATE
	                AND LN10.LA_CUR_PRI > 0
	                AND (
							PD32A.DX_ADR_EML IS NOT NULL 
							OR PD32B.DX_ADR_EML IS NOT NULL 
							OR PD32C.DX_ADR_EML IS NOT NULL
						)

                FOR READ ONLY WITH UR
                )
;

    DISCONNECT FROM DB2;

QUIT;

ENDRSUBMIT;

DATA R2;
	SET DUSTER.R2;
RUN;
DATA R3;
	SET DUSTER.R3;
RUN;

*CALCULATE ACS KEYLINE;
%MACRO KEY_CLC(TBL);
DATA &TBL (DROP = KEYSSN MODAY KEYLINE CHKDIG DIG I 
	CHKDIG CHK1 CHK2 CHK3 CHKDIGIT CHECK);
	SET &TBL;
	KEYSSN = TRANSLATE(BF_SSN,'MYLAUGHTER','0987654321');
	MODAY = PUT(DATE(),MMDDYYN4.);
	KEYLINE = "P"||KEYSSN||MODAY||DC_ADR;
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
%MEND KEY_CLC;

%KEY_CLC(R2);

/*Output R2*/
DATA _NULL_;
	SET R2;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
	FORMAT DF_SPE_ACC_ID $10.;
	FORMAT DM_PRS_1 $13. ;
	FORMAT DM_PRS_MID $23. ;
	FORMAT DM_PRS_LST $23. ;
    FORMAT DX_STR_ADR_1 $30. ;
	FORMAT DX_STR_ADR_2 $30. ;
	FORMAT DX_STR_ADR_3 $30. ;
	FORMAT DM_CT $20. ;
	FORMAT DC_DOM_ST $2. ;
	FORMAT DF_ZIP_CDE $17. ;
	FORMAT DM_FGN_CNY $25. ;
	FORMAT DM_FGN_ST $15. ;
	FORMAT ACSKEY $18. ;
	FORMAT COST_CENTER_CODE $6.;


	IF _N_ = 1 THEN        
	DO;
		PUT
		'AccountNumber' ','
		'FIRST_NAME' ','
		'MIDDLE_NAME' ','
		'LAST_NAME' ','
		'STREET_1' ','
		'STREET_2' ','
		'STREET_3' ','
        'City' ','
        'State' ','
		'ZIP' ','
		'FOREIGN_COUNTRY' ','
        'FOREIGN_STATE' ','
		'ACS_KEYLINE' ','
		'COST_CENTER_CODE';
	END;
	DO;
		PUT DF_SPE_ACC_ID $ @;
		PUT DM_PRS_1 $ @;
		PUT DM_PRS_MID $ @ ;
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
		PUT COST_CENTER_CODE $ ;
	END;
RUN;

/*Output R3*/
DATA _NULL_;
	SET R3;
	FILE REPORT3 DELIMITER=',' DSD DROPOVER LRECL=32767;
	FORMAT ACCOUNT_NUMBER $10.;
	FORMAT NAME $200. ;
	FORMAT RECIPIENT $200. ;

	/* WRITE COLUMN NAMES */
	IF _N_ = 1 THEN        
	DO;
		PUT
		'ACCOUNT NUMBER' ','
		'NAME' ','
		'RECIPIENT' ;
	END;
	DO;
		PUT ACCOUNT_NUMBER $ @;
		PUT NAME $ @;
		PUT RECIPIENT $ ;
	END;
RUN;
