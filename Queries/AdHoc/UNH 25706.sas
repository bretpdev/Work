/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

FILENAME REPORTZ "&RPTLIB/NH25706.RZ";
FILENAME REPORT2 "&RPTLIB/NH25706.R2";

/*LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;*/ /*live*/
LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK ; /*test*/
RSUBMIT;

/*%LET DB = DLGSUTWH;*/ /*live*/
%LET DB = DLGSWQUT; /*test*/

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
CREATE TABLE Balance AS
SELECT 
	*
FROM CONNECTION TO DB2 
	(
		SELECT
			LN10.BF_SSN AS Borrower_SSN,
			LN10.LN_SEQ AS Loan_Sequence,
			'NULL' AS Old_LI_BR_DET_RPD_XTN,
			'Y' AS New_LI_BR_DET_RPD_XTN
		FROM
			OLWHRM1.LN10_LON LN10
			LEFT OUTER JOIN OLWHRM1.LN15_DSB LN15
				ON LN10.BF_SSN = LN15.BF_SSN
				AND LN10.LN_SEQ = LN15.LN_SEQ
		WHERE
			COALESCE(LN10.LI_BR_DET_RPD_XTN,'') = '' /*Sufficient Debt Indicator*/
			AND LN10.LF_LON_CUR_OWN = '829769' /*BANA lender*/
		GROUP BY
			LN10.BF_SSN,
			LN10.LN_SEQ
		HAVING
			SUM(COALESCE(LN15.LA_DSB,0)-COALESCE(LN15.LA_DSB_CAN,0))  > 30000.00 /*original principal balance*/
		ORDER BY
			LN10.BF_SSN
	)
;
DISCONNECT FROM DB2;

%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  
%SQLCHECK;
QUIT;

ENDRSUBMIT;

DATA Balance;
	SET DUSTER.Balance;
RUN;

PROC EXPORT
		DATA=Balance
		OUTFILE="&RPTLIB\UNH 25706.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="LN10-Change";
RUN;
