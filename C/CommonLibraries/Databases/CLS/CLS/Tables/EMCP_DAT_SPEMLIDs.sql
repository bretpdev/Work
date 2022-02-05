CREATE TABLE [dbo].[EMCP_DAT_SPEMLIDs] (
    [EmailID]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [AccountNumber]  VARCHAR (10)  NOT NULL,
    [EmailCampignId] INT           NOT NULL,
    [EmailAddress]   VARCHAR (500) NOT NULL,
    CONSTRAINT [PK_EMCP_DAT_SPEMLIDs] PRIMARY KEY CLUSTERED ([EmailID] ASC)
);

