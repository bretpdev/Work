CREATE TABLE [quecomplet].[Queues](
	[QueueId] [int] IDENTITY(1,1) NOT NULL,
	[Queue] [varchar](2) NOT NULL,
	[SubQueue] [varchar](2) NOT NULL,
	[TaskControlNumber] [varchar](20) NOT NULL,
	[ARC] [char](5) NULL,
	[AccountIdentifier] [varchar](10) NOT NULL,
	[TaskStatusId] [int] NOT NULL,
	[ActionResponseId] [int] NULL,
	[PickedUpForProcessing] [datetime] NULL,
	[ProcessedAt] [datetime] NULL,
	[HadError] [bit] NOT NULL,
	[AddedAt] [datetime] NOT NULL,
	[AddedBy] [varchar](100) NOT NULL,
	[DeletedAt] [datetime] NULL,
	[DeletedBy] [varchar](100) NULL,
	[WasFound] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[QueueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [quecomplet].[Queues] ADD  DEFAULT ((0)) FOR [HadError]
GO

ALTER TABLE [quecomplet].[Queues] ADD  DEFAULT (getdate()) FOR [AddedAt]
GO

ALTER TABLE [quecomplet].[Queues]  WITH CHECK ADD  CONSTRAINT [FK_Queues_ToActionResponses] FOREIGN KEY([ActionResponseId])
REFERENCES [quecomplet].[ActionResponses] ([ActionResponseId])
GO

ALTER TABLE [quecomplet].[Queues] CHECK CONSTRAINT [FK_Queues_ToActionResponses]
GO

ALTER TABLE [quecomplet].[Queues]  WITH CHECK ADD  CONSTRAINT [FK_Queues_ToTaskStatuses] FOREIGN KEY([TaskStatusId])
REFERENCES [quecomplet].[TaskStatuses] ([TaskStatusId])
GO

ALTER TABLE [quecomplet].[Queues] CHECK CONSTRAINT [FK_Queues_ToTaskStatuses]
GO