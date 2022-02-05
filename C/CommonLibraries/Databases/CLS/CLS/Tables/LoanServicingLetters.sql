CREATE TABLE [dbo].[LoanServicingLetters] (
    [LetterType]         VARCHAR (50)  NOT NULL,
    [LetterOptions]      VARCHAR (80)  NOT NULL,
    [LetterChoices]      VARCHAR (650) NULL,
    [CheckForCoBorrower] BIT           NOT NULL,
    [ArcSearch]          VARCHAR (100) NOT NULL,
    [Arc]                VARCHAR (5)   NOT NULL,
    [LetterId]           VARCHAR (10)  NOT NULL,
    [Hierarchy]          INT           NULL,
    [Tx2xSearch]         VARCHAR (10)  NULL,
    [Note]               VARCHAR (MAX) NULL
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_LoanServicingLetters_LTLOLC]
    ON [dbo].[LoanServicingLetters]([LetterType] ASC, [LetterOptions] ASC, [LetterChoices] ASC);

