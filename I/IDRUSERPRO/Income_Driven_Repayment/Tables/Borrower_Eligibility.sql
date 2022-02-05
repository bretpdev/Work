CREATE TABLE [dbo].[Borrower_Eligibility] (
    [eligibility_id]          INT  NOT NULL,
    [eligibility_code]        CHAR (2)      NOT NULL,
    [eligibility_description] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Borrower_Eligibility] PRIMARY KEY CLUSTERED ([eligibility_id] ASC)
);

