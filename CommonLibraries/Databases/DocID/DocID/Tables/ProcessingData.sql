CREATE TABLE [dbo].[ProcessingData] (
    [RecNum]            BIGINT        IDENTITY (1, 1) NOT NULL,
    [UserName]          VARCHAR (200) NOT NULL,
    [AccountNumber]     VARCHAR (10)  NOT NULL,
    [DocID]             VARCHAR (500) NOT NULL,
    [TimeOfTransaction] DATETIME      NOT NULL,
    [Source]            CHAR (2)      NULL,
    CONSTRAINT [PK_ProcessingData] PRIMARY KEY CLUSTERED ([RecNum] ASC)
);

