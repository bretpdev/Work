CREATE TABLE [dbo].[TRAN_DAT_TrainingHistory] (
    [RecNum]          INT            IDENTITY (1, 1) NOT NULL,
    [WindowsUserName] NVARCHAR (50)  NULL,
    [Status]          NVARCHAR (50)  NULL,
    [StatusDate]      SMALLDATETIME  NULL,
    [Trainer]         NVARCHAR (100) NULL,
    [AppOrModName]    VARCHAR (255)  NULL,
    [Version]         SMALLDATETIME  NULL,
    CONSTRAINT [PK_TRAN_DAT_TrainingHistory] PRIMARY KEY CLUSTERED ([RecNum] ASC),
    CONSTRAINT [FK_TRAN_DAT_TrainingHistory_SYSA_LST_Users] FOREIGN KEY ([WindowsUserName]) REFERENCES [dbo].[SYSA_LST_Users] ([WindowsUserName]) ON UPDATE CASCADE
);

