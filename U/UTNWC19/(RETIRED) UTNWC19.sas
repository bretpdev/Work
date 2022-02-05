/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWC19.NWC19RZ";
FILENAME REPORT2 "&RPTLIB/UNWC19.NWC19R2";

*gets beginning and end dates of last month;
DATA _NULL_;
     CALL SYMPUT('BEGIN',INTNX('MONTH',TODAY(),-1,'BEGINNING'));
     CALL SYMPUT('END',INTNX('MONTH',TODAY(),-1,'END'));
RUN;

LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
%LET DB = DNFPUTDL;  *This is live;
LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;

*get base population of IBR,ICR,PAYE,REPAYE;
PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);
	CREATE TABLE POP AS
		SELECT	
			*
		FROM
			CONNECTION TO DB2 
			(
				SELECT DISTINCT
					LN10.BF_SSN
					,CASE 
						WHEN LN65.LC_TYP_SCH_DIS IN ('C1','C2','C3') 
							THEN 'ICR'
						WHEN LN65.LC_TYP_SCH_DIS = 'CA' 
							THEN 'PAYE'
						WHEN LN65.LC_TYP_SCH_DIS = 'I5'
							THEN 'REPAYE'
						ELSE 'IBR'
					 END AS RPY_TYP
					,LN65.LD_CRT_LON65
					,LN65.LC_STA_LON65
				FROM
					PKUB.LN10_LON LN10
					INNER JOIN PKUB.LN65_LON_RPS LN65
						ON LN10.BF_SSN = LN65.BF_SSN
						AND LN10.LN_SEQ = LN65.LN_SEQ
				WHERE
					LN10.LA_CUR_PRI > 0.00
					AND	LN10.LC_STA_LON10 = 'R'
					AND LN65.LC_TYP_SCH_DIS IN ('C1','C2','C3','CA','I5','IB','I3')

			FOR READ ONLY WITH UR
			);
	DISCONNECT FROM DB2;
QUIT;
ENDRSUBMIT;
DATA POP; 
	SET LEGEND.POP;
RUN;

*sets any missing numeric values to 0 instead of default . ;
OPTIONS MISSING = '0';

*sorts in preparation to get most recent occurrence;
PROC SORT DATA=POP ;
	BY RPY_TYP BF_SSN LD_CRT_LON65;
RUN;

*get most recent occurrence;
DATA POP;
	SET POP;
	BY RPY_TYP BF_SSN;
	IF LAST.BF_SSN THEN OUTPUT;
RUN;

*sorts in prep for next data step calculation;
PROC SORT DATA=POP ;
	BY RPY_TYP LC_STA_LON65 BF_SSN ;
RUN;

*creates summary table of repayment types and status;
DATA POP(DROP=BF_SSN LD_CRT_LON65 BASE LAST_MONTH);
	SET POP;
	BY RPY_TYP LC_STA_LON65;
	RETAIN BASE_TOTAL1 BASE_TOTAL2;
	BASE = 1;
	IF &BEGIN <= LD_CRT_LON65 <= &END 
		THEN LAST_MONTH = 1;
	ELSE LAST_MONTH = 0;
	IF FIRST.LC_STA_LON65 
		THEN 
			DO;
				BASE_TOTAL1 = 0;
				BASE_TOTAL2 = 0;
			END;
	BASE_TOTAL1+BASE;
	BASE_TOTAL2+LAST_MONTH;
	IF LAST.LC_STA_LON65;
RUN;

*transposes totals;
PROC TRANSPOSE DATA=POP OUT=POP2 (DROP=_NAME_ _LABEL_) PREFIX=TOT;
	VAR BASE_TOTAL1 BASE_TOTAL2;
	BY RPY_TYP;
RUN;

*adds order column to TOT2;
DATA POP3 (DROP=TOT2);
	SET POP2 (DROP=TOT1);
	TOT1=TOT2;
	ORD = _N_+2;
RUN;

*adds order column to TOT1;
DATA POP4;
	SET POP2 (DROP=TOT2);
	ORD = _N_;
RUN;

*unions data sets;
DATA IBR;
	SET POP4 POP3;
RUN;

*sorts unioned data set;
PROC SORT DATA=IBR;
	BY RPY_TYP ORD;
RUN;

*adds order column for value assignment;
DATA IBR;
	SET IBR;
	LAB_TEXT = _N_;
RUN;

*descriptions to be used as per lab_text;
PROC FORMAT ;
  VALUE DESC_TXT
		1 = 'IBR running total'
		2 = 'IBR for the month'
		3 = 'IBR removed running total'
		4 = 'IBR removed for the month'
		5 = 'ICR running total'
		6 = 'ICR for the month'
		7 = 'ICR removed running total'
		8 = 'ICR removed for the month'
		9 = 'PAYE running total'
		10 = 'PAYE for the month'
		11 = 'PAYE removed running total'
		12 = 'PAYE removed for the month'
		13 = 'REPAYE running total'
		14 = 'REPAYE for the month'
		15 = 'REPAYE removed running total'/*not used by BU*/
		16 = 'REPAYE removed for the month'/*not used by BU*/
	;
RUN;

*outputs R2;
DATA _NULL_;
	SET WORK.IBR END=EFIEOD;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
	FORMAT TOT1 BEST12.
		   LAB_TEXT DESC_TXT. ;
	DO;
		PUT LAB_TEXT @;
		PUT TOT1 ;
	END;
RUN;
