CREATE TABLE [dbo].[OLBL_REF_SasDetail] (
    [BaseFileName] VARCHAR (20)  NOT NULL,
    [LetterId]     VARCHAR (10)  NOT NULL,
    [ActionCode]   VARCHAR (5)   NOT NULL,
    [CommentText]  VARCHAR (500) NULL,
    CONSTRAINT [PK_OLBL_REF_SasDetail] PRIMARY KEY CLUSTERED ([BaseFileName] ASC)
);

