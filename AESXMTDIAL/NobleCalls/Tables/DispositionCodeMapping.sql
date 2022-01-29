CREATE TABLE [dbo].[DispositionCodeMapping] (
    [DispositionCodeMappingId] INT          IDENTITY (1, 1) NOT NULL,
    [DispositionCode]          VARCHAR (150) NOT NULL,
    [Disposition]              VARCHAR (2)  NULL,
    [OnelinkDisposition]              VARCHAR (2)  NULL,
    [ArcId]                    INT          NOT NULL,
    [CommentId]                INT          NOT NULL,
    [ResponseCodeId]           INT          NOT NULL,
    CONSTRAINT [PK_SCFU_ResultCodes_Arc_Comment] PRIMARY KEY CLUSTERED ([DispositionCodeMappingId] ASC),
    CONSTRAINT [AK_SCFU_ResultCodes_Arc_Comment_ResultCode] UNIQUE NONCLUSTERED ([DispositionCode] ASC)
);

