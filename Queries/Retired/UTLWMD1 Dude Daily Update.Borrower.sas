/**************************************************************
* IDENTIFY BORROWER ROWS WITH NO CHANGES - ONE QUERY PER DATA FILE
***************************************************************/
PROC SQL;
CREATE TABLE NO_PD10_DELTA AS
SELECT DISTINCT A.DF_SPE_ACC_ID
FROM PD10 A
INNER JOIN DUDE.BORROWER B
      ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
      AND A.BF_SSN = B.BF_SSN;

CREATE TABLE NO_DL200_DELTA AS
SELECT DISTINCT A.DF_SPE_ACC_ID
FROM DL200 A
INNER JOIN DUDE.BORROWER B
      ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
	  AND A.LAST_DL200 = B.LAST_DL200
      AND A.DL200 = B.DL200;

CREATE TABLE NO_DRLFA_DELTA AS
SELECT DISTINCT A.DF_SPE_ACC_ID
FROM DRLFA A
INNER JOIN DUDE.BORROWER B
      ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
      AND A.FEE_WAV_DOL = B.FEE_WAV_DOL
	  AND A.FEE_WAV_CT = B.FEE_WAV_CT;

CREATE TABLE NO_BR02_DELTA AS
SELECT DISTINCT A.DF_SPE_ACC_ID
FROM BR02 A
INNER JOIN DUDE.BORROWER B
      ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
      AND A.IM_IST_FUL = B.IM_IST_FUL
	  AND A.IX_GEN_STR_ADR_1 = B.IX_GEN_STR_ADR_1
	  AND A.IX_GEN_STR_ADR_2 = B.IX_GEN_STR_ADR_2
	  AND A.IM_GEN_CT = B.IM_GEN_CT
	  AND A.IC_GEN_ST = B.IC_GEN_ST
	  AND A.IF_GEN_ZIP = B.IF_GEN_ZIP
	  AND A.IN_GEN_PHN = B.IN_GEN_PHN;

CREATE TABLE NO_BR30_DELTA AS
SELECT DISTINCT A.DF_SPE_ACC_ID
FROM BR30 A
INNER JOIN DUDE.BORROWER B
      ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
	  	AND A.BN_EFT_SEQ = B.BN_EFT_SEQ
      AND A.BF_EFT_ABA = B.BF_EFT_ABA
	  AND A.BF_EFT_ACC = B.BF_EFT_ACC
	  AND A.BC_EFT_STA = B.BC_EFT_STA
	  	AND A.BD_EFT_STA = B.BD_EFT_STA
	  AND A.BA_EFT_ADD_WDR = B.BA_EFT_ADD_WDR
	  AND A.BN_EFT_NSF_CTR = B.BN_EFT_NSF_CTR
	  AND A.BC_EFT_DNL_REA = B.BC_EFT_DNL_REA;

CREATE TABLE NO_LN10_BOR_DELTA AS
SELECT DISTINCT A.DF_SPE_ACC_ID
FROM LN10_BOR A
INNER JOIN DUDE.BORROWER B
      ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
      AND A.LA_CUR_PRI = B.LA_CUR_PRI
	  AND A.WA_TOT_BRI_OTS = B.WA_TOT_BRI_OTS
	  AND A.COHORT = B.COHORT
	  AND A.COHORT_IND = B.COHORT_IND
	  AND A.LR_ITR_DLY = B.LR_ITR_DLY
	  AND A.LR_ITR_MONTH = B.LR_ITR_MONTH
	  AND A.LR_ITR_MONTH_5 = B.LR_ITR_MONTH_5
	  AND A.BIL_MTD = B.BIL_MTD
	  and a.cur_dlq = b.cur_dlq
	  and a.ld_dlq_occ = b.ld_dlq_occ
;

CREATE TABLE NO_LN65_BOR_DELTA AS
SELECT DISTINCT A.DF_SPE_ACC_ID
FROM LN65_BOR A
INNER JOIN DUDE.BORROWER B
      ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
      AND A.LD_CRT_LON65 = B.LD_CRT_LON65
	  AND A.DUE_DAY = B.DUE_DAY
	  AND A.MONTH_AMT = B.MONTH_AMT
	  AND A.MULT_DUE_DT = B.MULT_DUE_DT;

CREATE TABLE NO_MON_36_DELTA AS
SELECT DISTINCT A.DF_SPE_ACC_ID
FROM MON_36 A
INNER JOIN DUDE.BORROWER B
      ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
      AND A.FORB_36 = B.FORB_36; 

CREATE TABLE NO_MAX_PAY_DELTA AS
SELECT DISTINCT A.DF_SPE_ACC_ID
FROM MAX_PAY A
INNER JOIN DUDE.BORROWER B
      ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
      AND A.LST_PMT_RCVD = B.LST_PMT_RCVD; 

CREATE TABLE NO_RPF_DELTA AS
SELECT DISTINCT A.DF_SPE_ACC_ID
FROM RPF A
INNER JOIN DUDE.BORROWER B
      ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
	  AND A.MONTH_AMT = B.MONTH_AMT
	  AND A.DUE_DAY = B.DUE_DAY

