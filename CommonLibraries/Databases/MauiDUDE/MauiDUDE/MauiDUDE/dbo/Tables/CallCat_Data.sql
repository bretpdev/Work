CREATE TABLE [dbo].[CallCat_Data] (
    [RecNum]            BIGINT        IDENTITY (1, 1) NOT NULL,
    [Catergory]         VARCHAR (50)  NOT NULL,
    [Reason]            VARCHAR (50)  CONSTRAINT [DF_CallCat_Data_Reason] DEFAULT ('') NOT NULL,
    [LetterID]          VARCHAR (10)  CONSTRAINT [DF_CallCat_Data_LetterID] DEFAULT ('') NOT NULL,
    [Comments]          VARCHAR (30)  CONSTRAINT [DF_CallCat_Data_Comments] DEFAULT ('') NOT NULL,
    [DateAndTimeOfCall] DATETIME      CONSTRAINT [DF_CallCat_Data_DateAndTimeOfCall] DEFAULT (getdate()) NOT NULL,
    [UserID]            VARCHAR (100) CONSTRAINT [DF_CallCat_Data_UserID] DEFAULT ('') NOT NULL,
    [Region]            VARCHAR (11)  NULL,
    CONSTRAINT [PK_CallCat_Data] PRIMARY KEY CLUSTERED ([RecNum] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_Category_Region_CallDate]
    ON [dbo].[CallCat_Data]([Catergory] ASC, [Region] ASC, [DateAndTimeOfCall] ASC)
    INCLUDE([Reason]);

