CREATE TABLE [dbo].[GENR_LST_HoldReason] (
    [HoldReason]     VARCHAR (50) NOT NULL,
    [ApplicationKey] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_GENR_LST_HoldReason] PRIMARY KEY CLUSTERED ([HoldReason] ASC, [ApplicationKey] ASC)
);

