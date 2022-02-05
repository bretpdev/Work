CREATE PROCEDURE [pridrcrp].[AddMonthsInRepayment]
(
	@BorrowerInformationId INT,
	@LoanAddDate DATE,
	@DueDatePeriods DueDatePeriods READONLY
)
AS

DECLARE @StartDate DATE = (SELECT BI.FirstPayDue FROM CLS.pridrcrp.BorrowerInformation BI WHERE BI.BorrowerInformationId = @BorrowerInformationId)
DECLARE	@Day INT = 
(
	SELECT 
		CASE 
			WHEN 
				DDP.[Day] > DATEPART(DAY, DDP.BeginDate)
				AND DATEPART(MONTH, DDP.BeginDate) = DATEPART(MONTH, @StartDate) 
				AND DATEPART(YEAR, DDP.BeginDate) = DATEPART(YEAR, @StartDate) 
				THEN DDP.[Day] 
			WHEN	
				DATEPART(MONTH, DDP.BeginDate) != DATEPART(MONTH, @StartDate) 
				OR DATEPART(YEAR, DDP.BeginDate) != DATEPART(YEAR, @StartDate) 
				THEN DDP.[Day]
			ELSE DATEPART(DAY, @StartDate)
		END
	FROM 
		@DueDatePeriods DDP 
	WHERE 
		@StartDate BETWEEN DDP.BeginDate AND DDP.EndDate
)

IF @Day IS NULL
	BEGIN
		SET @Day = DATEPART(DAY, @StartDate)
	END

SET @StartDate = DATEFROMPARTS(DATEPART(YEAR, @StartDate), DATEPART(MONTH, @StartDate), @Day) 
DECLARE @BeginMonth DATE = DATEFROMPARTS(DATEPART(YEAR, DATEADD(MONTH, 1, @StartDate)), DATEPART(MONTH, DATEADD(MONTH, 1, @StartDate)), 1),
	@EndMonth DATE = DATEADD(DAY, -1, DATEADD(MONTH, 1, DATEFROMPARTS(DATEPART(YEAR, DATEADD(MONTH, 1, @StartDate)), DATEPART(MONTH, DATEADD(MONTH, 1, @StartDate)), 1) )),
	@ROWCOUNT INT = 0,
	@ERROR INT = 0

