/****** Script for SelectTopNRows command from SSMS  ******/
SELECT  
LKXX.PX_DSC_MDM AS PAYMENT_TYPE,
DATENAME(MONTH, DATEADD(MONTH, MONTH(LD_RMT_PAY_EFF) , -X)) [MONTH],
MONTH(LD_RMT_PAY_EFF),
COUNT(*) AS PAYMENT_COUNT,
SUM(LA_BR_RMT) AS AMOUNT

  FROM [CDW].[dbo].[RMXX_BR_RMT] rmXX
  inner join cdw..LKXX_LS_CDE_LKP lkXX
	on lkXX.PM_ATR = 'LC-RMT-PAY-SRC'
	and lkXX.PX_ATR_VAL = rmXX.LC_RMT_PAY_SRC
where
	LD_RMT_PAY_EFF between 'XX/XX/XXXX' AND CAST(GETDATE() AS DATE)
	AND LC_RMT_STA = 'A'
	AND PC_FAT_TYP = 'XX'
	AND PC_FAT_SUB_TYP = 'XX'

GROUP BY
	LKXX.PX_DSC_MDM,
	MONTH(LD_RMT_PAY_EFF),
	DATENAME(MONTH, DATEADD(MONTH, MONTH(LD_RMT_PAY_EFF) , -X)) 
ORDER BY
	MONTH(LD_RMT_PAY_EFF),
	PAYMENT_COUNT
	