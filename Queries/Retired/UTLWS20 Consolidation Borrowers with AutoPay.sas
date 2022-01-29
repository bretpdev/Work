/*UTLWS20*/
/*CONSOLIDATION BORROWER ON AUTOPAY*/

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWS20.LWS20R2";*/
/*FILENAME REPORTZ "&RPTLIB/ULWS20.LWS20SRZ";*/

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
FILENAME REPORT2 "T:\SAS\ULWS20.LWS20R2";

RSUBMIT;

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
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.BF_SSN AS SSN
	,A.IC_LON_PGM
	,A.LD_LON_1_DSB
FROM OLWHRM1.GR10_RPT_LON_APL A
INNER JOIN OLWHRM1.BR30_BR_EFT B
	ON B.BF_SSN = A.BF_SSN
WHERE B.BC_EFT_STA = 'A'
AND DAYS(A.LD_LON_1_DSB) = DAYS(CURRENT DATE) - 2
AND DAYS(B.BD_EFT_STA) < DAYS(CURRENT DATE) - 2
AND A.IC_LON_PGM IN ('SUBCNS','UNCNS','SUBSPC','UNSPC')

FOR READ ONLY WITH UR

);
DISCONNECT FROM DB2;

/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK;*/
/*QUIT;*/

ENDRSUBMIT;

DATA DEMO; 
SET WORKLOCL.DEMO; 
RUN;

PROC SORT DATA=DEMO NODUPKEY;
BY SSN;
RUN;

DATA _NULL_;
SET  DEMO;
FILE REPORT2 ;
FORMAT SSN 9. ;
PUT SSN $ ;
RUN;
