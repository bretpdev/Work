/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE X  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @XX " ********************************************************************* "
              / @XX " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @XX " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @XX " ********************************************************************* "
              / @XX " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @XX " ****  &SQLXMSG   **** "
              / @XX " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

%MACRO TBL_PUL(DSET,TABLE);
PROC SQL ;
CONNECT TO DBX (DATABASE=&DB);
CREATE TABLE &DSET AS
SELECT *
FROM CONNECTION TO DBX (
	SELECT DISTINCT
		A.*
	FROM
		PKUB.&TABLE A
		JOIN PKUB.PDXX_PRS_NME PDXX
			ON A.BF_SSN = PDXX.DF_PRS_ID
	WHERE PDXX.DF_SPE_ACC_ID = 'XXXXXXXXXX'

	FOR READ ONLY WITH UR
				)
	;

DISCONNECT FROM DBX;
%MEND TBL_PUL;
%TBL_PUL(RSXX,RSXX_IBR_RPS);
%TBL_PUL(RSXX,RSXX_BR_RPD);
%TBL_PUL(RSXX,RSXX_IBR_IRL_LON);
%TBL_PUL(LNXX,LNXX_LON_RPS);
QUIT;
ENDRSUBMIT;

%MACRO PUL_DWN(DSET);
DATA &DSET; SET LEGEND.&DSET; RUN;
%MEND PUL_DWN;
%PUL_DWN(RSXX);
%PUL_DWN(RSXX);
%PUL_DWN(RSXX);
%PUL_DWN(LNXX);

%MACRO EXP_EXL(DSET);
PROC EXPORT DATA=&DSET
            OUTFILE= "T:\SAS\NH XXXX.xlsx" 
            DBMS=EXCEL REPLACE;
     SHEET="&DSET"; 
RUN;
%MEND EXP_EXL;
%EXP_EXL(RSXX);
%EXP_EXL(RSXX);
%EXP_EXL(RSXX);
%EXP_EXL(LNXX);

