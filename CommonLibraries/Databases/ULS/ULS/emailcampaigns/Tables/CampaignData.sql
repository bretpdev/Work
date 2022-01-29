CREATE TABLE [emailcampaigns].[CampaignData] (
    [CampaignDataId] INT            IDENTITY (1, 1) NOT NULL,
    [SasFile]        VARCHAR (100)  NOT NULL,
    [HtmlFile]       VARCHAR (100)  NOT NULL,
    [SendingAddress] VARCHAR (100)  NOT NULL,
    [SubjectLine]    VARCHAR (300)  NOT NULL,
    [Arc]            VARCHAR (5)    NULL,
    [ActionCode]     VARCHAR (5)    NULL,
    [CommentText]    VARCHAR (1000) NULL,
    PRIMARY KEY CLUSTERED ([CampaignDataId] ASC),
    UNIQUE NONCLUSTERED ([SasFile] ASC)
);

