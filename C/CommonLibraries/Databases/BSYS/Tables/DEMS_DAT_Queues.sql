CREATE TABLE [dbo].[DEMS_DAT_Queues] (
    [Queue]                   VARCHAR (8)   NOT NULL,
    [Department]              VARCHAR (3)   NOT NULL,
    [System]                  VARCHAR (7)   NOT NULL,
    [Parser]                  VARCHAR (100) NOT NULL,
    [Processor]               VARCHAR (100) NOT NULL,
    [DemographicsReviewQueue] VARCHAR (8)   NOT NULL,
    [ForeignReviewQueue]      VARCHAR (8)   NOT NULL,
    CONSTRAINT [PK_DEMS_DAT_Queues] PRIMARY KEY CLUSTERED ([Queue] ASC)
);