;
QUIT;
/**************************************************
* IDENTIFY NEW OR CHANGED ROWS FOR BORROWER TABLE 
***************************************************/
PROC SQL;
CREATE TABLE PD10_DELTAS AS
SELECT DISTINCT A.*
FROM PD10 A
LEFT JOIN NO_PD10_DELTA B
	ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
WHERE B.DF_SPE_ACC_ID IS NULL;

CREATE TABLE DL200_DELTAS AS
SELECT DISTINCT A.*
FROM DL200 A
LEFT JOIN NO_DL200_DELTA B
	ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
WHERE B.DF_SPE_ACC_ID IS NULL;

CREATE TABLE DRLFA_DELTAS AS
SELECT DISTINCT A.*
FROM DRLFA A
LEFT JOIN NO_DRLFA_DELTA B
	ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
WHERE B.DF_SPE_ACC_ID IS NULL;

CREATE TABLE BR02_DELTAS AS
SELECT DISTINCT A.*
FROM BR02 A
LEFT JOIN NO_BR02_DELTA B
	ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
WHERE B.DF_SPE_ACC_ID IS NULL;

CREATE TABLE BR30_DELTAS AS
SELECT DISTINCT A.*
FROM BR30 A
LEFT JOIN NO_BR30_DELTA B
	ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
WHERE B.DF_SPE_ACC_ID IS NULL;

CREATE TABLE LN10_BOR_DELTAS AS
SELECT DISTINCT A.*
FROM LN10_BOR A
LEFT JOIN NO_LN10_BOR_DELTA B
	ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
WHERE B.DF_SPE_ACC_ID IS NULL;

CREATE TABLE LN65_BOR_DELTAS AS
SELECT DISTINCT A.*
FROM LN65_BOR A
LEFT JOIN NO_LN65_BOR_DELTA B
	ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
WHERE B.DF_SPE_ACC_ID IS NULL;

CREATE TABLE MON_36_DELTAS AS
SELECT DISTINCT A.*
FROM MON_36 A
LEFT JOIN NO_MON_36_DELTA B
	ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
WHERE B.DF_SPE_ACC_ID IS NULL;

CREATE TABLE MAX_PAY_DELTAS AS
SELECT DISTINCT A.*
FROM MAX_PAY A
LEFT JOIN NO_MAX_PAY_DELTA B
	ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
WHERE B.DF_SPE_ACC_ID IS NULL;

CREATE TABLE RPF_DELTAS AS
SELECT DISTINCT A.*
FROM RPF A
LEFT JOIN NO_RPF_DELTA B
	ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
WHERE B.DF_SPE_ACC_ID IS NULL;

QUIT;

/***************************************************
* GET DISTINCT LIST OF BORROWERS AND ROWS FROM LOCAL DB
****************************************************/
DATA BORROWER_DELTAS (KEEP=DF_SPE_ACC_ID);
/*FOR TESTING, JUST ONE TABLE*/
      SET PD10_DELTAS DL200_DELTAS DRLFA_DELTAS	 
		BR02_DELTAS BR30_DELTAS LN65_BOR_DELTAS  
		LN10_BOR_DELTAS	MON_36_DELTAS MAX_PAY_DELTAS RPF_DELTAS;
RUN;

PROC SORT DATA=BORROWER_DELTAS NODUPKEY;
	BY DF_SPE_ACC_ID;
RUN;


/***************************************************
* GET LOCAL DATA FOR BORROWERS WITH CHANGES
****************************************************/
PROC SQL;
CREATE TABLE LOCL_BORROWER_DELTAS AS
      SELECT A.* 
      FROM DUDE.BORROWER A
      INNER JOIN BORROWER_DELTAS B
            ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
	order by df_spe_acc_id;
QUIT;

PROC SQL ;
INSERT INTO DUDE.BORROWER_DELETE
      SELECT DF_SPE_ACC_ID
      FROM LOCL_BORROWER_DELTAS; 
QUIT;

%UPDATE_DATA(LOCL_BORROWER_DELTAS,PD10_DELTAS,DF_SPE_ACC_ID);
%UPDATE_DATA(LOCL_BORROWER_DELTAS,DL200_DELTAS,DF_SPE_ACC_ID);
%UPDATE_DATA(LOCL_BORROWER_DELTAS,DRLFA_DELTAS,DF_SPE_ACC_ID);
%UPDATE_DATA(LOCL_BORROWER_DELTAS,BR02_DELTAS,DF_SPE_ACC_ID);
%UPDATE_DATA(LOCL_BORROWER_DELTAS,BR30_DELTAS,DF_SPE_ACC_ID); 
%UPDATE_DATA(LOCL_BORROWER_DELTAS,LN65_BOR_DELTAS,DF_SPE_ACC_ID); 
%UPDATE_DATA(LOCL_BORROWER_DELTAS,LN10_BOR_DELTAS,DF_SPE_ACC_ID); 
%UPDATE_DATA(LOCL_BORROWER_DELTAS,MON_36_DELTAS,DF_SPE_ACC_ID);  
%UPDATE_DATA(LOCL_BORROWER_DELTAS,MAX_PAY_DELTAS,DF_SPE_ACC_ID);
%UPDATE_DATA(LOCL_BORROWER_DELTAS,RPF_DELTAS,DF_SPE_ACC_ID); 

