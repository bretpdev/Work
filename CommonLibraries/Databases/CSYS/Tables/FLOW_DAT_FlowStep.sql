CREATE TABLE [dbo].[FLOW_DAT_FlowStep] (
    [FlowID]                         VARCHAR (50)   NOT NULL,
    [FlowStepSequenceNumber]         INT            NOT NULL,
    [AccessAlsoBasedOffBusinessUnit] BIT            CONSTRAINT [DF_FLOW_DAT_FlowSteps_AccessAlsoBasedOffBusinessUnit] DEFAULT ((0)) NOT NULL,
    [AccessKey]                      VARCHAR (100)  NULL,
    [NotificationType]               VARCHAR (100)  NULL,
    [StaffAssignment]                INT            NULL,
    [StaffAssignmentCalculationID]   VARCHAR (100)  NULL,
    [ControlDisplayText]             VARCHAR (200)  CONSTRAINT [DF_FLOW_DAT_FlowSteps_ControlDisplayText] DEFAULT ('') NOT NULL,
    [Description]                    VARCHAR (8000) CONSTRAINT [DF_FLOW_DAT_FlowSteps_Description] DEFAULT ('') NOT NULL,
    [DataValidationID]               VARCHAR (50)   NULL,
    [Status]                         VARCHAR (50)   NOT NULL,
    CONSTRAINT [PK_FLOW_DAT_FlowSteps] PRIMARY KEY CLUSTERED ([FlowID] ASC, [FlowStepSequenceNumber] ASC),
    CONSTRAINT [FK_FLOW_DAT_FlowStep_FLOW_LST_DataValidator] FOREIGN KEY ([DataValidationID]) REFERENCES [dbo].[FLOW_LST_DataValidator] ([Name])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Flow ID for a defined flow.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FLOW_DAT_FlowStep', @level2type = N'COLUMN', @level2name = N'FlowID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Step sequence number.  This allow the system to know what order the steps should occur in during the process of the flow.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FLOW_DAT_FlowStep', @level2type = N'COLUMN', @level2name = N'FlowStepSequenceNumber';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Access key needed for a staff member to use the control.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FLOW_DAT_FlowStep', @level2type = N'COLUMN', @level2name = N'AccessKey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The text that will be displayed on the control that enables the next step in the flow to occur.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FLOW_DAT_FlowStep', @level2type = N'COLUMN', @level2name = N'ControlDisplayText';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The steps description.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FLOW_DAT_FlowStep', @level2type = N'COLUMN', @level2name = N'Description';

