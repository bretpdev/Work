CREATE TABLE [dbo].[CompassLoanMapping] (
    [br_ssn]             VARCHAR (9) NULL,
    [NelNetLoanSeq]      VARCHAR (2) NULL,
    [CommpassLoanSeq]    BIGINT      NULL,
    [NelNetLoanProgram]  VARCHAR (4) NULL,
    [CompassLoanProgram] VARCHAR (6) NULL
);




GO
CREATE NONCLUSTERED INDEX [IX_CLM]
    ON [dbo].[CompassLoanMapping]([br_ssn] ASC);

