/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWS64.LWS64RZ";
FILENAME REPORT2 "&RPTLIB/ULWS64.LWS64R2";

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;

%LET DB = DLGSWQUT; *test;
/*%LET DB = DLGSUTWH; *live;*/
LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1;

PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);
	CREATE TABLE COMPASS_BWRS AS
	SELECT	
		*
	FROM
		CONNECTION TO DB2 
		(
			SELECT DISTINCT 
				PD42.DF_PRS_ID AS ID,
				PD42.DN_DOM_PHN_ARA || PD42.DN_DOM_PHN_XCH || PD42.DN_DOM_PHN_LCL AS PHONE
			FROM
				OLWHRM1.PD42_PRS_PHN PD42
				INNER JOIN OLWHRM1.LN10_LON LN10
					ON PD42.DF_PRS_ID = LN10.BF_SSN
				LEFT JOIN OLWHRM1.WQ20_TSK_QUE WQ20
					ON WQ20.BF_SSN = LN10.BF_SSN
					AND WQ20.WF_QUE IN ('2A', '2P', 'GP', '9R', 'SI', 'S4', 'PR', 'VR', 'VB', 'MF', '1P', 'SF', 'DU', 'AT', '87')
					AND WQ20.WC_STA_WQUE20 IN ('A', 'H', 'P', 'U', 'W')
				LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY DOC ID*/
				(
					SELECT
						BF_SSN,
						MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
					FROM
						OLWHRM1.AY10_BR_LON_ATY
					WHERE
						PF_REQ_ACT = 'DIDDA'
					GROUP BY
						BF_SSN
				)DID 
					ON DID.BF_SSN = LN10.BF_SSN
				LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY APPROVAL*/
				(
					SELECT
						BF_SSN,
						MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
					FROM
						OLWHRM1.AY10_BR_LON_ATY
					WHERE
						PF_REQ_ACT = 'PAUTO'
					GROUP BY
						BF_SSN
				)AUTO 
					ON AUTO.BF_SSN = LN10.BF_SSN
				LEFT JOIN OLWHRM1.CT30_CALL_QUE CT30
					ON CT30.DF_PRS_ID_BR = LN10.BF_SSN
					AND CT30.IF_WRK_GRP IN ('ASBKP', 'ASCON', 'ASMOC')
					AND CT30.IF_WRK_GRP IN ('A', 'W')
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
							,CAST(TRANSLATE(AY20.LX_ATY,'',TRANSLATE(AY20.LX_ATY,'','1234567890',''),'') AS BIGINT) AS PARSED_DAYS
							/*inner translate sets all numbers to blanks to identify extraneous text*/
							/*outer translate strips out extraneous text found by inner translate*/
						FROM
							OLWHRM1.AY10_BR_LON_ATY AY10
							INNER JOIN OLWHRM1.AY20_ATY_TXT AY20
								ON AY10.BF_SSN = AY20.BF_SSN
								AND AY10.LN_ATY_SEQ = AY20.LN_ATY_SEQ
						WHERE
							AY10.LC_STA_ACTY10 = 'A'
							AND AY10.PF_REQ_ACT = 'BRPTP' /*borrower promise to repay*/
							AND TRANSLATE(AY20.LX_ATY,'',TRANSLATE(AY20.LX_ATY,'','1234567890',''),'') != ''
					)
				) ARC_BRPTP 
					ON LN10.BF_SSN = ARC_BRPTP.BF_SSN
					AND ARC_BRPTP.FUTURE_DATE = 1 /*exclude future arc date*/
			WHERE 
				ARC_BRPTP.BF_SSN IS NULL /*exclude future arc date*/
				AND PD42.DI_PHN_VLD = 'Y'
				AND LN10.LA_CUR_PRI > 0.00
				AND PD42.DC_ALW_ADL_PHN IN ('N','Q', 'U', ' ') /*Phone types with no consent*/
				AND LN10.LC_STA_LON10 = 'R'
				AND PD42.DN_DOM_PHN_ARA <> '' /*NOTE in the table a blank values is stored as '' and not a null*/
				AND WQ20.BF_SSN IS NULL 
				AND CT30.DF_PRS_ID_BR IS NULL
				AND (
						DID.BF_SSN IS NULL 
						OR (
								DID.BF_SSN IS NOT NULL 
								AND AUTO.BF_SSN IS NULL
							)
						OR (
								DID.LD_ATY_REQ_RCV > AUTO.LD_ATY_REQ_RCV
							)
					)
			FOR READ ONLY WITH UR
		);
