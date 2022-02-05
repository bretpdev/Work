CREATE TABLE [dbo].[Repayment_Type] (
    [repayment_type_id]          INT           IDENTITY (1, 1) NOT NULL,
    [repayment_plan_type_id]     INT           NULL,
    [repayment_type_code]        CHAR (2)      NOT NULL,
    [repayment_type_status]      VARCHAR (50)  NOT NULL,
    [repayment_type_description] VARCHAR (150) NOT NULL,
    CONSTRAINT [PK_Repayment_Type] PRIMARY KEY CLUSTERED ([repayment_type_id] ASC),
    CONSTRAINT [FK_Repayment_Type_Repayment_Plan_Type] FOREIGN KEY ([repayment_plan_type_id]) REFERENCES [dbo].[Repayment_Plan_Type] ([repayment_plan_type_id])
);

