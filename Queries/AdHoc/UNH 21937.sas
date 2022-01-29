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
	WHERE 
		A.PF_LST_DTS_&DSET BETWEEN '01/01/2014' AND '12/31/2014'

	FOR READ ONLY WITH UR
				)
	;

DISCONNECT FROM DB2;
%MEND TBL_PUL;
%TBL_PUL(LP01,LP01_CAP_OPT_RUL);
%TBL_PUL(LP02,LP02_DFR_PAR);
%TBL_PUL(LP03,LP03_RDI);
%TBL_PUL(LP04,LP04_UPD_SQR_ORD);
%TBL_PUL(LP05,LP05_FOR_PAR);
%TBL_PUL(LP06,LP06_ITR_AND_TYP);
%TBL_PUL(LP08,LP08_PAY_APL_PAR);
%TBL_PUL(LP09,LP09_OWN_DSB_PAR);
%TBL_PUL(LP10,LP10_RPY_PAR);
%TBL_PUL(LP11,LP11_WUP_RFD_PAR);
%TBL_PUL(LP12,LP12_GTR_DSB_PAR);
%TBL_PUL(LP13,LP13_DD_ACT_STA);
%TBL_PUL(LP14,LP14_DD_ACT_WDO);
%TBL_PUL(LP15,LP15_DD_ACT_STE);
%TBL_PUL(LP17,LP17_STP_PUR);
%TBL_PUL(LP18,LP18_DD_CYC_PAR);
%TBL_PUL(LP19,LP19_DD_SKP_PAR);
%TBL_PUL(LP20,LP20_GTR_OWN_FEE);
QUIT;
ENDRSUBMIT;

%MACRO PUL_DWN(DSET);
DATA &DSET; SET DUSTER.&DSET; RUN;
%MEND PUL_DWN;
%PUL_DWN(LP01);
%PUL_DWN(LP02);
%PUL_DWN(LP03);
%PUL_DWN(LP04);
%PUL_DWN(LP05);
%PUL_DWN(LP06);
%PUL_DWN(LP08);
%PUL_DWN(LP09);
%PUL_DWN(LP10);
%PUL_DWN(LP11);
%PUL_DWN(LP12);
%PUL_DWN(LP13);
%PUL_DWN(LP14);
%PUL_DWN(LP15);
%PUL_DWN(LP17);
%PUL_DWN(LP18);
%PUL_DWN(LP19);
%PUL_DWN(LP20);

%MACRO EXP_EXL(DSET);
PROC EXPORT DATA=&DSET
            OUTFILE= "T:\SAS\LP Table Dumps.xlsx" 
            DBMS=EXCEL REPLACE;
     SHEET="&DSET"; 
RUN;
%MEND EXP_EXL;
%EXP_EXL(LP01);
%EXP_EXL(LP02);
%EXP_EXL(LP03);
%EXP_EXL(LP04);
%EXP_EXL(LP05);
%EXP_EXL(LP06);
%EXP_EXL(LP08);
%EXP_EXL(LP09);
%EXP_EXL(LP10);
%EXP_EXL(LP11);
%EXP_EXL(LP12);
%EXP_EXL(LP13);
%EXP_EXL(LP14);
%EXP_EXL(LP15);
%EXP_EXL(LP17);
%EXP_EXL(LP18);
%EXP_EXL(LP19);
%EXP_EXL(LP20);
