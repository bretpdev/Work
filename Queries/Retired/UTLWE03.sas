/*UTLWE03 WELLS FARGO MONTHEND DATA FILE*/
LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir); 
FILENAME REPORT2 "&RPTLIB/ULWE03.LWE03R2";
FILENAME REPORTZ "&RPTLIB/ULWE03.LWE03RZ";
/*LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;*/
/*RSUBMIT;*/
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
CREATE TABLE WFMDF AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT   A.BF_SSN
		,A.LN_SEQ
		,A.WM_BR_LST
		,A.WM_BR_1
		,B.WX_STR_ADR_1
		,B.WX_STR_ADR_2
		,B.WM_CT
		,B.DC_DOM_ST
		,B.DF_ZIP_CDE
		,B.DD_BRT
		,A.IF_OWN
		,B.LD_LON_GTR
		,B.LA_LON_AMT_GTR
		,A.WR_ITR_1
		,A.ID_LON_SLE
		,A.WF_TIR_PCE_LN35
FROM OLWHRM1.MR5A_MR_LON_MTH_01 A
LEFT OUTER JOIN OLWHRM1.MR5B_MR_LON_MTH_02 B
	 ON A.BF_SSN = B.BF_SSN
	 AND A.LN_SEQ = B.LN_SEQ
	 AND A.IF_OWN = B.IF_OWN
WHERE	A.IF_OWN = '813894'
AND 	A.WA_CUR_PRI > 0
);
DISCONNECT FROM DB2;
%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;
%sqlcheck;
quit;
/*ENDRSUBMIT;*/
/*DATA WFMDF;*/
/*SET WORKLOCL.WFMDF;*/
/*RUN;*/
PROC SORT DATA = WFMDF;
BY BF_SSN LN_SEQ;
RUN;
DATA WFMDF; 
SET WFMDF; 
FORMAT TIR $10.;
IF IF_TIR_PCE = 'WNV' THEN TIR = 'Nevada';
else IF IF_TIR_PCE = 'WCA' THEN TIR = 'California';
else IF IF_TIR_PCE = 'WCO' THEN TIR = 'Colorado';
else TIR = '';
RUN;
DATA _NULL_;
SET  WORK.WFMDF;
*FILE 'C:\WINDOWS\TEMP\ULWE03.LWE03R2' DELIMITER=',' DSD DROPOVER LRECL=32767;
file REPORT2 delimiter=',' DSD DROPOVER lrecl=32767;
FORMAT BF_SSN $9. ;
FORMAT LN_SEQ 6. ;
FORMAT WM_BR_LST $23. ;
FORMAT WM_BR_1 $13. ;
FORMAT WX_STR_ADR_1 $30. ;
FORMAT WX_STR_ADR_2 $30. ;
FORMAT WM_CT $20. ;
FORMAT DC_DOM_ST $2. ;
FORMAT DF_ZIP_CDE $17. ;
FORMAT DD_BRT YYMMDDd10. ;
FORMAT IF_OWN $8. ;
FORMAT LD_LON_GTR YYMMDDd10. ;
FORMAT LA_LON_AMT_GTR 10.2 ;
FORMAT WR_ITR_1 7.3 ;
FORMAT ID_LON_SLE YYMMDDd10. ;
FORMAT TIR $10. ;
IF _N_ = 1 THEN        /* WRITE COLUMN NAMES */
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
'DD_BRT'
','
'IF_OWN'
','
'LD_LON_GTR'
','
'LA_LON_AMT_GTR'
','
'WR_ITR_1'
','
'ID_LON_SLE'
','
'TIR';
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
PUT DD_BRT @;
PUT IF_OWN $ @;
PUT LD_LON_GTR @;
PUT LA_LON_AMT_GTR @;
PUT WR_ITR_1 @;
PUT ID_LON_SLE @;
PUT TIR $;
END;
RUN;
