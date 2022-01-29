CREATE TABLE [dbo].[SCKR_DAT_Training] (
    [Request]     INT           NOT NULL,
    [Class]       NVARCHAR (3)  NOT NULL,
    [Trainer]     NVARCHAR (50) NULL,
    [TrainedDate] SMALLDATETIME NULL,
    [Summary]     NTEXT         NULL,
    [Comments]    NTEXT         NULL,
    [Update]      NTEXT         NULL,
    CONSTRAINT [PK_datTraining] PRIMARY KEY CLUSTERED ([Request] ASC, [Class] ASC) WITH (FILLFACTOR = 90)
);

