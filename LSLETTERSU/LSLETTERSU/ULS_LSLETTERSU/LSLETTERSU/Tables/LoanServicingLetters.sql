CREATE TABLE [lslettersu].[LoanServicingLetters]
(
	    [LoanServicingLettersId] INT NOT NULL IDENTITY,
    [LetterType]         VARCHAR (50)  NOT NULL,
    [LetterOptions]      VARCHAR (80)  NOT NULL,
    [LetterChoices]      VARCHAR (650) NULL,
    [Arc]                VARCHAR (5)   NOT NULL,
    [LetterId]           VARCHAR (10)  NOT NULL,
    [StoredProceduresId] INT NULL, 
    [DischargeAmount] BIT NOT NULL DEFAULT 0, 
    [SchoolName] BIT NOT NULL DEFAULT 0, 
    [LastDateAttendance] BIT NOT NULL DEFAULT 0, 
    [SchoolClosureDate] BIT NOT NULL DEFAULT 0, 
    [DefForbType] BIT NOT NULL DEFAULT 0, 
    [DefForbEndDate] BIT NOT NULL DEFAULT 0, 
    [LoanTermEndDate] BIT NOT NULL DEFAULT 0, 
    [SchoolYear] BIT NOT NULL DEFAULT 0, 
    [AdditionalReason] BIT NOT NULL DEFAULT 0, 
    [DeathLetter] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_LoanServicingLetters_StoredProcedures] FOREIGN KEY ([StoredProceduresId]) REFERENCES lslettersu.[StoredProcedures]([StoredProceduresId]), 
    CONSTRAINT [PK_LoanServicingLetters] PRIMARY KEY ([LoanServicingLettersId])
)
