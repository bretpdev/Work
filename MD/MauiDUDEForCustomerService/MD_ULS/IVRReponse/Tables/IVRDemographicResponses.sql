CREATE TABLE [dbo].[IVRDemographicResponses](
	[IVRDemographicResponseId] [int] IDENTITY(1,1) NOT NULL,
	[AccountNumber] [varchar](10) NOT NULL,
	[CallDate] [datetime] NOT NULL,
	[ResponseTypeId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[IVRDemographicResponseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[IVRDemographicResponses] ADD  DEFAULT (getdate()) FOR [CallDate]
GO

ALTER TABLE [dbo].[IVRDemographicResponses]  WITH CHECK ADD  CONSTRAINT [FK_IVRDemographicResponses_ResponseTypes_ResponseTypeId] FOREIGN KEY([ResponseTypeId])
REFERENCES [dbo].[ResponseTypes] ([ResponseTypeId])
GO

ALTER TABLE [dbo].[IVRDemographicResponses] CHECK CONSTRAINT [FK_IVRDemographicResponses_ResponseTypes_ResponseTypeId]
GO