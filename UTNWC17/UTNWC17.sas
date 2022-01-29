/*Note - there are two RSUBMITS in this job*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWC17.NWC17RZ";
FILENAME REPORT2 "&RPTLIB/UNWC17.NWC17R2";
FILENAME REPORT3 "&RPTLIB/UNWC17.NWC17R3";
FILENAME REPORT4 "&RPTLIB/UNWC17.NWC17R4";

/*this data null step can be commented out for prod, it is here because SYSLPUT wasn't working for some reason*/
data _null_;
	runday = today() - 0;
	call symput('eomt',put(intnx('month',runday,-1,'E'),mmddyy10.));
run;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT;

data _null_;
	runday = today() - 0;
	call symput('eom',intnx('month',runday,-1,'E'));
	call symput('eomt',put(intnx('month',runday,-1,'E'),mmddyy10.));
	CALL SYMPUT('BEGIN',"'"||PUT(INTNX('MONTH',TODAY(),-1,'beginning'), MMDDYYD10.)||"'");
run;

%put &eom;
%put &eomt;
%PUT &BEGIN;

/*%let DB = DNFPRQUT;  *This is test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;

PROC SQL;
	CREATE TABLE RPS_HAD AS
		SELECT LN65.BF_SSN
			,LN65.LN_SEQ
			,LN65.LC_STA_LON65
			,LN65.LC_TYP_SCH_DIS
			,MIN(LN65.ld_crt_lon65) AS BD_CRT_RS05 FORMAT=MMDDYY10.
			,LN50.LC_STA_LON50
			,LN10.LA_CUR_PRI
			,LN65B.LC_TYP_SCH_DIS AS CURRENT_TYP
			,LN10.ld_lon_acl_add
		FROM PKUB.LN65_LON_RPS LN65
			INNER JOIN PKUB.LN10_LON LN10
				ON LN65.BF_SSN = LN10.BF_SSN
				AND LN65.LN_SEQ = LN10.LN_SEQ
			INNER JOIN PKUB.RS10_BR_RPD RS10
				ON LN65.BF_SSN = RS10.BF_SSN
				AND LN65.LN_RPS_SEQ = RS10.LN_RPS_SEQ
			LEFT OUTER JOIN PKUB.LN50_BR_DFR_APV LN50
				ON LN65.BF_SSN = LN50.BF_SSN
				AND LN65.LN_SEQ = LN50.LN_SEQ
				AND TODAY() BETWEEN LD_DFR_BEG AND LD_DFR_END 
				AND LN50.LC_STA_LON50 = 'A'
			LEFT OUTER JOIN PKUB.LN65_LON_RPS LN65B
				ON LN65.BF_SSN = LN65B.BF_SSN
				AND LN65.LN_SEQ = LN65B.LN_SEQ
				AND LN65B.LC_STA_LON65 = 'A'
				AND LN65B.LC_TYP_SCH_DIS IN ('IB','IL','I3','IP')
		WHERE LN65.LC_TYP_SCH_DIS IN ('IB','IL','I3','IP')
			AND LN65.ld_crt_lon65 <= &eom
		GROUP BY LN65.BF_SSN, LN65.LN_SEQ
	;

	CREATE TABLE RPS_APP_REC AS
		SELECT DISTINCT 
			AY10.BF_SSN
			,AY10.LN_ATY_SEQ
			,AY10.PF_REQ_ACT
			,AY20.LX_ATY
			,AY10.LF_USR_REQ_ATY
			,AY10.LD_ATY_REQ_RCV
			,LN10.LD_LON_IBR_ENT
		FROM 	PKUB.AY10_BR_LON_ATY AY10
			LEFT JOIN PKUB.AY20_ATY_TXT AY20
				ON AY10.BF_SSN = AY20.BF_SSN
				AND AY10.LN_ATY_SEQ = AY20.LN_ATY_SEQ
			LEFT OUTER JOIN PKUB.LN10_LON LN10
				ON AY10.BF_SSN = LN10.BF_SSN
		WHERE 	AY10.LD_ATY_REQ_RCV <= &eom
			AND
				(
					AY10.PF_REQ_ACT IN ('IBDN1','IBDN2','IBDN3','IBDN4','IBDN5','IBDN6')
					OR (AY10.PF_REQ_ACT IN ('DRIBR','DILIB','CODCA','CODPA','IBRDF','IBREA') AND INTNX('MONTH',COALESCE(LN10.LD_LON_IBR_ENT,TODAY()),0,'E') >= AY10.LD_ATY_REQ_RCV)	
				)
	;

	CREATE TABLE TOT_BOR AS
		SELECT COUNT(DISTINCT BF_SSN) AS BOR_CT
		FROM PKUB.LN10_LON 
	;

	CREATE TABLE IN_REPAYMENT AS
		SELECT COUNT(DISTINCT A.BF_SSN) AS IN_RPY
		FROM PKUB.LN10_LON A
			LEFT OUTER JOIN PKUB.DW01_DW_CLC_CLU B
				ON A.BF_SSN = B.BF_SSN
				AND A.LN_SEQ = B.LN_SEQ
		WHERE WC_DW_LON_STA = '03'
	;

QUIT;

PROC SORT DATA=RPS_HAD; BY BF_SSN BD_CRT_RS05; RUN;

DATA CNT(KEEP=BF_SSN Approved);
	SET RPS_HAD END=LAST;
	where LC_TYP_SCH_DIS IN ('IB','I3')
		and ld_lon_acl_add <= bd_crt_rs05;
	BY BF_SSN BD_CRT_RS05;
	RETAIN Approved 0;
	IF FIRST.BD_CRT_RS05  and bd_crt_rs05 ^= . THEN do;
		Approved + 1;
	end;
	IF LAST THEN OUTPUT;
RUN;

PROC SORT DATA=RPS_HAD; BY BF_SSN LC_STA_LON65; RUN;

DATA current_cnt(KEEP=BF_SSN Cur_IBR_CT);
	SET RPS_HAD END=LAST;
	where CURRENT_TYP in ('IB','IL','I3','IP') AND LA_CUR_PRI > 0 AND lc_sta_lon50 ^= 'A';
	BY BF_SSN LC_STA_LON65;
	RETAIN Cur_IBR_CT 0;
	IF FIRST.BF_SSN THEN Cur_IBR_CT+1;
	IF LAST THEN OUTPUT;
RUN;

DATA RPS_APP_REC_DET;
	SET RPS_APP_REC;
RUN;

DATA RPS_APP_REC(KEEP=RECEIVED REJECTED);
	SET RPS_APP_REC END=LAST;
	RETAIN RECEIVED 0;
	RETAIN REJECTED 60;
	IF PF_REQ_ACT IN ('IBDN1','IBDN2','IBDN3','IBDN4','IBDN5','IBDN6') THEN REJECTED + 1;
	ELSE RECEIVED + 1;
	IF LAST THEN OUTPUT;
RUN;

DATA ALLINONE(DROP=I);
	MERGE CNT CURRENT_CNT RPS_APP_REC tot_bor in_repayment;
	array all{6}received Approved REJECTED Cur_IBR_CT bor_ct in_rpy;
	do i = 1 to 6;
		all(i) = coalesce(all(i),0);
	end;
RUN;

ENDRSUBMIT;

/*test files*/
/*data CNT; set legend.cnt;*/
/*data CURRENT_CNT; set legend.CURRENT_CNT;*/
/*data RPS_APP_REC; set legend.RPS_APP_REC;*/
/*data tot_bor; set legend.tot_bor;*/
/*data in_repayment; set legend.in_repayment;*/

