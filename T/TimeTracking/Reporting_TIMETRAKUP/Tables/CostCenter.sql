CREATE TABLE [dbo].[CostCenter] (
    [CostCenterId] INT          IDENTITY (1, 1) NOT NULL,
    [CostCenter]   VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([CostCenterId] ASC) WITH (FILLFACTOR = 95)
);

