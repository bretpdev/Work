USE [NeedHelpCornerStone]
GO
/****** Object:  Table [dbo].[DAT_Ticket]    Script Date: 04/09/2014 16:25:45 ******/
ALTER TABLE [dbo].[DAT_Ticket] DROP CONSTRAINT [DF_NDHP_DAT_Tickets_Subject]
GO
ALTER TABLE [dbo].[DAT_Ticket] DROP CONSTRAINT [DF_NDHP_DAT_Tickets_Requested]
GO
ALTER TABLE [dbo].[DAT_Ticket] DROP CONSTRAINT [DF_NDHP_DAT_Tickets_Area]
GO
ALTER TABLE [dbo].[DAT_Ticket] DROP CONSTRAINT [DF_NDHP_DAT_Tickets_Issue]
GO
ALTER TABLE [dbo].[DAT_Ticket] DROP CONSTRAINT [DF_NDHP_DAT_Tickets_Resolution]
GO
ALTER TABLE [dbo].[DAT_Ticket] DROP CONSTRAINT [DF_NDHP_DAT_Tickets_ResolutionFix]
GO
ALTER TABLE [dbo].[DAT_Ticket] DROP CONSTRAINT [DF_NDHP_DAT_Tickets_ResolutionPreventiion]
GO
ALTER TABLE [dbo].[DAT_Ticket] DROP CONSTRAINT [DF_NDHP_DAT_Tickets_Status]
GO
ALTER TABLE [dbo].[DAT_Ticket] DROP CONSTRAINT [DF_NDHP_DAT_Tickets_StatusDate]
GO
ALTER TABLE [dbo].[DAT_Ticket] DROP CONSTRAINT [DF_NDHP_DAT_Tickets_CourtDate]
GO
ALTER TABLE [dbo].[DAT_Ticket] DROP CONSTRAINT [DF_NDHP_DAT_Tickets_IssueUpdate]
GO
ALTER TABLE [dbo].[DAT_Ticket] DROP CONSTRAINT [DF_NDHP_DAT_Tickets_History]
GO
ALTER TABLE [dbo].[DAT_Ticket] DROP CONSTRAINT [DF_NDHP_DAT_Tickets_PreviousStatus]
GO
ALTER TABLE [dbo].[DAT_Ticket] DROP CONSTRAINT [DF_NDHP_DAT_Tickets_Urgency]
GO
ALTER TABLE [dbo].[DAT_Ticket] DROP CONSTRAINT [DF_NDHP_DAT_Tickets_Cat]
GO
ALTER TABLE [dbo].[DAT_Ticket] DROP CONSTRAINT [DF_NDHP_DAT_Tickets_LastUpdated]
GO
ALTER TABLE [dbo].[DAT_Ticket] DROP CONSTRAINT [DF_NDHP_DAT_Tickets_CCCIssue]
GO
ALTER TABLE [dbo].[DAT_Ticket] DROP CONSTRAINT [DF_NDHP_DAT_Tickets_RequestProjectNum]
GO
DROP TABLE [dbo].[DAT_Ticket]
GO
/****** Object:  Table [dbo].[DAT_Ticket]    Script Date: 04/09/2014 16:25:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DAT_Ticket](
	[Ticket] [bigint] IDENTITY(1,1) NOT NULL,
	[TicketCode] [varchar](50) NOT NULL,
	[Subject] [varchar](50) NULL CONSTRAINT [DF_NDHP_DAT_Tickets_Subject]  DEFAULT (''),
	[Requested] [datetime] NULL CONSTRAINT [DF_NDHP_DAT_Tickets_Requested]  DEFAULT (getdate()),
	[Unit] [int] NULL,
	[Area] [varchar](100) NULL CONSTRAINT [DF_NDHP_DAT_Tickets_Area]  DEFAULT (''),
	[Required] [datetime] NULL,
	[Issue] [text] NULL CONSTRAINT [DF_NDHP_DAT_Tickets_Issue]  DEFAULT (''),
	[ResolutionCause] [varchar](50) NULL CONSTRAINT [DF_NDHP_DAT_Tickets_Resolution]  DEFAULT (''),
	[ResolutionFix] [text] NULL CONSTRAINT [DF_NDHP_DAT_Tickets_ResolutionFix]  DEFAULT (''),
	[ResolutionPrevention] [text] NULL CONSTRAINT [DF_NDHP_DAT_Tickets_ResolutionPreventiion]  DEFAULT (''),
	[Status] [varchar](50) NULL CONSTRAINT [DF_NDHP_DAT_Tickets_Status]  DEFAULT ('Submitting'),
	[StatusDate] [datetime] NULL CONSTRAINT [DF_NDHP_DAT_Tickets_StatusDate]  DEFAULT (getdate()),
	[CourtDate] [datetime] NULL CONSTRAINT [DF_NDHP_DAT_Tickets_CourtDate]  DEFAULT (getdate()),
	[IssueUpdate] [text] NULL CONSTRAINT [DF_NDHP_DAT_Tickets_IssueUpdate]  DEFAULT (''),
	[History] [text] NULL CONSTRAINT [DF_NDHP_DAT_Tickets_History]  DEFAULT (''),
	[PreviousStatus] [varchar](50) NULL CONSTRAINT [DF_NDHP_DAT_Tickets_PreviousStatus]  DEFAULT (''),
	[UrgencyOption] [varchar](200) NULL CONSTRAINT [DF_NDHP_DAT_Tickets_Urgency]  DEFAULT (''),
	[CatOption] [varchar](200) NULL CONSTRAINT [DF_NDHP_DAT_Tickets_Cat]  DEFAULT (''),
	[Priority] [smallint] NULL,
	[LastUpdated] [datetime] NULL CONSTRAINT [DF_NDHP_DAT_Tickets_LastUpdated]  DEFAULT (getdate()),
	[CCCIssue] [varchar](20) NULL CONSTRAINT [DF_NDHP_DAT_Tickets_CCCIssue]  DEFAULT (''),
	[RequestProjectNum] [varchar](50) NULL CONSTRAINT [DF_NDHP_DAT_Tickets_RequestProjectNum]  DEFAULT (''),
	[Comments] [text] NULL,
	[RelatedQCIssues] [varchar](100) NULL,
	[RelatedProcedures] [varchar](1000) NULL,
 CONSTRAINT [PK_NDHP_DAT_Tickets] PRIMARY KEY CLUSTERED 
(
	[Ticket] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique ticket number.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DAT_Ticket', @level2type=N'COLUMN',@level2name=N'Ticket'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Three-character ticket type code.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DAT_Ticket', @level2type=N'COLUMN',@level2name=N'TicketCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short description of ticket issue.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DAT_Ticket', @level2type=N'COLUMN',@level2name=N'Subject'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date the ticket was requested.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DAT_Ticket', @level2type=N'COLUMN',@level2name=N'Requested'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the principal functional area associated with the ticket.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DAT_Ticket', @level2type=N'COLUMN',@level2name=N'Area'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date by which resolution of the issue is required.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DAT_Ticket', @level2type=N'COLUMN',@level2name=N'Required'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of the issue.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DAT_Ticket', @level2type=N'COLUMN',@level2name=N'Issue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DAT_Ticket', @level2type=N'COLUMN',@level2name=N'ResolutionCause'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Current status of the ticket.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DAT_Ticket', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Effective date of current status.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DAT_Ticket', @level2type=N'COLUMN',@level2name=N'StatusDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Effective date of current court.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DAT_Ticket', @level2type=N'COLUMN',@level2name=N'CourtDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Free-form text to add to ticket history.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DAT_Ticket', @level2type=N'COLUMN',@level2name=N'IssueUpdate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ticket history.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DAT_Ticket', @level2type=N'COLUMN',@level2name=N'History'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Previous status of the ticket.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DAT_Ticket', @level2type=N'COLUMN',@level2name=N'PreviousStatus'
GO
SET IDENTITY_INSERT [dbo].[DAT_Ticket] ON
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (136, N'DCR Fed', N'Testing System', CAST(0x0000A00000EBF17C AS DateTime), 30, N'', CAST(0x0000A00000000000 AS DateTime), N'Submitting with system', N'', N'', N'', N'Verified', CAST(0x0000A00700000000 AS DateTime), CAST(0x0000A00700000000 AS DateTime), N'', N'Bret Pehrson - 03/01/2012 10:24 AM - Verified

Verified

Bret Pehrson - 03/01/2012 10:24 AM - Complete

Complete

Bret Pehrson - 03/01/2012 10:22 AM - In Progress

approved

Bret Pehrson - 02/23/2012 02:44 PM - DCR Approval

test

 - 02/23/2012 02:19 PM - DCR Approval

submitting with system

Issue:
Submitting with system
', N'Complete', N'', N'Executive Request', 9, CAST(0x0000A00700AB4EA4 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (137, N'DCR Fed', N'Data Spot DCR', CAST(0x0000A000010C2D05 AS DateTime), 30, N'', CAST(0x0000A00000000000 AS DateTime), N'Testing System', N'', N'', N'', N'Verified', CAST(0x0000A00600000000 AS DateTime), CAST(0x0000A00600000000 AS DateTime), N'', N'Bret Pehrson - 02/29/2012 02:20 PM - Verified

Verified

Bret Pehrson - 02/29/2012 02:19 PM - Complete

complete

Bret Pehrson - 02/29/2012 02:19 PM - In Progress

approve

Bret Pehrson - 02/23/2012 04:19 PM - DCR Approval

No problems with System

 - 02/23/2012 04:16 PM - DCR Approval

Testing System

Issue:
Testing System
', N'Complete', N'', N'Executive Request', 9, CAST(0x0000A00600EC36ED AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (138, N'ACCM Fed', N'Test', CAST(0x0000A00500A5A7E6 AS DateTime), 35, N'', CAST(0x0000A00500000000 AS DateTime), N'Test', N'', N'', N'', N'Verified', CAST(0x0000A00500000000 AS DateTime), CAST(0x0000A00500000000 AS DateTime), N'', N'Parish Snyder iv - 02/28/2012 09:31 PM - Verified

Test, stayed in us court for verified

Parish Snyder iv - 02/28/2012 09:31 PM - Complete

Test, moved to ss an, complete

Parish Snyder iv - 02/28/2012 09:31 PM - In Progress

Test, moved to ss man. Approved

Parish Snyder iv - 02/28/2012 09:30 PM - Approval

Issue:
Test
', N'Complete', N'Improves Operational Efficiency', N'Executive Request', 9, CAST(0x0000A0050162A21A AS DateTime), N'', N'', N'Test', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (139, N'ACTD Fed', N'Test', CAST(0x0000A00500A61788 AS DateTime), 35, N'', CAST(0x0000A00500000000 AS DateTime), N'Test', N'', N'', N'', N'Verified', CAST(0x0000A00500000000 AS DateTime), CAST(0x0000A00500000000 AS DateTime), N'', N'Parish Snyder iv - 02/28/2012 10:07 AM - Verified

Test, move to diff ss an and verified

Parish Snyder iv - 02/28/2012 10:07 AM - Complete

Test, moved to ss an. move to complete

Parish Snyder iv - 02/28/2012 10:06 AM - In Progress

Test, move to approved

Parish Snyder iv - 02/28/2012 10:06 AM - Approval

Test, move to reviewed

 - 02/28/2012 10:05 AM - Review

Issue:
Test
', N'Complete', N'Impacts Small Number of Customers', N'Get in Compliance', 3, CAST(0x0000A00500A6C184 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (140, N'ARCA Fed', N'Test', CAST(0x0000A00500A6EB99 AS DateTime), 35, N'', CAST(0x0000A00500000000 AS DateTime), N'Test', N'', N'', N'', N'Verified', CAST(0x0000A00500000000 AS DateTime), CAST(0x0000A03F00000000 AS DateTime), N'', N'Parish Snyder iv - 02/28/2012 10:22 AM - Verified

Test, move to verified

Parish Snyder iv - 02/28/2012 10:22 AM - Complete

Test, move to complete

Parish Snyder iv - 02/28/2012 10:22 AM - In Progress

Test, move to approved

 - 02/28/2012 10:21 AM - DS Approval

Issue:
Test
', N'Complete', N'Impacts Large Number of Customers', N'Get in Compliance', 7, CAST(0x0000A03F00988176 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (141, N'ARCC Fed', N'Test', CAST(0x0000A00500AB093A AS DateTime), 35, N'', CAST(0x0000A00500000000 AS DateTime), N'Test', N'', N'', N'', N'Verified', CAST(0x0000A00500000000 AS DateTime), CAST(0x0000A03E00000000 AS DateTime), N'', N'Parish Snyder iv - 02/28/2012 10:28 AM - Verified

Test, moved to diff ss an. Moving to verified

Parish Snyder iv - 02/28/2012 10:28 AM - Complete

Test, moved to ss an. Moving to complete

Parish Snyder iv - 02/28/2012 10:27 AM - In Progress

Test, move to approved

 - 02/28/2012 10:27 AM - Approval

Issue:
Test
', N'Complete', N'Impacts Significant Number of Customers', N'Improve Employee Morale', 5, CAST(0x0000A03E00A10F32 AS DateTime), N'', N'', N'Test', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (142, N'ARCM Fed', N'Test', CAST(0x0000A00500AC9C2B AS DateTime), 35, N'', CAST(0x0000A00500000000 AS DateTime), N'Test', N'', N'', N'', N'Verified', CAST(0x0000A00500000000 AS DateTime), CAST(0x0000A00500000000 AS DateTime), N'', N'Parish Snyder iv - 02/28/2012 10:30 AM - Verified

Test, moved to ss an, complete. Moving to verified

Parish Snyder iv - 02/28/2012 10:30 AM - Complete

Test, moved to ss an. Moving to complete. 

Parish Snyder iv - 02/28/2012 10:30 AM - In Progress

Test, moved to approval. Moving to approved

 - 02/28/2012 10:30 AM - Approval

Issue:
Test
', N'Complete', N'Impacts Significant Number of Customers', N'Improve Employee Morale', 5, CAST(0x0000A00500AD3547 AS DateTime), N'', N'', N'Test', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (143, N'BBO Fed', N'Test', CAST(0x0000A00500AD4E30 AS DateTime), 35, N'', CAST(0x0000A00500000000 AS DateTime), N'Test', N'', N'', N'', N'Complete and Verified', CAST(0x0000A00500000000 AS DateTime), CAST(0x0000A00500000000 AS DateTime), N'', N'Parish Snyder iv - 02/28/2012 10:37 AM - Complete and Verified

Test, moved to complete. Moving to verified

Parish Snyder iv - 02/28/2012 10:33 AM - Complete

Test, moved to ss an in prog. Moving to complete

Parish Snyder iv - 02/28/2012 10:32 AM - In Progress

Test, moved to ss man for app. Moving to approved

 - 02/28/2012 10:32 AM - Approval

Issue:
Test
', N'Complete', N'Impacts Significant Number of Customers', N'Improve Employee Morale', 5, CAST(0x0000A00500AEF8E0 AS DateTime), N'', N'', N'Test', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (144, N'DCR Fed', N'test', CAST(0x0000A00500AF1816 AS DateTime), 35, N'', CAST(0x0000A00500000000 AS DateTime), N'Test', N'', N'', N'', N'Verified', CAST(0x0000A03F00000000 AS DateTime), CAST(0x0000A03F00000000 AS DateTime), N'', N'Parish Snyder iv - 04/26/2012 01:48 PM - Verified

Test

Parish Snyder iv - 04/26/2012 01:48 PM - Complete

Test

Parish Snyder iv - 04/26/2012 01:48 PM - In Progress

Test

Parish Snyder iv - 04/26/2012 01:47 PM - DCR Approval

Releasing from hold: 

Test

Parish Snyder iv - 04/20/2012 02:03 PM - Hold

Ticket Placed On Hold: 

Test HOLD

 - 02/28/2012 10:41 AM - DCR Approval

Issue:
Test
', N'Complete', N'Improves Operational Efficiency', N'Get in Compliance', 7, CAST(0x0000A03F00E381F8 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (145, N'FAR Fed', N'Test', CAST(0x0000A00500B2213B AS DateTime), 35, N'', CAST(0x0000A00500000000 AS DateTime), N'Test', N'', N'', N'', N'Resolved', CAST(0x0000A00500000000 AS DateTime), CAST(0x0000A00500000000 AS DateTime), N'', N'Parish Snyder iv - 02/28/2012 10:53 AM - Resolved

Test, moved to qc app. QC app

Parish Snyder iv - 02/28/2012 10:53 AM - QC Approval

Test, moved to opa in prog. Adj complete.

Parish Snyder iv - 02/28/2012 10:53 AM - OPA - In Progress

Test, moved to BS APP, BA. Approving

 - 02/28/2012 10:52 AM - BS Approval

Issue:
Test
', N'QC Approval', N'Improves Operational Efficiency', N'Other Efficiency', 4, CAST(0x0000A00500B38573 AS DateTime), N'', N'', N'Test', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (146, N'FTRANS Fed', N'Test', CAST(0x0000A00500B3E0C2 AS DateTime), 35, N'', CAST(0x0000A00500000000 AS DateTime), N'Test', N'', N'', N'', N'Completed', CAST(0x0000A03900000000 AS DateTime), CAST(0x0000A03900000000 AS DateTime), N'', N'Parish Snyder iv - 04/20/2012 02:11 PM - Completed

Test

Parish Snyder iv - 04/20/2012 02:10 PM - QC Approval

Test

Parish Snyder iv - 04/20/2012 02:10 PM - OPA - In Progress

Issue:
Test
', N'QC Approval', N'Impacts Small Number of Customers', N'Improve Employee Morale', 1, CAST(0x0000A03900E9B6E4 AS DateTime), N'', N'', N'Test', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (147, N'DCR Fed', N'All New Category', CAST(0x0000A00500C9127B AS DateTime), 2, N'', CAST(0x0000A07C00000000 AS DateTime), N'test', N'', N'', N'', N'In Progress', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10200000000 AS DateTime), N'', N'Sasha Vanorman - 11/13/2012 01:55 PM - In Progress

test

Sasha Vanorman - 11/13/2012 01:54 PM - test

est

Sasha Vanorman - 11/13/2012 01:53 PM - In Progress

test

Sasha Vanorman - 11/13/2012 12:59 PM - test

test

Sasha Vanorman - 11/13/2012 12:59 PM - test

test

Sasha Vanorman - 11/13/2012 12:59 PM - Hold

Ticket Placed On Hold: 

test

Sasha Vanorman - 11/13/2012 12:55 PM - test

test

Sasha Vanorman - 11/13/2012 12:55 PM - test

update

Sasha Vanorman - 11/13/2012 12:54 PM - Hold

test

Sasha Vanorman - 11/13/2012 12:54 PM - Hold

Ticket Placed On Hold: 

really placing on hold

Sasha Vanorman - 11/13/2012 12:54 PM - In Progress

placing on hold

Sasha Vanorman - 11/13/2012 12:54 PM - In Progress

test

Sasha Vanorman - 11/13/2012 12:53 PM - In Progress

44444

Bret Pehrson - 11/07/2012 03:40 PM - In Progress

test

Bret Pehrson - 11/07/2012 03:39 PM - DCR Approval

test

Issue:
test
', N'In Progress', N'', N'Executive Request', 9, CAST(0x0000A10800E520C3 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (148, N'IDEM Fed', N'Test', CAST(0x0000A005012E727E AS DateTime), 51, N'', CAST(0x0000A00500000000 AS DateTime), N'Test', N'', N'', N'', N'Verified', CAST(0x0000A00500000000 AS DateTime), CAST(0x0000A00500000000 AS DateTime), N'', N'Parish Snyder iv - 02/28/2012 08:07 PM - Verified

Test. Moved to ss an. Verified

Parish Snyder iv - 02/28/2012 08:07 PM - Complete

Test. Moved to ss an assign. Complete

Parish Snyder iv - 02/28/2012 08:06 PM - In Progress

Test. Moved to ss man. approved

Parish Snyder iv - 02/28/2012 08:06 PM - Approval

Issue:
Test
', N'Complete', N'Improves Operational Efficiency', N'Resolve Customer Service Issue', 6, CAST(0x0000A005014B84C0 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (149, N'LPDS Fed', N'Test', CAST(0x0000A005014C009E AS DateTime), 35, N'', CAST(0x0000A00500000000 AS DateTime), N'Test', N'', N'', N'', N'Verified', CAST(0x0000A00500000000 AS DateTime), CAST(0x0000A03F00000000 AS DateTime), N'', N'Parish Snyder iv - 02/28/2012 08:10 PM - Verified

Test. Moved to ss an, Verified

Parish Snyder iv - 02/28/2012 08:09 PM - Complete

Test. Moved to ss an. Complete

Parish Snyder iv - 02/28/2012 08:09 PM - In Progress

Test. Moved to  ss man. Approved

 - 02/28/2012 08:09 PM - Approval

Issue:
Test
', N'Complete', N'Other High Urgency', N'Get in Compliance', 9, CAST(0x0000A03F00986149 AS DateTime), N'', N'', N'Test', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (150, N'OTH Fed', N'Test', CAST(0x0000A005014C6493 AS DateTime), 35, N'Security', CAST(0x0000A00500000000 AS DateTime), N'Test', N'SAS/Script Problem', N'asdf', N'asdf', N'Resolved', CAST(0x0000A10900000000 AS DateTime), CAST(0x0000A10900000000 AS DateTime), N'', N'Parish Snyder iv - 11/14/2012 12:49 PM - Resolved

TEST

Cause:
SAS/Script Problem

Fix:
asdf

Prevention:
asdf


Parish Snyder iv - 11/14/2012 12:48 PM - BS Approval

Court changed from Brenda Cox to Parish Snyder iv

Parish Snyder iv - 11/14/2012 12:33 PM - BS Approval

Test. Moved to ss an, discussion. 

Cause:
SAS/Script Problem

Fix:
asdf

Prevention:
asdf


 - 02/28/2012 08:11 PM - Discussion

Issue:
Test
', N'BS Approval', N'Other High Urgency', N'Improve Employee Morale', 5, CAST(0x0000A10900D2F123 AS DateTime), N'4534534534', N'4753', N'', N'7537554', N'45373977')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (151, N'POL Fed', N'Test', CAST(0x0000A00501503621 AS DateTime), 35, N'Bankruptcy', CAST(0x0000A00500000000 AS DateTime), N'Test', N'Staff Error', N'asdf', N'asdf', N'Resolved', CAST(0x0000A00500000000 AS DateTime), CAST(0x0000A04A00000000 AS DateTime), N'', N'Parish Snyder iv - 02/28/2012 08:27 PM - Resolved

Moved to BS APP, BC.  Appr

Cause:
Staff Error

Fix:
asdf

Prevention:
asdf


Parish Snyder iv - 02/28/2012 08:27 PM - BS Approval

Test, moved to discussion, TV. Resolution

Cause:
Staff Error

Fix:
asdf

Prevention:
asdf


Parish Snyder iv - 02/28/2012 08:26 PM - Discussion

Issue:
Test
', N'BS Approval', N'Improves Operational Efficiency', N'Executive Request', 9, CAST(0x0000A04A00AB1577 AS DateTime), N'2345', N'2346', N'', N'2456', N'2346')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (152, N'QASMT Fed', N'Test', CAST(0x0000A00501508628 AS DateTime), 35, N'', CAST(0x0000A00500000000 AS DateTime), N'Test', N'', N'', N'', N'Verified', CAST(0x0000A00500000000 AS DateTime), CAST(0x0000A00500000000 AS DateTime), N'', N'Parish Snyder iv - 02/28/2012 09:26 PM - Verified

Test, moved to sep ss an. verified

Parish Snyder iv - 02/28/2012 09:26 PM - Complete

Test, moved to ss an. complete

Parish Snyder iv - 02/28/2012 09:26 PM - In Progress

Test. moved to ss man, approved

Parish Snyder iv - 02/28/2012 09:26 PM - Approval

Issue:
Test
', N'Complete', N'Impacts Small Number of Customers', N'Improve Employee Morale', 1, CAST(0x0000A00501615660 AS DateTime), N'', N'', N'Test', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (153, N'QCR Fed', N'Test', CAST(0x0000A0050161760E AS DateTime), 35, N'', CAST(0x0000A00500000000 AS DateTime), N'Test', N'', N'', N'', N'Verified', CAST(0x0000A00500000000 AS DateTime), CAST(0x0000A00500000000 AS DateTime), N'', N'Parish Snyder iv - 02/28/2012 09:32 PM - Verified

Test, moved to ss an, verified

Parish Snyder iv - 02/28/2012 09:32 PM - Complete

Test, moved to ss an, complete

Parish Snyder iv - 02/28/2012 09:32 PM - In Progress

Test, moved to ss man, approved

Parish Snyder iv - 02/28/2012 09:32 PM - Approval

Issue:
Test
', N'Complete', N'Impacts Large Number of Customers', N'Improve Customer Service', 6, CAST(0x0000A0050162EBD9 AS DateTime), N'', N'', N'Test', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (154, N'SPH Fed', N'Test', CAST(0x0000A00600C1EEBE AS DateTime), 35, N'Autodialer', CAST(0x0000A00600000000 AS DateTime), N'Test', N'SAS/Script Problem', N'asdf', N'asdf', N'Discussion', CAST(0x0000A00600000000 AS DateTime), CAST(0x0000A01D00000000 AS DateTime), N'', N'Bret Pehrson - 03/23/2012 11:21 AM - Discussion

testing update

Cause:
SAS/Script Problem

Fix:
asdf

Prevention:
asdf


Parish Snyder iv - 03/23/2012 11:04 AM - Discussion

Moving to Bret

Cause:
SAS/Script Problem

Fix:
asdf

Prevention:
asdf


Bret Pehrson - 02/29/2012 11:56 AM - Discussion

Moving to Melanie

Parish Snyder iv - 02/29/2012 11:50 AM - Discussion

Test, asked Mel a quesiton. Placing in her court. 


Parish Snyder iv - 02/29/2012 11:50 AM - Discussion

Test, asked Mel a quesiton. Placing in her court. 


Parish Snyder iv - 02/29/2012 11:49 AM - Discussion

Test, asked Mel a quesiton. Placing in her court. 


Parish Snyder iv - 02/29/2012 11:48 AM - Discussion

Test, asked Mel a quesiton. Placing in her court. 


Parish Snyder iv - 02/29/2012 11:48 AM - Discussion

Test, asked Mel a quesiton. Placing in her court. 

 - 02/29/2012 11:47 AM - Discussion

Issue:
Test
', N'Submitting', N'Impacts Small Number of Customers', N'Get in Compliance', 3, CAST(0x0000A01D00BB0362 AS DateTime), N'8974', N'984', N'', N'984', N'984')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (155, N'DCR Fed', N'test', CAST(0x0000A00600EC6C3C AS DateTime), 2, N'', CAST(0x0000A00600000000 AS DateTime), N'test', N'', N'', N'', N'In Progress', CAST(0x0000A10200000000 AS DateTime), CAST(0x0000A10200000000 AS DateTime), N'', N'Sasha Vanorman - 11/07/2012 12:52 PM - In Progress

Parish Snyder iv - 10/11/2012 02:07 PM - DCR Approval

Update

Parish Snyder iv - 10/11/2012 02:07 PM - DCR Approval

Update

Parish Snyder iv - 10/11/2012 02:07 PM - DCR Approval

Update

Bret Pehrson - 02/29/2012 02:21 PM - DCR Approval

test

Issue:
test
', N'DCR Approval', N'', N'Executive Request', 9, CAST(0x0000A10200D40D85 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (156, N'DCR Fed', N'test', CAST(0x0000A00600EFA02A AS DateTime), 2, N'', CAST(0x0000A00600000000 AS DateTime), N'test', N'', N'', N'', N'In Progress', CAST(0x0000A10200000000 AS DateTime), CAST(0x0000A10200000000 AS DateTime), N'', N'Sasha Vanorman - 11/07/2012 12:51 PM - In Progress

Sasha Vanorman - 10/09/2012 06:01 PM - DCR Approval

testing

 - 02/29/2012 02:33 PM - DCR Approval

test

Issue:
test
', N'DCR Approval', N'', N'Executive Request', 9, CAST(0x0000A10200D3EC4F AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (157, N'DCR Fed', N'test', CAST(0x0000A00600F20733 AS DateTime), 2, N'', CAST(0x0000A00600000000 AS DateTime), N'test', N'', N'', N'', N'Verified', CAST(0x0000A00600000000 AS DateTime), CAST(0x0000A00600000000 AS DateTime), N'', N'Bret Pehrson - 02/29/2012 02:42 PM - Verified

verified

Bret Pehrson - 02/29/2012 02:42 PM - Complete

complete

Bret Pehrson - 02/29/2012 02:42 PM - In Progress

approve

 - 02/29/2012 02:41 PM - DCR Approval

test

Issue:
test
', N'Complete', N'', N'Executive Request', 9, CAST(0x0000A00600F2468E AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (158, N'DCR Fed', N'Testing DCR', CAST(0x0000A0070099A703 AS DateTime), 2, N'', CAST(0x0000A00700000000 AS DateTime), N'Testing DCR', N'', N'', N'', N'Verified', CAST(0x0000A00700000000 AS DateTime), CAST(0x0000A00700000000 AS DateTime), N'', N'Bret Pehrson - 03/01/2012 09:20 AM - Verified

Testing Verified

Bret Pehrson - 03/01/2012 09:20 AM - Complete

Testing Complete

Bret Pehrson - 03/01/2012 09:20 AM - In Progress

Testing Approve

 - 03/01/2012 09:20 AM - DCR Approval

TEST

Issue:
Testing DCR
', N'Complete', N'Creates Significant Operational Efficiency', N'Data Integrity', 9, CAST(0x0000A0070099CE20 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (159, N'DCR Fed', N'Clear Out Court', CAST(0x0000A00700FD82FD AS DateTime), 2, N'', CAST(0x0000A00700000000 AS DateTime), N'test', N'', N'', N'', N'Submitting', CAST(0x0000A03900000000 AS DateTime), CAST(0x0000A03900000000 AS DateTime), N'', N'Kathryn Ferre - 09/07/2012 09:18 AM - Submitting

test update

Parish Snyder iv - 04/20/2012 03:39 PM - Submitting

test

Bret Pehrson - 03/01/2012 03:24 PM - Complete

Complete

Bret Pehrson - 03/01/2012 03:24 PM - In Progress

Approve

 - 03/01/2012 03:24 PM - DCR Approval

test

Issue:
test
', N'Complete', N'', N'Executive Request', 9, CAST(0x0000A0C500996BBE AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (160, N'FAR Fed', N'Test', CAST(0x0000A00F00B40572 AS DateTime), 35, N'', CAST(0x0000A00F00000000 AS DateTime), N'Test', N'', N'', N'', N'Resolved', CAST(0x0000A00F00000000 AS DateTime), CAST(0x0000A00F00000000 AS DateTime), N'', N'Parish Snyder iv - 03/09/2012 11:02 AM - Resolved

Test

Parish Snyder iv - 03/09/2012 10:56 AM - QC Approval

Test adjustment complete

Parish Snyder iv - 03/09/2012 10:56 AM - OPA - In Progress

Test

 - 03/09/2012 10:56 AM - BS Approval

Issue:
Test
', N'QC Approval', N'Impacts Small Number of Customers', N'Executive Request', 9, CAST(0x0000A00F00B5CBF1 AS DateTime), N'', N'', N'Test', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (161, N'FAR Fed', N'Test', CAST(0x0000A00F00BC8306 AS DateTime), 35, N'', CAST(0x0000A02100000000 AS DateTime), N'Test', N'', N'', N'', N'Resolved', CAST(0x0000A0EC00000000 AS DateTime), CAST(0x0000A0EC00000000 AS DateTime), N'', N'Sasha Vanorman - 10/16/2012 03:07 PM - Resolved

Sasha Vanorman - 10/16/2012 03:04 PM - QC Approval

Bret Pehrson - 04/26/2012 12:02 PM - OPA - In Progress

test

Parish Snyder iv - 04/25/2012 12:34 PM - OPA - In Progress

TEST-Placing in Bret''s court.

Parish Snyder iv - 03/27/2012 01:52 PM - OPA - In Progress

Test

Parish Snyder iv - 03/27/2012 01:52 PM - BS Approval

Issue:
Test
', N'QC Approval', N'Provides Small Cost Benefit', N'Provide New Service', 2, CAST(0x0000A0EC00F909DA AS DateTime), N'', N'', N'Test', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (162, N'SPH Fed', N'', CAST(0x0000A01D00B6A033 AS DateTime), 13, N'', CAST(0x0000A03C00000000 AS DateTime), N'', N'', N'', N'', N'Withdrawn', CAST(0x0000A03C00000000 AS DateTime), CAST(0x0000A03C00000000 AS DateTime), N'', N'Parish Snyder iv - 04/23/2012 03:39 PM - Submitting

Ticket Withdrawn: 

TEST withdrawn
', N'Submitting', N'', N'', 0, CAST(0x0000A03C0101E6C4 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (163, N'SPH Fed', N'asdf', CAST(0x0000A01D00B6FB24 AS DateTime), 35, N'Autodialer', CAST(0x0000A01D00000000 AS DateTime), N'asdf', N'Process Complete', N'test', N'test', N'BS Approval', CAST(0x0000A01D00000000 AS DateTime), CAST(0x0000A01D00000000 AS DateTime), N'', N'Sasha Vanorman - 10/23/2012 03:30 PM - BS Approval

adding multiple systems

Cause:
Process Complete

Fix:
test

Prevention:
test


Sasha Vanorman - 10/23/2012 03:28 PM - BS Approval

adding myself to this ticket

Cause:
Process Complete

Fix:
test

Prevention:
test


Bret Pehrson - 03/23/2012 12:01 PM - BS Approval

Testing to see if I am in Discussion Status

Cause:
Process Complete

Fix:
test

Prevention:
test


Bret Pehrson - 03/23/2012 12:00 PM - Discussion

test

Cause:
Process Complete

Fix:
test

Prevention:
test


Bret Pehrson - 03/23/2012 12:00 PM - BS Approval

test

Cause:
Process Complete

Fix:
test

Prevention:
test


Bret Pehrson - 03/23/2012 11:59 AM - Discussion

test

Cause:
Process Complete

Fix:
test

Prevention:
test


Bret Pehrson - 03/23/2012 11:58 AM - BS Approval

test

Cause:
Process Complete

Fix:
test

Prevention:
test


Bret Pehrson - 03/23/2012 11:58 AM - Discussion

test

Cause:
Process Complete

Fix:
test

Prevention:
test


Bret Pehrson - 03/23/2012 11:56 AM - BS Approval

test

Cause:
Process Complete

Fix:
test

Prevention:
test


Bret Pehrson - 03/23/2012 11:56 AM - Discussion

test

Cause:
Process Complete

Fix:
test

Prevention:
test


Bret Pehrson - 03/23/2012 11:55 AM - BS Approval

test

Cause:
Process Complete

Fix:
test

Prevention:
test


Bret Pehrson - 03/23/2012 11:55 AM - Discussion

test

Cause:
Process Complete

Fix:
test

Prevention:
test


Bret Pehrson - 03/23/2012 11:52 AM - BS Approval

Testing Resolution

Cause:
Process Complete

Fix:
test

Prevention:
test


Bret Pehrson - 03/23/2012 11:52 AM - Discussion

test

Cause:
Process Complete

Fix:
test

Prevention:
test


Bret Pehrson - 03/23/2012 11:51 AM - BS Approval

Testing Resolution

Cause:
Process Complete

Fix:
test

Prevention:
test


Bret Pehrson - 03/23/2012 11:46 AM - Discussion

test

Bret Pehrson - 03/23/2012 11:44 AM - Discussion

test test

Bret Pehrson - 03/23/2012 11:43 AM - Discussion

testing update

Bret Pehrson - 03/23/2012 11:42 AM - Discussion

Test

Bret Pehrson - 03/23/2012 11:39 AM - Discussion

test

Bret Pehrson - 03/23/2012 11:25 AM - Discussion

Testing Update

Bret Pehrson - 03/23/2012 11:23 AM - Discussion

Placing in Bret''s court.

 - 03/23/2012 11:14 AM - Discussion

Issue:
asdf
', N'Discussion', N'Impacts Small Number of Customers', N'Improve Operational Efficiency', 1, CAST(0x0000A0F300FF60BA AS DateTime), N'67', N'4567', N'', N'4567', N'567')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (164, N'OTH Fed', N'test', CAST(0x0000A03100F295D1 AS DateTime), 3, N'', CAST(0x0000A03100000000 AS DateTime), N'test', N'', N'', N'', N'Submitting', CAST(0x0000A03100F295D1 AS DateTime), CAST(0x0000A03100F295D1 AS DateTime), N'', N'Bret Pehrson - 04/12/2012 02:46 PM - Submitting

test

 - 04/12/2012 02:44 PM - Submitting

test
', N'', N'', N'Executive Request', 9, CAST(0x0000A03100F335C1 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (165, N'FNC Fed', N'Test', CAST(0x0000A03900D8FD72 AS DateTime), 13, N'System Settings, Access & Maintenance', CAST(0x0000A03900000000 AS DateTime), N'This is a TEST', N'Process Complete', N'Test', N'Test', N'Resolved', CAST(0x0000A03900000000 AS DateTime), CAST(0x0000A03900000000 AS DateTime), N'', N'Parish Snyder iv - 04/20/2012 01:26 PM - Resolved

Test

Cause:
Process Complete

Fix:
Test

Prevention:
Test


Parish Snyder iv - 04/20/2012 01:26 PM - BS Approval

TEST

Cause:
Process Complete

Fix:
Test

Prevention:
Test


 - 04/20/2012 01:11 PM - Discussion

This is a TEST

Issue:
This is a TEST
', N'BS Approval', N'Impacts Small Number of Customers', N'Resolve Audit Finding', 3, CAST(0x0000A03900DD714D AS DateTime), N'9841981', N'', N'', N'618681', N'68161')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (166, N'PRB Fed', N'TEST', CAST(0x0000A03900DECE7E AS DateTime), 13, N'', CAST(0x0000A03900000000 AS DateTime), N'TEST', N'Training', N'Test', N'Test', N'Resolved', CAST(0x0000A03900000000 AS DateTime), CAST(0x0000A03900000000 AS DateTime), N'', N'Parish Snyder iv - 04/20/2012 01:44 PM - Resolved

Test

Cause:
Training

Fix:
Test

Prevention:
Test


Parish Snyder iv - 04/20/2012 01:44 PM - BS Approval

Cause:
Training

Fix:
Test

Prevention:
Test


Parish Snyder iv - 04/20/2012 01:43 PM - Discussion

Test

 - 04/20/2012 01:43 PM - Discussion

TEST

Issue:
TEST
', N'BS Approval', N'Other Low Urgency', N'Resolve Audit Finding', 3, CAST(0x0000A03900E251A3 AS DateTime), N'9681984', N'', N'', N'984984', N'9849494')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (167, N'TOC Fed', N'TEST', CAST(0x0000A03900E2A035 AS DateTime), 13, N'', CAST(0x0000A03900000000 AS DateTime), N'This is a TEST', N'', N'', N'', N'Verified', CAST(0x0000A03900000000 AS DateTime), CAST(0x0000A03900000000 AS DateTime), N'', N'Parish Snyder iv - 04/20/2012 01:48 PM - Verified

TEST

Parish Snyder iv - 04/20/2012 01:48 PM - Complete

TEST

Parish Snyder iv - 04/20/2012 01:46 PM - In Progress

TEST

Parish Snyder iv - 04/20/2012 01:46 PM - System Support Review

Test

 - 04/20/2012 01:45 PM - Review

Issue:
This is a TEST
', N'Complete', N'Other Low Urgency', N'Resolve Audit Finding', 3, CAST(0x0000A03900E37E8E AS DateTime), N'', N'', N'TEST', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (168, N'SCHELIG Fed', N'TEST', CAST(0x0000A03900EA5FC4 AS DateTime), 12, N'', CAST(0x0000A03900000000 AS DateTime), N'TEST', N'', N'', N'', N'Complete And Verified', CAST(0x0000A03900000000 AS DateTime), CAST(0x0000A03900000000 AS DateTime), N'', N'Parish Snyder iv - 04/20/2012 02:15 PM - Complete And Verified

TEST

Parish Snyder iv - 04/20/2012 02:15 PM - Complete

TEST

Parish Snyder iv - 04/20/2012 02:15 PM - In Progress

TEST

Parish Snyder iv - 04/20/2012 02:14 PM - Approval

TEST

 - 04/20/2012 02:14 PM - Confirmation Review

Issue:
TEST
', N'Complete', N'Impacts Small Number of Customers', N'Provide Cost Benefit', 1, CAST(0x0000A03900EACE90 AS DateTime), N'', N'', N'TEST', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (169, N'OTH Fed', N'This is a TEST', CAST(0x0000A03C00CD4330 AS DateTime), 13, N'Batch Scripts', CAST(0x0000A03C00000000 AS DateTime), N'This is a TEST', N'', N'', N'', N'Discussion', CAST(0x0000A03C00000000 AS DateTime), CAST(0x0000A03C00000000 AS DateTime), N'', N' - 04/23/2012 12:33 PM - Discussion

This is a TEST

Issue:
This is a TEST
', N'Submitting', N'Impacts Small Number of Customers', N'Other Efficiency', 1, CAST(0x0000A03C00CEA6A4 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (170, N'OTH Fed', N'This is a TEST', CAST(0x0000A03C00CEE1A0 AS DateTime), 13, N'Projects', CAST(0x0000A03C00000000 AS DateTime), N'This is a TEST', N'', N'', N'', N'Discussion', CAST(0x0000A03C00000000 AS DateTime), CAST(0x0000A03C00000000 AS DateTime), N'', N' - 04/23/2012 12:34 PM - Discussion

This is a TEST

Issue:
This is a TEST
', N'Submitting', N'Impacts Small Number of Customers', N'Other Efficiency', 1, CAST(0x0000A03C00CF1B75 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (171, N'OTH Fed', N'This is a TEST', CAST(0x0000A03C00CF77A2 AS DateTime), 13, N'Batch Scripts', CAST(0x0000A03C00000000 AS DateTime), N'This is a TEST', N'', N'', N'', N'Discussion', CAST(0x0000A03C00000000 AS DateTime), CAST(0x0000A03E00000000 AS DateTime), N'', N' - 04/23/2012 12:36 PM - Discussion

This is a TEST

Issue:
This is a TEST
', N'Submitting', N'Impacts Small Number of Customers', N'Other Efficiency', 1, CAST(0x0000A03E00BBE9A3 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (172, N'BBO Fed', N'This is a TEST', CAST(0x0000A03F00A090D8 AS DateTime), 35, N'', CAST(0x0000A03F00000000 AS DateTime), N'This is a TEST', N'', N'', N'', N'Approval', CAST(0x0000A03F00000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Sasha Vanorman - 11/13/2012 05:38 PM - Approval

Court changed from Sasha Vanorman to Parish Snyder iv

 - 04/26/2012 09:46 AM - Approval

Issue:
This is a TEST
', N'Submitting', N'Provides Small Cost Benefit', N'Other Customer Service', 2, CAST(0x0000A1080122867E AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (173, N'BBO Fed', N'test', CAST(0x0000A03F00A228A3 AS DateTime), 35, N'', CAST(0x0000A03F00000000 AS DateTime), N'test', N'', N'', N'', N'Submitting', CAST(0x0000A03F00A228A3 AS DateTime), CAST(0x0000A04A00000000 AS DateTime), N'', N'Bret Pehrson - 04/26/2012 11:34 AM - Submitting

test
', N'', N'', N'Executive Request', 9, CAST(0x0000A04A00A8157E AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (174, N'BBO Fed', N'', CAST(0x0000A03F00A2B94F AS DateTime), 35, N'', CAST(0x0000A12B00000000 AS DateTime), N'TEST', N'', N'', N'', N'Approval', CAST(0x0000A12B00000000 AS DateTime), CAST(0x0000A12B00000000 AS DateTime), N'', N'Parish Snyder iv - 12/18/2012 12:28 PM - Approval

TEST

Issue:
TEST

Parish Snyder iv - 12/18/2012 12:25 PM - Submitting

TEST
', N'Submitting', N'', N'', 0, CAST(0x0000A12B00CDA89D AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (175, N'BBO Fed', N'', CAST(0x0000A03F00A6148A AS DateTime), 9, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A03F00A6148A AS DateTime), CAST(0x0000A03F00A6148A AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A03F00A6148A AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (176, N'BBO Fed', N'This is a TEST', CAST(0x0000A03F00AA2DA3 AS DateTime), 35, N'', CAST(0x0000A03F00000000 AS DateTime), N'This is a TEST', N'', N'', N'', N'Approval', CAST(0x0000A03F00000000 AS DateTime), CAST(0x0000A03F00000000 AS DateTime), N'', N' - 04/26/2012 10:23 AM - Approval

Issue:
This is a TEST
', N'Submitting', N'', N'', 0, CAST(0x0000A03F00AB39E4 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (177, N'BBO Fed', N'test', CAST(0x0000A03F00AC25C4 AS DateTime), 35, N'', CAST(0x0000A03F00000000 AS DateTime), N'test', N'', N'', N'', N'Submitting', CAST(0x0000A03F00AC25C4 AS DateTime), CAST(0x0000A03F00000000 AS DateTime), N'', N'Bret Pehrson - 04/26/2012 12:01 PM - Submitting

test

 - 04/26/2012 10:27 AM - Submitting

Test
', N'', N'', N'Executive Request', 9, CAST(0x0000A03F00C63015 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (178, N'BBO Fed', N'', CAST(0x0000A03F00ACC021 AS DateTime), 35, N'', CAST(0x0000A1A400000000 AS DateTime), N'test', N'', N'', N'', N'Approval', CAST(0x0000A1A400000000 AS DateTime), CAST(0x0000A1A400000000 AS DateTime), N'', N'Parish Snyder iv - 04/18/2013 12:08 PM - Approval

test

Issue:
test
', N'Submitting', N'Marginally Improves Operational Efficiency', N'Improve Employee Morale', 1, CAST(0x0000A1A400C7E7CE AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (179, N'BBO Fed', N'TEST', CAST(0x0000A03F00AD03E5 AS DateTime), 34, N'', CAST(0x0000A05500000000 AS DateTime), N'TEST', N'', N'', N'', N'Approval', CAST(0x0000A05500000000 AS DateTime), CAST(0x0000A05500000000 AS DateTime), N'', N'Melanie Garfield - 10/09/2012 08:55 AM - Approval

Test

Parish Snyder iv - 05/18/2012 09:10 AM - Approval

TEST

Issue:
TEST
', N'Submitting', N'Provides Large Cost Benefit', N'Get in Compliance', 7, CAST(0x0000A0E50092FDB1 AS DateTime), N'', N'', N'TEST', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (180, N'BBO Fed', N'This is a TEST', CAST(0x0000A03F00B8F030 AS DateTime), 35, N'', CAST(0x0000A12B00000000 AS DateTime), N'This is a TEST', N'', N'', N'', N'In Progress', CAST(0x0000A12B00000000 AS DateTime), CAST(0x0000A12B00000000 AS DateTime), N'', N'Wendy Hack - 12/18/2012 09:52 AM - In Progress

Wendy Hack - 10/24/2012 08:52 AM - Approval

testing

Wendy Hack - 10/24/2012 08:51 AM - Approval

Testing 

Parish Snyder iv - 04/27/2012 12:19 PM - Approval

TEST

Parish Snyder iv - 04/27/2012 12:11 PM - Approval

This is a TEST

Issue:
This is a TEST
', N'Approval', N'', N'', 0, CAST(0x0000A12B00A2B45D AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (181, N'DCR Fed', N'', CAST(0x0000A03F00BF5234 AS DateTime), 35, N'', CAST(0x0000A03F00000000 AS DateTime), N'', N'', N'', N'', N'Submitting', CAST(0x0000A03F00BF5234 AS DateTime), CAST(0x0000A03F00000000 AS DateTime), N'', N'', N'', N'', N'', 0, CAST(0x0000A03F00CAE2A6 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (182, N'DCR Fed', N'test', CAST(0x0000A03F00CA48FD AS DateTime), 35, N'', CAST(0x0000A03F00000000 AS DateTime), N'test', N'', N'', N'', N'Submitting', CAST(0x0000A03F00CA48FD AS DateTime), CAST(0x0000A03F00000000 AS DateTime), N'', N'Bret Pehrson - 04/26/2012 12:17 PM - Submitting

test
', N'', N'', N'Executive Request', 9, CAST(0x0000A03F00CA9834 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (183, N'IDEM Fed', N'TEST', CAST(0x0000A03F00D119BF AS DateTime), 35, N'', CAST(0x0000A03F00000000 AS DateTime), N'TEST', N'', N'', N'', N'Withdrawn', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Parish Snyder iv - 11/13/2012 06:27 PM - Submitting

Ticket Withdrawn: 

TEST

Parish Snyder iv - 11/13/2012 06:27 PM - Submitting

TEST

TEST

TEST

TESTEST

Parish Snyder iv - 11/13/2012 06:27 PM - Submitting

TEST

TEST

TEST

TESTTEST

TEST

TEST


TESTTEST
TEST

', N'Submitting', N'', N'', 0, CAST(0x0000A108012FFB65 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (184, N'QCR Fed', N'asdf', CAST(0x0000A03F00D12E12 AS DateTime), 35, N'', CAST(0x0000A03F00000000 AS DateTime), N'asdf', N'', N'', N'', N'Verified', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Parish Snyder iv - 11/13/2012 07:38 PM - Verified

asdf

Parish Snyder iv - 11/13/2012 07:38 PM - Complete

asdf

Parish Snyder iv - 11/13/2012 07:38 PM - In Progress

asdf

Parish Snyder iv - 11/13/2012 07:38 PM - Approval

asdf

asdfasdf

asdf

asdfasdf

asdf

asdf

asdf

asdfasdf

asdfasdf

asdfas

Issue:
asdf
', N'Complete', N'Other Low Urgency', N'Get in Compliance', 3, CAST(0x0000A1080143864F AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (185, N'ACCM Fed', N'', CAST(0x0000A04000C6A68B AS DateTime), 9, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A04000C6A68B AS DateTime), CAST(0x0000A04000C6A68B AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A04000C6A68B AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (186, N'DCR Fed', N'test', CAST(0x0000A0450102ECEB AS DateTime), 35, N'', CAST(0x0000A05500000000 AS DateTime), N'test', N'', N'', N'', N'Submitting', CAST(0x0000A0450102ECEB AS DateTime), CAST(0x0000A05500000000 AS DateTime), N'', N'Bret Pehrson - 05/18/2012 11:38 AM - Submitting

test
', N'', N'', N'Executive Request', 9, CAST(0x0000A05500BFAD87 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (187, N'DCR Fed', N'test', CAST(0x0000A04B00F4C9DA AS DateTime), 35, N'', CAST(0x0000A04E00000000 AS DateTime), N'test', N'', N'', N'', N'Verified', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Sasha Vanorman - 11/13/2012 01:36 PM - Verified

Sasha Vanorman - 11/13/2012 01:36 PM - Complete

Sasha Vanorman - 11/13/2012 01:36 PM - In Progress

Bret Pehrson - 05/11/2012 10:43 AM - DCR Approval

test

Issue:
test
', N'Complete', N'', N'Executive Request', 9, CAST(0x0000A10800E01C01 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (188, N'DCR Fed', N'test', CAST(0x0000A04B00F4F14F AS DateTime), 35, N'', CAST(0x0000A05400000000 AS DateTime), N'test', N'', N'', N'', N'In Progress', CAST(0x0000A05400000000 AS DateTime), CAST(0x0000A05400000000 AS DateTime), N'', N'Bret Pehrson - 05/17/2012 04:35 PM - In Progress

Moving to Parish

Bret Pehrson - 05/17/2012 04:35 PM - In Progress

approved

Bret Pehrson - 05/17/2012 04:35 PM - DCR Approval

Moving to Colton

Bret Pehrson - 05/17/2012 04:34 PM - DCR Approval

test

Issue:
test
', N'DCR Approval', N'', N'Executive Request', 9, CAST(0x0000A0540110FD6F AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (189, N'FAR Fed', N'test', CAST(0x0000A04D00924D4A AS DateTime), 35, N'', CAST(0x0000A04D00000000 AS DateTime), N'test', N'', N'', N'', N'OPA - In Progress', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Sasha Vanorman - 11/13/2012 02:37 PM - OPA - In Progress

test

Bret Pehrson - 05/10/2012 08:55 AM - BS Approval

test

Issue:
test
', N'BS Approval', N'', N'Executive Request', 9, CAST(0x0000A10800F0E6CC AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (190, N'TOC Fed', N'TEST', CAST(0x0000A05300D14528 AS DateTime), 35, N'', CAST(0x0000A05300000000 AS DateTime), N'TEST', N'', N'', N'', N'Review', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05300000000 AS DateTime), N'', N' - 05/16/2012 12:43 PM - Review

TEST

Issue:
TEST
', N'Submitting', N'Marginally Improves Operational Efficiency', N'Resolve Customer Service Issue', 2, CAST(0x0000A05300D16F2A AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (191, N'OTH Fed', N'TEST', CAST(0x0000A05300D1B59A AS DateTime), 2, N'Regents Scholarship', CAST(0x0000A05300000000 AS DateTime), N'TEST', N'Process Complete', N'asdf', N'asdf', N'Resolved', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05300000000 AS DateTime), N'', N'Bret Pehrson - 05/16/2012 03:42 PM - Resolved

test

Cause:
Process Complete

Fix:
asdf

Prevention:
asdf


Bret Pehrson - 05/16/2012 03:40 PM - Resolved

test

Cause:
Process Complete

Fix:
asdf

Prevention:
asdf


Bret Pehrson - 05/16/2012 03:38 PM - Resolved

TEST

Cause:
Process Complete

Fix:
asdf

Prevention:
asdf


Bret Pehrson - 05/16/2012 03:37 PM - Resolved

TEST

Cause:
Process Complete

Fix:
asdf

Prevention:
asdf


Bret Pehrson - 05/16/2012 03:33 PM - Resolved

TEST

Cause:
Process Complete

Fix:
asdf

Prevention:
asdf


Bret Pehrson - 05/16/2012 03:31 PM - Resolved

TEST

Cause:
Process Complete

Fix:
asdf

Prevention:
asdf


Bret Pehrson - 05/16/2012 02:45 PM - Resolved

TEST

Cause:
Process Complete

Fix:
asdf

Prevention:
asdf


Bret Pehrson - 05/16/2012 02:25 PM - Resolved

TEST

Cause:
Process Complete

Fix:
asdf

Prevention:
asdf


Parish Snyder iv - 05/16/2012 01:02 PM - Resolved

TEST

Cause:
Process Complete

Fix:
asdf

Prevention:
asdf


Parish Snyder iv - 05/16/2012 01:02 PM - BS Approval

Cause:
Process Complete

Fix:
asdf

Prevention:
asdf


Parish Snyder iv - 05/16/2012 01:01 PM - Discussion

TEST

Parish Snyder iv - 05/16/2012 01:01 PM - Discussion

TEST

Parish Snyder iv - 05/16/2012 01:00 PM - Discussion

TEST

Parish Snyder iv - 05/16/2012 12:57 PM - Discussion

TEST

Parish Snyder iv - 05/16/2012 12:57 PM - Discussion

TEST

Parish Snyder iv - 05/16/2012 12:54 PM - Discussion

Test

Parish Snyder iv - 05/16/2012 12:49 PM - Discussion

TEST updated business unit

Parish Snyder iv - 05/16/2012 12:49 PM - Discussion

Changing court

Parish Snyder iv - 05/16/2012 12:48 PM - Discussion

TEST updated bus unit

Parish Snyder iv - 05/16/2012 12:48 PM - Discussion

TEST Updated business unit

Parish Snyder iv - 05/16/2012 12:47 PM - Discussion

TEST updated business unit

 - 05/16/2012 12:47 PM - Discussion

TEST

Issue:
TEST
', N'BS Approval', N'Impacts Significant Number of Customers', N'Get in Compliance', 9, CAST(0x0000A0530102AA40 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (192, N'OTH Fed', N'TEST', CAST(0x0000A05300D6D7F4 AS DateTime), 31, N'Tax Reporting (1098, 1099, TRA)', CAST(0x0000A05300000000 AS DateTime), N'TEST', N'SAS/Script Problem', N'`1234567890-=[]\;'',./!~@#$%^&*()_+{}|:"<>?', N'`1234567890-=[]\;'',./!~@#$%^&*()_+{}|:"<>?', N'Resolved', CAST(0x0000A05400000000 AS DateTime), CAST(0x0000A05400000000 AS DateTime), N'', N'Parish Snyder iv - 05/17/2012 05:49 PM - Resolved

`1234567890-=[]\;'',./!~@#$%^&*()_+{}|:"<>?

Cause:
SAS/Script Problem

Fix:
`1234567890-=[]\;'',./!~@#$%^&*()_+{}|:"<>?

Prevention:
`1234567890-=[]\;'',./!~@#$%^&*()_+{}|:"<>?


Parish Snyder iv - 05/17/2012 05:49 PM - Resolved

`1234567890-=[]\;'',./!~@#$%^&*()_+{}|:"<>?

Cause:
SAS/Script Problem

Fix:
`1234567890-=[]\;'',./!~@#$%^&*()_+{}|:"<>?

Prevention:
`1234567890-=[]\;'',./!~@#$%^&*()_+{}|:"<>?


Parish Snyder iv - 05/17/2012 05:48 PM - BS Approval

`1234567890-=[]\;'',./!~@#$%^&*()_+{}|:"<>?

Cause:
SAS/Script Problem

Fix:
`1234567890-=[]\;'',./!~@#$%^&*()_+{}|:"<>?

Prevention:
`1234567890-=[]\;'',./!~@#$%^&*()_+{}|:"<>?


Parish Snyder iv - 05/17/2012 05:48 PM - Discussion

TESET

Issue:
TEST

 - 05/16/2012 01:03 PM - Submitting

TEST
', N'BS Approval', N'Marginally Improves Operational Efficiency', N'Get in Compliance', 3, CAST(0x0000A0540125783E AS DateTime), N'`1234567890-=[]\;'',.', N'`1234567890-=[]\;'',./!~@#$%^&*()_+{}|:"<>?', N'', N'', N'`1234567890-=[]\;'',./!~@#$%^&*()_+{}|:"<>?')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (193, N'POL Fed', N'', CAST(0x0000A05300D75603 AS DateTime), 9, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A05300D75603 AS DateTime), CAST(0x0000A05300D75603 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A05300D75603 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (194, N'PRB Fed', N'TEST', CAST(0x0000A053010720C7 AS DateTime), 36, N'', CAST(0x0000A05300000000 AS DateTime), N'TEST', N'Staff Error', N'TEST', N'TEST', N'Resolved', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05300000000 AS DateTime), N'', N'Parish Snyder iv - 05/16/2012 03:59 PM - Resolved

TEST

Cause:
Staff Error

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 05/16/2012 03:59 PM - BS Approval

TEST

Cause:
Staff Error

Fix:
TEST

Prevention:
TEST


 - 05/16/2012 03:59 PM - Discussion

TEST

Issue:
TEST
', N'BS Approval', N'Improves Operational Efficiency', N'Other Efficiency', 4, CAST(0x0000A05301076FF7 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (195, N'FNC Fed', N'TEST', CAST(0x0000A05301079E3A AS DateTime), 36, N'', CAST(0x0000A05300000000 AS DateTime), N'TEST `1234567890-=~!@#$%^&*()_+[]\{}|;'':",./<>?', N'Staff Error', N'`1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'`1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'Resolved', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05300000000 AS DateTime), N'', N'Parish Snyder iv - 05/16/2012 04:03 PM - Resolved

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Cause:
Staff Error

Fix:
`1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Prevention:
`1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?


Parish Snyder iv - 05/16/2012 04:02 PM - BS Approval

TEST

Cause:
Staff Error

Fix:
`1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Prevention:
`1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?


 - 05/16/2012 04:01 PM - Discussion

TEST`1234567890-=~!@#$%^&*(_)+[]\{}|;'':",./<>?

Issue:
TEST `1234567890-=~!@#$%^&*()_+[]\{}|;'':",./<>?
', N'BS Approval', N'Marginally Improves Operational Efficiency', N'Provide New Service', 2, CAST(0x0000A05301086D0B AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (196, N'SPH Fed', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', CAST(0x0000A05301089BE0 AS DateTime), 36, N'Autodialer', CAST(0x0000A05300000000 AS DateTime), N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'Staff Error', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'Resolved', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05300000000 AS DateTime), N'', N'Parish Snyder iv - 05/16/2012 04:06 PM - Resolved

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Cause:
Staff Error

Fix:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Prevention:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?


Parish Snyder iv - 05/16/2012 04:05 PM - BS Approval

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Cause:
Staff Error

Fix:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Prevention:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?


 - 05/16/2012 04:04 PM - Discussion

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Issue:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?
', N'BS Approval', N'Provides Small Cost Benefit', N'Resolve Audit Finding', 3, CAST(0x0000A053010945EC AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (197, N'SCHELIG Fed', N' TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', CAST(0x0000A0530109B909 AS DateTime), 36, N'', CAST(0x0000A05300000000 AS DateTime), N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'', N'', N'Complete And Verified', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05300000000 AS DateTime), N'', N'Parish Snyder iv - 05/16/2012 04:09 PM - Complete And Verified

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:09 PM - Complete

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:09 PM - In Progress

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:09 PM - Approval

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

 - 05/16/2012 04:08 PM - Confirmation Review

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Issue:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?
', N'Complete', N'Provides Small Cost Benefit', N'Resolve Audit Finding', 3, CAST(0x0000A053010A289A AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (198, N'QCR Fed', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', CAST(0x0000A053010A6203 AS DateTime), 36, N'', CAST(0x0000A05300000000 AS DateTime), N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'', N'', N'Verified', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05300000000 AS DateTime), N'', N'Parish Snyder iv - 05/16/2012 04:11 PM - Verified

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:10 PM - Complete

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:10 PM - In Progress

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

 - 05/16/2012 04:10 PM - Approval

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Issue:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?
', N'Complete', N'Provides Small Cost Benefit', N'Resolve Customer Service Issue', 2, CAST(0x0000A053010A8C68 AS DateTime), N'', N'', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (199, N'QASMT Fed', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', CAST(0x0000A053010AB45B AS DateTime), 9, N'', CAST(0x0000A05300000000 AS DateTime), N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'', N'', N'Verified', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05300000000 AS DateTime), N'', N'Parish Snyder iv - 05/16/2012 04:12 PM - Verified

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:11 PM - Verified

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:11 PM - Complete

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:11 PM - In Progress

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

 - 05/16/2012 04:11 PM - Approval

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Issue:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?
', N'Complete', N'Provides Small Cost Benefit', N'Resolve Audit Finding', 3, CAST(0x0000A053010AD7AB AS DateTime), N'', N'', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (200, N'POL Fed', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', CAST(0x0000A053010AF16A AS DateTime), 34, N'', CAST(0x0000A05300000000 AS DateTime), N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'Training', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'Resolved', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05300000000 AS DateTime), N'', N'Parish Snyder iv - 05/16/2012 04:13 PM - Resolved

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Cause:
Training

Fix:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Prevention:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?


Parish Snyder iv - 05/16/2012 04:13 PM - BS Approval

Cause:
Training

Fix:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Prevention:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?


Parish Snyder iv - 05/16/2012 04:13 PM - Discussion

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:13 PM - Discussion

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

 - 05/16/2012 04:12 PM - Discussion

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Issue:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?
', N'BS Approval', N'Prevents Loss of Guaranty', N'Resolve Audit Finding', 9, CAST(0x0000A053010B373E AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (201, N'SPH Fed', N'test', CAST(0x0000A053010CA2EB AS DateTime), 35, N'', CAST(0x0000A05300000000 AS DateTime), N'test', N'SAS/Script Problem', N'test', N'test', N'Resolved', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05300000000 AS DateTime), N'', N'Bret Pehrson - 05/16/2012 04:20 PM - Resolved

test

Cause:
SAS/Script Problem

Fix:
test

Prevention:
test


Bret Pehrson - 05/16/2012 04:19 PM - BS Approval

test

Cause:
SAS/Script Problem

Fix:
test

Prevention:
test


 - 05/16/2012 04:18 PM - Discussion

test

Issue:
test
', N'BS Approval', N'', N'Executive Request', 9, CAST(0x0000A053010D3E79 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (202, N'SPH Fed', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', CAST(0x0000A0530110FA6D AS DateTime), 9, N'Batch Scripts', CAST(0x0000A05300000000 AS DateTime), N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'Process Complete', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'Resolved', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05300000000 AS DateTime), N'', N'Parish Snyder iv - 05/16/2012 04:35 PM - Resolved

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Cause:
Process Complete

Fix:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Prevention:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?


Parish Snyder iv - 05/16/2012 04:35 PM - BS Approval

Cause:
Process Complete

Fix:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Prevention:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?


Parish Snyder iv - 05/16/2012 04:34 PM - Discussion

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

 - 05/16/2012 04:34 PM - Discussion

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Issue:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?
', N'BS Approval', N'Provides Significant Cost Benefit', N'Resolve Audit Finding', 9, CAST(0x0000A053011139DA AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (203, N'OTH Fed', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', CAST(0x0000A053011179FE AS DateTime), 9, N'', CAST(0x0000A05300000000 AS DateTime), N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'', N'', N'Discussion', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05300000000 AS DateTime), N'', N'Parish Snyder iv - 05/16/2012 04:36 PM - Discussion

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:36 PM - Discussion

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

 - 05/16/2012 04:36 PM - Discussion

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Issue:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?
', N'Submitting', N'Provides Small Cost Benefit', N'Provide New Service', 2, CAST(0x0000A0530111A49D AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (204, N'OTH Fed', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', CAST(0x0000A0530111B4EB AS DateTime), 9, N'', CAST(0x0000A05300000000 AS DateTime), N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'Process Complete', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'Resolved', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05300000000 AS DateTime), N'', N'Parish Snyder iv - 05/16/2012 04:40 PM - Resolved

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Cause:
Process Complete

Fix:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Prevention:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?


Parish Snyder iv - 05/16/2012 04:40 PM - BS Approval

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Cause:
Process Complete

Fix:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Prevention:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?


Parish Snyder iv - 05/16/2012 04:38 PM - Discussion

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:37 PM - Discussion

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:37 PM - Discussion

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:37 PM - Discussion

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

 - 05/16/2012 04:37 PM - Discussion

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Issue:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?
', N'BS Approval', N'Provides Large Cost Benefit', N'Other Compliance', 7, CAST(0x0000A05301129250 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (205, N'OTH Fed', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', CAST(0x0000A05301124282 AS DateTime), 9, N'', CAST(0x0000A05300000000 AS DateTime), N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'', N'', N'Discussion', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05300000000 AS DateTime), N'', N'Parish Snyder iv - 05/16/2012 04:41 PM - Discussion

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:41 PM - Discussion

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:40 PM - Discussion

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:40 PM - Discussion

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Issue:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?
', N'Submitting', N'Impacts Small Number of Customers', N'Get in Compliance', 3, CAST(0x0000A0530112DAB9 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (206, N'OTH Fed', N'TEST', CAST(0x0000A053011303ED AS DateTime), 9, N'', CAST(0x0000A05300000000 AS DateTime), N'TEST', N'', N'', N'', N'Discussion', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05500000000 AS DateTime), N'test', N'Bret Pehrson - 05/18/2012 11:39 AM - Discussion

test

Parish Snyder iv - 05/16/2012 04:42 PM - Discussion

TEST

Parish Snyder iv - 05/16/2012 04:42 PM - Discussion

TEST

Parish Snyder iv - 05/16/2012 04:42 PM - Discussion

TEST

 - 05/16/2012 04:42 PM - Discussion

TEST

Issue:
TEST
', N'Submitting', N'Improves Operational Efficiency', N'Improve Employee Morale', 4, CAST(0x0000A05500BFF57E AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (207, N'OTH Fed', N'TEST', CAST(0x0000A0530113658A AS DateTime), 9, N'', CAST(0x0000A05300000000 AS DateTime), N'TEST', N'', N'', N'', N'Discussion', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05300000000 AS DateTime), N'', N'Parish Snyder iv - 05/16/2012 04:43 PM - Discussion

TEST

Parish Snyder iv - 05/16/2012 04:43 PM - Discussion

TEST

 - 05/16/2012 04:43 PM - Discussion

TEST

Issue:
TEST
', N'Submitting', N'Improves Operational Efficiency', N'Improve Customer Service', 6, CAST(0x0000A053011387AB AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (208, N'LPDS Fed', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', CAST(0x0000A05301148665 AS DateTime), 9, N'', CAST(0x0000A05300000000 AS DateTime), N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'', N'', N'Verified', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05300000000 AS DateTime), N'', N'Parish Snyder iv - 05/16/2012 04:47 PM - Verified

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:47 PM - Complete

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:47 PM - In Progress

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

 - 05/16/2012 04:47 PM - Approval

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Issue:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?
', N'Complete', N'Improves Operational Efficiency', N'Improve Customer Service', 6, CAST(0x0000A0530114A03E AS DateTime), N'', N'', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (209, N'IDEM Fed', N' TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', CAST(0x0000A0530114B40B AS DateTime), 9, N'', CAST(0x0000A05300000000 AS DateTime), N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'', N'', N'Verified', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05300000000 AS DateTime), N'', N'Parish Snyder iv - 05/16/2012 04:50 PM - Verified

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:50 PM - Verified

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:50 PM - Complete

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:50 PM - In Progress

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:50 PM - Approval

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Issue:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:49 PM - Submitting

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:49 PM - In Progress

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:49 PM - Approval

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Issue:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:49 PM - Submitting

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:49 PM - Approval

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?d

Issue:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:49 PM - Submitting

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:49 PM - Complete

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:48 PM - In Progress

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:48 PM - In Progress

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:48 PM - In Progress

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

 - 05/16/2012 04:48 PM - Approval

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Issue:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?
', N'Complete', N'Prevents Significant Disruption to Operations', N'Provide New Service', 8, CAST(0x0000A05301157E08 AS DateTime), N'', N'', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (210, N'FTRANS Fed', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', CAST(0x0000A05301158EFE AS DateTime), 9, N'', CAST(0x0000A05300000000 AS DateTime), N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'', N'', N'Completed', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05300000000 AS DateTime), N'', N'Parish Snyder iv - 05/16/2012 04:51 PM - Completed

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:51 PM - QC Approval

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

 - 05/16/2012 04:51 PM - OPA - In Progress

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Issue:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?
', N'QC Approval', N'Improves Operational Efficiency', N'Improve Operational Efficiency', 4, CAST(0x0000A0530115C98B AS DateTime), N'', N'', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (211, N'FAR Fed', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', CAST(0x0000A0530115E2B1 AS DateTime), 9, N'', CAST(0x0000A05300000000 AS DateTime), N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'', N'', N'Resolved', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05300000000 AS DateTime), N'', N'Parish Snyder iv - 05/16/2012 04:53 PM - Resolved

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:53 PM - QC Approval

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:53 PM - OPA - In Progress

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

 - 05/16/2012 04:53 PM - BS Approval

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Issue:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?
', N'QC Approval', N'Impacts Small Number of Customers', N'Improve Employee Morale', 1, CAST(0x0000A05301165211 AS DateTime), N'', N'', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (212, N'DCR Fed', N'Testing DCR', CAST(0x0000A05301166D55 AS DateTime), 9, N'', CAST(0x0000A05300000000 AS DateTime), N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'', N'', N'Verified', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05300000000 AS DateTime), N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'Parish Snyder iv - 05/16/2012 04:56 PM - Verified

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:55 PM - Verified

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:55 PM - Verified

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:55 PM - Complete

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:55 PM - In Progress

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

 - 05/16/2012 04:55 PM - DCR Approval

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Issue:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?
', N'Complete', N'Provides Large Cost Benefit', N'Provide New Service', 6, CAST(0x0000A0530117193E AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (213, N'BBO Fed', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', CAST(0x0000A05301172A79 AS DateTime), 9, N'', CAST(0x0000A05300000000 AS DateTime), N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'', N'', N'Complete and Verified', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05300000000 AS DateTime), N'', N'Parish Snyder iv - 05/16/2012 04:58 PM - Complete and Verified

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:58 PM - Complete and Verified

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:57 PM - Complete

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:57 PM - In Progress

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:57 PM - In Progress

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:57 PM - In Progress

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

 - 05/16/2012 04:57 PM - Approval

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Issue:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?
', N'Complete', N'Improves Operational Efficiency', N'Improve Employee Morale', 4, CAST(0x0000A05301178A7B AS DateTime), N'', N'', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (214, N'ARCM Fed', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', CAST(0x0000A0530117A6E7 AS DateTime), 9, N'', CAST(0x0000A05300000000 AS DateTime), N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'', N'', N'Verified', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05300000000 AS DateTime), N'', N'Parish Snyder iv - 05/16/2012 05:00 PM - Verified

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:59 PM - Verified

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:59 PM - Complete

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 04:59 PM - In Progress

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

 - 05/16/2012 04:59 PM - Approval

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Issue:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?
', N'Complete', N'Provides Small Cost Benefit', N'Provide Cost Benefit', 1, CAST(0x0000A05301180C5D AS DateTime), N'', N'', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (215, N'ARCC Fed', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?TES', CAST(0x0000A05301181E6E AS DateTime), 9, N'', CAST(0x0000A05300000000 AS DateTime), N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'', N'', N'Verified', CAST(0x0000A05400000000 AS DateTime), CAST(0x0000A05400000000 AS DateTime), N'', N'Debbie Phillips - 05/17/2012 08:44 AM - Verified

test

Debbie Phillips - 05/17/2012 08:39 AM - Verified

test

Debbie Phillips - 05/17/2012 08:34 AM - Verified

test

Parish Snyder iv - 05/16/2012 05:10 PM - Verified

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 05:10 PM - Verified

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 05:10 PM - Complete

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 05:09 PM - In Progress

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

 - 05/16/2012 05:09 PM - Approval

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Issue:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?
', N'Complete', N'Prevents Significant Disruption to Operations', N'Resolve Customer Service Issue', 8, CAST(0x0000A05400901A68 AS DateTime), N'', N'', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (216, N'ARCA Fed', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', CAST(0x0000A053011F34B9 AS DateTime), 9, N'', CAST(0x0000A05300000000 AS DateTime), N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'', N'', N'Verified', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05300000000 AS DateTime), N'', N'Parish Snyder iv - 05/16/2012 05:26 PM - Verified

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 05:26 PM - Complete

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 05:26 PM - In Progress

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

 - 05/16/2012 05:26 PM - DS Approval

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Issue:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?
', N'Complete', N'Provides Small Cost Benefit', N'Resolve Audit Finding', 3, CAST(0x0000A053011F54EE AS DateTime), N'', N'', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (217, N'ACTD Fed', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', CAST(0x0000A053011FB2A8 AS DateTime), 16, N'', CAST(0x0000A05300000000 AS DateTime), N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'', N'', N'Verified', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05300000000 AS DateTime), N'', N'Parish Snyder iv - 05/16/2012 05:28 PM - Verified

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 05:28 PM - Complete

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 05:28 PM - In Progress

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 05:28 PM - Approval

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

 - 05/16/2012 05:28 PM - Review

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Issue:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?
', N'Complete', N'Provides Large Cost Benefit', N'Resolve Audit Finding', 7, CAST(0x0000A053011FE93B AS DateTime), N'', N'', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (218, N'ACCM Fed', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', CAST(0x0000A0530120B9C5 AS DateTime), 9, N'', CAST(0x0000A05300000000 AS DateTime), N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'', N'', N'Verified', CAST(0x0000A05300000000 AS DateTime), CAST(0x0000A05300000000 AS DateTime), N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'Parish Snyder iv - 05/16/2012 05:32 PM - Verified

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 05:32 PM - Complete

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/16/2012 05:32 PM - In Progress

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

 - 05/16/2012 05:31 PM - Approval

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Issue:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?
', N'Complete', N'Marginally Improves Operational Efficiency', N'Improve Customer Service', 2, CAST(0x0000A0530120D8F8 AS DateTime), N'', N'', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (219, N'ARCA Fed', N'TEST', CAST(0x0000A05400BFD23A AS DateTime), 9, N'', CAST(0x0000A05400000000 AS DateTime), N'TEST', N'', N'', N'', N'Verified', CAST(0x0000A05400000000 AS DateTime), CAST(0x0000A05400000000 AS DateTime), N'', N'Parish Snyder iv - 05/17/2012 11:39 AM - Verified

TEST

Parish Snyder iv - 05/17/2012 11:39 AM - Verified

TEST

Parish Snyder iv - 05/17/2012 11:39 AM - Complete

TEST

Parish Snyder iv - 05/17/2012 11:39 AM - In Progress

TEST

 - 05/17/2012 11:39 AM - DS Approval

TEST

Issue:
TEST
', N'Complete', N'Provides Small Cost Benefit', N'Resolve Audit Finding', 3, CAST(0x0000A0540113FC9D AS DateTime), N'', N'', N'TEST', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (220, N'ARCA Fed', N'TEST', CAST(0x0000A05400C0699B AS DateTime), 9, N'', CAST(0x0000A05400000000 AS DateTime), N'Test', N'', N'', N'', N'In Progress', CAST(0x0000A05400000000 AS DateTime), CAST(0x0000A05400000000 AS DateTime), N'TEST', N'Parish Snyder iv - 05/17/2012 11:47 AM - In Progress

Placing in Coltons court

Parish Snyder iv - 05/17/2012 11:44 AM - In Progress

TEST

Parish Snyder iv - 05/17/2012 11:44 AM - In Progress

TEST

Parish Snyder iv - 05/17/2012 11:43 AM - In Progress

Releasing from hold: 

TEST

Parish Snyder iv - 05/17/2012 11:43 AM - Hold

Ticket Placed On Hold: 

TEST

Parish Snyder iv - 05/17/2012 11:43 AM - In Progress

TEST

Parish Snyder iv - 05/17/2012 11:42 AM - In Progress

TEST

Parish Snyder iv - 05/17/2012 11:42 AM - DS Approval

TEST

Parish Snyder iv - 05/17/2012 11:42 AM - DS Approval

Test

 - 05/17/2012 11:42 AM - DS Approval

Test

Issue:
Test
', N'Hold', N'Provides Small Cost Benefit', N'Resolve Audit Finding', 3, CAST(0x0000A05400C33B1A AS DateTime), N'', N'', N'TEST', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (221, N'OTH Fed', N'TEST', CAST(0x0000A05400C4302F AS DateTime), 51, N'Administrative Review', CAST(0x0000A05400000000 AS DateTime), N'TEST', N'', N'', N'', N'Discussion', CAST(0x0000A05400000000 AS DateTime), CAST(0x0000A05400000000 AS DateTime), N'', N'Parish Snyder iv - 05/17/2012 12:01 PM - Discussion

TEST

Parish Snyder iv - 05/17/2012 12:00 PM - Discussion

Test

Parish Snyder iv - 05/17/2012 12:00 PM - Discussion

TEST

Parish Snyder iv - 05/17/2012 11:56 AM - Discussion

TEST

Parish Snyder iv - 05/17/2012 11:55 AM - Discussion

TEST

 - 05/17/2012 11:55 AM - Discussion

TEST

Issue:
TEST
', N'Submitting', N'Provides Small Cost Benefit', N'Provide Cost Benefit', 1, CAST(0x0000A05400C5E5A5 AS DateTime), N'517948', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (222, N'ARCA Fed', N'TEST', CAST(0x0000A05400DD3382 AS DateTime), 34, N'', CAST(0x0000A05400000000 AS DateTime), N'TEST', N'', N'', N'', N'In Progress', CAST(0x0000A05400000000 AS DateTime), CAST(0x0000A05400000000 AS DateTime), N'', N'Parish Snyder iv - 05/17/2012 01:56 PM - In Progress

TEST

Parish Snyder iv - 05/17/2012 01:56 PM - In Progress

TEST

Parish Snyder iv - 05/17/2012 01:55 PM - In Progress

TEST

Parish Snyder iv - 05/17/2012 01:31 PM - In Progress

TEST

Parish Snyder iv - 05/17/2012 01:30 PM - In Progress

TEST

Parish Snyder iv - 05/17/2012 01:30 PM - DS Approval

TEST TEST

 - 05/17/2012 01:26 PM - DS Approval

TEST

Issue:
TEST
', N'DS Approval', N'Improves Operational Efficiency', N'Other Efficiency', 4, CAST(0x0000A05401227F90 AS DateTime), N'', N'', N'TEST', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (223, N'FNC Fed', N'TEST', CAST(0x0000A05401197B33 AS DateTime), 34, N'Administrative Review', CAST(0x0000A05400000000 AS DateTime), N'TEST', N'Staff Error', N'TEST', N'TEST', N'Resolved', CAST(0x0000A05400000000 AS DateTime), CAST(0x0000A05400000000 AS DateTime), N'', N'Parish Snyder iv - 05/17/2012 05:08 PM - Resolved

TEST

Cause:
Staff Error

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 05/17/2012 05:08 PM - Resolved

TEST

Cause:
Staff Error

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 05/17/2012 05:08 PM - BS Approval

TEST

Cause:
Staff Error

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 05/17/2012 05:08 PM - BS Approval

TEST

Cause:
Staff Error

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 05/17/2012 05:07 PM - Discussion

TEST

Parish Snyder iv - 05/17/2012 05:06 PM - Discussion

TEST

Parish Snyder iv - 05/17/2012 05:06 PM - Discussion

TEST

Issue:
TEST

 - 05/17/2012 05:06 PM - Submitting

TEST
', N'BS Approval', N'', N'Executive Request', 9, CAST(0x0000A054011A6B99 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (224, N'SPH Fed', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', CAST(0x0000A054011A963A AS DateTime), 34, N'Administrative Review', CAST(0x0000A05400000000 AS DateTime), N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'File Transferred', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?', N'Resolved', CAST(0x0000A05400000000 AS DateTime), CAST(0x0000A05400000000 AS DateTime), N'', N'Parish Snyder iv - 05/17/2012 05:36 PM - Resolved

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Cause:
File Transferred

Fix:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Prevention:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?


Parish Snyder iv - 05/17/2012 05:36 PM - Resolved

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Cause:
File Transferred

Fix:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Prevention:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?


Parish Snyder iv - 05/17/2012 05:36 PM - BS Approval

Cause:
File Transferred

Fix:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Prevention:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?


Parish Snyder iv - 05/17/2012 05:36 PM - Discussion

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Parish Snyder iv - 05/17/2012 05:11 PM - Discussion

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

 - 05/17/2012 05:10 PM - Discussion

TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?

Issue:
TEST `1234567890-=[]\;'',./~!@#$%^&*()_+{}|:"<>?
', N'BS Approval', N'Provides Large Cost Benefit', N'Provide New Service', 6, CAST(0x0000A0540121F11C AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (225, N'OTH Fed', N'TEST', CAST(0x0000A0540122E097 AS DateTime), 28, N'Administrative Review', CAST(0x0000A05400000000 AS DateTime), N'TEST', N'File Transferred', N'TEST', N'TEST', N'Resolved', CAST(0x0000A05400000000 AS DateTime), CAST(0x0000A05400000000 AS DateTime), N'', N'Parish Snyder iv - 05/17/2012 05:45 PM - Resolved

TEST

Cause:
File Transferred

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 05/17/2012 05:45 PM - Resolved

TEST

Cause:
File Transferred

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 05/17/2012 05:45 PM - BS Approval

TEST

Cause:
File Transferred

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 05/17/2012 05:45 PM - Discussion

TEST

Cause:
File Transferred

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 05/17/2012 05:45 PM - Discussion

TEST

Parish Snyder iv - 05/17/2012 05:41 PM - Discussion

TEST

Issue:
TEST
', N'BS Approval', N'Other Low Urgency', N'Provide New Service', 2, CAST(0x0000A054012490C8 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (226, N'DCR Fed', N'test', CAST(0x0000A0540126C4BC AS DateTime), 27, N'', CAST(0x0000A05400000000 AS DateTime), N'TEST', N'', N'', N'', N'Verified', CAST(0x0000A05400000000 AS DateTime), CAST(0x0000A05400000000 AS DateTime), N'', N'Parish Snyder iv - 05/17/2012 05:56 PM - Verified

TEST

Parish Snyder iv - 05/17/2012 05:55 PM - Complete

TEST

Parish Snyder iv - 05/17/2012 05:55 PM - In Progress

TEST

Parish Snyder iv - 05/17/2012 05:55 PM - DCR Approval

TEST

Parish Snyder iv - 05/17/2012 05:55 PM - DCR Approval

TEST

Issue:
TEST

 - 05/17/2012 05:54 PM - Submitting

TEST
', N'Complete', N'Prevents Significant Disruption to Operations', N'Other Efficiency', 5, CAST(0x0000A05401275980 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (227, N'TOC Fed', N'TEST', CAST(0x0000A055009744B4 AS DateTime), 34, N'', CAST(0x0000A05500000000 AS DateTime), N'TEST', N'', N'', N'', N'Verified', CAST(0x0000A05500000000 AS DateTime), CAST(0x0000A05500000000 AS DateTime), N'', N'Parish Snyder iv - 05/18/2012 09:18 AM - Verified

TEST

Parish Snyder iv - 05/18/2012 09:15 AM - Verified

TEST

Parish Snyder iv - 05/18/2012 09:14 AM - Verified

TEST

Parish Snyder iv - 05/18/2012 09:14 AM - Complete

TEST

Parish Snyder iv - 05/18/2012 09:14 AM - In Progress

TEST

Parish Snyder iv - 05/18/2012 09:14 AM - System Support Review

TEST

Parish Snyder iv - 05/18/2012 09:13 AM - Review

TEST

Parish Snyder iv - 05/18/2012 09:13 AM - Review

TEST

Parish Snyder iv - 05/18/2012 09:13 AM - Review

TEST

 - 05/18/2012 09:12 AM - Review

TEST

Issue:
TEST
', N'Complete', N'Provides Large Cost Benefit', N'Improve Customer Service', 6, CAST(0x0000A055009931D2 AS DateTime), N'', N'', N'TEST', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (228, N'ARCC Fed', N'asdf', CAST(0x0000A05500A3F1DB AS DateTime), 34, N'', CAST(0x0000A05500000000 AS DateTime), N'asdf', N'', N'', N'', N'Verified', CAST(0x0000A05500000000 AS DateTime), CAST(0x0000A05500000000 AS DateTime), N'', N'Parish Snyder iv - 05/18/2012 09:58 AM - Verified

TEST

Parish Snyder iv - 05/18/2012 09:58 AM - Complete

TEST

Parish Snyder iv - 05/18/2012 09:58 AM - Complete

TEST

Parish Snyder iv - 05/18/2012 09:58 AM - In Progress

TEST

 - 05/18/2012 09:58 AM - Approval

TEST

Issue:
asdf
', N'Complete', N'Improves Operational Efficiency', N'Resolve Customer Service Issue', 6, CAST(0x0000A05500A443FA AS DateTime), N'', N'', N'asdf', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (229, N'ACCM Fed', N'TEST', CAST(0x0000A05500A83FB3 AS DateTime), 51, N'', CAST(0x0000A05500000000 AS DateTime), N'TEST', N'', N'', N'', N'In Progress', CAST(0x0000A05500000000 AS DateTime), CAST(0x0000A05500000000 AS DateTime), N'', N'Parish Snyder iv - 05/18/2012 10:34 AM - In Progress

TEST

Parish Snyder iv - 05/18/2012 10:30 AM - In Progress

TEST

Parish Snyder iv - 05/18/2012 10:23 AM - In Progress

TEST

Parish Snyder iv - 05/18/2012 10:19 AM - In Progress

TEST

Parish Snyder iv - 05/18/2012 10:19 AM - In Progress

TEST

Parish Snyder iv - 05/18/2012 10:18 AM - In Progress

TEST

 - 05/18/2012 10:17 AM - Approval

TEST

Issue:
TEST
', N'Approval', N'Prevents Significant Disruption to Operations', N'Provide Cost Benefit', 5, CAST(0x0000A05500ADF7AD AS DateTime), N'', N'', N'TEST', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (230, N'ARCM Fed', N'test', CAST(0x0000A05500C044F5 AS DateTime), 35, N'', CAST(0x0000A05500000000 AS DateTime), N'test', N'', N'', N'', N'Verified', CAST(0x0000A0F400000000 AS DateTime), CAST(0x0000A0F400000000 AS DateTime), N'', N'Wendy Hack - 10/24/2012 08:48 AM - Verified

Wendy Hack - 10/24/2012 08:48 AM - Complete

Bret Pehrson - 05/21/2012 08:50 AM - In Progress

test

 - 05/18/2012 11:40 AM - Approval

test

Issue:
test
', N'Complete', N'', N'Executive Request', 9, CAST(0x0000A0F40090E36F AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (231, N'DCR Fed', N'test', CAST(0x0000A06100A588FC AS DateTime), 35, N'', CAST(0x0000A06100000000 AS DateTime), N'test', N'', N'', N'', N'Verified', CAST(0x0000A10200000000 AS DateTime), CAST(0x0000A10200000000 AS DateTime), N'', N'Sasha Vanorman - 11/07/2012 01:05 PM - Verified

Sasha Vanorman - 11/07/2012 01:05 PM - Complete

Sasha Vanorman - 11/07/2012 01:05 PM - In Progress

 - 05/30/2012 10:03 AM - DCR Approval

test

Issue:
test
', N'Complete', N'', N'Executive Request', 9, CAST(0x0000A10200D7A3DD AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (232, N'FAR Fed', N' ', CAST(0x0000A07700D9B0DE AS DateTime), 9, N'', CAST(0x0000A0FD00000000 AS DateTime), N' ', N'', N'', N'', N'OPA - In Progress', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Sasha Vanorman - 11/13/2012 02:40 PM - OPA - In Progress

Sasha Vanorman - 11/02/2012 03:16 PM - BS Approval

test

Sasha Vanorman - 11/02/2012 03:15 PM - BS Approval

testing

Issue:
 
', N'BS Approval', N'Other High Urgency', N'Get in Compliance', 9, CAST(0x0000A10800F19B3A AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (233, N'ARCA Fed', N'test', CAST(0x0000A08900B54A07 AS DateTime), 30, N'', CAST(0x0000A08900000000 AS DateTime), N'test', N'', N'', N'', N'DS Approval', CAST(0x0000A08900000000 AS DateTime), CAST(0x0000A08900000000 AS DateTime), N'', N'Jarom Ryan - 07/09/2012 11:16 AM - DS Approval

test

Jarom Ryan - 07/09/2012 11:13 AM - DS Approval

test

Jarom Ryan - 07/09/2012 11:12 AM - DS Approval

test

Issue:
test
', N'Submitting', N'Prevents Compliance Penalty', N'Improve Operational Efficiency', 4, CAST(0x0000A08900B9DB46 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (234, N'DCR Fed', N'test', CAST(0x0000A0AC00B22CDA AS DateTime), 52, N'', CAST(0x0000A0AC00000000 AS DateTime), N'test', N'', N'', N'', N'In Progress', CAST(0x0000A0E500000000 AS DateTime), CAST(0x0000A0E500000000 AS DateTime), N'', N'Sasha Vanorman - 10/09/2012 04:17 PM - In Progress

Sasha Vanorman - 10/09/2012 04:17 PM - DCR Approval

Releasing from hold: 



Sasha Vanorman - 10/09/2012 04:17 PM - Hold

Ticket Placed On Hold: 

hold

Sasha Vanorman - 10/09/2012 04:16 PM - DCR Approval

Releasing from hold: 



Sasha Vanorman - 10/09/2012 04:16 PM - Hold

Ticket Placed On Hold: 

udpate

Bret Pehrson - 08/13/2012 12:14 PM - DCR Approval

test

Issue:
test
', N'DCR Approval', N'', N'Executive Request', 9, CAST(0x0000A0E5010C62CE AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (235, N'FAR Fed', N'test', CAST(0x0000A0B40094D7BC AS DateTime), 30, N'', CAST(0x0000A0B400000000 AS DateTime), N'test', N'', N'', N'', N'OPA - In Progress', CAST(0x0000A0B400000000 AS DateTime), CAST(0x0000A0B400000000 AS DateTime), N'', N'Bret Pehrson - 08/21/2012 09:02 AM - OPA - In Progress

test

 - 08/21/2012 09:02 AM - BS Approval

Issue:
test
', N'BS Approval', N'', N'Executive Request', 9, CAST(0x0000A0B40094E737 AS DateTime), N'', N'', N'', N'', N'')
GO
print 'Processed 100 total records'
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (236, N'FAR Fed', N'test', CAST(0x0000A0B40098F55C AS DateTime), 30, N'', CAST(0x0000A0B400000000 AS DateTime), N'test', N'', N'', N'', N'OPA - In Progress', CAST(0x0000A0B400000000 AS DateTime), CAST(0x0000A0B400000000 AS DateTime), N'', N'Bret Pehrson - 08/21/2012 09:17 AM - OPA - In Progress

test

 - 08/21/2012 09:17 AM - BS Approval

Issue:
test
', N'BS Approval', N'', N'Executive Request', 9, CAST(0x0000A0B400990CD4 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (237, N'BBO Fed', N'Borrower Benefits', CAST(0x0000A0E000A98594 AS DateTime), 3, N'', CAST(0x0000A0E400000000 AS DateTime), N'Request to review a borrower benefit.', N'', N'', N'', N'Withdrawn', CAST(0x0000A0E500000000 AS DateTime), CAST(0x0000A0E500000000 AS DateTime), N'', N'Sasha Vanorman - 10/09/2012 04:18 PM - Approval

Ticket Withdrawn: 

update

Katie Van ausdal - 10/08/2012 11:07 AM - Approval

Switching to Romney''s court and sending him an email.

Katie Van ausdal - 10/08/2012 11:04 AM - Approval

Update note of some kind on the file.

Katie Van ausdal - 10/08/2012 11:02 AM - Approval

Issue:
Request to review a borrower benefit.
', N'Approval', N'Other Medium Urgency', N'Other Customer Service', 6, CAST(0x0000A0E5010C9FEB AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (238, N'FAR Fed', N'Test Ticket-PMT ADJ', CAST(0x0000A0E400B29C5D AS DateTime), 10, N'', CAST(0x0000A0E400000000 AS DateTime), N'XXXXXXXXXX- This is a test ticket for the new Need Help. ', N'', N'', N'', N'Withdrawn', CAST(0x0000A0E600000000 AS DateTime), CAST(0x0000A0E600000000 AS DateTime), N'', N'Romney Slagowski - 10/10/2012 12:51 PM - BS Approval

Ticket Withdrawn: 

testing ticket withdrawal. 

Romney Slagowski - 10/10/2012 12:51 PM - BS Approval

Court changed from Brenda Adams to Romney Slagowski

Romney Slagowski - 10/10/2012 12:50 PM - BS Approval

Interesting....switching back to my court. 

Issue:
XXXXXXXXXX- This is a test ticket for the new Need Help. 

Romney Slagowski - 10/10/2012 12:50 PM - Submitting

Court changed from Katie Van ausdal to Romney Slagowski

Romney Slagowski - 10/10/2012 12:49 PM - Submitting

Court changed from Romney Slagowski to Katie Van ausdal

Romney Slagowski - 10/08/2012 11:01 AM - Submitting

Court changed from Lynn Guymon to Romney Slagowski

Romney Slagowski - 10/08/2012 11:01 AM - Submitting

Court changed from Romney Slagowski to Lynn Guymon

Romney Slagowski - 10/08/2012 10:58 AM - Submitting

Testing updates to see who gets an update. 
', N'BS Approval', N'Other Low Urgency', N'Other Customer Service', 2, CAST(0x0000A0E600D3CD16 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (239, N'OTH Fed', N'Incorrect Information Displaying Etc.', CAST(0x0000A0E400DDC1A3 AS DateTime), 3, N'', CAST(0x0000A0E500000000 AS DateTime), N'Some kind of system error...inquiry as to what is going on', N'', N'', N'', N'Discussion', CAST(0x0000A0E500000000 AS DateTime), CAST(0x0000A0E500000000 AS DateTime), N'', N'Katie Van ausdal - 10/26/2012 08:04 AM - Discussion

Email to Jesse to see if he can see the attached punniness

Katie Van ausdal - 10/18/2012 10:48 AM - Discussion

Attempting to email update to Romney

Katie Van ausdal - 10/12/2012 07:51 AM - Discussion

update

Katie Van ausdal - 10/11/2012 01:15 PM - Discussion

Attempting to email recipient not already listed on ticket

Katie Van ausdal - 10/11/2012 09:22 AM - Discussion

Sending email to court individual

Katie Van ausdal - 10/09/2012 01:05 PM - Discussion

Issue:
Some kind of system error...inquiry as to what is going on
', N'Submitting', N'Other Low Urgency', N'Other Customer Service', 2, CAST(0x0000A0F60084E1F0 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (240, N'ARCA Fed', N'', CAST(0x0000A0E5013A910C AS DateTime), 35, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A0E5013A910C AS DateTime), CAST(0x0000A0E5013A910C AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A0E5013A910C AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (241, N'ARCA Fed', N'', CAST(0x0000A0E5013A915F AS DateTime), 35, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A0E5013A915F AS DateTime), CAST(0x0000A0E5013A915F AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A0E5013A915F AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (242, N'ARCA Fed', N'', CAST(0x0000A0E5013A91B1 AS DateTime), 35, N'', CAST(0x0000A10200000000 AS DateTime), N'', N'', N'', N'', N'Submitting', CAST(0x0000A0E5013A91B1 AS DateTime), CAST(0x0000A0E5013A91B1 AS DateTime), N'', N'Sasha Vanorman - 11/07/2012 12:54 PM - Submitting

test
', N'', N'', N'', 0, CAST(0x0000A10200D4B81B AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (243, N'ARCA Fed', N'safd', CAST(0x0000A0E5013A9207 AS DateTime), 35, N'', CAST(0x0000A10800000000 AS DateTime), N'fdsfd', N'', N'', N'', N'DS Approval', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Parish Snyder iv - 11/13/2012 05:43 PM - DS Approval

Update

Parish Snyder iv - 11/13/2012 05:42 PM - DS Approval

Test

Sasha Vanorman - 11/13/2012 05:40 PM - DS Approval

Issue:
fdsfd

Sasha Vanorman - 11/13/2012 05:40 PM - Submitting

sfds

Sasha Vanorman - 11/13/2012 05:40 PM - Submitting

dsfds
', N'Submitting', N'', N'Executive Request', 9, CAST(0x0000A1080123BA03 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (244, N'ARCA Fed', N'', CAST(0x0000A0E5013A9259 AS DateTime), 35, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A0E5013A9259 AS DateTime), CAST(0x0000A0E5013A9259 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A0E5013A9259 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (245, N'ARCA Fed', N'', CAST(0x0000A0E5013A92B0 AS DateTime), 35, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A0E5013A92B0 AS DateTime), CAST(0x0000A0E5013A92B0 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A0E5013A92B0 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (246, N'ARCA Fed', N'', CAST(0x0000A0E5013A9303 AS DateTime), 35, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A0E5013A9303 AS DateTime), CAST(0x0000A0E5013A9303 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A0E5013A9303 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (247, N'ARCA Fed', N'', CAST(0x0000A0E5013A9549 AS DateTime), 35, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A0E5013A9549 AS DateTime), CAST(0x0000A0E5013A9549 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A0E5013A9549 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (248, N'ARCA Fed', N'                ', CAST(0x0000A0E5013AA26A AS DateTime), 35, N'', CAST(0x0000A0ED00000000 AS DateTime), N'testing', N'', N'', N'', N'In Progress', CAST(0x0000A0ED00000000 AS DateTime), CAST(0x0000A0ED00000000 AS DateTime), N'', N'Sasha Vanorman - 10/17/2012 03:37 PM - In Progress

I added bret multiple times

Sasha Vanorman - 10/17/2012 03:36 PM - In Progress

test

Sasha Vanorman - 10/17/2012 03:36 PM - In Progress

test

Sasha Vanorman - 10/17/2012 03:35 PM - In Progress

Sasha Vanorman - 10/17/2012 03:35 PM - DS Approval

Issue:
testing

Sasha Vanorman - 10/17/2012 03:35 PM - Submitting

 

Sasha Vanorman - 10/17/2012 03:34 PM - Submitting

test
', N'DS Approval', N'Other Low Urgency', N'Maintain Compliance', 3, CAST(0x0000A0ED010149D6 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (249, N'ACCM Fed', N'', CAST(0x0000A0E700EAE9FF AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A0E700EAE9FF AS DateTime), CAST(0x0000A0E700EAE9FF AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A0E700EAE9FF AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (250, N'ACCM Fed', N'', CAST(0x0000A0E700EAEAF4 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A0E700EAEAF4 AS DateTime), CAST(0x0000A0E700EAEAF4 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A0E700EAEAF4 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (251, N'ACCM Fed', N'', CAST(0x0000A0E700EAEBEA AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A0E700EAEBEA AS DateTime), CAST(0x0000A0E700EAEBEA AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A0E700EAEBEA AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (252, N'ACCM Fed', N'', CAST(0x0000A0E700EAECE3 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A0E700EAECE3 AS DateTime), CAST(0x0000A0E700EAECE3 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A0E700EAECE3 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (253, N'ACCM Fed', N'', CAST(0x0000A0E700EAEDD8 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A0E700EAEDD8 AS DateTime), CAST(0x0000A0E700EAEDD8 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A0E700EAEDD8 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (254, N'ACCM Fed', N'', CAST(0x0000A0E700EAEECD AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A0E700EAEECD AS DateTime), CAST(0x0000A0E700EAEECD AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A0E700EAEECD AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (255, N'ACCM Fed', N'', CAST(0x0000A0E700EAEFC1 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A0E700EAEFC1 AS DateTime), CAST(0x0000A0E700EAEFC1 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A0E700EAEFC1 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (256, N'ACCM Fed', N'', CAST(0x0000A0E700EAF0B7 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A0E700EAF0B7 AS DateTime), CAST(0x0000A0E700EAF0B7 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A0E700EAF0B7 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (257, N'ACCM Fed', N'', CAST(0x0000A0E700EAF1A6 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A0E700EAF1A6 AS DateTime), CAST(0x0000A0E700EAF1A6 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A0E700EAF1A6 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (258, N'ACCM Fed', N'', CAST(0x0000A0E700EAF295 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A0E700EAF295 AS DateTime), CAST(0x0000A0E700EAF295 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A0E700EAF295 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (259, N'ACCM Fed', N'TEST', CAST(0x0000A0E700EAF381 AS DateTime), 30, N'', CAST(0x0000A11600000000 AS DateTime), N'TEST', N'', N'', N'', N'Verified', CAST(0x0000A11600000000 AS DateTime), CAST(0x0000A11600000000 AS DateTime), N'', N'Parish Snyder iv - 11/27/2012 02:57 PM - Verified

TEST

Parish Snyder iv - 11/27/2012 02:57 PM - Complete

TEST

Parish Snyder iv - 11/27/2012 02:57 PM - In Progress

TEST

Parish Snyder iv - 11/27/2012 02:57 PM - Approval

TEST

Issue:
TEST

Parish Snyder iv - 11/27/2012 02:56 PM - Submitting

TEST
', N'Complete', N'Other Low Urgency', N'Improve Customer Service', 2, CAST(0x0000A11600F667C3 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (260, N'BBO Fed', N'TEst', CAST(0x0000A0E800CD8B4A AS DateTime), 3, N'', CAST(0x0000A0E800000000 AS DateTime), N'Test

adding info to the issue area of this ticket after it was submitted and changed to the previous status', N'', N'', N'', N'Complete and Verified', CAST(0x0000A0ED00000000 AS DateTime), CAST(0x0000A0ED00000000 AS DateTime), N'', N'Sasha Vanorman - 10/17/2012 03:33 PM - Complete and Verified

Sasha Vanorman - 10/17/2012 03:33 PM - Complete

Sasha Vanorman - 10/17/2012 03:33 PM - In Progress

Sasha Vanorman - 10/17/2012 03:33 PM - Approval

testing 321

Issue:
Test

adding info to the issue area of this ticket after it was submitted and changed to the previous status

Sasha Vanorman - 10/17/2012 03:33 PM - Submitting

changing to previous status

Katie Van ausdal - 10/12/2012 12:29 PM - Approval

Issue:
Test
', N'Complete', N'Other Low Urgency', N'Other Customer Service', 2, CAST(0x0000A0ED0100595F AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (261, N'FNC Fed', N'', CAST(0x0000A0E800F248F5 AS DateTime), NULL, N'', CAST(0x0000A0ED00000000 AS DateTime), N' ', N'', N'', N'', N'Closed', CAST(0x0000A0ED00000000 AS DateTime), CAST(0x0000A0ED00000000 AS DateTime), N'', N'Sasha Vanorman - 10/17/2012 03:47 PM - Closed

Closing issue

Sasha Vanorman - 10/17/2012 03:46 PM - complete

updating status

Sasha Vanorman - 10/17/2012 03:46 PM - 

updating info

Sasha Vanorman - 10/17/2012 03:45 PM - Discussion

updating info

Sasha Vanorman - 10/17/2012 03:45 PM - Discussion

addinginfo

Sasha Vanorman - 10/17/2012 03:45 PM - Discussion

Releasing from hold: 



Sasha Vanorman - 10/17/2012 03:45 PM - Hold

adding notes

Sasha Vanorman - 10/17/2012 03:44 PM - Hold

Ticket Placed On Hold: 

placing ticket on  hold

Sasha Vanorman - 10/17/2012 03:44 PM - Discussion

send email to everyone

Sasha Vanorman - 10/17/2012 03:43 PM - Discussion

this ticket is currently in no one''s court

Sasha Vanorman - 10/17/2012 03:43 PM - Discussion

Court changed from Sasha Vanorman to  

Sasha Vanorman - 10/17/2012 03:42 PM - Discussion

Court changed from Kathryn Ferre to Sasha Vanorman

Sasha Vanorman - 10/17/2012 03:39 PM - Discussion

adding documents

Sasha Vanorman - 10/17/2012 03:30 PM - Discussion

added attachmente

Sasha Vanorman - 10/17/2012 03:27 PM - Discussion

removing subject

Sasha Vanorman - 10/17/2012 03:27 PM - Discussion

changing priority

Sasha Vanorman - 10/17/2012 03:26 PM - Discussion

changing priority

Sasha Vanorman - 10/17/2012 03:25 PM - Discussion

test

Sasha Vanorman - 10/17/2012 03:25 PM - Discussion

test

Sasha Vanorman - 10/17/2012 03:24 PM - Discussion

test

Sasha Vanorman - 10/17/2012 03:24 PM - Discussion

test

Sasha Vanorman - 10/17/2012 03:24 PM - Discussion

Issue:
 

Sasha Vanorman - 10/17/2012 03:23 PM - Submitting

already returned, will try to return again

Sasha Vanorman - 10/17/2012 03:23 PM - Submitting

returning

Sasha Vanorman - 10/17/2012 03:23 PM - Discussion

yrdy

Sasha Vanorman - 10/17/2012 03:23 PM - Discussion

Releasing from hold: 



Sasha Vanorman - 10/17/2012 03:23 PM - Hold

Ticket Placed On Hold: 

test

Sasha Vanorman - 10/17/2012 03:22 PM - Discussion

testing321

Sasha Vanorman - 10/17/2012 03:22 PM - Discussion

Issue:
 

Parish Snyder iv - 10/17/2012 10:10 AM - Submitting

Test

Parish Snyder iv - 10/17/2012 10:10 AM - Submitting

Test
', N'complete', N'Other High Urgency', N'Maintain Compliance', 9, CAST(0x0000A0ED010409A8 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (262, N'ACCM Fed', N'test ticket', CAST(0x0000A0EC00D58E0A AS DateTime), 35, N'', CAST(0x0000A0EC00000000 AS DateTime), N'adding issue', N'', N'', N'', N'Approval', CAST(0x0000A0EC00000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'test', N'Sasha Vanorman - 11/13/2012 05:44 PM - Approval

Court changed from Patty Sherman to Sasha Vanorman

Sasha Vanorman - 11/13/2012 05:44 PM - Approval

Court changed from Parish Snyder iv to Patty Sherman

Sasha Vanorman - 11/13/2012 05:42 PM - Approval

test

Sasha Vanorman - 10/23/2012 05:00 PM - Approval

test

Sasha Vanorman - 10/23/2012 04:58 PM - Approval

test

Sasha Vanorman - 10/16/2012 01:19 PM - Approval

test

Sasha Vanorman - 10/16/2012 01:18 PM - Approval

creating ticket

Issue:
adding issue
', N'Submitting', N'Other Low Urgency', N'Maintain Compliance', 3, CAST(0x0000A1080124100A AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (263, N'ARCM Fed', N'test ticket', CAST(0x0000A0EC00DB7ED4 AS DateTime), 35, N'', CAST(0x0000A0EC00000000 AS DateTime), N'adding a new ticket', N'', N'', N'', N'Verified', CAST(0x0000A0ED00000000 AS DateTime), CAST(0x0000A0ED00000000 AS DateTime), N'', N'Sasha Vanorman - 10/17/2012 03:18 PM - Verified

Sasha Vanorman - 10/17/2012 03:18 PM - Complete

Sasha Vanorman - 10/16/2012 01:21 PM - Approval

testing

Sasha Vanorman - 10/16/2012 01:20 PM - Approval

Issue:
adding a new ticket
', N'Complete', N'', N'Executive Request', 9, CAST(0x0000A0ED00FC21A9 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (264, N'LPDS Fed', N'test ticket', CAST(0x0000A0EC00DC3549 AS DateTime), 35, N'', CAST(0x0000A0EC00000000 AS DateTime), N'creating ticket ', N'', N'', N'', N'Verified', CAST(0x0000A0ED00000000 AS DateTime), CAST(0x0000A0ED00000000 AS DateTime), N'', N'Bret Pehrson - 10/17/2012 01:34 PM - Verified

Bret Pehrson - 10/17/2012 01:34 PM - Complete

Bret Pehrson - 10/17/2012 01:33 PM - In Progress

the request date worked correclty!

Sasha Vanorman - 10/16/2012 01:22 PM - Approval

creating ticket with a future requested date

Issue:
creating ticket 
', N'Complete', N'Other Low Urgency', N'Audit Request', 9, CAST(0x0000A0ED00DF7D5F AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (265, N'DCR Fed', N'', CAST(0x0000A0EC01097575 AS DateTime), 30, N'', CAST(0x0000A0ED00000000 AS DateTime), N'', N'', N'', N'', N'Submitting', CAST(0x0000A0EC01097575 AS DateTime), CAST(0x0000A0ED00000000 AS DateTime), N'', N'Bret Pehrson - 10/17/2012 12:27 PM - Submitting

test

Bret Pehrson - 10/17/2012 12:26 PM - Submitting

test

Bret Pehrson - 10/17/2012 12:22 PM - Submitting

test

Bret Pehrson - 10/17/2012 12:22 PM - Submitting

test

Bret Pehrson - 10/17/2012 12:07 PM - Submitting

test

Bret Pehrson - 10/17/2012 12:01 PM - Submitting

test

Bret Pehrson - 10/17/2012 12:00 PM - Submitting

test

Bret Pehrson - 10/17/2012 11:55 AM - Submitting

test

Bret Pehrson - 10/17/2012 11:53 AM - Submitting

test
', N'', N'', N'', 0, CAST(0x0000A0ED00CD2E0F AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (266, N'ACCM Fed', N'This is a test', CAST(0x0000A0ED00DADC39 AS DateTime), 30, N'', CAST(0x0000A0ED00000000 AS DateTime), N'This is a test', N'', N'', N'', N'Approval', CAST(0x0000A0ED00000000 AS DateTime), CAST(0x0000A0ED00000000 AS DateTime), N'', N'Bret Pehrson - 10/17/2012 01:17 PM - Approval

Issue:
This is a test
', N'Submitting', N'Impacts Small Number of Customers', N'Data Integrity', 3, CAST(0x0000A0ED00DB014A AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (267, N'ACTD Fed', N'Test - CornerStone Ticket', CAST(0x0000A0F300FD0EBC AS DateTime), 30, N'', CAST(0x0000A0F300000000 AS DateTime), N'Testing - CornerStone Ticket', N'', N'', N'', N'Approval', CAST(0x0000A0F300000000 AS DateTime), CAST(0x0000A0F300000000 AS DateTime), N'', N'Romney Slagowski - 10/26/2012 10:53 AM - Approval

Testing

Melanie Garfield - 10/23/2012 03:27 PM - Approval

Releasing from hold: 



Melanie Garfield - 10/23/2012 03:27 PM - Hold

Ticket Placed On Hold: 

Placing ticket on HOLD

Melanie Garfield - 10/23/2012 03:26 PM - Approval

Updating Court

Melanie Garfield - 10/23/2012 03:25 PM - Review

Testing - Send to ALL function with additional recipients

Melanie Garfield - 10/23/2012 03:24 PM - Review

Test Send to ALL function

Melanie Garfield - 10/23/2012 03:23 PM - Review

Testing - Send to Individual

Melanie Garfield - 10/23/2012 03:23 PM - Review

Testing CornerStone Ticket

Issue:
Testing - CornerStone Ticket
', N'Hold', N'Other Low Urgency', N'Other Compliance', 3, CAST(0x0000A0F600B32416 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (268, N'ARCM Fed', N'TEST', CAST(0x0000A0F400A1D18F AS DateTime), 3, N'', CAST(0x0000A0F400000000 AS DateTime), N'Testing, testing, 1, 2, 3', N'', N'', N'', N'Approval', CAST(0x0000A0F400000000 AS DateTime), CAST(0x0000A0F400000000 AS DateTime), N'', N'Romney Slagowski - 10/26/2012 10:50 AM - Approval

testing, testing, 2, 3, 4

Katie Van ausdal - 10/24/2012 09:50 AM - Approval

Issue:
Testing, testing, 1, 2, 3
', N'Submitting', N'Impacts Significant Number of Customers', N'Other Customer Service', 8, CAST(0x0000A0F600B25453 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (269, N'FAR Fed', N'Test ticket', CAST(0x0000A0F400A213FA AS DateTime), 3, N'', CAST(0x0000A0F400000000 AS DateTime), N'Testing ticket', N'', N'', N'', N'BS Approval', CAST(0x0000A0F400000000 AS DateTime), CAST(0x0000A0F400000000 AS DateTime), N'', N'Katie Van ausdal - 10/26/2012 08:11 AM - BS Approval

Test: Emaling Kam

Katie Van ausdal - 10/24/2012 10:12 AM - BS Approval

Email update to Romney

Katie Van ausdal - 10/24/2012 09:52 AM - BS Approval

Issue:
Testing ticket
', N'Submitting', N'Other Low Urgency', N'Other Customer Service', 2, CAST(0x0000A0F600869E09 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (270, N'POL Fed', N'TEST ', CAST(0x0000A0F600B35431 AS DateTime), 10, N'Credit Bureau Reporting', CAST(0x0000A0F400000000 AS DateTime), N'Test ticket....this is a test. ', N'Staff Error', N'testing resolution', N'testing test test test ', N'Withdrawn', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Parish Snyder iv - 11/13/2012 07:28 PM - BS Approval

Ticket Withdrawn: 

TEST

Cause:
Staff Error

Fix:
testing resolution

Prevention:
testing test test test 


Romney Slagowski - 10/26/2012 10:57 AM - BS Approval

Cause:
Staff Error

Fix:
testing resolution

Prevention:
testing test test test 


Romney Slagowski - 10/26/2012 10:56 AM - Discussion

testing 

Romney Slagowski - 10/26/2012 10:56 AM - Discussion

Court changed from Teri Vig to Romney Slagowski

Romney Slagowski - 10/26/2012 10:55 AM - Discussion

Issue:
Test ticket....this is a test. 

Romney Slagowski - 10/26/2012 10:54 AM - Submitting

Requester changed from Katie Van ausdal to  

Romney Slagowski - 10/26/2012 10:54 AM - Submitting

Requester changed from Romney Slagowski to Katie Van ausdal
', N'BS Approval', N'Prevents Disruption to Operations', N'Get in Compliance', 7, CAST(0x0000A1080140AED2 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (271, N'ARCM Fed', N'Testing', CAST(0x0000A0F600B6C97D AS DateTime), 10, N'', CAST(0x0000A0F600000000 AS DateTime), N'Test, Test, Test, Test, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestvvTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestvTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestvTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestvTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestvTest, Test, TestTest, Test, TestvTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestvvTest, Test, TestvTest, Test, TestvvvTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestvTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, Test', N'', N'', N'', N'Approval', CAST(0x0000A0F600000000 AS DateTime), CAST(0x0000A0F600000000 AS DateTime), N'', N'Romney Slagowski - 10/26/2012 12:30 PM - Approval

test

Romney Slagowski - 10/26/2012 12:30 PM - Approval

Court changed from Sasha Vanorman to Romney Slagowski

Romney Slagowski - 10/26/2012 12:25 PM - Approval

testing

Issue:
Test, Test, Test, Test, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestvvTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestvTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestvTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestvTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestvTest, Test, TestTest, Test, TestvTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestvvTest, Test, TestvTest, Test, TestvvvTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestvTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, TestTest, Test, Test
', N'Submitting', N'Creates Significant Operational Efficiency', N'Provide New Service', 8, CAST(0x0000A0F600CE0349 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (272, N'ARCC Fed', N'', CAST(0x0000A0F600C58FF2 AS DateTime), 3, N'', CAST(0x0000A0F600000000 AS DateTime), N'', N'', N'', N'', N'Withdrawn', CAST(0x0000A0F600000000 AS DateTime), CAST(0x0000A0F600000000 AS DateTime), N'', N'Romney Slagowski - 10/26/2012 12:26 PM - Submitting

Ticket Withdrawn: 

testing withdrawing blank ticket
', N'Submitting', N'', N'', 0, CAST(0x0000A0F600CCB30E AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (273, N'ARCA Fed', N'test', CAST(0x0000A10200870E63 AS DateTime), 30, N'', CAST(0x0000A10200000000 AS DateTime), N'test', N'', N'', N'', N'Verified', CAST(0x0000A10200000000 AS DateTime), CAST(0x0000A10200000000 AS DateTime), N'', N'Bret Pehrson - 11/07/2012 08:12 AM - Verified

test

Bret Pehrson - 11/07/2012 08:12 AM - Complete

test

Bret Pehrson - 11/07/2012 08:12 AM - In Progress

test

Bret Pehrson - 11/07/2012 08:12 AM - DS Approval

Issue:
test
', N'Complete', N'Improves Operational Efficiency', N'Data Integrity', 7, CAST(0x0000A10200872FB5 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (274, N'DCR Fed', N'All New Category', CAST(0x0000A10200ED6932 AS DateTime), 1, N'', CAST(0x0000A10200000000 AS DateTime), N'test', N'', N'', N'', N'Verified', CAST(0x0000A10200000000 AS DateTime), CAST(0x0000A10200000000 AS DateTime), N'', N'Bret Pehrson - 11/07/2012 03:37 PM - Verified

test

Bret Pehrson - 11/07/2012 03:37 PM - Complete

test

Bret Pehrson - 11/07/2012 03:37 PM - In Progress

test

Bret Pehrson - 11/07/2012 03:37 PM - DCR Approval

test

Issue:
test
', N'Complete', N'', N'Executive Request', 9, CAST(0x0000A10201017AF2 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (275, N'TOC Fed', N'', CAST(0x0000A1020103BF68 AS DateTime), 1, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A1020103BF68 AS DateTime), CAST(0x0000A1020103BF68 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A1020103BF68 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (276, N'ACCM Fed', N'TEST', CAST(0x0000A10800E7AA39 AS DateTime), 30, N'', CAST(0x0000A10800000000 AS DateTime), N'TEST

TEST

TEST

TEST', N'', N'', N'', N'Verified', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Parish Snyder iv - 11/13/2012 02:07 PM - Verified

TEST

TEST

TEST

Parish Snyder iv - 11/13/2012 02:05 PM - Complete

TEST

TEST

TEST

Parish Snyder iv - 11/13/2012 02:05 PM - In Progress

TEST

TEST

TEST

Parish Snyder iv - 11/13/2012 02:05 PM - Approval

Issue:
TEST

TEST

TEST

TEST
', N'Complete', N'Other Low Urgency', N'Data Integrity', 3, CAST(0x0000A10800E87E92 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (277, N'PRB Fed', N'testing director or it', CAST(0x0000A10800EA777F AS DateTime), 49, N'', CAST(0x0000A10800000000 AS DateTime), N'test', N'SAS/Script Problem', N'test', N'test', N'Withdrawn', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Parish Snyder iv - 11/13/2012 07:32 PM - BS Approval

Ticket Withdrawn: 

TEST

Cause:
SAS/Script Problem

Fix:
test

Prevention:
test


Sasha Vanorman - 11/13/2012 02:15 PM - BS Approval

Cause:
SAS/Script Problem

Fix:
test

Prevention:
test


Sasha Vanorman - 11/13/2012 02:15 PM - Discussion

test

Sasha Vanorman - 11/13/2012 02:15 PM - Discussion

test

Issue:
test

Sasha Vanorman - 11/13/2012 02:14 PM - Submitting

submitting ticket
', N'BS Approval', N'Other High Urgency', N'Data Integrity', 9, CAST(0x0000A1080141D1AC AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (278, N'ACCM Fed', N'', CAST(0x0000A10800EF4E55 AS DateTime), 30, N'', CAST(0x0000A10800000000 AS DateTime), N'', N'', N'', N'', N'Withdrawn', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Parish Snyder iv - 11/13/2012 02:33 PM - Submitting

Ticket Withdrawn: 

TEST

Parish Snyder iv - 11/13/2012 02:32 PM - Submitting

TEST

TEST

TEST

TEST

TEST

TEST

TEST

TEST

TEST

TEST

TEST

TEST

TEST

TEST

', N'Submitting', N'', N'', 0, CAST(0x0000A10800EF8C6E AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (279, N'ACTD Fed', N'', CAST(0x0000A10800F0B5ED AS DateTime), 30, N'', CAST(0x0000A10800000000 AS DateTime), N'', N'', N'', N'', N'Withdrawn', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Parish Snyder iv - 11/13/2012 02:37 PM - Submitting

Ticket Withdrawn: 

TEST

Parish Snyder iv - 11/13/2012 02:37 PM - Submitting

TEST

TEST

TEST

TEST

TESTEST

TEST

TESTTEST

TEST

TEST

TEST

TEST

TEST

TESTEST
', N'Submitting', N'', N'', 0, CAST(0x0000A10800F0E1CC AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (280, N'ARCA Fed', N'', CAST(0x0000A10800F144CC AS DateTime), 30, N'', CAST(0x0000A10800000000 AS DateTime), N'', N'', N'', N'', N'Withdrawn', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Parish Snyder iv - 11/13/2012 02:39 PM - Submitting

Ticket Withdrawn: 

TEST

TEST

TEST

TEST

TESTESET

Parish Snyder iv - 11/13/2012 02:39 PM - Submitting

TEST

TESTEST

TEST

TEST

TESTSTESTESET

TEST

TEST

TESTESTEST

TEST

TEST

TESTTEST
', N'Submitting', N'', N'', 0, CAST(0x0000A10800F16D60 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (281, N'ARCC Fed', N'', CAST(0x0000A10800F1A5D6 AS DateTime), 30, N'', CAST(0x0000A10800000000 AS DateTime), N'', N'', N'', N'', N'Withdrawn', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Parish Snyder iv - 11/13/2012 02:41 PM - Submitting

Ticket Withdrawn: 

TEST

TEST

TEST

TEST

TEST

Parish Snyder iv - 11/13/2012 02:41 PM - Submitting

TEST

TESTEETST

TESETSETSE

TESTESETSET

TESTESETESETEST

TESETSETSETSETSETSE

SETSETSETSETSETSETSET

SETSETSETSETSETSETSETSET

TESt
TESET

TEST

TEST

TEST
', N'Submitting', N'', N'', 0, CAST(0x0000A10800F1D885 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (282, N'ARCM Fed', N'', CAST(0x0000A10800F3EECD AS DateTime), 30, N'', CAST(0x0000A10800000000 AS DateTime), N'', N'', N'', N'', N'Withdrawn', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Parish Snyder iv - 11/13/2012 02:49 PM - Submitting

Ticket Withdrawn: 

TEST

TEST

TEST

Parish Snyder iv - 11/13/2012 02:49 PM - Submitting

TEST

TEST

TEST

TEST

TEST

TEST

TEST

TEST

TEST

TEST

TEST

TEST

TESTTESTEST

TESETTESTESTESTESTS

TEST

TESTTESTTEST
', N'Submitting', N'', N'', 0, CAST(0x0000A10800F4175B AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (283, N'BBO Fed', N'', CAST(0x0000A10800F5342C AS DateTime), 30, N'', CAST(0x0000A10800000000 AS DateTime), N'', N'', N'', N'', N'Withdrawn', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Parish Snyder iv - 11/13/2012 02:54 PM - Submitting

Ticket Withdrawn: 

TEST

TEST

TESTTETS

TESTTEST

Parish Snyder iv - 11/13/2012 02:54 PM - Submitting

TEST

TESTTEST

TEST

TESTTESTTEST

TEST

TESTTESTTEST

TESTTESET

TEST

TEST

TESTTEST

TEST

TESTTEST

TEST

TEST
', N'Submitting', N'', N'', 0, CAST(0x0000A10800F579F0 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (284, N'DCR Fed', N'', CAST(0x0000A10800F6C582 AS DateTime), 30, N'', CAST(0x0000A10800000000 AS DateTime), N'', N'', N'', N'', N'Withdrawn', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Parish Snyder iv - 11/13/2012 03:00 PM - Submitting

Ticket Withdrawn: 

TEST

TEST

TESTTEST

TEST

TEST

Parish Snyder iv - 11/13/2012 02:59 PM - Submitting

TEST

TESTTEST

TEST

TEST

TESTTESTTEST

TEST

TEST

TESTTESTTEST

TEST
TEST

TESTTEST

TST
TEst

TEST

TEST
', N'Submitting', N'', N'', 0, CAST(0x0000A10800F6F75B AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (285, N'FAR Fed', N'TEST', CAST(0x0000A108010B0B07 AS DateTime), 30, N'', CAST(0x0000A10800000000 AS DateTime), N'TEST', N'', N'', N'', N'Resolved', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Parish Snyder iv - 11/13/2012 04:18 PM - Resolved

Parish Snyder iv - 11/13/2012 04:18 PM - QC Approval

Parish Snyder iv - 11/13/2012 04:17 PM - OPA - In Progress

Parish Snyder iv - 11/13/2012 04:17 PM - BS Approval

TEST

TESTTEST

TEST

TESTTESTTEST

TEST
tEST

TEST

TESTTEST

Issue:
TEST
', N'QC Approval', N'Other Low Urgency', N'Get in Compliance', 3, CAST(0x0000A108010C754D AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (286, N'FNC Fed', N'', CAST(0x0000A108011F466E AS DateTime), 30, N'', CAST(0x0000A10800000000 AS DateTime), N'', N'', N'', N'', N'Withdrawn', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Parish Snyder iv - 11/13/2012 05:27 PM - Submitting

Ticket Withdrawn: 

TEST

TESTETSETEST

Parish Snyder iv - 11/13/2012 05:27 PM - Submitting

TEST

TEST

TESTTESTTEST

TEST

TEST

TEST

TEST

TEST

TEST

TEST

TEST

TEST

TESTTESTTEST

TEST
', N'Submitting', N'', N'', 0, CAST(0x0000A108011F7C51 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (287, N'ACCM Fed', N'', CAST(0x0000A1080122156D AS DateTime), 30, N'', CAST(0x0000A10800000000 AS DateTime), N'', N'', N'', N'', N'Submitting', CAST(0x0000A1080122156D AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Sasha Vanorman - 11/13/2012 05:37 PM - Submitting

Court changed from Parish Snyder iv to Sasha Vanorman

Parish Snyder iv - 11/13/2012 05:37 PM - Submitting

Court changed from Sasha Vanorman to Parish Snyder iv

Parish Snyder iv - 11/13/2012 05:37 PM - Submitting

Court changed from Parish Snyder iv to Sasha Vanorman
', N'', N'', N'', 0, CAST(0x0000A10801224E1A AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (288, N'FTRANS Fed', N'TEST', CAST(0x0000A108012632D9 AS DateTime), 30, N'', CAST(0x0000A10800000000 AS DateTime), N'TEST', N'', N'', N'', N'OPA - In Progress', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Parish Snyder iv - 11/13/2012 05:52 PM - OPA - In Progress

TEST

TEST

TESTEST

TESTTESTTESTES

TESTESTESTES

TESTESTEST

T
T
T
T
T
T
T


Issue:
TEST
', N'Submitting', N'Other Low Urgency', N'Provide New Service', 2, CAST(0x0000A10801267206 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (289, N'FTRANS Fed', N'TEST', CAST(0x0000A108012B36F2 AS DateTime), 30, N'', CAST(0x0000A10800000000 AS DateTime), N'TEST', N'', N'', N'', N'OPA - In Progress', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Parish Snyder iv - 11/13/2012 06:10 PM - OPA - In Progress

TEST

TEST

TEST

TEST

TESTTEST

TEST

TEST

TEST

TEST

TESTTEST

Issue:
TEST
', N'Submitting', N'Other Low Urgency', N'Improve Customer Service', 2, CAST(0x0000A108012B6091 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (290, N'FTRANS Fed', N'TEST', CAST(0x0000A108012D3746 AS DateTime), 30, N'', CAST(0x0000A10800000000 AS DateTime), N'TEST', N'', N'', N'', N'OPA - In Progress', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Parish Snyder iv - 11/13/2012 06:17 PM - OPA - In Progress

TEST

Issue:
TEST
', N'Submitting', N'Other Low Urgency', N'Improve Operational Efficiency', 1, CAST(0x0000A108012D53E8 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (291, N'FTRANS Fed', N'TEST', CAST(0x0000A108012E3427 AS DateTime), 30, N'', CAST(0x0000A10800000000 AS DateTime), N'TEST', N'', N'', N'', N'OPA - In Progress', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Parish Snyder iv - 11/13/2012 06:21 PM - OPA - In Progress

TEST

Issue:
TEST
', N'Submitting', N'Other High Urgency', N'Improve Employee Morale', 5, CAST(0x0000A108012E4AD8 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (292, N'LPDS Fed', N'', CAST(0x0000A1080138C592 AS DateTime), 30, N'', CAST(0x0000A10800000000 AS DateTime), N'TEST', N'', N'', N'', N'Verified', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Parish Snyder iv - 11/13/2012 07:00 PM - Verified

TEST

Parish Snyder iv - 11/13/2012 07:00 PM - Complete

TEST

Parish Snyder iv - 11/13/2012 07:00 PM - In Progress

TEST

Parish Snyder iv - 11/13/2012 07:00 PM - Approval

asdf

asdf

asdfasdf

asdfasdf

asdfasdfasdf

asdf
asdf

asdf

asdf

asdfasdfasdf

asdf

Issue:
TEST
', N'Complete', N'Other Low Urgency', N'Get in Compliance', 3, CAST(0x0000A10801390B70 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (293, N'LPDS Fed', N'', CAST(0x0000A108013948D8 AS DateTime), 30, N'', CAST(0x0000A10800000000 AS DateTime), N'asdf', N'', N'', N'', N'Withdrawn', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Parish Snyder iv - 11/13/2012 07:01 PM - Submitting

Ticket Withdrawn: 

asdf

asdf

asdfasdf

asdfasdf

Parish Snyder iv - 11/13/2012 07:01 PM - Submitting

asdfasdf

asdf

asdfasdf

asdf
asdf

asdf

asdf

asdf

asdf

asdfasdf

asdfasdf

asdf

', N'Submitting', N'', N'', 0, CAST(0x0000A108013967F8 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (294, N'OTH Fed', N'asdf', CAST(0x0000A108013ABEE4 AS DateTime), 30, N'', CAST(0x0000A10800000000 AS DateTime), N'asdfasdf

asdf

asdfasdf

asdf', N'', N'', N'', N'Withdrawn', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Parish Snyder iv - 11/13/2012 07:07 PM - Discussion

Ticket Withdrawn: 

asdf

Parish Snyder iv - 11/13/2012 07:07 PM - Discussion

asdfasdf

asdfasdf

asdf
asdf

asdf

asdf

asdfasdf

asdf

Issue:
asdfasdf

asdf

asdfasdf

asdf
', N'Discussion', N'Other Low Urgency', N'Maintain Compliance', 3, CAST(0x0000A108013AF4A2 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (295, N'QASMT Fed', N'asdf', CAST(0x0000A10801429393 AS DateTime), 30, N'', CAST(0x0000A10800000000 AS DateTime), N'asdfasdf', N'', N'', N'', N'Withdrawn', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Parish Snyder iv - 11/13/2012 07:35 PM - Submitting

Ticket Withdrawn: 

asdfasdf

asdf

asdf

asdf

asdfasdf

Parish Snyder iv - 11/13/2012 07:35 PM - Submitting

asdfasdf

asdf

asdfasdf


asdfasdf

asdf

asdf

asdf

asdf


', N'Submitting', N'', N'', 0, CAST(0x0000A1080142B56A AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (296, N'QCR Fed', N'', CAST(0x0000A1080143B6C5 AS DateTime), 30, N'', CAST(0x0000A10800000000 AS DateTime), N'asdf', N'', N'', N'', N'Withdrawn', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Parish Snyder iv - 11/13/2012 07:39 PM - Submitting

Ticket Withdrawn: 

asdfasdf

asdf

Parish Snyder iv - 11/13/2012 07:39 PM - Submitting

asdf

asdf

asdf

asdfasdf

asdf

asdfasdf

asdfasdf

asdf

asdfasdf

asdfasdfasdfasdf
', N'Submitting', N'', N'', 0, CAST(0x0000A1080143D004 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (297, N'SCHELIG Fed', N'', CAST(0x0000A10801454FCB AS DateTime), 30, N'', CAST(0x0000A10800000000 AS DateTime), N'test', N'', N'', N'', N'Withdrawn', CAST(0x0000A10800000000 AS DateTime), CAST(0x0000A10800000000 AS DateTime), N'', N'Parish Snyder iv - 11/13/2012 07:45 PM - Submitting

Ticket Withdrawn: 

asdfasdf

Parish Snyder iv - 11/13/2012 07:45 PM - Submitting

asdf

asdf

asdf

asdfasdf

asdf

asdfasdf

asdfasdf

asdf

asdfasdfasdf

asdf

asdf
', N'Submitting', N'', N'', 0, CAST(0x0000A108014565A5 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (298, N'FNC Fed', N'TEST', CAST(0x0000A10900C60EEA AS DateTime), 30, N'', CAST(0x0000A10900000000 AS DateTime), N'TEST

TEST', N'Process Complete', N'TEST', N'TEST', N'Resolved', CAST(0x0000A10900000000 AS DateTime), CAST(0x0000A10900000000 AS DateTime), N'', N'Parish Snyder iv - 11/14/2012 12:03 PM - Resolved

Cause:
Process Complete

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 11/14/2012 12:02 PM - BS Approval

Cause:
Process Complete

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 11/14/2012 12:02 PM - Discussion

TEST

TEST

Parish Snyder iv - 11/14/2012 12:02 PM - Discussion

TESET

TESET

TEST

TESTTEST

Issue:
TEST

TEST
', N'BS Approval', N'Other Low Urgency', N'Get in Compliance', 3, CAST(0x0000A10900C6501C AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (299, N'SPH Fed', N'', CAST(0x0000A10900C94B07 AS DateTime), 30, N'', CAST(0x0000A10900000000 AS DateTime), N'TEST

TEST

asdf

asdf', N'', N'', N'', N'Withdrawn', CAST(0x0000A10900000000 AS DateTime), CAST(0x0000A10900000000 AS DateTime), N'', N'Parish Snyder iv - 11/14/2012 12:14 PM - Submitting

Ticket Withdrawn: 

asdf

asdf

Parish Snyder iv - 11/14/2012 12:14 PM - Submitting

asdf

asdf

asdfasdf

Parish Snyder iv - 11/14/2012 12:14 PM - Submitting

asdf

asdf

asdfasdf

asdf

asdf

asdfasdf
', N'Submitting', N'', N'', 0, CAST(0x0000A10900C96DF5 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (300, N'TOC Fed', N'', CAST(0x0000A10900C9A91D AS DateTime), 30, N'', CAST(0x0000A10900000000 AS DateTime), N'TEST

TEST

asdf

asdfasdf', N'', N'', N'', N'Withdrawn', CAST(0x0000A10900000000 AS DateTime), CAST(0x0000A10900000000 AS DateTime), N'', N'Parish Snyder iv - 11/14/2012 12:15 PM - Submitting

Ticket Withdrawn: 

asdf

asdfasdf

Parish Snyder iv - 11/14/2012 12:15 PM - Submitting

asdf

asdf

asdfasdf

Parish Snyder iv - 11/14/2012 12:15 PM - Submitting

asdf

asdf

asdfasdf

asdf

asdfasdf
', N'Submitting', N'', N'', 0, CAST(0x0000A10900C9C9B2 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (301, N'FNC Fed', N'TEST', CAST(0x0000A10900CDAC74 AS DateTime), 30, N'Autodialer', CAST(0x0000A10900000000 AS DateTime), N'TEST', N'SAS/Script Problem', N'TEST', N'TEST', N'Resolved', CAST(0x0000A10900000000 AS DateTime), CAST(0x0000A10900000000 AS DateTime), N'', N'Parish Snyder iv - 11/14/2012 12:30 PM - Resolved

TeST

Cause:
SAS/Script Problem

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 11/14/2012 12:30 PM - BS Approval

TEST

Cause:
SAS/Script Problem

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 11/14/2012 12:30 PM - Discussion

TEST

Issue:
TEST
', N'BS Approval', N'Other Low Urgency', N'Maintain Compliance', 3, CAST(0x0000A10900CDDC89 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (302, N'FNC Fed', N'TEST', CAST(0x0000A10900D3A676 AS DateTime), 30, N'Bankruptcy', CAST(0x0000A10900000000 AS DateTime), N'TEST', N'Staff Error', N'TEST', N'TEST', N'Resolved', CAST(0x0000A10900000000 AS DateTime), CAST(0x0000A10900000000 AS DateTime), N'', N'Parish Snyder iv - 11/14/2012 12:52 PM - Resolved

Cause:
Staff Error

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 11/14/2012 12:52 PM - BS Approval

TEST

Cause:
Staff Error

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 11/14/2012 12:51 PM - Discussion

TEST

Parish Snyder iv - 11/14/2012 12:51 PM - Discussion

TEST

Issue:
TEST
', N'BS Approval', N'Other Low Urgency', N'Maintain Compliance', 3, CAST(0x0000A10900D3D0E7 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (303, N'POL Fed', N'TEST', CAST(0x0000A10900D4680F AS DateTime), 30, N'', CAST(0x0000A10900000000 AS DateTime), N'TEST', N'Staff Error', N'TEST', N'TEST', N'Resolved', CAST(0x0000A10900000000 AS DateTime), CAST(0x0000A10900000000 AS DateTime), N'', N'Parish Snyder iv - 11/14/2012 12:54 PM - Resolved

TEST

Cause:
Staff Error

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 11/14/2012 12:54 PM - BS Approval

TEST

Cause:
Staff Error

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 11/14/2012 12:54 PM - Discussion

TEST

Issue:
TEST
', N'BS Approval', N'Other Low Urgency', N'Improve Customer Service', 2, CAST(0x0000A10900D49398 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (304, N'PRB Fed', N'TEST', CAST(0x0000A10900D5485B AS DateTime), 30, N'', CAST(0x0000A10900000000 AS DateTime), N'TSST', N'SAS/Script Problem', N'TEST', N'TEST', N'Resolved', CAST(0x0000A10900000000 AS DateTime), CAST(0x0000A10900000000 AS DateTime), N'', N'Parish Snyder iv - 11/14/2012 12:58 PM - Resolved

TEST

Cause:
SAS/Script Problem

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 11/14/2012 12:58 PM - BS Approval

TEST

Cause:
SAS/Script Problem

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 11/14/2012 12:57 PM - Discussion

TEST

Issue:
TSST
', N'BS Approval', N'Other Low Urgency', N'Other Efficiency', 1, CAST(0x0000A10900D57861 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (305, N'SPH Fed', N'TEST', CAST(0x0000A10900D663E6 AS DateTime), 30, N'', CAST(0x0000A10900000000 AS DateTime), N'TEST', N'SAS/Script Problem', N'TEST', N'TEST', N'Resolved', CAST(0x0000A10900000000 AS DateTime), CAST(0x0000A10900000000 AS DateTime), N'', N'Parish Snyder iv - 11/14/2012 01:02 PM - Resolved

TEST

Cause:
SAS/Script Problem

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 11/14/2012 01:02 PM - BS Approval

TEST

Cause:
SAS/Script Problem

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 11/14/2012 01:01 PM - Discussion

TEST

Issue:
TEST
', N'BS Approval', N'Other Low Urgency', N'Improve Employee Morale', 1, CAST(0x0000A10900D68C6D AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (306, N'FNC Fed', N'TEST', CAST(0x0000A10E00EAD9AB AS DateTime), 30, N'', CAST(0x0000A10E00000000 AS DateTime), N'TEST', N'File Transferred', N'TEST', N'TEST', N'Resolved', CAST(0x0000A10E00000000 AS DateTime), CAST(0x0000A10E00000000 AS DateTime), N'', N'Parish Snyder iv - 11/19/2012 02:34 PM - Resolved

TEST

Cause:
File Transferred

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 11/19/2012 02:34 PM - BS Approval

TEST

Cause:
File Transferred

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 11/19/2012 02:34 PM - Discussion

TEST

Issue:
TEST
', N'BS Approval', N'Creates Significant Operational Efficiency', N'Data Integrity', 9, CAST(0x0000A10E00F024D4 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (307, N'OTH Fed', N'TEST', CAST(0x0000A10E00F03BDC AS DateTime), 30, N'', CAST(0x0000A10E00000000 AS DateTime), N'TEST', N'SAS/Script Problem', N'TEST', N'TEST', N'Resolved', CAST(0x0000A10E00000000 AS DateTime), CAST(0x0000A10E00000000 AS DateTime), N'', N'Parish Snyder iv - 11/19/2012 02:35 PM - Resolved

TEST

Cause:
SAS/Script Problem

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 11/19/2012 02:35 PM - BS Approval

Cause:
SAS/Script Problem

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 11/19/2012 02:35 PM - Discussion

TEST

Issue:
TEST
', N'BS Approval', N'Creates Significant Operational Efficiency', N'Audit Request', 9, CAST(0x0000A10E00F06216 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (308, N'ARCA Fed', N'', CAST(0x0000A11600942612 AS DateTime), 30, N'', CAST(0x0000A12A00000000 AS DateTime), N'', N'', N'', N'', N'Submitting', CAST(0x0000A11600942612 AS DateTime), CAST(0x0000A11600942612 AS DateTime), N'', N'', N'', N'', N'', 0, CAST(0x0000A12A00A50C6F AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (309, N'ACCM Fed', N'TEST', CAST(0x0000A1160114CC5A AS DateTime), 30, N'', CAST(0x0000A11600000000 AS DateTime), N'TEST', N'', N'', N'', N'In Progress', CAST(0x0000A11600000000 AS DateTime), CAST(0x0000A11600000000 AS DateTime), N'', N'Parish Snyder iv - 11/27/2012 04:49 PM - In Progress

TEST

Parish Snyder iv - 11/27/2012 04:48 PM - Approval

TEST

Issue:
TEST
', N'Approval', N'Other Low Urgency', N'Provide Cost Benefit', 1, CAST(0x0000A116011507BC AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (310, N'ACCM Fed', N'TEST', CAST(0x0000A11700A2B2AC AS DateTime), 30, N'', CAST(0x0000A11700000000 AS DateTime), N'TEST', N'', N'', N'', N'Approval', CAST(0x0000A11700000000 AS DateTime), CAST(0x0000A11700000000 AS DateTime), N'', N'Parish Snyder iv - 11/28/2012 09:53 AM - Approval

TEST

Issue:
TEST
', N'Submitting', N'Other Low Urgency', N'Get in Compliance', 3, CAST(0x0000A11700A2E7B5 AS DateTime), N'', N'', N'TEST', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (311, N'ACTD Fed', N'', CAST(0x0000A11700A43E88 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11700A43E88 AS DateTime), CAST(0x0000A11700A43E88 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11700A43E88 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (312, N'ACCM Fed', N'', CAST(0x0000A11800A9115C AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11800A9115C AS DateTime), CAST(0x0000A11800A9115C AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11800A9115C AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (313, N'ACTD Fed', N'', CAST(0x0000A11800A91A42 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11800A91A42 AS DateTime), CAST(0x0000A11800A91A42 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11800A91A42 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (314, N'ARCA Fed', N'', CAST(0x0000A11800A921AF AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11800A921AF AS DateTime), CAST(0x0000A11800A921AF AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11800A921AF AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (315, N'ARCC Fed', N'', CAST(0x0000A11800A928A8 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11800A928A8 AS DateTime), CAST(0x0000A11800A928A8 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11800A928A8 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (316, N'ARCM Fed', N'', CAST(0x0000A11800A931A4 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11800A931A4 AS DateTime), CAST(0x0000A11800A931A4 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11800A931A4 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (317, N'BBO Fed', N'', CAST(0x0000A11800A94353 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11800A94353 AS DateTime), CAST(0x0000A11800A94353 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11800A94353 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (318, N'DCR Fed', N'', CAST(0x0000A11800A9B1E8 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11800A9B1E8 AS DateTime), CAST(0x0000A11800A9B1E8 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11800A9B1E8 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (319, N'FAR Fed', N'', CAST(0x0000A11800A9BAC2 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11800A9BAC2 AS DateTime), CAST(0x0000A11800A9BAC2 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11800A9BAC2 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (320, N'FAR Fed', N'', CAST(0x0000A11800A9C328 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11800A9C328 AS DateTime), CAST(0x0000A11800A9C328 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11800A9C328 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (321, N'FTRANS Fed', N'', CAST(0x0000A11800A9CC34 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11800A9CC34 AS DateTime), CAST(0x0000A11800A9CC34 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11800A9CC34 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (322, N'IDEM Fed', N'', CAST(0x0000A11800A9D73F AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11800A9D73F AS DateTime), CAST(0x0000A11800A9D73F AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11800A9D73F AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (323, N'LPDS Fed', N'', CAST(0x0000A11800A9E10E AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11800A9E10E AS DateTime), CAST(0x0000A11800A9E10E AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11800A9E10E AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (324, N'OTH Fed', N'', CAST(0x0000A11800A9E9CE AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11800A9E9CE AS DateTime), CAST(0x0000A11800A9E9CE AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11800A9E9CE AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (325, N'POL Fed', N'', CAST(0x0000A11800A9F163 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11800A9F163 AS DateTime), CAST(0x0000A11800A9F163 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11800A9F163 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (326, N'QASMT Fed', N'', CAST(0x0000A11800A9FA45 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11800A9FA45 AS DateTime), CAST(0x0000A11800A9FA45 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11800A9FA45 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (327, N'QCR Fed', N'', CAST(0x0000A11800AA0614 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11800AA0614 AS DateTime), CAST(0x0000A11800AA0614 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11800AA0614 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (328, N'SCHELIG Fed', N'', CAST(0x0000A11800AA1004 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11800AA1004 AS DateTime), CAST(0x0000A11800AA1004 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11800AA1004 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (329, N'SPH Fed', N'', CAST(0x0000A11800AA195F AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11800AA195F AS DateTime), CAST(0x0000A11800AA195F AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11800AA195F AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (330, N'FNC Fed', N'', CAST(0x0000A11800AA2364 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11800AA2364 AS DateTime), CAST(0x0000A11800AA2364 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11800AA2364 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (331, N'PRB Fed', N'', CAST(0x0000A11800AA2B0E AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11800AA2B0E AS DateTime), CAST(0x0000A11800AA2B0E AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11800AA2B0E AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (332, N'TOC Fed', N'', CAST(0x0000A11800AA3252 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11800AA3252 AS DateTime), CAST(0x0000A11800AA3252 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11800AA3252 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (333, N'TOC Fed', N'', CAST(0x0000A11900B80EAE AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11900B80EAE AS DateTime), CAST(0x0000A11900B80EAE AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11900B80EAE AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (334, N'ACCM Fed', N'', CAST(0x0000A11900D3936F AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11900D3936F AS DateTime), CAST(0x0000A11900D3936F AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11900D3936F AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (335, N'ACTD Fed', N'', CAST(0x0000A11900D39A02 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11900D39A02 AS DateTime), CAST(0x0000A11900D39A02 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11900D39A02 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (336, N'ARCA Fed', N'', CAST(0x0000A11900D3A0E2 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11900D3A0E2 AS DateTime), CAST(0x0000A11900D3A0E2 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11900D3A0E2 AS DateTime), N'', N'', NULL, NULL, NULL)
GO
print 'Processed 200 total records'
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (337, N'ARCC Fed', N'', CAST(0x0000A11900D3A70A AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11900D3A70A AS DateTime), CAST(0x0000A11900D3A70A AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11900D3A70A AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (338, N'ARCM Fed', N'', CAST(0x0000A11900D3B86B AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11900D3B86B AS DateTime), CAST(0x0000A11900D3B86B AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11900D3B86B AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (339, N'BBO Fed', N'', CAST(0x0000A11900D3C099 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11900D3C099 AS DateTime), CAST(0x0000A11900D3C099 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11900D3C099 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (340, N'DCR Fed', N'', CAST(0x0000A11900D3C6F4 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11900D3C6F4 AS DateTime), CAST(0x0000A11900D3C6F4 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11900D3C6F4 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (341, N'FAR Fed', N'', CAST(0x0000A11900D3CD94 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11900D3CD94 AS DateTime), CAST(0x0000A11900D3CD94 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11900D3CD94 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (342, N'FTRANS Fed', N'', CAST(0x0000A11900D3D757 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11900D3D757 AS DateTime), CAST(0x0000A11900D3D757 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11900D3D757 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (343, N'IDEM Fed', N'', CAST(0x0000A11900D3E0C5 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11900D3E0C5 AS DateTime), CAST(0x0000A11900D3E0C5 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11900D3E0C5 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (344, N'LPDS Fed', N'', CAST(0x0000A11900D3E92D AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11900D3E92D AS DateTime), CAST(0x0000A11900D3E92D AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11900D3E92D AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (345, N'OTH Fed', N'', CAST(0x0000A11900D3F490 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11900D3F490 AS DateTime), CAST(0x0000A11900D3F490 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11900D3F490 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (346, N'POL Fed', N'', CAST(0x0000A11900D4066E AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11900D4066E AS DateTime), CAST(0x0000A11900D4066E AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11900D4066E AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (347, N'QASMT Fed', N'', CAST(0x0000A11900D40E9A AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11900D40E9A AS DateTime), CAST(0x0000A11900D40E9A AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11900D40E9A AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (348, N'QCR Fed', N'', CAST(0x0000A11900D41583 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11900D41583 AS DateTime), CAST(0x0000A11900D41583 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11900D41583 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (349, N'SCHELIG Fed', N'', CAST(0x0000A11900D41D7E AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11900D41D7E AS DateTime), CAST(0x0000A11900D41D7E AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11900D41D7E AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (350, N'SPH Fed', N'TEST', CAST(0x0000A11900D4250F AS DateTime), 30, N'', CAST(0x0000A11900000000 AS DateTime), N'TEST', N'System Problem', N'TEST', N'TEST', N'Resolved', CAST(0x0000A11900000000 AS DateTime), CAST(0x0000A11900000000 AS DateTime), N'', N'Parish Snyder iv - 11/30/2012 04:51 PM - Resolved

TEST

Cause:
System Problem

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 11/30/2012 04:50 PM - BS Approval

TEST

Cause:
System Problem

Fix:
TEST

Prevention:
TEST


Parish Snyder iv - 11/30/2012 04:50 PM - Discussion

TEST

Issue:
TEST
', N'BS Approval', N'Other Low Urgency', N'Maintain Compliance', 3, CAST(0x0000A1190115B6F1 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (351, N'FNC Fed', N'', CAST(0x0000A11900D42D52 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11900D42D52 AS DateTime), CAST(0x0000A11900D42D52 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11900D42D52 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (352, N'PRB Fed', N'', CAST(0x0000A11900D434CF AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11900D434CF AS DateTime), CAST(0x0000A11900D434CF AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11900D434CF AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (353, N'TOC Fed', N'', CAST(0x0000A11900D43BC1 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11900D43BC1 AS DateTime), CAST(0x0000A11900D43BC1 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11900D43BC1 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (354, N'SPH Fed', N'', CAST(0x0000A11901134B8C AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11901134B8C AS DateTime), CAST(0x0000A11901134B8C AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11901134B8C AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (355, N'ARCA Fed', N'', CAST(0x0000A1190113C30F AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A1190113C30F AS DateTime), CAST(0x0000A1190113C30F AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A1190113C30F AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (356, N'ACCM Fed', N'', CAST(0x0000A11901168500 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A11901168500 AS DateTime), CAST(0x0000A11901168500 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A11901168500 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (357, N'ACCM Fed', N'', CAST(0x0000A119011DE2B0 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A119011DE2B0 AS DateTime), CAST(0x0000A119011DE2B0 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A119011DE2B0 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (358, N'DCR Fed', N'', CAST(0x0000A1F000A0FBB9 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A1F000A0FBB9 AS DateTime), CAST(0x0000A1F000A0FBB9 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A1F000A0FBB9 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (359, N'SCHELIG Fed', N'', CAST(0x0000A1F000A3D910 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A1F000A3D910 AS DateTime), CAST(0x0000A1F000A3D910 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A1F000A3D910 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (360, N'ARCM Fed', N'', CAST(0x0000A1F000A433B3 AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A1F000A433B3 AS DateTime), CAST(0x0000A1F000A433B3 AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A1F000A433B3 AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (361, N'ARCC Fed', N'', CAST(0x0000A1F000A6FAFF AS DateTime), 30, N'', NULL, N'', N'', N'', N'', N'Submitting', CAST(0x0000A1F000A6FAFF AS DateTime), CAST(0x0000A1F000A6FAFF AS DateTime), N'', N'', N'', N'', N'', NULL, CAST(0x0000A1F000A6FAFF AS DateTime), N'', N'', NULL, NULL, NULL)
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (362, N'DCR Fed', N'', CAST(0x0000A22E00B4016A AS DateTime), 30, N'', CAST(0x0000A24500000000 AS DateTime), N'', N'', N'', N'', N'Withdrawn', CAST(0x0000A24500000000 AS DateTime), CAST(0x0000A24500000000 AS DateTime), N'', N'Bret Pehrson - 09/26/2013 11:06 AM - Submitting

Ticket Withdrawn: 

test

Bret Pehrson - 09/24/2013 02:50 PM - Submitting

Return Return

Bret Pehrson - 09/24/2013 02:50 PM - Submitting

save save

Bret Pehrson - 09/24/2013 02:45 PM - Submitting

Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit Submit 

Bret Pehrson - 09/24/2013 02:43 PM - Submitting

UUpdate Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update pdate Update Update 

Bret Pehrson - 09/24/2013 02:41 PM - Submitting

test test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test test

Bret Pehrson - 09/24/2013 02:41 PM - Submitting

test test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test test

Bret Pehrson - 09/24/2013 02:43 PM - Submitting

UUpdate Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update Update pdate Update Update 

Bret Pehrson - 09/24/2013 02:41 PM - Submitting

test test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test test

Bret Pehrson - 09/24/2013 02:41 PM - Submitting

test test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test testtest test test

', N'Submitting', N'', N'', 0, CAST(0x0000A24500B71D77 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (363, N'DCR Fed', N'test', CAST(0x0000A24500B321DD AS DateTime), 30, N'', CAST(0x0000A24500000000 AS DateTime), N'test', N'', N'', N'', N'DCR Approval', CAST(0x0000A24500000000 AS DateTime), CAST(0x0000A24500000000 AS DateTime), N'', N'Bret Pehrson - 09/26/2013 10:59 AM - DCR Approval

test

Issue:
test

', N'Submitting', N'', N'Executive Request', 9, CAST(0x0000A24500B5063A AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (364, N'DCR Fed', N'test', CAST(0x0000A24500B7344F AS DateTime), 30, N'', CAST(0x0000A24500000000 AS DateTime), N'test', N'', N'', N'', N'Discussion', CAST(0x0000A24500000000 AS DateTime), CAST(0x0000A24500000000 AS DateTime), N'', N'Jarom Ryan - 11/01/2013 02:27 PM - Discussion

Releasing from hold: 



Jarom Ryan - 10/04/2013 09:29 AM - Hold

No progress has been made

Eric Barnes - 09/10/2013 02:00 PM - Hold

Ticket Placed On Hold: 

Placing on hold as this is not necessary until new loans are imported.

Jarom Ryan - 09/03/2013 10:11 AM - Discussion

This is still an outstanding issue, I have not had time to look at this.

Eric Barnes - 07/09/2013 02:35 PM - Discussion

Court changed from Jesse Gutierrez to Jarom Ryan

Eric Barnes - 07/09/2013 02:34 PM - Discussion

Issue:
Please run a query to determine why some accounts were not selected to get a welcome letter. 

The Loan add date= 03/18/2013 
Does not have ARC =WELCC, WELCF, WELCA, WELCD,WELCO

Eric Barnes - 07/09/2013 02:22 PM - Submitting

Court changed from Wendy Hack to Jarom Ryan

Eric Barnes - 07/09/2013 02:21 PM - Submitting

Returning to Jarom Ryan.

Jarom Ryan - 07/09/2013 09:11 AM - Submitting

I hit the return button by mistake.  Wendy can you hit submit and return the ticket to me?

Jarom Ryan - 07/09/2013 09:10 AM - Submitting

This is still an outstanding issue, I have not had time to look at this.

Wendy Hack - 04/08/2013 07:04 AM - Discussion

Moving to Jarom to provide explaination on the reson these borrowers were excluded. 

Wendy Hack - 03/28/2013 07:59 AM - Discussion

I am still waiting for an answer on why this happened. I am lowering the priority since the letters have gone out. 

Jarom Ryan - 03/25/2013 07:14 AM - Discussion

Really placing in  Wendy''s court

Jarom Ryan - 03/25/2013 07:14 AM - Discussion

I re-ran the query we had 17100 borrowers loaded on 03/18/2013 and we have set welcome letters to 17085 of those borrowers.  I have confirmd that 15 of the 17100 borrowers did not have valid addresses.  Placing back in Wendy''s court for resolution.

Jesse Gutierrez - 03/22/2013 01:16 PM - Discussion

I have ran the script against these borrowers.  Dean, please print the welcome letters for these borrowers.

Moving to Jarom to re-run the query to confirm we caught all borrowers. 

Jesse Gutierrez - 03/22/2013 12:20 PM - Discussion

Query ouputting a total of 470 letters.  Borrowers listed were not previously sent a welcome letter.  Borrowers listed are correctly appearing the the designated file.  Script correctly processes files without issue. 

Jesse Gutierrez - 03/22/2013 11:50 AM - Discussion

Reviewing the files

Jarom Ryan - 03/22/2013 11:33 AM - Discussion

really putting in Wendy''s court

Jarom Ryan - 03/22/2013 11:33 AM - Discussion

Court changed from Eric Barnes to Wendy Hack

Jarom Ryan - 03/22/2013 11:32 AM - Discussion

New files with the missing borrowers are in Y:\Development\SAS Test Files\Jarom\Welcome Letters\  placing back in Wendy''s court.

Eric Barnes - 03/21/2013 05:16 PM - Discussion

Jarom and I spent most of the day working on this request.  We believe there were 485  borrowers who were not sent a welcome letter, and we believe we have identified who those borrowers are.  So far we believe we have identified why 464 of the borrowers were excluded, and are continuing to try to identify what caused the other 21 borrowers to be excluded.

Wendy Hack - 03/21/2013 08:29 AM - Discussion

Also ARC WELCX

Wendy Hack - 03/21/2013 08:26 AM - Discussion

Moving to Eric to assign a programer.

Wendy Hack - 03/21/2013 08:26 AM - Discussion

Issue:
Please run a query to determine why some accounts were not selected to get a welcome letter. 

The Loan add date= 03/18/2013 
Does not have ARC =WELCC, WELCF, WELCA, WELCD,WELCO', N'Submitting', N'', N'Executive Request', 9, CAST(0x0000A24500B75179 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (365, N'OTH Fed', N'Time Tracking Test FED Ticket #1', CAST(0x0000A2F3010EB8A7 AS DateTime), 30, N'', CAST(0x0000A2F300000000 AS DateTime), N'Test FED Ticket #1', N'', N'', N'', N'Discussion', CAST(0x0000A2F300000000 AS DateTime), CAST(0x0000A2F300000000 AS DateTime), N'', N'Melanie Garfield - 03/19/2014 04:26 PM - Discussion

Test FED Ticket #1

Issue:
Test FED Ticket #1

', N'Submitting', N'Other Medium Urgency', N'Get in Compliance', 7, CAST(0x0000A2F3010EE1EA AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (366, N'ACCM Fed', N'TIME TRACKING TEST #1 - CS', CAST(0x0000A30100E9DABD AS DateTime), 30, N'', CAST(0x0000A30100000000 AS DateTime), N'TIME TRACKING TEST #1 - CS', N'', N'', N'', N'Submitting', CAST(0x0000A30100E9DABD AS DateTime), CAST(0x0000A30100E9DABD AS DateTime), N'TIME TRACKING TEST #1 - CS', N'', N'', N'', N'', 0, CAST(0x0000A30100EA07AA AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (367, N'ACTD Fed', N'Time Tracking Test #2 - CS', CAST(0x0000A30100EA4B39 AS DateTime), 30, N'', CAST(0x0000A30100000000 AS DateTime), N'Time Tracking Test #2 - CS', N'', N'', N'', N'Submitting', CAST(0x0000A30100EA4B39 AS DateTime), CAST(0x0000A30100EA4B39 AS DateTime), N'', N'', N'', N'', N'', 0, CAST(0x0000A30100EA6801 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (368, N'ARCA Fed', N'TIME TRACKING TEST #3 - CS', CAST(0x0000A30100EA8A60 AS DateTime), 30, N'', CAST(0x0000A30100000000 AS DateTime), N'Time Tracking Test #3 - CS', N'', N'', N'', N'Submitting', CAST(0x0000A30100EA8A60 AS DateTime), CAST(0x0000A30100EA8A60 AS DateTime), N'Time Tracking Test #3 - CS', N'', N'', N'', N'', 0, CAST(0x0000A30100EABBE9 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (369, N'OTH Fed', N'TIME TRACKING TEST #4 - CS', CAST(0x0000A30100EACCCB AS DateTime), 3, N'', CAST(0x0000A30100000000 AS DateTime), N'TIME TRACKING TEST #4 - CS', N'', N'', N'', N'Discussion', CAST(0x0000A30100000000 AS DateTime), CAST(0x0000A30100000000 AS DateTime), N'', N'Melanie Garfield - 04/02/2014 02:16 PM - Discussion

TIME TRACKING TEST #4 - CS

Issue:
TIME TRACKING TEST #4 - CS

', N'Submitting', N'Other Low Urgency', N'Other Efficiency', 1, CAST(0x0000A30100EAFD76 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (370, N'ARCC Fed', N'TIME TRACKING TEST #4 -CS', CAST(0x0000A30100EC0CFF AS DateTime), 30, N'', CAST(0x0000A30100000000 AS DateTime), N'TIME TRACKING TEST #4 -CS', N'', N'', N'', N'Submitting', CAST(0x0000A30100EC0CFF AS DateTime), CAST(0x0000A30100EC0CFF AS DateTime), N'', N'', N'', N'', N'', 0, CAST(0x0000A30100EC225D AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (371, N'ARCM Fed', N'TIME TRACKING TEST #5', CAST(0x0000A30100EC2BCA AS DateTime), 30, N'', CAST(0x0000A30100000000 AS DateTime), N'TIME TRACKING TEST #5', N'', N'', N'', N'Submitting', CAST(0x0000A30100EC2BCA AS DateTime), CAST(0x0000A30100EC2BCA AS DateTime), N'', N'', N'', N'', N'', 0, CAST(0x0000A30100EC3983 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (372, N'BBO Fed', N'TIME TRACKING TEST #6', CAST(0x0000A30100EC49C0 AS DateTime), 30, N'', CAST(0x0000A30100000000 AS DateTime), N'TIME TRACKING TEST #6', N'', N'', N'', N'Submitting', CAST(0x0000A30100EC49C0 AS DateTime), CAST(0x0000A30100EC49C0 AS DateTime), N'', N'', N'', N'', N'', 0, CAST(0x0000A30100EC5E97 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (373, N'DCR Fed', N'TIME TRACKING TEST #7 -CS', CAST(0x0000A30100EC6AED AS DateTime), 30, N'', CAST(0x0000A30100000000 AS DateTime), N'TIME TRACKING TEST #7', N'', N'', N'', N'Submitting', CAST(0x0000A30100EC6AED AS DateTime), CAST(0x0000A30100EC6AED AS DateTime), N'', N'', N'', N'', N'', 0, CAST(0x0000A30100EC7BB1 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (374, N'FAR Fed', N'TIME TRACKING TEST #8 - CS', CAST(0x0000A30601073B11 AS DateTime), 30, N'', CAST(0x0000A30600000000 AS DateTime), N'TIME TRACKING TEST #8 - CS', N'', N'', N'', N'Submitting', CAST(0x0000A30601073B11 AS DateTime), CAST(0x0000A30601073B11 AS DateTime), N'TIME TRACKING TEST #8 - CS', N'', N'', N'', N'', 0, CAST(0x0000A30601074EA3 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (375, N'IDEM Fed', N'TIME TRACKING TEST #9 - CS', CAST(0x0000A306010762DA AS DateTime), 30, N'', CAST(0x0000A30600000000 AS DateTime), N'TIME TRACKING TEST #9 - CS', N'', N'', N'', N'Submitting', CAST(0x0000A306010762DA AS DateTime), CAST(0x0000A306010762DA AS DateTime), N'', N'', N'', N'', N'', 0, CAST(0x0000A30601077105 AS DateTime), N'', N'', N'', N'', N'')
INSERT [dbo].[DAT_Ticket] ([Ticket], [TicketCode], [Subject], [Requested], [Unit], [Area], [Required], [Issue], [ResolutionCause], [ResolutionFix], [ResolutionPrevention], [Status], [StatusDate], [CourtDate], [IssueUpdate], [History], [PreviousStatus], [UrgencyOption], [CatOption], [Priority], [LastUpdated], [CCCIssue], [RequestProjectNum], [Comments], [RelatedQCIssues], [RelatedProcedures]) VALUES (376, N'IDEM Fed', N'TIME TRACKING TEST #10 - CS', CAST(0x0000A30601077F97 AS DateTime), 30, N'', CAST(0x0000A30600000000 AS DateTime), N'TEST', N'', N'', N'', N'Submitting', CAST(0x0000A30601077F97 AS DateTime), CAST(0x0000A30601077F97 AS DateTime), N'TEST', N'', N'', N'', N'', 0, CAST(0x0000A3060107918D AS DateTime), N'', N'', N'', N'', N'')
SET IDENTITY_INSERT [dbo].[DAT_Ticket] OFF
