﻿CREATE PROCEDURE [nsldslisfr].[GetBorrowersWithOpenIdrQueueTasks]
AS

SELECT DISTINCT 
	PD10.DF_SPE_ACC_ID AS AccountNumber, 
	WQ20.BF_SSN AS Ssn,
	RTRIM(PD10.DM_PRS_1) AS FirstName,
	RTRIM(PD10.DM_PRS_LST) AS LastName,
	CAST(PD10.DD_BRT AS DATE) AS Dob
FROM 
	WQ20_TSK_QUE WQ20
	INNER JOIN PD10_PRS_NME PD10 
		ON PD10.DF_PRS_ID = WQ20.BF_SSN
WHERE
	WQ20.WF_QUE IN ('2A', '2P') 
	AND WQ20.WC_STA_WQUE20 = 'U'


RETURN 0
