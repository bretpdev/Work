/****** Script for SelectTopNRows command from SSMS  ******/
SELECT distinct
    l.SSN,
	l.DOC_ID,
	l.SCAN_DATE
  FROM [CWORKFLO].[dbo].[FEDERAL_LOC] l
  inner join [CWORKFLO].[dbo].[WFTRACK] w
	on l.WPID = w.WPID
	inner join [CWORKFLO].[dbo].[TASKID] t
			on t.TASKid = w.TASKID
	inner join 
	(
		select distinct
		l.wpid,
		max(w.TRACKID) as TrackId
	
		from
		[CWORKFLO].[dbo].[FEDERAL_LOC] l
		inner join [CWORKFLO].[dbo].[WFTRACK] w
			on l.WPID = w.WPID
		group by
			l.wpid
	) en
		on en.WPID = l.WPID
		and en.TrackId = w.TRACKID
	
  where  
   isnull(l.SSN,'') != ''
   
  and t.TASK not in ('Federal Archive','Federal System Support Group')
