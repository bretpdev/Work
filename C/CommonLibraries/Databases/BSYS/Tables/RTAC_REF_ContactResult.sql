CREATE TABLE [dbo].[RTAC_REF_ContactResult] (
    [Result]      VARCHAR (50)  NOT NULL,
    [Arc]         VARCHAR (5)   NOT NULL,
    [MessageText] VARCHAR (500) NOT NULL,
    CONSTRAINT [PK_RTAC_DAT_ContactResult] PRIMARY KEY CLUSTERED ([Result] ASC)
);

