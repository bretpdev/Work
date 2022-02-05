/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS20.NWS20RZ";
FILENAME REPORT2 "&RPTLIB/UNWS20.NWS20R2";
FILENAME REPORT3 "&RPTLIB/UNWS20.NWS20R3";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND  SLIBREF=WORK;
RSUBMIT;

/*%LET DB = DNFPRQUT;  *This is test;*/
/*%LET DB = DNFPRUUT;  *This is VUK3 test;*/
%LET DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;
LIBNAME SESSION DB2 DATABASE=&DB SCHEMA=SESSION CONNECTION=GLOBAL;

PROC SQL;
CONNECT TO DB2 (DATABASE=&DB);
CREATE TABLE BorrowerDataSet AS
	SELECT	
		*
	FROM	
		CONNECTION TO DB2 
		(
			SELECT DISTINCT
				LN10.BF_SSN,
				LN10.LA_CUR_PRI,
				LN10.LF_LON_CUR_OWN,
				LN16.LD_DLQ_ITL_MIN,
				LN16.LN_DLQ_MAX + 1 AS LN_DLQ_MAX,
				TRIM(PD10.DM_PRS_1) || ', ' || PD10.DM_PRS_LST AS BOR_NAME,
				PD30.DX_STR_ADR_1,
				PD30.DX_STR_ADR_2,
				PD30.DX_STR_ADR_3,
				PD30.DM_CT,
				PD30.DC_DOM_ST,
				SUBSTR(PD30.DF_ZIP_CDE,1,5) AS DF_ZIP_CDE,
				'' AS REL,
				'' AS SSN
			FROM 
				PKUB.PD10_PRS_NME PD10
				INNER JOIN  /*GETS THE TOTAL BALANCE*/ 
				(
					SELECT 
						LN10.BF_SSN,
						LN10.LF_LON_CUR_OWN,
						SUM(COALESCE(LN10.LA_CUR_PRI,0) + COALESCE(DW01.WA_TOT_BRI_OTS,0)) * 100 AS LA_CUR_PRI
					FROM 
						PKUB.LN10_LON LN10
						LEFT JOIN PKUB.DW01_DW_CLC_CLU DW01
							ON LN10.BF_SSN = DW01.BF_SSN
							AND LN10.LN_SEQ = DW01.LN_SEQ
					WHERE 
						LN10.LC_STA_LON10 = 'R'
						AND LN10.LA_CUR_PRI > 0.00
					GROUP BY
						LN10.BF_SSN,
						LN10.LF_LON_CUR_OWN
				) LN10
					ON PD10.DF_PRS_ID = LN10.BF_SSN
				INNER JOIN 
				(
					SELECT 
						LN16.BF_SSN,
						MIN(LN16.LD_DLQ_OCC) AS LD_DLQ_ITL_MIN,
						LN16.LN_DLQ_MAX
					FROM
						PKUB.LN16_LON_DLQ_HST LN16
						INNER JOIN PKUB.LN10_LON LN10
							ON LN16.BF_SSN = LN10.BF_SSN
							AND LN16.LN_SEQ = LN10.LN_SEQ
					WHERE 
						LN16.LC_STA_LON16 = '1'
						AND LN10.LC_STA_LON10 = 'R'
						AND LN10.LA_CUR_PRI > 0.00
						AND DAYS(LN16.LD_DLQ_MAX) = DAYS(CURRENT_DATE) - 1
					GROUP BY 
						LN16.BF_SSN,
						LN16.LN_DLQ_MAX
				) LN16
					ON LN10.BF_SSN = LN16.BF_SSN
				LEFT JOIN PKUB.PD30_PRS_ADR PD30
					ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
					AND PD30.DC_ADR = 'L'
				LEFT JOIN 
				(
					SELECT DISTINCT 
						BF_SSN
					FROM
						PKUB.DW01_DW_CLC_CLU
					WHERE 
						WC_DW_LON_STA NOT IN ('21','19','16','17','04','05','18') /*If a borrower has a loan that isnt in this list, they should be called*/
				) DW01
					ON PD10.DF_PRS_ID = DW01.BF_SSN
				LEFT JOIN PKUB.WQ20_TSK_QUE WQ20 /*exclude borrowers with an outstanding processing queue task*/
					ON WQ20.BF_SSN = PD10.DF_PRS_ID
					AND WQ20.WF_QUE IN ('2A', '2P', 'GP', '9R', 'S4', 'PR', 'VR', 'VB', 'MF', 'DU', 'AT', 'AW', '87', '23', '15')
					AND WQ20.WC_STA_WQUE20 IN ('A', 'H', 'P', 'U', 'W')
				LEFT JOIN 
				(/*exclude future arc date*/
					SELECT DISTINCT
						BF_SSN
						,LD_ATY_REQ_RCV
						,PARSED_DAYS
						,COALESCE(MIN(PARSED_DAYS,14),14) AS COMMENT_DAYS /*takes lesser of 14 or parsed days*/
						,LD_ATY_REQ_RCV + COALESCE(MIN(PARSED_DAYS,14),14) DAYS AS CALC_DATE
						,CASE
							WHEN DAYS(LD_ATY_REQ_RCV + COALESCE(MIN(PARSED_DAYS,14),14) DAYS) > DAYS(CURRENT DATE)
							THEN 1
							ELSE 0
						END AS FUTURE_DATE
					FROM
					(/*parse out numeric days from any extraneous text*/
						SELECT 
							AY10.BF_SSN
							,AY10.LD_ATY_REQ_RCV
							,AY20.LX_ATY
							,CAST(substr(TRANSLATE(AY20.LX_ATY,'',TRANSLATE(AY20.LX_ATY,'','1234567890',''),''),1,3) AS BIGINT) as PARSED_DAYS
							/*inner translate sets all numbers to blanks to identify extraneous text*/
							/*outer translate strips out extraneous text found by inner translate*/
						FROM
							PKUB.AY10_BR_LON_ATY AY10
							INNER JOIN PKUB.AY15_ATY_CMT AY15
								ON AY10.BF_SSN = AY15.BF_SSN
								AND AY10.LN_ATY_SEQ = AY15.LN_ATY_SEQ
							INNER JOIN PKUB.AY20_ATY_TXT AY20
								ON AY10.BF_SSN = AY20.BF_SSN
								AND AY10.LN_ATY_SEQ = AY20.LN_ATY_SEQ
								AND AY15.LN_ATY_CMT_SEQ = AY20.LN_ATY_CMT_SEQ 
						WHERE
							AY10.LC_STA_ACTY10 = 'A'
							AND AY15.LC_STA_AY15 = 'A'
							AND AY10.PF_REQ_ACT = 'BRPTP' /*borrower promise to repay*/
							AND TRANSLATE(AY20.LX_ATY,'',TRANSLATE(AY20.LX_ATY,'','1234567890',''),'') != ''
							AND AY10.LD_ATY_REQ_RCV > (CURRENT DATE - 21 DAYS)
					)
				) ARC_BRPTP 
					ON LN10.BF_SSN = ARC_BRPTP.BF_SSN
					AND ARC_BRPTP.FUTURE_DATE = 1 /*exclude future arc date*/
			WHERE 
				DW01.BF_SSN IS NOT NULL
				AND WQ20.BF_SSN IS NULL
				AND ARC_BRPTP.BF_SSN IS NULL /*exclude future arc date*/

			UNION ALL
/*Coborrowers*/
			SELECT DISTINCT
				LN20.LF_EDS AS BF_SSN,
				LN10.LA_CUR_PRI,
				LN10.LF_LON_CUR_OWN,
				LN16.LD_DLQ_ITL_MIN,
				LN16.LN_DLQ_MAX + 1 AS LN_DLQ_MAX,
				TRIM(PD10.DM_PRS_1) || ', ' || PD10.DM_PRS_LST AS BOR_NAME,
				PD30.DX_STR_ADR_1,
				PD30.DX_STR_ADR_2,
				PD30.DX_STR_ADR_3,
				PD30.DM_CT,
				PD30.DC_DOM_ST,
				SUBSTR(PD30.DF_ZIP_CDE,1,5) AS DF_ZIP_CDE,
				CASE WHEN LN20.LC_EDS_TYP = 'M' THEN 'COBRWR'
				     ELSE 'ENDORSER'
				END AS REL,
				LN20.BF_SSN AS SSN
			FROM 
				PKUB.PD10_PRS_NME PD10
				INNER JOIN PKUB.LN20_EDS LN20
					ON LN20.LF_EDS = PD10.DF_PRS_ID
					AND LN20.LC_STA_LON20 = 'A'
				INNER JOIN  /*GETS THE TOTAL BALANCE*/ 
				(
					SELECT 
						LN10.BF_SSN,
						LN10.LF_LON_CUR_OWN,
						SUM(COALESCE(LN10.LA_CUR_PRI,0) + COALESCE(DW01.WA_TOT_BRI_OTS,0)) * 100 AS LA_CUR_PRI
					FROM 
						PKUB.LN20_EDS LN20
						INNER JOIN PKUB.LN10_LON LN10
							ON LN20.BF_SSN = LN10.BF_SSN
							AND LN20.LN_SEQ = LN10.LN_SEQ
							AND LN20.LC_STA_LON20 = 'A'
						LEFT JOIN PKUB.DW01_DW_CLC_CLU DW01
							ON LN10.BF_SSN = DW01.BF_SSN
							AND LN10.LN_SEQ = DW01.LN_SEQ
					WHERE 
						LN10.LC_STA_LON10 = 'R'
						AND LN10.LA_CUR_PRI > 0.00
					GROUP BY
						LN10.BF_SSN,
						LN10.LF_LON_CUR_OWN
				) LN10
					ON LN20.BF_SSN = LN10.BF_SSN
				INNER JOIN 
				(
					SELECT 
						LN16.BF_SSN,
						MIN(LN16.LD_DLQ_OCC) AS LD_DLQ_ITL_MIN,
						LN16.LN_DLQ_MAX
					FROM
						PKUB.LN16_LON_DLQ_HST LN16
						INNER JOIN PKUB.LN10_LON LN10
							ON LN16.BF_SSN = LN10.BF_SSN
							AND LN16.LN_SEQ = LN10.LN_SEQ
						INNER JOIN PKUB.LN20_EDS LN20
							ON LN20.BF_SSN = LN10.BF_SSN
							AND LN20.LN_SEQ = LN10.LN_SEQ
							AND LN20.LC_STA_LON20 = 'A'
					WHERE 
						LN16.LC_STA_LON16 = '1'
						AND LN10.LC_STA_LON10 = 'R'
						AND LN10.LA_CUR_PRI > 0.00
						AND DAYS(LN16.LD_DLQ_MAX) = DAYS(CURRENT_DATE) - 1
					GROUP BY 
						LN16.BF_SSN,
						LN16.LN_DLQ_MAX
				) LN16
					ON LN20.BF_SSN = LN16.BF_SSN
				LEFT JOIN PKUB.PD30_PRS_ADR PD30
					ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
					AND PD30.DC_ADR = 'L'
				LEFT JOIN 
				(
					SELECT DISTINCT 
						BF_SSN
					FROM
						PKUB.DW01_DW_CLC_CLU
					WHERE 
						WC_DW_LON_STA NOT IN ('21','19','16','17','04','05','18') /*If a borrower has a loan that isnt in this list, they should be called*/
				) DW01
					ON LN20.BF_SSN = DW01.BF_SSN
				LEFT JOIN PKUB.WQ20_TSK_QUE WQ20 /*exclude borrowers with an outstanding processing queue task*/
					ON WQ20.BF_SSN = LN20.BF_SSN
					AND WQ20.WF_QUE IN ('2A', '2P', 'GP', '9R', 'S4', 'PR', 'VR', 'VB', 'MF', 'DU', 'AT', 'AW', '87', '23', '15')
					AND WQ20.WC_STA_WQUE20 IN ('A', 'H', 'P', 'U', 'W')
				LEFT JOIN 
				(/*exclude future arc date*/
					SELECT DISTINCT
						BF_SSN
						,LD_ATY_REQ_RCV
						,PARSED_DAYS
						,COALESCE(MIN(PARSED_DAYS,14),14) AS COMMENT_DAYS /*takes lesser of 14 or parsed days*/
						,LD_ATY_REQ_RCV + COALESCE(MIN(PARSED_DAYS,14),14) DAYS AS CALC_DATE
						,CASE
							WHEN DAYS(LD_ATY_REQ_RCV + COALESCE(MIN(PARSED_DAYS,14),14) DAYS) > DAYS(CURRENT DATE)
							THEN 1
							ELSE 0
						END AS FUTURE_DATE
					FROM
					(/*parse out numeric days from any extraneous text*/
						SELECT 
							AY10.BF_SSN
							,AY10.LD_ATY_REQ_RCV
							,AY20.LX_ATY
							,CAST(substr(TRANSLATE(AY20.LX_ATY,'',TRANSLATE(AY20.LX_ATY,'','1234567890',''),''),1,3) AS BIGINT) as PARSED_DAYS
							/*inner translate sets all numbers to blanks to identify extraneous text*/
							/*outer translate strips out extraneous text found by inner translate*/
						FROM
							PKUB.AY10_BR_LON_ATY AY10
							INNER JOIN PKUB.AY15_ATY_CMT AY15
								ON AY10.BF_SSN = AY15.BF_SSN
								AND AY10.LN_ATY_SEQ = AY15.LN_ATY_SEQ
							INNER JOIN PKUB.AY20_ATY_TXT AY20
								ON AY10.BF_SSN = AY20.BF_SSN
								AND AY10.LN_ATY_SEQ = AY20.LN_ATY_SEQ
								AND AY15.LN_ATY_CMT_SEQ = AY20.LN_ATY_CMT_SEQ 
						WHERE
							AY10.LC_STA_ACTY10 = 'A'
							AND AY15.LC_STA_AY15 = 'A'
							AND AY10.PF_REQ_ACT = 'BRPTP' /*borrower promise to repay*/
							AND TRANSLATE(AY20.LX_ATY,'',TRANSLATE(AY20.LX_ATY,'','1234567890',''),'') != ''
							AND AY10.LD_ATY_REQ_RCV > (CURRENT DATE - 21 DAYS)
					)
				) ARC_BRPTP 
					ON LN20.BF_SSN = ARC_BRPTP.BF_SSN
					AND ARC_BRPTP.FUTURE_DATE = 1 /*exclude future arc date*/
			WHERE 
				DW01.BF_SSN IS NOT NULL
				AND WQ20.BF_SSN IS NULL
				AND ARC_BRPTP.BF_SSN IS NULL /*exclude future arc date*/
		);
	DISCONNECT FROM DB2;
