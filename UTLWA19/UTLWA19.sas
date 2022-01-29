*---------------------------------------*
| UTLWA19 LARS - Interest Data File.sas |
*---------------------------------------**;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWA19.LWA19R2";
FILENAME REPORTZ "&RPTLIB/ULWA19.LWA19RZ";
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
%MACRO SQLCHECK (SQLRPT= );
%IF &SQLXRC NE 0 %THEN %DO;
	DATA _NULL_;
    FILE REPORTZ NOTITLES;
    PUT @01 " ********************************************************************* "
      / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
      / @01 " ****  THE SAS LOG IN &SQLRPT SHOULD BE REVIEWED.          **** "       
      / @01 " ********************************************************************* "
      / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
      / @01 " ****  &SQLXMSG   **** "
      / @01 " ********************************************************************* ";
	RUN;
%END;
%MEND;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE MR82 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT *
FROM OLWHRM1.MR82_MR_799_INT
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWA19.LWA19RZ);*/
/*QUIT;*/
ENDRSUBMIT;

DATA MR82 ;
SET WORKLOCL.MR82;
RUN;

PROC SORT DATA=MR82;BY IF_OWN BF_SSN LN_SEQ;RUN;

DATA _NULL_;
SET MR82;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
   FORMAT IF_799_RPT $8. ;
   FORMAT IF_BND_ISS $8. ;
   FORMAT WC_799_LON_PGM $2. ;
   FORMAT BF_SSN $9. ;
   FORMAT LN_SEQ 6. ;
   FORMAT LR_ITR 7.3 ;
   FORMAT WC_799_INT_BIL $2. ;
   FORMAT WA_799_PRI_END_INT 11.2 ;
   FORMAT WA_799_AVG_BAL_INT 11.2 ;
   FORMAT WA_799_CUR_GOV_INT 11.2 ;
   FORMAT IF_OWN $8. ;
IF _N_ = 1 THEN DO;
	PUT
		'FEDERAL REPORTING CODE' 
		','
		'OWNER BOND ISSUE IDENTIFIER'
		','
		'799 LOAN TYPE'
		','
		'BORROWER SSN'
		','
		'LOAN SEQUENCE NUMBER'
		','
		'INTEREST RATE'
		','
		'799 INTEREST BILLING CODE'
		','
		'ENDING PRINCIPAL BALANCE REPORTED IN 799 PART III'
		','
		'AVERAGE DAILY BALANCE FOR INTEREST ELIGIBILITY'
		','
		'INTEREST AMOUNT BILLED TO THE GOVERNMENT'
		','
		'OWNER CODE';
END;
DO;
   PUT IF_799_RPT $ @;
   PUT IF_BND_ISS $ @;
   PUT WC_799_LON_PGM $ @;
   PUT BF_SSN $ @;
   PUT LN_SEQ @;
   PUT LR_ITR @;
   PUT WC_799_INT_BIL $ @;
   PUT WA_799_PRI_END_INT @;
   PUT WA_799_AVG_BAL_INT @;
   PUT WA_799_CUR_GOV_INT @;
   PUT IF_OWN $ ;
END;
RUN;