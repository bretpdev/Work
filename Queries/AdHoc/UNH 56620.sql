USE [BSYS]
GO
/****** Object:  StoredProcedure [dbo].[spQSTA_ReportIterationList]    Script Date: 5/15/2018 7:42:55 AM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spQSTA_ReportIterationList]

AS

SELECT DISTINCT
	A.BusinessUnit as 'Business Unit' 
FROM
	QSTA_LST_QueueDetail A 
	JOIN GENR_REF_BU_Agent_Xref B 
		ON A.BusinessUnit = B.BusinessUnit 
WHERE
	B.Role = 'Manager'