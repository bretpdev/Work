CREATE TABLE [dbo].[FLOW_DAT_Flow] (
    [FlowID]                        VARCHAR (50)   NOT NULL,
    [System]                        VARCHAR (30)   NOT NULL,
    [Description]                   VARCHAR (8000) CONSTRAINT [DF_FLOW_LST_Flows_Description] DEFAULT ('') NOT NULL,
    [ControlDisplayText]            VARCHAR (200)  CONSTRAINT [DF_FLOW_LST_Flows_ControlDisplayText] DEFAULT ('') NOT NULL,
    [UserInterfaceDisplayIndicator] VARCHAR (50)   CONSTRAINT [DF_FLOW_LST_Flows_UserInterfaceDisplayIndicator] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_FLOW_LST_Flows] PRIMARY KEY CLUSTERED ([FlowID] ASC, [System] ASC),
    CONSTRAINT [uc_Flow] UNIQUE NONCLUSTERED ([FlowID] ASC, [System] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Flow ID for a specified flow.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FLOW_DAT_Flow', @level2type = N'COLUMN', @level2name = N'FlowID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Description of specified flow.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FLOW_DAT_Flow', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Text to appear on control that will be clicked on when starting flow.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FLOW_DAT_Flow', @level2type = N'COLUMN', @level2name = N'ControlDisplayText';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'An id that helps decide what interface to use for a given flow.  For example if a system has multiple forms that handle several different flows this value should be used to tie a flow to one of those forms.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FLOW_DAT_Flow', @level2type = N'COLUMN', @level2name = N'UserInterfaceDisplayIndicator';


GO
GRANT SELECT
    ON OBJECT::[dbo].[FLOW_DAT_Flow] TO [db_executor]
    AS [dbo];

