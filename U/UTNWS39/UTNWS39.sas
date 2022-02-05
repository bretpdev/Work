/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

FILENAME REPORTZ "&RPTLIB/UNWS39.NWS39RZ";
FILENAME REPORT2 "&RPTLIB/UNWS39.NWS39R2";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
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
/*	recipients of K0ADD and K0PHN ARCs*/
	CREATE TABLE K0ARCS AS
		SELECT DISTINCT
			AY10.BF_SSN,
			AY10.LF_ATY_RCP AS RECIP_ID,
			AY10.PF_REQ_ACT,
			AY20.LX_ATY,
			AY10.LD_ATY_RSP AS K0_LD_ATY_RSP,
			CASE
				WHEN INVAC.BF_SSN IS NULL THEN 'N'
				ELSE 'Y'
			END AS HASINVAC,
			INVAC.LD_ATY_RSP AS INVAC_LD_ATY_RSP
		FROM
			PKUB.AY10_BR_LON_ATY AY10
			JOIN PKUB.AY15_ATY_CMT AY15
				ON AY10.BF_SSN = AY15.BF_SSN
				AND AY10.LN_ATY_SEQ = AY15.LN_ATY_SEQ
				AND AY10.PF_REQ_ACT IN ('K0ADD','K0PHN')
			JOIN PKUB.AY20_ATY_TXT AY20
				ON AY15.BF_SSN = AY20.BF_SSN
				AND AY15.LN_ATY_SEQ = AY20.LN_ATY_SEQ
				AND AY15.LN_ATY_CMT_SEQ = AY20.LN_ATY_CMT_SEQ
			LEFT JOIN PKUB.AY10_BR_LON_ATY INVAC
				ON AY10.LF_ATY_RCP = INVAC.LF_ATY_RCP
				AND INVAC.PF_REQ_ACT = 'INVAC'
/*				AND INVAC.LD_ATY_RSP IS NULL*/
/*		WHERE*/
/*			INVAC.BF_SSN IS NULL*/
	;

/*	borrowers with open loans*/
	CREATE TABLE BRWS AS
		SELECT DISTINCT
			LN10.BF_SSN,
			PD10.DF_SPE_ACC_ID,
			LN10.BF_SSN AS RECIP_ID,
			'B' AS REGARDS_CODE
		FROM
			PKUB.LN10_LON LN10
			JOIN PKUB.PD10_PRS_NME PD10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
		WHERE
			LC_STA_LON10 = 'R'
			AND LA_CUR_PRI > 0
	;

/*	references for borrowers with open loans*/
	CREATE TABLE REFS AS
		SELECT DISTINCT
			BRWS.BF_SSN,
			BRWS.DF_SPE_ACC_ID,
			RF10.BF_RFR AS RECIP_ID,
			'R' AS REGARDS_CODE
		FROM
			BRWS
			JOIN PKUB.RF10_RFR RF10
				ON BRWS.BF_SSN = RF10.BF_SSN
	;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

/*borrowers with open loans and their references*/
DATA BRWS_REFS;
	SET BRWS REFS;
RUN;

/*extract address or phone data from activity comment and build text of comment to add*/
DATA K0ARCS;
	SET K0ARCS;

	FORMAT K0_ADR_1 $35.;
	FORMAT K0_ADR_2 $35.;
	FORMAT K0_CITY $35.;
	FORMAT K0_ST $2.;
	FORMAT K0_ZIP $9.;
	FORMAT K0_CNY $35.;
	FORMAT K0_PHN $18.;
	FORMAT TEXT $200.;
	FORMAT REF_ID $9.;

	IF RECIP_ID NE BF_SSN THEN REF_ID = RECIP_ID;
	ELSE REF_ID = '';

	IF PF_REQ_ACT = 'K0ADD' THEN 
		DO;
			K0_ADR_1 = STRIP(COMPRESS(SCAN(LX_ATY,1,',','M'),' ','DK'));
			K0_ADR_2 = STRIP(COMPRESS(SCAN(LX_ATY,2,',','M'),' ','DK'));
			K0_CITY = SCAN(LX_ATY,3,',','M');
			K0_ST = SCAN(LX_ATY,4,',','M');
			K0_ZIP = TRANSTRN(SCAN(LX_ATY,5,',','M'),'-',TRIMN(''));
			K0_CNY = SCAN(LX_ATY,6,',','M');
			TEXT = CATX(';',REF_ID,SCAN(LX_ATY,1,',','M'),SCAN(LX_ATY,2,',','M'),SCAN(LX_ATY,3,',','M'),SCAN(LX_ATY,4,',','M'),SCAN(LX_ATY,5,',','M'),SCAN(LX_ATY,6,',','M'),'K0ADD',PUT(K0_LD_ATY_RSP, MMDDYY10.));
		END;

	IF PF_REQ_ACT = 'K0PHN' THEN
		DO;
			K0_PHN = SCAN(LX_ATY,1,',','M');
			TEXT = CATX(';',REF_ID,SCAN(LX_ATY,1,',','M'),'K0PHN',PUT(K0_LD_ATY_RSP, MMDDYY10.));
		END;

