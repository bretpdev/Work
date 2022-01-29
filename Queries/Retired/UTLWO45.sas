*------------------------------------------------*
| UTLWO45 - NEW CONSOLIDATION DISCLOSURE REQUEST |
*------------------------------------------------*;
LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
/*%LET RPTLIB = C:\WINDOWS\TEMP;*/
FILENAME REPORT2 "&RPTLIB/ULWO45.LWO45R2";
FILENAME REPORTZ "&RPTLIB/ULWO45.LWO45RZ";
DATA _NULL_;
     CALL SYMPUT('PRVDAY',"'"||PUT(INTNX('DAY',TODAY(),-1,'BEGINNING'), MMDDYYD10.)||"'");
RUN;
/*%SYSLPUT PRVDAY = &PRVDAY;*/
/*LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;*/
/*RSUBMIT;*/
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
CREATE TABLE NCDR AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.BF_SSN
FROM OLWHRM1.LN10_LON A
WHERE A.IC_LON_PGM IN (
	'SUBSPC','UNSPC','SUBCNS','UNCNS'
	)
AND A.LD_LON_1_DSB = &PRVDAY
ORDER BY A.BF_SSN
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;
%SQLCHECK (SQLRPT=ULWO45.LWO45RZ);
QUIT;
/*ENDRSUBMIT;*/
/*DATA NCDR;*/
/*SET WORKLOCL.NCDR;*/
/*RUN;*/
DATA _NULL_;
SET NCDR;
FILE REPORT2 DLM=',' DSD DROPOVER LRECL=32767;
FORMAT BF_SSN $9.;
DO;
	PUT BF_SSN $ ;
END;
RUN;
