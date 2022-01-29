CREATE TABLE [dbo].[SCKR_REF_TestCases] (
    [Sequence]    INT           IDENTITY (1, 1) NOT NULL,
    [Request]     INT           NULL,
    [Class]       NVARCHAR (3)  NULL,
    [TestNo]      INT           NULL,
    [TestType]    NVARCHAR (50) NULL,
    [Unit]        NVARCHAR (50) NULL,
    [Description] NTEXT         NULL,
    [Passed]      BIT           CONSTRAINT [DF_SCKR_REF_TestCases_Passed] DEFAULT ((0)) NOT NULL,
    [Failed]      BIT           CONSTRAINT [DF_SCKR_REF_TestCases_Failed] DEFAULT ((0)) NOT NULL,
    [NA]          BIT           CONSTRAINT [DF_SCKR_REF_TestCases_NA] DEFAULT ((0)) NOT NULL,
    [Other]       BIT           CONSTRAINT [DF_SCKR_REF_TestCases_Other] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_refTestCases] PRIMARY KEY CLUSTERED ([Sequence] ASC) WITH (FILLFACTOR = 90)
);

