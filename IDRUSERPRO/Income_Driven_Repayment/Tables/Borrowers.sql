CREATE TABLE [dbo].[Borrowers] (
    [borrower_id]    INT       IDENTITY (1, 1) NOT NULL,
    [SSN]            CHAR (9)  NULL,
    [account_number] CHAR (10) NULL,
    [first_name]     CHAR (35) NULL,
    [last_name]      CHAR (35) NULL,
    [middle_name]    CHAR (35) NULL,
    [created_at]     DATETIME  DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_Borrowers] PRIMARY KEY CLUSTERED ([borrower_id] ASC)
);

