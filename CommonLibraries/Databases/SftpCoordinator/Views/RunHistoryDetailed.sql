CREATE VIEW [dbo].[RunHistoryDetailed] AS

select rh.RunHistoryId, rh.StartedOn, rh.EndedOn, count(l.ActivityLogId) - count(l.InvalidFileId) as SuccessfulFiles, count(l.InvalidFileId) as InvalidFiles, rh.RunBy
  from [dbo].[RunHistory] rh
  left join [dbo].[ActivityLog] l on l.RunHistoryId = rh.RunHistoryId 
 group by rh.RunHistoryId, rh.StartedOn, rh.RunBy, rh.EndedOn
 
