CREATE TABLE [dbo].[BillingStatements] (
    [FileName]                  NCHAR (3)     NOT NULL,
    [BillTitle]                 VARCHAR (50)  NOT NULL,
    [FirstSpecialMessageTitle]  VARCHAR (500) NULL,
    [FirstSpecialMessageBody]   VARCHAR (500) NULL,
    [SecondSpecialMessageTitle] VARCHAR (500) NULL,
    [SecondSpecialMessageBody]  VARCHAR (500) NULL
);

