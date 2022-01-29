CREATE TABLE [dbo].[DTX7LExpiredLetters] (
    [LetterId] VARCHAR (15) NOT NULL,
    [Arc]      VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_DTX7LExpiredLetters] PRIMARY KEY CLUSTERED ([LetterId] ASC, [Arc] ASC)
);

