CREATE TABLE [dbo].[COST_DAT_CostCenters] (
    [CostCenterId]      INT          IDENTITY (1, 1) NOT NULL,
    [CostCenter]        VARCHAR (50) NOT NULL,
    [IsBillable]        BIT          CONSTRAINT [DF_COST_DAT_CostCenters_IsBillable] DEFAULT ((1)) NOT NULL,
    [IsChargedOverHead] BIT          CONSTRAINT [DF_COST_DAT_CostCenters_IsChargedOverHead] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_GENR_DAT_CostCenters] PRIMARY KEY CLUSTERED ([CostCenterId] ASC),
    CONSTRAINT [IX_COST_DAT_CostCenters] UNIQUE NONCLUSTERED ([CostCenterId] ASC)
);



