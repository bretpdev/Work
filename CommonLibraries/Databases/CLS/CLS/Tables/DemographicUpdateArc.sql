CREATE TABLE [dbo].[DemographicUpdateArc] (
    [Queue]           CHAR (4)      NOT NULL,
    [DemographicType] VARCHAR (10)  NOT NULL,
    [RejectReason]    VARCHAR (100) NOT NULL,
    [Arc]             VARCHAR (5)   NOT NULL,
    CONSTRAINT [PK_DemographicUpdateArc] PRIMARY KEY CLUSTERED ([Queue] ASC, [DemographicType] ASC, [RejectReason] ASC),
    CONSTRAINT [FK_DemographicUpdateArc_DemographicUpdateQueue] FOREIGN KEY ([Queue]) REFERENCES [dbo].[DemographicUpdateQueue] ([Queue]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_DemographicUpdateArc_DemographicUpdateRejectReason] FOREIGN KEY ([RejectReason]) REFERENCES [dbo].[DemographicUpdateRejectReason] ([RejectReason]) ON DELETE CASCADE ON UPDATE CASCADE
);

