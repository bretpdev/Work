CREATE TABLE [dbo].[Holidays] (
    [HolidayId]   INT          IDENTITY (1, 1) NOT NULL,
    [Date]        DATETIME     NULL,
    [Description] VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([HolidayId] ASC)
);

