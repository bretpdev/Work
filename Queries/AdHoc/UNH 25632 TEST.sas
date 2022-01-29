LIBNAME EA27 ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\EA27_BANA_2.dsn; update_lock_typ=nolock; bl_keepnulls=no");

PROC SQL;
      CREATE TABLE SAUCE AS
            SELECT DISTINCT
                  CLS.BORROWER_SSN AS BF_SSN
                  ,CLM.LN_SEQ
                  ,DCER.DEAL_ID
                  ,CLS.COMMONLINE_UNIQUE_ID AS CLUID
                  ,CLS.BORROWER_BENEFIT_CODE AS BBC
                  ,CASE
                        WHEN CLS.BORROWER_BENEFIT_CODE IN ('0012','0210','0230','0800','0810','0845','1010','1020','1030','1500',
                              '1505','1510','1520','1530','1570','1580','1706','1710','1719','2805','4400','5101','6741','6742',
                              '6745','6747','6749','6750','6751','7604','7713','7714','7716','7720','7722') THEN '82976901'
                        WHEN CLS.BORROWER_BENEFIT_CODE = '2585' THEN '82976902'
                        WHEN CLS.BORROWER_BENEFIT_CODE IN ('0830','2590') THEN '82976903'
                        WHEN CLS.BORROWER_BENEFIT_CODE IN ('0245','1548') THEN '82976904'
                        WHEN CLS.BORROWER_BENEFIT_CODE IN ('0290','1560','1565') THEN '82976905'
                        WHEN CLS.BORROWER_BENEFIT_CODE IN ('1550','1553','1555','1558','2510','7600','7601') THEN '82976906'
                        WHEN CLS.BORROWER_BENEFIT_CODE IN ('1545','1585','1590','2740','7602') THEN '82976907'
                        WHEN CLS.BORROWER_BENEFIT_CODE = '7603' THEN '82976908'
                        ELSE 'ERROR'
                  END AS OWN_ID
            FROM 
                  EA27.CRITERIA_LOAN_SALE CLS
                  JOIN EA27._07_08DisbClaimEnrollRecord DCER
                        ON CLS.BORROWER_SSN = DCER.BORROWERSSN
                        AND CLS.LOAN_IDENT = DCER.LOANIDENTIFICATION
                  JOIN EA27.CompassLoanMapping CLM
                        ON CLS.BORROWER_SSN = CLM.BORROWERSSN
                        AND DCER.LOAN_NUMBER = CLM.LOAN_NUMBER
      ;
QUIT;

/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWAB1.LWAB1RZ";
FILENAME REPORT2 "&RPTLIB/ULWAB1.LWAB1R2";
/*LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;*//*live*/
LIBNAME  QADBD004  REMOTE  SERVER=QADBD004 SLIBREF=WORK; /*test*/

DATA DUSTER.CRITERIA_LOAN_SALE; *Send data to Duster; 
      SET SAUCE; 
      WHERE OWN_ID NE 'ERROR';
RUN;

%SYSLPUT DATEINQUESTION = '01JAN2000';
/*RSUBMIT;*/
/*LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*//*live*/
RSUBMIT QADBD004;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSWQUT OWNER=OLWHRM1; /*test*/

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
/*      CONNECT TO DB2 (DATABASE=DLGSUTWH);*/
    CONNECT TO DB2 (DATABASE=DLGSWQUT);


      CREATE TABLE DEMO AS
            SELECT
                  PD10.DF_SPE_ACC_ID || PUT(LN10.LN_SEQ, Z3.) AS BOR
/*                CLS.BF_SSN || PUT(CLS.LN_SEQ, Z3.) AS BOR*/
                  ,CLS.LN_SEQ
                  ,CLS.OWN_ID
                  ,CLS.BF_SSN
                  ,CLS.CLUID
                  ,CLS.DEAL_ID
                          ,LN10.PF_MAJ_BCH
                          ,LN10.PF_MNR_BCH
            FROM  
                  CRITERIA_LOAN_SALE CLS
                  JOIN OLWHRM1.LN10_LON LN10
                        ON CLS.BF_SSN = LN10.BF_SSN
                        AND CLS.LN_SEQ = LN10.LN_SEQ