WHILE @StartDate <= @LoanAddDate
BEGIN
	INSERT INTO [pridrcrp].MonthsInRepayment(BorrowerInformationId, AmountDue,[Date],RepaymentPlanTypeId,CoveredByEHD,CoveredByDefFor)
	SELECT
		BI.BorrowerInformationId,
		PaymentAmountPeriod.PaymentAmount,
		@StartDate,
		RepaymentPlanPeriod.RepaymentPlanTypeId,
		CAST(MAX(CAST(ISNULL(EHD.Covered, 0) AS INT)) AS BIT),
		CAST(MAX(CAST(ISNULL(DF.Covered, 0) AS INT)) AS BIT)
	FROM
		CLS.pridrcrp.BorrowerInformation BI
		LEFT JOIN 
		(
			SELECT
				RPC.BorrowerInformationId,
				RPC.EffectiveDate AS StartDate,
				RPT.RepaymentPlanTypeId,
				CASE 
					WHEN NEXTRPC.RepaymentPlanChangeId IS NULL THEN '2099-01-01'
					WHEN NEXTRPC.EffectiveDate = RPC.EffectiveDate THEN NEXTRPC.EffectiveDate
					ELSE DATEADD(DAY, -1, NEXTRPC.EffectiveDate)
				END AS EndDate,
				ROW_NUMBER() OVER (PARTITION BY RPC.BorrowerInformationId ORDER BY RPC.RepaymentPlanChangeId DESC) AS PlanPriority
			FROM
				[pridrcrp].[RepaymentPlanChanges] RPC
				INNER JOIN [pridrcrp].[RepaymentPlanTypes] RPT
					ON RPT.RepaymentPlanType = RPC.PlanType
				LEFT JOIN
				(	
					SELECT	
						RPC.RepaymentPlanChangeId,
						MIN(NEXTRPC.EffectiveDate) AS EffectiveDate
					FROM
						[pridrcrp].[RepaymentPlanChanges] RPC
						INNER JOIN [pridrcrp].[RepaymentPlanChanges] NEXTRPC
							ON NEXTRPC.BorrowerInformationId = RPC.BorrowerInformationId
							AND NEXTRPC.RepaymentPlanChangeId > RPC.RepaymentPlanChangeId
							AND NEXTRPC.EffectiveDate >= RPC.EffectiveDate 
					WHERE
						RPC.InactivatedAt IS NULL
						AND NEXTRPC.InactivatedAt IS NULL
					GROUP BY
						RPC.RepaymentPlanChangeId
				) NEXTRPC
					ON NEXTRPC.RepaymentPlanChangeId = RPC.RepaymentPlanChangeId
			WHERE 
				RPC.InactivatedAt IS NULL
				AND @StartDate BETWEEN RPC.EffectiveDate AND 
					CASE 
						WHEN NEXTRPC.RepaymentPlanChangeId IS NULL THEN '2099-01-01'
						WHEN NEXTRPC.EffectiveDate = RPC.EffectiveDate THEN NEXTRPC.EffectiveDate
						ELSE DATEADD(DAY, -1, NEXTRPC.EffectiveDate)
					END
		) RepaymentPlanPeriod
			ON RepaymentPlanPeriod.BorrowerInformationId = BI.BorrowerInformationId
			AND PlanPriority = 1
		--The payment amount changes can happen on the same day, so the max effective date record needs to be
		--taken in addition to arranging the payment amount change as a period between records.
		LEFT JOIN 
		(
			SELECT
				PAC.BorrowerInformationId,
				PAC.EffectiveDate AS StartDate,
				CASE 
					WHEN NEXTPAC.PaymentAmountChangeId IS NULL THEN '2099-01-01'
					WHEN NEXTPAC.EffectiveDate = PAC.EffectiveDate THEN NEXTPAC.EffectiveDate
					ELSE DATEADD(DAY, -1, NEXTPAC.EffectiveDate)
				END AS EndDate,
				PAC.PaymentAmount
			FROM
				[pridrcrp].[PaymentAmountChanges] PAC
				INNER JOIN
				(
					SELECT
						MAX(PAC.PaymentAmountChangeId) AS PaymentAmountChangeId,
						PAC.BorrowerInformationId,
						PAC.EffectiveDate
					FROM
						[pridrcrp].[PaymentAmountChanges] PAC
						LEFT JOIN
						(	
							SELECT	
								PAC.PaymentAmountChangeId,
								PAC.BorrowerInformationId,
								MIN(NEXTPAC.EffectiveDate) AS EffectiveDate
							FROM
								[pridrcrp].[PaymentAmountChanges] PAC
								INNER JOIN [pridrcrp].[PaymentAmountChanges] NEXTPAC
									ON NEXTPAC.BorrowerInformationId = PAC.BorrowerInformationId
									--AND NEXTPAC.PaymentAmountChangeId > PAC.PaymentAmountChangeId
									AND NEXTPAC.EffectiveDate > PAC.EffectiveDate 
									AND NEXTPAC.PaymentAmountChangeId != PAC.PaymentAmountChangeId
							WHERE
								PAC.InactivatedAt IS NULL
								AND NEXTPAC.InactivatedAt IS NULL
							GROUP BY
								PAC.PaymentAmountChangeId,
								PAC.BorrowerInformationId
						) NEXTPAC
							ON NEXTPAC.PaymentAmountChangeId = PAC.PaymentAmountChangeId
							AND NEXTPAC.BorrowerInformationId = PAC.BorrowerInformationId
					WHERE 
						PAC.InactivatedAt IS NULL
					GROUP BY
						PAC.BorrowerInformationId,
						PAC.EffectiveDate
				) PAC_MAX
					ON PAC_MAX.PaymentAmountChangeId = PAC.PaymentAmountChangeId
					AND PAC_MAX.BorrowerInformationId = PAC.BorrowerInformationId
				LEFT JOIN
				(	
					SELECT	
						PAC.PaymentAmountChangeId,
						PAC.BorrowerInformationId,
						MIN(NEXTPAC.EffectiveDate) AS EffectiveDate
					FROM
						[pridrcrp].[PaymentAmountChanges] PAC
						INNER JOIN [pridrcrp].[PaymentAmountChanges] NEXTPAC
							ON NEXTPAC.BorrowerInformationId = PAC.BorrowerInformationId
							--AND NEXTPAC.PaymentAmountChangeId > PAC.PaymentAmountChangeId
							AND NEXTPAC.EffectiveDate > PAC.EffectiveDate 
							AND NEXTPAC.PaymentAmountChangeId != PAC.PaymentAmountChangeId
					WHERE
						PAC.InactivatedAt IS NULL
						AND NEXTPAC.InactivatedAt IS NULL
					GROUP BY
						PAC.BorrowerInformationId,
						PAC.PaymentAmountChangeId
				) NEXTPAC
					ON NEXTPAC.PaymentAmountChangeId = PAC_MAX.PaymentAmountChangeId
					AND NEXTPAC.BorrowerInformationId = PAC_MAX.BorrowerInformationId
			WHERE
				PAC.InactivatedAt IS NULL
		) PaymentAmountPeriod
			ON PaymentAmountPeriod.BorrowerInformationId = BI.BorrowerInformationId
			AND @StartDate BETWEEN PaymentAmountPeriod.StartDate AND PaymentAmountPeriod.EndDate
		--Pay Due Date Covered By Generic Deferment/Forbearance
		LEFT JOIN
		(
			SELECT
				BI.BorrowerInformationId,
				CAST(CASE WHEN Covered IS NULL THEN 0 ELSE Covered END AS BIT) AS Covered
			FROM
				CLS.pridrcrp.BorrowerInformation BI
				LEFT JOIN
				(
					SELECT
						BI.BorrowerInformationId,
						MAX(CASE WHEN LN50.BF_SSN IS NULL THEN 0 ELSE 1 END) AS Covered
					FROM
						CLS.pridrcrp.BorrowerInformation BI
						INNER JOIN CLS.pridrcrp.Disbursements D
							ON D.BorrowerInformationId = BI.BorrowerInformationId
						INNER JOIN CDW..FS10_DL_LON FS10
							ON FS10.BF_SSN = BI.Ssn
							AND FS10.LF_FED_AWD + RIGHT('000' + CAST(FS10.LN_FED_AWD_SEQ AS VARCHAR(3)), 3) = BI.Ssn + 
								CASE WHEN D.LoanType IN('PLUS','GPLUS','CON PLUS') THEN 'P'
										WHEN D.LoanType IN('SUB','CON SUB') THEN 'S'
										WHEN D.LoanType IN('UNSUB','CON USUB') THEN 'U'
								END + D.LoanId		
						LEFT JOIN CDW..LN50_BR_DFR_APV LN50
							ON FS10.BF_SSN = LN50.BF_SSN
							AND FS10.LN_SEQ = LN50.LN_SEQ	
							AND LN50.LC_STA_LON50 = 'A'
							AND LN50.LC_DFR_RSP != '003'
							AND @StartDate BETWEEN LN50.LD_DFR_BEG AND LN50.LD_DFR_END					
					WHERE
						BI.BorrowerInformationId = @BorrowerInformationId
						AND BI.DeletedAt IS NULL
					GROUP BY
						BI.BorrowerInformationId

					UNION
	
					SELECT
						BI.BorrowerInformationId,
						MAX(CASE WHEN LN60.BF_SSN IS NULL THEN 0 ELSE 1 END) AS Covered
					FROM
						CLS.pridrcrp.BorrowerInformation BI
						INNER JOIN CLS.pridrcrp.Disbursements D
							ON D.BorrowerInformationId = BI.BorrowerInformationId
						INNER JOIN CDW..FS10_DL_LON FS10
							ON FS10.BF_SSN = BI.Ssn
							AND FS10.LF_FED_AWD + RIGHT('000' + CAST(FS10.LN_FED_AWD_SEQ AS VARCHAR(3)), 3) = BI.Ssn + 
								CASE WHEN D.LoanType IN('PLUS','GPLUS','CON PLUS') THEN 'P'
										WHEN D.LoanType IN('SUB','CON SUB') THEN 'S'
										WHEN D.LoanType IN('UNSUB','CON USUB') THEN 'U'
								END + D.LoanId		
						LEFT JOIN CDW..LN60_BR_FOR_APV LN60
							ON FS10.BF_SSN = LN60.BF_SSN
							AND FS10.LN_SEQ = LN60.LN_SEQ	
							AND LN60.LC_STA_LON60 = 'A'
							AND LN60.LC_FOR_RSP != '003'	
							AND @StartDate BETWEEN LN60.LD_FOR_BEG AND LN60.LD_FOR_END				
					WHERE
						BI.BorrowerInformationId = @BorrowerInformationId
						AND BI.DeletedAt IS NULL
					GROUP BY
						BI.BorrowerInformationId
			) DefFor
				ON BI.BorrowerInformationId = DefFor.BorrowerInformationId
			WHERE
				BI.DeletedAt IS NULL
		) DF
			ON BI.BorrowerInformationId = DF.BorrowerInformationId
		LEFT JOIN
		(
			SELECT	
				BI.BorrowerInformationId,
				MAX(CASE WHEN LN50.BF_SSN IS NULL THEN 0 ELSE 1 END) AS Covered
			FROM
				CLS.pridrcrp.BorrowerInformation BI
				INNER JOIN CLS.pridrcrp.Disbursements D
					ON D.BorrowerInformationId = BI.BorrowerInformationId
				INNER JOIN CDW..FS10_DL_LON FS10
					ON FS10.BF_SSN = BI.Ssn
					AND FS10.LF_FED_AWD + RIGHT('000' + CAST(FS10.LN_FED_AWD_SEQ AS VARCHAR(3)), 3) = BI.Ssn + 
						CASE WHEN D.LoanType IN('PLUS','GPLUS','CON PLUS') THEN 'P'
								WHEN D.LoanType IN('SUB','CON SUB') THEN 'S'
								WHEN D.LoanType IN('UNSUB','CON USUB') THEN 'U'
						END + D.LoanId	
				LEFT JOIN CDW..DF10_BR_DFR_REQ DF10
					ON DF10.BF_SSN = FS10.BF_SSN
					AND DF10.LC_DFR_STA = 'A'
					AND DF10.LC_STA_DFR10 = 'A'
					AND DF10.LC_DFR_TYP = '29'
				LEFT JOIN CDW..LN50_BR_DFR_APV LN50
					ON LN50.BF_SSN = DF10.BF_SSN
					AND LN50.LN_SEQ = FS10.LN_SEQ
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
					AND LN50.LC_STA_LON50 = 'A'
					AND LN50.LC_DFR_RSP != '003' --exclude denied deferments
					AND @StartDate BETWEEN LN50.LD_DFR_BEG AND LN50.LD_DFR_END	
			WHERE
				CAST(LN50.LD_DFR_BEG AS DATE) <= @LoanAddDate
				AND BI.DeletedAt IS NULL
			GROUP BY
				BI.BorrowerInformationId
		) EHD
			ON EHD.BorrowerInformationId = BI.BorrowerInformationId
		--Make sure the record is not a duplicate
		LEFT JOIN pridrcrp.MonthsInRepayment MIR
			ON MIR.BorrowerInformationId = BI.BorrowerInformationId
			AND MIR.AmountDue = PaymentAmountPeriod.PaymentAmount
			AND MIR.[Date] = @StartDate 
			AND MIR.RepaymentPlanTypeId = RepaymentPlanPeriod.RepaymentPlanTypeId
			AND MIR.CoveredByEHD = ISNULL(EHD.Covered, 0)
			AND MIR.CoveredByDefFor = ISNULL(DF.Covered, 0)
			AND MIR.InactivatedAt IS NULL
	WHERE
		MIR.BorrowerInformationId IS NULL
		AND BI.BorrowerInformationId = @BorrowerInformationId
		AND BI.DeletedAt IS NULL
	GROUP BY
		BI.BorrowerInformationId,
		PaymentAmountPeriod.PaymentAmount,
		RepaymentPlanPeriod.RepaymentPlanTypeId

	SELECT @ERROR = @ERROR + @@ERROR, @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT

	--Get the day of the month payments are due, if one is available
	SET @Day = 
	(
		SELECT 
			CASE 
				WHEN 
					DDP.[Day] > DATEPART(DAY, DDP.BeginDate)
					AND DATEPART(MONTH, DDP.BeginDate) = DATEPART(MONTH, DATEADD(MONTH, 1, @StartDate))
					AND DATEPART(YEAR, DDP.BeginDate) = DATEPART(YEAR, DATEADD(MONTH, 1, @StartDate)) 
					THEN DDP.[Day] 
				WHEN	
					DATEPART(MONTH, DDP.BeginDate) != DATEPART(MONTH, DATEADD(MONTH, 1, @StartDate))
					OR DATEPART(YEAR, DDP.BeginDate) != DATEPART(YEAR, DATEADD(MONTH, 1, @StartDate)) 
					THEN DDP.[Day]
				ELSE DATEPART(DAY, @StartDate)
			END
		FROM 
			@DueDatePeriods DDP 
		WHERE 
			DATEADD(MONTH, 1, @StartDate) BETWEEN DDP.BeginDate AND DDP.EndDate
	)

	IF @Day IS NULL
	BEGIN
		SET @Day = DATEPART(DAY, @StartDate)
	END

	--Set the start date equal to the 
	SET @StartDate = DATEFROMPARTS(DATEPART(YEAR, DATEADD(MONTH, 1, @StartDate)), DATEPART(MONTH, DATEADD(MONTH, 1, @StartDate)), @Day) 
	SET @BeginMonth = DATEFROMPARTS(DATEPART(YEAR, DATEADD(MONTH, 1, @StartDate)), DATEPART(MONTH, DATEADD(MONTH, 1, @StartDate)), 1)
	SET @EndMonth = DATEADD(DAY, -1, DATEADD(MONTH, 1, DATEFROMPARTS(DATEPART(YEAR, DATEADD(MONTH, 1, @StartDate)), DATEPART(MONTH, DATEADD(MONTH, 1, @StartDate)), 1) ))
END

IF @ERROR > 0
	BEGIN
		PRINT 'ERROR: Errors inserting into MonthsInRepayment for borrower information id: ' + @BorrowerInformationId + ' rows inserted: ' + @ROWCOUNT
		RAISERROR(N'ERROR: Errors inserting into MonthsInRepayment for borrower information id: %d rows inserted: %d', 16, 1, @BorrowerInformationId, @ROWCOUNT)
	END
ELSE
	BEGIN
		PRINT ('Rows Added: ' + CAST(@ROWCOUNT AS VARCHAR))
	END
