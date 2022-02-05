CREATE TABLE [dbo].[Estimates] (
    [EstimateId]     INT             IDENTITY (1, 1) NOT NULL,
    [RequestType]    VARCHAR (10)    NOT NULL,
    [RequestNumber]  VARCHAR (50)    NOT NULL,
    [EstimatedHours] DECIMAL (18, 2) NOT NULL,
	[TestingFixes] DECIMAL (18, 2)  NULL,
	[ReasonForAdjustment] VARCHAR(MAX) NULL,
	[AdditionalHrs] DECIMAL(18,2) NULL,
	[AttachmentFileName] VARCHAR(500) NULL,
    [Employee]       VARCHAR (500)   NOT NULL,
	[CreatedAt]		DATETIME NOT NULL DEFAULT GETDATE()
    CONSTRAINT [PK_Estimates] PRIMARY KEY CLUSTERED ([EstimateId] ASC) WITH (FILLFACTOR = 95)
);

