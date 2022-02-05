/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS32.NWS32RZ";
FILENAME REPORT2 "&RPTLIB/UNWS32.NWS32R2";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
RSUBMIT LEGEND;

/*%LET DB = DNFPRQUT;  *VUK1 test;*/
/*%LET DB = DNFPRUUT;  *VUK3 test;*/
%LET DB = DNFPUTDL;  *live;

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;
PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);
	CREATE TABLE NoConsent AS
	SELECT DISTINCT
		PHN
	FROM
		CONNECTION TO DB2
		(
			/*BORROWERS*/
			SELECT DISTINCT
				PD40.DN_DOM_PHN_ARA || PD40.DN_DOM_PHN_XCH || PD40.DN_DOM_PHN_LCL AS PHN
			FROM
				PKUB.PD10_PRS_NME PD10
				INNER JOIN PKUB.LN10_LON LN10
					ON PD10.DF_PRS_ID = LN10.BF_SSN
				INNER JOIN PKUB.PD40_PRS_PHN PD40
					ON PD10.DF_PRS_ID = PD40.DF_PRS_ID
				LEFT JOIN PKUB.WQ20_TSK_QUE WQ20
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
				ARC_BRPTP.BF_SSN IS NULL /*exclude future arc date*/
				AND LN10.LA_CUR_PRI > 0.00
				AND LN10.LC_STA_LON10 = 'R'
				AND PD40.DI_PHN_VLD = 'Y'
				AND PD40.DC_ALW_ADL_PHN IN ('N','U','X',' ')
				AND PD40.DN_DOM_PHN_ARA <> ' '
				AND WQ20.BF_SSN IS NULL

			UNION ALL /*ENDORSERS*/

			SELECT DISTINCT
				PD40.DN_DOM_PHN_ARA || PD40.DN_DOM_PHN_XCH || PD40.DN_DOM_PHN_LCL AS "PHN"
			FROM
				PKUB.PD10_PRS_NME PD10
				INNER JOIN PKUB.LN20_EDS LN20
					ON PD10.DF_PRS_ID = LN20.LF_EDS
				INNER JOIN PKUB.LN10_LON LN10
					ON LN20.BF_SSN = LN10.BF_SSN
				INNER JOIN PKUB.PD40_PRS_PHN PD40
					ON PD10.DF_PRS_ID = PD40.DF_PRS_ID
				LEFT JOIN PKUB.WQ20_TSK_QUE WQ20
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
				ARC_BRPTP.BF_SSN IS NULL /*exclude future arc date*/
				AND LN10.LA_CUR_PRI > 0.00
				AND	LN10.LC_STA_LON10 = 'R'
				AND PD40.DI_PHN_VLD = 'Y'
				AND PD40.DC_ALW_ADL_PHN IN ('N','U','X',' ')
				AND PD40.DN_DOM_PHN_ARA <> ' '	
				AND WQ20.BF_SSN IS NULL

			UNION ALL /*REFERENCES*/

			SELECT DISTINCT
				PD40.DN_DOM_PHN_ARA || PD40.DN_DOM_PHN_XCH || PD40.DN_DOM_PHN_LCL AS "PHN"
			FROM	
				PKUB.PD10_PRS_NME PD10
				INNER JOIN PKUB.RF10_RFR RF10
					ON PD10.DF_PRS_ID = RF10.BF_RFR
				INNER JOIN PKUB.LN10_LON LN10
					ON RF10.BF_SSN = LN10.BF_SSN
				INNER JOIN PKUB.PD40_PRS_PHN PD40
					ON PD10.DF_PRS_ID = PD40.DF_PRS_ID
				LEFT JOIN PKUB.WQ20_TSK_QUE WQ20
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
				ARC_BRPTP.BF_SSN IS NULL /*exclude future arc date*/
				AND LN10.LA_CUR_PRI > 0.00
				AND	LN10.LC_STA_LON10 = 'R'
				AND PD40.DI_PHN_VLD = 'Y'
				AND PD40.DC_ALW_ADL_PHN IN ('N','U','X',' ')	
				AND	PD40.DN_DOM_PHN_ARA <> ' '
				AND WQ20.BF_SSN IS NULL

			FOR READ ONLY WITH UR
		)
	ORDER BY
		PHN
	;
	DISCONNECT FROM DB2;
QUIT;
ENDRSUBMIT;
DATA NoConsent; 
	SET LEGEND.NoConsent; 
RUN;
DATA _NULL_;
	SET	NoConsent;
	FILE REPORT2 delimiter='|' DSD DROPOVER lrecl=32767;
	DO;
		PUT PHN $ ;
	END;
RUN;
