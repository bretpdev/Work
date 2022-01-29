-- =============================================
-- Author:		Bret Pehrson
-- Create date: 7/18/2013
-- Description:	Returns a list of dates showing the day of the last run
-- =============================================
CREATE PROCEDURE [dbo].[spGetLastRunDate]
AS

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		report_run_history_id AS [DateID],
		run_date AS [RunDate],
		[start_date] AS [StartDate],
		end_date AS [EndDate],
		run_by as [RunBy]
	FROM
		Report_Run_History
	
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetLastRunDate] TO [db_executor]
    AS [dbo];

