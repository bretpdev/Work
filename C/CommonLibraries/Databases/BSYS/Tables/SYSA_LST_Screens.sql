CREATE TABLE [dbo].[SYSA_LST_Screens] (
    [ScreenID]    NVARCHAR (4)   NOT NULL,
    [Description] NVARCHAR (100) NULL,
    [Active]      BIT            CONSTRAINT [DF_SYSA_LST_Screens_Active] DEFAULT (1) NOT NULL,
    [Driver]      BIT            CONSTRAINT [DF_SYSA_LST_Screens_Driver] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_SYSA_LST_Screens] PRIMARY KEY CLUSTERED ([ScreenID] ASC)
);

