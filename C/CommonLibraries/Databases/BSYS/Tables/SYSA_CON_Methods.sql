CREATE TABLE [dbo].[SYSA_CON_Methods] (
    [MethodNo]       INT          IDENTITY (1, 1) NOT NULL,
    [Library]        VARCHAR (50) NULL,
    [Module]         VARCHAR (50) NULL,
    [Method]         VARCHAR (50) NOT NULL,
    [Type]           VARCHAR (50) NOT NULL,
    [HasNativeSql]   BIT          CONSTRAINT [DF_SYSA_CON_Methods_HasNativeSql] DEFAULT ((0)) NOT NULL,
    [UsesCommonData] BIT          CONSTRAINT [DF_SYSA_CON_Methods_UsesCommonData] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SYSA_CON_Methods_1] PRIMARY KEY CLUSTERED ([MethodNo] ASC)
);

