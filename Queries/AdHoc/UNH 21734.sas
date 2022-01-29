/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS46.NWS46RZ";
FILENAME REPORT2 "&RPTLIB/UNWS46.NWS46R2";

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER SLIBREF=work  ;
RSUBMIT DUSTER;
/*%let DB = DNFPRQUT;  *This is test;*/
%let DB = DLGSUTWH;  *This is live;

LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1;

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

%MACRO TBL_PUL(DSET,TABLE);
PROC SQL ;
CONNECT TO DB2 (DATABASE=&DB);
CREATE TABLE &DSET AS
SELECT *
FROM CONNECTION TO DB2 (
	SELECT DISTINCT
		A.*
	FROM
		OLWHRM1.&TABLE A

	FOR READ ONLY WITH UR
				)
	;

DISCONNECT FROM DB2;
%MEND TBL_PUL;
%TBL_PUL(LP13,LP13_DD_ACT_STA);
%TBL_PUL(LP14,LP14_DD_ACT_WDO);
%TBL_PUL(LP15,LP15_DD_ACT_STE);
%TBL_PUL(LP17,LP17_STP_PUR);
%TBL_PUL(LP18,LP18_DD_CYC_PAR);
%TBL_PUL(LP19,LP19_DD_SKP_PAR);
QUIT;
ENDRSUBMIT;

%MACRO PUL_DWN(DSET);
DATA &DSET; SET DUSTER.&DSET; RUN;
%MEND PUL_DWN;
%PUL_DWN(LP13);
%PUL_DWN(LP14);
%PUL_DWN(LP15);
%PUL_DWN(LP17);
%PUL_DWN(LP18);
%PUL_DWN(LP19);

%MACRO EXP_EXL(DSET);
PROC EXPORT DATA=&DSET
            OUTFILE= "T:\SAS\NH 21734 V2.xlsx" 
            DBMS=EXCEL REPLACE;
     SHEET="&DSET"; 
RUN;
%MEND EXP_EXL;
%EXP_EXL(LP13);
%EXP_EXL(LP14);
%EXP_EXL(LP15);
%EXP_EXL(LP17);
%EXP_EXL(LP18);
%EXP_EXL(LP19);

