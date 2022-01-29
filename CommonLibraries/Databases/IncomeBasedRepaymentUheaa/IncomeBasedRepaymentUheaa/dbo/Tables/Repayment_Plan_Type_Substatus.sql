CREATE TABLE [dbo].[Repayment_Plan_Type_Substatus] (
    [repayment_plan_type_substatus_id]          INT           IDENTITY (1, 1) NOT NULL,
    [repayment_plan_type_status_id]             INT           NULL,
    [repayment_type_status_id]                  INT           NOT NULL,
    [repayment_plan_reason_id]                  INT           NULL,
    [repayment_plan_type_substatus]             VARCHAR (255) NOT NULL,
    [repayment_plan_type_substatus_description] VARCHAR (500) NOT NULL,
    [created_at]                                DATETIME      CONSTRAINT [DF__Repayment__creat__300424B4] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Repayment_Plan_Type_Substatus] PRIMARY KEY CLUSTERED ([repayment_plan_type_substatus_id] ASC),
    CONSTRAINT [FK_Repayment_Plan_Type_Substatus_Repayment_Plan_Reason] FOREIGN KEY ([repayment_plan_reason_id]) REFERENCES [dbo].[Repayment_Plan_Reason] ([repayment_plan_reason_id]),
    CONSTRAINT [FK_Repayment_Plan_Type_Substatus_Repayment_Plan_Type_Status] FOREIGN KEY ([repayment_plan_type_status_id]) REFERENCES [dbo].[Repayment_Plan_Type_Status] ([repayment_plan_type_status_id]),
    CONSTRAINT [FK_Repayment_Plan_Type_Substatus_Repayment_Type_Status] FOREIGN KEY ([repayment_type_status_id]) REFERENCES [dbo].[Repayment_Type_Status] ([repayment_type_status_id])
);

