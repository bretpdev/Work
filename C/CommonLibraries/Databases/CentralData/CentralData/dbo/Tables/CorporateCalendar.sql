CREATE TABLE [dbo].[CorporateCalendar] (
    [CorporateCalendarId] INT           IDENTITY (1, 1) NOT NULL,
    [CalendarDate]        DATE          NOT NULL,
    [DateDescription]     VARCHAR (250) NULL,
    [AddedAt]             DATETIME      DEFAULT (getdate()) NOT NULL,
    [AddedBy]             VARCHAR (20)  NOT NULL,
    [Active]              BIT           DEFAULT ((1)) NOT NULL,
    UNIQUE NONCLUSTERED ([CalendarDate] ASC)
);



