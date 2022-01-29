/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK; /*live*/
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK ; /*test*/
RSUBMIT;

%LET DB = DLGSUTWH; /*live*/
/*%LET DB = DLGSWQUT; /*test*/

/*Report level variables*/
%LET NAME1 = 'PLUS';
%LET NAME2 = 'STUDENT';

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
CREATE TABLE Borrowers AS
SELECT 
	*
FROM CONNECTION TO DB2 
	(
		SELECT DISTINCT
			PD10.DF_PRS_ID
			,PD10.DM_PRS_1
			,PD10.DM_PRS_LST
			,LN10.IC_LON_PGM
/*			,LP10.IC_LON_PGM*/

		FROM
			OLWHRM1.PD10_PRS_NME PD10
			INNER JOIN OLWHRM1.LN10_LON LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
/*			INNER JOIN OLWHRM1.LP10_RPY_PAR LP10*/
/*				ON LP10.IC_LON_PGM = LN10.IC_LON_PGM*/
		WHERE
			(
			UPPER(PD10.DM_PRS_1) = &NAME1
			OR UPPER(PD10.DM_PRS_LST) = &NAME2
			OR UPPER(PD10.DM_PRS_1) = &NAME2
			OR UPPER(PD10.DM_PRS_LST) = &NAME1
			)
/*			AND UPPER(LP10.IC_LON_PGM) = &NAME1*/
	)
;
DISCONNECT FROM DB2;

%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  
%SQLCHECK;
QUIT;

ENDRSUBMIT;

DATA Borrowers;
	SET DUSTER.Borrowers;
RUN;

PROC EXPORT
		DATA=Borrowers
		OUTFILE="&RPTLIB\UNH 26234.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="Plus_Student";
RUN;
