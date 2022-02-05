CREATE TABLE [dbo].[COST_DAT_BatchScriptWeights] (
    [BatchScriptWeightId] INT        IDENTITY (1, 1) NOT NULL,
    [CostCenterId]        INT        NULL,
    [Weight]              FLOAT (53) NOT NULL,
    [EffectiveBegin]      DATE       NOT NULL,
    [EffectiveEnd]        DATE       NULL,
    CONSTRAINT [PK_COST_DAT_BatchScriptWeights] PRIMARY KEY CLUSTERED ([BatchScriptWeightId] ASC)
);

