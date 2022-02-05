CREATE TABLE [dbo].[GENR_REF_ScopeandAppKey] (
    [Scope]  VARCHAR (50) NOT NULL,
    [AppKey] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_GENR_REF_ScopeandAppKey] PRIMARY KEY CLUSTERED ([Scope] ASC, [AppKey] ASC)
);

