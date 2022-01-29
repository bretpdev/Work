/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS17.NWS17RZ";
FILENAME REPORT2 "&RPTLIB/UNWS17.NWS17R2";
FILENAME REPORT3 "&RPTLIB/UNWS17.NWS17R3";
FILENAME REPORT4 "&RPTLIB/UNWS17.NWS17R4";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT;
/*LIBNAME pkub DB2 DATABASE=DNFPRQUT OWNER=pkub;*/
LIBNAME pkub DB2 DATABASE=DNFPUTDL OWNER=pkub;
PROC SQL;
/*CONNECT TO DB2 (DATABASE=DNFPRQUT);*/
CONNECT TO DB2 (DATABASE=DNFPUTDL);
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.BF_SSN
	,A.LN_SEQ
	,A.IC_LON_PGM
	,A.LA_CUR_PRI
	,MAX(D.LD_ATY_REQ_RCV, B.LD_FOR_BEG) AS LAST_STATEMENT
	,B.LD_FOR_END
FROM pkub.LN10_LON A
LEFT OUTER JOIN pkub.LN60_BR_FOR_APV B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
LEFT OUTER join pkub.FB10_BR_FOR_REQ C
	ON b.BF_SSN = C.BF_SSN
	AND B.LF_FOR_CTL_NUM = C.LF_FOR_CTL_NUM 
LEFT OUTER JOIN pkub.AY10_BR_LON_ATY D
	ON A.BF_SSN = D.BF_SSN
	AND D.PF_REQ_ACT = 'FOR06'
WHERE A.LA_CUR_PRI > 0 
	AND A.LC_STA_LON10 = 'R' 
	AND A.LI_CON_PAY_STP_PUR != 'Y'
	AND B.LC_STA_LON60 = 'A'
	AND B.LC_FOR_RSP != '003'
	AND B.LD_FOR_END > CURRENT DATE
	AND DAYS(B.LD_FOR_BEG) < DAYS(CURRENT DATE) - 175
	AND DAYS(C.LD_FOR_INF_CER) < DAYS(CURRENT DATE) - 175
	AND (D.BF_SSN IS NULL
		or DAYS(D.LD_ATY_REQ_RCV) < DAYS(CURRENT DATE) - 175)
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
QUIT;

PROC SQL; 
CREATE TABLE END_BOR AS
SELECT DISTINCT bf_ssn
	,ln_seq
	,BF_SSN as df_prs_id
FROM DEMO
UNION 
SELECT DISTINCT b.bf_ssn
	,b.ln_seq
	,LF_EDS as df_prs_id
FROM DEMO A
INNER JOIN PKUB.LN20_EDS B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
WHERE B.LC_STA_LON20 = 'A';

CREATE TABLE ADDRESS AS
SELECT DF_SPE_ACC_ID
	,c.df_prs_id
	,BF_SSN
	,DM_PRS_1
	,DM_PRS_LST
	,DX_STR_ADR_1
	,DX_STR_ADR_2
	,DM_CT
	,DC_DOM_ST
	,DF_ZIP_CDE
	,DM_FGN_CNY
	,DC_ADR
	,DI_VLD_ADR
FROM END_BOR A
INNER JOIN PKUB.PD10_PRS_NME C
	ON A.DF_PRS_ID = C.DF_PRS_ID
INNER JOIN PKUB.PD30_PRS_ADR B
	ON A.DF_PRS_ID = B.DF_PRS_ID
WHERE B.DC_ADR = 'L';

CREATE TABLE LOAN_DATA AS
SELECT A.BF_SSN
	,A.LN_SEQ
	,A.IC_LON_PGM
	,A.LA_CUR_PRI
	,INT(A.LA_CUR_PRI * COALESCE(C.LR_ITR,0) / 365) / 100 AS LR_ITR_DLY
	,B.LA_NSI_OTS
	,B.LA_NSI_ACR
	,CALCULATED LR_ITR_DLY * (TODAY() - LAST_STATEMENT) AS INT_SINCE_LAST
	,A.LD_FOR_END
FROM DEMO A
INNER JOIN PKUB.DW01_DW_CLC_CLU B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
LEFT OUTER JOIN PKUB.LN72_INT_RTE_HST C
	ON A.BF_SSN = C.BF_SSN
	AND A.LN_SEQ = C.LN_SEQ
	AND C.LC_STA_LON72 = 'A'
	AND C.LD_ITR_EFF_BEG <= TODAY()
	AND C.LD_ITR_EFF_END >= TODAY();

QUIT;

ENDRSUBMIT;
DATA ADDRESS; SET LEGEND.ADDRESS; RUN;
DATA LOAN_DATA; SET LEGEND.LOAN_DATA; RUN;
DATA END_BOR; SET LEGEND.END_BOR; RUN;

proc sort data=LOAN_DATA; by bf_ssn ln_seq; run;
proc sort data=END_BOR; by bf_ssn ln_seq ; run;

data loan_data;
merge END_BOR loan_data;
by bf_ssn ln_seq;
run;

proc sort data=loan_data; by df_prs_id; run;
proc sort data=ADDRESS; by df_prs_id; run;

data loan_data;
merge address loan_data;
by df_prs_id;
run;

proc sort data=loan_data; by df_spe_acc_id; run;

DATA loan_data (DROP = KEYSSN MODAY KEYLINE CHKDIG DIG I 
	CHKDIG CHK1 CHK2 CHK3 CHKDIGIT CHECK);