RUN;

PROC SQL;
/*	borrowers with open loans and their references who have K0ADD or K0PHN ARCs*/
	CREATE TABLE K0_BRWS_REFS AS
		SELECT DISTINCT
			BRS.BF_SSN,
			BRS.DF_SPE_ACC_ID,
			BRS.RECIP_ID,
			BRS.REGARDS_CODE,
			K.HASINVAC,
			K.INVAC_LD_ATY_RSP,
			K.LX_ATY, /*only needed for testing*/
			K.PF_REQ_ACT,
			K.K0_ADR_1,
			K.K0_ADR_2,
			K.K0_CITY,
			K.K0_ST,
			K.K0_ZIP,
			K.K0_CNY,
			K.K0_PHN,
			K.TEXT
		FROM
			BRWS_REFS BRS
			JOIN K0ARCS K
				ON BRS.RECIP_ID = K.RECIP_ID
	;

/*	add address information for borrowers with open loans and their references who have K0ADD or K0PHN ARCs and whose addresses were verified yesterday*/
	CREATE TABLE DEMOS_VALD AS
		SELECT DISTINCT
			BRS.BF_SSN,
			BRS.DF_SPE_ACC_ID,
			BRS.RECIP_ID,
			BRS.REGARDS_CODE,
			BRS.TEXT,
			BRS.PF_REQ_ACT, /*only needed for testing*/
			BRS.LX_ATY,  /*only needed for testing*/
			BRS.K0_ADR_1,  /*only needed for testing*/
			PD30.DX_STR_ADR_1, /*only needed for testing*/
			STRIP(COMPRESS(PD30.DX_STR_ADR_1,' ','DK')) AS NDX_STR_ADR_1, 
			BRS.K0_ADR_2,  /*only needed for testing*/
			PD30.DX_STR_ADR_2, /*only needed for testing*/
			STRIP(COMPRESS(PD30.DX_STR_ADR_2,' ','DK')) AS NDX_STR_ADR_2, 
			BRS.K0_CITY, /*only needed for testing*/
			PD30.DM_CT,
			BRS.K0_ST, /*only needed for testing*/
			PD30.DC_DOM_ST,
			K0_ZIP, /*only needed for testing*/
			PD30.DF_ZIP_CDE,
			BRS.K0_CNY, /*only needed for testing*/
			PD30.DM_FGN_CNY, /*only needed for testing*/ 
			PD30.DC_ADR, /*only needed for testing*/
			PD30.DD_VER_ADR, /*only needed for testing*/
			PD30.DI_VLD_ADR, /*only needed for testing*/
			BRS.HASINVAC, /*only needed for testing*/
			BRS.INVAC_LD_ATY_RSP /*only needed for testing*/
		FROM
			K0_BRWS_REFS BRS
			JOIN PKUB.PD30_PRS_ADR PD30
				ON BRS.RECIP_ID = PD30.DF_PRS_ID
				AND PD30.DC_ADR IN ('L', 'D', 'B')
				AND PD30.DD_VER_ADR = TODAY()-1
				AND PD30.DI_VLD_ADR = 'Y'
				AND 
					(
						BRS.HASINVAC = 'N'
						OR 
						(BRS.INVAC_LD_ATY_RSP IS NOT NULL AND BRS.INVAC_LD_ATY_RSP < PD30.DD_VER_ADR)
					)
		WHERE
			BRS.PF_REQ_ACT = 'K0ADD'
			AND BRS.K0_ADR_1 = STRIP(COMPRESS(PD30.DX_STR_ADR_1,' ','DK'))
			AND BRS.K0_ADR_2 = STRIP(COMPRESS(PD30.DX_STR_ADR_2,' ','DK'))
			AND BRS.K0_CITY = PD30.DM_CT
			AND BRS.K0_ST = PD30.DC_DOM_ST
			AND BRS.K0_CNY = PD30.DM_FGN_CNY
			AND
				(
					(PD30.DM_FGN_CNY = '' AND SUBSTR(K0_ZIP,1,5) = SUBSTR(PD30.DF_ZIP_CDE,1,5))
					OR
					SUBSTR(K0_ZIP,1,5) = SUBSTR(PD30.DF_ZIP_CDE,1,5)
				)

	;

