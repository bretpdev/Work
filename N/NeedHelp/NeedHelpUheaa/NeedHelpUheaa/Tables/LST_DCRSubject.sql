CREATE TABLE [dbo].[LST_DCRSubject] (
    [DCRSubjectOption]   VARCHAR (50) NOT NULL,
    [AssignToProgrammer] BIT          NOT NULL,
    CONSTRAINT [PK_NDHP_LST_DCRSubjects] PRIMARY KEY CLUSTERED ([DCRSubjectOption] ASC)
);

