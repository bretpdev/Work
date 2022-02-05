CREATE TABLE [dbo].[Loans] (
    [loan_id]        INT         IDENTITY (1, 1) NOT NULL,
    [borrower_id]    INT         NOT NULL,
    [application_id] INT         NOT NULL,
    [loan_type]      CHAR (6)    NULL,
    [award_id]       CHAR (21)   NOT NULL,
    [loan_seq]       CHAR (4)    NULL,
    [disb_date]      VARCHAR (8) NULL,
    CONSTRAINT [PK_Loans] PRIMARY KEY CLUSTERED ([loan_id] ASC),
    CONSTRAINT [FK_Loans_Applications] FOREIGN KEY ([application_id]) REFERENCES [dbo].[Applications] ([application_id]),
    CONSTRAINT [FK_Loans_Borrowers] FOREIGN KEY ([borrower_id]) REFERENCES [dbo].[Borrowers] ([borrower_id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Loans_borrower_id]
    ON [dbo].[Loans]([borrower_id] ASC)
    INCLUDE([application_id]);

