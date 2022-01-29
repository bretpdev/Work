/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS85.NWS85RZ";
FILENAME REPORT2 "&RPTLIB/UNWS85.NWS85R2";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
%LET DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE 0  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @01 " ********************************************************************* "
              / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @01 " ****  THE SAS SHOULD BE RCVIEWED.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @01 " ****  &SQLXMSG   **** "
              / @01 " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE S85 AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						PD10.DF_SPE_ACC_ID
					FROM
						PKUB.PD10_PRS_NME PD10
						JOIN PKUB.DW01_DW_CLC_CLU DW01
							ON PD10.DF_PRS_ID = DW01.BF_SSN
/*						latest instance of a discharge request*/
						JOIN
							(
								SELECT DISTINCT
									AY10.BF_SSN,
									MAX(AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
								FROM
									PKUB.AY10_BR_LON_ATY AY10
								WHERE
									AY10.PF_REQ_ACT IN ('CSFSA','DEFSA','FCFSA','SSFSA','TLFSA','UPFSA')
								GROUP BY
									AY10.BF_SSN
							) DSCH
							ON PD10.DF_PRS_ID = DSCH.BF_SSN
/*						latest approval/denial*/
						LEFT JOIN
							(
								SELECT DISTINCT
									AY10.BF_SSN,
									MAX(AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
								FROM
									PKUB.AY10_BR_LON_ATY AY10
								WHERE
									AY10.PF_REQ_ACT IN ('ADCSH','CSDNY','ADDTH','DEDNY','ADFCR','FCDNY','ADS11','SSDNY','ADTLF','TLDNY','ADUPR','UPDNY')
								GROUP BY
									AY10.BF_SSN
							) APDN
							ON PD10.DF_PRS_ID = APDN.BF_SSN
					WHERE
						DW01.WC_DW_LON_STA = '03'
						AND (APDN.BF_SSN IS NULL OR APDN.LD_ATY_REQ_RCV < DSCH.LD_ATY_REQ_RCV) /*latest approval/denial does not exist or is before discharge request*/

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA S85; SET LEGEND.S85; RUN;

/*export to queue builder (fed) file*/
DATA _NULL_;
	SET S85 ;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
	PUT DF_SPE_ACC_ID 'DSFRB,,,,,,,ALL,DISCHARGE SENT TO FSA WITH NO FORBEARANCE' ;
RUN;