DATA RPS_APP_REC_DET; SET LEGEND.RPS_APP_REC_DET; RUN;
DATA ALLINONE; SET LEGEND.ALLINONE; RUN;

DATA _NULL_;
	SET ALLINONE ;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
	IF _N_ = 1 THEN DO;
		PUT "Month End Date,Portfolio Type,Cumulative Applications Received,Cumulative Applications Approved"
			",Cumulative Applications Rejected,Borrowers Currently on IBR,Total Portfolio(Borrowers),Repayment Portfolio(Borrowers)";
	END;
	   PUT "&eomt,ED," received Approved REJECTED Cur_IBR_CT ",";  *lone comma creates blank fields for total port and repay port;
RUN;

DATA _NULL_;
	SET RPS_APP_REC_DET;
	FILE REPORT3 DELIMITER=',' DSD DROPOVER LRECL=32767;
	IF _N_ = 1 THEN PUT 'SSN,Activity Sequence #,ARC,ARC Comment Text,ARC Adder,ARC Date,LD_LON_IBR_ENT';
	PUT BF_SSN LN_ATY_SEQ PF_REQ_ACT LX_ATY LF_USR_REQ_ATY LD_ATY_REQ_RCV LD_LON_IBR_ENT;
RUN;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
%let DB = DNFPUTDL;  *This is live;

data _null_;
	runday = today() - 0;
	call symput('eom',intnx('month',runday,-1,'E'));
	call symput('eomt',"'"||put(intnx('month',runday,-1,'E'),mmddyy10.)||"'");
     CALL SYMPUT('BEGIN',"'"||PUT(INTNX('MONTH',TODAY(),-1,'beginning'), MMDDYYD10.)||"'");
run;

%put &eom;
%put &eomt;
%PUT &BEGIN;

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE 0  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @01 " ********************************************************************* "
              / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @01 " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @01 " ****  &SQLXMSG   **** "
              / @01 " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE IDR_CNT AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT
						SUM(QRY.CNT) AS TOT_CNT
					FROM (
							SELECT DISTINCT
								AY10.BF_SSN
								,AY10.PF_REQ_ACT
								,AY10.LD_ATY_RSP
								,1 AS CNT
							FROM
								PKUB.AY10_BR_LON_ATY AY10
									JOIN PKUB.AC10_ACT_REQ AC10
										ON AY10.PF_REQ_ACT = AC10.PF_REQ_ACT
							WHERE
								AC10.WF_QUE = '2A'
								AND
								AY10.PF_REQ_ACT IN ('CODCA','CODPA','CODRV','G119I','IBADI','IBALN','IBRAF','IBRDF','IBRRW','IBRWB','IBRWV','ICRAL','IDRPR','T4506','WRIDR')
								AND
								AY10.LC_STA_ACTY10 = 'A'
								AND 
								AY10.PF_RSP_ACT = 'COMPL'
								AND
								DAYS(AY10.LD_ATY_RSP) BETWEEN DAYS(&BEGIN) AND DAYS(&EOMT)
							) QRY

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA IDR_CNT; SET LEGEND.IDR_CNT; RUN;

DATA _NULL_;
	SET IDR_CNT;
	FILE REPORT4 DELIMITER=',' DSD DROPOVER LRECL=32767;
	IF _N_ = 1 THEN PUT 'IDR_COUNT';
	PUT TOT_CNT;
RUN;
