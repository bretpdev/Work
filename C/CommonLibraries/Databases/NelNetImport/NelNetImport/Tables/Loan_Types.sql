CREATE TABLE [dbo].[Loan_Types] (
    [loan_type_id]          INT          IDENTITY (1, 1) NOT NULL,
    [nn_loan_type]          VARCHAR (10) NOT NULL,
    [compass_loan_type]     VARCHAR (10) NULL,
    [loan_type_description] VARCHAR (50) NOT NULL
);

