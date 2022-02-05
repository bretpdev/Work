LIBNAME  LEGEND REMOTE  SERVER=LEGEND  SLIBREF=WORK;
RSUBMIT;
/*%let DB = DNFPRQUT;  *This is test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;
LIBNAME PKUR DB2 DATABASE=&DB OWNER=PKUR;
LIBNAME FRDWODS DB2 DATABASE=&DB OWNER=FRDWODS;
PROC SQL;
CONNECT TO DB2 (DATABASE=&DB);
CREATE TABLE TESTQUERY AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT e.dm_prs_lst
	,E.df_spe_acc_id
	,G.LD_STA_RPST10
	,G.LD_RPS_1_PAY_DU
	,C.LD_FOR_REQ_END
	,CASE
		WHEN H.LC_STA_LN83 IS NOT NULL THEN 'Y'
		ELSE 'N'
	END AS LC_STA_LN83
FROM pkub.PD10_PRS_NME e
inner join pkub.LN65_LON_RPS a
	on e.df_prs_id = a.bf_ssn
inner join pkub.LN65_LON_RPS f
	on a.bf_ssn = f.bf_ssn
	and a.ln_seq = f.ln_seq
INNER JOIN pkub.RS10_BR_RPD G
	ON F.BF_SSN = G.BF_SSN
	AND F.LN_RPS_SEQ = G.LN_RPS_SEQ
INNER JOIN PKUB.LN10_LON B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
INNER JOIN pkub.LN60_BR_FOR_APV d
	on b.bf_ssn = d.bf_ssn
	and b.ln_seq = d.ln_seq
INNER JOIN pkub.FB10_BR_FOR_REQ c
	on d.bf_ssn = c.bf_ssn
	and d.lf_for_ctl_num = c.lf_for_ctl_num
LEFT OUTER JOIN pkub.LN83_EFT_TO_LON H
	ON A.BF_SSN = H.BF_SSN
	AND A.LN_SEQ = H.LN_SEQ
	AND H.LC_STA_LN83 = 'A'
WHERE	a.lc_typ_sch_dis in ('IB','I3')
	and c.lc_for_typ in ('17','28')
	and c.ld_for_req_beg >= b.ld_lon_acl_add
	and f.lc_sta_lon65 = 'A'
	AND F.LC_TYP_SCH_DIS IN ('IL','IP')
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
QUIT;

ENDRSUBMIT;
DATA TESTQUERY;
	SET LEGEND.TESTQUERY;
RUN;
proc sort data=testquery; by df_spe_acc_id; run;
PROC EXPORT DATA= WORK.TESTQUERY
            OUTFILE= "T:\SAS\IBR Schedule Issues - FED.xlsx" 
            DBMS=EXCEL label REPLACE;
     SHEET="IBR Schedule Issues"; 
     NEWFILE=YES;
RUN;
