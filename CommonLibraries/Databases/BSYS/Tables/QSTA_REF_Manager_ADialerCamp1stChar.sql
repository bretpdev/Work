CREATE TABLE [dbo].[QSTA_REF_Manager_ADialerCamp1stChar] (
    [WindowsUserName]  NVARCHAR (50) NOT NULL,
    [CampName4ADialer] NVARCHAR (1)  NOT NULL,
    CONSTRAINT [PK_QS_XRefUserID_ADialerCamp1stChar] PRIMARY KEY CLUSTERED ([WindowsUserName] ASC, [CampName4ADialer] ASC),
    CONSTRAINT [FK_QSTA_REF_Manager_ADialerCamp1stChar_SYSA_LST_Users] FOREIGN KEY ([WindowsUserName]) REFERENCES [dbo].[SYSA_LST_Users] ([WindowsUserName]) ON UPDATE CASCADE
);

