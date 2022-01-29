/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWAB1.LWAB1RZ";
FILENAME REPORT2 "&RPTLIB/ULWAB1.LWAB1R2";
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;
/*LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/

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

	CREATE TABLE DEMO AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						A.DF_PRS_ID			AS SSN
						,A.DM_PRS_LST		AS LAST
						,A.DM_PRS_1 		AS FIRST
						,A.DM_PRS_MID		AS MI
						,DF_ZIP				AS ZIP
					FROM	
						OLWHRM1.PD01_PDM_INF A
					WHERE	
						A.DF_ZIP LIKE '840%'

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA DEMO;
	SET DUSTER.DEMO;
RUN;
PROC SORT DATA=DEMO;
	BY SSN;
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\EXCEL OUTPUT.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
/*FOR LANDSCAPE REPORTS:*/
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127;
/*FOR PORTRAIT REPORTS;*/
OPTIONS ORIENTATION = PORTRAIT;
OPTIONS PS=52 LS=96;
TITLE 'TITLE ONE';
TITLE2	"RUNDATE &RUNDT";
FOOTNOTE1  	"THIS DOCUMENT MAY CONTAIN BORROWERS' SENSITIVE INFORMATION THAT UHEAA HAS PLEDGED TO PROTECT.";
FOOTNOTE2	'PLEASE TAKE APPROPRIATE PRECAUTIONS TO SAFEGUARD THIS INFORMATION.';
FOOTNOTE3	;
FOOTNOTE4   'JOB = UTLWAB1  	 REPORT = ULWAB1.LWAB1R2';

PROC CONTENTS DATA=DEMO OUT=EMPTYSET NOPRINT;
DATA _NULL_;
	SET EMPTYSET;
	FILE PRINT;

	IF  NOBS=0 AND _N_ =1 THEN 
		DO;
			PUT // 132*'-';
			PUT      //////
				@51 '**** NO OBSERVATIONS FOUND ****';
			PUT //////
				@57 '-- END OF REPORT --';
			PUT //////////////
				@46 "JOB = UTLWAB1  	 REPORT = ULWAB1.LWAB1R2";
		END;
	RETURN;
RUN;

PROC PRINT NOOBS SPLIT='/' DATA=DEMO WIDTH=UNIFORM WIDTH=MIN LABEL;
	FORMAT ;
	VAR ;
	LABEL
		BF_SSN = 'SSN'
		LN_SEQ = 'LN SEQ'
		LF_LON_CUR_OWN = 'CURRENT OWNER'
		LD_DSB = 'DISBURSEMENT DATE'
		LD_FAT_PST = 'TRANSACTION POSTED DATE'
		LA_FAT_CUR_PRI = 'DISBURSEMENT AMOUNT'
		TXTYP = 'TRANSACTION TYPE'
		LC_LDR_DSB_MDM = 'DISB MEDIUM CODE'
	;
RUN;

PROC PRINTTO;
RUN;


/*write to comma delimited file*/
DATA _NULL_;
	SET  WORK.DEMO;
	FILE 'T:\SAS\ACH_not_pulled.txt' delimiter=',' DSD DROPOVER lrecl=32767;

	FORMAT DF_SPE_ACC_ID $10. ;
	FORMAT LD_BIL_CRT YYMMDDd10. ;

	IF _N_ = 1 THEN        /* write column names */
		DO;
			PUT
			'DF_SPE_ACC_ID'
			','
			'LD_BIL_CRT';
		END;

	DO;
		PUT DF_SPE_ACC_ID $ @;
		PUT LD_BIL_CRT;
		;
	END;
RUN;

/*write to file for queue builder*/
DATA _NULL_;
	SET  WORK.S60; 
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;

	ARC_NAME = 'CL055';
	FROM_DATE = '';
	TO_DATE = '';
	NEEDED_BY_DATE = '';
	RECIPIENT_ID = '';
	REGARDS_TO_CODE = '';
	REGARDS_TO_ID = '';
	LOAN_SEQ = 'ALL';
	FORMAT COMMENTS $200. ;
	COMMENTS = 'Review MOC and Pacer for Chapter 13';

	DO;
		PUT DF_SPE_ACC_ID $ @;
		PUT ARC_NAME $ @;
		PUT FROM_DATE @;
		PUT TO_DATE @;
		PUT NEEDED_BY_DATE @;
		PUT RECIPIENT_ID @;
		PUT REGARDS_TO_CODE @;
		PUT REGARDS_TO_ID @;
		PUT LOAN_SEQ @;
		PUT COMMENTS $ ;
	END;
RUN;
