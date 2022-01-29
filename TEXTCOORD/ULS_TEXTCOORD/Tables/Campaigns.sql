CREATE TABLE [textcoord].[Campaigns] (
    [CampaignId] INT           IDENTITY (1, 1) NOT NULL,
    [Campaign]   VARCHAR (300) NOT NULL,
    [CampaignCode] VARCHAR(20) NULL,
    [Sproc]      VARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([CampaignId] ASC)
);