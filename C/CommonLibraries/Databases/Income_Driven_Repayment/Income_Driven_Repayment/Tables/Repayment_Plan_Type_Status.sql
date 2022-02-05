CREATE TABLE [dbo].[Repayment_Plan_Type_Status] (
    [repayment_plan_type_status_id] INT           IDENTITY (1, 1) NOT NULL,
    [repayment_plan_type_status]    VARCHAR (255) NOT NULL,
    [created_at]                    DATETIME      CONSTRAINT [DF__Repayment__creat__276EDEB3] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Repayment_Plan_Type_Status] PRIMARY KEY CLUSTERED ([repayment_plan_type_status_id] ASC)
);

