CREATE TABLE [dbo].[TSKB_DAT_TaskData] (
    [RecNum]             INT           IDENTITY (1, 1) NOT NULL,
    [WindowsUserName]    NVARCHAR (50) NOT NULL,
    [Task]               VARCHAR (255) NOT NULL,
    [SubTask]            VARCHAR (255) CONSTRAINT [DF_TSKB_DAT_TaskData_SubTask] DEFAULT ('') NOT NULL,
    [UserEnteredDetails] VARCHAR (255) CONSTRAINT [DF_TSKB_DAT_TaskData_UserEnteredDetails] DEFAULT ('') NOT NULL,
    [BeginTime]          DATETIME      NOT NULL,
    [EndTime]            DATETIME      NULL,
    [TaskBar]            VARCHAR (200) NOT NULL,
    [Error]              BIT           CONSTRAINT [DF_TSKB_DAT_TaskData_Error] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_TSKB_DAT_TaskData] PRIMARY KEY CLUSTERED ([RecNum] ASC)
);

