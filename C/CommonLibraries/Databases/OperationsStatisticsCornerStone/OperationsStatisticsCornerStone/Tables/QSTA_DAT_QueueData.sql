CREATE TABLE [dbo].[QSTA_DAT_QueueData]
 (
	QueueDataId int not null IDENTITY,
    [RunDateTime] DATETIME     NOT NULL,
    [Queue]       VARCHAR(8) NOT NULL,
    [Total]       BIGINT       NOT NULL,
    [Complete]    BIGINT       NOT NULL,
    [Critical]    BIGINT       NOT NULL,
    [Canceled]   BIGINT       NOT NULL,
    [Outstanding] BIGINT       NOT NULL,
    [Problem]     BIGINT       NOT NULL,
    [Late]        BIGINT       NULL,
    [Dept]        VARCHAR(3) NULL, 
    CONSTRAINT [PK_QSTA_DAT_QueueData] PRIMARY KEY ([QueueDataId])
);

