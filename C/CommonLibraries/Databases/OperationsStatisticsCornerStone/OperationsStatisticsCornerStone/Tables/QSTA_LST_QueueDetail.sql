CREATE TABLE [dbo].[QSTA_LST_QueueDetail] (
	QueueId int not null IDENTITY,
    [QueueName]         VARCHAR (8)   NOT NULL,
    [BusinessUnit]      INT            NULL,
    [QueueDesc]         VARCHAR (500) NOT NULL,
    [COMPASSShrtDesc]   VARCHAR (50)  NOT NULL,
    [SystemIndicator]   VARCHAR (50)  NOT NULL,
    [NumOfDaysLateTask] INT            NOT NULL,
    [SystemQInd]        CHAR (1)       NOT NULL, 
    CONSTRAINT [PK_QSTA_LST_QueueDetail] PRIMARY KEY ([QueueId])
);

