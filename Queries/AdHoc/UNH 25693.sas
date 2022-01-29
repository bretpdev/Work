/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/NH25523.RZ";
FILENAME REPORT2 "&RPTLIB/NH25523.R2";

/*LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;*/ /*live*/
LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK ; /*test*/
RSUBMIT;

/*%LET DB = DLGSUTWH;*/ /*live*/
%LET DB = DLGSWQUT; /*test*/

/*Report level variables*/
%LET BanaLoan = '829769';
%LET BanaBlanketCode = '99999999';
%LET DummyCode = '77777700';

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
CONNECT TO DB2 (DATABASE=&DB); 
CREATE TABLE SchoolCodes AS
SELECT 
	*
FROM CONNECTION TO DB2 
	(
		SELECT
			LN10.BF_SSN,
			LN10.LN_SEQ,
			LN10.LF_DOE_SCL_ORG AS LF_DOE_SCL_ORG_current, 
			CASE WHEN LN10.IC_LON_PGM IN ('CNSLDN','SUBCNS','SUBSPC','UNCNS','UNSPC') THEN &DummyCode
				ELSE &BanaBlanketCode
			END AS LF_DOE_SCL_ORG_new
		FROM
			OLWHRM1.LN10_LON LN10
		WHERE
			LN10.LF_LON_CUR_OWN = &BanaLoan
			AND LN10.LC_STA_LON10 = 'P' 
			AND 
				(
				LN10.LF_DOE_SCL_ORG = '' 
				OR LN10.LF_DOE_SCL_ORG = '      00' /*same as null?*/
				OR LN10.LF_DOE_SCL_ORG IS NULL
				) /*no original school code*/
		ORDER BY 
			LN10.LF_DOE_SCL_ORG
	)
;
DISCONNECT FROM DB2;

%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  
%SQLCHECK;
QUIT;

ENDRSUBMIT;

DATA SchoolCodes;
	SET DUSTER.SchoolCodes;
Run;


PROC EXPORT
		DATA=SchoolCodes
		OUTFILE="&RPTLIB\UNH 25693.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="SchoolCodes";
RUN;
