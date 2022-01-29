﻿CREATE PROCEDURE [monitor].[GetMonitorReasons]
AS

SELECT
	RTRIM(WQ20.WX_MSG_1_TSK) Reason, 
	COUNT(DISTINCT WQ20.BF_SSN) BorrowerCount
FROM 
	WQ20_TSK_QUE WQ20
WHERE
	WQ20.WF_QUE = 'R0'
GROUP BY
	WQ20.WX_MSG_1_TSK


RETURN 0