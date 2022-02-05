CREATE TABLE [dbo].[CKPH_DAT_OPSCheckByPhone](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SSN] [varchar](9) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[DOB] [varchar](10) NOT NULL,
	[ABA] [varchar](50) NOT NULL,
	[BankAccountNumber] [varchar](200) NOT NULL,
	[AccountType] [varchar](50) NOT NULL,
	[Amount] [varchar](50) NOT NULL,
	[EffectiveDate] [varchar](50) NOT NULL,
	[AccountHolderName] [varchar](200) NOT NULL,
	[ProcessedDate] [datetime] NULL,
	[EncryptedBankAccountNumber] [varbinary](128) NULL,
	[EncryptedRoutingNumber] [varbinary](128) NULL,
 CONSTRAINT [PK_CKPH_DAT_OPSCheckByPhone] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

