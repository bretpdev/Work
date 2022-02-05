CREATE TABLE [dbo].[COST_DAT_AgentWeights] (
    [AgentWeightId]  INT        IDENTITY (1, 1) NOT NULL,
    [SqlUserId]      INT        NOT NULL,
    [Weight]         FLOAT (53) NOT NULL,
    [EffectiveBegin] DATE       CONSTRAINT [DF_COST_DAT_AgentWeights_EffectiveBegin] DEFAULT (getdate()) NOT NULL,
    [EffectiveEnd]   DATE       NULL,
    CONSTRAINT [PK_COST_DAT_AgentWeights] PRIMARY KEY CLUSTERED ([AgentWeightId] ASC)
);

