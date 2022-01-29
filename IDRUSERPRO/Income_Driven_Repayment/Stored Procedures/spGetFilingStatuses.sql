﻿-- =============================================
-- Author:		Jarom Ryan
-- Create date: 05/22/2013	
-- Description:	Will pull all data from dbo.Filing_Statuses
-- =============================================
CREATE PROCEDURE [dbo].[spGetFilingStatuses]

AS
BEGIN

	SELECT 
		filing_status_id AS FilingStatusId,
		filing_status AS FilingStatusCode,
		filing_status_description AS FilingStatusDescription,
		status_for_married AS StatusForMarried
	FROM 
		dbo.Filing_Statuses
END