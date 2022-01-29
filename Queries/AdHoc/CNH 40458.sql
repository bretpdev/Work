/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (XXXX) [#]
      ,[Servicer]
      ,[SSN]
      ,[CEMS Case number]
      ,[Type]
      ,[Case Opened Date]
      ,[Borrower Notification Date]
      ,[Days of Interest Credit]
      ,[FX]
      ,[Loan Seq]
      ,[Current int rate]
      ,[Current Balance]
      ,[ITSXR]
      ,[Int Calculator]
      ,[Total int to adj ITSXR]
      ,[made payments?]
	  ,FSXX.LF_FED_AWD + RIGHT('XXX' + CAST(FSXX.LN_FED_AWD_SEQ AS VARCHAR(XX)), X) AS AWARD_ID
  FROM [CDW].[dbo].[CNH XXXXX] CNH
  INNER JOIN CDW..FSXX_DL_LON FSXX
	ON FSXX.BF_SSN = CNH.SSN
	AND FSXX.LN_SEQ = CNH.[LOAN SEQ]