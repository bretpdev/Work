CREATE PROCEDURE [acurintc].[GetSystemSources]
AS

SELECT
	[SystemSourceId],
    [Name],
	[LocateType], 
    [OneLinkSourceCode], 
    [CompassSourceCode], 
    [ActivityType], 
    [ContactType]
FROM
	acurintc.SystemSources

RETURN 0