QUIT;

PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);
	CREATE TABLE COM_ENDS AS
	SELECT
		*
	FROM
		CONNECTION TO DB2 
		(
			SELECT DISTINCT 
				LN20.LF_EDS AS ID,
				PD42.DN_DOM_PHN_ARA || PD42.DN_DOM_PHN_XCH || PD42.DN_DOM_PHN_LCL AS PHONE
			FROM
				OLWHRM1.PD10_PRS_NME PD10
				INNER JOIN OLWHRM1.PD42_PRS_PHN PD42
					ON PD10.DF_PRS_ID = PD42.DF_PRS_ID
				INNER JOIN OLWHRM1.LN20_EDS LN20
					ON PD10.DF_PRS_ID = LN20.LF_EDS
				INNER JOIN OLWHRM1.LN10_LON LN10
					ON LN20.BF_SSN = LN10.BF_SSN
					AND LN20.LN_SEQ = LN10.LN_SEQ
				LEFT JOIN OLWHRM1.WQ20_TSK_QUE WQ20
					ON WQ20.BF_SSN = LN10.BF_SSN
					AND WQ20.WF_QUE IN ('2A', '2P', 'GP', '9R', 'SI', 'S4', 'PR', 'VR', 'VB', 'MF', '1P', 'SF', 'DU', 'AT', '87')
					AND WQ20.WC_STA_WQUE20 IN ('A', 'H', 'P', 'U', 'W')
				LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY DOC ID*/
				(
					SELECT
						BF_SSN,
						MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
					FROM
						OLWHRM1.AY10_BR_LON_ATY
					WHERE
						PF_REQ_ACT = 'DIDDA'
					GROUP BY
						BF_SSN
				)DID 
					ON DID.BF_SSN = LN10.BF_SSN
				LEFT JOIN /*GETS THE MOST RECENT INSTANCE OF AUTOPAY APPROVAL*/
				(
					SELECT
						BF_SSN,
						MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
					FROM
						OLWHRM1.AY10_BR_LON_ATY
					WHERE
						PF_REQ_ACT = 'PAUTO'
					GROUP BY
						BF_SSN
				)AUTO 
					ON AUTO.BF_SSN = LN10.BF_SSN
				LEFT JOIN OLWHRM1.CT30_CALL_QUE CT30
					ON CT30.DF_PRS_ID_BR = LN10.BF_SSN
					AND CT30.IF_WRK_GRP IN ('ASBKP', 'ASCON', 'ASMOC')
					AND CT30.IF_WRK_GRP IN ('A', 'W')
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
							,CAST(TRANSLATE(AY20.LX_ATY,'',TRANSLATE(AY20.LX_ATY,'','1234567890',''),'') AS INT) AS PARSED_DAYS
							/*inner translate sets all numbers to blanks to identify extraneous text*/
							/*outer translate strips out extraneous text found by inner translate*/
						FROM
							OLWHRM1.AY10_BR_LON_ATY AY10
							LEFT JOIN OLWHRM1.AY20_ATY_TXT AY20
								ON AY10.BF_SSN = AY20.BF_SSN
								AND AY10.LN_ATY_SEQ = AY20.LN_ATY_SEQ
						WHERE
							AY10.LC_STA_ACTY10 = 'A'
							AND AY10.PF_REQ_ACT = 'BRPTP' /*borrower promise to repay*/
					)
				) ARC_BRPTP 
					ON LN10.BF_SSN = ARC_BRPTP.BF_SSN
					AND ARC_BRPTP.FUTURE_DATE = 1 /*exclude future arc date*/
			WHERE 
				ARC_BRPTP.BF_SSN IS NULL /*exclude future arc date*/
				AND PD42.DI_PHN_VLD = 'Y'
				AND LN10.LA_CUR_PRI > 0.00
				AND PD42.DC_ALW_ADL_PHN IN ('N','Q','U',' ')
				AND LN10.LC_STA_LON10 = 'R'
				AND PD42.DN_DOM_PHN_ARA <> '' /*NOTE in the table a blank values is stored as '' and not a null*/
				AND WQ20.BF_SSN IS NULL 
				AND CT30.DF_PRS_ID_BR IS NULL
				AND (
						DID.BF_SSN IS NULL 
						OR (
								DID.BF_SSN IS NOT NULL 
								AND AUTO.BF_SSN IS NULL
							)
						OR (
								DID.LD_ATY_REQ_RCV > AUTO.LD_ATY_REQ_RCV
							)
					)
			FOR READ ONLY WITH UR
		);
