CREATE TABLE [dbo].[EMCP_DAT_EmailCampaigns] (
    [CampID]               BIGINT         IDENTITY (1, 1) NOT NULL,
    [EmailSubjectLine]     VARCHAR (200)  NOT NULL,
    [CornerStone]          BIT            NOT NULL,
    [IncludeAccountNumber] BIT            NOT NULL,
    [Arc]                  VARCHAR (5)    NULL,
    [CommentText]          VARCHAR (1238) NOT NULL,
    [DataFile]             VARCHAR (1000) NOT NULL,
    [HTMLFile]             VARCHAR (1000) NOT NULL,
    [EmailFrom]            VARCHAR (200)  NOT NULL,
    [DateSetup]            DATETIME       NULL,
    [DateComplete]         DATETIME       NULL,
    CONSTRAINT [PK_EMCP_DAT_EmailCampaigns1] PRIMARY KEY CLUSTERED ([CampID] ASC)
);

