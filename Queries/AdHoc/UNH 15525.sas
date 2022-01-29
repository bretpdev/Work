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
SELECT *
FROM CONNECTION TO DB2 (
SELECT
	BL10.BF_SSN,
	PUT(BL10.LD_BIL_CRT), DDMMYYD10. AS Date,	
	BL10.LC_STA_BIL10,		
	BL10.LC_IND_BIL_SNT,
	BR30.BC_EFT_STA		
				
FROM	
	OLWHRM1.BL10_BR_BIL BL10
INNER JOIN OLWHRM1.BR30_BR_EFT BR30
	ON BR30.BF_SSN = BL10.BF_SSN

WHERE	BL10.LC_STA_BIL10 = 'A' 
		AND  BL10.LC_IND_BIL_SNT = 'F'
		AND BR30.BC_EFT_STA = 'A' 
		AND BL10.LD_BIL_CRT > '01/01/2012'
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK;*/
/*QUIT;*/

ENDRSUBMIT;

DATA DEMO;
	SET DUSTER.DEMO;
RUN;

PROC EXPORT DATA= WORK.DEMO 
            OUTFILE= "T:\SAS\NH15525.xlsx" 
            DBMS=EXCEL REPLACE;
     SHEET="A"; 
RUN;
