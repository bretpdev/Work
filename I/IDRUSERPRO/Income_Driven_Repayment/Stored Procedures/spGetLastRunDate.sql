-- =============================================
-- Author:		Bret Pehrson
-- Create date: 7/18/2013
-- Description:	Returns a list of dates showing the day of the last run
-- =============================================
CREATE PROCEDURE [dbo].[spGetLastRunDate]
AS

BEGIN

   
	SELECT
		report_run_history_id AS [DateID],
		run_date AS [RunDate],
		[start_date] AS [StartDate],
		end_date AS [EndDate],
		run_by as [RunBy]
	FROM
		Report_Run_History
	
END