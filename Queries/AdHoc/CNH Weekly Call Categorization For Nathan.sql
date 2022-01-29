use MauiDUDE
go

SELECT [RecNum] as [Record Number]
      ,[Catergory]
      ,[Reason]
      ,[LetterID]
      ,[Comments]
      ,cast([DateAndTimeOfCall] as Date) as [Date of Call]
	  ,cast([DateAndTimeOfCall] as Time) as [Time of Call]
      ,[UserID]
  FROM [MauiDUDE].[dbo].[CallCat_Data]
  where DateAndTimeOfCall between DATEADD(ww, -X, GetDate()) and DATEADD(d, -X, GetDate()) -- This is for if this is run on a monday