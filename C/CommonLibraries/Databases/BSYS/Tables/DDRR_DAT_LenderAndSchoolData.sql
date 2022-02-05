CREATE TABLE [dbo].[DDRR_DAT_LenderAndSchoolData] (
    [LenderID]           VARCHAR (50) NOT NULL,
    [SchoolID]           VARCHAR (50) NOT NULL,
    [ProcessDate]        DATETIME     NOT NULL,
    [LoanProgram]        VARCHAR (50) NOT NULL,
    [DisbursementMethod] VARCHAR (50) NOT NULL,
    [BorrowerCount]      INT          NOT NULL,
    [LoanCount]          INT          NOT NULL,
    [GrossDisbursement]  MONEY        NOT NULL,
    [NetDisbursement]    MONEY        NOT NULL,
    CONSTRAINT [PK_DDRR_DAT_LenderAndSchoolData] PRIMARY KEY CLUSTERED ([LenderID] ASC, [SchoolID] ASC, [ProcessDate] ASC, [LoanProgram] ASC, [DisbursementMethod] ASC)
);

