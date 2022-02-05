CREATE TABLE [dbo].[Letters] (
    [LetterId]          INT           IDENTITY (1, 1) NOT NULL,
    [Letter]            VARCHAR (10)  NOT NULL,
    [LetterTypeId]      INT           NOT NULL,
    [DocId]             VARCHAR (10)  NOT NULL,
    [Viewable]          CHAR (1)      NOT NULL,
    [ReportDescription] VARCHAR (60)  NOT NULL,
    [ReportName]        VARCHAR (17)  NOT NULL,
    [Viewed]            CHAR (1)      NOT NULL,
    [MainframeRegion]   VARCHAR (8)   NOT NULL,
    [SubjectLine]       VARCHAR (50)  NOT NULL,
    [DocSource]         VARCHAR (10)  NOT NULL,
    [DocComment]        VARCHAR (255) NOT NULL,
    [WorkFlow]          CHAR (1)      NOT NULL,
    [DocDelete]         CHAR (1)      NOT NULL,
    [Active]            BIT           DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Letters] PRIMARY KEY CLUSTERED ([LetterId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [CK_DocumentDetails_DocId] CHECK ([DocId] collate latin1_general_cs_as='XERO' OR [DocId] collate latin1_general_cs_as='XELT' OR [DocId] collate latin1_general_cs_as='XECL' OR [DocId] collate latin1_general_cs_as='XEIL' OR [DocId] collate latin1_general_cs_as='XEBL'),
    CONSTRAINT [FK_Letters_LetterTypes] FOREIGN KEY ([LetterTypeId]) REFERENCES [dbo].[LetterTypes] ([LetterTypeId])
);