/*	add phone information for borrowers with open loans and their references who have K0ADD or K0PHN ARCs and whose phone numbers were verified yesterday	*/
	CREATE TABLE PHONE_VALD AS
		SELECT DISTINCT
			BRS.BF_SSN,
			BRS.DF_SPE_ACC_ID,
			BRS.RECIP_ID,
			BRS.REGARDS_CODE,
			BRS.TEXT,
			BRS.PF_REQ_ACT, /*only needed for testing*/
			BRS.LX_ATY, /*only needed for testing*/
			CASE
				WHEN CATX('',PD40.DN_DOM_PHN_ARA,PD40.DN_DOM_PHN_XCH,PD40.DN_DOM_PHN_LCL,PD40.DN_PHN_XTN) <> '' THEN CATX('',PD40.DN_DOM_PHN_ARA,PD40.DN_DOM_PHN_XCH,PD40.DN_DOM_PHN_LCL,PD40.DN_PHN_XTN)
				ELSE CATX('',PD40.DN_FGN_PHN_CNY,PD40.DN_FGN_PHN_CT,PD40.DN_FGN_PHN_LCL,PD40.DN_PHN_XTN)
			END AS PHN

		FROM
			K0_BRWS_REFS BRS
			JOIN PKUB.PD40_PRS_PHN PD40
				ON BRS.RECIP_ID = PD40.DF_PRS_ID
				AND PD40.DC_PHN IN ('H', 'A', 'W', 'M')
				AND PD40.DD_PHN_VER = TODAY()-1
				AND PD40.DI_PHN_VLD = 'Y'
				AND 
					(
						BRS.HASINVAC = 'N'
						OR
						(BRS.INVAC_LD_ATY_RSP IS NOT NULL AND BRS.INVAC_LD_ATY_RSP < PD40.DD_PHN_VER)
					)
		WHERE
			BRS.PF_REQ_ACT = 'K0PHN'
			AND
				(
					CATX('',PD40.DN_DOM_PHN_ARA,PD40.DN_DOM_PHN_XCH,PD40.DN_DOM_PHN_LCL,PD40.DN_PHN_XTN) <> '' AND BRS.K0_PHN = CATX('',PD40.DN_DOM_PHN_ARA,PD40.DN_DOM_PHN_XCH,PD40.DN_DOM_PHN_LCL,PD40.DN_PHN_XTN)
					OR 
					BRS.K0_PHN = CATX('',PD40.DN_FGN_PHN_CNY,PD40.DN_FGN_PHN_CT,PD40.DN_FGN_PHN_LCL,PD40.DN_PHN_XTN)
				)
	;

/*only include borrower and references where the most recent history address is different than the current address (address changed)*/
	CREATE TABLE DEMOS_CHANGED AS
		SELECT DISTINCT
			VALD.BF_SSN,
			VALD.DF_SPE_ACC_ID,
			VALD.RECIP_ID,
			VALD.REGARDS_CODE,
			VALD.TEXT
		FROM
			DEMOS_VALD VALD
			LEFT JOIN 
				(
					SELECT
						PD31.DF_PRS_ID,
						STRIP(COMPRESS(PD31.DX_STR_ADR_1_HST,' ','DK')) AS DX_STR_ADR_1_HST,
						STRIP(COMPRESS(PD31.DX_STR_ADR_2_HST,' ','DK')) AS DX_STR_ADR_2_HST,
						PD31.DM_CT_HST,
						PD31.DC_DOM_ST_HST,
						CASE
							WHEN PD31.DM_FGN_CNY_HST = '' THEN SUBSTR(PD31.DF_ZIP_CDE_HST,1,5)
							ELSE PD31.DF_ZIP_CDE_HST
						END AS DF_ZIP_CDE_HST,
						PD31.DM_FGN_CNY_HST
					FROM
						PKUB.PD31_PRS_INA PD31
						JOIN 
							(
								SELECT
									DF_PRS_ID,
									DC_ADR_HST,
									MAX(DN_ADR_SEQ) AS DN_ADR_SEQ
								FROM
									PKUB.PD31_PRS_INA
								GROUP BY
									DF_PRS_ID,
									DC_ADR_HST
							) MAXA
							ON PD31.DF_PRS_ID = MAXA.DF_PRS_ID
							AND PD31.DC_ADR_HST = MAXA.DC_ADR_HST
							AND PD31.DN_ADR_SEQ = MAXA.DN_ADR_SEQ
				) HIST
				ON HIST.DF_PRS_ID = VALD.RECIP_ID
		WHERE
			VALD.NDX_STR_ADR_1 <> HIST.DX_STR_ADR_1_HST
			OR VALD.NDX_STR_ADR_2 <> HIST.DX_STR_ADR_2_HST
			OR VALD.DM_CT <> HIST.DM_CT_HST
			OR VALD.DC_DOM_ST <> HIST.DC_DOM_ST_HST
			OR SUBSTR(DF_ZIP_CDE,1,5) <> SUBSTR(DF_ZIP_CDE_HST,1,5)
			OR VALD.DM_FGN_CNY <> HIST.DM_FGN_CNY_HST
	;

