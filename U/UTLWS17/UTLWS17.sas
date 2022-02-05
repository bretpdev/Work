/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWS17.LWS17RZ";
FILENAME REPORT2 "&RPTLIB/ULWS17.LWS17R2";
FILENAME REPORT5 "&RPTLIB/ULWS17.LWS17R5";
FILENAME REPORT6 "&RPTLIB/ULWS17.LWS17R6";
FILENAME REPORT7 "&RPTLIB/ULWS17.LWS17R7";
FILENAME REPORT8 "&RPTLIB/ULWS17.LWS17R8";
FILENAME REPORT9 "&RPTLIB/ULWS17.LWS17R9";
FILENAME REPORT10 "&RPTLIB/ULWS17.LWS17R10";
FILENAME REPORT11 "&RPTLIB/ULWS17.LWS17R11";
FILENAME REPORT12 "&RPTLIB/ULWS17.LWS17R12";

LIBNAME  DUSTER REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT duster;
DATA _NULL_;
runday = today();
/*runday = today()-25;*/
	CALL SYMPUT('CRIT_DT',"'"||PUT(INTNX('DAY',runday,-7,'BEGINNING'), MMDDYY10.)||"'");
RUN;

LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%MACRO SQLCHECK (SQLRPT= );
%IF &SQLXRC NE 0 %THEN %DO;
	DATA _NULL_;
    FILE REPORTZ NOTITLES;
    PUT @01 " ********************************************************************* "
      / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
      / @01 " ****  THE SAS LOG IN &SQLRPT SHOULD BE REVIEWED.          **** "       
      / @01 " ********************************************************************* "
      / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
      / @01 " ****  &SQLXMSG   **** "
      / @01 " ********************************************************************* ";
	RUN;
%END;
%MEND;

PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE BILLNOTSENT AS
SELECT *
FROM CONNECTION TO DB2 (
select distinct d.df_spe_acc_id
	,a.bf_ssn
	,c.ln_seq
	,case when a.ld_rbl_lst is null then a.ld_bil_crt
		else a.ld_rbl_lst
	end as ld_bil_crt
	,C.ld_bil_du_LON
		,a.LC_IND_BIL_SNT
		,a.lc_bil_mtd
	,trim(d.dm_prs_1) || ' ' || trim(d.dm_prs_lst) as BorName
	,h.ic_lon_pgm
	,h.lf_lon_cur_own
	,h.ld_lon_1_dsb
	,la_lte_fee_ots_prt
	,la_bil_pas_du
	,c.LF_FOR_CTL_NUM
		,COALESCE(c.LA_LTE_FEE_OTS_PRT,0) +
			 COALESCE(c.LA_BIL_PAS_DU,0) +
			 COALESCE(c.LA_BIL_DU_PRT,0) AS TOT_DU
FROM OLWHRM1.PD10_PRS_NME d
inner join OLWHRM1.BL10_BR_BIL a
	on d.df_prs_id = a.bf_ssn
inner join OLWHRM1.LN80_LON_BIL_CRF c
	on a.bf_ssn = c.bf_ssn
	and a.ld_bil_crt = c.ld_bil_crt
	and a.ln_seq_bil_wi_dte = c.ln_seq_bil_wi_dte
inner join OLWHRM1.LN10_LON h
	on c.bf_ssn = h.bf_ssn
	and c.ln_seq = h.ln_seq
inner join (select bf_ssn
			from OLWHRM1.LN10_LON 
			where lc_sta_lon10 = 'R'
				AND LA_CUR_PRI > 0
			group by bf_ssn
			HAVING SUM(LA_CUR_PRI) >= 50) Z
	ON D.DF_PRS_ID = Z.BF_SSN
left outer join OLWHRM1.AY10_BR_LON_ATY b
	on a.bf_ssn = b.bf_ssn
	and (b.ld_aty_req_rcv >= a.LD_BIL_CRT and a.ld_rbl_lst is null
		or b.ld_aty_req_rcv >= a.ld_rbl_lst)
	and b.ld_aty_req_rcv <= C.ld_bil_du_LON
	and b.pf_req_act in ('BILLN','DL200','DL400','DL800','DL910','DL140','DL170','DL202','DL230','DL201','DL401','DL801','DL911'
		,'DL141','DL171','DL203','DL231')
	and substr(b.lf_usr_req_aty,1,2) = 'UT'
WHERE  
	h.LC_LON_SND_CHC = 'Y'
	and h.la_cur_pri > 0
	and h.lc_sta_lon10 = 'R'
	and a.lc_sta_bIl10 = 'A'
	and c.lc_sta_lon80 = 'A'
	and B.BF_SSN IS NULL
	AND (A.LD_BIL_CRT >= &CRIT_DT OR A.LD_RBL_LST >= &CRIT_DT)
	AND (
		 h.lc_lon_snd_chc = 'Y'
		 OR EXISTS 
			(
				SELECT m.* 
				FROM OLWHRM1.BR30_BR_EFT m 
				WHERE d.df_prs_id = m.bf_ssn and m.bc_eft_sta IN ('A','I','P')
		 	)
		)
);

DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWS17.LWS17RZ);*/
quit;



proc sql;
CREATE TABLE DLQ AS
SELECT DISTINCT A.BF_SSN
	,a.ln_seq
	,a.LC_IND_BIL_SNT
	,max(CASE
		WHEN COALESCE(B.LD_DLQ_OCC,0) < COALESCE(C.LD_FOR_BEG,0)
			THEN today() - (COALESCE(B.LD_DLQ_OCC,0) - (COALESCE(C.LD_FOR_END,0) - COALESCE(C.LD_FOR_BEG,0)))
		ELSE today() - COALESCE(B.LD_DLQ_OCC,0)
	 END) AS DAYS_DLQ
FROM BILLNOTSENT A
inner JOIN OLWHRM1.LN16_LON_DLQ_HST b
	on a.bf_ssn = b.bf_ssn
	and a.ln_seq = b.ln_seq
LEFT OUTER JOIN OLWHRM1.LN60_BR_FOR_APV c
	ON a.BF_SSN = c.BF_SSN
	AND a.LN_SEQ = c.LN_SEQ 
	AND a.LF_FOR_CTL_NUM = c.LF_FOR_CTL_NUM
	and c.lc_sta_lon60 = 'A'
where b.lc_sta_lon16 = '1'
group by a.bf_ssn, a.ln_seq
having calculated days_dlq between 31 and 270
/*	having calculated days_dlq between 31 and 60*/
;
quit;

proc sort data=dlq; by bf_ssn ln_seq; run;
proc sort data=BILLNOTSENT; by bf_ssn ln_seq; run;
data dlq;
merge BILLNOTSENT dlq(in=a);
by bf_ssn ln_seq;
if a;
run;

proc sql;
CREATE TABLE BORR_ADR AS
SELECT DISTINCT A.BF_SSN
	,B.DX_STR_ADR_1
	,B.DX_STR_ADR_2
	,B.DM_CT
	,B.DC_DOM_ST
	,B.DF_ZIP_CDE
	,B.DC_FGN_CNY
FROM dlq A
INNER JOIN OLWHRM1.PD30_PRS_ADR b
	on a.bf_ssn = b.df_prs_id
where dc_adr = 'L';
quit;
proc sql;

create table endorser as
select DISTINCT a.bf_ssn
	,a.ln_seq
	,trim(d.dm_prs_1) || ' ' || trim(d.dm_prs_lst) as EdsName
	,c.lc_eds_typ
	,D.DF_SPE_ACC_ID AS CDF_SPE_ACC_ID
	,e.DX_STR_ADR_1 as cDX_STR_ADR_1
	,e.DX_STR_ADR_2 as cDX_STR_ADR_2
	,e.DM_CT as cDM_CT
	,e.DC_DOM_ST as cDC_DOM_ST
	,e.DF_ZIP_CDE as cDF_ZIP_CDE
	,e.DC_FGN_CNY as cDC_FGN_CNY