QUIT;

PROC SQL;
CONNECT TO DB2 (DATABASE=&DB CONNECTION=GLOBAL);

/*Create a temp table with the data from BorrowerDataSet*/
EXECUTE (DECLARE GLOBAL TEMPORARY TABLE SESSION.BDS_BASE
			(
				BF_SSN CHAR(9),
				LA_CUR_PRI DECIMAL(14,0),
				LF_LON_CUR_OWN CHAR(8),
				LD_DLQ_ITL_MIN DATE,
				LN_DLQ_MAX INT,
				BOR_NAME VARCHAR(40), /*will need trimmed*/
				DX_STR_ADR_1 CHAR(30),
				DX_STR_ADR_2 CHAR(30),
				DX_STR_ADR_3 CHAR(30),
				DM_CT CHAR(20),
				DC_DOM_ST CHAR(2),
				DF_ZIP_CDE CHAR(5),
				REL VARCHAR(10),
				SSN CHAR(9)
			)
			ON COMMIT PRESERVE ROWS
		) BY DB2;

INSERT INTO SESSION.BDS_BASE
/*	select higher delinquency in cases where borrower has split delinquency*/
	SELECT 
		BDS.BF_SSN,
		BDS.LA_CUR_PRI,
		BDS.LF_LON_CUR_OWN,
		BDS.LD_DLQ_ITL_MIN,
		BDS.LN_DLQ_MAX,
		TRIM(BDS.BOR_NAME), /*will need trimmed*/
		BDS.DX_STR_ADR_1,
		BDS.DX_STR_ADR_2,
		BDS.DX_STR_ADR_3,
		BDS.DM_CT,
		BDS.DC_DOM_ST,
		PUT(BDS.DF_ZIP_CDE, $5.),
		TRIM(BDS.REL),
		BDS.SSN
	FROM
		BorrowerDataSet BDS
		INNER JOIN
		(
			SELECT
				BF_SSN,
				MAX(LN_DLQ_MAX) AS LN_DLQ_MAX
			FROM
				BorrowerDataSet
			GROUP BY
				BF_SSN
		) DLQ
			ON BDS.BF_SSN = DLQ.BF_SSN
			AND BDS.LN_DLQ_MAX = DLQ.LN_DLQ_MAX;
	;


