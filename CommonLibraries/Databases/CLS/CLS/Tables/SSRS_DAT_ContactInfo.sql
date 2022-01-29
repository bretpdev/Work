CREATE TABLE [dbo].[SSRS_DAT_ContactInfo] (
    [SchoolReportId]       INT           IDENTITY (1, 1) NOT NULL,
    [SchoolName]           VARCHAR (250) NOT NULL,
    [SchoolCode]           VARCHAR (20)  NOT NULL,
    [BranchCode]           VARCHAR (10)  NOT NULL,
    [ContactNameSchool]    VARCHAR (100) NOT NULL,
    [EmailAddressSchool]   VARCHAR (200) NOT NULL,
    [ContactName3rdParty]  VARCHAR (100) NULL,
    [EmailAddress3rdParty] VARCHAR (200) NULL,
    [Recipient]            VARCHAR (10)  NOT NULL,
    CONSTRAINT [PK_SSRS_DAT_ContactInfo] PRIMARY KEY CLUSTERED ([SchoolReportId] ASC)
);

