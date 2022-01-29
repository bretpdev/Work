CREATE TABLE [dbo].[BankoReceiveAdditionalEvents] (
    [RecordNumber]     BIGINT        IDENTITY (1, 1) NOT NULL,
    [CaseNumber]       VARCHAR (12)  NULL,
    [DateOfEvent]      VARCHAR (10)  NULL,
    [EventCode]        VARCHAR (5)   NULL,
    [EventDescription] VARCHAR (200) NULL
);

