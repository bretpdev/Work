CREATE TABLE [dbo].[HomePageScriptsAndServices] (
    [ScriptID]               INT            IDENTITY (1, 1) NOT NULL,
    [InternalOrExternal]     NVARCHAR (8)   NOT NULL,
    [Category]               NVARCHAR (50)  NOT NULL,
    [SubCategory1]           NVARCHAR (50)  NOT NULL,
    [SubCategory2]           NVARCHAR (50)  NOT NULL,
    [SubCategory3]           NVARCHAR (50)  NOT NULL,
    [DisplayName]            NVARCHAR (50)  NOT NULL,
    [SubToBeCalled]          NVARCHAR (100) NOT NULL,
    [ToBeCalledImm]          NVARCHAR (5)   NOT NULL,
    [ToBeCalledAtEnd]        NVARCHAR (5)   NOT NULL,
    [HomePage]               NVARCHAR (50)  NOT NULL,
    [DataForFunctionCall]    NVARCHAR (50)  NOT NULL,
    [CallForNoteDUDECleanUp] NVARCHAR (5)   NOT NULL,
    [CompletionFile]         VARCHAR (300)  NULL,
    [DLLToLoad]              NVARCHAR (50)  NULL,
    [DLLsToCopy]             VARCHAR (1000) NULL,
    [ObjectToCreate]         NVARCHAR (50)  NULL
);

