CREATE TABLE [dbo].[LCTR_DAT_FlowChart] (
    [ID]          BIGINT         NOT NULL,
    [Type]        CHAR (10)      NOT NULL,
    [Description] VARCHAR (8000) NULL,
    CONSTRAINT [PK_LCTR_DAT_FlowChart] PRIMARY KEY CLUSTERED ([ID] ASC, [Type] ASC),
    CONSTRAINT [FK_LCTR_DAT_FlowChart_LCTR_LST_Type] FOREIGN KEY ([Type]) REFERENCES [dbo].[LCTR_LST_Type] ([Type])
);

