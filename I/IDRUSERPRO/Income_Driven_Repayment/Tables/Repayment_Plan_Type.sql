CREATE TABLE [dbo].[Repayment_Plan_Type] (
    [repayment_plan_type_id] INT NOT NULL,
    [repayment_plan]         VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Repayment_Plan_Type_1] PRIMARY KEY CLUSTERED ([repayment_plan_type_id] ASC)
);