SET loan_data;
KEYSSN = TRANSLATE(DF_PRS_ID,'MYLAUGHTER','0987654321');
MODAY = PUT(DATE(),MMDDYYN4.);
KEYLINE = "P"||KEYSSN||MODAY||"L";
CHKDIG = 0;
LENGTH DIG $2.;
DO I = 1 TO LENGTH(KEYLINE);
	IF I/2 NE ROUND(I/2,1) 
		THEN DIG = PUT(INPUT(SUBSTR(KEYLINE,I,1),BITS4.4) * 2, 2.);
	ELSE DIG = PUT(INPUT(SUBSTR(KEYLINE,I,1),BITS4.4), 2.);
	IF SUBSTR(DIG,1,1) = " " 
		THEN CHKDIG = CHKDIG + INPUT(SUBSTR(DIG,2,1),1.);
		ELSE DO;
			CHK1 = INPUT(SUBSTR(DIG,1,1),1.);
			CHK2 = INPUT(SUBSTR(DIG,2,1),1.);
			IF CHK1 + CHK2 >= 10
				THEN DO;
					CHK3 = PUT(CHK1 + CHK2,2.);
					CHK1 = INPUT(SUBSTR(CHK3,1,1),1.);
					CHK2 = INPUT(SUBSTR(CHK3,2,1),1.);
				END;
			CHKDIG = CHKDIG + CHK1 + CHK2;
		END;
END;
CHKDIGIT = 10 - INPUT(SUBSTR((RIGHT(PUT(CHKDIG,3.))),3,1),3.);
IF CHKDIGIT = 10 THEN CHKDIGIT = 0;
CHECK = PUT(CHKDIGIT,1.);
ACSKEY = "#"||KEYLINE||CHECK||"#";
RUN;

%LET LOTS_LOANS = 'NOT LIKELY';
PROC SQL NOPRINT;
SELECT "'" || DF_SPE_ACC_ID || "'" INTO: LOTS_LOANS SEPARATED BY ','
FROM LOAN_DATA
GROUP BY DF_SPE_ACC_ID
HAVING COUNT(DISTINCT LN_SEQ) > 25;
QUIT;
%PUT &LOTS_LOANS;

%MACRO REP(R,VAL);
DATA _NULL_;
SET loan_data ;
WHERE DI_VLD_ADR = &VAL
	AND DF_SPE_ACC_ID NOT IN (&LOTS_LOANS);
BY df_spe_acc_id;
PRO_DT_CAP = LD_FOR_END + 1;
FORMAT PRO_DT_CAP LD_FOR_END MMDDYY10.;
FORMAT LA_CUR_PRI LA_NSI_OTS LA_NSI_ACR INT_SINCE_LAST 14.2;
FILE REPORT&R DELIMITER=',' DSD LRECL=32767;
RETAIN A 0;
IF _N_ = 1 THEN DO;
	PUT "DF_SPE_ACC_ID,DM_PRS_1,DM_PRS_LST,DX_STR_ADR_1,DX_STR_ADR_2,DM_CT,DC_DOM_ST,DF_ZIP_CDE,DM_FGN_CNY"
	%DO I = 1 %TO 25; 
		",LN_SEQ&i,IC_LON_PGM&i,LA_CUR_PRI&i,LA_NSI_OTS&I,LA_NSI_ACR&I,INT_SINCE_LAST&I,LD_FOR_END&I,PRO_DT_CAP&I"
	%END;
	",ACSKEY" ;
END;

IF FIRST.DF_SPE_ACC_ID THEN DO;
	A = 0;
	PUT DF_SPE_ACC_ID DM_PRS_1 DM_PRS_LST DX_STR_ADR_1 DX_STR_ADR_2 DM_CT DC_DOM_ST DF_ZIP_CDE DM_FGN_CNY @;
END;
DO;
	A + 1;
	IF LAST.DF_SPE_ACC_ID THEN do;
		PUT LN_SEQ IC_LON_PGM LA_CUR_PRI LA_NSI_OTS LA_NSI_ACR INT_SINCE_LAST LD_FOR_END PRO_DT_CAP @;
		do while (A <= 24);
			put 8*',' @;
			A + 1;
		end;
		PUT ACSKEY $;
	end;
	ELSE PUT LN_SEQ IC_LON_PGM LA_CUR_PRI LA_NSI_OTS LA_NSI_ACR INT_SINCE_LAST LD_FOR_END PRO_DT_CAP @;
END;

RUN;
%MEND;
proc sort data=loan_data NODUPKEY; by df_spe_acc_id LN_SEQ; run;

%REP(2,'Y');
%REP(3,'N');

PROC PRINTTO PRINT=REPORT4 NEW;
RUN;

OPTIONS ORIENTATION = PORTRAIT;
OPTIONS PS=52 LS=96;

PROC PRINT NOOBS SPLIT='/' DATA=loan_data WIDTH=UNIFORM WIDTH=MIN LABEL;
WHERE DF_SPE_ACC_ID IN (&LOTS_LOANS);
VAR DF_SPE_ACC_ID DM_PRS_1 DM_PRS_LST;
LABEL DF_SPE_ACC_ID = 'Account #'
	DM_PRS_1 = 'First Name'
	DM_PRS_LST = 'Last Name';
RUN;

PROC PRINTTO;
RUN;
