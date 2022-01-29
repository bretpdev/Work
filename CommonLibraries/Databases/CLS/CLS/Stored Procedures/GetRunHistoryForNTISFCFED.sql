﻿CREATE PROCEDURE [dbo].[GetRunHistoryForNTISFCFED]
	
AS
	SELECT 
		(COUNT(*) + 1) AS RunCount
	FROM
		NTISFCFEDRunHistory
	WHERE
		RunDateTime BETWEEN DATEADD(DAY,DATEDIFF(DAY,0,GETDATE()),0) AND DATEADD(DAY,DATEDIFF(DAY,-1,GETDATE()),0)
RETURN 0
