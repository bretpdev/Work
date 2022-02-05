CREATE TABLE [dbo].[SYSA_LST_Users] (
    [WindowsUserName]    NVARCHAR (50) NOT NULL,
    [LastName]           NVARCHAR (50) NOT NULL,
    [FirstName]          NVARCHAR (50) NOT NULL,
    [Middle Initial]     NVARCHAR (1)  NULL,
    [Ext]                VARCHAR (4)   NULL,
    [Ext2]               VARCHAR (4)   NULL,
    [Gather4PhnDat]      BIT           CONSTRAINT [DF_SYSA_LST_Users_Gather4PhnDat] DEFAULT (0) NOT NULL,
    [PseudoUser]         BIT           CONSTRAINT [DF_SYSA_LST_Users_PseudoUser] DEFAULT (0) NOT NULL,
    [PersonalLoanInfoID] VARCHAR (10)  NULL,
    [title]              VARCHAR (100) NULL,
    CONSTRAINT [PK_SYSA_LST_Users] PRIMARY KEY CLUSTERED ([WindowsUserName] ASC)
);

