CREATE PROCEDURE [dbo].[UpdateLetter]
    
    @LetterId int,
    @Letter varchar(10),
    @LetterTypeId int,
    @DocId varchar(10),
    @Viewable char(1),
    @ReportDescription varchar(60),
    @ReportName varchar(17),
    @Viewed char(1),
    @MainFrameRegion varchar(8),
    @SubjectLine varchar(50),
    @DocSource varchar(10),
    @DocComment varchar(255),
    @WorkFlow char(1),
    @DocDelete char(1)

AS
    
    UPDATE
        [dbo].[Letters]
    SET
        [Letter] = @Letter,
        [LetterTypeId] = @LetterTypeId,
        [DocId] = @DocId,
        [Viewable] = @Viewable,
        [ReportDescription] = @ReportDescription,
        [ReportName] = @ReportName,
        [Viewed] = @Viewed,
        [MainframeRegion] = @MainFrameRegion,
        [SubjectLine] = @SubjectLine, 
        [DocSource] = @DocSource,
        [DocComment] = @DocComment,
        [WorkFlow] = @WorkFlow,
        [DocDelete] = @DocDelete
    WHERE
        [LetterId] = @LetterId

RETURN 0
