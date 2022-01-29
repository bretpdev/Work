USE CDW
GO

BEGIN TRY;
	BEGIN TRANSACTION;
		WITH DLQ AS
		(--delinquent population
			SELECT DISTINCT
				 PD10.DF_SPE_ACC_ID
				,LN10.BF_SSN
				,LN10.LN_SEQ
				,DW01.WC_DW_LON_STA
				,LN16.LN_DLQ_MAX AS DAYS_DLQ
			FROM 
				PD10_PRS_NME PD10
				INNER JOIN LN10_LON LN10
					ON PD10.DF_PRS_ID = LN10.BF_SSN
				INNER JOIN DW01_DW_CLC_CLU DW01
					ON LN10.BF_SSN = DW01.BF_SSN
					AND LN10.LN_SEQ = DW01.LN_SEQ
				INNER JOIN LN16_LON_DLQ_HST LN16
					ON LN10.BF_SSN = LN16.BF_SSN 
					AND LN10.LN_SEQ = LN16.LN_SEQ
				LEFT JOIN FB10_BR_FOR_REQ FB10 
					ON LN10.BF_SSN = FB10.BF_SSN
					AND FB10.LC_FOR_TYP IN ('13','28') --death, F-collection suspension
			WHERE
				LN10.LC_STP_PUR NOT IN ('P','Y') --identity theft pending/validated
				AND DW01.WC_DW_LON_STA NOT IN ('17','19','21','22') --death,disability,bankruptcy,PIF
				AND LN16.LC_STA_LON16 = '1'
				AND LN10.LA_CUR_PRI > 0.00
				AND LN10.LC_STA_LON10 = 'R'
				AND FB10.BF_SSN IS NULL
		),
		R2 AS
		(--DQACH arc population (Queue D7/01)
			SELECT DISTINCT
				DLQ.DF_SPE_ACC_ID
			FROM
				DLQ
				INNER JOIN LN83_EFT_TO_LON LN83
					ON LN83.BF_SSN = DLQ.BF_SSN
					AND LN83.LN_SEQ = DLQ.LN_SEQ
				LEFT JOIN AY10_BR_LON_ATY AY10
					ON LN83.BF_SSN = AY10.BF_SSN
					AND AY10.PF_REQ_ACT = 'DQACH'
				LEFT JOIN WQ20_TSK_QUE WQ20_D701 --flag for exclusion: open D7-01 queue task
					ON DLQ.BF_SSN = WQ20_D701.BF_SSN
					AND WQ20_D701.WF_QUE = 'D7'
					AND WQ20_D701.WF_SUB_QUE = '01'
					AND WQ20_D701.WC_STA_WQUE20 NOT IN ('C','X') --all other statuses indicate the queue is open
				LEFT JOIN WQ20_TSK_QUE WQ20_CQ --flag for exclusion: closed queue since ACH add date
					ON DLQ.BF_SSN = WQ20_CQ.BF_SSN
					AND WQ20_CQ.WF_QUE = 'D7'
					AND WQ20_CQ.WF_SUB_QUE = '01'
					AND WQ20_CQ.WC_STA_WQUE20 = 'C' --closed queue
					AND WQ20_CQ.WF_LST_DTS_WQ20 > LN83.LF_EFT_OCC_DTS --ACH add date
			WHERE 
				DLQ.DAYS_DLQ > 6
				AND LN83.LC_STA_LN83 = 'A'
				AND LN83.LC_EFT_SUS_REA = ''
				AND LN83.LF_EFT_OCC_DTS >= DATEADD(DAY,-30,CONVERT(DATE,GETDATE()))
				AND LN83.LF_EFT_OCC_DTS < DATEADD(DAY,1,CONVERT(DATE,GETDATE()))
				AND (--checks for arc added prior to ACH add date
						AY10.LD_ATY_RSP < LN83.LD_EFT_EFF_BEG
						OR AY10.LD_ATY_RSP IS NULL
					)
				AND WQ20_D701.BF_SSN IS NULL --excludes open D7-01 queue task
				AND WQ20_CQ.BF_SSN IS NULL --excludes closed queue since ACH add date
		),
		R3 AS
		(--DELQR arc population (Queue D8-01)
			SELECT DISTINCT
				DLQ.DF_SPE_ACC_ID
			FROM
				DLQ
				LEFT JOIN AY10_BR_LON_ATY AY10 --flag for exclusion: existing DELQR arc
					ON DLQ.BF_SSN = AY10.BF_SSN
					AND AY10.PF_REQ_ACT = 'DELQR'
					AND AY10.LD_ATY_RSP IS NULL
				LEFT JOIN WQ20_TSK_QUE WQ20 --flag for exclusion: open D8-01 queue task
					ON DLQ.BF_SSN = WQ20.BF_SSN
					AND WQ20.WF_QUE = 'D8'
					AND WQ20.WF_SUB_QUE = '01'
					AND WQ20.WC_STA_WQUE20 NOT IN ('C','X') --all other statuses indicate the queue is 'open' 
				--deferments:
				LEFT JOIN LN50_BR_DFR_APV LN50
					ON DLQ.BF_SSN = LN50.BF_SSN
					AND DLQ.LN_SEQ = LN50.LN_SEQ
					AND LN50.LC_STA_LON50 = 'A'
				LEFT JOIN DF10_BR_DFR_REQ DF10
					ON LN50.BF_SSN = DF10.BF_SSN
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
					AND DF10.LC_DFR_STA = 'A'
					AND DF10.LC_STA_DFR10 = 'A'
				--forbearances:
				LEFT JOIN LN60_BR_FOR_APV LN60
					ON DLQ.BF_SSN = LN60.BF_SSN
					AND DLQ.LN_SEQ = LN60.LN_SEQ
					AND LN60.LC_STA_LON60 = 'A'
				LEFT JOIN FB10_BR_FOR_REQ FB10
					ON LN60.BF_SSN = FB10.BF_SSN
					AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
					AND FB10.LC_FOR_STA = 'A'
					AND FB10.LC_STA_FOR10 = 'A'				
			WHERE
				DLQ.WC_DW_LON_STA IN ('04','05') --deferment,forbearance
				AND AY10.BF_SSN IS NULL --excludes existing DELQR arc
				AND WQ20.BF_SSN IS NULL --excludes open D8-01 queue task
				AND (--keep only active deferments or forbearances
						(
							LN50.BF_SSN IS NOT NULL
							AND DF10.BF_SSN IS NOT NULL
						)
						OR
						(
							LN60.BF_SSN IS NOT NULL
							AND FB10.BF_SSN IS NOT NULL	
						)
					)
		)
		INSERT INTO CLS.dbo.ArcAddProcessing
		(
			 ArcTypeId
			,AccountNumber
			,ARC
			,ScriptId
			,ProcessOn
			,IsReference
			,IsEndorser
			,ProcessingAttempts
			,CreatedAt
			,CreatedBy
		)
		SELECT DISTINCT
			 1 AS ArcTypeId --Atd22AllLoans
			,FINAL.AccountNumber
			,FINAL.ARC
			,'UTNWS07' AS ScriptId
			,GETDATE() AS ProcessOn
			,0 AS IsReference
			,0 AS IsEndorser
			,1 AS ProcessingAttempts
			,GETDATE() AS CreatedAt
			,SYSTEM_USER AS CreatedBy
		FROM
			(
				SELECT
					 R2.DF_SPE_ACC_ID AS AccountNumber
					,'DQACH' AS ARC
				FROM
					R2

				UNION

				SELECT
					 R3_FINAL.DF_SPE_ACC_ID AS AccountNumber
					,'DELQR' AS ARC
				FROM
					(--remove R2 borrowers from R3
						SELECT DF_SPE_ACC_ID FROM R3

						EXCEPT

						SELECT DF_SPE_ACC_ID FROM R2
					) R3_FINAL
			) FINAL
			LEFT JOIN CLS.dbo.ArcAddProcessing EXISTING_DATA
				ON EXISTING_DATA.AccountNumber = FINAL.AccountNumber
				AND EXISTING_DATA.ARC = FINAL.ARC
				AND EXISTING_DATA.ScriptId = 'UTNWS07'
				AND EXISTING_DATA.CreatedAt >= CONVERT(DATE,GETDATE())
				AND EXISTING_DATA.CreatedAt < DATEADD(DAY,1,CONVERT(DATE,GETDATE())) --not added today
		WHERE
			EXISTING_DATA.AccountNumber IS NULL
		;
	COMMIT TRANSACTION;
END TRY
BEGIN CATCH;
	ROLLBACK TRANSACTION;
	PRINT 'Transaction NOT committed';
	THROW;
END CATCH;

--TEST:
--select * from cls..ArcAddProcessing where ScriptId = 'UTNWS07' and ARC = 'DQACH' --R2
--select * from cls..ArcAddProcessing where ScriptId = 'UTNWS07' and ARC = 'DELQR' --R3
