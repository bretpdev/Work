CREATE TABLE [dbo].[StrictStates] (
    [StrictStateId] INT          IDENTITY (1, 1) NOT NULL,
    [StateCode]     VARCHAR (2)  NOT NULL,
    [CreatedAt]     DATETIME     CONSTRAINT [DF_StrictStates_CreatedAt] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]     VARCHAR (50) CONSTRAINT [DF_StrictStates_CreatedBy] DEFAULT (suser_sname()) NOT NULL,
    [DeletedAt]     DATETIME     NULL,
    [DeletedBy]     VARCHAR (50) NULL,
    CONSTRAINT [PK_StrictStates] PRIMARY KEY CLUSTERED ([StrictStateId] ASC) WITH (FILLFACTOR = 95)
);

