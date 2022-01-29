CREATE TABLE [dbo].[Repayment_Type_Status] (
    [repayment_type_status_id] INT      IDENTITY (1, 1) NOT NULL,
    [repayment_type_status]    CHAR (2) NULL,
    CONSTRAINT [PK_Repayment_Type_Status] PRIMARY KEY CLUSTERED ([repayment_type_status_id] ASC)
);

