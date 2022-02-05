CREATE TABLE [dbo].[DemographicUpdateQueue] (
    [Queue]     CHAR (4)      NOT NULL,
    [Parser]    VARCHAR (100) NOT NULL,
    [Processor] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_DemographicUpdateQueue] PRIMARY KEY CLUSTERED ([Queue] ASC)
);

