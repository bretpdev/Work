CREATE TABLE [dbo].[SCKR_REF_SackerAgents] (
    [Agent]  NVARCHAR (50) NOT NULL,
    [Action] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_refSackerAgents] PRIMARY KEY CLUSTERED ([Agent] ASC, [Action] ASC) WITH (FILLFACTOR = 90)
);

