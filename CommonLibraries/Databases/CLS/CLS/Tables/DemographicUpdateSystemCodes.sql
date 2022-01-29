CREATE TABLE [dbo].[DemographicUpdateSystemCodes] (
    [SourceName]        VARCHAR (50) NOT NULL,
    [SourceCode]        CHAR (2)     NOT NULL,
    [LocateType]        VARCHAR (3)  NOT NULL,
    [LocateDescription] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_DemographicUpdateSystemCodes] PRIMARY KEY CLUSTERED ([SourceName] ASC)
);

