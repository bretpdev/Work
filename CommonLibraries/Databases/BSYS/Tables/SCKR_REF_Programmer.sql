CREATE TABLE [dbo].[SCKR_REF_Programmer] (
    [Sequence]   INT           IDENTITY (1, 1) NOT NULL,
    [Request]    INT           NOT NULL,
    [Class]      NVARCHAR (3)  NOT NULL,
    [Programmer] NVARCHAR (50) NOT NULL,
    [Begin]      SMALLDATETIME CONSTRAINT [DF_SCKR_REF_Programmer_Begin] DEFAULT (convert(datetime,floor(convert(real,getdate())))) NOT NULL,
    [End]        SMALLDATETIME NULL,
    CONSTRAINT [PK_refProgrammer] PRIMARY KEY CLUSTERED ([Sequence] ASC) WITH (FILLFACTOR = 90)
);

