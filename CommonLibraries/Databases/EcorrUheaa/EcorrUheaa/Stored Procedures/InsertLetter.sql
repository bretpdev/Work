CREATE PROCEDURE [dbo].[InsertLetter]
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
    

    INSERT INTO [dbo].[Letters]([Letter],[LetterTypeId],[DocId],[Viewable],[ReportDescription],[ReportName],[Viewed],[MainframeRegion],[SubjectLine],[DocSource],[DocComment],[WorkFlow],[DocDelete])
    VALUES(@Letter, @LetterTypeId, @DocId, @Viewable, @ReportDescription, @ReportName, @Viewed, @MainFrameRegion, @SubjectLine, @DocSource, @DocComment, @WorkFlow, @DocDelete)

RETURN 0

