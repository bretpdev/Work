/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

USE [AlignImport]
GO
/****** Object:  Table [dbo].[Deferment_Forbearance_Mapping]    Script Date: 5/12/2014 9:11:02 AM ******/
DROP TABLE [dbo].[Deferment_Forbearance_Mapping]
GO
/****** Object:  Table [dbo].[Deferment_Forbearance_Mapping]    Script Date: 5/12/2014 9:11:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Deferment_Forbearance_Mapping](
	[deferment_forbearance_mapping_id] [int] IDENTITY(1,1) NOT NULL,
	[nelnet_code] [char](3) NULL,
	[compass_code] [char](2) NULL,
	[description] [varchar](100) NULL,
	[cap_indicator] [char](1) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING ON
GO
SET IDENTITY_INSERT [dbo].[Deferment_Forbearance_Mapping] ON 

GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (1, N'FA0', N'09', N'Admin', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (2, N'FA4', N'09', N'ADMIN  PRE-SVC FORBEARANCE', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (3, N'FA6', N'09', N'HI/LOW', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (4, N'FAF', N'09', N'ADMIN  60-DAY ADMIN FORB', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (5, N'FAD', N'09', N'DELINQ PRIOR TO DISASTERR FORB', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (6, N'FA0', N'09', N'ADMIN', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (7, N'FBR', N'09', N'ADMIN DELNQ PRIOR REPAY OPTION', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (8, N'FBS', N'09', N'ADMIN DELNQ PRIOR REPAY OPTION', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (9, N'FCL', N'02', N'ADMIN COVER DELIQ AT RECERT', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (10, N'FDA', N'40', N'ADMIN  DISASTER AREA', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (11, N'FDD', N'09', N'ADMIN  DELINQUENT PRIOR TO DEF', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (12, N'FD0', N'20', N'NO PAY NATIONAL SERVICE ', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (13, N'FDP', N'09', N'ADMIN DELQ POST DEFR FORB', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (14, N'FDR', N'11', N'DELINQUENCY PRIOR TO REFINANCE', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (15, N'FFB', N'10', N'BNKRUP BANKRUPTCY', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (16, N'FFD', N'13', N'ADMIN  DEATH', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (17, N'FFH', N'05', N'NO PAY HARDSHIP', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (18, N'FFI', N'05', N'NO PAY HARDSHIP IVR', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (19, N'FFO', N'05', N'VERBAL NO PAY HARDSHIP OUTBOUND', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (20, N'FFV', N'05', N'VERBAL NO PAY HARDSHIP INBOUND', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (21, N'FFW', N'05', N'NO PAY HARDSHIP WEB', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (22, N'FGF', N'06', N'ADMIN  GRACE', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (23, N'FH1', N'31', N'REDUCED PYMT FORBEARANCE', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (24, N'FID', N'09', N'ADMIN  INELIGIBLE DEFERMENT', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (25, N'FIR', N'07', N'ADMIN  MEDICAL INTERNSHIP/RESIDENT', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (26, N'FLC', N'02', N'ADMIN COVER DELIQ AT RECERTIFICATION', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (27, N'FLF', N'21', N'ADMIN  LOAN FORGIVENESS ', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (28, N'FLT', N'09', N'ADMIN  LOAN SALE/TRANSFER', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (29, N'FMH', N'05', N'NO PAY MANDATORY HARDSHIP', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (30, N'FMM', N'01', N'ADMIN  MILITARY MOBILIZATION', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (31, N'FNS', N'20', N'ADMIN  NATIONAL SERVICE', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (32, N'FO1', N'31', N'LOW PAY ', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (33, N'FPC', N'15', N'ADMIN POST CONSOL', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (34, N'FRA', N'09', N'HI/LOW REPAYMENT ACCOMMODATION', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (35, N'FRM', N'10', N'RISK MGMT USE ONLY', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (36, N'FRO', N'09', N'FOR RETRO PROCESSING ONLY', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (37, N'FSD', N'09', N'ADMIN  60-DAY ADMIN FORB', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (38, N'FTF', N'09', N'ADMIN 60-DAY ADMIN FORB ', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (39, N'FV1', N'31', N' LOW PAY  ', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (40, N'FW1', N'31', N'LOW PAY WEB', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (41, N'D10', N'15', N'FT STUDENT', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (42, N'D11', N'16', N'SUMMER', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (43, N'D12', N'18', N'SCHOOL (DEPENDENT ENROLLED)', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (44, N'D13', N'18', N'School 1/2 time ', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (45, N'D14', N'06', N'GRAD FELLOWSHIP DEFERMENT', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (46, N'D16', N'04', N'Military / Public Health ', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (47, N'D19', N'05', N'VISTA (ACTION) ', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (48, N'D20', N'19', N'PLUS IN SCHOOL PERIOD', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (49, N'D22', N'10', N'TAX EXEMPT ORGANIZATION   ', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (50, N'D24', N'13', N'UNEMPLOYMENT ', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (51, N'D25', N'13', N'UNEMPLOYMENT ', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (52, N'D26', N'02', N'TEMPORARY DISABILITY ', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (53, N'D28', N'03', N'REHABILITATION TRAINING', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (54, N'D30', N'08', N'INTERNSHIP/RESIDENCY', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (55, N'D32', N'12', N'PARENTAL LEAVE', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (56, N'D34', N'11', N'WORKING MOTHER ', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (57, N'D36', N'14', N'TEACHER SHORTAGE ', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (58, N'D38', N'29', N'ECONOMIC HARDSHIP', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (59, N'D40', N'29', N'PSR DEFR UNKNOWN PCON ', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (60, N'D44', N'38', N'MILITARY DEFERMENT ', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (61, N'D46', N'38', N'MILITARY EXTEN (NON SCHOOL)', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (62, N'D50', N'38', N'MILITARY DEFERMENT ', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (63, N'D55', N'38', N'MILITARY DEFERMENT', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (64, N'D56', N'38', N'MILITARY DEFERMENT EXTENSION', NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (65, N'FIS', N'24', NULL, NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (66, N'FA1', N'05', NULL, NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (67, N'FA3', N'10', NULL, NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (68, N'FHI', N'31', NULL, NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (69, N'FEH', N'22', NULL, NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (70, N'FV0', N'05', NULL, NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (71, N'FWY', N'04', NULL, NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (72, N'FFC', N'09', NULL, NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (73, N'FLN', N'02', NULL, NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (74, N'D48', N'38', NULL, NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (75, N'D47', N'38', NULL, NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (76, N'D45', N'40', NULL, NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (77, N'FDS', N'14', NULL, NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (78, N'FA5', N'21', NULL, NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (79, N'FTD', N'14', NULL, NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (80, N'FVM', N'01', NULL, NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (81, N'FA2', N'17', NULL, NULL)
GO
INSERT [dbo].[Deferment_Forbearance_Mapping] ([deferment_forbearance_mapping_id], [nelnet_code], [compass_code], [description], [cap_indicator]) VALUES (82, N'I50', N'01', NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Deferment_Forbearance_Mapping] OFF
GO

