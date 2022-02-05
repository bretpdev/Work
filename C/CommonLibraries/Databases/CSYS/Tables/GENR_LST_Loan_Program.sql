CREATE TABLE [dbo].[GENR_LST_Loan_Program] (
    [loan_program_id] INT          IDENTITY (1, 1) NOT NULL,
    [loan_program]    VARCHAR (10) NOT NULL,
    [direct]          BIT          NOT NULL,
    [NSLDSLoanCode] VARCHAR(5) NULL, 
    CONSTRAINT [PK_GENR_LST_Loan_Program] PRIMARY KEY CLUSTERED ([loan_program_id] ASC)
);

