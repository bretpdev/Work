%LET EA27_STR = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\EA27_BANA.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME ea27 ODBC &BANA ;

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
	CREATE TABLE OUTPUT AS 
		SELECT DISTINCT
			SSN,
			ROUTNG_NO,
			ACCNO,
			ACCTYP,
			'$0.00' AS ADDITIONAL_AMT,
			COALESCE(BL10.DUE_DAY, LN65.DUE_DAY, DAY(DATEPART(Cycle_Dt))) AS DUE_DATE
		FROM
			SOURCE S
			LEFT JOIN 
			(
				SELECT
					BF_SSN,
					MAX(day(LD_RPS_1_PAY_DU)) AS DUE_DAY
				FROM
					OLWHRM1.RS10_BR_RPD rs10
				WHERE
					LC_STA_RPST10 = 'A'
				GROUP BY
					BF_SSN
			)LN65
				ON LN65.BF_SSN = S.SSN
			LEFT JOIN
			(
				SELECT
					BF_SSN,
					MAX(day(LD_BIL_DU)) AS DUE_DAY
				FROM
					OLWHRM1.BL10_BR_BIL 
				WHERE
					LC_STA_BIL10 = 'A'
				GROUP BY
					BF_SSN
			)BL10
				ON BL10.BF_SSN = S.SSN
;
QUIT;
ENDRSUBMIT;

data output;
set duster.output;
WHERE DUE_DATE = 28;
run;

DATA ERROR;
SET DUSTER.OUTPUT;
WHERE DUE_DATE ^= 28;
RUN;


PROC EXPORT DATA = WORK.OUTPUT 
            OUTFILE = "T:\NH 25546 28TH.txt" 
            DBMS = CSV 
			REPLACE;
     PUTNAMES = YES;
RUN;

PROC EXPORT DATA = WORK.ERROR 
            OUTFILE = "T:\NH 25546 NON 28TH.txt" 
            DBMS = CSV 
			REPLACE;
     PUTNAMES = YES;
RUN;
