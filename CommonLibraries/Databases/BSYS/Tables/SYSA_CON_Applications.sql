CREATE TABLE [dbo].[SYSA_CON_Applications] (
    [Application]    VARCHAR (50) NOT NULL,
    [Type]           VARCHAR (50) NOT NULL,
    [HasNativeSql]   BIT          CONSTRAINT [DF_SYSA_CON_Applications_HasNativeSql] DEFAULT ((0)) NOT NULL,
    [UsesCommonData] BIT          CONSTRAINT [DF_SYSA_CON_Applications_UsesCommonData] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SYSA_CON_Applications] PRIMARY KEY CLUSTERED ([Application] ASC)
);

