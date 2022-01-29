CREATE TABLE [dbo].[COST_DAT_BusinessUnitCostCenters] (
    [BusinessUnitCostCenterId] INT        IDENTITY (1, 1) NOT NULL,
    [BusinessUnitId]           INT        NOT NULL,
    [CostCenterId]             INT        NOT NULL,
    [Weight]                   FLOAT (53) NOT NULL,
    [EffectiveBegin]           DATE       NOT NULL,
    [EffectiveEnd]             DATE       NULL,
    CONSTRAINT [PK__COST_DAT_BusinessUnitCostCenters] PRIMARY KEY CLUSTERED ([BusinessUnitCostCenterId] ASC) WITH (FILLFACTOR = 95)
);



