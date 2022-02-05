/*UTLWDP8 - Enrolled Preclaim Accounts*/

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
/*FILENAME REPORTZ "&RPTLIB/ULWDP8.LWDP8RZ";*/
/*FILENAME REPORT2 "&RPTLIB/ULWDP8.LWDP8R2";*/

FILENAME REPORT2 "T:\SAS\ULWDP8.LWDP8R2.TEST.TXT";

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
CREATE TABLE ENRPRE AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	A.BF_SSN
	,B.LD_XPC_GRD
	,B.LD_ENR_CER
FROM	OLWHRM1.DC01_LON_CLM_INF A
		INNER JOIN OLWHRM1.SD02_STU_ENR B ON
			A.BF_SSN = B.DF_PRS_ID_STU
			AND A.LC_STA_DC10 = '01'
			AND A.LC_PCL_REA IN ('DB','DF','DQ')
			AND B.LC_STA_SD02 = 'A'
			AND B.LC_STU_ENR_STA = 'I'
			AND DAYS(B.LD_XPC_GRD) > DAYS(CURRENT DATE)
ORDER BY BF_SSN

FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;

DATA ENRPRE;
SET WORKLOCL.ENRPRE;
RUN;

DATA _NULL_;
SET  WORK.ENRPRE;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;

FORMAT BF_SSN $9. ;
FORMAT LD_XPC_GRD MMDDYY10. ;
FORMAT LD_ENR_CER MMDDYY10. ;

DO;
PUT BF_SSN $ @;
PUT LD_XPC_GRD $ @;
PUT LD_ENR_CER $ ;
END;
RUN;
