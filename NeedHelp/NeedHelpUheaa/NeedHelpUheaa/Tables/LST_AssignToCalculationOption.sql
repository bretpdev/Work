CREATE TABLE [dbo].[LST_AssignToCalculationOption] (
    [SqlUserId]  INT          NOT NULL,
    [OptionCode] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_LST_AssignToCalculationOption] PRIMARY KEY CLUSTERED ([SqlUserId] ASC, [OptionCode] ASC)
);

