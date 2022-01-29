﻿CREATE PROCEDURE [verforbfed].[GetAllValidLoansForForbearance]
	@Ssn AS CHAR(9)
AS

	SELECT DISTINCT
		DW01.[WC_DW_LON_STA]
	FROM 
		[dbo].DW01_DW_CLC_CLU DW01
	WHERE
		DW01.BF_SSN = @Ssn

RETURN 0