/*Create a temp table with the data from PhoneTypeRank and PhoneConsentRank*/
EXECUTE (DECLARE GLOBAL TEMPORARY TABLE SESSION.PhoneTypeRank
			(
				DC_PHN CHAR(1),
				TypeRank INT
			)
			ON COMMIT PRESERVE ROWS
		) BY DB2;

INSERT INTO SESSION.PhoneTypeRank (DC_PHN, TypeRank)
SELECT DISTINCT 'H', 1 FROM PKUB.PD10_PRS_NME UNION ALL /*table reference was required to get the insert to work*/
SELECT DISTINCT 'A', 2 FROM PKUB.PD10_PRS_NME UNION ALL
SELECT DISTINCT 'W', 3 FROM PKUB.PD10_PRS_NME UNION ALL
SELECT DISTINCT 'M', 4 FROM PKUB.PD10_PRS_NME;

EXECUTE (DECLARE GLOBAL TEMPORARY TABLE SESSION.PhoneConsentRank
			(
				Consent CHAR(1),
				ConsentRank INT
			)
			ON COMMIT PRESERVE ROWS
		) BY DB2;

INSERT INTO SESSION.PhoneConsentRank (Consent, ConsentRank)
SELECT DISTINCT 'N', 1 FROM PKUB.PD10_PRS_NME UNION ALL /*table reference was required to get the insert to work*/
SELECT DISTINCT 'P', 2 FROM PKUB.PD10_PRS_NME UNION ALL
SELECT DISTINCT 'X', 3 FROM PKUB.PD10_PRS_NME UNION ALL
SELECT DISTINCT 'L', 4 FROM PKUB.PD10_PRS_NME UNION ALL
SELECT DISTINCT 'U', 5 FROM PKUB.PD10_PRS_NME ;

