/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS18.NWS18RZ";
FILENAME REPORT2 "&RPTLIB/UNWS18.NWS18R2";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
%let DB = DNFPRQUT;  *This is test;
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
/*%let DB = DNFPUTDL;  *This is live;*/

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;

/*LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;*/
/*LIBNAME PKUR DB2 DATABASE=&DB OWNER=PKUR;*/
/*LIBNAME FRDWODS DB2 DATABASE=&DB OWNER=FRDWODS;*/

PROC SQL;
CONNECT TO DB2 (DATABASE=&db);
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT PD10.DF_SPE_ACC_ID
, RFR.NUM_RFR

FROM PKUB.PD10_PRS_NME PD10
      INNER JOIN pkub.PD26_PRS_SKP_PRC PD26
      ON PD26.DF_PRS_ID = PD10.DF_PRS_ID
LEFT JOIN(
          SELECT 
                      RF10.BF_SSN
                      
                ,COUNT(DISTINCT RF10.BF_RFR)as NUM_RFR
         
                FROM pkub.RF10_RFR RF10
                      
          
                WHERE RF10.BC_STA_REFR10 = 'A'
                AND RF10.BC_RFR_REL_BR NOT IN ('13','15')
          GROUP BY RF10.BF_SSN
                ) RFR
            ON PD10.DF_PRS_ID = RFR.BF_SSN
      Left JOIN (
                Select DISTINCT 
                RF10.BF_SSN

                FROM pkub.RF10_RFR RF10
                WHERE RF10.BC_REA_RFR_HST IN ('D','R')
				) RFR2
            ON PD10.DF_PRS_ID = RFR2.BF_SSN
INNER JOIN PKUB.LN10_LON LN10
	ON PD10.DF_PRS_ID = LN10.BF_SSN
	AND LN10.LC_STA_LON10 = 'R'
	AND LN10.LA_CUR_PRI > 0
LEFT JOIN (
			SELECT DISTINCT
				BF_SSN
			FROM
				PKUB.AY10_BR_LON_ATY AY10
			WHERE 
				AY10.PF_REQ_ACT = 'SKP2R'
			) AY10_ARC
		ON AY10_ARC.BF_SSN = LN10.BF_SSN
WHERE PD26.DC_STA_SKP = '2' 
AND (RFR.NUM_RFR < 2 or RFR.NUM_RFR is null)
AND RFR2.BF_SSN is NULL
AND AY10_ARC.BF_SSN IS NULL

FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

QUIT;

ENDRSUBMIT;
DATA DEMO; 	SET LEGEND.DEMO; RUN;
proc sort data=demo; by df_spe_acc_id; run;

DATA _NULL_;
SET DEMO ;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
PUT DF_SPE_ACC_ID 'SKP2R,,,,,,,ALL,Borrower is in skip with less than two references' ;
RUN;


