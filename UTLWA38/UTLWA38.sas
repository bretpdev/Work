/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWA38.LWA38RZ";
FILENAME REPORT2 "&RPTLIB/ULWA38.LWA38R2";
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;

DATA _NULL_;
     CALL SYMPUT('EFFDATE',"'"||PUT(INTNX('MONTH',TODAY(),-1,'END'), MMDDYYD10.)||"'");
RUN;

%SYSLPUT EFFDATE = &EFFDATE;

RSUBMIT;
/*LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/

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
      CONNECT TO DB2 (DATABASE=DLGSUTWH);

      CREATE TABLE DEMO AS
            SELECT 
                  *
            FROM 
                  CONNECTION TO DB2 
                        (
                              SELECT DISTINCT
                                    LN10.BF_SSN
                                    ,LN10.LN_SEQ
                                    ,LN10.LF_GTR_RFR
                                    ,LN10.LF_LON_CUR_OWN
                                    ,LN10.IC_LON_PGM
                                    ,CASE
                                          WHEN LN83.LC_STA_LN83 = 'A' AND LN83.LC_EFT_SUS_REA = ' ' THEN 'Y'
                                          ELSE 'N'
                                    END AS ACH_STATUS
                                    ,LN83.LD_EFT_EFF_BEG
                                    ,LN83.LD_EFT_EFF_END
                                    ,CASE
                                          When LN83.LC_STA_LN83 = 'A' AND LN83.LC_EFT_SUS_REA = ' ' then LN83.LR_EFT_RDC
                                          ELSE NULL
                                    END as LR_EFT_RDC
                                    ,CASE
                                          WHEN LN54.PM_BBS_PGM = 'GRD' AND LN54.LC_BBS_ELG = 'Y' THEN 'GRAD'
                                          WHEN LN54.PM_BBS_PGM = 'W24' AND LN54.LC_BBS_ELG = 'Y' THEN 'Q24M'
                                          WHEN LN54.PM_BBS_PGM = 'O24' AND LN54.LC_BBS_ELG = 'Y' THEN 'Q24OT'
                                          WHEN LN54.PM_BBS_PGM = 'WY2' AND LN54.LC_BBS_ELG = 'Y' THEN 'WY2'
                                          WHEN LN54.PM_BBS_PGM = 'WY3' AND LN54.LC_BBS_ELG = 'Y' THEN 'WY3'
                                          WHEN LN54.PM_BBS_PGM IN ('GRD','W24','O24','WY2','WY3') AND LN54.LC_BBS_ELG IN ('N','X') THEN 'D'
                                          ELSE 'N'
                                      END AS TIMELY_PAY_STATUS
                                    ,LN84.LD_RDC_EFF_BEG
                                    ,LN84.LD_RDC_EFF_END
                                    ,LN84.LR_RDC
                                    ,LN72.LR_ITR
                                    ,LN72.LR_INT_RDC_PGM_ORG
                                    ,LN72.LD_ITR_EFF_BEG AS RATE_EFF_BEG_DTE
                                    ,LN72.LD_ITR_EFF_END AS RATE_EFF_END_DTE
                              FROM  
                                    OLWHRM1.LN10_LON LN10
                                    LEFT JOIN OLWHRM1.LN83_EFT_TO_LON LN83
                                          ON LN10.BF_SSN = LN83.BF_SSN
                                          AND LN10.LN_SEQ = LN83.LN_SEQ
                                          AND LN83.LC_STA_LN83 = 'A'
                                          and (&EFFDATE between LN83.LD_EFT_EFF_BEG and LN83.LD_EFT_EFF_END or 
    										  &EFFDATE > LN83.LD_EFT_EFF_BEG and LN83.LD_EFT_EFF_END is null)
                                    LEFT JOIN OLWHRM1.LN54_LON_BBS LN54
                                          ON LN10.BF_SSN = LN54.BF_SSN
                                          AND LN10.LN_SEQ = LN54.LN_SEQ
                                          AND LN54.LC_STA_LN54 = 'A'
                                    LEFT JOIN OLWHRM1.LN84_LON_RTE_RDC LN84
                                          ON LN10.BF_SSN = LN84.BF_SSN
                                          AND LN10.LN_SEQ = LN84.LN_SEQ
                                          AND LN84.LC_STA_LON84 = 'A'
                                    LEFT JOIN OLWHRM1.LN72_INT_RTE_HST LN72
                                          ON LN10.BF_SSN = LN72.BF_SSN
                                          AND LN10.LN_SEQ = LN72.LN_SEQ
                                          AND LN72.LC_STA_LON72 = 'A'
                                          AND &EFFDATE BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
                              WHERE 
                                    LN10.LF_LON_CUR_OWN IN ('826717','830248')
                                    AND LN10.LD_LON_EFF_ADD > '01/01/2014'

                              FOR READ ONLY WITH UR
                        )
      ;

      DISCONNECT FROM DB2;

      /*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
      /*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO;
      SET DUSTER.DEMO;
RUN;

DATA _NULL_;
SET  WORK.DEMO; 
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
   FORMAT BF_SSN $9. ;
   FORMAT LN_SEQ $3. ;
   FORMAT LF_GTR_RFR $12. ;
   FORMAT LF_LON_CUR_OWN $8. ;
   FORMAT IC_LON_PGM $6. ;
   FORMAT ACH_STATUS $1. ;
   FORMAT LD_EFT_EFF_BEG DATE9. ;
   FORMAT LD_EFT_EFF_END DATE9. ;
   FORMAT LR_EFT_RDC BEST12. ;
   FORMAT TIMELY_PAY_STATUS $5. ;
   FORMAT LD_RDC_EFF_BEG DATE9. ;
   FORMAT LD_RDC_EFF_END DATE9. ;
   FORMAT LR_RDC BEST12. ;
   FORMAT LR_ITR BEST12. ;
   FORMAT LR_INT_RDC_PGM_ORG BEST12. ;
   FORMAT LD_ITR_EFF_BEG DATE9. ;
   FORMAT LD_ITR_EFF_END DATE9. ;

      IF _N_ = 1 THEN   PUT      'BF_SSN,LN_SEQ,CLUID,IF_OWN,IC_LON_PGM,ACH_STATUS,LD_EFT_EFF_BEG,LD_RDC_EFF_END,LR_EFT_RDC,TIMELY_PMT_STATUS,LR_RDC_EFF_BEG,LR_RDC_EFF_END,LR_RDC,LR_ITR,LR_ITR_ORG,LD_ITR_EFF_BEG,LD_ITR_EFF_END';

DO;
      PUT BF_SSN $ @;
      PUT LN_SEQ $ @;
      PUT LF_GTR_RFR $ @;
      PUT LF_LON_CUR_OWN $ @;
      PUT IC_LON_PGM $ @;
      PUT ACH_STATUS $ @;
      PUT LD_EFT_EFF_BEG @;
      PUT LD_EFT_EFF_END @;
      PUT LR_EFT_RDC @;
      PUT TIMELY_PAY_STATUS $ @;
      PUT LD_RDC_EFF_BEG @;
      PUT LD_RDC_EFF_END @;
      PUT LR_RDC @;
      PUT LR_ITR @;
      PUT LR_INT_RDC_PGM_ORG @;
      PUT LD_ITR_EFF_BEG @;
      PUT LD_ITR_EFF_END ;
END;
RUN;
