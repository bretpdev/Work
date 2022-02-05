CREATE TABLE [dbo].[Other_Loans] (
    [application_id]      INT            NOT NULL,
    [spouse_indicator]    BIT            NOT NULL,
    [loan_type]           VARCHAR (10)   NULL,
    [owner_lender]        VARCHAR (50)   NULL,
    [outstanding_balance] MONEY          NULL,
    [monthly_pay]         MONEY          NULL,
    [interest_rate]       DECIMAL (7, 3) NULL,
    [ffelp]               BIT            NULL,
    [other_loans_id]      INT            IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_Other_Loans] PRIMARY KEY CLUSTERED ([other_loans_id] ASC) WITH (FILLFACTOR = 95),
    CONSTRAINT [FK_Other_Loans_Applications] FOREIGN KEY ([application_id]) REFERENCES [dbo].[Applications] ([application_id])
);



