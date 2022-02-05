CREATE TABLE [covidforb].[ForbearanceProcessing](
	[ForbearanceProcessingId] [bigint] IDENTITY(1,1) NOT NULL,
	[AccountNumber] [varchar](10) NOT NULL,
	[ForbCode] [varchar](1) NOT NULL,
	[DateRequested] [date] NOT NULL,
	[ForbearanceType] [varchar](2) NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[DateCertified] [date] NOT NULL,
	[SubType] [varchar](2) NULL,
	[SchoolCode] [varchar](8) NULL,
	[MedicalInternship] [char](1) NULL,
	[StateLicensingCertificationProvided] [char](1) NULL,
	[SchoolEnrollment] [char](1) NULL,
	[ReservistNationalGuard] [char](1) NULL,
	[CoMakerEligibility] [char](1) NULL,
	[AuthorizedToExceedMax] [char](1) NULL,
	[DodForm] [char](1) NULL,
	[ForbToClearDelq] [char](1) NULL,
	[CapitalizeInterest] [char](1) NULL,
	[PaymentAmount] [varchar](12) NULL,
	[SignatureOfBorrower] [char](1) NULL,
	[SignatureOfOfficial] [char](1) NULL,
	[PhysiciansCertification] [char](1) NULL,
	[SelectAllLoans] [bit] NOT NULL,
	[BusinessUnitId] [bigint] NOT NULL,
	[ForbearanceProcessedOn] [datetime] NULL,
	[ArcComment] VARCHAR(300) NULL,
	[Failure] BIT NULL,
	[ArcAddProcessingId] [bigint] NULL,
	[PrintProcessingId] [bigint] NULL,
	[InvalidAddressNotOnEcorr] bit null default 0,
	[ProcessOn] [datetime] NOT NULL,
	[AddedBy] [varchar](50) NULL,
	[AddedAt] [datetime] NULL,
	[DeletedBy] [varchar](50) NULL,
	[DeletedAt] [datetime] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[ForbearanceProcessingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [covidforb].[ForbearanceProcessing] ADD  DEFAULT ((1)) FOR [SelectAllLoans]
GO

ALTER TABLE [covidforb].[ForbearanceProcessing] ADD  DEFAULT (NULL) FOR [ForbearanceProcessedOn]
GO

ALTER TABLE [covidforb].[ForbearanceProcessing] ADD  DEFAULT (suser_name()) FOR [AddedBy]
GO

ALTER TABLE [covidforb].[ForbearanceProcessing] ADD  DEFAULT (getdate()) FOR [AddedAt]
GO

ALTER TABLE [covidforb].[ForbearanceProcessing] ADD  DEFAULT (NULL) FOR [DeletedBy]
GO

ALTER TABLE [covidforb].[ForbearanceProcessing] ADD  DEFAULT (NULL) FOR [DeletedAt]
GO

ALTER TABLE [covidforb].[ForbearanceProcessing]  WITH NOCHECK ADD FOREIGN KEY([BusinessUnitId])
REFERENCES [covidforb].[BusinessUnits] ([BusinessUnitId])
GO
