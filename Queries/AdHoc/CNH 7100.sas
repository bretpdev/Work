/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
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
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						WQXX.WN_CTL_TSK,
						WQXX.WX_MSG_X_TSK || ' ' || WQXX.WX_MSG_X_TSK,
						BAL.BALANCE,
						CASE
							WHEN LNXX.LD_LON_RHB_PCV IS NULL THEN 'Y'
							ELSE 'N'
						END AS REHAB
					FROM
						PKUB.LNXX_LON LNXX
						INNER JOIN PKUB.LNXX_LON_RPS LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
						INNER JOIN PKUB.WQXX_TSK_QUE WQXX
							ON WQXX.BF_SSN = LNXX.BF_SSN
						LEFT JOIN PKUB.LNXX_RPD_PIO_CVN LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
						INNER JOIN
						(
							SELECT
								BF_SSN,
								SUM(LA_CUR_PRI) AS BALANCE
							FROM
								PKUB.LNXX_LON
							GROUP BY
								BF_SSN
						)BAL
							ON BAL.BF_SSN = LNXX.BF_SSN
					WHERE
						LNXX.LA_CUR_PRI > X
						AND LNXX.LC_STA_LONXX = 'R'
						AND WQXX.WF_QUE = 'RX'
						AND WQXX.WF_SUB_QUE = 'XX'
						AND LNXX.LC_TYP_SCH_DIS NOT IN ('FS','FG')
						AND LNXX.LC_STA_LONXX = 'A'
					
				)
	;

	CREATE TABLE RX AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						WQXX.WN_CTL_TSK,
						WQXX.WX_MSG_X_TSK || ' ' || WQXX.WX_MSG_X_TSK,
						BAL.BALANCE,
						CASE
							WHEN LNXX.LD_LON_RHB_PCV IS NULL THEN 'Y'
							ELSE 'N'
						END AS REHAB
					FROM
						PKUB.LNXX_LON LNXX
						INNER JOIN PKUB.LNXX_LON_RPS LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
						INNER JOIN PKUB.WQXX_TSK_QUE WQXX
							ON WQXX.BF_SSN = LNXX.BF_SSN
						LEFT JOIN PKUB.LNXX_RPD_PIO_CVN LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
						INNER JOIN
						(
							SELECT
								BF_SSN,
								SUM(LA_CUR_PRI) AS BALANCE
							FROM
								PKUB.LNXX_LON
							GROUP BY
								BF_SSN
						)BAL
							ON BAL.BF_SSN = LNXX.BF_SSN
					WHERE
						LNXX.LA_CUR_PRI > X
						AND LNXX.LC_STA_LONXX = 'R'
						AND WQXX.WF_QUE = 'RX'
						AND WQXX.WF_SUB_QUE = 'XX'
						AND LNXX.LC_TYP_SCH_DIS IN ('FS','FG')
						AND LNXX.LC_STA_LONXX = 'A'
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA RX; SET LEGEND.RX; RUN;
DATA RX; SET LEGEND.RX; RUN;

PROC EXPORT DATA = WORK.RX 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RX"; 
RUN;

PROC EXPORT DATA = WORK.RX 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RX"; 
RUN;

