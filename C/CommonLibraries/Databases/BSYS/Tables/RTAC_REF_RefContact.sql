CREATE TABLE [dbo].[RTAC_REF_RefContact] (
    [Contact]     VARCHAR (50)  NOT NULL,
    [Result]      VARCHAR (50)  NOT NULL,
    [Arc]         VARCHAR (5)   NOT NULL,
    [MessageText] VARCHAR (500) NULL,
    CONSTRAINT [PK_RTAC_DAT_RefContact] PRIMARY KEY CLUSTERED ([Contact] ASC, [Result] ASC),
    CONSTRAINT [FK_RTAC_DAT_RefContact_RTAC_DAT_ContactResult] FOREIGN KEY ([Contact]) REFERENCES [dbo].[RTAC_REF_ContactResult] ([Result])
);

