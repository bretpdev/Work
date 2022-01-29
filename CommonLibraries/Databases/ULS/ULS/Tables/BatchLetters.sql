CREATE TABLE [dbo].[BatchLetters]
(
	[BatchLettersId] [int] IDENTITY(1,1) NOT NULL,
	[LetterId] [varchar](10) NOT NULL,
	[SasFilePattern] [varchar](50) NOT NULL,
	[StateFieldCodeName] [varchar](25) NOT NULL,
	[AccountNumberFieldName] [varchar](25) NOT NULL,
	[CostCenterFieldCodeName] [varchar](25) NOT NULL,
	[IsDuplex] bit not null,
	[OkIfMissing] [bit] NOT NULL,
	[ProcessAllFiles] [bit] NOT NULL,
	[Arc] [varchar](5) NULL,
	[Comment] [varchar](1200) NULL,
	[CreatedAt] [datetime] NOT NULL DEFAULT getdate(),
	[CreatedBy] [varchar](250) NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[UpdatedBy] [varchar](250) NULL,
	[Active] [bit] NOT NULL DEFAULT 1, 
    CONSTRAINT [PK_BatchLetters] PRIMARY KEY ([BatchLettersId])
)
