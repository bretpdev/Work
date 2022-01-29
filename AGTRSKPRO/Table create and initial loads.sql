
USE CentralData
GO
CREATE SCHEMA agtrskpro AUTHORIZATION dbo;
GO


CREATE TABLE [agtrskpro].[TaskRegions](
	[TaskRegionId] [int] IDENTITY(1,1) NOT NULL,
	[TaskRegion] [varchar](50) NOT NULL,
	[AddedAt] [datetime] NOT NULL,
	[AddedBy] [varchar](100) NOT NULL,
	[DeletedAt] [datetime] NULL,
	[DeletedBy] [varchar](100) NULL,
 CONSTRAINT [PK_TaskRegions] PRIMARY KEY CLUSTERED 
(
	[TaskRegionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [agtrskpro].[TaskRegions] ADD  CONSTRAINT [DF_TaskRegions_AddedAt]  DEFAULT (getdate()) FOR [AddedAt]
GO

ALTER TABLE [agtrskpro].[TaskRegions] ADD  CONSTRAINT [DF_TaskRegions_AddedBy]  DEFAULT (suser_sname()) FOR [AddedBy]
GO

CREATE TABLE [agtrskpro].[BusinessUnitTaskMapping](
	[BusinessUnitTaskMappingId] [int] IDENTITY(1,1) NOT NULL,
	[TaskRegionId] [int] NOT NULL,
	[BusinessUnitName] [varchar](100) NOT NULL,
	[Queue] [varchar](15) NOT NULL,
	[Subqueue] [varchar](2) NULL,
	[AddedAt] [datetime] NOT NULL,
	[AddedBy] [varchar](100) NOT NULL,
	[DeletedAt] [datetime] NULL,
	[DeletedBy] [varchar](100) NULL,
 CONSTRAINT [PK_BusinessUnitTaskMapping] PRIMARY KEY CLUSTERED 
(
	[BusinessUnitTaskMappingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [agtrskpro].[BusinessUnitTaskMapping] ADD  CONSTRAINT [DF_BusinessUnitTaskMapping_AddedAt]  DEFAULT (getdate()) FOR [AddedAt]
GO

ALTER TABLE [agtrskpro].[BusinessUnitTaskMapping] ADD  CONSTRAINT [DF_BusinessUnitTaskMapping_AddedBy]  DEFAULT (suser_sname()) FOR [AddedBy]
GO

ALTER TABLE [agtrskpro].[BusinessUnitTaskMapping]  WITH CHECK ADD  CONSTRAINT [FK_BusinessUnitTaskMapping_TaskRegions] FOREIGN KEY([TaskRegionId])
REFERENCES [agtrskpro].[TaskRegions] ([TaskRegionId])
GO

ALTER TABLE [agtrskpro].[BusinessUnitTaskMapping] CHECK CONSTRAINT [FK_BusinessUnitTaskMapping_TaskRegions]
GO

INSERT INTO agtrskpro.TaskRegions(TaskRegion, AddedAt, AddedBy)
VALUES
('Compass', GETDATE(), SUSER_SNAME()),
('Onelink', GETDATE(), SUSER_SNAME())

DECLARE @TaskRegionId INT = (SELECT TaskRegionId from agtrskpro.TaskRegions WHERE TaskRegion = 'Compass');

INSERT INTO agtrskpro.BusinessUnitTaskMapping(BusinessUnitName, TaskRegionId, Queue, Subqueue, AddedAt, AddedBy)
VALUES
('Doc Services – UHEAA', @TaskRegionId, 'DV', '01', GETDATE(), SUSER_SNAME()),
('Doc Services – UHEAA', @TaskRegionId, 'NO', '01', GETDATE(), SUSER_SNAME()),
('Doc Services – UHEAA', @TaskRegionId, 'LX', '01', GETDATE(), SUSER_SNAME()),
('Doc Services – UHEAA', @TaskRegionId, '1E', '01', GETDATE(), SUSER_SNAME()),
('Doc Services – UHEAA', @TaskRegionId, '1E', '02', GETDATE(), SUSER_SNAME()),
('Doc Services – UHEAA', @TaskRegionId, 'MU', '01', GETDATE(), SUSER_SNAME()),
('Doc Services – FED', @TaskRegionId, 'T3', '01', GETDATE(), SUSER_SNAME()),
('Doc Services – FED', @TaskRegionId, 'S6', '01', GETDATE(), SUSER_SNAME()),
('Doc Services – FED', @TaskRegionId, 'AK', '01', GETDATE(), SUSER_SNAME()),
('Doc Services – FED', @TaskRegionId, '1E', '01', GETDATE(), SUSER_SNAME()),
('Doc Services – FED', @TaskRegionId, 'SD', '01', GETDATE(), SUSER_SNAME()),
('Doc Services – FED', @TaskRegionId, 'LX', '01', GETDATE(), SUSER_SNAME()),
('Doc Services – FED', @TaskRegionId, 'MT', '01', GETDATE(), SUSER_SNAME()),
('Doc Services – FED', @TaskRegionId, '03', '01', GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '15', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '21', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '22', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '23', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '25', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '26', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '27', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '28', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '30', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '32', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '34', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '35', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '36', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '37', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '40', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '42', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '44', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '51', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '55', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '87', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '90', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '94', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '95', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '99', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '0M', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '1P', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '1R', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '2A', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '2P', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '3B', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '3D', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '3F', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '3K', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '3L', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '3S', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '3W', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '3X', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '3Z', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '4B', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '4D', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '4F', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '4H', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '4K', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '4M', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '4P', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '4R', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '4X', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, '5C', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'AD', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'AQ', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'AT', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'AW', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'BC', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'BK', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'CR', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'D7', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'D8', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'DC', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'DR', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'DU', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'E6', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'E8', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'ED', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'ER', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'F0', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'F2', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'G8', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'GF', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'GG', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'GP', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'H0', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'H1', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'HF', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'HV', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'HZ', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'I0', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'LG', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'LH', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'LN', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'MN', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'MS', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'N1', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'NB', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'NC', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'NE', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'NF', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'O9', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'OB', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'OF', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'OH', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'OK', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'ON', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'OU', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'OV', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'OW', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'OZ', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'P0', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'P1', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'P7', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'PM', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'PR', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'R0', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'R1', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'RB', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'RZ', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'S4', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'SA', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'SB', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'SF', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'SI', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'SV', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'T0', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'T1', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'T2', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'T9', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'TB', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'TW', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'TX', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'U0', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'U1', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'U4', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'US', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'V4', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'V5', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'V8', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'VB', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'VC', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'VR', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'X5', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'X9', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'XB', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, '1P', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'P4', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'S4', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'MF', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'ML', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'PR', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'VB', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'VR', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'SF', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, '40', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'SB', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, '88', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'I0', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'AT', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'AP', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'PM', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'AH', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'AW', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'D7', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'D8', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'HH', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, '11', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, '12', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, '13', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, '14', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, '1R', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, '51', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'LN', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, '2A', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, '2P', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'M3', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'HN', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'BO', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'MO', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'R0', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'DU', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, '9R', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'GP', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'SB', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, '7I', null, GETDATE(), SUSER_SNAME()),
('Repayment Services', @TaskRegionId, 'MN', null, GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, 'DR', null, GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, 'E0', null, GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, 'SD', null, GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, 'RB', '06', GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, 'RB', '07', GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, 'RB', '19', GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, 'RB', '20', GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, 'PD', null, GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, 'SE', null, GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, '23', null, GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, 'OF', null, GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, '6A', null, GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, '6R', null, GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, 'RZ', null, GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, 'SO', null, GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, '87', null, GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, '95', null, GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, '96', null, GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, '98', null, GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, 'BY', null, GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, '94', null, GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, 'AD', null, GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, 'AQ', null, GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, '55', '03', GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, 'B4', null, GETDATE(), SUSER_SNAME()),
('Account Services', @TaskRegionId, 'P8', null, GETDATE(), SUSER_SNAME()),
('Financial Services', @TaskRegionId, '15', null, GETDATE(), SUSER_SNAME()),
('Financial Services', @TaskRegionId, 'T9', null, GETDATE(), SUSER_SNAME()),
('Financial Services', @TaskRegionId, 'R3', null, GETDATE(), SUSER_SNAME()),
('Financial Services', @TaskRegionId, 'PA', null, GETDATE(), SUSER_SNAME()),
('Financial Services', @TaskRegionId, '01', null, GETDATE(), SUSER_SNAME()),
('Financial Services', @TaskRegionId, 'AV', null, GETDATE(), SUSER_SNAME()),
('Financial Services', @TaskRegionId, '7C', null, GETDATE(), SUSER_SNAME())

SET @TaskRegionId = (SELECT TaskRegionId from agtrskpro.TaskRegions WHERE TaskRegion = 'Onelink');


INSERT INTO agtrskpro.BusinessUnitTaskMapping(BusinessUnitName, TaskRegionId, Queue, Subqueue, AddedAt, AddedBy)
VALUES
('Doc Services - UHEAA', @TaskRegionId, 'SERVCRVW', null, GETDATE(), SUSER_SNAME()),
('Doc Services - UHEAA', @TaskRegionId, 'ACURINTR', null, GETDATE(), SUSER_SNAME()),
('Doc Services - UHEAA', @TaskRegionId, 'XDEMOE', null, GETDATE(), SUSER_SNAME()),
('Doc Services - UHEAA', @TaskRegionId, 'XDEMOG', null, GETDATE(), SUSER_SNAME()),
('Doc Services - UHEAA', @TaskRegionId, 'POAEXPIR', null, GETDATE(), SUSER_SNAME()),
('Doc Services - UHEAA', @TaskRegionId, 'SKPSTADD', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'ASBKP', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'ASCON', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'ASMOC', null, GETDATE(), SUSER_SNAME()),
('Loan Services', @TaskRegionId, 'BKCP7RVW', null, GETDATE(), SUSER_SNAME())
