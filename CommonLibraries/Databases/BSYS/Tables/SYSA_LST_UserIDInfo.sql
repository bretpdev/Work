CREATE TABLE [dbo].[SYSA_LST_UserIDInfo] (
    [UserID]                  NVARCHAR (7)  NOT NULL,
    [DateEstablished]         SMALLDATETIME CONSTRAINT [DF_SYSA_LST_UserIDInfo_DateEstablished] DEFAULT (getdate()) NOT NULL,
    [Date Access Removed]     SMALLDATETIME NULL,
    [Last Modified]           SMALLDATETIME CONSTRAINT [DF_SYSA_LST_UserIDInfo_Last Modified] DEFAULT (getdate()) NULL,
    [Access To Certify Loans] BIT           CONSTRAINT [DF_SYSA_LST_UserIDInfo_Access To Certify Loans] DEFAULT (0) NOT NULL,
    [InstitutionID]           NVARCHAR (10) NULL,
    [Notes]                   NTEXT         NULL,
    [WindowsUserName]         NVARCHAR (50) NOT NULL,
    [QUserType]               NVARCHAR (1)  CONSTRAINT [DF_SYSA_LST_UserIDInfo_QUserType] DEFAULT (N'W') NOT NULL,
    CONSTRAINT [PK_SYSA_LST_UserIDInfo] PRIMARY KEY CLUSTERED ([UserID] ASC),
    CONSTRAINT [FK_SYSA_LST_UserIDInfo_SYSA_LST_Institutions] FOREIGN KEY ([InstitutionID]) REFERENCES [dbo].[SYSA_LST_Institutions] ([InstitutionID]) ON UPDATE CASCADE,
    CONSTRAINT [FK_SYSA_LST_UserIDInfo_SYSA_LST_Users] FOREIGN KEY ([WindowsUserName]) REFERENCES [dbo].[SYSA_LST_Users] ([WindowsUserName]) ON UPDATE CASCADE
);

