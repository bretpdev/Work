/****** Script for SelectTopNRows command from SSMS  ******/
SELECT  [DF_PRS_ID]
      ,[BC_ATY_TYP]
      ,[BD_ATY_PRF]
      ,[PF_ACT]
      ,[BC_ATY_CNC_TYP]
      ,[BX_CMT]
  FROM [ODW].[dbo].[AY01_BR_ATY]
  where PF_ACT = 'DINAC' and BD_ATY_PRF >= '12/01/2019'