CREATE TABLE PHN AS
	SELECT	
		*
	FROM	
		CONNECTION TO DB2 
		(
			SELECT DISTINCT
				BDS.*,
				PD40.DN_DOM_PHN_ARA || PD40.DN_DOM_PHN_XCH || PD40.DN_DOM_PHN_LCL AS PHN,
				PD40.DC_PHN,
				CASE
					WHEN PD40.DC_ALW_ADL_PHN IN ('L','X','P') 
						THEN 'Y'
						ELSE 'N'
				END AS DC_ALW_ADL_PHN,
				PD40.DC_ALW_ADL_PHN AS Consent,
				COALESCE(BILL_DATA.AMTDU,0) * 100 AS AMTDU,
				/*Rank phones by category and type*/
				DENSE_RANK() OVER (PARTITION BY BDS.BF_SSN ORDER BY PCR.ConsentRank, PTR.TypeRank) AS OverallRank
			FROM 
				SESSION.BDS_BASE BDS
				INNER JOIN PKUB.PD40_PRS_PHN PD40
					ON BDS.BF_SSN = PD40.DF_PRS_ID
					AND PD40.DC_NO_HME_PHN != 'J'
					AND PD40.DI_PHN_VLD = 'Y'
				LEFT JOIN /*GETS THE AMOUNT DUE*/
				(
					SELECT 
						LN80.BF_SSN,
						SUM(COALESCE(LN80.LA_BIL_PAS_DU,0) + COALESCE(LN80.LA_BIL_DU_PRT,0) + COALESCE(LN80.LA_LTE_FEE_OTS_PRT,0)) AS AMTDU
					FROM 
						PKUB.LN80_LON_BIL_CRF LN80
						INNER JOIN 
						(
							SELECT
								BIL_SEQ.BF_SSN,
								BIL_SEQ.LD_BIL_CRT,
								MAX(LN_SEQ_BIL_WI_DTE) AS LN_SEQ_BIL_WI_DTE
							FROM
								PKUB.LN80_LON_BIL_CRF BIL_SEQ
								INNER JOIN 
								(
									SELECT 
										BF_SSN,
										MAX(LD_BIL_CRT) AS LD_BIL_CRT
									FROM 
										PKUB.LN80_LON_BIL_CRF 
									WHERE 
										LC_STA_LON80 = 'A'
										AND LC_BIL_TYP_LON = 'P'
									GROUP BY
										BF_SSN
								) BIL_DT
									ON BIL_DT.BF_SSN = BIL_SEQ.BF_SSN
									AND BIL_DT.LD_BIL_CRT = BIL_SEQ.LD_BIL_CRT
							WHERE 
								BIL_SEQ.LC_STA_LON80 = 'A'
								AND BIL_SEQ.LC_BIL_TYP_LON = 'P'
							GROUP BY 
								BIL_SEQ.BF_SSN,
								BIL_SEQ.LD_BIL_CRT
						) THE_BILL
							ON LN80.BF_SSN = THE_BILL.BF_SSN
							AND LN80.LD_BIL_CRT = THE_BILL.LD_BIL_CRT
							AND LN80.LN_SEQ_BIL_WI_DTE = THE_BILL.LN_SEQ_BIL_WI_DTE
						LEFT JOIN PKUB.PD10_PRS_NME PD10
							ON LN80.BF_SSN = PD10.DF_PRS_ID
						LEFT JOIN PKUB.LN10_LON LN10
							ON LN80.BF_SSN = LN10.BF_SSN
							AND LN80.LN_SEQ = LN10.LN_SEQ
					WHERE 
						LN80.LC_STA_LON80 = 'A'
						AND LN10.LA_CUR_PRI > 0
						AND LN10.LC_STA_LON10 = 'R'
					GROUP BY
						LN80.BF_SSN
				) BILL_DATA
					ON BDS.BF_SSN = BILL_DATA.BF_SSN
				INNER JOIN SESSION.PhoneTypeRank PTR
					ON PTR.DC_PHN = PD40.DC_PHN
				INNER JOIN SESSION.PhoneConsentRank PCR
					ON PCR.Consent = PD40.DC_ALW_ADL_PHN
			ORDER BY
				BDS.BF_SSN	
		);
	DISCONNECT FROM DB2;
