﻿CREATE PROCEDURE [verforbfed].[GetLoanStatusInformation]
	@Ssn AS CHAR(9)
AS

	SELECT DISTINCT
		RTRIM(DW01.[WC_DW_LON_STA]) [WC_DW_LON_STA],
		RTRIM(DW01.[WX_OVR_DW_LON_STA]) [WX_OVR_DW_LON_STA]
	FROM 
		[dbo].DW01_DW_CLC_CLU DW01
	WHERE
		DW01.BF_SSN = @Ssn

RETURN 0