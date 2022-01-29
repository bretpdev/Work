/****** Script for SelectTopNRows command from SSMS  ******/
SELECT distinct
      dd.DocName, 
	  dd.id
  FROM [BSYS].[dbo].[LTDB_REF_KeyWords] kw
  inner join bsys..LTDB_DAT_DocDetail dd
	on dd.DocName = kw.DocName
  where
	(
		KeyWord like '%deli%'
		or KeyWord like '%notice%'
		or KeyWord like '%past%'
		or KeyWord like '%default%'
		or KeyWord like '%bill%'
	)

 and dd.DocName not like '%FED%'
and dd.[Status] = 'Active'
and DocTyp != 'OneLINK'
 