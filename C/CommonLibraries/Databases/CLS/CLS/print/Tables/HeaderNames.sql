CREATE TABLE [print].[HeaderNames] (
    [HeaderNameId] INT          IDENTITY (1, 1) NOT NULL,
    [HeaderName]   VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([HeaderNameId] ASC) WITH (FILLFACTOR = 95)
);

