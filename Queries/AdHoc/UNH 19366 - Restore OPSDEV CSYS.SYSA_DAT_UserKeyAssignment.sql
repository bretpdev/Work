USE [CSYS]
GO
/****** Object:  Table [dbo].[SYSA_DAT_UserKeyAssignment]    Script Date: 04/11/2014 14:54:00 ******/
ALTER TABLE [dbo].[SYSA_DAT_UserKeyAssignment] DROP CONSTRAINT [DF_SYSA_DAT_UserKeyAssignment_StartDate]
GO
DROP TABLE [dbo].[SYSA_DAT_UserKeyAssignment]
GO
/****** Object:  Table [dbo].[SYSA_DAT_UserKeyAssignment]    Script Date: 04/11/2014 14:54:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SYSA_DAT_UserKeyAssignment](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SqlUserId] [int] NOT NULL,
	[UserKey] [varchar](100) NOT NULL,
	[BusinessUnit] [int] NULL,
	[StartDate] [datetime] NOT NULL CONSTRAINT [DF_SYSA_DAT_UserKeyAssignment_StartDate]  DEFAULT (getdate()),
	[EndDate] [datetime] NULL,
	[AddedBy] [int] NOT NULL,
	[RemovedBy] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
SET IDENTITY_INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ON
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8622, 1261, N'Bill Not Sent', 0, CAST(0x0000A18000E6086E AS DateTime), NULL, 0, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8623, 1131, N'Bill Not Sent', 0, CAST(0x0000A18000E6086E AS DateTime), NULL, 0, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8624, 1163, N'Bill Not Sent', 0, CAST(0x0000A18000E6086E AS DateTime), NULL, 0, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8625, 1170, N'Bill Not Sent', 0, CAST(0x0000A18000E6086E AS DateTime), NULL, 0, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8626, 1263, N'Bill Not Sent', 0, CAST(0x0000A18000E6086E AS DateTime), NULL, 0, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8627, 1264, N'Bill Not Sent', 0, CAST(0x0000A18000E6086E AS DateTime), NULL, 0, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8628, 1153, N'Accurint-FED', 35, CAST(0x0000A24D009268EF AS DateTime), NULL, 1256, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8629, 1198, N'Accurint-FED', 35, CAST(0x0000A24D009268EF AS DateTime), NULL, 1256, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8630, 1152, N'Accurint-FED', 35, CAST(0x0000A24D009268EF AS DateTime), NULL, 1256, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8631, 1423, N'Accurint-FED', 35, CAST(0x0000A24D009268EF AS DateTime), NULL, 1256, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8632, 1256, N'Accurint-FED', 35, CAST(0x0000A24D009268EF AS DateTime), NULL, 1256, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8633, 1261, N'Accurint-FED', 10, CAST(0x0000A24D009268EF AS DateTime), NULL, 1256, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8634, 1163, N'Accurint-FED', 10, CAST(0x0000A24D009268EF AS DateTime), NULL, 1256, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8635, 1263, N'Accurint-FED', 10, CAST(0x0000A24D009268EF AS DateTime), NULL, 1256, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8636, 1154, N'CRKEY', 1, CAST(0x0000A2E600D8EF17 AS DateTime), NULL, 0, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8637, 1155, N'CRKEY', 1, CAST(0x0000A2E600D8EF17 AS DateTime), NULL, 0, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8638, 1243, N'CRKEY', 1, CAST(0x0000A2E600D8EF17 AS DateTime), NULL, 0, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8639, 1131, N'CRKEY', 3, CAST(0x0000A2E600D8EF17 AS DateTime), NULL, 0, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8640, 1264, N'CRKEY', 3, CAST(0x0000A2E600D8EF17 AS DateTime), NULL, 0, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8641, 1263, N'CRKEY', 3, CAST(0x0000A2E600D8EF17 AS DateTime), NULL, 0, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8642, 1319, N'CRKEY', 10, CAST(0x0000A2E600D8EF17 AS DateTime), NULL, 0, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8643, 1163, N'CRKEY', 10, CAST(0x0000A2E600D8EF17 AS DateTime), NULL, 0, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8644, 1261, N'CRKEY', 10, CAST(0x0000A2E600D8EF17 AS DateTime), NULL, 0, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8645, 1455, N'CRKEY', 10, CAST(0x0000A2E600D8EF17 AS DateTime), NULL, 0, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8646, 1120, N'CRKEY', 11, CAST(0x0000A2E600D8EF17 AS DateTime), NULL, 0, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8647, 1178, N'CRKEY', 11, CAST(0x0000A2E600D8EF17 AS DateTime), NULL, 0, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8649, 1492, N'CRKEY', 10, CAST(0x0000A30000000000 AS DateTime), NULL, 1492, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (7914, 1124, N'SSHELP', 0, CAST(0x0000A00400000000 AS DateTime), NULL, 1124, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (7915, 1152, N'SSHELP', 0, CAST(0x0000A00400000000 AS DateTime), NULL, 1124, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (7918, 1124, N'QSTATS', 0, CAST(0x00009FF500000000 AS DateTime), NULL, 1124, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (7919, 1124, N'QSTATS', 0, CAST(0x0000A00400000000 AS DateTime), NULL, 1124, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (7920, 1124, N'QSTATS', 0, CAST(0x0000A00400000000 AS DateTime), NULL, 1124, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (7921, 1124, N'QSTATS', 0, CAST(0x0000A00400000000 AS DateTime), NULL, 1124, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (7922, 1124, N'QSTATS', 0, CAST(0x0000A00400000000 AS DateTime), NULL, 1124, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (7923, 1124, N'QSTATS', 0, CAST(0x0000A00400000000 AS DateTime), NULL, 1124, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (7924, 1124, N'QSTATS', 0, CAST(0x0000A00400000000 AS DateTime), NULL, 1124, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (7925, 1124, N'QSTATS', 0, CAST(0x0000A00400000000 AS DateTime), NULL, 1124, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (7926, 1124, N'QSTATS', 0, CAST(0x0000A00400000000 AS DateTime), NULL, 1124, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (7927, 1124, N'QSTATS', 0, CAST(0x0000A00400000000 AS DateTime), NULL, 1124, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (7928, 1124, N'QSTATS', 0, CAST(0x0000A00400000000 AS DateTime), NULL, 1124, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (7929, 1124, N'QSTATS', 0, CAST(0x0000A00400000000 AS DateTime), NULL, 1124, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (7930, 1124, N'QSTATS', 0, CAST(0x0000A00400000000 AS DateTime), NULL, 1124, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (7933, 1124, N'QSTATS Error Mail', 0, CAST(0x0000A00600000000 AS DateTime), NULL, 1124, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (7934, 1152, N'QSTATS Error Mail', 0, CAST(0x0000A00600000000 AS DateTime), NULL, 1124, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8023, 1124, N'ARC Add Error', 0, CAST(0x0000A00700000000 AS DateTime), NULL, 1124, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8581, 1198, N'Rtn Mail', 0, CAST(0x0000A09000000000 AS DateTime), NULL, 1280, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8582, 1256, N'Rtn Mail', 0, CAST(0x0000A09000000000 AS DateTime), NULL, 1280, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8583, 1175, N'Rtn Mail', 0, CAST(0x0000A09000000000 AS DateTime), NULL, 1280, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8586, 1178, N'Rtn Mail', 0, CAST(0x0000A0A800000000 AS DateTime), NULL, 1280, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8603, 1178, N'Rtn Mail', 0, CAST(0x0000A10200000000 AS DateTime), NULL, 1276, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8604, 1120, N'Rtn Mail', 0, CAST(0x0000A10200000000 AS DateTime), NULL, 1276, NULL)
INSERT [dbo].[SYSA_DAT_UserKeyAssignment] ([ID], [SqlUserId], [UserKey], [BusinessUnit], [StartDate], [EndDate], [AddedBy], [RemovedBy]) VALUES (8605, 1152, N'Rtn Mail', 0, CAST(0x0000A10200000000 AS DateTime), NULL, 1276, NULL)
SET IDENTITY_INSERT [dbo].[SYSA_DAT_UserKeyAssignment] OFF
