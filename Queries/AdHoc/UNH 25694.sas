/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/NH25694.RZ";
FILENAME REPORT2 "&RPTLIB/NH25694.R2";

/*LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;*/ /*live*/
LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK ; /*test*/
RSUBMIT;

/*%LET DB = DLGSUTWH;*/ /*live*/
%LET DB = DLGSWQUT; /*test*/

/*Report level variables*/
%LET BanaLoan = '829769';
%LET EFF_DATE = '03FEB2016'; /*1st run, will need change for 2nd & 3rd runs*/

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
CREATE TABLE NoLIBOR AS
SELECT 
	*
FROM CONNECTION TO DB2 
	(
		SELECT
			LN10.BF_SSN,
			LN10.LN_SEQ,
			LN10.LD_EFF_LBR_RTE AS LN10_LD_EFF_LBR_RTE_Current, /*LIBOR date*/
			&EFF_DATE AS LN10_LD_EFF_LBR_RTE_new /*effective load add date*/
		FROM
			OLWHRM1.LN10_LON LN10
		WHERE
			LN10.LF_LON_CUR_OWN = &BanaLoan
			AND LN10.LC_STA_LON10 = 'P' /*newest BANA borrowers*/
			AND LN10.LD_EFF_LBR_RTE IS NULL /*no LIBOR date*/
	)
;
DISCONNECT FROM DB2;

%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  
%SQLCHECK;
QUIT;

ENDRSUBMIT;

DATA NoLIBOR;
	SET DUSTER.NoLIBOR;
RUN;

PROC EXPORT
		DATA=NoLIBOR
		OUTFILE="&RPTLIB\UNH 25694.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="NoLIBOR";
RUN;
