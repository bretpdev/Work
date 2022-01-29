/****** Script for SelectTopNRows command from SSMS  ******/
SELECT  CNH.[BF_SSN]
      ,CNH.[LN_SEQ]
      ,[Current WD_ERL_PAY_RPT_NDS]
      ,[Update WD_ERL_PAY_RPT_NDS]
      ,[Current WC_NDS_PAY_REC_RPT]
      ,[Update WC_NDS_PAY_REC_RPT],
	  GRXX.WN_SEQ_GRXX
  FROM [CDW].[dbo].[CNH XXXXX] CNH
INNER JOIN CDW..GRXX_RPT_LON_APL GRXX
	ON GRXX.BF_SSN = CNH.BF_SSN
	AND GRXX.LN_SEQ = CNH .LN_SEQ
