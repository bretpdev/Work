-- =============================================
-- Author:		Bret Pehrson
-- Create date: 7/18/2013
-- Description:	Set the date and time of the current run
-- =============================================
CREATE PROCEDURE [dbo].[spSetRunDate] 
	@RunDate DateTime,
	@StartDate DateTime,
	@EndDate DateTime,
	@PreviousRunId int = null,
	@RunBy nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Report_Run_History(run_date, [start_date], end_date, previous_run_history_id, run_by)
	VALUES(@RunDate, @StartDate, @EndDate, @PreviousRunId, @RunBy)
	
END
