USE ULS
GO

CREATE SCHEMA loboxskip Authorization dbo;

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE TABLE loboxskip.Regions(
	[RegionId] [int] IDENTITY(1,1) NOT NULL,
	[Region] [varchar](10) NOT NULL,
	[Active] bit NOT NULL,
	[AddedAt] datetime DEFAULT(GETDATE()),
	[AddedBy] [varchar](50) DEFAULT(SUSER_SNAME()),
	[InactivatedAt] datetime NULL,
	[InactivatedBy] [varchar](50) NULL
PRIMARY KEY CLUSTERED 
(
	[RegionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE loboxskip.SkipQueue(
	[SkipQueueId] [int] IDENTITY(1,1) NOT NULL,
	[RegionId] [int] NOT NULL,
	[SkipQueue] [varchar](10) NOT NULL,
	[Description] [varchar](50) NULL,
	[Active] bit NOT NULL,
	[AddedAt] datetime DEFAULT(GETDATE()),
	[AddedBy] [varchar](50) DEFAULT(SUSER_SNAME()),
	[InactivatedAt] datetime NULL,
	[InactivatedBy] [varchar](50) NULL
PRIMARY KEY CLUSTERED 
(
	[SkipQueueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE loboxskip.SkipQueue  WITH CHECK ADD  CONSTRAINT [FK_Regions] FOREIGN KEY(RegionId)
REFERENCES loboxskip.Regions (RegionId)

GO


INSERT INTO loboxskip.Regions([Region],[Active],[AddedAt],[AddedBy],[InactivatedAt],[InactivatedBy])
VALUES
('COMPASS',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
('ONELINK',1,GETDATE(),SUSER_SNAME(),NULL,NULL)

GO

INSERT INTO loboxskip.SkipQueue([RegionId],[SkipQueue],[Description],[Active],[AddedAt],[AddedBy],[InactivatedAt],[InactivatedBy])
VALUES
(1,'IA','Borr Call 1',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(1,'IB','Borr Call 2',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(1,'IC','Borr D/A',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(1,'IG','Refr Call 1',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(1,'IH','Refr Call 2',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(1,'II','Refr D/A',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(1,'JI','Credit Return',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(1,'JN','Skip Acct Rvw',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(1,'SO','State Offc Ed',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(2,'ACURINTR','Fr Accurint',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(2,'KADRCALL','Bad Phones',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(2,'KAREFCLL','Admin Refr Call',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(2,'KCREFCLL','Refr Call - SRV',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(2,'KDREFCLL','Refr Call - DFT',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(2,'KEMPCALL','Employer Call',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(2,'KFAXNMBR','Fax Machine Rvw',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(2,'KFINALRV','Final Review',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(2,'KFRGNADD','Rvw Frgn Format',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(2,'KORGLNDR','Original Lender',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(2,'KOTHDEMO','Skip w Alt Addr',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(2,'KREDARTN','Real EDA',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(2,'KREFRLTR','Refr Letter',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(2,'KSKEMAIL','Skip w Vld Email',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(2,'KSKPREVW','Review Skip',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(2,'REFRCALR','Directory Asst',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(2,'REFRCALS','Refr Cal',1,GETDATE(),SUSER_SNAME(),NULL,NULL),
(2,'KLOANAPP','Ref Images',1,GETDATE(),SUSER_SNAME(),NULL,NULL)

