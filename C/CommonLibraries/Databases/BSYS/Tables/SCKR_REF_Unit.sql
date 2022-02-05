CREATE TABLE [dbo].[SCKR_REF_Unit] (
    [Program] NVARCHAR (50) NOT NULL,
    [Unit]    NVARCHAR (50) NOT NULL,
    [Manager] NVARCHAR (50) NULL,
    CONSTRAINT [PK_refUnit] PRIMARY KEY CLUSTERED ([Program] ASC, [Unit] ASC) WITH (FILLFACTOR = 90)
);

