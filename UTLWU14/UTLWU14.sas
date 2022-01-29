/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
/*%LET FILEDIR = /sas/whse/progrevw;*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWU14.LWU14RZ";
FILENAME REPORT2 "&RPTLIB/ULWU14.LWU14R2";

/*%SYSLPUT FILEDIR = &FILEDIR;*/
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;

DATA PARMCARDTXT;
/*INFILE "/sas/whse/progrevw/Invalid_Phone_Parm_Card_TEST.txt" DLM=',' MISSOVER DSD;*/
INFILE "/sas/whse/progrevw/Invalid_Phone_Parm_Card.txt" DLM=',' MISSOVER DSD;
INFORMAT SSN $ 9. PHNUM $ 10.;
INPUT SSN $ PHNUM $;
RUN;

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
CREATE TABLE COINFO AS
SELECT DISTINCT *
FROM PARMCARDTXT A 
LEFT JOIN CONNECTION TO DB2 (
SELECT A.DF_PRS_ID,
	A.DF_LST_USR_PD42,
	A.DN_DOM_PHN_ARA || A.DN_DOM_PHN_XCH || A.DN_DOM_PHN_LCL AS SYSPHNNUM,
	A.DD_PHN_VER,
	A.DC_PHN,
	B.DF_SPE_ACC_ID
FROM OLWHRM1.PD42_PRS_PHN A
JOIN OLWHRM1.PD10_PRS_NME B
	ON A.DF_PRS_ID = B.DF_PRS_ID
WHERE A.DI_PHN_VLD = 'Y'
FOR READ ONLY WITH UR
) B ON A.SSN = B.DF_PRS_ID
WHERE B.SYSPHNNUM = A.PHNUM;
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;


DATA COINFO;
SET WORKLOCL.COINFO;
RUN;

DATA _NULL_;
	SET COINFO ;
	LENGTH DESCRIPTION $600.;
	USER = DF_LST_USR_PD42;
	ACT_DT = DD_PHN_VER;
	DESCRIPTION = CATX(',',	DF_SPE_ACC_ID,SYSPHNNUM,DC_PHN);
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
