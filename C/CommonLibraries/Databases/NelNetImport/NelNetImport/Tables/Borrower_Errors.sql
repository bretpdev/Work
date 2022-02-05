CREATE TABLE [dbo].[Borrower_Errors] (
    [br_ssn]      VARCHAR (50) NULL,
    [ln_seq]      VARCHAR (50) NULL,
    [error_code]  VARCHAR (50) NULL,
    [minor_batch] VARCHAR (50) NULL
);


GO
CREATE CLUSTERED INDEX [CX_BE]
    ON [dbo].[Borrower_Errors]([br_ssn] ASC, [ln_seq] ASC, [error_code] ASC);

