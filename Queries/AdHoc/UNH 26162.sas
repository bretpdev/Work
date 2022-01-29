/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

FILENAME REPORTZ "&RPTLIB/NH26162.RZ";
FILENAME REPORT2 "&RPTLIB/NH26162.R2";

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
CREATE TABLE Borrowers AS
SELECT 
	*
FROM CONNECTION TO DB2 
	(
		SELECT DISTINCT
			PD10.DF_PRS_ID
			,PD10.DM_PRS_1
			,PD10.DM_PRS_LST
			,LP10.IC_LON_PGM
		FROM
			OLWHRM1.PD10_PRS_NME PD10
			INNER JOIN OLWHRM1.LN10_LON LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
			INNER JOIN OLWHRM1.LP10_RPY_PAR LP10
				ON LP10.IC_LON_PGM = LN10.IC_LON_PGM
		WHERE
			UPPER(PD10.DM_PRS_1) = 'PLUS'
			AND UPPER(PD10.DM_PRS_LST) = 'STUDENT'
			AND UPPER(LP10.IC_LON_PGM) ^= 'PLUS'
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
		OUTFILE="&RPTLIB\UNH 26162.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="Plus_Student";
RUN;
