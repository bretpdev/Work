CREATE TABLE [dbo].[SCKR_REF_Session] (
    [Script]  NVARCHAR (50) CONSTRAINT [DF_SCKR_REF_Session_Script] DEFAULT (0) NOT NULL,
    [Session] NVARCHAR (50) NOT NULL,
    [Toolbar] BIT           CONSTRAINT [DF_SCKR_REF_Session_Toolbar] DEFAULT (0) NOT NULL,
    [Menu]    NVARCHAR (50) NULL,
    CONSTRAINT [PK_refSession] PRIMARY KEY CLUSTERED ([Script] ASC, [Session] ASC) WITH (FILLFACTOR = 90)
);

