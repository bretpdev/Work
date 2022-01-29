--MOPYMTUP.sql MONITOR PAYMENT INCREASES

--TEST (also uncomment accompanying line 63):
--declare @acct varchar(10) = '0123456789';
--declare @ssn varchar(9) = (select df_prs_id from cdw..pd10_prs_nme where df_spe_acc_id = @acct);

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE	@DAYSAG0 SMALLINT = -7, --TEST = -50, LIVE = -7
		@TODAY DATE = CONVERT(DATE, GETDATE()),
		@ArcTypeId TINYINT = 1,
		@ARC VARCHAR(5) = 'OVRPS',
		@ScriptID VARCHAR(8) = 'MoPymtUp';

BEGIN TRY
	BEGIN TRANSACTION

	;WITH ALL_LOANS AS
	(--get all loans for base pop
		SELECT 
			 Ranking.BF_SSN
			,Ranking.LN_SEQ
			,Ranking.redisclosed_LD_CRT_LON65
			,Ranking.redisclosed_LD_RPS_1_PAY_DU
			,Ranking.IsRedisclosed
		FROM
			(--ranks each disclosure type
				SELECT DISTINCT
					 LN10.BF_SSN
					,LN10.LN_SEQ
					,IIF(BasePop.LN_SEQ = LN10.LN_SEQ, BasePop.LD_CRT_LON65, NULL) AS redisclosed_LD_CRT_LON65
					,IIF(BasePop.LN_SEQ = LN10.LN_SEQ, BasePop.LD_RPS_1_PAY_DU, NULL) AS redisclosed_LD_RPS_1_PAY_DU
					,IIF(BasePop.LN_SEQ = LN10.LN_SEQ, 1, 0) AS IsRedisclosed
					,RANK() OVER(PARTITION BY LN10.BF_SSN, LN10.LN_SEQ ORDER BY IIF(BasePop.LN_SEQ = LN10.LN_SEQ, 1, 0) DESC) AS IsRedisclosedRank
				FROM
					CDW..LN10_LON LN10
					INNER JOIN 
					(--base pop: redisclosed within previous week
						SELECT DISTINCT
							 LN65.BF_SSN
							,LN65.LN_SEQ
							,LN65.LD_CRT_LON65 --repayment schedule create date
							,RS10.LD_RPS_1_PAY_DU --new RPS's first due date
						FROM
							CDW..LN65_LON_RPS LN65
							INNER JOIN CDW..RS10_BR_RPD RS10
								ON LN65.BF_SSN = RS10.BF_SSN
								AND LN65.LN_RPS_SEQ = RS10.LN_RPS_SEQ
							INNER JOIN CDW..LN85_LON_ATY LN85
								ON LN65.BF_SSN = LN85.BF_SSN
								AND LN65.LN_SEQ = LN85.LN_SEQ
							LEFT JOIN CDW..AY10_BR_LON_ATY AY10 --exclude OVRPS arcs on/after current RPS create date
								ON LN85.BF_SSN = AY10.BF_SSN
								AND AY10.LC_STA_ACTY10 = 'A'
								AND AY10.PF_REQ_ACT = 'OVRPS'
								AND AY10.LD_ATY_REQ_RCV > LN65.LD_CRT_LON65
						WHERE
							AY10.BF_SSN IS NULL --excludes OVRPS arcs on/after current RPS create date
							AND LN65.LD_CRT_LON65 > DATEADD(DAY, @DAYSAG0, @TODAY) --within previous week
							AND LN65.LD_CRT_LON65 < @TODAY
							AND LN65.LC_STA_LON65 = 'A' --active
							AND RS10.LF_USR_RPS_REQ NOT LIKE 'UT%'
							--and ln65.bf_ssn = @ssn --test
					) BasePop
						ON LN10.BF_SSN = BasePop.BF_SSN
				WHERE
					LN10.LA_CUR_PRI > 0.00 --open
					AND LN10.LC_STA_LON10 = 'R' --released
			) Ranking
		WHERE
			IsRedisclosedRank = 1 --filters out false positives
	)
	,PreviousMonthlyPaymentCalc AS
	(
		SELECT
			 UNIONS.BF_SSN
			,SUM(UNIONS.LA_BIL_CUR_DU) AS PreviousMonthlyPayment
		FROM
			(	--Redisclosed Loans: calculate previous monthly payment
				SELECT DISTINCT
					 LN80.BF_SSN
					,LN80.LA_BIL_CUR_DU
					,LN80.LN_SEQ
					,LN80.LD_BIL_DU_LON
					,LN80_MAX.IsRedisclosed
				FROM
					CDW..LN80_LON_BIL_CRF LN80
					INNER JOIN 
					(--get max future due date
						SELECT
							 LN80.BF_SSN
							,LN80.LN_SEQ
							,MAX(LN80.LD_BIL_DU_LON) AS max_LD_BIL_DU_LON
							,AL.IsRedisclosed
						FROM
							CDW..LN80_LON_BIL_CRF LN80
							INNER JOIN ALL_LOANS AL
								ON LN80.BF_SSN = AL.BF_SSN
								AND LN80.LN_SEQ = AL.LN_SEQ
						WHERE
							AL.IsRedisclosed = 1
							AND LN80.LD_BIL_DU_LON < AL.redisclosed_LD_RPS_1_PAY_DU
							AND LN80.LD_BIL_DU_LON < DATEADD(DAY, 30, @TODAY) --due date no more than a month in the future
							AND LN80.LD_BIL_DU_LON > DATEADD(DAY, -60, @TODAY) --due date no more than 2 months in the past
							AND LN80.LC_STA_LON80 = 'A' --active
							AND LN80.LC_BIL_TYP_LON = 'P' --most recent installment
						GROUP BY
							 LN80.BF_SSN
							,LN80.LN_SEQ
							,AL.IsRedisclosed
					) LN80_MAX
						ON LN80.BF_SSN = LN80_MAX.BF_SSN
						AND LN80.LN_SEQ = LN80_MAX.LN_SEQ
						AND LN80.LD_BIL_DU_LON = LN80_MAX.max_LD_BIL_DU_LON

				UNION ALL

				--Non-Redisclosed Loans: calculate previous monthly payment
				SELECT DISTINCT
					 LN80.BF_SSN
					,LN80.LA_BIL_CUR_DU
					,LN80.LN_SEQ
					,LN80.LD_BIL_DU_LON
					,LN80_MAX.IsRedisclosed
				FROM
					CDW..LN80_LON_BIL_CRF LN80
					INNER JOIN 
					(--get max future due date
						SELECT DISTINCT
							 AL.BF_SSN
							,AL.LN_SEQ
							,AL.IsRedisclosed
							,MAX(LN80.LD_BIL_DU_LON) AS max_LD_BIL_DU_LON
						FROM
							ALL_LOANS AL
							INNER JOIN CDW..LN80_LON_BIL_CRF LN80
								ON LN80.BF_SSN = AL.BF_SSN
								AND LN80.LN_SEQ = AL.LN_SEQ
						WHERE
							AL.IsRedisclosed = 0
							AND LN80.LD_BIL_DU_LON < DATEADD(DAY, 30, @TODAY) --due date no more than a month in the future
							AND LN80.LD_BIL_DU_LON > DATEADD(DAY, -60, @TODAY) --due date no more than 2 months in the past
							AND LN80.LC_STA_LON80 = 'A' --active
							AND LN80.LC_BIL_TYP_LON = 'P' --most recent installment
						GROUP BY
							 AL.BF_SSN
							,AL.LN_SEQ
							,AL.IsRedisclosed
					) LN80_MAX
						ON LN80.BF_SSN = LN80_MAX.BF_SSN
						AND LN80.LN_SEQ = LN80_MAX.LN_SEQ
						AND LN80.LD_BIL_DU_LON = LN80_MAX.max_LD_BIL_DU_LON
			) UNIONS
		GROUP BY
			UNIONS.BF_SSN
	)
	INSERT INTO CLS..ArcAddProcessing
	(
		ArcTypeId
		,AccountNumber
		,ARC
		,ScriptId
		,ProcessOn
		,Comment
		,IsReference
		,IsEndorser
		,ProcessingAttempts
		,CreatedAt
		,CreatedBy
	)
	SELECT DISTINCT
		 @ArcTypeId
		,GT100.AccountNumber
		,@ARC
		,@ScriptID
		,GETDATE() AS ProcessOn
		,CONCAT('Contact borrower to discuss payment increase from $', GT100.PreviousMonthlyPayment, ' to $', GT100.CurrentMonthlyPayment) AS Comment
		,0 AS IsReference
		,0 AS IsEndorser
		,0 AS ProcessingAttempts
		,GETDATE() AS CreatedAt
		,SYSTEM_USER AS CreatedBy
	FROM
		(
			SELECT DISTINCT
				 IVR.AccountNumber
				,IVR.MonthlyPaymentAmount AS CurrentMonthlyPayment
				,PMPC.PreviousMonthlyPayment
				,IIF((IVR.MonthlyPaymentAmount - PMPC.PreviousMonthlyPayment) > 100, 1, 0) AS IsGreaterThan100
			FROM
				CDW.calc.IvrData IVR
				INNER JOIN PreviousMonthlyPaymentCalc PMPC
					ON IVR.SSN = PMPC.BF_SSN
		) GT100
		LEFT JOIN CLS..ArcAddProcessing EXISTING_DATA
			ON EXISTING_DATA.ArcTypeId = @ArcTypeId
			AND EXISTING_DATA.AccountNumber = GT100.AccountNumber
			AND EXISTING_DATA.ARC = @ARC
			AND EXISTING_DATA.ScriptId = @ScriptID
			AND EXISTING_DATA.CreatedBy = SYSTEM_USER
			AND EXISTING_DATA.CreatedAt >= @TODAY
			AND EXISTING_DATA.CreatedAt < DATEADD(DAY, 1, @TODAY)
	WHERE
		GT100.IsGreaterThan100 = 1
		AND EXISTING_DATA.AccountNumber IS NULL;

	COMMIT TRANSACTION;
END TRY
BEGIN CATCH
	PRINT 'MOPYMTUP.sql transaction NOT committed.';
	ROLLBACK TRANSACTION;
	THROW;
END CATCH;
