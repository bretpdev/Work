/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS64.NWS64RZ";
FILENAME REPORT2 "&RPTLIB/UNWS64.NWS64R2";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
%let DB = DNFPRQUT;  *This is test;
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
/*%let DB = DNFPUTDL;  *This is live;*/

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

	CREATE TABLE DEL_SKP_EML AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						PD10.DF_SPE_ACC_ID,
						PD10.DM_PRS_1,
						PD10.DM_PRS_LST,
						PD32.DX_ADR_EML,
						pd40.DI_PHN_VLD,
						pd30.DI_VLD_ADR
					FROM
						PKUB.PD10_PRS_NME PD10
						JOIN PKUB.LN10_LON LN10
							ON PD10.DF_PRS_ID = LN10.BF_SSN
							AND LN10.LA_CUR_PRI > 0
							AND LN10.LC_STA_LON10 = 'R'
						JOIN PKUB.PD32_PRS_ADR_EML PD32
							ON PD10.DF_PRS_ID = PD32.DF_PRS_ID
							AND PD32.DI_VLD_ADR_EML = 'Y'
							AND PD32.DC_STA_PD32 = 'A'
						JOIN PKUB.DW01_DW_CLC_CLU DW01
							ON PD10.DF_PRS_ID = DW01.BF_SSN
							AND DW01.WC_DW_LON_STA NOT IN ('21', '20', '17', '16', '22', '12', '19', '18')
						JOIN PKUB.LN16_LON_DLQ_HST LN16
							ON PD10.DF_PRS_ID = LN16.BF_SSN
							AND LN16.LC_STA_LON16 = '1'
							AND LN16.LN_DLQ_MAX >= 30
						LEFT JOIN PKUB.PD30_PRS_ADR PD30
							ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
							AND PD30.DI_VLD_ADR = 'Y'
						LEFT JOIN PKUB.PD40_PRS_PHN PD40
							ON PD30.DF_PRS_ID = PD40.DF_PRS_ID
							AND PD40.DC_PHN IN ('H','W','A','M')
							AND PD40.DI_PHN_VLD = 'Y'
						LEFT JOIN PKUB.PD30_PRS_ADR PD30N
							ON PD10.DF_PRS_ID = PD30N.DF_PRS_ID
							AND PD30N.DI_VLD_ADR = 'N'
						LEFT JOIN PKUB.PD40_PRS_PHN PD40N
							ON PD10.DF_PRS_ID = PD40N.DF_PRS_ID
							AND PD40N.DC_PHN IN ('H','W','A','M')
							AND PD40N.DI_PHN_VLD = 'N'
						LEFT JOIN PKUB.AY10_BR_LON_ATY AY10
							ON PD10.DF_PRS_ID = AY10.BF_SSN
							AND AY10.PF_REQ_ACT IN ('S1BRC','S1BRA')
					WHERE
						(PD30.DF_PRS_ID IS NULL
						or PD40.DF_PRS_ID IS NULL)
						AND 
							(
								COALESCE(DAYS(AY10.LD_ATY_REQ_RCV),0) >= COALESCE(DAYS(PD30N.DD_VER_ADR),0)
								AND
								COALESCE(DAYS(AY10.LD_ATY_REQ_RCV),0) >= COALESCE(DAYS(PD40N.DD_PHN_VER),0)
							)
					ORDER BY
						DF_SPE_ACC_ID

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEL_SKP_EML; SET LEGEND.DEL_SKP_EML; RUN;

/*write to comma delimited file for the Email Batch Script - FED script*/
DATA _NULL_;
	SET		WORK.DEL_SKP_EML;
	FILE	REPORT2 delimiter=',' DSD DROPOVER lrecl=32767;

	/* write column names, remove this to create a file without a header row */
	IF _N_ = 1 THEN
		DO;
			PUT	
				'DF_SPE_ACC_ID'
				','
				'DM_PRS_1'
				','
				'DM_PRS_LST'
				','
				'DX_ADR_EML'
			;
		END;

	/* write data*/	
	DO;
		PUT DF_SPE_ACC_ID $ @;
		PUT DM_PRS_1 $ @;
		PUT DM_PRS_LST $ @;
		PUT DX_ADR_EML $;
		;
	END;
RUN;
