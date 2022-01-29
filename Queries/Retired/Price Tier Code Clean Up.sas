/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWPRICE_TIER.LWPRICE_TIERRZ";
FILENAME REPORT2 "&RPTLIB/ULWPRICE_TIER.LWPRICE_TIERR2";
FILENAME REPORT3 "&RPTLIB/ULWPRICE_TIER.LWPRICE_TIERR3";
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
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	B.DF_SPE_ACC_ID		
	,A.AF_APL_ID		
	,A.LN_SEQ 			
	,A.AN_SEQ
	,C.LD_LON_1_DSB	
FROM OLWHRM1.LN15_DSB A
INNER JOIN OLWHRM1.PD10_PRS_NME	B
	ON A.BF_SSN = B.DF_PRS_ID
INNER JOIN  OLWHRM1.LN10_LON C
	ON A.BF_SSN = C.BF_SSN 
	AND A.LN_SEQ = C.LN_SEQ
INNER JOIN  OLWHRM1.LN18_DSB_FEE D
	ON A.BF_SSN = D.BF_SSN
	AND A.LN_BR_DSB_SEQ = D.LN_BR_DSB_SEQ
WHERE D.LC_DSB_FEE = '21'
	AND D.LA_DSB_FEE > 0
	AND C.IF_TIR_PCE = 'OFD'
	AND C.LC_STA_LON10 = 'R'
	AND C.LA_CUR_PRI > 0
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

ENDRSUBMIT;
DATA DEMO2 DEMO3;
	SET WORKLOCL.DEMO;
	IF LN_SEQ = . THEN OUTPUT DEMO3;
	ELSE OUTPUT DEMO2;
RUN;

/*FOR PORTRAIT REPORTS;*/
OPTIONS ORIENTATION = PORTRAIT;
OPTIONS PS=52 LS=96;
DATA _NULL_;
     CALL SYMPUT('RUNDT',PUT(INTNX('DAY',TODAY(),0), WORDDATE12.));
RUN;
%MACRO REPORTS(REP);
PROC SORT DATA=DEMO&REP;
	BY DF_SPE_ACC_ID;
RUN;
TITLE 'PRICE TIER CODE CLEAN UP';
/*TITLE2	"RUNDATE &RUNDT";*/
FOOTNOTE1  	"THIS DOCUMENT MAY CONTAIN BORROWERS' SENSITIVE INFORMATION THAT UHEAA HAS PLEDGED TO PROTECT.";
FOOTNOTE2	'PLEASE TAKE APPROPRIATE PRECAUTIONS TO SAFEGUARD THIS INFORMATION.';
FOOTNOTE3	;
FOOTNOTE4   "JOB = PRICE TIER CODE CLEAN UP  	 REPORT = ULWPRICE_TIER.LWPRICE_TIERR&REP";
OPTIONS PAGENO=1;

DATA _NULL_;
SET DEMO&REP ;
FILE REPORT&REP DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT LD_LON_1_DSB MMDDYY10.;
IF _N_ = 1 THEN DO;
	PUT "ACCOUNT #,APP ID,LOAN SEQ #,1ST DISB";
END;
DO;
   PUT DF_SPE_ACC_ID @;
   PUT AF_APL_ID @;
   PUT LN_SEQ @ ;
   PUT LD_LON_1_DSB $ ;
END;
RUN;

%MEND;
%REPORTS(2);
%REPORTS(3);



/*PROC EXPORT DATA= WORK.DEMO */
/*            OUTFILE= "T:\SAS\Price Tier Code Clean Up %sysfunc(translate(%sysfunc(datetime(),datetime20.3),--,.:)).csv"*/
/*            DBMS=CSV REPLACE;*/
/*     PUTNAMES=YES;*/
/*RUN;*/
