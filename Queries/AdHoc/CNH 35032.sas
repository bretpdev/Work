/*%LET RPTLIB = %SYSGET(reportdir);*/
/*%LET FILEDIR = /sas/whse/progrevw;*/
%LET RPTLIB = T:\SAS;
%LET FILEDIR = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWFXX.NWFXXRZ";
FILENAME REPORTX "&RPTLIB/UNWFXX.NWFXXRX";
FILENAME REPORTX "&RPTLIB/UNWFXX.NWFXXRX";

DATA DAYS;
INFILE "&FILEDIR/SmallBalanceWriteOffDays.txt" ;
INPUT VALX $ ;
RUN;

DATA _NULL_;
SET DAYS;
	CALL SYMPUT('NUM_DAYS', VALX);
RUN;

%SYSLPUT NUM_DAYS = &NUM_DAYS;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT;
/*%let DB = DNFPRQUT;  *This is test;*/
%let DB = DNFPUTDL;  *This is live;



LIBNAME pkub DBX DATABASE=&db OWNER=pkub;
proc sql;
create table demon as
select distinct bf_ssn
from pkub.LNXX_LON a
where A.LA_CUR_PRI + A.LA_NSI_OTS < X;

create table demo as
select a.bf_ssn
	,a.ln_seq
	,a.ld_lon_X_dsb
	,sum(a.la_cur_pri + a.la_nsi_ots) as outst
	,C.OUTST_DISB
FROM pkub.LNXX_LON A
INNER JOIN pkub.DWXX_DW_CLC_CLU B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
LEFT OUTER JOIN (SELECT C.BF_SSN
			,C.LN_SEQ
			,SUM(C.LA_DSB - C.LA_DSB_CAN) AS OUTST_DISB
		FROM pkub.LNXX_DSB C
		GROUP BY C.BF_SSN, C.LN_SEQ ) C
	ON A.BF_SSN = C.BF_SSN 
	AND A.LN_SEQ = C.LN_SEQ
left outer join demon d
	on a.bf_ssn = d.bf_ssn
WHERE (A.LC_STA_LONXX = 'R' OR A.LC_STA_LONXX = 'D')
	AND B.WC_DW_LON_STA ^= 'XX'
	AND A.LA_CUR_PRI + A.LA_NSI_OTS ^= X
	and d.bf_ssn is null
group by a.bf_ssn
having calculated outst < XX
and  calculated outst > X
;

CREATE TABLE MAX_TYP AS
SELECT distinct D.DF_SPE_ACC_ID
	,c.LN_SEQ
	,C.LD_LON_X_DSB
	,a.pc_fat_sub_typ
	,C.OUTST
	,C.OUTST_DISB
FROM DEMO C
INNER JOIN pkub.PDXX_PRS_NME D
	ON C.BF_SSN = D.DF_PRS_ID
left outer join (select bf_ssn
			,ln_seq
			,max(ln_fat_seq)
			,MAX(LD_FAT_EFF) AS MAX_LD_FAT_EFF
		from pkub.lnXX_fin_aty
		GROUP BY BF_SSN, LN_SEQ) b
	on c.bf_ssn = b.bf_ssn
	and c.ln_seq = b.ln_seq
left outer  JOIN PKUB.LNXX_FIN_ATY a
	ON b.BF_SSN = A.BF_SSN
	AND b.LN_SEQ = A.LN_SEQ
	and a.pc_fat_typ = 'XX'
	and a.pc_fat_sub_typ in ('XX','XX','XX')
where (a.pc_fat_sub_typ = 'XX'
	or a.bf_ssn is null)
	and (TODAY() - b.MAX_LD_FAT_EFF) > &NUM_DAYS;

quit;

PROC SQL;
CREATE TABLE DEMO AS
SELECT A.DF_SPE_ACC_ID
	,A.LN_SEQ
	,A.LD_LON_X_DSB
	,A.PC_FAT_SUB_TYP
FROM MAX_TYP A
LEFT OUTER JOIN(SELECT DF_SPE_ACC_ID
			FROM MAX_TYP
			WHERE OUTST = OUTST_DISB) B
	ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
WHERE B.DF_SPE_ACC_ID IS NULL;
QUIT;
ENDRSUBMIT;
DATA DEMO; SET LEGEND.DEMO; RUN;
DATA MAX_TYP; SET LEGEND.MAX_TYP; RUN;



DATA _NULL_;
SET MAX_TYP ;
FILE REPORTX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
where pc_fat_sub_typ = '';
FORMAT ld_lon_X_dsb MMDDYYXX. ;
if _n_ = X then put "DF_SPE_ACC_ID,LN_SEQ,LD_LON_X_DSB";
DO;
   PUT df_spe_acc_id @;
   PUT ln_seq @;
   PUT ld_lon_X_dsb  ;
END;
RUN;

DATA _NULL_;
SET MAX_TYP ;
where pc_fat_sub_typ = 'XX';

ARC = 'SMBLR';
FROM_DATE = '';
TO_DATE = '';
NEEDED_BY_DATE = '';
RECIPIENT_ID = '';
REGARDS_TO_CODE = '';
REGARDS_TO_ID = 'B';
LOAN_SEQ = 'ALL';
COMMENTS = 'SMALL BALANCE CAUSED BY SCHOOL REFUND. REVIEW ACCOUNT.';
FILE REPORTX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
DO;
   PUT df_spe_acc_id @;
   PUT ARC @;
   PUT FROM_DATE @;
   PUT TO_DATE @;
   PUT NEEDED_BY_DATE @;
   PUT RECIPIENT_ID @;
   PUT REGARDS_TO_CODE @;
   PUT REGARDS_TO_ID @;
   PUT LOAN_SEQ @;
   PUT COMMENTS $;
END;
RUN;
