/****** Script for SelectTopNRows command from SSMS  ******/
SELECT 
	PD10.DF_SPE_ACC_ID,
	[SSN],
    [LOAN SEQ],
    [FEB25TH],
    [FEB26TH],
    [JAN_CREDIT_RPT],
    [FEB_CREDIT_RPT],
    [FS Comments],
    [C1],
    [C2],
    [C3],
	isnull(DD.LN_DLQ_MAX,0) as [days_past_due_03-26]
  FROM [UDW].[dbo].[UNH_70932] unh
  inner join UDW..PD10_PRS_NME PD10
	ON PD10.DF_PRS_ID = UNH.SSN
LEFT JOIN UDW.CALC.DailyDelinquency DD
	ON DD.BF_SSN = UNH.SSN
	and dd.LN_SEQ = unh.[LOAN SEQ]
	AND DD.AddedAt = '03/26/2021'