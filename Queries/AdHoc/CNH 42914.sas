DATA _NULL_;
/*for previous month use -X, for current use X, etc*/
RUN_MON = -X;
	CALL SYMPUT('BEGIN',"'" || PUT(INTNX('MONTH',TODAY(),RUN_MON,'B'),MMDDYYXX.)|| "'");
	CALL SYMPUT('FINISH',"'" || PUT(INTNX('MONTH',TODAY(),RUN_MON,'E'),MMDDYYXX.)|| "'");
RUN;

/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWCXX.NWCXXRZ";
FILENAME REPORTX "&RPTLIB/UNWCXX.NWCXXRX";
FILENAME REPORTX "&RPTLIB/UNWCXX.NWCXXRX";

%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT FINISH = &FINISH;
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK  ;
RSUBMIT;
/*%let DB = DNFPRQUT;  *This is test;*/
%let DB = DNFPUTDL;  *This is live;

PROC SQL;
CONNECT TO DBX (DATABASE=&DB);
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DBX (
SELECT DISTINCT CASE
		WHEN DWXX.WC_DW_LON_STA = 'XX' and (LNXX.ln_dlq_max < XX or LNXX.bf_ssn is null)  THEN 'Current Repay'
		WHEN DWXX.WC_DW_LON_STA = 'XX' then 'Forbearance'
		WHEN DWXX.WC_DW_LON_STA = 'XX' then 'Deferment'
		WHEN DWXX.WC_DW_LON_STA in ('XX','XX','XX','XX','XX','XX','XX','XX') then 'Suspense'
		WHEN LNXX.LN_DLQ_MAX >=XX THEN 'Delinquent'
		ELSE 'Other'
	END AS STATUS 
/*	,COUNT(*) AS LON_CT*/
/*	,COUNT(DISTINCT LNXX.BF_SSN) AS BOR_CT*/
/*	,SUM(LNXX.LA_CUR_PRI) AS PBO */
/*	,SUM(DWXX.WA_TOT_BRI_OTS) AS IRB */
/*	,SUM(LNXX.LA_CUR_PRI + DWXX.WA_TOT_BRI_OTS) AS TOT */
	,CASE
		WHEN LNXX.IC_LON_PGM IN ('DL%%%','TEACH','FISL') THEN 'X'
		ELSE 'X'
	END AS RNUM
	,lnXX.bf_ssn
	,lnXX.ln_seq
FROM PKUB.LNXX_LON LNXX
LEFT OUTER JOIN PKUB.DWXX_DW_CLC_CLU DWXX
	ON LNXX.BF_SSN = DWXX.BF_SSN
	AND LNXX.LN_SEQ = DWXX.LN_SEQ
LEFT OUTER JOIN (select bf_ssn
					,ln_seq
					,max(ln_dlq_max) as ln_dlq_max
				from PKUB.LNXX_LON_DLQ_HST 
				where lc_sta_lonXX = 'X'
				group by bf_ssn, ln_seq) LNXX
	ON LNXX.BF_SSN = LNXX.BF_SSN
	AND LNXX.LN_SEQ = LNXX.LN_SEQ
LEFT OUTER JOIN PKUB.LNXX_RPD_PIO_CVN LNXX
	ON LNXX.BF_SSN = LNXX.BF_SSN
	AND LNXX.LN_SEQ = LNXX.LN_SEQ
WHERE LNXX.LA_CUR_PRI + DWXX.WA_TOT_BRI_OTS > X
	AND LNXX.LD_LON_RHB_PCV is not null
/*GROUP BY CASE*/
/*		WHEN LNXX.IC_LON_PGM IN ('DL%%%','TEACH','FISL') THEN 'X'*/
/*		ELSE 'X'*/
/*	END*/
/*	,CASE*/
/*		WHEN DWXX.WC_DW_LON_STA = 'XX' and (LNXX.ln_dlq_max < XX or LNXX.bf_ssn is null)  THEN 'Current Repay'*/
/*		WHEN DWXX.WC_DW_LON_STA = 'XX' then 'Forbearance'*/
/*		WHEN DWXX.WC_DW_LON_STA = 'XX' then 'Deferment'*/
/*		WHEN DWXX.WC_DW_LON_STA in ('XX','XX','XX','XX','XX','XX','XX','XX') then 'Suspense'*/
/*		WHEN LNXX.LN_DLQ_MAX >=XX THEN 'Delinquent'*/
/*		ELSE 'Other'*/
/*	END*/
);
DISCONNECT FROM DBX;
QUIT;

ENDRSUBMIT;
DATA DEMO; SET LEGEND.DEMO; RUN;
proc sort data=demo; by RNUM status; run; 

/*output data needed by SS for NH ticket*/
proc sql; select * from demo where status='Delinquent';quit;
data ss_output (drop=rnum); set demo; if status='Delinquent';run;

/**/
/*DATA _NULL_;*/
/*SET demo ;*/
/*where rnum = 'X';*/
/*FILE REPORTX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*if _n_ = X then do;*/
/*	put "CURRENT STATUS,COUNT OF LOANS,TOTAL PBO,TOTAL INTEREST,GRAND TOTAL";*/
/*end;*/
/*DO;*/
/*   PUT status @;*/
/*   PUT LON_CT @;*/
/*   PUT PBO @;*/
/*   PUT IRB @;*/
/*   PUT TOT $;*/
/*END;*/
/*RUN;*/
/*DATA _NULL_;*/
/*SET demo ;*/
/*where rnum = 'X';*/
/*FILE REPORTX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*if _n_ = X then do;*/
/*	put "CURRENT STATUS,COUNT OF LOANS,TOTAL PBO,TOTAL INTEREST,GRAND TOTAL";*/
/*end;*/
/*DO;*/
/*   PUT status @;*/
/*   PUT LON_CT @;*/
/*   PUT PBO @;*/
/*   PUT IRB @;*/
/*   PUT TOT $;*/
/*END;*/
/*RUN;*/