/*only include borrower and references where the most recent history phone is different than the current phone (phone changed)*/
	CREATE TABLE PHONE_CHANGED AS
		SELECT DISTINCT
			VALD.BF_SSN,
			VALD.DF_SPE_ACC_ID,
			VALD.RECIP_ID,
			VALD.REGARDS_CODE,
			VALD.TEXT
		FROM
			PHONE_VALD VALD
			LEFT JOIN 
				(
					SELECT
						PD41.DF_PRS_ID,
						CASE
							WHEN CATX('',PD41.DN_DOM_PHN_ARA_HST,PD41.DN_DOM_PHN_XCH_HST,PD41.DN_DOM_PHN_LCL_HST,PD41.DN_PHN_XTN_HST) <> '' THEN CATX('',PD41.DN_DOM_PHN_ARA_HST,PD41.DN_DOM_PHN_XCH_HST,PD41.DN_DOM_PHN_LCL_HST,PD41.DN_PHN_XTN_HST)
							ELSE CATX('',PD41.DN_FGN_PHN_CNY_HST,PD41.DN_FGN_PHN_CT_HST,PD41.DN_FGN_PHN_LCL_HST,PD41.DN_PHN_XTN_HST)
						END AS PHN_HST
					FROM
						PKUB.PD41_PHN_HST PD41
						JOIN 
							(
								SELECT
									DF_PRS_ID,
									DC_PHN_HST,
									MAX(DN_PHN_SEQ) AS DN_PHN_SEQ
								FROM
									PKUB.PD41_PHN_HST
								GROUP BY
									DF_PRS_ID,
									DC_PHN_HST
							) MAXA
							ON PD41.DF_PRS_ID = MAXA.DF_PRS_ID
							AND PD41.DC_PHN_HST = MAXA.DC_PHN_HST
							AND PD41.DN_PHN_SEQ = MAXA.DN_PHN_SEQ
				) HIST
				ON HIST.DF_PRS_ID = VALD.RECIP_ID
		WHERE
			VALD.PHN <> HIST.PHN_HST

	;
QUIT;

DATA R2;
	SET DEMOS_CHANGED PHONE_CHANGED;
RUN;

ENDRSUBMIT;

DATA R2; SET LEGEND.R2; RUN;

DATA _NULL_;
	SET R2 ;
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
	PUT DF_SPE_ACC_ID 'INVAC,,,,' RECIP_ID REGARDS_CODE RECIP_ID 'ALL,' TEXT;
RUN;

/*DATA K0ARCS; SET LEGEND.K0ARCS; RUN;*/
/*DATA BRWS_REFS; SET LEGEND.BRWS_REFS; RUN;*/
/*DATA K0_BRWS_REFS; SET LEGEND.K0_BRWS_REFS; RUN;*/
/*DATA DEMOS_VALD; SET LEGEND.DEMOS_VALD; RUN;*/
/*DATA PHONE_VALD; SET LEGEND.PHONE_VALD; RUN;*/
/*DATA DEMOS_CHANGED; SET LEGEND.DEMOS_CHANGED; RUN;*/
/*DATA PHONE_CHANGED; SET LEGEND.PHONE_CHANGED; RUN;*/
