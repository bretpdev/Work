CREATE TABLE [dbo].[CorrectEndTime] (
    [CorrectEndTimeId] INT             IDENTITY (1, 1) NOT NULL,
    [SqlUserId]        INT             NOT NULL,
    [TicketId]         INT             NOT NULL,
    [StartTime]        DATETIME        NOT NULL,
    [EndTime]          DATETIME        NULL,
    [Name]             VARCHAR (50)    NOT NULL,
    [Hours]            INT             NOT NULL,
    [Minutes]          FLOAT (53)      NOT NULL,
    [CorrectHours]     DECIMAL (18, 2) NULL,
    [CorrectEndTime]   DATETIME        NOT NULL,
    CONSTRAINT [PK_CorrectEndTime] PRIMARY KEY CLUSTERED ([CorrectEndTimeId] ASC)
);

