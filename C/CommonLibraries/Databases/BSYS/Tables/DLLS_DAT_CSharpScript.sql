CREATE TABLE [dbo].[DLLS_DAT_CSharpScript] (
    [ScriptId]                  VARCHAR (10)  NOT NULL,
    [ScriptName]                VARCHAR (100) NOT NULL,
    [AssemblyName]              VARCHAR (50)  NOT NULL,
    [StartingNamespaceAndClass] VARCHAR (100) NOT NULL,
    [Region]                    VARCHAR (11)  NOT NULL,
    CONSTRAINT [PK_DLLS_DAT_CSharpScript] PRIMARY KEY CLUSTERED ([ScriptId] ASC)
);