QUIT;

PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);
	CREATE TABLE ONELINK_BWRS AS
	SELECT	
		*
	FROM
		CONNECTION TO DB2 
		(
			SELECT DISTINCT 
				PD03.DF_PRS_ID AS ID,
				PD03.DN_PHN AS PHONE
			FROM
				OLWHRM1.GA14_LON_STA GA14
				INNER JOIN OLWHRM1.GA01_APP GA01
					ON GA01.AF_APL_ID = GA14.AF_APL_ID
				INNER JOIN OLWHRM1.PD03_PRS_ADR_PHN PD03
					ON PD03.DF_PRS_ID = GA01.DF_PRS_ID_BR
				INNER JOIN OLWHRM1.DC01_LON_CLM_INF DC01
					ON DC01.AF_APL_ID = GA14.AF_APL_ID
					AND DC01.AF_APL_ID_SFX = GA14.AF_APL_ID_SFX
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
							,CAST(TRANSLATE(AY20.LX_ATY,'',TRANSLATE(AY20.LX_ATY,'','1234567890',''),'') AS INT) AS PARSED_DAYS
							/*inner translate sets all numbers to blanks to identify extraneous text*/
							/*outer translate strips out extraneous text found by inner translate*/
						FROM
							OLWHRM1.AY10_BR_LON_ATY AY10
							LEFT JOIN OLWHRM1.AY20_ATY_TXT AY20
								ON AY10.BF_SSN = AY20.BF_SSN
								AND AY10.LN_ATY_SEQ = AY20.LN_ATY_SEQ
						WHERE
							AY10.LC_STA_ACTY10 = 'A'
							AND AY10.PF_REQ_ACT = 'BRPTP' /*borrower promise to repay*/
					)
				) ARC_BRPTP 
					ON PD03.DF_PRS_ID = ARC_BRPTP.BF_SSN
					AND ARC_BRPTP.FUTURE_DATE = 1 /*exclude future arc date*/
			WHERE 
				ARC_BRPTP.BF_SSN IS NULL /*exclude future arc date*/
				AND PD03.DI_PHN_VLD = 'Y'
				AND PD03.DC_CEP IN ('N','U',' ', 'T')
				AND PD03.DI_FGN_PHN = 'N' /*DOMESTIC PHONE*/
				AND GA14.AC_STA_GA14 = 'A' /*ACTIVE STATUS CODE FOR THE GA14 TABLE*/
				AND GA14.AC_LON_STA_TYP = 'CP' /*LOAN STATUS TYPE CODE*/
				AND DC01.LC_STA_DC10 = '03'
				AND DC01.LC_REA_CLM_ASN_DOE NOT IN ('03','08','21','23','24')/*SUBROGATION, FORCED SUBROGATION, PENDING SUBROGATION, PENDING TPD, TPD ASSIGNED, FINAL DISCHARGE*/
				AND PD03.DN_PHN <> '' /*NOTE in the table a blank values is stored as '' and not a null*/

			FOR READ ONLY WITH UR
		);
