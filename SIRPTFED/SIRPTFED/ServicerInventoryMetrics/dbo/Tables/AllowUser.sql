CREATE TABLE [dbo].[AllowUser] (
    [AllowUserId]        INT           IDENTITY (1, 1) NOT NULL,
    [UserName]           VARCHAR (48)  NOT NULL,
    [IsAdmin]            BIT           DEFAULT ((0)) NOT NULL,
    [ServicerCategory]   VARCHAR (128) NOT NULL,
    [ServicerMetric]     VARCHAR (128) NOT NULL,
    [ServicerMetricGoal] VARCHAR (256) NOT NULL,
    [IsManualUpdate]     BIT           NOT NULL,
    PRIMARY KEY CLUSTERED ([AllowUserId] ASC) WITH (FILLFACTOR = 95)
);

