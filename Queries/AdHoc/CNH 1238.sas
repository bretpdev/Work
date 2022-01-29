/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

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
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE RX AS
		SELECT	*
		FROM	
			CONNECTION TO DBX
				(
					SELECT	
						DISTINCT
						A.BF_SSN
					FROM	
						PKUB.LNXX_LON_RPS A
					WHERE
						A.LC_TYP_SCH_DIS IN ('CA', 'CL', 'CP', 'CQ', 'CX', 'CX', 'CX', 'IB', 'IL')
						AND NOT EXISTS
							(
								SELECT *
								FROM
									PKUB.LNXX_LON_RPS_SPF B
								WHERE
									A.BF_SSN = B.BF_SSN
									AND A.LN_SEQ = B.LN_SEQ
									AND A.LN_RPS_SEQ = B.LN_RPS_SEQ
							)

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE RX AS
		SELECT	*
		FROM	
			CONNECTION TO DBX
				(
					SELECT	
						DISTINCT
						A.BF_SSN
					FROM	
						PKUB.LNXX_LON_RPS_SPF A
					WHERE
						NOT EXISTS
							(
								SELECT *
								FROM
									PKUB.LNXX_LON_RPS B
								WHERE
									A.BF_SSN = B.BF_SSN
									AND A.LN_SEQ = B.LN_SEQ
									AND A.LN_RPS_SEQ = B.LN_RPS_SEQ
							)

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE NOT_IN_RSXX AS 
		SELECT	*
		FROM	
			CONNECTION TO DBX
				(
					SELECT	
						DISTINCT
						A.BF_SSN
					FROM	
						PKUB.LNXX_LON_RPS A
					WHERE
						A.LC_TYP_SCH_DIS IN ('CA', 'CL', 'CP', 'CQ', 'CX', 'CX', 'CX', 'IB', 'IL')
						AND NOT EXISTS
							(
								SELECT *
								FROM
									PKUB.RSXX_IBR_IRL_LON C
								WHERE
									A.BF_SSN = C.BF_SSN		
									AND A.LN_SEQ = C.LN_SEQ	
							)
					ORDER BY
						A.BF_SSN

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE IS_IN_RSXX AS 
		SELECT	CDBX.*
		FROM	
			CONNECTION TO DBX
				(
					SELECT	
						DISTINCT
						A.BF_SSN
					FROM	
						PKUB.LNXX_LON_RPS A
					WHERE
						A.LC_TYP_SCH_DIS IN ('CA', 'CL', 'CP', 'CQ', 'CX', 'CX', 'CX', 'IB', 'IL')

					FOR READ ONLY WITH UR
				) CDBX
		WHERE
			NOT EXISTS
				(
					SELECT *
					FROM
						NOT_IN_RSXX RSXX
					WHERE
						CDBX.BF_SSN = RSXX.BF_SSN	
				)
	;


	CREATE TABLE RX AS
		SELECT	CDBX.*
		FROM	
			CONNECTION TO DBX
				(
					SELECT	
						DISTINCT
						A.BF_SSN
					FROM	
						PKUB.LNXX_LON_RPS A
						INNER JOIN PKUB.RSXX_IBR_RPS B
							ON A.BF_SSN = B.BF_SSN
						INNER JOIN PKUB.RSXX_BR_RPD C
							ON A.BF_SSN = C.BF_SSN
					WHERE
						A.LC_TYP_SCH_DIS IN ('CA', 'CL', 'CP', 'CQ', 'CX', 'CX', 'CX', 'IB', 'IL')

					FOR READ ONLY WITH UR
				) CDBX
			INNER JOIN NOT_IN_RSXX RSXX
				ON CDBX.BF_SSN = RSXX.BF_SSN
		ORDER BY
			CDBX.BF_SSN
	;

	CREATE TABLE RX AS
		SELECT	CDBX.*
		FROM	
			CONNECTION TO DBX
				(
					SELECT	
						DISTINCT
						A.BF_SSN
					FROM	
						PKUB.LNXX_LON_RPS A
						INNER JOIN PKUB.RSXX_IBR_RPS B
							ON A.BF_SSN = B.BF_SSN
					WHERE
						A.LC_TYP_SCH_DIS IN ('CA', 'CL', 'CP', 'CQ', 'CX', 'CX', 'CX', 'IB', 'IL')
						AND NOT EXISTS
							(
								SELECT *
								FROM
									PKUB.RSXX_BR_RPD C
								WHERE
									A.BF_SSN = C.BF_SSN									
							)

					FOR READ ONLY WITH UR
				) CDBX
			INNER JOIN IS_IN_RSXX RSXX
				ON CDBX.BF_SSN = RSXX.BF_SSN
		ORDER BY
			CDBX.BF_SSN
	;

	CREATE TABLE RX AS
		SELECT	CDBX.*
		FROM	
			CONNECTION TO DBX
				(
					SELECT	
						DISTINCT
						A.BF_SSN
					FROM	
						PKUB.LNXX_LON_RPS A
						INNER JOIN PKUB.RSXX_IBR_RPS B
							ON A.BF_SSN = B.BF_SSN
					WHERE
						A.LC_TYP_SCH_DIS IN ('CA', 'CL', 'CP', 'CQ', 'CX', 'CX', 'CX', 'IB', 'IL')
						AND NOT EXISTS
							(
								SELECT *
								FROM
									PKUB.RSXX_BR_RPD C
								WHERE
									A.BF_SSN = C.BF_SSN									
							)

					FOR READ ONLY WITH UR
				) CDBX
			INNER JOIN NOT_IN_RSXX RSXX
				ON CDBX.BF_SSN = RSXX.BF_SSN
		ORDER BY
			CDBX.BF_SSN
	;

	CREATE TABLE RX AS
		SELECT	CDBX.*
		FROM	
			CONNECTION TO DBX
				(
					SELECT	
						DISTINCT
						A.BF_SSN
					FROM	
						PKUB.LNXX_LON_RPS A
						INNER JOIN PKUB.RSXX_BR_RPD B
							ON A.BF_SSN = B.BF_SSN
					WHERE
						A.LC_TYP_SCH_DIS IN ('CA', 'CL', 'CP', 'CQ', 'CX', 'CX', 'CX', 'IB', 'IL')
						AND NOT EXISTS
							(
								SELECT *
								FROM
									PKUB.RSXX_IBR_RPS C
								WHERE
									A.BF_SSN = C.BF_SSN									
							)
					FOR READ ONLY WITH UR
				) CDBX
			INNER JOIN IS_IN_RSXX RSXX
				ON CDBX.BF_SSN = RSXX.BF_SSN
		ORDER BY
			CDBX.BF_SSN
	;

	CREATE TABLE RX AS
		SELECT	CDBX.*
		FROM	
			CONNECTION TO DBX
				(
					SELECT	
						DISTINCT
						A.BF_SSN
					FROM	
						PKUB.LNXX_LON_RPS A
						INNER JOIN PKUB.RSXX_BR_RPD B
							ON A.BF_SSN = B.BF_SSN
					WHERE
						A.LC_TYP_SCH_DIS IN ('CA', 'CL', 'CP', 'CQ', 'CX', 'CX', 'CX', 'IB', 'IL')
						AND NOT EXISTS
							(
								SELECT *
								FROM
									PKUB.RSXX_IBR_RPS C
								WHERE
									A.BF_SSN = C.BF_SSN																	
							)
					FOR READ ONLY WITH UR
				) CDBX
			INNER JOIN NOT_IN_RSXX RSXX
				ON CDBX.BF_SSN = RSXX.BF_SSN
		ORDER BY
			CDBX.BF_SSN
	;

	CREATE TABLE RX AS
		SELECT	CDBX.*
		FROM	
			CONNECTION TO DBX
				(
					SELECT	
						DISTINCT
						A.BF_SSN
					FROM	
						PKUB.LNXX_LON_RPS A
					WHERE
						A.LC_TYP_SCH_DIS IN ('CA', 'CL', 'CP', 'CQ', 'CX', 'CX', 'CX', 'IB', 'IL')
						AND NOT EXISTS
							(
								SELECT *
								FROM
									PKUB.RSXX_IBR_RPS C
								WHERE
									A.BF_SSN = C.BF_SSN									
							)
						AND NOT EXISTS
							(
								SELECT *
								FROM
									PKUB.RSXX_BR_RPD D
								WHERE
									A.BF_SSN = D.BF_SSN									
							)
					FOR READ ONLY WITH UR
				) CDBX
			INNER JOIN IS_IN_RSXX RSXX
				ON CDBX.BF_SSN = RSXX.BF_SSN
		ORDER BY
			CDBX.BF_SSN
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

DATA DUMP;
	SET RX RX RX RX RX RX RX RX;
RUN;

PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE LNXX AS
	SELECT
		B.*
	FROM
		DUMP A
		INNER JOIN CONNECTION TO DBX
			(
				SELECT
					*
				FROM	
					PKUB.LNXX_LON_RPS
			) B
			ON A.BF_SSN = B.BF_SSN 
	;

	CREATE TABLE LNXX AS
	SELECT
		B.*
	FROM
		DUMP A
		INNER JOIN CONNECTION TO DBX
			(
				SELECT
					*
				FROM	
					PKUB.LNXX_LON_RPS_SPF
			) B
			ON A.BF_SSN = B.BF_SSN 
	;

	CREATE TABLE RSXX AS
	SELECT
		B.*
	FROM
		DUMP A
		INNER JOIN CONNECTION TO DBX
			(
				SELECT
					*
				FROM	
					PKUB.RSXX_IBR_RPS
			) B
			ON A.BF_SSN = B.BF_SSN 
	;

	CREATE TABLE RSXX AS
	SELECT
		B.*
	FROM
		DUMP A
		INNER JOIN CONNECTION TO DBX
			(
				SELECT
					*
				FROM	
					PKUB.RSXX_BR_RPD
			) B
			ON A.BF_SSN = B.BF_SSN 
	;

	CREATE TABLE RSXX AS
	SELECT
		B.*
	FROM
		DUMP A
		INNER JOIN CONNECTION TO DBX
			(
				SELECT
					*
				FROM	
					PKUB.RSXX_IBR_IRL_LON
			) B
			ON A.BF_SSN = B.BF_SSN 
	;

	DISCONNECT FROM DBX;

QUIT;

ENDRSUBMIT;

DATA RX; SET LEGEND.RX; RUN;
DATA RX; SET LEGEND.RX; RUN;
DATA RX; SET LEGEND.RX; RUN;
DATA RX; SET LEGEND.RX; RUN;
DATA RX; SET LEGEND.RX; RUN;
DATA RX; SET LEGEND.RX; RUN;
DATA RX; SET LEGEND.RX; RUN;
DATA RX; SET LEGEND.RX; RUN;
DATA LNXX; SET LEGEND.LNXX; RUN;
DATA LNXX; SET LEGEND.LNXX; RUN;
DATA RSXX; SET LEGEND.RSXX; RUN;
DATA RSXX; SET LEGEND.RSXX; RUN;
DATA RSXX; SET LEGEND.RSXX; RUN;

/*export to Excel spreadsheet*/
/*PROC EXPORT DATA= WORK.RX */
/*            OUTFILE= "T:\SAS\HP XXXX RX.xls" */
/*            DBMS=EXCEL REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
/**/
/*PROC EXPORT DATA= WORK.RX */
/*            OUTFILE= "T:\SAS\HP XXXX RX.xls" */
/*            DBMS=EXCEL REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
/**/
/*PROC EXPORT DATA= WORK.RX */
/*            OUTFILE= "T:\SAS\HP XXXX RX.xls" */
/*            DBMS=EXCEL REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
/**/
/*PROC EXPORT DATA= WORK.RX */
/*            OUTFILE= "T:\SAS\HP XXXX RX.xls" */
/*            DBMS=EXCEL REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
/**/
/*PROC EXPORT DATA= WORK.RX */
/*            OUTFILE= "T:\SAS\HP XXXX RX.xls" */
/*            DBMS=EXCEL REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
/**/
/*PROC EXPORT DATA= WORK.RX */
/*            OUTFILE= "T:\SAS\HP XXXX RX.xls" */
/*            DBMS=EXCEL REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
/**/
/*PROC EXPORT DATA= WORK.RX */
/*            OUTFILE= "T:\SAS\HP XXXX RX.xls" */
/*            DBMS=EXCEL REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
/**/
/*PROC EXPORT DATA= WORK.RX */
/*            OUTFILE= "T:\SAS\HP XXXX RX.xls" */
/*            DBMS=EXCEL REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
/**/
/*PROC EXPORT DATA= WORK.LNXX */
/*            OUTFILE= "T:\SAS\HP XXXX LNXX.xls" */
/*            DBMS=EXCEL REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
/**/
/*PROC EXPORT DATA= WORK.LNXX*/
/*            OUTFILE= "T:\SAS\HP XXXX LNXX.xls" */
/*            DBMS=EXCEL REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
/**/
/*PROC EXPORT DATA= WORK.RSXX*/
/*            OUTFILE= "T:\SAS\HP XXXX RSXX.xls" */
/*            DBMS=EXCEL REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
/**/
/*PROC EXPORT DATA= WORK.RSXX*/
/*            OUTFILE= "T:\SAS\HP XXXX RSXX.xls" */
/*            DBMS=EXCEL REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
/**/
/*PROC EXPORT DATA= WORK.RSXX */
/*            OUTFILE= "T:\SAS\HP XXXX RSXX.xls" */
/*            DBMS=EXCEL REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
