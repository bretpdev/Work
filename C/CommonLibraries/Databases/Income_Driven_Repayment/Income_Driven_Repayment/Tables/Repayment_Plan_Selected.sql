CREATE TABLE [dbo].[Repayment_Plan_Selected] (
    [repayment_plan_type_id] INT IDENTITY (1, 1) NOT NULL,
    [application_id]         INT NOT NULL,
    [repayment_type_id]      INT NOT NULL,
    CONSTRAINT [PK_Repayment_Plan_Type] PRIMARY KEY CLUSTERED ([repayment_plan_type_id] ASC),
    CONSTRAINT [FK_Repayment_Plan_Selected_Applications] FOREIGN KEY ([application_id]) REFERENCES [dbo].[Applications] ([application_id]),
    CONSTRAINT [FK_Repayment_Plan_Selected_Repayment_Type] FOREIGN KEY ([repayment_type_id]) REFERENCES [dbo].[Repayment_Type] ([repayment_type_id])
);

