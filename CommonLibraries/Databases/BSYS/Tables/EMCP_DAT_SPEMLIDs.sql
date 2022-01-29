CREATE TABLE [dbo].[EMCP_DAT_SPEMLIDs] (
    [EmailID]       BIGINT       IDENTITY (1, 1) NOT NULL,
    [AccountNumber] VARCHAR (50) NOT NULL,
    [EmailCampID]   INT          NOT NULL,
    [EmailAddress]  VARCHAR (50) NULL,
    CONSTRAINT [PK_EMCP_DAT_SPEMLIDs] PRIMARY KEY CLUSTERED ([EmailID] ASC)
);

