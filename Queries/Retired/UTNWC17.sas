%LET RPTLIB = %SYSGET(reportdir);
/*%LET RPTLIB = T:\SAS;*/
FILENAME REPORTZ "&RPTLIB/UNWC17.NWC17RZ";
FILENAME REPORT2 "&RPTLIB/UNWC17.NWC17R2";
data _null_;
runday = today() - 0;
/*runday = '01jan1962'd;*/
call symput('eom',put(intnx('month',runday,-1,'E'),mmddyy10.));
run;

/*LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;*/
/*RSUBMIT;*/
/*%let DB = DNFPRQUT;  *This is test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;

PROC SQL;
CREATE TABLE RPS_HAD AS
SELECT A.BF_SSN
	,A.LN_SEQ
	,A.LC_STA_LON65
	,A.LC_TYP_SCH_DIS
	,MIN(BD_CRT_RS05) AS BD_CRT_RS05 FORMAT=MMDDYY10.
	,C.LC_STA_LON50
	,D.LA_CUR_PRI
	,E.LC_TYP_SCH_DIS AS CURRENT_TYP
FROM pkub.LN65_LON_RPS A
INNER JOIN pkub.LN10_LON D
	ON A.BF_SSN = D.BF_SSN
	AND A.LN_SEQ = D.LN_SEQ
INNER JOIN pkub.RS10_BR_RPD B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_RPS_SEQ = B.LN_RPS_SEQ
LEFT OUTER JOIN pkub.LN50_BR_DFR_APV C
	ON A.BF_SSN = C.BF_SSN
	AND A.LN_SEQ = C.LN_SEQ
	AND TODAY() BETWEEN LD_DFR_BEG AND LD_DFR_END 
	AND C.LC_STA_LON50 = 'A'
LEFT OUTER JOIN pkub.LN65_LON_RPS E
	ON A.BF_SSN = E.BF_SSN
	AND A.LN_SEQ = E.LN_SEQ
	AND E.LC_STA_LON65 = 'A'
	AND E.LC_TYP_SCH_DIS IN ('IB','IL')
WHERE A.LC_TYP_SCH_DIS IN ('IB','IL')
GROUP BY A.BF_SSN, A.LN_SEQ
;

CREATE TABLE RPS_APP_REC AS
SELECT DISTINCT A.BF_SSN
	,A.LN_ATY_SEQ
	,A.PF_REQ_ACT
FROM pkub.AY10_BR_LON_ATY A
LEFT OUTER JOIN pkub.AY20_ATY_TXT B
	on a.bf_ssn = b.bf_ssn
	and a.ln_aty_seq = b.ln_aty_seq
	AND A.PF_REQ_ACT ='MDCID'
WHERE PF_REQ_ACT = 'IBDN1' 
	OR (PF_REQ_ACT = 'MDCID' AND SUBSTR(LX_ATY,1,5) = 'CRIBR')
;

create table tot_bor as
select count(distinct bf_ssn) as bor_ct
from pkub.LN10_LON 
;

create table in_repayment as
select count(distinct a.bf_ssn) as in_rpy
from pkub.LN10_LON a
left outer join pkub.DW01_DW_CLC_CLU b
	on a.bf_ssn = b.bf_ssn
	and a.ln_seq = b.ln_seq
where wc_dw_lon_sta = '03';

QUIT;

PROC SORT DATA=RPS_HAD; BY BF_SSN BD_CRT_RS05; RUN;
DATA CNT(KEEP=BF_SSN Approved);
SET RPS_HAD END=LAST;
where LC_TYP_SCH_DIS = 'IB';
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
where CURRENT_TYP in ('IB','IL') AND LA_CUR_PRI > 0 AND lc_sta_lon50 ^= 'A';
BY BF_SSN LC_STA_LON65;
RETAIN Cur_IBR_CT 0;
IF FIRST.BF_SSN THEN Cur_IBR_CT+1;
IF LAST THEN OUTPUT;
RUN;

DATA RPS_APP_REC(KEEP=RECEIVED REJECTED);
SET RPS_APP_REC END=LAST;
RETAIN RECEIVED REJECTED 0;
IF PF_REQ_ACT = 'MDCID' THEN RECEIVED + 1;
IF PF_REQ_ACT = 'IBDN1' THEN REJECTED + 1;
IF LAST THEN OUTPUT;
RUN;

DATA ALLINONE(DROP=I);
MERGE CNT CURRENT_CNT RPS_APP_REC tot_bor in_repayment;
array all{6}received Approved REJECTED Cur_IBR_CT bor_ct in_rpy;
do i = 1 to 6;
all(i) = coalesce(all(i),0);
end;
RUN;


/*ENDRSUBMIT;*/
/*DATA RPS_APP_REC; SET LEGEND.RPS_APP_REC; RUN;*/
/*DATA ALLINONE; SET LEGEND.ALLINONE; RUN;*/
title;
proc printto print=report2 new;
run;
proc print data=allinone;
var bf_ssn;
sum Approved cur_ibr_ct;
run;
proc printto;
run;
DATA _NULL_;
SET ALLINONE ;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
IF _N_ = 1 THEN DO;
	PUT "Month End Date,Portfolio Type,Cumulative Applications Received,Cumulative Applications Approved"
		",Cumulative Applications Rejected,Borrowers Currently on IBR,Total Portfolio(Borrowers),Repayment Portfolio(Borrowers)";
END;
   PUT "&eom,ED," received Approved REJECTED Cur_IBR_CT bor_ct in_rpy;
RUN;
