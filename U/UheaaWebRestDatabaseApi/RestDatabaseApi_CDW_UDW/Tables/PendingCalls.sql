CREATE TABLE [NobleController].[PendingCalls](
	[Name] [varchar](50) NULL,
	[CountLast45] [int] NULL,
	[State] [varchar](2) NULL,
	[DaysDelinquent] [int] NULL,
	[AmountDue] [varchar](4000) NULL,
	[AccountNumber] [varchar](10) NOT NULL,
	[Primary] [varchar](15) NULL,
	[Alternate] [varchar](15) NULL,
	[Agent] [varchar](100) NOT NULL,
	[Campaign] [varchar](500) NULL
)