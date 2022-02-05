CREATE TABLE [dbo].[SCKR_LST_Predecessors] (
    [Predecessor] NVARCHAR (100) NOT NULL,
    [Description] NVARCHAR (50)  NULL,
    CONSTRAINT [PK_lstPredecessors] PRIMARY KEY CLUSTERED ([Predecessor] ASC) WITH (FILLFACTOR = 90)
);

