CREATE TABLE [dbo].[QSTA_LST_QueueDetail] (
    [QueueName]         NVARCHAR (8)   NOT NULL,
    [BusinessUnit]      NVARCHAR (50)  NULL,
    [QueueDesc]         NVARCHAR (500) NOT NULL,
    [QueueStatusCode]   NVARCHAR (1)   NULL,
    [QueueStatusDesc]   NVARCHAR (50)  NULL,
    [COMPASSShrtDesc]   NVARCHAR (50)  NOT NULL,
    [SystemIndicator]   NVARCHAR (50)  NOT NULL,
    [NumOfDaysLateTask] INT            NOT NULL,
    [SystemQInd]        CHAR (1)       NOT NULL,
    CONSTRAINT [PK_QSTA_LST_QueueDetail] PRIMARY KEY CLUSTERED ([QueueName] ASC)
);