/*                      AND CLS.CLUID = LN10.LF_LON_ALT || PUT(LN10.LN_LON_ALT_SEQ, 2.)*/
                  JOIN OLWHRM1.PD10_PRS_NME PD10
                        ON CLS.BF_SSN = PD10.DF_PRS_ID
            WHERE 
                  LN10.LC_STA_LON10 = 'P'
                  AND 
                  LN10.LA_CUR_PRI > 0
                  AND
                  LN10.LF_LON_CUR_OWN = '829769'
                          and LN10.PF_MAJ_BCH in ('2016029001','2016029002','2016029003','2016029004')
/*                  AND*/
/*                  LN10.LD_LON_ACL_ADD > INPUT(&DATEINQUESTION, DATE9.)*/
/*                CLS.BF_SSN NOT LIKE '%0'*/

      ;

      CREATE TABLE ALL AS
            SELECT
                  CLS.*
                  ,PD10.DF_SPE_ACC_ID
            FROM 
                  CRITERIA_LOAN_SALE CLS
                  JOIN OLWHRM1.PD10_PRS_NME PD10
                        ON CLS.BF_SSN = PD10.DF_PRS_ID
      ;

      DISCONNECT FROM DB2;

      /*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
      /*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA DEMO;
      SET DUSTER.DEMO;
RUN;

DATA ALL;
      SET DUSTER.ALL;
RUN;

PROC SQL;
      CREATE TABLE R3 AS
            SELECT
                  A.DF_SPE_ACC_ID
                  ,A.BF_SSN
                  ,A.CLUID
                  ,A.OWN_ID
            FROM 
                  ALL A
                  LEFT JOIN DEMO D
                        ON A.BF_SSN = D.BF_SSN
                        AND A.LN_SEQ = D.LN_SEQ
            WHERE 
                  (D.BF_SSN IS NULL AND D.LN_SEQ IS NULL)
;
QUIT;

PROC SQL;
      CREATE TABLE R2 AS
            SELECT
                  D.BOR
                  ,D.OWN_ID
                  ,D.DEAL_ID
            FROM 
                  DEMO D
            ORDER BY 
                  D.DEAL_ID
                  ,D.OWN_ID
                  ,D.BOR
;
QUIT;

/*write to comma delimited file*/
DATA _NULL_;
      SET  WORK.R3;
      FILE 'T:\SAS\NH 25632 R3.txt' delimiter=',' DSD DROPOVER lrecl=32767;

      FORMAT DF_SPE_ACC_ID $10. ;
      FORMAT BF_SSN $9. ;
      FORMAT CLUID $20. ;
      FORMAT OWN_ID $8. ;

      IF _N_ = 1 THEN        /* write column names */
            DO;
                  PUT
                  'DF_SPE_ACC_ID'
                  ','
                  'BORROWER SSN'
                  ','
                  'COMMONLINE UNIQUE ID'
                  ','
                  'OWNER CODE';
            END;

      DO;
            PUT DF_SPE_ACC_ID $ @;
            PUT BF_SSN $ @;
            PUT CLUID $ @;
            PUT OWN_ID $;
            ;
      END;
RUN;

DATA _NULL_;
      SET  WORK.R2;
      FILE 'T:\SAS\NH 25632 R2_UU.txt' delimiter=',' DSD DROPOVER lrecl=32767;
      BY OWN_ID notsorted;
      WHERE DEAL_ID = 'UU';
      FORMAT BOR $13. ;

      IF FIRST.OWN_ID THEN       /* write column names */
            DO;
                  PUT
                  OWN_ID $;
            END;

      DO;
            PUT BOR $;
            ;
      END;
RUN;

DATA _NULL_;
      SET  WORK.R2;
      FILE 'T:\SAS\NH 25632 R2_UO.txt' delimiter=',' DSD DROPOVER lrecl=32767;
      BY OWN_ID notsorted;
      WHERE DEAL_ID = 'UO';
      FORMAT BOR $13. ;

      IF FIRST.OWN_ID THEN       /* write column names */
            DO;
                  PUT
                  OWN_ID $;
            END;

      DO;
            PUT BOR $;
            ;
      END;
RUN;
