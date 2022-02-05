/*UTLWA14 - AFCU - LARS Orig Fee MR81 Table*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWA14.LWA14RZ";
FILENAME REPORT2 "&RPTLIB/ULWA14.LWA14R2";

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;

%macro sqlcheck ;
  %if  &sqlxrc ne 0  %then  %do  ;
    data _null_  ;
            file reportz notitles  ;
            put @01 " ********************************************************************* "
              / @01 " ****  The SQL code above has experienced an error.               **** "
              / @01 " ****  The SAS should be reviewed.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  The SQL error code is  &sqlxrc  and the SQL error message  **** "
              / @01 " ****  &sqlxmsg   **** "
              / @01 " ********************************************************************* "
            ;
         run  ;
  %end  ;
%mend  ;


PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE AFCU AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT
	BF_SSN,
	LN_SEQ,
	WC_799_LON_PGM,
	WC_799_RTE_ORG_FEE,
	WC_799_ORG_FEE,
	WC_799_ITR_ORG_FEE,
	WA_PRI_AMT_ORG,
	WA_799_ORG_FEE,
	PC_FAT_TYP,
	PC_FAT_SUB_TYP,
	IF_OWN
FROM	OLWHRM1.MR81_MR_799_ORG
WHERE	IF_OWN = '822373'
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;
DATA AFCU;
SET WORKLOCL.AFCU;
RUN;
PROC SORT DATA=AFCU;
BY BF_SSN LN_SEQ;
RUN;

DATA _NULL_;
SET  WORK.AFCU;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
   FORMAT BF_SSN $9. ;
   FORMAT LN_SEQ 3. ;
   FORMAT WC_799_LON_PGM $2. ;
   FORMAT WC_799_RTE_ORG_FEE $5. ;
   FORMAT WC_799_ORG_FEE $2. ;
   FORMAT WC_799_ITR_ORG_FEE $6. ;
   FORMAT WA_PRI_AMT_ORG 11.2 ;
   FORMAT WA_799_ORG_FEE 11.2 ;
   FORMAT PC_FAT_TYP $2.;
   FORMAT PC_FAT_SUB_TYP $2.;
   FORMAT IF_OWN $8.;
IF _N_ = 1 THEN        /* write column names */
 DO;
   PUT
   'SSN,'
   'LoanSeq,'
   'LoanType,'
   'OrigFeeRate,'
   'OrigFeeCode,'
   'LoanIntRate,'
   'PrincipalAmt,'
   'AmtOfOF,'
   'FinActType,'
   'FinActSubtype,'
   'Owner'
	;
 END;
 DO;
   PUT BF_SSN $ @;
   PUT LN_SEQ @;
   PUT WC_799_LON_PGM $ @;
   PUT WC_799_RTE_ORG_FEE $ @;
   PUT WC_799_ORG_FEE $ @;
   PUT WC_799_ITR_ORG_FEE $ @;
   PUT WA_PRI_AMT_ORG @;
   PUT WA_799_ORG_FEE @;
   PUT PC_FAT_TYP $ @;
   PUT PC_FAT_SUB_TYP $ @;
   PUT IF_OWN $;
 END;
RUN;
