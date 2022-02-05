CREATE PROCEDURE [dashcache].[AddCacheResult]
	@DashboardItemId INT,
	@UheaaCount INT = NULL,
	@CornerstoneCount INT = NULL,
	@UheaaElapsedTimeInMilliseconds INT = NULL,
	@CornerstoneElapsedTimeInMilliseconds INT = NULL
AS

INSERT INTO dashcache.DashboardCache (DashboardItemId, UheaaProblemCount, CornerstoneProblemCount, UheaaElapsedTimeInMilliseconds, CornerstoneElapsedTimeInMilliseconds)
VALUES (@DashboardItemId, @UheaaCount, @CornerstoneCount, @UheaaElapsedTimeInMilliseconds, @CornerstoneElapsedTimeInMilliseconds)

RETURN 0