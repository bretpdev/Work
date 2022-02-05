/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS66.NWS66RZ";
FILENAME REPORT2 "&RPTLIB/UNWS66.NWS66R2";

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
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE BRWS AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						LN10.BF_SSN,
						LN20.LF_EDS
					FROM 
						PKUB.LN10_LON LN10		
						JOIN PKUB.LN16_LON_DLQ_HST LN16
							ON LN10.BF_SSN = LN16.BF_SSN
							AND LN10.LN_SEQ = LN16.LN_SEQ
							AND LN10.LC_STA_LON10 = 'R'
							AND LN10.LA_CUR_PRI > 0
							AND LN10.IC_LON_PGM NOT IN ('DLPLUS', 'DPLUS', 'DLPCNS', 'PLUS', 'FISL')
							AND LN10.LC_TL4_IBR_ELG <> 'I'
							AND LN16.LN_DLQ_MAX + 1 BETWEEN 90 AND 270
							AND LN16.LC_STA_LON16 = '1'
						JOIN PKUB.DW01_DW_CLC_CLU DW01
							ON LN10.BF_SSN = DW01.BF_SSN
							AND LN10.LN_SEQ = DW01.LN_SEQ
							AND DW01.WC_DW_LON_STA IN ('03','13','14')
						LEFT JOIN PKUB.LN20_EDS LN20
							ON LN10.BF_SSN = LN20.BF_SSN
							AND LN10.LN_SEQ = LN20.LN_SEQ
							AND LN20.LC_STA_LON20 = 'A'
							AND LN20.LC_EDS_TYP = 'M'

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE DEMOS AS
		SELECT
			*
		FROM
			CONNECTION TO DB2
				(
					SELECT DISTINCT
						PD10.DF_PRS_ID,
						PD10.DF_SPE_ACC_ID,
						PD10.DM_PRS_1,
						PD10.DM_PRS_LST,
						PD32.DX_ADR_EML
					FROM
						PKUB.PD10_PRS_NME PD10
						JOIN PKUB.PD32_PRS_ADR_EML PD32
							ON PD10.DF_PRS_ID = PD32.DF_PRS_ID
							AND PD32.DC_STA_PD32 = 'A'
							AND PD32.DI_VLD_ADR_EML = 'Y'

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE S66 AS
		SELECT DISTINCT
			DF_SPE_ACC_ID,
			DM_PRS_1,
			DM_PRS_LST,
			DX_ADR_EML
		FROM
			(
				SELECT
					D.DF_SPE_ACC_ID,
					D.DM_PRS_1,
					D.DM_PRS_LST,
					D.DX_ADR_EML
				FROM
					BRWS B
					JOIN DEMOS D
						ON B.BF_SSN = D.DF_PRS_ID

				UNION

				SELECT
					D.DF_SPE_ACC_ID,
					D.DM_PRS_1,
					D.DM_PRS_LST,
					D.DX_ADR_EML
				FROM
					BRWS B
					JOIN DEMOS D
						ON B.LF_EDS = D.DF_PRS_ID
			)
	;
	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA S66; SET LEGEND.S66; RUN;

/*write to comma delimited file for the Email Batch Script - FED script*/
DATA _NULL_;
	SET		WORK.S66;
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
