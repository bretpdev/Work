CREATE TABLE [dbo].[FILE_DAT_WhenToCheck] (
    [CheckTimeID]         INT           IDENTITY (1, 1) NOT NULL,
    [FileNameDescription] VARCHAR (MAX) NOT NULL,
    [TimeOfDayToCheck]    TIME (0)      NOT NULL,
    CONSTRAINT [PK_FILE_DAT_WhenToCheck] PRIMARY KEY CLUSTERED ([CheckTimeID] ASC)
);

