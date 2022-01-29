CREATE TABLE [dbo].[Repayment_Plan_Reason] (
    [repayment_plan_reason_id]          INT          IDENTITY (1, 1) NOT NULL,
    [repayment_plan_reason_code]        CHAR (1)     NOT NULL,
    [repayment_plan_reason_description] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Repayment_Plan_Reason] PRIMARY KEY CLUSTERED ([repayment_plan_reason_id] ASC)
);

