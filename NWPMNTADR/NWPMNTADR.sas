/*Setup pull and save directories for later*/
%LET RPTLIB = X:\PADD\FTP;
FILENAME REPORT2 "&RPTLIB/NWPMNTADR.R2.&sysdate";

/*UNCOMMENT BELOW FOR TEST*/
/*%LET drive=T:; 
%LET dir1=%NRSTR(SAS\Bana);
%LET dir2=%NRSTR(SAS\Bana\TestCopy);*/

/*UNCOMMENT BELOW FOR LIVE*/
%let drive=X:; 
%let dir1=%nrstr(PADD\Accounting);
%let dir2=%nrstr(PADD\Accounting\BANA Lockbox);

DATA WORK.LOCKBOX;
	LENGTH Borrower_SSN 8 Borrrower_Name $40 Effective_Date 8 Payment_Amount 8;
	FORMAT Effective_Date MMDDYY10. Payment_Amount BEST12.;
RUN;
  
%macro MultImp(dir=,out=);

%let rc=%str(%'dir %")&dir.%str(\%"*.csv /A-D-H-S/B/ON%');
filename myfiles pipe %unquote(&rc);

data list;
length fname $256.;
infile myfiles truncover;
input myfiles $100.;

fname=quote(upcase(cats("&dir",'\',myfiles)));
out="&out";
drop myfiles;
call execute('
  proc import dbms=csv out= _test
            datafile= '||fname||' replace ;
  run;
  proc append data=_test base='||out||' force; run;
  proc delete data=_test; run;
');
run;
filename myfiles clear;

%mend;

%MultImp(dir=&drive\&dir1,out=WORK.LOCKBOX);

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER SLIBREF=WORK  ; /*LIVE*/
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK  ;*/ /*TEST*/

DATA DUSTER.LOCKBOX; /*Move data to DUSTER*/
    SET LOCKBOX;
	SSN = PUT(Borrower_SSN,z9.);
RUN;

RSUBMIT;
%LET DB = DLGSUTWH ; /*LIVE*/
/*%LET DB = DLGSWQUT ;*/ /*TEST*/
LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1;
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
CREATE TABLE R2 AS
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
			/*ACS KEYLINE NEEDS TO BE INSERTED STILL*/
			'MA2324' AS COST_CENTER_CODE
		FROM WORK.LOCKBOX LCK
			INNER JOIN OLWHRM1.PD10_PRS_NME PD10
				ON LCK.SSN = PD10.DF_PRS_ID
			INNER JOIN OLWHRM1.LN10_LON LN10
				ON PD10.DF_PRS_ID = LN10.BF_SSN
			INNER JOIN OLWHRM1.PD30_PRS_ADR PD30
				ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
				AND PD30.DI_VLD_ADR = 'Y'
				AND PD30.DC_ADR = 'L'
		WHERE 
			LN10.LC_STA_LON10 = 'R'
			AND LN10.LF_LON_CUR_OWN IN ('829769','82976901','82976902','82976903','82976904','82976905','82976906','82976907','82976908')
			AND LN10.LA_CUR_PRI > 0
;

QUIT;
ENDRSUBMIT;

DATA R2; SET DUSTER.R2; RUN;

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
/*Move file from processing folder to archive folder*/
options noxwait;
%sysExec move /y "&drive.\&dir1\*" "&drive.\&dir2";