QUIT;

PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);
	CREATE TABLE ONELINK_BWRS_ALT AS
	SELECT	
		*
	FROM
		CONNECTION TO DB2 
		(
			SELECT DISTINCT 
				PD03.DF_PRS_ID AS ID,
				PD03.DN_ALT_PHN AS PHONE
			FROM
				OLWHRM1.GA14_LON_STA GA14
				INNER JOIN OLWHRM1.GA01_APP GA01
					ON GA01.AF_APL_ID = GA14.AF_APL_ID
				INNER JOIN OLWHRM1.PD03_PRS_ADR_PHN PD03
					ON PD03.DF_PRS_ID = GA01.DF_PRS_ID_BR
				INNER JOIN OLWHRM1.DC01_LON_CLM_INF DC01
					ON DC01.AF_APL_ID = GA14.AF_APL_ID
					AND DC01.AF_APL_ID_SFX = GA14.AF_APL_ID_SFX
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
							,CAST(TRANSLATE(AY20.LX_ATY,'',TRANSLATE(AY20.LX_ATY,'','1234567890',''),'') AS INT) AS PARSED_DAYS
							/*inner translate sets all numbers to blanks to identify extraneous text*/
							/*outer translate strips out extraneous text found by inner translate*/
						FROM
							OLWHRM1.AY10_BR_LON_ATY AY10
							LEFT JOIN OLWHRM1.AY20_ATY_TXT AY20
								ON AY10.BF_SSN = AY20.BF_SSN
								AND AY10.LN_ATY_SEQ = AY20.LN_ATY_SEQ
						WHERE
							AY10.LC_STA_ACTY10 = 'A'
							AND AY10.PF_REQ_ACT = 'BRPTP' /*borrower promise to repay*/
					)
				) ARC_BRPTP 
					ON PD03.DF_PRS_ID = ARC_BRPTP.BF_SSN
					AND ARC_BRPTP.FUTURE_DATE = 1 /*exclude future arc date*/
			WHERE 
				ARC_BRPTP.BF_SSN IS NULL /*exclude future arc date*/
				AND PD03.DI_ALT_PHN_VLD = 'Y'
				AND PD03.DC_ALT_CEP IN ('N','U',' ', 'T')
				AND PD03.DI_FGN_ALT_PHN = 'N' /*DOMESTIC PHONE*/
				AND GA14.AC_STA_GA14 = 'A' /*ACTIVE STATUS CODE FOR THE GA14 TABLE*/
				AND GA14.AC_LON_STA_TYP = 'CP' /*LOAN STATUS TYPE CODE*/
				AND DC01.LC_STA_DC10 = '03'
				AND DC01.LC_REA_CLM_ASN_DOE NOT IN ('03','08','21','23','24')/*SUBROGATION, FORCED SUBROGATION, PENDING SUBROGATION, PENDING TPD, TPD ASSIGNED, FINAL DISCHARGE*/
				AND PD03.DN_ALT_PHN <> '' /*NOTE in the table a blank values is stored as '' and not a null*/

			FOR READ ONLY WITH UR
		);
QUIT;

PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);	
	CREATE TABLE REF AS
	SELECT 
		*
	FROM	
		CONNECTION TO DB2
		(
			SELECT DISTINCT
				PD42.DF_PRS_ID AS ID,
				PD42.DN_DOM_PHN_ARA || PD42.DN_DOM_PHN_XCH || PD42.DN_DOM_PHN_LCL AS PHONE
			FROM	
				OLWHRM1.RF10_RFR RF10
				INNER JOIN OLWHRM1.LN10_LON LN10
					ON RF10.BF_SSN = LN10.BF_SSN
				LEFT JOIN OLWHRM1.PD42_PRS_PHN PD42
					ON RF10.BF_RFR = PD42.DF_PRS_ID
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
							,CAST(TRANSLATE(AY20.LX_ATY,'',TRANSLATE(AY20.LX_ATY,'','1234567890',''),'') AS INT) AS PARSED_DAYS
							/*inner translate sets all numbers to blanks to identify extraneous text*/
							/*outer translate strips out extraneous text found by inner translate*/
						FROM
							OLWHRM1.AY10_BR_LON_ATY AY10
							LEFT JOIN OLWHRM1.AY20_ATY_TXT AY20
								ON AY10.BF_SSN = AY20.BF_SSN
								AND AY10.LN_ATY_SEQ = AY20.LN_ATY_SEQ
						WHERE
							AY10.LC_STA_ACTY10 = 'A'
							AND AY10.PF_REQ_ACT = 'BRPTP' /*borrower promise to repay*/
					)
				) ARC_BRPTP 
					ON LN10.BF_SSN = ARC_BRPTP.BF_SSN
					AND ARC_BRPTP.FUTURE_DATE = 1 /*exclude future arc date*/
			WHERE
				ARC_BRPTP.BF_SSN IS NULL /*exclude future arc date*/
				AND PD42.DI_PHN_VLD = 'Y'
				AND LN10.LA_CUR_PRI > 0.00
				AND PD42.DC_ALW_ADL_PHN IN ('N','Q','U',' ')
				AND LN10.LC_STA_LON10 = 'R'
				AND PD42.DN_DOM_PHN_ARA <> '' /*NOTE in the table a blank values is stored as '' and not a null*/

			FOR READ ONLY WITH UR
		);
QUIT;

PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);
	CREATE TABLE ONELINK_REF_PRIM AS
	SELECT 
		*
	FROM
		CONNECTION TO DB2 
		(
			SELECT DISTINCT	
				BR03.DF_PRS_ID_RFR AS ID,
				BR03.BN_RFR_DOM_PHN AS PHONE
			FROM
				OLWHRM1.BR03_BR_REF BR03
				INNER JOIN OLWHRM1.DC01_LON_CLM_INF DC01
					ON DC01.BF_SSN = BR03.DF_PRS_ID_BR
				INNER JOIN OLWHRM1.GA10_LON_APP GA10
					ON GA10.AF_APL_ID = DC01.AF_APL_ID
					AND GA10.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
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
							,CAST(TRANSLATE(AY20.LX_ATY,'',TRANSLATE(AY20.LX_ATY,'','1234567890',''),'') AS INT) AS PARSED_DAYS
							/*inner translate sets all numbers to blanks to identify extraneous text*/
							/*outer translate strips out extraneous text found by inner translate*/
						FROM
							OLWHRM1.AY10_BR_LON_ATY AY10
							LEFT JOIN OLWHRM1.AY20_ATY_TXT AY20
								ON AY10.BF_SSN = AY20.BF_SSN
								AND AY10.LN_ATY_SEQ = AY20.LN_ATY_SEQ
						WHERE
							AY10.LC_STA_ACTY10 = 'A'
							AND AY10.PF_REQ_ACT = 'BRPTP' /*borrower promise to repay*/
					)
				) ARC_BRPTP 
					ON DC01.BF_SSN = ARC_BRPTP.BF_SSN
					AND ARC_BRPTP.FUTURE_DATE = 1 /*exclude future arc date*/
			WHERE
				ARC_BRPTP.BF_SSN IS NULL /*exclude future arc date*/
				AND BR03.BI_DOM_PHN_VLD = 'Y' 
				AND BR03.BC_PRI_PHN_ALW IN ('N',' ')
				AND GA10.AA_CUR_PRI > 0.00
				AND BR03.BN_RFR_DOM_PHN <> '' /*NOTE in the table a blank values is stored as '' and not a null*/
				AND BR03.BC_STA_BR03 = 'A'
		);
