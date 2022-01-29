CREATE TABLE [dbo].[SCKR_DAT_Promotes] (
    [Request]  INT           NOT NULL,
    [Class]    NVARCHAR (50) NOT NULL,
    [Promotes] NTEXT         NULL,
    CONSTRAINT [PK_datPromotes] PRIMARY KEY CLUSTERED ([Request] ASC, [Class] ASC) WITH (FILLFACTOR = 90)
);

