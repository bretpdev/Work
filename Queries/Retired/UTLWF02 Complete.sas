****************************************************************************;
*UTLWF02																    ;
****************************************************************************;
*THIS JOB WAS ORIGINALLY CREATED BY AES AND RAN AGAINST PRODUCTION TABLES   ;	 
*BUT AFTER THE MAINFRAM REPLATFORM (2005) THIS JOB STOPPED WORKING. THE RM02; 
*TABLE WAS ADDED TO THE UHEAA WAREHOUSE AND THEN THE JOB WAS MODIFIED TO RUN;
*AGAINST THE WARESHOUSE AND THE JOB THEN WORKED AS EXPECTED.			    ; 
****************************************************************************;

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWF02.LWF02R2";
FILENAME REPORTZ "&RPTLIB/ULWF02.LWF02RZ";*/

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;

%MACRO SQLCHECK (SQLRPT= );
%IF &SQLXRC NE 0 %THEN %DO;
	FILENAME REPORTZ "&RPTLIB/&SQLRPT";
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
CREATE TABLE REMIT AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT   LD_RMT_BCH_INI
        ,LN_RMT_BCH_SEQ
        ,LF_CRT_DTS_RM02
        ,LA_RMT_TRX
        ,LD_RMT_TRX_EFF
        ,LC_RMT_TRX_TYP
        ,DF_PRS_ID
        ,LF_CLM_ID
FROM OLWHRM1.RM02_DFL_RMT_DTL
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  *INCLUDES ERROR MESSAGES TO SAS LOG;*/
/*%SQLCHECK (SQLRPT=ULWF02.LWF02RZ);*/
/*QUIT;*/

ENDRSUBMIT;

DATA REMIT;
SET WORKLOCL.REMIT;
RUN;

DATA REMIT;
 SET REMIT;
 BATCHDT=PUT(LD_RMT_BCH_INI,MMDDYY10.);
 BATCH=BATCHDT||PUT(LN_RMT_BCH_SEQ,Z11.);

PROC SORT;
BY BATCH lf_crt_dts_rm02 DF_PRS_ID LF_CLM_ID LD_RMT_TRX_EFF;
RUN;

OPTIONS LS=80 PS=58 nocenter errors = 0;

/*PROC PRINTTO PRINT=REPORT2 NEW;*/
/*RUN;*/
%MACRO RPTPRNT(DSNAME);
   %LET DSID=%SYSFUNC(OPEN(&DSNAME));
   %LET HASOBS=%SYSFUNC(ATTRN(&DSID,ANY));
   %LET RC=%SYSFUNC(CLOSE(&DSID));
   %IF &HASOBS=1 %THEN %DO;
		PROC PRINT NOOBS SPLIT='/' N DATA=&DSNAME;
		BY BATCH;
		VAR DF_PRS_ID LA_RMT_TRX LD_RMT_TRX_EFF LC_RMT_TRX_TYP LF_CLM_ID;
		LABEL   DF_PRS_ID      = 'SS NUMBER'
		        LF_CLM_ID      = 'CLAIM ID'
		        LD_RMT_TRX_EFF = 'EFFECTIVE/DATE'
		        LA_RMT_TRX     = 'REMITTED/AMOUNT'
		        LC_RMT_TRX_TYP = 'TRANSACTION/TYPE';
		SUM LA_RMT_TRX;
		FORMAT  LA_RMT_TRX DOLLAR16.2
		        LD_RMT_TRX_EFF MMDDYY10.;
		TITLE1 'REMITTANCE DETAIL';
		RUN;
     %END;
   %ELSE %IF &HASOBS=0 %THEN
      %DO;
         DATA _NULL_;
            FILE PRINT NOTITLES;
            PUT @01 'NO BATCH RECORDS FOUND';
         RUN;
      %END;
%MEND RPTPRNT;
%RPTPRNT(REMIT);