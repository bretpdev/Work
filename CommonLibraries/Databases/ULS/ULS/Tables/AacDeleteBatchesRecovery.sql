CREATE TABLE [dbo].[AacDeleteBatchesRecovery] (
    [MajorBatchToDelete] VARCHAR (10) NOT NULL,
    [UserId]             VARCHAR (7)  NOT NULL,
    CONSTRAINT [PK_AacDeleteBatchesRecovery] PRIMARY KEY CLUSTERED ([MajorBatchToDelete] ASC)
);

