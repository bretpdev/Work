/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

DATA Datafile;
    INFILE 'X:\Archive\BANA\Production Files\Borrower Issue\Bana ACH File Wave 1.txt' DLM="," DSD MISSOVER FIRSTOBS=2;
	FORMAT 	SSN $9. ROUTING_NO $9. ACCOUNTNO $30. ACCTYPE $1. ADDITIONAL_AMT $10. DUE_DAY BEST12.;
	INPUT 	SSN $ ROUTING_NO $ ACCOUNTNO $ ACCTYPE $ ADDITIONAL_AMT $ DUE_DAY ;
RUN;


LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK; /*live*/
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK ;*/ /*test*/

DATA DUSTER.Borrowers; *Send data to Duster;
SET Datafile;
RUN;

RSUBMIT;

%LET DB = DLGSUTWH; /*live*/
/*%LET DB = DLGSWQUT;*/ /*test*/
LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1; /*needed for SQL queries, but not for DB2 queries*/

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
CREATE TABLE LoanDetail AS
	SELECT DISTINCT
		BORR.SSN AS SSN,
		LN60.LN_SEQ AS LN_SEQ,
		LN60.LD_FOR_END AS EndDate,
		'Forbearance' AS Type
	FROM
		WORK.Borrowers BORR
		INNER JOIN OLWHRM1.LN10_LON LN10
			ON LN10.BF_SSN = BORR.SSN
		INNER JOIN OLWHRM1.LN60_BR_FOR_APV LN60
			ON LN60.BF_SSN = LN10.BF_SSN
			AND LN60.LN_SEQ = LN10.LN_SEQ
			AND LN60.LC_STA_LON60 ='A'
			AND LN60.LD_FOR_END > MDY(02,01,2016)
		LEFT OUTER JOIN OLWHRM1.LN83_EFT_TO_LON LN83
			ON LN83.BF_SSN = LN10.BF_SSN
			AND LN83.LN_SEQ = LN10.LN_SEQ
			AND LN83.LC_STA_LN83 = 'A'
	WHERE
		LN83.BF_SSN IS NULL

	UNION ALL

	SELECT DISTINCT
		BORR.SSN AS SSN,
		LN50.LN_SEQ AS LN_SEQ,
		LN50.LD_DFR_END AS EndDate,
		'Deferment' AS Type
	FROM
		WORK.Borrowers BORR
		INNER JOIN OLWHRM1.LN10_LON LN10
			ON LN10.BF_SSN = BORR.SSN
		INNER JOIN OLWHRM1.LN50_BR_DFR_APV LN50
			ON LN50.BF_SSN = LN10.BF_SSN
			AND LN50.LN_SEQ = LN10.LN_SEQ
			AND LN50.LC_STA_LON50 ='A'
			AND LN50.LD_DFR_END > MDY(02,01,2016)
		LEFT OUTER JOIN OLWHRM1.LN83_EFT_TO_LON LN83
			ON LN83.BF_SSN = LN10.BF_SSN
			AND LN83.LN_SEQ = LN10.LN_SEQ
			AND LN83.LC_STA_LN83 = 'A'
	WHERE
		LN83.BF_SSN IS NULL
;		
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  

QUIT;

ENDRSUBMIT;

DATA LoanDetail;
	SET DUSTER.LoanDetail;
	FORMAT EndDate MMDDYY10.;
RUN;

PROC EXPORT
		DATA=LoanDetail
		OUTFILE="&RPTLIB\UNH 26117.xlsx"
		DBMS = EXCEL
		REPLACE;
RUN;
