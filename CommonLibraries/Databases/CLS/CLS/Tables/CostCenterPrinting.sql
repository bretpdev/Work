CREATE TABLE [dbo].[CostCenterPrinting] (
    [PrintDateTime]  DATETIME     NOT NULL,
    [LetterId]       VARCHAR (10) NOT NULL,
    [ForeignCount]   INT          NOT NULL,
    [DomesticCount]  INT          NOT NULL,
    [CostCenterCode] CHAR (6)     NOT NULL,
    CONSTRAINT [PK_CostCenterPrinting] PRIMARY KEY CLUSTERED ([PrintDateTime] ASC, [LetterId] ASC)
);

