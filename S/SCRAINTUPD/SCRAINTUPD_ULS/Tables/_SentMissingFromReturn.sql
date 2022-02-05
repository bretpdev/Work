CREATE TABLE [scra].[_SentMissingFromReturn] (
    [SentMissingFromReturnId] INT          IDENTITY (1, 1) NOT NULL,
    [BF_SSN]                  VARCHAR (10) NULL,
    [FILENAME]                VARCHAR (54) NULL,
    PRIMARY KEY CLUSTERED ([SentMissingFromReturnId] ASC) WITH (FILLFACTOR = 95)
);