from dlq A
INNER JOIN BORR_ADR F
	ON A.BF_SSN = F.BF_SSN
inner join OLWHRM1.LN20_EDS c
	on a.bf_ssn = c.bf_ssn
	and a.ln_seq = c.ln_seq
left outer join OLWHRM1.PD10_PRS_NME d
	on c.lf_eds = d.df_prs_id
LEFT OUTER JOIN OLWHRM1.PD30_PRS_ADR e
	on d.df_prs_id = e.df_prs_id
	and e.dc_adr = 'L'
	and substr(e.DX_STR_ADR_1,1,4) ^= substr(F.DX_STR_ADR_1,1,4)
;
quit;



ENDRSUBMIT;

DATA BILLNOTSENT; SET DUSTER.BILLNOTSENT; RUN;
DATA dlq; SET DUSTER.dlq; RUN;
DATA BORR_ADR; SET DUSTER.BORR_ADR; RUN;
DATA endorser; SET DUSTER.endorser; RUN;

PROC FORMAT LIB=work;
	VALUE $SNTIND
		'A' = 'ACH BILL-NOT PRINTED'
		'B' = 'ACH BILL NOT PRINTED < 15 DAYS NOTICE'
		'C' = 'ACH 1ST NOTICE NOT PRINTED'
		'D' ='ACH INSUFFICIENT LEAD TIME NOT PRINTED'
		'E' = 'MONTHLY ACH BILL NOT PRINTED'
		'F' = 'ACH BILL PRINTED NOT SENT<15 DAYS NOTICE'
		'H' = 'ACH INSUFFICIENT LEAD TIME NOT PRINTED'
		'I' = 'MONTHLY ACH BILL NOT PRINTED'
		'J' = '<15 DAYS REPRINT REQUEST NOT PRINTED'
		'K' = 'ACH 1ST NOTICE REPRINT RQST NOT PRINTD'
		'L' = 'ACH INSUFFICIENT LEAD TIME RPQ NOT PRNTD'
		'M' = 'MONTHLY ACH BILL REPRINT RQST NOT PRINTD'
		'N' = 'ACH PAID AHEAD NOT PRINTED'
		'P' = 'ACH REPRINT REQUEST NOT PRINTED'
		'Q' = 'ACH PAID AHEAD REPRINT REQST NOT PRINTED'
		'0' = 'PRE-CONVERSION BILL'
		'5' = 'PAID AHEAD BILL CREATED NOT PRINTED'
		'6' = 'BILL NOT SENT - INVALID ADDRESS'
		'8' = 'INACTIVE LOANS INCLUDED IN BILL'
		'9' = 'LOANS PAID IN FULL NOT PRINTED';
RUN;

PROC SORT DATA=BILLNOTSENT; BY BorName; RUN;

DATA _NULL_;
SET BILLNOTSENT ;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
lc_ind_bil_snt_desc = lc_ind_bil_snt;
FORMAT tot_du 10.2 ;
FORMAT ld_bil_crt ld_bil_du_lon MMDDYY10. ;
FORMAT lc_ind_bil_snt_desc $SNTIND. ;
PUT df_spe_acc_id ln_seq ld_bil_crt ld_bil_du_lon tot_du lc_ind_bil_snt lc_ind_bil_snt_desc lc_bil_mtd $ ;
RUN;

PROC SORT DATA=dlq; BY BF_SSN LN_SEQ; RUN;
PROC SORT DATA=BORR_ADR; BY BF_SSN ; RUN;
PROC SORT DATA=endorser; BY BF_SSN LN_SEQ; RUN;

DATA dlq;
MERGE dlq BORR_ADR(IN=A);
BY BF_SSN;
IF A;
RUN;

DATA dlq(KEEP=LN_SEQ DF_SPE_ACC_ID BF_SSN BorName EDSNAME DX_STR_ADR_1 DX_STR_ADR_2 DM_CT DC_DOM_ST DF_ZIP_CDE DC_FGN_CNY ACSKEY IC_LON_PGM 
	LF_LON_CUR_OWN LD_LON_1_DSB DAYS_DLQ LD_BIL_DU_LON LA_LTE_FEE_OTS_PRT LA_BIL_PAS_DU L_TYPE STATE_CODE CCC);
