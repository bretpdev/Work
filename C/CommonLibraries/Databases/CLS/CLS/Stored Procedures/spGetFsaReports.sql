-- =============================================
-- Author:		Jarom Ryan
-- Create date: 02/01/2013
-- Description:	This sp will pull all data from dbo.CreateFsaReports
-- =============================================
CREATE PROCEDURE [dbo].[spGetFsaReports] 


AS
BEGIN

	SET NOCOUNT ON;

	Select
		[ReportId],
		[ReportName],
		[FileName],
		[Occurance]
	From dbo.CreateFsaReports
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetFsaReports] TO [db_executor]
    AS [dbo];



