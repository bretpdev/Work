CREATE TABLE [dbo].[SCKR_LST_ReturnReasons] (
    [ReturnReason] NVARCHAR (100) NOT NULL,
    [ReturnType]   NVARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_lstReturnReasons] PRIMARY KEY CLUSTERED ([ReturnReason] ASC, [ReturnType] ASC) WITH (FILLFACTOR = 90)
);

