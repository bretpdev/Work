/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS29.NWS29RZ";
FILENAME REPORT2 "&RPTLIB/UNWS29.NWS29R2";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;

%LET DB = DNFPUTDL;

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
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT	
						DISTINCT
						PD10.DF_SPE_ACC_ID
						,PD10.DM_PRS_1
						,PD10.DM_PRS_LST
						,PD32.DX_ADR_EML
					FROM
						PKUB.PD10_PRS_NME PD10
						INNER JOIN PKUB.LN10_LON LN10
							ON PD10.DF_PRS_ID = LN10.BF_SSN
						INNER JOIN PKUB.LN16_LON_DLQ_HST LN16
							ON LN10.BF_SSN = LN16.BF_SSN
							AND LN10.LN_SEQ = LN16.LN_SEQ
						INNER JOIN PKUB.DW01_DW_CLC_CLU DW01
							ON LN10.BF_SSN = DW01.BF_SSN
							AND LN10.LN_SEQ = DW01.LN_SEQ 
						INNER JOIN PKUB.PD32_PRS_ADR_EML PD32
							ON PD10.DF_PRS_ID = PD32.DF_PRS_ID
					WHERE
						LN10.LC_STA_LON10 = 'R'
						AND LN_DLQ_MAX + 1 BETWEEN 270 AND 330
						AND LN16.LC_STA_LON16 = 1
						AND DW01.WC_DW_LON_STA IN ('03','14')
						AND PD32.DC_STA_PD32 = 'A'
						AND PD32.DI_VLD_ADR_EML = 'Y'					

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

/*	%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*	%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA DEMO; SET LEGEND.DEMO; RUN;

DATA _NULL_;
	SET DEMO ;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
	IF _N_ = 1 THEN DO;
		PUT "DF_SPE_ACC_ID,DM_PRS_1,DM_PRS_LST,DX_ADR_EML";
	END;
	DO;
	   PUT DF_SPE_ACC_ID @;
	   PUT DM_PRS_1 @;
	   PUT DM_PRS_LST @;
	   PUT DX_ADR_EML $ ;
	END;
RUN;
