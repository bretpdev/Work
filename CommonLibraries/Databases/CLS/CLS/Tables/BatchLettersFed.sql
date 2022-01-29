CREATE TABLE [dbo].[BatchLettersFed] (
    [BatchLettersFedId]       INT            IDENTITY (1, 1) NOT NULL,
    [LetterId]                VARCHAR (10)   NOT NULL,
    [SasFilePattern]          VARCHAR (50)   NOT NULL,
    [StateFieldCodeName]      VARCHAR (25)   NOT NULL,
    [AccountNumberFieldName]  VARCHAR (25)   NOT NULL,
	[BorrowerSsnIndex]		  INT			 DEFAULT -1 NOT NULL,
    [AccountNumberIndex]      INT            DEFAULT 0 NOT NULL,
    [CostCenterFieldCodeName] VARCHAR (25)   NULL,
    [OkIfMissing]             BIT            NOT NULL,
    [ProcessAllFiles]         BIT            NOT NULL,
    [Arc]                     VARCHAR (5)    NULL,
    [Comment]                 VARCHAR (1200) NULL,
	[DoNotProcessEcorr]		  BIT			 DEFAULT(0) NOT NULL,
    [CreatedAt]               DATETIME       DEFAULT (getdate()) NOT NULL,
    [CreatedBy]               VARCHAR (250)  NOT NULL,
    [UpdatedAt]               DATETIME       NULL,
    [UpdatedBy]               VARCHAR (250)  NULL,
    [Active]                  BIT            DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_BatchLettersFed] PRIMARY KEY CLUSTERED ([BatchLettersFedId] ASC)
);

