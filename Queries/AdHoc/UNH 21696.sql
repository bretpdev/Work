/****** Script for SelectTopNRows command from SSMS  ******/
SELECT 
	[WindowsUserName]
    ,[System]
    ,[System Specific User ID]
    ,[Notes]
    ,[DtAccessRemoved]
FROM 
	[BSYS].[dbo].[SYSA_REF_User_Systems]
WHERE
	[System] = 'EdConnect'