MERGE dlq endorser;
BY BF_SSN LN_SEQ;
KEYSSN = TRANSLATE(BF_SSN,'MYLAUGHTER','0987654321');
MODAY = PUT(DATE(),MMDDYYN4.);
KEYLINE = "P"||KEYSSN||MODAY||'L';
CHKDIG = 0;
LENGTH DIG L_TYPE $2.;
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
STATE_CODE = DC_DOM_ST;
if DC_DOM_ST = 'FC' THEN STATE_CODE = '';
CCC = 'MA2324';

IF lc_eds_typ = '' THEN L_TYPE = 'B';
ELSE IF LC_EDS_TYP = 'M' THEN DO;
	IF cDX_STR_ADR_1 = '' THEN L_TYPE = 'BC'; 
	ELSE L_TYPE = 'B';
END;
ELSE IF LC_EDS_TYP = 'S' THEN DO;
	IF cDX_STR_ADR_1 = '' THEN L_TYPE = 'BE'; 
	ELSE L_TYPE = 'B';
END;

OUTPUT;
IF LC_EDS_TYP IN ('S','M') AND L_TYPE = 'B' THEN DO;
	DF_SPE_ACC_ID = CDF_SPE_ACC_ID;
	DX_STR_ADR_1 = cDX_STR_ADR_1;
	DX_STR_ADR_2 = cDX_STR_ADR_2;
	DM_CT = cDM_CT;
	DC_DOM_ST = cDC_DOM_ST;
	DF_ZIP_CDE = cDF_ZIP_CDE;
	DC_FGN_CNY = cDC_FGN_CNY;
	BorName = EDSNAME;
	IF LC_EDS_TYP = 'S' THEN L_TYPE = 'E';
	ELSE L_TYPE = 'C';
	OUTPUT;
END;
RUN;
PROC SORT DATA=dlq; BY STATE_CODE BF_SSN DF_SPE_ACC_ID LN_SEQ; RUN;

%MACRO DDL(RPNO,NUM1,NUM2);

data _null_;
set dlq;
WHERE DAYS_DLQ BETWEEN &NUM1 AND &NUM2;
file REPORT&RPNO delimiter=',' DSD DROPOVER lrecl=32767;
   format LD_LON_1_DSB LD_BIL_DU_LON MMDDYY10. ;
   format LA_LTE_FEE_OTS_PRT LA_BIL_PAS_DU 9.2;

IF L_TYPE = 'B' THEN EDSNAME = '';
if _n_ = 1 then put 'LN_SEQ,DF_SPE_ACC_ID,BOR_SSN,NAME,C_NAME,DX_STR_ADR_1,DX_STR_ADR_2,DM_CT,DC_DOM_ST,DF_ZIP_CDE,DC_FGN_CNY,ACSKEY,'
	'IC_LON_PGM,LF_LON_CUR_OWN,LD_DSB,DAYS_DLQ,LD_BIL_DU,LA_LTE_FEE_OTS_PRT,LA_BIL_PAS_DU,L_TYPE,STATE_CODE,CCC';

PUT LN_SEQ DF_SPE_ACC_ID BF_SSN BorName EDSNAME DX_STR_ADR_1 DX_STR_ADR_2 DM_CT DC_DOM_ST DF_ZIP_CDE DC_FGN_CNY ACSKEY IC_LON_PGM 
	LF_LON_CUR_OWN LD_LON_1_DSB DAYS_DLQ LD_BIL_DU_LON LA_LTE_FEE_OTS_PRT LA_BIL_PAS_DU L_TYPE STATE_CODE CCC $;
run;

%MEND DDL;

%DDL(5,31,60);
%DDL(6,61,90);
%DDL(7,91,120);
%DDL(8,121,150);
%DDL(9,151,180);
%DDL(10,181,210);
%DDL(11,211,240);
%DDL(12,241,270);
