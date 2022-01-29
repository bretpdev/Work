USE [NobleCalls]
GO

/****** Object:  Table [nblclbck].[Callback]    Script Date: 2/14/2020 1:05:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE SCHEMA [nblclbck] AUTHORIZATION dbo;

CREATE TABLE [nblclbck].[Callback](
	[CallbackId] [bigint] IDENTITY(1,1) NOT NULL,
	[rowid] [int] NOT NULL,
	[cb_rowid] [int] NULL,
	[cb_appl] [varchar](4) NULL,
	[cb_acode] [int] NULL,
	[cb_phone] [varchar](10) NULL,
	[cb_date] [date] NULL,
	[cb_time] [int] NULL,
	[cb_tsr] [varchar](4) NULL,
	[cb_status] [varchar](2) NULL,
	[cb_adate] [date] NULL,
	[cb_atime] [int] NULL,
	[cb_btries] [int] NULL,
	[cb_ntries] [int] NULL,
	[cb_country_id] [smallint] NULL,
	[cb_listid] [int] NULL,
	[cb_dnis] [varchar](20) NULL,
	[has_lapsed] [int] NOT NULL,
	[AddedAt] [datetime] NULL,
 CONSTRAINT [PK_Callback] PRIMARY KEY CLUSTERED 
(
	[CallbackId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO


