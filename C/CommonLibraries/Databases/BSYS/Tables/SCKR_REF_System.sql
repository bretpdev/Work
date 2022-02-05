CREATE TABLE [dbo].[SCKR_REF_System] (
    [Program] NVARCHAR (50) NOT NULL,
    [System]  NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_refSystem] PRIMARY KEY CLUSTERED ([Program] ASC, [System] ASC) WITH (FILLFACTOR = 90)
);

