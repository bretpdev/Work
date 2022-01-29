CREATE TABLE [dbo].[BORG_LST_AutopayGracePeriods] (
    [LoanProgram] VARCHAR (50) NOT NULL,
    [GraceLength] INT          NOT NULL,
    CONSTRAINT [PK_BORG_LST_AutopayGracePeriods] PRIMARY KEY CLUSTERED ([LoanProgram] ASC)
);

