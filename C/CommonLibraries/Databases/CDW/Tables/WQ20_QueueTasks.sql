CREATE TABLE [dbo].[WQ20_QueueTasks]
( 
    [WF_QUE] CHAR(2) NOT NULL, 
    [WF_SUB_QUE] CHAR(2) NOT NULL, 
    [WN_CTL_TSK] VARCHAR(18) NOT NULL, 
    [PF_REQ_ACT] VARCHAR(5) NOT NULL, 
	[DF_SPE_ACC_ID] VARCHAR(10) NOT NULL ,
    [WD_ACT_REQ] DATETIME NULL, 
    [WD_ACT_RQR] DATETIME NULL, 
    [WC_CND_CTC] CHAR NULL DEFAULT (' '), 
    [WD_INI_TSK] DATETIME NULL, 
    [WT_INI_TSK] TIME NULL, 
    [WF_USR_ASN_TSK] VARCHAR(8) NULL DEFAULT (' '), 
    [WF_USR_ASN_BY TSK] VARCHAR(8) NULL DEFAULT (' '), 
    [WX_MSG_1_TSK] VARCHAR(77) NULL DEFAULT (' '), 
    [WX_MSG_2_TSK] VARCHAR(77) NULL DEFAULT (' '), 
    [WC_STA_WQUE20] CHAR NULL DEFAULT (' '), 
    [WF_LST_DTS_WQ20] DATETIME NULL, 
    [LN_ATY_SEQ] INT NULL, 
    [WF_CRT_DTS_WQ20] DATETIME NULL, 
    PRIMARY KEY ([WF_QUE], [WF_SUB_QUE], [WN_CTL_TSK], [PF_REQ_ACT])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Queue',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WQ20_QueueTasks',
    @level2type = N'COLUMN',
    @level2name = N'WF_QUE'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Sub Queue',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WQ20_QueueTasks',
    @level2type = N'COLUMN',
    @level2name = N'WF_SUB_QUE'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Task Control Number',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WQ20_QueueTasks',
    @level2type = N'COLUMN',
    @level2name = N'WN_CTL_TSK'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Action Request Code',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WQ20_QueueTasks',
    @level2type = N'COLUMN',
    @level2name = N'PF_REQ_ACT'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Account Number ',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WQ20_QueueTasks',
    @level2type = N'COLUMN',
    @level2name = N'DF_SPE_ACC_ID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Action Request Date',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WQ20_QueueTasks',
    @level2type = N'COLUMN',
    @level2name = N'WD_ACT_REQ'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Date Action Required',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WQ20_QueueTasks',
    @level2type = N'COLUMN',
    @level2name = N'WD_ACT_RQR'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Critical Condition Code',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WQ20_QueueTasks',
    @level2type = N'COLUMN',
    @level2name = N'WC_CND_CTC'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Task Initiation Date',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WQ20_QueueTasks',
    @level2type = N'COLUMN',
    @level2name = N'WD_INI_TSK'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Task Initiation Time',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WQ20_QueueTasks',
    @level2type = N'COLUMN',
    @level2name = N'WT_INI_TSK'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'User Initiated Task',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WQ20_QueueTasks',
    @level2type = N'COLUMN',
    @level2name = N'WF_USR_ASN_TSK'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'User Assigned Task',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WQ20_QueueTasks',
    @level2type = N'COLUMN',
    @level2name = N'WF_USR_ASN_BY TSK'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Task Message 1',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WQ20_QueueTasks',
    @level2type = N'COLUMN',
    @level2name = N'WX_MSG_1_TSK'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Task Message 2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WQ20_QueueTasks',
    @level2type = N'COLUMN',
    @level2name = N'WX_MSG_2_TSK'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Status Code',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WQ20_QueueTasks',
    @level2type = N'COLUMN',
    @level2name = N'WC_STA_WQUE20'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Date Last Updated',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WQ20_QueueTasks',
    @level2type = N'COLUMN',
    @level2name = N'WF_LST_DTS_WQ20'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Numeric ID of Activity',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WQ20_QueueTasks',
    @level2type = N'COLUMN',
    @level2name = N'LN_ATY_SEQ'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Row Create Date',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WQ20_QueueTasks',
    @level2type = N'COLUMN',
    @level2name = N'WF_CRT_DTS_WQ20'