CREATE TABLE [dbo].[GENR_LST_Suffixes] (
    [suffix_id] INT          IDENTITY (1, 1) NOT NULL,
    [suffix]    VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_GENR_LST_Suffixes] PRIMARY KEY CLUSTERED ([suffix_id] ASC)
);

