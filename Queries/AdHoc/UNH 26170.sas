/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK; /*live*/
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK ; /*test*/
RSUBMIT;

%LET DB = DLGSUTWH;/*live*/
/*%LET DB = DLGSWQUT; /*test*/

/*Report level variables*/
%LET BEGINDATE = '01-01-2014';
%LET ENDDATE = '12-31-2015';

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
CREATE TABLE IBRCounts AS
SELECT 
	*
FROM CONNECTION TO DB2 
	(
		SELECT 
			MONTHNAME(RS10.LD_RPS_1_PAY_DU) AS MONTH
			,YEAR(RS10.LD_RPS_1_PAY_DU) AS YEAR
			,COUNT(DISTINCT LN65.BF_SSN) AS COUNT_BORROWERS
		FROM
			OLWHRM1.LN65_LON_RPS LN65
			INNER JOIN OLWHRM1.RS10_BR_RPD RS10
				ON LN65.BF_SSN = RS10.BF_SSN
				AND LN65.LN_RPS_SEQ = RS10.LN_RPS_SEQ
		WHERE
			LN65.LC_TYP_SCH_DIS IN ('IL','IB')
			AND DAYS(RS10.LD_RPS_1_PAY_DU) BETWEEN DAYS(&BEGINDATE) AND DAYS(&ENDDATE)
		GROUP BY
			MONTHNAME(RS10.LD_RPS_1_PAY_DU)
			,YEAR(RS10.LD_RPS_1_PAY_DU)
		ORDER BY
			MONTHNAME(RS10.LD_RPS_1_PAY_DU)
			,YEAR(RS10.LD_RPS_1_PAY_DU)
	)
;
CREATE TABLE Borrowers AS
SELECT 
	*
FROM CONNECTION TO DB2 
	(
		SELECT 
			DISTINCT LN65.BF_SSN
			,MONTHNAME(RS10.LD_RPS_1_PAY_DU) AS MONTH
			,YEAR(RS10.LD_RPS_1_PAY_DU) AS YEAR
			
		FROM
			OLWHRM1.LN65_LON_RPS LN65
			INNER JOIN OLWHRM1.RS10_BR_RPD RS10
				ON LN65.BF_SSN = RS10.BF_SSN
				AND LN65.LN_RPS_SEQ = RS10.LN_RPS_SEQ
		WHERE
			LN65.LC_TYP_SCH_DIS IN ('IL','IB')
			AND DAYS(RS10.LD_RPS_1_PAY_DU) BETWEEN DAYS(&BEGINDATE) AND DAYS(&ENDDATE)
		ORDER BY
			LN65.BF_SSN
			,MONTHNAME(RS10.LD_RPS_1_PAY_DU)
			,YEAR(RS10.LD_RPS_1_PAY_DU)
	)
;
DISCONNECT FROM DB2;

%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  
%SQLCHECK;
QUIT;

ENDRSUBMIT;

DATA IBRCounts;
	SET DUSTER.IBRCounts;
RUN;

DATA Borrowers;
	SET DUSTER.Borrowers;
RUN;

PROC EXPORT
		DATA=Borrowers
		OUTFILE="&RPTLIB\UNH 26170.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="IBR_active";
RUN;

PROC EXPORT
		DATA=IBRCounts
		OUTFILE="&RPTLIB\UNH 26170.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="IBR_raw";
RUN;
