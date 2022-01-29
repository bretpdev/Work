%LET RPTLIB = T:\SAS;
/*LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn; update_lock_typ=nolock; bl_keepnulls=no";*/
/*%LET DSN = 'FILEDSN=X:\PADR\ODBC\CLS_TEST.dsn;';*/
LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CLS.dsn; update_lock_typ=nolock; bl_keepnulls=no";
%LET DSN = 'FILEDSN=X:\PADR\ODBC\CLS.dsn;';

PROC SQL;
	CREATE TABLE SOURCE AS
		SELECT
			SSN
		FROM
			SQL.DUEDATECHANGE
		WHERE
			SUCCESSFUL = X
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
	CREATE TABLE POPX AS 
		SELECT DISTINCT
			S.SSN,
			lnXX.LC_TYP_SCH_DIS,
			LNXX.NEWPMT AS NEW_PAYMENT,
			PMT_A.OLDPMT AS OLD_PAYMENT_CAL_A,
			PMT_B.OLDPMT AS OLD_PAYMENT_CAL_B,
			PMT_C.OLDPMT AS OLD_PAYMENT_CAL_C
		FROM
			SOURCE S
			LEFT JOIN PKUB.WQXX_TSK_QUE WQXX
				ON WQXX.BF_SSN = S.SSN
			LEFT JOIN
			(	
				SELECT
					LNXX.BF_SSN,
					lnXX.LC_TYP_SCH_DIS,
					SUM(LNXX.LA_RPS_ISL) AS NEWPMT
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
					and LC_TYP_SCH_DIS
				GROUP BY 
					LNXX.BF_SSN ,
					lnXX.LC_TYP_SCH_DIS
					
			)LNXX
				ON LNXX.BF_SSN = S.SSN
			LEFT JOIN
			(
				SELECT
					LNXX.BF_SSN,
					SUM(LNXX.LA_BIL_DU_PRT) AS OLDPMT
				FROM
					PKUB.LNXX_LON_BIL_CRF LNXX
				WHERE
					LNXX.LD_BIL_DU_LON BETWEEN INPUT('XX/XX/XXXX',MMDDYYXX.) AND INPUT('XX/XX/XXXX', MMDDYYXX.)
					AND LC_BIL_TYP_LON = 'P'
					AND LC_STA_LONXX = 'I'
				GROUP BY
					LNXX.BF_SSN
			)PMT_A
				ON PMT_A.BF_SSN = S.SSN
			LEFT JOIN
			(
				SELECT
					LNXX.BF_SSN,
					SUM(LNXX.LA_BIL_CUR_DU) AS OLDPMT
				FROM
					PKUB.LNXX_LON_BIL_CRF LNXX
				WHERE
					LNXX.LD_BIL_DU_LON BETWEEN INPUT('XX/XX/XXXX',MMDDYYXX.) AND INPUT('XX/XX/XXXX', MMDDYYXX.)
					AND LC_BIL_TYP_LON = 'P'
					AND LC_STA_LONXX = 'I'
				GROUP BY
					LNXX.BF_SSN
			)PMT_B
				ON PMT_B.BF_SSN = S.SSN
			LEFT JOIN
			(	
				SELECT
					LNXX.BF_SSN,
					SUM(LNXX.LA_RPS_ISL) AS OLDPMT
				FROM
					PKUB.LNXX_LON_RPS LNXX
				INNER JOIN PKUB.LNXX_LON_RPS_SPF LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
					AND LNXX.LN_GRD_RPS_SEQ = X
				WHERE
					LC_STA_LONXX = 'I'
					AND DATEPART(LNXX.LF_LST_DTS_LNXX) = INPUT('XX/XX/XXXX',MMDDYYXX.)
				GROUP BY 
					LNXX.BF_SSN 
					
			)PMT_C
				ON PMT_C.BF_SSN = S.SSN
;


QUIT;
ENDRSUBMIT;

DATA POP;SET LEGEND.POP;RUN;

/*DATA POPX;*/
/*	SET POP;*/
/*	WHERE TASK IS NOT NULL AND WF_QUE = 'RX'AND WF_SUB_QUE = 'XX';*/
/*RUN;*/
/**/
PROC SQL;	
	CREATE TABLE TOTAL_NUMBERS AS 
		SELECT
			SSN,
			LC_TYP_SCH_DIS,
			NEW_PAYMENT,
			OLD_PAYMENT_CAL_A,
			OLD_PAYMENT_CAL_B,
			OLD_PAYMENT_CAL_C,
			(NEW_PAYMENT - OLD_PAYMENT_CAL_A) AS DIFFERENCE_A,
			(NEW_PAYMENT - OLD_PAYMENT_CAL_B) AS DIFFERENCE_B,
			(NEW_PAYMENT - OLD_PAYMENT_CAL_C) AS DIFFERENCE_C
		FROM 
			POP
		having 
			(NEW_PAYMENT - OLD_PAYMENT_CAL_C) > XXX
;
QUIT;

proc sql;
	create table tt as 
		select
			LC_TYP_SCH_DIS,
			count(*) as c
		from
			TOTAL_NUMBERS
		group by
			LC_TYP_SCH_DIS
;
quit;	


/**/
PROC EXPORT DATA = WORK.tt
            OUTFILE = "T:\number change.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RXXX DATA"; 
RUN;
/**/
/*PROC EXPORT DATA = WORK.TOTAL_NUMBERS*/
/*            OUTFILE = "T:\NH XXXX.xlsx" */
/*            DBMS = EXCEL*/
/*			REPLACE;*/
/*     SHEET="TOTAL POP"; */
/*RUN;*/