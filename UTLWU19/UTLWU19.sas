/*UTLWU19 - 1027 Transaction Non-UHEAA Loan - QC Report*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWU19.LWU19RZ";
FILENAME REPORT2 "&RPTLIB/ULWU19.LWU19R2";

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
CREATE TABLE QUERY AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT
	B.DF_SPE_ACC_ID,
	A.LN_SEQ,
	B.DM_PRS_LST,
	C.LD_FAT_EFF,
	ABS(C.LA_FAT_CUR_PRI)	AS LA_FAT_CUR_PRI
FROM	OLWHRM1.LN10_LON A
	INNER JOIN OLWHRM1.PD10_PRS_NME B
		ON A.BF_SSN = B.DF_PRS_ID
	INNER JOIN OLWHRM1.LN90_FIN_ATY C
		ON A.BF_SSN = C.BF_SSN
		AND A.LN_SEQ = C.LN_SEQ
WHERE	A.LA_CUR_PRI > 0
	AND A.LC_STA_LON10 = 'R'
	AND A.LF_LON_CUR_OWN <> '828476'
	AND A.LF_LON_CUR_OWN <> '834529'
	AND C.PC_FAT_TYP = '10'
	AND C.PC_FAT_SUB_TYP = '27'
	AND C.LC_STA_LON90 = 'A'
	AND C.LC_FAT_REV_REA = ''
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;
DATA QUERY;
SET WORKLOCL.QUERY;
RUN;
PROC SORT DATA=QUERY;
BY DF_SPE_ACC_ID;
RUN;

DATA _NULL_;
SET QUERY ;
LENGTH DESCRIPTION $600.;
USER = '';
ACT_DT = LD_FAT_EFF;
DESCRIPTION = CATX(',',
	'BORROWER ACCOUNT NUMBER = '||DF_SPE_ACC_ID,
	'LOAN SEQUENCE NUMBER = '||TRIM(LEFT(LN_SEQ)),
	'BORROWER LAST NAME = '||DM_PRS_LST,   
	'EFFECTIVE DATE OF 1027 = '||(PUT(LD_FAT_EFF,MMDDYY10.)),
	'AMOUNT	OF 1027 = '|| TRIM(LEFT(LA_FAT_CUR_PRI))
);
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT USER $10. ;
FORMAT ACT_DT MMDDYY10. ;
FORMAT DESCRIPTION $600. ;
IF _N_ = 1 THEN DO;
	PUT "USER,ACT_DT,DESCRIPTION";
END;
DO;
   PUT USER $ @;
   PUT ACT_DT @;
   PUT DESCRIPTION $ ;
END;
RUN;
