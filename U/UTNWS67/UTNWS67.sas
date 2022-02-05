/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS67.NWS67RZ";
FILENAME REPORT2 "&RPTLIB/UNWS67.NWS67R2";

LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
%LET DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;

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
						INNER JOIN PKUB.DW01_DW_CLC_CLU DW01
							ON LN10.BF_SSN = DW01.BF_SSN
							AND LN10.LN_SEQ = DW01.LN_SEQ
							AND LN10.LC_STA_LON10 = 'R'
							AND LN10.LA_CUR_PRI > 0
							AND LN10.LC_TL4_IBR_ELG <> 'I'
							AND LN10.IC_LON_PGM NOT IN ('DLPLUS', 'DPLUS', 'DLPCNS', 'PLUS')
						LEFT JOIN PKUB.LN20_EDS LN20
							ON LN10.BF_SSN = LN20.BF_SSN
							AND LN10.LN_SEQ = LN20.LN_SEQ
							AND LN20.LC_STA_LON20 = 'A'
							AND LN20.LC_EDS_TYP = 'M'	/*COMAKER*/
						LEFT JOIN /*current deferment*/
							(
								SELECT
									LN50.BF_SSN,
									LN50.LN_SEQ,
									LN50.LD_DFR_END
								FROM
									PKUB.LN50_BR_DFR_APV LN50
									INNER JOIN PKUB.DF10_BR_DFR_REQ DF10
										ON LN50.BF_SSN = DF10.BF_SSN
										AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
										AND LN50.LC_STA_LON50 = 'A'
										AND DAYS(LN50.LD_DFR_END) BETWEEN DAYS(CURRENT_DATE) + 15 AND DAYS(CURRENT_DATE) + 30
										AND DF10.LC_DFR_STA = 'A'
										AND DF10.LC_STA_DFR10 = 'A'
							) DEFR
							ON LN10.BF_SSN = DEFR.BF_SSN
							AND LN10.LN_SEQ = DEFR.LN_SEQ
						LEFT JOIN /*current forbearance*/
							(
								SELECT
									LN60.BF_SSN,
									LN60.LN_SEQ,
									LN60.LD_FOR_END
								FROM
									PKUB.LN60_BR_FOR_APV LN60
									INNER JOIN PKUB.FB10_BR_FOR_REQ FB10
										ON LN60.BF_SSN = FB10.BF_SSN
										AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
										AND LN60.LC_STA_LON60 = 'A'
										AND DAYS(LN60.LD_FOR_END) BETWEEN DAYS(CURRENT_DATE) + 15 AND DAYS(CURRENT_DATE) + 30
										AND FB10.LC_FOR_STA = 'A'
										AND FB10.LC_STA_FOR10 = 'A'
										AND FB10.LC_FOR_TYP NOT IN ('28','09','10','13')
							) FORB
							ON LN10.BF_SSN = FORB.BF_SSN
							AND LN10.LN_SEQ = FORB.LN_SEQ
						LEFT JOIN /*future deferment or forbearance*/
							(
								/*future deferment*/
								SELECT
									LN50.BF_SSN,
									LN50.LN_SEQ,
									LN50.LD_DFR_BEG AS LD_NEW_BEG
								FROM
									PKUB.LN50_BR_DFR_APV LN50
									INNER JOIN PKUB.DF10_BR_DFR_REQ DF10
										ON LN50.BF_SSN = DF10.BF_SSN
										AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
										AND LN50.LC_STA_LON50 = 'A'
										AND DF10.LC_DFR_STA = 'A'
										AND DF10.LC_STA_DFR10 = 'A'
										AND DAYS(LN50.LD_DFR_BEG) > DAYS(CURRENT_DATE) + 15
								
								UNION

								/*future forbearance*/
								SELECT
									LN60.BF_SSN,
									LN60.LN_SEQ,
									LN60.LD_FOR_BEG AS LD_NEW_BEG
								FROM
									PKUB.LN60_BR_FOR_APV LN60
									INNER JOIN PKUB.FB10_BR_FOR_REQ FB10
										ON LN60.BF_SSN = FB10.BF_SSN
										AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
										AND LN60.LC_STA_LON60 = 'A'										
										AND FB10.LC_FOR_STA = 'A'
										AND FB10.LC_STA_FOR10 = 'A'
										AND FB10.LC_FOR_TYP NOT IN ('28','09','10','13')
										AND DAYS(LN60.LD_FOR_BEG) > DAYS(CURRENT_DATE) + 15
							) NEW
							ON LN10.BF_SSN = NEW.BF_SSN
							AND LN10.LN_SEQ = NEW.LN_SEQ
							AND
								(
									DAYS(DEFR.LD_DFR_END) + 1 = DAYS(NEW.LD_NEW_BEG)
									OR DAYS(FORB.LD_FOR_END) + 1 = DAYS(NEW.LD_NEW_BEG)
								)
					WHERE
						(
							DW01.WC_DW_LON_STA = '04' 
							AND DEFR.BF_SSN IS NOT NULL
							AND NEW.BF_SSN IS NULL
						)
						OR 
						(
							DW01.WC_DW_LON_STA = '05'
							AND FORB.BF_SSN IS NOT NULL
							AND NEW.BF_SSN IS NULL
						)
						OR
						(	DW01.WC_DW_LON_STA = '01'
							AND DAYS(LN10.LD_END_GRC_PRD) < DAYS(CURRENT_DATE) + 31
						)

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
						INNER JOIN PKUB.PD32_PRS_ADR_EML PD32
							ON PD10.DF_PRS_ID = PD32.DF_PRS_ID
							AND PD32.DC_STA_PD32 = 'A'
							AND PD32.DI_VLD_ADR_EML = 'Y'

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE S67 AS
		SELECT DISTINCT
			DF_SPE_ACC_ID,
			DM_PRS_1,
			DM_PRS_LST,
			DX_ADR_EML
		FROM
			(
				/*borrower and co-borrowers (who are really just borrowers)*/
				SELECT
					D.DF_SPE_ACC_ID,
					D.DM_PRS_1,
					D.DM_PRS_LST,
					D.DX_ADR_EML
				FROM
					BRWS B
					INNER JOIN DEMOS D
						ON B.BF_SSN = D.DF_PRS_ID

				UNION

				/*comakers*/
				SELECT
					D.DF_SPE_ACC_ID,
					D.DM_PRS_1,
					D.DM_PRS_LST,
					D.DX_ADR_EML
				FROM
					BRWS B
					INNER JOIN DEMOS D
						ON B.LF_EDS = D.DF_PRS_ID
			)
	;
	DISCONNECT FROM DB2;
QUIT;

ENDRSUBMIT;

DATA S67; 
	SET LEGEND.S67; 
RUN;

/*write to comma delimited file for the Email Batch Script - FED script*/
DATA _NULL_;
	SET	WORK.S67;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;

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
