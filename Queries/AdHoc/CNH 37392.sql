
SELECT distinct 
	max([Request]) over(partition by d.docname) as [Request]
		,d.id
      ,[Title]
      ,d.[DocName]
      ,replace(cast(d.[description] as varchar(max)), char(XX) + char(XX), '') as [description]
	  ,d.[Status]

  FROM [BSYS].[dbo].[LTDB_DAT_Requests] r 
  inner join [BSYS].[dbo].[LTDB_DAT_DocDetail] d
	on r.DocName = d.DocName 
  where Title like '%Retire%' or Title like '%Retired%' or Title like '%Retires%' or Title like '%Retirement%'
  or r.DocName like '%Retire%'  or r.DocName  like '%Retired%' or r.DocName  like '%Retires%' or r.DocName  like '%Retirement%' 
   or d.[Description] like '%Retire%'  or d.[Description]   like '%Retired%' or d.[Description]   like '%Retires%' or d.[Description]   like '%Retirement%' 
