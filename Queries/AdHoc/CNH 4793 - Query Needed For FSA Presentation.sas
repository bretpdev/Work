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

	CREATE TABLE ALL_SCH AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						RSXX.LD_RPS_X_PAY_DU,
						RSXX.LC_STA_RPSTXX,
						RSXX.LD_STA_RPSTXX,
						RSXX.BF_SSN,
						CASE
							WHEN LNXX.LC_TYP_SCH_DIS IN ('IB','IL') THEN 'IBR'
							WHEN LNXX.LC_TYP_SCH_DIS IN ('CX', 'CX', 'CX', 'CQ') THEN 'ICR'
							WHEN LNXX.LC_TYP_SCH_DIS IN ('CA', 'CP') THEN 'PAYE'
						END AS SCH_TYP
					FROM
						PKUB.RSXX_BR_RPD RSXX
						INNER JOIN PKUB.LNXX_LON_RPS LNXX
							ON RSXX.BF_SSN = LNXX.BF_SSN
							AND RSXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
					WHERE 
						LNXX.LC_TYP_SCH_DIS IN ('IB','IL','CX', 'CX', 'CX', 'CQ','CA', 'CP')
						AND DAYS(RSXX.LD_RPS_X_PAY_DU) <= DAYS('XXXX-XX-XX')
						AND 
							(
								RSXX.LC_STA_RPSTXX = 'A'
								OR
								DAYS(RSXX.LD_STA_RPSTXX) > DAYS('XXXX-XX-XX')
							)
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA ALL_SCH; SET LEGEND.ALL_SCH; RUN;

%MACRO BYMON(TBL,DT,MON,YR);
	PROC SQL;
		CREATE TABLE &TBL AS
			SELECT
				*,
				&DT AS MON,
				PUT(&DT, MMDDYYXX.) AS MONTH_END
			FROM
				ALL_SCH
			WHERE
				LD_RPS_X_PAY_DU <= &DT
				AND 
					(
						LC_STA_RPSTXX = 'A'
						OR
						LD_STA_RPSTXX >= &DT
					)
		;
	QUIT;
%MEND BYMON;

%BYMON(JUNXX,'XXJUNXXXX'D);
%BYMON(JULXX,'XXJULXXXX'D);
%BYMON(AUGXX,'XXAUGXXXX'D);
%BYMON(SEPXX,'XXSEPXXXX'D);
%BYMON(OCTXX,'XXOCTXXXX'D);
%BYMON(NOVXX,'XXNOVXXXX'D);
%BYMON(DECXX,'XXDECXXXX'D);
%BYMON(JANXX,'XXJANXXXX'D);
%BYMON(FEBXX,'XXFEBXXXX'D);

DATA ALL_MONS;
	SET JUNXX JULXX AUGXX SEPXX OCTXX NOVXX DECXX JANXX FEBXX ;
RUN;

PROC SQL;
	CREATE TABLE CNTS AS
		SELECT DISTINCT
			MON,
			SCH_TYP,
			COUNT(DISTINCT BF_SSN) AS CNT
		FROM
			ALL_MONS
		GROUP BY
			SCH_TYP,
			MON
		ORDER BY
			MON
	;
QUIT;

ODS RTF BODY = "T:\SAS\Query for FSA Presentation.RTF";

PROC PRINT NOOBS SPLIT='/' DATA=CNTS WIDTH=UNIFORM WIDTH=MIN LABEL;
	FORMAT 
		MON MMDDYYXX.
		CNT COMMAX.
	;

	BY
		MON
	;

	VAR 	
		SCH_TYP
		CNT
	;

	LABEL
		MON = 'Month End'
		SCH_TYP = 'Schedule Type'
		CNT = 'Number of Borrowers'
	;

	TITLE 'Query for FSA Presentation';
RUN;

ODS RTF CLOSE;

PROC EXPORT
	DATA = ALL_MONS
	OUTFILE = "T:\SAS\Query for FSA Presentation Detail"
	DBMS = EXCEL
	REPLACE;
RUN;
