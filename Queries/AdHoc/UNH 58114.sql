/****** Script for SelectTopNRows command from SSMS  ******/
SELECT  [CallRecordId]
      ,r.ReasonText
      ,[Comments]
      ,[LetterID]
      ,[IsCornerstone]
      ,[IsOutbound]
      ,[RecordedOn]
      ,[RecordedBy]
  FROM [MauiDUDE].[calls].[CallRecords] cr
  inner join MauiDUDE.calls.Reasons r
	on r.ReasonId = cr.ReasonId
  where LetterID != '' OR 