PROC SQL NOPRINT;
CONNECT TO ODBC AS MD (&MD);
SELECT COUNT(*) 
FROM CONNECTION TO MD (
      DELETE BORROWER
      FROM BORROWER A
      INNER JOIN BORROWER_DELETE B
            ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID 
      );
DISCONNECT FROM MD;
QUIT;

PROC SQL ;
INSERT INTO DUDE.BORROWER
      SELECT * 
      FROM LOCL_BORROWER_DELTAS
	  where bf_ssn ^= ''
	; 
QUIT;


/*ELIMINATE NULL VALUES*/

%GOODBYE_NULL(BORROWER,DL200);
%GOODBYE_NULLCHAR(BORROWER,LAST_DL200);
%GOODBYE_NULL(BORROWER,FEE_WAV_DOL);
%GOODBYE_NULL(BORROWER,FEE_WAV_CT);
%GOODBYE_NULLCHAR(BORROWER,LD_CRT_LON65);
%GOODBYE_NULLCHAR(BORROWER,DUE_DAY);
%GOODBYE_NULLCHAR(BORROWER,MONTH_AMT);
%GOODBYE_NULL(BORROWER,BN_EFT_SEQ);
%GOODBYE_NULLCHAR(BORROWER,BF_EFT_ABA);
%GOODBYE_NULLCHAR(BORROWER,BF_EFT_ACC);
%GOODBYE_NULLCHAR(BORROWER,BC_EFT_STA);
%GOODBYE_NULLCHAR(BORROWER,BD_EFT_STA);
%GOODBYE_NULL(BORROWER,BA_EFT_ADD_WDR);
%GOODBYE_NULL(BORROWER,BN_EFT_NSF_CTR);
%GOODBYE_NULLCHAR(BORROWER,BC_EFT_DNL_REA);
%GOODBYE_NULLCHAR(BORROWER,LD_DLQ_OCC);
%GOODBYE_NULL(BORROWER,CUR_DLQ);
%GOODBYE_NULLCHAR(BORROWER,IM_IST_FUL);
%GOODBYE_NULLCHAR(BORROWER,IX_GEN_STR_ADR_1);
%GOODBYE_NULLCHAR(BORROWER,IX_GEN_STR_ADR_2);
%GOODBYE_NULLCHAR(BORROWER,IM_GEN_CT);
%GOODBYE_NULLCHAR(BORROWER,IC_GEN_ST);
%GOODBYE_NULLCHAR(BORROWER,IF_GEN_ZIP);
%GOODBYE_NULLCHAR(BORROWER,IN_GEN_PHN);
%GOODBYE_NULL(BORROWER,WA_TOT_BRI_OTS);
%GOODBYE_NULLCHAR(BORROWER,COHORT_IND,BLANK='N');
%GOODBYE_NULL(BORROWER,LA_BIL_CUR_DU);
%GOODBYE_NULL(BORROWER,LA_BIL_PAS_DU);
%GOODBYE_NULL(BORROWER,TOT_DUE);
%GOODBYE_NULL(BORROWER,TOT_DUE_FEE);
%GOODBYE_NULL(BORROWER,LA_CUR_PRI);
%GOODBYE_NULLCHAR(BORROWER,LD_RHB_IND,BLANK='N');
%GOODBYE_NULL(BORROWER,LR_ITR_DLY);
%GOODBYE_NULL(BORROWER,LR_ITR_MONTH);
%GOODBYE_NULL(BORROWER,LR_ITR_MONTH_5);
%GOODBYE_NULLCHAR(BORROWER,LC_BIL_MTD);
%GOODBYE_NULLCHAR(BORROWER,BIL_MTD);
%GOODBYE_NULLCHAR(BORROWER,COBORROWER,BLANK='N');
%GOODBYE_NULLCHAR(BORROWER,FORB_36,BLANK='N');
%GOODBYE_NULLCHAR(BORROWER,MULT_BIL_MTD,BLANK='N');
%GOODBYE_NULLCHAR(BORROWER,MULT_DUE_DT,BLANK='N');
%GOODBYE_NULLCHAR(BORROWER,LD_RHB);
%GOODBYE_NULLCHAR(BORROWER,COHORT);
%GOODBYE_NULLCHAR(BORROWER,LST_PMT_RCVD);
%GOODBYE_NULLCHAR(BORROWER,PEND_DISB,BLANK='N');
%GOODBYE_NULL(BORROWER,CUR_DLQ);
%GOODBYE_NULLCHAR(BORROWER,ld_dlq_occ);


PROC SQL NOPRINT;
CONNECT TO ODBC AS MD (&MD);
SELECT COUNT(*) 
FROM CONNECTION TO MD (
      DELETE 
      FROM BORROWER_DELETE 
      );
QUIT;
