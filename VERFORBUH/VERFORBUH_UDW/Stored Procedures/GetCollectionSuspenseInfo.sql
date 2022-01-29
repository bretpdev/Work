﻿CREATE PROCEDURE [verforbuh].[GetCollectionSuspenseInfo]
	@Ssn CHAR(10)
AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SELECT DISTINCT
		FB10.LC_FOR_TYP,
		CAST(LN60.LD_FOR_BEG AS DATETIME) AS CollectionSuspenseForbearanceBeginDate,
		CAST(LN60.LD_FOR_END AS DATETIME) AS CollectionSuspenseForbearanceEndDate
	FROM	
		[dbo].FB10_BR_FOR_REQ FB10
		INNER JOIN [dbo].LN60_BR_FOR_APV LN60 ON FB10.BF_SSN = LN60.BF_SSN AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
	WHERE 
		FB10.BF_SSN = @Ssn
		AND 
		FB10.LC_FOR_TYP in(25, 28)
		AND
		FB10.LC_FOR_STA = 'A'
		AND
		LN60.LC_STA_LON60 = 'A'

RETURN 0