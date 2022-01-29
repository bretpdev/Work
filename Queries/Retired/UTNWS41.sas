%LET RPTLIB = %SYSGET(reportdir);
/*%LET RPTLIB = T:\SAS;*/
FILENAME REPORTZ "&RPTLIB/UNWS41.NWS41RZ";
FILENAME REPORT2 "&RPTLIB/UNWS41.NWS41R2";

/*LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;*/
/*RSUBMIT LEGEND;*/
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
%let DB = DNFPUTDL;  *This is live;

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
	CREATE TABLE S41 AS
		SELECT DISTINCT
			PD10.DF_SPE_ACC_ID
		FROM
			PKUB.PD10_PRS_NME PD10
			JOIN PKUB.LN10_LON LN10
				ON PD10.DF_PRS_ID = LN10.BF_SSN
			JOIN PKUB.DW01_DW_CLC_CLU DW01
				ON PD10.DF_PRS_ID = DW01.BF_SSN
			JOIN PKUB.LN72_INT_RTE_HST LN72
				ON LN10.BF_SSN = LN72.BF_SSN
				AND LN10.LN_SEQ = LN72.LN_SEQ
			LEFT JOIN PKUB.AY10_BR_LON_ATY AY10
				ON LN10.BF_SSN = AY10.BF_SSN
				AND AY10.PF_REQ_ACT = 'RSCRA'
				AND AY10.LD_ATY_RSP >= INTNX('MONTH',TODAY(),-12,'SAMEDAY')		
		WHERE
			LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI + DW01.WA_TOT_BRI_OTS > 0
			AND LN72.LC_INT_RDC_PGM = 'M'
			AND LN72.LD_ITR_EFF_END BETWEEN TODAY() AND TODAY() + 14
			AND LN72.LR_ITR <= 6
			AND AY10.BF_SSN IS NULL
	;

	%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;
	%SQLCHECK;
QUIT;

/*ENDRSUBMIT;*/
/**/
/*DATA S41; SET LEGEND.S41; RUN;*/

/*export to queue builder (fed) file*/
DATA _NULL_;
	SET S41 ;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
	PUT DF_SPE_ACC_ID 'RSCRA,,,,,,,ALL,SCRA rate extension review' ;
RUN;
