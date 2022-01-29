CREATE TABLE [dbo].[COST_DAT_MailCodeCostCenters] (
    [MailCodeCostCenterId] INT        IDENTITY (1, 1) NOT NULL,
    [MailCode]             CHAR (6)   NOT NULL,
    [CostCenterId]         INT        NOT NULL,
    [Weight]               FLOAT (53) NOT NULL,
    [EffectiveBegin]       DATE       NOT NULL,
    [EffectiveEnd]         DATE       NULL,
    CONSTRAINT [PK_COST_DAT_MailCodeCostCenters] PRIMARY KEY CLUSTERED ([MailCodeCostCenterId] ASC)
);

