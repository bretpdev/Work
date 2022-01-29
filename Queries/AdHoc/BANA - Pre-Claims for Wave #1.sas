/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

FILENAME REPORTZ "&RPTLIB/NH25706.RZ";
FILENAME REPORT2 "&RPTLIB/NH25706.R2";

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK; /*live*/
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK ; /*test*/
RSUBMIT;

%LET DB = DLGSUTWH; /*live*/
/*%LET DB = DLGSWQUT; /*test*/

/*Report level variables
%LET BanaLoan = '829769';*/

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

CREATE TABLE Preclaim_LN16 AS
SELECT 
	*
FROM CONNECTION TO DB2 
	(
		SELECT
			*
		FROM
			OLWHRM1.LN16_LON_DLQ_HST LN16
			INNER JOIN OLWHRM1.PD10_PRS_NME PD10
				ON LN16.BF_SSN = PD10.DF_PRS_ID
		WHERE
			PD10.DF_SPE_ACC_ID IN ('8123826530', '0383193775', '1234097133')
	)
;

CREATE TABLE Preclaim_LN40 AS
SELECT 
	*
FROM CONNECTION TO DB2 
	(
		SELECT
			*
		FROM
			OLWHRM1.LN40_LON_CLM_PCL LN40
			INNER JOIN OLWHRM1.PD10_PRS_NME PD10
				ON LN40.BF_SSN = PD10.DF_PRS_ID
		WHERE
			PD10.DF_SPE_ACC_ID IN ('8123826530', '0383193775', '1234097133')
	)
;

CREATE TABLE Preclaim_CL10 AS
SELECT 
	*
FROM CONNECTION TO DB2 
	(
		SELECT
			*
		FROM
			OLWHRM1.CL10_CLM_PCL CL10
			INNER JOIN OLWHRM1.PD10_PRS_NME PD10
				ON CL10.BF_SSN = PD10.DF_PRS_ID
		WHERE
			PD10.DF_SPE_ACC_ID IN ('8123826530', '0383193775', '1234097133')
	)
;

DISCONNECT FROM DB2;

%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  
%SQLCHECK;
QUIT;

ENDRSUBMIT;

DATA Preclaim_LN16;
	SET DUSTER.Preclaim_LN16;
RUN;

PROC EXPORT
		DATA=Preclaim_LN16
		OUTFILE="&RPTLIB\Preclaim_Wave1.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="LN16";
RUN;

DATA Preclaim_LN40;
	SET DUSTER.Preclaim_LN40;
RUN;

PROC EXPORT
		DATA=Preclaim_LN40
		OUTFILE="&RPTLIB\Preclaim_Wave1.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="LN40";
RUN;

DATA Preclaim_CL10;
	SET DUSTER.Preclaim_CL10;
RUN;

PROC EXPORT
		DATA=Preclaim_CL10
		OUTFILE="&RPTLIB\Preclaim_Wave1.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="CL10";
RUN;
