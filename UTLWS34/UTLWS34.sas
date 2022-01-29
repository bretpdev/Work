*---------------------------------------------------*
| UTLWS34 Borrower Services Special E-mail Campaign |
*---------------------------------------------------*;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWS34.LWS34RZ";
FILENAME REPORT2 "&RPTLIB/ULWS34.LWS34R2";
FILENAME REPORT3 "&RPTLIB/ULWS34.LWS34R3";
FILENAME REPORT4 "&RPTLIB/ULWS34.LWS34R4";
FILENAME REPORT5 "&RPTLIB/ULWS34.LWS34R5";
FILENAME REPORT6 "&RPTLIB/ULWS34.LWS34R6";
FILENAME REPORT7 "&RPTLIB/ULWS34.LWS34R7";
FILENAME REPORT8 "&RPTLIB/ULWS34.LWS34R8";
FILENAME REPORT9 "&RPTLIB/ULWS34.LWS34R9";
FILENAME REPORT10 "&RPTLIB/ULWS34.LWS34R10";
FILENAME REPORT11 "&RPTLIB/ULWS34.LWS34R11";
FILENAME REPORT12 "&RPTLIB/ULWS34.LWS34R12";
FILENAME REPORT13 "&RPTLIB/ULWS34.LWS34R13";
FILENAME REPORT14 "&RPTLIB/ULWS34.LWS34R14";
FILENAME REPORT15 "&RPTLIB/ULWS34.LWS34R15";
FILENAME REPORT16 "&RPTLIB/ULWS34.LWS34R16";
FILENAME REPORT17 "&RPTLIB/ULWS34.LWS34R17";
FILENAME REPORT18 "&RPTLIB/ULWS34.LWS34R18";
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
%macro sqlcheck ;
  %if  &sqlxrc ne 0  %then  %do  ;
    data _null_  ;
            file reportz notitles  ;
            put @01 " ********************************************************************* "
              / @01 " ****  The SQL code above has experienced an error.               **** "
              / @01 " ****  The SAS should be reviewed.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  The SQL error code is  &sqlxrc  and the SQL error message  **** "
              / @01 " ****  &sqlxmsg   **** "
              / @01 " ********************************************************************* "
            ;
         run  ;
  %end  ;
%mend  ;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE BRWSMAIN AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A1.DF_SPE_ACC_ID AS ACCOUNT_NUMBER
		,RTRIM(A1.DM_PRS_1)||' '||RTRIM(A1.DM_PRS_LST) AS NAME
		,UCASE(A2.DX_EML_ADR) AS DX_EML_ADR
		,A1.DF_PRS_ID
		,DAYS(CURRENT DATE) - DAYS(D.LD_DLQ_OCC) AS TOT_DLQ
		,CASE
				WHEN E.LD_FOR_END >= CURRENT DATE AND E.LD_FOR_BEG >= D.LD_DLQ_OCC
					THEN DAYS(CURRENT DATE) - DAYS(LD_FOR_BEG) + 1
				WHEN E.LD_FOR_END < CURRENT DATE AND E.LD_FOR_BEG >= D.LD_DLQ_OCC
					THEN DAYS(E.LD_FOR_END) - DAYS(LD_FOR_BEG) + 1
				WHEN E.LD_FOR_END >= CURRENT DATE AND E.LD_FOR_BEG < D.LD_DLQ_OCC
				 	THEN DAYS(CURRENT DATE) - DAYS(D.LD_DLQ_OCC) + 1
				WHEN E.LD_FOR_END < CURRENT DATE AND E.LD_FOR_BEG < D.LD_DLQ_OCC
					THEN DAYS(E.LD_FOR_END) - DAYS(D.LD_DLQ_OCC) + 1
				WHEN E.LD_FOR_END IS NULL 
				 	THEN 0
				END AS FORB_X
			,D.LD_DLQ_OCC
			,B.LI_CON_PAY_STP_PUR 
FROM OLWHRM1.PD01_PDM_INF A1
INNER JOIN OLWHRM1.PD03_PRS_ADR_PHN A2
	ON  A1.DF_PRS_ID = A2.DF_PRS_ID
INNER JOIN OLWHRM1.LN10_LON B
	ON  A1.DF_PRS_ID = B.BF_SSN
INNER JOIN OLWHRM1.DW01_DW_CLC_CLU C
	ON  B.BF_SSN = C.BF_SSN
	AND B.LN_SEQ = C.LN_SEQ
INNER JOIN (SELECT D.BF_SSN
					,D.LN_DLQ_SEQ
					,D.LN_SEQ
					,MAX(D.LD_DLQ_OCC) AS LD_DLQ_OCC
				FROM OLWHRM1.LN16_LON_DLQ_HST D
				GROUP BY D.BF_SSN,D.LN_SEQ,D.LN_DLQ_SEQ
) D
	ON  C.BF_SSN = D.BF_SSN
	AND C.LN_SEQ = D.LN_SEQ
INNER JOIN OLWHRM1.LN16_LON_DLQ_HST LN16
	ON D.BF_SSN = LN16.BF_SSN
	AND D.LN_SEQ = LN16.LN_SEQ
	AND D.LN_DLQ_SEQ = LN16.LN_DLQ_SEQ
