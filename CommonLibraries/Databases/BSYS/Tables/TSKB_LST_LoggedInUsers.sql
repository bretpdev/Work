CREATE TABLE [dbo].[TSKB_LST_LoggedInUsers] (
    [LoggedInUser] NVARCHAR (50) NOT NULL,
    [LoggedInAt]   DATETIME      NOT NULL,
    CONSTRAINT [PK_TSKB_LST_LoggedInUsers] PRIMARY KEY CLUSTERED ([LoggedInUser] ASC)
);

