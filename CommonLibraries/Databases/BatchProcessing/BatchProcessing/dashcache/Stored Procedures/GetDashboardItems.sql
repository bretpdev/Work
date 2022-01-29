CREATE PROCEDURE [dashcache].[GetDashboardItems]
AS

SELECT
	[DashboardItemId],
    [ItemName], 
    [UheaaSprocName], 
    [UheaaDatabase], 
	[CornerstoneSprocName], 
    [CornerstoneDatabase], 
    [Retired]
FROM
	dashcache.DashboardItems


RETURN 0