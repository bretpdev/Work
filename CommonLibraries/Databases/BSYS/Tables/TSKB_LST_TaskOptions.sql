CREATE TABLE [dbo].[TSKB_LST_TaskOptions] (
    [RecNum]     INT           IDENTITY (1, 1) NOT NULL,
    [OptionText] VARCHAR (200) NOT NULL,
    [TaskBar]    VARCHAR (200) NOT NULL,
    [Valid]      BIT           CONSTRAINT [DF_TaskOptions_Valid] DEFAULT (1) NOT NULL,
    [ParentTask] VARCHAR (200) CONSTRAINT [DF_TSKB_LST_TaskOptions_ParentTask] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_TaskOptions] PRIMARY KEY CLUSTERED ([RecNum] ASC)
);

