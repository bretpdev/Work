/****** Script for SelectTopNRows command from SSMS  ******/
UPDATE 
	[MauiDUDE].[dbo].[MenuOptionsScriptsAndServices]
SET
	ObjectToCreate = 'OPSCBPFED.QInterface' 
WHERE
	SubToBeCalled LIKE 'OPSCBPFED%'