QUIT;

PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);
	CREATE TABLE ONELINK_REF_ALT AS
	SELECT 
		*
	FROM
		CONNECTION TO DB2 
		(
			SELECT DISTINCT	
				BR03.DF_PRS_ID_RFR AS ID,
				BR03.BN_RFR_ALT_PHN AS PHONE
			FROM
				OLWHRM1.BR03_BR_REF BR03
				INNER JOIN OLWHRM1.DC01_LON_CLM_INF DC01
					ON DC01.BF_SSN = BR03.DF_PRS_ID_BR
				INNER JOIN OLWHRM1.GA10_LON_APP GA10
					ON GA10.AF_APL_ID = DC01.AF_APL_ID
					AND GA10.AF_APL_ID_SFX = DC01.AF_APL_ID_SFX
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
							,CAST(TRANSLATE(AY20.LX_ATY,'',TRANSLATE(AY20.LX_ATY,'','1234567890',''),'') AS INT) AS PARSED_DAYS
							/*inner translate sets all numbers to blanks to identify extraneous text*/
							/*outer translate strips out extraneous text found by inner translate*/
						FROM
							OLWHRM1.AY10_BR_LON_ATY AY10
							LEFT JOIN OLWHRM1.AY20_ATY_TXT AY20
								ON AY10.BF_SSN = AY20.BF_SSN
								AND AY10.LN_ATY_SEQ = AY20.LN_ATY_SEQ
						WHERE
							AY10.LC_STA_ACTY10 = 'A'
							AND AY10.PF_REQ_ACT = 'BRPTP' /*borrower promise to repay*/
					)
				) ARC_BRPTP 
					ON DC01.BF_SSN = ARC_BRPTP.BF_SSN
					AND ARC_BRPTP.FUTURE_DATE = 1 /*exclude future arc date*/
			WHERE
				ARC_BRPTP.BF_SSN IS NULL /*exclude future arc date*/
				AND BR03.BI_ALT_PHN_VLD = 'Y' 
				AND BR03.BN_RFR_ALT_PHN IN ('N',' ')
				AND GA10.AA_CUR_PRI > 0.00
				AND BR03.BN_RFR_ALT_PHN <> '' /*NOTE in the table a blank values is stored as '' and not a null*/
				AND BR03.BC_STA_BR03 = 'A'
		);
DISCONNECT FROM DB2;
QUIT;

%MACRO SORT(TAB);
	PROC SORT DATA=&TAB; 
		BY ID; 
	RUN;
%MEND SORT;
%SORT (COMPASS_BWRS);
%SORT (COM_ENDS);
%SORT (ONELINK_BWRS);
%SORT (ONELINK_BWRS_ALT);
%SORT (REF);
%SORT (ONELINK_REF_PRIM);
%SORT (ONELINK_REF_ALT);

DATA ALL_PHONES;
	MERGE COMPASS_BWRS COM_ENDS ONELINK_BWRS ONELINK_BWRS_ALT REF ONELINK_REF_PRIM ONELINK_REF_ALT;
	BY ID;
RUN;

PROC SQL;
	CREATE TABLE R2 AS
	SELECT DISTINCT
		PHONE
	FROM
		ALL_PHONES
	ORDER BY
		PHONE
	;
QUIT;

ENDRSUBMIT;

DATA R2; 
	SET DUSTER.R2; 
RUN;

DATA _NULL_;
	SET	WORK.R2;
	FILE REPORT2 delimiter='|' DSD DROPOVER lrecl=32767;
	DO;
		PUT PHONE;
	;
	END;
RUN;
