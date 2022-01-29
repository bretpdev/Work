%LET EA27_STR = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\EA27_BANA.dsn; update_lock_typ=nolock; bl_keepnulls=no");

PROC SQL noprint;
CONNECT TO ODBC AS EA27 (&EA27_STR);

CREATE TABLE SOURCE AS
	SELECT *
		FROM CONNECTION TO EA27 
		(
			SELECT
				SSN,
				Pkt,
				Prof,
				Cycle_Dt,
				PLID,
				Commonline_ID,
				Loan_Identification,
				dbo.Decryptor([EncryptedRoutingNumber]) AS ROUTNG_NO,
				dbo.Decryptor([EncryptedAccountNumber]) AS ACCNO,
				Acctyp
			FROM ACH_DATA;
		);

DISCONNECT FROM EA27;
QUIT;

/*TEST*/
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK  ;*/

/*LIVE*/
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
DATA DUSTER.SOURCE;
SET SOURCE;
RUN;

RSUBMIT;  
/*%let DB = DLGSWQUT;  *This is test;*/
%let DB = DLGSUTWH;  *This is live;

LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1;
PROC SQL;
	CREATE TABLE ln80 AS 
		SELECT DISTINCT
			s.ssn,
			S.Cycle_Dt,
			ln80.*
		FROM
			SOURCE S
			INNER JOIN OLWHRM1.LN80_LON_BIL_CRF ln80
				on ln80.bf_ssn = s.ssn

;
	CREATE TABLE rs10 AS 
		SELECT DISTINCT
			s.ssn,
			S.Cycle_Dt,
			rs10.*
		FROM
			SOURCE S
			INNER JOIN OLWHRM1.RS10_BR_RPD rs10
				on rs10.bf_ssn = s.ssn
			
;
QUIT;
ENDRSUBMIT;

data ln80;
set duster.ln80;
run;

data rs10;
set duster.rs10;
run;
