CREATE TABLE [dbo].[StatusSchedules] (
    [StatusScheduleId] INT          IDENTITY (1, 1) NOT NULL,
    [StartAt]          DATETIME     NOT NULL,
    [EndAt]            DATETIME     NOT NULL,
    [StatusCodeId]     INT          NOT NULL,
    [RegionId]         INT          NOT NULL,
    [AddedAt]          DATETIME     DEFAULT (getdate()) NOT NULL,
    [AddedBy]          VARCHAR (50) NOT NULL,
    [UpdatedAt]        DATETIME     NULL,
    [UpdatedBy]        VARCHAR (50) NULL,
    [DeletedAt]        DATETIME     NULL,
    [DeletedBy]        VARCHAR (50) NULL,
    CONSTRAINT [PK_StatusSchedules] PRIMARY KEY CLUSTERED ([StatusScheduleId] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [FK_StatusSchedules_Regions] FOREIGN KEY ([RegionId]) REFERENCES [dbo].[Regions] ([RegionId]),
    CONSTRAINT [FK_StatusSchedules_StatusCodes] FOREIGN KEY ([StatusCodeId]) REFERENCES [dbo].[StatusCodes] ([StatusCodeId])
);

