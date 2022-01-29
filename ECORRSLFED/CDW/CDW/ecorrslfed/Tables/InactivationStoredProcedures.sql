CREATE TABLE [ecorrslfed].[InactivationStoredProcedures](
	[InactivationStoredProcedureId] [int] IDENTITY(1,1) NOT NULL,
	[StoredProcedureName] [varchar](100) NULL,
	[AddedAt] [datetime] NULL,
	[AddedBy] [varchar](50) NULL,
	[DeletedAt] [datetime] NULL,
	[DeletedBy] [varchar](50) NULL
) ON [PRIMARY]