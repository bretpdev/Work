/****** Script for SelectTopNRows command from SSMS  ******/
SELECT C.[BF_SSN]
      ,C.[LN_SEQ]
      ,[Correct WD_ERL_PAY_RPT_NDS]
	  ,GRXX.WC_NDS_PAY_REC_RPT
  FROM [CDW].[dbo].[CNHXXXXX] C
  INNER JOIN CDW..GRXX_RPT_LON_APL GRXX
	ON GRXX.BF_SSN = C.BF_SSN
	AND GRXX.LN_SEQ = C.LN_SEQ