%LET RPTLIB = T:\SAS;
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
/*LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\ServicerInventoryMetrics_TEST.dsn; bl_keepnulls=no; READ_ISOLATION_LEVEL=RU" SCHEMA= dbo;*/
LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\ServicerInventoryMetrics.dsn; bl_keepnulls=no; READ_ISOLATION_LEVEL=RU" SCHEMA= dbo;


RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME AES DB2 DATABASE=&DB OWNER=AES;
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
%MEND SQLCHECK ;


/*	Pull data down from AES to insert into ServicerInventoryMetrics database.
	Data being pulled down are all open tasks within each metric.
	SASR 4006 is a correlated job that will analyze the data and calculate percentages. */
PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);

    CREATE TABLE Forbearance AS
        SELECT
            *
        FROM 
            CONNECTION TO DB2 
				(
					SELECT DISTINCT
						WQ20.BF_SSN,
						WQ20.WF_QUE,
						WQ20.WF_SUB_QUE,
						WQ20.WN_CTL_TSK,
						WQ20.PF_REQ_ACT,
						WQ20.WD_ACT_REQ,
						WQ20.WC_STA_WQUE20,
						WQ20.WF_LST_DTS_WQ20
					FROM
						PKUB.WQ20_TSK_QUE WQ20
					WHERE
						(
							WQ20.WF_QUE = 'SF' 
							OR 
								( 
									WQ20.WF_QUE IN ('VB','VR','WR') 
									AND WQ20.PF_REQ_ACT IN ('XFORB','BRRPF','G7096')
								)
						)
						AND WQ20.WC_STA_WQUE20 NOT IN ('C','X')
					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE Deferment AS
		SELECT
            *
        FROM 
            CONNECTION TO DB2 
				(
					SELECT DISTINCT
						WQ20.BF_SSN,
						WQ20.WF_QUE,
						WQ20.WF_SUB_QUE,
						WQ20.WN_CTL_TSK,
						WQ20.PF_REQ_ACT,
						WQ20.WD_ACT_REQ,
						WQ20.WC_STA_WQUE20,
						WQ20.WF_LST_DTS_WQ20
					FROM
						PKUB.WQ20_TSK_QUE WQ20
				WHERE
					(
						WQ20.WF_QUE = 'S4'
						OR
							(
								WQ20.WF_QUE = 'WR'
								AND WQ20.PF_REQ_ACT = 'G708C'
							)
					)
					AND WQ20.WC_STA_WQUE20 NOT IN ('C','X')
					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE IDR AS
		SELECT
            *
        FROM 
            CONNECTION TO DB2 
				(
					SELECT DISTINCT
						WQ20.BF_SSN,
						WQ20.WF_QUE,
						WQ20.WF_SUB_QUE,
						WQ20.WN_CTL_TSK,
						WQ20.PF_REQ_ACT,
						WQ20.WD_ACT_REQ,
						WQ20.WC_STA_WQUE20,
						WQ20.WF_LST_DTS_WQ20
					FROM
						PKUB.WQ20_TSK_QUE WQ20
					WHERE
						WQ20.WF_QUE IN ('2A')
						AND WQ20.PF_REQ_ACT IN ('CODCA', 'CODPA', 'IBRDF', 'IDRPR')
						AND WQ20.WC_STA_WQUE20 NOT IN ('C','X')

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE BankruptcyNotice AS 
		SELECT
            *
        FROM 
            CONNECTION TO DB2 
				(
					SELECT DISTINCT
						WQ20.BF_SSN,
						WQ20.WF_QUE,
						WQ20.WF_SUB_QUE,
						WQ20.WN_CTL_TSK,
						WQ20.PF_REQ_ACT,
						WQ20.WD_ACT_REQ,
						WQ20.WC_STA_WQUE20,
						WQ20.WF_LST_DTS_WQ20
					FROM
						PKUB.WQ20_TSK_QUE WQ20
					WHERE
						WQ20.WF_QUE IN ('87')
						AND WQ20.PF_REQ_ACT IN ('DIBKP','CRBKP')
						AND WQ20.WC_STA_WQUE20 NOT IN ('C','X')

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE BankruptcyProofOfClaim AS 
		SELECT
            *
        FROM 
            CONNECTION TO DB2 
				(
					SELECT DISTINCT
						WQ20.BF_SSN,
						WQ20.WF_QUE,
						WQ20.WF_SUB_QUE,
						WQ20.WN_CTL_TSK,
						WQ20.PF_REQ_ACT,
						WQ20.WD_ACT_REQ,
						WQ20.WC_STA_WQUE20,
						WQ20.WF_LST_DTS_WQ20,
						DATE(Deadline.LX_ATY) AS LX_ATY
					FROM
						PKUB.WQ20_TSK_QUE WQ20
						INNER JOIN 
							(
								SELECT
									AY10I.BF_SSN,
									AY20I.LX_ATY,
									AY10I.LD_ATY_REQ_RCV
								FROM /*LX_ATY and LD_ATY_REQ_RCV for max LD_ATY_REQ_RCV for a BPOCD ARC*/
									(
										SELECT
											AY10.BF_SSN,
											AY10.LD_ATY_REQ_RCV,
											AY10.LN_ATY_SEQ
										FROM
											( /*max LD_ATY_REQ_RCV for a BPOCD ARC*/
												SELECT 
													BF_SSN,
													MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
												FROM
													PKUB.AY10_BR_LON_ATY
												WHERE
													PF_REQ_ACT = 'BPOCD'
													AND LC_STA_ACTY10 = 'A'
												GROUP BY
													BF_SSN
											) MAXARC
											INNER JOIN PKUB.AY10_BR_LON_ATY AY10
												ON MAXARC.BF_SSN = AY10.BF_SSN
												AND MAXARC.LD_ATY_REQ_RCV = AY10.LD_ATY_REQ_RCV
												AND AY10.PF_REQ_ACT = 'BPOCD'
												AND AY10.LC_STA_ACTY10 = 'A'
									) AY10I
									INNER JOIN PKUB.AY15_ATY_CMT AY15
										ON AY15.BF_SSN = AY10I.BF_SSN
										AND AY15.LN_ATY_SEQ = AY10I.LN_ATY_SEQ  
									INNER JOIN PKUB.AY20_ATY_TXT AY20I
										ON AY10I.BF_SSN = AY20I.BF_SSN
										AND AY10I.LN_ATY_SEQ = AY20I.LN_ATY_SEQ
										AND AY15.LN_ATY_CMT_SEQ = AY20I.LN_ATY_CMT_SEQ
								WHERE
									AY15.LC_STA_AY15 = 'A'
							) Deadline
								ON WQ20.BF_SSN = Deadline.BF_SSN
					WHERE
						WQ20.WF_QUE IN ('BY')
						AND WQ20.PF_REQ_ACT IN ('BPOCR')
						AND WQ20.WC_STA_WQUE20 NOT IN ('C','X')

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE DeathDischarge AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2
				(
					SELECT DISTINCT
						DIDTH.BF_SSN,
						COALESCE(WQ20.WF_QUE,WQ21.WF_QUE) AS WF_QUE,
						COALESCE(WQ20.WF_SUB_QUE,WQ21.WF_SUB_QUE) AS WF_SUB_QUE,
						COALESCE(WQ20.WN_CTL_TSK,WQ21.WN_CTL_TSK) AS WN_CTL_TSK,
						COALESCE(WQ20.PF_REQ_ACT,WQ21.PF_REQ_ACT) AS PF_REQ_ACT,
						COALESCE(WQ20.WD_ACT_REQ,WQ21.WD_ACT_REQ) AS WD_ACT_REQ,
						COALESCE(WQ20.WC_STA_WQUE20,WQ21.WC_STA_WQUE20) AS WC_STA_WQUE20,
						COALESCE(WQ20.WF_LST_DTS_WQ20,WQ21.WF_LST_DTS_WQ20) AS WF_LST_DTS_WQ20
					FROM 
						( /* max LD_ATY_REQ_RCV for DIDTH ARC*/
							SELECT
								BF_SSN,
								MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
							FROM
								PKUB.AY10_BR_LON_ATY
							WHERE
								LC_STA_ACTY10 = 'A'
								AND PF_REQ_ACT = 'DIDTH'
							GROUP BY
								BF_SSN
						) DIDTH
						INNER JOIN
							(
								SELECT
									DET.BF_SSN,
									DET.WF_QUE,
									DET.WF_SUB_QUE,
									DET.WN_CTL_TSK,
									DET.PF_REQ_ACT,
									DET.WD_ACT_REQ,
									DET.WC_STA_WQUE20,
									DET.WF_LST_DTS_WQ20
								FROM
									( /*most recent type 23 queue task*/
										SELECT
											BF_SSN,
											MAX(WF_LST_DTS_WQ20) AS WF_LST_DTS_WQ20
										FROM
											PKUB.WQ20_TSK_QUE
										WHERE
											WF_QUE = '23'
										GROUP BY
											BF_SSN
									) MXWQ20 /*most recent type 23 queue task*/
									INNER JOIN PKUB.WQ20_TSK_QUE DET /*get detail for most recent type 23 queue task*/
										ON MXWQ20.BF_SSN = DET.BF_SSN
										AND MXWQ20.WF_LST_DTS_WQ20 = DET.WF_LST_DTS_WQ20
							) WQ20
								ON DIDTH.BF_SSN = WQ20.BF_SSN
						LEFT JOIN
							(
								SELECT
									DET.BF_SSN,
									DET.WF_QUE,
									DET.WF_SUB_QUE,
									DET.WN_CTL_TSK,
									DET.PF_REQ_ACT,
									DET.WD_ACT_REQ,
									DET.WC_STA_WQUE20,
									DET.WF_LST_DTS_WQ20
								FROM
									(
										SELECT
											BF_SSN,
											MAX(WF_LST_DTS_WQ20) AS WF_LST_DTS_WQ20
										FROM
											PKUB.WQ21_TSK_QUE_HST
										WHERE
											WF_QUE = '23'
										GROUP BY
											BF_SSN
									) MXWQ21 /*most recent type 23 queue task*/
									INNER JOIN PKUB.WQ21_TSK_QUE_HST DET /*get detail for most recent type 23 queue task*/
										ON MXWQ21.BF_SSN = DET.BF_SSN
										AND MXWQ21.WF_LST_DTS_WQ20 = DET.WF_LST_DTS_WQ20
							) WQ21
								ON DIDTH.BF_SSN = WQ21.BF_SSN
						LEFT JOIN /*'DEFSA' activity records*/
						( /* max LD_ATY_REQ_RCV for DEFSA ARC*/
							SELECT DISTINCT
								AY10.BF_SSN,
								MAX(AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
							FROM
								PKUB.AY10_BR_LON_ATY AY10
							WHERE
								AY10.PF_REQ_ACT = 'DEFSA'
								AND AY10.LC_STA_ACTY10 = 'A'
							GROUP BY
								BF_SSN
						) AS DEFSA
							ON DIDTH.BF_SSN = DEFSA.BF_SSN
							AND DEFSA.LD_ATY_REQ_RCV > DIDTH.LD_ATY_REQ_RCV /*DEFSA ARC dated after the most recent DIDTH ARC*/						
					WHERE
						DEFSA.BF_SSN IS NULL /* Denotes there isnt an DEFSA ARC*/
					
					FOR READ ONLY WITH UR
				)
	;  

	CREATE TABLE ClosedSchoolApp AS 
		SELECT
            *
        FROM 
            CONNECTION TO DB2 
				(
					SELECT DISTINCT
						WQ20.BF_SSN,
						WQ20.WF_QUE,
						WQ20.WF_SUB_QUE,
						WQ20.WN_CTL_TSK,
						WQ20.PF_REQ_ACT,
						WQ20.WD_ACT_REQ,
						WQ20.WC_STA_WQUE20,
						WQ20.WF_LST_DTS_WQ20
					FROM
						PKUB.WQ20_TSK_QUE WQ20
					WHERE
						WQ20.WF_QUE IN ('15')
						AND WQ20.PF_REQ_ACT IN ('DICSK')
						AND WQ20.WC_STA_WQUE20 NOT IN ('C','X')

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE BorrowerMail AS 
		SELECT
            *
        FROM 
            CONNECTION TO DB2 
				(
					SELECT DISTINCT
						WQ20.BF_SSN,
						WQ20.WF_QUE,
						WQ20.WF_SUB_QUE,
						WQ20.WN_CTL_TSK,
						WQ20.PF_REQ_ACT,
						WQ20.WD_ACT_REQ,
						WQ20.WC_STA_WQUE20,
						WQ20.WF_LST_DTS_WQ20
					FROM
						PKUB.WQ20_TSK_QUE WQ20
					WHERE
						WQ20.WF_QUE IN ('88')
						AND WQ20.PF_REQ_ACT IN ('DIBCR')
						AND WQ20.WC_STA_WQUE20 NOT IN ('C','X')

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE DeathDischargeFSA AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2
				(
					SELECT
						AY10.BF_SSN,
						AY10.PF_REQ_ACT,
						AY10.LD_ATY_REQ_RCV
					FROM 
						( /* max LD_ATY_REQ_RCV for DEFSA ARC*/
							SELECT
								BF_SSN,
								PF_REQ_ACT,
								MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
							FROM
								PKUB.AY10_BR_LON_ATY
							WHERE
								LC_STA_ACTY10 = 'A'
								AND PF_REQ_ACT = 'DEFSA'
							GROUP BY
								BF_SSN,
								PF_REQ_ACT
						) AY10
			/*				'ADDTH' and'DEDNY' activity records*/
						LEFT JOIN 
							(
								SELECT
									AY10I.BF_SSN
								FROM
									PKUB.AY10_BR_LON_ATY AY10I
								WHERE
									AY10I.PF_REQ_ACT IN('ADDTH','DEDNY')
									AND AY10I.LC_STA_ACTY10 = 'A'
							) AS AY10Complete
								ON AY10.BF_SSN = AY10Complete.BF_SSN
					WHERE
						AY10Complete.BF_SSN IS NULL /* Denotes there isnt an ADDTH or DEDNY */
					FOR READ ONLY WITH UR
				) 
	;

	CREATE TABLE ClosedSchoolFSA AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2
				(
					SELECT
						AY10.BF_SSN,
						AY10.PF_REQ_ACT,
						AY10.LD_ATY_REQ_RCV
					FROM 
						( /* max LD_ATY_REQ_RCV for CSFSA ARC*/
							SELECT
								BF_SSN,
								PF_REQ_ACT,
								MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
							FROM
								PKUB.AY10_BR_LON_ATY
							WHERE
								LC_STA_ACTY10 = 'A'
								AND PF_REQ_ACT = 'CSFSA'
							GROUP BY
								BF_SSN,
								PF_REQ_ACT
						) AY10
			/*			'CSDNY' 'ADCSH' activity records*/
						LEFT JOIN
							( /*LN_ATY_SEQ and LD_ATY_REQ_RCV for max LD_ATY_REQ_RCV for CSDNY or ADCSH ARC*/
								SELECT
									AY10I.BF_SSN,
									AY10I.LN_ATY_SEQ,
									AY10I.LD_ATY_REQ_RCV
								FROM
									PKUB.AY10_BR_LON_ATY AY10I
									INNER JOIN
										(/* max LD_ATY_REQ_RCV for CSDNY or ADCSH ARC*/
											SELECT
												BF_SSN,
												MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
											FROM
												PKUB.AY10_BR_LON_ATY
											WHERE
												PF_REQ_ACT IN('CSDNY','ADCSH')
												AND LC_STA_ACTY10 = 'A'
											GROUP BY
												BF_SSN
										) MAXARC
											ON AY10I.BF_SSN = MAXARC.BF_SSN
											AND AY10I.LD_ATY_REQ_RCV = MAXARC.LD_ATY_REQ_RCV
								WHERE
									AY10I.PF_REQ_ACT IN('CSDNY','ADCSH')
									AND AY10I.LC_STA_ACTY10 = 'A'
							) AS AY10Complete
								ON AY10.BF_SSN = AY10Complete.BF_SSN
								AND AY10Complete.LD_ATY_REQ_RCV > AY10.LD_ATY_REQ_RCV
					WHERE
						AY10Complete.BF_SSN IS NULL

					FOR READ ONLY WITH UR
				) /* Denotes there isnt an ADDTH or DEDNY */
	;

	CREATE TABLE Aging360AtServicer AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2
				(
					SELECT
						LN10.BF_SSN,
						LN10.LN_SEQ,
						LN16.LD_DLQ_OCC + 360 DAYS AS AGING_DATE
					FROM
						PKUB.LN10_LON LN10
						INNER JOIN PKUB.LN16_LON_DLQ_HST LN16
							ON LN16.BF_SSN = LN10.BF_SSN
							AND LN16.LN_SEQ = LN10.LN_SEQ
			/*select borrowers in collection suspension status */
						LEFT JOIN 
							(
								SELECT
									LN60.BF_SSN,
									LN60.LN_SEQ
								FROM
									PKUB.LN60_BR_FOR_APV LN60
									INNER JOIN PKUB.FB10_BR_FOR_REQ FB10
										ON LN60.BF_SSN = FB10.BF_SSN
										AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
								WHERE
									LN60.LC_STA_LON60 = 'A'
									AND FB10.LC_FOR_STA = 'A'
									AND FB10.LC_STA_FOR10 = 'A'
									AND FB10.LC_FOR_TYP = '28'
									AND LN60.LD_FOR_END  >= CURRENT DATE	
							) SUSP
								ON LN10.BF_SSN = SUSP.BF_SSN
								AND LN10.LN_SEQ = SUSP.LN_SEQ
			/*select borrowers with DCSTR activity records*/
						LEFT JOIN 
							(
								SELECT
									AY10I.BF_SSN
								FROM
									PKUB.AY10_BR_LON_ATY AY10I
								WHERE
									AY10I.PF_REQ_ACT = 'DCSTR'
									AND LC_STA_ACTY10 = 'A'
							) AS AY10
								ON AY10.BF_SSN = LN10.BF_SSN
					WHERE
						LN16.LN_DLQ_MAX >= 360
						AND CURRENT DATE - COALESCE(LN16.LD_DLQ_MAX,CURRENT DATE) > 5
						AND LN16.LC_STA_LON16 = '1'
						AND LN10.LC_STA_LON10 <> 'D'
						AND LN10.LA_CUR_PRI > 0
						AND AY10.BF_SSN IS NULL /*exclude borrowers with DCSTR ARC*/
						AND SUSP.BF_SSN IS NULL  /* exclude borrowers in collection suspension status*/

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE Aging360SentToDMCS AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2
				(
					SELECT
						LN10.BF_SSN,
						LN10.LN_SEQ,
						AY10.LD_ATY_REQ_RCV AS AGING_DATE /* max LD_ATY_REQ_RCV for DCSTR ARC*/
					FROM
						PKUB.LN10_LON LN10
						INNER JOIN PKUB.LN16_LON_DLQ_HST LN16
							ON LN16.BF_SSN = LN10.BF_SSN
							AND LN16.LN_SEQ = LN10.LN_SEQ
			/*select borrowers with DCSTR activity records*/
						LEFT JOIN
							( /* max LD_ATY_REQ_RCV for DCSTR ARC*/
								SELECT
									AY10I.BF_SSN,
									MAX(AY10I.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
								FROM
									PKUB.AY10_BR_LON_ATY AY10I
								WHERE
									AY10I.PF_REQ_ACT = 'DCSTR'
									AND AY10I.LC_STA_ACTY10 = 'A'
								GROUP BY
									AY10I.BF_SSN
							) AS AY10
								ON AY10.BF_SSN = LN10.BF_SSN

			/*select borrowers with DCSLD activity records*/
						LEFT JOIN 
							( /* max LD_ATY_REQ_RCV for DCSLD ARC*/
								SELECT
									AY10E.BF_SSN,
									MAX(AY10E.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
								FROM
									PKUB.AY10_BR_LON_ATY AY10E
								WHERE
									AY10E.PF_REQ_ACT = 'DCSLD'
									AND AY10E.LC_STA_ACTY10 = 'A'
								GROUP BY
								AY10E.BF_SSN
							) AS AY10Ex
								ON AY10Ex.BF_SSN = AY10.BF_SSN
								AND AY10Ex.LD_ATY_REQ_RCV > AY10.LD_ATY_REQ_RCV
					WHERE
						LN16.LN_DLQ_MAX >= 360
						AND LN16.LC_STA_LON16 = '1'
						AND LN10.LC_STA_LON10 = 'D'
						AND LN10.LC_SST_LON10 IN('5','7')
						AND AY10.BF_SSN IS NOT NULL
						AND AY10Ex.BF_SSN IS NULL
						AND LN10.LA_CUR_PRI > 0
					FOR READ ONLY WITH UR
				)
	;
	
	CREATE TABLE Aging360NotAcceptedByDMCS AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2
				(
					SELECT
						LN10.BF_SSN,
						LN10.LN_SEQ,
						AY10.LD_ATY_REQ_RCV AS AGING_DATE
					FROM
						PKUB.LN10_LON LN10
						INNER JOIN PKUB.LN16_LON_DLQ_HST LN16
							ON LN16.BF_SSN = LN10.BF_SSN
							AND LN16.LN_SEQ = LN10.LN_SEQ
						INNER JOIN 
							(
								SELECT
									ATY.BF_SSN,
									ATY.LN_ATY_SEQ,  /* LN_ATY_SEQ for max LD_ATY_REQ_RCV for DCSRR ARC*/
									MAXARC.LD_ATY_REQ_RCV
								FROM
									PKUB.AY10_BR_LON_ATY ATY
									INNER JOIN
										( /* max LD_ATY_REQ_RCV for DCSRR ARC*/
											SELECT 
												BF_SSN,
												MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
											FROM
												PKUB.AY10_BR_LON_ATY
											WHERE
												PF_REQ_ACT = 'DCSRR'
												AND LC_STA_ACTY10 = 'A' 
											GROUP BY
												BF_SSN
										) MAXARC
											ON ATY.BF_SSN = MAXARC.BF_SSN
											AND ATY.LD_ATY_REQ_RCV = MAXARC.LD_ATY_REQ_RCV
									WHERE
										ATY.PF_REQ_ACT = 'DCSRR'
										AND ATY.LC_STA_ACTY10 = 'A'
							) AY10
								ON LN10.BF_SSN = AY10.BF_SSN
						INNER JOIN PKUB.AY15_ATY_CMT AY15
							ON AY15.BF_SSN = AY10.BF_SSN
							AND AY15.LN_ATY_SEQ = AY10.LN_ATY_SEQ	
						INNER JOIN PKUB.AY20_ATY_TXT AY20
							ON AY10.BF_SSN = AY20.BF_SSN
							AND AY10.LN_ATY_SEQ = AY20.LN_ATY_SEQ
							AND AY15.LN_ATY_CMT_SEQ = AY20.LN_ATY_CMT_SEQ
			/*select borrowers with DCSTR activity records*/
						LEFT JOIN
							(/* max LD_ATY_REQ_RCV for DCSTR ARC*/
								SELECT
									AY10I.BF_SSN,
									MAX(AY10I.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
								FROM
									PKUB.AY10_BR_LON_ATY AY10I
								WHERE
									AY10I.PF_REQ_ACT = 'DCSTR'
									AND AY10I.LC_STA_ACTY10 = 'A'
								GROUP BY
									BF_SSN
							) AS AY10Complete
							ON AY10.BF_SSN = AY10Complete.BF_SSN
							AND AY10Complete.LD_ATY_REQ_RCV > AY10.LD_ATY_REQ_RCV
			WHERE
				AY15.LC_STA_AY15 = 'A'
				AND RIGHT(AY20.LX_ATY,4) != '0031'
				AND LN16.LN_DLQ_MAX >= 360
				AND LN16.LC_STA_LON16 = '1'
				AND LN10.LC_STA_LON10 <> 'D'
				AND AY10Complete.BF_SSN IS NULL
				AND LN10.LA_CUR_PRI > 0

			FOR READ ONLY WITH UR
		);

				DISCONNECT FROM DB2;
	%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;
	%SQLCHECK;
QUIT;

ENDRSUBMIT;

DATA Forbearance; SET LEGEND.Forbearance; RUN;
DATA Deferment; SET LEGEND.Deferment; RUN;
DATA IDR; SET LEGEND.IDR; RUN;
DATA BankruptcyNotice; SET LEGEND.BankruptcyNotice; RUN;
DATA BankruptcyProofOfClaim; SET LEGEND.BankruptcyProofOfClaim; RUN;
DATA DeathDischarge; SET LEGEND.DeathDischarge; RUN;
DATA ClosedSchoolApp; SET LEGEND.ClosedSchoolApp; RUN;
DATA BorrowerMail; SET LEGEND.BorrowerMail; RUN;
DATA DeathDischargeFSA; SET LEGEND.DeathDischargeFSA; RUN;
DATA ClosedSchoolFSA; SET LEGEND.ClosedSchoolFSA; RUN;
DATA Aging360AtServicer; SET LEGEND.Aging360AtServicer; RUN;
DATA Aging360SentToDMCS; SET LEGEND.Aging360SentToDMCS; RUN;
DATA Aging360NotAcceptedByDMCS; SET LEGEND.Aging360NotAcceptedByDMCS; RUN;

/*MACROS FOR INSERTING DATA INTO ServicerInventoryMetrics DATABASE*/
%MACRO INSERT_ARC_STYLE_METRIC(TABLE_NAME);
	PROC SQL;
		INSERT INTO 
			SQL.&TABLE_NAME
			(
				BF_SSN,
				WF_QUE,
				WF_SUB_QUE,
				WN_CTL_TSK,
				PF_REQ_ACT,
				WD_ACT_REQ,
				WC_STA_WQUE20,
				WF_LST_DTS_WQ20
			)
		SELECT
			BF_SSN,
			WF_QUE,
			WF_SUB_QUE,
			WN_CTL_TSK,
			PF_REQ_ACT,
			WD_ACT_REQ * 86400,
			WC_STA_WQUE20,
			WF_LST_DTS_WQ20
		FROM
			WORK.&TABLE_NAME;
	QUIT;
%MEND INSERT_ARC_STYLE_METRIC;

%MACRO INSERT_ARC_STYLE_METRIC2(TABLE_NAME);
	PROC SQL;
		INSERT INTO 
			SQL.&TABLE_NAME
			(
				BF_SSN,
				WF_QUE,
				WF_SUB_QUE,
				WN_CTL_TSK,
				PF_REQ_ACT,
				WD_ACT_REQ,
				WC_STA_WQUE20,
				WF_LST_DTS_WQ20,
				DEADLINE
			)
		SELECT
			BF_SSN,
			WF_QUE,
			WF_SUB_QUE,
			WN_CTL_TSK,
			PF_REQ_ACT,
			WD_ACT_REQ * 86400,
			WC_STA_WQUE20,
			WF_LST_DTS_WQ20,
			LX_ATY *86400
		FROM
			WORK.&TABLE_NAME;
	QUIT;
%MEND INSERT_ARC_STYLE_METRIC2;

%MACRO INSERT_FSA_DISCHARGE_METRIC(TABLE_NAME);
	PROC SQL;
		INSERT INTO
			SQL.&TABLE_NAME
			(
				BF_SSN,
				PF_REQ_ACT,
				LD_ATY_REQ_RCV
			)
		SELECT
			BF_SSN,
			PF_REQ_ACT,
			LD_ATY_REQ_RCV * 86400
		FROM
			WORK.&TABLE_NAME;
	QUIT;
%MEND INSERT_FSA_DISCHARGE_METRIC;

%MACRO INSERT_AGING_METRIC(TABLE_NAME);
	PROC SQL;
		INSERT INTO
			SQL.&TABLE_NAME
			(
				BF_SSN,
				LN_SEQ,
				AGING_DATE
			)
		SELECT
			BF_SSN,
			LN_SEQ,
			AGING_DATE * 86400
		FROM
			WORK.&TABLE_NAME;
	QUIT;
%MEND INSERT_AGING_METRIC;

%MACRO DELETE_TRANSACTIONAL_DATA(TABLE_NAME);
	PROC SQL;
		DELETE FROM
			SQL.&TABLE_NAME;
	QUIT;
%MEND DELETE_TRANSACTIONAL_DATA;

/* Delete data from transactional tables */
%DELETE_TRANSACTIONAL_DATA(Forbearance);
%DELETE_TRANSACTIONAL_DATA(Deferment);
%DELETE_TRANSACTIONAL_DATA(IDR);
%DELETE_TRANSACTIONAL_DATA(BankruptcyNotice);
%DELETE_TRANSACTIONAL_DATA(BankruptcyProofOfClaim);
%DELETE_TRANSACTIONAL_DATA(DeathDischarge);
%DELETE_TRANSACTIONAL_DATA(ClosedSchoolApp);
%DELETE_TRANSACTIONAL_DATA(BorrowerMail);
%DELETE_TRANSACTIONAL_DATA(DeathDischargeFSA);
%DELETE_TRANSACTIONAL_DATA(ClosedSchoolFSA);
%DELETE_TRANSACTIONAL_DATA(Aging360AtServicer);
%DELETE_TRANSACTIONAL_DATA(Aging360SentToDMCS);
%DELETE_TRANSACTIONAL_DATA(Aging360NotAcceptedByDMCS);

/* Move data to ServicerInventoryMetrics Database */
%INSERT_ARC_STYLE_METRIC(Forbearance);
%INSERT_ARC_STYLE_METRIC(Deferment);
%INSERT_ARC_STYLE_METRIC(IDR);
%INSERT_ARC_STYLE_METRIC(BankruptcyNotice);
%INSERT_ARC_STYLE_METRIC2(BankruptcyProofOfClaim);
%INSERT_ARC_STYLE_METRIC(DeathDischarge);
%INSERT_ARC_STYLE_METRIC(ClosedSchoolApp);
%INSERT_ARC_STYLE_METRIC(BorrowerMail);
%INSERT_FSA_DISCHARGE_METRIC(DeathDischargeFSA);
%INSERT_FSA_DISCHARGE_METRIC(ClosedSchoolFSA);
%INSERT_AGING_METRIC(Aging360AtServicer);
%INSERT_AGING_METRIC(Aging360SentToDMCS);
%INSERT_AGING_METRIC(Aging360NotAcceptedByDMCS);
