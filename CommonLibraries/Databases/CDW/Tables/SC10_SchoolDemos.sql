CREATE TABLE [dbo].[SC10_SchoolDemos]
(
	[DF_SPE_ACC_ID] CHAR(10) NOT NULL,
	[IF_DOE_SCL] VARCHAR(8) NOT NULL , 
    [IM_SCL_SHO] VARCHAR(20) NULL, 
    [IM_SCL_FUL] VARCHAR(40) NULL, 
    [IC_TYP_SCL] VARCHAR(2) NULL, 
    [SchoolTypeDescrption] VARCHAR(300) NULL, 
    [IC_PRV_SCL_STA] CHAR NULL, 
    [IC_CUR_SCL_STA] CHAR NULL, 
    [II_SCL_CHS_PTC] CHAR NULL, 
    [IC_LEN_LNG_PGM_STY] CHAR(2) NULL, 
    PRIMARY KEY ([DF_SPE_ACC_ID], [IF_DOE_SCL])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'School Code',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SC10_SchoolDemos',
    @level2type = N'COLUMN',
    @level2name = N'IF_DOE_SCL'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Short Name',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SC10_SchoolDemos',
    @level2type = N'COLUMN',
    @level2name = N'IM_SCL_SHO'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Full Name',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SC10_SchoolDemos',
    @level2type = N'COLUMN',
    @level2name = N'IM_SCL_FUL'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'School Type',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SC10_SchoolDemos',
    @level2type = N'COLUMN',
    @level2name = N'IC_TYP_SCL'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'School Type Description',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SC10_SchoolDemos',
    @level2type = N'COLUMN',
    @level2name = N'SchoolTypeDescrption'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Previous Status',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SC10_SchoolDemos',
    @level2type = N'COLUMN',
    @level2name = N'IC_PRV_SCL_STA'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Current Status',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SC10_SchoolDemos',
    @level2type = N'COLUMN',
    @level2name = N'IC_CUR_SCL_STA'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Clearing House',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SC10_SchoolDemos',
    @level2type = N'COLUMN',
    @level2name = N'II_SCL_CHS_PTC'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Length Long Program',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SC10_SchoolDemos',
    @level2type = N'COLUMN',
    @level2name = N'IC_LEN_LNG_PGM_STY'