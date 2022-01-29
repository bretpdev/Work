CREATE TABLE [compfafsa].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [varchar](300) NOT NULL,
	[EmailAddress] [varchar](250) NOT NULL,
	[HashedPassword] [varchar](500) NOT NULL,
	[HashedResetPassword] [varchar](500) NULL,
	[ResetPasswordCreated] DATETIME NULL,
	[LastLogin] [datetime] NULL,
	[FailedLogonAttempts] [int] NULL,
	[PasswordLastUpdated] [datetime] NOT NULL,
	[Admin] [bit] NOT NULL DEFAULT(0),
	[PasswordSet] [bit] NOT NULL DEFAULT(0),
	[DeletedAt] DATETIME NULL,
	[DeletedBy] VARCHAR(200) NULL
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]

) ON [PRIMARY]

GO

ALTER TABLE [compfafsa].[Users] ADD  DEFAULT ((0)) FOR [FailedLogonAttempts]
GO
