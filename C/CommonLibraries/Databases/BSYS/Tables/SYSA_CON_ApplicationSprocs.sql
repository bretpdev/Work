CREATE TABLE [dbo].[SYSA_CON_ApplicationSprocs] (
    [Application] VARCHAR (100) NOT NULL,
    [Sproc]       VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_SYSA_CON_ApplicationSprocs] PRIMARY KEY CLUSTERED ([Application] ASC, [Sproc] ASC)
);

