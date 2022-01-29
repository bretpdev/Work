/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS75.NWS75RZ";
FILENAME REPORT2 "&RPTLIB/UNWS75.NWS75R2";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
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
	CREATE TABLE DELQ AS
		SELECT DISTINCT
			PD10.DF_SPE_ACC_ID
		FROM
			PKUB.PD10_PRS_NME PD10
			JOIN PKUB.LN10_LON LN10
				ON PD10.DF_PRS_ID = LN10.BF_SSN
				AND LN10.LC_STA_LON10 = 'R'
				AND LN10.LA_CUR_PRI > 0
			JOIN PKUB.LN16_LON_DLQ_HST LN16
				ON LN10.BF_SSN = LN16.BF_SSN
				AND LN10.LN_SEQ = LN16.LN_SEQ
				AND LN16.LC_STA_LON16 = '1'
				AND LN16.LN_DLQ_MAX > 29
			JOIN PKUB.PD40_PRS_PHN PD40
				ON PD10.DF_PRS_ID = PD40.DF_PRS_ID
				AND PD40.DI_PHN_VLD = 'Y'
			LEFT JOIN 
				(
					SELECT
						AY10.BF_SSN
					FROM
						PKUB.AY10_BR_LON_ATY AY10
						JOIN PKUB.AC10_ACT_REQ AC10
							ON AY10.LD_ATY_RSP > Today()-30 
							AND AC10.PC_STA_ACT10 = 'A'
							AND AC10.PC_CCI_CLM_COL_ATY IN ('SD','SO','ZZ','YY')
							AND AY10.PF_REQ_ACT = AC10.PF_REQ_ACT
				) ACT
				ON PD10.DF_PRS_ID = ACT.BF_SSN
			JOIN PKUB.DW01_DW_CLC_CLU DW01
				ON LN10.BF_SSN = DW01.BF_SSN
				AND DW01.WC_DW_LON_STA NOT IN ('16','17','18','19','20','21','22')
			LEFT JOIN PKUB.WQ20_TSK_QUE WQ20
				ON LN10.BF_SSN = WQ20.BF_SSN
				AND (WQ20.WF_QUE = '3C' AND WQ20.WF_SUB_QUE = '01')
				AND WQ20.WC_STA_WQUE20 IN ('A','H','P','U','W')
		WHERE
			ACT.BF_SSN IS NULL
			AND WQ20.BF_SSN IS NULL
	;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DELQ; SET LEGEND.DELQ; RUN;

/*export to queue builder (fed) file*/
DATA _NULL_;
	SET DELQ ;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
	PUT DF_SPE_ACC_ID 'CSV30,,,,,,,ALL,' ;
RUN;
