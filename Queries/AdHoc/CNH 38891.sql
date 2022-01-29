--UTNWSXX.sql

USE CDW
GO

--BEGIN TRY;
--	BEGIN TRANSACTION;
		WITH DLQ AS
		(--delinquent population
			SELECT DISTINCT
				 PDXX.DF_SPE_ACC_ID
				,LNXX.BF_SSN
				,LNXX.LN_SEQ
				,DWXX.WC_DW_LON_STA
				,LNXX.LN_DLQ_MAX AS DAYS_DLQ

				,fbXX.LC_FOR_TYP
			FROM 
				PDXX_PRS_NME PDXX
				INNER JOIN LNXX_LON LNXX
					ON PDXX.DF_PRS_ID = LNXX.BF_SSN
				INNER JOIN DWXX_DW_CLC_CLU DWXX
					ON LNXX.BF_SSN = DWXX.BF_SSN
					AND LNXX.LN_SEQ = DWXX.LN_SEQ
				INNER JOIN LNXX_LON_DLQ_HST LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN 
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
				INNER JOIN FBXX_BR_FOR_REQ FBXX 
					ON LNXX.BF_SSN = FBXX.BF_SSN
			WHERE
				LNXX.LC_STP_PUR NOT IN ('P','Y') --identity theft pending/validated
				AND DWXX.WC_DW_LON_STA NOT IN ('XX','XX','XX','XX') --death,disability,bankruptcy,PIF
				AND LNXX.LC_STA_LONXX = 'X'
				AND LNXX.LA_CUR_PRI > X.XX
				AND LNXX.LC_STA_LONXX = 'R'
				AND FBXX.LC_FOR_TYP not IN ('XX','XX') --death, F-collection suspension
				AND FBXX.LC_FOR_STA = 'A'
				AND FBXX.LC_STA_FORXX = 'A'

				and pdXX.DF_SPE_ACC_ID = 'XXXXXXXXXX'
		),
		RX AS
		(--DQACH arc population (Queue DX/XX)
			SELECT DISTINCT
				DLQ.DF_SPE_ACC_ID
			FROM
				DLQ
				INNER JOIN LNXX_EFT_TO_LON LNXX
					ON LNXX.BF_SSN = DLQ.BF_SSN
					AND LNXX.LN_SEQ = DLQ.LN_SEQ
				LEFT JOIN AYXX_BR_LON_ATY AYXX
					ON LNXX.BF_SSN = AYXX.BF_SSN
					AND AYXX.PF_REQ_ACT = 'DQACH'
				LEFT JOIN WQXX_TSK_QUE WQXX_DXXX --flag for exclusion: open DX-XX queue task
					ON DLQ.BF_SSN = WQXX_DXXX.BF_SSN
					AND WQXX_DXXX.WF_QUE = 'DX'
					AND WQXX_DXXX.WF_SUB_QUE = 'XX'
					AND WQXX_DXXX.WC_STA_WQUEXX NOT IN ('C','X') --all other statuses indicate the queue is open
				LEFT JOIN WQXX_TSK_QUE WQXX_CQ --flag for exclusion: closed queue since ACH add date
					ON DLQ.BF_SSN = WQXX_CQ.BF_SSN
					AND WQXX_CQ.WF_QUE = 'DX'
					AND WQXX_CQ.WF_SUB_QUE = 'XX'
					AND WQXX_CQ.WC_STA_WQUEXX = 'C' --closed queue
					AND WQXX_CQ.WF_LST_DTS_WQXX > LNXX.LF_EFT_OCC_DTS --ACH add date
			WHERE 
				DLQ.DAYS_DLQ > X
				AND LNXX.LC_STA_LNXX = 'A'
				AND LNXX.LC_EFT_SUS_REA = ''
				--AND LNXX.LF_EFT_OCC_DTS >= DATEADD(DAY,-XX,CONVERT(DATE,GETDATE()))
				--AND LNXX.LF_EFT_OCC_DTS < DATEADD(DAY,X,CONVERT(DATE,GETDATE()))
				AND (--checks for arc added prior to ACH add date
						AYXX.LD_ATY_RSP < LNXX.LD_EFT_EFF_BEG
						OR AYXX.LD_ATY_RSP IS NULL
					)
				AND WQXX_DXXX.BF_SSN IS NULL --excludes open DX-XX queue task
				AND WQXX_CQ.BF_SSN IS NULL --excludes closed queue since ACH add date
		),
		RX AS
		(--DELQR arc population (Queue DX-XX)
			SELECT DISTINCT
				DLQ.DF_SPE_ACC_ID
			FROM
				DLQ
				LEFT JOIN AYXX_BR_LON_ATY AYXX --flag for exclusion: existing DELQR arc
					ON DLQ.BF_SSN = AYXX.BF_SSN
					AND AYXX.PF_REQ_ACT = 'DELQR'
					AND AYXX.LD_ATY_RSP IS NULL
				LEFT JOIN WQXX_TSK_QUE WQXX --flag for exclusion: open DX-XX queue task
					ON DLQ.BF_SSN = WQXX.BF_SSN
					AND WQXX.WF_QUE = 'DX'
					AND WQXX.WF_SUB_QUE = 'XX'
					AND WQXX.WC_STA_WQUEXX NOT IN ('C','X') --all other statuses indicate the queue is 'open' 
				--deferments:
				LEFT JOIN LNXX_BR_DFR_APV LNXX
					ON DLQ.BF_SSN = LNXX.BF_SSN
					AND DLQ.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = 'A'
				LEFT JOIN DFXX_BR_DFR_REQ DFXX
					ON LNXX.BF_SSN = DFXX.BF_SSN
					AND LNXX.LF_DFR_CTL_NUM = DFXX.LF_DFR_CTL_NUM
					AND DFXX.LC_DFR_STA = 'A'
					AND DFXX.LC_STA_DFRXX = 'A'
				--forbearances:
				LEFT JOIN LNXX_BR_FOR_APV LNXX
					ON DLQ.BF_SSN = LNXX.BF_SSN
					AND DLQ.LN_SEQ = LNXX.LN_SEQ
					AND LNXX.LC_STA_LONXX = 'A'
				LEFT JOIN FBXX_BR_FOR_REQ FBXX
					ON LNXX.BF_SSN = FBXX.BF_SSN
					AND LNXX.LF_FOR_CTL_NUM = FBXX.LF_FOR_CTL_NUM
					AND FBXX.LC_FOR_STA = 'A'
					AND FBXX.LC_STA_FORXX = 'A'				
			WHERE
				DLQ.WC_DW_LON_STA IN ('XX','XX') --deferment,forbearance
				AND AYXX.BF_SSN IS NULL --excludes existing DELQR arc
				AND WQXX.BF_SSN IS NULL --excludes open DX-XX queue task
				AND (--keep only active deferments or forbearances
						(
							LNXX.BF_SSN IS NOT NULL
							AND DFXX.BF_SSN IS NOT NULL
						)
						OR
						(
							LNXX.BF_SSN IS NOT NULL
							AND FBXX.BF_SSN IS NOT NULL	
						)
					)
		)
		--INSERT INTO CLS.dbo.ArcAddProcessing
		--(
		--	 ArcTypeId
		--	,AccountNumber
		--	,ARC
		--	,ScriptId
		--	,ProcessOn
		--	,IsReference
		--	,IsEndorser
		--	,ProcessingAttempts
		--	,CreatedAt
		--	,CreatedBy
		--)
		SELECT DISTINCT
			 X AS ArcTypeId --AtdXXAllLoans
			,FINAL.AccountNumber
			,FINAL.ARC
			,'UTNWSXX' AS ScriptId
			,GETDATE() AS ProcessOn
			,X AS IsReference
			,X AS IsEndorser
			,X AS ProcessingAttempts
			,GETDATE() AS CreatedAt
			,SYSTEM_USER AS CreatedBy
		FROM
			(
				SELECT
					 RX.DF_SPE_ACC_ID AS AccountNumber
					,'DQACH' AS ARC
				FROM
					RX

				UNION

				SELECT
					 RX_FINAL.DF_SPE_ACC_ID AS AccountNumber
					,'DELQR' AS ARC
				FROM
					(--remove RX borrowers from RX
						SELECT DF_SPE_ACC_ID FROM RX

						EXCEPT

						SELECT DF_SPE_ACC_ID FROM RX
					) RX_FINAL
			) FINAL
	--		LEFT JOIN CLS.dbo.ArcAddProcessing EXISTING_DATA
	--			ON EXISTING_DATA.AccountNumber = FINAL.AccountNumber
	--			AND EXISTING_DATA.ARC = FINAL.ARC
	--			AND EXISTING_DATA.ScriptId = 'UTNWSXX'
	--			AND EXISTING_DATA.CreatedAt >= CONVERT(DATE,GETDATE())
	--			AND EXISTING_DATA.CreatedAt < DATEADD(DAY,X,CONVERT(DATE,GETDATE())) --not added today
	--	WHERE
	--		EXISTING_DATA.AccountNumber IS NULL
	--	;
	--COMMIT TRANSACTION;
--END TRY
--BEGIN CATCH;
--	ROLLBACK TRANSACTION;
--	PRINT 'Transaction NOT committed';
--	THROW;
--END CATCH;

--TEST:
--select * from cls..ArcAddProcessing where ScriptId = 'UTNWSXX' and ARC = 'DQACH' --RX
--select * from cls..ArcAddProcessing where ScriptId = 'UTNWSXX' and ARC = 'DELQR' --RX