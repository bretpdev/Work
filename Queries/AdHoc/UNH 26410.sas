%LET RPTLIB = T:\SAS;

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK; /*live*/
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK ; /*test*/
RSUBMIT;

%LET DB = DLGSUTWH; /*live*/

/*Report level variables*/
%LET VAR1 = 'R0';

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
CREATE TABLE Accounts AS
SELECT 
	*
FROM CONNECTION TO DB2 
	(
		SELECT
			*
		FROM
			OLWHRM1.WQ20_TSK_QUE WQ20
		WHERE
			WF_QUE = &VAR1
	)
;
DISCONNECT FROM DB2;

%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  
%SQLCHECK;
QUIT;

ENDRSUBMIT;

DATA Accounts;
	SET DUSTER.Accounts;
RUN;

PROC EXPORT
		DATA=Accounts
		OUTFILE="&RPTLIB\UNH 26410.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="WQ20_R0";
RUN;