QUIT;

PROC SQL;
CREATE TABLE PHN_FINAL AS
	SELECT
		PHN1.BF_SSN,
		PHN1.LA_CUR_PRI,
		PHN1.LF_LON_CUR_OWN,
		PHN1.LD_DLQ_ITL_MIN,
		PHN1.LN_DLQ_MAX,
		TRIM(PHN1.BOR_NAME) AS BOR_NAME, /*will need trimmed*/
		PHN1.DX_STR_ADR_1,
		PHN1.DX_STR_ADR_2,
		PHN1.DX_STR_ADR_3,
		PHN1.DM_CT,
		PHN1.DC_DOM_ST,
		PUT(PHN1.DF_ZIP_CDE, $5.) AS DF_ZIP_CDE,
		PHN1.REL,
		PHN1.SSN,
		PHN1.AMTDU,
		PHN1.DC_ALW_ADL_PHN AS PHN_H_CON,
		PHN1.PHN AS PHN_H,
		PHN2.DC_ALW_ADL_PHN AS PHN_A_CON,
		PHN2.PHN AS PHN_A,
		PHN3.DC_ALW_ADL_PHN AS PHN_W_CON,
		PHN3.PHN AS PHN_W
	FROM
		PHN PHN1
		LEFT JOIN PHN PHN2
			ON PHN1.BF_SSN = PHN2.BF_SSN
			AND PHN2.OverallRank = 2
		LEFT JOIN PHN PHN3
			ON PHN1.BF_SSN = PHN3.BF_SSN
			AND PHN3.OverallRank = 3
	WHERE
		PHN1.OverallRank = 1
