*--------------------------------------------------------*
| UTLWN07 - WELLS FARGO MONTHLY GUARARANTEES DETAIL FILE |
*--------------------------------------------------------*;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWN07.LWN07R2";
FILENAME REPORTZ "&RPTLIB/ULWN07.LWN07RZ";
DATA _NULL_;
	CALL SYMPUT('BEGIN',"'"||PUT(INTNX('MONTH',TODAY(),-1,'BEGINNING'), MMDDYYD10.)||"'");
	CALL SYMPUT('END',"'"||PUT(INTNX('MONTH',TODAY(),-1,'END'), MMDDYYD10.)||"'");
	CALL SYMPUT('RUNDT',PUT(INTNX('DAY',TODAY(),0,'BEGINNING'), MMDDYYD10.));
RUN;
%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT END = &END;
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
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
CREATE TABLE WFMDF AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	A.BF_SSN
	,A.LN_SEQ
	,A.WM_BR_LST
	,A.WM_BR_1
	,B.WX_STR_ADR_1
	,B.WX_STR_ADR_2
	,B.WM_CT
	,B.DC_DOM_ST
	,B.DF_ZIP_CDE
	,B.DM_FGN_CNY
	,B.DM_FGN_ST
	,B.DD_BRT
	,B.LD_LON_GTR
	,B.LA_LON_AMT_GTR
FROM OLWHRM1.MR5A_MR_LON_MTH_01 A
INNER JOIN OLWHRM1.MR5B_MR_LON_MTH_02 B
	 ON A.BF_SSN = B.BF_SSN
	 AND A.LN_SEQ = B.LN_SEQ
	 AND A.IF_OWN = B.IF_OWN
WHERE A.IF_OWN = '813894'
AND B.LD_LON_GTR BETWEEN &BEGIN AND &END
);
DISCONNECT FROM DB2;
/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/
ENDRSUBMIT;

DATA WFMDF;
SET WORKLOCL.WFMDF;
RUN;

PROC SORT DATA = WFMDF;
BY BF_SSN LN_SEQ;
RUN;

DATA WFMDF;
SET WFMDF;
BY BF_SSN;
IF FIRST.BF_SSN THEN BR_NUM + 1;
IF FIRST.BF_SSN THEN BR_CT = BR_NUM;
ELSE BR_CT = 0;

RUN;

DATA _NULL_;
SET  WORK.WFMDF;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT BF_SSN $9. ;
FORMAT LN_SEQ 6. ;
FORMAT WM_BR_LST $23. ;
FORMAT WM_BR_1 $13. ;
FORMAT WX_STR_ADR_1 $30. ;
FORMAT WX_STR_ADR_2 $30. ;
FORMAT WM_CT $20. ;
FORMAT DC_DOM_ST $2. ;
FORMAT DF_ZIP_CDE $17. ;
FORMAT DM_FGN_CNY $30.;
FORMAT DM_FGN_ST $30.;
FORMAT DD_BRT MMDDYY10. ;
FORMAT LD_LON_GTR MMDDYY10. ;
FORMAT LA_LON_AMT_GTR 10.2 ;
FORMAT BR_CT BEST12.;
IF _N_ = 1 THEN       
DO;
	PUT
	'BF_SSN'
	','
	'LN_SEQ'
	','
	'WM_BR_LST'
	','
	'WM_BR_1'
	','
	'WX_STR_ADR_1'
	','
	'WX_STR_ADR_2'
	','
	'WM_CT'
	','
	'DC_DOM_ST'
	','
	'DF_ZIP_CDE'
	','
	'DM_FGN_CNY'
	','
	'DM_FGN_ST'
	','
	'DD_BRT'
	','
	'LD_LON_GTR'
	','
	'LA_LON_AMT_GTR'
	','
	'BR_CT'
	;
END;
DO;
PUT BF_SSN $ @;
PUT LN_SEQ @;
PUT WM_BR_LST $ @;
PUT WM_BR_1 $ @;
PUT WX_STR_ADR_1 $ @;
PUT WX_STR_ADR_2 $ @;
PUT WM_CT $ @;
PUT DC_DOM_ST $ @;
PUT DF_ZIP_CDE $ @;
PUT DM_FGN_CNY $ @;
PUT DM_FGN_ST $ @;
PUT DD_BRT @;
PUT LD_LON_GTR @;
PUT LA_LON_AMT_GTR @;
PUT BR_CT ;
END;
RUN;
