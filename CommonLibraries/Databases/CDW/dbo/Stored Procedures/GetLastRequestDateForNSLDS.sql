﻿
CREATE PROCEDURE [dbo].[GetLastRequestDateForNSLDS]
	@SSN CHAR(9)
AS
	SELECT 
	DATEDIFF(DAY,MAX(LF_CRT_DTS_GRSP), GETDATE()) AS LF_CRT_DTS_GRSP
FROM 
	GRSP_NDS_LON_RSP GRSP 	
WHERE 
	GRSP.DF_PRS_ID =  @SSN  
RETURN 0