;

DATA PHN_FINAL;
	SET PHN_FINAL;
	IF LN_DLQ_MAX > 2;
RUN;

ENDRSUBMIT;

DATA PHN_FINAL; 
	SET LEGEND.PHN_FINAL; 
RUN;

DATA _NULL_;
	SET PHN_FINAL;
	FILE REPORT2 DROPOVER LRECL=32767;
	DO;
		PUT @1 BF_SSN   			
			@15 BOR_NAME
			@70 LN_DLQ_MAX
			@82 REL
			@94 SSN
			@159 PHN_H_CON
			@160 PHN_H
			@175 PHN_A_CON
			@176 PHN_A
			@191 PHN_W_CON
			@192 PHN_W
			@260 DX_STR_ADR_1
			@290 DX_STR_ADR_2
			@320 DX_STR_ADR_3
			@350 DM_CT
			@370 DC_DOM_ST
			@372 DF_ZIP_CDE
			@414 LF_LON_CUR_OWN
			@422 AMTDU
			@429 LA_CUR_PRI;	
	END;
RUN;

DATA _NULL_;
	SET PHN_FINAL;

	FILE
		REPORT3
		DELIMITER = ','
		DSD
		DROPOVER
		LRECL = 32767
	;

	FORMAT
		AMTDU DOLLAR15.2
		LA_CUR_PRI DOLLAR15.2
	;

	IF _N_ = 1 THEN 
		DO;
			PUT 'SSN,Name,Days Delinquent,Relationship,Borrower SSN,Consent for Home Phone,Home Phone,Consent for Alternate Phone,Alternate Phone,' @;
			PUT 'Consent for Work Phone,Work Phone,Street 1,Street 2,Street 3,City,State,Zip Code,Owner Code,Amount Due,Total balance of all loans';
		END;

	AMTDU = AMTDU/100;
	LA_CUR_PRI = LA_CUR_PRI/100;

	PUT BF_SSN @;			
	PUT BOR_NAME @;
	PUT LN_DLQ_MAX @;
	PUT REL @;
	PUT SSN @;
	PUT PHN_H_CON @;
	PUT PHN_H @;
	PUT PHN_A_CON @;
	PUT PHN_A @;
	PUT PHN_W_CON @;
	PUT PHN_W @;
	PUT DX_STR_ADR_1 @;
	PUT DX_STR_ADR_2 @;
	PUT DX_STR_ADR_3 @;
	PUT DM_CT @;
	PUT DC_DOM_ST @;
	PUT DF_ZIP_CDE @;
	PUT LF_LON_CUR_OWN @;
	PUT AMTDU @;
	PUT LA_CUR_PRI;	
RUN;
