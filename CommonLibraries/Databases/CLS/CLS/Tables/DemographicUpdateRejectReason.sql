CREATE TABLE [dbo].[DemographicUpdateRejectReason] (
    [RejectReason] VARCHAR (100) NOT NULL,
    [Comment]      VARCHAR (100) NULL,
    [Notes]        VARCHAR (100) NULL,
    CONSTRAINT [PK_DemographicUpdateRejectReason] PRIMARY KEY CLUSTERED ([RejectReason] ASC)
);

