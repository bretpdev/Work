CREATE TABLE [complaints].[ComplaintFlags] (
    [ComplaintFlagId] INT IDENTITY (1, 1) NOT NULL,
    [ComplaintId]     INT NOT NULL,
    [FlagId]          INT NOT NULL,
    PRIMARY KEY CLUSTERED ([ComplaintFlagId] ASC),
    CONSTRAINT [FK_ComplaintFlags_Complaints] FOREIGN KEY ([ComplaintId]) REFERENCES [complaints].[Complaints] ([ComplaintId]),
    CONSTRAINT [FK_ComplaintFlags_Flags] FOREIGN KEY ([FlagId]) REFERENCES [complaints].[Flags] ([FlagId])
);

