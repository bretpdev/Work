CREATE TABLE [dbo].[LCTR_DAT_Policy] (
    [ID]     BIGINT    NOT NULL,
    [Type]   CHAR (10) NOT NULL,
    [Policy] TEXT      NULL,
    CONSTRAINT [PK_LCTR_DAT_Policy] PRIMARY KEY CLUSTERED ([ID] ASC, [Type] ASC),
    CONSTRAINT [FK_LCTR_DAT_Policy_LCTR_LST_Type] FOREIGN KEY ([Type]) REFERENCES [dbo].[LCTR_LST_Type] ([Type])
);

