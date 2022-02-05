CREATE PROCEDURE [dbo].[GetAllLetterData]

AS
    SELECT 
        [LetterId],
        [Letter],
        [LetterTypeId],
        [DocId],
        [Viewable],
        [ReportDescription],
        [ReportName],
        [Viewed],
        [MainframeRegion],
        [SubjectLine],
        [DocSource],
        [DocComment],
        [WorkFlow],
        [DocDelete]
    FROM 
        [dbo].[Letters]
    WHERE Active = 1

RETURN 0
