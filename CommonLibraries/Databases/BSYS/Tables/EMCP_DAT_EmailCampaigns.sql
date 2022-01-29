CREATE TABLE [dbo].[EMCP_DAT_EmailCampaigns] (
    [CampID]           INT            IDENTITY (1, 1) NOT NULL,
    [EmailSubjectLine] VARCHAR (200)  NOT NULL,
    [Compass]          CHAR (1)       NOT NULL,
    [OneLINK]          CHAR (1)       NOT NULL,
    [ARC]              VARCHAR (5)    NOT NULL,
    [ActionCode]       VARCHAR (5)    NOT NULL,
    [CommentText]      TEXT           NOT NULL,
    [DataFile]         VARCHAR (8000) CONSTRAINT [DF_EMCP_DAT_EmailCampaigns_DataFile] DEFAULT ('') NOT NULL,
    [HTMLFile]         VARCHAR (8000) CONSTRAINT [DF_EMCP_DAT_EmailCampaigns_HTMLFile] DEFAULT ('') NOT NULL,
    [EmailFrom]        VARCHAR (50)   NOT NULL,
    [EmailFromDisplay] VARCHAR (50)   NOT NULL,
    [DateSetup]        DATETIME       CONSTRAINT [DF_EMCP_DAT_EmailCampaigns_DateSetup] DEFAULT (getdate()) NOT NULL,
    [DateComplete]     DATETIME       NULL,
    CONSTRAINT [PK_EMCP_DAT_EmailCampaigns] PRIMARY KEY CLUSTERED ([CampID] ASC)
);

