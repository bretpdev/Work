CREATE TABLE [dbo].[Repayment_Plan_Type_Requested] (
    [repayment_plan_type_requested_id]          INT,
    [repayment_plan_type_requested_description] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Repayment_Plan_Type_Requested] PRIMARY KEY CLUSTERED ([repayment_plan_type_requested_id] ASC)
);

