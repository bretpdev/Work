CREATE TABLE [dbo].[Countries] (
    [CountryId]   INT          IDENTITY (1, 1) NOT NULL,
    [CountryCode] CHAR (2)     NOT NULL,
    [CountryName] VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([CountryId] ASC)
);

