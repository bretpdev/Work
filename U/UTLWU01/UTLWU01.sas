/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWU01.LWU01RZ";
FILENAME REPORT2 "&RPTLIB/ULWU01.LWU01R2";

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
CREATE TABLE ADDR3 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	A.DF_PRS_ID
	,A.DX_STR_ADR_3
	,A.DC_ADR
	,A.DD_VER_ADR
	,A.DF_LST_USR_PD30
	,C.DF_SPE_ACC_ID
FROM	OLWHRM1.PD30_PRS_ADR A
		INNER JOIN OLWHRM1.LN10_LON B ON
			A.DF_PRS_ID = B.BF_SSN
			AND A.DX_STR_ADR_3 <> ''
			AND B.LA_CUR_PRI > 0
		INNER JOIN OLWHRM1.PD10_PRS_NME C ON 
			B.BF_SSN = C.DF_PRS_ID
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;

DATA ADDR3; SET WORKLOCL.ADDR3; RUN;	

DATA _NULL_;
SET ADDR3 ;
LENGTH DESCRIPTION $600.;
USER = DF_LST_USR_PD30;
ACT_DT = DD_VER_ADR;
DESCRIPTION = CATX(',',
	DF_SPE_ACC_ID,
	DX_STR_ADR_3,
	DC_ADR
);
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT USER $10. ;
FORMAT ACT_DT MMDDYY10. ;
FORMAT DESCRIPTION $600. ;
IF _N_ = 1 THEN DO;
	PUT "USER,ACT_DT,DESCRIPTION";
END;
DO;
   PUT USER $ @;
   PUT ACT_DT @;
   PUT DESCRIPTION $ ;
END;
RUN;
