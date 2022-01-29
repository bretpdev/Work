%LET RPTLIB = T:\SAS;
/*LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn; update_lock_typ=nolock; bl_keepnulls=no";*/
/*%LET DSN = 'FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn;';*/
LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS.dsn; update_lock_typ=nolock; bl_keepnulls=no";
%LET DSN = 'FILEDSN=X:\PADR\ODBC\CLS.dsn;';

PROC SQL;
	CREATE TABLE SOURCE AS
		SELECT
			SSN,
			Accountnumber
		FROM
			SQL.DUEDATECHANGE
		WHERE
			SUCCESSFUL = X
			AND accountnumber not in ('XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX','XXXXXXXXXX')
;
QUIT;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
DATA LEGEND.SOURCE; *Send data to Duster;
SET SOURCE;
RUN;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;


LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;
LIBNAME AES DBX DATABASE=&DB OWNER=AES;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE X  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @XX " ********************************************************************* "
              / @XX " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @XX " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @XX " ********************************************************************* "
              / @XX " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @XX " ****  &SQLXMSG   **** "
              / @XX " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL;	
	CREATE TABLE POP AS 
		SELECT DISTINCT
			S.SSN,
			S.accountnumber
		FROM
			SOURCE S
			INNER JOIN
			(	
				SELECT
					LNXX.BF_SSN
				FROM
					PKUB.LNXX_LON_RPS LNXX
				INNER JOIN PKUB.LNXX_LON_RPS_SPF LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
					AND LNXX.LN_GRD_RPS_SEQ = X
				WHERE
					LC_STA_LONXX = 'A'
					AND LNXX.LD_CRT_LONXX = INPUT('XX/XX/XXXX',MMDDYYXX.)
					and LC_TYP_SCH_DIS IN ('EL','EG')
				GROUP BY 
					LNXX.BF_SSN 
					
			)LNXX
				ON LNXX.BF_SSN = S.SSN
			
;
QUIT;
ENDRSUBMIT;

DATA POP;SET LEGEND.POP;RUN;

DATA _NULL_;
	SET POP;
	FILE "&RPTLIB/NH XXXX_EL_EG_Cleanup.txt" DELIMITER=',' DSD DROPOVER LRECL=XXXXX;

	IF _N_ = X THEN
		DO;
			PUT
				'DF_SPE_ACC_ID'
				','
				'DUE_DATE'
				;

		END;
	DO;
		PUT accountnumber $ @;
		PUT 'XX' ;

	;
	END;
RUN;

/*LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn; update_lock_typ=nolock; bl_keepnulls=no";*/
/*%LET DSN = 'FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn;';*/
LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS.dsn; update_lock_typ=nolock; bl_keepnulls=no";
%LET DSN = 'FILEDSN=X:\PADR\ODBC\CLS.dsn;';

PROC SQL;
	INSERT INTO	SQL.DueDateChange(Ssn,AccountNumber,DueDate)
	SELECT
		ssn,
		accountnumber,
		'XX'
	FROM
		POP
;
QUIT;