LEFT OUTER JOIN (SELECT E.BF_SSN
					,E.LN_SEQ
					,E.LD_FOR_BEG
					,E.LD_FOR_END
				FROM OLWHRM1.LN60_BR_FOR_APV E
				INNER JOIN OLWHRM1.FB10_BR_FOR_REQ F
					ON E.BF_SSN = F.BF_SSN
					AND E.LF_FOR_CTL_NUM = F.LF_FOR_CTL_NUM
				WHERE F.LC_FOR_TYP IN ('28')
) E
			ON B.BF_SSN = E.BF_SSN
			AND B.LN_SEQ = E.LN_SEQ
			AND D.LD_DLQ_OCC <= E.LD_FOR_END
			AND E.LD_FOR_BEG <= CURRENT DATE
WHERE B.LA_CUR_PRI > 0
	AND B.LA_CUR_PRI + COALESCE(C.WA_TOT_BRI_OTS,0) > 0
	AND DAYS(CURRENT DATE) - DAYS(D.LD_DLQ_OCC) >= 15
	AND DAYS(CURRENT DATE) - DAYS(D.LD_DLQ_OCC) < 360
	AND A2.DI_EML_ADR_VAL = 'Y'
	AND B.LC_STA_LON10 = 'R'
	AND B.LI_CON_PAY_STP_PUR <> 'Y'
	AND C.WC_DW_LON_STA IN ('03','14','08','07')
	AND LN16.LC_STA_LON16 = '1'

ORDER BY  A1.DF_SPE_ACC_ID
FOR READ ONLY WITH UR
);

CREATE TABLE ARCS AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT X.BF_SSN
	,X.PF_REQ_ACT
	,DAYS(CURRENT DATE) - DAYS(X.LD_ATY_REQ_RCV) AS SINCE_ARC
FROM OLWHRM1.AY10_BR_LON_ATY X
WHERE ((DAYS(CURRENT DATE) - DAYS(X.LD_ATY_REQ_RCV) <= 15 AND X.PF_REQ_ACT = 'EMBWR')
		OR (DAYS(CURRENT DATE) - DAYS(X.LD_ATY_REQ_RCV) <= 13 
			AND X.PF_REQ_ACT IN ('DQE05','DQE10','DQE15','DQE55','DQE60','DQE65','DQE70','DQE75','DQE80'))
		OR (DAYS(CURRENT DATE) - DAYS(X.LD_ATY_REQ_RCV) <= 27 
			AND X.PF_REQ_ACT IN ('DQE20','DQE25','DQE30','DQE35','DQE40','DQE45','DQE50')))
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
	/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
	/*%sqlcheck;*/
	/*quit;*/

ENDRSUBMIT;
DATA BRWSMAIN;
SET WORKLOCL.BRWSMAIN;
RUN;

PROC SORT DATA=BRWSMAIN;
BY ACCOUNT_NUMBER;
RUN;
/*FORBEARANCES EXCLUDED FROM DELINQUENCY CALCULATIONS*/
DATA BRWSMAIN(DROP=A FORB_X);
SET BRWSMAIN;
BY ACCOUNT_NUMBER;
RETAIN A;
IF FIRST.ACCOUNT_NUMBER THEN A = FORB_X;
IF FIRST.ACCOUNT_NUMBER = 0 THEN A = A + FORB_X;
IF LAST.ACCOUNT_NUMBER THEN DO;
	TOT_DLQ = TOT_DLQ - A;
	OUTPUT;
END;
RUN;

DATA ARCS; SET WORKLOCL.ARCS; RUN;

%MACRO REPORTS(REP,ARC,ARCD,DELMIN,DELMAX);
PROC SQL;
CREATE TABLE REP&REP AS
SELECT DISTINCT A.*
FROM BRWSMAIN A
WHERE A.TOT_DLQ >= &DELMIN
	AND A.TOT_DLQ <= &DELMAX
	AND NOT EXISTS (
		SELECT *
		FROM ARCS B
		WHERE  A.DF_PRS_ID = B.BF_SSN
			AND PF_REQ_ACT = &ARC
			AND SINCE_ARC <= &ARCD
		);
QUIT;
DATA _NULL_;
	SET REP&REP;
	FILE REPORT&REP DELIMITER=',' DSD DROPOVER LRECL=32767;
	IF _N_ = 1 THEN DO;
IF &REP = '2' THEN PUT "ACCOUNT_NUMBER,NAME,RECIPIENT";
ELSE PUT "ACCOUNTNUMBER,NAME,RECIPIENT";
	END;
	FORMAT ACCOUNT_NUMBER $10. ;
	FORMAT NAME $37. ;
	FORMAT DX_EML_ADR $254. ;
	DO;
		PUT ACCOUNT_NUMBER $ @;
		PUT NAME $ @;
		PUT DX_EML_ADR $;
	END;
RUN;
%MEND;

%REPORTS(2,'EMBWR',15,15,360);
%REPORTS(3,'DQE05',13,15,29);
%REPORTS(4,'DQE10',13,30,44);
%REPORTS(5,'DQE15',13,45,59);
%REPORTS(6,'DQE20',27,60,89);
%REPORTS(7,'DQE25',27,90,119);
%REPORTS(8,'DQE30',27,120,149);
%REPORTS(9,'DQE35',27,150,179);
%REPORTS(10,'DQE40',27,180,209);
%REPORTS(11,'DQE45',27,210,239);
%REPORTS(12,'DQE50',27,240,254);
%REPORTS(13,'DQE55',13,270,284);
%REPORTS(14,'DQE60',13,285,299);
%REPORTS(15,'DQE65',13,300,314);
%REPORTS(16,'DQE70',13,315,329);
%REPORTS(17,'DQE75',13,330,344);
%REPORTS(18,'DQE80',13,345,359);