
CREATE Procedure BorrowerSelect_Info
	(
		@AccountNumber char(10)
	)
AS

select isnull(Summaries.WA_TOT_BRI_OTS, 0.00) as Interest,
       isnull(Summaries.COHORT, '') as CohortYear,
       isnull(Summaries.COHORT_IND, 'N') as CohortAlertIndicator,
       isnull(Summaries.LA_CUR_PRI, 0.00) as Principal,
       isnull(Summaries.LR_ITR_DLY, 0.00) as DailyInterest,
       isnull(Summaries.LR_ITR_MONTH, 0.00) as MonthlyInterestAccrual,
       isnull(Summaries.LR_ITR_MONTH_5, 0.00) as MonthlyInterestAccrualPlus5,

       isnull(Repayment.DUE_DAY, '') as NextDueDate,
       isnull(Repayment.LD_CRT_LON65, '') as LastRPSPrintDate,
       isnull(Repayment.MONTH_AMT, '') as MonthlyPaymentAmount,
       isnull(Repayment.MULT_DUE_DT, 'N') as MultipleDueDatesAlertIndicator,
       isnull(Repayment.DUE_DAY, '') as DueDay,

       isnull(AmountDue.CUR_DUE, 0.00) as CurrentAmountDue, 
       isnull(AmountDue.PAST_DUE, 0.00) as AmountPastDue,
       isnull(AmountDue.TOT_DUE, 0.00) as TotalAmountDue,
       isnull(AmountDue.TOT_DUE_FEE, 0.00) as TotalAmountPlusLateFees,

       isnull(Bill.MULT_BIL_MTD, 'N') as MultipleBillMethodAlertIndicator,
       isnull(Bill.BIL_MTD, '') as BillType,

       isnull(LastPayment.LST_PMT_RCVD, '') as LastPaymentReceivedDate,
       isnull(Endorser.COBORROWER, 'N') as CoborrowerAlertIndicator,
       isnull(Hardship.FORB_36, 'N') as Exceeded36MonthForbAlertIndicator,
       isnull(Delinquency.LD_DLQ_OCC, '') as DateDelinquencyOccured,
       isnull(Delinquency.CUR_DLQ, 0) as DaysDelinquent,
       isnull(WaiveFee.FEE_WAV_DOL, 0) as TotalLateFeesWaived,
       isnull(WaiveFee.FEE_WAV_CT, 0) as TotalNumberOfTimesLateFeesHaveBeenWaived,
       isnull(SpecialHandling.SPHAN,'N') AS VipAlertIndicator,
       isnull(SpecialHandling.VIPSS,'N') AS SpecialHandlingAlertIndicator,
       isnull(Info411.LX_ATY, '') as Info411,
       isnull(LetterSummary.DL200, 0) as NumberOf20DayLettersSent,
       isnull(Rehabilitation.LD_LON_RHB_PCV, '') as Rehabilitated,
       isnull(Rehabilitation.REHAB_IND, 'N') AS RehabilitatedAlertIndicator,
       isnull(PendingDisbursement.PendDisb, 'N') AS HasPendingDisbursement,			
       isnull(Suspense.LA_BR_RMT_PST, 0.00) AS PaymentsInSuspense
  from dbo.PD10_Borrower Borrower
  left join dbo.BORR_Repayment Repayment 
         on Borrower.DF_SPE_ACC_ID = Repayment.DF_SPE_ACC_ID
  left join dbo.BORR_Bill Bill 
         on Borrower.DF_SPE_ACC_ID = Bill.DF_SPE_ACC_ID
  left join dbo.BORR_Summaries Summaries 
         on Borrower.DF_SPE_ACC_ID = Summaries.DF_SPE_ACC_ID
  left join dbo.BORR_AmountDue AmountDue 
         on Borrower.DF_SPE_ACC_ID = AmountDue.DF_SPE_ACC_ID
  left join dbo.BORR_LastPayment LastPayment 
         on Borrower.DF_SPE_ACC_ID = LastPayment.DF_SPE_ACC_ID
  left join dbo.BORR_Delinquency Delinquency 
         on	Borrower.DF_SPE_ACC_ID = Delinquency.DF_SPE_ACC_ID			
  left join dbo.BORR_Hardship Hardship 
         on	Borrower.DF_SPE_ACC_ID = Hardship.DF_SPE_ACC_ID	
  left join dbo.BORR_Endorser Endorser 
         on Borrower.DF_SPE_ACC_ID = Endorser.DF_SPE_ACC_ID	
  left join dbo.AY10_WaiveFee WaiveFee 
         on	Borrower.DF_SPE_ACC_ID = WaiveFee.DF_SPE_ACC_ID
  left join dbo.BORR_Rehabilitation Rehabilitation 
         on	Borrower.DF_SPE_ACC_ID = Rehabilitation.DF_SPE_ACC_ID
  left join dbo.AY10_20DayLetterSummary LetterSummary
         on Borrower.DF_SPE_ACC_ID = LetterSummary.DF_SPE_ACC_ID
  left join dbo.AY10_M1411 Info411
         on Borrower.DF_SPE_ACC_ID = Info411.DF_SPE_ACC_ID
		and Info411.LN_ATY_SEQ = (select max(LN_ATY_SEQ)
								    from dbo.AY10_M1411 
								   where DF_SPE_ACC_ID = Borrower.DF_SPE_ACC_ID)
  left join dbo.AY10_SpecialHandling SpecialHandling 
         on	Borrower.DF_SPE_ACC_ID = SpecialHandling.DF_SPE_ACC_ID
  left join dbo.RM31_Suspense Suspense 
         on	Borrower.DF_SPE_ACC_ID = Suspense.DF_SPE_ACC_ID
  left join dbo.BORR_PendingDisb PendingDisbursement 
         on Borrower.DF_SPE_ACC_ID = PendingDisbursement.DF_SPE_ACC_ID
 where Borrower.DF_SPE_ACC_ID = @AccountNumber