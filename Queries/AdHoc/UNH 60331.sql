/****** Script for SelectTopNRows command from SSMS  ******/
SELECT 
      [DocName]

      ,[ID]
      
  FROM [BSYS].[dbo].[LTDB_DAT_DocDetail]
  where DocTyp = 'Script' and [status] = 'Active' and DocName not like '%FED%' and ID is not null and Letterhead != 'Loan Management' and unit != 'Loan Management'